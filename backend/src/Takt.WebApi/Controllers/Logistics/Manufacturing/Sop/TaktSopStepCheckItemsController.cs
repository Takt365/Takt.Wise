// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepCheckItemsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步检验项目控制器
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
/// SOP工步检验项目控制器
/// 提供SOP工步检验项目的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP工步检验项目")]
public class TaktSopStepCheckItemsController : TaktControllerBase
{
    private readonly ITaktSopStepCheckItemService _sopStepCheckItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepCheckItemService">SOP工步检验项目服务</param>
    public TaktSopStepCheckItemsController(ITaktSopStepCheckItemService sopStepCheckItemService)
    {
        _sopStepCheckItemService = sopStepCheckItemService;
    }

    /// <summary>
    /// 获取SOP工步检验项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:list", "SOP工步检验项目列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopStepCheckItemListAsync([FromQuery] TaktSopStepCheckItemQueryDto queryDto)
    {
        try
        {
            var result = await _sopStepCheckItemService.GetSopStepCheckItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <returns>SOP工步检验项目DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP工步检验项目详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopStepCheckItemByIdAsync(long id)
    {
        try
        {
            var result = await _sopStepCheckItemService.GetSopStepCheckItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP工步检验项目不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP工步检验项目选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP工步检验项目选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopStepCheckItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopStepCheckItemService.GetSopStepCheckItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP工步检验项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP工步检验项目DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:create", "创建SOP工步检验项目")]
    [HttpPost]
    public async Task<IActionResult> CreateSopStepCheckItemAsync([FromBody] TaktSopStepCheckItemCreateDto dto)
    {
        try
        {
            var result = await _sopStepCheckItemService.CreateSopStepCheckItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP工步检验项目DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP工步检验项目")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopStepCheckItemAsync(long id, [FromBody] TaktSopStepCheckItemUpdateDto dto)
    {
        try
        {
            var result = await _sopStepCheckItemService.UpdateSopStepCheckItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP工步检验项目
    /// </summary>
    /// <param name="id">SOP工步检验项目ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "删除SOP工步检验项目")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopStepCheckItemByIdAsync(long id)
    {
        try
        {
            await _sopStepCheckItemService.DeleteSopStepCheckItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP工步检验项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "批量删除SOP工步检验项目")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopStepCheckItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopStepCheckItemService.DeleteSopStepCheckItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工步检验项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>SOP工步检验项目DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP工步检验项目排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSopStepCheckItemSortAsync([FromBody] TaktSopStepCheckItemSortDto dto)
    {
        try
        {
            var result = await _sopStepCheckItemService.UpdateSopStepCheckItemSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:doc:import", "获取SOP工步检验项目导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopStepCheckItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopStepCheckItemService.GetSopStepCheckItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP工步检验项目
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:import", "导入SOP工步检验项目")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopStepCheckItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopStepCheckItemService.ImportSopStepCheckItemAsync(stream, sheetName);
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
    /// 导出SOP工步检验项目
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:export", "导出SOP工步检验项目")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopStepCheckItemAsync([FromQuery] TaktSopStepCheckItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopStepCheckItemService.ExportSopStepCheckItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
