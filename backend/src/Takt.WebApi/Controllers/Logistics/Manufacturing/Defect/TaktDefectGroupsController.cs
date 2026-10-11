// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectGroupsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：不良组主数据控制器
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
/// 不良组主数据控制器
/// 提供不良组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "不良组主数据")]
public class TaktDefectGroupsController : TaktControllerBase
{
    private readonly ITaktDefectGroupService _defectGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="defectGroupService">不良组主数据服务</param>
    public TaktDefectGroupsController(ITaktDefectGroupService defectGroupService)
    {
        _defectGroupService = defectGroupService;
    }

    /// <summary>
    /// 获取不良组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:group:list", "不良组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDefectGroupListAsync([FromQuery] TaktDefectGroupQueryDto queryDto)
    {
        try
        {
            var result = await _defectGroupService.GetDefectGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取不良组主数据
    /// </summary>
    /// <param name="id">不良组主数据ID</param>
    /// <returns>不良组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:group:query", "不良组主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDefectGroupByIdAsync(long id)
    {
        try
        {
            var result = await _defectGroupService.GetDefectGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("不良组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取不良组主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:group:query", "不良组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDefectGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectGroupService.GetDefectGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建不良组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>不良组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:group:create", "创建不良组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateDefectGroupAsync([FromBody] TaktDefectGroupCreateDto dto)
    {
        try
        {
            var result = await _defectGroupService.CreateDefectGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新不良组主数据
    /// </summary>
    /// <param name="id">不良组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>不良组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:group:update", "更新不良组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDefectGroupAsync(long id, [FromBody] TaktDefectGroupUpdateDto dto)
    {
        try
        {
            var result = await _defectGroupService.UpdateDefectGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除不良组主数据
    /// </summary>
    /// <param name="id">不良组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:group:delete", "删除不良组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDefectGroupByIdAsync(long id)
    {
        try
        {
            await _defectGroupService.DeleteDefectGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除不良组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:group:delete", "批量删除不良组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDefectGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _defectGroupService.DeleteDefectGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新不良组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>不良组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:group:update", "更新不良组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateDefectGroupStatusAsync([FromBody] TaktDefectGroupStatusDto dto)
    {
        try
        {
            var result = await _defectGroupService.UpdateDefectGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新不良组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>不良组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:group:update", "更新不良组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateDefectGroupSortAsync([FromBody] TaktDefectGroupSortDto dto)
    {
        try
        {
            var result = await _defectGroupService.UpdateDefectGroupSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:defect:group:import", "获取不良组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDefectGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _defectGroupService.GetDefectGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入不良组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:group:import", "导入不良组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDefectGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _defectGroupService.ImportDefectGroupAsync(stream, sheetName);
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
    /// 导出不良组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:group:export", "导出不良组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDefectGroupAsync([FromQuery] TaktDefectGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _defectGroupService.ExportDefectGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
