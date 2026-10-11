// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：设变明细实体（技术阶段一 ③）；BOM/料号变更行，与主表/附件保存后自动生成通知并初始化各部门执行行
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细实体（技术阶段一 ③，隶属 TaktEcGijutsu，外键 EcGijutsuId）。技术维护 BOM/料号变更行；存在明细时保存主表后系统自动生成 TaktEcNotification。
/// 数据主从：本表为主，各部门执行表为子（子表外键 EcDetailId → 本表 Id）。视图侧「执行→明细」按业务键手工查询填充 DTO，不在实体上伪造反向 OneToMany。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_detail", "设变明细表")]
[SugarIndex("ix_ec_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcGijutsuId), OrderByType.Asc, nameof(EcRootMaterialCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_detail_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM行号
    /// </summary>
    [SugarColumn(ColumnName = "ec_bom_line_code", ColumnDescription = "BOM行号", Length = 8, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcBomLineCode { get; set; }

    /// <summary>
    /// 机种编码（按根物料编码 EcRootMaterialCode 查询型号目的地回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code", ColumnDescription = "机种编码", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 根物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_code", ColumnDescription = "根物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 根物料描述（按根物料编码 EcRootMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_root_material_description", ColumnDescription = "根物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcRootMaterialDescription { get; set; }

    /// <summary>
    /// 停产状态（按根物料编码 EcRootMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "Z0")]
    public string DiscontinuedStatus { get; set; } = "Z0";

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_code", ColumnDescription = "上阶物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcParentMaterialCode { get; set; }

    /// <summary>
    /// 上阶物料描述（按上阶物料编码 EcParentMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_parent_material_description", ColumnDescription = "上阶物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcParentMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_material_code", ColumnDescription = "旧物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldMaterialCode { get; set; }

    /// <summary>
    /// 旧物料描述
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_material_description", ColumnDescription = "旧物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldMaterialDescription { get; set; }

    /// <summary>
    /// 旧用量
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_usage_quantity", ColumnDescription = "旧用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧位置
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_item_position", ColumnDescription = "旧位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldItemPosition { get; set; }

    /// <summary>
    /// 旧品在库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_stock", ColumnDescription = "旧品在库", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcOldStock { get; set; }

    /// <summary>
    /// 旧品仓库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_warehouse", ColumnDescription = "旧品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldWarehouse { get; set; }

    /// <summary>
    /// 旧采购类型（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_purchase_type", ColumnDescription = "旧采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EcOldPurchaseType { get; set; }

    /// <summary>
    /// 旧品检验（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_requires_inspection", ColumnDescription = "旧品检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcOldRequiresInspection { get; set; } = 0;

    /// <summary>
    /// 新物料编码
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_code", ColumnDescription = "新物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewMaterialCode { get; set; }

    /// <summary>
    /// 新物料描述
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_material_description", ColumnDescription = "新物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewMaterialDescription { get; set; }

    /// <summary>
    /// 新用量
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_usage_quantity", ColumnDescription = "新用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewUsageQuantity { get; set; }

    /// <summary>
    /// 新位置
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_item_position", ColumnDescription = "新位置", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewItemPosition { get; set; }

    /// <summary>
    /// 新品在库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_stock", ColumnDescription = "新品在库", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = true)]
    public decimal? EcNewStock { get; set; }

    /// <summary>
    /// 新品仓库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_warehouse", ColumnDescription = "新品仓库", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNewWarehouse { get; set; }

    /// <summary>
    /// 新采购类型（按新物料编码 EcNewMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_purchase_type", ColumnDescription = "新采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EcNewPurchaseType { get; set; }

    /// <summary>
    /// 新品检验（按新物料编码 EcNewMaterialCode 查询工厂物料回填）
    /// </summary>
    [SugarColumn(ColumnName = "ec_new_requires_inspection", ColumnDescription = "新品检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcNewRequiresInspection { get; set; } = 0;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_bomdate", ColumnDescription = "BOM生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcBomDate { get; set; }

    /// <summary>
    /// 兼容性
    /// </summary>
    [SugarColumn(ColumnName = "ec_is_compatible", ColumnDescription = "兼容性", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcIsCompatible { get; set; }

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    [SugarColumn(ColumnName = "ec_2nd_vendor", ColumnDescription = "第二供应商", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? Ec2ndVendor { get; set; }

    /// <summary>
    /// 生产指令
    /// </summary>
    [SugarColumn(ColumnName = "ec_instruction", ColumnDescription = "生产指令", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcInstruction { get; set; }

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "ec_old_part_disposition", ColumnDescription = "旧品处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcOldPartDisposition { get; set; }

    /// <summary>
    /// 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
    /// </summary>
    [SugarColumn(ColumnName = "ec_scope", ColumnDescription = "实施范围", ColumnDataType = "int", IsNullable = false)]
    public int EcScope { get; set; }
    /// <summary>
    /// 制二/制一/品管实施范围（冗余自主表；字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课的选择，执行行再按部门固定规则解析
    /// </summary>
    [SugarColumn(ColumnName = "ec_dept_scope", ColumnDescription = "部门范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 技术课主表 ID（TaktEcGijutsu 主键；序列化为 string 避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_gijutsu_id", ColumnDescription = "技术课主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 是否作废
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 生管执行行列表（数据主从；一对多；子表外键 TaktEcSeikan.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcSeikan.EcDetailId))]
    public List<TaktEcSeikan>? EcSeikans { get; set; }

    /// <summary>
    /// 采购执行行列表（数据主从；一对多；子表外键 TaktEcKoubai.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcKoubai.EcDetailId))]
    public List<TaktEcKoubai>? EcKoubais { get; set; }

    /// <summary>
    /// 受检执行行列表（数据主从；一对多；子表外键 TaktEcUkeken.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcUkeken.EcDetailId))]
    public List<TaktEcUkeken>? EcUkekens { get; set; }

    /// <summary>
    /// 部管执行行列表（数据主从；一对多；子表外键 TaktEcBukan.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcBukan.EcDetailId))]
    public List<TaktEcBukan>? EcBukans { get; set; }

    /// <summary>
    /// 制二执行行列表（数据主从；一对多；子表外键 TaktEcSeizounika.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcSeizounika.EcDetailId))]
    public List<TaktEcSeizounika>? EcSeizounikas { get; set; }

    /// <summary>
    /// SMT执行行列表（数据主从；一对多；子表外键 TaktEcSmt.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcSmt.EcDetailId))]
    public List<TaktEcSmt>? EcSmts { get; set; }

    /// <summary>
    /// 制一执行行列表（数据主从；一对多；子表外键 TaktEcSeizouikka.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcSeizouikka.EcDetailId))]
    public List<TaktEcSeizouikka>? EcSeizouikkas { get; set; }

    /// <summary>
    /// 品管执行行列表（数据主从；一对多；子表外键 TaktEcHinkan.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcHinkan.EcDetailId))]
    public List<TaktEcHinkan>? EcHinkans { get; set; }

    /// <summary>
    /// 制技执行行列表（数据主从；一对多；子表外键 TaktEcSeizougijutsu.EcDetailId → 本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcSeizougijutsu.EcDetailId))]
    public List<TaktEcSeizougijutsu>? EcSeizougijutsus { get; set; }
}
