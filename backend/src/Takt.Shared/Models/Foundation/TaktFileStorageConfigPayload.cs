// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models
// 文件名称：TaktFileStorageConfigPayload.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktFile.StorageConfig JSON 载荷（与前端 takt-file-storage-config 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

/// <summary>
/// 文件存储配置 JSON 载荷（写入 TaktFile.StorageConfig）
/// </summary>
public class TaktFileStorageConfigPayload
{
    /// <summary>
    /// 上传路径（一级目录菜单 RoutePath 首段，如 uploads/human-resource）
    /// </summary>
    public string? UploadPath { get; set; }

    /// <summary>
    /// 存储命名规则（字典 sys_storage_naming：0/1/2）
    /// </summary>
    public int? StorageNaming { get; set; }

    /// <summary>
    /// OSS 提供商标识（字典 sys_oss_provider，如 aliyun；StorageType=1 时使用）
    /// </summary>
    public string? OssProvider { get; set; }

    /// <summary>
    /// FTP 提供商标识（字典 sys_ftp_provider，如 teac_cn；StorageType=2 时使用）
    /// </summary>
    public string? FtpProvider { get; set; }
}
