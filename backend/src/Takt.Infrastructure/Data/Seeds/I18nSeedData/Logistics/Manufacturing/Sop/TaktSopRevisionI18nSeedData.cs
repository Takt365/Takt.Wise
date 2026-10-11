// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevisionI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopRevision 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopRevision 实体国际化翻译种子（键前缀 entity.soprevision.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopRevisionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopRevision 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 soprevision 实体翻译...", tenantCode);

        foreach (var item in GetSopRevisionTranslations())
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

        TaktLogger.Information("TaktSopRevision 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopRevision 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.soprevision._self / entity.soprevision.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopRevisionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.soprevision._self
            new TranslationSeedItem("entity.soprevision._self", "en-US", "Sop Revision Information_us", "实体名称"),
            // entity.soprevision._self
            new TranslationSeedItem("entity.soprevision._self", "ja-JP", "SOP 版本信息_jp", "实体名称"),
            // entity.soprevision._self
            new TranslationSeedItem("entity.soprevision._self", "zh-CN", "SOP 版本信息", "实体名称"),
            // entity.soprevision._self
            new TranslationSeedItem("entity.soprevision._self", "zh-HK", "SOP 版本信息_hk", "实体名称"),

            // entity.soprevision.sopid
            new TranslationSeedItem("entity.soprevision.sopid", "en-US", "SOP文档头ID_us", "SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.soprevision.sopid
            new TranslationSeedItem("entity.soprevision.sopid", "ja-JP", "SOP文档头ID_jp", "SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.soprevision.sopid
            new TranslationSeedItem("entity.soprevision.sopid", "zh-CN", "SOP文档头ID", "SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.soprevision.sopid
            new TranslationSeedItem("entity.soprevision.sopid", "zh-HK", "SOP文档头ID_hk", "SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）"),

            // entity.soprevision.revision
            new TranslationSeedItem("entity.soprevision.revision", "en-US", "版本号_us", "版本号（主版本.次版本，如 1.0、A.01）"),
            // entity.soprevision.revision
            new TranslationSeedItem("entity.soprevision.revision", "ja-JP", "版本号_jp", "版本号（主版本.次版本，如 1.0、A.01）"),
            // entity.soprevision.revision
            new TranslationSeedItem("entity.soprevision.revision", "zh-CN", "版本号", "版本号（主版本.次版本，如 1.0、A.01）"),
            // entity.soprevision.revision
            new TranslationSeedItem("entity.soprevision.revision", "zh-HK", "版本号_hk", "版本号（主版本.次版本，如 1.0、A.01）"),

            // entity.soprevision.fileurl
            new TranslationSeedItem("entity.soprevision.fileurl", "en-US", "受控PDF URL_us", "受控 PDF URL"),
            // entity.soprevision.fileurl
            new TranslationSeedItem("entity.soprevision.fileurl", "ja-JP", "受控PDF URL_jp", "受控 PDF URL"),
            // entity.soprevision.fileurl
            new TranslationSeedItem("entity.soprevision.fileurl", "zh-CN", "受控PDF URL", "受控 PDF URL"),
            // entity.soprevision.fileurl
            new TranslationSeedItem("entity.soprevision.fileurl", "zh-HK", "受控PDF URL_hk", "受控 PDF URL"),

            // entity.soprevision.changedesc
            new TranslationSeedItem("entity.soprevision.changedesc", "en-US", "变更说明_us", "变更说明"),
            // entity.soprevision.changedesc
            new TranslationSeedItem("entity.soprevision.changedesc", "ja-JP", "变更说明_jp", "变更说明"),
            // entity.soprevision.changedesc
            new TranslationSeedItem("entity.soprevision.changedesc", "zh-CN", "变更说明", "变更说明"),
            // entity.soprevision.changedesc
            new TranslationSeedItem("entity.soprevision.changedesc", "zh-HK", "变更说明_hk", "变更说明"),

            // entity.soprevision.ecnid
            new TranslationSeedItem("entity.soprevision.ecnid", "en-US", "ECN主表ID_us", "关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）"),
            // entity.soprevision.ecnid
            new TranslationSeedItem("entity.soprevision.ecnid", "ja-JP", "ECN主表ID_jp", "关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）"),
            // entity.soprevision.ecnid
            new TranslationSeedItem("entity.soprevision.ecnid", "zh-CN", "ECN主表ID", "关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）"),
            // entity.soprevision.ecnid
            new TranslationSeedItem("entity.soprevision.ecnid", "zh-HK", "ECN主表ID_hk", "关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）"),

            // entity.soprevision.islocked
            new TranslationSeedItem("entity.soprevision.islocked", "en-US", "是否锁定_us", "是否锁定（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.islocked
            new TranslationSeedItem("entity.soprevision.islocked", "ja-JP", "是否锁定_jp", "是否锁定（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.islocked
            new TranslationSeedItem("entity.soprevision.islocked", "zh-CN", "是否锁定", "是否锁定（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.islocked
            new TranslationSeedItem("entity.soprevision.islocked", "zh-HK", "是否锁定_hk", "是否锁定（字典 sys_yes_no；0=否，1=是）"),

            // entity.soprevision.forceleaderack
            new TranslationSeedItem("entity.soprevision.forceleaderack", "en-US", "是否强制班组长确认_us", "是否强制班组长确认（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.forceleaderack
            new TranslationSeedItem("entity.soprevision.forceleaderack", "ja-JP", "是否强制班组长确认_jp", "是否强制班组长确认（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.forceleaderack
            new TranslationSeedItem("entity.soprevision.forceleaderack", "zh-CN", "是否强制班组长确认", "是否强制班组长确认（字典 sys_yes_no；0=否，1=是）"),
            // entity.soprevision.forceleaderack
            new TranslationSeedItem("entity.soprevision.forceleaderack", "zh-HK", "是否强制班组长确认_hk", "是否强制班组长确认（字典 sys_yes_no；0=否，1=是）"),

            // entity.soprevision.revisionstatus
            new TranslationSeedItem("entity.soprevision.revisionstatus", "en-US", "版本状态_us", "版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）"),
            // entity.soprevision.revisionstatus
            new TranslationSeedItem("entity.soprevision.revisionstatus", "ja-JP", "版本状态_jp", "版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）"),
            // entity.soprevision.revisionstatus
            new TranslationSeedItem("entity.soprevision.revisionstatus", "zh-CN", "版本状态", "版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）"),
            // entity.soprevision.revisionstatus
            new TranslationSeedItem("entity.soprevision.revisionstatus", "zh-HK", "版本状态_hk", "版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）"),

            // entity.soprevision.effectiverule
            new TranslationSeedItem("entity.soprevision.effectiverule", "en-US", "生效规则_us", "生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）"),
            // entity.soprevision.effectiverule
            new TranslationSeedItem("entity.soprevision.effectiverule", "ja-JP", "生效规则_jp", "生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）"),
            // entity.soprevision.effectiverule
            new TranslationSeedItem("entity.soprevision.effectiverule", "zh-CN", "生效规则", "生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）"),
            // entity.soprevision.effectiverule
            new TranslationSeedItem("entity.soprevision.effectiverule", "zh-HK", "生效规则_hk", "生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）"),

            // entity.soprevision.sopdoc
            new TranslationSeedItem("entity.soprevision.sopdoc", "en-US", "SOP 文档头_us", "SOP 文档头"),
            // entity.soprevision.sopdoc
            new TranslationSeedItem("entity.soprevision.sopdoc", "ja-JP", "SOP 文档头_jp", "SOP 文档头"),
            // entity.soprevision.sopdoc
            new TranslationSeedItem("entity.soprevision.sopdoc", "zh-CN", "SOP 文档头", "SOP 文档头"),
            // entity.soprevision.sopdoc
            new TranslationSeedItem("entity.soprevision.sopdoc", "zh-HK", "SOP 文档头_hk", "SOP 文档头"),

            // entity.soprevision.contents
            new TranslationSeedItem("entity.soprevision.contents", "en-US", "多语言正文_us", "多语言正文"),
            // entity.soprevision.contents
            new TranslationSeedItem("entity.soprevision.contents", "ja-JP", "多语言正文_jp", "多语言正文"),
            // entity.soprevision.contents
            new TranslationSeedItem("entity.soprevision.contents", "zh-CN", "多语言正文", "多语言正文"),
            // entity.soprevision.contents
            new TranslationSeedItem("entity.soprevision.contents", "zh-HK", "多语言正文_hk", "多语言正文"),
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
        translation.ResourceGroup = "Sop";
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
