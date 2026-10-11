// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：ITaktQualityIssueService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质问题应对主应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质问题应对主应用服务接口
/// </summary>
public interface ITaktQualityIssueService
{
    /// <summary>
    /// 获取品质问题应对主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktQualityIssueDto>> GetQualityIssueListAsync(TaktQualityIssueQueryDto queryDto);

    /// <summary>
    /// 根据ID获取品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssueDto?> GetQualityIssueByIdAsync(long id);

    /// <summary>
    /// 获取品质问题应对主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetQualityIssueOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建品质问题应对主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssueDto> CreateQualityIssueAsync(TaktQualityIssueCreateDto dto);

    /// <summary>
    /// 更新品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssueDto> UpdateQualityIssueAsync(long id, TaktQualityIssueUpdateDto dto);

    /// <summary>
    /// 删除品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>任务</returns>
    Task DeleteQualityIssueByIdAsync(long id);

    /// <summary>
    /// 批量删除品质问题应对主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteQualityIssueBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetQualityIssueTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入品质问题应对主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportQualityIssueAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出品质问题应对主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportQualityIssueAsync(TaktQualityIssueQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取品质应对金额统计（数据看板；按 IssueDate 汇总 TotalCost）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质应对金额统计</returns>
    Task<TaktQualityCostStatDto> GetQualityIssueCostStatAsync(TaktQualityCostStatQueryDto queryDto);
}
