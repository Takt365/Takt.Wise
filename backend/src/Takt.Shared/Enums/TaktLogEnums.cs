// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktLogEnums.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统计日志域审计枚举（AOP/HTTP/认证写入，非前端表单字典项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 日志级别（数值越大优先级越高，与前端 utils/common.ts 一致）
/// </summary>
public enum TaktLogLevel
{
    /// <summary>
    /// 调试信息
    /// </summary>
    Debug = 0,

    /// <summary>
    /// 一般信息
    /// </summary>
    Info = 1,

    /// <summary>
    /// 警告
    /// </summary>
    Warn = 2,

    /// <summary>
    /// 错误
    /// </summary>
    Error = 3,

    /// <summary>
    /// 致命错误
    /// </summary>
    Fatal = 4
}

/// <summary>
/// 差异日志 CUD 操作类型（SqlSugar AOP 写入，与 DiffType 对齐）
/// </summary>
public enum TaktDeltaOperType
{
    /// <summary>
    /// 新增
    /// </summary>
    [Display(Name = "新增")]
    Insert = 1,

    /// <summary>
    /// 修改
    /// </summary>
    [Display(Name = "修改")]
    Update = 2,

    /// <summary>
    /// 删除
    /// </summary>
    [Display(Name = "删除")]
    Delete = 3,
}

/// <summary>
/// HTTP 请求推导的操作类型（操作日志 AOP 写入，非 sys_oper_type 字典）
/// </summary>
public enum TaktHttpAuditOperType
{
    /// <summary>
    /// 未知或未映射的 HTTP 方法
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,

    /// <summary>
    /// 新增（POST）
    /// </summary>
    [Display(Name = "新增")]
    Insert = 1,

    /// <summary>
    /// 修改（PUT/PATCH）
    /// </summary>
    [Display(Name = "修改")]
    Update = 2,

    /// <summary>
    /// 删除（DELETE）
    /// </summary>
    [Display(Name = "删除")]
    Delete = 3,

    /// <summary>
    /// 查询（GET 等读操作）
    /// </summary>
    [Display(Name = "查询")]
    Query = 4,
}
