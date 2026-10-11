// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPlantsController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 工厂控制器
/// 提供工厂的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工厂")]
public class TaktPlantsController : TaktControllerBase
{
    private readonly ITaktPlantService _plantService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="plantService">工厂服务</param>
    public TaktPlantsController(ITaktPlantService plantService)
    {
        _plantService = plantService;
    }

    /// <summary>
    /// 获取工厂列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:plant:list", "工厂列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPlantListAsync([FromQuery] TaktPlantQueryDto queryDto)
    {
        try
        {
            var result = await _plantService.GetPlantListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>工厂DTO</returns>
    [TaktPermission("logistics:materials:plant:query", "工厂详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlantByIdAsync(long id)
    {
        try
        {
            var result = await _plantService.GetPlantByIdAsync(id);
            if (result == null)
            {
                return NotFound("工厂不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工厂选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:plant:query", "工厂选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _plantService.GetPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工厂
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工厂DTO</returns>
    [TaktPermission("logistics:materials:plant:create", "创建工厂")]
    [HttpPost]
    public async Task<IActionResult> CreatePlantAsync([FromBody] TaktPlantCreateDto dto)
    {
        try
        {
            var result = await _plantService.CreatePlantAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工厂DTO</returns>
    [TaktPermission("logistics:materials:plant:update", "更新工厂")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlantAsync(long id, [FromBody] TaktPlantUpdateDto dto)
    {
        try
        {
            var result = await _plantService.UpdatePlantAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:plant:delete", "删除工厂")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlantByIdAsync(long id)
    {
        try
        {
            await _plantService.DeletePlantByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工厂
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:plant:delete", "批量删除工厂")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePlantBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _plantService.DeletePlantBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工厂DTO</returns>
    [TaktPermission("logistics:materials:plant:update", "更新工厂状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePlantStatusAsync([FromBody] TaktPlantStatusDto dto)
    {
        try
        {
            var result = await _plantService.UpdatePlantStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>工厂DTO</returns>
    [TaktPermission("logistics:materials:plant:update", "更新工厂排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePlantSortAsync([FromBody] TaktPlantSortDto dto)
    {
        try
        {
            var result = await _plantService.UpdatePlantSortAsync(dto);
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
    [TaktPermission("logistics:materials:plant:import", "获取工厂导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPlantTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _plantService.GetPlantTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工厂
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:plant:import", "导入工厂")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPlantAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _plantService.ImportPlantAsync(stream, sheetName);
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
    /// 导出工厂
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:plant:export", "导出工厂")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPlantAsync([FromQuery] TaktPlantQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _plantService.ExportPlantAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
