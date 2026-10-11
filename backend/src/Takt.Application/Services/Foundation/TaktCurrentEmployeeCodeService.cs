// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktCurrentEmployeeCodeService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：按当前登录用户解析 TaktUser.EmployeeId 对应 TaktEmployee.EmployeeCode
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 当前登录用户员工编码解析服务
/// </summary>
public class TaktCurrentEmployeeCodeService : ITaktCurrentEmployeeCodeService
{
    private readonly ITaktUserContext _userContext;
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userContext">用户上下文</param>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    public TaktCurrentEmployeeCodeService(
        ITaktUserContext userContext,
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// 获取当前登录用户对应的 EmployeeCode；未登录或未关联员工时返回 null
    /// </summary>
    /// <returns>员工编码</returns>
    public async Task<string?> GetCurrentEmployeeCodeAsync()
    {
        var userId = _userContext.UserId;
        if (!_userContext.IsAuthenticated || userId is not > 0)
        {
            return null;
        }

        var tenantCode = _userContext.TenantCode?.Trim();
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user == null || !string.Equals(user.TenantCode, tenantCode, StringComparison.Ordinal) || user.EmployeeId <= 0)
        {
            return null;
        }

        var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);
        if (employee == null || !string.Equals(employee.TenantCode, tenantCode, StringComparison.Ordinal))
        {
            return null;
        }

        var employeeCode = employee.EmployeeCode?.Trim();
        return string.IsNullOrWhiteSpace(employeeCode) ? null : employeeCode;
    }
}
