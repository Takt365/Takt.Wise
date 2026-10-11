// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktSystemPropertyService.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：系统属性应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 系统属性应用服务接口
/// </summary>
public interface ITaktSystemPropertyService
{
    /// <summary>
    /// 获取系统属性列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSystemPropertyDto>> GetSystemPropertyListAsync(TaktSystemPropertyQueryDto queryDto);

    /// <summary>
    /// 根据ID获取系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto?> GetSystemPropertyByIdAsync(long id);

    /// <summary>
    /// 获取系统属性选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSystemPropertyOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建系统属性
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto> CreateSystemPropertyAsync(TaktSystemPropertyCreateDto dto);

    /// <summary>
    /// 更新系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto> UpdateSystemPropertyAsync(long id, TaktSystemPropertyUpdateDto dto);

    /// <summary>
    /// 删除系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>任务</returns>
    Task DeleteSystemPropertyByIdAsync(long id);

    /// <summary>
    /// 批量删除系统属性
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSystemPropertyBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新系统属性状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto> UpdateSystemPropertyStatusAsync(TaktSystemPropertyStatusDto dto);

    /// <summary>
    /// 更新系统属性内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto> UpdateSystemPropertyBuiltInAsync(TaktSystemPropertyBuiltInDto dto);

    /// <summary>
    /// 更新系统属性排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSystemPropertyDto> UpdateSystemPropertySortAsync(TaktSystemPropertySortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSystemPropertyTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入系统属性
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSystemPropertyAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出系统属性
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSystemPropertyAsync(TaktSystemPropertyQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
