// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceOtherDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityAssuranceOther 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityAssuranceOther 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityAssuranceOther 响应 DTO
// ========================================

/// <summary>
/// 品质业务明细 - 其他通常业务费用
/// 对应前端 TaktQualityAssuranceOtherDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityAssuranceOtherDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityAssuranceOtherID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceOtherId { get; set; }

    /// <summary>
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务主表 名称（填充字段）
    /// </summary>
    public string? QualityAssuranceName { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// （主表：TaktQualityAssurance）
    /// </summary>
    public TaktQualityAssuranceDto? Operation { get; set; }

}

// ========================================
// QualityAssuranceOther 查询 DTO
// ========================================

/// <summary>
/// QualityAssuranceOther 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityAssuranceOtherQueryDto : TaktPagedQuery
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
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal? OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
// 创建QualityAssuranceOther DTO
// ========================================

/// <summary>
/// 创建QualityAssuranceOther DTO
/// </summary>
public class TaktQualityAssuranceOtherCreateDto
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
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
// 更新QualityAssuranceOther DTO
// ========================================

/// <summary>
/// 更新QualityAssuranceOther DTO
/// 继承 TaktQualityAssuranceOtherCreateDto，添加 QualityAssuranceOtherId 字段
/// </summary>
public class TaktQualityAssuranceOtherUpdateDto : TaktQualityAssuranceOtherCreateDto
{
    /// <summary>
    /// QualityAssuranceOtherID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceOtherId { get; set; }

}

// ========================================
// QualityAssuranceOther 作废 DTO
// ========================================

/// <summary>
/// QualityAssuranceOther 作废/撤销作废 DTO
/// </summary>
public class TaktQualityAssuranceOtherObsoleteDto
{
    /// <summary>
    /// QualityAssuranceOtherID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceOtherId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityAssuranceOther 导入模板行 DTO
/// </summary>
public class TaktQualityAssuranceOtherTemplateDto
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
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal? OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
/// QualityAssuranceOther 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityAssuranceOtherImportDto
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
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal? OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int? WorkTimeMinutes { get; set; }

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
/// QualityAssuranceOther 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityAssuranceOtherExportDto
{
    /// <summary>
    /// QualityAssuranceOtherID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceOtherId { get; set; }

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
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 项号（如10, 20, 30，步长严格为10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 其他通常业务费用(元)
    /// </summary>
    public decimal OperationsCost { get; set; }

    /// <summary>
    /// 通常业务作业时间(分钟)
    /// </summary>
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 通常业务其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 通常业务其他备注
    /// </summary>
    public string? OtherNote { get; set; } = string.Empty;

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
