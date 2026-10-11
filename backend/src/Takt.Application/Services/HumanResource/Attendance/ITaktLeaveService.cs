// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：ITaktLeaveService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：请假信息应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 请假信息应用服务接口
/// </summary>
public interface ITaktLeaveService
{
    /// <summary>
    /// 获取请假信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktLeaveDto>> GetLeaveListAsync(TaktLeaveQueryDto queryDto);

    /// <summary>
    /// 根据ID获取请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>DTO</returns>
    Task<TaktLeaveDto?> GetLeaveByIdAsync(long id);

    /// <summary>
    /// 获取请假信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetLeaveOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建请假信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktLeaveDto> CreateLeaveAsync(TaktLeaveCreateDto dto);

    /// <summary>
    /// 更新请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktLeaveDto> UpdateLeaveAsync(long id, TaktLeaveUpdateDto dto);

    /// <summary>
    /// 删除请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>任务</returns>
    Task DeleteLeaveByIdAsync(long id);

    /// <summary>
    /// 批量删除请假信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteLeaveBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新请假信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktLeaveDto> UpdateLeaveStatusAsync(TaktLeaveStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetLeaveTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入请假信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportLeaveAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出请假信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportLeaveAsync(TaktLeaveQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
