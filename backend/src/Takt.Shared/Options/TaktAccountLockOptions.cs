// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktAccountLockOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：账户锁定配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 账户锁定配置选项
/// </summary>
public class TaktAccountLockOptions
{
    public const string SectionName = "AccountLock";

    /// <summary>
    /// 是否启用账户锁定
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 错误次数限制
    /// </summary>
    public int ErrorLimit { get; set; } = 5;

    /// <summary>
    /// 锁定时长（分钟）
    /// </summary>
    public int LockDurationMinutes { get; set; } = 30;

    /// <summary>
    /// 锁定原因模板
    /// </summary>
    public string LockReason { get; set; } = "连续登录失败{ErrorCount}次，达到错误次数限制（{ErrorLimit}次）";

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (ErrorLimit <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:ErrorLimit 必须大于 0");
        }

        if (LockDurationMinutes <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:LockDurationMinutes 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(LockReason))
        {
            throw new InvalidOperationException($"{SectionName}:LockReason 不能为空");
        }
    }
}
