// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsScheduleItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktApsScheduleItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Aps;

// ========================================
// ApsScheduleItem 响应 DTO
// ========================================

/// <summary>
/// APS排程明细（排程的具体工序任务）
/// 对应前端 TaktApsScheduleItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktApsScheduleItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ApsScheduleItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleItemId { get; set; }

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程名称（填充字段）
    /// </summary>
    public string? ApsScheduleName { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单 名称（填充字段）
    /// </summary>
    public string? ApsOrderName { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// APS 工序排程 名称（填充字段）
    /// </summary>
    public string? ApsOperationName { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工艺路线工序 名称（填充字段）
    /// </summary>
    public string? RoutingItemName { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int ProcessSequence { get; set; } = 0;

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int ProcessStandardSTUnit { get; set; } = 0;

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// APS排程主表（主表）
    /// （主表：TaktApsSchedule）
    /// </summary>
    public TaktApsScheduleDto? Schedule { get; set; }

}

// ========================================
// ApsScheduleItem 查询 DTO
// ========================================

/// <summary>
/// ApsScheduleItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktApsScheduleItemQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string? ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int? ProcessSequence { get; set; }

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal? ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int? ProcessStandardSTUnit { get; set; }

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal? ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlanStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlanStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlanEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlanEndTimeEnd { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartTimeStart { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartTimeEnd { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndTimeStart { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndTimeEnd { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int? ProcessStatus { get; set; }

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ApsScheduleItem DTO
// ========================================

/// <summary>
/// 创建ApsScheduleItem DTO
/// </summary>
public class TaktApsScheduleItemCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    [Required(ErrorMessage = "生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    [Required(ErrorMessage = "产品名称不能为空")]
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    [Required(ErrorMessage = "工序编码不能为空")]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    [Required(ErrorMessage = "工序名称不能为空")]
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int ProcessSequence { get; set; } = 0;

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int ProcessStandardSTUnit { get; set; } = 0;

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ApsScheduleItem DTO
// ========================================

/// <summary>
/// 更新ApsScheduleItem DTO
/// 继承 TaktApsScheduleItemCreateDto，添加 ApsScheduleItemId 字段
/// </summary>
public class TaktApsScheduleItemUpdateDto : TaktApsScheduleItemCreateDto
{
    /// <summary>
    /// ApsScheduleItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleItemId { get; set; }

}

// ========================================
// ApsScheduleItem 状态 DTO
// ========================================

/// <summary>
/// ApsScheduleItem 状态更新 DTO
/// </summary>
public class TaktApsScheduleItemStatusDto
{
    /// <summary>
    /// ApsScheduleItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleItemId { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    [Required(ErrorMessage = "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）不能为空")]
    public int ProcessStatus { get; set; } = 0;
}

// ========================================
// ApsScheduleItem 作废 DTO
// ========================================

/// <summary>
/// ApsScheduleItem 作废/撤销作废 DTO
/// </summary>
public class TaktApsScheduleItemObsoleteDto
{
    /// <summary>
    /// ApsScheduleItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ApsScheduleItem 导入模板行 DTO
/// </summary>
public class TaktApsScheduleItemTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string? ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int? ProcessSequence { get; set; }

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal? ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int? ProcessStandardSTUnit { get; set; }

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal? ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int? ProcessStatus { get; set; }

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ApsScheduleItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktApsScheduleItemImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string? ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int? ProcessSequence { get; set; }

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal? ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int? ProcessStandardSTUnit { get; set; }

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal? ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int? ProcessStatus { get; set; }

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// ApsScheduleItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktApsScheduleItemExportDto
{
    /// <summary>
    /// ApsScheduleItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// APS排程编码（冗余字段，便于查询）
    /// </summary>
    public string ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    public int ProcessSequence { get; set; } = 0;

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    public decimal ProcessStandardST { get; set; }

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    public int ProcessStandardSTUnit { get; set; } = 0;

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    public decimal ExtraMinutes { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
