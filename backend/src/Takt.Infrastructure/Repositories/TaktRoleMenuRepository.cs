// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktRoleMenuRepository.cs
// 创建时间：2026-09-15
// 创建人：Takt365(Cursor AI)
// 功能描述：角色-菜单关联仓储（不按会话租户/公司过滤；由业务查询显式指定）
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
/// 角色-菜单关联仓储（TenantCode/CompanyCode 由业务查询显式指定，便于登录鉴权按公司解析）
/// </summary>
public class TaktRoleMenuRepository : TaktCompanyRepository<TaktRoleMenu>
{
    /// <summary>
    /// 关联表不按当前会话租户过滤
    /// </summary>
    protected override bool UseSessionTenantFilter => false;

    /// <summary>
    /// 关联表不按当前会话公司过滤
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
    public TaktRoleMenuRepository(
        TaktSqlSugarContext dbContext,
        ITaktUserContext userContext,
        Microsoft.Extensions.Options.IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions,
        Microsoft.Extensions.Options.IOptions<TaktExcelOptions> excelOptions,
        Microsoft.Extensions.Options.IOptions<TaktDatabaseOptions> databaseOptions)
        : base(dbContext, userContext, primaryKeyTypeOptions, excelOptions, databaseOptions)
    {
    }
}
