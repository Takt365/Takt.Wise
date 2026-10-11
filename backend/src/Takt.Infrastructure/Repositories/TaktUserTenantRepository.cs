// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktUserTenantRepository.cs
// 创建时间：2026-09-15
// 创建人：Takt365(Cursor AI)
// 功能描述：用户-租户关联仓储（行内 TenantCode 为可访问租户；不按会话租户/公司过滤）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 用户-租户关联仓储（TenantCode 为关联目标租户；CompanyCode 为公司隔离；查询由业务显式指定）
/// </summary>
public class TaktUserTenantRepository : TaktCompanyRepository<TaktUserTenant>
{
    /// <summary>
    /// 关联表不按当前会话租户过滤（行内 TenantCode 为可访问租户）
    /// </summary>
    protected override bool UseSessionTenantFilter => false;

    /// <summary>
    /// 关联表不按当前会话公司过滤（由业务按 CompanyCode 显式隔离）
    /// </summary>
    protected override bool UseSessionCompanyFilter => false;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    /// <param name="excelOptions">Excel 导入导出配置</param>
    /// <param name="databaseOptions">Database 配置</param>
    public TaktUserTenantRepository(
        TaktSqlSugarContext dbContext,
        ITaktUserContext userContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions,
        Microsoft.Extensions.Options.IOptions<TaktExcelOptions> excelOptions,
        Microsoft.Extensions.Options.IOptions<TaktDatabaseOptions> databaseOptions)
        : base(dbContext, userContext, primaryKeyTypeOptions, excelOptions, databaseOptions)
    {
    }
}
