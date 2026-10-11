// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktHolidaysController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：假日信息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Application.Services.HumanResource.Attendance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Attendance;

/// <summary>
/// 假日信息控制器
/// 提供假日信息的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "假日信息")]
public class TaktHolidaysController : TaktControllerBase
{
    private readonly ITaktHolidayService _holidayService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="holidayService">假日信息服务</param>
    public TaktHolidaysController(ITaktHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    /// <summary>
    /// 获取假日信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:holiday:list", "假日信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetHolidayListAsync([FromQuery] TaktHolidayQueryDto queryDto)
    {
        try
        {
            var result = await _holidayService.GetHolidayListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>假日信息DTO</returns>
    [TaktPermission("human:resource:attendance:holiday:query", "假日信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHolidayByIdAsync(long id)
    {
        try
        {
            var result = await _holidayService.GetHolidayByIdAsync(id);
            if (result == null)
            {
                return NotFound("假日信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取假日信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:holiday:query", "假日信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetHolidayOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _holidayService.GetHolidayOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建假日信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>假日信息DTO</returns>
    [TaktPermission("human:resource:attendance:holiday:create", "创建假日信息")]
    [HttpPost]
    public async Task<IActionResult> CreateHolidayAsync([FromBody] TaktHolidayCreateDto dto)
    {
        try
        {
            var result = await _holidayService.CreateHolidayAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>假日信息DTO</returns>
    [TaktPermission("human:resource:attendance:holiday:update", "更新假日信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHolidayAsync(long id, [FromBody] TaktHolidayUpdateDto dto)
    {
        try
        {
            var result = await _holidayService.UpdateHolidayAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:holiday:delete", "删除假日信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHolidayByIdAsync(long id)
    {
        try
        {
            await _holidayService.DeleteHolidayByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除假日信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:holiday:delete", "批量删除假日信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteHolidayBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _holidayService.DeleteHolidayBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:holiday:import", "获取假日信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetHolidayTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _holidayService.GetHolidayTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入假日信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:holiday:import", "导入假日信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportHolidayAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _holidayService.ImportHolidayAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出假日信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:holiday:export", "导出假日信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportHolidayAsync([FromQuery] TaktHolidayQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _holidayService.ExportHolidayAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    
    /// <summary>
    /// 登录前预览：指定租户与公司下的当日假日主题（未签发 OAuth 访问令牌，非匿名登录）
    /// </summary>
    /// <param name="tenantCode">租户编码（与 X-Tenant-Code、登录页输入一致）</param>
    /// <param name="companyCode">公司编码（由 session/login-preview-locale 解析）</param>
    /// <returns>假日主题 DTO（字段对齐 TaktHoliday 实体）</returns>
    [AllowAnonymous]
    [HttpGet("theme")]
    public async Task<IActionResult> GetHolidayThemeAsync([FromQuery] string tenantCode, [FromQuery] string companyCode)
    {
        try
        {
            var tenantError = ValidateLoginPreviewTenantHeader(tenantCode);
            if (tenantError != null)
            {
                return tenantError;
            }
            var result = await _holidayService.GetHolidayThemeAsync(tenantCode, companyCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
