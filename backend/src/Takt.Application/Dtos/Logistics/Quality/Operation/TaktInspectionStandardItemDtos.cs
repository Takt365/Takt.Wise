// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：InspectionStandardItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktInspectionStandardItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// InspectionStandardItem 响应 DTO
// ========================================

/// <summary>
/// 检验标准明细实体
/// 对应前端 TaktInspectionStandardItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktInspectionStandardItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// InspectionStandardItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardItemId { get; set; }

    /// <summary>
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 检验标准 名称（填充字段）
    /// </summary>
    public string? InspectionStandardName { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 检验项目编码
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int InspectionMode { get; set; } = 0;

    /// <summary>
    /// 检验标准值
    /// </summary>
    public string StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    public string UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    public string LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    public string InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    public string InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    public string AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    public string RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int IsQualifiedBasis { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 检验标准（主表）
    /// （主表：TaktInspectionStandard）
    /// </summary>
    public TaktInspectionStandardDto? Standard { get; set; }

}

// ========================================
// InspectionStandardItem 查询 DTO
// ========================================

/// <summary>
/// InspectionStandardItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktInspectionStandardItemQueryDto : TaktPagedQuery
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
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检验项目编码
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int? InspectionMode { get; set; }

    /// <summary>
    /// 检验标准值
    /// </summary>
    public string? StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    public string? UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    public string? LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    public string? InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    public string? InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    public string? AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    public string? RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int? IsQualifiedBasis { get; set; }

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
// 创建InspectionStandardItem DTO
// ========================================

/// <summary>
/// 创建InspectionStandardItem DTO
/// </summary>
public class TaktInspectionStandardItemCreateDto
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
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 检验项目编码
    /// </summary>
    [Required(ErrorMessage = "检验项目编码不能为空")]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    [Required(ErrorMessage = "检验项目名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    [Required(ErrorMessage = "缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）不能为空")]
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int InspectionMode { get; set; } = 0;

    /// <summary>
    /// 检验标准值
    /// </summary>
    [Required(ErrorMessage = "检验标准值不能为空")]
    public string StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    [Required(ErrorMessage = "检验上限值不能为空")]
    public string UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    [Required(ErrorMessage = "检验下限值不能为空")]
    public string LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    [Required(ErrorMessage = "检验工具（手输名称）不能为空")]
    public string InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    [Required(ErrorMessage = "检验方法说明不能为空")]
    public string InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    [Required(ErrorMessage = "接收标准（AC值）不能为空")]
    public string AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    [Required(ErrorMessage = "拒收标准（RE值）不能为空")]
    public string RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int IsQualifiedBasis { get; set; } = 0;

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
// 更新InspectionStandardItem DTO
// ========================================

/// <summary>
/// 更新InspectionStandardItem DTO
/// 继承 TaktInspectionStandardItemCreateDto，添加 InspectionStandardItemId 字段
/// </summary>
public class TaktInspectionStandardItemUpdateDto : TaktInspectionStandardItemCreateDto
{
    /// <summary>
    /// InspectionStandardItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardItemId { get; set; }

}

// ========================================
// InspectionStandardItem 作废 DTO
// ========================================

/// <summary>
/// InspectionStandardItem 作废/撤销作废 DTO
/// </summary>
public class TaktInspectionStandardItemObsoleteDto
{
    /// <summary>
    /// InspectionStandardItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// InspectionStandardItem 导入模板行 DTO
/// </summary>
public class TaktInspectionStandardItemTemplateDto
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
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检验项目编码
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int? InspectionMode { get; set; }

    /// <summary>
    /// 检验标准值
    /// </summary>
    public string? StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    public string? UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    public string? LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    public string? InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    public string? InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    public string? AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    public string? RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int? IsQualifiedBasis { get; set; }

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
/// InspectionStandardItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktInspectionStandardItemImportDto
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
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 检验项目编码
    /// </summary>
    public string? ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int? InspectionMode { get; set; }

    /// <summary>
    /// 检验标准值
    /// </summary>
    public string? StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    public string? UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    public string? LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    public string? InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    public string? InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    public string? AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    public string? RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int? IsQualifiedBasis { get; set; }

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
/// InspectionStandardItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktInspectionStandardItemExportDto
{
    /// <summary>
    /// InspectionStandardItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardItemId { get; set; }

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
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 检验项目编码
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    public int InspectionMode { get; set; } = 0;

    /// <summary>
    /// 检验标准值
    /// </summary>
    public string StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    public string UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    public string LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    public string InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    public string InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    public string AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    public string RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    public int IsQualifiedBasis { get; set; } = 0;

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
