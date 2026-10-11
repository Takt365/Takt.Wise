// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktNumberingI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNumbering 实体字段国际化种子（已对齐前端 locales：src/locales/foundation/numbering）
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
/// TaktNumbering 实体国际化翻译种子（键前缀 entity.numbering.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNumberingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNumbering 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 numbering 实体翻译...", tenantCode);

        foreach (var item in GetNumberingTranslations())
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

        TaktLogger.Information("TaktNumbering 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNumbering 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.numbering._self / entity.numbering.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNumberingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "en-US", "Numbering Information_us", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "ja-JP", "编码规则信息_jp", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "zh-CN", "编码规则信息", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "zh-HK", "编码规则信息_hk", "实体名称"),

            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "en-US", "规则编码_us", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "ja-JP", "规则编码_jp", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "zh-CN", "规则编码", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "zh-HK", "规则编码_hk", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),

            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "en-US", "规则名称_us", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "ja-JP", "规则名称_jp", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "zh-CN", "规则名称", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "zh-HK", "规则名称_hk", "规则名称（如：销售订单号、采购订单号）"),

            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "en-US", "单据类型_us", "单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name；存菜单名称非 Id）"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "ja-JP", "单据类型_jp", "单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name；存菜单名称非 Id）"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "zh-CN", "单据类型", "单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name；存菜单名称非 Id）"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "zh-HK", "单据类型_hk", "单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name；存菜单名称非 Id）"),

            // entity.numbering.deptcode
            new TranslationSeedItem("entity.numbering.deptcode", "en-US", "部门编码_us", "部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options；与部门简称一致，长度 6）"),
            // entity.numbering.deptcode
            new TranslationSeedItem("entity.numbering.deptcode", "ja-JP", "部门编码_jp", "部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options；与部门简称一致，长度 6）"),
            // entity.numbering.deptcode
            new TranslationSeedItem("entity.numbering.deptcode", "zh-CN", "部门编码", "部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options；与部门简称一致，长度 6）"),
            // entity.numbering.deptcode
            new TranslationSeedItem("entity.numbering.deptcode", "zh-HK", "部门编码_hk", "部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options；与部门简称一致，长度 6）"),

            // entity.numbering.prefixcode
            new TranslationSeedItem("entity.numbering.prefixcode", "en-US", "前缀编码_us", "前缀编码（如：PUR、SORD、ANN）"),
            // entity.numbering.prefixcode
            new TranslationSeedItem("entity.numbering.prefixcode", "ja-JP", "前缀编码_jp", "前缀编码（如：PUR、SORD、ANN）"),
            // entity.numbering.prefixcode
            new TranslationSeedItem("entity.numbering.prefixcode", "zh-CN", "前缀编码", "前缀编码（如：PUR、SORD、ANN）"),
            // entity.numbering.prefixcode
            new TranslationSeedItem("entity.numbering.prefixcode", "zh-HK", "前缀编码_hk", "前缀编码（如：PUR、SORD、ANN）"),

            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "en-US", "日期格式_us", "日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "ja-JP", "日期格式_jp", "日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "zh-CN", "日期格式", "日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "zh-HK", "日期格式_hk", "日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）"),

            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "en-US", "流水位数_us", "流水位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "ja-JP", "流水位数_jp", "流水位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "zh-CN", "流水位数", "流水位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "zh-HK", "流水位数_hk", "流水位数（3=001, 4=0001, 5=00001, 6=000001）"),

            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "en-US", "流水步长_us", "流水步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "ja-JP", "流水步长_jp", "流水步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "zh-CN", "流水步长", "流水步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "zh-HK", "流水步长_hk", "流水步长（每次递增的数值，默认1）"),

            // entity.numbering.suffixcode
            new TranslationSeedItem("entity.numbering.suffixcode", "en-US", "后缀编码_us", "后缀编码（可选，最多 4 位）"),
            // entity.numbering.suffixcode
            new TranslationSeedItem("entity.numbering.suffixcode", "ja-JP", "后缀编码_jp", "后缀编码（可选，最多 4 位）"),
            // entity.numbering.suffixcode
            new TranslationSeedItem("entity.numbering.suffixcode", "zh-CN", "后缀编码", "后缀编码（可选，最多 4 位）"),
            // entity.numbering.suffixcode
            new TranslationSeedItem("entity.numbering.suffixcode", "zh-HK", "后缀编码_hk", "后缀编码（可选，最多 4 位）"),

            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "en-US", "重置周期_us", "重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "ja-JP", "重置周期_jp", "重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "zh-CN", "重置周期", "重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "zh-HK", "重置周期_hk", "重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）"),

            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "en-US", "当前流水_us", "当前流水（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "ja-JP", "当前流水_jp", "当前流水（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "zh-CN", "当前流水", "当前流水（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "zh-HK", "当前流水_hk", "当前流水（用于记录下一个流水号值）"),

            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "en-US", "起始编码_us", "起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "ja-JP", "起始编码_jp", "起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "zh-CN", "起始编码", "起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "zh-HK", "起始编码_hk", "起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码"),

            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "en-US", "分隔符_us", "分隔符（空=段直接拼接；-=连字符分隔，默认 -）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "ja-JP", "分隔符_jp", "分隔符（空=段直接拼接；-=连字符分隔，默认 -）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "zh-CN", "分隔符", "分隔符（空=段直接拼接；-=连字符分隔，默认 -）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "zh-HK", "分隔符_hk", "分隔符（空=段直接拼接；-=连字符分隔，默认 -）"),

            // entity.numbering.isbuiltin
            new TranslationSeedItem("entity.numbering.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.numbering.isbuiltin
            new TranslationSeedItem("entity.numbering.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.numbering.isbuiltin
            new TranslationSeedItem("entity.numbering.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.numbering.isbuiltin
            new TranslationSeedItem("entity.numbering.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；0=否 1=是）"),

            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "en-US", "描述说明_us", "描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "ja-JP", "描述说明_jp", "描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "zh-CN", "描述说明", "描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "zh-HK", "描述说明_hk", "描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）"),

            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
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
