// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：ITaktSopAckService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP确认应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP确认应用服务接口
/// </summary>
public interface ITaktSopAckService
{
    /// <summary>
    /// 获取SOP确认列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSopAckDto>> GetSopAckListAsync(TaktSopAckQueryDto queryDto);

    /// <summary>
    /// 根据ID获取SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>DTO</returns>
    Task<TaktSopAckDto?> GetSopAckByIdAsync(long id);

    /// <summary>
    /// 获取SOP确认选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSopAckOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建SOP确认
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSopAckDto> CreateSopAckAsync(TaktSopAckCreateDto dto);

    /// <summary>
    /// 更新SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSopAckDto> UpdateSopAckAsync(long id, TaktSopAckUpdateDto dto);

    /// <summary>
    /// 删除SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>任务</returns>
    Task DeleteSopAckByIdAsync(long id);

    /// <summary>
    /// 批量删除SOP确认
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSopAckBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSopAckTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入SOP确认
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSopAckAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出SOP确认
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSopAckAsync(TaktSopAckQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
