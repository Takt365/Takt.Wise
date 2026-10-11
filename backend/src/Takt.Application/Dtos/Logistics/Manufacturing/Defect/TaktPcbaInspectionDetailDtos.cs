// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaInspectionDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaInspectionDetail 生成，请按需审阅）
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
// PcbaInspectionDetail 响应 DTO
// ========================================

/// <summary>
/// PCBA检查明细实体
/// 对应前端 TaktPcbaInspectionDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaInspectionDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaInspectionDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

    /// <summary>
    /// PCBA检查日报名称（填充字段）
    /// </summary>
    public string? PcbaInspectionName { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// PCBA检查日报（主表）
    /// （主表：TaktPcbaInspection）
    /// </summary>
    public TaktPcbaInspectionDto? PcbaInspection { get; set; }

}

// ========================================
// PcbaInspectionDetail 查询 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaInspectionDetailQueryDto : TaktPagedQuery
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期（范围查询-开始）
    /// </summary>
    public DateTime? BSideAssemblyDateStart { get; set; }

    /// <summary>
    /// B面实装日期（范围查询-结束）
    /// </summary>
    public DateTime? BSideAssemblyDateEnd { get; set; }

    /// <summary>
    /// T面实装日期（范围查询-开始）
    /// </summary>
    public DateTime? TSideAssemblyDateStart { get; set; }

    /// <summary>
    /// T面实装日期（范围查询-结束）
    /// </summary>
    public DateTime? TSideAssemblyDateEnd { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal? InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal? InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal? AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
// 创建PcbaInspectionDetail DTO
// ========================================

/// <summary>
/// 创建PcbaInspectionDetail DTO
/// </summary>
public class TaktPcbaInspectionDetailCreateDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
// 更新PcbaInspectionDetail DTO
// ========================================

/// <summary>
/// 更新PcbaInspectionDetail DTO
/// 继承 TaktPcbaInspectionDetailCreateDto，添加 PcbaInspectionDetailId 字段
/// </summary>
public class TaktPcbaInspectionDetailUpdateDto : TaktPcbaInspectionDetailCreateDto
{
    /// <summary>
    /// PcbaInspectionDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

}

// ========================================
// PcbaInspectionDetail 状态 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 状态更新 DTO
/// </summary>
public class TaktPcbaInspectionDetailStatusDto
{
    /// <summary>
    /// PcbaInspectionDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    [Required(ErrorMessage = "检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）不能为空")]
    public int InspectionStatus { get; set; } = 0;
}

// ========================================
// PcbaInspectionDetail 作废 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 作废/撤销作废 DTO
/// </summary>
public class TaktPcbaInspectionDetailObsoleteDto
{
    /// <summary>
    /// PcbaInspectionDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 导入模板行 DTO
/// </summary>
public class TaktPcbaInspectionDetailTemplateDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal? InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal? InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal? AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
/// PcbaInspectionDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaInspectionDetailImportDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal? InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal? InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal? AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
/// PcbaInspectionDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaInspectionDetailExportDto
{
    /// <summary>
    /// PcbaInspectionDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
