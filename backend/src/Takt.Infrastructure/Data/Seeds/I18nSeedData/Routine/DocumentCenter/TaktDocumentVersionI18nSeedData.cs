// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDocumentVersion 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter;

/// <summary>
/// TaktDocumentVersion 实体国际化翻译种子（键前缀 entity.documentversion.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDocumentVersionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDocumentVersion 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 documentversion 实体翻译...", tenantCode);

        foreach (var item in GetDocumentVersionTranslations())
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

        TaktLogger.Information("TaktDocumentVersion 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDocumentVersion 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.documentversion._self / entity.documentversion.{{field}}；ResourceGroup=DocumentCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentVersionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "en-US", "Document Version Information_us", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "ja-JP", "文管文档版本子信息_jp", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "zh-CN", "文管文档版本子信息", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "zh-HK", "文管文档版本子信息_hk", "实体名称"),

            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "en-US", "文档ID_us", "文档 ID（选项 TaktDocuments/options；DictValue=Id）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "ja-JP", "文档ID_jp", "文档 ID（选项 TaktDocuments/options；DictValue=Id）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "zh-CN", "文档ID", "文档 ID（选项 TaktDocuments/options；DictValue=Id）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "zh-HK", "文档ID_hk", "文档 ID（选项 TaktDocuments/options；DictValue=Id）"),

            // entity.documentversion.linenumber
            new TranslationSeedItem("entity.documentversion.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.documentversion.linenumber
            new TranslationSeedItem("entity.documentversion.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.documentversion.linenumber
            new TranslationSeedItem("entity.documentversion.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.documentversion.linenumber
            new TranslationSeedItem("entity.documentversion.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "en-US", "版本号_us", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "ja-JP", "版本号_jp", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "zh-CN", "版本号", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "zh-HK", "版本号_hk", "版本号"),

            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "en-US", "版本说明_us", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "ja-JP", "版本说明_jp", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "zh-CN", "版本说明", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "zh-HK", "版本说明_hk", "版本说明"),

            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "en-US", "文件ID_us", "文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "ja-JP", "文件ID_jp", "文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "zh-CN", "文件ID", "文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "zh-HK", "文件ID_hk", "文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）"),

            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "en-US", "修订人ID_us", "修订人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "ja-JP", "修订人ID_jp", "修订人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "zh-CN", "修订人ID", "修订人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "zh-HK", "修订人ID_hk", "修订人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "en-US", "修订人姓名_us", "修订人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "ja-JP", "修订人姓名_jp", "修订人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "zh-CN", "修订人姓名", "修订人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "zh-HK", "修订人姓名_hk", "修订人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "en-US", "修订时间_us", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "ja-JP", "修订时间_jp", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "zh-CN", "修订时间", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "zh-HK", "修订时间_hk", "修订时间"),

            // entity.documentversion.isobsolete
            new TranslationSeedItem("entity.documentversion.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.documentversion.isobsolete
            new TranslationSeedItem("entity.documentversion.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.documentversion.isobsolete
            new TranslationSeedItem("entity.documentversion.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.documentversion.isobsolete
            new TranslationSeedItem("entity.documentversion.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "en-US", "文档_us", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "ja-JP", "文档_jp", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "zh-CN", "文档", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "zh-HK", "文档_hk", "文档（主表）"),
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
        translation.ResourceGroup = "DocumentCenter";
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
