// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktProductionStatHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：生产统计达成率计算（MonthProdActualQty / MonthStdCapacity）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

using Takt.Shared.Constants;

/// <summary>
/// 生产统计辅助（达成率 = 实际产量 / 标准产能 × 100%）
/// </summary>
public static class TaktProductionStatHelper
{
    /// <summary>
    /// 组立标准生产稼动率类型：人员（OperationType=1）
    /// </summary>
    public const int AssyStandardOperationRateTypePersonnel = 1;

    /// <summary>
    /// 标准生产稼动率启用状态（字典 sys_normal_disable=1）
    /// </summary>
    public const int StandardOperationRateStatusEnabled = 1;

    /// <summary>
    /// 标准工时审批通过状态（流程归档 ApprovalStatus=2）
    /// </summary>
    public const int StandardOperationTimeApprovalCompleted = 2;
    /// <summary>
    /// PCBA完成状态：未完成（字典 logistics_manufacturing_pcba_completed_status=0）
    /// </summary>
    public const int PcbaCompletedStatusNotCompleted = 0;

    /// <summary>
    /// PCBA完成状态：部分完成（字典 logistics_manufacturing_pcba_completed_status=1）
    /// </summary>
    public const int PcbaCompletedStatusPartial = 1;

    /// <summary>
    /// PCBA完成状态：已完成（字典 logistics_manufacturing_pcba_completed_status=2）
    /// </summary>
    public const int PcbaCompletedStatusCompleted = 2;

    /// <summary>
    /// 汇总 PCBA 明细当日完成数得到累计完成数（保留 1 位小数）
    /// </summary>
    /// <param name="dailyCompletedQuantities">桶内各明细当日完成数</param>
    /// <returns>累计完成数</returns>
    public static decimal CalculatePcbaTotalCompletedQty(IEnumerable<decimal> dailyCompletedQuantities)
    {
        ArgumentNullException.ThrowIfNull(dailyCompletedQuantities);
        decimal total = 0;
        foreach (var qty in dailyCompletedQuantities)
        {
            total = decimal.Round(total + qty, 1, MidpointRounding.AwayFromZero);
        }
        return total;
    }

    /// <summary>
    /// 按累计完成数与批次数量解析 PCBA 完成状态（字典 logistics_manufacturing_pcba_completed_status）
    /// </summary>
    /// <param name="totalCompletedQty">累计完成数</param>
    /// <param name="batchQty">批次数量</param>
    /// <returns>0=未完成；1=部分完成；2=已完成</returns>
    public static int ResolvePcbaCompletedStatus(decimal totalCompletedQty, decimal batchQty)
    {
        if (totalCompletedQty <= 0)
        {
            return PcbaCompletedStatusNotCompleted;
        }
        if (batchQty > 0 && totalCompletedQty >= batchQty)
        {
            return PcbaCompletedStatusCompleted;
        }
        return PcbaCompletedStatusPartial;
    }

    /// <summary>
    /// 计算达成率（百分比，保留 2 位小数；标准产能为 0 时返回 0）
    /// </summary>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <param name="stdCapacity">标准产能</param>
    /// <returns>达成率(%)</returns>
    public static decimal CalculateAchievementRatePercent(decimal prodActualQty, decimal stdCapacity)
    {
        if (stdCapacity <= 0)
        {
            return 0;
        }
        var rate = prodActualQty / stdCapacity * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 组立日报明细是否无产量且无报工（投入/实际工时为 0）
    /// </summary>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <returns>无产量且无报工时为 true</returns>
    public static bool IsAssyDetailWithoutProduction(decimal prodActualQty, decimal confirmMinutes)
    {
        return prodActualQty <= 0 && confirmMinutes <= 0;
    }

    /// <summary>
    /// 组立日报明细是否无产出但有报工工时（需同步生产切换记录）
    /// </summary>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <returns>无产出且报工工时大于 0 时为 true</returns>
    public static bool IsAssyChangeoverCandidate(decimal prodActualQty, decimal confirmMinutes)
    {
        return prodActualQty <= 0 && confirmMinutes > 0;
    }

    /// <summary>
    /// 计算组立日报明细投入工时（分钟）：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为直接人员×60
    /// </summary>
    /// <param name="directLabor">主表直接人员</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <returns>投入工时(分钟)</returns>
    public static decimal CalculateAssyInputMinutes(int directLabor, decimal confirmMinutes = 0, decimal prodActualQty = 0)
    {
        if (IsAssyDetailWithoutProduction(prodActualQty, confirmMinutes))
        {
            return 0;
        }
        if (confirmMinutes > 0)
        {
            return confirmMinutes;
        }
        return checked(directLabor * 60);
    }

    /// <summary>
    /// 计算组立日报明细实际工时（分钟）：报工工时大于 0 时取报工工时减停线时间，否则取投入工时减停线时间；结果不小于 0；有产量时兜底投入工时减停线时间
    /// </summary>
    /// <param name="inputMinutes">投入工时(分钟)</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <param name="downtimeMinutes">停线时间(分钟)</param>
    /// <param name="prodActualQty">实际生产数量（大于 0 时不允许实际工时为 0 或负数）</param>
    /// <returns>实际工时(分钟)</returns>
    public static decimal CalculateAssyActualMinutes(
        decimal inputMinutes,
        decimal confirmMinutes,
        int downtimeMinutes,
        decimal prodActualQty = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(downtimeMinutes);
        if (IsAssyDetailWithoutProduction(prodActualQty, confirmMinutes))
        {
            return 0;
        }
        var baseMinutes = confirmMinutes > 0 ? confirmMinutes : inputMinutes;
        var actual = baseMinutes - downtimeMinutes;
        if (actual < 0)
        {
            actual = 0;
        }
        if (prodActualQty > 0 && actual <= 0 && inputMinutes > 0)
        {
            actual = inputMinutes - downtimeMinutes;
            if (actual < 0)
            {
                actual = 0;
            }
        }
        return actual;
    }

    /// <summary>
    /// 计算组立日报明细间接工时（分钟）：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)
    /// </summary>
    /// <param name="indirectLabor">主表间接人员</param>
    /// <param name="directLabor">主表直接人员</param>
    /// <param name="actualMinutes">实际工时(分钟)</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <param name="prodActualQty">实际生产数量</param>
    /// <returns>间接工时(分钟)</returns>
    public static decimal CalculateAssyIndirectMinutes(
        int indirectLabor,
        int directLabor,
        decimal actualMinutes,
        decimal confirmMinutes = 0,
        decimal prodActualQty = 0)
    {
        if (IsAssyDetailWithoutProduction(prodActualQty, confirmMinutes))
        {
            return 0;
        }
        if (indirectLabor <= 0 || directLabor <= 0)
        {
            return 0;
        }
        var perDirectLabor = Math.Floor(actualMinutes / directLabor);
        return checked(indirectLabor * perDirectLabor);
    }

    /// <summary>
    /// 计算组立日报明细标准产能：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行
    /// </summary>
    /// <param name="stdMinutes">主表标准工时(分钟)</param>
    /// <param name="masterHourlyStdCapacity">主表小时标准产能（表头 StdCapacity）</param>
    /// <param name="confirmMinutes">报工工时(分钟)</param>
    /// <param name="operationRate">标准生产稼动率（比例或历史百分数）</param>
    /// <param name="prodActualQty">实际生产数量（保留参数，与调用方签名对齐；本公式不参与分支）</param>
    /// <returns>明细标准产能（保留 2 位小数，四舍五入）</returns>
    public static decimal CalculateAssyDetailStdCapacity(
        decimal stdMinutes,
        decimal masterHourlyStdCapacity,
        decimal confirmMinutes,
        decimal operationRate,
        decimal prodActualQty = 0)
    {
        _ = prodActualQty;
        if (confirmMinutes > 0)
        {
            var rateFactor = NormalizeStandardOperationRate(operationRate);
            if (stdMinutes <= 0 || rateFactor <= 0)
            {
                return 0;
            }
            var capacity = confirmMinutes / stdMinutes * rateFactor;
            return Math.Round(capacity, 2, MidpointRounding.AwayFromZero);
        }
        return masterHourlyStdCapacity > 0
            ? Math.Round(masterHourlyStdCapacity, 2, MidpointRounding.AwayFromZero)
            : 0;
    }

    /// <summary>
    /// 根据同班组同生产时段桶内有产量/报工明细总数计算混合生产笔数
    /// </summary>
    /// <param name="activeDetailCount">桶内有产量/报工明细总数（含当前行）</param>
    /// <returns>0=非混合；N≥2 表示同时段共有 N 笔有产量/报工</returns>
    public static int CalculateAssyMixedProdCount(int activeDetailCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(activeDetailCount);
        return activeDetailCount >= 2 ? activeDetailCount : 0;
    }

    /// <summary>
    /// 是否为组立日报固定清洁停线生产时段
    /// </summary>
    /// <param name="timePeriod">生产时段</param>
    /// <returns>是清洁时段时为 true</returns>
    public static bool IsAssyCleaningTimePeriod(string? timePeriod)
    {
        return TaktAssyOutputTimePeriodConstants.IsCleaningTimePeriod(timePeriod);
    }

    /// <summary>
    /// 计算清洁时段停线时间（分钟）：直接人员×4
    /// </summary>
    /// <param name="directLabor">主表直接人员</param>
    /// <returns>停线时间(分钟)</returns>
    public static int CalculateAssyCleaningDowntimeMinutes(int directLabor)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(directLabor);
        return checked(directLabor * TaktAssyOutputTimePeriodConstants.CleaningDowntimeMinutesPerDirectLabor);
    }

    /// <summary>
    /// 组立混合生产桶备注：同生产时段多笔报工时写入「工单号-生产时段」，多项以中文逗号分隔
    /// </summary>
    /// <param name="bucketEntries">桶内各明细的工单号与生产时段</param>
    /// <returns>混合生产备注；不足 2 笔时返回空串</returns>
    public static string BuildAssyMixedProdBucketRemark(IEnumerable<(string ProdOrderCode, string TimePeriod)> bucketEntries)
    {
        ArgumentNullException.ThrowIfNull(bucketEntries);
        var tokens = bucketEntries
            .Select(entry =>
            {
                var orderCode = entry.ProdOrderCode?.Trim() ?? string.Empty;
                var timePeriod = entry.TimePeriod?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(orderCode) || string.IsNullOrEmpty(timePeriod))
                {
                    return string.Empty;
                }
                return $"{orderCode}-{timePeriod}";
            })
            .Where(token => !string.IsNullOrEmpty(token))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(token => token, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return tokens.Count > 1 ? string.Join('，', tokens) : string.Empty;
    }

    /// <summary>
    /// 判断备注是否为组立混合生产自动备注（格式：工单号-生产时段，多项以中文逗号分隔）
    /// </summary>
    /// <param name="remark">备注</param>
    /// <returns>符合自动备注格式时为 true</returns>
    public static bool IsAssyMixedProdAutoRemark(string? remark)
    {
        if (string.IsNullOrWhiteSpace(remark))
        {
            return false;
        }
        var segments = remark.Split('，', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments.Length == 0)
        {
            return false;
        }
        foreach (var segment in segments)
        {
            var separatorIndex = segment.LastIndexOf('-');
            if (separatorIndex <= 0 || separatorIndex >= segment.Length - 1)
            {
                return false;
            }
            var timePeriod = segment[(separatorIndex + 1)..];
            if (!timePeriod.Contains('~', StringComparison.Ordinal))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 将标准生产稼动率规范为比例（0.85=85%）；历史百分数存值（如 85）自动除以 100
    /// </summary>
    /// <param name="operationRate">稼动率（比例或历史百分数）</param>
    /// <returns>比例值</returns>
    public static decimal NormalizeStandardOperationRate(decimal operationRate)
    {
        if (operationRate <= 0)
        {
            return 0;
        }
        return operationRate > 1m ? operationRate / 100m : operationRate;
    }

    /// <summary>
    /// 计算组立日报小时标准产能：DirectLabor×60÷StdMinutes×稼动率比例；标准工时为 0 或稼动率为 0 时返回 0
    /// </summary>
    /// <param name="directLabor">直接人员</param>
    /// <param name="stdMinutes">标准工时(分钟)</param>
    /// <param name="operationRate">标准生产稼动率（比例，如 0.85 表示 85%）</param>
    /// <returns>标准产能（小时产能，保留 2 位小数，四舍五入）</returns>
    public static decimal CalculateAssyStdCapacity(int directLabor, decimal stdMinutes, decimal operationRate)
    {
        var rateFactor = NormalizeStandardOperationRate(operationRate);
        if (directLabor <= 0 || stdMinutes <= 0 || rateFactor <= 0)
        {
            return 0;
        }
        var capacity = directLabor * 60m / stdMinutes * rateFactor;
        return Math.Round(capacity, 2, MidpointRounding.AwayFromZero);
    }
}
