// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：ITaktQualityIssuePcbaReworkService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题PCBA不良改修费用明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 质量问题PCBA不良改修费用明细应用服务接口
/// </summary>
public interface ITaktQualityIssuePcbaReworkService
{
    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktQualityIssuePcbaReworkDto>> GetQualityIssuePcbaReworkListAsync(TaktQualityIssuePcbaReworkQueryDto queryDto);

    /// <summary>
    /// 根据ID获取质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssuePcbaReworkDto?> GetQualityIssuePcbaReworkByIdAsync(long id);

    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetQualityIssuePcbaReworkOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssuePcbaReworkDto> CreateQualityIssuePcbaReworkAsync(TaktQualityIssuePcbaReworkCreateDto dto);

    /// <summary>
    /// 更新质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssuePcbaReworkDto> UpdateQualityIssuePcbaReworkAsync(long id, TaktQualityIssuePcbaReworkUpdateDto dto);

    /// <summary>
    /// 删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>任务</returns>
    Task DeleteQualityIssuePcbaReworkByIdAsync(long id);

    /// <summary>
    /// 批量删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteQualityIssuePcbaReworkBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新质量问题PCBA不良改修费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityIssuePcbaReworkDto> UpdateQualityIssuePcbaReworkObsoleteAsync(TaktQualityIssuePcbaReworkObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetQualityIssuePcbaReworkTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportQualityIssuePcbaReworkAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportQualityIssuePcbaReworkAsync(TaktQualityIssuePcbaReworkQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
