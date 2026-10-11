// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrderDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionOrder 生成，请按需审阅）
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
// ProductionOrder 响应 DTO
// ========================================

/// <summary>
/// 生产工单实体
/// 对应前端 TaktProductionOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }


    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单 名称（填充字段）
    /// </summary>
    public string? PlannedOrderName { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 名称（填充字段）
    /// </summary>
    public string? ApsOrderName { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

}

// ========================================
// ProductionOrder 查询 DTO
// ========================================

/// <summary>
/// ProductionOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionOrderQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal? ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartDateStart { get; set; }

    /// <summary>
    /// 实际开始日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartDateEnd { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndDateStart { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndDateEnd { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划开工时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划完工时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndTimeStart { get; set; }

    /// <summary>
    /// 计划完工时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndTimeEnd { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
// 创建ProductionOrder DTO
// ========================================

/// <summary>
/// 创建ProductionOrder DTO
/// </summary>
public class TaktProductionOrderCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=Id）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    [Required(ErrorMessage = "工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）不能为空")]
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    [Required(ErrorMessage = "工单号不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    [Required(ErrorMessage = "物料描述（回填：随物料）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    [Required(ErrorMessage = "计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
// 更新ProductionOrder DTO
// ========================================

/// <summary>
/// 更新ProductionOrder DTO
/// 继承 TaktProductionOrderCreateDto，添加 ProductionOrderId 字段
/// </summary>
public class TaktProductionOrderUpdateDto : TaktProductionOrderCreateDto
{
    /// <summary>
    /// ProductionOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

}

// ========================================
// ProductionOrder 状态 DTO
// ========================================

/// <summary>
/// ProductionOrder 状态更新 DTO
/// </summary>
public class TaktProductionOrderStatusDto
{
    /// <summary>
    /// ProductionOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionOrder 导入模板行 DTO
/// </summary>
public class TaktProductionOrderTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal? ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// ProductionOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionOrderImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal? ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// ProductionOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionOrderExportDto
{
    /// <summary>
    /// ProductionOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
