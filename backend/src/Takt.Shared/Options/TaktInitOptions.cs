// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktInitOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：启动初始化开关（appsettings Init 节；数据范围见 TaktDatabaseOptions）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 启动初始化开关（<c>Init</c> 节，仅控制是否执行各步骤；租户/公司/种子源在 TaktDatabaseOptions）
/// </summary>
public class TaktInitOptions
{
    public const string SectionName = "Init";

    /// <summary>
    /// 是否按 TaktDatabaseOptions.TenantCodes 建库建表
    /// </summary>
    public bool InitDb { get; set; }

    /// <summary>
    /// 是否执行业务种子（范围由 TaktDatabaseOptions.TenantCodes、TaktDatabaseOptions.CompanyCodes 决定）
    /// </summary>
    public bool SeedData { get; set; }
}
