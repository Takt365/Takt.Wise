// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcAttachmentStorageConstants.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件默认存储路径常量（一级菜单后勤管理 uploads/logistics + /ec；引擎再拼租户/公司/日期）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变附件物理存储路径约定。完整相对路径为
/// uploads/logistics/ec/{租户码}/{公司码}/{年}/{月}/{日}/{文件名}。
/// </summary>
public static class TaktEcAttachmentStorageConstants
{
    /// <summary>
    /// 一级菜单「后勤管理」对应上传根路径（TaktFile.StorageConfig.uploadPath）
    /// </summary>
    public const string MenuUploadPath = "uploads/logistics";

    /// <summary>
    /// 上传引擎 CategoryPath（去掉 uploads/ 前缀后加 /ec；引擎再追加租户、公司、日期）
    /// </summary>
    public const string CategoryPath = "logistics/ec";
}
