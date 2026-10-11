// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktSelfServicesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：自助服务控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 自助服务控制器
/// 提供自助服务的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "自助服务")]
public class TaktSelfServicesController : TaktControllerBase
{
    private readonly ITaktSelfServiceService _selfServiceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="selfServiceService">自助服务服务</param>
    public TaktSelfServicesController(ITaktSelfServiceService selfServiceService)
    {
        _selfServiceService = selfServiceService;
    }

    /// <summary>
    /// 获取自助服务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:self:service:list", "自助服务列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSelfServiceListAsync([FromQuery] TaktSelfServiceQueryDto queryDto)
    {
        try
        {
            var result = await _selfServiceService.GetSelfServiceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>自助服务DTO</returns>
    [TaktPermission("routine:help:desk:self:service:query", "自助服务详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSelfServiceByIdAsync(long id)
    {
        try
        {
            var result = await _selfServiceService.GetSelfServiceByIdAsync(id);
            if (result == null)
            {
                return NotFound("自助服务不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取自助服务选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:self:service:query", "自助服务选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSelfServiceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _selfServiceService.GetSelfServiceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建自助服务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>自助服务DTO</returns>
    [TaktPermission("routine:help:desk:self:service:create", "创建自助服务")]
    [HttpPost]
    public async Task<IActionResult> CreateSelfServiceAsync([FromBody] TaktSelfServiceCreateDto dto)
    {
        try
        {
            var result = await _selfServiceService.CreateSelfServiceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>自助服务DTO</returns>
    [TaktPermission("routine:help:desk:self:service:update", "更新自助服务")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSelfServiceAsync(long id, [FromBody] TaktSelfServiceUpdateDto dto)
    {
        try
        {
            var result = await _selfServiceService.UpdateSelfServiceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:self:service:delete", "删除自助服务")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSelfServiceByIdAsync(long id)
    {
        try
        {
            await _selfServiceService.DeleteSelfServiceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除自助服务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:self:service:delete", "批量删除自助服务")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSelfServiceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _selfServiceService.DeleteSelfServiceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自助服务状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>自助服务DTO</returns>
    [TaktPermission("routine:help:desk:self:service:update", "更新自助服务状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSelfServiceStatusAsync([FromBody] TaktSelfServiceStatusDto dto)
    {
        try
        {
            var result = await _selfServiceService.UpdateSelfServiceStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自助服务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>自助服务DTO</returns>
    [TaktPermission("routine:help:desk:self:service:update", "更新自助服务排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSelfServiceSortAsync([FromBody] TaktSelfServiceSortDto dto)
    {
        try
        {
            var result = await _selfServiceService.UpdateSelfServiceSortAsync(dto);
            return Success(result, "更新成功");
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
    [TaktPermission("routine:help:desk:self:service:import", "获取自助服务导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSelfServiceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _selfServiceService.GetSelfServiceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入自助服务
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:self:service:import", "导入自助服务")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSelfServiceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _selfServiceService.ImportSelfServiceAsync(stream, sheetName);
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
    /// 导出自助服务
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:self:service:export", "导出自助服务")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSelfServiceAsync([FromQuery] TaktSelfServiceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _selfServiceService.ExportSelfServiceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
