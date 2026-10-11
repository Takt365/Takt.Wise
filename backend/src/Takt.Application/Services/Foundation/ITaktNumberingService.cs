// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktNumberingService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 编码规则应用服务接口
/// </summary>
public interface ITaktNumberingService
{
    /// <summary>
    /// 获取编码规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktNumberingDto>> GetNumberingListAsync(TaktNumberingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>DTO</returns>
    Task<TaktNumberingDto?> GetNumberingByIdAsync(long id);

    /// <summary>
    /// 获取编码规则选项列表
    /// </summary>
    /// <param name="documentType">单据类型（TaktMenu.MenuName）；有值时仅返回该类型下启用规则</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetNumberingOptionsAsync(string? plantCode = null, string? keyword = null, string? documentType = null);

    /// <summary>
    /// 预览下一个业务编码（不占用流水号、不写库）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    Task<TaktNumberingPreviewDto> PreviewNumberingNextAsync(string ruleCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 创建编码规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNumberingDto> CreateNumberingAsync(TaktNumberingCreateDto dto);

    /// <summary>
    /// 更新编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNumberingDto> UpdateNumberingAsync(long id, TaktNumberingUpdateDto dto);

    /// <summary>
    /// 删除编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>任务</returns>
    Task DeleteNumberingByIdAsync(long id);

    /// <summary>
    /// 批量删除编码规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteNumberingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新编码规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNumberingDto> UpdateNumberingStatusAsync(TaktNumberingStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetNumberingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入编码规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportNumberingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出编码规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportNumberingAsync(TaktNumberingQueryDto? query = null, string? sheetName = null, string? fileName = null);
}