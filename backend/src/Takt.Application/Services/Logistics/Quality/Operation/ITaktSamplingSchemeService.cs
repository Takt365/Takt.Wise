// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：ITaktSamplingSchemeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：抽样方案应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 抽样方案应用服务接口
/// </summary>
public interface ITaktSamplingSchemeService
{
    /// <summary>
    /// 获取抽样方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSamplingSchemeDto>> GetSamplingSchemeListAsync(TaktSamplingSchemeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>DTO</returns>
    Task<TaktSamplingSchemeDto?> GetSamplingSchemeByIdAsync(long id);

    /// <summary>
    /// 获取抽样方案选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSamplingSchemeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建抽样方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSamplingSchemeDto> CreateSamplingSchemeAsync(TaktSamplingSchemeCreateDto dto);

    /// <summary>
    /// 更新抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSamplingSchemeDto> UpdateSamplingSchemeAsync(long id, TaktSamplingSchemeUpdateDto dto);

    /// <summary>
    /// 删除抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>任务</returns>
    Task DeleteSamplingSchemeByIdAsync(long id);

    /// <summary>
    /// 批量删除抽样方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSamplingSchemeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新抽样方案状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSamplingSchemeDto> UpdateSamplingSchemeStatusAsync(TaktSamplingSchemeStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSamplingSchemeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入抽样方案
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSamplingSchemeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出抽样方案
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSamplingSchemeAsync(TaktSamplingSchemeQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
