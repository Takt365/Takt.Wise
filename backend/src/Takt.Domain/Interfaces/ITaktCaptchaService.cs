// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktCaptchaService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：验证码服务（Slider / Behavior，按 appsettings Captcha 节点）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Options;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 验证码服务接口。实现类按 <c>appsettings.json</c> 中 <c>Captcha:Type</c> 提供 Slider 拼图或 Behavior 行为校验能力。
/// </summary>
public interface ITaktCaptchaService
{
    /// <summary>
    /// 是否启用验证码（对应配置 <c>Captcha:Enabled</c>）。为 false 时登录流程可跳过验证码步骤。
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// 生成一条验证码挑战：Slider 返回背景/滑块 data URL；Behavior 仅返回 CaptchaId 与内存中的目标位置。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>包含 CaptchaId、Type、图片及可选 TargetPosition 的生成结果</returns>
    Task<TaktCaptchaGenerateResult> GenerateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 校验用户提交的验证码。UserInput 通常为 JSON 字符串，字段含 position（0–100%）、timeSpent、mouseTrajectory。
    /// </summary>
    /// <param name="request">含 CaptchaId 与 UserInput 的验证请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否通过、本地化消息键及 Behavior 模式下的 Score</returns>
    Task<TaktCaptchaVerifyResult> VerifyAsync(
        TaktCaptchaVerifyRequest request,
        CancellationToken cancellationToken = default);
}
