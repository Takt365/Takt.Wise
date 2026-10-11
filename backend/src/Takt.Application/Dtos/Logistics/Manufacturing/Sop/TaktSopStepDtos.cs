// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopStep 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopStep 生成，请按需审阅）
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
// SopStep 响应 DTO
// ========================================

/// <summary>
/// SOP 工步实体
/// 对应前端 TaktSopStepDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopStepDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopStepID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepId { get; set; }

    /// <summary>
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ContentId { get; set; }

    /// <summary>
    /// 正文 名称（填充字段）
    /// </summary>
    public string? ContentName { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 工步标题
    /// </summary>
    public string StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int SafetyPopupRequired { get; set; } = 0;

    /// <summary>
    /// 正文
    /// （主表：TaktSopContent）
    /// </summary>
    public TaktSopContentDto? Content { get; set; }

    /// <summary>
    /// 多媒体
    /// （子表：TaktSopStepMedia）
    /// </summary>
    public List<TaktSopStepMediaDto>? MediaList { get; set; }

    /// <summary>
    /// 检验项目
    /// （子表：TaktSopStepCheckItem）
    /// </summary>
    public List<TaktSopStepCheckItemDto>? CheckItems { get; set; }

}

// ========================================
// SopStep 查询 DTO
// ========================================

/// <summary>
/// SopStep 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopStepQueryDto : TaktPagedQuery
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
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 工步标题
    /// </summary>
    public string? StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? SafetyPopupRequired { get; set; }

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
// 创建SopStep DTO
// ========================================

/// <summary>
/// 创建SopStep DTO
/// </summary>
public class TaktSopStepCreateDto
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
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 工步标题
    /// </summary>
    [Required(ErrorMessage = "工步标题不能为空")]
    public string StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int SafetyPopupRequired { get; set; } = 0;

    /// <summary>
    /// 多媒体（子表，级联保存）
    /// </summary>
    public List<TaktSopStepMediaCreateDto>? MediaList { get; set; }

    /// <summary>
    /// 检验项目（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCheckItemCreateDto>? CheckItems { get; set; }

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
// 更新SopStep DTO
// ========================================

/// <summary>
/// 更新SopStep DTO
/// 继承 TaktSopStepCreateDto，添加 SopStepId 字段
/// </summary>
public class TaktSopStepUpdateDto : TaktSopStepCreateDto
{
    /// <summary>
    /// SopStepID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepId { get; set; }

    /// <summary>
    /// 多媒体（子表，级联保存）
    /// </summary>
    public new List<TaktSopStepMediaUpdateDto>? MediaList { get; set; }

    /// <summary>
    /// 检验项目（子表，级联保存）
    /// </summary>
    public new List<TaktSopStepCheckItemUpdateDto>? CheckItems { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopStep 导入模板行 DTO
/// </summary>
public class TaktSopStepTemplateDto
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
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 工步标题
    /// </summary>
    public string? StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? SafetyPopupRequired { get; set; }

    /// <summary>
    /// 多媒体（子表，级联保存）
    /// </summary>
    public List<TaktSopStepMediaCreateDto>? MediaList { get; set; }

    /// <summary>
    /// 检验项目（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCheckItemCreateDto>? CheckItems { get; set; }

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
/// SopStep 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopStepImportDto
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
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 工步标题
    /// </summary>
    public string? StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? SafetyPopupRequired { get; set; }

    /// <summary>
    /// 多媒体（子表，级联保存）
    /// </summary>
    public List<TaktSopStepMediaCreateDto>? MediaList { get; set; }

    /// <summary>
    /// 检验项目（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCheckItemCreateDto>? CheckItems { get; set; }

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
/// SopStep 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopStepExportDto
{
    /// <summary>
    /// SopStepID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepId { get; set; }

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
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 工步标题
    /// </summary>
    public string StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    public string? StepDescription { get; set; } = string.Empty;

    /// <summary>
    /// 安全警示
    /// </summary>
    public string? SafetyAlert { get; set; } = string.Empty;

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int SafetyPopupRequired { get; set; } = 0;

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
