// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktPasswordTransportOptions.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录密码传输 RSA 配置（挂接 PasswordPolicy:Transport）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 登录密码传输 RSA 配置（前端 RSA 加密密文，后端解密后 PBKDF2 验密）
/// </summary>
public class TaktPasswordTransportOptions
{
    /// <summary>
    /// 是否启用 RSA 传输加密（启用时禁止明文密码入参）
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// RSA 公钥（PEM，匿名接口下发给前端）
    /// </summary>
    public string PublicKeyPem { get; set; } = string.Empty;

    /// <summary>
    /// RSA 私钥（PEM，仅服务端解密使用）
    /// </summary>
    public string PrivateKeyPem { get; set; } = string.Empty;

    /// <summary>
    /// 校验传输加密配置
    /// </summary>
    public void Validate()
    {
        if (!Enabled)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(PublicKeyPem))
        {
            throw new InvalidOperationException("PasswordPolicy:Transport:PublicKeyPem 未配置");
        }

        if (string.IsNullOrWhiteSpace(PrivateKeyPem))
        {
            throw new InvalidOperationException("PasswordPolicy:Transport:PrivateKeyPem 未配置");
        }
    }
}
