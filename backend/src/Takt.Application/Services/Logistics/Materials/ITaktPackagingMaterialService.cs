// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktPackagingMaterialService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：包装物料应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 包装物料应用服务接口
/// </summary>
public interface ITaktPackagingMaterialService
{
    /// <summary>
    /// 获取包装物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPackagingMaterialDto>> GetPackagingMaterialListAsync(TaktPackagingMaterialQueryDto queryDto);

    /// <summary>
    /// 根据ID获取包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingMaterialDto?> GetPackagingMaterialByIdAsync(long id);

    /// <summary>
    /// 获取包装物料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPackagingMaterialOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建包装物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingMaterialDto> CreatePackagingMaterialAsync(TaktPackagingMaterialCreateDto dto);

    /// <summary>
    /// 更新包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingMaterialDto> UpdatePackagingMaterialAsync(long id, TaktPackagingMaterialUpdateDto dto);

    /// <summary>
    /// 删除包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>任务</returns>
    Task DeletePackagingMaterialByIdAsync(long id);

    /// <summary>
    /// 批量删除包装物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePackagingMaterialBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新包装物料排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingMaterialDto> UpdatePackagingMaterialSortAsync(TaktPackagingMaterialSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPackagingMaterialTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入包装物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPackagingMaterialAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出包装物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPackagingMaterialAsync(TaktPackagingMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
