// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：ITaktMeetingRoomService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：会议室应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议室应用服务接口
/// </summary>
public interface ITaktMeetingRoomService
{
    /// <summary>
    /// 获取会议室列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMeetingRoomDto>> GetMeetingRoomListAsync(TaktMeetingRoomQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingRoomDto?> GetMeetingRoomByIdAsync(long id);

    /// <summary>
    /// 获取会议室选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMeetingRoomOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建会议室
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingRoomDto> CreateMeetingRoomAsync(TaktMeetingRoomCreateDto dto);

    /// <summary>
    /// 更新会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingRoomDto> UpdateMeetingRoomAsync(long id, TaktMeetingRoomUpdateDto dto);

    /// <summary>
    /// 删除会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>任务</returns>
    Task DeleteMeetingRoomByIdAsync(long id);

    /// <summary>
    /// 批量删除会议室
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMeetingRoomBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新会议室状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingRoomDto> UpdateMeetingRoomStatusAsync(TaktMeetingRoomStatusDto dto);

    /// <summary>
    /// 更新会议室排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingRoomDto> UpdateMeetingRoomSortAsync(TaktMeetingRoomSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMeetingRoomTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入会议室
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMeetingRoomAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出会议室
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMeetingRoomAsync(TaktMeetingRoomQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
