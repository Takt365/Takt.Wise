// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceOthersController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务其他通常业务费用明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 品质业务其他通常业务费用明细控制器
/// 提供品质业务其他通常业务费用明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "品质业务其他通常业务费用明细")]
public class TaktQualityAssuranceOthersController : TaktControllerBase
{
    private readonly ITaktQualityAssuranceOtherService _qualityAssuranceOtherService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityAssuranceOtherService">品质业务其他通常业务费用明细服务</param>
    public TaktQualityAssuranceOthersController(ITaktQualityAssuranceOtherService qualityAssuranceOtherService)
    {
        _qualityAssuranceOtherService = qualityAssuranceOtherService;
    }

    /// <summary>
    /// 获取品质业务其他通常业务费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:assurance:list", "品质业务其他通常业务费用明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityAssuranceOtherListAsync([FromQuery] TaktQualityAssuranceOtherQueryDto queryDto)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.GetQualityAssuranceOtherListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="id">品质业务其他通常业务费用明细ID</param>
    /// <returns>品质业务其他通常业务费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:assurance:query", "品质业务其他通常业务费用明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityAssuranceOtherByIdAsync(long id)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.GetQualityAssuranceOtherByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质业务其他通常业务费用明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质业务其他通常业务费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:assurance:query", "品质业务其他通常业务费用明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityAssuranceOtherOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.GetQualityAssuranceOtherOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质业务其他通常业务费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:assurance:create", "创建品质业务其他通常业务费用明细")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityAssuranceOtherAsync([FromBody] TaktQualityAssuranceOtherCreateDto dto)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.CreateQualityAssuranceOtherAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="id">品质业务其他通常业务费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质业务其他通常业务费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:assurance:update", "更新品质业务其他通常业务费用明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityAssuranceOtherAsync(long id, [FromBody] TaktQualityAssuranceOtherUpdateDto dto)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.UpdateQualityAssuranceOtherAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="id">品质业务其他通常业务费用明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:assurance:delete", "删除品质业务其他通常业务费用明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityAssuranceOtherByIdAsync(long id)
    {
        try
        {
            await _qualityAssuranceOtherService.DeleteQualityAssuranceOtherByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:assurance:delete", "批量删除品质业务其他通常业务费用明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityAssuranceOtherBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityAssuranceOtherService.DeleteQualityAssuranceOtherBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质业务其他通常业务费用明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>品质业务其他通常业务费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:assurance:update", "更新品质业务其他通常业务费用明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateQualityAssuranceOtherObsoleteAsync([FromBody] TaktQualityAssuranceOtherObsoleteDto dto)
    {
        try
        {
            var result = await _qualityAssuranceOtherService.UpdateQualityAssuranceOtherObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:cost:assurance:import", "获取品质业务其他通常业务费用明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityAssuranceOtherTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityAssuranceOtherService.GetQualityAssuranceOtherTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质业务其他通常业务费用明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:assurance:import", "导入品质业务其他通常业务费用明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityAssuranceOtherAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityAssuranceOtherService.ImportQualityAssuranceOtherAsync(stream, sheetName);
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
    /// 导出品质业务其他通常业务费用明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:assurance:export", "导出品质业务其他通常业务费用明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityAssuranceOtherAsync([FromQuery] TaktQualityAssuranceOtherQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityAssuranceOtherService.ExportQualityAssuranceOtherAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
