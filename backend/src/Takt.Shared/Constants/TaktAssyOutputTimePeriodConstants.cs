// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktAssyOutputTimePeriodConstants.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细固定生产时段（新增主表时自动创建 13 条子表行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 组立日报明细固定生产时段常量
/// </summary>
public static class TaktAssyOutputTimePeriodConstants
{
    /// <summary>
    /// 新增组立日报时固定的生产时段列表（共 13 条，与 TaktAssyOutputDetail.TimePeriod 列长度 20 对齐）
    /// </summary>
    public static readonly string[] DefaultTimePeriods =
    {
        "08:00:00~09:00:00",
        "09:00:00~10:00:00",
        "10:10:00~11:10:00",
        "11:10:00~12:10:00",
        "13:30:00~14:30:00",
        "14:30:00~15:30:00",
        "15:40:00~16:40:00",
        "16:40:00~17:40:00",
        "18:30:00~19:30:00",
        "19:30:00~20:30:00",
        "20:30:00~21:30:00",
        "21:30:00~22:30:00",
        "22:30:00~23:30:00",
    };

    /// <summary>
    /// 固定清洁停线生产时段（停线原因=清洁，停线时间=直接人员×4分钟）
    /// </summary>
    public static readonly string[] CleaningTimePeriods =
    {
        "11:10:00~12:10:00",
        "16:40:00~17:40:00",
    };

    /// <summary>
    /// 清洁时段停线原因字典标签（logistics_manufacturing_stop_reason · 清洁）
    /// </summary>
    public const string CleaningStopReasonDictLabel = "清洁";

    /// <summary>
    /// 清洁时段每位直接人员停线分钟数
    /// </summary>
    public const int CleaningDowntimeMinutesPerDirectLabor = 4;

    /// <summary>
    /// 是否为固定清洁停线生产时段
    /// </summary>
    /// <param name="timePeriod">生产时段</param>
    /// <returns>是清洁时段时为 true</returns>
    public static bool IsCleaningTimePeriod(string? timePeriod)
    {
        if (string.IsNullOrWhiteSpace(timePeriod))
        {
            return false;
        }
        var normalized = NormalizeTimePeriod(timePeriod);
        foreach (var period in CleaningTimePeriods)
        {
            if (period == normalized)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 规范化生产时段分隔符（~ / - / -- 等统一为 ~）
    /// </summary>
    /// <param name="timePeriod">生产时段</param>
    /// <returns>规范化后的时段</returns>
    public static string NormalizeTimePeriod(string timePeriod)
    {
        var trimmed = timePeriod.Trim();
        return System.Text.RegularExpressions.Regex.Replace(
            trimmed,
            @"(\d{1,2}:\d{2}:\d{2})\s*[-–—～~]+\s*(\d{1,2}:\d{2}:\d{2})",
            "$1~$2");
    }
}
