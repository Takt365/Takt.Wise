// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：ITaktBonusPlanService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：奖金方案应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 奖金方案应用服务接口
/// </summary>
public interface ITaktBonusPlanService
{
    /// <summary>
    /// 获取奖金方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBonusPlanDto>> GetBonusPlanListAsync(TaktBonusPlanQueryDto queryDto);

    /// <summary>
    /// 根据ID获取奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <returns>DTO</returns>
    Task<TaktBonusPlanDto?> GetBonusPlanByIdAsync(long id);

    /// <summary>
    /// 获取奖金方案选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBonusPlanOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建奖金方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBonusPlanDto> CreateBonusPlanAsync(TaktBonusPlanCreateDto dto);

    /// <summary>
    /// 更新奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBonusPlanDto> UpdateBonusPlanAsync(long id, TaktBonusPlanUpdateDto dto);

    /// <summary>
    /// 删除奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <returns>任务</returns>
    Task DeleteBonusPlanByIdAsync(long id);

    /// <summary>
    /// 批量删除奖金方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBonusPlanBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新奖金方案状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBonusPlanDto> UpdateBonusPlanStatusAsync(TaktBonusPlanStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBonusPlanTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入奖金方案
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBonusPlanAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出奖金方案
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBonusPlanAsync(TaktBonusPlanQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
