// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.MeetingCenter
// 文件名称：TaktMeetingRoomDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingRoom 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMeetingRoom 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.MeetingCenter;

// ========================================
// MeetingRoom 响应 DTO
// ========================================

/// <summary>
/// 会议室实体 维护线下会议室编码、位置、容量与设施，供会议排期预约
/// 对应前端 TaktMeetingRoomDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMeetingRoomDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MeetingRoomID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingRoomId { get; set; }

    /// <summary>
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    public string RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int Capacity { get; set; } = 0;

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int RoomStatus { get; set; } = 0;

}

// ========================================
// MeetingRoom 查询 DTO
// ========================================

/// <summary>
/// MeetingRoom 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMeetingRoomQueryDto : TaktPagedQuery
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
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    public string? RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    public string? RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int? RoomStatus { get; set; }

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
// 创建MeetingRoom DTO
// ========================================

/// <summary>
/// 创建MeetingRoom DTO
/// </summary>
public class TaktMeetingRoomCreateDto
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
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "会议室编码（租户+公司内唯一）不能为空")]
    public string RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    [Required(ErrorMessage = "会议室名称不能为空")]
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int Capacity { get; set; } = 0;

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int RoomStatus { get; set; } = 0;

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
// 更新MeetingRoom DTO
// ========================================

/// <summary>
/// 更新MeetingRoom DTO
/// 继承 TaktMeetingRoomCreateDto，添加 MeetingRoomId 字段
/// </summary>
public class TaktMeetingRoomUpdateDto : TaktMeetingRoomCreateDto
{
    /// <summary>
    /// MeetingRoomID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingRoomId { get; set; }

}

// ========================================
// MeetingRoom 状态 DTO
// ========================================

/// <summary>
/// MeetingRoom 状态更新 DTO
/// </summary>
public class TaktMeetingRoomStatusDto
{
    /// <summary>
    /// MeetingRoomID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingRoomId { get; set; }

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    [Required(ErrorMessage = "会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）不能为空")]
    public int RoomStatus { get; set; } = 0;
}

// ========================================
// MeetingRoom 排序 DTO
// ========================================

/// <summary>
/// MeetingRoom 排序更新 DTO
/// </summary>
public class TaktMeetingRoomSortDto
{
    /// <summary>
    /// MeetingRoomID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingRoomId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MeetingRoom 导入模板行 DTO
/// </summary>
public class TaktMeetingRoomTemplateDto
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
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    public string? RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    public string? RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int? RoomStatus { get; set; }

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
/// MeetingRoom 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMeetingRoomImportDto
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
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    public string? RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    public string? RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int? RoomStatus { get; set; }

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
/// MeetingRoom 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMeetingRoomExportDto
{
    /// <summary>
    /// MeetingRoomID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingRoomId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室编码（租户+公司内唯一）
    /// </summary>
    public string RoomCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议室名称
    /// </summary>
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// 楼栋/建筑
    /// </summary>
    public string? Building { get; set; } = string.Empty;

    /// <summary>
    /// 楼层
    /// </summary>
    public string? Floor { get; set; } = string.Empty;

    /// <summary>
    /// 详细位置说明
    /// </summary>
    public string? LocationDetail { get; set; } = string.Empty;

    /// <summary>
    /// 容纳人数（0 表示不限）
    /// </summary>
    public int Capacity { get; set; } = 0;

    /// <summary>
    /// 设施说明（投影、视频会议设备等）
    /// </summary>
    public string? Facilities { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）
    /// </summary>
    public int RoomStatus { get; set; } = 0;

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
