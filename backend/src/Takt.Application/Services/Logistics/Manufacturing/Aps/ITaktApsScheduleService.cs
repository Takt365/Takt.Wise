// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：ITaktApsScheduleService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程主应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程主应用服务接口
/// </summary>
public interface ITaktApsScheduleService
{
    /// <summary>
    /// 获取APS排程主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktApsScheduleDto>> GetApsScheduleListAsync(TaktApsScheduleQueryDto queryDto);

    /// <summary>
    /// 根据ID获取APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>DTO</returns>
    Task<TaktApsScheduleDto?> GetApsScheduleByIdAsync(long id);

    /// <summary>
    /// 获取APS排程主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetApsScheduleOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建APS排程主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktApsScheduleDto> CreateApsScheduleAsync(TaktApsScheduleCreateDto dto);

    /// <summary>
    /// 更新APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktApsScheduleDto> UpdateApsScheduleAsync(long id, TaktApsScheduleUpdateDto dto);

    /// <summary>
    /// 删除APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>任务</returns>
    Task DeleteApsScheduleByIdAsync(long id);

    /// <summary>
    /// 批量删除APS排程主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteApsScheduleBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新APS排程主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktApsScheduleDto> UpdateApsScheduleStatusAsync(TaktApsScheduleStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetApsScheduleTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入APS排程主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportApsScheduleAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出APS排程主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportApsScheduleAsync(TaktApsScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
