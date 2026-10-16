// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRoutingItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktRoutingItem 实体国际化翻译种子（键前缀 entity.routingitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoutingItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRoutingItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 routingitem 实体翻译...", tenantCode);

        foreach (var item in GetRoutingItemTranslations())
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

        TaktLogger.Information("TaktRoutingItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRoutingItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.routingitem._self / entity.routingitem.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetRoutingItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.routingitem._self
            new TranslationSeedItem("entity.routingitem._self", "en-US", "Routing Item Information_us", "实体名称"),
            // entity.routingitem._self
            new TranslationSeedItem("entity.routingitem._self", "ja-JP", "工艺路线明细表信息_jp", "实体名称"),
            // entity.routingitem._self
            new TranslationSeedItem("entity.routingitem._self", "zh-CN", "工艺路线明细表信息", "实体名称"),
            // entity.routingitem._self
            new TranslationSeedItem("entity.routingitem._self", "zh-HK", "工艺路线明细表信息_hk", "实体名称"),

            // entity.routingitem.routingid
            new TranslationSeedItem("entity.routingitem.routingid", "en-US", "工艺路线ID_us", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingitem.routingid
            new TranslationSeedItem("entity.routingitem.routingid", "ja-JP", "工艺路线ID_jp", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingitem.routingid
            new TranslationSeedItem("entity.routingitem.routingid", "zh-CN", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingitem.routingid
            new TranslationSeedItem("entity.routingitem.routingid", "zh-HK", "工艺路线ID_hk", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.routingitem.routingcode
            new TranslationSeedItem("entity.routingitem.routingcode", "en-US", "工艺路线编码_us", "工艺路线编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.routingitem.routingcode
            new TranslationSeedItem("entity.routingitem.routingcode", "ja-JP", "工艺路线编码_jp", "工艺路线编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.routingitem.routingcode
            new TranslationSeedItem("entity.routingitem.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.routingitem.routingcode
            new TranslationSeedItem("entity.routingitem.routingcode", "zh-HK", "工艺路线编码_hk", "工艺路线编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.routingitem.linenumber
            new TranslationSeedItem("entity.routingitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.routingitem.linenumber
            new TranslationSeedItem("entity.routingitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.routingitem.linenumber
            new TranslationSeedItem("entity.routingitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.routingitem.linenumber
            new TranslationSeedItem("entity.routingitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.routingitem.baseunit
            new TranslationSeedItem("entity.routingitem.baseunit", "en-US", "计量单位_us", "作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.routingitem.baseunit
            new TranslationSeedItem("entity.routingitem.baseunit", "ja-JP", "计量单位_jp", "作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.routingitem.baseunit
            new TranslationSeedItem("entity.routingitem.baseunit", "zh-CN", "计量单位", "作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.routingitem.baseunit
            new TranslationSeedItem("entity.routingitem.baseunit", "zh-HK", "计量单位_hk", "作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.routingitem.basequantity
            new TranslationSeedItem("entity.routingitem.basequantity", "en-US", "基本数量_us", "基本数量"),
            // entity.routingitem.basequantity
            new TranslationSeedItem("entity.routingitem.basequantity", "ja-JP", "基本数量_jp", "基本数量"),
            // entity.routingitem.basequantity
            new TranslationSeedItem("entity.routingitem.basequantity", "zh-CN", "基本数量", "基本数量"),
            // entity.routingitem.basequantity
            new TranslationSeedItem("entity.routingitem.basequantity", "zh-HK", "基本数量_hk", "基本数量"),

            // entity.routingitem.standardminutes
            new TranslationSeedItem("entity.routingitem.standardminutes", "en-US", "标准工时_us", "标准工时（分钟）"),
            // entity.routingitem.standardminutes
            new TranslationSeedItem("entity.routingitem.standardminutes", "ja-JP", "标准工时_jp", "标准工时（分钟）"),
            // entity.routingitem.standardminutes
            new TranslationSeedItem("entity.routingitem.standardminutes", "zh-CN", "标准工时", "标准工时（分钟）"),
            // entity.routingitem.standardminutes
            new TranslationSeedItem("entity.routingitem.standardminutes", "zh-HK", "标准工时_hk", "标准工时（分钟）"),

            // entity.routingitem.timeunit
            new TranslationSeedItem("entity.routingitem.timeunit", "en-US", "工时单位_us", "工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）"),
            // entity.routingitem.timeunit
            new TranslationSeedItem("entity.routingitem.timeunit", "ja-JP", "工时单位_jp", "工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）"),
            // entity.routingitem.timeunit
            new TranslationSeedItem("entity.routingitem.timeunit", "zh-CN", "工时单位", "工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）"),
            // entity.routingitem.timeunit
            new TranslationSeedItem("entity.routingitem.timeunit", "zh-HK", "工时单位_hk", "工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）"),

            // entity.routingitem.standardshorts
            new TranslationSeedItem("entity.routingitem.standardshorts", "en-US", "标准点数_us", "标准点数"),
            // entity.routingitem.standardshorts
            new TranslationSeedItem("entity.routingitem.standardshorts", "ja-JP", "标准点数_jp", "标准点数"),
            // entity.routingitem.standardshorts
            new TranslationSeedItem("entity.routingitem.standardshorts", "zh-CN", "标准点数", "标准点数"),
            // entity.routingitem.standardshorts
            new TranslationSeedItem("entity.routingitem.standardshorts", "zh-HK", "标准点数_hk", "标准点数"),

            // entity.routingitem.pointsunit
            new TranslationSeedItem("entity.routingitem.pointsunit", "en-US", "点数单位_us", "点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）"),
            // entity.routingitem.pointsunit
            new TranslationSeedItem("entity.routingitem.pointsunit", "ja-JP", "点数单位_jp", "点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）"),
            // entity.routingitem.pointsunit
            new TranslationSeedItem("entity.routingitem.pointsunit", "zh-CN", "点数单位", "点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）"),
            // entity.routingitem.pointsunit
            new TranslationSeedItem("entity.routingitem.pointsunit", "zh-HK", "点数单位_hk", "点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）"),

            // entity.routingitem.pointstominutesrate
            new TranslationSeedItem("entity.routingitem.pointstominutesrate", "en-US", "转换汇率_us", "点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）"),
            // entity.routingitem.pointstominutesrate
            new TranslationSeedItem("entity.routingitem.pointstominutesrate", "ja-JP", "转换汇率_jp", "点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）"),
            // entity.routingitem.pointstominutesrate
            new TranslationSeedItem("entity.routingitem.pointstominutesrate", "zh-CN", "转换汇率", "点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）"),
            // entity.routingitem.pointstominutesrate
            new TranslationSeedItem("entity.routingitem.pointstominutesrate", "zh-HK", "转换汇率_hk", "点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）"),

            // entity.routingitem.convertedminutes
            new TranslationSeedItem("entity.routingitem.convertedminutes", "en-US", "转换工时_us", "转换后标准工时（分钟）"),
            // entity.routingitem.convertedminutes
            new TranslationSeedItem("entity.routingitem.convertedminutes", "ja-JP", "转换工时_jp", "转换后标准工时（分钟）"),
            // entity.routingitem.convertedminutes
            new TranslationSeedItem("entity.routingitem.convertedminutes", "zh-CN", "转换工时", "转换后标准工时（分钟）"),
            // entity.routingitem.convertedminutes
            new TranslationSeedItem("entity.routingitem.convertedminutes", "zh-HK", "转换工时_hk", "转换后标准工时（分钟）"),

            // entity.routingitem.setupminutes
            new TranslationSeedItem("entity.routingitem.setupminutes", "en-US", "准备时间_us", "准备时间（分钟），如换模、调试等"),
            // entity.routingitem.setupminutes
            new TranslationSeedItem("entity.routingitem.setupminutes", "ja-JP", "准备时间_jp", "准备时间（分钟），如换模、调试等"),
            // entity.routingitem.setupminutes
            new TranslationSeedItem("entity.routingitem.setupminutes", "zh-CN", "准备时间", "准备时间（分钟），如换模、调试等"),
            // entity.routingitem.setupminutes
            new TranslationSeedItem("entity.routingitem.setupminutes", "zh-HK", "准备时间_hk", "准备时间（分钟），如换模、调试等"),

            // entity.routingitem.teardownminutes
            new TranslationSeedItem("entity.routingitem.teardownminutes", "en-US", "清理时间_us", "清理时间（分钟），如清洁、整理等"),
            // entity.routingitem.teardownminutes
            new TranslationSeedItem("entity.routingitem.teardownminutes", "ja-JP", "清理时间_jp", "清理时间（分钟），如清洁、整理等"),
            // entity.routingitem.teardownminutes
            new TranslationSeedItem("entity.routingitem.teardownminutes", "zh-CN", "清理时间", "清理时间（分钟），如清洁、整理等"),
            // entity.routingitem.teardownminutes
            new TranslationSeedItem("entity.routingitem.teardownminutes", "zh-HK", "清理时间_hk", "清理时间（分钟），如清洁、整理等"),

            // entity.routingitem.isinspection
            new TranslationSeedItem("entity.routingitem.isinspection", "en-US", "检验_us", "检验（字典 sys_yes_no：0=否，1=是）"),
            // entity.routingitem.isinspection
            new TranslationSeedItem("entity.routingitem.isinspection", "ja-JP", "检验_jp", "检验（字典 sys_yes_no：0=否，1=是）"),
            // entity.routingitem.isinspection
            new TranslationSeedItem("entity.routingitem.isinspection", "zh-CN", "检验", "检验（字典 sys_yes_no：0=否，1=是）"),
            // entity.routingitem.isinspection
            new TranslationSeedItem("entity.routingitem.isinspection", "zh-HK", "检验_hk", "检验（字典 sys_yes_no：0=否，1=是）"),

            // entity.routingitem.sortorder
            new TranslationSeedItem("entity.routingitem.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.routingitem.sortorder
            new TranslationSeedItem("entity.routingitem.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.routingitem.sortorder
            new TranslationSeedItem("entity.routingitem.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.routingitem.sortorder
            new TranslationSeedItem("entity.routingitem.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.routingitem.processdescription
            new TranslationSeedItem("entity.routingitem.processdescription", "en-US", "工序说明_us", "工序说明"),
            // entity.routingitem.processdescription
            new TranslationSeedItem("entity.routingitem.processdescription", "ja-JP", "工序说明_jp", "工序说明"),
            // entity.routingitem.processdescription
            new TranslationSeedItem("entity.routingitem.processdescription", "zh-CN", "工序说明", "工序说明"),
            // entity.routingitem.processdescription
            new TranslationSeedItem("entity.routingitem.processdescription", "zh-HK", "工序说明_hk", "工序说明"),

            // entity.routingitem.processsegmenttype
            new TranslationSeedItem("entity.routingitem.processsegmenttype", "en-US", "工艺段类型_us", "工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.routingitem.processsegmenttype
            new TranslationSeedItem("entity.routingitem.processsegmenttype", "ja-JP", "工艺段类型_jp", "工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.routingitem.processsegmenttype
            new TranslationSeedItem("entity.routingitem.processsegmenttype", "zh-CN", "工艺段类型", "工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.routingitem.processsegmenttype
            new TranslationSeedItem("entity.routingitem.processsegmenttype", "zh-HK", "工艺段类型_hk", "工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）"),

            // entity.routingitem.extjson
            new TranslationSeedItem("entity.routingitem.extjson", "en-US", "工序扩展JSON_us", "工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）"),
            // entity.routingitem.extjson
            new TranslationSeedItem("entity.routingitem.extjson", "ja-JP", "工序扩展JSON_jp", "工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）"),
            // entity.routingitem.extjson
            new TranslationSeedItem("entity.routingitem.extjson", "zh-CN", "工序扩展JSON", "工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）"),
            // entity.routingitem.extjson
            new TranslationSeedItem("entity.routingitem.extjson", "zh-HK", "工序扩展JSON_hk", "工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）"),

            // entity.routingitem.isobsolete
            new TranslationSeedItem("entity.routingitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.routingitem.isobsolete
            new TranslationSeedItem("entity.routingitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.routingitem.isobsolete
            new TranslationSeedItem("entity.routingitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.routingitem.isobsolete
            new TranslationSeedItem("entity.routingitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.routingitem.routing
            new TranslationSeedItem("entity.routingitem.routing", "en-US", "工艺路线主表_us", "工艺路线主表（主表）"),
            // entity.routingitem.routing
            new TranslationSeedItem("entity.routingitem.routing", "ja-JP", "工艺路线主表_jp", "工艺路线主表（主表）"),
            // entity.routingitem.routing
            new TranslationSeedItem("entity.routingitem.routing", "zh-CN", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingitem.routing
            new TranslationSeedItem("entity.routingitem.routing", "zh-HK", "工艺路线主表_hk", "工艺路线主表（主表）"),

            // entity.routingitem.arguments
            new TranslationSeedItem("entity.routingitem.arguments", "en-US", "工序参数定义_us", "工序参数定义"),
            // entity.routingitem.arguments
            new TranslationSeedItem("entity.routingitem.arguments", "ja-JP", "工序参数定义_jp", "工序参数定义"),
            // entity.routingitem.arguments
            new TranslationSeedItem("entity.routingitem.arguments", "zh-CN", "工序参数定义", "工序参数定义"),
            // entity.routingitem.arguments
            new TranslationSeedItem("entity.routingitem.arguments", "zh-HK", "工序参数定义_hk", "工序参数定义"),
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
