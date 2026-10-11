// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcAttachmentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件应用服务接口
/// </summary>
public interface ITaktEcAttachmentService
{
    /// <summary>
    /// 获取设变附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcAttachmentDto>> GetEcAttachmentListAsync(TaktEcAttachmentQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcAttachmentDto?> GetEcAttachmentByIdAsync(long id);

    /// <summary>
    /// 预览设变附件（按 AccessUrl 打开 TaktFile 物理流）
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与文件名</returns>
    Task<TaktFileDownloadStreamResult> PreviewEcAttachmentAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取设变附件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcAttachmentOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建设变附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcAttachmentDto> CreateEcAttachmentAsync(TaktEcAttachmentCreateDto dto);

    /// <summary>
    /// 更新设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcAttachmentDto> UpdateEcAttachmentAsync(long id, TaktEcAttachmentUpdateDto dto);

    /// <summary>
    /// 删除设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>任务</returns>
    Task DeleteEcAttachmentByIdAsync(long id);

    /// <summary>
    /// 批量删除设变附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcAttachmentBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变附件作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcAttachmentDto> UpdateEcAttachmentObsoleteAsync(TaktEcAttachmentObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcAttachmentTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcAttachmentAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcAttachmentAsync(TaktEcAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
