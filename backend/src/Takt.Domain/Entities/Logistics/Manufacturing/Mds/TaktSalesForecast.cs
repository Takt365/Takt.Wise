// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecast.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售预测主表（客户→我方接收；四阶维度+物料；含接收日/版本；明细财年×月）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mds;

/// <summary>
/// Takt销售预测实体（公司级；客户发给我方的销售预测，可进 MDS；同编码多版靠接收版本号）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mds_sales_forecast", "销售预测表")]
[SugarIndex("ix_sales_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesForecastCode), OrderByType.Asc, nameof(ReceiveVersionNo), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_plan_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_receive_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReceiveDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProductCategoryCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_sales_forecast_customer", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
public class TaktSalesForecast : TaktApprovalEntityBase
{

    /// <summary>
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_code", ColumnDescription = "销售预测编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划编制日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    [SugarColumn(ColumnName = "receive_date", ColumnDescription = "接收日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ReceiveDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    [SugarColumn(ColumnName = "receive_version_no", ColumnDescription = "接收版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ReceiveVersionNo { get; set; } = 1;

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    [SugarColumn(ColumnName = "sales_product", ColumnDescription = "产品", ColumnDataType = "nvarchar", Length = 7, IsNullable = false, DefaultValue = "Product")]
    public string SalesProduct { get; set; } = "Product";

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    [SugarColumn(ColumnName = "product_category_code", ColumnDescription = "产品类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ProfitCenterCode { get; set; }

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ModelCode { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? CustomerName1 { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "planner_group_code", ColumnDescription = "计划组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "planner_group_name", ColumnDescription = "计划组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? PlannerGroupName { get; set; }

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "计划总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 计划总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "计划总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转生产销售数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转生产销售金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "plan_status", ColumnDescription = "计划状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanStatus { get; set; } = 1;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "plan_description", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? PlanDescription { get; set; }

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesForecastItem.SalesForecastId))]
    public List<TaktSalesForecastItem>? Items { get; set; }
}
