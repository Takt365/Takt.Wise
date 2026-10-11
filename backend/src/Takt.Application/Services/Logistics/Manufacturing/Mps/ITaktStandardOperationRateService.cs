// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：ITaktStandardOperationRateService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 标准生产稼动率应用服务接口
/// </summary>
public interface ITaktStandardOperationRateService
{
    /// <summary>
    /// 获取标准生产稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktStandardOperationRateDto>> GetStandardOperationRateListAsync(TaktStandardOperationRateQueryDto queryDto);

    /// <summary>
    /// 根据ID获取标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationRateDto?> GetStandardOperationRateByIdAsync(long id);

    /// <summary>
    /// 获取标准生产稼动率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetStandardOperationRateOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 按生产日期解析有效标准生产稼动率（%）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="operationType">稼动率类型（默认 1=人员）</param>
    /// <returns>稼动率(%)</returns>
    Task<decimal> GetEffectiveStandardOperationRatePercentAsync(string plantCode, DateTime prodDate, int operationType = 1);

    /// <summary>
    /// 创建标准生产稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationRateDto> CreateStandardOperationRateAsync(TaktStandardOperationRateCreateDto dto);

    /// <summary>
    /// 更新标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationRateDto> UpdateStandardOperationRateAsync(long id, TaktStandardOperationRateUpdateDto dto);

    /// <summary>
    /// 删除标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>任务</returns>
    Task DeleteStandardOperationRateByIdAsync(long id);

    /// <summary>
    /// 批量删除标准生产稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteStandardOperationRateBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新标准生产稼动率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationRateDto> UpdateStandardOperationRateStatusAsync(TaktStandardOperationRateStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetStandardOperationRateTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入标准生产稼动率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportStandardOperationRateAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出标准生产稼动率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportStandardOperationRateAsync(TaktStandardOperationRateQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
