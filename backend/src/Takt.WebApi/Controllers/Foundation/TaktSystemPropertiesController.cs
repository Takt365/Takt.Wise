// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktSystemPropertiesController.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：系统属性控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 系统属性控制器
/// 提供系统属性的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "系统属性")]
public class TaktSystemPropertiesController : TaktControllerBase
{
    private readonly ITaktSystemPropertyService _systemPropertyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="systemPropertyService">系统属性服务</param>
    public TaktSystemPropertiesController(ITaktSystemPropertyService systemPropertyService)
    {
        _systemPropertyService = systemPropertyService;
    }

    /// <summary>
    /// 获取系统属性列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:system:property:list", "系统属性列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSystemPropertyListAsync([FromQuery] TaktSystemPropertyQueryDto queryDto)
    {
        try
        {
            var result = await _systemPropertyService.GetSystemPropertyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>系统属性DTO</returns>
    [TaktPermission("foundation:system:property:query", "系统属性详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSystemPropertyByIdAsync(long id)
    {
        try
        {
            var result = await _systemPropertyService.GetSystemPropertyByIdAsync(id);
            if (result == null)
            {
                return NotFound("系统属性不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取系统属性选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:system:property:query", "系统属性选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSystemPropertyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _systemPropertyService.GetSystemPropertyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建系统属性
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>系统属性DTO</returns>
    [TaktPermission("foundation:system:property:create", "创建系统属性")]
    [HttpPost]
    public async Task<IActionResult> CreateSystemPropertyAsync([FromBody] TaktSystemPropertyCreateDto dto)
    {
        try
        {
            var result = await _systemPropertyService.CreateSystemPropertyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>系统属性DTO</returns>
    [TaktPermission("foundation:system:property:update", "更新系统属性")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSystemPropertyAsync(long id, [FromBody] TaktSystemPropertyUpdateDto dto)
    {
        try
        {
            var result = await _systemPropertyService.UpdateSystemPropertyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除系统属性
    /// </summary>
    /// <param name="id">系统属性ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:system:property:delete", "删除系统属性")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSystemPropertyByIdAsync(long id)
    {
        try
        {
            await _systemPropertyService.DeleteSystemPropertyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除系统属性
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:system:property:delete", "批量删除系统属性")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSystemPropertyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _systemPropertyService.DeleteSystemPropertyBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新系统属性状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>系统属性DTO</returns>
    [TaktPermission("foundation:system:property:update", "更新系统属性状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSystemPropertyStatusAsync([FromBody] TaktSystemPropertyStatusDto dto)
    {
        try
        {
            var result = await _systemPropertyService.UpdateSystemPropertyStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新系统属性排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>系统属性DTO</returns>
    [TaktPermission("foundation:system:property:update", "更新系统属性排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSystemPropertySortAsync([FromBody] TaktSystemPropertySortDto dto)
    {
        try
        {
            var result = await _systemPropertyService.UpdateSystemPropertySortAsync(dto);
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
    [TaktPermission("foundation:system:property:import", "获取系统属性导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSystemPropertyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _systemPropertyService.GetSystemPropertyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入系统属性
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:system:property:import", "导入系统属性")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSystemPropertyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _systemPropertyService.ImportSystemPropertyAsync(stream, sheetName);
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
    /// 导出系统属性
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:system:property:export", "导出系统属性")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSystemPropertyAsync([FromQuery] TaktSystemPropertyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _systemPropertyService.ExportSystemPropertyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
