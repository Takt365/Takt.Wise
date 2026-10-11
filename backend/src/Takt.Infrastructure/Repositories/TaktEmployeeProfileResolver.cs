// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktEmployeeProfileResolver.cs
// 创建时间：2026-09-12
// 创建人：Takt365(Cursor AI)
// 功能描述：员工档案跨公司解析（按 EmployeeId → EmployeeCode → 当前公司同行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 员工档案资料解析实现（不按会话公司过滤，显式 TenantCode/CompanyCode）
/// </summary>
public class TaktEmployeeProfileResolver : ITaktEmployeeProfileResolver
{
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">SqlSugar 上下文</param>
    public TaktEmployeeProfileResolver(TaktSqlSugarContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<TaktEmployee?> ResolveForCompanyAsync(long employeeId, string tenantCode, string companyCode)
    {
        if (employeeId <= 0
            || string.IsNullOrWhiteSpace(tenantCode)
            || string.IsNullOrWhiteSpace(companyCode))
        {
            return null;
        }

        var tenant = tenantCode.Trim();
        var company = companyCode.Trim();
        var linked = await _dbContext.Db.Queryable<TaktEmployee>()
            .Where(e => e.Id == employeeId && e.TenantCode == tenant && e.IsDeleted == 0)
            .FirstAsync();
        if (linked == null)
        {
            return null;
        }

        if (string.Equals(linked.CompanyCode, company, StringComparison.OrdinalIgnoreCase))
        {
            return linked;
        }

        var employeeCode = (linked.EmployeeCode ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(employeeCode))
        {
            return null;
        }

        return await _dbContext.Db.Queryable<TaktEmployee>()
            .Where(e =>
                e.TenantCode == tenant
                && e.CompanyCode == company
                && e.EmployeeCode == employeeCode
                && e.IsDeleted == 0)
            .FirstAsync();
    }
}
