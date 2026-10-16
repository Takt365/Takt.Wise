// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktBonusPlanI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBonusPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation;

/// <summary>
/// TaktBonusPlan 实体国际化翻译种子（键前缀 entity.bonusplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBonusPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBonusPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 bonusplan 实体翻译...", tenantCode);

        foreach (var item in GetBonusPlanTranslations())
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

        TaktLogger.Information("TaktBonusPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBonusPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.bonusplan._self / entity.bonusplan.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBonusPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.bonusplan._self
            new TranslationSeedItem("entity.bonusplan._self", "en-US", "Bonus Plan Information_us", "实体名称"),
            // entity.bonusplan._self
            new TranslationSeedItem("entity.bonusplan._self", "ja-JP", "奖金方案信息_jp", "实体名称"),
            // entity.bonusplan._self
            new TranslationSeedItem("entity.bonusplan._self", "zh-CN", "奖金方案信息", "实体名称"),
            // entity.bonusplan._self
            new TranslationSeedItem("entity.bonusplan._self", "zh-HK", "奖金方案信息_hk", "实体名称"),

            // entity.bonusplan.plancode
            new TranslationSeedItem("entity.bonusplan.plancode", "en-US", "方案编码_us", "方案编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：奖金方案）"),
            // entity.bonusplan.plancode
            new TranslationSeedItem("entity.bonusplan.plancode", "ja-JP", "方案编码_jp", "方案编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：奖金方案）"),
            // entity.bonusplan.plancode
            new TranslationSeedItem("entity.bonusplan.plancode", "zh-CN", "方案编码", "方案编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：奖金方案）"),
            // entity.bonusplan.plancode
            new TranslationSeedItem("entity.bonusplan.plancode", "zh-HK", "方案编码_hk", "方案编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：奖金方案）"),

            // entity.bonusplan.planname
            new TranslationSeedItem("entity.bonusplan.planname", "en-US", "方案名称_us", "方案名称"),
            // entity.bonusplan.planname
            new TranslationSeedItem("entity.bonusplan.planname", "ja-JP", "方案名称_jp", "方案名称"),
            // entity.bonusplan.planname
            new TranslationSeedItem("entity.bonusplan.planname", "zh-CN", "方案名称", "方案名称"),
            // entity.bonusplan.planname
            new TranslationSeedItem("entity.bonusplan.planname", "zh-HK", "方案名称_hk", "方案名称"),

            // entity.bonusplan.bonustype
            new TranslationSeedItem("entity.bonusplan.bonustype", "en-US", "奖金类型_us", "奖金类型（字典 humanresource_compensation_bonus_type；1=绩效奖金 2=项目奖金 3=年终奖金 4=专项奖金）"),
            // entity.bonusplan.bonustype
            new TranslationSeedItem("entity.bonusplan.bonustype", "ja-JP", "奖金类型_jp", "奖金类型（字典 humanresource_compensation_bonus_type；1=绩效奖金 2=项目奖金 3=年终奖金 4=专项奖金）"),
            // entity.bonusplan.bonustype
            new TranslationSeedItem("entity.bonusplan.bonustype", "zh-CN", "奖金类型", "奖金类型（字典 humanresource_compensation_bonus_type；1=绩效奖金 2=项目奖金 3=年终奖金 4=专项奖金）"),
            // entity.bonusplan.bonustype
            new TranslationSeedItem("entity.bonusplan.bonustype", "zh-HK", "奖金类型_hk", "奖金类型（字典 humanresource_compensation_bonus_type；1=绩效奖金 2=项目奖金 3=年终奖金 4=专项奖金）"),

            // entity.bonusplan.calcmethod
            new TranslationSeedItem("entity.bonusplan.calcmethod", "en-US", "计算方式_us", "计算方式（字典 humanresource_compensation_bonus_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.bonusplan.calcmethod
            new TranslationSeedItem("entity.bonusplan.calcmethod", "ja-JP", "计算方式_jp", "计算方式（字典 humanresource_compensation_bonus_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.bonusplan.calcmethod
            new TranslationSeedItem("entity.bonusplan.calcmethod", "zh-CN", "计算方式", "计算方式（字典 humanresource_compensation_bonus_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.bonusplan.calcmethod
            new TranslationSeedItem("entity.bonusplan.calcmethod", "zh-HK", "计算方式_hk", "计算方式（字典 humanresource_compensation_bonus_calc_method；1=固定金额 2=按比例 3=按公式）"),

            // entity.bonusplan.salaryformulaid
            new TranslationSeedItem("entity.bonusplan.salaryformulaid", "en-US", "计算公式ID_us", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.bonusplan.salaryformulaid
            new TranslationSeedItem("entity.bonusplan.salaryformulaid", "ja-JP", "计算公式ID_jp", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.bonusplan.salaryformulaid
            new TranslationSeedItem("entity.bonusplan.salaryformulaid", "zh-CN", "计算公式ID", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.bonusplan.salaryformulaid
            new TranslationSeedItem("entity.bonusplan.salaryformulaid", "zh-HK", "计算公式ID_hk", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),

            // entity.bonusplan.defaultamount
            new TranslationSeedItem("entity.bonusplan.defaultamount", "en-US", "默认金额_us", "默认奖金金额或基数（元）"),
            // entity.bonusplan.defaultamount
            new TranslationSeedItem("entity.bonusplan.defaultamount", "ja-JP", "默认金额_jp", "默认奖金金额或基数（元）"),
            // entity.bonusplan.defaultamount
            new TranslationSeedItem("entity.bonusplan.defaultamount", "zh-CN", "默认金额", "默认奖金金额或基数（元）"),
            // entity.bonusplan.defaultamount
            new TranslationSeedItem("entity.bonusplan.defaultamount", "zh-HK", "默认金额_hk", "默认奖金金额或基数（元）"),

            // entity.bonusplan.effectivedate
            new TranslationSeedItem("entity.bonusplan.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.bonusplan.effectivedate
            new TranslationSeedItem("entity.bonusplan.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.bonusplan.effectivedate
            new TranslationSeedItem("entity.bonusplan.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.bonusplan.effectivedate
            new TranslationSeedItem("entity.bonusplan.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.bonusplan.description
            new TranslationSeedItem("entity.bonusplan.description", "en-US", "方案说明_us", "方案说明"),
            // entity.bonusplan.description
            new TranslationSeedItem("entity.bonusplan.description", "ja-JP", "方案说明_jp", "方案说明"),
            // entity.bonusplan.description
            new TranslationSeedItem("entity.bonusplan.description", "zh-CN", "方案说明", "方案说明"),
            // entity.bonusplan.description
            new TranslationSeedItem("entity.bonusplan.description", "zh-HK", "方案说明_hk", "方案说明"),

            // entity.bonusplan.planstatus
            new TranslationSeedItem("entity.bonusplan.planstatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.bonusplan.planstatus
            new TranslationSeedItem("entity.bonusplan.planstatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.bonusplan.planstatus
            new TranslationSeedItem("entity.bonusplan.planstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.bonusplan.planstatus
            new TranslationSeedItem("entity.bonusplan.planstatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
        translation.ResourceGroup = "Compensation";
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
