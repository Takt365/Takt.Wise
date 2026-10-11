// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.CustomerService
// 文件名称：TaktCustomerServiceRequest.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务请求实体，记录客户发起的咨询、报修、安装等服务诉求
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.CustomerService;

/// <summary>
/// 服务请求实体
/// </summary>
[SugarTable("takt_logistics_customer_service_request", "服务请求表")]
[SugarIndex("ix_customer_service_request_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_service_request_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_request_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ServiceRequestCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_customer_service_request_client_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ClientId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_request_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RequestStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_request_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RequestDate), OrderByType.Desc, false)]
public class TaktCustomerServiceRequest : TaktCompanyEntityBase
{

    /// <summary>
    /// 服务请求单号（组合唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "service_request_code", ColumnDescription = "服务请求单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ServiceRequestCode { get; set; } = string.Empty;

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
    /// 请求日期
    /// </summary>
    [SugarColumn(ColumnName = "request_date", ColumnDescription = "请求日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime RequestDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 期望服务日期
    /// </summary>
    [SugarColumn(ColumnName = "expected_service_date", ColumnDescription = "期望服务日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpectedServiceDate { get; set; }

    /// <summary>
    /// 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "request_type", ColumnDescription = "请求类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RequestType { get; set; } = 0;

    /// <summary>
    /// 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "source_channel", ColumnDescription = "请求来源", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SourceChannel { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Priority { get; set; } = 3;

    /// <summary>
    /// 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "request_status", ColumnDescription = "请求状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 请求主题
    /// </summary>
    [SugarColumn(ColumnName = "request_subject", ColumnDescription = "请求主题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string RequestSubject { get; set; } = string.Empty;

    /// <summary>
    /// 请求描述
    /// </summary>
    [SugarColumn(ColumnName = "request_description", ColumnDescription = "请求描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string RequestDescription { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "联系人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 服务地址
    /// </summary>
    [SugarColumn(ColumnName = "service_address", ColumnDescription = "服务地址", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ServiceAddress { get; set; }

    /// <summary>
    /// 受理人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "assigned_employee_id", ColumnDescription = "受理人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 受理人姓名
    /// </summary>
    [SugarColumn(ColumnName = "assigned_employee_name", ColumnDescription = "受理人姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AssignedEmployeeName { get; set; }

    /// <summary>
    /// 受理时间
    /// </summary>
    [SugarColumn(ColumnName = "assigned_at", ColumnDescription = "受理时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? AssignedAt { get; set; }

    /// <summary>
    /// 关闭时间
    /// </summary>
    [SugarColumn(ColumnName = "closed_at", ColumnDescription = "关闭时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClosedAt { get; set; }

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
    /// 关联服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceRequestId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerServiceOrder.ServiceRequestId))]
    public List<TaktCustomerServiceOrder>? ServiceOrders { get; set; }

    /// <summary>
    /// 服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceRequestId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerServiceTicket.ServiceRequestId))]
    public List<TaktCustomerServiceTicket>? Tickets { get; set; }
}
