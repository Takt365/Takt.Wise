// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.LaborHour
// 文件名称：TaktPcbaMiLaborHourDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaMiLaborHour 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaMiLaborHour 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.LaborHour;

// ========================================
// PcbaMiLaborHour 响应 DTO
// ========================================

/// <summary>
/// PCBA手插工数统计实体
/// 对应前端 TaktPcbaMiLaborHourDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaMiLaborHourDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaMiLaborHourID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaMiLaborHourId { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal ActualMinutes { get; set; }

}

// ========================================
// PcbaMiLaborHour 查询 DTO
// ========================================

/// <summary>
/// PcbaMiLaborHour 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaMiLaborHourQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal? DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

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
// 创建PcbaMiLaborHour DTO
// ========================================

/// <summary>
/// 创建PcbaMiLaborHour DTO
/// </summary>
public class TaktPcbaMiLaborHourCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal ActualMinutes { get; set; }

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
// 更新PcbaMiLaborHour DTO
// ========================================

/// <summary>
/// 更新PcbaMiLaborHour DTO
/// 继承 TaktPcbaMiLaborHourCreateDto，添加 PcbaMiLaborHourId 字段
/// </summary>
public class TaktPcbaMiLaborHourUpdateDto : TaktPcbaMiLaborHourCreateDto
{
    /// <summary>
    /// PcbaMiLaborHourID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaMiLaborHourId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaMiLaborHour 导入模板行 DTO
/// </summary>
public class TaktPcbaMiLaborHourTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal? DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

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
/// PcbaMiLaborHour 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaMiLaborHourImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal? DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

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
/// PcbaMiLaborHour 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaMiLaborHourExportDto
{
    /// <summary>
    /// PcbaMiLaborHourID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaMiLaborHourId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    public decimal DowntimeMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    public decimal ActualMinutes { get; set; }

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
