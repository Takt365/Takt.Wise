// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良明细控制器
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
/// 组立不良明细控制器
/// 提供组立不良明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立不良明细")]
public class TaktAssyDefectDetailsController : TaktControllerBase
{
    private readonly ITaktAssyDefectDetailService _assyDefectDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectDetailService">组立不良明细服务</param>
    public TaktAssyDefectDetailsController(ITaktAssyDefectDetailService assyDefectDetailService)
    {
        _assyDefectDetailService = assyDefectDetailService;
    }

    /// <summary>
    /// 获取组立不良明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:list", "组立不良明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssyDefectDetailListAsync([FromQuery] TaktAssyDefectDetailQueryDto queryDto)
    {
        try
        {
            var result = await _assyDefectDetailService.GetAssyDefectDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>组立不良明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:query", "组立不良明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssyDefectDetailByIdAsync(long id)
    {
        try
        {
            var result = await _assyDefectDetailService.GetAssyDefectDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("组立不良明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立不良明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:query", "组立不良明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssyDefectDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assyDefectDetailService.GetAssyDefectDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建组立不良明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>组立不良明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:create", "创建组立不良明细")]
    [HttpPost]
    public async Task<IActionResult> CreateAssyDefectDetailAsync([FromBody] TaktAssyDefectDetailCreateDto dto)
    {
        try
        {
            var result = await _assyDefectDetailService.CreateAssyDefectDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>组立不良明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:update", "更新组立不良明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssyDefectDetailAsync(long id, [FromBody] TaktAssyDefectDetailUpdateDto dto)
    {
        try
        {
            var result = await _assyDefectDetailService.UpdateAssyDefectDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:delete", "删除组立不良明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssyDefectDetailByIdAsync(long id)
    {
        try
        {
            await _assyDefectDetailService.DeleteAssyDefectDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除组立不良明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:delete", "批量删除组立不良明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssyDefectDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assyDefectDetailService.DeleteAssyDefectDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立不良明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>组立不良明细DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:update", "更新组立不良明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateAssyDefectDetailObsoleteAsync([FromBody] TaktAssyDefectDetailObsoleteDto dto)
    {
        try
        {
            var result = await _assyDefectDetailService.UpdateAssyDefectDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:defect:mfg1:import", "获取组立不良明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssyDefectDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assyDefectDetailService.GetAssyDefectDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入组立不良明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:import", "导入组立不良明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssyDefectDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assyDefectDetailService.ImportAssyDefectDetailAsync(stream, sheetName);
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
    /// 导出组立不良明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg1:export", "导出组立不良明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssyDefectDetailAsync([FromQuery] TaktAssyDefectDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assyDefectDetailService.ExportAssyDefectDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
