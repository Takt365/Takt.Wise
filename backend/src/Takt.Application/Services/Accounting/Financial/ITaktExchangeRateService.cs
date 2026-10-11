// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktExchangeRateService.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：汇率应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 汇率应用服务接口
/// </summary>
public interface ITaktExchangeRateService
{
    /// <summary>
    /// 获取汇率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktExchangeRateDto>> GetExchangeRateListAsync(TaktExchangeRateQueryDto queryDto);

    /// <summary>
    /// 根据ID获取汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>DTO</returns>
    Task<TaktExchangeRateDto?> GetExchangeRateByIdAsync(long id);

    /// <summary>
    /// 获取汇率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetExchangeRateOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建汇率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExchangeRateDto> CreateExchangeRateAsync(TaktExchangeRateCreateDto dto);

    /// <summary>
    /// 更新汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExchangeRateDto> UpdateExchangeRateAsync(long id, TaktExchangeRateUpdateDto dto);

    /// <summary>
    /// 删除汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>任务</returns>
    Task DeleteExchangeRateByIdAsync(long id);

    /// <summary>
    /// 批量删除汇率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteExchangeRateBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新汇率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExchangeRateDto> UpdateExchangeRateStatusAsync(TaktExchangeRateStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetExchangeRateTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入汇率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportExchangeRateAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出汇率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportExchangeRateAsync(TaktExchangeRateQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
