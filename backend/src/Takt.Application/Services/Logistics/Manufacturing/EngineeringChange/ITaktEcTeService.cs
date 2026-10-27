// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcTeService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主应用服务接口
/// </summary>
public interface ITaktEcTeService
{
    /// <summary>
    /// 获取设变技术课主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcTeDto>> GetEcTeListAsync(TaktEcTeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcTeDto?> GetEcTeByIdAsync(long id);

    /// <summary>
    /// 获取设变技术课主表选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcTeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建设变技术课主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcTeDto> CreateEcTeAsync(TaktEcTeCreateDto dto);

    /// <summary>
    /// 入队前校验新增（唯一键等；不落库）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>任务</returns>
    Task EnsureEcTeCreateReadyAsync(TaktEcTeCreateDto dto);

    /// <summary>
    /// 更新设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcTeDto> UpdateEcTeAsync(long id, TaktEcTeUpdateDto dto);

    /// <summary>
    /// 入队前校验更新（存在性、唯一键等；不落库）
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>任务</returns>
    Task EnsureEcTeUpdateReadyAsync(long id, TaktEcTeUpdateDto dto);

    /// <summary>
    /// 统计设变明细未作废行数（表 takt_logistics_manufacturing_ec_detail）
    /// </summary>
    /// <param name="ecTeId">设变技术课主表 Id</param>
    /// <returns>未作废明细行数</returns>
    Task<int> CountLiveEcDetailsAsync(long ecTeId);

    /// <summary>
    /// 删除设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>任务</returns>
    Task DeleteEcTeByIdAsync(long id);

    /// <summary>
    /// 批量删除设变技术课主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcTeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变技术课主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcTeDto> UpdateEcTeStatusAsync(TaktEcTeStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcTeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变技术课主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcTeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变技术课主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcTeAsync(TaktEcTeQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取设变技术课主表统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变统计</returns>
    Task<TaktEcTeStatDto> GetEcTeStatAsync(TaktEcTeStatQueryDto queryDto);

    /// <summary>
    /// 获取尚未导入设变技术课主的来源设变列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcTeSourceEcInputItemDto>> GetUnimportedSourceEcTeListAsync(TaktEcTeSourceEcInputQueryDto queryDto);

    /// <summary>
    /// 获取当前公司对应的来源设变目标工厂代码（Database:CompanyCodes 与 PlantCodes 同序映射）
    /// </summary>
    /// <returns>公司代码与映射工厂代码</returns>
    Task<TaktEcTeSourcePlantCodeDto> GetEcTeSourcePlantCodeAsync();

    /// <summary>
    /// 从来源设变导入设变技术课主、明细、附件，并初始化各部门执行行与设变通知
    /// </summary>
    /// <param name="dto">导入 DTO</param>
    /// <returns>导入结果</returns>
    Task<TaktEcTeImportFromSourceResultDto> ImportEcTeFromSourceAsync(TaktEcTeImportFromSourceDto dto);

    /// <summary>
    /// 从来源设变构建创建草稿 DTO（不落库；EcLeader、EcImplScope 留空供前端填写）
    /// </summary>
    /// <param name="dto">草稿请求 DTO</param>
    /// <returns>与 Create 接口一致的创建 DTO</returns>
    Task<TaktEcTeCreateDto> GetEcTeDraftFromSourceEcAsync(TaktEcTeDraftFromSourceDto dto);

}
