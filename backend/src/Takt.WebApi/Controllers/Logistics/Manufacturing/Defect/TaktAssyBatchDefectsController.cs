// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefectsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：组立批量不良统计控制器
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
/// 组立批量不良统计控制器
/// 提供组立批量不良统计的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立批量不良统计")]
public class TaktAssyBatchDefectsController : TaktControllerBase
{
    private readonly ITaktAssyBatchDefectService _assyBatchDefectService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyBatchDefectService">组立批量不良统计服务</param>
    public TaktAssyBatchDefectsController(ITaktAssyBatchDefectService assyBatchDefectService)
    {
        _assyBatchDefectService = assyBatchDefectService;
    }

    /// <summary>
    /// 获取组立批量不良统计列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:list", "组立批量不良统计列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssyBatchDefectListAsync([FromQuery] TaktAssyBatchDefectQueryDto queryDto)
    {
        try
        {
            var result = await _assyBatchDefectService.GetAssyBatchDefectListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <returns>组立批量不良统计DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:query", "组立批量不良统计详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssyBatchDefectByIdAsync(long id)
    {
        try
        {
            var result = await _assyBatchDefectService.GetAssyBatchDefectByIdAsync(id);
            if (result == null)
            {
                return NotFound("组立批量不良统计不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立批量不良统计选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:query", "组立批量不良统计选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssyBatchDefectOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assyBatchDefectService.GetAssyBatchDefectOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建组立批量不良统计
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>组立批量不良统计DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:create", "创建组立批量不良统计")]
    [HttpPost]
    public async Task<IActionResult> CreateAssyBatchDefectAsync([FromBody] TaktAssyBatchDefectCreateDto dto)
    {
        try
        {
            var result = await _assyBatchDefectService.CreateAssyBatchDefectAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>组立批量不良统计DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:update", "更新组立批量不良统计")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssyBatchDefectAsync(long id, [FromBody] TaktAssyBatchDefectUpdateDto dto)
    {
        try
        {
            var result = await _assyBatchDefectService.UpdateAssyBatchDefectAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除组立批量不良统计
    /// </summary>
    /// <param name="id">组立批量不良统计ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:delete", "删除组立批量不良统计")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssyBatchDefectByIdAsync(long id)
    {
        try
        {
            await _assyBatchDefectService.DeleteAssyBatchDefectByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除组立批量不良统计
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:delete", "批量删除组立批量不良统计")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssyBatchDefectBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assyBatchDefectService.DeleteAssyBatchDefectBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立批量不良统计状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>组立批量不良统计DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:update", "更新组立批量不良统计状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAssyBatchDefectStatusAsync([FromBody] TaktAssyBatchDefectStatusDto dto)
    {
        try
        {
            var result = await _assyBatchDefectService.UpdateAssyBatchDefectStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:import", "获取组立批量不良统计导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssyBatchDefectTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assyBatchDefectService.GetAssyBatchDefectTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入组立批量不良统计
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:import", "导入组立批量不良统计")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssyBatchDefectAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assyBatchDefectService.ImportAssyBatchDefectAsync(stream, sheetName);
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
    /// 导出组立批量不良统计
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:batch:export", "导出组立批量不良统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssyBatchDefectAsync([FromQuery] TaktAssyBatchDefectQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assyBatchDefectService.ExportAssyBatchDefectAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
