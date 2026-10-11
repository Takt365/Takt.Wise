// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：ITaktHolidayService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：假日信息应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 假日信息应用服务接口
/// </summary>
public interface ITaktHolidayService
{
    /// <summary>
    /// 获取假日信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktHolidayDto>> GetHolidayListAsync(TaktHolidayQueryDto queryDto);

    /// <summary>
    /// 根据ID获取假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>DTO</returns>
    Task<TaktHolidayDto?> GetHolidayByIdAsync(long id);

    /// <summary>
    /// 获取假日信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetHolidayOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建假日信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktHolidayDto> CreateHolidayAsync(TaktHolidayCreateDto dto);

    /// <summary>
    /// 更新假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktHolidayDto> UpdateHolidayAsync(long id, TaktHolidayUpdateDto dto);

    /// <summary>
    /// 删除假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>任务</returns>
    Task DeleteHolidayByIdAsync(long id);

    /// <summary>
    /// 批量删除假日信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteHolidayBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetHolidayTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入假日信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportHolidayAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出假日信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportHolidayAsync(TaktHolidayQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取服务器当日、指定租户与公司下的假日主题色与问候信息
    /// </summary>
    /// <param name="tenantCode">租户编码（登录页须显式传入，与 X-Tenant-Code 一致）</param>
    /// <param name="companyCode">公司编码（登录预览语言接口解析的默认公司）</param>
    /// <returns>假日主题 DTO；无匹配记录时 IsHolidayToday 为 false、HolidayTheme 为空</returns>
    Task<TaktHolidayThemeDto> GetHolidayThemeAsync(string tenantCode, string companyCode);
}
