// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：ITaktSalaryFormulaService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资计算公式应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 薪资计算公式应用服务接口
/// </summary>
public interface ITaktSalaryFormulaService
{
    /// <summary>
    /// 获取薪资计算公式列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalaryFormulaDto>> GetSalaryFormulaListAsync(TaktSalaryFormulaQueryDto queryDto);

    /// <summary>
    /// 根据ID获取薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryFormulaDto?> GetSalaryFormulaByIdAsync(long id);

    /// <summary>
    /// 获取薪资计算公式选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalaryFormulaOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建薪资计算公式
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryFormulaDto> CreateSalaryFormulaAsync(TaktSalaryFormulaCreateDto dto);

    /// <summary>
    /// 更新薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryFormulaDto> UpdateSalaryFormulaAsync(long id, TaktSalaryFormulaUpdateDto dto);

    /// <summary>
    /// 删除薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>任务</returns>
    Task DeleteSalaryFormulaByIdAsync(long id);

    /// <summary>
    /// 批量删除薪资计算公式
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalaryFormulaBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新薪资计算公式状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryFormulaDto> UpdateSalaryFormulaStatusAsync(TaktSalaryFormulaStatusDto dto);

    /// <summary>
    /// 更新薪资计算公式排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryFormulaDto> UpdateSalaryFormulaSortAsync(TaktSalaryFormulaSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalaryFormulaTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入薪资计算公式
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalaryFormulaAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出薪资计算公式
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalaryFormulaAsync(TaktSalaryFormulaQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
