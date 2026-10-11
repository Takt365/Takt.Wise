// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：ITaktDocumentVersionService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档版本应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.DocumentCenter;

/// <summary>
/// 文管文档版本应用服务接口
/// </summary>
public interface ITaktDocumentVersionService
{
    /// <summary>
    /// 获取文管文档版本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDocumentVersionDto>> GetDocumentVersionListAsync(TaktDocumentVersionQueryDto queryDto);

    /// <summary>
    /// 根据ID获取文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>DTO</returns>
    Task<TaktDocumentVersionDto?> GetDocumentVersionByIdAsync(long id);

    /// <summary>
    /// 获取文管文档版本选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDocumentVersionOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建文管文档版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDocumentVersionDto> CreateDocumentVersionAsync(TaktDocumentVersionCreateDto dto);

    /// <summary>
    /// 更新文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDocumentVersionDto> UpdateDocumentVersionAsync(long id, TaktDocumentVersionUpdateDto dto);

    /// <summary>
    /// 删除文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>任务</returns>
    Task DeleteDocumentVersionByIdAsync(long id);

    /// <summary>
    /// 批量删除文管文档版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteDocumentVersionBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新文管文档版本作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktDocumentVersionDto> UpdateDocumentVersionObsoleteAsync(TaktDocumentVersionObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetDocumentVersionTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入文管文档版本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportDocumentVersionAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出文管文档版本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportDocumentVersionAsync(TaktDocumentVersionQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
