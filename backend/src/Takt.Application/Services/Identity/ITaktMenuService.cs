// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktMenuService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 菜单应用服务接口
/// </summary>
public interface ITaktMenuService
{
    /// <summary>
    /// 获取菜单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMenuDto>> GetMenuListAsync(TaktMenuQueryDto queryDto);

    /// <summary>
    /// 根据ID获取菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>DTO</returns>
    Task<TaktMenuDto?> GetMenuByIdAsync(long id);

    /// <summary>
    /// 获取菜单树形选项列表
    /// </summary>
    /// <param name="valueBy">选项值：id=菜单Id（默认）；name=菜单名称 MenuName（编码规则 DocumentType）</param>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项</returns>
    Task<List<TaktTreeSelectOption>> GetMenuTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null, string? valueBy = null);

    /// <summary>
    /// 获取菜单树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    Task<List<TaktMenuTreeDto>> GetMenuTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 角色菜单分配：互斥懒加载树（一层）。未分配与已分配节点不重叠；父在对侧时子节点升为该侧根。
    /// </summary>
    /// <param name="queryDto">父级、侧别、已分配 Id 列表</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktMenuTreeDto>> GetMenuAssignTreeAsync(TaktMenuAssignTreeQueryDto queryDto);

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMenuDto> CreateMenuAsync(TaktMenuCreateDto dto);

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMenuDto> UpdateMenuAsync(long id, TaktMenuUpdateDto dto);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>任务</returns>
    Task DeleteMenuByIdAsync(long id);

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMenuBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新菜单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMenuDto> UpdateMenuStatusAsync(TaktMenuStatusDto dto);

    /// <summary>
    /// 更新菜单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMenuDto> UpdateMenuSortAsync(TaktMenuSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMenuTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入菜单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMenuAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出菜单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMenuAsync(TaktMenuQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
