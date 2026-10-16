// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktComplaintGroupsController.cs
// 创建时间：2026-10-12
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Application.Services.Logistics.Quality.Complaint;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Complaint;

/// <summary>
/// 客诉组主数据控制器
/// 提供客诉组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客诉组主数据")]
public class TaktComplaintGroupsController : TaktControllerBase
{
    private readonly ITaktComplaintGroupService _complaintGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="complaintGroupService">客诉组主数据服务</param>
    public TaktComplaintGroupsController(ITaktComplaintGroupService complaintGroupService)
    {
        _complaintGroupService = complaintGroupService;
    }

    /// <summary>
    /// 获取客诉组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:group:list", "客诉组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetComplaintGroupListAsync([FromQuery] TaktComplaintGroupQueryDto queryDto)
    {
        try
        {
            var result = await _complaintGroupService.GetComplaintGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <returns>客诉组主数据DTO</returns>
    [TaktPermission("logistics:quality:complaint:group:query", "客诉组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetComplaintGroupByIdAsync(long id)
    {
        try
        {
            var result = await _complaintGroupService.GetComplaintGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("客诉组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客诉组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="complaintGroupCategory">complaintGroupCategory</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:group:query", "客诉组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetComplaintGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] int? complaintGroupCategory = null)
    {
        try
        {
            var result = await _complaintGroupService.GetComplaintGroupOptionsAsync(plantCode, keyword, complaintGroupCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客诉组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客诉组主数据DTO</returns>
    [TaktPermission("logistics:quality:complaint:group:create", "创建客诉组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateComplaintGroupAsync([FromBody] TaktComplaintGroupCreateDto dto)
    {
        try
        {
            var result = await _complaintGroupService.CreateComplaintGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客诉组主数据DTO</returns>
    [TaktPermission("logistics:quality:complaint:group:update", "更新客诉组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComplaintGroupAsync(long id, [FromBody] TaktComplaintGroupUpdateDto dto)
    {
        try
        {
            var result = await _complaintGroupService.UpdateComplaintGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客诉组主数据
    /// </summary>
    /// <param name="id">客诉组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:group:delete", "删除客诉组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComplaintGroupByIdAsync(long id)
    {
        try
        {
            await _complaintGroupService.DeleteComplaintGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客诉组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:group:delete", "批量删除客诉组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteComplaintGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _complaintGroupService.DeleteComplaintGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客诉组主数据DTO</returns>
    [TaktPermission("logistics:quality:complaint:group:update", "更新客诉组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateComplaintGroupStatusAsync([FromBody] TaktComplaintGroupStatusDto dto)
    {
        try
        {
            var result = await _complaintGroupService.UpdateComplaintGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客诉组主数据DTO</returns>
    [TaktPermission("logistics:quality:complaint:group:update", "更新客诉组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateComplaintGroupSortAsync([FromBody] TaktComplaintGroupSortDto dto)
    {
        try
        {
            var result = await _complaintGroupService.UpdateComplaintGroupSortAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:group:import", "获取客诉组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetComplaintGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _complaintGroupService.GetComplaintGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客诉组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:group:import", "导入客诉组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportComplaintGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _complaintGroupService.ImportComplaintGroupAsync(stream, sheetName);
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
    /// 导出客诉组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:group:export", "导出客诉组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportComplaintGroupAsync([FromQuery] TaktComplaintGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _complaintGroupService.ExportComplaintGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
