// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：ITaktDefectStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 不良统计：组立工单/批次全列，SMT 检查与 PCBA 修理明细
/// </summary>
public interface ITaktDefectStatService
{
    /// <summary>
    /// 当前页签工厂
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetDefectStatPlantOptionsAsync(string? statTab, string? keyword = null);

    /// <summary>
    /// 当前页签工单
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetDefectStatOrderOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签批次
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetDefectStatBatchOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签机种
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetDefectStatModelOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签物料
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetDefectStatMaterialOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>结果</returns>
    Task<TaktDefectOrderStatResultDto> GetDefectStatListAsync(TaktDefectOrderStatQueryDto queryDto);

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="fileName">文件名</param>
    /// <returns>文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportDefectStatAsync(TaktDefectOrderStatQueryDto query, string? sheetName = null, string? fileName = null);
}
