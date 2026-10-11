// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：ITaktLoginLogService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Logging;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 登录日志应用服务接口
/// </summary>
public interface ITaktLoginLogService
{
    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktLoginLogDto>> GetLoginLogListAsync(TaktLoginLogQueryDto queryDto);

    /// <summary>
    /// 获取当前登录用户的登录日志列表（分页；强制 UserName=当前用户，供个人中心）
    /// </summary>
    /// <param name="queryDto">查询DTO（高级条件可用；UserName 由服务端强制为当前用户）</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktLoginLogDto>> GetMyLoginLogListAsync(TaktLoginLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>DTO</returns>
    Task<TaktLoginLogDto?> GetLoginLogByIdAsync(long id);

    /// <summary>
    /// 获取登录日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetLoginLogOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建登录日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktLoginLogDto> CreateLoginLogAsync(TaktLoginLogCreateDto dto);

    /// <summary>
    /// 更新登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktLoginLogDto> UpdateLoginLogAsync(long id, TaktLoginLogUpdateDto dto);

    /// <summary>
    /// 删除登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>任务</returns>
    Task DeleteLoginLogByIdAsync(long id);

    /// <summary>
    /// 批量删除登录日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteLoginLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出登录日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportLoginLogAsync(TaktLoginLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
