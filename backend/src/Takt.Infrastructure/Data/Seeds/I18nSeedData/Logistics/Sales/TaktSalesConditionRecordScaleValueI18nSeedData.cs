// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesConditionRecordScaleValueI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesConditionRecordScaleValue 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesConditionRecordScaleValue 实体国际化翻译种子（键前缀 entity.salesconditionrecordscalevalue.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesConditionRecordScaleValueI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesConditionRecordScaleValue 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesconditionrecordscalevalue 实体翻译...", tenantCode);

        foreach (var item in GetSalesConditionRecordScaleValueTranslations())
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

        TaktLogger.Information("TaktSalesConditionRecordScaleValue 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesConditionRecordScaleValue 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesconditionrecordscalevalue._self / entity.salesconditionrecordscalevalue.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesConditionRecordScaleValueTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesconditionrecordscalevalue._self
            new TranslationSeedItem("entity.salesconditionrecordscalevalue._self", "en-US", "Sales Condition Record Scale Value Information_us", "实体名称"),
            // entity.salesconditionrecordscalevalue._self
            new TranslationSeedItem("entity.salesconditionrecordscalevalue._self", "ja-JP", "Takt销售阶梯价格信息_jp", "实体名称"),
            // entity.salesconditionrecordscalevalue._self
            new TranslationSeedItem("entity.salesconditionrecordscalevalue._self", "zh-CN", "Takt销售阶梯价格信息", "实体名称"),
            // entity.salesconditionrecordscalevalue._self
            new TranslationSeedItem("entity.salesconditionrecordscalevalue._self", "zh-HK", "Takt销售阶梯价格信息_hk", "实体名称"),

            // entity.salesconditionrecordscalevalue.salesconditionrecorditemid
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecorditemid", "en-US", "销售条件记录行ID_us", "销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecorditemid
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecorditemid", "ja-JP", "销售条件记录行ID_jp", "销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecorditemid
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecorditemid", "zh-CN", "销售条件记录行ID", "销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecorditemid
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecorditemid", "zh-HK", "销售条件记录行ID_hk", "销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）"),

            // entity.salesconditionrecordscalevalue.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordcode", "en-US", "定价记录号_us", "定价记录号（冗余：与明细 SalesConditionRecordCode 一致）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordcode", "ja-JP", "定价记录号_jp", "定价记录号（冗余：与明细 SalesConditionRecordCode 一致）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordcode", "zh-CN", "定价记录号", "定价记录号（冗余：与明细 SalesConditionRecordCode 一致）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordcode", "zh-HK", "定价记录号_hk", "定价记录号（冗余：与明细 SalesConditionRecordCode 一致）"),

            // entity.salesconditionrecordscalevalue.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordseq", "en-US", "定价序号_us", "定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordseq", "ja-JP", "定价序号_jp", "定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordseq", "zh-CN", "定价序号", "定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesconditionrecordseq", "zh-HK", "定价序号_hk", "定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）"),

            // entity.salesconditionrecordscalevalue.salesscaleseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesscaleseq", "en-US", "等级序号_us", "等级序号（回填：同一明细内阶梯序号，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesscaleseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesscaleseq", "ja-JP", "等级序号_jp", "等级序号（回填：同一明细内阶梯序号，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesscaleseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesscaleseq", "zh-CN", "等级序号", "等级序号（回填：同一明细内阶梯序号，固定步长=10）"),
            // entity.salesconditionrecordscalevalue.salesscaleseq
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.salesscaleseq", "zh-HK", "等级序号_hk", "等级序号（回填：同一明细内阶梯序号，固定步长=10）"),

            // entity.salesconditionrecordscalevalue.scalevalue
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.scalevalue", "en-US", "等级值_us", "等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.salesconditionrecordscalevalue.scalevalue
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.scalevalue", "ja-JP", "等级值_jp", "等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.salesconditionrecordscalevalue.scalevalue
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.scalevalue", "zh-CN", "等级值", "等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）"),
            // entity.salesconditionrecordscalevalue.scalevalue
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.scalevalue", "zh-HK", "等级值_hk", "等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）"),

            // entity.salesconditionrecordscalevalue.price
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.price", "en-US", "价格_us", "价格"),
            // entity.salesconditionrecordscalevalue.price
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.price", "ja-JP", "价格_jp", "价格"),
            // entity.salesconditionrecordscalevalue.price
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.price", "zh-CN", "价格", "价格"),
            // entity.salesconditionrecordscalevalue.price
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.price", "zh-HK", "价格_hk", "价格"),

            // entity.salesconditionrecordscalevalue.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.untaxedprice", "en-US", "未税价格_us", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.untaxedprice", "zh-CN", "未税价格", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionrecordscalevalue.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxincludedprice", "en-US", "含税价格_us", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxincludedprice", "zh-CN", "含税价格", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecordscalevalue.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionrecordscalevalue.taxamount
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxamount", "en-US", "税费_us", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecordscalevalue.taxamount
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxamount", "ja-JP", "税费_jp", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecordscalevalue.taxamount
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxamount", "zh-CN", "税费", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecordscalevalue.taxamount
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.taxamount", "zh-HK", "税费_hk", "税费（冗余；含税−未税，打印用）"),

            // entity.salesconditionrecordscalevalue.isobsolete
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecordscalevalue.isobsolete
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecordscalevalue.isobsolete
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecordscalevalue.isobsolete
            new TranslationSeedItem("entity.salesconditionrecordscalevalue.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Sales";
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
