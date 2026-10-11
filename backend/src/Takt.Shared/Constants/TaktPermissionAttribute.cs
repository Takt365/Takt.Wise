// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktPermissionAttribute.cs
// 创建时间：2026-05-20
// 创建人：Takt365(Cursor AI)
// 功能描述：API 权限特性（Constants 层 Attribute；权限码为 string，非枚举）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// API 权限特性
/// 用于标记控制器或操作所需的权限标识
/// 格式：领域:目录:实体:操作（如：accounting:controlling:costcenter:list）
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class TaktPermissionAttribute : Attribute
{
    /// <summary>
    /// 获取权限标识
    /// 格式：领域:目录:实体:操作
    /// </summary>
    public string PermissionCode { get; }

    /// <summary>
    /// 获取权限显示名称
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// 初始化权限特性
    /// </summary>
    /// <param name="permissionCode">权限标识（如：accounting:controlling:costcenter:list）</param>
    /// <param name="displayName">权限显示名称</param>
    public TaktPermissionAttribute(string permissionCode, string displayName)
    {
        PermissionCode = permissionCode ?? throw new ArgumentNullException(nameof(permissionCode));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
    }
}
