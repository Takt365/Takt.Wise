// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssue 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssue 实体国际化翻译种子（键前缀 entity.qualityissue.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssue 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityissue 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueTranslations())
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

        TaktLogger.Information("TaktQualityIssue 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssue 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityissue._self / entity.qualityissue.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityissue._self
            new TranslationSeedItem("entity.qualityissue._self", "en-US", "Quality Issue Information_us", "实体名称"),
            // entity.qualityissue._self
            new TranslationSeedItem("entity.qualityissue._self", "ja-JP", "品质问题应对主表信息_jp", "实体名称"),
            // entity.qualityissue._self
            new TranslationSeedItem("entity.qualityissue._self", "zh-CN", "品质问题应对主表信息", "实体名称"),
            // entity.qualityissue._self
            new TranslationSeedItem("entity.qualityissue._self", "zh-HK", "品质问题应对主表信息_hk", "实体名称"),

            // entity.qualityissue.code
            new TranslationSeedItem("entity.qualityissue.code", "en-US", "品质问题编码_us", "品质问题编码（租户+公司+工厂内唯一，如：QF-2026-0001；前端表单选择编码规则后自动通过 TaktNumbering 品质问题编码规则生成并展示，非手输；单据类型菜单：品质问题）"),
            // entity.qualityissue.code
            new TranslationSeedItem("entity.qualityissue.code", "ja-JP", "品质问题编码_jp", "品质问题编码（租户+公司+工厂内唯一，如：QF-2026-0001；前端表单选择编码规则后自动通过 TaktNumbering 品质问题编码规则生成并展示，非手输；单据类型菜单：品质问题）"),
            // entity.qualityissue.code
            new TranslationSeedItem("entity.qualityissue.code", "zh-CN", "品质问题编码", "品质问题编码（租户+公司+工厂内唯一，如：QF-2026-0001；前端表单选择编码规则后自动通过 TaktNumbering 品质问题编码规则生成并展示，非手输；单据类型菜单：品质问题）"),
            // entity.qualityissue.code
            new TranslationSeedItem("entity.qualityissue.code", "zh-HK", "品质问题编码_hk", "品质问题编码（租户+公司+工厂内唯一，如：QF-2026-0001；前端表单选择编码规则后自动通过 TaktNumbering 品质问题编码规则生成并展示，非手输；单据类型菜单：品质问题）"),

            // entity.qualityissue.issuedate
            new TranslationSeedItem("entity.qualityissue.issuedate", "en-US", "问题日期_us", "问题日期"),
            // entity.qualityissue.issuedate
            new TranslationSeedItem("entity.qualityissue.issuedate", "ja-JP", "问题日期_jp", "问题日期"),
            // entity.qualityissue.issuedate
            new TranslationSeedItem("entity.qualityissue.issuedate", "zh-CN", "问题日期", "问题日期"),
            // entity.qualityissue.issuedate
            new TranslationSeedItem("entity.qualityissue.issuedate", "zh-HK", "问题日期_hk", "问题日期"),

            // entity.qualityissue.model
            new TranslationSeedItem("entity.qualityissue.model", "en-US", "机种_us", "机种/产品型号"),
            // entity.qualityissue.model
            new TranslationSeedItem("entity.qualityissue.model", "ja-JP", "机种_jp", "机种/产品型号"),
            // entity.qualityissue.model
            new TranslationSeedItem("entity.qualityissue.model", "zh-CN", "机种", "机种/产品型号"),
            // entity.qualityissue.model
            new TranslationSeedItem("entity.qualityissue.model", "zh-HK", "机种_hk", "机种/产品型号"),

            // entity.qualityissue.lot
            new TranslationSeedItem("entity.qualityissue.lot", "en-US", "批次号_us", "批次号/Lot No"),
            // entity.qualityissue.lot
            new TranslationSeedItem("entity.qualityissue.lot", "ja-JP", "批次号_jp", "批次号/Lot No"),
            // entity.qualityissue.lot
            new TranslationSeedItem("entity.qualityissue.lot", "zh-CN", "批次号", "批次号/Lot No"),
            // entity.qualityissue.lot
            new TranslationSeedItem("entity.qualityissue.lot", "zh-HK", "批次号_hk", "批次号/Lot No"),

            // entity.qualityissue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityissue.qualityproblemsresponse", "en-US", "品质问题应对_us", "品质问题应对摘要(汇总说明)"),
            // entity.qualityissue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityissue.qualityproblemsresponse", "ja-JP", "品质问题应对_jp", "品质问题应对摘要(汇总说明)"),
            // entity.qualityissue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityissue.qualityproblemsresponse", "zh-CN", "品质问题应对", "品质问题应对摘要(汇总说明)"),
            // entity.qualityissue.qualityproblemsresponse
            new TranslationSeedItem("entity.qualityissue.qualityproblemsresponse", "zh-HK", "品质问题应对_hk", "品质问题应对摘要(汇总说明)"),

            // entity.qualityissue.reworkduetodefects
            new TranslationSeedItem("entity.qualityissue.reworkduetodefects", "en-US", "不良改修应对_us", "不良改修应对摘要(汇总说明)"),
            // entity.qualityissue.reworkduetodefects
            new TranslationSeedItem("entity.qualityissue.reworkduetodefects", "ja-JP", "不良改修应对_jp", "不良改修应对摘要(汇总说明)"),
            // entity.qualityissue.reworkduetodefects
            new TranslationSeedItem("entity.qualityissue.reworkduetodefects", "zh-CN", "不良改修应对", "不良改修应对摘要(汇总说明)"),
            // entity.qualityissue.reworkduetodefects
            new TranslationSeedItem("entity.qualityissue.reworkduetodefects", "zh-HK", "不良改修应对_hk", "不良改修应对摘要(汇总说明)"),

            // entity.qualityissue.needrework
            new TranslationSeedItem("entity.qualityissue.needrework", "en-US", "是否需要不良改修应对_us", "是否需要不良改修应对(Y/N)"),
            // entity.qualityissue.needrework
            new TranslationSeedItem("entity.qualityissue.needrework", "ja-JP", "是否需要不良改修应对_jp", "是否需要不良改修应对(Y/N)"),
            // entity.qualityissue.needrework
            new TranslationSeedItem("entity.qualityissue.needrework", "zh-CN", "是否需要不良改修应对", "是否需要不良改修应对(Y/N)"),
            // entity.qualityissue.needrework
            new TranslationSeedItem("entity.qualityissue.needrework", "zh-HK", "是否需要不良改修应对_hk", "是否需要不良改修应对(Y/N)"),

            // entity.qualityissue.totaltimeminutes
            new TranslationSeedItem("entity.qualityissue.totaltimeminutes", "en-US", "总时间_us", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityissue.totaltimeminutes
            new TranslationSeedItem("entity.qualityissue.totaltimeminutes", "ja-JP", "总时间_jp", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityissue.totaltimeminutes
            new TranslationSeedItem("entity.qualityissue.totaltimeminutes", "zh-CN", "总时间", "总时间(分钟,自动计算 = 各子表时间合计)"),
            // entity.qualityissue.totaltimeminutes
            new TranslationSeedItem("entity.qualityissue.totaltimeminutes", "zh-HK", "总时间_hk", "总时间(分钟,自动计算 = 各子表时间合计)"),

            // entity.qualityissue.totalcost
            new TranslationSeedItem("entity.qualityissue.totalcost", "en-US", "总费用_us", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityissue.totalcost
            new TranslationSeedItem("entity.qualityissue.totalcost", "ja-JP", "总费用_jp", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityissue.totalcost
            new TranslationSeedItem("entity.qualityissue.totalcost", "zh-CN", "总费用", "总费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityissue.totalcost
            new TranslationSeedItem("entity.qualityissue.totalcost", "zh-HK", "总费用_hk", "总费用(元,自动计算 = 各子表费用合计)"),

            // entity.qualityissue.currencycode
            new TranslationSeedItem("entity.qualityissue.currencycode", "en-US", "成本币种_us", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityissue.currencycode
            new TranslationSeedItem("entity.qualityissue.currencycode", "ja-JP", "成本币种_jp", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityissue.currencycode
            new TranslationSeedItem("entity.qualityissue.currencycode", "zh-CN", "成本币种", "成本币种（CNY/USD/JPY等）"),
            // entity.qualityissue.currencycode
            new TranslationSeedItem("entity.qualityissue.currencycode", "zh-HK", "成本币种_hk", "成本币种（CNY/USD/JPY等）"),

            // entity.qualityissue.meetingitems
            new TranslationSeedItem("entity.qualityissue.meetingitems", "en-US", "会议/调查/试验费用明细列表_us", "会议/调查/试验费用明细列表"),
            // entity.qualityissue.meetingitems
            new TranslationSeedItem("entity.qualityissue.meetingitems", "ja-JP", "会议/调查/试验费用明细列表_jp", "会议/调查/试验费用明细列表"),
            // entity.qualityissue.meetingitems
            new TranslationSeedItem("entity.qualityissue.meetingitems", "zh-CN", "会议/调查/试验费用明细列表", "会议/调查/试验费用明细列表"),
            // entity.qualityissue.meetingitems
            new TranslationSeedItem("entity.qualityissue.meetingitems", "zh-HK", "会议/调查/试验费用明细列表_hk", "会议/调查/试验费用明细列表"),

            // entity.qualityissue.assyreworkitems
            new TranslationSeedItem("entity.qualityissue.assyreworkitems", "en-US", "组装不良改修应对明细列表_us", "组装不良改修应对明细列表"),
            // entity.qualityissue.assyreworkitems
            new TranslationSeedItem("entity.qualityissue.assyreworkitems", "ja-JP", "组装不良改修应对明细列表_jp", "组装不良改修应对明细列表"),
            // entity.qualityissue.assyreworkitems
            new TranslationSeedItem("entity.qualityissue.assyreworkitems", "zh-CN", "组装不良改修应对明细列表", "组装不良改修应对明细列表"),
            // entity.qualityissue.assyreworkitems
            new TranslationSeedItem("entity.qualityissue.assyreworkitems", "zh-HK", "组装不良改修应对明细列表_hk", "组装不良改修应对明细列表"),

            // entity.qualityissue.pcbareworkitems
            new TranslationSeedItem("entity.qualityissue.pcbareworkitems", "en-US", "PCBA不良改修应对明细列表_us", "PCBA不良改修应对明细列表"),
            // entity.qualityissue.pcbareworkitems
            new TranslationSeedItem("entity.qualityissue.pcbareworkitems", "ja-JP", "PCBA不良改修应对明细列表_jp", "PCBA不良改修应对明细列表"),
            // entity.qualityissue.pcbareworkitems
            new TranslationSeedItem("entity.qualityissue.pcbareworkitems", "zh-CN", "PCBA不良改修应对明细列表", "PCBA不良改修应对明细列表"),
            // entity.qualityissue.pcbareworkitems
            new TranslationSeedItem("entity.qualityissue.pcbareworkitems", "zh-HK", "PCBA不良改修应对明细列表_hk", "PCBA不良改修应对明细列表"),
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
