// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良日报控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良日报控制器
/// 提供组立不良日报的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立不良日报")]
public class TaktAssyDefectsController : TaktControllerBase
{
    private readonly ITaktAssyDefectService _assyDefectService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectService">组立不良日报服务</param>
    public TaktAssyDefectsController(ITaktAssyDefectService assyDefectService)
    {
        _assyDefectService = assyDefectService;
    }

    /// <summary>
    /// 获取组立不良日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:list", "组立不良日报列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssyDefectListAsync([FromQuery] TaktAssyDefectQueryDto queryDto)
    {
        try
        {
            var result = await _assyDefectService.GetAssyDefectListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立不良统计（数据看板 defect-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>组立不良统计</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:list", "组立不良统计")]
    [HttpGet("defect-stat")]
    public async Task<IActionResult> GetAssyDefectStatAsync([FromQuery] TaktDefectStatQueryDto queryDto)
    {
        try
        {
            var result = await _assyDefectService.GetAssyDefectStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>组立不良日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:query", "组立不良日报详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssyDefectByIdAsync(long id)
    {
        try
        {
            var result = await _assyDefectService.GetAssyDefectByIdAsync(id);
            if (result == null)
            {
                return NotFound("组立不良日报不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立不良日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:query", "组立不良日报选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssyDefectOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assyDefectService.GetAssyDefectOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建组立不良日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>组立不良日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:create", "创建组立不良日报")]
    [HttpPost]
    public async Task<IActionResult> CreateAssyDefectAsync([FromBody] TaktAssyDefectCreateDto dto)
    {
        try
        {
            var result = await _assyDefectService.CreateAssyDefectAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>组立不良日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:update", "更新组立不良日报")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssyDefectAsync(long id, [FromBody] TaktAssyDefectUpdateDto dto)
    {
        try
        {
            var result = await _assyDefectService.UpdateAssyDefectAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:delete", "删除组立不良日报")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssyDefectByIdAsync(long id)
    {
        try
        {
            await _assyDefectService.DeleteAssyDefectByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除组立不良日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:delete", "批量删除组立不良日报")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssyDefectBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assyDefectService.DeleteAssyDefectBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:manufacturing:defect:assy:import", "获取组立不良日报导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssyDefectTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assyDefectService.GetAssyDefectTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入组立不良日报
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:import", "导入组立不良日报")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssyDefectAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assyDefectService.ImportAssyDefectAsync(stream, sheetName);
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
    /// 导出组立不良日报
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:assy:export", "导出组立不良日报")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssyDefectAsync([FromQuery] TaktAssyDefectQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assyDefectService.ExportAssyDefectAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
