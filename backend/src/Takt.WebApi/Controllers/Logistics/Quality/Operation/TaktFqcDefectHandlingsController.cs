// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktFqcDefectHandlingsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验不良处理记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services.Logistics.Quality.Operation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Operation;

/// <summary>
/// 出货检验不良处理记录控制器
/// 提供出货检验不良处理记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "出货检验不良处理记录")]
public class TaktFqcDefectHandlingsController : TaktControllerBase
{
    private readonly ITaktFqcDefectHandlingService _fqcDefectHandlingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcDefectHandlingService">出货检验不良处理记录服务</param>
    public TaktFqcDefectHandlingsController(ITaktFqcDefectHandlingService fqcDefectHandlingService)
    {
        _fqcDefectHandlingService = fqcDefectHandlingService;
    }

    /// <summary>
    /// 获取出货检验不良处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:list", "出货检验不良处理记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFqcDefectHandlingListAsync([FromQuery] TaktFqcDefectHandlingQueryDto queryDto)
    {
        try
        {
            var result = await _fqcDefectHandlingService.GetFqcDefectHandlingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <returns>出货检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验不良处理记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFqcDefectHandlingByIdAsync(long id)
    {
        try
        {
            var result = await _fqcDefectHandlingService.GetFqcDefectHandlingByIdAsync(id);
            if (result == null)
            {
                return NotFound("出货检验不良处理记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取出货检验不良处理记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验不良处理记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFqcDefectHandlingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _fqcDefectHandlingService.GetFqcDefectHandlingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建出货检验不良处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>出货检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:create", "创建出货检验不良处理记录")]
    [HttpPost]
    public async Task<IActionResult> CreateFqcDefectHandlingAsync([FromBody] TaktFqcDefectHandlingCreateDto dto)
    {
        try
        {
            var result = await _fqcDefectHandlingService.CreateFqcDefectHandlingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>出货检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验不良处理记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFqcDefectHandlingAsync(long id, [FromBody] TaktFqcDefectHandlingUpdateDto dto)
    {
        try
        {
            var result = await _fqcDefectHandlingService.UpdateFqcDefectHandlingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "删除出货检验不良处理记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFqcDefectHandlingByIdAsync(long id)
    {
        try
        {
            await _fqcDefectHandlingService.DeleteFqcDefectHandlingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除出货检验不良处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "批量删除出货检验不良处理记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFqcDefectHandlingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fqcDefectHandlingService.DeleteFqcDefectHandlingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验不良处理记录状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>出货检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验不良处理记录状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFqcDefectHandlingStatusAsync([FromBody] TaktFqcDefectHandlingStatusDto dto)
    {
        try
        {
            var result = await _fqcDefectHandlingService.UpdateFqcDefectHandlingStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验不良处理记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>出货检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验不良处理记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateFqcDefectHandlingObsoleteAsync([FromBody] TaktFqcDefectHandlingObsoleteDto dto)
    {
        try
        {
            var result = await _fqcDefectHandlingService.UpdateFqcDefectHandlingObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:operation:fqc:order:import", "获取出货检验不良处理记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFqcDefectHandlingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _fqcDefectHandlingService.GetFqcDefectHandlingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入出货检验不良处理记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:import", "导入出货检验不良处理记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFqcDefectHandlingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _fqcDefectHandlingService.ImportFqcDefectHandlingAsync(stream, sheetName);
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
    /// 导出出货检验不良处理记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:export", "导出出货检验不良处理记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFqcDefectHandlingAsync([FromQuery] TaktFqcDefectHandlingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fqcDefectHandlingService.ExportFqcDefectHandlingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
