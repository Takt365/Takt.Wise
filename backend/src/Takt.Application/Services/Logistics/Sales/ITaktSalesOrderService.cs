// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesOrderService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售订单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售订单应用服务接口
/// </summary>
public interface ITaktSalesOrderService
{
    /// <summary>
    /// 获取销售订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesOrderDto>> GetSalesOrderListAsync(TaktSalesOrderQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesOrderDto?> GetSalesOrderByIdAsync(long id);

    /// <summary>
    /// 获取销售订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesOrderOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建销售订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesOrderDto> CreateSalesOrderAsync(TaktSalesOrderCreateDto dto);

    /// <summary>
    /// 更新销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesOrderDto> UpdateSalesOrderAsync(long id, TaktSalesOrderUpdateDto dto);

    /// <summary>
    /// 删除销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesOrderByIdAsync(long id);

    /// <summary>
    /// 批量删除销售订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesOrderBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新销售订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesOrderDto> UpdateSalesOrderStatusAsync(TaktSalesOrderStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalesOrderTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入销售订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalesOrderAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出销售订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesOrderAsync(TaktSalesOrderQueryDto? query = null, string? sheetName = null, string? fileName = null);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取销售订单统计（数据看板；按订单日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>销售订单统计</returns>
    Task<TaktSalesOrderStatDto> GetSalesOrderStatAsync(TaktSalesStatQueryDto queryDto);

}
