// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchaseInvoiceService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购发票应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购发票应用服务接口
/// </summary>
public interface ITaktPurchaseInvoiceService
{
    /// <summary>
    /// 获取采购发票列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPurchaseInvoiceDto>> GetPurchaseInvoiceListAsync(TaktPurchaseInvoiceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseInvoiceDto?> GetPurchaseInvoiceByIdAsync(long id);

    /// <summary>
    /// 获取采购发票选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseInvoiceOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建采购发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseInvoiceDto> CreatePurchaseInvoiceAsync(TaktPurchaseInvoiceCreateDto dto);

    /// <summary>
    /// 更新采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseInvoiceDto> UpdatePurchaseInvoiceAsync(long id, TaktPurchaseInvoiceUpdateDto dto);

    /// <summary>
    /// 删除采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>任务</returns>
    Task DeletePurchaseInvoiceByIdAsync(long id);

    /// <summary>
    /// 批量删除采购发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePurchaseInvoiceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPurchaseInvoiceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入采购发票
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPurchaseInvoiceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出采购发票
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchaseInvoiceAsync(TaktPurchaseInvoiceQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
