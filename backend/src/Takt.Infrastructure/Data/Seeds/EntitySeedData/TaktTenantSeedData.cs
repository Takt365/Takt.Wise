// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktTenantSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：租户种子数据初始化（DEV/QAS/PRD三个环境）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 租户种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktTenantSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（租户最先初始化）
    /// </summary>
    public int Order => 1;

    /// <summary>
    /// 初始化租户种子数据
    /// 注意：每个租户数据库只初始化自己的租户记录（1条）
    /// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户创建记录
    /// 示例：Takt_000_DEV 中只有租户 000 的1条记录
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCodeParam = null)
    {
        TaktLogger.Information("开始初始化租户种子数据...");
        int insertCount = 0;
        int updateCount = 0;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var tenantCodes = configuration.GetTenantCodes();
        var seedContext = serviceProvider.GetRequiredService<Takt.Infrastructure.Data.Context.TaktSeedContext>();
        for (var index = 0; index < tenantCodes.Count; index++)
        {
            var code = tenantCodes[index];
            var tenantIndex = index + 1;
            var tenantTotal = tenantCodes.Count;
            TaktLogger.Information(
                "正在初始化租户 {TenantCode} ({Index}/{Total})...",
                code,
                tenantIndex,
                tenantTotal);
            seedContext.SwitchTenant(code);
            
            // 使用 ITaktSeedRepository（现在连接到正确的租户数据库）
            var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTenant>>();

            // 完全依赖传入参数，使用动态模板生成默认值
            var (tenant, i, u) = await CreateOrUpdateTenantAsync(
                repository, 
                code, 
                $"租户 {code}",  // 动态生成名称
                "系统管理员",        // 默认联系人
                "13800000000",       // 默认电话
                $"admin@takt.com");  // 动态生成邮箱
            
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("租户种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新租户
    /// </summary>
    private static async Task<(TaktTenant Tenant, int InsertCount, int UpdateCount)> CreateOrUpdateTenantAsync(
        ITaktTenantSeedRepository<TaktTenant> repository,
        string tenantCode,
        string tenantName,
        string contactName,
        string contactPhone,
        string contactEmail)
    {
        var tenant = await repository.FirstAsync(t => t.TenantCode == tenantCode);
        
        if (tenant == null)
        {
            // 不存在：创建新记录（仓储会自动生成雪花ID和审计字段）
            tenant = new TaktTenant
            {
                TenantCode = tenantCode,
                TenantName = tenantName,
                IsBuiltIn = 1,
                TenantStatus = 1,
                SubscriptionStartTime = DateTime.Now,
                SubscriptionEndTime = new DateTime(9999, 12, 31, 23, 59, 59),
                ContactName = contactName,
                ContactPhone = contactPhone,
                ContactEmail = contactEmail,
                Remark = TaktZeroCharHelper.BrandTakt365
            };
            tenant = await repository.CreateAsync(tenant);
            return (tenant, 1, 0);
        }
        else
        {
            // 存在：更新记录
            tenant.TenantName = tenantName;
            tenant.IsBuiltIn = 1;
            tenant.TenantStatus = 1;
            tenant.SubscriptionStartTime = DateTime.Now;
            tenant.SubscriptionEndTime = new DateTime(9999, 12, 31, 23, 59, 59);
            tenant.ContactName = contactName;
            tenant.ContactPhone = contactPhone;
            tenant.ContactEmail = contactEmail;
            tenant.Remark = TaktZeroCharHelper.BrandTakt365;

            await repository.UpdateAsync(tenant);
            return (tenant, 0, 1);
        }
    }
}
