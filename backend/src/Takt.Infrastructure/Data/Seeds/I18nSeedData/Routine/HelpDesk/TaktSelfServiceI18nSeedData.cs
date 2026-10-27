// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktSelfServiceI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSelfService 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktSelfService 实体国际化翻译种子（键前缀 entity.selfservice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSelfServiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSelfService 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 selfservice 实体翻译...", tenantCode);

        foreach (var item in GetSelfServiceTranslations())
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

        TaktLogger.Information("TaktSelfService 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSelfService 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.selfservice._self / entity.selfservice.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSelfServiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.selfservice._self
            new TranslationSeedItem("entity.selfservice._self", "en-US", "Self Service Information_us", "实体名称"),
            // entity.selfservice._self
            new TranslationSeedItem("entity.selfservice._self", "ja-JP", "服务台自助服务项信息_jp", "实体名称"),
            // entity.selfservice._self
            new TranslationSeedItem("entity.selfservice._self", "zh-CN", "服务台自助服务项信息", "实体名称"),
            // entity.selfservice._self
            new TranslationSeedItem("entity.selfservice._self", "zh-HK", "服务台自助服务项信息_hk", "实体名称"),

            // entity.selfservice.servicename
            new TranslationSeedItem("entity.selfservice.servicename", "en-US", "服务名称_us", "自助服务名称"),
            // entity.selfservice.servicename
            new TranslationSeedItem("entity.selfservice.servicename", "ja-JP", "服务名称_jp", "自助服务名称"),
            // entity.selfservice.servicename
            new TranslationSeedItem("entity.selfservice.servicename", "zh-CN", "服务名称", "自助服务名称"),
            // entity.selfservice.servicename
            new TranslationSeedItem("entity.selfservice.servicename", "zh-HK", "服务名称_hk", "自助服务名称"),

            // entity.selfservice.servicetype
            new TranslationSeedItem("entity.selfservice.servicetype", "en-US", "服务类型_us", "服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）"),
            // entity.selfservice.servicetype
            new TranslationSeedItem("entity.selfservice.servicetype", "ja-JP", "服务类型_jp", "服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）"),
            // entity.selfservice.servicetype
            new TranslationSeedItem("entity.selfservice.servicetype", "zh-CN", "服务类型", "服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）"),
            // entity.selfservice.servicetype
            new TranslationSeedItem("entity.selfservice.servicetype", "zh-HK", "服务类型_hk", "服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）"),

            // entity.selfservice.description
            new TranslationSeedItem("entity.selfservice.description", "en-US", "描述_us", "描述"),
            // entity.selfservice.description
            new TranslationSeedItem("entity.selfservice.description", "ja-JP", "描述_jp", "描述"),
            // entity.selfservice.description
            new TranslationSeedItem("entity.selfservice.description", "zh-CN", "描述", "描述"),
            // entity.selfservice.description
            new TranslationSeedItem("entity.selfservice.description", "zh-HK", "描述_hk", "描述"),

            // entity.selfservice.linkorcode
            new TranslationSeedItem("entity.selfservice.linkorcode", "en-US", "链接或表单编码_us", "链接地址或表单编码"),
            // entity.selfservice.linkorcode
            new TranslationSeedItem("entity.selfservice.linkorcode", "ja-JP", "链接或表单编码_jp", "链接地址或表单编码"),
            // entity.selfservice.linkorcode
            new TranslationSeedItem("entity.selfservice.linkorcode", "zh-CN", "链接或表单编码", "链接地址或表单编码"),
            // entity.selfservice.linkorcode
            new TranslationSeedItem("entity.selfservice.linkorcode", "zh-HK", "链接或表单编码_hk", "链接地址或表单编码"),

            // entity.selfservice.iconurl
            new TranslationSeedItem("entity.selfservice.iconurl", "en-US", "图标URL_us", "图标或图片 URL"),
            // entity.selfservice.iconurl
            new TranslationSeedItem("entity.selfservice.iconurl", "ja-JP", "图标URL_jp", "图标或图片 URL"),
            // entity.selfservice.iconurl
            new TranslationSeedItem("entity.selfservice.iconurl", "zh-CN", "图标URL", "图标或图片 URL"),
            // entity.selfservice.iconurl
            new TranslationSeedItem("entity.selfservice.iconurl", "zh-HK", "图标URL_hk", "图标或图片 URL"),

            // entity.selfservice.attachments
            new TranslationSeedItem("entity.selfservice.attachments", "en-US", "附件_us", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.selfservice.attachments
            new TranslationSeedItem("entity.selfservice.attachments", "ja-JP", "附件_jp", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.selfservice.attachments
            new TranslationSeedItem("entity.selfservice.attachments", "zh-CN", "附件", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.selfservice.attachments
            new TranslationSeedItem("entity.selfservice.attachments", "zh-HK", "附件_hk", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),

            // entity.selfservice.sortorder
            new TranslationSeedItem("entity.selfservice.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.selfservice.sortorder
            new TranslationSeedItem("entity.selfservice.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.selfservice.sortorder
            new TranslationSeedItem("entity.selfservice.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.selfservice.sortorder
            new TranslationSeedItem("entity.selfservice.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.selfservice.status
            new TranslationSeedItem("entity.selfservice.status", "en-US", "状态_us", "自助服务状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.selfservice.status
            new TranslationSeedItem("entity.selfservice.status", "ja-JP", "状态_jp", "自助服务状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.selfservice.status
            new TranslationSeedItem("entity.selfservice.status", "zh-CN", "状态", "自助服务状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.selfservice.status
            new TranslationSeedItem("entity.selfservice.status", "zh-HK", "状态_hk", "自助服务状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
        translation.ResourceGroup = "HelpDesk";
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
