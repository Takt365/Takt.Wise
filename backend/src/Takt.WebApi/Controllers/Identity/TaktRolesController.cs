// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktRolesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：角色控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Identity;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// 角色控制器
/// 提供角色的 REST API
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "角色")]
public class TaktRolesController : TaktControllerBase
{
    private readonly ITaktRoleService _roleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleService">角色服务</param>
    public TaktRolesController(ITaktRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// 获取角色列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("identity:role:list", "角色列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetRoleListAsync([FromQuery] TaktRoleQueryDto queryDto)
    {
        try
        {
            var result = await _roleService.GetRoleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:query", "角色详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleByIdAsync(long id)
    {
        try
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            if (result == null)
            {
                return NotFound("角色不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取角色选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("identity:role:query", "角色选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetRoleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _roleService.GetRoleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:create", "创建角色")]
    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] TaktRoleCreateDto dto)
    {
        try
        {
            var result = await _roleService.CreateRoleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:update", "更新角色")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoleAsync(long id, [FromBody] TaktRoleUpdateDto dto)
    {
        try
        {
            var result = await _roleService.UpdateRoleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:role:delete", "删除角色")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoleByIdAsync(long id)
    {
        try
        {
            await _roleService.DeleteRoleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除角色
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:role:delete", "批量删除角色")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteRoleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _roleService.DeleteRoleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新角色状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:update", "更新角色状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateRoleStatusAsync([FromBody] TaktRoleStatusDto dto)
    {
        try
        {
            var result = await _roleService.UpdateRoleStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新角色内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:update", "更新角色内置")]
    [HttpPut("built-in")]
    public async Task<IActionResult> UpdateRoleBuiltInAsync([FromBody] TaktRoleBuiltInDto dto)
    {
        try
        {
            var result = await _roleService.UpdateRoleBuiltInAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新角色排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>角色DTO</returns>
    [TaktPermission("identity:role:update", "更新角色排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateRoleSortAsync([FromBody] TaktRoleSortDto dto)
    {
        try
        {
            var result = await _roleService.UpdateRoleSortAsync(dto);
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
    [TaktPermission("identity:role:import", "获取角色导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetRoleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _roleService.GetRoleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入角色
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("identity:role:import", "导入角色")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportRoleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _roleService.ImportRoleAsync(stream, sheetName);
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
    /// 导出角色
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:role:export", "导出角色")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportRoleAsync([FromQuery] TaktRoleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _roleService.ExportRoleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
