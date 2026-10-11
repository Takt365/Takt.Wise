// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktTranslationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：翻译应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 翻译应用服务接口
/// </summary>
public interface ITaktTranslationService
{
    /// <summary>
    /// 获取翻译列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTranslationDto>> GetTranslationListAsync(TaktTranslationQueryDto queryDto);

    /// <summary>
    /// 根据ID获取翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>DTO</returns>
    Task<TaktTranslationDto?> GetTranslationByIdAsync(long id);

    /// <summary>
    /// 获取翻译选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTranslationOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建翻译
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTranslationDto> CreateTranslationAsync(TaktTranslationCreateDto dto);

    /// <summary>
    /// 更新翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTranslationDto> UpdateTranslationAsync(long id, TaktTranslationUpdateDto dto);

    /// <summary>
    /// 删除翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>任务</returns>
    Task DeleteTranslationByIdAsync(long id);

    /// <summary>
    /// 批量删除翻译
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTranslationBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTranslationTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入翻译
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTranslationAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出翻译
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTranslationAsync(TaktTranslationQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取翻译转置列表（分页，行=业务键，列=各语言）
    /// </summary>
    Task<TaktTranslationTransposedResultDto> GetTranslationTransposedListAsync(TaktTranslationTransposedQueryDto queryDto);

    /// <summary>
    /// 批量保存翻译转置数据
    /// </summary>
    Task<int> SaveTranslationTransposedBatchAsync(TaktTranslationTransposedBatchDto dto);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取指定文化下的前端动态翻译键值（扁平 I18nKey → 文本）
    /// </summary>
    /// <param name="cultureCode">区域文化编码（BCP47）</param>
    /// <returns>前端 messages 包</returns>
    Task<TaktTranslationMessagesDto> GetTranslationMessagesAsync(string cultureCode);

}
