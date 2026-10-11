// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models
// 文件名称：TaktNumberingModel.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则快照与取号结果（拼接/流水计算输入、ITaktNumberingGenerator 输出共用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

/// <summary>
/// 编码规则模型（规则字段快照 + 可选业务编码产出）
/// </summary>
public sealed class TaktNumberingModel
{
    /// <summary>
    /// 规则编码
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务编码（取号结果或起始编码样例）
    /// </summary>
    public string BusinessCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码
    /// </summary>
    public string? PrefixCode { get; set; }

    /// <summary>
    /// 日期格式
    /// </summary>
    public string? DateFormat { get; set; }

    /// <summary>
    /// 流水位数
    /// </summary>
    public int SequenceLength { get; set; } = 6;

    /// <summary>
    /// 流水步长
    /// </summary>
    public int SequenceStep { get; set; } = 1;

    /// <summary>
    /// 后缀编码
    /// </summary>
    public string? SuffixCode { get; set; }

    /// <summary>
    /// 重置周期
    /// </summary>
    public string ResetPeriod { get; set; } = "None";

    /// <summary>
    /// 当前流水
    /// </summary>
    public int CurrentSequence { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    public string? Separator { get; set; }

    /// <summary>
    /// 段配置描述（segments:…）
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 规则上次更新时间（重置周期判断）
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
