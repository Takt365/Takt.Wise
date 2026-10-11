// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktNumberingHelper.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则归一化、段配置、业务编码拼接与流水计算（无 I/O；与 TaktNumberingService / TaktNumberingGenerator 一致）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Exceptions;
using Takt.Shared.Models;

namespace Takt.Shared.Helpers;

/// <summary>
/// 编码规则辅助（DateFormat/ResetPeriod 对齐、段配置、编码拼接与流水计算）
/// </summary>
public static class TaktNumberingHelper
{
    private static readonly string[] DefaultSegmentKeys =
    {
        "CompanyCode",
        "DeptCode",
        "PrefixCode",
        "DateSequence",
    };

    private const string DefaultSegmentsDescription =
        "segments:CompanyCode,DeptCode,PrefixCode,DateSequence";

    private const string SegmentsCompanyLevelDescription =
        "segments:CompanyCode,PrefixCode,DateSequence";

    private const string SegmentsCompanyNoDateDescription =
        "segments:CompanyCode,PrefixCode,Sequence";

    private const string SegmentsWithDepartmentNoDateDescription =
        "segments:CompanyCode,DeptCode,PrefixCode,Sequence";

    /// <summary>
    /// 归一化日期格式键（none/空 → none；其余去空白）
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>归一化键</returns>
    public static string NormalizeDateFormatKey(string? dateFormat)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
        {
            return "none";
        }
        var trimmed = dateFormat.Trim();
        return trimmed.Equals("none", StringComparison.OrdinalIgnoreCase) ? "none" : trimmed;
    }

    /// <summary>
    /// 归一化重置周期（字典 sys_reset_period：None|Annually|Monthly|Daily；兼容 legacy）
    /// </summary>
    /// <param name="resetPeriod">重置周期</param>
    /// <returns>标准值 None/Annually/Monthly/Daily</returns>
    public static string NormalizeResetPeriod(string? resetPeriod)
    {
        if (string.IsNullOrWhiteSpace(resetPeriod))
        {
            return "None";
        }
        return resetPeriod.Trim().ToLowerInvariant() switch
        {
            "none" => "None",
            "annually" or "year" or "yearly" => "Annually",
            "monthly" or "month" => "Monthly",
            "daily" or "day" => "Daily",
            // 已废弃 hour：按日重置
            "hour" or "hourly" or "minutely" or "minute" => "Daily",
            _ => resetPeriod.Trim(),
        };
    }

    /// <summary>
    /// 归一化并迁移已废弃的日期格式（yyyyMMddHHmm → yyyyMMddHH）
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>none 时为 null；否则为支持的格式键</returns>
    public static string? NormalizeSupportedDateFormat(string? dateFormat)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
        {
            return null;
        }
        var trimmed = dateFormat.Trim();
        if (trimmed.Equals("none", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        if (trimmed.Equals("yyyyMMddHHmm", StringComparison.OrdinalIgnoreCase))
        {
            return "yyyyMMddHH";
        }
        return trimmed;
    }

    /// <summary>
    /// 根据 DateFormat 解析与之匹配的重置周期
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="resetPeriod">匹配的重置周期</param>
    /// <returns>是否支持该日期格式</returns>
    public static bool TryResolveRequiredResetPeriod(string? dateFormat, out string resetPeriod)
    {
        resetPeriod = "None";
        var formatKey = NormalizeSupportedDateFormat(dateFormat);
        if (formatKey == null)
        {
            return Assign("None", out resetPeriod);
        }
        return formatKey switch
        {
            "yyyy" => Assign("Annually", out resetPeriod),
            "yyyyMM" => Assign("Monthly", out resetPeriod),
            "yyyyMMdd" => Assign("Daily", out resetPeriod),
            // 年月日时无独立 Hour 字典项，按日重置
            "yyyyMMddHH" => Assign("Daily", out resetPeriod),
            _ => false,
        };
    }

    /// <summary>
    /// 根据 DateFormat 返回必须使用的重置周期
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>重置周期</returns>
    /// <exception cref="ArgumentException">不支持的日期格式</exception>
    public static string ResolveRequiredResetPeriod(string? dateFormat)
    {
        if (!TryResolveRequiredResetPeriod(dateFormat, out var resetPeriod))
        {
            throw new ArgumentException($"不支持的日期格式: {dateFormat}", nameof(dateFormat));
        }
        return resetPeriod;
    }

    /// <summary>
    /// 重置周期是否与 DateFormat 粒度匹配
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="resetPeriod">重置周期</param>
    /// <returns>是否匹配</returns>
    public static bool IsResetPeriodMatched(string? dateFormat, string? resetPeriod)
    {
        if (!TryResolveRequiredResetPeriod(dateFormat, out var required))
        {
            return false;
        }
        return string.Equals(NormalizeResetPeriod(resetPeriod), required, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 归一化单据类型：菜单 Id（全数字）原样返回；兼容旧版数字领域码 → 领域名（遗留数据）
    /// </summary>
    /// <param name="documentType">菜单 Id、业务领域或旧版数字</param>
    /// <returns>标准 DocumentType（优先菜单 Id）</returns>
    public static string NormalizeDocumentType(string? documentType)
    {
        if (string.IsNullOrWhiteSpace(documentType))
        {
            return string.Empty;
        }
        var trimmed = documentType.Trim();
        // 菜单 Id（雪花/长数字）不作旧版领域映射
        if (trimmed.Length > 2 && trimmed.All(char.IsDigit))
        {
            return trimmed;
        }
        if (int.TryParse(trimmed, out var legacyCode))
        {
            return legacyCode switch
            {
                1 or 9 => "Foundation",
                2 => "Routine",
                3 => "Accounting",
                4 => "Logistics",
                5 => "HumanResource",
                6 => "Identity",
                7 => "Workflow",
                8 => "Code",
                _ => string.Empty,
            };
        }
        return trimmed;
    }

    /// <summary>
    /// 归一化编码段键（兼容旧版 Prefix/Suffix 段名）
    /// </summary>
    /// <param name="segmentKey">段键</param>
    /// <returns>标准段名</returns>
    public static string NormalizeSegmentKey(string segmentKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(segmentKey);
        if (segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
        {
            return "PrefixCode";
        }
        if (segmentKey.Equals("Suffix", StringComparison.OrdinalIgnoreCase))
        {
            return "SuffixCode";
        }
        return segmentKey.Trim();
    }

    /// <summary>
    /// 归一化 segments 描述中的段名（Prefix → PrefixCode）
    /// </summary>
    /// <param name="description">Description 字段</param>
    /// <returns>归一化后的段配置</returns>
    public static string NormalizeSegmentDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return description ?? string.Empty;
        }
        var text = description.Trim();
        if (!text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return text;
        }
        const string marker = "segments:";
        var body = text[marker.Length..];
        var keys = body.Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (keys.Length == 0)
        {
            return text;
        }
        return marker + string.Join(",", keys.Select(NormalizeSegmentKey));
    }

    /// <summary>
    /// 规范化编码规则快照默认值
    /// </summary>
    /// <param name="rule">编码规则快照</param>
    public static void NormalizeNumberingModel(TaktNumberingModel rule)
    {
        if (rule.SequenceLength <= 0)
        {
            rule.SequenceLength = 6;
        }
        if (rule.SequenceStep <= 0)
        {
            rule.SequenceStep = 1;
        }
        if (string.IsNullOrWhiteSpace(rule.ResetPeriod))
        {
            rule.ResetPeriod = "None";
        }
        if (string.Equals(rule.DateFormat?.Trim(), "none", StringComparison.OrdinalIgnoreCase))
        {
            rule.DateFormat = null;
        }
        else
        {
            rule.DateFormat = NormalizeSupportedDateFormat(rule.DateFormat);
        }
        if (TryResolveRequiredResetPeriod(rule.DateFormat, out var requiredResetPeriod))
        {
            rule.ResetPeriod = requiredResetPeriod;
        }
        else
        {
            rule.ResetPeriod = NormalizeResetPeriod(rule.ResetPeriod);
        }
        rule.Description = NormalizeSegmentDescription(
            ResolveEffectiveSegmentDescription(rule.Description));
        rule.DocumentType = NormalizeDocumentType(rule.DocumentType);
        if (string.IsNullOrWhiteSpace(rule.Separator))
        {
            rule.Separator = "-";
        }
    }

    /// <summary>
    /// 按编码规则拼接业务编码
    /// </summary>
    /// <param name="rule">编码规则快照</param>
    /// <param name="sequence">流水号</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>业务编码</returns>
    public static string FormatBusinessCode(TaktNumberingModel rule, int sequence, DateTime? referenceTime = null)
    {
        if (sequence < 0)
        {
            throw new TaktBusinessException("流水号不能小于 0");
        }
        var time = referenceTime ?? DateTime.Now;
        var length = rule.SequenceLength <= 0 ? 6 : rule.SequenceLength;
        var separator = string.IsNullOrWhiteSpace(rule.Separator) ? "-" : rule.Separator.Trim();
        var parts = new List<string>();
        foreach (var segmentKey in ResolveSegmentKeys(rule))
        {
            var part = ResolveSegmentValue(rule, segmentKey, sequence, length, time);
            if (!string.IsNullOrWhiteSpace(part))
            {
                parts.Add(part);
            }
        }
        var code = string.IsNullOrEmpty(separator)
            ? string.Concat(parts)
            : string.Join(separator, parts);
        var suffixCode = rule.SuffixCode?.Trim();
        if (!string.IsNullOrWhiteSpace(suffixCode))
        {
            code += suffixCode.StartsWith(separator, StringComparison.Ordinal) ? suffixCode : suffixCode;
        }
        return code;
    }

    /// <summary>
    /// 计算预览用下一个流水号（不持久化）
    /// </summary>
    /// <param name="rule">编码规则快照</param>
    /// <param name="now">当前时间</param>
    /// <returns>预览流水号</returns>
    public static int ComputePreviewSequence(TaktNumberingModel rule, DateTime now)
    {
        if (ShouldResetSequence(rule.ResetPeriod, rule.UpdatedAt, now))
        {
            return rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        }
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        return rule.CurrentSequence <= 0 ? step : rule.CurrentSequence + step;
    }

    /// <summary>
    /// 解析创建/导入时的起始编码与当前流水
    /// </summary>
    /// <param name="rule">编码规则快照</param>
    /// <param name="exampleCodeInput">起始编码输入</param>
    /// <param name="autoGenerateWhenEmpty">为空时是否自动生成</param>
    /// <returns>起始编码与当前流水</returns>
    public static (string ExampleCode, int CurrentSequence) ResolveExampleCodeOnCreate(
        TaktNumberingModel rule,
        string? exampleCodeInput,
        bool autoGenerateWhenEmpty)
    {
        var now = DateTime.Now;
        if (string.IsNullOrWhiteSpace(exampleCodeInput))
        {
            if (!autoGenerateWhenEmpty)
            {
                throw new TaktBusinessException("起始编码不能为空");
            }
            return BuildInitialExampleCode(rule, now);
        }
        var exampleCode = exampleCodeInput.Trim();
        if (!TryParseCurrentSequenceFromExampleCode(
                exampleCode,
                rule.SequenceLength,
                rule.Separator,
                out var currentSequence))
        {
            throw new TaktBusinessException("起始编码末段须为有效流水号");
        }
        return (exampleCode, currentSequence);
    }

    /// <summary>
    /// 按规则生成初始业务编码
    /// </summary>
    /// <param name="rule">编码规则快照</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>起始编码与当前流水号</returns>
    public static (string ExampleCode, int CurrentSequence) BuildInitialExampleCode(
        TaktNumberingModel rule,
        DateTime referenceTime)
    {
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var initialSequence = step;
        var code = FormatBusinessCode(rule, initialSequence, referenceTime);
        return (code, initialSequence);
    }

    /// <summary>
    /// 按规则生成下一个业务编码（含重置周期判断）
    /// </summary>
    /// <param name="rule">编码规则快照（会临时修改 CurrentSequence 以应用重置）</param>
    /// <param name="now">当前时间</param>
    /// <returns>业务编码与下一流水号</returns>
    public static (string BusinessCode, int NextSequence) BuildNextBusinessCode(TaktNumberingModel rule, DateTime now)
    {
        if (ShouldResetSequence(rule.ResetPeriod, rule.UpdatedAt, now))
        {
            rule.CurrentSequence = 0;
        }
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var nextSequence = checked(rule.CurrentSequence + step);
        var code = FormatBusinessCode(rule, nextSequence, now);
        return (code, nextSequence);
    }

    /// <summary>
    /// 从起始编码末段解析当前流水号
    /// </summary>
    /// <param name="exampleCode">起始编码</param>
    /// <param name="sequenceLength">流水位数</param>
    /// <param name="separator">分隔符</param>
    /// <param name="currentSequence">解析出的流水号</param>
    /// <returns>是否解析成功</returns>
    public static bool TryParseCurrentSequenceFromExampleCode(
        string exampleCode,
        int sequenceLength,
        string? separator,
        out int currentSequence)
    {
        currentSequence = 0;
        if (string.IsNullOrWhiteSpace(exampleCode))
        {
            return false;
        }
        var trimmed = exampleCode.Trim();
        var lastSegment = trimmed;
        var sep = separator?.Trim();
        if (!string.IsNullOrEmpty(sep) && sep.Length == 1)
        {
            var idx = trimmed.LastIndexOf(sep[0]);
            if (idx >= 0)
            {
                lastSegment = trimmed[(idx + 1)..];
            }
        }
        else
        {
            var lastSeparator = Math.Max(trimmed.LastIndexOf('-'), trimmed.LastIndexOf('_'));
            lastSegment = lastSeparator >= 0 ? trimmed[(lastSeparator + 1)..] : trimmed;
        }
        if (string.IsNullOrWhiteSpace(lastSegment) || lastSegment.Length > 18)
        {
            return false;
        }
        for (var i = 0; i < lastSegment.Length; i++)
        {
            if (!char.IsDigit(lastSegment[i]))
            {
                return false;
            }
        }
        if (!int.TryParse(lastSegment, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
            || parsed < 0)
        {
            return false;
        }
        if (sequenceLength > 0 && lastSegment.Length > sequenceLength)
        {
            return false;
        }
        currentSequence = parsed;
        return true;
    }

    private static bool Assign(string value, out string resetPeriod)
    {
        resetPeriod = value;
        return true;
    }

    private static bool ShouldResetSequence(string resetPeriod, DateTime? lastUpdatedAt, DateTime now)
    {
        var period = NormalizeResetPeriod(resetPeriod).ToLowerInvariant();
        if (period == "none" || !lastUpdatedAt.HasValue)
        {
            return false;
        }
        var last = lastUpdatedAt.Value;
        return period switch
        {
            "daily" or "day" => last.Date < now.Date,
            "monthly" or "month" => last.Year < now.Year || (last.Year == now.Year && last.Month < now.Month),
            "annually" or "year" or "yearly" => last.Year < now.Year,
            _ => false,
        };
    }

    private static IReadOnlyList<string> ResolveSegmentKeys(TaktNumberingModel rule)
    {
        var fromDescription = TryParseSegmentKeysFromDescription(ResolveEffectiveSegmentDescription(rule.Description));
        return fromDescription ?? DefaultSegmentKeys;
    }

    private static string? ResolveSegmentValue(
        TaktNumberingModel rule,
        string segmentKey,
        int sequence,
        int sequenceLength,
        DateTime time)
    {
        if (segmentKey.Equals("Sequence", StringComparison.OrdinalIgnoreCase))
        {
            return sequence.ToString().PadLeft(sequenceLength, '0');
        }
        if (segmentKey.Equals("DateSequence", StringComparison.OrdinalIgnoreCase))
        {
            var datePart = FormatDateSegment(rule.DateFormat, time);
            var seqPart = sequence.ToString().PadLeft(sequenceLength, '0');
            var combined = string.Concat(datePart, seqPart);
            return string.IsNullOrWhiteSpace(combined) ? null : combined;
        }
        if (segmentKey.Equals("DateFormat", StringComparison.OrdinalIgnoreCase))
        {
            return FormatDateSegment(rule.DateFormat, time);
        }
        if (segmentKey.Equals("CompanyCode", StringComparison.OrdinalIgnoreCase))
        {
            var text = rule.CompanyCode?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        if (segmentKey.Equals("DeptCode", StringComparison.OrdinalIgnoreCase)
            || segmentKey.Equals("DepartmentCode", StringComparison.OrdinalIgnoreCase))
        {
            var text = rule.DeptCode?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        if (segmentKey.Equals("PrefixCode", StringComparison.OrdinalIgnoreCase)
            || segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
        {
            var text = rule.PrefixCode?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : TrimTrailingSeparators(text);
        }
        if (segmentKey.Equals("DocumentType", StringComparison.OrdinalIgnoreCase))
        {
            var text = rule.DocumentType?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        return null;
    }

    private static string ResolveEffectiveSegmentDescription(string? description)
    {
        var text = description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text)
            || !text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return DefaultSegmentsDescription;
        }
        var body = text["segments:".Length..];
        var isLegacy = body.Contains("DocumentType", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateFormat,Sequence", StringComparison.OrdinalIgnoreCase);
        if (!isLegacy)
        {
            return text;
        }
        var hasDepartment = body.Contains("DeptCode", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DepartmentCode", StringComparison.OrdinalIgnoreCase);
        var hasDate = body.Contains("DateFormat", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateSequence", StringComparison.OrdinalIgnoreCase);
        if (hasDepartment)
        {
            return hasDate ? DefaultSegmentsDescription : SegmentsWithDepartmentNoDateDescription;
        }
        return hasDate ? SegmentsCompanyLevelDescription : SegmentsCompanyNoDateDescription;
    }

    private static string[]? TryParseSegmentKeysFromDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }
        var text = description.Trim();
        const string prefix = "segments:";
        if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        var body = text[prefix.Length..].Trim();
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }
        var keys = body.Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return keys.Length == 0 ? null : keys;
    }

    private static string FormatDateSegment(string? dateFormat, DateTime time)
    {
        if (string.IsNullOrWhiteSpace(dateFormat)
            || dateFormat.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }
        return dateFormat.Trim() switch
        {
            "yyyy" => time.ToString("yyyy"),
            "yyyyMM" => time.ToString("yyyyMM"),
            "yyyyMMdd" => time.ToString("yyyyMMdd"),
            "yyyyMMddHH" => time.ToString("yyyyMMddHH"),
            _ => time.ToString(dateFormat.Trim()),
        };
    }

    private static string TrimTrailingSeparators(string value)
    {
        return value.TrimEnd('-', '_', '/', ' ');
    }
}
