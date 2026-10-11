// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Training
// 文件名称：ITaktTrainingPlanService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：培训计划应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Training;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Training;

/// <summary>
/// 培训计划应用服务接口
/// </summary>
public interface ITaktTrainingPlanService
{
    /// <summary>
    /// 获取培训计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTrainingPlanDto>> GetTrainingPlanListAsync(TaktTrainingPlanQueryDto queryDto);

    /// <summary>
    /// 根据ID获取培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingPlanDto?> GetTrainingPlanByIdAsync(long id);

    /// <summary>
    /// 获取培训计划选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTrainingPlanOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建培训计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingPlanDto> CreateTrainingPlanAsync(TaktTrainingPlanCreateDto dto);

    /// <summary>
    /// 更新培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingPlanDto> UpdateTrainingPlanAsync(long id, TaktTrainingPlanUpdateDto dto);

    /// <summary>
    /// 删除培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>任务</returns>
    Task DeleteTrainingPlanByIdAsync(long id);

    /// <summary>
    /// 批量删除培训计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTrainingPlanBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新培训计划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingPlanDto> UpdateTrainingPlanStatusAsync(TaktTrainingPlanStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTrainingPlanTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入培训计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTrainingPlanAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出培训计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTrainingPlanAsync(TaktTrainingPlanQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
