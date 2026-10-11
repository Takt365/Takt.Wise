// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.CustomerService
// 文件名称：TaktCustomerServiceOrder.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务订单实体，记录面向客户的服务交付与计费结算单据
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.CustomerService;

/// <summary>
/// 服务订单实体
/// </summary>
[SugarTable("takt_logistics_customer_service_order", "服务订单表")]
[SugarIndex("ix_customer_service_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_service_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_order_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ServiceOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_customer_service_order_client_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ClientId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_order_contract_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ServiceContractId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_order_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_order_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderDate), OrderByType.Desc, false)]
public class TaktCustomerServiceOrder : TaktCompanyEntityBase
{

    /// <summary>
    /// 服务订单编码（组合唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "service_order_code", ColumnDescription = "服务订单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ServiceOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "client_id", ColumnDescription = "客户端ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "client_code", ColumnDescription = "客户端编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "client_name1", ColumnDescription = "客户端名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 关联服务合同ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "service_contract_id", ColumnDescription = "关联服务合同ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceContractId { get; set; }

    /// <summary>
    /// 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "service_contract_code", ColumnDescription = "关联服务合同编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ServiceContractCode { get; set; }

    /// <summary>
    /// 关联服务请求ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "service_request_id", ColumnDescription = "关联服务请求ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceRequestId { get; set; }

    /// <summary>
    /// 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "service_request_code", ColumnDescription = "关联服务请求单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ServiceRequestCode { get; set; }

    /// <summary>
    /// 订单日期
    /// </summary>
    [SugarColumn(ColumnName = "order_date", ColumnDescription = "订单日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "order_type", ColumnDescription = "订单类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OrderType { get; set; } = 0;

    /// <summary>
    /// 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 订单总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "订单总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    /// <summary>
    /// 折扣金额
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 订单实付金额
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "订单实付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 计划开始日期
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_date", ColumnDescription = "计划开始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_date", ColumnDescription = "计划结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 实际开始日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_date", ColumnDescription = "实际开始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_date", ColumnDescription = "实际结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 服务负责人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "service_employee_id", ColumnDescription = "服务负责人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceEmployeeId { get; set; }
    /// <summary>
    /// 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "service_employee_name", ColumnDescription = "服务负责人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ServiceEmployeeName { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联服务合同
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ServiceContractId))]
    public TaktCustomerServiceContract? CustomerServiceContract { get; set; }

    /// <summary>
    /// 关联服务请求
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ServiceRequestId))]
    public TaktCustomerServiceRequest? CustomerServiceRequest { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceOrderId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerServiceTicket.ServiceOrderId))]
    public List<TaktCustomerServiceTicket>? Tickets { get; set; }
}
