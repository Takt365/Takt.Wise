// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceEc 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSourceEc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// SourceEc 响应 DTO
// ========================================

/// <summary>
/// 设变来源明细列表
/// 对应前端 TaktSourceEcDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSourceEcDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SourceEcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变号码
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    public string SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string SourceEcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变来源明细列表
    /// （子表：TaktSourceEcDetail）
    /// </summary>
    public List<TaktSourceEcDetailDto>? SourceEcDetails { get; set; }

}

// ========================================
// SourceEc 查询 DTO
// ========================================

/// <summary>
/// SourceEc 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSourceEcQueryDto : TaktPagedQuery
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
    /// 设变号码
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string? SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    public string? SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期（范围查询-开始）
    /// </summary>
    public DateTime? SourceIssueDateStart { get; set; }

    /// <summary>
    /// 发行日期（范围查询-结束）
    /// </summary>
    public DateTime? SourceIssueDateEnd { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal? SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal? SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? SourceEcContent { get; set; } = string.Empty;

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
// 创建SourceEc DTO
// ========================================

/// <summary>
/// 创建SourceEc DTO
/// </summary>
public class TaktSourceEcCreateDto
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
    /// 设变号码
    /// </summary>
    [Required(ErrorMessage = "设变号码不能为空")]
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    [Required(ErrorMessage = "机种不能为空")]
    public string SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    [Required(ErrorMessage = "标题不能为空")]
    public string SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    [Required(ErrorMessage = "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）不能为空")]
    public string SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    [Required(ErrorMessage = "设变内容不能为空")]
    public string SourceEcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变来源明细列表（子表，级联保存）
    /// </summary>
    public List<TaktSourceEcDetailCreateDto>? SourceEcDetails { get; set; }

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
// 更新SourceEc DTO
// ========================================

/// <summary>
/// 更新SourceEc DTO
/// 继承 TaktSourceEcCreateDto，添加 SourceEcId 字段
/// </summary>
public class TaktSourceEcUpdateDto : TaktSourceEcCreateDto
{
    /// <summary>
    /// SourceEcID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 设变来源明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktSourceEcDetailUpdateDto>? SourceEcDetails { get; set; }

}

// ========================================
// SourceEc 状态 DTO
// ========================================

/// <summary>
/// SourceEc 状态更新 DTO
/// </summary>
public class TaktSourceEcStatusDto
{
    /// <summary>
    /// SourceEcID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    [Required(ErrorMessage = "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）不能为空")]
    public string SourceStatus { get; set; } = string.Empty;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SourceEc 导入模板行 DTO
/// </summary>
public class TaktSourceEcTemplateDto
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
    /// 设变号码
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string? SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    public string? SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime? SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal? SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal? SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? SourceEcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变来源明细列表（子表，级联保存）
    /// </summary>
    public List<TaktSourceEcDetailCreateDto>? SourceEcDetails { get; set; }

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
/// SourceEc 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSourceEcImportDto
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
    /// 设变号码
    /// </summary>
    public string? SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string? SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string? SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    public string? SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime? SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal? SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal? SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? SourceEcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变来源明细列表（子表，级联保存）
    /// </summary>
    public List<TaktSourceEcDetailCreateDto>? SourceEcDetails { get; set; }

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
/// SourceEc 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSourceEcExportDto
{
    /// <summary>
    /// SourceEcID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

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
    /// 设变号码
    /// </summary>
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string SourceModel { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    public string SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    public string? SourceTcjOwner { get; set; } = string.Empty;

    /// <summary>
    /// TCJ依赖
    /// </summary>
    public string? SourceTcjDependency { get; set; } = string.Empty;

    /// <summary>
    /// 设变会议
    /// </summary>
    public string? SourceEcMeeting { get; set; } = string.Empty;

    /// <summary>
    /// PP番号
    /// </summary>
    public string? SourcePpCode { get; set; } = string.Empty;

    /// <summary>
    /// 技联书
    /// </summary>
    public string? SourceTechnicalNoticeCode { get; set; } = string.Empty;

    /// <summary>
    /// 实施
    /// </summary>
    public string? SourceImplementation { get; set; } = string.Empty;

    /// <summary>
    /// 主变更理由
    /// </summary>
    public string? SourceMainChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 次变更理由
    /// </summary>
    public string? SourceSecondaryChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 安规
    /// </summary>
    public string? SourceSafetyRegulation { get; set; } = string.Empty;

    /// <summary>
    /// 进行状况
    /// </summary>
    public string? SourceProgressStatus { get; set; } = string.Empty;

    /// <summary>
    /// 机番管理
    /// </summary>
    public string? SourceSerialNumberControl { get; set; } = string.Empty;

    /// <summary>
    /// 客户承认
    /// </summary>
    public string? SourceCustomerApproval { get; set; } = string.Empty;

    /// <summary>
    /// 服务手册订正
    /// </summary>
    public string? SourceServiceManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 用户手册订正
    /// </summary>
    public string? SourceUserManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    public string? SourcePromotionManualRevision { get; set; } = string.Empty;

    /// <summary>
    /// 标准书订正
    /// </summary>
    public string? SourceStandardDocumentRevision { get; set; } = string.Empty;

    /// <summary>
    /// 情报发行
    /// </summary>
    public string? SourceInformationRelease { get; set; } = string.Empty;

    /// <summary>
    /// 成本变动
    /// </summary>
    public string? SourceCostChange { get; set; } = string.Empty;

    /// <summary>
    /// 单位成本
    /// </summary>
    public decimal SourceUnitCost { get; set; }

    /// <summary>
    /// 模具改修费
    /// </summary>
    public decimal SourceMoldModificationCost { get; set; }

    /// <summary>
    /// 相关图纸
    /// </summary>
    public string? SourceRelatedDrawing { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string SourceEcContent { get; set; } = string.Empty;

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
