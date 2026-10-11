// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报明细控制器
/// 提供组立日报明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立日报明细")]
public class TaktAssyOutputDetailsController : TaktControllerBase
{
    private readonly ITaktAssyOutputDetailService _assyOutputDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputDetailService">组立日报明细服务</param>
    public TaktAssyOutputDetailsController(ITaktAssyOutputDetailService assyOutputDetailService)
    {
        _assyOutputDetailService = assyOutputDetailService;
    }

    /// <summary>
    /// 获取组立日报明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:assy:list", "组立日报明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssyOutputDetailListAsync([FromQuery] TaktAssyOutputDetailQueryDto queryDto)
    {
        try
        {
            var result = await _assyOutputDetailService.GetAssyOutputDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>组立日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:assy:query", "组立日报明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssyOutputDetailByIdAsync(long id)
    {
        try
        {
            var result = await _assyOutputDetailService.GetAssyOutputDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("组立日报明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立日报明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:assy:query", "组立日报明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssyOutputDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assyOutputDetailService.GetAssyOutputDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建组立日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>组立日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:assy:create", "创建组立日报明细")]
    [HttpPost]
    public async Task<IActionResult> CreateAssyOutputDetailAsync([FromBody] TaktAssyOutputDetailCreateDto dto)
    {
        try
        {
            var result = await _assyOutputDetailService.CreateAssyOutputDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>组立日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:assy:update", "更新组立日报明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssyOutputDetailAsync(long id, [FromBody] TaktAssyOutputDetailUpdateDto dto)
    {
        try
        {
            var result = await _assyOutputDetailService.UpdateAssyOutputDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:assy:delete", "删除组立日报明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssyOutputDetailByIdAsync(long id)
    {
        try
        {
            await _assyOutputDetailService.DeleteAssyOutputDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除组立日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:assy:delete", "批量删除组立日报明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssyOutputDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assyOutputDetailService.DeleteAssyOutputDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立日报明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>组立日报明细DTO</returns>
    [TaktPermission("logistics:manufacturing:output:assy:update", "更新组立日报明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateAssyOutputDetailObsoleteAsync([FromBody] TaktAssyOutputDetailObsoleteDto dto)
    {
        try
        {
            var result = await _assyOutputDetailService.UpdateAssyOutputDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:output:assy:import", "获取组立日报明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssyOutputDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assyOutputDetailService.GetAssyOutputDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入组立日报明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:assy:import", "导入组立日报明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssyOutputDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assyOutputDetailService.ImportAssyOutputDetailAsync(stream, sheetName);
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
    /// 导出组立日报明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:assy:export", "导出组立日报明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssyOutputDetailAsync([FromQuery] TaktAssyOutputDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assyOutputDetailService.ExportAssyOutputDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
