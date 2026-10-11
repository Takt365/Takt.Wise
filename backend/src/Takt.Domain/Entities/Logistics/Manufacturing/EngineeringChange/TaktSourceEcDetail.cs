// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetail.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源子表实体，存储旧物料/新物料、用量、安装位置、BOM与实施日期等信息。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源子表
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_source_detail", "设变来源子表")]
[SugarIndex("ix_ec_source_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_source_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_source_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceEcId), OrderByType.Asc, nameof(SourceRootMaterialCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_ec_source_detail_ecid", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceEcId), OrderByType.Asc, false)]
[SugarIndex("ix_ec_source_detail_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSourceEcDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 主ID（选项 TaktSourceEcs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_id", ColumnDescription = "主ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变号码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_code", ColumnDescription = "设变号码", Length = 6, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 根物料编码
    /// </summary>
    [SugarColumn(ColumnName = "source_root_material_code", ColumnDescription = "根物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料编码
    /// </summary>
    [SugarColumn(ColumnName = "source_parent_material_code", ColumnDescription = "上阶物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料编码
    /// </summary>
    [SugarColumn(ColumnName = "source_old_material_code", ColumnDescription = "旧物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceOldMaterialCode { get; set; }

    /// <summary>
    /// 旧物料描述
    /// </summary>
    [SugarColumn(ColumnName = "source_old_material_description", ColumnDescription = "旧物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceOldMaterialDescription { get; set; }

    /// <summary>
    /// 旧物料用量
    /// </summary>
    [SugarColumn(ColumnName = "source_old_usage_quantity", ColumnDescription = "旧物料用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = true)]
    public decimal? SourceOldUsageQuantity { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    [SugarColumn(ColumnName = "source_old_item_position", ColumnDescription = "旧物料安装位置", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceOldItemPosition { get; set; }

    /// <summary>
    /// 新物料编码
    /// </summary>
    [SugarColumn(ColumnName = "source_new_material_code", ColumnDescription = "新物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceNewMaterialCode { get; set; }

    /// <summary>
    /// 新物料描述
    /// </summary>
    [SugarColumn(ColumnName = "source_new_material_description", ColumnDescription = "新物料描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceNewMaterialDescription { get; set; }

    /// <summary>
    /// 新物料用量
    /// </summary>
    [SugarColumn(ColumnName = "source_new_usage_quantity", ColumnDescription = "新物料用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = true)]
    public decimal? SourceNewUsageQuantity { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    [SugarColumn(ColumnName = "source_new_item_position", ColumnDescription = "新物料安装位置", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceNewItemPosition { get; set; }

    /// <summary>
    /// BOM番号
    /// </summary>
    [SugarColumn(ColumnName = "source_bom_code", ColumnDescription = "BOM番号", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceBomCode { get; set; }

    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    [SugarColumn(ColumnName = "source_compatibility", ColumnDescription = "兼容性", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceCompatibility { get; set; }

    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    [SugarColumn(ColumnName = "source_2nd_vendor", ColumnDescription = "第二供应商", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? Source2ndVendor { get; set; }

    /// <summary>
    /// 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "source_instruction", ColumnDescription = "安排指示", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceInstruction { get; set; }

    /// <summary>
    /// 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "source_old_part_disposition", ColumnDescription = "旧物料处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceOldPartDisposition { get; set; }

    /// <summary>
    /// BOM生效日期
    /// </summary>
    [SugarColumn(ColumnName = "source_bom_effective_date", ColumnDescription = "BOM生效日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 设变来源主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SourceEcId))]
    public TaktSourceEc? SourceEc { get; set; }
}
