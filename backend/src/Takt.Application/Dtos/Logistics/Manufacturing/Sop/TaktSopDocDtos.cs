// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDocDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopDoc 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopDoc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopDoc 响应 DTO
// ========================================

/// <summary>
/// SOP 文档头实体。FlowInstanceId 由业务在发起流程后写入；审批状态见 ApprovalStatus。
/// 对应前端 TaktSopDocDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktSopDocDto : TaktApprovalDtoBase
{
    /// <summary>
    /// SopDocID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopDocId { get; set; }

    /// <summary>
    /// SOP 编码
    /// </summary>
    public string SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    public string SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺路线明细 名称（填充字段）
    /// </summary>
    public string? RoutingItemName { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 工位 名称（填充字段）
    /// </summary>
    public string? WorkstationName { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 当前生效版本 名称（填充字段）
    /// </summary>
    public string? CurrentRevisionName { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int SopStatus { get; set; } = 0;

    /// <summary>
    /// 工序
    /// （主表：TaktRoutingItem）
    /// </summary>
    public TaktRoutingItemDto? RoutingItem { get; set; }

    /// <summary>
    /// 工位
    /// （主表：TaktSopWorkstation）
    /// </summary>
    public TaktSopWorkstationDto? Workstation { get; set; }

    /// <summary>
    /// 版本列表
    /// （子表：TaktSopRevision）
    /// </summary>
    public List<TaktSopRevisionDto>? Revisions { get; set; }

}

// ========================================
// SopDoc 查询 DTO
// ========================================

/// <summary>
/// SopDoc 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopDocQueryDto : TaktPagedQuery
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
    /// SOP 编码
    /// </summary>
    public string? SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    public string? SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? SopStatus { get; set; }

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
// 创建SopDoc DTO
// ========================================

/// <summary>
/// 创建SopDoc DTO
/// </summary>
public class TaktSopDocCreateDto
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
    /// SOP 编码
    /// </summary>
    [Required(ErrorMessage = "SOP 编码不能为空")]
    public string SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    [Required(ErrorMessage = "SOP 名称不能为空")]
    public string SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int SopStatus { get; set; } = 0;

    /// <summary>
    /// 版本列表（子表，级联保存）
    /// </summary>
    public List<TaktSopRevisionCreateDto>? Revisions { get; set; }

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
// 更新SopDoc DTO
// ========================================

/// <summary>
/// 更新SopDoc DTO
/// 继承 TaktSopDocCreateDto，添加 SopDocId 字段
/// </summary>
public class TaktSopDocUpdateDto : TaktSopDocCreateDto
{
    /// <summary>
    /// SopDocID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopDocId { get; set; }

    /// <summary>
    /// 版本列表（子表，级联保存）
    /// </summary>
    public new List<TaktSopRevisionUpdateDto>? Revisions { get; set; }

}

// ========================================
// SopDoc 状态 DTO
// ========================================

/// <summary>
/// SopDoc 状态更新 DTO
/// </summary>
public class TaktSopDocStatusDto
{
    /// <summary>
    /// SopDocID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopDocId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）不能为空")]
    public int SopStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopDoc 导入模板行 DTO
/// </summary>
public class TaktSopDocTemplateDto
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
    /// SOP 编码
    /// </summary>
    public string? SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    public string? SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? SopStatus { get; set; }

    /// <summary>
    /// 版本列表（子表，级联保存）
    /// </summary>
    public List<TaktSopRevisionCreateDto>? Revisions { get; set; }

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
/// SopDoc 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopDocImportDto
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
    /// SOP 编码
    /// </summary>
    public string? SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    public string? SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? SopStatus { get; set; }

    /// <summary>
    /// 版本列表（子表，级联保存）
    /// </summary>
    public List<TaktSopRevisionCreateDto>? Revisions { get; set; }

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
/// SopDoc 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopDocExportDto
{
    /// <summary>
    /// SopDocID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopDocId { get; set; }

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
    /// SOP 编码
    /// </summary>
    public string SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    public string SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int SopStatus { get; set; } = 0;

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
