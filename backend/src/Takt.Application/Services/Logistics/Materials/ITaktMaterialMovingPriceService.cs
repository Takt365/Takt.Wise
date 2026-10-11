// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialMovingPriceService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 移动价格应用服务接口
/// </summary>
public interface ITaktMaterialMovingPriceService
{
    /// <summary>
    /// 获取移动价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaterialMovingPriceDto>> GetMaterialMovingPriceListAsync(TaktMaterialMovingPriceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialMovingPriceDto?> GetMaterialMovingPriceByIdAsync(long id);

    /// <summary>
    /// 获取物料移动价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialMovingPriceOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建移动价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialMovingPriceDto> CreateMaterialMovingPriceAsync(TaktMaterialMovingPriceCreateDto dto);

    /// <summary>
    /// 更新移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialMovingPriceDto> UpdateMaterialMovingPriceAsync(long id, TaktMaterialMovingPriceUpdateDto dto);

    /// <summary>
    /// 删除移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>任务</returns>
    Task DeleteMaterialMovingPriceByIdAsync(long id);

    /// <summary>
    /// 批量删除移动价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaterialMovingPriceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaterialMovingPriceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入移动价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaterialMovingPriceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出移动价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceAsync(TaktMaterialMovingPriceQueryDto? query = null, string? sheetName = null, string? fileName = null);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取在库金额统计（数据看板；按评估期间汇总 StockAmount）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>在库金额统计</returns>
    Task<TaktMaterialMovingPriceStatDto> GetMaterialMovingPriceStatAsync(TaktMaterialMovingPriceStatQueryDto queryDto);
}
