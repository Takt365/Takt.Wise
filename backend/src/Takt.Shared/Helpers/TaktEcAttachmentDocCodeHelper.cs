// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEcAttachmentDocCodeHelper.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件 DocCode 格式校验、PDF 扩展名判定，以及由 DocCode 生成存储/展示文件名（与前端 takt-ec-attachment-doc-code 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 设变附件文件编码格式校验（按 logistics_manufacturing_ec_attachment_type），以及上传后文件名强制为 DocCode + 原扩展名。
/// 附件仅允许 PDF（IsPdfFileName）。EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字。
/// </summary>
public static partial class TaktEcAttachmentDocCodeHelper
{
    /// <summary>
    /// EPP/FPP：P- + 4 位数字
    /// </summary>
    [GeneratedRegex(@"^P-\d{4}$", RegexOptions.CultureInvariant)]
    private static partial Regex EppFppPattern();

    /// <summary>
    /// 联络 TL：DTS- + 4 位数字
    /// </summary>
    [GeneratedRegex(@"^DTS-\d{4}$", RegexOptions.CultureInvariant)]
    private static partial Regex TlPattern();

    /// <summary>
    /// TCJ / 外部联络 EL：xxxx-xxxx（各 4 位数字）
    /// </summary>
    [GeneratedRegex(@"^\d{4}-\d{4}$", RegexOptions.CultureInvariant)]
    private static partial Regex QuadDashQuadPattern();

    /// <summary>
    /// 校验文件编码是否符合选定文件类别规则。
    /// </summary>
    /// <param name="attachmentType">文件类别 DictValue（TL/EPP/FPP/EL/TCJ/EC 等）</param>
    /// <param name="docCode">文件编码</param>
    /// <param name="ecCode">设变单号（类型为 EC 时必填且须与 docCode 一致）</param>
    /// <returns>合法返回 true</returns>
    public static bool IsValidDocCode(string? attachmentType, string? docCode, string? ecCode)
    {
        var type = (attachmentType ?? string.Empty).Trim();
        var code = (docCode ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(code))
        {
            return false;
        }

        return type switch
        {
            TaktEcAttachmentTypeConstants.Ec =>
                string.Equals(code, (ecCode ?? string.Empty).Trim(), StringComparison.Ordinal),
            TaktEcAttachmentTypeConstants.Epp or TaktEcAttachmentTypeConstants.Fpp =>
                EppFppPattern().IsMatch(code),
            TaktEcAttachmentTypeConstants.Liaison =>
                TlPattern().IsMatch(code),
            TaktEcAttachmentTypeConstants.Tcj or TaktEcAttachmentTypeConstants.ExternalLiaison =>
                QuadDashQuadPattern().IsMatch(code),
            // 源 PDF 等未约定格式：仅要求非空（上层 NotEmpty 已保证）
            _ => true,
        };
    }

    /// <summary>
    /// 由文件编码生成存储/展示文件名：基名为 DocCode，扩展名取自源文件名或访问地址；与源文件基名无关。
    /// </summary>
    /// <param name="docCode">文件编码</param>
    /// <param name="sourceFileName">源文件名或当前 FileName（可含路径）</param>
    /// <param name="accessUrl">访问地址（源文件名无扩展名时回退）</param>
    /// <returns>DocCode + 扩展名；无扩展名时仅为 DocCode</returns>
    /// <exception cref="ArgumentException">docCode 为空</exception>
    public static string BuildFileNameFromDocCode(string docCode, string? sourceFileName, string? accessUrl = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(docCode);
        var trimmed = docCode.Trim();
        var ext = ExtractFileExtension(sourceFileName);
        if (string.IsNullOrEmpty(ext))
        {
            ext = ExtractFileExtension(accessUrl);
        }

        return string.IsNullOrEmpty(ext) ? trimmed : trimmed + ext;
    }

    /// <summary>
    /// 是否为 PDF 附件文件名（扩展名 .pdf，大小写不敏感；与前端 isEcAttachmentPdfFileName 对齐）。
    /// </summary>
    /// <param name="fileName">源文件名或存储文件名</param>
    /// <returns>扩展名为 .pdf 时 true；空值或非 PDF 为 false</returns>
    public static bool IsPdfFileName(string? fileName)
    {
        return string.Equals(ExtractFileExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 从文件名或 URL 提取扩展名（含点）。
    /// </summary>
    /// <param name="source">文件名或 URL</param>
    /// <returns>如 .pdf；无法解析时返回空串</returns>
    private static string ExtractFileExtension(string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return string.Empty;
        }

        var withoutQuery = source.Trim().Split('?', 2)[0].Replace('\\', '/');
        return Path.GetExtension(Path.GetFileName(withoutQuery));
    }

    /// <summary>
    /// 返回与类型对应的格式说明（供异常文案）。
    /// </summary>
    /// <param name="attachmentType">文件类别</param>
    /// <returns>示例说明</returns>
    public static string GetFormatHint(string? attachmentType)
    {
        var type = (attachmentType ?? string.Empty).Trim();
        return type switch
        {
            TaktEcAttachmentTypeConstants.Ec => "须与设变单号一致",
            TaktEcAttachmentTypeConstants.Epp or TaktEcAttachmentTypeConstants.Fpp => "P-xxxx（P- + 4 位数字）",
            TaktEcAttachmentTypeConstants.Liaison => "DTS-xxxx（DTS- + 4 位数字）",
            TaktEcAttachmentTypeConstants.Tcj or TaktEcAttachmentTypeConstants.ExternalLiaison => "xxxx-xxxx（各 4 位数字）",
            _ => string.Empty,
        };
    }
}
