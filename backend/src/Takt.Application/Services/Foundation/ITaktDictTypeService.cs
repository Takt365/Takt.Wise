// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktDictTypeService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 字典类型应用服务接口
/// </summary>
public interface ITaktDictTypeService
{
    /// <summary>
    /// 获取字典类型列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDictTypeDto>> GetDictTypeListAsync(TaktDictTypeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto?> GetDictTypeByIdAsync(long id);

    /// <summary>
    /// 获取字典类型选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDictTypeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建字典类型
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto> CreateDictTypeAsync(TaktDictTypeCreateDto dto);

    /// <summary>
    /// 更新字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto> UpdateDictTypeAsync(long id, TaktDictTypeUpdateDto dto);

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>任务</returns>
    Task DeleteDictTypeByIdAsync(long id);

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteDictTypeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新字典类型状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto> UpdateDictTypeStatusAsync(TaktDictTypeStatusDto dto);

    /// <summary>
    /// 更新字典类型排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto> UpdateDictTypeSortAsync(TaktDictTypeSortDto dto);

    /// <summary>
    /// 更新字典类型内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDictTypeDto> UpdateDictTypeBuiltInAsync(TaktDictTypeBuiltInDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetDictTypeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入字典类型
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportDictTypeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出字典类型
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportDictTypeAsync(TaktDictTypeQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
