// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Organization
// 文件名称：TaktEmployeeDeptDtos.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktEmployeeDept RBAC 关联 DTO（仅列表，分配走 TaktRbacsController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.HumanResource.Organization;

/// <summary>
/// 员工-部门关联列表 DTO（对应 TaktEmployeeDept）
/// </summary>
public class TaktEmployeeDeptDto : TaktCompanyDtoBase
{
    /// <summary>
    /// 关联主键（适配实体 Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDeptId { get; set; }

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门名称（填充字段）
    /// </summary>
    public string? DeptName { get; set; }
}
