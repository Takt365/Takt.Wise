// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutputDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaOutputDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// PcbaOutputDetail 响应 DTO
// ========================================

/// <summary>
/// PCBA明细实体
/// 对应前端 TaktPcbaOutputDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaOutputDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// PCBA日报名称（填充字段）
    /// </summary>
    public string? PcbaOutputName { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal AchievementRate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// PCBA日报（主表）
    /// （主表：TaktPcbaOutput）
    /// </summary>
    public TaktPcbaOutputDto? PcbaOutput { get; set; }

}

// ========================================
// PcbaOutputDetail 查询 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaOutputDetailQueryDto : TaktPagedQuery
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal? StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal? StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal? BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal? TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal? RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal? SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal? StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal? TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal? AchievementRate { get; set; }

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
// 创建PcbaOutputDetail DTO
// ========================================

/// <summary>
/// 创建PcbaOutputDetail DTO
/// </summary>
public class TaktPcbaOutputDetailCreateDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

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
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    [Required(ErrorMessage = "生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）不能为空")]
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）不能为空")]
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    [Required(ErrorMessage = "PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）不能为空")]
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    [Required(ErrorMessage = "面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）不能为空")]
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    [Required(ErrorMessage = "序列号（明细级）不能为空")]
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal AchievementRate { get; set; }

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
// 更新PcbaOutputDetail DTO
// ========================================

/// <summary>
/// 更新PcbaOutputDetail DTO
/// 继承 TaktPcbaOutputDetailCreateDto，添加 PcbaOutputDetailId 字段
/// </summary>
public class TaktPcbaOutputDetailUpdateDto : TaktPcbaOutputDetailCreateDto
{
    /// <summary>
    /// PcbaOutputDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

}

// ========================================
// PcbaOutputDetail 状态 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 状态更新 DTO
/// </summary>
public class TaktPcbaOutputDetailStatusDto
{
    /// <summary>
    /// PcbaOutputDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    [Required(ErrorMessage = "完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）不能为空")]
    public int CompletedStatus { get; set; } = 0;
}

// ========================================
// PcbaOutputDetail 作废 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 作废/撤销作废 DTO
/// </summary>
public class TaktPcbaOutputDetailObsoleteDto
{
    /// <summary>
    /// PcbaOutputDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 导入模板行 DTO
/// </summary>
public class TaktPcbaOutputDetailTemplateDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal? StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal? StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal? BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal? TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal? RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal? SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal? StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal? TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal? AchievementRate { get; set; }

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
/// PcbaOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaOutputDetailImportDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 间接人员
    /// </summary>
    public int? IndirectLabor { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal? StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal? StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int? StdShorts { get; set; }

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal? StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal? BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal? TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal? RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal? SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal? StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal? TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal? AchievementRate { get; set; }

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
/// PcbaOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaOutputDetailExportDto
{
    /// <summary>
    /// PcbaOutputDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    public decimal StdLaborCapacity { get; set; }

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    public decimal StdEquipmentCapacity { get; set; }

    /// <summary>
    /// PCB板别（存 DictLabel；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面）
    /// </summary>
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    public decimal AchievementRate { get; set; }

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
