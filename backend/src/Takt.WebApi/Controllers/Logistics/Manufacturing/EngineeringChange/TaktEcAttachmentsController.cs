// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件控制器
/// 提供设变附件的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变附件")]
public class TaktEcAttachmentsController : TaktControllerBase
{
    private readonly ITaktEcAttachmentService _ecAttachmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecAttachmentService">设变附件服务</param>
    public TaktEcAttachmentsController(ITaktEcAttachmentService ecAttachmentService)
    {
        _ecAttachmentService = ecAttachmentService;
    }

    /// <summary>
    /// 获取设变附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:list", "设变附件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcAttachmentListAsync([FromQuery] TaktEcAttachmentQueryDto queryDto)
    {
        try
        {
            var result = await _ecAttachmentService.GetEcAttachmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>设变附件DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:query", "设变附件详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcAttachmentByIdAsync(long id)
    {
        try
        {
            var result = await _ecAttachmentService.GetEcAttachmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变附件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 预览设变附件（打开物理文件流；权限与菜单按钮 common.page.button.preview 对齐）
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>文件流</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:preview", "预览设变附件")]
    [HttpGet("{id}/preview")]
    public async Task<IActionResult> PreviewEcAttachmentAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ecAttachmentService.PreviewEcAttachmentAsync(id, cancellationToken);
            var contentType = string.IsNullOrWhiteSpace(result.ContentType)
                ? "application/octet-stream"
                : result.ContentType;
            return File(result.Stream, contentType, result.FileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:query", "设变附件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcAttachmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecAttachmentService.GetEcAttachmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变附件DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:create", "创建设变附件")]
    [HttpPost]
    public async Task<IActionResult> CreateEcAttachmentAsync([FromBody] TaktEcAttachmentCreateDto dto)
    {
        try
        {
            var result = await _ecAttachmentService.CreateEcAttachmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变附件DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:update", "更新设变附件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcAttachmentAsync(long id, [FromBody] TaktEcAttachmentUpdateDto dto)
    {
        try
        {
            var result = await _ecAttachmentService.UpdateEcAttachmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:delete", "删除设变附件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcAttachmentByIdAsync(long id)
    {
        try
        {
            await _ecAttachmentService.DeleteEcAttachmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:delete", "批量删除设变附件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcAttachmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecAttachmentService.DeleteEcAttachmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变附件作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变附件DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:update", "更新设变附件作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcAttachmentObsoleteAsync([FromBody] TaktEcAttachmentObsoleteDto dto)
    {
        try
        {
            var result = await _ecAttachmentService.UpdateEcAttachmentObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:te:import", "获取设变附件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcAttachmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecAttachmentService.GetEcAttachmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变附件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:import", "导入设变附件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcAttachmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecAttachmentService.ImportEcAttachmentAsync(stream, sheetName);
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
    /// 导出设变附件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:te:export", "导出设变附件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcAttachmentAsync([FromQuery] TaktEcAttachmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecAttachmentService.ExportEcAttachmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
