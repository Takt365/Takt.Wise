// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktEquipmentOperationRatesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：机器稼动率控制器
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
/// 机器稼动率控制器
/// 提供机器稼动率的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "机器稼动率")]
public class TaktEquipmentOperationRatesController : TaktControllerBase
{
    private readonly ITaktEquipmentOperationRateService _equipmentOperationRateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentOperationRateService">机器稼动率服务</param>
    public TaktEquipmentOperationRatesController(ITaktEquipmentOperationRateService equipmentOperationRateService)
    {
        _equipmentOperationRateService = equipmentOperationRateService;
    }

    /// <summary>
    /// 获取机器稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:list", "机器稼动率列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEquipmentOperationRateListAsync([FromQuery] TaktEquipmentOperationRateQueryDto queryDto)
    {
        try
        {
            var result = await _equipmentOperationRateService.GetEquipmentOperationRateListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <returns>机器稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:query", "机器稼动率详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEquipmentOperationRateByIdAsync(long id)
    {
        try
        {
            var result = await _equipmentOperationRateService.GetEquipmentOperationRateByIdAsync(id);
            if (result == null)
            {
                return NotFound("机器稼动率不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取机器稼动率选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:query", "机器稼动率选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEquipmentOperationRateOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _equipmentOperationRateService.GetEquipmentOperationRateOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建机器稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>机器稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:create", "创建机器稼动率")]
    [HttpPost]
    public async Task<IActionResult> CreateEquipmentOperationRateAsync([FromBody] TaktEquipmentOperationRateCreateDto dto)
    {
        try
        {
            var result = await _equipmentOperationRateService.CreateEquipmentOperationRateAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>机器稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:update", "更新机器稼动率")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipmentOperationRateAsync(long id, [FromBody] TaktEquipmentOperationRateUpdateDto dto)
    {
        try
        {
            var result = await _equipmentOperationRateService.UpdateEquipmentOperationRateAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:delete", "删除机器稼动率")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipmentOperationRateByIdAsync(long id)
    {
        try
        {
            await _equipmentOperationRateService.DeleteEquipmentOperationRateByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除机器稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:delete", "批量删除机器稼动率")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEquipmentOperationRateBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _equipmentOperationRateService.DeleteEquipmentOperationRateBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新机器稼动率状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>机器稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:update", "更新机器稼动率状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEquipmentOperationRateStatusAsync([FromBody] TaktEquipmentOperationRateStatusDto dto)
    {
        try
        {
            var result = await _equipmentOperationRateService.UpdateEquipmentOperationRateStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:import", "获取机器稼动率导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEquipmentOperationRateTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _equipmentOperationRateService.GetEquipmentOperationRateTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入机器稼动率
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:import", "导入机器稼动率")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEquipmentOperationRateAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _equipmentOperationRateService.ImportEquipmentOperationRateAsync(stream, sheetName);
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
    /// 导出机器稼动率
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:equipment:operation:rate:export", "导出机器稼动率")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEquipmentOperationRateAsync([FromQuery] TaktEquipmentOperationRateQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _equipmentOperationRateService.ExportEquipmentOperationRateAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
