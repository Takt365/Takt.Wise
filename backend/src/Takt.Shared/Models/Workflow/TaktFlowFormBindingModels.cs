// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Workflow
// 文件名称：TaktFlowFormBindingModels.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单 RelatedFormField 绑定模型（字段映射 + 可选业务状态列）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Workflow;

/// <summary>
/// RelatedFormField JSON 根结构（兼容纯数组旧格式）
/// </summary>
public class TaktFlowFormBindingRoot
{
    /// <summary>
    /// 表单字段与库列映射
    /// </summary>
    public List<TaktFlowFormFieldMapping> Fields { get; set; } = new();

    /// <summary>
    /// 业务状态列终态映射（可选）
    /// </summary>
    public TaktFlowFormBusinessBinding? Business { get; set; }
}

/// <summary>
/// 单字段映射（FrmData camelCase ↔ 库列）
/// </summary>
public class TaktFlowFormFieldMapping
{
    /// <summary>
    /// 数据库列名（蛇形）
    /// </summary>
    public string DbColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C# / FrmData 字段名（camelCase）
    /// </summary>
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列说明
    /// </summary>
    public string? ColumnDescription { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string? DataType { get; set; }

    /// <summary>
    /// 展示类型
    /// </summary>
    public string? DisplayType { get; set; }

    /// <summary>
    /// 字典类型编码
    /// </summary>
    public string? DictTypeCode { get; set; }
}

/// <summary>
/// 业务单据状态列与流程终态映射（配置在表单 RelatedFormField.business）
/// </summary>
public class TaktFlowFormBusinessBinding
{
    /// <summary>
    /// 业务状态列名（蛇形，如 leave_status）
    /// </summary>
    public string? BusinessStatusColumn { get; set; }

    /// <summary>
    /// 审批中业务状态值
    /// </summary>
    public int? StatusInProgress { get; set; }

    /// <summary>
    /// 已通过业务状态值
    /// </summary>
    public int? StatusApproved { get; set; }

    /// <summary>
    /// 已驳回业务状态值
    /// </summary>
    public int? StatusRejected { get; set; }

    /// <summary>
    /// 已撤销/撤回业务状态值
    /// </summary>
    public int? StatusCancelled { get; set; }

    /// <summary>
    /// 允许提交审批的业务状态值列表（未配置则不校验业务状态列）
    /// </summary>
    public List<int>? SubmitAllowedBusinessStatuses { get; set; }
}
