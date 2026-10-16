// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDocument.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料凭证主表实体（行项目见 TaktMaterialDocumentItem）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料凭证主表实体（公司级）
/// </summary>
[SugarTable("takt_logistics_materials_material_document", "物料凭证表")]
[SugarIndex("ix_material_document_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_document_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_doc_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentYear), OrderByType.Asc, nameof(MaterialDocumentCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_document_posting_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PostingDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_document_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentType), OrderByType.Asc, false)]
public class TaktMaterialDocument : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料凭证（前端表单选择编码规则后自动通过 TaktNumbering 物料凭证规则生成并展示，非手输；单据类型菜单：物料凭证）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_code", ColumnDescription = "物料凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证的年份
    /// </summary>
    [SugarColumn(ColumnName = "material_document_year", ColumnDescription = "物料凭证的年份", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string MaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 交易/事件类型（字典 logistics_material_document_transaction_event_type）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_event_type", ColumnDescription = "交易/事件类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TransactionEventType { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_material_document_type）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "凭证类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DocumentType { get; set; }

    /// <summary>
    /// 凭证类型重新评估
    /// </summary>
    [SugarColumn(ColumnName = "revaluation_type", ColumnDescription = "凭证类型重新评估", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RevaluationType { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    [SugarColumn(ColumnName = "document_date", ColumnDescription = "凭证日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    [SugarColumn(ColumnName = "posting_date", ColumnDescription = "过帐日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 参照（最长 16，故 Length=16）
    /// </summary>
    [SugarColumn(ColumnName = "reference_code", ColumnDescription = "参照", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 凭证抬头文本（最长 25，故 Length=25）
    /// </summary>
    [SugarColumn(ColumnName = "header_text", ColumnDescription = "凭证抬头文本", ColumnDataType = "nvarchar", Length = 25, IsNullable = true)]
    public string? HeaderText { get; set; }

    /// <summary>
    /// 提货单（最长 16，故 Length=16）
    /// </summary>
    [SugarColumn(ColumnName = "bill_of_lading_code", ColumnDescription = "提货单", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? BillOfLadingCode { get; set; }

    /// <summary>
    /// 交货单
    /// </summary>
    [SugarColumn(ColumnName = "delivery_code", ColumnDescription = "交货单", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? DeliveryCode { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    [SugarColumn(ColumnName = "transaction_code", ColumnDescription = "事务代码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? TransactionCode { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaterialDocumentItem.MaterialDocumentId))]
    public List<TaktMaterialDocumentItem>? Items { get; set; }
}
