// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktUserPreferenceService.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：用户个人偏好应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 用户个人偏好应用服务接口
/// </summary>
public interface ITaktUserPreferenceService
{
    /// <summary>
    /// 获取用户个人偏好列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktUserPreferenceDto>> GetUserPreferenceListAsync(TaktUserPreferenceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>DTO</returns>
    Task<TaktUserPreferenceDto?> GetUserPreferenceByIdAsync(long id);

    /// <summary>
    /// 获取用户个人偏好选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetUserPreferenceOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建用户个人偏好
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktUserPreferenceDto> CreateUserPreferenceAsync(TaktUserPreferenceCreateDto dto);

    /// <summary>
    /// 更新用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktUserPreferenceDto> UpdateUserPreferenceAsync(long id, TaktUserPreferenceUpdateDto dto);

    /// <summary>
    /// 删除用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>任务</returns>
    Task DeleteUserPreferenceByIdAsync(long id);

    /// <summary>
    /// 批量删除用户个人偏好
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteUserPreferenceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新用户个人偏好状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktUserPreferenceDto> UpdateUserPreferenceStatusAsync(TaktUserPreferenceStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetUserPreferenceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入用户个人偏好
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportUserPreferenceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出用户个人偏好
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportUserPreferenceAsync(TaktUserPreferenceQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取当前登录用户的个人偏好（无记录返回 null）
    /// </summary>
    /// <returns>DTO 或 null</returns>
    Task<TaktUserPreferenceDto?> GetMyUserPreferenceAsync();

    /// <summary>
    /// 按当前登录用户 Upsert 个人偏好（忽略 body 中的 UserId）
    /// </summary>
    /// <param name="dto">Upsert DTO</param>
    /// <returns>DTO</returns>
    Task<TaktUserPreferenceDto> UpsertMyUserPreferenceAsync(TaktUserPreferenceMeUpsertDto dto);
}
