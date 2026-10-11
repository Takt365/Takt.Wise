// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：ITaktProfitCenterService.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 利润中心应用服务接口
/// </summary>
public interface ITaktProfitCenterService
{
    /// <summary>
    /// 获取利润中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktProfitCenterDto>> GetProfitCenterListAsync(TaktProfitCenterQueryDto queryDto);

    /// <summary>
    /// 根据ID获取利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>DTO</returns>
    Task<TaktProfitCenterDto?> GetProfitCenterByIdAsync(long id);

    /// <summary>
    /// 获取利润中心树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetProfitCenterTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取利润中心树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    Task<List<TaktProfitCenterTreeDto>> GetProfitCenterTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建利润中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProfitCenterDto> CreateProfitCenterAsync(TaktProfitCenterCreateDto dto);

    /// <summary>
    /// 更新利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProfitCenterDto> UpdateProfitCenterAsync(long id, TaktProfitCenterUpdateDto dto);

    /// <summary>
    /// 删除利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>任务</returns>
    Task DeleteProfitCenterByIdAsync(long id);

    /// <summary>
    /// 批量删除利润中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteProfitCenterBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新利润中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProfitCenterDto> UpdateProfitCenterStatusAsync(TaktProfitCenterStatusDto dto);

    /// <summary>
    /// 更新利润中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProfitCenterDto> UpdateProfitCenterSortAsync(TaktProfitCenterSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetProfitCenterTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入利润中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportProfitCenterAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出利润中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportProfitCenterAsync(TaktProfitCenterQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
