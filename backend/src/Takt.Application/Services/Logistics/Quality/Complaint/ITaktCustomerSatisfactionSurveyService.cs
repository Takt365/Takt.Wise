// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：ITaktCustomerSatisfactionSurveyService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 客户满意度调查应用服务接口
/// </summary>
public interface ITaktCustomerSatisfactionSurveyService
{
    /// <summary>
    /// 获取客户满意度调查列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCustomerSatisfactionSurveyDto>> GetCustomerSatisfactionSurveyListAsync(TaktCustomerSatisfactionSurveyQueryDto queryDto);

    /// <summary>
    /// 根据ID获取客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerSatisfactionSurveyDto?> GetCustomerSatisfactionSurveyByIdAsync(long id);

    /// <summary>
    /// 获取客户满意度调查选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetCustomerSatisfactionSurveyOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建客户满意度调查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerSatisfactionSurveyDto> CreateCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyCreateDto dto);

    /// <summary>
    /// 更新客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyAsync(long id, TaktCustomerSatisfactionSurveyUpdateDto dto);

    /// <summary>
    /// 删除客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>任务</returns>
    Task DeleteCustomerSatisfactionSurveyByIdAsync(long id);

    /// <summary>
    /// 批量删除客户满意度调查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCustomerSatisfactionSurveyBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新客户满意度调查状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyStatusAsync(TaktCustomerSatisfactionSurveyStatusDto dto);

    /// <summary>
    /// 更新客户满意度调查排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveySortAsync(TaktCustomerSatisfactionSurveySortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetCustomerSatisfactionSurveyTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入客户满意度调查
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportCustomerSatisfactionSurveyAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出客户满意度调查
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
