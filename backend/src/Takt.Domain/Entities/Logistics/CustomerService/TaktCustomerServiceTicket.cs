// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.CustomerService
// 文件名称：TaktCustomerServiceTicket.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务工单实体，记录现场/远程服务执行、派工与验收过程
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.CustomerService;

/// <summary>
/// 服务工单实体
/// </summary>
[SugarTable("takt_logistics_customer_service_ticket", "服务工单表")]
[SugarIndex("ix_customer_service_ticket_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_service_ticket_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_ticket_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ServiceTicketCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_customer_service_ticket_request_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ServiceRequestId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_ticket_order_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ServiceOrderId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_customer_service_ticket_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketStatus), OrderByType.Asc, false)]
public class TaktCustomerServiceTicket : TaktCompanyEntityBase
{

    /// <summary>
    /// 服务工单编码（组合唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "service_ticket_code", ColumnDescription = "服务工单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ServiceTicketCode { get; set; } = string.Empty;

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
    /// 关联服务订单ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "service_order_id", ColumnDescription = "关联服务订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ServiceOrderId { get; set; }

    /// <summary>
    /// 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "service_order_code", ColumnDescription = "关联服务订单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ServiceOrderCode { get; set; }

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
    /// 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_type", ColumnDescription = "工单类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TicketType { get; set; } = 0;

    /// <summary>
    /// 优先级（字典 sys_priority_level）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Priority { get; set; } = 3;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_status", ColumnDescription = "工单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TicketStatus { get; set; } = 0;

    /// <summary>
    /// 工单主题
    /// </summary>
    [SugarColumn(ColumnName = "ticket_subject", ColumnDescription = "工单主题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TicketSubject { get; set; } = string.Empty;

    /// <summary>
    /// 故障/问题描述
    /// </summary>
    [SugarColumn(ColumnName = "fault_description", ColumnDescription = "故障描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? FaultDescription { get; set; }

    /// <summary>
    /// 处理方案/解决说明
    /// </summary>
    [SugarColumn(ColumnName = "solution_description", ColumnDescription = "处理方案", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? SolutionDescription { get; set; }

    /// <summary>
    /// 服务地点
    /// </summary>
    [SugarColumn(ColumnName = "service_location", ColumnDescription = "服务地点", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ServiceLocation { get; set; }

    /// <summary>
    /// 指派服务人员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "assigned_employee_id", ColumnDescription = "指派服务人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssignedEmployeeId { get; set; }

    /// <summary>
    /// 指派服务人员姓名
    /// </summary>
    [SugarColumn(ColumnName = "assigned_employee_name", ColumnDescription = "指派服务人员姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AssignedEmployeeName { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ScheduledStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ScheduledEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_time", ColumnDescription = "实际开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_time", ColumnDescription = "实际结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 验收结果（0=不合格，1=合格，2=部分合格）
    /// </summary>
    [SugarColumn(ColumnName = "acceptance_result", ColumnDescription = "验收结果", ColumnDataType = "int", IsNullable = true)]
    public int? AcceptanceResult { get; set; }

    /// <summary>
    /// 验收人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "accepted_by_employee_id", ColumnDescription = "验收人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AcceptedByEmployeeId { get; set; }
    /// <summary>
    /// 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "accepted_by_employee_name", ColumnDescription = "验收人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AcceptedByEmployeeName { get; set; }

    /// <summary>
    /// 验收时间
    /// </summary>
    [SugarColumn(ColumnName = "accepted_at", ColumnDescription = "验收时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联服务请求
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ServiceRequestId))]
    public TaktCustomerServiceRequest? CustomerServiceRequest { get; set; }

    /// <summary>
    /// 关联服务订单
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ServiceOrderId))]
    public TaktCustomerServiceOrder? CustomerServiceOrder { get; set; }

    /// <summary>
    /// 关联服务合同
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ServiceContractId))]
    public TaktCustomerServiceContract? CustomerServiceContract { get; set; }
}
