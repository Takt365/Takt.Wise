// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuePcbaReworkDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIssuePcbaRework 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityIssuePcbaRework 生成，请按需审阅）
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
// QualityIssuePcbaRework 响应 DTO
// ========================================

/// <summary>
/// 品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
/// 对应前端 TaktQualityIssuePcbaReworkDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityIssuePcbaReworkDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityIssuePcbaReworkID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssuePcbaReworkId { get; set; }

    /// <summary>
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题主表 名称（填充字段）
    /// </summary>
    public string? QualityIssueName { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 质量问题主表（导航属性）
    /// （主表：TaktQualityIssue）
    /// </summary>
    public TaktQualityIssueDto? Issue { get; set; }

}

// ========================================
// QualityIssuePcbaRework 查询 DTO
// ========================================

/// <summary>
/// QualityIssuePcbaRework 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityIssuePcbaReworkQueryDto : TaktPagedQuery
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
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal? PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal? PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal? PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal? PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

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
// 创建QualityIssuePcbaRework DTO
// ========================================

/// <summary>
/// 创建QualityIssuePcbaRework DTO
/// </summary>
public class TaktQualityIssuePcbaReworkCreateDto
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
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

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
// 更新QualityIssuePcbaRework DTO
// ========================================

/// <summary>
/// 更新QualityIssuePcbaRework DTO
/// 继承 TaktQualityIssuePcbaReworkCreateDto，添加 QualityIssuePcbaReworkId 字段
/// </summary>
public class TaktQualityIssuePcbaReworkUpdateDto : TaktQualityIssuePcbaReworkCreateDto
{
    /// <summary>
    /// QualityIssuePcbaReworkID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssuePcbaReworkId { get; set; }

}

// ========================================
// QualityIssuePcbaRework 作废 DTO
// ========================================

/// <summary>
/// QualityIssuePcbaRework 作废/撤销作废 DTO
/// </summary>
public class TaktQualityIssuePcbaReworkObsoleteDto
{
    /// <summary>
    /// QualityIssuePcbaReworkID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssuePcbaReworkId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityIssuePcbaRework 导入模板行 DTO
/// </summary>
public class TaktQualityIssuePcbaReworkTemplateDto
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
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal? PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal? PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal? PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal? PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

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
/// QualityIssuePcbaRework 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityIssuePcbaReworkImportDto
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
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal? PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal? PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal? PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal? PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

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
/// QualityIssuePcbaRework 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityIssuePcbaReworkExportDto
{
    /// <summary>
    /// QualityIssuePcbaReworkID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssuePcbaReworkId { get; set; }

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
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? ResponseGroupName { get; set; }

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
