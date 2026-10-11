// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesQuotationItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售报价明细应用服务接口
/// </summary>
public interface ITaktSalesQuotationItemService
{
    /// <summary>
    /// 获取销售报价明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesQuotationItemDto>> GetSalesQuotationItemListAsync(TaktSalesQuotationItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationItemDto?> GetSalesQuotationItemByIdAsync(long id);

    /// <summary>
    /// 获取销售报价明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesQuotationItemOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建销售报价明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationItemDto> CreateSalesQuotationItemAsync(TaktSalesQuotationItemCreateDto dto);

    /// <summary>
    /// 更新销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationItemDto> UpdateSalesQuotationItemAsync(long id, TaktSalesQuotationItemUpdateDto dto);

    /// <summary>
    /// 删除销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesQuotationItemByIdAsync(long id);

    /// <summary>
    /// 批量删除销售报价明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesQuotationItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新销售报价明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationItemDto> UpdateSalesQuotationItemObsoleteAsync(TaktSalesQuotationItemObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalesQuotationItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入销售报价明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalesQuotationItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出销售报价明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesQuotationItemAsync(TaktSalesQuotationItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
