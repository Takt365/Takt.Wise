// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktMessage.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息实体，用于通过 SignalR 管理在线消息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 在线消息实体
/// 公司级实体：消息按租户+公司双重隔离
/// </summary>
[SugarTable("takt_foundation_message", "在线消息表")]
[SugarIndex("ix_message_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_message_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_message_from_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FromUserId), OrderByType.Asc, false)]
[SugarIndex("ix_message_read_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReadStatus), OrderByType.Asc, false)]
[SugarIndex("ix_message_send_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SendTime), OrderByType.Desc, false)]
[SugarIndex("ix_message_to_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ToUserId), OrderByType.Asc, false)]
[SugarIndex("ix_message_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MessageType), OrderByType.Asc, false)]
public class TaktMessage : TaktCompanyEntityBase
{
    /// <summary>
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "from_user_id", ColumnDescription = "发送者用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }
    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "from_user_name", ColumnDescription = "发送者用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string FromUserName { get; set; } = string.Empty;
    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "to_user_id", ColumnDescription = "接收者用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }
    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "to_user_name", ColumnDescription = "接收者用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string ToUserName { get; set; } = string.Empty;
    /// <summary>
    /// 消息标题
    /// </summary>
    [SugarColumn(ColumnName = "message_title", ColumnDescription = "消息标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string MessageTitle { get; set; } = string.Empty;
    /// <summary>
    /// 消息内容
    /// </summary>
    [SugarColumn(ColumnName = "message_content", ColumnDescription = "消息内容", ColumnDataType = "ntext", IsNullable = false)]
    public string MessageContent { get; set; } = string.Empty;
    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    [SugarColumn(ColumnName = "message_type", ColumnDescription = "消息类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "system")]
    public string MessageType { get; set; } = "system";
    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    [SugarColumn(ColumnName = "message_group", ColumnDescription = "消息分组", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "message")]
    public string MessageGroup { get; set; } = "message";
    /// <summary>
    /// 读取时间
    /// </summary>
    [SugarColumn(ColumnName = "read_time", ColumnDescription = "读取时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ReadTime { get; set; }
    /// <summary>
    /// 发送时间
    /// </summary>
    [SugarColumn(ColumnName = "send_time", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime SendTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_cc", ColumnDescription = "抄送", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsCc { get; set; } = 0;
    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? AccessUrl { get; set; }
    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    [SugarColumn(ColumnName = "message_ext_data", ColumnDescription = "消息扩展数据", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? MessageExtData { get; set; }
    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    [SugarColumn(ColumnName = "read_status", ColumnDescription = "读取状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReadStatus { get; set; } = 0;
}
