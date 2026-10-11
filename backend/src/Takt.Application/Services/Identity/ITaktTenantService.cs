// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktTenantService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：租户应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 租户应用服务接口
/// </summary>
public interface ITaktTenantService
{
    /// <summary>
    /// 获取租户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTenantDto>> GetTenantListAsync(TaktTenantQueryDto queryDto);

    /// <summary>
    /// 根据ID获取租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>DTO</returns>
    Task<TaktTenantDto?> GetTenantByIdAsync(long id);

    /// <summary>
    /// 获取当前登录会话的租户选项（仅一项，DictValue 为 TenantCode；登录后不可跨租户切换）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>当前租户下拉项</returns>
    Task<List<TaktSelectOption>> GetTenantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTenantDto> CreateTenantAsync(TaktTenantCreateDto dto);

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTenantDto> UpdateTenantAsync(long id, TaktTenantUpdateDto dto);

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>任务</returns>
    Task DeleteTenantByIdAsync(long id);

    /// <summary>
    /// 批量删除租户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTenantBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新租户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTenantDto> UpdateTenantStatusAsync(TaktTenantStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTenantTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入租户
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTenantAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出租户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTenantAsync(TaktTenantQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
