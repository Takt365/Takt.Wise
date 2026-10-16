// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Benefits
// 文件名称：TaktBenefitItem.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：福利项目主数据；类型由字典 humanresource_benefits_benefit_type 区分（社保/公积金/商保/年假额度/餐补/培训补贴/折扣等，不另建多种福利实体）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Benefits;

/// <summary>
/// 福利项目（非直接现金福利主数据；年假请假走考勤模块，培训实施走培训模块，此处仅配置福利项）
/// </summary>
[SugarTable("takt_human_resource_benefit_item", "福利项目表")]
[SugarIndex("ix_benefit_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_benefit_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_benefit_item_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ItemCode), OrderByType.Asc, true)]
public class TaktBenefitItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 福利项目编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 福利项目编码规则生成并展示，非手输；单据类型菜单：福利项目）
    /// </summary>
    [SugarColumn(ColumnName = "item_code", ColumnDescription = "福利项目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 福利项目名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "福利项目名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category；1=保险 2=补贴 3=休假 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "benefit_category", ColumnDescription = "福利大类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BenefitCategory { get; set; } = 0;
    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type；1=社保 2=公积金 3=商业保险 4=年假额度 5=餐补 6=培训补贴 7=员工折扣）
    /// </summary>
    [SugarColumn(ColumnName = "benefit_type", ColumnDescription = "福利类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BenefitType { get; set; } = 0;
    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle；1=月度 2=季度 3=年度 4=一次性）
    /// </summary>
    [SugarColumn(ColumnName = "payment_cycle", ColumnDescription = "发放周期", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentCycle { get; set; } = 0;
    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    [SugarColumn(ColumnName = "default_amount", ColumnDescription = "默认金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DefaultAmount { get; set; }
    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    [SugarColumn(ColumnName = "max_amount", ColumnDescription = "金额上限", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaxAmount { get; set; }
    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    [SugarColumn(ColumnName = "employer_ratio", ColumnDescription = "公司承担比例", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal EmployerRatio { get; set; }
    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    [SugarColumn(ColumnName = "employee_ratio", ColumnDescription = "个人承担比例", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal EmployeeRatio { get; set; }
    /// <summary>
    /// 是否强制福利（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_mandatory", ColumnDescription = "是否强制福利", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsMandatory { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "item_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ItemStatus { get; set; } = 1;
}
