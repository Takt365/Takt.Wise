// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：ITaktMasterProductionScheduleLineService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS行应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 主生产计划MPS行应用服务接口
/// </summary>
public interface ITaktMasterProductionScheduleLineService
{
    /// <summary>
    /// 获取主生产计划MPS行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMasterProductionScheduleLineDto>> GetMasterProductionScheduleLineListAsync(TaktMasterProductionScheduleLineQueryDto queryDto);

    /// <summary>
    /// 根据ID获取主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>DTO</returns>
    Task<TaktMasterProductionScheduleLineDto?> GetMasterProductionScheduleLineByIdAsync(long id);

    /// <summary>
    /// 获取主生产计划MPS行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMasterProductionScheduleLineOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建主生产计划MPS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMasterProductionScheduleLineDto> CreateMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineCreateDto dto);

    /// <summary>
    /// 更新主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMasterProductionScheduleLineDto> UpdateMasterProductionScheduleLineAsync(long id, TaktMasterProductionScheduleLineUpdateDto dto);

    /// <summary>
    /// 删除主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>任务</returns>
    Task DeleteMasterProductionScheduleLineByIdAsync(long id);

    /// <summary>
    /// 批量删除主生产计划MPS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMasterProductionScheduleLineBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新主生产计划MPS行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMasterProductionScheduleLineDto> UpdateMasterProductionScheduleLineObsoleteAsync(TaktMasterProductionScheduleLineObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMasterProductionScheduleLineTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入主生产计划MPS行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMasterProductionScheduleLineAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出主生产计划MPS行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
