// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：ITaktSelfServiceService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：自助服务应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 自助服务应用服务接口
/// </summary>
public interface ITaktSelfServiceService
{
    /// <summary>
    /// 获取自助服务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSelfServiceDto>> GetSelfServiceListAsync(TaktSelfServiceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>DTO</returns>
    Task<TaktSelfServiceDto?> GetSelfServiceByIdAsync(long id);

    /// <summary>
    /// 获取自助服务选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSelfServiceOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建自助服务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSelfServiceDto> CreateSelfServiceAsync(TaktSelfServiceCreateDto dto);

    /// <summary>
    /// 更新自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSelfServiceDto> UpdateSelfServiceAsync(long id, TaktSelfServiceUpdateDto dto);

    /// <summary>
    /// 删除自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>任务</returns>
    Task DeleteSelfServiceByIdAsync(long id);

    /// <summary>
    /// 批量删除自助服务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSelfServiceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新自助服务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSelfServiceDto> UpdateSelfServiceStatusAsync(TaktSelfServiceStatusDto dto);

    /// <summary>
    /// 更新自助服务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSelfServiceDto> UpdateSelfServiceSortAsync(TaktSelfServiceSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSelfServiceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入自助服务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSelfServiceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出自助服务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSelfServiceAsync(TaktSelfServiceQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
