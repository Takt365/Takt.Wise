// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktEcDeptExecEntity.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变 8 课部门执行表公共字段（查询/去重/保存单链路）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 设变部门执行行公共字段（生管/采购/受检/部管/制二/制一/品管/制技）
/// </summary>
public interface ITaktEcDeptExecEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    long Id { get; }
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    long EcDetailId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    string EcCode { get; set; }
    /// <summary>
    /// 行号
    /// </summary>
    int LineNumber { get; set; }
    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别，如 D0420）
    /// </summary>
    string DeptCode { get; set; }
    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    string DeptName { get; set; }
    /// <summary>
    /// 是否实施
    /// </summary>
    int IsImplemented { get; set; }
    /// <summary>
    /// 执行内容
    /// </summary>
    string? ExecContent { get; set; }
    /// <summary>
    /// 是否作废
    /// </summary>
    int IsObsolete { get; set; }
}
