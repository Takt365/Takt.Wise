// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.MeetingCenter
// 文件名称：TaktMeetingRoom.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：会议室实体，维护线下会议室资源与容量、设施及可用状态
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.MeetingCenter;

/// <summary>
/// 会议室实体
/// 维护线下会议室编码、位置、容量与设施，供会议排期预约
/// </summary>
[SugarTable("takt_routine_meeting_center_room", "会议室表")]
[SugarIndex("ix_meeting_room_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_room_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_room_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoomCode), OrderByType.Asc, true)]
[SugarIndex("ix_meeting_room_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoomStatus), OrderByType.Asc, false)]
public class TaktMeetingRoom : TaktCompanyEntityBase
{
    /// <summary>
    /// 会议室编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议室编码规则生成并展示，非手输；单据类型菜单：会议室）
    /// </summary>
    [SugarColumn(ColumnName = "room_code", ColumnDescription = "会议室编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string RoomCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议室名称
    /// </summary>
    [SugarColumn(ColumnName = "room_name", ColumnDescription = "会议室名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string RoomName { get; set; } = string.Empty;
    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    [SugarColumn(ColumnName = "building", ColumnDescription = "楼栋", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? Building { get; set; }
    /// <summary>
    /// 楼层
    /// </summary>
    [SugarColumn(ColumnName = "floor", ColumnDescription = "楼层", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? Floor { get; set; }
    /// <summary>
    /// 详细位置说明
    /// </summary>
    [SugarColumn(ColumnName = "location_detail", ColumnDescription = "详细位置", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? LocationDetail { get; set; }
    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    [SugarColumn(ColumnName = "capacity", ColumnDescription = "容纳人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Capacity { get; set; } = 0;
    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    [SugarColumn(ColumnName = "facilities", ColumnDescription = "设施说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Facilities { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    [SugarColumn(ColumnName = "room_status", ColumnDescription = "会议室状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RoomStatus { get; set; } = 0;
}
