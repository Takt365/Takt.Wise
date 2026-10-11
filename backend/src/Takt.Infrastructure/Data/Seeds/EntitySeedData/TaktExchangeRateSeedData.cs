// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktExchangeRateSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：汇率种子（租户级）；计划/预算/预测 P 类：TAC/DTA/TSZ FY2027-1 BUDGET（2026/04～2026/09）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 汇率种子数据（租户级；租户内各公司共用）
/// 幂等：按租户 + 源币种 + 目标币种 + 汇率类型 + 生效日 匹配后更新
/// </summary>
public class TaktExchangeRateSeedData : ITaktSeedDataCoordinator
{
    private const string RateTypePlan = "P";

    private static readonly DateTime PlanBudgetValidFrom = new(2026, 4, 1);

    private static readonly DateTime PlanBudgetValidTo = new(2026, 9, 30, 23, 59, 59);

    private const string PlanBudgetRemark = "TAC/DTA/TSZ FY2027-1 BUDGET（2026/04～2026/09）";

    /// <summary>
    /// 执行顺序（字典数据之后、假日种子之前）
    /// </summary>
    public int Order => 461;

    /// <summary>
    /// 初始化汇率种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化汇率种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过汇率种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktExchangeRate>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化汇率数据...", tenantCode);

        foreach (var seed in GetPlanBudgetExchangeRates())
        {
            var (_, inserted, updated) = await CreateOrUpdateExchangeRateAsync(repository, tenantCode, seed);
            insertCount += inserted;
            updateCount += updated;
        }

        TaktLogger.Information("汇率种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 计划/预算/预测汇率（类型 P；RMB 按字典 accounting_financial_currency_code 存为 CNY）
    /// 来源：TAC/DTA/TSZ FY2027-1 BUDGET，生效日 2026-04-01
    /// </summary>
    private static List<TaktExchangeRateSeedItem> GetPlanBudgetExchangeRates()
    {
        return
        [
            new("CNY", "HKD", RateTypePlan, 1.170m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("CNY", "USD", RateTypePlan, 0.152m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("CNY", "JPY", RateTypePlan, 22.003m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("USD", "HKD", RateTypePlan, 7.750m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("USD", "CNY", RateTypePlan, 6.590m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("USD", "JPY", RateTypePlan, 145.000m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("JPY", "HKD", RateTypePlan, 0.053m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("JPY", "CNY", RateTypePlan, 0.0454m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("JPY", "USD", RateTypePlan, 0.0069m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("HKD", "CNY", RateTypePlan, 0.850m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("HKD", "USD", RateTypePlan, 0.129m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
            new("HKD", "JPY", RateTypePlan, 18.703m, PlanBudgetValidFrom, PlanBudgetValidTo, PlanBudgetRemark),
        ];
    }

    /// <summary>
    /// 创建或更新单条汇率
    /// </summary>
    private static async Task<(TaktExchangeRate Entity, int InsertCount, int UpdateCount)> CreateOrUpdateExchangeRateAsync(
        ITaktTenantSeedRepository<TaktExchangeRate> repository,
        string tenantCode,
        TaktExchangeRateSeedItem seed)
    {
        var entity = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.FromCurrencyCode == seed.FromCurrencyCode
            && x.ToCurrencyCode == seed.ToCurrencyCode
            && x.ExchangeRateType == seed.ExchangeRateType
            && x.ValidFrom == seed.ValidFrom);

        if (entity == null)
        {
            entity = new TaktExchangeRate
            {
                TenantCode = tenantCode,
            };
            ApplySeedFields(entity, seed);
            await repository.CreateAsync(entity);
            return (entity, 1, 0);
        }

        ApplySeedFields(entity, seed);
        await repository.UpdateAsync(entity);
        return (entity, 0, 1);
    }

    /// <summary>
    /// 写入种子字段
    /// </summary>
    private static void ApplySeedFields(TaktExchangeRate entity, TaktExchangeRateSeedItem seed)
    {
        entity.FromCurrencyCode = seed.FromCurrencyCode;
        entity.ToCurrencyCode = seed.ToCurrencyCode;
        entity.ExchangeRateType = seed.ExchangeRateType;
        entity.ExchangeRate = seed.ExchangeRate;
        entity.RatioFrom = 1;
        entity.RatioTo = 1;
        entity.ValidFrom = seed.ValidFrom;
        entity.ValidTo = seed.ValidTo;
        entity.ExchangeRateStatus = 1;
        entity.Remark = seed.Remark;
    }

    /// <summary>
    /// 汇率种子项
    /// </summary>
    /// <param name="FromCurrencyCode">源币种（字典 accounting_financial_currency_code）</param>
    /// <param name="ToCurrencyCode">目标币种</param>
    /// <param name="ExchangeRateType">汇率类型（字典 accounting_financial_exchange_rate_type）</param>
    /// <param name="ExchangeRate">直接标价：1 源币种 = ExchangeRate 目标币种</param>
    /// <param name="ValidFrom">生效日期</param>
    /// <param name="ValidTo">失效日期</param>
    /// <param name="Remark">备注（预算版本说明）</param>
    private sealed record TaktExchangeRateSeedItem(
        string FromCurrencyCode,
        string ToCurrencyCode,
        string ExchangeRateType,
        decimal ExchangeRate,
        DateTime ValidFrom,
        DateTime ValidTo,
        string Remark);
}
