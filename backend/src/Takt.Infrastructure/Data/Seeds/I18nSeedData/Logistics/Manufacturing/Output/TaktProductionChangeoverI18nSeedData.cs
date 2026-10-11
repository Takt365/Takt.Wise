// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeoverI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionChangeover 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionChangeover 实体国际化翻译种子（键前缀 entity.productionchangeover.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionChangeoverI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionChangeover 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionchangeover 实体翻译...", tenantCode);

        foreach (var item in GetProductionChangeoverTranslations())
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

        TaktLogger.Information("TaktProductionChangeover 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionChangeover 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionchangeover._self / entity.productionchangeover.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionChangeoverTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "en-US", "Production Changeover Information_us", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "ja-JP", "生产切换记录信息_jp", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "zh-CN", "生产切换记录信息", "实体名称"),
            // entity.productionchangeover._self
            new TranslationSeedItem("entity.productionchangeover._self", "zh-HK", "生产切换记录信息_hk", "实体名称"),

            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "en-US", "生产类别_us", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与 ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "ja-JP", "生产类别_jp", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与 ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "zh-CN", "生产类别", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与 ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.prodcategory
            new TranslationSeedItem("entity.productionchangeover.prodcategory", "zh-HK", "生产类别_hk", "生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与 ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.changeovercategory
            new TranslationSeedItem("entity.productionchangeover.changeovercategory", "en-US", "切换类别_us", "切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA；与 ProdCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeovercategory
            new TranslationSeedItem("entity.productionchangeover.changeovercategory", "ja-JP", "切换类别_jp", "切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA；与 ProdCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeovercategory
            new TranslationSeedItem("entity.productionchangeover.changeovercategory", "zh-CN", "切换类别", "切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA；与 ProdCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeovercategory
            new TranslationSeedItem("entity.productionchangeover.changeovercategory", "zh-HK", "切换类别_hk", "切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA；与 ProdCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "en-US", "生产日期_us", "生产日期（与 ProdCategory、ChangeoverCategory、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "ja-JP", "生产日期_jp", "生产日期（与 ProdCategory、ChangeoverCategory、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "zh-CN", "生产日期", "生产日期（与 ProdCategory、ChangeoverCategory、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.proddate
            new TranslationSeedItem("entity.productionchangeover.proddate", "zh-HK", "生产日期_hk", "生产日期（与 ProdCategory、ChangeoverCategory、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.teamcode
            new TranslationSeedItem("entity.productionchangeover.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤；与 ProdCategory、ChangeoverCategory、ProdDate、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.teamcode
            new TranslationSeedItem("entity.productionchangeover.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤；与 ProdCategory、ChangeoverCategory、ProdDate、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.teamcode
            new TranslationSeedItem("entity.productionchangeover.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤；与 ProdCategory、ChangeoverCategory、ProdDate、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.teamcode
            new TranslationSeedItem("entity.productionchangeover.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤；与 ProdCategory、ChangeoverCategory、ProdDate、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.currentprodordercode
            new TranslationSeedItem("entity.productionchangeover.currentprodordercode", "en-US", "当前工单_us", "当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.currentprodordercode
            new TranslationSeedItem("entity.productionchangeover.currentprodordercode", "ja-JP", "当前工单_jp", "当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.currentprodordercode
            new TranslationSeedItem("entity.productionchangeover.currentprodordercode", "zh-CN", "当前工单", "当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、ChangeoverProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.currentprodordercode
            new TranslationSeedItem("entity.productionchangeover.currentprodordercode", "zh-HK", "当前工单_hk", "当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、ChangeoverProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.currentmodelcode
            new TranslationSeedItem("entity.productionchangeover.currentmodelcode", "en-US", "当前机种_us", "当前机种（回填：随工单）"),
            // entity.productionchangeover.currentmodelcode
            new TranslationSeedItem("entity.productionchangeover.currentmodelcode", "ja-JP", "当前机种_jp", "当前机种（回填：随工单）"),
            // entity.productionchangeover.currentmodelcode
            new TranslationSeedItem("entity.productionchangeover.currentmodelcode", "zh-CN", "当前机种", "当前机种（回填：随工单）"),
            // entity.productionchangeover.currentmodelcode
            new TranslationSeedItem("entity.productionchangeover.currentmodelcode", "zh-HK", "当前机种_hk", "当前机种（回填：随工单）"),

            // entity.productionchangeover.changeoverprodordercode
            new TranslationSeedItem("entity.productionchangeover.changeoverprodordercode", "en-US", "切换后工单_us", "切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeoverprodordercode
            new TranslationSeedItem("entity.productionchangeover.changeoverprodordercode", "ja-JP", "切换后工单_jp", "切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeoverprodordercode
            new TranslationSeedItem("entity.productionchangeover.changeoverprodordercode", "zh-CN", "切换后工单", "切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode 组成唯一键）"),
            // entity.productionchangeover.changeoverprodordercode
            new TranslationSeedItem("entity.productionchangeover.changeoverprodordercode", "zh-HK", "切换后工单_hk", "切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode 组成唯一键）"),

            // entity.productionchangeover.changeovermodelcode
            new TranslationSeedItem("entity.productionchangeover.changeovermodelcode", "en-US", "切换后机种_us", "切换后机种（回填：随工单）"),
            // entity.productionchangeover.changeovermodelcode
            new TranslationSeedItem("entity.productionchangeover.changeovermodelcode", "ja-JP", "切换后机种_jp", "切换后机种（回填：随工单）"),
            // entity.productionchangeover.changeovermodelcode
            new TranslationSeedItem("entity.productionchangeover.changeovermodelcode", "zh-CN", "切换后机种", "切换后机种（回填：随工单）"),
            // entity.productionchangeover.changeovermodelcode
            new TranslationSeedItem("entity.productionchangeover.changeovermodelcode", "zh-HK", "切换后机种_hk", "切换后机种（回填：随工单）"),

            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "en-US", "切换次数_us", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "ja-JP", "切换次数_jp", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "zh-CN", "切换次数", "切换次数"),
            // entity.productionchangeover.changeovercount
            new TranslationSeedItem("entity.productionchangeover.changeovercount", "zh-HK", "切换次数_hk", "切换次数"),

            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "en-US", "切换时间_us", "切换时间（单次，单位：分钟）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "ja-JP", "切换时间_jp", "切换时间（单次，单位：分钟）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "zh-CN", "切换时间", "切换时间（单次，单位：分钟）"),
            // entity.productionchangeover.changeovertime
            new TranslationSeedItem("entity.productionchangeover.changeovertime", "zh-HK", "切换时间_hk", "切换时间（单次，单位：分钟）"),

            // entity.productionchangeover.instrumentsetuptime
            new TranslationSeedItem("entity.productionchangeover.instrumentsetuptime", "en-US", "仪设时间_us", "仪设时间（仪器/设备设置耗时，单位：分钟）"),
            // entity.productionchangeover.instrumentsetuptime
            new TranslationSeedItem("entity.productionchangeover.instrumentsetuptime", "ja-JP", "仪设时间_jp", "仪设时间（仪器/设备设置耗时，单位：分钟）"),
            // entity.productionchangeover.instrumentsetuptime
            new TranslationSeedItem("entity.productionchangeover.instrumentsetuptime", "zh-CN", "仪设时间", "仪设时间（仪器/设备设置耗时，单位：分钟）"),
            // entity.productionchangeover.instrumentsetuptime
            new TranslationSeedItem("entity.productionchangeover.instrumentsetuptime", "zh-HK", "仪设时间_hk", "仪设时间（仪器/设备设置耗时，单位：分钟）"),

            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "en-US", "切换总时间_us", "切换总时间（单位：分钟）"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "ja-JP", "切换总时间_jp", "切换总时间（单位：分钟）"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "zh-CN", "切换总时间", "切换总时间（单位：分钟）"),
            // entity.productionchangeover.totalchangeovertime
            new TranslationSeedItem("entity.productionchangeover.totalchangeovertime", "zh-HK", "切换总时间_hk", "切换总时间（单位：分钟）"),

            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "en-US", "读取SOP时间_us", "读取SOP时间（单位：分钟）"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "ja-JP", "读取SOP时间_jp", "读取SOP时间（单位：分钟）"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "zh-CN", "读取SOP时间", "读取SOP时间（单位：分钟）"),
            // entity.productionchangeover.readsoptime
            new TranslationSeedItem("entity.productionchangeover.readsoptime", "zh-HK", "读取SOP时间_hk", "读取SOP时间（单位：分钟）"),

            // entity.productionchangeover.learningtime
            new TranslationSeedItem("entity.productionchangeover.learningtime", "en-US", "学习时间_us", "学习时间（切换学习/培训耗时，单位：分钟）"),
            // entity.productionchangeover.learningtime
            new TranslationSeedItem("entity.productionchangeover.learningtime", "ja-JP", "学习时间_jp", "学习时间（切换学习/培训耗时，单位：分钟）"),
            // entity.productionchangeover.learningtime
            new TranslationSeedItem("entity.productionchangeover.learningtime", "zh-CN", "学习时间", "学习时间（切换学习/培训耗时，单位：分钟）"),
            // entity.productionchangeover.learningtime
            new TranslationSeedItem("entity.productionchangeover.learningtime", "zh-HK", "学习时间_hk", "学习时间（切换学习/培训耗时，单位：分钟）"),

            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "en-US", "人数_us", "人数（参与切换人数）"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "ja-JP", "人数_jp", "人数（参与切换人数）"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "zh-CN", "人数", "人数（参与切换人数）"),
            // entity.productionchangeover.personcount
            new TranslationSeedItem("entity.productionchangeover.personcount", "zh-HK", "人数_hk", "人数（参与切换人数）"),

            // entity.productionchangeover.totallearningtime
            new TranslationSeedItem("entity.productionchangeover.totallearningtime", "en-US", "学习总时间_us", "学习总时间（单位：分钟）"),
            // entity.productionchangeover.totallearningtime
            new TranslationSeedItem("entity.productionchangeover.totallearningtime", "ja-JP", "学习总时间_jp", "学习总时间（单位：分钟）"),
            // entity.productionchangeover.totallearningtime
            new TranslationSeedItem("entity.productionchangeover.totallearningtime", "zh-CN", "学习总时间", "学习总时间（单位：分钟）"),
            // entity.productionchangeover.totallearningtime
            new TranslationSeedItem("entity.productionchangeover.totallearningtime", "zh-HK", "学习总时间_hk", "学习总时间（单位：分钟）"),

            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "en-US", "SOP总时间_us", "SOP总时间（单位：分钟）"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "ja-JP", "SOP总时间_jp", "SOP总时间（单位：分钟）"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "zh-CN", "SOP总时间", "SOP总时间（单位：分钟）"),
            // entity.productionchangeover.totalsoptime
            new TranslationSeedItem("entity.productionchangeover.totalsoptime", "zh-HK", "SOP总时间_hk", "SOP总时间（单位：分钟）"),
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
        translation.ResourceGroup = "Output";
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
