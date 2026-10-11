// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktBankI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBank 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBank 实体国际化翻译种子（键前缀 entity.bank.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBankI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBank 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 bank 实体翻译...", tenantCode);

        foreach (var item in GetBankTranslations())
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

        TaktLogger.Information("TaktBank 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBank 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.bank._self / entity.bank.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBankTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.bank._self
            new TranslationSeedItem("entity.bank._self", "en-US", "Bank Information_us", "实体名称"),
            // entity.bank._self
            new TranslationSeedItem("entity.bank._self", "ja-JP", "银行信息信息_jp", "实体名称"),
            // entity.bank._self
            new TranslationSeedItem("entity.bank._self", "zh-CN", "银行信息信息", "实体名称"),
            // entity.bank._self
            new TranslationSeedItem("entity.bank._self", "zh-HK", "银行信息信息_hk", "实体名称"),

            // entity.bank.countryregion
            new TranslationSeedItem("entity.bank.countryregion", "en-US", "国家地区_us", "国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）"),
            // entity.bank.countryregion
            new TranslationSeedItem("entity.bank.countryregion", "ja-JP", "国家地区_jp", "国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）"),
            // entity.bank.countryregion
            new TranslationSeedItem("entity.bank.countryregion", "zh-CN", "国家地区", "国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）"),
            // entity.bank.countryregion
            new TranslationSeedItem("entity.bank.countryregion", "zh-HK", "国家地区_hk", "国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）"),

            // entity.bank.code
            new TranslationSeedItem("entity.bank.code", "en-US", "银行代码_us", "银行代码（；CHAR 15；与国家地区组成业务唯一键）"),
            // entity.bank.code
            new TranslationSeedItem("entity.bank.code", "ja-JP", "银行代码_jp", "银行代码（；CHAR 15；与国家地区组成业务唯一键）"),
            // entity.bank.code
            new TranslationSeedItem("entity.bank.code", "zh-CN", "银行代码", "银行代码（；CHAR 15；与国家地区组成业务唯一键）"),
            // entity.bank.code
            new TranslationSeedItem("entity.bank.code", "zh-HK", "银行代码_hk", "银行代码（；CHAR 15；与国家地区组成业务唯一键）"),

            // entity.bank.name1
            new TranslationSeedItem("entity.bank.name1", "en-US", "银行名称1_us", "银行名称1"),
            // entity.bank.name1
            new TranslationSeedItem("entity.bank.name1", "ja-JP", "银行名称1_jp", "银行名称1"),
            // entity.bank.name1
            new TranslationSeedItem("entity.bank.name1", "zh-CN", "银行名称1", "银行名称1"),
            // entity.bank.name1
            new TranslationSeedItem("entity.bank.name1", "zh-HK", "银行名称1_hk", "银行名称1"),

            // entity.bank.name2
            new TranslationSeedItem("entity.bank.name2", "en-US", "银行名称2_us", "银行名称2"),
            // entity.bank.name2
            new TranslationSeedItem("entity.bank.name2", "ja-JP", "银行名称2_jp", "银行名称2"),
            // entity.bank.name2
            new TranslationSeedItem("entity.bank.name2", "zh-CN", "银行名称2", "银行名称2"),
            // entity.bank.name2
            new TranslationSeedItem("entity.bank.name2", "zh-HK", "银行名称2_hk", "银行名称2"),

            // entity.bank.province
            new TranslationSeedItem("entity.bank.province", "en-US", "州省_us", "州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.bank.province
            new TranslationSeedItem("entity.bank.province", "ja-JP", "州省_jp", "州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.bank.province
            new TranslationSeedItem("entity.bank.province", "zh-CN", "州省", "州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.bank.province
            new TranslationSeedItem("entity.bank.province", "zh-HK", "州省_hk", "州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.bank.prefecture
            new TranslationSeedItem("entity.bank.prefecture", "en-US", "地市_us", "地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.bank.prefecture
            new TranslationSeedItem("entity.bank.prefecture", "ja-JP", "地市_jp", "地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.bank.prefecture
            new TranslationSeedItem("entity.bank.prefecture", "zh-CN", "地市", "地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.bank.prefecture
            new TranslationSeedItem("entity.bank.prefecture", "zh-HK", "地市_hk", "地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.bank.district
            new TranslationSeedItem("entity.bank.district", "en-US", "区县_us", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.bank.district
            new TranslationSeedItem("entity.bank.district", "ja-JP", "区县_jp", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.bank.district
            new TranslationSeedItem("entity.bank.district", "zh-CN", "区县", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.bank.district
            new TranslationSeedItem("entity.bank.district", "zh-HK", "区县_hk", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),

            // entity.bank.township
            new TranslationSeedItem("entity.bank.township", "en-US", "乡镇街道_us", "乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）"),
            // entity.bank.township
            new TranslationSeedItem("entity.bank.township", "ja-JP", "乡镇街道_jp", "乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）"),
            // entity.bank.township
            new TranslationSeedItem("entity.bank.township", "zh-CN", "乡镇街道", "乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）"),
            // entity.bank.township
            new TranslationSeedItem("entity.bank.township", "zh-HK", "乡镇街道_hk", "乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）"),

            // entity.bank.village
            new TranslationSeedItem("entity.bank.village", "en-US", "行政村_us", "行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）"),
            // entity.bank.village
            new TranslationSeedItem("entity.bank.village", "ja-JP", "行政村_jp", "行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）"),
            // entity.bank.village
            new TranslationSeedItem("entity.bank.village", "zh-CN", "行政村", "行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）"),
            // entity.bank.village
            new TranslationSeedItem("entity.bank.village", "zh-HK", "行政村_hk", "行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）"),

            // entity.bank.address1
            new TranslationSeedItem("entity.bank.address1", "en-US", "地址1_us", "地址1（详细地址行1）"),
            // entity.bank.address1
            new TranslationSeedItem("entity.bank.address1", "ja-JP", "地址1_jp", "地址1（详细地址行1）"),
            // entity.bank.address1
            new TranslationSeedItem("entity.bank.address1", "zh-CN", "地址1", "地址1（详细地址行1）"),
            // entity.bank.address1
            new TranslationSeedItem("entity.bank.address1", "zh-HK", "地址1_hk", "地址1（详细地址行1）"),

            // entity.bank.address2
            new TranslationSeedItem("entity.bank.address2", "en-US", "地址2_us", "地址2（详细地址行2）"),
            // entity.bank.address2
            new TranslationSeedItem("entity.bank.address2", "ja-JP", "地址2_jp", "地址2（详细地址行2）"),
            // entity.bank.address2
            new TranslationSeedItem("entity.bank.address2", "zh-CN", "地址2", "地址2（详细地址行2）"),
            // entity.bank.address2
            new TranslationSeedItem("entity.bank.address2", "zh-HK", "地址2_hk", "地址2（详细地址行2）"),

            // entity.bank.swiftbic
            new TranslationSeedItem("entity.bank.swiftbic", "en-US", "银行识别码_us", "银行识别码（SWIFT/BIC；CHAR 11）"),
            // entity.bank.swiftbic
            new TranslationSeedItem("entity.bank.swiftbic", "ja-JP", "银行识别码_jp", "银行识别码（SWIFT/BIC；CHAR 11）"),
            // entity.bank.swiftbic
            new TranslationSeedItem("entity.bank.swiftbic", "zh-CN", "银行识别码", "银行识别码（SWIFT/BIC；CHAR 11）"),
            // entity.bank.swiftbic
            new TranslationSeedItem("entity.bank.swiftbic", "zh-HK", "银行识别码_hk", "银行识别码（SWIFT/BIC；CHAR 11）"),

            // entity.bank.group
            new TranslationSeedItem("entity.bank.group", "en-US", "银行组_us", "银行组（；CHAR 2）"),
            // entity.bank.group
            new TranslationSeedItem("entity.bank.group", "ja-JP", "银行组_jp", "银行组（；CHAR 2）"),
            // entity.bank.group
            new TranslationSeedItem("entity.bank.group", "zh-CN", "银行组", "银行组（；CHAR 2）"),
            // entity.bank.group
            new TranslationSeedItem("entity.bank.group", "zh-HK", "银行组_hk", "银行组（；CHAR 2）"),

            // entity.bank.pobkcurac
            new TranslationSeedItem("entity.bank.pobkcurac", "en-US", "邮政银行往来账户_us", "邮政银行往来账户（字典 sys_yes_no）"),
            // entity.bank.pobkcurac
            new TranslationSeedItem("entity.bank.pobkcurac", "ja-JP", "邮政银行往来账户_jp", "邮政银行往来账户（字典 sys_yes_no）"),
            // entity.bank.pobkcurac
            new TranslationSeedItem("entity.bank.pobkcurac", "zh-CN", "邮政银行往来账户", "邮政银行往来账户（字典 sys_yes_no）"),
            // entity.bank.pobkcurac
            new TranslationSeedItem("entity.bank.pobkcurac", "zh-HK", "邮政银行往来账户_hk", "邮政银行往来账户（字典 sys_yes_no）"),

            // entity.bank.number
            new TranslationSeedItem("entity.bank.number", "en-US", "银行编码_us", "银行编码（；CHAR 15）"),
            // entity.bank.number
            new TranslationSeedItem("entity.bank.number", "ja-JP", "银行编码_jp", "银行编码（；CHAR 15）"),
            // entity.bank.number
            new TranslationSeedItem("entity.bank.number", "zh-CN", "银行编码", "银行编码（；CHAR 15）"),
            // entity.bank.number
            new TranslationSeedItem("entity.bank.number", "zh-HK", "银行编码_hk", "银行编码（；CHAR 15）"),

            // entity.bank.postalbank
            new TranslationSeedItem("entity.bank.postalbank", "en-US", "邮政银行_us", "邮政银行（；CHAR 16）"),
            // entity.bank.postalbank
            new TranslationSeedItem("entity.bank.postalbank", "ja-JP", "邮政银行_jp", "邮政银行（；CHAR 16）"),
            // entity.bank.postalbank
            new TranslationSeedItem("entity.bank.postalbank", "zh-CN", "邮政银行", "邮政银行（；CHAR 16）"),
            // entity.bank.postalbank
            new TranslationSeedItem("entity.bank.postalbank", "zh-HK", "邮政银行_hk", "邮政银行（；CHAR 16）"),

            // entity.bank.addressnumber
            new TranslationSeedItem("entity.bank.addressnumber", "en-US", "地址号_us", "地址号（；CHAR 10）"),
            // entity.bank.addressnumber
            new TranslationSeedItem("entity.bank.addressnumber", "ja-JP", "地址号_jp", "地址号（；CHAR 10）"),
            // entity.bank.addressnumber
            new TranslationSeedItem("entity.bank.addressnumber", "zh-CN", "地址号", "地址号（；CHAR 10）"),
            // entity.bank.addressnumber
            new TranslationSeedItem("entity.bank.addressnumber", "zh-HK", "地址号_hk", "地址号（；CHAR 10）"),

            // entity.bank.branch
            new TranslationSeedItem("entity.bank.branch", "en-US", "分行_us", "分行（；CHAR 40）"),
            // entity.bank.branch
            new TranslationSeedItem("entity.bank.branch", "ja-JP", "分行_jp", "分行（；CHAR 40）"),
            // entity.bank.branch
            new TranslationSeedItem("entity.bank.branch", "zh-CN", "分行", "分行（；CHAR 40）"),
            // entity.bank.branch
            new TranslationSeedItem("entity.bank.branch", "zh-HK", "分行_hk", "分行（；CHAR 40）"),

            // entity.bank.method
            new TranslationSeedItem("entity.bank.method", "en-US", "方法_us", "方法（CHAR 4）"),
            // entity.bank.method
            new TranslationSeedItem("entity.bank.method", "ja-JP", "方法_jp", "方法（CHAR 4）"),
            // entity.bank.method
            new TranslationSeedItem("entity.bank.method", "zh-CN", "方法", "方法（CHAR 4）"),
            // entity.bank.method
            new TranslationSeedItem("entity.bank.method", "zh-HK", "方法_hk", "方法（CHAR 4）"),

            // entity.bank.format
            new TranslationSeedItem("entity.bank.format", "en-US", "格式_us", "格式（含银行数据文件的格式；CHAR 3）"),
            // entity.bank.format
            new TranslationSeedItem("entity.bank.format", "ja-JP", "格式_jp", "格式（含银行数据文件的格式；CHAR 3）"),
            // entity.bank.format
            new TranslationSeedItem("entity.bank.format", "zh-CN", "格式", "格式（含银行数据文件的格式；CHAR 3）"),
            // entity.bank.format
            new TranslationSeedItem("entity.bank.format", "zh-HK", "格式_hk", "格式（含银行数据文件的格式；CHAR 3）"),

            // entity.bank.ibanrule
            new TranslationSeedItem("entity.bank.ibanrule", "en-US", "IBAN规则_us", "IBAN 规则（CHAR 6）"),
            // entity.bank.ibanrule
            new TranslationSeedItem("entity.bank.ibanrule", "ja-JP", "IBAN规则_jp", "IBAN 规则（CHAR 6）"),
            // entity.bank.ibanrule
            new TranslationSeedItem("entity.bank.ibanrule", "zh-CN", "IBAN规则", "IBAN 规则（CHAR 6）"),
            // entity.bank.ibanrule
            new TranslationSeedItem("entity.bank.ibanrule", "zh-HK", "IBAN规则_hk", "IBAN 规则（CHAR 6）"),

            // entity.bank.sddb2b
            new TranslationSeedItem("entity.bank.sddb2b", "en-US", "企业间_us", "企业间（字典 sys_yes_no）"),
            // entity.bank.sddb2b
            new TranslationSeedItem("entity.bank.sddb2b", "ja-JP", "企业间_jp", "企业间（字典 sys_yes_no）"),
            // entity.bank.sddb2b
            new TranslationSeedItem("entity.bank.sddb2b", "zh-CN", "企业间", "企业间（字典 sys_yes_no）"),
            // entity.bank.sddb2b
            new TranslationSeedItem("entity.bank.sddb2b", "zh-HK", "企业间_hk", "企业间（字典 sys_yes_no）"),

            // entity.bank.sddcore
            new TranslationSeedItem("entity.bank.sddcore", "en-US", "核心个人_us", "核心个人（字典 sys_yes_no）"),
            // entity.bank.sddcore
            new TranslationSeedItem("entity.bank.sddcore", "ja-JP", "核心个人_jp", "核心个人（字典 sys_yes_no）"),
            // entity.bank.sddcore
            new TranslationSeedItem("entity.bank.sddcore", "zh-CN", "核心个人", "核心个人（字典 sys_yes_no）"),
            // entity.bank.sddcore
            new TranslationSeedItem("entity.bank.sddcore", "zh-HK", "核心个人_hk", "核心个人（字典 sys_yes_no）"),

            // entity.bank.sddrtrans
            new TranslationSeedItem("entity.bank.sddrtrans", "en-US", "SEPA拒付交易支持标识_us", "SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）"),
            // entity.bank.sddrtrans
            new TranslationSeedItem("entity.bank.sddrtrans", "ja-JP", "SEPA拒付交易支持标识_jp", "SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）"),
            // entity.bank.sddrtrans
            new TranslationSeedItem("entity.bank.sddrtrans", "zh-CN", "SEPA拒付交易支持标识", "SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）"),
            // entity.bank.sddrtrans
            new TranslationSeedItem("entity.bank.sddrtrans", "zh-HK", "SEPA拒付交易支持标识_hk", "SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）"),

            // entity.bank.bicplusnumber
            new TranslationSeedItem("entity.bank.bicplusnumber", "en-US", "BIC+编码_us", "BIC+ 编码（CHAR 12）"),
            // entity.bank.bicplusnumber
            new TranslationSeedItem("entity.bank.bicplusnumber", "ja-JP", "BIC+编码_jp", "BIC+ 编码（CHAR 12）"),
            // entity.bank.bicplusnumber
            new TranslationSeedItem("entity.bank.bicplusnumber", "zh-CN", "BIC+编码", "BIC+ 编码（CHAR 12）"),
            // entity.bank.bicplusnumber
            new TranslationSeedItem("entity.bank.bicplusnumber", "zh-HK", "BIC+编码_hk", "BIC+ 编码（CHAR 12）"),

            // entity.bank.pathcode
            new TranslationSeedItem("entity.bank.pathcode", "en-US", "路径代码_us", "路径代码（CHAR 15）"),
            // entity.bank.pathcode
            new TranslationSeedItem("entity.bank.pathcode", "ja-JP", "路径代码_jp", "路径代码（CHAR 15）"),
            // entity.bank.pathcode
            new TranslationSeedItem("entity.bank.pathcode", "zh-CN", "路径代码", "路径代码（CHAR 15）"),
            // entity.bank.pathcode
            new TranslationSeedItem("entity.bank.pathcode", "zh-HK", "路径代码_hk", "路径代码（CHAR 15）"),
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
