// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktQuotesI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：二十四节气文案种子（24 节气 × 4 语言；键 common.page.quote.{节气拼音}；中/繁诗句，en/ja 为节气季节问候）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 二十四节气文案国际化种子（中/繁诗句；en-US / ja-JP 为二十四节气季节问候）
/// 幂等性操作：存在则更新，不存在则创建
/// 按立春→大寒顺序；en 为美式 Seasonal greeting，ja 为時候の挨拶
/// </summary>
public class TaktQuotesI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在通用翻译之后）
    /// </summary>
    public int Order => 50;

    /// <summary>
    /// 初始化二十四节气诗词国际化翻译种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化二十四节气诗词国际化翻译种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过二十四节气诗词国际化翻译种子数据初始化");
            return (0, 0);
        }
        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化二十四节气诗词翻译数据...", tenantCode);
        foreach (var row in GetSolarTermQuoteTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }
            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }
        TaktLogger.Information("二十四节气诗词国际化翻译种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取二十四节气文案列表（立春→大寒；键 common.page.quote.{拼音}）
    /// zh-CN/zh-HK：中国古典诗；en-US：美式 24 Solar Terms seasonal greetings；ja-JP：時候の挨拶
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetSolarTermQuoteTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // 立春
            ("common.page.quote.lichun", "zh-CN", "律回岁晚冰霜少，春到人间草木知。", "立春·张栻"),
            ("common.page.quote.lichun", "en-US", "Still a bit chilly out there. Hope you're doing well.", "立春 · Seasonal greeting"),
            ("common.page.quote.lichun", "ja-JP", "春寒の折柄、いかがお過ごしでしょうか。", "立春・時候の挨拶"),
            ("common.page.quote.lichun", "zh-HK", "律回歲晚冰霜少，春到人間草木知。", "立春·張栻"),
            // 雨水
            ("common.page.quote.yushui", "zh-CN", "好雨知时节，当春乃发生。", "雨水·杜甫"),
            ("common.page.quote.yushui", "en-US", "Snowmelt is feeding the land. Hope you're staying well.", "雨水 · Seasonal greeting"),
            ("common.page.quote.yushui", "ja-JP", "雪解けの水が大地を潤す頃となりました。", "雨水・時候の挨拶"),
            ("common.page.quote.yushui", "zh-HK", "好雨知時節，當春乃發生。", "雨水·杜甫"),
            // 惊蛰
            ("common.page.quote.jingzhe", "zh-CN", "微雨众卉新，一雷惊蛰始。", "惊蛰·韦应物"),
            ("common.page.quote.jingzhe", "en-US", "The world is stirring again. Hope you're doing well.", "惊蛰 · Seasonal greeting"),
            ("common.page.quote.jingzhe", "ja-JP", "冬ごもりの虫も動き出す頃となりました。", "啓蟄・時候の挨拶"),
            ("common.page.quote.jingzhe", "zh-HK", "微雨眾卉新，一雷驚蟄始。", "驚蟄·韋應物"),
            // 春分
            ("common.page.quote.chunfen", "zh-CN", "等闲识得东风面，万紫千红总是春。", "春分·朱熹"),
            ("common.page.quote.chunfen", "en-US", "Day and night are even. Hope you're enjoying the season.", "春分 · Seasonal greeting"),
            ("common.page.quote.chunfen", "ja-JP", "春たけなわの折、いかがお過ごしでしょうか。", "春分・時候の挨拶"),
            ("common.page.quote.chunfen", "zh-HK", "等閒識得東風面，萬紫千紅總是春。", "春分·朱熹"),
            // 清明
            ("common.page.quote.qingming", "zh-CN", "清明时节雨纷纷，路上行人欲断魂。", "清明·杜牧"),
            ("common.page.quote.qingming", "en-US", "Soft spring air all around. Hope you're doing well.", "清明 · Seasonal greeting"),
            ("common.page.quote.qingming", "ja-JP", "清らかな春の陽気に包まれる頃となりました。", "清明・時候の挨拶"),
            ("common.page.quote.qingming", "zh-HK", "清明時節雨紛紛，路上行人欲斷魂。", "清明·杜牧"),
            // 谷雨
            ("common.page.quote.guyu", "zh-CN", "雨前初见花间蕊，雨后全无叶底花。", "谷雨·王驾"),
            ("common.page.quote.guyu", "en-US", "Those spring showers that help the crops. Hope you're staying well.", "谷雨 · Seasonal greeting"),
            ("common.page.quote.guyu", "ja-JP", "百穀を潤す恵みの雨の季節となりました。", "穀雨・時候の挨拶"),
            ("common.page.quote.guyu", "zh-HK", "雨前初見花間蕊，雨後全無葉底花。", "穀雨·王駕"),
            // 立夏
            ("common.page.quote.lixia", "zh-CN", "孟夏草木长，绕屋树扶疏。", "立夏·陶渊明"),
            ("common.page.quote.lixia", "en-US", "Fresh green everywhere. Hope you're doing well.", "立夏 · Seasonal greeting"),
            ("common.page.quote.lixia", "ja-JP", "新緑まぶしい季節となりました。", "立夏・時候の挨拶"),
            ("common.page.quote.lixia", "zh-HK", "孟夏草木長，繞屋樹扶疏。", "立夏·陶淵明"),
            // 小满
            ("common.page.quote.xiaoman", "zh-CN", "夜莺啼绿柳，皓月醒长空。", "小满·节气诗"),
            ("common.page.quote.xiaoman", "en-US", "Everything's filling out nicely. Hope you're doing well.", "小满 · Seasonal greeting"),
            ("common.page.quote.xiaoman", "ja-JP", "万物が満ち始める頃となりました。", "小満・時候の挨拶"),
            ("common.page.quote.xiaoman", "zh-HK", "夜鶯啼綠柳，皓月醒長空。", "小滿·節氣詩"),
            // 芒种
            ("common.page.quote.mangzhong", "zh-CN", "绿遍山原白满川，子规声里雨如烟。", "芒种·翁卷"),
            ("common.page.quote.mangzhong", "en-US", "Planting season is in full swing. Hope you're staying well.", "芒种 · Seasonal greeting"),
            ("common.page.quote.mangzhong", "ja-JP", "稲の種まきを急ぐ頃となりました。", "芒種・時候の挨拶"),
            ("common.page.quote.mangzhong", "zh-HK", "綠遍山原白滿川，子規聲裏雨如煙。", "芒種·翁卷"),
            // 夏至
            ("common.page.quote.xiazhi", "zh-CN", "绿树阴浓夏日长，楼台倒影入池塘。", "夏至·高骈"),
            ("common.page.quote.xiazhi", "en-US", "The longest days of the year. Hope you're enjoying them.", "夏至 · Seasonal greeting"),
            ("common.page.quote.xiazhi", "ja-JP", "一年で最も昼の長い季節となりました。", "夏至・時候の挨拶"),
            ("common.page.quote.xiazhi", "zh-HK", "綠樹陰濃夏日長，樓臺倒影入池塘。", "夏至·高駢"),
            // 小暑
            ("common.page.quote.xiaoshu", "zh-CN", "倏忽温风至，因循小暑来。", "小暑·元稹"),
            ("common.page.quote.xiaoshu", "en-US", "The real warmth is just getting started. Stay cool.", "小暑 · Seasonal greeting"),
            ("common.page.quote.xiaoshu", "ja-JP", "本格的な暑さの始まりとなりました。", "小暑・時候の挨拶"),
            ("common.page.quote.xiaoshu", "zh-HK", "倏忽溫風至，因循小暑來。", "小暑·元稹"),
            // 大暑
            ("common.page.quote.dashu", "zh-CN", "赤日炎炎似火烧，野田禾稻半枯焦。", "大暑·水浒传"),
            ("common.page.quote.dashu", "en-US", "Peak summer heat is here. Stay cool and take care.", "大暑 · Seasonal greeting"),
            ("common.page.quote.dashu", "ja-JP", "連日厳しい暑さが続いております。", "大暑・時候の挨拶"),
            ("common.page.quote.dashu", "zh-HK", "赤日炎炎似火燒，野田禾稻半枯焦。", "大暑·水滸傳"),
            // 立秋
            ("common.page.quote.liqiu", "zh-CN", "乳鸦啼散玉屏空，一枕新凉一扇风。", "立秋·刘翰"),
            ("common.page.quote.liqiu", "en-US", "Still plenty of heat left. Hope you're hanging in there.", "立秋 · Seasonal greeting"),
            ("common.page.quote.liqiu", "ja-JP", "残暑厳しき折、いかがお過ごしでしょうか。", "立秋・時候の挨拶"),
            ("common.page.quote.liqiu", "zh-HK", "乳鴉啼散玉屏空，一枕新涼一扇風。", "立秋·劉翰"),
            // 处暑
            ("common.page.quote.chushu", "zh-CN", "秋风萧瑟天气凉，草木摇落露为霜。", "处暑·曹丕"),
            ("common.page.quote.chushu", "en-US", "Mornings and evenings are finally cooling off. Hope you're well.", "处暑 · Seasonal greeting"),
            ("common.page.quote.chushu", "ja-JP", "朝晩しだいに涼しくなってまいりました。", "処暑・時候の挨拶"),
            ("common.page.quote.chushu", "zh-HK", "秋風蕭瑟天氣涼，草木搖落露為霜。", "處暑·曹丕"),
            // 白露
            ("common.page.quote.bailu", "zh-CN", "蒹葭苍苍，白露为霜。", "白露·诗经"),
            ("common.page.quote.bailu", "en-US", "Cool dew on the grass each morning. Hope you're doing well.", "白露 · Seasonal greeting"),
            ("common.page.quote.bailu", "ja-JP", "草花に白い露が降りる頃となりました。", "白露・時候の挨拶"),
            ("common.page.quote.bailu", "zh-HK", "蒹葭蒼蒼，白露為霜。", "白露·詩經"),
            // 秋分
            ("common.page.quote.qiufen", "zh-CN", "秋分气爽云天阔，红叶黄花处处明。", "秋分·节气诗"),
            ("common.page.quote.qiufen", "en-US", "Fall is in full swing. Hope you're enjoying the season.", "秋分 · Seasonal greeting"),
            ("common.page.quote.qiufen", "ja-JP", "秋たけなわの折、いかがお過ごしでしょうか。", "秋分・時候の挨拶"),
            ("common.page.quote.qiufen", "zh-HK", "秋分氣爽雲天闊，紅葉黃花處處明。", "秋分·節氣詩"),
            // 寒露
            ("common.page.quote.hanlu", "zh-CN", "寒露惊秋晚，朝看菊渐黄。", "寒露·节气诗"),
            ("common.page.quote.hanlu", "en-US", "Mornings are getting crisp. Hope you're staying warm.", "寒露 · Seasonal greeting"),
            ("common.page.quote.hanlu", "ja-JP", "露も冷たく感じられる頃となりました。", "寒露・時候の挨拶"),
            ("common.page.quote.hanlu", "zh-HK", "寒露驚秋晚，朝看菊漸黃。", "寒露·節氣詩"),
            // 霜降
            ("common.page.quote.shuangjiang", "zh-CN", "月落乌啼霜满天，江枫渔火对愁眠。", "霜降·张继"),
            ("common.page.quote.shuangjiang", "en-US", "Fall is deepening. Hope you're doing well.", "霜降 · Seasonal greeting"),
            ("common.page.quote.shuangjiang", "ja-JP", "霜が降り始め秋も深まってまいりました。", "霜降・時候の挨拶"),
            ("common.page.quote.shuangjiang", "zh-HK", "月落烏啼霜滿天，江楓漁火對愁眠。", "霜降·張繼"),
            // 立冬
            ("common.page.quote.lidong", "zh-CN", "荷尽已无擎雨盖，菊残犹有傲霜枝。", "立冬·苏轼"),
            ("common.page.quote.lidong", "en-US", "You can feel the season turning. Stay warm.", "立冬 · Seasonal greeting"),
            ("common.page.quote.lidong", "ja-JP", "冬の気配が感じられる頃となりました。", "立冬・時候の挨拶"),
            ("common.page.quote.lidong", "zh-HK", "荷盡已無擎雨蓋，菊殘猶有傲霜枝。", "立冬·蘇軾"),
            // 小雪
            ("common.page.quote.xiaoxue", "zh-CN", "夜深知雪重，时闻折竹声。", "小雪·白居易"),
            ("common.page.quote.xiaoxue", "en-US", "The first flurries may be on the way. Stay warm.", "小雪 · Seasonal greeting"),
            ("common.page.quote.xiaoxue", "ja-JP", "初雪の便りが届く頃となりました。", "小雪・時候の挨拶"),
            ("common.page.quote.xiaoxue", "zh-HK", "夜深知雪重，時聞折竹聲。", "小雪·白居易"),
            // 大雪
            ("common.page.quote.daxue", "zh-CN", "燕山雪花大如席，片片吹落轩辕台。", "大雪·李白"),
            ("common.page.quote.daxue", "en-US", "Winter is here in earnest. Stay warm and take care.", "大雪 · Seasonal greeting"),
            ("common.page.quote.daxue", "ja-JP", "本格的な冬の到来となりました。", "大雪・時候の挨拶"),
            ("common.page.quote.daxue", "zh-HK", "燕山雪花大如席，片片吹落軒轅台。", "大雪·李白"),
            // 冬至
            ("common.page.quote.dongzhi", "zh-CN", "邯郸驿里逢冬至，抱膝灯前影伴身。", "冬至·白居易"),
            ("common.page.quote.dongzhi", "en-US", "The longest nights of the year. Hope you're doing well.", "冬至 · Seasonal greeting"),
            ("common.page.quote.dongzhi", "ja-JP", "一年で最も夜の長い季節となりました。", "冬至・時候の挨拶"),
            ("common.page.quote.dongzhi", "zh-HK", "邯鄲驛裏逢冬至，抱膝燈前影伴身。", "冬至·白居易"),
            // 小寒
            ("common.page.quote.xiaohan", "zh-CN", "小寒时节雪纷纷，笑语声声入梦中。", "小寒·节气诗"),
            ("common.page.quote.xiaohan", "en-US", "The chill is really settling in. Stay warm.", "小寒 · Seasonal greeting"),
            ("common.page.quote.xiaohan", "ja-JP", "寒さひとしお厳しくなってまいりました。", "小寒・時候の挨拶"),
            ("common.page.quote.xiaohan", "zh-HK", "小寒時節雪紛紛，笑語聲聲入夢中。", "小寒·節氣詩"),
            // 大寒
            ("common.page.quote.dahan", "zh-CN", "岁寒然后知松柏之后凋也。", "大寒·论语"),
            ("common.page.quote.dahan", "en-US", "The coldest stretch of the year. Stay warm and take care.", "大寒 · Seasonal greeting"),
            ("common.page.quote.dahan", "ja-JP", "一年で最も寒い盛りとなりました。", "大寒・時候の挨拶"),
            ("common.page.quote.dahan", "zh-HK", "歲寒然後知松柏之後凋也。", "大寒·論語"),
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
        translation.ResourceGroup = "Foundation";
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
