// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketEvaluationDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TicketEvaluation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTicketEvaluation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// TicketEvaluation 响应 DTO
// ========================================

/// <summary>
/// 工单服务评价（一个工单对应一条评价）
/// 对应前端 TaktTicketEvaluationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTicketEvaluationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TicketEvaluationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketEvaluationId { get; set; }

    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 工单 名称（填充字段）
    /// </summary>
    public string? TicketName { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int Score { get; set; } = 0;

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime EvaluatedAt { get; set; }

    /// <summary>
    /// 工单（主表）
    /// （主表：TaktTicket）
    /// </summary>
    public TaktTicketDto? Ticket { get; set; }

}

// ========================================
// TicketEvaluation 查询 DTO
// ========================================

/// <summary>
/// TicketEvaluation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTicketEvaluationQueryDto : TaktPagedQuery
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间（范围查询-开始）
    /// </summary>
    public DateTime? EvaluatedAtStart { get; set; }

    /// <summary>
    /// 评价时间（范围查询-结束）
    /// </summary>
    public DateTime? EvaluatedAtEnd { get; set; }

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
// 创建TicketEvaluation DTO
// ========================================

/// <summary>
/// 创建TicketEvaluation DTO
/// </summary>
public class TaktTicketEvaluationCreateDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int Score { get; set; } = 0;

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime EvaluatedAt { get; set; }

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
// 更新TicketEvaluation DTO
// ========================================

/// <summary>
/// 更新TicketEvaluation DTO
/// 继承 TaktTicketEvaluationCreateDto，添加 TicketEvaluationId 字段
/// </summary>
public class TaktTicketEvaluationUpdateDto : TaktTicketEvaluationCreateDto
{
    /// <summary>
    /// TicketEvaluationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketEvaluationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TicketEvaluation 导入模板行 DTO
/// </summary>
public class TaktTicketEvaluationTemplateDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime? EvaluatedAt { get; set; }

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
/// TicketEvaluation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTicketEvaluationImportDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime? EvaluatedAt { get; set; }

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
/// TicketEvaluation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTicketEvaluationExportDto
{
    /// <summary>
    /// TicketEvaluationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketEvaluationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    public int Score { get; set; } = 0;

    /// <summary>
    /// 评价内容
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名
    /// </summary>
    public string? EvaluatorName { get; set; } = string.Empty;

    /// <summary>
    /// 评价时间
    /// </summary>
    public DateTime EvaluatedAt { get; set; }

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
