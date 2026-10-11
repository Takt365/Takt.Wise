// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：ITaktMeetingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议中心应用服务接口
/// </summary>
public interface ITaktMeetingService
{
    /// <summary>
    /// 获取会议中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMeetingDto>> GetMeetingListAsync(TaktMeetingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingDto?> GetMeetingByIdAsync(long id);

    /// <summary>
    /// 获取会议中心主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMeetingOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingDto> CreateMeetingAsync(TaktMeetingCreateDto dto);

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingDto> UpdateMeetingAsync(long id, TaktMeetingUpdateDto dto);

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>任务</returns>
    Task DeleteMeetingByIdAsync(long id);

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMeetingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingDto> UpdateMeetingStatusAsync(TaktMeetingStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMeetingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMeetingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出会议中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMeetingAsync(TaktMeetingQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取会议件数统计（数据看板；按 StartTime 与 MeetingStatus）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>会议件数统计</returns>
    Task<TaktMeetingStatDto> GetMeetingStatAsync(TaktMeetingStatQueryDto queryDto);
}
