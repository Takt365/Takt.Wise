// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCentersController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// 工作中心控制器
/// 提供工作中心的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工作中心")]
public class TaktWorkCentersController : TaktControllerBase
{
    private readonly ITaktWorkCenterService _workCenterService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workCenterService">工作中心服务</param>
    public TaktWorkCentersController(ITaktWorkCenterService workCenterService)
    {
        _workCenterService = workCenterService;
    }

    /// <summary>
    /// 获取工作中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:list", "工作中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetWorkCenterListAsync([FromQuery] TaktWorkCenterQueryDto queryDto)
    {
        try
        {
            var result = await _workCenterService.GetWorkCenterListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>工作中心DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:query", "工作中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkCenterByIdAsync(long id)
    {
        try
        {
            var result = await _workCenterService.GetWorkCenterByIdAsync(id);
            if (result == null)
            {
                return NotFound("工作中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工作中心选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:query", "工作中心选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetWorkCenterOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _workCenterService.GetWorkCenterOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工作中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工作中心DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:create", "创建工作中心")]
    [HttpPost]
    public async Task<IActionResult> CreateWorkCenterAsync([FromBody] TaktWorkCenterCreateDto dto)
    {
        try
        {
            var result = await _workCenterService.CreateWorkCenterAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工作中心DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:update", "更新工作中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkCenterAsync(long id, [FromBody] TaktWorkCenterUpdateDto dto)
    {
        try
        {
            var result = await _workCenterService.UpdateWorkCenterAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:delete", "删除工作中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkCenterByIdAsync(long id)
    {
        try
        {
            await _workCenterService.DeleteWorkCenterByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工作中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:delete", "批量删除工作中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteWorkCenterBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _workCenterService.DeleteWorkCenterBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工作中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工作中心DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:update", "更新工作中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateWorkCenterStatusAsync([FromBody] TaktWorkCenterStatusDto dto)
    {
        try
        {
            var result = await _workCenterService.UpdateWorkCenterStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:work:center:import", "获取工作中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetWorkCenterTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _workCenterService.GetWorkCenterTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工作中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:import", "导入工作中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportWorkCenterAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _workCenterService.ImportWorkCenterAsync(stream, sheetName);
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
    /// 导出工作中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:export", "导出工作中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportWorkCenterAsync([FromQuery] TaktWorkCenterQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _workCenterService.ExportWorkCenterAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
