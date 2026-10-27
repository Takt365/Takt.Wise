// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachment.cs
// 功能描述：设变附件实体（技术阶段一 ②）；按租户+公司+设变ID+文件编码+行号唯一；技术维护联络/EPP/FPP 等文档，与主表、明细一并保存后触发通知自动生成
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件实体（技术阶段一 ②，隶属 TaktEcTe；按租户+公司+设变ID + 文件编码 + 行号唯一）。文件类别见字典 logistics_manufacturing_ec_attachment_type；与主表、明细保存后由系统生成 TaktEcNotification。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_attachment", "设变附件表")]
[SugarIndex("ix_ec_attachment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_attachment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcTeId), OrderByType.Asc, nameof(DocCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_attachment_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AttachmentType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_doc_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocCode), OrderByType.Asc, false)]
public class TaktEcAttachment : TaktCompanyEntityBase
{
    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10；与租户、EcTeId、DocCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别（字典 logistics_manufacturing_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    [SugarColumn(ColumnName = "attachment_type", ColumnDescription = "文件类别", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（按 AttachmentType：EC=与设变单号一致；EPP/FPP=P-四位数字；TL=DTS-四位数字；TCJ/EL=四位-四位数字；与租户、EcTeId、LineNumber 组成唯一键；前端表单选择编码规则后自动通过 TaktNumbering 文件编码规则生成并展示，非手输；单据类型菜单：设变附件）
    /// </summary>
    [SugarColumn(ColumnName = "doc_code", ColumnDescription = "文件编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（上传后强制等于文件编码 DocCode + 原扩展名，与源文件名无关；含扩展名故 Length=200）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 技术课主表 ID（TaktEcTe 主键；与租户、DocCode、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "ec_te_id", ColumnDescription = "技术课主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变主表（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcTeId))]
    public TaktEcTe? EcTe { get; set; }
}
