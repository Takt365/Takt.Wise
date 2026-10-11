// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktSeedUserContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据专用用户上下文（固定租户/系统用户，无 HTTP）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktUserContext 种子数据实现
/// 在种子初始化阶段使用，不依赖 HTTP 请求头；租户由协调器或 Create 传入
/// </summary>
public class TaktSeedUserContext : ITaktUserContext
{
    /// <summary>
    /// 系统种子用户 ID（与种子数据审计字段一致）
    /// </summary>
    private const long SeedUserId = 900001;

    /// <summary>
    /// 系统种子用户名
    /// </summary>
    private const string SeedUserName = "SystemSeed";

    /// <summary>
    /// 当前租户编码（由种子数据协调器或 Create 传入）
    /// </summary>
    public string? TenantCode { get; set; }

    /// <summary>
    /// 当前公司编码（种子数据阶段通常为空）
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// 请求头中的公司编码（种子阶段与 CompanyCode 一致）
    /// </summary>
    public string? RequestCompanyCode => CompanyCode;

    /// <summary>
    /// 当前用户ID（种子阶段固定为系统初始化用户 900001）
    /// </summary>
    public long? UserId => SeedUserId;

    /// <summary>
    /// 当前用户名（SystemSeed）
    /// </summary>
    public string? UserName => SeedUserName;

    /// <summary>
    /// 是否已认证（种子数据阶段恒为 true）
    /// </summary>
    public bool IsAuthenticated => true;

    /// <summary>
    /// 创建种子数据用户上下文
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码（可选）</param>
    /// <returns>种子数据用户上下文实例</returns>
    public static TaktSeedUserContext Create(string tenantCode, string? companyCode = null)
    {
        return new TaktSeedUserContext
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
        };
    }
}
