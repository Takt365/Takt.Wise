// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentOfferI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentOffer 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTalentOffer 实体国际化翻译种子（键前缀 entity.talentoffer.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentOfferI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentOffer 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentoffer 实体翻译...", tenantCode);

        foreach (var item in GetTalentOfferTranslations())
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

        TaktLogger.Information("TaktTalentOffer 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentOffer 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentoffer._self / entity.talentoffer.{{field}}；ResourceGroup=Talent；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentOfferTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentoffer._self
            new TranslationSeedItem("entity.talentoffer._self", "en-US", "Talent Offer Information_us", "实体名称"),
            // entity.talentoffer._self
            new TranslationSeedItem("entity.talentoffer._self", "ja-JP", "录用信息_jp", "实体名称"),
            // entity.talentoffer._self
            new TranslationSeedItem("entity.talentoffer._self", "zh-CN", "录用信息", "实体名称"),
            // entity.talentoffer._self
            new TranslationSeedItem("entity.talentoffer._self", "zh-HK", "录用信息_hk", "实体名称"),

            // entity.talentoffer.jobpostingid
            new TranslationSeedItem("entity.talentoffer.jobpostingid", "en-US", "职位发布ID_us", "职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）"),
            // entity.talentoffer.jobpostingid
            new TranslationSeedItem("entity.talentoffer.jobpostingid", "ja-JP", "职位发布ID_jp", "职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）"),
            // entity.talentoffer.jobpostingid
            new TranslationSeedItem("entity.talentoffer.jobpostingid", "zh-CN", "职位发布ID", "职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）"),
            // entity.talentoffer.jobpostingid
            new TranslationSeedItem("entity.talentoffer.jobpostingid", "zh-HK", "职位发布ID_hk", "职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）"),

            // entity.talentoffer.offercode
            new TranslationSeedItem("entity.talentoffer.offercode", "en-US", "录用编码_us", "录用编码（租户+公司内业务编码；前端表单选择编码规则后自动通过 TaktNumbering 录用编码规则生成并展示，非手输；单据类型菜单：录用）"),
            // entity.talentoffer.offercode
            new TranslationSeedItem("entity.talentoffer.offercode", "ja-JP", "录用编码_jp", "录用编码（租户+公司内业务编码；前端表单选择编码规则后自动通过 TaktNumbering 录用编码规则生成并展示，非手输；单据类型菜单：录用）"),
            // entity.talentoffer.offercode
            new TranslationSeedItem("entity.talentoffer.offercode", "zh-CN", "录用编码", "录用编码（租户+公司内业务编码；前端表单选择编码规则后自动通过 TaktNumbering 录用编码规则生成并展示，非手输；单据类型菜单：录用）"),
            // entity.talentoffer.offercode
            new TranslationSeedItem("entity.talentoffer.offercode", "zh-HK", "录用编码_hk", "录用编码（租户+公司内业务编码；前端表单选择编码规则后自动通过 TaktNumbering 录用编码规则生成并展示，非手输；单据类型菜单：录用）"),

            // entity.talentoffer.hiredate
            new TranslationSeedItem("entity.talentoffer.hiredate", "en-US", "录用日期_us", "录用日期（确认录用/发 offer）"),
            // entity.talentoffer.hiredate
            new TranslationSeedItem("entity.talentoffer.hiredate", "ja-JP", "录用日期_jp", "录用日期（确认录用/发 offer）"),
            // entity.talentoffer.hiredate
            new TranslationSeedItem("entity.talentoffer.hiredate", "zh-CN", "录用日期", "录用日期（确认录用/发 offer）"),
            // entity.talentoffer.hiredate
            new TranslationSeedItem("entity.talentoffer.hiredate", "zh-HK", "录用日期_hk", "录用日期（确认录用/发 offer）"),

            // entity.talentoffer.employeeid
            new TranslationSeedItem("entity.talentoffer.employeeid", "en-US", "关联员工ID_us", "关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）"),
            // entity.talentoffer.employeeid
            new TranslationSeedItem("entity.talentoffer.employeeid", "ja-JP", "关联员工ID_jp", "关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）"),
            // entity.talentoffer.employeeid
            new TranslationSeedItem("entity.talentoffer.employeeid", "zh-CN", "关联员工ID", "关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）"),
            // entity.talentoffer.employeeid
            new TranslationSeedItem("entity.talentoffer.employeeid", "zh-HK", "关联员工ID_hk", "关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）"),

            // entity.talentoffer.employeename
            new TranslationSeedItem("entity.talentoffer.employeename", "en-US", "员工名称_us", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentoffer.employeename
            new TranslationSeedItem("entity.talentoffer.employeename", "ja-JP", "员工名称_jp", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentoffer.employeename
            new TranslationSeedItem("entity.talentoffer.employeename", "zh-CN", "员工名称", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.talentoffer.employeename
            new TranslationSeedItem("entity.talentoffer.employeename", "zh-HK", "员工名称_hk", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.talentoffer.deptid
            new TranslationSeedItem("entity.talentoffer.deptid", "en-US", "拟录用部门ID_us", "拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.talentoffer.deptid
            new TranslationSeedItem("entity.talentoffer.deptid", "ja-JP", "拟录用部门ID_jp", "拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.talentoffer.deptid
            new TranslationSeedItem("entity.talentoffer.deptid", "zh-CN", "拟录用部门ID", "拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.talentoffer.deptid
            new TranslationSeedItem("entity.talentoffer.deptid", "zh-HK", "拟录用部门ID_hk", "拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),

            // entity.talentoffer.deptname
            new TranslationSeedItem("entity.talentoffer.deptname", "en-US", "拟录用部门名称_us", "拟录用部门名称"),
            // entity.talentoffer.deptname
            new TranslationSeedItem("entity.talentoffer.deptname", "ja-JP", "拟录用部门名称_jp", "拟录用部门名称"),
            // entity.talentoffer.deptname
            new TranslationSeedItem("entity.talentoffer.deptname", "zh-CN", "拟录用部门名称", "拟录用部门名称"),
            // entity.talentoffer.deptname
            new TranslationSeedItem("entity.talentoffer.deptname", "zh-HK", "拟录用部门名称_hk", "拟录用部门名称"),

            // entity.talentoffer.postid
            new TranslationSeedItem("entity.talentoffer.postid", "en-US", "拟录用岗位ID_us", "拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）"),
            // entity.talentoffer.postid
            new TranslationSeedItem("entity.talentoffer.postid", "ja-JP", "拟录用岗位ID_jp", "拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）"),
            // entity.talentoffer.postid
            new TranslationSeedItem("entity.talentoffer.postid", "zh-CN", "拟录用岗位ID", "拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）"),
            // entity.talentoffer.postid
            new TranslationSeedItem("entity.talentoffer.postid", "zh-HK", "拟录用岗位ID_hk", "拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）"),

            // entity.talentoffer.postname
            new TranslationSeedItem("entity.talentoffer.postname", "en-US", "拟录用岗位名称_us", "拟录用岗位名称"),
            // entity.talentoffer.postname
            new TranslationSeedItem("entity.talentoffer.postname", "ja-JP", "拟录用岗位名称_jp", "拟录用岗位名称"),
            // entity.talentoffer.postname
            new TranslationSeedItem("entity.talentoffer.postname", "zh-CN", "拟录用岗位名称", "拟录用岗位名称"),
            // entity.talentoffer.postname
            new TranslationSeedItem("entity.talentoffer.postname", "zh-HK", "拟录用岗位名称_hk", "拟录用岗位名称"),

            // entity.talentoffer.reason
            new TranslationSeedItem("entity.talentoffer.reason", "en-US", "录用说明_us", "录用说明"),
            // entity.talentoffer.reason
            new TranslationSeedItem("entity.talentoffer.reason", "ja-JP", "录用说明_jp", "录用说明"),
            // entity.talentoffer.reason
            new TranslationSeedItem("entity.talentoffer.reason", "zh-CN", "录用说明", "录用说明"),
            // entity.talentoffer.reason
            new TranslationSeedItem("entity.talentoffer.reason", "zh-HK", "录用说明_hk", "录用说明"),

            // entity.talentoffer.jobposting
            new TranslationSeedItem("entity.talentoffer.jobposting", "en-US", "职位发布_us", "职位发布"),
            // entity.talentoffer.jobposting
            new TranslationSeedItem("entity.talentoffer.jobposting", "ja-JP", "职位发布_jp", "职位发布"),
            // entity.talentoffer.jobposting
            new TranslationSeedItem("entity.talentoffer.jobposting", "zh-CN", "职位发布", "职位发布"),
            // entity.talentoffer.jobposting
            new TranslationSeedItem("entity.talentoffer.jobposting", "zh-HK", "职位发布_hk", "职位发布"),

            // entity.talentoffer.employeeonboardings
            new TranslationSeedItem("entity.talentoffer.employeeonboardings", "en-US", "入职待办_us", "入职待办"),
            // entity.talentoffer.employeeonboardings
            new TranslationSeedItem("entity.talentoffer.employeeonboardings", "ja-JP", "入职待办_jp", "入职待办"),
            // entity.talentoffer.employeeonboardings
            new TranslationSeedItem("entity.talentoffer.employeeonboardings", "zh-CN", "入职待办", "入职待办"),
            // entity.talentoffer.employeeonboardings
            new TranslationSeedItem("entity.talentoffer.employeeonboardings", "zh-HK", "入职待办_hk", "入职待办"),
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
