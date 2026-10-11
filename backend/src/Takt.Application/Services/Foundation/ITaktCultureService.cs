// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktCultureService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：区域应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 区域应用服务接口
/// </summary>
public interface ITaktCultureService
{
    /// <summary>
    /// 获取区域列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCultureDto>> GetCultureListAsync(TaktCultureQueryDto queryDto);

    /// <summary>
    /// 根据ID获取区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>DTO</returns>
    Task<TaktCultureDto?> GetCultureByIdAsync(long id);

    /// <summary>
    /// 获取语言切换选项列表（仅启用，TaktSelectOption）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项（DictValue=CultureCode，DictLabel=NativeName，ExtValue=Icon，ExtLabel=IsDefault）</returns>
    Task<List<TaktSelectOption>> GetCultureOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建区域
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCultureDto> CreateCultureAsync(TaktCultureCreateDto dto);

    /// <summary>
    /// 更新区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCultureDto> UpdateCultureAsync(long id, TaktCultureUpdateDto dto);

    /// <summary>
    /// 删除区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>任务</returns>
    Task DeleteCultureByIdAsync(long id);

    /// <summary>
    /// 批量删除区域
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCultureBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新区域排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCultureDto> UpdateCultureSortAsync(TaktCultureSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCultureTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入区域
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCultureAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出区域
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCultureAsync(TaktCultureQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
