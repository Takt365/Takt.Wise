// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMe.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造技术课部门执行表（部门 D0630；未作废明细均落行。任何范围按设变单号+机种去重，根物料写入编码组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制造技术课（部门 D0630）执行表。未作废明细均落行。任何范围去重键为设变单号+机种，根物料写入编码组
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_me", "设变制技执行表")]
[SugarIndex("ix_ec_me_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_me_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_me_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcMe : TaktCompanyEntityBase, ITaktEcDeptExecEntity
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
    /// 机种编码组（单个或多个机种，分号分隔；ntext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code_group", ColumnDescription = "机种编码组", ColumnDataType = "ntext", IsNullable = false)]
    public string EcModelCodeGroup { get; set; } = string.Empty;
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
    /// 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int IsImplemented { get; set; }
    /// <summary>
    /// 执行内容（制技不落库；仅满足 ITaktEcDeptExecEntity）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string? ExecContent { get; set; }
    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
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
    /// 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
    /// </summary>
    [SugarColumn(ColumnName = "ec_impl_scope", ColumnDescription = "实施范围", ColumnDataType = "int", IsNullable = false)]
    public int EcImplScope { get; set; }
    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
    /// </summary>
    [SugarColumn(ColumnName = "ec_control_scope", ColumnDescription = "控制范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcControlScope { get; set; } = 2;

    /// <summary>
    /// 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_grain_key", ColumnDescription = "执行去重键", ColumnDataType = "ntext", IsNullable = false)]
    public string EcGrainKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
