// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeAddress.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：员工地址子表（家庭/工作/常住；主子表关系，外键 EmployeeId）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工地址（主档子表；同一员工每种地址类型至多一条）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_address", "员工地址表")]
[SugarIndex("ix_employee_address_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_address_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_address_type_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(AddressType), OrderByType.Asc, true)]
public class TaktEmployeeAddress : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string EmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    [SugarColumn(ColumnName = "address_type", ColumnDescription = "地址类型", ColumnDataType = "int", IsNullable = false)]
    public int AddressType { get; set; }
    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country", ColumnDescription = "国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string Country { get; set; } = string.Empty;
    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "province", ColumnDescription = "省", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string Province { get; set; } = string.Empty;
    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "city", ColumnDescription = "市", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    [SugarColumn(ColumnName = "district", ColumnDescription = "区县", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string District { get; set; } = string.Empty;
    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    [SugarColumn(ColumnName = "address1", ColumnDescription = "地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string Address1 { get; set; } = string.Empty;
    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    [SugarColumn(ColumnName = "address2", ColumnDescription = "地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Address2 { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}
