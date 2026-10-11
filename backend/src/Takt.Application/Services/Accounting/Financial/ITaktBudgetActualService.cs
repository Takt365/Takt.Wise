// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktBudgetActualService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：预算实绩应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 预算实绩应用服务接口
/// </summary>
public interface ITaktBudgetActualService
{
    /// <summary>
    /// 获取预算实绩列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBudgetActualDto>> GetBudgetActualListAsync(TaktBudgetActualQueryDto queryDto);

    /// <summary>
    /// 根据ID获取预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>DTO</returns>
    Task<TaktBudgetActualDto?> GetBudgetActualByIdAsync(long id);

    /// <summary>
    /// 获取预算实绩选项列表（DictValue=Id，DictLabel=预算项名称，ExtValue=本期预算金额）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBudgetActualOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建预算实绩
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBudgetActualDto> CreateBudgetActualAsync(TaktBudgetActualCreateDto dto);

    /// <summary>
    /// 更新预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBudgetActualDto> UpdateBudgetActualAsync(long id, TaktBudgetActualUpdateDto dto);

    /// <summary>
    /// 删除预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>任务</returns>
    Task DeleteBudgetActualByIdAsync(long id);

    /// <summary>
    /// 批量删除预算实绩
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBudgetActualBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新预算实绩状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBudgetActualDto> UpdateBudgetActualStatusAsync(TaktBudgetActualStatusDto dto);

    /// <summary>
    /// 更新预算实绩排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBudgetActualDto> UpdateBudgetActualSortAsync(TaktBudgetActualSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBudgetActualTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入预算实绩
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBudgetActualAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出预算实绩
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBudgetActualAsync(TaktBudgetActualQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
