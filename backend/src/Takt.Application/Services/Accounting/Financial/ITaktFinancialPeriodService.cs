// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktFinancialPeriodService.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：财务期间应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 财务期间应用服务接口
/// </summary>
public interface ITaktFinancialPeriodService
{
    /// <summary>
    /// 获取财务期间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktFinancialPeriodDto>> GetFinancialPeriodListAsync(TaktFinancialPeriodQueryDto queryDto);

    /// <summary>
    /// 根据ID获取财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>DTO</returns>
    Task<TaktFinancialPeriodDto?> GetFinancialPeriodByIdAsync(long id);

    /// <summary>
    /// 获取财务期间选项列表（按 FinancialYearCode 去重；DictValue=FinancialYearCode）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetFinancialPeriodOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建财务期间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFinancialPeriodDto> CreateFinancialPeriodAsync(TaktFinancialPeriodCreateDto dto);

    /// <summary>
    /// 更新财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFinancialPeriodDto> UpdateFinancialPeriodAsync(long id, TaktFinancialPeriodUpdateDto dto);

    /// <summary>
    /// 删除财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>任务</returns>
    Task DeleteFinancialPeriodByIdAsync(long id);

    /// <summary>
    /// 批量删除财务期间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteFinancialPeriodBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetFinancialPeriodTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入财务期间
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportFinancialPeriodAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出财务期间
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportFinancialPeriodAsync(TaktFinancialPeriodQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
