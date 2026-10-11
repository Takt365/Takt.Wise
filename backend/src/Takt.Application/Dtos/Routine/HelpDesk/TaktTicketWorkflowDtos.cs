// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketWorkflowDtos.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：工单 ITSM 工作流动作 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

/// <summary>
/// 门户/用户提交工单 DTO
/// </summary>
public class TaktTicketSubmitDto
{
    /// <summary>
    /// 工单标题
    /// </summary>
    [Required(ErrorMessage = "工单标题不能为空")]
    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容
    /// </summary>
    public string? TicketContent { get; set; }

    /// <summary>
    /// 附件 JSON
    /// </summary>
    public string? attachments { get; set; }

    /// <summary>
    /// 紧急度（字典 sys_urgency_level）
    /// </summary>
    public int Urgency { get; set; } = 3;

    /// <summary>
    /// 影响范围（字典 sys_impact_level）
    /// </summary>
    public int Impact { get; set; } = 3;

    /// <summary>
    /// 分类编码
    /// </summary>
    public string? CategoryCode { get; set; }

    /// <summary>
    /// 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（关联 TaktItAsset.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 关联知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 邮件/API 建单 DTO
/// </summary>
public class TaktTicketCreateFromChannelDto : TaktTicketSubmitDto
{
    /// <summary>
    /// 工单来源
    /// </summary>
    public int TicketSource { get; set; } = 1;

    /// <summary>
    /// 外部邮件 Message-Id 或 API 幂等键（可选）
    /// </summary>
    public string? ExternalMessageId { get; set; }

    /// <summary>
    /// 提交人 ID（邮件/API 场景可指定；门户由当前用户填充）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名
    /// </summary>
    public string? SubmitterName { get; set; }
}

/// <summary>
/// 指派/领取工单 DTO
/// </summary>
public class TaktTicketAssignDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 处理人 ID（为空则领取到当前用户）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名
    /// </summary>
    public string? AssigneeName { get; set; }

    /// <summary>
    /// 领取后立即进入处理中
    /// </summary>
    public bool StartImmediately { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 工单通用动作 DTO（开始/等待/解决/确认/重开）
/// </summary>
public class TaktTicketWorkflowActionDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 备注或原因
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 我的资产汇总 DTO（按工单 AssetCode 聚合，关联 TaktAsset）
/// </summary>
public class TaktTicketMyAssetDto
{
    /// <summary>
    /// 资产号码
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产名称（来自 TaktAsset）
    /// </summary>
    public string? AssetName { get; set; }

    /// <summary>
    /// 关联工单数量
    /// </summary>
    public int TicketCount { get; set; }

    /// <summary>
    /// 最近工单时间
    /// </summary>
    public DateTime? LastTicketAt { get; set; }
}

/// <summary>
/// 我的资产分页查询
/// </summary>
public class TaktTicketMyAssetQueryDto : TaktPagedQuery
{
}

/// <summary>
/// 工单会话回复 DTO（工作流入口，租户/作者由服务端填充）
/// </summary>
public class TaktTicketSessionReplyCreateDto
{
    /// <summary>
    /// 工单 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    [Required(ErrorMessage = "回复内容不能为空")]
    public string TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 是否内部备注（仅客服可见；0=否，1=是）
    /// </summary>
    public int IsInternal { get; set; } = 0;
}