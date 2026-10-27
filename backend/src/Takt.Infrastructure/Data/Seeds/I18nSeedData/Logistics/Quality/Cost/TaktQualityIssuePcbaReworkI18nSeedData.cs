// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuePcbaReworkI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssuePcbaRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssuePcbaRework 实体国际化翻译种子（键前缀 entity.qualityissuepcbarework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssuePcbaReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssuePcbaRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityissuepcbarework 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssuePcbaReworkTranslations())
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

        TaktLogger.Information("TaktQualityIssuePcbaRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssuePcbaRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityissuepcbarework._self / entity.qualityissuepcbarework.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssuePcbaReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityissuepcbarework._self
            new TranslationSeedItem("entity.qualityissuepcbarework._self", "en-US", "Quality Issue Pcba Rework Information_us", "实体名称"),
            // entity.qualityissuepcbarework._self
            new TranslationSeedItem("entity.qualityissuepcbarework._self", "ja-JP", "品质问题应对明细 - PCBA不良改修应对信息_jp", "实体名称"),
            // entity.qualityissuepcbarework._self
            new TranslationSeedItem("entity.qualityissuepcbarework._self", "zh-CN", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),
            // entity.qualityissuepcbarework._self
            new TranslationSeedItem("entity.qualityissuepcbarework._self", "zh-HK", "品质问题应对明细 - PCBA不良改修应对信息_hk", "实体名称"),

            // entity.qualityissuepcbarework.qualityissueid
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissueid", "en-US", "品质问题主表ID_us", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuepcbarework.qualityissueid
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissueid", "ja-JP", "品质问题主表ID_jp", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuepcbarework.qualityissueid
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuepcbarework.qualityissueid
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissueid", "zh-HK", "品质问题主表ID_hk", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),

            // entity.qualityissuepcbarework.qualityissuecode
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissuecode", "en-US", "品质问题编码_us", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuepcbarework.qualityissuecode
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissuecode", "ja-JP", "品质问题编码_jp", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuepcbarework.qualityissuecode
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuepcbarework.qualityissuecode
            new TranslationSeedItem("entity.qualityissuepcbarework.qualityissuecode", "zh-HK", "品质问题编码_hk", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.qualityissuepcbarework.linenumber
            new TranslationSeedItem("entity.qualityissuepcbarework.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuepcbarework.linenumber
            new TranslationSeedItem("entity.qualityissuepcbarework.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuepcbarework.linenumber
            new TranslationSeedItem("entity.qualityissuepcbarework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuepcbarework.linenumber
            new TranslationSeedItem("entity.qualityissuepcbarework.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityissuepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadefectparts", "en-US", "PCBA不良内容_us", "PCBA不良内容(Parts/Components)"),
            // entity.qualityissuepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadefectparts", "ja-JP", "PCBA不良内容_jp", "PCBA不良内容(Parts/Components)"),
            // entity.qualityissuepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadefectparts", "zh-CN", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityissuepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadefectparts", "zh-HK", "PCBA不良内容_hk", "PCBA不良内容(Parts/Components)"),

            // entity.qualityissuepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworkcost", "en-US", "PCBA选别改修费用_us", "PCBA选别・改修费用（元）"),
            // entity.qualityissuepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworkcost", "ja-JP", "PCBA选别改修费用_jp", "PCBA选别・改修费用（元）"),
            // entity.qualityissuepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworkcost", "zh-CN", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityissuepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworkcost", "zh-HK", "PCBA选别改修费用_hk", "PCBA选别・改修费用（元）"),

            // entity.qualityissuepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworktimeminutes", "en-US", "PCBA选别改修时间_us", "PCBA选别・改修时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworktimeminutes", "ja-JP", "PCBA选别改修时间_jp", "PCBA选别・改修时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworktimeminutes", "zh-CN", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworktimeminutes", "zh-HK", "PCBA选别改修时间_hk", "PCBA选别・改修时间（分钟）"),

            // entity.qualityissuepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareinspectiontimeminutes", "en-US", "PCBA再检查时间_us", "PCBA再检查时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareinspectiontimeminutes", "ja-JP", "PCBA再检查时间_jp", "PCBA再检查时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareinspectiontimeminutes", "zh-CN", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityissuepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareinspectiontimeminutes", "zh-HK", "PCBA再检查时间_hk", "PCBA再检查时间（分钟）"),

            // entity.qualityissuepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbatravelcost", "en-US", "PCBA交通费旅费_us", "PCBA交通费、旅费（元）"),
            // entity.qualityissuepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbatravelcost", "ja-JP", "PCBA交通费旅费_jp", "PCBA交通费、旅费（元）"),
            // entity.qualityissuepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbatravelcost", "zh-CN", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityissuepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbatravelcost", "zh-HK", "PCBA交通费旅费_hk", "PCBA交通费、旅费（元）"),

            // entity.qualityissuepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbawarehousecost", "en-US", "PCBA仓库管理费_us", "PCBA仓库管理费（元）"),
            // entity.qualityissuepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbawarehousecost", "ja-JP", "PCBA仓库管理费_jp", "PCBA仓库管理费（元）"),
            // entity.qualityissuepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbawarehousecost", "zh-CN", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityissuepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbawarehousecost", "zh-HK", "PCBA仓库管理费_hk", "PCBA仓库管理费（元）"),

            // entity.qualityissuepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses", "en-US", "PCBA选别改修其他费用_us", "PCBA选别・改修其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses", "ja-JP", "PCBA选别改修其他费用_jp", "PCBA选别・改修其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses", "zh-CN", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses", "zh-HK", "PCBA选别改修其他费用_hk", "PCBA选别・改修其他费用（元）"),

            // entity.qualityissuepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworknote", "en-US", "PCBA选别改修备注_us", "PCBA选别・改修备注"),
            // entity.qualityissuepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworknote", "ja-JP", "PCBA选别改修备注_jp", "PCBA选别・改修备注"),
            // entity.qualityissuepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworknote", "zh-CN", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityissuepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbareworknote", "zh-HK", "PCBA选别改修备注_hk", "PCBA选别・改修备注"),

            // entity.qualityissuepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbascrapcost", "en-US", "PCBA向顾客费用请求_us", "PCBA向顾客的费用请求（元）"),
            // entity.qualityissuepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbascrapcost", "ja-JP", "PCBA向顾客费用请求_jp", "PCBA向顾客的费用请求（元）"),
            // entity.qualityissuepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbascrapcost", "zh-CN", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityissuepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbascrapcost", "zh-HK", "PCBA向顾客费用请求_hk", "PCBA向顾客的费用请求（元）"),

            // entity.qualityissuepcbarework.pcbacustomername1
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbacustomername1", "en-US", "PCBA顾客名1_us", "PCBA顾客名"),
            // entity.qualityissuepcbarework.pcbacustomername1
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbacustomername1", "ja-JP", "PCBA顾客名1_jp", "PCBA顾客名"),
            // entity.qualityissuepcbarework.pcbacustomername1
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbacustomername1", "zh-CN", "PCBA顾客名1", "PCBA顾客名"),
            // entity.qualityissuepcbarework.pcbacustomername1
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbacustomername1", "zh-HK", "PCBA顾客名1_hk", "PCBA顾客名"),

            // entity.qualityissuepcbarework.pcbadebitnotecode
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadebitnotecode", "en-US", "PCBA Debit Note No_us", "PCBA Debit Note No"),
            // entity.qualityissuepcbarework.pcbadebitnotecode
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadebitnotecode", "ja-JP", "PCBA Debit Note No_jp", "PCBA Debit Note No"),
            // entity.qualityissuepcbarework.pcbadebitnotecode
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadebitnotecode", "zh-CN", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityissuepcbarework.pcbadebitnotecode
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbadebitnotecode", "zh-HK", "PCBA Debit Note No_hk", "PCBA Debit Note No"),

            // entity.qualityissuepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses2", "en-US", "PCBA其他费用_us", "PCBA其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses2", "ja-JP", "PCBA其他费用_jp", "PCBA其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses2", "zh-CN", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityissuepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbaotherexpenses2", "zh-HK", "PCBA其他费用_hk", "PCBA其他费用（元）"),

            // entity.qualityissuepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbanote", "en-US", "PCBA备注_us", "PCBA备注"),
            // entity.qualityissuepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbanote", "ja-JP", "PCBA备注_jp", "PCBA备注"),
            // entity.qualityissuepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbanote", "zh-CN", "PCBA备注", "PCBA备注"),
            // entity.qualityissuepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityissuepcbarework.pcbanote", "zh-HK", "PCBA备注_hk", "PCBA备注"),

            // entity.qualityissuepcbarework.responsegroupcode
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupcode", "en-US", "对应组编码_us", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuepcbarework.responsegroupcode
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupcode", "ja-JP", "对应组编码_jp", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuepcbarework.responsegroupcode
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupcode", "zh-CN", "对应组编码", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuepcbarework.responsegroupcode
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupcode", "zh-HK", "对应组编码_hk", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),

            // entity.qualityissuepcbarework.responsegroupname
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupname", "en-US", "对应组名称_us", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuepcbarework.responsegroupname
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupname", "ja-JP", "对应组名称_jp", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuepcbarework.responsegroupname
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupname", "zh-CN", "对应组名称", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuepcbarework.responsegroupname
            new TranslationSeedItem("entity.qualityissuepcbarework.responsegroupname", "zh-HK", "对应组名称_hk", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),

            // entity.qualityissuepcbarework.isobsolete
            new TranslationSeedItem("entity.qualityissuepcbarework.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuepcbarework.isobsolete
            new TranslationSeedItem("entity.qualityissuepcbarework.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuepcbarework.isobsolete
            new TranslationSeedItem("entity.qualityissuepcbarework.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuepcbarework.isobsolete
            new TranslationSeedItem("entity.qualityissuepcbarework.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityissuepcbarework.issue
            new TranslationSeedItem("entity.qualityissuepcbarework.issue", "en-US", "质量问题主表_us", "质量问题主表（导航属性）"),
            // entity.qualityissuepcbarework.issue
            new TranslationSeedItem("entity.qualityissuepcbarework.issue", "ja-JP", "质量问题主表_jp", "质量问题主表（导航属性）"),
            // entity.qualityissuepcbarework.issue
            new TranslationSeedItem("entity.qualityissuepcbarework.issue", "zh-CN", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityissuepcbarework.issue
            new TranslationSeedItem("entity.qualityissuepcbarework.issue", "zh-HK", "质量问题主表_hk", "质量问题主表（导航属性）"),
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
