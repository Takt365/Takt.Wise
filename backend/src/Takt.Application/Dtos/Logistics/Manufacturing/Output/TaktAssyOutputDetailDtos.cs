// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutputDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyOutputDetail 生成，请按需审阅）
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
// AssyOutputDetail 响应 DTO
// ========================================

/// <summary>
/// 组立日报明细（产出子表）实体
/// 对应前端 TaktAssyOutputDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyOutputDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

    /// <summary>
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 组立日报名称（填充字段）
    /// </summary>
    public string? AssyOutputName { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（固定值）
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
    /// </summary>
    public decimal AchievementRate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 组立日报（主表）
    /// （主表：TaktAssyOutput）
    /// </summary>
    public TaktAssyOutputDto? AssyOutput { get; set; }

}

// ========================================
// AssyOutputDetail 查询 DTO
// ========================================

/// <summary>
/// AssyOutputDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyOutputDetailQueryDto : TaktPagedQuery
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（固定值）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal? ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal? IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
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
// 创建AssyOutputDetail DTO
// ========================================

/// <summary>
/// 创建AssyOutputDetail DTO
/// </summary>
public class TaktAssyOutputDetailCreateDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

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
    /// 生产时段（固定值）
    /// </summary>
    [Required(ErrorMessage = "生产时段（固定值）不能为空")]
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
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
// 更新AssyOutputDetail DTO
// ========================================

/// <summary>
/// 更新AssyOutputDetail DTO
/// 继承 TaktAssyOutputDetailCreateDto，添加 AssyOutputDetailId 字段
/// </summary>
public class TaktAssyOutputDetailUpdateDto : TaktAssyOutputDetailCreateDto
{
    /// <summary>
    /// AssyOutputDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

}

// ========================================
// AssyOutputDetail 作废 DTO
// ========================================

/// <summary>
/// AssyOutputDetail 作废/撤销作废 DTO
/// </summary>
public class TaktAssyOutputDetailObsoleteDto
{
    /// <summary>
    /// AssyOutputDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyOutputDetail 导入模板行 DTO
/// </summary>
public class TaktAssyOutputDetailTemplateDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（固定值）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal? ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal? IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
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
/// AssyOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyOutputDetailImportDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段（固定值）
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal? ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal? IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal? ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int? MixedProd { get; set; }

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
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
/// AssyOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyOutputDetailExportDto
{
    /// <summary>
    /// AssyOutputDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（固定值）
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

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
    /// 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    public decimal IndirectMinutes { get; set; }

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    public decimal ConfirmMinutes { get; set; }

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
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
