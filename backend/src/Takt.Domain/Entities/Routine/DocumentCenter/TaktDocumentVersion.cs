// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.DocumentCenter
// 文件名称：TaktDocumentVersion.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档版本子实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.DocumentCenter;

/// <summary>
/// 文管文档版本子实体
/// </summary>
[SugarTable("takt_routine_document_center_version", "文管文档版本表")]
[SugarIndex("ix_document_version_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_document_version_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_document_version_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentId), OrderByType.Asc, nameof(VersionNo), OrderByType.Asc, true)]
[SugarIndex("ix_document_version_document_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocumentId), OrderByType.Asc, false)]
public class TaktDocumentVersion : TaktCompanyEntityBase
{
    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "document_id", ColumnDescription = "文档ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }
    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;
    /// <summary>
    /// 版本号
    /// </summary>
    [SugarColumn(ColumnName = "version_no", ColumnDescription = "版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VersionNo { get; set; } = 0;
    /// <summary>
    /// 版本说明
    /// </summary>
    [SugarColumn(ColumnName = "version_note", ColumnDescription = "版本说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? VersionNote { get; set; }
    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
    /// </summary>
    [SugarColumn(ColumnName = "file_id", ColumnDescription = "文件ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }
    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "revised_by", ColumnDescription = "修订人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisedBy { get; set; }
    /// <summary>
    /// 修订人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "revised_by_name", ColumnDescription = "修订人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? RevisedByName { get; set; }
    /// <summary>
    /// 修订时间
    /// </summary>
    [SugarColumn(ColumnName = "revised_at", ColumnDescription = "修订时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime RevisedAt { get; set; } = DateTime.Now;
    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 文档（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DocumentId))]
    public TaktDocument? Document { get; set; }
}
