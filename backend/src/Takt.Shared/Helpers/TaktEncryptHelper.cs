// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEncryptHelper.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt加密帮助类，使用PBKDF2算法进行密码哈希，防彩虹表攻击
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt加密帮助类
/// </summary>
/// <remarks>无状态；HashPassword 使用随机盐，输出随调用变化；VerifyPassword 为 Try 语义，无效输入返回 false。</remarks>
public static class TaktEncryptHelper
{
    private const int SaltSize = 32; // 盐值长度（字节），增加到32字节以增强防彩虹表攻击
    private const int Iterations = 60000; // PBKDF2 迭代次数（默认60000）
    private const int KeyLength = 32; // 密钥长度（字节）

    /// <summary>
    /// 使用PBKDF2哈希密码（使用BouncyCastle，防彩虹表攻击）
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <returns>哈希后的密码（格式：salt+hash，Base64编码）</returns>
    /// <exception cref="ArgumentException"><paramref name="password"/> 为空</exception>
    /// <remarks>
    /// 防彩虹表攻击原理：
    /// 1. 存储阶段：每个密码使用32字节随机盐值，确保每个密码都有唯一的盐值
    ///    即使两个用户使用相同的密码（如"123456"），由于盐值不同，存储的哈希值也不同
    /// 2. 验证阶段：接收原始密码，从存储的哈希值中提取盐值，使用相同的盐值重新计算哈希并比较
    /// 3. 攻击者即使获得数据库中的哈希值，也无法使用预计算的彩虹表直接查找原始密码
    ///    因为需要为每个不同的盐值重新生成彩虹表，这几乎不可能
    /// 
    /// 其他安全措施：
    /// - 使用PBKDF2慢速哈希算法，增加计算成本
    /// - 60000次迭代，大幅增加破解时间
    /// - 每个密码使用不同的盐值，使彩虹表失效
    /// </remarks>
    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);

        // 使用加密安全的随机数生成器生成随机盐值
        // 每个密码使用唯一的盐值，防止彩虹表攻击
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // 使用PBKDF2生成密钥（慢速哈希，增加破解成本）
        var pbeParametersGenerator = new Pkcs5S2ParametersGenerator();
        pbeParametersGenerator.Init(
            PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
            salt,
            Iterations);

        var key = ((KeyParameter)pbeParametersGenerator.GenerateDerivedParameters("AES", KeyLength * 8)).GetKey();
        
        // 组合盐值和哈希值：salt:hash
        // 存储格式确保验证时可以提取盐值
        var result = new byte[SaltSize + KeyLength];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(key, 0, result, SaltSize, KeyLength);
        
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password">原始密码（前端发送的明文密码）</param>
    /// <param name="hashedPassword">数据库中存储的哈希密码（格式：salt+hash）</param>
    /// <returns>是否匹配</returns>
    /// <remarks>
    /// 验证流程：
    /// 1. 从存储的哈希值中提取盐值（前32字节）
    /// 2. 使用相同的盐值和参数（60000次迭代）重新计算哈希
    /// 3. 比较新计算的哈希与存储的哈希值
    /// 
    /// 安全措施：
    /// - 使用恒定时间比较防止时序攻击
    /// - 防彩虹表攻击已在存储阶段完成（每个密码使用唯一盐值）
    /// </remarks>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            return false;

        try
        {
            // 解码哈希值
            var hashBytes = Convert.FromBase64String(hashedPassword);
            if (hashBytes.Length != SaltSize + KeyLength)
                return false;

            // 提取盐值和哈希值
            var salt = new byte[SaltSize];
            var storedHash = new byte[KeyLength];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, KeyLength);

            // 使用相同的盐值和参数重新计算哈希
            var pbeParametersGenerator = new Pkcs5S2ParametersGenerator();
            pbeParametersGenerator.Init(
                PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
                salt,
                Iterations);

            var key = ((KeyParameter)pbeParametersGenerator.GenerateDerivedParameters("AES", KeyLength * 8)).GetKey();

            // 使用恒定时间比较防止时序攻击
            return ConstantTimeEquals(key, storedHash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 恒定时间比较两个字节数组，防止时序攻击
    /// </summary>
    /// <param name="a">第一个字节数组</param>
    /// <param name="b">第二个字节数组</param>
    /// <returns>是否相等</returns>
    private static bool ConstantTimeEquals(byte[] a, byte[] b)
    {
        if (a == null || b == null || a.Length != b.Length)
            return false;

        var result = 0;
        for (var i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }

        return result == 0;
    }

    /// <summary>
    /// 解密前端 RSA 传输的登录密码（PKCS#1 v1.5，Base64 密文；仅供传输层，验密仍走 VerifyPassword）
    /// </summary>
    /// <param name="cipherPasswordBase64">前端 RSA 加密后的 Base64 密文</param>
    /// <param name="privateKeyPem">RSA 私钥 PEM</param>
    /// <returns>明文密码</returns>
    public static string DecryptPassword(string cipherPasswordBase64, string privateKeyPem)
    {
        if (string.IsNullOrWhiteSpace(cipherPasswordBase64))
        {
            throw new ArgumentException("密码密文不能为空", nameof(cipherPasswordBase64));
        }

        if (string.IsNullOrWhiteSpace(privateKeyPem))
        {
            throw new ArgumentException("RSA 私钥未配置", nameof(privateKeyPem));
        }

        byte[] cipherBytes;
        try
        {
            cipherBytes = Convert.FromBase64String(cipherPasswordBase64);
        }
        catch (FormatException ex)
        {
            throw new CryptographicException("密码密文 Base64 无效", ex);
        }

        using var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);
        var plainBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
