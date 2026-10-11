// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktCountersignDetail.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单明细实体（主子表从表，外键 CountersignId）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 会签单明细实体
/// </summary>
[SugarTable("takt_accounting_financial_countersign_detail", "会签单明细表")]
[SugarIndex("ix_countersign_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CountersignId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_countersign_detail_countersign_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CountersignCode), OrderByType.Asc, false)]
public class TaktCountersignDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 会签单 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_id", ColumnDescription = "会签单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }
    /// <summary>
    /// 会签编码（冗余，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_code", ColumnDescription = "会签编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string CountersignCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; }
    /// <summary>
    /// 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    [SugarColumn(ColumnName = "allocation_category", ColumnDescription = "分配类别", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string AllocationCategory { get; set; } = string.Empty;
    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "account_title", ColumnDescription = "会计科目", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? AccountTitle { get; set; }
    /// <summary>
    /// 明细项名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "明细项名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// 明细项说明
    /// </summary>
    [SugarColumn(ColumnName = "item_description", ColumnDescription = "明细项说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ItemDescription { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnName = "item_quantity", ColumnDescription = "数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ItemQuantity { get; set; }
    /// <summary>
    /// 金额
    /// </summary>
    [SugarColumn(ColumnName = "item_amount", ColumnDescription = "金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ItemAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
