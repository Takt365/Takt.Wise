// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktUserService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户管理应用服务接口
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Shared.Models;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 用户管理应用服务接口
/// </summary>
public interface ITaktUserService
{
    /// <summary>
    /// 获取用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktUserDto>> GetUserListAsync(TaktUserQueryDto queryDto);

    /// <summary>
    /// 根据ID获取用户（包含子表数据）
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户DTO</returns>
    Task<TaktUserDto?> GetUserByIdAsync(long id);

    /// <summary>
    /// 获取用户选项列表（用于下拉框等）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>用户选项列表</returns>
    Task<List<TaktSelectOption>> GetUserOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建用户（包含子表数据）
    /// </summary>
    /// <param name="dto">创建用户DTO</param>
    /// <returns>用户DTO</returns>
    Task<TaktUserDto> CreateUserAsync(TaktCreateUserDto dto);

    /// <summary>
    /// 更新用户（包含子表数据）
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">更新用户DTO</param>
    /// <returns>用户DTO</returns>
    Task<TaktUserDto> UpdateUserAsync(long id, TaktUpdateUserDto dto);

    /// <summary>
    /// 删除用户（级联删除子表）
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>任务</returns>
    Task DeleteUserByIdAsync(long id);

    /// <summary>
    /// 批量删除用户（级联删除子表）
    /// </summary>
    /// <param name="ids">用户ID列表</param>
    /// <returns>任务</returns>
    Task DeleteUserBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新用户状态
    /// </summary>
    /// <param name="dto">用户状态DTO</param>
    /// <returns>用户DTO</returns>
    Task<TaktUserDto> UpdateUserStatusAsync(TaktUserStatusDto dto);

    #region 密码管理

    /// <summary>
    /// 重置用户密码（管理员按 UserId 重置）
    /// </summary>
    /// <param name="dto">重置密码 DTO（TaktResetPasswordDto）</param>
    /// <returns>任务</returns>
    Task ResetUserPasswordAsync(TaktResetPasswordDto dto);

    /// <summary>
    /// 修改密码（当前登录用户修改自己的密码，TaktChangePasswordDto 不含 UserId）
    /// </summary>
    /// <param name="dto">修改密码 DTO</param>
    /// <returns>任务</returns>
    Task ChangePasswordAsync(TaktChangePasswordDto dto);

    /// <summary>
    /// 更新当前登录用户头像（写入关联员工 Avatar；须已绑定 EmployeeId；空串清除）
    /// </summary>
    /// <param name="dto">头像 DTO</param>
    /// <returns>更新后的头像 URL（清除时为 null）</returns>
    Task<string?> UpdateMyAvatarAsync(TaktUpdateMyAvatarDto dto);

    /// <summary>
    /// 更新当前登录用户个人资料（NickName→TaktUser；手机/邮箱→当前公司 TaktEmployee；姓名/性别只读）
    /// </summary>
    /// <param name="dto">个人资料 DTO</param>
    /// <returns>更新后的可展示字段</returns>
    Task<TaktUpdateMyProfileResultDto> UpdateMyProfileAsync(TaktUpdateMyProfileDto dto);

    /// <summary>
    /// 忘记密码（发送密码重置邮件）
    /// </summary>
    /// <param name="dto">忘记密码 DTO（TaktForgotPasswordDto）</param>
    /// <returns>结果，Success 为 false 时 Code 为 EmailNotFound 或 ProtectedUser</returns>
    Task<TaktForgotPasswordResultDto> ForgotPasswordAsync(TaktForgotPasswordDto dto);

    /// <summary>
    /// 解锁用户（清除登录失败计数与锁定时间）
    /// </summary>
    /// <param name="dto">解锁用户 DTO（TaktUserUnlockDto）</param>
    /// <returns>用户 DTO</returns>
    Task<TaktUserDto> UnlockUserAsync(TaktUserUnlockDto dto);

    #endregion

    #region 统计分析

    /// <summary>
    /// 统计用户总数
    /// </summary>
    /// <returns>用户总数</returns>
    Task<long> GetUserCountAsync();

    #endregion

    #region 导入导出

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel模板文件信息（文件名和内容）</returns>
    Task<(string fileName, byte[] content)> GetUserTemplateAsync(string? sheetName, string? fileName);

    /// <summary>
    /// 导入用户
    /// </summary>
    /// <param name="fileStream">Excel文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果（成功数量、失败数量、错误信息列表）</returns>
    Task<(int success, int fail, List<string> errors)> ImportUserAsync(Stream fileStream, string? sheetName);

    /// <summary>
    /// 导出用户
    /// </summary>
    /// <param name="query">用户查询DTO（包含查询条件）</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel文件信息（文件名和内容）</returns>
    Task<(string fileName, byte[] content)> ExportUserAsync(TaktUserQueryDto query, string? sheetName, string? fileName);

    #endregion
}
