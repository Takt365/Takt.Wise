// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktCostCentersController.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心控制器
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
/// 成本中心控制器
/// 提供成本中心的 REST API
/// </summary>
[ApiModule(3, "管控会计")]
[Route("api/[controller]", Name = "成本中心")]
public class TaktCostCentersController : TaktControllerBase
{
    private readonly ITaktCostCenterService _costCenterService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costCenterService">成本中心服务</param>
    public TaktCostCentersController(ITaktCostCenterService costCenterService)
    {
        _costCenterService = costCenterService;
    }

    /// <summary>
    /// 获取成本中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:cost:center:list", "成本中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCostCenterListAsync([FromQuery] TaktCostCenterQueryDto queryDto)
    {
        try
        {
            var result = await _costCenterService.GetCostCenterListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>成本中心DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:query", "成本中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCostCenterByIdAsync(long id)
    {
        try
        {
            var result = await _costCenterService.GetCostCenterByIdAsync(id);
            if (result == null)
            {
                return NotFound("成本中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本中心树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("accounting:controlling:cost:center:query", "成本中心树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetCostCenterTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _costCenterService.GetCostCenterTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本中心树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("accounting:controlling:cost:center:query", "成本中心树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetCostCenterTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _costCenterService.GetCostCenterTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建成本中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>成本中心DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:create", "创建成本中心")]
    [HttpPost]
    public async Task<IActionResult> CreateCostCenterAsync([FromBody] TaktCostCenterCreateDto dto)
    {
        try
        {
            var result = await _costCenterService.CreateCostCenterAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>成本中心DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:update", "更新成本中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostCenterAsync(long id, [FromBody] TaktCostCenterUpdateDto dto)
    {
        try
        {
            var result = await _costCenterService.UpdateCostCenterAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:center:delete", "删除成本中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostCenterByIdAsync(long id)
    {
        try
        {
            await _costCenterService.DeleteCostCenterByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除成本中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:center:delete", "批量删除成本中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCostCenterBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _costCenterService.DeleteCostCenterBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>成本中心DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:update", "更新成本中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCostCenterStatusAsync([FromBody] TaktCostCenterStatusDto dto)
    {
        try
        {
            var result = await _costCenterService.UpdateCostCenterStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>成本中心DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:update", "更新成本中心排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCostCenterSortAsync([FromBody] TaktCostCenterSortDto dto)
    {
        try
        {
            var result = await _costCenterService.UpdateCostCenterSortAsync(dto);
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
    [TaktPermission("accounting:controlling:cost:center:import", "获取成本中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCostCenterTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _costCenterService.GetCostCenterTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入成本中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:controlling:cost:center:import", "导入成本中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCostCenterAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _costCenterService.ImportCostCenterAsync(stream, sheetName);
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
    /// 导出成本中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:cost:center:export", "导出成本中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCostCenterAsync([FromQuery] TaktCostCenterQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _costCenterService.ExportCostCenterAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
