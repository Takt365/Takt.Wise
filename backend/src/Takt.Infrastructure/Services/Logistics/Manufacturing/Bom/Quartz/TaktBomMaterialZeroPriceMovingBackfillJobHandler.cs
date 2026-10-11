// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz
// 文件名称：TaktBomMaterialZeroPriceMovingBackfillJobHandler.cs
// 创建时间：2026-08-19
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 零价格回填移动价（ExecuteParams 可指定 costingPeriod/targetDatabase；空则当月；查询落目标库）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;

/// <summary>
/// Quartz：BOM 零价格回填移动平均价（与零价格视图「回填移动价格」同口径；targetDatabase 由执行器切库）
/// </summary>
public sealed class TaktBomMaterialZeroPriceMovingBackfillJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktBomMaterialZeroPriceService _zeroPriceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="zeroPriceService">零价格服务</param>
    public TaktBomMaterialZeroPriceMovingBackfillJobHandler(ITaktBomMaterialZeroPriceService zeroPriceService)
    {
        ArgumentNullException.ThrowIfNull(zeroPriceService);
        _zeroPriceService = zeroPriceService;
    }

    /// <summary>
    /// nameof
    /// </summary>
    public string HandlerKey => nameof(TaktBomMaterialZeroPriceMovingBackfillJobHandler);

    /// <summary>
    /// 执行任务逻辑
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        ArgumentException.ThrowIfNullOrWhiteSpace(task.TenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.CompanyCode);
        var asOfDate = TaktBomQuartzExecuteParamsHelper.ResolveAsOfDate(context.ExecuteParams);
        var result = await _zeroPriceService.RunScheduledBomMaterialZeroPriceMovingBackfillAsync(asOfDate);
        if (result == null)
        {
            context.ExecuteMessage = "零价格回填无结果（无关联工厂或上下文无效）";
            TaktLogger.Warning(
                "[BomZeroPriceMpBk] Quartz 无结果 TaskCode={TaskCode} Tenant={Tenant} Company={Company}",
                task.TaskCode,
                task.TenantCode,
                task.CompanyCode);
            return;
        }
        var itemTable = string.IsNullOrWhiteSpace(result.ItemPhysicalTable)
            ? string.Empty
            : $"，明细表 {result.ItemPhysicalTable.Trim()}";
        context.ExecuteMessage =
            $"零价格回填完成（月份 {result.ProcessedMonth}）：组件 {result.ComponentProcessedCount}，扫描 {result.ScannedRowCount}，更新 {result.UpdatedRowCount}，无建议价跳过 {result.SkippedNoPriceCount}，未变 {result.UnchangedRowCount}，产品月成本 {result.ProductMonthlyCostUpdatedCount}，机种月均 {result.ModelMonthlyAverageUpdatedCount}{itemTable}";
        TaktLogger.Information(
            "[BomZeroPriceMpBk] Quartz 完成 Month={Month} Components={Components} Scanned={Scanned} Updated={Updated} SkippedNoPrice={Skipped} Unchanged={Unchanged} Pmc={Pmc} ModelAvg={ModelAvg} Tenant={Tenant} Company={Company}",
            result.ProcessedMonth,
            result.ComponentProcessedCount,
            result.ScannedRowCount,
            result.UpdatedRowCount,
            result.SkippedNoPriceCount,
            result.UnchangedRowCount,
            result.ProductMonthlyCostUpdatedCount,
            result.ModelMonthlyAverageUpdatedCount,
            task.TenantCode,
            task.CompanyCode);
    }
}
