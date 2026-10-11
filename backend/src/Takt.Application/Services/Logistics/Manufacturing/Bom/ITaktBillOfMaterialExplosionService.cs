// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBillOfMaterialExplosionService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 递归展开服务接口（与物料清单 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 递归展开服务（读 BOM/明细；与 TaktBillOfMaterialService 分离）
/// </summary>
public interface ITaktBillOfMaterialExplosionService
{
    /// <summary>
    /// BOM 递归展开（运行时多层展开，单层存储）
    /// </summary>
    /// <param name="query">展开参数</param>
    /// <returns>展开结果；BOM 不存在时返回 null</returns>
    Task<TaktBillOfMaterialExplosionDto?> GetBillOfMaterialExplosionAsync(TaktBillOfMaterialExplosionQueryDto query);
}
