// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcMeService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制技执行应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制技执行应用服务接口
/// </summary>
public interface ITaktEcMeService
{
    /// <summary>
    /// 获取设变制技执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcMeDto>> GetEcMeListAsync(TaktEcMeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcMeDto?> GetEcMeByIdAsync(long id);

    /// <summary>
    /// 获取设变制技执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcMeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建设变制技执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcMeDto> CreateEcMeAsync(TaktEcMeCreateDto dto);

    /// <summary>
    /// 更新设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcMeDto> UpdateEcMeAsync(long id, TaktEcMeUpdateDto dto);

    /// <summary>
    /// 删除设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>任务</returns>
    Task DeleteEcMeByIdAsync(long id);

    /// <summary>
    /// 批量删除设变制技执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcMeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变制技执行停产状态（同步明细并自动填充/清除执行内容）
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcMeDto> UpdateEcMeDiscontinuedStatusAsync(TaktEcMeDiscontinuedStatusDto dto);

    /// <summary>
    /// 更新设变执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcMeDto> UpdateEcMeObsoleteAsync(TaktEcMeObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcMeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变制技执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcMeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变制技执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcMeAsync(TaktEcMeQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
