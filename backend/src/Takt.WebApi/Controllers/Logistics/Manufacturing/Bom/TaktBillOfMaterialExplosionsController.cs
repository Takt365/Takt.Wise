// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialExplosionsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 递归展开控制器（与物料清单 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 递归展开控制器（与 TaktBillOfMaterialsController 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料清单展开")]
public class TaktBillOfMaterialExplosionsController : TaktControllerBase
{
    private readonly ITaktBillOfMaterialExplosionService _billOfMaterialExplosionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialExplosionService">BOM 展开服务</param>
    public TaktBillOfMaterialExplosionsController(ITaktBillOfMaterialExplosionService billOfMaterialExplosionService)
    {
        _billOfMaterialExplosionService = billOfMaterialExplosionService;
    }

    /// <summary>
    /// BOM 递归展开（运行时多层展开，单层存储）
    /// </summary>
    /// <param name="query">展开参数</param>
    /// <returns>展开结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "物料清单展开")]
    [HttpGet]
    public async Task<IActionResult> GetBillOfMaterialExplosionAsync([FromQuery] TaktBillOfMaterialExplosionQueryDto query)
    {
        try
        {
            var result = await _billOfMaterialExplosionService.GetBillOfMaterialExplosionAsync(query);
            if (result == null)
            {
                return NotFound("物料清单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
