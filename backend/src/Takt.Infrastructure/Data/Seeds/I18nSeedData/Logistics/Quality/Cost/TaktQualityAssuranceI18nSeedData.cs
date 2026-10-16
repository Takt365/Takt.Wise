// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityAssurance 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityAssurance 实体国际化翻译种子（键前缀 entity.qualityassurance.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityAssuranceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityAssurance 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityassurance 实体翻译...", tenantCode);

        foreach (var item in GetQualityAssuranceTranslations())
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

        TaktLogger.Information("TaktQualityAssurance 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityAssurance 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityassurance._self / entity.qualityassurance.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityAssuranceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityassurance._self
            new TranslationSeedItem("entity.qualityassurance._self", "en-US", "Quality Assurance Information_us", "实体名称"),
            // entity.qualityassurance._self
            new TranslationSeedItem("entity.qualityassurance._self", "ja-JP", "品质业务主表信息_jp", "实体名称"),
            // entity.qualityassurance._self
            new TranslationSeedItem("entity.qualityassurance._self", "zh-CN", "品质业务主表信息", "实体名称"),
            // entity.qualityassurance._self
            new TranslationSeedItem("entity.qualityassurance._self", "zh-HK", "品质业务主表信息_hk", "实体名称"),

            // entity.qualityassurance.code
            new TranslationSeedItem("entity.qualityassurance.code", "en-US", "品质业务编码_us", "品质业务编码(租户+公司+工厂内唯一，如:QO-2026-0001)（前端表单选择编码规则后自动通过 TaktNumbering 品质业务编码规则生成并展示，非手输；单据类型菜单：品质保证）"),
            // entity.qualityassurance.code
            new TranslationSeedItem("entity.qualityassurance.code", "ja-JP", "品质业务编码_jp", "品质业务编码(租户+公司+工厂内唯一，如:QO-2026-0001)（前端表单选择编码规则后自动通过 TaktNumbering 品质业务编码规则生成并展示，非手输；单据类型菜单：品质保证）"),
            // entity.qualityassurance.code
            new TranslationSeedItem("entity.qualityassurance.code", "zh-CN", "品质业务编码", "品质业务编码(租户+公司+工厂内唯一，如:QO-2026-0001)（前端表单选择编码规则后自动通过 TaktNumbering 品质业务编码规则生成并展示，非手输；单据类型菜单：品质保证）"),
            // entity.qualityassurance.code
            new TranslationSeedItem("entity.qualityassurance.code", "zh-HK", "品质业务编码_hk", "品质业务编码(租户+公司+工厂内唯一，如:QO-2026-0001)（前端表单选择编码规则后自动通过 TaktNumbering 品质业务编码规则生成并展示，非手输；单据类型菜单：品质保证）"),

            // entity.qualityassurance.assurancemonth
            new TranslationSeedItem("entity.qualityassurance.assurancemonth", "en-US", "业务年月_us", "业务年月(格式:2026-05)"),
            // entity.qualityassurance.assurancemonth
            new TranslationSeedItem("entity.qualityassurance.assurancemonth", "ja-JP", "业务年月_jp", "业务年月(格式:2026-05)"),
            // entity.qualityassurance.assurancemonth
            new TranslationSeedItem("entity.qualityassurance.assurancemonth", "zh-CN", "业务年月", "业务年月(格式:2026-05)"),
            // entity.qualityassurance.assurancemonth
            new TranslationSeedItem("entity.qualityassurance.assurancemonth", "zh-HK", "业务年月_hk", "业务年月(格式:2026-05)"),

            // entity.qualityassurance.customername1
            new TranslationSeedItem("entity.qualityassurance.customername1", "en-US", "客户名称1_us", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.qualityassurance.customername1
            new TranslationSeedItem("entity.qualityassurance.customername1", "ja-JP", "客户名称1_jp", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.qualityassurance.customername1
            new TranslationSeedItem("entity.qualityassurance.customername1", "zh-CN", "客户名称1", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.qualityassurance.customername1
            new TranslationSeedItem("entity.qualityassurance.customername1", "zh-HK", "客户名称1_hk", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),

            // entity.qualityassurance.debitnotecode
            new TranslationSeedItem("entity.qualityassurance.debitnotecode", "en-US", "Debit Note No_us", "Debit Note No"),
            // entity.qualityassurance.debitnotecode
            new TranslationSeedItem("entity.qualityassurance.debitnotecode", "ja-JP", "Debit Note No_jp", "Debit Note No"),
            // entity.qualityassurance.debitnotecode
            new TranslationSeedItem("entity.qualityassurance.debitnotecode", "zh-CN", "Debit Note No", "Debit Note No"),
            // entity.qualityassurance.debitnotecode
            new TranslationSeedItem("entity.qualityassurance.debitnotecode", "zh-HK", "Debit Note No_hk", "Debit Note No"),

            // entity.qualityassurance.recordgroupcode
            new TranslationSeedItem("entity.qualityassurance.recordgroupcode", "en-US", "记录组编码_us", "记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）"),
            // entity.qualityassurance.recordgroupcode
            new TranslationSeedItem("entity.qualityassurance.recordgroupcode", "ja-JP", "记录组编码_jp", "记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）"),
            // entity.qualityassurance.recordgroupcode
            new TranslationSeedItem("entity.qualityassurance.recordgroupcode", "zh-CN", "记录组编码", "记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）"),
            // entity.qualityassurance.recordgroupcode
            new TranslationSeedItem("entity.qualityassurance.recordgroupcode", "zh-HK", "记录组编码_hk", "记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）"),

            // entity.qualityassurance.recordgroupname
            new TranslationSeedItem("entity.qualityassurance.recordgroupname", "en-US", "记录组名称_us", "记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityassurance.recordgroupname
            new TranslationSeedItem("entity.qualityassurance.recordgroupname", "ja-JP", "记录组名称_jp", "记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityassurance.recordgroupname
            new TranslationSeedItem("entity.qualityassurance.recordgroupname", "zh-CN", "记录组名称", "记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityassurance.recordgroupname
            new TranslationSeedItem("entity.qualityassurance.recordgroupname", "zh-HK", "记录组名称_hk", "记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）"),

            // entity.qualityassurance.totalqualitycost
            new TranslationSeedItem("entity.qualityassurance.totalqualitycost", "en-US", "质量总成本_us", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityassurance.totalqualitycost
            new TranslationSeedItem("entity.qualityassurance.totalqualitycost", "ja-JP", "质量总成本_jp", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityassurance.totalqualitycost
            new TranslationSeedItem("entity.qualityassurance.totalqualitycost", "zh-CN", "质量总成本", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityassurance.totalqualitycost
            new TranslationSeedItem("entity.qualityassurance.totalqualitycost", "zh-HK", "质量总成本_hk", "质量总成本(元,自动计算 = 各子表费用合计)"),

            // entity.qualityassurance.currencycode
            new TranslationSeedItem("entity.qualityassurance.currencycode", "en-US", "成本币种_us", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityassurance.currencycode
            new TranslationSeedItem("entity.qualityassurance.currencycode", "ja-JP", "成本币种_jp", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityassurance.currencycode
            new TranslationSeedItem("entity.qualityassurance.currencycode", "zh-CN", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityassurance.currencycode
            new TranslationSeedItem("entity.qualityassurance.currencycode", "zh-HK", "成本币种_hk", "成本币种(CNY/USD/JPY等)"),

            // entity.qualityassurance.incomingitems
            new TranslationSeedItem("entity.qualityassurance.incomingitems", "en-US", "来料检验费用明细列表_us", "来料检验费用明细列表"),
            // entity.qualityassurance.incomingitems
            new TranslationSeedItem("entity.qualityassurance.incomingitems", "ja-JP", "来料检验费用明细列表_jp", "来料检验费用明细列表"),
            // entity.qualityassurance.incomingitems
            new TranslationSeedItem("entity.qualityassurance.incomingitems", "zh-CN", "来料检验费用明细列表", "来料检验费用明细列表"),
            // entity.qualityassurance.incomingitems
            new TranslationSeedItem("entity.qualityassurance.incomingitems", "zh-HK", "来料检验费用明细列表_hk", "来料检验费用明细列表"),

            // entity.qualityassurance.firstarticleitems
            new TranslationSeedItem("entity.qualityassurance.firstarticleitems", "en-US", "初期/定期检定费用明细列表_us", "初期/定期检定费用明细列表"),
            // entity.qualityassurance.firstarticleitems
            new TranslationSeedItem("entity.qualityassurance.firstarticleitems", "ja-JP", "初期/定期检定费用明细列表_jp", "初期/定期检定费用明细列表"),
            // entity.qualityassurance.firstarticleitems
            new TranslationSeedItem("entity.qualityassurance.firstarticleitems", "zh-CN", "初期/定期检定费用明细列表", "初期/定期检定费用明细列表"),
            // entity.qualityassurance.firstarticleitems
            new TranslationSeedItem("entity.qualityassurance.firstarticleitems", "zh-HK", "初期/定期检定费用明细列表_hk", "初期/定期检定费用明细列表"),

            // entity.qualityassurance.calibrationitems
            new TranslationSeedItem("entity.qualityassurance.calibrationitems", "en-US", "设备校正费用明细列表_us", "设备校正费用明细列表"),
            // entity.qualityassurance.calibrationitems
            new TranslationSeedItem("entity.qualityassurance.calibrationitems", "ja-JP", "设备校正费用明细列表_jp", "设备校正费用明细列表"),
            // entity.qualityassurance.calibrationitems
            new TranslationSeedItem("entity.qualityassurance.calibrationitems", "zh-CN", "设备校正费用明细列表", "设备校正费用明细列表"),
            // entity.qualityassurance.calibrationitems
            new TranslationSeedItem("entity.qualityassurance.calibrationitems", "zh-HK", "设备校正费用明细列表_hk", "设备校正费用明细列表"),

            // entity.qualityassurance.otheritems
            new TranslationSeedItem("entity.qualityassurance.otheritems", "en-US", "其他通常业务费用明细列表_us", "其他通常业务费用明细列表"),
            // entity.qualityassurance.otheritems
            new TranslationSeedItem("entity.qualityassurance.otheritems", "ja-JP", "其他通常业务费用明细列表_jp", "其他通常业务费用明细列表"),
            // entity.qualityassurance.otheritems
            new TranslationSeedItem("entity.qualityassurance.otheritems", "zh-CN", "其他通常业务费用明细列表", "其他通常业务费用明细列表"),
            // entity.qualityassurance.otheritems
            new TranslationSeedItem("entity.qualityassurance.otheritems", "zh-HK", "其他通常业务费用明细列表_hk", "其他通常业务费用明细列表"),

            // entity.qualityassurance.outgoingitems
            new TranslationSeedItem("entity.qualityassurance.outgoingitems", "en-US", "出货检验费用明细列表_us", "出货检验费用明细列表"),
            // entity.qualityassurance.outgoingitems
            new TranslationSeedItem("entity.qualityassurance.outgoingitems", "ja-JP", "出货检验费用明细列表_jp", "出货检验费用明细列表"),
            // entity.qualityassurance.outgoingitems
            new TranslationSeedItem("entity.qualityassurance.outgoingitems", "zh-CN", "出货检验费用明细列表", "出货检验费用明细列表"),
            // entity.qualityassurance.outgoingitems
            new TranslationSeedItem("entity.qualityassurance.outgoingitems", "zh-HK", "出货检验费用明细列表_hk", "出货检验费用明细列表"),

            // entity.qualityassurance.reliabilityitems
            new TranslationSeedItem("entity.qualityassurance.reliabilityitems", "en-US", "信赖性评价/ORT费用明细列表_us", "信赖性评价/ORT费用明细列表"),
            // entity.qualityassurance.reliabilityitems
            new TranslationSeedItem("entity.qualityassurance.reliabilityitems", "ja-JP", "信赖性评价/ORT费用明细列表_jp", "信赖性评价/ORT费用明细列表"),
            // entity.qualityassurance.reliabilityitems
            new TranslationSeedItem("entity.qualityassurance.reliabilityitems", "zh-CN", "信赖性评价/ORT费用明细列表", "信赖性评价/ORT费用明细列表"),
            // entity.qualityassurance.reliabilityitems
            new TranslationSeedItem("entity.qualityassurance.reliabilityitems", "zh-HK", "信赖性评价/ORT费用明细列表_hk", "信赖性评价/ORT费用明细列表"),

            // entity.qualityassurance.customerresponseitems
            new TranslationSeedItem("entity.qualityassurance.customerresponseitems", "en-US", "顾客品质要求对应费用明细列表_us", "顾客品质要求对应费用明细列表"),
            // entity.qualityassurance.customerresponseitems
            new TranslationSeedItem("entity.qualityassurance.customerresponseitems", "ja-JP", "顾客品质要求对应费用明细列表_jp", "顾客品质要求对应费用明细列表"),
            // entity.qualityassurance.customerresponseitems
            new TranslationSeedItem("entity.qualityassurance.customerresponseitems", "zh-CN", "顾客品质要求对应费用明细列表", "顾客品质要求对应费用明细列表"),
            // entity.qualityassurance.customerresponseitems
            new TranslationSeedItem("entity.qualityassurance.customerresponseitems", "zh-HK", "顾客品质要求对应费用明细列表_hk", "顾客品质要求对应费用明细列表"),
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
        translation.ResourceGroup = "Cost";
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
