// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCalculatesController.cs
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 计算控制器（计算成本 / 重算成本 / 计算平均成本 / 回填采购价 / 计算最近采购成本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 计算控制器（API 保留；页面已并入 BOM零价格；权限对齐 zeroprice）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM计算")]
public class TaktBomCalculatesController : TaktControllerBase
{
    /// <summary>
    /// BOM 计算服务
    /// </summary>
    private readonly ITaktBomCalculateService _bomCalculateService;
    /// <summary>
    /// 计算/重算后台调度
    /// </summary>
    private readonly ITaktBomMaterialCostItemRecalculateBackgroundService _recalculateBackgroundService;
    /// <summary>
    /// 在线消息（操作结果落库；导出不落库）
    /// </summary>
    private readonly ITaktMessageService _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomCalculateService">BOM 计算服务</param>
    /// <param name="recalculateBackgroundService">计算/重算后台调度</param>
    /// <param name="messageService">在线消息服务</param>
    public TaktBomCalculatesController(
        ITaktBomCalculateService bomCalculateService,
        ITaktBomMaterialCostItemRecalculateBackgroundService recalculateBackgroundService,
        ITaktMessageService messageService)
    {
        _bomCalculateService = bomCalculateService;
        _recalculateBackgroundService = recalculateBackgroundService;
        _messageService = messageService;
    }

    /// <summary>
    /// 工厂选项（查询栏）
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:query", "查询BOM零价格工厂")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetBomCalculatePlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomCalculateService.GetBomCalculatePlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 计算成本（后台合计；完成后 SignalR 通知）
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>已提交回执</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "计算BOM成本")]
    [HttpPut("sum")]
    public async Task<IActionResult> SumBomCalculateCostAsync([FromBody] TaktBomCalculateQueryDto queryDto)
    {
        try
        {
            var prepared = TaktBomCalculateService.PrepareBomCalculateQuery(queryDto);
            await _recalculateBackgroundService.EnqueueRecalculateAsync(prepared.Query, forceRecalculate: false);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildCostJobSubmitted(prepared.ProcessedMonth, forceRecalculate: false));
            return Success(
                new TaktBomCalculateSubmittedDto
                {
                    ProcessedMonth = prepared.ProcessedMonth,
                    ForceRecalculate = false,
                },
                "已提交后台计算成本，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 重算成本（后台归档旧成本到 ExtField 后按所选物料类型重写；完成后 SignalR 通知）
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>已提交回执</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "重算BOM成本")]
    [HttpPut("recalculate")]
    public async Task<IActionResult> RecalculateBomCalculateCostAsync([FromBody] TaktBomCalculateQueryDto queryDto)
    {
        try
        {
            var prepared = TaktBomCalculateService.PrepareBomCalculateQuery(queryDto);
            await _recalculateBackgroundService.EnqueueRecalculateAsync(prepared.Query, forceRecalculate: true);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildCostJobSubmitted(prepared.ProcessedMonth, forceRecalculate: true));
            return Success(
                new TaktBomCalculateSubmittedDto
                {
                    ProcessedMonth = prepared.ProcessedMonth,
                    ForceRecalculate = true,
                },
                "已提交后台重算成本，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 计算平均成本（先回填空机种/空物料类型，再按类型+机种写月均；始终全部物料类型）
    /// </summary>
    /// <param name="queryDto">工厂 + 核算期间；机种可选；MaterialType 忽略</param>
    /// <returns>平均结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "计算BOM平均成本")]
    [HttpPost("average")]
    public async Task<IActionResult> CalculateBomCalculateAverageAsync(
        [FromBody] TaktBomCalculateAverageQueryDto queryDto)
    {
        try
        {
            var result = await _bomCalculateService.CalculateBomCalculateAverageAsync(queryDto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildAverageSuccess(result));
            return Success(result, "计算平均成本完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按核算日回填 BOM 明细采购组织/采购组/供应商/净价/采购货币/采购价格单位
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>回填统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "回填BOM采购价格")]
    [HttpPut("purchase-price")]
    public async Task<IActionResult> BackfillBomCalculatePurchaseInfoAsync(
        [FromBody] TaktBomCalculateQueryDto queryDto)
    {
        try
        {
            var result = await _bomCalculateService.BackfillBomCalculatePurchaseInfoAsync(queryDto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildPurchaseInfoBackfillSuccess(result));
            return Success(result, "回填采购价格完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 计算最近采购成本（与产品月成本同一快照；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>合计统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "计算BOM最近采购成本")]
    [HttpPut("latest-purchase-cost")]
    public async Task<IActionResult> SumBomCalculateLatestPurchaseCostAsync(
        [FromBody] TaktBomCalculateQueryDto queryDto)
    {
        try
        {
            var result = await _bomCalculateService.SumBomCalculateLatestPurchaseCostAsync(queryDto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildLatestPurchaseCostSuccess(result));
            return Success(result, "计算最近采购成本完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
