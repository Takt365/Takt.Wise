// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContract.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务合同实体，定义与客户的服务协议、SLA 及有效期
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.CustomerService;

/// <summary>
/// 服务合同实体
/// </summary>
[SugarTable("takt_logistics_customer_service_contract", "服务合同表")]
[SugarIndex("ix_customer_service_contract_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_service_contract_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_contract_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ServiceContractCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_customer_service_contract_client_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ClientId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_contract_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ContractStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_contract_effective_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EffectiveDate), OrderByType.Desc, false)]
public class TaktCustomerServiceContract : TaktCompanyEntityBase
{

    /// <summary>
    /// 服务合同编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务合同编码规则生成并展示，非手输；单据类型菜单：服务合同）
    /// </summary>
    [SugarColumn(ColumnName = "service_contract_code", ColumnDescription = "服务合同编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ServiceContractCode { get; set; } = string.Empty;

    /// <summary>
    /// 合同名称
    /// </summary>
    [SugarColumn(ColumnName = "contract_name", ColumnDescription = "合同名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ContractName { get; set; } = string.Empty;

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
    /// 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "contract_type", ColumnDescription = "合同类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ContractType { get; set; } = 0;

    /// <summary>
    /// 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
    /// </summary>
    [SugarColumn(ColumnName = "contract_status", ColumnDescription = "合同状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ContractStatus { get; set; } = 0;

    /// <summary>
    /// 签订日期
    /// </summary>
    [SugarColumn(ColumnName = "sign_date", ColumnDescription = "签订日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? SignDate { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EffectiveDate { get; set; } = DateTime.Today;

    /// <summary>
    /// 到期日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "到期日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 合同金额
    /// </summary>
    [SugarColumn(ColumnName = "contract_amount", ColumnDescription = "合同金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ContractAmount { get; set; } = 0;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 服务范围描述
    /// </summary>
    [SugarColumn(ColumnName = "service_scope", ColumnDescription = "服务范围描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ServiceScope { get; set; }

    /// <summary>
    /// SLA 响应时限（小时）
    /// </summary>
    [SugarColumn(ColumnName = "sla_response_hours", ColumnDescription = "SLA响应时限（小时）", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SlaResponseHours { get; set; } = 0;

    /// <summary>
    /// SLA 解决时限（小时）
    /// </summary>
    [SugarColumn(ColumnName = "sla_resolve_hours", ColumnDescription = "SLA解决时限（小时）", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SlaResolveHours { get; set; } = 0;

    /// <summary>
    /// 客户经理（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "account_manager_employee_id", ColumnDescription = "客户经理员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AccountManagerEmployeeId { get; set; }
    /// <summary>
    /// 客户经理名称（冗余：按 AccountManagerEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "account_manager_employee_name", ColumnDescription = "客户经理名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AccountManagerEmployeeName { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceContractId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerServiceOrder.ServiceContractId))]
    public List<TaktCustomerServiceOrder>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务请求列表（外键在子表 TaktCustomerServiceRequest.ServiceContractId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerServiceRequest.ServiceContractId))]
    public List<TaktCustomerServiceRequest>? ServiceRequests { get; set; }
}
