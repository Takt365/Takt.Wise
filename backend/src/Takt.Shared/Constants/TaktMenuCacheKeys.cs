// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktMenuCacheKeys.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单 Redis 缓存键前缀与键名拼接（仅 string const/方法，非枚举）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 菜单缓存键（供 TaktMenuService 使用）
/// </summary>
public static class TaktMenuCacheKeys
{
    /// <summary>
    /// 缓存键前缀
    /// </summary>
    public const string Prefix = "takt:menu:";

    /// <summary>
    /// 租户全量菜单列表缓存键
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>缓存键</returns>
    public static string TenantAll(string tenantCode) =>
        $"{Prefix}tenant:{tenantCode}:all";

    /// <summary>
    /// 单条菜单 DTO 缓存键（仅 IsCached=1 的菜单写入）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="menuId">菜单 ID</param>
    /// <returns>缓存键</returns>
    public static string ById(string tenantCode, long menuId) =>
        $"{Prefix}tenant:{tenantCode}:id:{menuId}";
}
