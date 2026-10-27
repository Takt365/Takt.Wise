// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialUsageService.cs
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 通用物料与专用物料：主表 TaktBomMaterialCost 与子表 TaktBomMaterialCostItem 按产品编码联合查询。只在一个机种出现为专用，出现在两个及以上机种为通用。
/// </summary>
public interface ITaktBomMaterialUsageService
{
    /// <summary>
    /// 通用物料分页（物料出现在两个及以上机种）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>分页行</returns>
    Task<TaktPagedResult<TaktBomMaterialUsageDto>> GetBomCommonMaterialListAsync(
        TaktBomMaterialUsageQueryDto queryDto);

    /// <summary>
    /// 专用物料分页（物料只出现在一个机种，其它机种都没有）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>分页行</returns>
    Task<TaktPagedResult<TaktBomMaterialUsageDto>> GetBomDedicatedMaterialListAsync(
        TaktBomMaterialUsageQueryDto queryDto);

    /// <summary>
    /// 导出通用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomCommonMaterialAsync(
        TaktBomMaterialUsageQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 物料下拉：TaktBomMaterialCostItem.ComponentCode 去重（工厂 + 核算年月；采购类型 F、生产相关 X、PCB SECT 标识为空）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算年月 yyyy-MM</param>
    /// <param name="keyword">编码或描述关键字，空=该月全部物料</param>
    /// <returns>DictValue=ComponentCode</returns>
    Task<List<TaktSelectOption>> GetBomMaterialUsageComponentOptionsAsync(
        string? plantCode,
        string? costingPeriod,
        string? keyword = null);

    /// <summary>
    /// 导出专用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomDedicatedMaterialAsync(
        TaktBomMaterialUsageQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
