// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktQuartzCronHelper.cs
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz Cron 表达式结构校验（6/7 段、日/周互斥），供 Application 校验层使用；完整语义校验由 Infrastructure Quartz CronExpression 负责
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;

namespace Takt.Shared.Helpers;

/// <summary>
/// Quartz Cron 表达式辅助工具（纯结构校验，无 Quartz 依赖）
/// </summary>
public static class TaktQuartzCronHelper
{
    private static readonly Regex FieldTokenPattern = new(
        @"^(\*|\?|[0-9]+(?:L|W|#\d+)?|[0-9]+/[0-9]+|[0-9]+-[0-9]+|[0-9,]+(?:L|W|#\d+)?)$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// 尝试校验 Quartz Cron 表达式基本结构（秒 分 时 日 月 周 [年]）
    /// </summary>
    /// <param name="expression">Cron 表达式</param>
    /// <param name="errorMessage">失败时的错误说明</param>
    /// <returns>结构是否合法</returns>
    public static bool TryValidateQuartzCronExpression(string? expression, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(expression))
        {
            errorMessage = "Cron 表达式不能为空";
            return false;
        }

        var trimmed = expression.Trim();
        if (trimmed.Length > 100)
        {
            errorMessage = "Cron 表达式长度不能超过100个字符";
            return false;
        }

        var parts = trimmed.Split((char[]?)[' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length is not (6 or 7))
        {
            errorMessage = "Cron 表达式须为 6 段（秒 分 时 日 月 周）或 7 段（含年）";
            return false;
        }

        for (var i = 0; i < parts.Length; i++)
        {
            if (!FieldTokenPattern.IsMatch(parts[i]))
            {
                errorMessage = $"Cron 表达式第 {i + 1} 段格式无效：{parts[i]}";
                return false;
            }
        }

        var dayField = parts[3];
        var weekField = parts[5];
        var dayIsQuestion = dayField == "?";
        var weekIsQuestion = weekField == "?";
        if (dayIsQuestion == weekIsQuestion)
        {
            errorMessage = "Cron 表达式「日」与「周」字段须且仅能有一个为 ?";
            return false;
        }

        return true;
    }
}
