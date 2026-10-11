// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktStandardOperationRatesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率控制器
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
/// 标准生产稼动率控制器
/// 提供标准生产稼动率的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "标准生产稼动率")]
public class TaktStandardOperationRatesController : TaktControllerBase
{
    private readonly ITaktStandardOperationRateService _standardOperationRateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationRateService">标准生产稼动率服务</param>
    public TaktStandardOperationRatesController(ITaktStandardOperationRateService standardOperationRateService)
    {
        _standardOperationRateService = standardOperationRateService;
    }

    /// <summary>
    /// 获取标准生产稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:list", "标准生产稼动率列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStandardOperationRateListAsync([FromQuery] TaktStandardOperationRateQueryDto queryDto)
    {
        try
        {
            var result = await _standardOperationRateService.GetStandardOperationRateListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取标准生产稼动率选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:query", "标准生产稼动率选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStandardOperationRateOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _standardOperationRateService.GetStandardOperationRateOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按生产日期解析有效标准生产稼动率（%）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="operationType">稼动率类型（默认 1=人员）</param>
    /// <returns>稼动率(%)</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:query", "有效标准生产稼动率")]
    [HttpGet("effective-rate")]
    public async Task<IActionResult> GetEffectiveStandardOperationRatePercentAsync(
        [FromQuery] string plantCode,
        [FromQuery] DateTime prodDate,
        [FromQuery] int operationType = 1)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return BadRequest("工厂代码不能为空");
            }
            var result = await _standardOperationRateService.GetEffectiveStandardOperationRatePercentAsync(plantCode, prodDate, operationType);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>标准生产稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:query", "标准生产稼动率详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetStandardOperationRateByIdAsync(long id)
    {
        try
        {
            var result = await _standardOperationRateService.GetStandardOperationRateByIdAsync(id);
            if (result == null)
            {
                return NotFound("标准生产稼动率不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建标准生产稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>标准生产稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:create", "创建标准生产稼动率")]
    [HttpPost]
    public async Task<IActionResult> CreateStandardOperationRateAsync([FromBody] TaktStandardOperationRateCreateDto dto)
    {
        try
        {
            var result = await _standardOperationRateService.CreateStandardOperationRateAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>标准生产稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:update", "更新标准生产稼动率")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStandardOperationRateAsync(long id, [FromBody] TaktStandardOperationRateUpdateDto dto)
    {
        try
        {
            var result = await _standardOperationRateService.UpdateStandardOperationRateAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:delete", "删除标准生产稼动率")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandardOperationRateByIdAsync(long id)
    {
        try
        {
            await _standardOperationRateService.DeleteStandardOperationRateByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除标准生产稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:delete", "批量删除标准生产稼动率")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStandardOperationRateBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _standardOperationRateService.DeleteStandardOperationRateBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准生产稼动率状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>标准生产稼动率DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:update", "更新标准生产稼动率状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStandardOperationRateStatusAsync([FromBody] TaktStandardOperationRateStatusDto dto)
    {
        try
        {
            var result = await _standardOperationRateService.UpdateStandardOperationRateStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:import", "获取标准生产稼动率导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetStandardOperationRateTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _standardOperationRateService.GetStandardOperationRateTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入标准生产稼动率
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:import", "导入标准生产稼动率")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportStandardOperationRateAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _standardOperationRateService.ImportStandardOperationRateAsync(stream, sheetName);
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
    /// 导出标准生产稼动率
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:standard:operation:rate:export", "导出标准生产稼动率")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStandardOperationRateAsync([FromQuery] TaktStandardOperationRateQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _standardOperationRateService.ExportStandardOperationRateAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
