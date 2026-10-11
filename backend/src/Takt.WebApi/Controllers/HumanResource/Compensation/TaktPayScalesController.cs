// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktPayScalesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪级控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Application.Services.HumanResource.Compensation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Compensation;

/// <summary>
/// 薪级控制器
/// 提供薪级的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "薪级")]
public class TaktPayScalesController : TaktControllerBase
{
    private readonly ITaktPayScaleService _payScaleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payScaleService">薪级服务</param>
    public TaktPayScalesController(ITaktPayScaleService payScaleService)
    {
        _payScaleService = payScaleService;
    }

    /// <summary>
    /// 获取薪级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:pay:scale:list", "薪级列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPayScaleListAsync([FromQuery] TaktPayScaleQueryDto queryDto)
    {
        try
        {
            var result = await _payScaleService.GetPayScaleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <returns>薪级DTO</returns>
    [TaktPermission("human:resource:compensation:pay:scale:query", "薪级详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayScaleByIdAsync(long id)
    {
        try
        {
            var result = await _payScaleService.GetPayScaleByIdAsync(id);
            if (result == null)
            {
                return NotFound("薪级不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取薪级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:pay:scale:query", "薪级选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPayScaleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _payScaleService.GetPayScaleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建薪级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>薪级DTO</returns>
    [TaktPermission("human:resource:compensation:pay:scale:create", "创建薪级")]
    [HttpPost]
    public async Task<IActionResult> CreatePayScaleAsync([FromBody] TaktPayScaleCreateDto dto)
    {
        try
        {
            var result = await _payScaleService.CreatePayScaleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>薪级DTO</returns>
    [TaktPermission("human:resource:compensation:pay:scale:update", "更新薪级")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayScaleAsync(long id, [FromBody] TaktPayScaleUpdateDto dto)
    {
        try
        {
            var result = await _payScaleService.UpdatePayScaleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除薪级
    /// </summary>
    /// <param name="id">薪级ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:pay:scale:delete", "删除薪级")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayScaleByIdAsync(long id)
    {
        try
        {
            await _payScaleService.DeletePayScaleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除薪级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:pay:scale:delete", "批量删除薪级")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePayScaleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _payScaleService.DeletePayScaleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪级状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>薪级DTO</returns>
    [TaktPermission("human:resource:compensation:pay:scale:update", "更新薪级状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePayScaleStatusAsync([FromBody] TaktPayScaleStatusDto dto)
    {
        try
        {
            var result = await _payScaleService.UpdatePayScaleStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪级排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>薪级DTO</returns>
    [TaktPermission("human:resource:compensation:pay:scale:update", "更新薪级排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePayScaleSortAsync([FromBody] TaktPayScaleSortDto dto)
    {
        try
        {
            var result = await _payScaleService.UpdatePayScaleSortAsync(dto);
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
    [TaktPermission("human:resource:compensation:pay:scale:import", "获取薪级导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPayScaleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _payScaleService.GetPayScaleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入薪级
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:pay:scale:import", "导入薪级")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPayScaleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _payScaleService.ImportPayScaleAsync(stream, sheetName);
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
    /// 导出薪级
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:pay:scale:export", "导出薪级")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPayScaleAsync([FromQuery] TaktPayScaleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _payScaleService.ExportPayScaleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
