// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEcExecTransposedHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行转置单元格解析（各部门完成日期与展示文本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 设变部门执行转置单元格（列=部门编码）
/// </summary>
public sealed class TaktEcDeptTransposedCell
{
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; init; } = string.Empty;
    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; init; }
    /// <summary>
    /// 完成日期（部门业务日期或更新时间）
    /// </summary>
    public DateTime? CompletedDate { get; init; }
    /// <summary>
    /// 展示文本（已实施时为 yyyyMMdd；未实施为 null，前端渲染「未处理」）
    /// </summary>
    public string? DisplayText { get; init; }
}

/// <summary>
/// 设变部门执行转置单元格解析
/// </summary>
public static class TaktEcExecTransposedHelper
{
    /// <summary>
    /// 构建转置单元格（无记录或未实施视为未处理）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="isImplemented">是否实施</param>
    /// <param name="completedDate">完成日期</param>
    /// <returns>转置单元格</returns>
    public static TaktEcDeptTransposedCell BuildCell(string deptCode, int isImplemented, DateTime? completedDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        if (isImplemented != 1)
        {
            return new TaktEcDeptTransposedCell
            {
                DeptCode = deptCode,
                IsImplemented = 0,
            };
        }
        var date = completedDate?.Date;
        return new TaktEcDeptTransposedCell
        {
            DeptCode = deptCode,
            IsImplemented = 1,
            CompletedDate = date,
            DisplayText = date?.ToString("yyyyMMdd"),
        };
    }

    /// <summary>
    /// 按部门编码解析业务完成日期
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="scheduledDate">预定日期（生管）</param>
    /// <param name="purchaseOrderIssueDate">采购订单发行日期（采购）</param>
    /// <param name="inspectionDate">检验日期（受检/品管）</param>
    /// <param name="outboundDate">出库日期（部管）</param>
    /// <param name="productionDate">生产日期（制二/制一）</param>
    /// <param name="updatedAt">更新时间</param>
    /// <param name="createdAt">创建时间</param>
    /// <returns>完成日期</returns>
    public static DateTime? ResolveCompletedDate(
        string deptCode,
        DateTime? scheduledDate,
        DateTime? purchaseOrderIssueDate,
        DateTime? inspectionDate,
        DateTime? outboundDate,
        DateTime? productionDate,
        DateTime? updatedAt,
        DateTime createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        DateTime? primary = deptCode switch
        {
            _ when deptCode == TaktEcDeptCodes.Pmc => scheduledDate,
            _ when deptCode == TaktEcDeptCodes.Mp => purchaseOrderIssueDate,
            _ when deptCode == TaktEcDeptCodes.Iqc => inspectionDate,
            _ when deptCode == TaktEcDeptCodes.Mc => outboundDate,
            _ when deptCode == TaktEcDeptCodes.Pcba => productionDate,
            _ when deptCode == TaktEcDeptCodes.Assy => productionDate,
            _ when deptCode == TaktEcDeptCodes.Qa => inspectionDate,
            _ => null,
        };
        if (primary.HasValue)
        {
            return primary.Value.Date;
        }
        if (updatedAt.HasValue)
        {
            return updatedAt.Value.Date;
        }
        return createdAt.Date;
    }
}
