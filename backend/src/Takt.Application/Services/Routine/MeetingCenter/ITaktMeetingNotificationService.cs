// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：ITaktMeetingNotificationService.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议通知应用服务接口
/// </summary>
public interface ITaktMeetingNotificationService
{
    /// <summary>
    /// 获取会议通知列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMeetingNotificationDto>> GetMeetingNotificationListAsync(TaktMeetingNotificationQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingNotificationDto?> GetMeetingNotificationByIdAsync(long id);

    /// <summary>
    /// 获取会议通知选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMeetingNotificationOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建会议通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingNotificationDto> CreateMeetingNotificationAsync(TaktMeetingNotificationCreateDto dto);

    /// <summary>
    /// 更新会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingNotificationDto> UpdateMeetingNotificationAsync(long id, TaktMeetingNotificationUpdateDto dto);

    /// <summary>
    /// 删除会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>任务</returns>
    Task DeleteMeetingNotificationByIdAsync(long id);

    /// <summary>
    /// 批量删除会议通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMeetingNotificationBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新会议通知状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMeetingNotificationDto> UpdateMeetingNotificationStatusAsync(TaktMeetingNotificationStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMeetingNotificationTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入会议通知
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMeetingNotificationAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出会议通知
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMeetingNotificationAsync(TaktMeetingNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 按邮件令牌确认收到会议通知（匿名；凭令牌定位记录）
    /// </summary>
    /// <param name="dto">令牌 DTO</param>
    /// <returns>确认结果</returns>
    Task<TaktMeetingNotificationConfirmReceiptResultDto> ConfirmMeetingNotificationReceiptByTokenAsync(
        TaktMeetingNotificationConfirmReceiptByTokenDto dto);

    /// <summary>
    /// 当前登录用户确认收到会议通知（须为通知收件人）
    /// </summary>
    /// <param name="id">会议通知 ID</param>
    /// <returns>确认结果</returns>
    Task<TaktMeetingNotificationConfirmReceiptResultDto> ConfirmMeetingNotificationReceiptAsync(long id);

}
