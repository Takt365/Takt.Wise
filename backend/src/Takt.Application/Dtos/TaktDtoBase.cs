// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos
// 文件名称：TaktDtoBase.cs
// 创建时间：2026-05-20
// 创建人：Takt365(Cursor AI)
// 功能描述：DTO 基类；租户按「工厂×语言」四组合对齐实体 Scope；公司/审批含 PlantCode+CultureCode
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos;

// ========================================
// 租户 DTO 四组合（对齐 Scope）
// 4 Core / 2 Culture / 3 Plant / 1 Tenant(默认)
// ========================================

/// <summary>
/// 租户组合 4 DTO：无关联工厂、无语言（无公司隔离）
/// </summary>
public abstract class TaktTenantCoreDtoBase
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}

/// <summary>
/// 租户组合 2 DTO：无关联工厂、有语言
/// </summary>
public abstract class TaktTenantCultureDtoBase : TaktTenantCoreDtoBase
{
    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = "mul";
}

/// <summary>
/// 租户组合 3 DTO：有关联工厂、无语言
/// </summary>
public abstract class TaktTenantPlantDtoBase : TaktTenantCoreDtoBase
{
    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;
}

/// <summary>
/// 租户组合 1 DTO：有关联工厂、有语言（默认）
/// </summary>
public abstract class TaktTenantDtoBase : TaktTenantCultureDtoBase
{
    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;
}

// ========================================
// 公司级 DTO 基类（对应 TaktCompanyEntityBase）
// ========================================

/// <summary>
/// 公司级 DTO 基类
/// </summary>
public abstract class TaktCompanyDtoBase
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 审批 DTO 基类（对应 TaktApprovalEntityBase）
// ========================================

/// <summary>
/// 审批级 DTO 基类
/// </summary>
public abstract class TaktApprovalDtoBase
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 审批状态
    /// </summary>
    public int ApprovalStatus { get; set; } = 0;

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? ApprovalOpinion { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// 流程实例ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
