// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKoubai.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变采购课部门执行表（部门 D0510；采购类型 F 且新物料有效才落行。任何范围按设变单号+新物料去重，机种与根物料写入编码组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变采购课（部门 D0510）执行表。采购类型 F 且新物料有效才落行。任何范围去重键为设变单号+新物料，机种与根物料写入编码组
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_koubai", "设变采购执行表")]
[SugarIndex("ix_ec_koubai_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_koubai_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcCode), OrderByType.Asc, nameof(EcNewPurchaseType), OrderByType.Asc, nameof(EcNewMaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_koubai_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcKoubai : TaktCompanyEntityBase, ITaktEcDeptExecEntity
{
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=Z0/ZQ 等）。同设变单号+新采购类型+新物料下任意成品不是停产（空或 Z0）时为 Z0，全部停产时取保留明细状态。
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码组（同设变单号+新采购类型+新物料下的机种编码，分号分隔；ntext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code_group", ColumnDescription = "机种编码组", ColumnDataType = "ntext", IsNullable = false)]
    public string EcModelCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_code", ColumnDescription = "新物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewMaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 根物料编码组（同设变单号+新采购类型+新物料下的根物料编码，分号分隔；ntext）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_code_group", ColumnDescription = "根物料编码组", ColumnDataType = "ntext", IsNullable = false)]
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;

    /// <summary>
    /// 根物料描述（仅实施范围=全仕向且部门实施范围=按成品时取明细描述；机种组或物料组时为空）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_description", ColumnDescription = "根物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcRootMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_description", ColumnDescription = "新物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 新品仓库（冗余：来自 TaktEcDetail.EcNewWarehouse）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_warehouse", ColumnDescription = "新品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNewWarehouse { get; set; } = string.Empty;

    /// <summary>
    /// 新采购类型（冗余：来自 TaktEcDetail.EcNewPurchaseType）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_purchase_type", ColumnDescription = "新采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string EcNewPurchaseType { get; set; } = string.Empty;


    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_implemented", ColumnDescription = "实施", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    [SugarColumn(ColumnName = "exec_content", ColumnDescription = "执行内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ExecContent { get; set; }

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_issue_date", ColumnDescription = "采购订单发行日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    [SugarColumn(ColumnName = "supplier", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Supplier { get; set; }

    /// <summary>
    /// 采购订单号码
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单号码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// 旧品处理（冗余：来自 TaktEcDetail.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_part_disposition", ColumnDescription = "旧品处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldPartDisposition { get; set; }


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
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false)]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcDetailId))]
    public TaktEcDetail? EcDetail { get; set; }
}
