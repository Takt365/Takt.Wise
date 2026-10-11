// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyDefectDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyDefectDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

// ========================================
// AssyDefectDetail 响应 DTO
// ========================================

/// <summary>
/// 组立不良明细实体
/// 对应前端 TaktAssyDefectDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyDefectDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyDefectDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

    /// <summary>
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

    /// <summary>
    /// 组立不良日报名称（填充字段）
    /// </summary>
    public string? AssyDefectName { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 组立不良日报（主表）
    /// （主表：TaktAssyDefect）
    /// </summary>
    public TaktAssyDefectDto? AssyDefect { get; set; }

}

// ========================================
// AssyDefectDetail 查询 DTO
// ========================================

/// <summary>
/// AssyDefectDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyDefectDetailQueryDto : TaktPagedQuery
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal? CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

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
// 创建AssyDefectDetail DTO
// ========================================

/// <summary>
/// 创建AssyDefectDetail DTO
/// </summary>
public class TaktAssyDefectDetailCreateDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "工单号（冗余字段,便于查询）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

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
// 更新AssyDefectDetail DTO
// ========================================

/// <summary>
/// 更新AssyDefectDetail DTO
/// 继承 TaktAssyDefectDetailCreateDto，添加 AssyDefectDetailId 字段
/// </summary>
public class TaktAssyDefectDetailUpdateDto : TaktAssyDefectDetailCreateDto
{
    /// <summary>
    /// AssyDefectDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

}

// ========================================
// AssyDefectDetail 作废 DTO
// ========================================

/// <summary>
/// AssyDefectDetail 作废/撤销作废 DTO
/// </summary>
public class TaktAssyDefectDetailObsoleteDto
{
    /// <summary>
    /// AssyDefectDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyDefectDetail 导入模板行 DTO
/// </summary>
public class TaktAssyDefectDetailTemplateDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal? CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

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
/// AssyDefectDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyDefectDetailImportDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal? GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal? CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

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
/// AssyDefectDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyDefectDetailExportDto
{
    /// <summary>
    /// AssyDefectDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    public decimal GoodQuantity { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

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
