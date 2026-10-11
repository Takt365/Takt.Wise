// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Database
// 文件名称：TaktTableArchivesController.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表归档控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Code.Database;

/// <summary>
/// 数据表归档控制器
/// 提供数据表归档的 REST API
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "数据表归档")]
public class TaktTableArchivesController : TaktControllerBase
{
    private readonly ITaktTableArchiveService _tableArchiveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tableArchiveService">数据表归档服务</param>
    public TaktTableArchivesController(ITaktTableArchiveService tableArchiveService)
    {
        _tableArchiveService = tableArchiveService;
    }

    /// <summary>
    /// 获取数据表归档列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("code:database:table:archive:list", "数据表归档列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTableArchiveListAsync([FromQuery] TaktTableArchiveQueryDto queryDto)
    {
        try
        {
            var result = await _tableArchiveService.GetTableArchiveListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>数据表归档DTO</returns>
    [TaktPermission("code:database:table:archive:query", "数据表归档详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTableArchiveByIdAsync(long id)
    {
        try
        {
            var result = await _tableArchiveService.GetTableArchiveByIdAsync(id);
            if (result == null)
            {
                return NotFound("数据表归档不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取数据表归档选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("code:database:table:archive:query", "数据表归档选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTableArchiveOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _tableArchiveService.GetTableArchiveOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建数据表归档
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>数据表归档DTO</returns>
    [TaktPermission("code:database:table:archive:create", "创建数据表归档")]
    [HttpPost]
    public async Task<IActionResult> CreateTableArchiveAsync([FromBody] TaktTableArchiveCreateDto dto)
    {
        try
        {
            var result = await _tableArchiveService.CreateTableArchiveAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>数据表归档DTO</returns>
    [TaktPermission("code:database:table:archive:update", "更新数据表归档")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTableArchiveAsync(long id, [FromBody] TaktTableArchiveUpdateDto dto)
    {
        try
        {
            var result = await _tableArchiveService.UpdateTableArchiveAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:database:table:archive:delete", "删除数据表归档")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTableArchiveByIdAsync(long id)
    {
        try
        {
            await _tableArchiveService.DeleteTableArchiveByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除数据表归档
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:database:table:archive:delete", "批量删除数据表归档")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTableArchiveBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _tableArchiveService.DeleteTableArchiveBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新数据表归档状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>数据表归档DTO</returns>
    [TaktPermission("code:database:table:archive:update", "更新数据表归档状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTableArchiveStatusAsync([FromBody] TaktTableArchiveStatusDto dto)
    {
        try
        {
            var result = await _tableArchiveService.UpdateTableArchiveStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新数据表归档排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>数据表归档DTO</returns>
    [TaktPermission("code:database:table:archive:update", "更新数据表归档排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateTableArchiveSortAsync([FromBody] TaktTableArchiveSortDto dto)
    {
        try
        {
            var result = await _tableArchiveService.UpdateTableArchiveSortAsync(dto);
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
    [TaktPermission("code:database:table:archive:import", "获取数据表归档导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTableArchiveTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _tableArchiveService.GetTableArchiveTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入数据表归档
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("code:database:table:archive:import", "导入数据表归档")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTableArchiveAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _tableArchiveService.ImportTableArchiveAsync(stream, sheetName);
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
    /// 导出数据表归档
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("code:database:table:archive:export", "导出数据表归档")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTableArchiveAsync([FromQuery] TaktTableArchiveQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _tableArchiveService.ExportTableArchiveAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 预览按年归档行数
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>预览结果</returns>
    [TaktPermission("code:database:table:archive:archive", "预览按年归档")]
    [HttpPost("archive/preview")]
    public async Task<IActionResult> PreviewTableArchiveAsync([FromBody] TaktTableArchiveExecuteDto dto)
    {
        try
        {
            var result = await _tableArchiveService.PreviewTableArchiveAsync(dto);
            return Success(result, "预览成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 执行按年归档（同步，供 Quartz Job 调用）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>执行结果</returns>
    [TaktPermission("code:database:table:archive:archive", "执行按年归档")]
    [HttpPost("archive/execute")]
    public async Task<IActionResult> ExecuteTableArchiveAsync([FromBody] TaktTableArchiveExecuteDto dto)
    {
        try
        {
            var result = await _tableArchiveService.ExecuteTableArchiveAsync(dto);
            return Success(result, "归档完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 立即归档（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>调度结果</returns>
    [TaktPermission("code:database:table:archive:archive", "立即按年归档")]
    [HttpPost("archive/run-now")]
    public async Task<IActionResult> RunTableArchiveNowAsync([FromBody] TaktTableArchiveScheduleDto dto)
    {
        try
        {
            var result = await _tableArchiveService.RunTableArchiveNowAsync(dto);
            return Success(result, "已创建立即归档任务");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 后台归档（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>调度结果</returns>
    [TaktPermission("code:database:table:archive:schedule", "后台按年归档")]
    [HttpPost("archive/schedule")]
    public async Task<IActionResult> ScheduleTableArchiveAsync([FromBody] TaktTableArchiveScheduleDto dto)
    {
        try
        {
            var result = await _tableArchiveService.ScheduleTableArchiveAsync(dto);
            return Success(result, "已创建后台归档任务");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 预建年分表
    /// </summary>
    /// <param name="dto">建表请求</param>
    /// <returns>建表结果</returns>
    [TaktPermission("code:database:table:archive:create", "预建年分表")]
    [HttpPost("archive/ensure-year-tables")]
    public async Task<IActionResult> EnsureYearTablesAsync([FromBody] TaktTableEnsureYearTablesDto dto)
    {
        try
        {
            var result = await _tableArchiveService.EnsureYearTablesAsync(dto);
            return Success(result, "年分表已就绪");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
