// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：ITaktCostElementService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 成本要素应用服务接口
/// </summary>
public interface ITaktCostElementService
{
    /// <summary>
    /// 获取成本要素列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCostElementDto>> GetCostElementListAsync(TaktCostElementQueryDto queryDto);

    /// <summary>
    /// 根据ID获取成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>DTO</returns>
    Task<TaktCostElementDto?> GetCostElementByIdAsync(long id);

    /// <summary>
    /// 获取成本要素树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 CostElementCode）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetCostElementTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取成本要素树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktCostElementTreeDto>> GetCostElementTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建成本要素
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostElementDto> CreateCostElementAsync(TaktCostElementCreateDto dto);

    /// <summary>
    /// 更新成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostElementDto> UpdateCostElementAsync(long id, TaktCostElementUpdateDto dto);

    /// <summary>
    /// 删除成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>任务</returns>
    Task DeleteCostElementByIdAsync(long id);

    /// <summary>
    /// 批量删除成本要素
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCostElementBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新成本要素状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostElementDto> UpdateCostElementStatusAsync(TaktCostElementStatusDto dto);

    /// <summary>
    /// 更新成本要素排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostElementDto> UpdateCostElementSortAsync(TaktCostElementSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCostElementTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入成本要素
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCostElementAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出成本要素
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCostElementAsync(TaktCostElementQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
