// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：ITaktIqcDefectHandlingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验不良处理记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 进货检验不良处理记录应用服务接口
/// </summary>
public interface ITaktIqcDefectHandlingService
{
    /// <summary>
    /// 获取进货检验不良处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktIqcDefectHandlingDto>> GetIqcDefectHandlingListAsync(TaktIqcDefectHandlingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取进货检验不良处理记录
    /// </summary>
    /// <param name="id">进货检验不良处理记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktIqcDefectHandlingDto?> GetIqcDefectHandlingByIdAsync(long id);

    /// <summary>
    /// 获取进货检验不良处理记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetIqcDefectHandlingOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建进货检验不良处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIqcDefectHandlingDto> CreateIqcDefectHandlingAsync(TaktIqcDefectHandlingCreateDto dto);

    /// <summary>
    /// 更新进货检验不良处理记录
    /// </summary>
    /// <param name="id">进货检验不良处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIqcDefectHandlingDto> UpdateIqcDefectHandlingAsync(long id, TaktIqcDefectHandlingUpdateDto dto);

    /// <summary>
    /// 删除进货检验不良处理记录
    /// </summary>
    /// <param name="id">进货检验不良处理记录ID</param>
    /// <returns>任务</returns>
    Task DeleteIqcDefectHandlingByIdAsync(long id);

    /// <summary>
    /// 批量删除进货检验不良处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteIqcDefectHandlingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新进货检验不良处理记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIqcDefectHandlingDto> UpdateIqcDefectHandlingStatusAsync(TaktIqcDefectHandlingStatusDto dto);

    /// <summary>
    /// 更新进货检验不良处理记录作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIqcDefectHandlingDto> UpdateIqcDefectHandlingObsoleteAsync(TaktIqcDefectHandlingObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetIqcDefectHandlingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入进货检验不良处理记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportIqcDefectHandlingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出进货检验不良处理记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportIqcDefectHandlingAsync(TaktIqcDefectHandlingQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
