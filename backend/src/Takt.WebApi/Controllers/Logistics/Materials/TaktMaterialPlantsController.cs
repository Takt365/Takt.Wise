// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialPlantsController.cs
// 创建时间：2026-08-05
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂物料控制器
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
/// 工厂物料控制器
/// 提供工厂物料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工厂物料")]
public class TaktMaterialPlantsController : TaktControllerBase
{
    private readonly ITaktMaterialPlantService _materialPlantService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialPlantService">工厂物料服务</param>
    public TaktMaterialPlantsController(ITaktMaterialPlantService materialPlantService)
    {
        _materialPlantService = materialPlantService;
    }

    /// <summary>
    /// 获取工厂物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:plant:list", "工厂物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialPlantListAsync([FromQuery] TaktMaterialPlantQueryDto queryDto)
    {
        try
        {
            var result = await _materialPlantService.GetMaterialPlantListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <returns>工厂物料DTO</returns>
    [TaktPermission("logistics:materials:material:plant:query", "工厂物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialPlantByIdAsync(long id)
    {
        try
        {
            var result = await _materialPlantService.GetMaterialPlantByIdAsync(id);
            if (result == null)
            {
                return NotFound("工厂物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:plant:query", "工厂物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialPlantService.GetMaterialPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工厂物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工厂物料DTO</returns>
    [TaktPermission("logistics:materials:material:plant:create", "创建工厂物料")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialPlantAsync([FromBody] TaktMaterialPlantCreateDto dto)
    {
        try
        {
            var result = await _materialPlantService.CreateMaterialPlantAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工厂物料DTO</returns>
    [TaktPermission("logistics:materials:material:plant:update", "更新工厂物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialPlantAsync(long id, [FromBody] TaktMaterialPlantUpdateDto dto)
    {
        try
        {
            var result = await _materialPlantService.UpdateMaterialPlantAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:plant:delete", "删除工厂物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialPlantByIdAsync(long id)
    {
        try
        {
            await _materialPlantService.DeleteMaterialPlantByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工厂物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:plant:delete", "批量删除工厂物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialPlantBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialPlantService.DeleteMaterialPlantBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂物料状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工厂物料DTO</returns>
    [TaktPermission("logistics:materials:material:plant:update", "更新工厂物料状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaterialPlantStatusAsync([FromBody] TaktMaterialPlantStatusDto dto)
    {
        try
        {
            var result = await _materialPlantService.UpdateMaterialPlantStatusAsync(dto);
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
    [TaktPermission("logistics:materials:material:plant:import", "获取工厂物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialPlantTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialPlantService.GetMaterialPlantTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工厂物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:plant:import", "导入工厂物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialPlantAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialPlantService.ImportMaterialPlantAsync(stream, sheetName);
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
    /// 导出工厂物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:plant:export", "导出工厂物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialPlantAsync([FromQuery] TaktMaterialPlantQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialPlantService.ExportMaterialPlantAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
