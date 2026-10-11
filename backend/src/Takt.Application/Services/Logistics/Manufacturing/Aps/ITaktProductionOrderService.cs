// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：ITaktProductionOrderService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单应用服务接口
/// </summary>
public interface ITaktProductionOrderService
{
    /// <summary>
    /// 获取生产工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktProductionOrderDto>> GetProductionOrderListAsync(TaktProductionOrderQueryDto queryDto);

    /// <summary>
    /// 根据ID获取生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>DTO</returns>
    Task<TaktProductionOrderDto?> GetProductionOrderByIdAsync(long id);

    /// <summary>
    /// 获取生产工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（工单号/物料号模糊匹配，可选）</param>
    /// <returns>下拉选项（不限制条数，返回全部匹配记录）</returns>
    Task<List<TaktSelectOption>> GetProductionOrderOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建生产工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProductionOrderDto> CreateProductionOrderAsync(TaktProductionOrderCreateDto dto);

    /// <summary>
    /// 更新生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProductionOrderDto> UpdateProductionOrderAsync(long id, TaktProductionOrderUpdateDto dto);

    /// <summary>
    /// 删除生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>任务</returns>
    Task DeleteProductionOrderByIdAsync(long id);

    /// <summary>
    /// 批量删除生产工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteProductionOrderBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新生产工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProductionOrderDto> UpdateProductionOrderStatusAsync(TaktProductionOrderStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetProductionOrderTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入生产工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportProductionOrderAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出生产工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportProductionOrderAsync(TaktProductionOrderQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
