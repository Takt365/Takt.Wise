// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaintItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerComplaintItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Complaint;

// ========================================
// CustomerComplaintItem 响应 DTO
// ========================================

/// <summary>
/// 客诉明细实体
/// 对应前端 TaktCustomerComplaintItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerComplaintItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerComplaintItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉 名称（填充字段）
    /// </summary>
    public string? ComplaintName { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 客诉主表
    /// （主表：TaktCustomerComplaint）
    /// </summary>
    public TaktCustomerComplaintDto? Complaint { get; set; }

}

// ========================================
// CustomerComplaintItem 查询 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerComplaintItemQueryDto : TaktPagedQuery
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
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedCompletionDateStart { get; set; }

    /// <summary>
    /// 计划完成日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedCompletionDateEnd { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualCompletionDateStart { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualCompletionDateEnd { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
// 创建CustomerComplaintItem DTO
// ========================================

/// <summary>
/// 创建CustomerComplaintItem DTO
/// </summary>
public class TaktCustomerComplaintItemCreateDto
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
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [Required(ErrorMessage = "客诉单号（冗余：按对应 Id 取主数据名称联动）不能为空")]
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    [Required(ErrorMessage = "不良现象描述不能为空")]
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    [Required(ErrorMessage = "缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）不能为空")]
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

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
// 更新CustomerComplaintItem DTO
// ========================================

/// <summary>
/// 更新CustomerComplaintItem DTO
/// 继承 TaktCustomerComplaintItemCreateDto，添加 CustomerComplaintItemId 字段
/// </summary>
public class TaktCustomerComplaintItemUpdateDto : TaktCustomerComplaintItemCreateDto
{
    /// <summary>
    /// CustomerComplaintItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

}

// ========================================
// CustomerComplaintItem 状态 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 状态更新 DTO
/// </summary>
public class TaktCustomerComplaintItemStatusDto
{
    /// <summary>
    /// CustomerComplaintItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    [Required(ErrorMessage = "改善状态（字典 logistics_quality_improvement_status）不能为空")]
    public int ImprovementStatus { get; set; } = 0;
}

// ========================================
// CustomerComplaintItem 作废 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 作废/撤销作废 DTO
/// </summary>
public class TaktCustomerComplaintItemObsoleteDto
{
    /// <summary>
    /// CustomerComplaintItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 导入模板行 DTO
/// </summary>
public class TaktCustomerComplaintItemTemplateDto
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
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
/// CustomerComplaintItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerComplaintItemImportDto
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
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
/// CustomerComplaintItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerComplaintItemExportDto
{
    /// <summary>
    /// CustomerComplaintItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

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
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? ImprovementResponsibleGroupCode { get; set; }

    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? ImprovementResponsibleGroupName { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

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
