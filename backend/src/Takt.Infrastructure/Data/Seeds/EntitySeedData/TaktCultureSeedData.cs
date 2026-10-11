// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktCultureSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：区域文化种子数据初始化（图标与前端 flag-icons ^7.5.0 一致：fi-xx → fi fi-xx）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 区域文化种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktCultureSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序
    /// </summary>
    public int Order => 40;

    /// <summary>
    /// 初始化区域文化种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化区域文化种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过区域文化种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化区域文化数据...", tenantCode);

        foreach (var cultureData in GetStandardCultures())
        {
            var (_, i, u) = await CreateOrUpdateCultureAsync(repository, tenantCode, cultureData);

            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("区域文化种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准区域文化（与前端 takt-locale-flag / flag-icons 对齐）
    /// Icon 存 fi-xx，前端 resolveCultureFlagClass 会规范为 fi fi-xx
    /// NativeName 为本族语展示；Remark 为中文说明
    /// </summary>
    private static List<TaktCultureSeedItem> GetStandardCultures()
    {
        return
        [
            new TaktCultureSeedItem("en-US", "English(US)", "fi-us", 1, true, "区域文化编码.英语(美国)"),
            new TaktCultureSeedItem("ja-JP", "日本語(JP)", "fi-jp", 2, false, "区域文化编码.日语(日本)"),
            new TaktCultureSeedItem("zh-HK", "中文(香港)", "fi-hk", 3, false, "区域文化编码.中文(香港)"),
            new TaktCultureSeedItem("zh-CN", "中文(简体)", "fi-cn", 4, false, "区域文化编码.中文(简体)"),
        ];
    }

    /// <summary>
    /// 创建或更新区域文化
    /// </summary>
    private static async Task<(TaktCulture Culture, int InsertCount, int UpdateCount)> CreateOrUpdateCultureAsync(
        ITaktTenantSeedRepository<TaktCulture> repository,
        string tenantCode,
        TaktCultureSeedItem seed)
    {
        var culture = await repository.FirstAsync(c => c.TenantCode == tenantCode && c.CultureCode == seed.CultureCode);

        if (culture == null)
        {
            culture = new TaktCulture
            {
                TenantCode = tenantCode,
                CultureCode = seed.CultureCode,
                NativeName = seed.NativeName,
                Icon = seed.Icon,
                IsDefault = seed.IsDefault ? 1 : 0,
                SortOrder = seed.SortOrder,
                Remark = seed.Remark,
            };
            await repository.CreateAsync(culture);
            return (culture, 1, 0);
        }

        culture.NativeName = seed.NativeName;
        culture.Icon = seed.Icon;
        culture.IsDefault = seed.IsDefault ? 1 : 0;
        culture.SortOrder = seed.SortOrder;
        culture.Remark = seed.Remark;

        await repository.UpdateAsync(culture);
        return (culture, 0, 1);
    }

    /// <summary>
    /// 区域文化种子项
    /// </summary>
    /// <param name="CultureCode">文化编码</param>
    /// <param name="NativeName">本地化名称（本族语）</param>
    /// <param name="Icon">flag-icons 类后缀（fi-cn / fi-us / fi-jp / fi-hk）</param>
    /// <param name="SortOrder">排序号</param>
    /// <param name="IsDefault">默认语言</param>
    /// <param name="Remark">中文备注说明</param>
    private sealed record TaktCultureSeedItem(
        string CultureCode,
        string NativeName,
        string Icon,
        int SortOrder,
        bool IsDefault,
        string Remark);
}
