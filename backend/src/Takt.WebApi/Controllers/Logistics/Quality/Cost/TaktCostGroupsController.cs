// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktCostGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：成本组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 成本组主数据控制器
/// 提供成本组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "成本组主数据")]
public class TaktCostGroupsController : TaktControllerBase
{
    private readonly ITaktCostGroupService _costGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costGroupService">成本组主数据服务</param>
    public TaktCostGroupsController(ITaktCostGroupService costGroupService)
    {
        _costGroupService = costGroupService;
    }

    /// <summary>
    /// 获取成本组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:group:list", "成本组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCostGroupListAsync([FromQuery] TaktCostGroupQueryDto queryDto)
    {
        try
        {
            var result = await _costGroupService.GetCostGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取成本组主数据
    /// </summary>
    /// <param name="id">成本组主数据ID</param>
    /// <returns>成本组主数据DTO</returns>
    [TaktPermission("logistics:quality:cost:group:query", "成本组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCostGroupByIdAsync(long id)
    {
        try
        {
            var result = await _costGroupService.GetCostGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("成本组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="costGroupCategory">成本组类别（可选；1=记录组 2=废弃组 3=对应组）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:group:query", "成本组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCostGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] int? costGroupCategory = null)
    {
        try
        {
            var result = await _costGroupService.GetCostGroupOptionsAsync(plantCode, keyword, costGroupCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建成本组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>成本组主数据DTO</returns>
    [TaktPermission("logistics:quality:cost:group:create", "创建成本组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateCostGroupAsync([FromBody] TaktCostGroupCreateDto dto)
    {
        try
        {
            var result = await _costGroupService.CreateCostGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本组主数据
    /// </summary>
    /// <param name="id">成本组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>成本组主数据DTO</returns>
    [TaktPermission("logistics:quality:cost:group:update", "更新成本组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostGroupAsync(long id, [FromBody] TaktCostGroupUpdateDto dto)
    {
        try
        {
            var result = await _costGroupService.UpdateCostGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除成本组主数据
    /// </summary>
    /// <param name="id">成本组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:group:delete", "删除成本组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostGroupByIdAsync(long id)
    {
        try
        {
            await _costGroupService.DeleteCostGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除成本组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:group:delete", "批量删除成本组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCostGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _costGroupService.DeleteCostGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>成本组主数据DTO</returns>
    [TaktPermission("logistics:quality:cost:group:update", "更新成本组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCostGroupStatusAsync([FromBody] TaktCostGroupStatusDto dto)
    {
        try
        {
            var result = await _costGroupService.UpdateCostGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>成本组主数据DTO</returns>
    [TaktPermission("logistics:quality:cost:group:update", "更新成本组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCostGroupSortAsync([FromBody] TaktCostGroupSortDto dto)
    {
        try
        {
            var result = await _costGroupService.UpdateCostGroupSortAsync(dto);
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
    [TaktPermission("logistics:quality:cost:group:import", "获取成本组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCostGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _costGroupService.GetCostGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入成本组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:group:import", "导入成本组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCostGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _costGroupService.ImportCostGroupAsync(stream, sheetName);
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
    /// 导出成本组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:group:export", "导出成本组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCostGroupAsync([FromQuery] TaktCostGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _costGroupService.ExportCostGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
