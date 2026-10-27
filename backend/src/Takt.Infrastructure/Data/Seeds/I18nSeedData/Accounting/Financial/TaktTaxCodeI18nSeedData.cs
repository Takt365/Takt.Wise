// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktTaxCodeI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTaxCode 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktTaxCode 实体国际化翻译种子（键前缀 entity.taxcode.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTaxCodeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTaxCode 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 taxcode 实体翻译...", tenantCode);

        foreach (var item in GetTaxCodeTranslations())
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

        TaktLogger.Information("TaktTaxCode 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTaxCode 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.taxcode._self / entity.taxcode.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTaxCodeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.taxcode._self
            new TranslationSeedItem("entity.taxcode._self", "en-US", "Tax Code Information_us", "实体名称"),
            // entity.taxcode._self
            new TranslationSeedItem("entity.taxcode._self", "ja-JP", "税收代码信息_jp", "实体名称"),
            // entity.taxcode._self
            new TranslationSeedItem("entity.taxcode._self", "zh-CN", "税收代码信息", "实体名称"),
            // entity.taxcode._self
            new TranslationSeedItem("entity.taxcode._self", "zh-HK", "税收代码信息_hk", "实体名称"),

            // entity.taxcode.taxprocedurecode
            new TranslationSeedItem("entity.taxcode.taxprocedurecode", "en-US", "过程_us", "过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）"),
            // entity.taxcode.taxprocedurecode
            new TranslationSeedItem("entity.taxcode.taxprocedurecode", "ja-JP", "过程_jp", "过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）"),
            // entity.taxcode.taxprocedurecode
            new TranslationSeedItem("entity.taxcode.taxprocedurecode", "zh-CN", "过程", "过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）"),
            // entity.taxcode.taxprocedurecode
            new TranslationSeedItem("entity.taxcode.taxprocedurecode", "zh-HK", "过程_hk", "过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）"),

            // entity.taxcode.taxcode
            new TranslationSeedItem("entity.taxcode.taxcode", "en-US", "税码_us", "税码（长度 2；同一租户、公司、过程、税类别下唯一；前端表单选择编码规则后自动通过 TaktNumbering 税码规则生成并展示，非手输；单据类型菜单：税收代码）"),
            // entity.taxcode.taxcode
            new TranslationSeedItem("entity.taxcode.taxcode", "ja-JP", "税码_jp", "税码（长度 2；同一租户、公司、过程、税类别下唯一；前端表单选择编码规则后自动通过 TaktNumbering 税码规则生成并展示，非手输；单据类型菜单：税收代码）"),
            // entity.taxcode.taxcode
            new TranslationSeedItem("entity.taxcode.taxcode", "zh-CN", "税码", "税码（长度 2；同一租户、公司、过程、税类别下唯一；前端表单选择编码规则后自动通过 TaktNumbering 税码规则生成并展示，非手输；单据类型菜单：税收代码）"),
            // entity.taxcode.taxcode
            new TranslationSeedItem("entity.taxcode.taxcode", "zh-HK", "税码_hk", "税码（长度 2；同一租户、公司、过程、税类别下唯一；前端表单选择编码规则后自动通过 TaktNumbering 税码规则生成并展示，非手输；单据类型菜单：税收代码）"),

            // entity.taxcode.taxcategory
            new TranslationSeedItem("entity.taxcode.taxcategory", "en-US", "税类别_us", "税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）"),
            // entity.taxcode.taxcategory
            new TranslationSeedItem("entity.taxcode.taxcategory", "ja-JP", "税类别_jp", "税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）"),
            // entity.taxcode.taxcategory
            new TranslationSeedItem("entity.taxcode.taxcategory", "zh-CN", "税类别", "税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）"),
            // entity.taxcode.taxcategory
            new TranslationSeedItem("entity.taxcode.taxcategory", "zh-HK", "税类别_hk", "税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）"),

            // entity.taxcode.description
            new TranslationSeedItem("entity.taxcode.description", "en-US", "描述_us", "描述（税码文本，如销项增值税、进项税档位说明；Length=50）"),
            // entity.taxcode.description
            new TranslationSeedItem("entity.taxcode.description", "ja-JP", "描述_jp", "描述（税码文本，如销项增值税、进项税档位说明；Length=50）"),
            // entity.taxcode.description
            new TranslationSeedItem("entity.taxcode.description", "zh-CN", "描述", "描述（税码文本，如销项增值税、进项税档位说明；Length=50）"),
            // entity.taxcode.description
            new TranslationSeedItem("entity.taxcode.description", "zh-HK", "描述_hk", "描述（税码文本，如销项增值税、进项税档位说明；Length=50）"),

            // entity.taxcode.sortorder
            new TranslationSeedItem("entity.taxcode.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.taxcode.sortorder
            new TranslationSeedItem("entity.taxcode.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.taxcode.sortorder
            new TranslationSeedItem("entity.taxcode.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.taxcode.sortorder
            new TranslationSeedItem("entity.taxcode.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.taxcode.status
            new TranslationSeedItem("entity.taxcode.status", "en-US", "税收代码状态_us", "税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcode.status
            new TranslationSeedItem("entity.taxcode.status", "ja-JP", "税收代码状态_jp", "税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcode.status
            new TranslationSeedItem("entity.taxcode.status", "zh-CN", "税收代码状态", "税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcode.status
            new TranslationSeedItem("entity.taxcode.status", "zh-HK", "税收代码状态_hk", "税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Financial";
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
