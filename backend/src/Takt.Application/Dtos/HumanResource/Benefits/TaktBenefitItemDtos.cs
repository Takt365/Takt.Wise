// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Benefits
// 文件名称：TaktBenefitItemDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：BenefitItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBenefitItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Benefits;

// ========================================
// BenefitItem 响应 DTO
// ========================================

/// <summary>
/// 福利项目（非直接现金福利主数据；年假请假走考勤模块，培训实施走培训模块，此处仅配置福利项）
/// 对应前端 TaktBenefitItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBenefitItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BenefitItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

}

// ========================================
// BenefitItem 查询 DTO
// ========================================

/// <summary>
/// BenefitItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBenefitItemQueryDto : TaktPagedQuery
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
    /// 福利项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
    /// </summary>
    public int? BenefitCategory { get; set; }

    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
    /// </summary>
    public int? BenefitType { get; set; }

    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
    /// </summary>
    public int? PaymentCycle { get; set; }

    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    public decimal? EmployerRatio { get; set; }

    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    public decimal? EmployeeRatio { get; set; }

    /// <summary>
    /// 是否强制福利（字典 sys_yes_no）
    /// </summary>
    public int? IsMandatory { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
// 创建BenefitItem DTO
// ========================================

/// <summary>
/// 创建BenefitItem DTO
/// </summary>
public class TaktBenefitItemCreateDto
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
    /// 福利项目编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "福利项目编码（租户+公司内唯一）不能为空")]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目名称
    /// </summary>
    [Required(ErrorMessage = "福利项目名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
    /// </summary>
    public int BenefitCategory { get; set; } = 0;

    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
    /// </summary>
    public int BenefitType { get; set; } = 0;

    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
    /// </summary>
    public int PaymentCycle { get; set; } = 0;

    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    public decimal MaxAmount { get; set; }

    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    public decimal EmployerRatio { get; set; }

    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    public decimal EmployeeRatio { get; set; }

    /// <summary>
    /// 是否强制福利（字典 sys_yes_no）
    /// </summary>
    public int IsMandatory { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

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
// 更新BenefitItem DTO
// ========================================

/// <summary>
/// 更新BenefitItem DTO
/// 继承 TaktBenefitItemCreateDto，添加 BenefitItemId 字段
/// </summary>
public class TaktBenefitItemUpdateDto : TaktBenefitItemCreateDto
{
    /// <summary>
    /// BenefitItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

}

// ========================================
// BenefitItem 状态 DTO
// ========================================

/// <summary>
/// BenefitItem 状态更新 DTO
/// </summary>
public class TaktBenefitItemStatusDto
{
    /// <summary>
    /// BenefitItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int ItemStatus { get; set; } = 0;
}

// ========================================
// BenefitItem 排序 DTO
// ========================================

/// <summary>
/// BenefitItem 排序更新 DTO
/// </summary>
public class TaktBenefitItemSortDto
{
    /// <summary>
    /// BenefitItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BenefitItem 导入模板行 DTO
/// </summary>
public class TaktBenefitItemTemplateDto
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
    /// 福利项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
    /// </summary>
    public int? BenefitCategory { get; set; }

    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
    /// </summary>
    public int? BenefitType { get; set; }

    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
    /// </summary>
    public int? PaymentCycle { get; set; }

    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    public decimal? EmployerRatio { get; set; }

    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    public decimal? EmployeeRatio { get; set; }

    /// <summary>
    /// 是否强制福利（字典 sys_yes_no）
    /// </summary>
    public int? IsMandatory { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
/// BenefitItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBenefitItemImportDto
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
    /// 福利项目编码（租户+公司内唯一）
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
    /// </summary>
    public int? BenefitCategory { get; set; }

    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
    /// </summary>
    public int? BenefitType { get; set; }

    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
    /// </summary>
    public int? PaymentCycle { get; set; }

    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    public decimal? EmployerRatio { get; set; }

    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    public decimal? EmployeeRatio { get; set; }

    /// <summary>
    /// 是否强制福利（字典 sys_yes_no）
    /// </summary>
    public int? IsMandatory { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ItemStatus { get; set; }

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
/// BenefitItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBenefitItemExportDto
{
    /// <summary>
    /// BenefitItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目编码（租户+公司内唯一）
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
    /// </summary>
    public int BenefitCategory { get; set; } = 0;

    /// <summary>
    /// 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
    /// </summary>
    public int BenefitType { get; set; } = 0;

    /// <summary>
    /// 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
    /// </summary>
    public int PaymentCycle { get; set; } = 0;

    /// <summary>
    /// 默认金额或补贴标准（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 金额上限（元，0 表示不限制）
    /// </summary>
    public decimal MaxAmount { get; set; }

    /// <summary>
    /// 公司承担比例（%，如公积金单位缴存比例）
    /// </summary>
    public decimal EmployerRatio { get; set; }

    /// <summary>
    /// 个人承担比例（%，如公积金个人缴存比例）
    /// </summary>
    public decimal EmployeeRatio { get; set; }

    /// <summary>
    /// 是否强制福利（字典 sys_yes_no）
    /// </summary>
    public int IsMandatory { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ItemStatus { get; set; } = 0;

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
