// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：ITaktCustomerComplaintItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 客诉明细应用服务接口
/// </summary>
public interface ITaktCustomerComplaintItemService
{
    /// <summary>
    /// 获取客诉明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCustomerComplaintItemDto>> GetCustomerComplaintItemListAsync(TaktCustomerComplaintItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerComplaintItemDto?> GetCustomerComplaintItemByIdAsync(long id);

    /// <summary>
    /// 获取客诉明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetCustomerComplaintItemOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建客诉明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerComplaintItemDto> CreateCustomerComplaintItemAsync(TaktCustomerComplaintItemCreateDto dto);

    /// <summary>
    /// 更新客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemAsync(long id, TaktCustomerComplaintItemUpdateDto dto);

    /// <summary>
    /// 删除客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>任务</returns>
    Task DeleteCustomerComplaintItemByIdAsync(long id);

    /// <summary>
    /// 批量删除客诉明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCustomerComplaintItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新客诉明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemStatusAsync(TaktCustomerComplaintItemStatusDto dto);

    /// <summary>
    /// 更新客诉明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemObsoleteAsync(TaktCustomerComplaintItemObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCustomerComplaintItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入客诉明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCustomerComplaintItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出客诉明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintItemAsync(TaktCustomerComplaintItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
