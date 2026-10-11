// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktInventoryReserveService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：存货跌价准备应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 存货跌价准备应用服务接口
/// </summary>
public interface ITaktInventoryReserveService
{
    /// <summary>
    /// 获取存货跌价准备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktInventoryReserveDto>> GetInventoryReserveListAsync(TaktInventoryReserveQueryDto queryDto);

    /// <summary>
    /// 根据ID获取存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>DTO</returns>
    Task<TaktInventoryReserveDto?> GetInventoryReserveByIdAsync(long id);

    /// <summary>
    /// 获取存货跌价准备选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetInventoryReserveOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建存货跌价准备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktInventoryReserveDto> CreateInventoryReserveAsync(TaktInventoryReserveCreateDto dto);

    /// <summary>
    /// 更新存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktInventoryReserveDto> UpdateInventoryReserveAsync(long id, TaktInventoryReserveUpdateDto dto);

    /// <summary>
    /// 删除存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>任务</returns>
    Task DeleteInventoryReserveByIdAsync(long id);

    /// <summary>
    /// 批量删除存货跌价准备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteInventoryReserveBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新存货跌价准备状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktInventoryReserveDto> UpdateInventoryReserveStatusAsync(TaktInventoryReserveStatusDto dto);

    /// <summary>
    /// 更新存货跌价准备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktInventoryReserveDto> UpdateInventoryReserveSortAsync(TaktInventoryReserveSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetInventoryReserveTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入存货跌价准备
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportInventoryReserveAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出存货跌价准备
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportInventoryReserveAsync(TaktInventoryReserveQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
