// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktDictDataI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDictData 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktDictData 实体国际化翻译种子（键前缀 entity.dictdata.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDictDataI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDictData 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dictdata 实体翻译...", tenantCode);

        foreach (var item in GetDictDataTranslations())
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

        TaktLogger.Information("TaktDictData 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDictData 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dictdata._self / entity.dictdata.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDictDataTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dictdata._self
            new TranslationSeedItem("entity.dictdata._self", "en-US", "Dict Data Information_us", "实体名称"),
            // entity.dictdata._self
            new TranslationSeedItem("entity.dictdata._self", "ja-JP", "字典数据信息_jp", "实体名称"),
            // entity.dictdata._self
            new TranslationSeedItem("entity.dictdata._self", "zh-CN", "字典数据信息", "实体名称"),
            // entity.dictdata._self
            new TranslationSeedItem("entity.dictdata._self", "zh-HK", "字典数据信息_hk", "实体名称"),

            // entity.dictdata.dicttypeid
            new TranslationSeedItem("entity.dictdata.dicttypeid", "en-US", "字典类型ID_us", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.dictdata.dicttypeid
            new TranslationSeedItem("entity.dictdata.dicttypeid", "ja-JP", "字典类型ID_jp", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.dictdata.dicttypeid
            new TranslationSeedItem("entity.dictdata.dicttypeid", "zh-CN", "字典类型ID", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.dictdata.dicttypeid
            new TranslationSeedItem("entity.dictdata.dicttypeid", "zh-HK", "字典类型ID_hk", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),

            // entity.dictdata.dicttypecode
            new TranslationSeedItem("entity.dictdata.dicttypecode", "en-US", "字典类型编码_us", "字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）"),
            // entity.dictdata.dicttypecode
            new TranslationSeedItem("entity.dictdata.dicttypecode", "ja-JP", "字典类型编码_jp", "字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）"),
            // entity.dictdata.dicttypecode
            new TranslationSeedItem("entity.dictdata.dicttypecode", "zh-CN", "字典类型编码", "字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）"),
            // entity.dictdata.dicttypecode
            new TranslationSeedItem("entity.dictdata.dicttypecode", "zh-HK", "字典类型编码_hk", "字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；varchar Length=140）"),

            // entity.dictdata.dictlabel
            new TranslationSeedItem("entity.dictdata.dictlabel", "en-US", "字典项标签_us", "字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；nvarchar Length=40；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）"),
            // entity.dictdata.dictlabel
            new TranslationSeedItem("entity.dictdata.dictlabel", "ja-JP", "字典项标签_jp", "字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；nvarchar Length=40；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）"),
            // entity.dictdata.dictlabel
            new TranslationSeedItem("entity.dictdata.dictlabel", "zh-CN", "字典项标签", "字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；nvarchar Length=40；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）"),
            // entity.dictdata.dictlabel
            new TranslationSeedItem("entity.dictdata.dictlabel", "zh-HK", "字典项标签_hk", "字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；nvarchar Length=40；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）"),

            // entity.dictdata.dictvalue
            new TranslationSeedItem("entity.dictdata.dictvalue", "en-US", "字典项值_us", "字典项值（实际存储值，如：0, 1, 2；varchar Length=40）"),
            // entity.dictdata.dictvalue
            new TranslationSeedItem("entity.dictdata.dictvalue", "ja-JP", "字典项值_jp", "字典项值（实际存储值，如：0, 1, 2；varchar Length=40）"),
            // entity.dictdata.dictvalue
            new TranslationSeedItem("entity.dictdata.dictvalue", "zh-CN", "字典项值", "字典项值（实际存储值，如：0, 1, 2；varchar Length=40）"),
            // entity.dictdata.dictvalue
            new TranslationSeedItem("entity.dictdata.dictvalue", "zh-HK", "字典项值_hk", "字典项值（实际存储值，如：0, 1, 2；varchar Length=40）"),

            // entity.dictdata.i18nkey
            new TranslationSeedItem("entity.dictdata.i18nkey", "en-US", "国际化键_us", "国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f；varchar Length=140）"),
            // entity.dictdata.i18nkey
            new TranslationSeedItem("entity.dictdata.i18nkey", "ja-JP", "国际化键_jp", "国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f；varchar Length=140）"),
            // entity.dictdata.i18nkey
            new TranslationSeedItem("entity.dictdata.i18nkey", "zh-CN", "国际化键", "国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f；varchar Length=140）"),
            // entity.dictdata.i18nkey
            new TranslationSeedItem("entity.dictdata.i18nkey", "zh-HK", "国际化键_hk", "国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f；varchar Length=140）"),

            // entity.dictdata.extlabel
            new TranslationSeedItem("entity.dictdata.extlabel", "en-US", "扩展标签_us", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等；nvarchar Length=140）"),
            // entity.dictdata.extlabel
            new TranslationSeedItem("entity.dictdata.extlabel", "ja-JP", "扩展标签_jp", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等；nvarchar Length=140）"),
            // entity.dictdata.extlabel
            new TranslationSeedItem("entity.dictdata.extlabel", "zh-CN", "扩展标签", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等；nvarchar Length=140）"),
            // entity.dictdata.extlabel
            new TranslationSeedItem("entity.dictdata.extlabel", "zh-HK", "扩展标签_hk", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等；nvarchar Length=140）"),

            // entity.dictdata.extvalue
            new TranslationSeedItem("entity.dictdata.extvalue", "en-US", "扩展值_us", "扩展值（用于存储额外的业务数据，如：编码、标识符等；varchar Length=140）"),
            // entity.dictdata.extvalue
            new TranslationSeedItem("entity.dictdata.extvalue", "ja-JP", "扩展值_jp", "扩展值（用于存储额外的业务数据，如：编码、标识符等；varchar Length=140）"),
            // entity.dictdata.extvalue
            new TranslationSeedItem("entity.dictdata.extvalue", "zh-CN", "扩展值", "扩展值（用于存储额外的业务数据，如：编码、标识符等；varchar Length=140）"),
            // entity.dictdata.extvalue
            new TranslationSeedItem("entity.dictdata.extvalue", "zh-HK", "扩展值_hk", "扩展值（用于存储额外的业务数据，如：编码、标识符等；varchar Length=140）"),

            // entity.dictdata.listclass
            new TranslationSeedItem("entity.dictdata.listclass", "en-US", "列表样式类_us", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictdata.listclass
            new TranslationSeedItem("entity.dictdata.listclass", "ja-JP", "列表样式类_jp", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictdata.listclass
            new TranslationSeedItem("entity.dictdata.listclass", "zh-CN", "列表样式类", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictdata.listclass
            new TranslationSeedItem("entity.dictdata.listclass", "zh-HK", "列表样式类_hk", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),

            // entity.dictdata.cssclass
            new TranslationSeedItem("entity.dictdata.cssclass", "en-US", "CSS类名_us", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictdata.cssclass
            new TranslationSeedItem("entity.dictdata.cssclass", "ja-JP", "CSS类名_jp", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictdata.cssclass
            new TranslationSeedItem("entity.dictdata.cssclass", "zh-CN", "CSS类名", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictdata.cssclass
            new TranslationSeedItem("entity.dictdata.cssclass", "zh-HK", "CSS类名_hk", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),

            // entity.dictdata.isdefault
            new TranslationSeedItem("entity.dictdata.isdefault", "en-US", "是否默认项_us", "是否默认项（1=是，0=否）"),
            // entity.dictdata.isdefault
            new TranslationSeedItem("entity.dictdata.isdefault", "ja-JP", "是否默认项_jp", "是否默认项（1=是，0=否）"),
            // entity.dictdata.isdefault
            new TranslationSeedItem("entity.dictdata.isdefault", "zh-CN", "是否默认项", "是否默认项（1=是，0=否）"),
            // entity.dictdata.isdefault
            new TranslationSeedItem("entity.dictdata.isdefault", "zh-HK", "是否默认项_hk", "是否默认项（1=是，0=否）"),

            // entity.dictdata.sortorder
            new TranslationSeedItem("entity.dictdata.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.dictdata.sortorder
            new TranslationSeedItem("entity.dictdata.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.dictdata.sortorder
            new TranslationSeedItem("entity.dictdata.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.dictdata.sortorder
            new TranslationSeedItem("entity.dictdata.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.dictdata.dicttype
            new TranslationSeedItem("entity.dictdata.dicttype", "en-US", "字典类型_us", "字典类型（多对一关联）"),
            // entity.dictdata.dicttype
            new TranslationSeedItem("entity.dictdata.dicttype", "ja-JP", "字典类型_jp", "字典类型（多对一关联）"),
            // entity.dictdata.dicttype
            new TranslationSeedItem("entity.dictdata.dicttype", "zh-CN", "字典类型", "字典类型（多对一关联）"),
            // entity.dictdata.dicttype
            new TranslationSeedItem("entity.dictdata.dicttype", "zh-HK", "字典类型_hk", "字典类型（多对一关联）"),
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
        translation.ResourceGroup = "Foundation";
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
