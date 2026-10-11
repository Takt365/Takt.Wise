// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktPcbaOutputService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报应用服务接口
/// </summary>
public interface ITaktPcbaOutputService
{
    /// <summary>
    /// 获取PCBA日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPcbaOutputDto>> GetPcbaOutputListAsync(TaktPcbaOutputQueryDto queryDto);

    /// <summary>
    /// 根据ID获取PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaOutputDto?> GetPcbaOutputByIdAsync(long id);

    /// <summary>
    /// 获取PCBA日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPcbaOutputOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建PCBA日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaOutputDto> CreatePcbaOutputAsync(TaktPcbaOutputCreateDto dto);

    /// <summary>
    /// 更新PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPcbaOutputDto> UpdatePcbaOutputAsync(long id, TaktPcbaOutputUpdateDto dto);

    /// <summary>
    /// 删除PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>任务</returns>
    Task DeletePcbaOutputByIdAsync(long id);

    /// <summary>
    /// 批量删除PCBA日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePcbaOutputBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPcbaOutputTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入PCBA日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPcbaOutputAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出PCBA日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPcbaOutputAsync(TaktPcbaOutputQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取 PCBA 生产统计（数据看板 production-stat；按生产日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 生产统计</returns>
    Task<TaktPcbaOutputProductionStatDto> GetPcbaOutputProductionStatAsync(TaktOutputProductionStatQueryDto queryDto);
}
