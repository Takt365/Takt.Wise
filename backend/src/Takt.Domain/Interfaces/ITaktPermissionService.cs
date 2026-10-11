// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktPermissionService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：权限服务接口，提供数据权限验证能力
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 权限服务接口
/// 提供数据权限验证能力
/// </summary>
public interface ITaktPermissionService
{
    /// <summary>
    /// 检查用户是否有权限访问指定公司
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>是否有权限</returns>
    Task<bool> HasCompanyAccessAsync(long userId, string tenantCode, string companyCode);

    /// <summary>
    /// 获取用户可访问的公司列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>公司编码列表</returns>
    Task<List<string>> GetAccessibleCompaniesAsync(long userId, string tenantCode);

    /// <summary>
    /// 检查用户是否有指定权限类型
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="permissionType">权限类型</param>
    /// <returns>是否有权限</returns>
    Task<bool> HasPermissionAsync(long userId, string tenantCode, string companyCode, TaktPermissionType permissionType);

    /// <summary>
    /// 获取用户的数据权限范围
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>数据权限范围</returns>
    Task<int> GetDataScopeAsync(long userId, string tenantCode, string companyCode);
}
