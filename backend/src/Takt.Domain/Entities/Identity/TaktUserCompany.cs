// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktUserCompany.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户公司关联实体，支持用户跨公司访问（多对多关系）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Accounting.Financial;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 用户公司关联实体
/// 支持用户跨公司访问（多对多关系）；演示种子为所有启用用户关联全部公司
/// </summary>
[SugarTable("takt_identity_user_company", "用户公司关联表")]
[SugarIndex("ix_user_company_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_user_company_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_user_company_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_user_company_user_default", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, nameof(IsDefault), OrderByType.Asc, false)]
public class TaktUserCompany : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    public long UserId { get; set; }

    /// <summary>
    /// 是否默认登录公司（字典 sys_yes_no；1=是 0=否）
    /// 同一用户在同一租户下仅应有一条为 1；登录时由 TaktAuthService 按 IsDefault=1 解析默认公司
    /// 演示种子 TaktUserCompanySeedData 为所有用户关联全部公司，默认登录公司为 <c>2300</c>
    /// </summary>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "默认公司", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDefault { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用户（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(UserId), nameof(TaktUser.Id))]
    public TaktUser User { get; set; } = null!;

    /// <summary>
    /// 可访问公司（多对一，按 CompanyCode 关联）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(CompanyCode), nameof(TaktCompany.CompanyCode))]
    public TaktCompany Company { get; set; } = null!;
}
