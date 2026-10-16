// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemArgumentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRoutingItemArgument 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktRoutingItemArgument 实体国际化翻译种子（键前缀 entity.routingitemargument.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoutingItemArgumentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRoutingItemArgument 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 routingitemargument 实体翻译...", tenantCode);

        foreach (var item in GetRoutingItemArgumentTranslations())
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

        TaktLogger.Information("TaktRoutingItemArgument 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRoutingItemArgument 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.routingitemargument._self / entity.routingitemargument.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetRoutingItemArgumentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.routingitemargument._self
            new TranslationSeedItem("entity.routingitemargument._self", "en-US", "Routing Item Argument Information_us", "实体名称"),
            // entity.routingitemargument._self
            new TranslationSeedItem("entity.routingitemargument._self", "ja-JP", "工艺路线工序参数定义信息_jp", "实体名称"),
            // entity.routingitemargument._self
            new TranslationSeedItem("entity.routingitemargument._self", "zh-CN", "工艺路线工序参数定义信息", "实体名称"),
            // entity.routingitemargument._self
            new TranslationSeedItem("entity.routingitemargument._self", "zh-HK", "工艺路线工序参数定义信息_hk", "实体名称"),

            // entity.routingitemargument.routingitemid
            new TranslationSeedItem("entity.routingitemargument.routingitemid", "en-US", "工序ID_us", "工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）"),
            // entity.routingitemargument.routingitemid
            new TranslationSeedItem("entity.routingitemargument.routingitemid", "ja-JP", "工序ID_jp", "工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）"),
            // entity.routingitemargument.routingitemid
            new TranslationSeedItem("entity.routingitemargument.routingitemid", "zh-CN", "工序ID", "工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）"),
            // entity.routingitemargument.routingitemid
            new TranslationSeedItem("entity.routingitemargument.routingitemid", "zh-HK", "工序ID_hk", "工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）"),

            // entity.routingitemargument.paramcode
            new TranslationSeedItem("entity.routingitemargument.paramcode", "en-US", "参数编码_us", "参数编码"),
            // entity.routingitemargument.paramcode
            new TranslationSeedItem("entity.routingitemargument.paramcode", "ja-JP", "参数编码_jp", "参数编码"),
            // entity.routingitemargument.paramcode
            new TranslationSeedItem("entity.routingitemargument.paramcode", "zh-CN", "参数编码", "参数编码"),
            // entity.routingitemargument.paramcode
            new TranslationSeedItem("entity.routingitemargument.paramcode", "zh-HK", "参数编码_hk", "参数编码"),

            // entity.routingitemargument.paramname
            new TranslationSeedItem("entity.routingitemargument.paramname", "en-US", "参数名称_us", "参数名称"),
            // entity.routingitemargument.paramname
            new TranslationSeedItem("entity.routingitemargument.paramname", "ja-JP", "参数名称_jp", "参数名称"),
            // entity.routingitemargument.paramname
            new TranslationSeedItem("entity.routingitemargument.paramname", "zh-CN", "参数名称", "参数名称"),
            // entity.routingitemargument.paramname
            new TranslationSeedItem("entity.routingitemargument.paramname", "zh-HK", "参数名称_hk", "参数名称"),

            // entity.routingitemargument.paramunit
            new TranslationSeedItem("entity.routingitemargument.paramunit", "en-US", "单位_us", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.routingitemargument.paramunit
            new TranslationSeedItem("entity.routingitemargument.paramunit", "ja-JP", "单位_jp", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.routingitemargument.paramunit
            new TranslationSeedItem("entity.routingitemargument.paramunit", "zh-CN", "单位", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.routingitemargument.paramunit
            new TranslationSeedItem("entity.routingitemargument.paramunit", "zh-HK", "单位_hk", "单位（字典 logistics_materials_unit_of_measure_code）"),

            // entity.routingitemargument.standardvalue
            new TranslationSeedItem("entity.routingitemargument.standardvalue", "en-US", "标准值_us", "标准值"),
            // entity.routingitemargument.standardvalue
            new TranslationSeedItem("entity.routingitemargument.standardvalue", "ja-JP", "标准值_jp", "标准值"),
            // entity.routingitemargument.standardvalue
            new TranslationSeedItem("entity.routingitemargument.standardvalue", "zh-CN", "标准值", "标准值"),
            // entity.routingitemargument.standardvalue
            new TranslationSeedItem("entity.routingitemargument.standardvalue", "zh-HK", "标准值_hk", "标准值"),

            // entity.routingitemargument.lowerlimit
            new TranslationSeedItem("entity.routingitemargument.lowerlimit", "en-US", "下限_us", "下限"),
            // entity.routingitemargument.lowerlimit
            new TranslationSeedItem("entity.routingitemargument.lowerlimit", "ja-JP", "下限_jp", "下限"),
            // entity.routingitemargument.lowerlimit
            new TranslationSeedItem("entity.routingitemargument.lowerlimit", "zh-CN", "下限", "下限"),
            // entity.routingitemargument.lowerlimit
            new TranslationSeedItem("entity.routingitemargument.lowerlimit", "zh-HK", "下限_hk", "下限"),

            // entity.routingitemargument.upperlimit
            new TranslationSeedItem("entity.routingitemargument.upperlimit", "en-US", "上限_us", "上限"),
            // entity.routingitemargument.upperlimit
            new TranslationSeedItem("entity.routingitemargument.upperlimit", "ja-JP", "上限_jp", "上限"),
            // entity.routingitemargument.upperlimit
            new TranslationSeedItem("entity.routingitemargument.upperlimit", "zh-CN", "上限", "上限"),
            // entity.routingitemargument.upperlimit
            new TranslationSeedItem("entity.routingitemargument.upperlimit", "zh-HK", "上限_hk", "上限"),

            // entity.routingitemargument.sortorder
            new TranslationSeedItem("entity.routingitemargument.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.routingitemargument.sortorder
            new TranslationSeedItem("entity.routingitemargument.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.routingitemargument.sortorder
            new TranslationSeedItem("entity.routingitemargument.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.routingitemargument.sortorder
            new TranslationSeedItem("entity.routingitemargument.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.routingitemargument.routingitem
            new TranslationSeedItem("entity.routingitemargument.routingitem", "en-US", "工序_us", "工序"),
            // entity.routingitemargument.routingitem
            new TranslationSeedItem("entity.routingitemargument.routingitem", "ja-JP", "工序_jp", "工序"),
            // entity.routingitemargument.routingitem
            new TranslationSeedItem("entity.routingitemargument.routingitem", "zh-CN", "工序", "工序"),
            // entity.routingitemargument.routingitem
            new TranslationSeedItem("entity.routingitemargument.routingitem", "zh-HK", "工序_hk", "工序"),
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
        translation.ResourceGroup = "Bom";
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
