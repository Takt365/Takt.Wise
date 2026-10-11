// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktUserService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户管理应用服务实现（完整CRUD + 导入导出 + 状态管理 + 密码重置）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SqlSugar;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 用户管理应用服务
/// </summary>
public class TaktUserService : TaktServiceBase, ITaktUserService
{
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktEmployeeProfileResolver _employeeProfileResolver;
    private readonly ITaktRbacService _rbacService;
    private readonly ITaktPermissionService _permissionService;
    private readonly ITaktDictDataService _dictDataService;
    private readonly TaktPasswordPolicyOptions _passwordPolicyOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="employeeProfileResolver">员工档案跨公司解析</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>
    /// <param name="permissionService">权限服务</param>
    /// <param name="dictDataService">字典数据服务</param>
    /// <param name="passwordPolicyOptions">密码策略配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktUserService(
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktEmployeeProfileResolver employeeProfileResolver,
        ITaktRbacService rbacService,
        ITaktPermissionService permissionService,
        ITaktDictDataService dictDataService,
        IOptions<TaktPasswordPolicyOptions> passwordPolicyOptions,
        ITaktUserContext userContext,
        ITaktLocalizationService localizationService)
        : base(userContext, localizationService)
    {
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _employeeProfileResolver = employeeProfileResolver;
        _rbacService = rbacService;
        _permissionService = permissionService;
        _dictDataService = dictDataService;
        _passwordPolicyOptions = passwordPolicyOptions.Value;
    }

    // ========================================
    // CRUD 方法
    // ========================================

    /// <summary>
    /// 获取用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktUserDto>> GetUserListAsync(TaktUserQueryDto queryDto)
    {
        LogInformation("开始查询用户列表");

        try
        {
            var predicate = QueryExpression(queryDto);
            var (data, total) = await _userRepository.GetPagedAsync(
                queryDto.PageIndex,
                queryDto.PageSize,
                predicate);

            var dtos = data.Adapt<List<TaktUserDto>>();

            // 填充关联数据
            foreach (var dto in dtos)
            {
                var user = data.First(u => u.Id == dto.UserId);
                await FillUserDtoAsync(dto, user);
            }

            LogInformation("查询用户列表成功，总数: {Total}", total);

            return TaktPagedResult<TaktUserDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
        }
        catch (Exception ex)
        {
            LogError(ex, "查询用户列表失败");
            throw;
        }
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto?> GetUserByIdAsync(long id)
    {
        var entity = await _userRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }

        var dto = entity.Adapt<TaktUserDto>();
        await FillUserDtoAsync(dto, entity);

        return dto;
    }

    /// <summary>
    /// 获取用户选项列表（用于下拉/分配；DB 分页上限 200；keyword 模糊用户名/昵称）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配用户名/昵称）</param>
    /// <returns>用户选项列表</returns>
    public async Task<List<TaktSelectOption>> GetUserOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        const int maxOptions = 200;
        const int fetchSize = 500;
        var kw = keyword?.Trim();
        Expression<Func<TaktUser, bool>> predicate = string.IsNullOrEmpty(kw)
            ? (u => u.TenantCode == CurrentTenantCode)
            : (u => u.TenantCode == CurrentTenantCode
                && ((u.UserName != null && u.UserName.Contains(kw))
                    || (u.NickName != null && u.NickName.Contains(kw))));
        var (users, _) = await _userRepository.GetPagedAsync(
            predicate,
            1,
            fetchSize,
            u => u.UserName ?? string.Empty,
            false);
        var options = new List<TaktSelectOption>();
        foreach (var user in users)
        {
            if (options.Count >= maxOptions)
            {
                break;
            }
            if (user.Id <= 0)
            {
                continue;
            }
            if (!await _permissionService.HasCompanyAccessAsync(user.Id, CurrentTenantCode, CurrentCompanyCode))
            {
                continue;
            }
            options.Add(new TaktSelectOption
            {
                DictValue = user.Id.ToString(),
                DictLabel = $"{user.UserName} ({user.NickName})",
                ExtValue = user.UserName,
            });
        }
        return options;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="dto">创建用户DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> CreateUserAsync(TaktCreateUserDto dto)
    {
        // 1. 业务校验：用户名唯一性
        var exists = await _userRepository.FirstAsync(u => 
            u.TenantCode == CurrentTenantCode && u.UserName == dto.UserName);
        if (exists != null)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.Duplicate, TaktValidationI18nKeys.EntityUserName, dto.UserName);
        }

        // 2. 业务校验：员工ID必须存在
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee == null || employee.TenantCode != CurrentTenantCode)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserEmployeeId, dto.EmployeeId);
        }

        ValidateNewPassword(dto.PasswordHash);

        // 3. 创建实体
        var entity = dto.Adapt<TaktUser>();
        entity.TenantCode = CurrentTenantCode;
        entity.IsBuiltIn = 0;
        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.PasswordHash);
        entity.CreatedBy = CurrentUserId ?? 0;
        entity.CreatedAt = DateTime.Now;

        entity = await _userRepository.CreateAsync(entity);

        if (dto.RoleIds != null)
        {
            await _rbacService.AssignUserRolesAsync(entity.Id, dto.RoleIds);
        }
        if (dto.CompanyCodes != null)
        {
            await _rbacService.AssignUserCompaniesAsync(entity.Id, dto.CompanyCodes);
        }

        // 4. 返回结果
        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">更新用户DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> UpdateUserAsync(long id, TaktUpdateUserDto dto)
    {
        // 1. 检查实体存在
        var entity = await _userRepository.GetByIdAsync(id);
        entity = EnsureExists(entity, "用户不存在");

        if (entity.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改此用户");
        }

        // 2. 业务规则校验：员工ID必须存在
        if (entity.EmployeeId != dto.EmployeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null || employee.TenantCode != CurrentTenantCode)
            {
                ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserEmployeeId, dto.EmployeeId);
            }
        }

        // 3. 更新实体（保留不参与更新的字段）
        var originalPasswordHash = entity.PasswordHash;
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.PasswordHash = originalPasswordHash;
        entity.IsBuiltIn = originalIsBuiltIn;

        // 4. 更新审计字段
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        // 5. 保存
        await _userRepository.UpdateAsync(entity);

        if (dto.RoleIds != null)
        {
            await _rbacService.AssignUserRolesAsync(id, dto.RoleIds);
        }
        if (dto.CompanyCodes != null)
        {
            await _rbacService.AssignUserCompaniesAsync(id, dto.CompanyCodes);
        }

        // 6. 返回结果
        return await GetUserByIdAsync(id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>任务</returns>
    public async Task DeleteUserByIdAsync(long id)
    {
        LogInformation("开始删除用户，ID: {Id}", id);

        try
        {
            // 1. 查询实体
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null)
            {
                ThrowBusinessException("用户不存在");
            }

            // 2. 租户权限校验
            if (entity.TenantCode != CurrentTenantCode)
            {
                ThrowBusinessException("无权限删除此用户");
            }

            // 3. 保护规则校验：内置用户不可删
            if (entity.IsBuiltIn == 1)
            {
                ThrowBusinessException("内置用户不允许删除");
            }

            await _rbacService.AssignUserRolesAsync(id, Array.Empty<long>());
            await _rbacService.AssignUserCompaniesAsync(id, Array.Empty<string>());

            // 4. 软删除：设置 IsDeleted = 1，并同步更新 UserStatus = Disabled
            entity.IsDeleted = 1;
            entity.UserStatus = 0;

            await _userRepository.UpdateAsync(entity);

            LogInformation("删除用户成功，ID: {Id}", id);
        }
        catch (Exception ex)
        {
            LogError(ex, $"删除用户失败，ID: {id}");
            throw;
        }
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids">用户ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteUserBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0) return;

        LogInformation("开始批量删除用户，数量: {Count}", idList.Count);

        try
        {
            // 1. 获取所有要删除的实体（带租户过滤）
            var entities = new List<TaktUser>();
            foreach (var id in idList)
            {
                var entity = await _userRepository.GetByIdAsync(id);
                if (entity != null && entity.TenantCode == CurrentTenantCode)
                {
                    entities.Add(entity);
                }
            }

            if (entities.Count == 0)
            {
                ThrowBusinessException("未找到可删除的用户");
            }

            // 2. 保护规则校验：内置用户不可删
            if (entities.Any(e => e.IsBuiltIn == 1))
            {
                ThrowBusinessException("内置用户不允许删除");
            }

            foreach (var entity in entities)
            {
                await _rbacService.AssignUserRolesAsync(entity.Id, Array.Empty<long>());
                await _rbacService.AssignUserCompaniesAsync(entity.Id, Array.Empty<string>());
            }

            // 3. 批量软删：设置 IsDeleted = 1，并同步更新 UserStatus = Disabled
            foreach (var entity in entities)
            {
                entity.IsDeleted = 1;
                entity.UserStatus = 0;
            }

            await _userRepository.UpdateRangeAsync(entities);

            LogInformation("批量删除用户成功，数量: {Count}", entities.Count);
        }
        catch (Exception ex)
        {
            LogError(ex, "批量删除用户失败");
            throw;
        }
    }

    // ========================================
    // 状态管理方法
    // ========================================

    /// <summary>
    /// 更新用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> UpdateUserStatusAsync(TaktUserStatusDto dto)
    {
        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        entity = EnsureExists(entity, "用户不存在");

        if (entity.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改此用户状态");
        }

        // 业务规则：禁止禁用内置用户
        if (entity.IsBuiltIn == 1 && dto.UserStatus == 0)
        {
            ThrowBusinessException("不允许禁用内置用户");
        }

        entity.UserStatus = dto.UserStatus;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户状态更新成功: UserId={UserId}, Status={Status}", dto.UserId, dto.UserStatus);

        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    // ========================================
    // 密码管理
    // ========================================

    /// <summary>
    /// 重置用户密码（管理员按 UserId 重置）
    /// </summary>
    /// <param name="dto">重置密码 DTO</param>
    /// <returns>任务</returns>
    public async Task ResetUserPasswordAsync(TaktResetPasswordDto dto)
    {
        if (dto.UserId <= 0)
        {
            ThrowBusinessException("用户ID无效");
        }

        ValidateNewPassword(dto.NewPassword);

        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限重置此用户密码");
        }

        if (entity.IsBuiltIn == 1)
        {
            ThrowBusinessException("不允许重置内置用户密码");
        }

        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.NewPassword);
        entity.LoginFailCount = 0;
        entity.LockedUntil = null;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户密码重置成功: UserId={UserId}", dto.UserId);
    }

    /// <summary>
    /// 修改密码（当前登录用户）
    /// </summary>
    /// <param name="dto">修改密码 DTO</param>
    /// <returns>任务</returns>
    public async Task ChangePasswordAsync(TaktChangePasswordDto dto)
    {
        if (!IsAuthenticated || !CurrentUserId.HasValue)
        {
            ThrowBusinessException("用户未登录，无法修改密码");
        }

        if (string.IsNullOrWhiteSpace(dto.OldPassword))
        {
            ThrowBusinessException("旧密码不能为空");
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            ThrowBusinessException("两次输入的新密码不一致");
        }

        if (dto.OldPassword == dto.NewPassword)
        {
            ThrowBusinessException("新密码不能与旧密码相同");
        }

        ValidateNewPassword(dto.NewPassword);

        var entity = await _userRepository.GetByIdAsync(CurrentUserId.Value);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改密码");
        }

        if (!TaktEncryptHelper.VerifyPassword(dto.OldPassword, entity.PasswordHash))
        {
            ThrowBusinessException("旧密码不正确");
        }

        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.NewPassword);
        entity.LoginFailCount = 0;
        entity.LockedUntil = null;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户密码修改成功: UserId={UserId}", CurrentUserId.Value);
    }

    /// <summary>
    /// 更新当前登录用户头像（关联员工 Avatar）
    /// </summary>
    /// <param name="dto">头像 DTO</param>
    /// <returns>更新后的头像 URL（清除时为 null）</returns>
    public async Task<string?> UpdateMyAvatarAsync(TaktUpdateMyAvatarDto dto)
    {
        if (!IsAuthenticated || !CurrentUserId.HasValue)
        {
            ThrowBusinessException("用户未登录，无法更新头像");
        }

        var user = await _userRepository.GetByIdAsync(CurrentUserId.Value);
        EnsureExists(user, "用户不存在");

        if (user!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限更新头像");
        }

        if (user.EmployeeId <= 0)
        {
            ThrowBusinessException("当前账号未绑定员工档案，无法更新头像");
        }

        var companyCode = (CurrentCompanyCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            ThrowBusinessException("当前会话公司无效，无法更新头像");
        }

        var employee = await _employeeProfileResolver.ResolveForCompanyAsync(
            user.EmployeeId,
            CurrentTenantCode!,
            companyCode);
        EnsureExists(employee, "员工档案不存在");

        if (employee!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限更新头像");
        }

        var avatar = string.IsNullOrWhiteSpace(dto.Avatar) ? null : dto.Avatar.Trim();
        if (avatar != null && avatar.Length > 500)
        {
            ThrowBusinessException("头像地址过长");
        }

        employee.Avatar = avatar;
        employee.UpdatedBy = CurrentUserId;
        employee.UpdatedAt = DateTime.Now;
        await _employeeRepository.UpdateAsync(employee);

        LogInformation("用户头像更新成功: UserId={UserId}, EmployeeId={EmployeeId}", CurrentUserId.Value, employee.Id);
        return avatar;
    }

    /// <summary>
    /// 更新当前登录用户个人资料
    /// NickName → takt_identity_user；Mobile/Email → 当前会话公司员工行（姓名/性别不可自助改）
    /// </summary>
    /// <param name="dto">个人资料 DTO</param>
    /// <returns>更新后的可展示字段</returns>
    public async Task<TaktUpdateMyProfileResultDto> UpdateMyProfileAsync(TaktUpdateMyProfileDto dto)
    {
        if (!IsAuthenticated || !CurrentUserId.HasValue)
        {
            ThrowBusinessException("用户未登录，无法更新资料");
        }

        var user = await _userRepository.GetByIdAsync(CurrentUserId.Value);
        EnsureExists(user, "用户不存在");

        if (user!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限更新资料");
        }

        var nickName = (dto.NickName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nickName))
        {
            ThrowBusinessException("昵称不能为空");
        }

        if (nickName.Length < TaktRegexHelper.NickNameMinLength || nickName.Length > TaktRegexHelper.NickNameMaxLength)
        {
            ThrowBusinessException($"昵称长度须为{TaktRegexHelper.NickNameMinLength}–{TaktRegexHelper.NickNameMaxLength}个字符");
        }

        user.NickName = nickName;
        user.UpdatedBy = CurrentUserId;
        user.UpdatedAt = DateTime.Now;
        await _userRepository.UpdateAsync(user);

        var result = new TaktUpdateMyProfileResultDto
        {
            NickName = user.NickName,
        };

        if (user.EmployeeId <= 0)
        {
            LogInformation("用户资料更新成功（仅账号昵称）: UserId={UserId}", CurrentUserId.Value);
            return result;
        }

        var companyCode = (CurrentCompanyCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            ThrowBusinessException("当前会话公司无效，无法更新员工档案");
        }

        var employee = await _employeeProfileResolver.ResolveForCompanyAsync(
            user.EmployeeId,
            CurrentTenantCode!,
            companyCode);
        EnsureExists(employee, "员工档案不存在");

        if (employee!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限更新员工档案");
        }

        var mobile = (dto.Mobile ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(mobile))
        {
            ThrowBusinessException("手机号码不能为空");
        }

        if (mobile.Length > 11)
        {
            ThrowBusinessException("手机号码长度不能超过11个字符");
        }

        var email = (dto.Email ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(email))
        {
            ThrowBusinessException("电子邮箱不能为空");
        }

        if (email.Length > 100)
        {
            ThrowBusinessException("电子邮箱长度不能超过100个字符");
        }

        employee.Mobile = mobile;
        employee.Email = email;
        employee.UpdatedBy = CurrentUserId;
        employee.UpdatedAt = DateTime.Now;
        await _employeeRepository.UpdateAsync(employee);

        result.Mobile = employee.Mobile;
        result.Email = employee.Email;

        LogInformation(
            "用户资料更新成功: UserId={UserId}, EmployeeId={EmployeeId}",
            CurrentUserId.Value,
            employee.Id);
        return result;
    }

    /// <summary>
    /// 忘记密码（发送密码重置邮件）
    /// </summary>
    /// <param name="dto">忘记密码 DTO</param>
    /// <returns>结果</returns>
    public async Task<TaktForgotPasswordResultDto> ForgotPasswordAsync(TaktForgotPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserNameOrEmail))
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Message = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldUserNameOrEmail),
            };
        }

        if (string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Message = TaktValidationMessageHelper.Build(
                    k => GetLocalizedMessage(k),
                    TaktValidationI18nKeys.TipSelectFirst,
                    extraTokens: new Dictionary<string, string> { ["target"] = GetLocalizedMessage(TaktValidationI18nKeys.EntityTenantSelf) }),
            };
        }

        var keyword = dto.UserNameOrEmail.Trim();
        var user = await _userRepository.FirstAsync(u =>
            u.TenantCode == CurrentTenantCode && u.UserName == keyword);

        if (user == null)
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Code = "EmailNotFound",
                Message = GetValidationMessage(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserSelf),
            };
        }

        if (user.IsBuiltIn == 1)
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Code = "ProtectedUser",
                Message = GetLocalizedMessage(TaktValidationI18nKeys.TipResetPasswordUnavailable),
            };
        }

        // TODO: 发送密码重置邮件
        LogInformation("忘记密码请求已受理: UserId={UserId}, UserName={UserName}", user.Id, user.UserName);

        return new TaktForgotPasswordResultDto { Success = true };
    }

    /// <summary>
    /// 解锁用户
    /// </summary>
    /// <param name="dto">解锁用户 DTO</param>
    /// <returns>用户 DTO</returns>
    public async Task<TaktUserDto> UnlockUserAsync(TaktUserUnlockDto dto)
    {
        if (dto.UserId <= 0)
        {
            ThrowBusinessException("用户ID无效");
        }

        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限解锁此用户");
        }

        entity.LoginFailCount = 0;
        entity.LockedUntil = null;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        var reasonText = string.IsNullOrWhiteSpace(dto.Reason) ? "无" : dto.Reason;
        LogInformation("用户解锁成功: UserId={UserId}, Reason={Reason}", dto.UserId, reasonText);

        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 统计用户总数
    /// </summary>
    /// <returns>用户总数</returns>
    public async Task<long> GetUserCountAsync()
    {
        return await _userRepository.CountAsync(u => u.TenantCode == CurrentTenantCode);
    }

    // ========================================
    // 导入导出方法
    // ========================================

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名（可选）</param>
    /// <param name="fileName">文件名（可选）</param>
    /// <returns>Excel文件信息</returns>
    public async Task<(string fileName, byte[] content)> GetUserTemplateAsync(
        string? sheetName = null, 
        string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktUserTemplateDto>(
            sheetName ?? "用户导入模板",
            fileName ?? "用户导入模板.xlsx");
    }

    /// <summary>
    /// 导入用户
    /// </summary>
    /// <param name="fileStream">Excel流</param>
    /// <param name="sheetName">工作表名称（可选）</param>
    /// <returns>成功数、失败数、错误信息列表</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportUserAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        int success = 0, fail = 0;

        try
        {
            var importData = await TaktExcelHelper.ImportAsync<TaktUserImportDto>(fileStream, sheetName ?? "用户导入模板");

            if (importData == null || importData.Count == 0)
            {
                errors.Add("Excel文件中没有数据");
                return (0, 0, errors);
            }

            var dictSnapshot = await _dictDataService.CreateDictSnapshotAsync("sys_user_type", "sys_yes_no");
            var entitiesToInsert = new List<TaktUser>();

            for (int i = 0; i < importData.Count; i++)
            {
                var row = importData[i];
                int rowNumber = i + 2; // Excel 行号（从2开始，1是表头）

                try
                {
                    // 业务校验
                    if (string.IsNullOrWhiteSpace(row.UserName))
                    {
                        errors.Add($"第{rowNumber}行：用户名不能为空");
                        fail++;
                        continue;
                    }

                    if (row.UserName.Length != 8)
                    {
                        errors.Add($"第{rowNumber}行：用户名必须为8位");
                        fail++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(row.EmployeeCode))
                    {
                        errors.Add($"第{rowNumber}行：员工编码不能为空");
                        fail++;
                        continue;
                    }

                    // 唯一性校验
                    var exists = await _userRepository.FirstAsync(u => 
                        u.TenantCode == CurrentTenantCode && u.UserName == row.UserName);
                    if (exists != null)
                    {
                        errors.Add($"第{rowNumber}行：用户名{row.UserName}已存在");
                        fail++;
                        continue;
                    }

                    // 查找员工
                    var employee = await _employeeRepository.FirstAsync(e => 
                        e.TenantCode == CurrentTenantCode && e.EmployeeCode == row.EmployeeCode);
                    if (employee == null)
                    {
                        errors.Add($"第{rowNumber}行：员工编码{row.EmployeeCode}不存在");
                        fail++;
                        continue;
                    }

                    var plainPassword = string.IsNullOrWhiteSpace(row.PasswordHash)
                        ? "12345678"
                        : row.PasswordHash;
                    try
                    {
                        ValidateNewPassword(plainPassword);
                    }
                    catch (TaktBusinessException ex)
                    {
                        errors.Add($"第{rowNumber}行：{ex.Message}");
                        fail++;
                        continue;
                    }

                    if (!dictSnapshot.TryResolveImportCode("sys_user_type", row.UserType ?? 0, row.UserTypeName, out var userType, out var dictError)
                        || !dictSnapshot.TryResolveImportCode("sys_yes_no", row.UserStatus ?? 0, row.StatusName, out var userStatus, out dictError))
                    {
                        errors.Add($"第{rowNumber}行：{dictError}");
                        fail++;
                        continue;
                    }

                    // 添加到待插入列表
                    var entity = new TaktUser
                    {
                        UserName = row.UserName,
                        NickName = row.NickName,
                        UserType = userType,
                        EmployeeId = row.EmployeeId is > 0 ? row.EmployeeId.Value : employee.Id,
                        UserStatus = userStatus,
                        PasswordHash = TaktEncryptHelper.HashPassword(plainPassword),
                        TenantCode = CurrentTenantCode,
                        CreatedBy = CurrentUserId ?? 0,
                        CreatedAt = DateTime.Now
                    };
                    entitiesToInsert.Add(entity);
                }
                catch (Exception ex)
                {
                    errors.Add($"第{rowNumber}行：{ex.Message}");
                    fail++;
                }
            }

            // 批量插入
            if (entitiesToInsert.Any())
            {
                await _userRepository.CreateRangeAsync(entitiesToInsert);
                success = entitiesToInsert.Count;
            }
        }
        catch (Exception ex)
        {
            errors.Add($"导入过程发生错误：{ex.Message}");
            fail++;
        }

        return (success, fail, errors);
    }

    /// <summary>
    /// 导出用户
    /// </summary>
    /// <param name="query">查询DTO（可为null）</param>
    /// <param name="sheetName">工作表名（可选）</param>
    /// <param name="fileName">文件名（可选）</param>
    /// <returns>Excel文件信息（文件名与内容）</returns>
    public async Task<(string fileName, byte[] content)> ExportUserAsync(
        TaktUserQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktUserQueryDto());

        var list = await _userRepository.GetListForExportAsync(predicate);

        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktUserExportDto>(),
                sheetName ?? "用户数据",
                fileName ?? "用户导出.xlsx");
        }

        var dictSnapshot = await _dictDataService.CreateDictSnapshotAsync("sys_user_type", "sys_yes_no");
        var employeeIds = list
            .Where(u => u.EmployeeId > 0)
            .Select(u => u.EmployeeId)
            .Distinct()
            .ToList();
        var employees = employeeIds.Count > 0
            ? await _employeeRepository.GetListAsync(e => employeeIds.Contains(e.Id))
            : [];
        var employeeNameMap = employees.ToDictionary(e => e.Id, e => e.EmployeeName ?? string.Empty);
        var userIds = list.Select(u => u.Id).ToList();
        var roleNamesMap = await _rbacService.GetUserRoleNamesMapAsync(userIds);
        var exportData = new List<TaktUserExportDto>(list.Count);

        foreach (var user in list)
        {
            var exportDto = user.Adapt<TaktUserExportDto>();
            exportDto.UserTypeName = dictSnapshot.GetName("sys_user_type", user.UserType, string.Empty);
            exportDto.StatusName = dictSnapshot.GetName("sys_yes_no", user.UserStatus, string.Empty);
            if (user.EmployeeId > 0 && employeeNameMap.TryGetValue(user.EmployeeId, out var employeeName))
            {
                exportDto.EmployeeName = employeeName;
            }
            if (roleNamesMap.TryGetValue(user.Id, out var roleNames))
            {
                exportDto.RoleNames = roleNames;
            }
            exportData.Add(exportDto);
        }

        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用户数据",
            fileName ?? "用户导出.xlsx");
    }

    /// <summary>
    /// 校验新密码是否满足 PasswordPolicy 配置
    /// </summary>
    /// <param name="password">明文密码</param>
    private void ValidateNewPassword(string password)
    {
        if (!TaktPasswordPolicyHelper.TryValidate(password, _passwordPolicyOptions, out var i18nKey))
        {
            if (i18nKey == TaktValidationI18nKeys.Required)
            {
                ThrowValidationLocalized(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.EntityUserPassword);
            }

            ThrowBusinessExceptionLocalized(i18nKey);
        }
    }

    // ========================================
    // DTO 填充辅助方法
    // ========================================

    /// <summary>
    /// 填充用户 DTO 关联数据（员工信息、角色信息）
    /// </summary>
    /// <param name="dto">用户DTO</param>
    /// <param name="user">用户实体</param>
    /// <returns>任务</returns>
    private async Task FillUserDtoAsync(TaktUserDto dto, TaktUser user)
    {
        // 填充员工信息
        if (user.EmployeeId > 0)
        {
            var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);
            if (employee != null)
            {
                dto.EmployeeName = employee.EmployeeName;
            }
        }

        var userRoles = await _rbacService.GetUserRoleIdsAsync(user.Id);
        if (userRoles.Count > 0)
        {
            dto.RoleIds = userRoles.Select(ur => ur.RoleId).ToArray();
            dto.RoleNames = userRoles
                .Select(ur => ur.RoleName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .ToArray();
        }

        var userCompanies = await _rbacService.GetUserCompanyIdsAsync(user.Id);
        if (userCompanies.Count > 0)
        {
            dto.CompanyCodes = userCompanies
                .Select(uc => uc.CompanyCode)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code!)
                .ToArray();
        }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建用户查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktUser, bool>> QueryExpression(TaktUserQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktUser>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.UserName != null && x.UserName.Contains(keywords))
                || (x.NickName != null && x.NickName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || SqlFunc.ToString(x.UserType).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.UserStatus).Contains(keywords)
                || SqlFunc.ToString(x.CreatedBy).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.NickName))
        {
            exp = exp.And(x => x.NickName != null && x.NickName.Contains(queryDto.NickName));
        }

        if (queryDto?.UserType.HasValue == true)
        {
            exp = exp.And(x => x.UserType == queryDto.UserType);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.UserStatus.HasValue == true)
        {
            exp = exp.And(x => x.UserStatus == queryDto.UserStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (queryDto?.CreatedBy.HasValue == true)
        {
            exp = exp.And(x => x.CreatedBy == queryDto.CreatedBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
