// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Training
// 文件名称：TaktTrainingCoursesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：培训课程控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Training;
using Takt.Application.Services.HumanResource.Training;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Training;

/// <summary>
/// 培训课程控制器
/// 提供培训课程的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "培训课程")]
public class TaktTrainingCoursesController : TaktControllerBase
{
    private readonly ITaktTrainingCourseService _trainingCourseService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingCourseService">培训课程服务</param>
    public TaktTrainingCoursesController(ITaktTrainingCourseService trainingCourseService)
    {
        _trainingCourseService = trainingCourseService;
    }

    /// <summary>
    /// 获取培训课程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:training:course:list", "培训课程列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTrainingCourseListAsync([FromQuery] TaktTrainingCourseQueryDto queryDto)
    {
        try
        {
            var result = await _trainingCourseService.GetTrainingCourseListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <returns>培训课程DTO</returns>
    [TaktPermission("human:resource:training:course:query", "培训课程详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingCourseByIdAsync(long id)
    {
        try
        {
            var result = await _trainingCourseService.GetTrainingCourseByIdAsync(id);
            if (result == null)
            {
                return NotFound("培训课程不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取培训课程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:training:course:query", "培训课程选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTrainingCourseOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _trainingCourseService.GetTrainingCourseOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建培训课程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>培训课程DTO</returns>
    [TaktPermission("human:resource:training:course:create", "创建培训课程")]
    [HttpPost]
    public async Task<IActionResult> CreateTrainingCourseAsync([FromBody] TaktTrainingCourseCreateDto dto)
    {
        try
        {
            var result = await _trainingCourseService.CreateTrainingCourseAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>培训课程DTO</returns>
    [TaktPermission("human:resource:training:course:update", "更新培训课程")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingCourseAsync(long id, [FromBody] TaktTrainingCourseUpdateDto dto)
    {
        try
        {
            var result = await _trainingCourseService.UpdateTrainingCourseAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:training:course:delete", "删除培训课程")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingCourseByIdAsync(long id)
    {
        try
        {
            await _trainingCourseService.DeleteTrainingCourseByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除培训课程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:training:course:delete", "批量删除培训课程")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTrainingCourseBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _trainingCourseService.DeleteTrainingCourseBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训课程状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>培训课程DTO</returns>
    [TaktPermission("human:resource:training:course:update", "更新培训课程状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTrainingCourseStatusAsync([FromBody] TaktTrainingCourseStatusDto dto)
    {
        try
        {
            var result = await _trainingCourseService.UpdateTrainingCourseStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训课程排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>培训课程DTO</returns>
    [TaktPermission("human:resource:training:course:update", "更新培训课程排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateTrainingCourseSortAsync([FromBody] TaktTrainingCourseSortDto dto)
    {
        try
        {
            var result = await _trainingCourseService.UpdateTrainingCourseSortAsync(dto);
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
    [TaktPermission("human:resource:training:course:import", "获取培训课程导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTrainingCourseTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _trainingCourseService.GetTrainingCourseTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入培训课程
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:training:course:import", "导入培训课程")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTrainingCourseAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _trainingCourseService.ImportTrainingCourseAsync(stream, sheetName);
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
    /// 导出培训课程
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:training:course:export", "导出培训课程")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTrainingCourseAsync([FromQuery] TaktTrainingCourseQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _trainingCourseService.ExportTrainingCourseAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
