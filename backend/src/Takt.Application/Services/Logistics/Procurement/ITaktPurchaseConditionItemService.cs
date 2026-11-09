// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchaseConditionItemService.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：采购条件行应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购条件行应用服务接口
/// </summary>
public interface ITaktPurchaseConditionItemService
{
    /// <summary>
    /// 获取采购条件行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPurchaseConditionItemDto>> GetPurchaseConditionItemListAsync(TaktPurchaseConditionItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseConditionItemDto?> GetPurchaseConditionItemByIdAsync(long id);

    /// <summary>
    /// 获取采购条件行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseConditionItemOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建采购条件行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseConditionItemDto> CreatePurchaseConditionItemAsync(TaktPurchaseConditionItemCreateDto dto);

    /// <summary>
    /// 更新采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseConditionItemDto> UpdatePurchaseConditionItemAsync(long id, TaktPurchaseConditionItemUpdateDto dto);

    /// <summary>
    /// 删除采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <returns>任务</returns>
    Task DeletePurchaseConditionItemByIdAsync(long id);

    /// <summary>
    /// 批量删除采购条件行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePurchaseConditionItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新采购条件行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchaseConditionItemDto> UpdatePurchaseConditionItemObsoleteAsync(TaktPurchaseConditionItemObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPurchaseConditionItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入采购条件行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPurchaseConditionItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出采购条件行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchaseConditionItemAsync(TaktPurchaseConditionItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
