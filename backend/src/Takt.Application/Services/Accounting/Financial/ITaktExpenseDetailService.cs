// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktExpenseDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 费用单明细应用服务接口
/// </summary>
public interface ITaktExpenseDetailService
{
    /// <summary>
    /// 获取费用单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktExpenseDetailDto>> GetExpenseDetailListAsync(TaktExpenseDetailQueryDto queryDto);

    /// <summary>
    /// 根据ID获取费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktExpenseDetailDto?> GetExpenseDetailByIdAsync(long id);

    /// <summary>
    /// 获取费用单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetExpenseDetailOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建费用单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExpenseDetailDto> CreateExpenseDetailAsync(TaktExpenseDetailCreateDto dto);

    /// <summary>
    /// 更新费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExpenseDetailDto> UpdateExpenseDetailAsync(long id, TaktExpenseDetailUpdateDto dto);

    /// <summary>
    /// 删除费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>任务</returns>
    Task DeleteExpenseDetailByIdAsync(long id);

    /// <summary>
    /// 批量删除费用单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteExpenseDetailBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新费用单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktExpenseDetailDto> UpdateExpenseDetailObsoleteAsync(TaktExpenseDetailObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetExpenseDetailTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入费用单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportExpenseDetailAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出费用单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportExpenseDetailAsync(TaktExpenseDetailQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
