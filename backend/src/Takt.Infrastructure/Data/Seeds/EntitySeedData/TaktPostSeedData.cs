// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktPostSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位种子数据初始化
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 岗位种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktPostSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在部门之后，语言之前）
    /// </summary>
    public int Order => 35;

    /// <summary>
    /// 初始化岗位种子数据
    /// 注意：每个租户数据库只初始化自己的岗位数据
    /// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化岗位
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化岗位种子数据...");

        // 参数验证：必须使用协调器传入的租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过岗位种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktPost>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<Takt.Domain.Entities.Accounting.Financial.TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;

        int insertCount = 0;
        int updateCount = 0;

        var companies = await companyRepository.GetListAsync(c => c.TenantCode == tenantCode && c.CompanyStatus == 1);

        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过岗位种子数据初始化", tenantCode);
            return (0, 0);
        }

        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        TaktLogger.Information("正在为租户 {TenantCode} 初始化岗位数据...", tenantCode);

        foreach (var company in orderedCompanies)
        {
            TaktLogger.Information("正在为公司 {CompanyCode} ({CompanyName1}) 初始化岗位...", company.CompanyCode, company.CompanyName1);
            
            var posts = GetStandardPosts(tenantCode, company.CompanyCode);
            var deptRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<Takt.Domain.Entities.HumanResource.Organization.TaktDept>>();
            
            foreach (var postData in posts)
            {
                var (post, isInserted) = await CreateOrUpdatePostAsync(
                    repository,
                    deptRepository,
                    tenantCode,
                    company.CompanyCode,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                    postData.PostCode,
                    postData.PostName,
                    postData.PostCategory,
                    postData.PostLevel,
                    postData.SortOrder,
                    postData.DeptCode);
                
                if (post != null)
                {
                    if (isInserted)
                        insertCount++;
                    else
                        updateCount++;
                }
            }
        }

        TaktLogger.Information("租户 {TenantCode} 岗位种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", 
            tenantCode, insertCount, updateCount);

        TaktLogger.Information("岗位种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 公司组织根部门编码（挂靠各公司组织根）
    /// </summary>
    /// <param name="companyCode">公司代码</param>
    /// <returns>组织根 DeptCode</returns>
    private static string ResolveOrgRootDeptCode(string companyCode) => companyCode switch
    {
        "1000" => "1000",
        "2300" => "2300",
        "2400" => "2400",
        _ => companyCode,
    };

    /// <summary>
    /// 获取标准岗位配置（挂靠各公司组织根）
    /// </summary>
    private static IEnumerable<(string PostCode, string PostName, string PostCategory, string PostLevel, int SortOrder, string DeptCode)> GetStandardPosts(string tenantCode, string companyCode)
    {
        var rootDeptCode = ResolveOrgRootDeptCode(companyCode);
        // PostCategory：字典 sys_post_category（MGT/PRO/TEC/SUP/OPS）；PostLevel：字典 sys_post_level（P1~P4 / M1~M5）
        return new[]
        {
            // ===== 管理岗 =====
            ("CHAIRMAN", "董事长", "MGT", "M5", 1, rootDeptCode),
            ("VICE_CHAIRMAN", "副董事长", "MGT", "M4", 2, rootDeptCode),
            ("GENERAL_MANAGER", "总经理", "MGT", "M5", 3, rootDeptCode),
            ("VICE_GENERAL_MANAGER", "副总经理", "MGT", "M4", 4, rootDeptCode),
            ("FACTORY_DIRECTOR", "厂长", "MGT", "M4", 5, rootDeptCode),
            ("BU_HEAD", "本部长", "MGT", "M4", 6, rootDeptCode),
            ("DEPUTY_BU_HEAD", "副本部长", "MGT", "M3", 7, rootDeptCode),
            ("DEPARTMENT_HEAD", "部长", "MGT", "M3", 8, rootDeptCode),
            ("DEPUTY_DEPARTMENT_HEAD", "副部长", "MGT", "M3", 9, rootDeptCode),
            ("MANAGER", "经理", "MGT", "M2", 10, rootDeptCode),
            ("DEPUTY_MANAGER", "副经理", "MGT", "M2", 11, rootDeptCode),
            ("SECTION_CHIEF", "课长", "MGT", "M1", 12, rootDeptCode),
            ("DEPUTY_SECTION_CHIEF", "副课长", "MGT", "M1", 13, rootDeptCode),
            ("SUBSECTION_CHIEF", "股长", "MGT", "M1", 14, rootDeptCode),
            ("TEAM_LEADER", "班长", "MGT", "M1", 15, rootDeptCode),
            ("DEPUTY_TEAM_LEADER", "副班长", "MGT", "M1", 16, rootDeptCode),
            // ===== 技术岗 =====
            ("LEVEL4_ENGINEER", "四级工程师", "TEC", "P4", 20, rootDeptCode),
            ("LEVEL3_ENGINEER", "三级工程师", "TEC", "P3", 21, rootDeptCode),
            ("LEVEL3_TECH_ENGINEER", "三级技术工程师", "TEC", "P3", 22, rootDeptCode),
            ("LEVEL2_ENGINEER", "二级工程师", "TEC", "P2", 23, rootDeptCode),
            ("LEVEL1_TECHNICIAN", "一级技术员", "TEC", "P1", 24, rootDeptCode),
            // ===== 专业岗 / 支持岗 =====
            ("LEVEL4_SPECIALIST", "四级专员", "PRO", "P4", 30, rootDeptCode),
            ("LEVEL3_SPECIALIST", "三级专员", "PRO", "P3", 31, rootDeptCode),
            ("LEVEL2_SPECIALIST", "二级专员", "PRO", "P2", 32, rootDeptCode),
            ("LEVEL1_SPECIALIST", "一级专员", "PRO", "P1", 33, rootDeptCode),
            ("LEVEL1_CLERK", "一级事务员", "SUP", "P1", 34, rootDeptCode),
            // ===== 操作岗 =====
            ("SENIOR_MULTI_SKILL_WORKER", "资深多能工", "OPS", "P4", 35, rootDeptCode),
            ("LEVEL1_MULTI_SKILL_WORKER", "一级多能工", "OPS", "P1", 36, rootDeptCode),
            ("LEVEL2_MULTI_SKILL_WORKER", "二级多能工", "OPS", "P2", 37, rootDeptCode),
            ("LEVEL3_MULTI_SKILL_WORKER", "三级多能工", "OPS", "P3", 38, rootDeptCode),
            ("OPERATOR", "作业员", "OPS", "P1", 60, rootDeptCode),
            ("INSPECTOR", "质检员", "OPS", "P2", 39, rootDeptCode),
            ("WAREHOUSE_KEEPER", "仓管员", "OPS", "P2", 40, rootDeptCode),
            ("SECURITY_CAPTAIN", "保安队长", "SUP", "M1", 50, rootDeptCode),
            ("SECURITY_DEPUTY_CAPTAIN", "保安副队长", "SUP", "M1", 51, rootDeptCode),
            ("LEVEL1_SECURITY_GUARD", "一级保安员", "SUP", "P1", 52, rootDeptCode),
            ("CLEANER", "清洁工", "SUP", "P1", 53, rootDeptCode),
        };
    }

    /// <summary>
    /// 创建或更新岗位
    /// </summary>
    /// <param name="repository">岗位仓储</param>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="postCode">岗位编码</param>
    /// <param name="postName">岗位名称</param>
    /// <param name="postCategory">岗位类别</param>
    /// <param name="postLevel">岗位职级</param>
    /// <param name="sortOrder">排序</param>
    /// <param name="deptCode">所属部门编码</param>
    /// <returns>(岗位实体, 是否为新插入)</returns>
    private static async Task<(TaktPost Post, bool IsInserted)> CreateOrUpdatePostAsync(
        ITaktCompanySeedRepository<TaktPost> repository,
        ITaktCompanySeedRepository<Takt.Domain.Entities.HumanResource.Organization.TaktDept> deptRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string postCode,
        string postName,
        string postCategory,
        string postLevel,
        int sortOrder,
        string deptCode)
    {
        // 根据部门编码查找部门ID；冗余 DeptName 按 DeptId 对应部门名称写入（Length=100）
        var dept = await deptRepository.FirstAsync(d => d.TenantCode == tenantCode && d.CompanyCode == companyCode && d.DeptCode == deptCode);
        if (dept == null)
        {
            TaktLogger.Warning("未找到部门 {DeptCode} (租户: {TenantCode}, 公司: {CompanyCode})，跳过岗位 {PostCode}", 
                deptCode, tenantCode, companyCode, postCode);
            return (null!, false);
        }
        var deptName = dept.DeptName1 ?? string.Empty;
        if (deptName.Length > 100)
        {
            deptName = deptName[..100];
        }

        var post = await repository.FirstAsync(p => p.TenantCode == tenantCode && p.CompanyCode == companyCode && p.PostCode == postCode);
        
        if (post == null)
        {
            // 不存在：创建新记录（仓储会自动生成雪花ID和审计字段）
            post = new TaktPost
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                DeptId = dept.Id,
                DeptName = deptName,
                PostCode = postCode,
                PostName = postName,
                PostCategory = postCategory,
                PostLevel = postLevel,
                Responsibilities = "待完善",
                Requirements = "待完善",
                EducationRequired = 1,
                ExperienceYears = 1,
                PostStatus = 1,
                SortOrder = sortOrder,
                IsBuiltIn = 1,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            post = await repository.CreateAsync(post);
            return (post, true);
        }
        else
        {
            // 存在：检查是否需要更新
            bool needUpdate = false;
            
            if (post.DeptId != dept.Id || post.DeptName != deptName)
            {
                post.DeptId = dept.Id;
                post.DeptName = deptName;
                needUpdate = true;
            }

            if (post.PostName != postName)
            {
                post.PostName = postName;
                needUpdate = true;
            }
            
            if (post.PostCategory != postCategory)
            {
                post.PostCategory = postCategory;
                needUpdate = true;
            }

            if (post.PostLevel != postLevel)
            {
                post.PostLevel = postLevel;
                needUpdate = true;
            }
            
            if (post.SortOrder != sortOrder)
            {
                post.SortOrder = sortOrder;
                needUpdate = true;
            }
            if (post.IsBuiltIn != 1)
            {
                post.IsBuiltIn = 1;
                needUpdate = true;
            }
            if (post.PlantCode != plantCode)
            {
                post.PlantCode = plantCode;
                needUpdate = true;
            }
            if (post.CultureCode != cultureCode)
            {
                post.CultureCode = cultureCode;
                needUpdate = true;
            }
            
            // 只有数据发生变化时才更新
            if (needUpdate)
            {
                post.PostStatus = 1;
                await repository.UpdateAsync(post);
            }
            
            return (post, false);
        }
    }
}
