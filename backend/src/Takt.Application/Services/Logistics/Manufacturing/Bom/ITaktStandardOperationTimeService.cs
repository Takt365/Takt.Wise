// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktStandardOperationTimeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工序时间应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 标准工序时间应用服务接口
/// </summary>
public interface ITaktStandardOperationTimeService
{
    /// <summary>
    /// 获取标准工序时间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktStandardOperationTimeDto>> GetStandardOperationTimeListAsync(TaktStandardOperationTimeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationTimeDto?> GetStandardOperationTimeByIdAsync(long id);

    /// <summary>
    /// 获取标准工序时间选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetStandardOperationTimeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 根据物料编码获取当前有效的标准工序时间列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="prodDate">生产日期（可选；未传时按当天解析有效期）</param>
    /// <returns>标准工序时间 DTO 列表</returns>
    Task<List<TaktStandardOperationTimeDto>> GetStandardOperationTimeByMaterialAsync(string materialCode, string? plantCode = null, DateTime? prodDate = null);

    /// <summary>
    /// 根据物料编码获取当前有效的标准工时选项列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="prodDate">生产日期（可选；未传时按当天解析有效期）</param>
    /// <returns>下拉选项（ExtValue=标准工时分钟数）</returns>
    Task<List<TaktSelectOption>> GetStandardOperationTimeOptionsByMaterialAsync(string materialCode, string? plantCode = null, DateTime? prodDate = null);

    /// <summary>
    /// 创建标准工序时间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationTimeDto> CreateStandardOperationTimeAsync(TaktStandardOperationTimeCreateDto dto);

    /// <summary>
    /// 更新标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktStandardOperationTimeDto> UpdateStandardOperationTimeAsync(long id, TaktStandardOperationTimeUpdateDto dto);

    /// <summary>
    /// 删除标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>任务</returns>
    Task DeleteStandardOperationTimeByIdAsync(long id);

    /// <summary>
    /// 批量删除标准工序时间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteStandardOperationTimeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetStandardOperationTimeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入标准工序时间
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportStandardOperationTimeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出标准工序时间
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportStandardOperationTimeAsync(TaktStandardOperationTimeQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
