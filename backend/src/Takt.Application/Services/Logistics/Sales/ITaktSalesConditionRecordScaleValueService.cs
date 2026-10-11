// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesConditionRecordScaleValueService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售阶梯价格应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售阶梯价格应用服务接口
/// </summary>
public interface ITaktSalesConditionRecordScaleValueService
{
    /// <summary>
    /// 获取销售阶梯价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesConditionRecordScaleValueDto>> GetSalesConditionRecordScaleValueListAsync(TaktSalesConditionRecordScaleValueQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesConditionRecordScaleValueDto?> GetSalesConditionRecordScaleValueByIdAsync(long id);

    /// <summary>
    /// 获取销售阶梯价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesConditionRecordScaleValueOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建销售阶梯价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesConditionRecordScaleValueDto> CreateSalesConditionRecordScaleValueAsync(TaktSalesConditionRecordScaleValueCreateDto dto);

    /// <summary>
    /// 更新销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesConditionRecordScaleValueDto> UpdateSalesConditionRecordScaleValueAsync(long id, TaktSalesConditionRecordScaleValueUpdateDto dto);

    /// <summary>
    /// 删除销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesConditionRecordScaleValueByIdAsync(long id);

    /// <summary>
    /// 批量删除销售阶梯价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesConditionRecordScaleValueBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新销售阶梯价格作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesConditionRecordScaleValueDto> UpdateSalesConditionRecordScaleValueObsoleteAsync(TaktSalesConditionRecordScaleValueObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalesConditionRecordScaleValueTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入销售阶梯价格
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalesConditionRecordScaleValueAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出销售阶梯价格
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesConditionRecordScaleValueAsync(TaktSalesConditionRecordScaleValueQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
