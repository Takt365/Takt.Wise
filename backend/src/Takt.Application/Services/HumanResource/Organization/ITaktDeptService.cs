// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Organization
// 文件名称：ITaktDeptService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：部门应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Organization;

/// <summary>
/// 部门应用服务接口
/// </summary>
public interface ITaktDeptService
{
    /// <summary>
    /// 获取部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDeptDto>> GetDeptListAsync(TaktDeptQueryDto queryDto);

    /// <summary>
    /// 根据ID获取部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto?> GetDeptByIdAsync(long id);

    /// <summary>
    /// 获取部门树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetDeptTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取部门选项（DictValue=DeptCode，DictLabel=DeptName1）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDeptOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取部门 ISO 编码树形选项列表（懒加载：DictValue=IsoCode，用于编码规则等部门段选择）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetDeptIsoTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取部门树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktDeptTreeDto>> GetDeptTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto> CreateDeptAsync(TaktDeptCreateDto dto);

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto> UpdateDeptAsync(long id, TaktDeptUpdateDto dto);

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>任务</returns>
    Task DeleteDeptByIdAsync(long id);

    /// <summary>
    /// 批量删除部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteDeptBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto> UpdateDeptStatusAsync(TaktDeptStatusDto dto);

    /// <summary>
    /// 更新部门内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto> UpdateDeptBuiltInAsync(TaktDeptBuiltInDto dto);

    /// <summary>
    /// 更新部门排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDeptDto> UpdateDeptSortAsync(TaktDeptSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetDeptTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入部门
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportDeptAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出部门
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportDeptAsync(TaktDeptQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
