// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktNumbering.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则实体，定义各类业务单据的编码生成规则
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 编码规则实体
/// 定义系统中各类业务单据的编码生成规则，如：订单号、合同号、发票号等
/// 支持灵活的前缀、日期格式、流水号组合
/// 
/// 编码顺序：单据类型-公司-部门-前缀-日期-流水号
/// 示例：order-1000-DEPT01-SO-20250120-000001
/// </summary>
[SugarTable("takt_foundation_numbering", "编码规则表")]
[SugarIndex("ix_numbering_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_numbering_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_numbering_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RuleCode), OrderByType.Asc, true)]
public class TaktNumbering : TaktCompanyEntityBase
{    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    [SugarColumn(ColumnName = "rule_code", ColumnDescription = "规则编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string RuleCode { get; set; } = string.Empty;
    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    [SugarColumn(ColumnName = "rule_name", ColumnDescription = "规则名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string RuleName { get; set; } = string.Empty;
    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name；存菜单名称非 Id）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "单据类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string DocumentType { get; set; } = string.Empty;
    /// <summary>
    /// 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options；与部门简称一致，长度 6）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN）
    /// </summary>
    [SugarColumn(ColumnName = "prefix_code", ColumnDescription = "前缀编码", ColumnDataType = "varchar", Length = 4, IsNullable = true)]
    public string? PrefixCode { get; set; }
    /// <summary>
    /// 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
    /// </summary>
    [SugarColumn(ColumnName = "date_format", ColumnDescription = "日期格式", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? DateFormat { get; set; }
    /// <summary>
    /// 流水位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_length", ColumnDescription = "流水位数", ColumnDataType = "int", IsNullable = false, DefaultValue = "6")]
    public int SequenceLength { get; set; } = 6;
    /// <summary>
    /// 流水步长（每次递增的数值，默认1）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_step", ColumnDescription = "流水步长", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SequenceStep { get; set; } = 1;
    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    [SugarColumn(ColumnName = "suffix_code", ColumnDescription = "后缀编码", ColumnDataType = "varchar", Length = 4, IsNullable = true)]
    public string? SuffixCode { get; set; }
    /// <summary>
    /// 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
    /// </summary>
    [SugarColumn(ColumnName = "reset_period", ColumnDescription = "重置周期", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "None")]
    public string ResetPeriod { get; set; } = "None";
    /// <summary>
    /// 当前流水（用于记录下一个流水号值）
    /// </summary>
    [SugarColumn(ColumnName = "current_sequence", ColumnDescription = "当前流水", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CurrentSequence { get; set; } = 0;
    /// <summary>
    /// 起始编码（新增时必填；完整业务编码样例，末段为当前流水号）
    /// 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
    /// </summary>
    [SugarColumn(ColumnName = "example_code", ColumnDescription = "起始编码", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string ExampleCode { get; set; } = string.Empty;
    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    [SugarColumn(ColumnName = "separator", ColumnDescription = "分隔符", ColumnDataType = "varchar", Length = 1, IsNullable = true)]
    public string? Separator { get; set; }
    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
    /// </summary>
    [SugarColumn(ColumnName = "numbering_description", ColumnDescription = "描述说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? NumberingDescription { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "numbering_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int NumberingStatus { get; set; } = 1;
}
