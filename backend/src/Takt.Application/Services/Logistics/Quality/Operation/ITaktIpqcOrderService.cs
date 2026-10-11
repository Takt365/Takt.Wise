// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：ITaktIpqcOrderService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 制程检验单应用服务接口
/// </summary>
public interface ITaktIpqcOrderService
{
    /// <summary>
    /// 获取制程检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktIpqcOrderDto>> GetIpqcOrderListAsync(TaktIpqcOrderQueryDto queryDto);

    /// <summary>
    /// 根据ID获取制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderDto?> GetIpqcOrderByIdAsync(long id);

    /// <summary>
    /// 获取制程检验单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetIpqcOrderOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建制程检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderDto> CreateIpqcOrderAsync(TaktIpqcOrderCreateDto dto);

    /// <summary>
    /// 更新制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderDto> UpdateIpqcOrderAsync(long id, TaktIpqcOrderUpdateDto dto);

    /// <summary>
    /// 删除制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>任务</returns>
    Task DeleteIpqcOrderByIdAsync(long id);

    /// <summary>
    /// 批量删除制程检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteIpqcOrderBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新制程检验单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderDto> UpdateIpqcOrderStatusAsync(TaktIpqcOrderStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetIpqcOrderTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入制程检验单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportIpqcOrderAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出制程检验单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportIpqcOrderAsync(TaktIpqcOrderQueryDto? query = null, string? sheetName = null, string? fileName = null);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取 IPQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>IPQC 检验统计</returns>
    Task<TaktIpqcOrderStatDto> GetIpqcOrderStatAsync(TaktQualityStatQueryDto queryDto);

}
