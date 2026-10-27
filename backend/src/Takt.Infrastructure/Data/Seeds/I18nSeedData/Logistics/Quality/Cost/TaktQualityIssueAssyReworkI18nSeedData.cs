// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueAssyReworkI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssueAssyRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssueAssyRework 实体国际化翻译种子（键前缀 entity.qualityissueassyrework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueAssyReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssueAssyRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityissueassyrework 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueAssyReworkTranslations())
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

        TaktLogger.Information("TaktQualityIssueAssyRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssueAssyRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityissueassyrework._self / entity.qualityissueassyrework.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueAssyReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityissueassyrework._self
            new TranslationSeedItem("entity.qualityissueassyrework._self", "en-US", "Quality Issue Assy Rework Information_us", "实体名称"),
            // entity.qualityissueassyrework._self
            new TranslationSeedItem("entity.qualityissueassyrework._self", "ja-JP", "品质问题应对明细 - 组装不良改修应对信息_jp", "实体名称"),
            // entity.qualityissueassyrework._self
            new TranslationSeedItem("entity.qualityissueassyrework._self", "zh-CN", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),
            // entity.qualityissueassyrework._self
            new TranslationSeedItem("entity.qualityissueassyrework._self", "zh-HK", "品质问题应对明细 - 组装不良改修应对信息_hk", "实体名称"),

            // entity.qualityissueassyrework.qualityissueid
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissueid", "en-US", "品质问题主表ID_us", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissueassyrework.qualityissueid
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissueid", "ja-JP", "品质问题主表ID_jp", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissueassyrework.qualityissueid
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissueassyrework.qualityissueid
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissueid", "zh-HK", "品质问题主表ID_hk", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),

            // entity.qualityissueassyrework.qualityissuecode
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissuecode", "en-US", "品质问题编码_us", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissueassyrework.qualityissuecode
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissuecode", "ja-JP", "品质问题编码_jp", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissueassyrework.qualityissuecode
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissueassyrework.qualityissuecode
            new TranslationSeedItem("entity.qualityissueassyrework.qualityissuecode", "zh-HK", "品质问题编码_hk", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.qualityissueassyrework.linenumber
            new TranslationSeedItem("entity.qualityissueassyrework.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissueassyrework.linenumber
            new TranslationSeedItem("entity.qualityissueassyrework.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissueassyrework.linenumber
            new TranslationSeedItem("entity.qualityissueassyrework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissueassyrework.linenumber
            new TranslationSeedItem("entity.qualityissueassyrework.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityissueassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityissueassyrework.assydefectparts", "en-US", "组装不良内容_us", "组装不良内容(Parts/Components)"),
            // entity.qualityissueassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityissueassyrework.assydefectparts", "ja-JP", "组装不良内容_jp", "组装不良内容(Parts/Components)"),
            // entity.qualityissueassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityissueassyrework.assydefectparts", "zh-CN", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityissueassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityissueassyrework.assydefectparts", "zh-HK", "组装不良内容_hk", "组装不良内容(Parts/Components)"),

            // entity.qualityissueassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworkcost", "en-US", "组装选别改修费用_us", "组装选别・改修费用(元)"),
            // entity.qualityissueassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworkcost", "ja-JP", "组装选别改修费用_jp", "组装选别・改修费用(元)"),
            // entity.qualityissueassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworkcost", "zh-CN", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityissueassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworkcost", "zh-HK", "组装选别改修费用_hk", "组装选别・改修费用(元)"),

            // entity.qualityissueassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworktimeminutes", "en-US", "组装选别改修时间_us", "组装选别・改修时间(分钟)"),
            // entity.qualityissueassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworktimeminutes", "ja-JP", "组装选别改修时间_jp", "组装选别・改修时间(分钟)"),
            // entity.qualityissueassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworktimeminutes", "zh-CN", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityissueassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworktimeminutes", "zh-HK", "组装选别改修时间_hk", "组装选别・改修时间(分钟)"),

            // entity.qualityissueassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreinspectiontimeminutes", "en-US", "组装再检查时间_us", "组装再检查时间(分钟)"),
            // entity.qualityissueassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreinspectiontimeminutes", "ja-JP", "组装再检查时间_jp", "组装再检查时间(分钟)"),
            // entity.qualityissueassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreinspectiontimeminutes", "zh-CN", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityissueassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityissueassyrework.assyreinspectiontimeminutes", "zh-HK", "组装再检查时间_hk", "组装再检查时间(分钟)"),

            // entity.qualityissueassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityissueassyrework.assytravelcost", "en-US", "组装交通费旅费_us", "组装交通费、旅费(元)"),
            // entity.qualityissueassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityissueassyrework.assytravelcost", "ja-JP", "组装交通费旅费_jp", "组装交通费、旅费(元)"),
            // entity.qualityissueassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityissueassyrework.assytravelcost", "zh-CN", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityissueassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityissueassyrework.assytravelcost", "zh-HK", "组装交通费旅费_hk", "组装交通费、旅费(元)"),

            // entity.qualityissueassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityissueassyrework.assywarehousecost", "en-US", "组装仓库管理费_us", "组装仓库管理费(元)"),
            // entity.qualityissueassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityissueassyrework.assywarehousecost", "ja-JP", "组装仓库管理费_jp", "组装仓库管理费(元)"),
            // entity.qualityissueassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityissueassyrework.assywarehousecost", "zh-CN", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityissueassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityissueassyrework.assywarehousecost", "zh-HK", "组装仓库管理费_hk", "组装仓库管理费(元)"),

            // entity.qualityissueassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses", "en-US", "组装选别改修其他费用_us", "组装选别・改修其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses", "ja-JP", "组装选别改修其他费用_jp", "组装选别・改修其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses", "zh-CN", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses", "zh-HK", "组装选别改修其他费用_hk", "组装选别・改修其他费用(元)"),

            // entity.qualityissueassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworknote", "en-US", "组装选别改修备注_us", "组装选别・改修备注"),
            // entity.qualityissueassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworknote", "ja-JP", "组装选别改修备注_jp", "组装选别・改修备注"),
            // entity.qualityissueassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworknote", "zh-CN", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityissueassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityissueassyrework.assyreworknote", "zh-HK", "组装选别改修备注_hk", "组装选别・改修备注"),

            // entity.qualityissueassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyscrapcost", "en-US", "组装向顾客费用请求_us", "组装向顾客的费用请求(元)"),
            // entity.qualityissueassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyscrapcost", "ja-JP", "组装向顾客费用请求_jp", "组装向顾客的费用请求(元)"),
            // entity.qualityissueassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyscrapcost", "zh-CN", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityissueassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityissueassyrework.assyscrapcost", "zh-HK", "组装向顾客费用请求_hk", "组装向顾客的费用请求(元)"),

            // entity.qualityissueassyrework.assycustomername1
            new TranslationSeedItem("entity.qualityissueassyrework.assycustomername1", "en-US", "组装顾客名1_us", "组装顾客名"),
            // entity.qualityissueassyrework.assycustomername1
            new TranslationSeedItem("entity.qualityissueassyrework.assycustomername1", "ja-JP", "组装顾客名1_jp", "组装顾客名"),
            // entity.qualityissueassyrework.assycustomername1
            new TranslationSeedItem("entity.qualityissueassyrework.assycustomername1", "zh-CN", "组装顾客名1", "组装顾客名"),
            // entity.qualityissueassyrework.assycustomername1
            new TranslationSeedItem("entity.qualityissueassyrework.assycustomername1", "zh-HK", "组装顾客名1_hk", "组装顾客名"),

            // entity.qualityissueassyrework.assydebitnotecode
            new TranslationSeedItem("entity.qualityissueassyrework.assydebitnotecode", "en-US", "组装 Debit Note No_us", "组装 Debit Note No"),
            // entity.qualityissueassyrework.assydebitnotecode
            new TranslationSeedItem("entity.qualityissueassyrework.assydebitnotecode", "ja-JP", "组装 Debit Note No_jp", "组装 Debit Note No"),
            // entity.qualityissueassyrework.assydebitnotecode
            new TranslationSeedItem("entity.qualityissueassyrework.assydebitnotecode", "zh-CN", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityissueassyrework.assydebitnotecode
            new TranslationSeedItem("entity.qualityissueassyrework.assydebitnotecode", "zh-HK", "组装 Debit Note No_hk", "组装 Debit Note No"),

            // entity.qualityissueassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses2", "en-US", "组装其他费用_us", "组装其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses2", "ja-JP", "组装其他费用_jp", "组装其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses2", "zh-CN", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityissueassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityissueassyrework.assyotherexpenses2", "zh-HK", "组装其他费用_hk", "组装其他费用(元)"),

            // entity.qualityissueassyrework.assynote
            new TranslationSeedItem("entity.qualityissueassyrework.assynote", "en-US", "组装备注_us", "组装备注"),
            // entity.qualityissueassyrework.assynote
            new TranslationSeedItem("entity.qualityissueassyrework.assynote", "ja-JP", "组装备注_jp", "组装备注"),
            // entity.qualityissueassyrework.assynote
            new TranslationSeedItem("entity.qualityissueassyrework.assynote", "zh-CN", "组装备注", "组装备注"),
            // entity.qualityissueassyrework.assynote
            new TranslationSeedItem("entity.qualityissueassyrework.assynote", "zh-HK", "组装备注_hk", "组装备注"),

            // entity.qualityissueassyrework.responsegroupcode
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupcode", "en-US", "对应组编码_us", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissueassyrework.responsegroupcode
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupcode", "ja-JP", "对应组编码_jp", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissueassyrework.responsegroupcode
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupcode", "zh-CN", "对应组编码", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissueassyrework.responsegroupcode
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupcode", "zh-HK", "对应组编码_hk", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),

            // entity.qualityissueassyrework.responsegroupname
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupname", "en-US", "对应组名称_us", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissueassyrework.responsegroupname
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupname", "ja-JP", "对应组名称_jp", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissueassyrework.responsegroupname
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupname", "zh-CN", "对应组名称", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissueassyrework.responsegroupname
            new TranslationSeedItem("entity.qualityissueassyrework.responsegroupname", "zh-HK", "对应组名称_hk", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),

            // entity.qualityissueassyrework.isobsolete
            new TranslationSeedItem("entity.qualityissueassyrework.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissueassyrework.isobsolete
            new TranslationSeedItem("entity.qualityissueassyrework.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissueassyrework.isobsolete
            new TranslationSeedItem("entity.qualityissueassyrework.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissueassyrework.isobsolete
            new TranslationSeedItem("entity.qualityissueassyrework.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityissueassyrework.issue
            new TranslationSeedItem("entity.qualityissueassyrework.issue", "en-US", "品质问题主表_us", "品质问题主表(导航属性)"),
            // entity.qualityissueassyrework.issue
            new TranslationSeedItem("entity.qualityissueassyrework.issue", "ja-JP", "品质问题主表_jp", "品质问题主表(导航属性)"),
            // entity.qualityissueassyrework.issue
            new TranslationSeedItem("entity.qualityissueassyrework.issue", "zh-CN", "品质问题主表", "品质问题主表(导航属性)"),
            // entity.qualityissueassyrework.issue
            new TranslationSeedItem("entity.qualityissueassyrework.issue", "zh-HK", "品质问题主表_hk", "品质问题主表(导航属性)"),
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
