// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepCheckItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopStepCheckItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopStepCheckItem 生成，请按需审阅）
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
// SopStepCheckItem 响应 DTO
// ========================================

/// <summary>
/// SOP 工步检验项目实体
/// 对应前端 TaktSopStepCheckItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopStepCheckItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopStepCheckItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepCheckItemId { get; set; }

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步 名称（填充字段）
    /// </summary>
    public string? StepName { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工步
    /// （主表：TaktSopStep）
    /// </summary>
    public TaktSopStepDto? Step { get; set; }

}

// ========================================
// SopStepCheckItem 查询 DTO
// ========================================

/// <summary>
/// SopStepCheckItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopStepCheckItemQueryDto : TaktPagedQuery
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
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsRequired { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建SopStepCheckItem DTO
// ========================================

/// <summary>
/// 创建SopStepCheckItem DTO
/// </summary>
public class TaktSopStepCheckItemCreateDto
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
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    [Required(ErrorMessage = "检验项目名称不能为空")]
    public string CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

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
// 更新SopStepCheckItem DTO
// ========================================

/// <summary>
/// 更新SopStepCheckItem DTO
/// 继承 TaktSopStepCheckItemCreateDto，添加 SopStepCheckItemId 字段
/// </summary>
public class TaktSopStepCheckItemUpdateDto : TaktSopStepCheckItemCreateDto
{
    /// <summary>
    /// SopStepCheckItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepCheckItemId { get; set; }

}

// ========================================
// SopStepCheckItem 排序 DTO
// ========================================

/// <summary>
/// SopStepCheckItem 排序更新 DTO
/// </summary>
public class TaktSopStepCheckItemSortDto
{
    /// <summary>
    /// SopStepCheckItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepCheckItemId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopStepCheckItem 导入模板行 DTO
/// </summary>
public class TaktSopStepCheckItemTemplateDto
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
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsRequired { get; set; }

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
/// SopStepCheckItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopStepCheckItemImportDto
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
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsRequired { get; set; }

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
/// SopStepCheckItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopStepCheckItemExportDto
{
    /// <summary>
    /// SopStepCheckItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepCheckItemId { get; set; }

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
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    public string? CheckMethod { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准
    /// </summary>
    public string? CheckStandard { get; set; } = string.Empty;

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
