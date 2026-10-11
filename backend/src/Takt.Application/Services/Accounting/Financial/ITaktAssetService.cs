// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktAssetService.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：资产应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 资产应用服务接口
/// </summary>
public interface ITaktAssetService
{
    /// <summary>
    /// 获取资产列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAssetDto>> GetAssetListAsync(TaktAssetQueryDto queryDto);

    /// <summary>
    /// 根据ID获取资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>DTO</returns>
    Task<TaktAssetDto?> GetAssetByIdAsync(long id);

    /// <summary>
    /// 获取资产选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetAssetOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建资产
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssetDto> CreateAssetAsync(TaktAssetCreateDto dto);

    /// <summary>
    /// 更新资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssetDto> UpdateAssetAsync(long id, TaktAssetUpdateDto dto);

    /// <summary>
    /// 删除资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>任务</returns>
    Task DeleteAssetByIdAsync(long id);

    /// <summary>
    /// 批量删除资产
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAssetBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新资产状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssetDto> UpdateAssetStatusAsync(TaktAssetStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetAssetTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入资产
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportAssetAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出资产
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAssetAsync(TaktAssetQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
