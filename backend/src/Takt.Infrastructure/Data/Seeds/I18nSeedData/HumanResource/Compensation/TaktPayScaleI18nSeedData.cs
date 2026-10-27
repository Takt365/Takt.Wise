// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktPayScaleI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPayScale 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPayScale 实体国际化翻译种子（键前缀 entity.payscale.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPayScaleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPayScale 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 payscale 实体翻译...", tenantCode);

        foreach (var item in GetPayScaleTranslations())
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

        TaktLogger.Information("TaktPayScale 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPayScale 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.payscale._self / entity.payscale.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPayScaleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.payscale._self
            new TranslationSeedItem("entity.payscale._self", "en-US", "Pay Scale Information_us", "实体名称"),
            // entity.payscale._self
            new TranslationSeedItem("entity.payscale._self", "ja-JP", "薪级薪等信息_jp", "实体名称"),
            // entity.payscale._self
            new TranslationSeedItem("entity.payscale._self", "zh-CN", "薪级薪等信息", "实体名称"),
            // entity.payscale._self
            new TranslationSeedItem("entity.payscale._self", "zh-HK", "薪级薪等信息_hk", "实体名称"),

            // entity.payscale.scalecode
            new TranslationSeedItem("entity.payscale.scalecode", "en-US", "薪级编码_us", "薪级编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪级编码规则生成并展示，非手输；单据类型菜单：薪级）"),
            // entity.payscale.scalecode
            new TranslationSeedItem("entity.payscale.scalecode", "ja-JP", "薪级编码_jp", "薪级编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪级编码规则生成并展示，非手输；单据类型菜单：薪级）"),
            // entity.payscale.scalecode
            new TranslationSeedItem("entity.payscale.scalecode", "zh-CN", "薪级编码", "薪级编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪级编码规则生成并展示，非手输；单据类型菜单：薪级）"),
            // entity.payscale.scalecode
            new TranslationSeedItem("entity.payscale.scalecode", "zh-HK", "薪级编码_hk", "薪级编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪级编码规则生成并展示，非手输；单据类型菜单：薪级）"),

            // entity.payscale.scalename
            new TranslationSeedItem("entity.payscale.scalename", "en-US", "薪级名称_us", "薪级名称"),
            // entity.payscale.scalename
            new TranslationSeedItem("entity.payscale.scalename", "ja-JP", "薪级名称_jp", "薪级名称"),
            // entity.payscale.scalename
            new TranslationSeedItem("entity.payscale.scalename", "zh-CN", "薪级名称", "薪级名称"),
            // entity.payscale.scalename
            new TranslationSeedItem("entity.payscale.scalename", "zh-HK", "薪级名称_hk", "薪级名称"),

            // entity.payscale.gradelevel
            new TranslationSeedItem("entity.payscale.gradelevel", "en-US", "等级_us", "等级（数字越大等级越高）"),
            // entity.payscale.gradelevel
            new TranslationSeedItem("entity.payscale.gradelevel", "ja-JP", "等级_jp", "等级（数字越大等级越高）"),
            // entity.payscale.gradelevel
            new TranslationSeedItem("entity.payscale.gradelevel", "zh-CN", "等级", "等级（数字越大等级越高）"),
            // entity.payscale.gradelevel
            new TranslationSeedItem("entity.payscale.gradelevel", "zh-HK", "等级_hk", "等级（数字越大等级越高）"),

            // entity.payscale.minsalary
            new TranslationSeedItem("entity.payscale.minsalary", "en-US", "下限金额_us", "下限金额（元）"),
            // entity.payscale.minsalary
            new TranslationSeedItem("entity.payscale.minsalary", "ja-JP", "下限金额_jp", "下限金额（元）"),
            // entity.payscale.minsalary
            new TranslationSeedItem("entity.payscale.minsalary", "zh-CN", "下限金额", "下限金额（元）"),
            // entity.payscale.minsalary
            new TranslationSeedItem("entity.payscale.minsalary", "zh-HK", "下限金额_hk", "下限金额（元）"),

            // entity.payscale.midsalary
            new TranslationSeedItem("entity.payscale.midsalary", "en-US", "中位金额_us", "中位金额（元）"),
            // entity.payscale.midsalary
            new TranslationSeedItem("entity.payscale.midsalary", "ja-JP", "中位金额_jp", "中位金额（元）"),
            // entity.payscale.midsalary
            new TranslationSeedItem("entity.payscale.midsalary", "zh-CN", "中位金额", "中位金额（元）"),
            // entity.payscale.midsalary
            new TranslationSeedItem("entity.payscale.midsalary", "zh-HK", "中位金额_hk", "中位金额（元）"),

            // entity.payscale.maxsalary
            new TranslationSeedItem("entity.payscale.maxsalary", "en-US", "上限金额_us", "上限金额（元）"),
            // entity.payscale.maxsalary
            new TranslationSeedItem("entity.payscale.maxsalary", "ja-JP", "上限金额_jp", "上限金额（元）"),
            // entity.payscale.maxsalary
            new TranslationSeedItem("entity.payscale.maxsalary", "zh-CN", "上限金额", "上限金额（元）"),
            // entity.payscale.maxsalary
            new TranslationSeedItem("entity.payscale.maxsalary", "zh-HK", "上限金额_hk", "上限金额（元）"),

            // entity.payscale.sortorder
            new TranslationSeedItem("entity.payscale.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.payscale.sortorder
            new TranslationSeedItem("entity.payscale.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.payscale.sortorder
            new TranslationSeedItem("entity.payscale.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.payscale.sortorder
            new TranslationSeedItem("entity.payscale.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.payscale.scalestatus
            new TranslationSeedItem("entity.payscale.scalestatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payscale.scalestatus
            new TranslationSeedItem("entity.payscale.scalestatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payscale.scalestatus
            new TranslationSeedItem("entity.payscale.scalestatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payscale.scalestatus
            new TranslationSeedItem("entity.payscale.scalestatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
