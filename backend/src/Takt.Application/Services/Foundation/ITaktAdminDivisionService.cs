// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktAdminDivisionService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：行政区划应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 行政区划应用服务接口
/// </summary>
public interface ITaktAdminDivisionService
{
    /// <summary>
    /// 获取行政区划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAdminDivisionDto>> GetAdminDivisionListAsync(TaktAdminDivisionQueryDto queryDto);

    /// <summary>
    /// 根据ID获取行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>DTO</returns>
    Task<TaktAdminDivisionDto?> GetAdminDivisionByIdAsync(long id);

    /// <summary>
    /// 获取行政区划平铺选项（DictValue=DivisionCode；供省/市等业务字段下拉，非 parentId）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选；本实体无工厂列，忽略）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配编码/名称）</param>
    /// <param name="level">层级（可选；1～6，对应 sys_admin_division_level）</param>
    /// <returns>下拉选项（有上限，禁止全表）</returns>
    Task<List<TaktSelectOption>> GetAdminDivisionOptionsAsync(string? plantCode = null, string? keyword = null, int? level = null);

    /// <summary>
    /// 获取行政区划树形选项（懒加载：仅 parentId 直接子级一层；DictValue=Id 字符串，供表单 parentId）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetAdminDivisionTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取行政区划树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktAdminDivisionTreeDto>> GetAdminDivisionTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建行政区划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAdminDivisionDto> CreateAdminDivisionAsync(TaktAdminDivisionCreateDto dto);

    /// <summary>
    /// 更新行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAdminDivisionDto> UpdateAdminDivisionAsync(long id, TaktAdminDivisionUpdateDto dto);

    /// <summary>
    /// 删除行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>任务</returns>
    Task DeleteAdminDivisionByIdAsync(long id);

    /// <summary>
    /// 批量删除行政区划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAdminDivisionBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新行政区划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAdminDivisionDto> UpdateAdminDivisionStatusAsync(TaktAdminDivisionStatusDto dto);

    /// <summary>
    /// 更新行政区划排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAdminDivisionDto> UpdateAdminDivisionSortAsync(TaktAdminDivisionSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetAdminDivisionTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入行政区划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportAdminDivisionAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出行政区划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAdminDivisionAsync(TaktAdminDivisionQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
