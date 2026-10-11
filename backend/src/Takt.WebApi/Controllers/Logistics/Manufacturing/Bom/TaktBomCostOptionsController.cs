// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCostOptionsController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项控制器（工厂 / 期间 / 机种 / 产品 / 物料）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本查询栏共用选项（五页同一数据源；仅登录鉴权，供各菜单页调用）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM成本查询选项")]
public class TaktBomCostOptionsController : TaktControllerBase
{
    private readonly ITaktBomCostOptionService _bomCostOptionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomCostOptionService">BOM 成本选项服务</param>
    public TaktBomCostOptionsController(ITaktBomCostOptionService bomCostOptionService)
    {
        _bomCostOptionService = bomCostOptionService;
    }

    /// <summary>
    /// 工厂选项（当前公司 RelatedPlant ∩ 头表未删除）
    /// </summary>
    /// <returns>下拉选项</returns>
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetBomCostOptionPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomCostOptionService.GetBomCostOptionPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料类型去重（头表；须工厂+期间；仅未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间</param>
    /// <returns>物料类型选项</returns>
    [HttpGet("material-type-options")]
    public async Task<IActionResult> GetBomCostOptionMaterialTypeOptionsAsync(
        [FromQuery] TaktBomCostOptionDto queryDto)
    {
        return await QueryBomCostOptionAsync(
            queryDto,
            dto => _bomCostOptionService.GetBomCostOptionMaterialTypeOptionsAsync(dto?.PlantCode, null, dto));
    }

    /// <summary>
    /// 机种去重（头表；须工厂+期间；仅未删除；Keyword 按编码/名称远程搜索）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType、Keyword 可选</param>
    /// <returns>机种选项</returns>
    [HttpGet("model-options")]
    public async Task<IActionResult> GetBomCostOptionModelOptionsAsync(
        [FromQuery] TaktBomCostOptionDto queryDto)
    {
        return await QueryBomCostOptionAsync(
            queryDto,
            dto => _bomCostOptionService.GetBomCostOptionModelOptionsAsync(dto?.PlantCode, dto?.Keyword, dto));
    }

    /// <summary>
    /// 产品去重（头表；须工厂+期间；仅未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType/ModelCode 可选</param>
    /// <returns>产品选项</returns>
    [HttpGet("product-options")]
    public async Task<IActionResult> GetBomCostOptionProductOptionsAsync(
        [FromQuery] TaktBomCostOptionDto queryDto)
    {
        return await QueryBomCostOptionAsync(
            queryDto,
            dto => _bomCostOptionService.GetBomCostOptionProductOptionsAsync(dto?.PlantCode, null, dto));
    }

    /// <summary>
    /// 物料/组件去重（明细；须工厂+期间；X+F+未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；Keyword/机种/产品均可空</param>
    /// <returns>物料选项</returns>
    [HttpGet("material-options")]
    public async Task<IActionResult> GetBomCostOptionMaterialOptionsAsync(
        [FromQuery] TaktBomCostOptionDto queryDto)
    {
        return await QueryBomCostOptionAsync(
            queryDto,
            dto => _bomCostOptionService.GetBomCostOptionMaterialOptionsAsync(dto?.PlantCode, dto?.Keyword, dto));
    }

    /// <summary>
    /// 级联选项入口：工厂+期间缺失时直接空列表
    /// </summary>
    /// <param name="queryDto">选项查询</param>
    /// <param name="loader">服务加载</param>
    /// <returns>下拉选项</returns>
    private async Task<IActionResult> QueryBomCostOptionAsync(
        TaktBomCostOptionDto queryDto,
        Func<TaktBomCostOptionDto?, Task<List<TaktSelectOption>>> loader)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto?.PlantCode)
                || (string.IsNullOrWhiteSpace(queryDto.PeriodStart)
                    && string.IsNullOrWhiteSpace(queryDto.PeriodEnd)))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await loader(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
