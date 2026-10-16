// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentStaffingRequirement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent;

/// <summary>
/// TaktTalentStaffingRequirement 实体国际化翻译种子（键前缀 entity.talentstaffingrequirement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentStaffingRequirementI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktTalentStaffingRequirement 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentstaffingrequirement 实体翻译...", tenantCode);

        foreach (var item in GetTalentStaffingRequirementTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktTalentStaffingRequirement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentStaffingRequirement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentstaffingrequirement._self / entity.talentstaffingrequirement.{{field}}；ResourceGroup=Talent；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentStaffingRequirementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentstaffingrequirement._self
            new TranslationSeedItem("entity.talentstaffingrequirement._self", "en-US", "Talent Staffing Requirement Information_us", "实体名称"),
            // entity.talentstaffingrequirement._self
            new TranslationSeedItem("entity.talentstaffingrequirement._self", "ja-JP", "用人需求信息_jp", "实体名称"),
            // entity.talentstaffingrequirement._self
            new TranslationSeedItem("entity.talentstaffingrequirement._self", "zh-CN", "用人需求信息", "实体名称"),
            // entity.talentstaffingrequirement._self
            new TranslationSeedItem("entity.talentstaffingrequirement._self", "zh-HK", "用人需求信息_hk", "实体名称"),

            // entity.talentstaffingrequirement.reqcode
            new TranslationSeedItem("entity.talentstaffingrequirement.reqcode", "en-US", "需求单号_us", "需求单号（租户+公司内唯一；自动生成，如 PR-2026-00123；前端表单选择编码规则后自动通过 TaktNumbering 需求单号规则生成并展示，非手输；单据类型菜单：用人需求）"),
            // entity.talentstaffingrequirement.reqcode
            new TranslationSeedItem("entity.talentstaffingrequirement.reqcode", "ja-JP", "需求单号_jp", "需求单号（租户+公司内唯一；自动生成，如 PR-2026-00123；前端表单选择编码规则后自动通过 TaktNumbering 需求单号规则生成并展示，非手输；单据类型菜单：用人需求）"),
            // entity.talentstaffingrequirement.reqcode
            new TranslationSeedItem("entity.talentstaffingrequirement.reqcode", "zh-CN", "需求单号", "需求单号（租户+公司内唯一；自动生成，如 PR-2026-00123；前端表单选择编码规则后自动通过 TaktNumbering 需求单号规则生成并展示，非手输；单据类型菜单：用人需求）"),
            // entity.talentstaffingrequirement.reqcode
            new TranslationSeedItem("entity.talentstaffingrequirement.reqcode", "zh-HK", "需求单号_hk", "需求单号（租户+公司内唯一；自动生成，如 PR-2026-00123；前端表单选择编码规则后自动通过 TaktNumbering 需求单号规则生成并展示，非手输；单据类型菜单：用人需求）"),

            // entity.talentstaffingrequirement.deptid
            new TranslationSeedItem("entity.talentstaffingrequirement.deptid", "en-US", "申请部门ID_us", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.talentstaffingrequirement.deptid
            new TranslationSeedItem("entity.talentstaffingrequirement.deptid", "ja-JP", "申请部门ID_jp", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.talentstaffingrequirement.deptid
            new TranslationSeedItem("entity.talentstaffingrequirement.deptid", "zh-CN", "申请部门ID", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.talentstaffingrequirement.deptid
            new TranslationSeedItem("entity.talentstaffingrequirement.deptid", "zh-HK", "申请部门ID_hk", "申请部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.talentstaffingrequirement.deptname
            new TranslationSeedItem("entity.talentstaffingrequirement.deptname", "en-US", "申请部门名称_us", "申请部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.talentstaffingrequirement.deptname
            new TranslationSeedItem("entity.talentstaffingrequirement.deptname", "ja-JP", "申请部门名称_jp", "申请部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.talentstaffingrequirement.deptname
            new TranslationSeedItem("entity.talentstaffingrequirement.deptname", "zh-CN", "申请部门名称", "申请部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.talentstaffingrequirement.deptname
            new TranslationSeedItem("entity.talentstaffingrequirement.deptname", "zh-HK", "申请部门名称_hk", "申请部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),

            // entity.talentstaffingrequirement.postid
            new TranslationSeedItem("entity.talentstaffingrequirement.postid", "en-US", "申请岗位ID_us", "申请岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.talentstaffingrequirement.postid
            new TranslationSeedItem("entity.talentstaffingrequirement.postid", "ja-JP", "申请岗位ID_jp", "申请岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.talentstaffingrequirement.postid
            new TranslationSeedItem("entity.talentstaffingrequirement.postid", "zh-CN", "申请岗位ID", "申请岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.talentstaffingrequirement.postid
            new TranslationSeedItem("entity.talentstaffingrequirement.postid", "zh-HK", "申请岗位ID_hk", "申请岗位（选项 TaktPosts/options；DictValue=Id）"),

            // entity.talentstaffingrequirement.jobgrade
            new TranslationSeedItem("entity.talentstaffingrequirement.jobgrade", "en-US", "职级_us", "职级（可参照字典 sys_post_level；列存业务码，如 P3/M2）"),
            // entity.talentstaffingrequirement.jobgrade
            new TranslationSeedItem("entity.talentstaffingrequirement.jobgrade", "ja-JP", "职级_jp", "职级（可参照字典 sys_post_level；列存业务码，如 P3/M2）"),
            // entity.talentstaffingrequirement.jobgrade
            new TranslationSeedItem("entity.talentstaffingrequirement.jobgrade", "zh-CN", "职级", "职级（可参照字典 sys_post_level；列存业务码，如 P3/M2）"),
            // entity.talentstaffingrequirement.jobgrade
            new TranslationSeedItem("entity.talentstaffingrequirement.jobgrade", "zh-HK", "职级_hk", "职级（可参照字典 sys_post_level；列存业务码，如 P3/M2）"),

            // entity.talentstaffingrequirement.requestqty
            new TranslationSeedItem("entity.talentstaffingrequirement.requestqty", "en-US", "需求人数_us", "需求人数（默认 1）"),
            // entity.talentstaffingrequirement.requestqty
            new TranslationSeedItem("entity.talentstaffingrequirement.requestqty", "ja-JP", "需求人数_jp", "需求人数（默认 1）"),
            // entity.talentstaffingrequirement.requestqty
            new TranslationSeedItem("entity.talentstaffingrequirement.requestqty", "zh-CN", "需求人数", "需求人数（默认 1）"),
            // entity.talentstaffingrequirement.requestqty
            new TranslationSeedItem("entity.talentstaffingrequirement.requestqty", "zh-HK", "需求人数_hk", "需求人数（默认 1）"),

            // entity.talentstaffingrequirement.headcounttype
            new TranslationSeedItem("entity.talentstaffingrequirement.headcounttype", "en-US", "编制类型_us", "编制类型（字典 humanresource_talent_headcount_type；列存 DictValue：formal/dispatch/intern/temp）"),
            // entity.talentstaffingrequirement.headcounttype
            new TranslationSeedItem("entity.talentstaffingrequirement.headcounttype", "ja-JP", "编制类型_jp", "编制类型（字典 humanresource_talent_headcount_type；列存 DictValue：formal/dispatch/intern/temp）"),
            // entity.talentstaffingrequirement.headcounttype
            new TranslationSeedItem("entity.talentstaffingrequirement.headcounttype", "zh-CN", "编制类型", "编制类型（字典 humanresource_talent_headcount_type；列存 DictValue：formal/dispatch/intern/temp）"),
            // entity.talentstaffingrequirement.headcounttype
            new TranslationSeedItem("entity.talentstaffingrequirement.headcounttype", "zh-HK", "编制类型_hk", "编制类型（字典 humanresource_talent_headcount_type；列存 DictValue：formal/dispatch/intern/temp）"),

            // entity.talentstaffingrequirement.reasoncode
            new TranslationSeedItem("entity.talentstaffingrequirement.reasoncode", "en-US", "需求原因_us", "需求原因（字典 humanresource_talent_staffing_reason_code；列存 DictValue：new_headcount/replacement/expansion/substitute）"),
            // entity.talentstaffingrequirement.reasoncode
            new TranslationSeedItem("entity.talentstaffingrequirement.reasoncode", "ja-JP", "需求原因_jp", "需求原因（字典 humanresource_talent_staffing_reason_code；列存 DictValue：new_headcount/replacement/expansion/substitute）"),
            // entity.talentstaffingrequirement.reasoncode
            new TranslationSeedItem("entity.talentstaffingrequirement.reasoncode", "zh-CN", "需求原因", "需求原因（字典 humanresource_talent_staffing_reason_code；列存 DictValue：new_headcount/replacement/expansion/substitute）"),
            // entity.talentstaffingrequirement.reasoncode
            new TranslationSeedItem("entity.talentstaffingrequirement.reasoncode", "zh-HK", "需求原因_hk", "需求原因（字典 humanresource_talent_staffing_reason_code；列存 DictValue：new_headcount/replacement/expansion/substitute）"),

            // entity.talentstaffingrequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeeid", "en-US", "替补员工ID_us", "替补员工（选项 TaktEmployees/options；离职补充时填原员工，可空，DictValue=Id）"),
            // entity.talentstaffingrequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeeid", "ja-JP", "替补员工ID_jp", "替补员工（选项 TaktEmployees/options；离职补充时填原员工，可空，DictValue=Id）"),
            // entity.talentstaffingrequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeeid", "zh-CN", "替补员工ID", "替补员工（选项 TaktEmployees/options；离职补充时填原员工，可空，DictValue=Id）"),
            // entity.talentstaffingrequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeeid", "zh-HK", "替补员工ID_hk", "替补员工（选项 TaktEmployees/options；离职补充时填原员工，可空，DictValue=Id）"),

            // entity.talentstaffingrequirement.replaceemployeename
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeename", "en-US", "替补员工名称_us", "替补员工名称（冗余：按 ReplaceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentstaffingrequirement.replaceemployeename
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeename", "ja-JP", "替补员工名称_jp", "替补员工名称（冗余：按 ReplaceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentstaffingrequirement.replaceemployeename
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeename", "zh-CN", "替补员工名称", "替补员工名称（冗余：按 ReplaceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentstaffingrequirement.replaceemployeename
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployeename", "zh-HK", "替补员工名称_hk", "替补员工名称（冗余：按 ReplaceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.talentstaffingrequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentstaffingrequirement.expectedonboarddate", "en-US", "期望入职日_us", "期望入职日"),
            // entity.talentstaffingrequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentstaffingrequirement.expectedonboarddate", "ja-JP", "期望入职日_jp", "期望入职日"),
            // entity.talentstaffingrequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentstaffingrequirement.expectedonboarddate", "zh-CN", "期望入职日", "期望入职日"),
            // entity.talentstaffingrequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentstaffingrequirement.expectedonboarddate", "zh-HK", "期望入职日_hk", "期望入职日"),

            // entity.talentstaffingrequirement.contracttype
            new TranslationSeedItem("entity.talentstaffingrequirement.contracttype", "en-US", "合同类型_us", "合同类型（字典 humanresource_talent_staffing_contract_type；列存 DictValue：fixed/indefinite/intern_agreement）"),
            // entity.talentstaffingrequirement.contracttype
            new TranslationSeedItem("entity.talentstaffingrequirement.contracttype", "ja-JP", "合同类型_jp", "合同类型（字典 humanresource_talent_staffing_contract_type；列存 DictValue：fixed/indefinite/intern_agreement）"),
            // entity.talentstaffingrequirement.contracttype
            new TranslationSeedItem("entity.talentstaffingrequirement.contracttype", "zh-CN", "合同类型", "合同类型（字典 humanresource_talent_staffing_contract_type；列存 DictValue：fixed/indefinite/intern_agreement）"),
            // entity.talentstaffingrequirement.contracttype
            new TranslationSeedItem("entity.talentstaffingrequirement.contracttype", "zh-HK", "合同类型_hk", "合同类型（字典 humanresource_talent_staffing_contract_type；列存 DictValue：fixed/indefinite/intern_agreement）"),

            // entity.talentstaffingrequirement.worklocation
            new TranslationSeedItem("entity.talentstaffingrequirement.worklocation", "en-US", "工作地点_us", "工作地点（如工厂/分公司）"),
            // entity.talentstaffingrequirement.worklocation
            new TranslationSeedItem("entity.talentstaffingrequirement.worklocation", "ja-JP", "工作地点_jp", "工作地点（如工厂/分公司）"),
            // entity.talentstaffingrequirement.worklocation
            new TranslationSeedItem("entity.talentstaffingrequirement.worklocation", "zh-CN", "工作地点", "工作地点（如工厂/分公司）"),
            // entity.talentstaffingrequirement.worklocation
            new TranslationSeedItem("entity.talentstaffingrequirement.worklocation", "zh-HK", "工作地点_hk", "工作地点（如工厂/分公司）"),

            // entity.talentstaffingrequirement.jobdesc
            new TranslationSeedItem("entity.talentstaffingrequirement.jobdesc", "en-US", "岗位职责_us", "岗位职责"),
            // entity.talentstaffingrequirement.jobdesc
            new TranslationSeedItem("entity.talentstaffingrequirement.jobdesc", "ja-JP", "岗位职责_jp", "岗位职责"),
            // entity.talentstaffingrequirement.jobdesc
            new TranslationSeedItem("entity.talentstaffingrequirement.jobdesc", "zh-CN", "岗位职责", "岗位职责"),
            // entity.talentstaffingrequirement.jobdesc
            new TranslationSeedItem("entity.talentstaffingrequirement.jobdesc", "zh-HK", "岗位职责_hk", "岗位职责"),

            // entity.talentstaffingrequirement.qualification
            new TranslationSeedItem("entity.talentstaffingrequirement.qualification", "en-US", "任职要求_us", "任职要求（学历/经验/技能）"),
            // entity.talentstaffingrequirement.qualification
            new TranslationSeedItem("entity.talentstaffingrequirement.qualification", "ja-JP", "任职要求_jp", "任职要求（学历/经验/技能）"),
            // entity.talentstaffingrequirement.qualification
            new TranslationSeedItem("entity.talentstaffingrequirement.qualification", "zh-CN", "任职要求", "任职要求（学历/经验/技能）"),
            // entity.talentstaffingrequirement.qualification
            new TranslationSeedItem("entity.talentstaffingrequirement.qualification", "zh-HK", "任职要求_hk", "任职要求（学历/经验/技能）"),

            // entity.talentstaffingrequirement.budgetyear
            new TranslationSeedItem("entity.talentstaffingrequirement.budgetyear", "en-US", "预算年度_us", "预算年度（用于 headcount 控制）"),
            // entity.talentstaffingrequirement.budgetyear
            new TranslationSeedItem("entity.talentstaffingrequirement.budgetyear", "ja-JP", "预算年度_jp", "预算年度（用于 headcount 控制）"),
            // entity.talentstaffingrequirement.budgetyear
            new TranslationSeedItem("entity.talentstaffingrequirement.budgetyear", "zh-CN", "预算年度", "预算年度（用于 headcount 控制）"),
            // entity.talentstaffingrequirement.budgetyear
            new TranslationSeedItem("entity.talentstaffingrequirement.budgetyear", "zh-HK", "预算年度_hk", "预算年度（用于 headcount 控制）"),

            // entity.talentstaffingrequirement.dept
            new TranslationSeedItem("entity.talentstaffingrequirement.dept", "en-US", "申请部门_us", "申请部门"),
            // entity.talentstaffingrequirement.dept
            new TranslationSeedItem("entity.talentstaffingrequirement.dept", "ja-JP", "申请部门_jp", "申请部门"),
            // entity.talentstaffingrequirement.dept
            new TranslationSeedItem("entity.talentstaffingrequirement.dept", "zh-CN", "申请部门", "申请部门"),
            // entity.talentstaffingrequirement.dept
            new TranslationSeedItem("entity.talentstaffingrequirement.dept", "zh-HK", "申请部门_hk", "申请部门"),

            // entity.talentstaffingrequirement.post
            new TranslationSeedItem("entity.talentstaffingrequirement.post", "en-US", "申请岗位_us", "申请岗位"),
            // entity.talentstaffingrequirement.post
            new TranslationSeedItem("entity.talentstaffingrequirement.post", "ja-JP", "申请岗位_jp", "申请岗位"),
            // entity.talentstaffingrequirement.post
            new TranslationSeedItem("entity.talentstaffingrequirement.post", "zh-CN", "申请岗位", "申请岗位"),
            // entity.talentstaffingrequirement.post
            new TranslationSeedItem("entity.talentstaffingrequirement.post", "zh-HK", "申请岗位_hk", "申请岗位"),

            // entity.talentstaffingrequirement.replaceemployee
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployee", "en-US", "替补员工_us", "替补员工"),
            // entity.talentstaffingrequirement.replaceemployee
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployee", "ja-JP", "替补员工_jp", "替补员工"),
            // entity.talentstaffingrequirement.replaceemployee
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployee", "zh-CN", "替补员工", "替补员工"),
            // entity.talentstaffingrequirement.replaceemployee
            new TranslationSeedItem("entity.talentstaffingrequirement.replaceemployee", "zh-HK", "替补员工_hk", "替补员工"),

            // entity.talentstaffingrequirement.talentjobpostings
            new TranslationSeedItem("entity.talentstaffingrequirement.talentjobpostings", "en-US", "职位发布_us", "职位发布"),
            // entity.talentstaffingrequirement.talentjobpostings
            new TranslationSeedItem("entity.talentstaffingrequirement.talentjobpostings", "ja-JP", "职位发布_jp", "职位发布"),
            // entity.talentstaffingrequirement.talentjobpostings
            new TranslationSeedItem("entity.talentstaffingrequirement.talentjobpostings", "zh-CN", "职位发布", "职位发布"),
            // entity.talentstaffingrequirement.talentjobpostings
            new TranslationSeedItem("entity.talentstaffingrequirement.talentjobpostings", "zh-HK", "职位发布_hk", "职位发布"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Talent";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
