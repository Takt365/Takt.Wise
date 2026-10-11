// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktMenusController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Identity;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// 菜单控制器
/// 提供菜单的 REST API
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "菜单")]
public class TaktMenusController : TaktControllerBase
{
    private readonly ITaktMenuService _menuService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="menuService">菜单服务</param>
    public TaktMenusController(ITaktMenuService menuService)
    {
        _menuService = menuService;
    }

    /// <summary>
    /// 获取菜单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("identity:menu:list", "菜单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMenuListAsync([FromQuery] TaktMenuQueryDto queryDto)
    {
        try
        {
            var result = await _menuService.GetMenuListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>菜单DTO</returns>
    [TaktPermission("identity:menu:query", "菜单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuByIdAsync(long id)
    {
        try
        {
            var result = await _menuService.GetMenuByIdAsync(id);
            if (result == null)
            {
                return NotFound("菜单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取菜单树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    [TaktPermission("identity:menu:query", "菜单树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetMenuTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? valueBy = null)
    {
        try
        {
            var result = await _menuService.GetMenuTreeOptionsAsync(parentId, plantCode, keyword, valueBy);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取菜单树形列表
    /// </summary>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 枚举字段，如 1）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("identity:menu:query", "菜单树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetMenuTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _menuService.GetMenuTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 角色菜单分配：互斥懒加载树（未分配 / 已分配各一层）
    /// </summary>
    /// <param name="queryDto">父级、侧别、已分配 Id</param>
    /// <returns>树形一层</returns>
    [TaktPermission("identity:role:query", "角色菜单分配树")]
    [HttpPost("assign-tree")]
    public async Task<IActionResult> GetMenuAssignTreeAsync([FromBody] TaktMenuAssignTreeQueryDto queryDto)
    {
        try
        {
            var result = await _menuService.GetMenuAssignTreeAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>菜单DTO</returns>
    [TaktPermission("identity:menu:create", "创建菜单")]
    [HttpPost]
    public async Task<IActionResult> CreateMenuAsync([FromBody] TaktMenuCreateDto dto)
    {
        try
        {
            var result = await _menuService.CreateMenuAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>菜单DTO</returns>
    [TaktPermission("identity:menu:update", "更新菜单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuAsync(long id, [FromBody] TaktMenuUpdateDto dto)
    {
        try
        {
            var result = await _menuService.UpdateMenuAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:menu:delete", "删除菜单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuByIdAsync(long id)
    {
        try
        {
            await _menuService.DeleteMenuByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:menu:delete", "批量删除菜单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMenuBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _menuService.DeleteMenuBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新菜单状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>菜单DTO</returns>
    [TaktPermission("identity:menu:update", "更新菜单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMenuStatusAsync([FromBody] TaktMenuStatusDto dto)
    {
        try
        {
            var result = await _menuService.UpdateMenuStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新菜单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>菜单DTO</returns>
    [TaktPermission("identity:menu:update", "更新菜单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateMenuSortAsync([FromBody] TaktMenuSortDto dto)
    {
        try
        {
            var result = await _menuService.UpdateMenuSortAsync(dto);
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
    [TaktPermission("identity:menu:import", "获取菜单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMenuTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _menuService.GetMenuTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入菜单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("identity:menu:import", "导入菜单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMenuAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _menuService.ImportMenuAsync(stream, sheetName);
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
    /// 导出菜单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:menu:export", "导出菜单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMenuAsync([FromQuery] TaktMenuQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _menuService.ExportMenuAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
