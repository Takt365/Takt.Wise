// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.VisitorCenter
// 文件名称：ITaktVisitorCompanionService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：来访人员应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.VisitorCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.VisitorCenter;

/// <summary>
/// 来访人员应用服务接口
/// </summary>
public interface ITaktVisitorCompanionService
{
    /// <summary>
    /// 获取来访人员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktVisitorCompanionDto>> GetVisitorCompanionListAsync(TaktVisitorCompanionQueryDto queryDto);

    /// <summary>
    /// 根据ID获取来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>DTO</returns>
    Task<TaktVisitorCompanionDto?> GetVisitorCompanionByIdAsync(long id);

    /// <summary>
    /// 获取来访人员选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetVisitorCompanionOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建来访人员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktVisitorCompanionDto> CreateVisitorCompanionAsync(TaktVisitorCompanionCreateDto dto);

    /// <summary>
    /// 更新来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktVisitorCompanionDto> UpdateVisitorCompanionAsync(long id, TaktVisitorCompanionUpdateDto dto);

    /// <summary>
    /// 删除来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>任务</returns>
    Task DeleteVisitorCompanionByIdAsync(long id);

    /// <summary>
    /// 批量删除来访人员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteVisitorCompanionBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetVisitorCompanionTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入来访人员
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportVisitorCompanionAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出来访人员
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportVisitorCompanionAsync(TaktVisitorCompanionQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
