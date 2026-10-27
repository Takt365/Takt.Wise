// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktUsersController.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户部门管控器（完整CRUD + 导入导出 + 状态管理 + 密码重置）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Identity;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// 用户部门管控器
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "用户管理")]
public class TaktUsersController : TaktControllerBase
{
    private readonly ITaktUserService _userService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userService">用户服务</param>
    public TaktUsersController(ITaktUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取用户列表（分页）
    /// </summary>
    /// <param name="query">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("identity:user:list", "用户列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetUserListAsync([FromQuery] TaktUserQueryDto query)
    {
        try
        {
            var result = await _userService.GetUserListAsync(query);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户DTO</returns>
    [TaktPermission("identity:user:query", "用户详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(long id)
    {
        try
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result == null)
            {
                return NotFound("用户不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用户选项列表（用于下拉框等）
    /// </summary>
    /// <returns>用户选项列表</returns>
    [TaktPermission("identity:user:query", "用户选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetUserOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _userService.GetUserOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="dto">创建用户DTO</param>
    /// <returns>用户DTO</returns>
    [TaktPermission("identity:user:create", "创建用户")]
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] TaktCreateUserDto dto)
    {
        try
        {
            var result = await _userService.CreateUserAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">更新用户DTO</param>
    /// <returns>用户DTO</returns>
    [TaktPermission("identity:user:update", "更新用户")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(long id, [FromBody] TaktUpdateUserDto dto)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>任务</returns>
    [TaktPermission("identity:user:delete", "删除用户")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserByIdAsync(long id)
    {
        try
        {
            await _userService.DeleteUserByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids">用户ID列表</param>
    /// <returns>任务</returns>
    [TaktPermission("identity:user:delete", "批量删除用户")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteUserBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _userService.DeleteUserBatchAsync(ids);
            return Success("批量删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用户状态
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">状态DTO</param>
    /// <returns>用户DTO</returns>
    [TaktPermission("identity:user:update", "更新用户状态")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateUserStatusAsync(long id, [FromBody] TaktUserStatusDto dto)
    {
        try
        {
            // 确保路由ID与DTO中的ID一致
            if (id != dto.UserId)
            {
                return BadRequest("用户ID不匹配");
            }

            var result = await _userService.UpdateUserStatusAsync(dto);
            return Success(result, "状态更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 重置用户密码（管理员操作）
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">重置密码DTO</param>
    /// <returns>任务</returns>
    [TaktPermission("identity:user:update", "重置用户密码")]
    [HttpPut("{id}/reset-password")]
    public async Task<IActionResult> ResetUserPasswordAsync(long id, [FromBody] TaktResetPasswordDto dto)
    {
        try
        {
            // 确保路由ID与DTO中的ID一致
            if (id != dto.UserId)
            {
                return BadRequest("用户ID不匹配");
            }

            await _userService.ResetUserPasswordAsync(dto);
            return Success("密码重置成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="dto">重置密码DTO</param>
    /// <returns>任务</returns>
    [TaktPermission("identity:user:update", "重置密码")]
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] TaktResetPasswordDto dto)
    {
        try
        {
            await _userService.ResetUserPasswordAsync(dto);
            return Success("密码重置成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 修改密码（用户自己操作）
    /// </summary>
    /// <param name="dto">修改密码DTO</param>
    /// <returns>任务</returns>
    [TaktPermission("identity:user:update", "修改密码")]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] TaktChangePasswordDto dto)
    {
        try
        {
            await _userService.ChangePasswordAsync(dto);
            return Success("密码修改成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新当前登录用户头像（个人中心；写入关联员工 Avatar；无需管理端 update 权限）
    /// </summary>
    /// <param name="dto">头像 DTO</param>
    /// <returns>更新后的头像 URL</returns>
    [HttpPut("me/avatar")]
    public async Task<IActionResult> UpdateMyAvatarAsync([FromBody] TaktUpdateMyAvatarDto dto)
    {
        try
        {
            var avatar = await _userService.UpdateMyAvatarAsync(dto);
            return Success(avatar, "头像更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新当前登录用户个人资料（NickName→用户表；手机/邮箱→当前公司员工表；姓名/性别不可改）
    /// </summary>
    /// <param name="dto">个人资料 DTO</param>
    /// <returns>更新后的可展示字段</returns>
    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateMyProfileAsync([FromBody] TaktUpdateMyProfileDto dto)
    {
        try
        {
            var result = await _userService.UpdateMyProfileAsync(dto);
            return Success(result, "个人资料更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 忘记密码（发送密码重置邮件）
    /// </summary>
    /// <param name="dto">忘记密码DTO</param>
    /// <returns>结果</returns>
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] TaktForgotPasswordDto dto)
    {
        try
        {
            var result = await _userService.ForgotPasswordAsync(dto);
            if (!result.Success)
            {
                return Error(result.Message ?? GetLocalizedString("common.feedback.failed"), Takt.Shared.Enums.TaktResultCode.BadRequest);
            }
            return Success(GetLocalizedString(TaktValidationI18nKeys.FeedbackEmailSent));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 解锁用户
    /// </summary>
    /// <param name="dto">解锁用户DTO</param>
    /// <returns>用户DTO</returns>
    [TaktPermission("identity:user:update", "解锁用户")]
    [HttpPut("unlock")]
    public async Task<IActionResult> UnlockAsync([FromBody] TaktUserUnlockDto dto)
    {
        try
        {
            var result = await _userService.UnlockUserAsync(dto);
            return Success(result, "用户解锁成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #region 统计分析

    /// <summary>
    /// 统计用户总数
    /// </summary>
    /// <returns>用户总数</returns>
    [TaktPermission("identity:user:list", "统计用户总数")]
    [HttpGet("count")]
    public async Task<IActionResult> GetUserCountAsync()
    {
        try
        {
            var result = await _userService.GetUserCountAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 导入导出

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:user:import", "导入用户")]
    [HttpGet("template")]
    public async Task<IActionResult> GetUserTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (fileName, content) = await _userService.GetUserTemplateAsync(sheetName, templateName);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入用户
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    [TaktPermission("identity:user:import", "导入用户")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportUserAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _userService.ImportUserAsync(stream, sheetName);

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
    /// 导出用户
    /// </summary>
    /// <param name="query">查询DTO</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel文件</returns>
    [TaktPermission("identity:user:export", "导出用户")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportUserAsync([FromQuery] TaktUserQueryDto query, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, content) = await _userService.ExportUserAsync(query, sheetName, exportName);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion
}
