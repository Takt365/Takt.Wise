// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktEcGroupSeedData.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变组主数据种子（部门种子之后；EcGroupCategory 写入部门编码 DeptCode；描述/电话/邮箱写入非空；姓名循环张三/李四/王五/赵六）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 设变组主数据种子（Order 在部门之后；EcGroupCategory 为部门编码）
/// </summary>
public class TaktEcGroupSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;

    /// <summary>
    /// 执行顺序（部门种子 Order=30 之后）
    /// </summary>
    public int Order => 31;

    /// <summary>
    /// 初始化设变组主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化设变组主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过设变组主数据种子初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEcGroup>>();
        var deptRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDept>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过设变组主数据种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过设变组主数据种子", tenantCode);
            return (0, 0);
        }
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化设变组主数据...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过设变组主数据种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            var depts = await deptRepository.GetListAsync(g =>
                g.TenantCode == tenantCode
                && g.CompanyCode == company.CompanyCode
                && g.PlantCode == plantCode
                && g.IsDeleted == 0);
            var templates = BuildEcGroupsFromDepts(depts);
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化设变组主数据（{Count} 条）...",
                company.CompanyCode,
                plantCode,
                templates.Count);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateEcGroupAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "设变组主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 担当姓名循环（每个类别都从张三开始）
    /// </summary>
    private static readonly string[] PersonNames = ["张三", "李四", "王五", "赵六","孙七"];

    /// <summary>
    /// 按已写入的部门生成担当（技术课及原类别 2～10；部门不存在则跳过）
    /// </summary>
    /// <param name="depts">本公司本工厂的部门</param>
    /// <returns>设变组种子项列表</returns>
    private static List<EcGroupSeedItem> BuildEcGroupsFromDepts(IReadOnlyList<TaktDept> depts)
    {
        var byCode = new Dictionary<string, TaktDept>(StringComparer.OrdinalIgnoreCase);
        foreach (var dept in depts)
        {
            var code = dept.DeptCode?.Trim();
            if (string.IsNullOrEmpty(code) || byCode.ContainsKey(code))
            {
                continue;
            }
            byCode[code] = dept;
        }
        (int Category, int CodeFrom, int CodeTo)[] blocks =
        [
            (TaktEcGroupCategoryConstants.EcOwner, 1001, 1005),
            (TaktEcGroupCategoryConstants.Sop, 1006, 1010),
            (TaktEcGroupCategoryConstants.Pc, 1011, 1015),
            (TaktEcGroupCategoryConstants.Pur, 1016, 1020),
            (TaktEcGroupCategoryConstants.Mc, 1021, 1025),
            (TaktEcGroupCategoryConstants.Iqc, 1026, 1030),
            (TaktEcGroupCategoryConstants.Mtl, 1031, 1035),
            (TaktEcGroupCategoryConstants.Mfg2, 1036, 1040),
            (TaktEcGroupCategoryConstants.Mfg1, 1041, 1045),
            (TaktEcGroupCategoryConstants.Qc, 1046, 1050),
        ];
        var list = new List<EcGroupSeedItem>();
        foreach (var block in blocks)
        {
            var deptCode = TaktEcGroupCategoryConstants.ToDeptCode(block.Category);
            if (string.IsNullOrEmpty(deptCode) || !byCode.TryGetValue(deptCode, out var dept))
            {
                continue;
            }
            AppendEcGroupBlock(list, dept, deptCode, block.CodeFrom, block.CodeTo);
        }
        return list;
    }

    /// <summary>
    /// 追加一个部门下的连续编码，姓名按张三、李四、王五、赵六循环
    /// </summary>
    /// <param name="list">种子项</param>
    /// <param name="dept">已存在的部门</param>
    /// <param name="deptCode">部门编码（写入 EcGroupCategory）</param>
    /// <param name="codeFrom">起始数字（含）</param>
    /// <param name="codeTo">结束数字（含）</param>
    private static void AppendEcGroupBlock(
        List<EcGroupSeedItem> list,
        TaktDept dept,
        string deptCode,
        int codeFrom,
        int codeTo)
    {
        var sortOrder = 1;
        for (var code = codeFrom; code <= codeTo; code++)
        {
            var personName = PersonNames[(sortOrder - 1) % PersonNames.Length];
            var deptName = string.IsNullOrWhiteSpace(dept.DeptName1) ? deptCode : dept.DeptName1.Trim();
            list.Add(new EcGroupSeedItem(
                dept.PlantCode,
                $"D{code}",
                personName,
                $"{deptName}担当",
                $"138{code:D8}",
                $"d{code}@takt.cn",
                deptCode,
                sortOrder));
            sortOrder++;
        }
    }

    /// <summary>
    /// 创建或更新设变组主数据
    /// </summary>
    /// <param name="repository">设变组种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="cultureCode">区域文化</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktEcGroup Group, int InsertCount, int UpdateCount)> CreateOrUpdateEcGroupAsync(
        ITaktCompanySeedRepository<TaktEcGroup> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        EcGroupSeedItem seed)
    {
        var group = await repository.FirstAsync(g =>
            g.TenantCode == tenantCode
            && g.CompanyCode == companyCode
            && g.PlantCode == seed.PlantCode
            && g.EcGroupCode == seed.EcGroupCode);
        if (group == null)
        {
            group = new TaktEcGroup
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = seed.PlantCode,
                EcGroupCode = seed.EcGroupCode,
                EcGroupName = seed.EcGroupName,
                EcGroupDescription = seed.EcGroupDescription,
                ContactPhone = seed.ContactPhone,
                ContactEmail = seed.ContactEmail,
                EcGroupCategory = seed.EcGroupCategory,
                IsBuiltIn = IsBuiltInYes,
                SortOrder = seed.SortOrder,
                GroupStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            group = await repository.CreateAsync(group);
            return (group, 1, 0);
        }
        var needUpdate = false;
        if (group.EcGroupName != seed.EcGroupName)
        {
            group.EcGroupName = seed.EcGroupName;
            needUpdate = true;
        }
        if (group.EcGroupDescription != seed.EcGroupDescription)
        {
            group.EcGroupDescription = seed.EcGroupDescription;
            needUpdate = true;
        }
        if (group.ContactPhone != seed.ContactPhone)
        {
            group.ContactPhone = seed.ContactPhone;
            needUpdate = true;
        }
        if (group.ContactEmail != seed.ContactEmail)
        {
            group.ContactEmail = seed.ContactEmail;
            needUpdate = true;
        }
        if (group.EcGroupCategory != seed.EcGroupCategory)
        {
            group.EcGroupCategory = seed.EcGroupCategory;
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
    /// 设变组种子项
    /// </summary>
    /// <param name="PlantCode">工厂编码</param>
    /// <param name="EcGroupCode">设变组编码</param>
    /// <param name="EcGroupName">设变组名称</param>
    /// <param name="EcGroupDescription">设变组描述（部门名称 + 担当）</param>
    /// <param name="ContactPhone">联系电话</param>
    /// <param name="ContactEmail">联系邮箱</param>
    /// <param name="EcGroupCategory">设变组类别（部门编码 DeptCode）</param>
    /// <param name="SortOrder">排序号</param>
    private sealed record EcGroupSeedItem(
        string PlantCode,
        string EcGroupCode,
        string EcGroupName,
        string EcGroupDescription,
        string ContactPhone,
        string ContactEmail,
        string EcGroupCategory,
        int SortOrder);
}
