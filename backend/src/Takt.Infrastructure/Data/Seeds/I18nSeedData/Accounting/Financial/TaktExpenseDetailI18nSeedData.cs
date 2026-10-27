// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktExpenseDetailI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktExpenseDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktExpenseDetail 实体国际化翻译种子（键前缀 entity.expensedetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktExpenseDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktExpenseDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 expensedetail 实体翻译...", tenantCode);

        foreach (var item in GetExpenseDetailTranslations())
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

        TaktLogger.Information("TaktExpenseDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktExpenseDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.expensedetail._self / entity.expensedetail.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetExpenseDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.expensedetail._self
            new TranslationSeedItem("entity.expensedetail._self", "en-US", "Expense Detail Information_us", "实体名称"),
            // entity.expensedetail._self
            new TranslationSeedItem("entity.expensedetail._self", "ja-JP", "费用单明细信息_jp", "实体名称"),
            // entity.expensedetail._self
            new TranslationSeedItem("entity.expensedetail._self", "zh-CN", "费用单明细信息", "实体名称"),
            // entity.expensedetail._self
            new TranslationSeedItem("entity.expensedetail._self", "zh-HK", "费用单明细信息_hk", "实体名称"),

            // entity.expensedetail.expenseid
            new TranslationSeedItem("entity.expensedetail.expenseid", "en-US", "费用单ID_us", "费用单 ID（主子表关系）"),
            // entity.expensedetail.expenseid
            new TranslationSeedItem("entity.expensedetail.expenseid", "ja-JP", "费用单ID_jp", "费用单 ID（主子表关系）"),
            // entity.expensedetail.expenseid
            new TranslationSeedItem("entity.expensedetail.expenseid", "zh-CN", "费用单ID", "费用单 ID（主子表关系）"),
            // entity.expensedetail.expenseid
            new TranslationSeedItem("entity.expensedetail.expenseid", "zh-HK", "费用单ID_hk", "费用单 ID（主子表关系）"),

            // entity.expensedetail.expensecode
            new TranslationSeedItem("entity.expensedetail.expensecode", "en-US", "费用单编码_us", "费用单编码（冗余，便于查询）"),
            // entity.expensedetail.expensecode
            new TranslationSeedItem("entity.expensedetail.expensecode", "ja-JP", "费用单编码_jp", "费用单编码（冗余，便于查询）"),
            // entity.expensedetail.expensecode
            new TranslationSeedItem("entity.expensedetail.expensecode", "zh-CN", "费用单编码", "费用单编码（冗余，便于查询）"),
            // entity.expensedetail.expensecode
            new TranslationSeedItem("entity.expensedetail.expensecode", "zh-HK", "费用单编码_hk", "费用单编码（冗余，便于查询）"),

            // entity.expensedetail.linenumber
            new TranslationSeedItem("entity.expensedetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.expensedetail.linenumber
            new TranslationSeedItem("entity.expensedetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.expensedetail.linenumber
            new TranslationSeedItem("entity.expensedetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.expensedetail.linenumber
            new TranslationSeedItem("entity.expensedetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.expensedetail.allocationcategory
            new TranslationSeedItem("entity.expensedetail.allocationcategory", "en-US", "分配类别_us", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.expensedetail.allocationcategory
            new TranslationSeedItem("entity.expensedetail.allocationcategory", "ja-JP", "分配类别_jp", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.expensedetail.allocationcategory
            new TranslationSeedItem("entity.expensedetail.allocationcategory", "zh-CN", "分配类别", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.expensedetail.allocationcategory
            new TranslationSeedItem("entity.expensedetail.allocationcategory", "zh-HK", "分配类别_hk", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),

            // entity.expensedetail.itemname
            new TranslationSeedItem("entity.expensedetail.itemname", "en-US", "明细项名称_us", "明细项名称"),
            // entity.expensedetail.itemname
            new TranslationSeedItem("entity.expensedetail.itemname", "ja-JP", "明细项名称_jp", "明细项名称"),
            // entity.expensedetail.itemname
            new TranslationSeedItem("entity.expensedetail.itemname", "zh-CN", "明细项名称", "明细项名称"),
            // entity.expensedetail.itemname
            new TranslationSeedItem("entity.expensedetail.itemname", "zh-HK", "明细项名称_hk", "明细项名称"),

            // entity.expensedetail.itemdescription
            new TranslationSeedItem("entity.expensedetail.itemdescription", "en-US", "明细项说明_us", "明细项说明"),
            // entity.expensedetail.itemdescription
            new TranslationSeedItem("entity.expensedetail.itemdescription", "ja-JP", "明细项说明_jp", "明细项说明"),
            // entity.expensedetail.itemdescription
            new TranslationSeedItem("entity.expensedetail.itemdescription", "zh-CN", "明细项说明", "明细项说明"),
            // entity.expensedetail.itemdescription
            new TranslationSeedItem("entity.expensedetail.itemdescription", "zh-HK", "明细项说明_hk", "明细项说明"),

            // entity.expensedetail.itemquantity
            new TranslationSeedItem("entity.expensedetail.itemquantity", "en-US", "数量_us", "数量"),
            // entity.expensedetail.itemquantity
            new TranslationSeedItem("entity.expensedetail.itemquantity", "ja-JP", "数量_jp", "数量"),
            // entity.expensedetail.itemquantity
            new TranslationSeedItem("entity.expensedetail.itemquantity", "zh-CN", "数量", "数量"),
            // entity.expensedetail.itemquantity
            new TranslationSeedItem("entity.expensedetail.itemquantity", "zh-HK", "数量_hk", "数量"),

            // entity.expensedetail.itemamount
            new TranslationSeedItem("entity.expensedetail.itemamount", "en-US", "金额_us", "金额"),
            // entity.expensedetail.itemamount
            new TranslationSeedItem("entity.expensedetail.itemamount", "ja-JP", "金额_jp", "金额"),
            // entity.expensedetail.itemamount
            new TranslationSeedItem("entity.expensedetail.itemamount", "zh-CN", "金额", "金额"),
            // entity.expensedetail.itemamount
            new TranslationSeedItem("entity.expensedetail.itemamount", "zh-HK", "金额_hk", "金额"),

            // entity.expensedetail.accounttitle
            new TranslationSeedItem("entity.expensedetail.accounttitle", "en-US", "会计科目_us", "会计科目（选项 TaktAccountTitles/options；DictValue=Id）"),
            // entity.expensedetail.accounttitle
            new TranslationSeedItem("entity.expensedetail.accounttitle", "ja-JP", "会计科目_jp", "会计科目（选项 TaktAccountTitles/options；DictValue=Id）"),
            // entity.expensedetail.accounttitle
            new TranslationSeedItem("entity.expensedetail.accounttitle", "zh-CN", "会计科目", "会计科目（选项 TaktAccountTitles/options；DictValue=Id）"),
            // entity.expensedetail.accounttitle
            new TranslationSeedItem("entity.expensedetail.accounttitle", "zh-HK", "会计科目_hk", "会计科目（选项 TaktAccountTitles/options；DictValue=Id）"),

            // entity.expensedetail.invoicecode
            new TranslationSeedItem("entity.expensedetail.invoicecode", "en-US", "发票号码_us", "发票号码"),
            // entity.expensedetail.invoicecode
            new TranslationSeedItem("entity.expensedetail.invoicecode", "ja-JP", "发票号码_jp", "发票号码"),
            // entity.expensedetail.invoicecode
            new TranslationSeedItem("entity.expensedetail.invoicecode", "zh-CN", "发票号码", "发票号码"),
            // entity.expensedetail.invoicecode
            new TranslationSeedItem("entity.expensedetail.invoicecode", "zh-HK", "发票号码_hk", "发票号码"),

            // entity.expensedetail.date
            new TranslationSeedItem("entity.expensedetail.date", "en-US", "费用发生日期_us", "费用发生日期"),
            // entity.expensedetail.date
            new TranslationSeedItem("entity.expensedetail.date", "ja-JP", "费用发生日期_jp", "费用发生日期"),
            // entity.expensedetail.date
            new TranslationSeedItem("entity.expensedetail.date", "zh-CN", "费用发生日期", "费用发生日期"),
            // entity.expensedetail.date
            new TranslationSeedItem("entity.expensedetail.date", "zh-HK", "费用发生日期_hk", "费用发生日期"),

            // entity.expensedetail.isobsolete
            new TranslationSeedItem("entity.expensedetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.expensedetail.isobsolete
            new TranslationSeedItem("entity.expensedetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.expensedetail.isobsolete
            new TranslationSeedItem("entity.expensedetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.expensedetail.isobsolete
            new TranslationSeedItem("entity.expensedetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
