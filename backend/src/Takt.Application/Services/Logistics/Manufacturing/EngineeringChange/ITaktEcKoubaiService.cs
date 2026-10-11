// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcKoubaiService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变采购执行应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变采购执行应用服务接口
/// </summary>
public interface ITaktEcKoubaiService
{
    /// <summary>
    /// 获取设变采购执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcKoubaiDto>> GetEcKoubaiListAsync(TaktEcKoubaiQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变采购执行
    /// </summary>
    /// <param name="id">设变采购执行ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcKoubaiDto?> GetEcKoubaiByIdAsync(long id);

    /// <summary>
    /// 获取设变采购执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcKoubaiOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建设变采购执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcKoubaiDto> CreateEcKoubaiAsync(TaktEcKoubaiCreateDto dto);

    /// <summary>
    /// 更新设变采购执行
    /// </summary>
    /// <param name="id">设变采购执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcKoubaiDto> UpdateEcKoubaiAsync(long id, TaktEcKoubaiUpdateDto dto);

    /// <summary>
    /// 删除设变采购执行
    /// </summary>
    /// <param name="id">设变采购执行ID</param>
    /// <returns>任务</returns>
    Task DeleteEcKoubaiByIdAsync(long id);

    /// <summary>
    /// 批量删除设变采购执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcKoubaiBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变采购执行停产状态（同步明细并自动填充/清除执行内容）
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcKoubaiDto> UpdateEcKoubaiDiscontinuedStatusAsync(TaktEcKoubaiDiscontinuedStatusDto dto);

    /// <summary>
    /// 更新设变执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcKoubaiDto> UpdateEcKoubaiObsoleteAsync(TaktEcKoubaiObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcKoubaiTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变采购执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcKoubaiAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变采购执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcKoubaiAsync(TaktEcKoubaiQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
