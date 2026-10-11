// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaRepairDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaRepairDetail 生成，请按需审阅）
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
// PcbaRepairDetail 响应 DTO
// ========================================

/// <summary>
/// PCBA改修明细实体
/// 对应前端 TaktPcbaRepairDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaRepairDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaRepairDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairDetailId { get; set; }

    /// <summary>
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairId { get; set; }

    /// <summary>
    /// PCBA改修日报名称（填充字段）
    /// </summary>
    public string? PcbaRepairName { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? RepairOperatorName { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// PCBA改修日报（主表）
    /// （主表：TaktPcbaRepair）
    /// </summary>
    public TaktPcbaRepairDto? PcbaRepair { get; set; }

}

// ========================================
// PcbaRepairDetail 查询 DTO
// ========================================

/// <summary>
/// PcbaRepairDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaRepairDetailQueryDto : TaktPagedQuery
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
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaRepairId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

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
// 创建PcbaRepairDetail DTO
// ========================================

/// <summary>
/// 创建PcbaRepairDetail DTO
/// </summary>
public class TaktPcbaRepairDetailCreateDto
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
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "工单号（冗余字段,便于查询）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

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
// 更新PcbaRepairDetail DTO
// ========================================

/// <summary>
/// 更新PcbaRepairDetail DTO
/// 继承 TaktPcbaRepairDetailCreateDto，添加 PcbaRepairDetailId 字段
/// </summary>
public class TaktPcbaRepairDetailUpdateDto : TaktPcbaRepairDetailCreateDto
{
    /// <summary>
    /// PcbaRepairDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairDetailId { get; set; }

}

// ========================================
// PcbaRepairDetail 作废 DTO
// ========================================

/// <summary>
/// PcbaRepairDetail 作废/撤销作废 DTO
/// </summary>
public class TaktPcbaRepairDetailObsoleteDto
{
    /// <summary>
    /// PcbaRepairDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaRepairDetail 导入模板行 DTO
/// </summary>
public class TaktPcbaRepairDetailTemplateDto
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
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaRepairId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

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
/// PcbaRepairDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaRepairDetailImportDto
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
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaRepairId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

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
/// PcbaRepairDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaRepairDetailExportDto
{
    /// <summary>
    /// PcbaRepairDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairDetailId { get; set; }

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
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 生产实绩
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    public string? DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    public string? DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    public string? DefectNature { get; set; } = string.Empty;

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
