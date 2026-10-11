// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktSalaryItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalaryItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalaryItem 实体国际化翻译种子（键前缀 entity.salaryitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalaryItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalaryItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salaryitem 实体翻译...", tenantCode);

        foreach (var item in GetSalaryItemTranslations())
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

        TaktLogger.Information("TaktSalaryItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalaryItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salaryitem._self / entity.salaryitem.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalaryItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "en-US", "Salary Item Information_us", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "ja-JP", "薪资项目信息_jp", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "zh-CN", "薪资项目信息", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "zh-HK", "薪资项目信息_hk", "实体名称"),

            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "en-US", "项目编码_us", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "ja-JP", "项目编码_jp", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "zh-CN", "项目编码", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "zh-HK", "项目编码_hk", "项目编码（租户+公司内唯一）"),

            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "en-US", "项目名称_us", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "ja-JP", "项目名称_jp", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "zh-CN", "项目名称", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "zh-HK", "项目名称_hk", "项目名称"),

            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "en-US", "简称_us", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "ja-JP", "简称_jp", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "zh-CN", "简称", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "zh-HK", "简称_hk", "简称"),

            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "en-US", "项目类型_us", "项目类型（字典 humanresource_compensation_salary_item_type；1=基本工资 2=岗位工资 3=津贴 4=奖金 5=股权激励）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "ja-JP", "项目类型_jp", "项目类型（字典 humanresource_compensation_salary_item_type；1=基本工资 2=岗位工资 3=津贴 4=奖金 5=股权激励）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "zh-CN", "项目类型", "项目类型（字典 humanresource_compensation_salary_item_type；1=基本工资 2=岗位工资 3=津贴 4=奖金 5=股权激励）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "zh-HK", "项目类型_hk", "项目类型（字典 humanresource_compensation_salary_item_type；1=基本工资 2=岗位工资 3=津贴 4=奖金 5=股权激励）"),

            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "en-US", "计算方式_us", "计算方式（字典 humanresource_compensation_salary_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "ja-JP", "计算方式_jp", "计算方式（字典 humanresource_compensation_salary_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "zh-CN", "计算方式", "计算方式（字典 humanresource_compensation_salary_calc_method；1=固定金额 2=按比例 3=按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "zh-HK", "计算方式_hk", "计算方式（字典 humanresource_compensation_salary_calc_method；1=固定金额 2=按比例 3=按公式）"),

            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "en-US", "计算公式ID_us", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "ja-JP", "计算公式ID_jp", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "zh-CN", "计算公式ID", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "zh-HK", "计算公式ID_hk", "计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）"),

            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "en-US", "默认金额_us", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "ja-JP", "默认金额_jp", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "zh-CN", "默认金额", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "zh-HK", "默认金额_hk", "默认金额（元）"),

            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "en-US", "默认比例_us", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "ja-JP", "默认比例_jp", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "zh-CN", "默认比例", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "zh-HK", "默认比例_hk", "默认比例（%，0~100）"),

            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "en-US", "默认行权价格_us", "默认行权/授予价格（元；item_type=5 股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "ja-JP", "默认行权价格_jp", "默认行权/授予价格（元；item_type=5 股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "zh-CN", "默认行权价格", "默认行权/授予价格（元；item_type=5 股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "zh-HK", "默认行权价格_hk", "默认行权/授予价格（元；item_type=5 股权激励时使用）"),

            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "en-US", "默认归属年限_us", "默认归属年限（年；item_type=5 股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "ja-JP", "默认归属年限_jp", "默认归属年限（年；item_type=5 股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "zh-CN", "默认归属年限", "默认归属年限（年；item_type=5 股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "zh-HK", "默认归属年限_hk", "默认归属年限（年；item_type=5 股权激励时使用）"),

            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "en-US", "是否扣款项_us", "是否扣款项（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "ja-JP", "是否扣款项_jp", "是否扣款项（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "zh-CN", "是否扣款项", "是否扣款项（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "zh-HK", "是否扣款项_hk", "是否扣款项（字典 sys_yes_no；0=否 1=是）"),

            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "en-US", "是否计入应税所得_us", "是否计入应税所得（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "ja-JP", "是否计入应税所得_jp", "是否计入应税所得（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "zh-CN", "是否计入应税所得", "是否计入应税所得（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "zh-HK", "是否计入应税所得_hk", "是否计入应税所得（字典 sys_yes_no；0=否 1=是）"),

            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "en-US", "是否计入社保基数_us", "是否计入社保基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "ja-JP", "是否计入社保基数_jp", "是否计入社保基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "zh-CN", "是否计入社保基数", "是否计入社保基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "zh-HK", "是否计入社保基数_hk", "是否计入社保基数（字典 sys_yes_no；0=否 1=是）"),

            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "en-US", "是否计入公积金基数_us", "是否计入公积金基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "ja-JP", "是否计入公积金基数_jp", "是否计入公积金基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "zh-CN", "是否计入公积金基数", "是否计入公积金基数（字典 sys_yes_no；0=否 1=是）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "zh-HK", "是否计入公积金基数_hk", "是否计入公积金基数（字典 sys_yes_no；0=否 1=是）"),

            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
