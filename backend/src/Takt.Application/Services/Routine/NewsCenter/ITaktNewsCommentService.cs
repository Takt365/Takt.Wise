// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：ITaktNewsCommentService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心评论应用服务接口
/// </summary>
public interface ITaktNewsCommentService
{
    /// <summary>
    /// 获取新闻中心评论列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktNewsCommentDto>> GetNewsCommentListAsync(TaktNewsCommentQueryDto queryDto);

    /// <summary>
    /// 根据ID获取新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>DTO</returns>
    Task<TaktNewsCommentDto?> GetNewsCommentByIdAsync(long id);

    /// <summary>
    /// 获取新闻中心评论树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    Task<List<TaktTreeSelectOption>> GetNewsCommentTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取新闻中心评论树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    Task<List<TaktNewsCommentTreeDto>> GetNewsCommentTreeAsync(long parentId = 0, bool includeDisabled = false);

    /// <summary>
    /// 创建新闻中心评论
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsCommentDto> CreateNewsCommentAsync(TaktNewsCommentCreateDto dto);

    /// <summary>
    /// 更新新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsCommentDto> UpdateNewsCommentAsync(long id, TaktNewsCommentUpdateDto dto);

    /// <summary>
    /// 删除新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>任务</returns>
    Task DeleteNewsCommentByIdAsync(long id);

    /// <summary>
    /// 批量删除新闻中心评论
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteNewsCommentBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新新闻中心评论状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsCommentDto> UpdateNewsCommentStatusAsync(TaktNewsCommentStatusDto dto);

    /// <summary>
    /// 更新新闻中心评论作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsCommentDto> UpdateNewsCommentObsoleteAsync(TaktNewsCommentObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetNewsCommentTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入新闻中心评论
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportNewsCommentAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出新闻中心评论
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportNewsCommentAsync(TaktNewsCommentQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
