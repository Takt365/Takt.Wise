// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBillOfMaterialSubstituteService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM替代料应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM替代料应用服务接口
/// </summary>
public interface ITaktBillOfMaterialSubstituteService
{
    /// <summary>
    /// 获取BOM替代料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBillOfMaterialSubstituteDto>> GetBillOfMaterialSubstituteListAsync(TaktBillOfMaterialSubstituteQueryDto queryDto);

    /// <summary>
    /// 根据ID获取BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>DTO</returns>
    Task<TaktBillOfMaterialSubstituteDto?> GetBillOfMaterialSubstituteByIdAsync(long id);

    /// <summary>
    /// 获取BOM替代料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBillOfMaterialSubstituteOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建BOM替代料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBillOfMaterialSubstituteDto> CreateBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteCreateDto dto);

    /// <summary>
    /// 更新BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteAsync(long id, TaktBillOfMaterialSubstituteUpdateDto dto);

    /// <summary>
    /// 删除BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>任务</returns>
    Task DeleteBillOfMaterialSubstituteByIdAsync(long id);

    /// <summary>
    /// 批量删除BOM替代料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBillOfMaterialSubstituteBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新BOM替代料作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteObsoleteAsync(TaktBillOfMaterialSubstituteObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBillOfMaterialSubstituteTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入BOM替代料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialSubstituteAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出BOM替代料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
