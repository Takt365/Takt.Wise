// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktFileEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传引擎专用枚举（分类/存储等字典字段存 int）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 文件上传类型（上传引擎路由：本地/OSS/分片等）
/// </summary>
public enum TaktFileUploadType
{
    /// <summary>
    /// 普通上传
    /// </summary>
    [Display(Name = "普通上传")]
    Normal = 0,
    /// <summary>
    /// 分片上传
    /// </summary>
    [Display(Name = "分片上传")]
    Chunk = 1,
    /// <summary>
    /// 直传 OSS
    /// </summary>
    [Display(Name = "直传OSS")]
    DirectOss = 2,
}
