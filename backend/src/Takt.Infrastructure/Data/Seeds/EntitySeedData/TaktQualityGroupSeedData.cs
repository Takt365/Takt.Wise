// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktQualityGroupSeedData.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：质量组主数据种子（IQC组/QA组/IPQC组 各类别各 01/02 演示目录；责任组、处理组为字典类别；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 质量组主数据种子（IQC组、QA组、IPQC组 各写入 01/02；责任组、处理组见字典 logistics_quality_group_category）
/// </summary>
public class TaktQualityGroupSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;

    /// <summary>
    /// 检查类别：IQC
    /// </summary>
    private const int QualityGroupCategoryIqc = 0;

    /// <summary>
    /// 检查类别：QA
    /// </summary>
    private const int QualityGroupCategoryQa = 1;

    /// <summary>
    /// 检查类别：IPQC
    /// </summary>
    private const int QualityGroupCategoryIpqc = 2;

    /// <summary>
    /// 执行顺序（销售组种子之后）
    /// </summary>
    public int Order => 11;

    /// <summary>
    /// 初始化质量组主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化质量组主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过质量组主数据种子初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktQualityGroup>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过质量组主数据种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过质量组主数据种子", tenantCode);
            return (0, 0);
        }
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化质量组主数据...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过质量组主数据种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            var templates = GetStandardQualityGroups(plantCode);
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化质量组主数据（{Count} 条）...",
                company.CompanyCode,
                plantCode,
                templates.Count);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateQualityGroupAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "质量组主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准质量组目录（PlantCode + QualityGroupCategory + QualityGroupCode 唯一；每类别各 01/02）
    /// </summary>
    /// <param name="plantCode">工厂编码</param>
    /// <returns>质量组种子项列表</returns>
    private static List<QualityGroupSeedItem> GetStandardQualityGroups(string plantCode)
    {
        var codePrefix = plantCode[0];
        return
        [
            // IQC
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryIqc, $"{codePrefix}01", "张三", null, 1),
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryIqc, $"{codePrefix}02", "李四", null, 2),
            // QA
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryQa, $"{codePrefix}01", "张三", null, 1),
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryQa, $"{codePrefix}02", "李四", null, 2),
            // IPQC
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryIpqc, $"{codePrefix}01", "张三", null, 1),
            new QualityGroupSeedItem(plantCode, QualityGroupCategoryIpqc, $"{codePrefix}02", "李四", null, 2),
        ];
    }

    /// <summary>
    /// 创建或更新质量组主数据
    /// </summary>
    /// <param name="repository">质量组种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktQualityGroup Group, int InsertCount, int UpdateCount)> CreateOrUpdateQualityGroupAsync(
        ITaktCompanySeedRepository<TaktQualityGroup> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        QualityGroupSeedItem seed)
    {
        var group = await repository.FirstAsync(g =>
            g.TenantCode == tenantCode
            && g.CompanyCode == companyCode
            && g.PlantCode == seed.PlantCode
            && g.QualityGroupCategory == seed.QualityGroupCategory
            && g.QualityGroupCode == seed.QualityGroupCode);
        if (group == null)
        {
            group = new TaktQualityGroup
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = seed.PlantCode,
                QualityGroupCategory = seed.QualityGroupCategory,
                QualityGroupCode = seed.QualityGroupCode,
                QualityGroupName = seed.QualityGroupName,
                QualityGroupDescription = seed.QualityGroupDescription,
                IsBuiltIn = IsBuiltInYes,
                SortOrder = seed.SortOrder,
                GroupStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            group = await repository.CreateAsync(group);
            return (group, 1, 0);
        }
        var needUpdate = false;
        if (group.QualityGroupName != seed.QualityGroupName)
        {
            group.QualityGroupName = seed.QualityGroupName;
            needUpdate = true;
        }
        if (group.QualityGroupDescription != seed.QualityGroupDescription)
        {
            group.QualityGroupDescription = seed.QualityGroupDescription;
            needUpdate = true;
        }
        if (group.SortOrder != seed.SortOrder)
        {
            group.SortOrder = seed.SortOrder;
            needUpdate = true;
        }
        if (group.IsBuiltIn != IsBuiltInYes)
        {
            group.IsBuiltIn = IsBuiltInYes;
            needUpdate = true;
        }
        if (group.GroupStatus != StatusEnabled)
        {
            group.GroupStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(group);
            return (group, 0, 1);
        }
        return (group, 0, 0);
    }

    /// <summary>
    /// 质量组种子项
    /// </summary>
    /// <param name="PlantCode">工厂编码</param>
    /// <param name="QualityGroupCategory">质量组类别</param>
    /// <param name="QualityGroupCode">质量组编码</param>
    /// <param name="QualityGroupName">质量组名称</param>
    /// <param name="QualityGroupDescription">质量组描述</param>
    /// <param name="SortOrder">排序号</param>
    private sealed record QualityGroupSeedItem(
        string PlantCode,
        int QualityGroupCategory,
        string QualityGroupCode,
        string QualityGroupName,
        string? QualityGroupDescription,
        int SortOrder);
}
