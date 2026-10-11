// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：ITaktMaterialRequirementsPlanningService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划MRP头应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 物料需求计划MRP头应用服务接口
/// </summary>
public interface ITaktMaterialRequirementsPlanningService
{
    /// <summary>
    /// 获取物料需求计划MRP头列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaterialRequirementsPlanningDto>> GetMaterialRequirementsPlanningListAsync(TaktMaterialRequirementsPlanningQueryDto queryDto);

    /// <summary>
    /// 根据ID获取物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialRequirementsPlanningDto?> GetMaterialRequirementsPlanningByIdAsync(long id);

    /// <summary>
    /// 获取物料需求计划MRP头选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialRequirementsPlanningOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建物料需求计划MRP头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialRequirementsPlanningDto> CreateMaterialRequirementsPlanningAsync(TaktMaterialRequirementsPlanningCreateDto dto);

    /// <summary>
    /// 更新物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialRequirementsPlanningDto> UpdateMaterialRequirementsPlanningAsync(long id, TaktMaterialRequirementsPlanningUpdateDto dto);

    /// <summary>
    /// 删除物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <returns>任务</returns>
    Task DeleteMaterialRequirementsPlanningByIdAsync(long id);

    /// <summary>
    /// 批量删除物料需求计划MRP头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaterialRequirementsPlanningBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新物料需求计划MRP头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialRequirementsPlanningDto> UpdateMaterialRequirementsPlanningStatusAsync(TaktMaterialRequirementsPlanningStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaterialRequirementsPlanningTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入物料需求计划MRP头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaterialRequirementsPlanningAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出物料需求计划MRP头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaterialRequirementsPlanningAsync(TaktMaterialRequirementsPlanningQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
