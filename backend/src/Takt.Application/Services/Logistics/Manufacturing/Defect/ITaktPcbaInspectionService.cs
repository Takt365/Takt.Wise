// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：ITaktPcbaInspectionService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查日报应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查日报应用服务接口
/// </summary>
public interface ITaktPcbaInspectionService
{
    /// <summary>
    /// 获取PCBA检查日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPcbaInspectionDto>> GetPcbaInspectionListAsync(TaktPcbaInspectionQueryDto queryDto);

    /// <summary>
    /// 根据ID获取PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaInspectionDto?> GetPcbaInspectionByIdAsync(long id);

    /// <summary>
    /// 获取PCBA检查日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPcbaInspectionOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建PCBA检查日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaInspectionDto> CreatePcbaInspectionAsync(TaktPcbaInspectionCreateDto dto);

    /// <summary>
    /// 更新PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaInspectionDto> UpdatePcbaInspectionAsync(long id, TaktPcbaInspectionUpdateDto dto);

    /// <summary>
    /// 删除PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>任务</returns>
    Task DeletePcbaInspectionByIdAsync(long id);

    /// <summary>
    /// 批量删除PCBA检查日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePcbaInspectionBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPcbaInspectionTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入PCBA检查日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPcbaInspectionAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出PCBA检查日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPcbaInspectionAsync(TaktPcbaInspectionQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取 PCBA 检查不良统计（数据看板 defect-stat；按明细实装日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 检查统计</returns>
    Task<TaktPcbaInspectionStatDto> GetPcbaInspectionStatAsync(TaktDefectStatQueryDto queryDto);
}
