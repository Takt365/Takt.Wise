// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostItemService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM物料成本明细应用服务接口
/// </summary>
public interface ITaktBomMaterialCostItemService
{
    /// <summary>
    /// 获取BOM物料成本明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBomMaterialCostItemDto>> GetBomMaterialCostItemListAsync(TaktBomMaterialCostItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto?> GetBomMaterialCostItemByIdAsync(long id);

    /// <summary>
    /// 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项（DictValue=产品编码）</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostItemOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto> CreateBomMaterialCostItemAsync(TaktBomMaterialCostItemCreateDto dto);

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto> UpdateBomMaterialCostItemAsync(long id, TaktBomMaterialCostItemUpdateDto dto);

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostItemByIdAsync(long id);

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBomMaterialCostItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出BOM物料成本明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemAsync(TaktBomMaterialCostItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
