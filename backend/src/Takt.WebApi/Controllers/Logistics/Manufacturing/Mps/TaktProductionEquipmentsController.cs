// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipmentsController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：生产设备控制器
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
/// 生产设备控制器
/// 提供生产设备的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产设备")]
public class TaktProductionEquipmentsController : TaktControllerBase
{
    private readonly ITaktProductionEquipmentService _productionEquipmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionEquipmentService">生产设备服务</param>
    public TaktProductionEquipmentsController(ITaktProductionEquipmentService productionEquipmentService)
    {
        _productionEquipmentService = productionEquipmentService;
    }

    /// <summary>
    /// 获取生产设备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:list", "生产设备列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionEquipmentListAsync([FromQuery] TaktProductionEquipmentQueryDto queryDto)
    {
        try
        {
            var result = await _productionEquipmentService.GetProductionEquipmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <returns>生产设备DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:query", "生产设备详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionEquipmentByIdAsync(long id)
    {
        try
        {
            var result = await _productionEquipmentService.GetProductionEquipmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产设备不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产设备选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:query", "生产设备选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionEquipmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionEquipmentService.GetProductionEquipmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产设备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产设备DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:create", "创建生产设备")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionEquipmentAsync([FromBody] TaktProductionEquipmentCreateDto dto)
    {
        try
        {
            var result = await _productionEquipmentService.CreateProductionEquipmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产设备DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:update", "更新生产设备")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionEquipmentAsync(long id, [FromBody] TaktProductionEquipmentUpdateDto dto)
    {
        try
        {
            var result = await _productionEquipmentService.UpdateProductionEquipmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产设备
    /// </summary>
    /// <param name="id">生产设备ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:delete", "删除生产设备")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionEquipmentByIdAsync(long id)
    {
        try
        {
            await _productionEquipmentService.DeleteProductionEquipmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产设备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:delete", "批量删除生产设备")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionEquipmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionEquipmentService.DeleteProductionEquipmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产设备状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>生产设备DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:update", "更新生产设备状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProductionEquipmentStatusAsync([FromBody] TaktProductionEquipmentStatusDto dto)
    {
        try
        {
            var result = await _productionEquipmentService.UpdateProductionEquipmentStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产设备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>生产设备DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:update", "更新生产设备排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateProductionEquipmentSortAsync([FromBody] TaktProductionEquipmentSortDto dto)
    {
        try
        {
            var result = await _productionEquipmentService.UpdateProductionEquipmentSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:production:equipment:import", "获取生产设备导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionEquipmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionEquipmentService.GetProductionEquipmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产设备
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:import", "导入生产设备")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionEquipmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionEquipmentService.ImportProductionEquipmentAsync(stream, sheetName);
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
    /// 导出生产设备
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:production:equipment:export", "导出生产设备")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionEquipmentAsync([FromQuery] TaktProductionEquipmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionEquipmentService.ExportProductionEquipmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
