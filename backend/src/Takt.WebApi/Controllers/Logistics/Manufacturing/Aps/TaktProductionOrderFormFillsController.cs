// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrderFormFillsController.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单表单回填控制器（独立非实体 CRUD；generate-controllers 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单表单回填控制器（组立/PCBA 等选工单后自动赋值）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产工单表单回填")]
public class TaktProductionOrderFormFillsController : TaktControllerBase
{
    private readonly ITaktProductionOrderFormFillService _productionOrderFormFillService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderFormFillService">表单回填服务</param>
    public TaktProductionOrderFormFillsController(
        ITaktProductionOrderFormFillService productionOrderFormFillService)
    {
        _productionOrderFormFillService = productionOrderFormFillService;
    }

    /// <summary>
    /// 按工单号获取表单回填（工单类别/机种/物料/批次/数量/序号/标准工时等）
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>回填 DTO；不存在时 data 为 null</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:query", "生产工单表单回填")]
    [HttpGet("by-code")]
    public async Task<IActionResult> GetProductionOrderFormFillByCodeAsync(
        [FromQuery] TaktProductionOrderFormFillQueryDto queryDto)
    {
        try
        {
            var result = await _productionOrderFormFillService.GetProductionOrderFormFillAsync(queryDto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
