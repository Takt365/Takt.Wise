// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktCostElementsController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Application.Services.Accounting.Controlling;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Controlling;

/// <summary>
/// 成本要素控制器
/// 提供成本要素的 REST API
/// </summary>
[ApiModule(3, "管控会计")]
[Route("api/[controller]", Name = "成本要素")]
public class TaktCostElementsController : TaktControllerBase
{
    private readonly ITaktCostElementService _costElementService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costElementService">成本要素服务</param>
    public TaktCostElementsController(ITaktCostElementService costElementService)
    {
        _costElementService = costElementService;
    }

    /// <summary>
    /// 获取成本要素列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:cost:element:list", "成本要素列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCostElementListAsync([FromQuery] TaktCostElementQueryDto queryDto)
    {
        try
        {
            var result = await _costElementService.GetCostElementListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>成本要素DTO</returns>
    [TaktPermission("accounting:controlling:cost:element:query", "成本要素详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCostElementByIdAsync(long id)
    {
        try
        {
            var result = await _costElementService.GetCostElementByIdAsync(id);
            if (result == null)
            {
                return NotFound("成本要素不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本要素树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 CostElementCode）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("accounting:controlling:cost:element:query", "成本要素树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetCostElementTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _costElementService.GetCostElementTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本要素树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("accounting:controlling:cost:element:query", "成本要素树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetCostElementTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _costElementService.GetCostElementTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建成本要素
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>成本要素DTO</returns>
    [TaktPermission("accounting:controlling:cost:element:create", "创建成本要素")]
    [HttpPost]
    public async Task<IActionResult> CreateCostElementAsync([FromBody] TaktCostElementCreateDto dto)
    {
        try
        {
            var result = await _costElementService.CreateCostElementAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>成本要素DTO</returns>
    [TaktPermission("accounting:controlling:cost:element:update", "更新成本要素")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostElementAsync(long id, [FromBody] TaktCostElementUpdateDto dto)
    {
        try
        {
            var result = await _costElementService.UpdateCostElementAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:element:delete", "删除成本要素")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostElementByIdAsync(long id)
    {
        try
        {
            await _costElementService.DeleteCostElementByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除成本要素
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:element:delete", "批量删除成本要素")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCostElementBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _costElementService.DeleteCostElementBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本要素状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>成本要素DTO</returns>
    [TaktPermission("accounting:controlling:cost:element:update", "更新成本要素状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCostElementStatusAsync([FromBody] TaktCostElementStatusDto dto)
    {
        try
        {
            var result = await _costElementService.UpdateCostElementStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本要素排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>成本要素DTO</returns>
    [TaktPermission("accounting:controlling:cost:element:update", "更新成本要素排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCostElementSortAsync([FromBody] TaktCostElementSortDto dto)
    {
        try
        {
            var result = await _costElementService.UpdateCostElementSortAsync(dto);
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
    [TaktPermission("accounting:controlling:cost:element:import", "获取成本要素导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCostElementTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _costElementService.GetCostElementTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入成本要素
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:controlling:cost:element:import", "导入成本要素")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCostElementAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _costElementService.ImportCostElementAsync(stream, sheetName);
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
    /// 导出成本要素
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:cost:element:export", "导出成本要素")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCostElementAsync([FromQuery] TaktCostElementQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _costElementService.ExportCostElementAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
