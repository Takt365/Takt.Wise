// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktPersonnelOperationRatesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率控制器
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
/// 人员稼动率控制器
/// 提供人员稼动率的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "人员稼动率")]
public class TaktPersonnelOperationRatesController : TaktControllerBase
{
    private readonly ITaktPersonnelOperationRateService _personnelOperationRateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="personnelOperationRateService">人员稼动率服务</param>
    public TaktPersonnelOperationRatesController(ITaktPersonnelOperationRateService personnelOperationRateService)
    {
        _personnelOperationRateService = personnelOperationRateService;
    }

    /// <summary>
    /// 获取人员稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:list", "人员稼动率列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPersonnelOperationRateListAsync([FromQuery] TaktPersonnelOperationRateQueryDto queryDto)
    {
        try
        {
            var result = await _personnelOperationRateService.GetPersonnelOperationRateListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>人员稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:query", "人员稼动率详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonnelOperationRateByIdAsync(long id)
    {
        try
        {
            var result = await _personnelOperationRateService.GetPersonnelOperationRateByIdAsync(id);
            if (result == null)
            {
                return NotFound("人员稼动率不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取人员稼动率选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:query", "人员稼动率选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPersonnelOperationRateOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _personnelOperationRateService.GetPersonnelOperationRateOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建人员稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>人员稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:create", "创建人员稼动率")]
    [HttpPost]
    public async Task<IActionResult> CreatePersonnelOperationRateAsync([FromBody] TaktPersonnelOperationRateCreateDto dto)
    {
        try
        {
            var result = await _personnelOperationRateService.CreatePersonnelOperationRateAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>人员稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:update", "更新人员稼动率")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersonnelOperationRateAsync(long id, [FromBody] TaktPersonnelOperationRateUpdateDto dto)
    {
        try
        {
            var result = await _personnelOperationRateService.UpdatePersonnelOperationRateAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:delete", "删除人员稼动率")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonnelOperationRateByIdAsync(long id)
    {
        try
        {
            await _personnelOperationRateService.DeletePersonnelOperationRateByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除人员稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:delete", "批量删除人员稼动率")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePersonnelOperationRateBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _personnelOperationRateService.DeletePersonnelOperationRateBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新人员稼动率状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>人员稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:update", "更新人员稼动率状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePersonnelOperationRateStatusAsync([FromBody] TaktPersonnelOperationRateStatusDto dto)
    {
        try
        {
            var result = await _personnelOperationRateService.UpdatePersonnelOperationRateStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:import", "获取人员稼动率导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPersonnelOperationRateTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _personnelOperationRateService.GetPersonnelOperationRateTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入人员稼动率
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:import", "导入人员稼动率")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPersonnelOperationRateAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _personnelOperationRateService.ImportPersonnelOperationRateAsync(stream, sheetName);
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
    /// 导出人员稼动率
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:personnel:operation:rate:export", "导出人员稼动率")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPersonnelOperationRateAsync([FromQuery] TaktPersonnelOperationRateQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _personnelOperationRateService.ExportPersonnelOperationRateAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
