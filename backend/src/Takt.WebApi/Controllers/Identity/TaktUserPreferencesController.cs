// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktUserPreferencesController.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：用户个人偏好控制器
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
/// 用户个人偏好控制器
/// 提供用户个人偏好的 REST API
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "用户个人偏好")]
public class TaktUserPreferencesController : TaktControllerBase
{
    private readonly ITaktUserPreferenceService _userPreferenceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userPreferenceService">用户个人偏好服务</param>
    public TaktUserPreferencesController(ITaktUserPreferenceService userPreferenceService)
    {
        _userPreferenceService = userPreferenceService;
    }

    /// <summary>
    /// 获取当前登录用户的个人偏好（无记录时 Data 为 null）
    /// </summary>
    /// <returns>用户个人偏好DTO</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserPreferenceAsync()
    {
        try
        {
            var result = await _userPreferenceService.GetMyUserPreferenceAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// Upsert 当前登录用户的个人偏好
    /// </summary>
    /// <param name="dto">Upsert DTO</param>
    /// <returns>用户个人偏好DTO</returns>
    [HttpPut("me")]
    public async Task<IActionResult> UpsertMyUserPreferenceAsync([FromBody] TaktUserPreferenceMeUpsertDto dto)
    {
        try
        {
            var result = await _userPreferenceService.UpsertMyUserPreferenceAsync(dto);
            return Success(result, "保存成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用户个人偏好列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("identity:user:preference:list", "用户个人偏好列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetUserPreferenceListAsync([FromQuery] TaktUserPreferenceQueryDto queryDto)
    {
        try
        {
            var result = await _userPreferenceService.GetUserPreferenceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>用户个人偏好DTO</returns>
    [TaktPermission("identity:user:preference:query", "用户个人偏好详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserPreferenceByIdAsync(long id)
    {
        try
        {
            var result = await _userPreferenceService.GetUserPreferenceByIdAsync(id);
            if (result == null)
            {
                return NotFound("用户个人偏好不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用户个人偏好选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("identity:user:preference:query", "用户个人偏好选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetUserPreferenceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _userPreferenceService.GetUserPreferenceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建用户个人偏好
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>用户个人偏好DTO</returns>
    [TaktPermission("identity:user:preference:create", "创建用户个人偏好")]
    [HttpPost]
    public async Task<IActionResult> CreateUserPreferenceAsync([FromBody] TaktUserPreferenceCreateDto dto)
    {
        try
        {
            var result = await _userPreferenceService.CreateUserPreferenceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>用户个人偏好DTO</returns>
    [TaktPermission("identity:user:preference:update", "更新用户个人偏好")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserPreferenceAsync(long id, [FromBody] TaktUserPreferenceUpdateDto dto)
    {
        try
        {
            var result = await _userPreferenceService.UpdateUserPreferenceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:user:preference:delete", "删除用户个人偏好")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserPreferenceByIdAsync(long id)
    {
        try
        {
            await _userPreferenceService.DeleteUserPreferenceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除用户个人偏好
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("identity:user:preference:delete", "批量删除用户个人偏好")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteUserPreferenceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _userPreferenceService.DeleteUserPreferenceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用户个人偏好状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>用户个人偏好DTO</returns>
    [TaktPermission("identity:user:preference:update", "更新用户个人偏好状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateUserPreferenceStatusAsync([FromBody] TaktUserPreferenceStatusDto dto)
    {
        try
        {
            var result = await _userPreferenceService.UpdateUserPreferenceStatusAsync(dto);
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
    [TaktPermission("identity:user:preference:import", "获取用户个人偏好导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetUserPreferenceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _userPreferenceService.GetUserPreferenceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入用户个人偏好
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("identity:user:preference:import", "导入用户个人偏好")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportUserPreferenceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _userPreferenceService.ImportUserPreferenceAsync(stream, sheetName);
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
    /// 导出用户个人偏好
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:user:preference:export", "导出用户个人偏好")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportUserPreferenceAsync([FromQuery] TaktUserPreferenceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _userPreferenceService.ExportUserPreferenceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
