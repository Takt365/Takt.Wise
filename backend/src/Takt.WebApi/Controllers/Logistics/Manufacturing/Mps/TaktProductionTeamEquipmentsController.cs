// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipmentsController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组设备组控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Application.Services.Logistics.Manufacturing.Mps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组设备组控制器
/// 提供生产班组设备组的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产班组设备组")]
public class TaktProductionTeamEquipmentsController : TaktControllerBase
{
    private readonly ITaktProductionTeamEquipmentService _productionTeamEquipmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionTeamEquipmentService">生产班组设备组服务</param>
    public TaktProductionTeamEquipmentsController(ITaktProductionTeamEquipmentService productionTeamEquipmentService)
    {
        _productionTeamEquipmentService = productionTeamEquipmentService;
    }

    /// <summary>
    /// 获取生产班组设备组列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:list", "生产班组设备组列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionTeamEquipmentListAsync([FromQuery] TaktProductionTeamEquipmentQueryDto queryDto)
    {
        try
        {
            var result = await _productionTeamEquipmentService.GetProductionTeamEquipmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <returns>生产班组设备组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:query", "生产班组设备组详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionTeamEquipmentByIdAsync(long id)
    {
        try
        {
            var result = await _productionTeamEquipmentService.GetProductionTeamEquipmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产班组设备组不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产班组设备组选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:query", "生产班组设备组选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionTeamEquipmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionTeamEquipmentService.GetProductionTeamEquipmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产班组设备组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产班组设备组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:create", "创建生产班组设备组")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionTeamEquipmentAsync([FromBody] TaktProductionTeamEquipmentCreateDto dto)
    {
        try
        {
            var result = await _productionTeamEquipmentService.CreateProductionTeamEquipmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产班组设备组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:update", "更新生产班组设备组")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionTeamEquipmentAsync(long id, [FromBody] TaktProductionTeamEquipmentUpdateDto dto)
    {
        try
        {
            var result = await _productionTeamEquipmentService.UpdateProductionTeamEquipmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:delete", "删除生产班组设备组")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionTeamEquipmentByIdAsync(long id)
    {
        try
        {
            await _productionTeamEquipmentService.DeleteProductionTeamEquipmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产班组设备组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:delete", "批量删除生产班组设备组")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionTeamEquipmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionTeamEquipmentService.DeleteProductionTeamEquipmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产班组设备组状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>生产班组设备组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:update", "更新生产班组设备组状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProductionTeamEquipmentStatusAsync([FromBody] TaktProductionTeamEquipmentStatusDto dto)
    {
        try
        {
            var result = await _productionTeamEquipmentService.UpdateProductionTeamEquipmentStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产班组设备组作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>生产班组设备组DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:update", "更新生产班组设备组作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateProductionTeamEquipmentObsoleteAsync([FromBody] TaktProductionTeamEquipmentObsoleteDto dto)
    {
        try
        {
            var result = await _productionTeamEquipmentService.UpdateProductionTeamEquipmentObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:production:team:import", "获取生产班组设备组导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionTeamEquipmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionTeamEquipmentService.GetProductionTeamEquipmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产班组设备组
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:import", "导入生产班组设备组")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionTeamEquipmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionTeamEquipmentService.ImportProductionTeamEquipmentAsync(stream, sheetName);
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
    /// 导出生产班组设备组
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:production:team:export", "导出生产班组设备组")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionTeamEquipmentAsync([FromQuery] TaktProductionTeamEquipmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionTeamEquipmentService.ExportProductionTeamEquipmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
