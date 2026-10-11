// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktTenantsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：租户控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Identity;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// 租户控制器
/// 提供租户的 REST API
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "租户")]
public class TaktTenantsController : TaktControllerBase
{
    private readonly ITaktTenantService _tenantService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tenantService">租户服务</param>
    public TaktTenantsController(ITaktTenantService tenantService)
    {
        _tenantService = tenantService;
    }

    /// <summary>
    /// 获取租户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("identity:tenant:list", "租户列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTenantListAsync([FromQuery] TaktTenantQueryDto queryDto)
    {
        try
        {
            var result = await _tenantService.GetTenantListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>租户DTO</returns>
    [TaktPermission("identity:tenant:query", "租户详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTenantByIdAsync(long id)
    {
        try
        {
            var result = await _tenantService.GetTenantByIdAsync(id);
            if (result == null)
            {
                return NotFound("租户不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前登录会话的租户选项（仅一项，DictValue 为 TenantCode；登录后不可跨租户切换）
    /// </summary>
    /// <returns>下拉选项</returns>
    [AllowAnonymous]
    [HttpGet("options")]
    public async Task<IActionResult> GetTenantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _tenantService.GetTenantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>租户DTO</returns>
    [TaktPermission("identity:tenant:create", "创建租户")]
    [HttpPost]
    public async Task<IActionResult> CreateTenantAsync([FromBody] TaktTenantCreateDto dto)
    {
        try
        {
            var result = await _tenantService.CreateTenantAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>租户DTO</returns>
    [TaktPermission("identity:tenant:update", "更新租户")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenantAsync(long id, [FromBody] TaktTenantUpdateDto dto)
    {
        try
        {
            var result = await _tenantService.UpdateTenantAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:tenant:delete", "删除租户")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenantByIdAsync(long id)
    {
        try
        {
            await _tenantService.DeleteTenantByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除租户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:tenant:delete", "批量删除租户")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTenantBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _tenantService.DeleteTenantBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新租户状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>租户DTO</returns>
    [TaktPermission("identity:tenant:update", "更新租户状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTenantStatusAsync([FromBody] TaktTenantStatusDto dto)
    {
        try
        {
            var result = await _tenantService.UpdateTenantStatusAsync(dto);
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
    [TaktPermission("identity:tenant:import", "获取租户导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTenantTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _tenantService.GetTenantTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入租户
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("identity:tenant:import", "导入租户")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTenantAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _tenantService.ImportTenantAsync(stream, sheetName);
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
    /// 导出租户
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:tenant:export", "导出租户")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTenantAsync([FromQuery] TaktTenantQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _tenantService.ExportTenantAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
