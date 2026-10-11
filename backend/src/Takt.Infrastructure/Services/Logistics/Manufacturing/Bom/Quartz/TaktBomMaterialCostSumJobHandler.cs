// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz
// 文件名称：TaktBomMaterialCostSumJobHandler.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 成本合计（ExecuteParams 可指定 costingPeriod/targetDatabase；空则当月；查询落目标库 RelatedPlant）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;

/// <summary>
/// Quartz：BOM 物料成本合计处理器（targetDatabase 由执行器切库；核算月→RelatedPlant）
/// </summary>
public sealed class TaktBomMaterialCostSumJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktBomCalculateService _bomCalculateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomCalculateService">BOM 计算服务</param>
    public TaktBomMaterialCostSumJobHandler(ITaktBomCalculateService bomCalculateService)
    {
        ArgumentNullException.ThrowIfNull(bomCalculateService);
        _bomCalculateService = bomCalculateService;
    }

    /// <summary>
    /// nameof
    /// </summary>
    public string HandlerKey => nameof(TaktBomMaterialCostSumJobHandler);

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
        var result = await _bomCalculateService.RunScheduledBomCalculateSumAsync(asOfDate);
        if (result == null)
        {
            context.ExecuteMessage = "成本合计无结果（目标月无明细或上下文无效）";
            TaktLogger.Warning(
                "[BomMaterialCost] Quartz 合计无结果 TaskCode={TaskCode} Tenant={Tenant} Company={Company}",
                task.TaskCode,
                task.TenantCode,
                task.CompanyCode);
            return;
        }
        context.ExecuteMessage =
            $"成本合计完成（月份 {result.ProcessedMonth}）：扫描 {result.ScannedRowCount}，刷新 {result.RefreshedGroupCount}，跳过 {result.SkippedGroupCount}";
        TaktLogger.Information(
            "[BomMaterialCost] Quartz 合计完成 Month={Month} Scanned={Scanned} Refreshed={Refreshed} Skipped={Skipped} Tenant={Tenant} Company={Company}",
            result.ProcessedMonth,
            result.ScannedRowCount,
            result.RefreshedGroupCount,
            result.SkippedGroupCount,
            task.TenantCode,
            task.CompanyCode);
    }
}
