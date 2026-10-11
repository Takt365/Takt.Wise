// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcGroupService.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变组主数据应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变组主数据应用服务接口
/// </summary>
public interface ITaktEcGroupService
{
    /// <summary>
    /// 获取设变组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcGroupDto>> GetEcGroupListAsync(TaktEcGroupQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcGroupDto?> GetEcGroupByIdAsync(long id);

    /// <summary>
    /// 获取设变组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="ecGroupCategory">设变组类别（可选；部门编码，多个逗号分隔）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcGroupOptionsAsync(string? plantCode = null, string? keyword = null, string? ecGroupCategory = null);

    /// <summary>
    /// 创建设变组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcGroupDto> CreateEcGroupAsync(TaktEcGroupCreateDto dto);

    /// <summary>
    /// 更新设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcGroupDto> UpdateEcGroupAsync(long id, TaktEcGroupUpdateDto dto);

    /// <summary>
    /// 删除设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>任务</returns>
    Task DeleteEcGroupByIdAsync(long id);

    /// <summary>
    /// 批量删除设变组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcGroupBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcGroupDto> UpdateEcGroupStatusAsync(TaktEcGroupStatusDto dto);

    /// <summary>
    /// 更新设变组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcGroupDto> UpdateEcGroupSortAsync(TaktEcGroupSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcGroupTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcGroupAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcGroupAsync(TaktEcGroupQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
