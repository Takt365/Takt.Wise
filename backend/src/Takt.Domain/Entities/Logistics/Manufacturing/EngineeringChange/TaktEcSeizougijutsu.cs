// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsu.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造技术课部门执行表（每设变明细一行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制造技术课（D0630）部门执行表
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_seizougijutsu", "设变制技执行表")]
[SugarIndex("ix_ec_seizougijutsu_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_seizougijutsu_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcDetailId), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_seizougijutsu_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcSeizougijutsu : TaktCompanyEntityBase, ITaktEcDeptExecEntity, ITaktEcExecModelRootMaterialRow
{
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 机种编码（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code", ColumnDescription = "机种编码", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_code_group", ColumnDescription = "根物料编码组", ColumnDataType = "ntext", IsNullable = false)]
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_sop_updated", ColumnDescription = "更新SOP", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsSopUpdated { get; set; } = 0;
    /// <summary>
    /// SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    [SugarColumn(ColumnName = "sop_leader", ColumnDescription = "SOP担当", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? SopLeader { get; set; }
    /// <summary>
    /// SOP日期
    /// </summary>
    [SugarColumn(ColumnName = "sop_date", ColumnDescription = "SOP日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? SopDate { get; set; }
    /// <summary>
    /// 设变EC担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    [SugarColumn(ColumnName = "tech_leader", ColumnDescription = "设变EC担当", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? TechLeader { get; set; }
    /// <summary>
    /// 是否实施（制技不落库；仅满足 ITaktEcDeptExecEntity；有输入判定见 TaktEcDeptEntityHelper.HasDeptInput）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int IsImplemented { get; set; }
    /// <summary>
    /// 执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string? ExecContent { get; set; }
    /// <summary>
    /// 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_description", ColumnDescription = "根物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcDetail.EcScope）
    /// </summary>
    [SugarColumn(ColumnName = "ec_scope", ColumnDescription = "实施范围", ColumnDataType = "int", IsNullable = false)]
    public int EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    [SugarColumn(ColumnName = "ec_dept_scope", ColumnDescription = "部门实施范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [SugarColumn(ColumnName = "ec_detail_id", ColumnDescription = "设变明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcDetailId))]
    public TaktEcDetail? EcDetail { get; set; }
}
