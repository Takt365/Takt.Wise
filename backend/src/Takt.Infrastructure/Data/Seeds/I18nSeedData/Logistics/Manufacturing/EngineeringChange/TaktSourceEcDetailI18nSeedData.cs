// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSourceEcDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktSourceEcDetail 实体国际化翻译种子（键前缀 entity.sourceecdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSourceEcDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSourceEcDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sourceecdetail 实体翻译...", tenantCode);

        foreach (var item in GetSourceEcDetailTranslations())
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

        TaktLogger.Information("TaktSourceEcDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSourceEcDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sourceecdetail._self / entity.sourceecdetail.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSourceEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "en-US", "Source Ec Detail Information_us", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "ja-JP", "设变来源子表信息_jp", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "zh-CN", "设变来源子表信息", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "zh-HK", "设变来源子表信息_hk", "实体名称"),

            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "en-US", "主ID_us", "主ID（选项 TaktSourceEcs/options；DictValue=Id）"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "ja-JP", "主ID_jp", "主ID（选项 TaktSourceEcs/options；DictValue=Id）"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "zh-CN", "主ID", "主ID（选项 TaktSourceEcs/options；DictValue=Id）"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "zh-HK", "主ID_hk", "主ID（选项 TaktSourceEcs/options；DictValue=Id）"),

            // entity.sourceecdetail.sourceeccode
            new TranslationSeedItem("entity.sourceecdetail.sourceeccode", "en-US", "设变号码_us", "设变号码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.sourceecdetail.sourceeccode
            new TranslationSeedItem("entity.sourceecdetail.sourceeccode", "ja-JP", "设变号码_jp", "设变号码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.sourceecdetail.sourceeccode
            new TranslationSeedItem("entity.sourceecdetail.sourceeccode", "zh-CN", "设变号码", "设变号码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.sourceecdetail.sourceeccode
            new TranslationSeedItem("entity.sourceecdetail.sourceeccode", "zh-HK", "设变号码_hk", "设变号码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.sourceecdetail.linenumber
            new TranslationSeedItem("entity.sourceecdetail.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.sourceecdetail.linenumber
            new TranslationSeedItem("entity.sourceecdetail.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.sourceecdetail.linenumber
            new TranslationSeedItem("entity.sourceecdetail.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.sourceecdetail.linenumber
            new TranslationSeedItem("entity.sourceecdetail.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.sourceecdetail.sourcerootmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcerootmaterialcode", "en-US", "根物料编码_us", "根物料编码"),
            // entity.sourceecdetail.sourcerootmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcerootmaterialcode", "ja-JP", "根物料编码_jp", "根物料编码"),
            // entity.sourceecdetail.sourcerootmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcerootmaterialcode", "zh-CN", "根物料编码", "根物料编码"),
            // entity.sourceecdetail.sourcerootmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcerootmaterialcode", "zh-HK", "根物料编码_hk", "根物料编码"),

            // entity.sourceecdetail.sourceparentmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceparentmaterialcode", "en-US", "上阶物料编码_us", "上阶物料编码"),
            // entity.sourceecdetail.sourceparentmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceparentmaterialcode", "ja-JP", "上阶物料编码_jp", "上阶物料编码"),
            // entity.sourceecdetail.sourceparentmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码"),
            // entity.sourceecdetail.sourceparentmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceparentmaterialcode", "zh-HK", "上阶物料编码_hk", "上阶物料编码"),

            // entity.sourceecdetail.sourceoldmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialcode", "en-US", "旧物料编码_us", "旧物料编码"),
            // entity.sourceecdetail.sourceoldmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialcode", "ja-JP", "旧物料编码_jp", "旧物料编码"),
            // entity.sourceecdetail.sourceoldmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialcode", "zh-CN", "旧物料编码", "旧物料编码"),
            // entity.sourceecdetail.sourceoldmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialcode", "zh-HK", "旧物料编码_hk", "旧物料编码"),

            // entity.sourceecdetail.sourceoldmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialdescription", "en-US", "旧物料描述_us", "旧物料描述"),
            // entity.sourceecdetail.sourceoldmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialdescription", "ja-JP", "旧物料描述_jp", "旧物料描述"),
            // entity.sourceecdetail.sourceoldmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialdescription", "zh-CN", "旧物料描述", "旧物料描述"),
            // entity.sourceecdetail.sourceoldmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourceoldmaterialdescription", "zh-HK", "旧物料描述_hk", "旧物料描述"),

            // entity.sourceecdetail.sourceoldusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourceoldusagequantity", "en-US", "旧物料用量_us", "旧物料用量"),
            // entity.sourceecdetail.sourceoldusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourceoldusagequantity", "ja-JP", "旧物料用量_jp", "旧物料用量"),
            // entity.sourceecdetail.sourceoldusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourceoldusagequantity", "zh-CN", "旧物料用量", "旧物料用量"),
            // entity.sourceecdetail.sourceoldusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourceoldusagequantity", "zh-HK", "旧物料用量_hk", "旧物料用量"),

            // entity.sourceecdetail.sourceolditemposition
            new TranslationSeedItem("entity.sourceecdetail.sourceolditemposition", "en-US", "旧物料安装位置_us", "旧物料安装位置"),
            // entity.sourceecdetail.sourceolditemposition
            new TranslationSeedItem("entity.sourceecdetail.sourceolditemposition", "ja-JP", "旧物料安装位置_jp", "旧物料安装位置"),
            // entity.sourceecdetail.sourceolditemposition
            new TranslationSeedItem("entity.sourceecdetail.sourceolditemposition", "zh-CN", "旧物料安装位置", "旧物料安装位置"),
            // entity.sourceecdetail.sourceolditemposition
            new TranslationSeedItem("entity.sourceecdetail.sourceolditemposition", "zh-HK", "旧物料安装位置_hk", "旧物料安装位置"),

            // entity.sourceecdetail.sourcenewmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialcode", "en-US", "新物料编码_us", "新物料编码"),
            // entity.sourceecdetail.sourcenewmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码"),
            // entity.sourceecdetail.sourcenewmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialcode", "zh-CN", "新物料编码", "新物料编码"),
            // entity.sourceecdetail.sourcenewmaterialcode
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码"),

            // entity.sourceecdetail.sourcenewmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialdescription", "en-US", "新物料描述_us", "新物料描述"),
            // entity.sourceecdetail.sourcenewmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述"),
            // entity.sourceecdetail.sourcenewmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialdescription", "zh-CN", "新物料描述", "新物料描述"),
            // entity.sourceecdetail.sourcenewmaterialdescription
            new TranslationSeedItem("entity.sourceecdetail.sourcenewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述"),

            // entity.sourceecdetail.sourcenewusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourcenewusagequantity", "en-US", "新物料用量_us", "新物料用量"),
            // entity.sourceecdetail.sourcenewusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourcenewusagequantity", "ja-JP", "新物料用量_jp", "新物料用量"),
            // entity.sourceecdetail.sourcenewusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourcenewusagequantity", "zh-CN", "新物料用量", "新物料用量"),
            // entity.sourceecdetail.sourcenewusagequantity
            new TranslationSeedItem("entity.sourceecdetail.sourcenewusagequantity", "zh-HK", "新物料用量_hk", "新物料用量"),

            // entity.sourceecdetail.sourcenewitemposition
            new TranslationSeedItem("entity.sourceecdetail.sourcenewitemposition", "en-US", "新物料安装位置_us", "新物料安装位置"),
            // entity.sourceecdetail.sourcenewitemposition
            new TranslationSeedItem("entity.sourceecdetail.sourcenewitemposition", "ja-JP", "新物料安装位置_jp", "新物料安装位置"),
            // entity.sourceecdetail.sourcenewitemposition
            new TranslationSeedItem("entity.sourceecdetail.sourcenewitemposition", "zh-CN", "新物料安装位置", "新物料安装位置"),
            // entity.sourceecdetail.sourcenewitemposition
            new TranslationSeedItem("entity.sourceecdetail.sourcenewitemposition", "zh-HK", "新物料安装位置_hk", "新物料安装位置"),

            // entity.sourceecdetail.sourcebomcode
            new TranslationSeedItem("entity.sourceecdetail.sourcebomcode", "en-US", "BOM番号_us", "BOM番号"),
            // entity.sourceecdetail.sourcebomcode
            new TranslationSeedItem("entity.sourceecdetail.sourcebomcode", "ja-JP", "BOM番号_jp", "BOM番号"),
            // entity.sourceecdetail.sourcebomcode
            new TranslationSeedItem("entity.sourceecdetail.sourcebomcode", "zh-CN", "BOM番号", "BOM番号"),
            // entity.sourceecdetail.sourcebomcode
            new TranslationSeedItem("entity.sourceecdetail.sourcebomcode", "zh-HK", "BOM番号_hk", "BOM番号"),

            // entity.sourceecdetail.sourcecompatibility
            new TranslationSeedItem("entity.sourceecdetail.sourcecompatibility", "en-US", "兼容性_us", "兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）"),
            // entity.sourceecdetail.sourcecompatibility
            new TranslationSeedItem("entity.sourceecdetail.sourcecompatibility", "ja-JP", "兼容性_jp", "兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）"),
            // entity.sourceecdetail.sourcecompatibility
            new TranslationSeedItem("entity.sourceecdetail.sourcecompatibility", "zh-CN", "兼容性", "兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）"),
            // entity.sourceecdetail.sourcecompatibility
            new TranslationSeedItem("entity.sourceecdetail.sourcecompatibility", "zh-HK", "兼容性_hk", "兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）"),

            // entity.sourceecdetail.source2ndvendor
            new TranslationSeedItem("entity.sourceecdetail.source2ndvendor", "en-US", "第二供应商_us", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.sourceecdetail.source2ndvendor
            new TranslationSeedItem("entity.sourceecdetail.source2ndvendor", "ja-JP", "第二供应商_jp", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.sourceecdetail.source2ndvendor
            new TranslationSeedItem("entity.sourceecdetail.source2ndvendor", "zh-CN", "第二供应商", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.sourceecdetail.source2ndvendor
            new TranslationSeedItem("entity.sourceecdetail.source2ndvendor", "zh-HK", "第二供应商_hk", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),

            // entity.sourceecdetail.sourceinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourceinstruction", "en-US", "安排指示_us", "安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.sourceecdetail.sourceinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourceinstruction", "ja-JP", "安排指示_jp", "安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.sourceecdetail.sourceinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourceinstruction", "zh-CN", "安排指示", "安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.sourceecdetail.sourceinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourceinstruction", "zh-HK", "安排指示_hk", "安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),

            // entity.sourceecdetail.sourceoldpartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourceoldpartdisposition", "en-US", "旧物料处理_us", "旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.sourceecdetail.sourceoldpartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourceoldpartdisposition", "ja-JP", "旧物料处理_jp", "旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.sourceecdetail.sourceoldpartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourceoldpartdisposition", "zh-CN", "旧物料处理", "旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.sourceecdetail.sourceoldpartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourceoldpartdisposition", "zh-HK", "旧物料处理_hk", "旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),

            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "en-US", "BOM生效日期_us", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "ja-JP", "BOM生效日期_jp", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "zh-CN", "BOM生效日期", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "zh-HK", "BOM生效日期_hk", "BOM生效日期"),

            // entity.sourceecdetail.isobsolete
            new TranslationSeedItem("entity.sourceecdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.sourceecdetail.isobsolete
            new TranslationSeedItem("entity.sourceecdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.sourceecdetail.isobsolete
            new TranslationSeedItem("entity.sourceecdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.sourceecdetail.isobsolete
            new TranslationSeedItem("entity.sourceecdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "en-US", "设变来源主表_us", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "ja-JP", "设变来源主表_jp", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "zh-CN", "设变来源主表", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "zh-HK", "设变来源主表_hk", "设变来源主表"),
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
        translation.ResourceGroup = "EngineeringChange";
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
