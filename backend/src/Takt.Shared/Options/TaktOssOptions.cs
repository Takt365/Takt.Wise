// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktOssOptions.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：OSS 对象存储配置选项，绑定 appsettings <c>Oss:{provider}</c>（与字典 sys_oss_provider 一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 单个 OSS 提供商配置（如 <c>Oss:aliyun</c>）
/// </summary>
public class TaktOssOptions
{
    /// <summary>
    /// 配置根节名称
    /// </summary>
    public const string SectionName = "Oss";

    /// <summary>
    /// 服务端点
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// 访问密钥 ID
    /// </summary>
    public string AccessKeyId { get; set; } = string.Empty;

    /// <summary>
    /// 访问密钥 Secret
    /// </summary>
    public string AccessKeySecret { get; set; } = string.Empty;

    /// <summary>
    /// 存储桶名称
    /// </summary>
    public string Bucket { get; set; } = string.Empty;

    /// <summary>
    /// 区域
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// 校验 OSS 提供商配置
    /// </summary>
    /// <param name="provider">提供商标识（用于异常信息）</param>
    public void Validate(string provider)
    {
        if (string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Endpoint 不能为空");
        }

        if (string.IsNullOrWhiteSpace(AccessKeyId))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:AccessKeyId 不能为空");
        }

        if (string.IsNullOrWhiteSpace(AccessKeySecret))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:AccessKeySecret 不能为空");
        }

        if (string.IsNullOrWhiteSpace(Bucket))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Bucket 不能为空");
        }

        if (string.IsNullOrWhiteSpace(Region))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Region 不能为空");
        }
    }
}
