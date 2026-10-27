// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomZeroPriceOperationMessageHelper.cs
// 创建时间：2026-08-17
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 零价格视图操作结果落库消息正文（无 I/O；导出不调用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Foundation;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 零价格操作结果消息正文与安全落库（失败仅记日志，不阻断业务）
/// </summary>
public static class TaktBomZeroPriceOperationMessageHelper
{
    /// <summary>
    /// 尝试向当前用户落库并推送操作消息
    /// </summary>
    /// <param name="messageService">在线消息服务</param>
    /// <param name="content">正文</param>
    /// <returns>任务</returns>
    public static async Task TryNotifyAsync(ITaktMessageService messageService, string content)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }
        try
        {
            await messageService.CreateAndSendSelfOperationMessageAsync(content.Trim());
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[BomZeroPrice] 操作消息落库/推送失败 Content={Content}", content.Trim());
        }
    }

    /// <summary>
    /// 已提交后台计算/重算
    /// </summary>
    /// <param name="month">核算月</param>
    /// <param name="forceRecalculate">是否重算</param>
    /// <returns>正文</returns>
    public static string BuildCostJobSubmitted(string month, bool forceRecalculate)
    {
        var m = string.IsNullOrWhiteSpace(month) ? "—" : month.Trim();
        return forceRecalculate
            ? $"已提交 {m} 后台重算成本（全部物料类型），完成后将通知您"
            : $"已提交 {m} 后台计算成本（全部物料类型），完成后将通知您";
    }

    /// <summary>
    /// 后台计算/重算完成或失败
    /// </summary>
    /// <param name="month">核算月</param>
    /// <param name="success">是否成功</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="refreshed">刷新组数</param>
    /// <param name="skipped">跳过组数</param>
    /// <param name="errorMessage">失败信息</param>
    /// <returns>正文</returns>
    public static string BuildCostJobCompleted(
        string month,
        bool success,
        long durationMs,
        int refreshed,
        int skipped,
        string? errorMessage)
    {
        var m = string.IsNullOrWhiteSpace(month) ? "—" : month.Trim();
        if (!success)
        {
            var err = string.IsNullOrWhiteSpace(errorMessage) ? "未知错误" : errorMessage.Trim();
            return $"{m} 成本处理失败：{err}";
        }
        return $"{m} 处理完成（耗时 {FormatDuration(durationMs)}，刷新 {refreshed} 组，跳过 {skipped} 组）";
    }

    /// <summary>
    /// 计算平均成本完成
    /// </summary>
    /// <param name="result">结果</param>
    /// <returns>正文</returns>
    public static string BuildAverageSuccess(TaktBomCalculateAverageResultDto result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return $"{result.CostingPeriod} 计算平均成本完成：扫描 {result.ScannedRowCount}，正成本行 {result.PositiveProductCostRowCount}，机种更新 {result.ModelCodeUpdatedCount}，类型更新 {result.MaterialTypeUpdatedCount}，平均更新 {result.AverageUpdatedCount}，分组 {result.ModelGroupCount}（有成本 {result.GroupsWithProductCostCount}，无成本 {result.GroupsWithoutProductCostCount}）";
    }

    /// <summary>
    /// 回填采购价完成
    /// </summary>
    /// <param name="result">结果</param>
    /// <returns>正文</returns>
    public static string BuildPurchaseInfoBackfillSuccess(TaktBomCalculatePurchaseInfoBackfillResultDto result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return $"{result.ProcessedMonth} 回填采购价完成：扫描 {result.ScannedRowCount} 行，更新 {result.UpdatedRowCount}，无价格 {result.SkippedNoPriceCount}，未变化 {result.UnchangedRowCount}";
    }

    /// <summary>
    /// 最近采购成本完成
    /// </summary>
    /// <param name="result">结果</param>
    /// <returns>正文</returns>
    public static string BuildLatestPurchaseCostSuccess(TaktBomCalculateCostResultDto result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return $"{result.ProcessedMonth} 最近采购成本完成：扫描 {result.ScannedRowCount}，刷新 {result.RefreshedGroupCount} 组，跳过 {result.SkippedGroupCount} 组";
    }

    /// <summary>
    /// 移动价回填/手工更新完成
    /// </summary>
    /// <param name="result">结果</param>
    /// <param name="isManual">是否手工更新</param>
    /// <param name="isBatch">是否批量回填替代价格（ComponentCode 为空）</param>
    /// <returns>正文</returns>
    public static string BuildMovingPriceBackfillSuccess(
        TaktBomMaterialZeroPriceMovingBackfillResultDto result,
        bool isManual,
        bool isBatch = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        var head = isManual
            ? "手工更新移动价格完成"
            : isBatch
                ? "批量回填替代价格完成"
                : "回填移动价格完成";
        return $"{result.ProcessedMonth} {head}：组件 {result.ComponentProcessedCount}，扫描 {result.ScannedRowCount}，更新 {result.UpdatedRowCount}，未变化 {result.UnchangedRowCount}，无建议价 {result.SkippedNoPriceCount}，产品月成本 {result.ProductMonthlyCostUpdatedCount}，机种月成本 {result.ModelMonthlyAverageUpdatedCount}";
    }

    /// <summary>
    /// PCB SECT 整树标识列打标完成
    /// </summary>
    /// <param name="result">结果</param>
    /// <returns>正文</returns>
    public static string BuildPcbSectMarkSuccess(TaktBomMaterialZeroPricePcbSectMarkResultDto result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return $"{result.ProcessedMonth} PCB SECT 整树标识列打标完成：扫描 {result.ScannedRowCount}，整树 {result.PcbSectRowCount}，新标 {result.UpdatedRowCount}，已有 {result.UnchangedRowCount}";
    }

    /// <summary>
    /// 格式化耗时
    /// </summary>
    /// <param name="ms">毫秒</param>
    /// <returns>可读耗时</returns>
    private static string FormatDuration(long ms)
    {
        if (ms < 0)
        {
            return "0ms";
        }
        if (ms < 1000)
        {
            return $"{ms}ms";
        }
        var totalSeconds = (ms + 500) / 1000;
        if (totalSeconds < 60)
        {
            return $"{totalSeconds}s";
        }
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;
        return seconds > 0 ? $"{minutes}m {seconds}s" : $"{minutes}m";
    }
}
