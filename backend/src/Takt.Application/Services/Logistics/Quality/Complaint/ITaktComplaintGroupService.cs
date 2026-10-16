// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：ITaktComplaintGroupService.cs
// 创建时间：2026-10-12
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉组主数据应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 客诉组主数据应用服务接口
/// </summary>
public interface ITaktComplaintGroupService
{
    /// <summary>
    /// 获取客诉组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktComplaintGroupDto>> GetComplaintGroupListAsync(TaktComplaintGroupQueryDto queryDto);

    /// <summary>
    /// 根据ID获取客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto?> GetComplaintGroupByIdAsync(long id);

    /// <summary>
    /// 获取客诉组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="complaintGroupCategory">客诉组类别（可选；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetComplaintGroupOptionsAsync(string? plantCode = null, string? keyword = null, int? complaintGroupCategory = null);

    /// <summary>
    /// 创建客诉组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto> CreateComplaintGroupAsync(TaktComplaintGroupCreateDto dto);

    /// <summary>
    /// 更新客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto> UpdateComplaintGroupAsync(long id, TaktComplaintGroupUpdateDto dto);

    /// <summary>
    /// 删除客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <returns>任务</returns>
    Task DeleteComplaintGroupByIdAsync(long id);

    /// <summary>
    /// 批量删除客诉组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteComplaintGroupBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新客诉组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto> UpdateComplaintGroupStatusAsync(TaktComplaintGroupStatusDto dto);

    /// <summary>
    /// 更新客诉组主数据内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto> UpdateComplaintGroupBuiltInAsync(TaktComplaintGroupBuiltInDto dto);

    /// <summary>
    /// 更新客诉组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktComplaintGroupDto> UpdateComplaintGroupSortAsync(TaktComplaintGroupSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetComplaintGroupTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入客诉组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportComplaintGroupAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出客诉组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportComplaintGroupAsync(TaktComplaintGroupQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
