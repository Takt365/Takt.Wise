// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：ITaktFlowTaskService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：流程用户任务应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Workflow;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程用户任务应用服务接口
/// </summary>
public interface ITaktFlowTaskService
{
    /// <summary>
    /// 获取流程用户任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktFlowTaskDto>> GetFlowTaskListAsync(TaktFlowTaskQueryDto queryDto);

    /// <summary>
    /// 根据ID获取流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>DTO</returns>
    Task<TaktFlowTaskDto?> GetFlowTaskByIdAsync(long id);

    /// <summary>
    /// 获取流程用户任务选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetFlowTaskOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建流程用户任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFlowTaskDto> CreateFlowTaskAsync(TaktFlowTaskCreateDto dto);

    /// <summary>
    /// 更新流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFlowTaskDto> UpdateFlowTaskAsync(long id, TaktFlowTaskUpdateDto dto);

    /// <summary>
    /// 删除流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>任务</returns>
    Task DeleteFlowTaskByIdAsync(long id);

    /// <summary>
    /// 批量删除流程用户任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteFlowTaskBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新流程用户任务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFlowTaskDto> UpdateFlowTaskStatusAsync(TaktFlowTaskStatusDto dto);

    /// <summary>
    /// 更新流程用户任务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFlowTaskDto> UpdateFlowTaskSortAsync(TaktFlowTaskSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetFlowTaskTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入流程用户任务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportFlowTaskAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出流程用户任务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportFlowTaskAsync(TaktFlowTaskQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
