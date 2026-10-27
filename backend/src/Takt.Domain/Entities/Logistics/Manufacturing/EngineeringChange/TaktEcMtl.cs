// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMtl.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变物料执行表（部门 D0625；采购类型 F 且仓库 C003、新物料有效才落行。去重键为设变单号+机种+新物料，行号不参与；根物料为该机种下成品编码组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变物料（部门 D0625）执行表。采购类型 F 且仓库 C003、新物料有效才落行。去重键为设变单号+机种+新物料，行号不参与，根物料为该机种下成品编码组
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_mtl", "设变物料执行表")]
[SugarIndex("ix_ec_mtl_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_mtl_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcCode), OrderByType.Asc, nameof(EcNewMaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_mtl_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcMtl : TaktCompanyEntityBase, ITaktEcDeptExecEntity
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
    /// 根物料编码组（同去重键下可见明细的根物料编码，分号分隔；ntext，同机种根物料可能很多）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_code_group", ColumnDescription = "根物料编码组", ColumnDataType = "ntext", IsNullable = false)]
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_description", ColumnDescription = "根物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_code", ColumnDescription = "上阶物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_description", ColumnDescription = "上阶物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_code", ColumnDescription = "新物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_description", ColumnDescription = "新物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_purchase_type", ColumnDescription = "新采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string EcNewPurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_warehouse", ColumnDescription = "新品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_implemented", ColumnDescription = "实施", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsImplemented { get; set; } = 1;
    /// <summary>
    /// 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_date", ColumnDescription = "预定日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_batch", ColumnDescription = "预定批次", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    [SugarColumn(ColumnName = "exec_content", ColumnDescription = "执行内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ExecContent { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    [SugarColumn(ColumnName = "outbound_batch", ColumnDescription = "出库批次", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? OutboundBatch { get; set; }

    /// <summary>
    /// 出库日期
    /// </summary>
    [SugarColumn(ColumnName = "outbound_date", ColumnDescription = "出库日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 部门编码（固定 D0625 物料）
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
