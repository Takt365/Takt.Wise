// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktSeedDataCoordinator.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据协调器接口，定义种子数据初始化标准
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 种子数据协调器接口
/// 所有种子数据类必须实现此接口，与 TaktSeedDataCoordinator 配对
/// </summary>
public interface ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（数字越小越先执行）
    /// </summary>
    int Order { get; }

    /// <summary>
    /// 初始化种子数据（幂等性：存在则更新，不存在则创建）
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（可选，用于租户级实体种子数据）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null);
}
