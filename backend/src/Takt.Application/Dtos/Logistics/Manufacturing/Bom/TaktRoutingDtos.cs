// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Routing 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktRouting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// Routing 响应 DTO
// ========================================

/// <summary>
/// 工艺路线主表实体
/// 对应前端 TaktRoutingDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktRoutingDto : TaktApprovalDtoBase
{
    /// <summary>
    /// RoutingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int Purpose { get; set; } = 0;

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int RoutingStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细列表（主子表关系）
    /// （子表：TaktRoutingItem）
    /// </summary>
    public List<TaktRoutingItemDto>? Items { get; set; }

}

// ========================================
// Routing 查询 DTO
// ========================================

/// <summary>
/// Routing 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktRoutingQueryDto : TaktPagedQuery
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
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string? RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int? Purpose { get; set; }

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string? Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int? RoutingStatus { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建Routing DTO
// ========================================

/// <summary>
/// 创建Routing DTO
/// </summary>
public class TaktRoutingCreateDto
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
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）不能为空")]
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    [Required(ErrorMessage = "工艺路线编码不能为空")]
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    [Required(ErrorMessage = "工艺路线名称不能为空")]
    public string RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int Purpose { get; set; } = 0;

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    [Required(ErrorMessage = "版本号不能为空")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int RoutingStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktRoutingItemCreateDto>? Items { get; set; }

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
// 更新Routing DTO
// ========================================

/// <summary>
/// 更新Routing DTO
/// 继承 TaktRoutingCreateDto，添加 RoutingId 字段
/// </summary>
public class TaktRoutingUpdateDto : TaktRoutingCreateDto
{
    /// <summary>
    /// RoutingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktRoutingItemUpdateDto>? Items { get; set; }

}

// ========================================
// Routing 状态 DTO
// ========================================

/// <summary>
/// Routing 状态更新 DTO
/// </summary>
public class TaktRoutingStatusDto
{
    /// <summary>
    /// RoutingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）不能为空")]
    public int RoutingStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Routing 导入模板行 DTO
/// </summary>
public class TaktRoutingTemplateDto
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
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string? RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int? Purpose { get; set; }

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string? Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int? RoutingStatus { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktRoutingItemCreateDto>? Items { get; set; }

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
/// Routing 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktRoutingImportDto
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
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string? RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int? Purpose { get; set; }

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string? Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int? RoutingStatus { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktRoutingItemCreateDto>? Items { get; set; }

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
/// Routing 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktRoutingExportDto
{
    /// <summary>
    /// RoutingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

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
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    public int Purpose { get; set; } = 0;

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    public int RoutingStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    public string? RoutingDescription { get; set; } = string.Empty;

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
