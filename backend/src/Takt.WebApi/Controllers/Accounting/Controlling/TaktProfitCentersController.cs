// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktProfitCentersController.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心控制器
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
/// 利润中心控制器
/// 提供利润中心的 REST API
/// </summary>
[ApiModule(3, "管控会计")]
[Route("api/[controller]", Name = "利润中心")]
public class TaktProfitCentersController : TaktControllerBase
{
    private readonly ITaktProfitCenterService _profitCenterService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitCenterService">利润中心服务</param>
    public TaktProfitCentersController(ITaktProfitCenterService profitCenterService)
    {
        _profitCenterService = profitCenterService;
    }

    /// <summary>
    /// 获取利润中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:profit:center:list", "利润中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProfitCenterListAsync([FromQuery] TaktProfitCenterQueryDto queryDto)
    {
        try
        {
            var result = await _profitCenterService.GetProfitCenterListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>利润中心DTO</returns>
    [TaktPermission("accounting:controlling:profit:center:query", "利润中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfitCenterByIdAsync(long id)
    {
        try
        {
            var result = await _profitCenterService.GetProfitCenterByIdAsync(id);
            if (result == null)
            {
                return NotFound("利润中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取利润中心树形选项列表（DictValue 为 ProfitCenterCode，DictLabel 为利润中心名称）
    /// </summary>
    /// <returns>树形选项</returns>
    [TaktPermission("accounting:controlling:profit:center:query", "利润中心树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetProfitCenterTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _profitCenterService.GetProfitCenterTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取利润中心树形列表
    /// </summary>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 枚举字段，如 TaktCommonStatus.Enabled）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("accounting:controlling:profit:center:query", "利润中心树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetProfitCenterTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _profitCenterService.GetProfitCenterTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建利润中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>利润中心DTO</returns>
    [TaktPermission("accounting:controlling:profit:center:create", "创建利润中心")]
    [HttpPost]
    public async Task<IActionResult> CreateProfitCenterAsync([FromBody] TaktProfitCenterCreateDto dto)
    {
        try
        {
            var result = await _profitCenterService.CreateProfitCenterAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>利润中心DTO</returns>
    [TaktPermission("accounting:controlling:profit:center:update", "更新利润中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfitCenterAsync(long id, [FromBody] TaktProfitCenterUpdateDto dto)
    {
        try
        {
            var result = await _profitCenterService.UpdateProfitCenterAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:profit:center:delete", "删除利润中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfitCenterByIdAsync(long id)
    {
        try
        {
            await _profitCenterService.DeleteProfitCenterByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除利润中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:profit:center:delete", "批量删除利润中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProfitCenterBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _profitCenterService.DeleteProfitCenterBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>利润中心DTO</returns>
    [TaktPermission("accounting:controlling:profit:center:update", "更新利润中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProfitCenterStatusAsync([FromBody] TaktProfitCenterStatusDto dto)
    {
        try
        {
            var result = await _profitCenterService.UpdateProfitCenterStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>利润中心DTO</returns>
    [TaktPermission("accounting:controlling:profit:center:update", "更新利润中心排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateProfitCenterSortAsync([FromBody] TaktProfitCenterSortDto dto)
    {
        try
        {
            var result = await _profitCenterService.UpdateProfitCenterSortAsync(dto);
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
    [TaktPermission("accounting:controlling:profit:center:import", "获取利润中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProfitCenterTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _profitCenterService.GetProfitCenterTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入利润中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:controlling:profit:center:import", "导入利润中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProfitCenterAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _profitCenterService.ImportProfitCenterAsync(stream, sheetName);
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
    /// 导出利润中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:profit:center:export", "导出利润中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProfitCenterAsync([FromQuery] TaktProfitCenterQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _profitCenterService.ExportProfitCenterAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
