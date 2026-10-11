// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopWorkstationsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工位主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Application.Services.Logistics.Manufacturing.Sop;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工位主数据控制器
/// 提供SOP工位主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP工位主数据")]
public class TaktSopWorkstationsController : TaktControllerBase
{
    private readonly ITaktSopWorkstationService _sopWorkstationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopWorkstationService">SOP工位主数据服务</param>
    public TaktSopWorkstationsController(ITaktSopWorkstationService sopWorkstationService)
    {
        _sopWorkstationService = sopWorkstationService;
    }

    /// <summary>
    /// 获取SOP工位主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:list", "SOP工位主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopWorkstationListAsync([FromQuery] TaktSopWorkstationQueryDto queryDto)
    {
        try
        {
            var result = await _sopWorkstationService.GetSopWorkstationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <returns>SOP工位主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:query", "SOP工位主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopWorkstationByIdAsync(long id)
    {
        try
        {
            var result = await _sopWorkstationService.GetSopWorkstationByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP工位主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP工位主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:query", "SOP工位主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopWorkstationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopWorkstationService.GetSopWorkstationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP工位主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP工位主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:create", "创建SOP工位主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateSopWorkstationAsync([FromBody] TaktSopWorkstationCreateDto dto)
    {
        try
        {
            var result = await _sopWorkstationService.CreateSopWorkstationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP工位主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:update", "更新SOP工位主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopWorkstationAsync(long id, [FromBody] TaktSopWorkstationUpdateDto dto)
    {
        try
        {
            var result = await _sopWorkstationService.UpdateSopWorkstationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP工位主数据
    /// </summary>
    /// <param name="id">SOP工位主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:delete", "删除SOP工位主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopWorkstationByIdAsync(long id)
    {
        try
        {
            await _sopWorkstationService.DeleteSopWorkstationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP工位主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:delete", "批量删除SOP工位主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopWorkstationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopWorkstationService.DeleteSopWorkstationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工位主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>SOP工位主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:update", "更新SOP工位主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSopWorkstationStatusAsync([FromBody] TaktSopWorkstationStatusDto dto)
    {
        try
        {
            var result = await _sopWorkstationService.UpdateSopWorkstationStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工位主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>SOP工位主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:update", "更新SOP工位主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSopWorkstationSortAsync([FromBody] TaktSopWorkstationSortDto dto)
    {
        try
        {
            var result = await _sopWorkstationService.UpdateSopWorkstationSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:workstation:import", "获取SOP工位主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopWorkstationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopWorkstationService.GetSopWorkstationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP工位主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:import", "导入SOP工位主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopWorkstationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopWorkstationService.ImportSopWorkstationAsync(stream, sheetName);
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
    /// 导出SOP工位主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:workstation:export", "导出SOP工位主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopWorkstationAsync([FromQuery] TaktSopWorkstationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopWorkstationService.ExportSopWorkstationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
