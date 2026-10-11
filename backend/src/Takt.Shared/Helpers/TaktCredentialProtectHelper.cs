// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktCredentialProtectHelper.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：可逆凭据保护（AES-256-CBC）；密钥由调用方传入（如 DatabaseBackup:CredentialProtectionKey）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Cryptography;
using System.Text;

namespace Takt.Shared.Helpers;

/// <summary>
/// 可逆凭据保护（AES）；无状态纯工具
/// </summary>
public static class TaktCredentialProtectHelper
{
    private const string Prefix = "taktaes1:";

    /// <summary>
    /// 加密明文密码
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="protectionKey">保护密钥（任意长度，内部 SHA256 派生 32 字节）</param>
    /// <returns>带前缀的 Base64 密文</returns>
    /// <exception cref="ArgumentException">参数为空</exception>
    public static string Protect(string plainText, string protectionKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plainText);
        ArgumentException.ThrowIfNullOrWhiteSpace(protectionKey);
        var key = DeriveKey(protectionKey);
        var iv = RandomNumberGenerator.GetBytes(16);
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        var payload = new byte[iv.Length + cipherBytes.Length];
        Buffer.BlockCopy(iv, 0, payload, 0, iv.Length);
        Buffer.BlockCopy(cipherBytes, 0, payload, iv.Length, cipherBytes.Length);
        return Prefix + Convert.ToBase64String(payload);
    }

    /// <summary>
    /// 解密密文；非本工具前缀时原样返回（兼容历史明文）
    /// </summary>
    /// <param name="cipherText">密文或明文</param>
    /// <param name="protectionKey">保护密钥</param>
    /// <returns>明文</returns>
    /// <exception cref="ArgumentException">参数为空</exception>
    /// <exception cref="CryptographicException">解密失败</exception>
    public static string Unprotect(string cipherText, string protectionKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cipherText);
        ArgumentException.ThrowIfNullOrWhiteSpace(protectionKey);
        if (!cipherText.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return cipherText;
        }
        var payload = Convert.FromBase64String(cipherText[Prefix.Length..]);
        if (payload.Length <= 16)
        {
            throw new CryptographicException("凭据密文格式无效");
        }
        var iv = new byte[16];
        var cipherBytes = new byte[payload.Length - 16];
        Buffer.BlockCopy(payload, 0, iv, 0, 16);
        Buffer.BlockCopy(payload, 16, cipherBytes, 0, cipherBytes.Length);
        var key = DeriveKey(protectionKey);
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 由配置密钥派生 32 字节 AES 密钥
    /// </summary>
    /// <param name="protectionKey">配置密钥</param>
    /// <returns>32 字节密钥</returns>
    private static byte[] DeriveKey(string protectionKey)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(protectionKey));
    }
}
