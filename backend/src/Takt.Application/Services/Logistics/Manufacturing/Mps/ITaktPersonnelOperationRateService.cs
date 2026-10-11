// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：ITaktPersonnelOperationRateService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 人员稼动率应用服务接口
/// </summary>
public interface ITaktPersonnelOperationRateService
{
    /// <summary>
    /// 获取人员稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPersonnelOperationRateDto>> GetPersonnelOperationRateListAsync(TaktPersonnelOperationRateQueryDto queryDto);

    /// <summary>
    /// 根据ID获取人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>DTO</returns>
    Task<TaktPersonnelOperationRateDto?> GetPersonnelOperationRateByIdAsync(long id);

    /// <summary>
    /// 获取人员稼动率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPersonnelOperationRateOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建人员稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPersonnelOperationRateDto> CreatePersonnelOperationRateAsync(TaktPersonnelOperationRateCreateDto dto);

    /// <summary>
    /// 更新人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPersonnelOperationRateDto> UpdatePersonnelOperationRateAsync(long id, TaktPersonnelOperationRateUpdateDto dto);

    /// <summary>
    /// 删除人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>任务</returns>
    Task DeletePersonnelOperationRateByIdAsync(long id);

    /// <summary>
    /// 批量删除人员稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePersonnelOperationRateBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新人员稼动率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPersonnelOperationRateDto> UpdatePersonnelOperationRateStatusAsync(TaktPersonnelOperationRateStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPersonnelOperationRateTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入人员稼动率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPersonnelOperationRateAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出人员稼动率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPersonnelOperationRateAsync(TaktPersonnelOperationRateQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
