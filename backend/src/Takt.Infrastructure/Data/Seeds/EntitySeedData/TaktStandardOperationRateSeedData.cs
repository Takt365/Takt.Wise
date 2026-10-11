// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktStandardOperationRateSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率种子（FY2000～FY2099；类型1人员稼动率0.85；生产工厂 C100/J100；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 标准生产稼动率种子（FY2000～FY2099；生产工厂 C100/J100；类型 1 人员；稼动率 0.85）
/// </summary>
public class TaktStandardOperationRateSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int OperationTypePersonnel = 1;
    private const decimal DefaultOperationRate = 0.85m;
    private const int FiscalYearStart = 2000;
    private const int FiscalYearEnd = 2099;

    private static readonly HashSet<string> ManufacturingPlantCodes = new(StringComparer.Ordinal)
    {
        "C100",
        "J100",
    };

    /// <summary>
    /// 执行顺序（生产班组种子之后）
    /// </summary>
    public int Order => 491;

    /// <summary>
    /// 初始化标准生产稼动率种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化标准生产稼动率种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过标准生产稼动率种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktStandardOperationRate>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过标准生产稼动率种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过标准生产稼动率种子", tenantCode);
            return (0, 0);
        }
        var fiscalYears = GetFiscalYearDefinitions();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化标准生产稼动率（FY{Start}～FY{End}）...",
            tenantCode,
            FiscalYearStart,
            FiscalYearEnd);
        foreach (var company in orderedCompanies)
        {
            string plantCode;
            try
            {
                plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            }
            catch (InvalidOperationException ex)
            {
                TaktLogger.Warning(
                    "公司 {CompanyCode} 未映射工厂，跳过标准生产稼动率种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            if (!ManufacturingPlantCodes.Contains(plantCode))
            {
                TaktLogger.Information(
                    "公司 {CompanyCode} 工厂 {PlantCode} 非制造工厂，跳过标准生产稼动率种子",
                    company.CompanyCode,
                    plantCode);
                continue;
            }
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化标准生产稼动率...",
                company.CompanyCode,
                plantCode);
            foreach (var fiscalYear in fiscalYears)
            {
                var (_, inserted, updated) = await CreateOrUpdateStandardOperationRateAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    plantCode,
                    fiscalYear);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "标准生产稼动率种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 财务年度定义（FY2000=1999/4/1～2000/3/31；FY2027=2026/4/1～2027/3/31）
    /// </summary>
    /// <returns>财务年度种子项列表</returns>
    private static List<FiscalYearSeedItem> GetFiscalYearDefinitions()
    {
        var items = new List<FiscalYearSeedItem>();
        for (var year = FiscalYearStart; year <= FiscalYearEnd; year++)
        {
            items.Add(new FiscalYearSeedItem(
                $"FY{year}",
                year.ToString(),
                new DateTime(year - 1, 4, 1),
                new DateTime(year, 3, 31)));
        }
        return items;
    }

    /// <summary>
    /// 创建或更新标准生产稼动率
    /// </summary>
    /// <param name="repository">标准生产稼动率种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂编码</param>
    /// <param name="fiscalYear">财务年度定义</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktStandardOperationRate Rate, int InsertCount, int UpdateCount)> CreateOrUpdateStandardOperationRateAsync(
        ITaktCompanySeedRepository<TaktStandardOperationRate> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        string plantCode,
        FiscalYearSeedItem fiscalYear)
    {
        var fyCode = fiscalYear.FinancialYear;
        var rate = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plantCode
            && x.OperationType == OperationTypePersonnel
            && (x.FinancialYear == fyCode || x.FinancialYear == fiscalYear.LegacyFinancialYearSuffix));
        if (rate == null)
        {
            rate = new TaktStandardOperationRate
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                FinancialYear = fiscalYear.FinancialYear,
                OperationType = OperationTypePersonnel,
                OperationRate = DefaultOperationRate,
                EffectiveDate = fiscalYear.EffectiveDate,
                ExpiryDate = fiscalYear.ExpiryDate,
                RateStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            rate = await repository.CreateAsync(rate);
            return (rate, 1, 0);
        }
        var needUpdate = false;
        if (rate.FinancialYear != fyCode)
        {
            rate.FinancialYear = fyCode;
            needUpdate = true;
        }
        if (rate.OperationRate != DefaultOperationRate
            || TaktProductionStatHelper.NormalizeStandardOperationRate(rate.OperationRate) != DefaultOperationRate)
        {
            rate.OperationRate = DefaultOperationRate;
            needUpdate = true;
        }
        if (rate.EffectiveDate.Date != fiscalYear.EffectiveDate.Date)
        {
            rate.EffectiveDate = fiscalYear.EffectiveDate;
            needUpdate = true;
        }
        if (rate.ExpiryDate?.Date != fiscalYear.ExpiryDate.Date)
        {
            rate.ExpiryDate = fiscalYear.ExpiryDate;
            needUpdate = true;
        }
        if (rate.RateStatus != StatusEnabled)
        {
            rate.RateStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(rate);
            return (rate, 0, 1);
        }
        return (rate, 0, 0);
    }

    /// <summary>
    /// 财务年度种子项（FinancialYear 存 FY 编码 6 位，如 FY2027=2026/4/1～2027/3/31）
    /// </summary>
    /// <param name="FinancialYear">财务年度编码（如 FY2027）</param>
    /// <param name="LegacyFinancialYearSuffix">历史 4 位末年年号（迁移用，如 2027）</param>
    /// <param name="EffectiveDate">生效日期（FY 起始日：上年 4/1，如 FY2027 为 2026/4/1）</param>
    /// <param name="ExpiryDate">失效日期（FY 结束日：当年 3/31，如 FY2027 为 2027/3/31）</param>
    private sealed record FiscalYearSeedItem(
        string FinancialYear,
        string LegacyFinancialYearSuffix,
        DateTime EffectiveDate,
        DateTime ExpiryDate);
}
