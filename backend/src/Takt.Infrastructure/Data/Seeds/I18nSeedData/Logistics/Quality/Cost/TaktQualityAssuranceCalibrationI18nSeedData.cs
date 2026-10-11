// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceCalibrationI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityAssuranceCalibration 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityAssuranceCalibration 实体国际化翻译种子（键前缀 entity.qualityassurancecalibration.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityAssuranceCalibrationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityAssuranceCalibration 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityassurancecalibration 实体翻译...", tenantCode);

        foreach (var item in GetQualityAssuranceCalibrationTranslations())
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

        TaktLogger.Information("TaktQualityAssuranceCalibration 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityAssuranceCalibration 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityassurancecalibration._self / entity.qualityassurancecalibration.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityAssuranceCalibrationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityassurancecalibration._self
            new TranslationSeedItem("entity.qualityassurancecalibration._self", "en-US", "Quality Assurance Calibration Information_us", "实体名称"),
            // entity.qualityassurancecalibration._self
            new TranslationSeedItem("entity.qualityassurancecalibration._self", "ja-JP", "品质业务明细 - 测定器校正费用信息_jp", "实体名称"),
            // entity.qualityassurancecalibration._self
            new TranslationSeedItem("entity.qualityassurancecalibration._self", "zh-CN", "品质业务明细 - 测定器校正费用信息", "实体名称"),
            // entity.qualityassurancecalibration._self
            new TranslationSeedItem("entity.qualityassurancecalibration._self", "zh-HK", "品质业务明细 - 测定器校正费用信息_hk", "实体名称"),

            // entity.qualityassurancecalibration.qualityassuranceid
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassuranceid", "en-US", "品质业务主表ID_us", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassurancecalibration.qualityassuranceid
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassuranceid", "ja-JP", "品质业务主表ID_jp", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassurancecalibration.qualityassuranceid
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassuranceid", "zh-CN", "品质业务主表ID", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassurancecalibration.qualityassuranceid
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassuranceid", "zh-HK", "品质业务主表ID_hk", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),

            // entity.qualityassurancecalibration.qualityassurancecode
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassurancecode", "en-US", "品质业务编码_us", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassurancecalibration.qualityassurancecode
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassurancecode", "ja-JP", "品质业务编码_jp", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassurancecalibration.qualityassurancecode
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassurancecode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassurancecalibration.qualityassurancecode
            new TranslationSeedItem("entity.qualityassurancecalibration.qualityassurancecode", "zh-HK", "品质业务编码_hk", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityassurancecalibration.linenumber
            new TranslationSeedItem("entity.qualityassurancecalibration.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassurancecalibration.linenumber
            new TranslationSeedItem("entity.qualityassurancecalibration.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassurancecalibration.linenumber
            new TranslationSeedItem("entity.qualityassurancecalibration.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassurancecalibration.linenumber
            new TranslationSeedItem("entity.qualityassurancecalibration.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityassurancecalibration.calibrationcost
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationcost", "en-US", "测定器校正业务费用_us", "测定器校正业务费用(元)"),
            // entity.qualityassurancecalibration.calibrationcost
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationcost", "ja-JP", "测定器校正业务费用_jp", "测定器校正业务费用(元)"),
            // entity.qualityassurancecalibration.calibrationcost
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationcost", "zh-CN", "测定器校正业务费用", "测定器校正业务费用(元)"),
            // entity.qualityassurancecalibration.calibrationcost
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationcost", "zh-HK", "测定器校正业务费用_hk", "测定器校正业务费用(元)"),

            // entity.qualityassurancecalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityassurancecalibration.worktimeminutes", "en-US", "校正作业时间_us", "校正作业时间(分钟)"),
            // entity.qualityassurancecalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityassurancecalibration.worktimeminutes", "ja-JP", "校正作业时间_jp", "校正作业时间(分钟)"),
            // entity.qualityassurancecalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityassurancecalibration.worktimeminutes", "zh-CN", "校正作业时间", "校正作业时间(分钟)"),
            // entity.qualityassurancecalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityassurancecalibration.worktimeminutes", "zh-HK", "校正作业时间_hk", "校正作业时间(分钟)"),

            // entity.qualityassurancecalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityassurancecalibration.externalagentservicefee", "en-US", "外部委托费运搬费_us", "外部委托费、运搬费(元)"),
            // entity.qualityassurancecalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityassurancecalibration.externalagentservicefee", "ja-JP", "外部委托费运搬费_jp", "外部委托费、运搬费(元)"),
            // entity.qualityassurancecalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityassurancecalibration.externalagentservicefee", "zh-CN", "外部委托费运搬费", "外部委托费、运搬费(元)"),
            // entity.qualityassurancecalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityassurancecalibration.externalagentservicefee", "zh-HK", "外部委托费运搬费_hk", "外部委托费、运搬费(元)"),

            // entity.qualityassurancecalibration.otherexpenses
            new TranslationSeedItem("entity.qualityassurancecalibration.otherexpenses", "en-US", "校正其他费用_us", "校正其他费用(元)"),
            // entity.qualityassurancecalibration.otherexpenses
            new TranslationSeedItem("entity.qualityassurancecalibration.otherexpenses", "ja-JP", "校正其他费用_jp", "校正其他费用(元)"),
            // entity.qualityassurancecalibration.otherexpenses
            new TranslationSeedItem("entity.qualityassurancecalibration.otherexpenses", "zh-CN", "校正其他费用", "校正其他费用(元)"),
            // entity.qualityassurancecalibration.otherexpenses
            new TranslationSeedItem("entity.qualityassurancecalibration.otherexpenses", "zh-HK", "校正其他费用_hk", "校正其他费用(元)"),

            // entity.qualityassurancecalibration.calibrationnote
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationnote", "en-US", "校正备注_us", "校正备注"),
            // entity.qualityassurancecalibration.calibrationnote
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationnote", "ja-JP", "校正备注_jp", "校正备注"),
            // entity.qualityassurancecalibration.calibrationnote
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationnote", "zh-CN", "校正备注", "校正备注"),
            // entity.qualityassurancecalibration.calibrationnote
            new TranslationSeedItem("entity.qualityassurancecalibration.calibrationnote", "zh-HK", "校正备注_hk", "校正备注"),

            // entity.qualityassurancecalibration.isobsolete
            new TranslationSeedItem("entity.qualityassurancecalibration.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassurancecalibration.isobsolete
            new TranslationSeedItem("entity.qualityassurancecalibration.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassurancecalibration.isobsolete
            new TranslationSeedItem("entity.qualityassurancecalibration.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassurancecalibration.isobsolete
            new TranslationSeedItem("entity.qualityassurancecalibration.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityassurancecalibration.operation
            new TranslationSeedItem("entity.qualityassurancecalibration.operation", "en-US", "品质业务主表_us", "品质业务主表(导航属性)"),
            // entity.qualityassurancecalibration.operation
            new TranslationSeedItem("entity.qualityassurancecalibration.operation", "ja-JP", "品质业务主表_jp", "品质业务主表(导航属性)"),
            // entity.qualityassurancecalibration.operation
            new TranslationSeedItem("entity.qualityassurancecalibration.operation", "zh-CN", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityassurancecalibration.operation
            new TranslationSeedItem("entity.qualityassurancecalibration.operation", "zh-HK", "品质业务主表_hk", "品质业务主表(导航属性)"),
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
