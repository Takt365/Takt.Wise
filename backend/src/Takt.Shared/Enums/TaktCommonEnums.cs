// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktCommonEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：基础设施/引擎专用枚举（字典字段实体存 int，服务层直接比较字面量）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 执行结果状态枚举（全局通用：0=失败，1=成功）
/// </summary>
public enum TaktExecuteStatus
{
    /// <summary>
    /// 失败
    /// </summary>
    [Display(Name = "失败")]
    Failed = 0,
    /// <summary>
    /// 成功
    /// </summary>
    [Display(Name = "成功")]
    Success = 1
}

/// <summary>
/// 项目模块枚举（全局通用）
/// </summary>
public enum TaktModule
{
    /// <summary>
    /// 仪表盘
    /// </summary>
    [Display(Name = "仪表盘")]
    Dashboard = 0,
    /// <summary>
    /// 身份认证
    /// </summary>
    [Display(Name = "身份认证")]
    Identity = 1,
    /// <summary>
    /// 日常事务
    /// </summary>
    [Display(Name = "日常事务")]
    Routine = 2,
    /// <summary>
    /// 财务核算
    /// </summary>
    [Display(Name = "财务核算")]
    Accounting = 3,
    /// <summary>
    /// 后勤管理
    /// </summary>
    [Display(Name = "后勤管理")]
    Logistics = 4,
    /// <summary>
    /// 人力资源
    /// </summary>
    [Display(Name = "人力资源")]
    HumanResource = 5,
    /// <summary>
    /// 工作流
    /// </summary>
    [Display(Name = "工作流")]
    Workflow = 6,
    /// <summary>
    /// 代码管理
    /// </summary>
    [Display(Name = "代码管理")]
    Code = 7,
    /// <summary>
    /// 基础设置
    /// </summary>
    [Display(Name = "基础设置")]
    Foundation = 8,
    /// <summary>
    /// 统计看板
    /// </summary>
    [Display(Name = "统计看板")]
    Statistics = 9,
    /// <summary>
    /// 实体字段（Domain 实体元数据翻译）
    /// </summary>
    [Display(Name = "实体")]
    Entity = 10
}

/// <summary>
/// 审批状态枚举（工作流引擎与 TaktApprovalEntityBase）
/// </summary>
public enum TaktApprovalStatus
{
    /// <summary>
    /// 待审批
    /// </summary>
    [Display(Name = "待审批")]
    Pending = 0,
    /// <summary>
    /// 审批中（处理中）
    /// </summary>
    [Display(Name = "审批中")]
    InProgress = 1,
    /// <summary>
    /// 已通过
    /// </summary>
    [Display(Name = "已通过")]
    Approved = 2,
    /// <summary>
    /// 已驳回
    /// </summary>
    [Display(Name = "已驳回")]
    Rejected = 3,
    /// <summary>
    /// 已撤销（申请人主动撤回）
    /// </summary>
    [Display(Name = "已撤销")]
    Cancelled = 4,
    /// <summary>
    /// 已终止（强制结束，非撤回）
    /// </summary>
    [Display(Name = "已终止")]
    Terminated = 5
}

/// <summary>
/// SQL 执行模式（TaktSqlExecuteOptions）
/// </summary>
public enum TaktSqlExecuteMode
{
    /// <summary>
    /// 只读查询（仅 SELECT / WITH，禁止 DML/DDL）
    /// </summary>
    [Display(Name = "只读查询")]
    ReadOnly = 0,
    /// <summary>
    /// 非查询脚本（允许 MERGE/INSERT/UPDATE 等多语句；供 Quartz SqlScript 业务同步）
    /// </summary>
    [Display(Name = "非查询脚本")]
    NonQuery = 1,
}

/// <summary>
/// 应用端枚举（全局通用：前端/后端）
/// </summary>
public enum TaktAppSide
{
    /// <summary>
    /// 前端
    /// </summary>
    [Display(Name = "前端")]
    Frontend = 0,
    /// <summary>
    /// 后端
    /// </summary>
    [Display(Name = "后端")]
    Backend = 1
}

/// <summary>
/// 权限类型枚举（全局通用）
/// </summary>
public enum TaktPermissionType
{
    /// <summary>
    /// 菜单权限（页面访问权限）
    /// </summary>
    [Display(Name = "菜单权限")]
    Menu = 1,
    /// <summary>
    /// 按钮权限（操作权限）
    /// </summary>
    [Display(Name = "按钮权限")]
    Button = 2,
    /// <summary>
    /// 数据权限（数据访问范围）
    /// </summary>
    [Display(Name = "数据权限")]
    Data = 3
}
