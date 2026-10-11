// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：ITaktCustomerServiceOrderService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务订单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 服务订单应用服务接口
/// </summary>
public interface ITaktCustomerServiceOrderService
{
    /// <summary>
    /// 获取服务订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCustomerServiceOrderDto>> GetCustomerServiceOrderListAsync(TaktCustomerServiceOrderQueryDto queryDto);

    /// <summary>
    /// 根据ID获取服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceOrderDto?> GetCustomerServiceOrderByIdAsync(long id);

    /// <summary>
    /// 获取服务订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetCustomerServiceOrderOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建服务订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceOrderDto> CreateCustomerServiceOrderAsync(TaktCustomerServiceOrderCreateDto dto);

    /// <summary>
    /// 更新服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderAsync(long id, TaktCustomerServiceOrderUpdateDto dto);

    /// <summary>
    /// 删除服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>任务</returns>
    Task DeleteCustomerServiceOrderByIdAsync(long id);

    /// <summary>
    /// 批量删除服务订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCustomerServiceOrderBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新服务订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderStatusAsync(TaktCustomerServiceOrderStatusDto dto);

    /// <summary>
    /// 更新服务订单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderSortAsync(TaktCustomerServiceOrderSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCustomerServiceOrderTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入服务订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCustomerServiceOrderAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出服务订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCustomerServiceOrderAsync(TaktCustomerServiceOrderQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
