// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterResourcesController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心资源控制器
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
/// 工作中心资源控制器
/// 提供工作中心资源的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工作中心资源")]
public class TaktWorkCenterResourcesController : TaktControllerBase
{
    private readonly ITaktWorkCenterResourceService _workCenterResourceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workCenterResourceService">工作中心资源服务</param>
    public TaktWorkCenterResourcesController(ITaktWorkCenterResourceService workCenterResourceService)
    {
        _workCenterResourceService = workCenterResourceService;
    }

    /// <summary>
    /// 获取工作中心资源列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:list", "工作中心资源列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetWorkCenterResourceListAsync([FromQuery] TaktWorkCenterResourceQueryDto queryDto)
    {
        try
        {
            var result = await _workCenterResourceService.GetWorkCenterResourceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <returns>工作中心资源DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:query", "工作中心资源详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkCenterResourceByIdAsync(long id)
    {
        try
        {
            var result = await _workCenterResourceService.GetWorkCenterResourceByIdAsync(id);
            if (result == null)
            {
                return NotFound("工作中心资源不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工作中心资源选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:query", "工作中心资源选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetWorkCenterResourceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _workCenterResourceService.GetWorkCenterResourceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工作中心资源
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工作中心资源DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:create", "创建工作中心资源")]
    [HttpPost]
    public async Task<IActionResult> CreateWorkCenterResourceAsync([FromBody] TaktWorkCenterResourceCreateDto dto)
    {
        try
        {
            var result = await _workCenterResourceService.CreateWorkCenterResourceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工作中心资源DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:update", "更新工作中心资源")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkCenterResourceAsync(long id, [FromBody] TaktWorkCenterResourceUpdateDto dto)
    {
        try
        {
            var result = await _workCenterResourceService.UpdateWorkCenterResourceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:delete", "删除工作中心资源")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkCenterResourceByIdAsync(long id)
    {
        try
        {
            await _workCenterResourceService.DeleteWorkCenterResourceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工作中心资源
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:delete", "批量删除工作中心资源")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteWorkCenterResourceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _workCenterResourceService.DeleteWorkCenterResourceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工作中心资源状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工作中心资源DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:update", "更新工作中心资源状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateWorkCenterResourceStatusAsync([FromBody] TaktWorkCenterResourceStatusDto dto)
    {
        try
        {
            var result = await _workCenterResourceService.UpdateWorkCenterResourceStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:work:center:import", "获取工作中心资源导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetWorkCenterResourceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _workCenterResourceService.GetWorkCenterResourceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工作中心资源
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:import", "导入工作中心资源")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportWorkCenterResourceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _workCenterResourceService.ImportWorkCenterResourceAsync(stream, sheetName);
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
    /// 导出工作中心资源
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:work:center:export", "导出工作中心资源")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportWorkCenterResourceAsync([FromQuery] TaktWorkCenterResourceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _workCenterResourceService.ExportWorkCenterResourceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
