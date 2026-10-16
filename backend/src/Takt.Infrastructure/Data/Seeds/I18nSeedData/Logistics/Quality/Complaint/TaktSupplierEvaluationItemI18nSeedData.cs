// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplierEvaluationItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktSupplierEvaluationItem 实体国际化翻译种子（键前缀 entity.supplierevaluationitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierEvaluationItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplierEvaluationItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplierevaluationitem 实体翻译...", tenantCode);

        foreach (var item in GetSupplierEvaluationItemTranslations())
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

        TaktLogger.Information("TaktSupplierEvaluationItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplierEvaluationItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplierevaluationitem._self / entity.supplierevaluationitem.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierEvaluationItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplierevaluationitem._self
            new TranslationSeedItem("entity.supplierevaluationitem._self", "en-US", "Supplier Evaluation Item Information_us", "实体名称"),
            // entity.supplierevaluationitem._self
            new TranslationSeedItem("entity.supplierevaluationitem._self", "ja-JP", "供应商评价考核项目明细信息_jp", "实体名称"),
            // entity.supplierevaluationitem._self
            new TranslationSeedItem("entity.supplierevaluationitem._self", "zh-CN", "供应商评价考核项目明细信息", "实体名称"),
            // entity.supplierevaluationitem._self
            new TranslationSeedItem("entity.supplierevaluationitem._self", "zh-HK", "供应商评价考核项目明细信息_hk", "实体名称"),

            // entity.supplierevaluationitem.evaluationid
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationid", "en-US", "评价表ID_us", "评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）"),
            // entity.supplierevaluationitem.evaluationid
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationid", "ja-JP", "评价表ID_jp", "评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）"),
            // entity.supplierevaluationitem.evaluationid
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationid", "zh-CN", "评价表ID", "评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）"),
            // entity.supplierevaluationitem.evaluationid
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationid", "zh-HK", "评价表ID_hk", "评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）"),

            // entity.supplierevaluationitem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierevaluationitem.supplierevaluationcode", "en-US", "评价表编码_us", "评价表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.supplierevaluationitem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierevaluationitem.supplierevaluationcode", "ja-JP", "评价表编码_jp", "评价表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.supplierevaluationitem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierevaluationitem.supplierevaluationcode", "zh-CN", "评价表编码", "评价表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.supplierevaluationitem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierevaluationitem.supplierevaluationcode", "zh-HK", "评价表编码_hk", "评价表编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.supplierevaluationitem.linenumber
            new TranslationSeedItem("entity.supplierevaluationitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.supplierevaluationitem.linenumber
            new TranslationSeedItem("entity.supplierevaluationitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.supplierevaluationitem.linenumber
            new TranslationSeedItem("entity.supplierevaluationitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.supplierevaluationitem.linenumber
            new TranslationSeedItem("entity.supplierevaluationitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.supplierevaluationitem.categorytype
            new TranslationSeedItem("entity.supplierevaluationitem.categorytype", "en-US", "评价类别_us", "评价类别类型（字典 logistics_quality_evaluation_category）"),
            // entity.supplierevaluationitem.categorytype
            new TranslationSeedItem("entity.supplierevaluationitem.categorytype", "ja-JP", "评价类别_jp", "评价类别类型（字典 logistics_quality_evaluation_category）"),
            // entity.supplierevaluationitem.categorytype
            new TranslationSeedItem("entity.supplierevaluationitem.categorytype", "zh-CN", "评价类别", "评价类别类型（字典 logistics_quality_evaluation_category）"),
            // entity.supplierevaluationitem.categorytype
            new TranslationSeedItem("entity.supplierevaluationitem.categorytype", "zh-HK", "评价类别_hk", "评价类别类型（字典 logistics_quality_evaluation_category）"),

            // entity.supplierevaluationitem.itemname
            new TranslationSeedItem("entity.supplierevaluationitem.itemname", "en-US", "评价项目_us", "评价项目名称"),
            // entity.supplierevaluationitem.itemname
            new TranslationSeedItem("entity.supplierevaluationitem.itemname", "ja-JP", "评价项目_jp", "评价项目名称"),
            // entity.supplierevaluationitem.itemname
            new TranslationSeedItem("entity.supplierevaluationitem.itemname", "zh-CN", "评价项目", "评价项目名称"),
            // entity.supplierevaluationitem.itemname
            new TranslationSeedItem("entity.supplierevaluationitem.itemname", "zh-HK", "评价项目_hk", "评价项目名称"),

            // entity.supplierevaluationitem.itemdescription
            new TranslationSeedItem("entity.supplierevaluationitem.itemdescription", "en-US", "项目说明_us", "评价项目说明"),
            // entity.supplierevaluationitem.itemdescription
            new TranslationSeedItem("entity.supplierevaluationitem.itemdescription", "ja-JP", "项目说明_jp", "评价项目说明"),
            // entity.supplierevaluationitem.itemdescription
            new TranslationSeedItem("entity.supplierevaluationitem.itemdescription", "zh-CN", "项目说明", "评价项目说明"),
            // entity.supplierevaluationitem.itemdescription
            new TranslationSeedItem("entity.supplierevaluationitem.itemdescription", "zh-HK", "项目说明_hk", "评价项目说明"),

            // entity.supplierevaluationitem.weight
            new TranslationSeedItem("entity.supplierevaluationitem.weight", "en-US", "权重_us", "权重（%）"),
            // entity.supplierevaluationitem.weight
            new TranslationSeedItem("entity.supplierevaluationitem.weight", "ja-JP", "权重_jp", "权重（%）"),
            // entity.supplierevaluationitem.weight
            new TranslationSeedItem("entity.supplierevaluationitem.weight", "zh-CN", "权重", "权重（%）"),
            // entity.supplierevaluationitem.weight
            new TranslationSeedItem("entity.supplierevaluationitem.weight", "zh-HK", "权重_hk", "权重（%）"),

            // entity.supplierevaluationitem.scoringstandard
            new TranslationSeedItem("entity.supplierevaluationitem.scoringstandard", "en-US", "评分标准_us", "评分标准"),
            // entity.supplierevaluationitem.scoringstandard
            new TranslationSeedItem("entity.supplierevaluationitem.scoringstandard", "ja-JP", "评分标准_jp", "评分标准"),
            // entity.supplierevaluationitem.scoringstandard
            new TranslationSeedItem("entity.supplierevaluationitem.scoringstandard", "zh-CN", "评分标准", "评分标准"),
            // entity.supplierevaluationitem.scoringstandard
            new TranslationSeedItem("entity.supplierevaluationitem.scoringstandard", "zh-HK", "评分标准_hk", "评分标准"),

            // entity.supplierevaluationitem.score
            new TranslationSeedItem("entity.supplierevaluationitem.score", "en-US", "评分_us", "评分（0-100分）"),
            // entity.supplierevaluationitem.score
            new TranslationSeedItem("entity.supplierevaluationitem.score", "ja-JP", "评分_jp", "评分（0-100分）"),
            // entity.supplierevaluationitem.score
            new TranslationSeedItem("entity.supplierevaluationitem.score", "zh-CN", "评分", "评分（0-100分）"),
            // entity.supplierevaluationitem.score
            new TranslationSeedItem("entity.supplierevaluationitem.score", "zh-HK", "评分_hk", "评分（0-100分）"),

            // entity.supplierevaluationitem.ratinglevel
            new TranslationSeedItem("entity.supplierevaluationitem.ratinglevel", "en-US", "评级_us", "评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluationitem.ratinglevel
            new TranslationSeedItem("entity.supplierevaluationitem.ratinglevel", "ja-JP", "评级_jp", "评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluationitem.ratinglevel
            new TranslationSeedItem("entity.supplierevaluationitem.ratinglevel", "zh-CN", "评级", "评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluationitem.ratinglevel
            new TranslationSeedItem("entity.supplierevaluationitem.ratinglevel", "zh-HK", "评级_hk", "评级（字典 logistics_quality_supplier_rating）"),

            // entity.supplierevaluationitem.evaluationcomment
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationcomment", "en-US", "评价说明_us", "评价说明/事实依据"),
            // entity.supplierevaluationitem.evaluationcomment
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationcomment", "ja-JP", "评价说明_jp", "评价说明/事实依据"),
            // entity.supplierevaluationitem.evaluationcomment
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationcomment", "zh-CN", "评价说明", "评价说明/事实依据"),
            // entity.supplierevaluationitem.evaluationcomment
            new TranslationSeedItem("entity.supplierevaluationitem.evaluationcomment", "zh-HK", "评价说明_hk", "评价说明/事实依据"),

            // entity.supplierevaluationitem.existingissues
            new TranslationSeedItem("entity.supplierevaluationitem.existingissues", "en-US", "存在问题_us", "存在问题"),
            // entity.supplierevaluationitem.existingissues
            new TranslationSeedItem("entity.supplierevaluationitem.existingissues", "ja-JP", "存在问题_jp", "存在问题"),
            // entity.supplierevaluationitem.existingissues
            new TranslationSeedItem("entity.supplierevaluationitem.existingissues", "zh-CN", "存在问题", "存在问题"),
            // entity.supplierevaluationitem.existingissues
            new TranslationSeedItem("entity.supplierevaluationitem.existingissues", "zh-HK", "存在问题_hk", "存在问题"),

            // entity.supplierevaluationitem.improvementrequirement
            new TranslationSeedItem("entity.supplierevaluationitem.improvementrequirement", "en-US", "改进要求_us", "改进要求"),
            // entity.supplierevaluationitem.improvementrequirement
            new TranslationSeedItem("entity.supplierevaluationitem.improvementrequirement", "ja-JP", "改进要求_jp", "改进要求"),
            // entity.supplierevaluationitem.improvementrequirement
            new TranslationSeedItem("entity.supplierevaluationitem.improvementrequirement", "zh-CN", "改进要求", "改进要求"),
            // entity.supplierevaluationitem.improvementrequirement
            new TranslationSeedItem("entity.supplierevaluationitem.improvementrequirement", "zh-HK", "改进要求_hk", "改进要求"),

            // entity.supplierevaluationitem.rectificationrequired
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationrequired", "en-US", "整改要求_us", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierevaluationitem.rectificationrequired
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationrequired", "ja-JP", "整改要求_jp", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierevaluationitem.rectificationrequired
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationrequired", "zh-CN", "整改要求", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierevaluationitem.rectificationrequired
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationrequired", "zh-HK", "整改要求_hk", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),

            // entity.supplierevaluationitem.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationdeadline", "en-US", "整改期限_us", "整改期限"),
            // entity.supplierevaluationitem.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationdeadline", "ja-JP", "整改期限_jp", "整改期限"),
            // entity.supplierevaluationitem.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationdeadline", "zh-CN", "整改期限", "整改期限"),
            // entity.supplierevaluationitem.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationdeadline", "zh-HK", "整改期限_hk", "整改期限"),

            // entity.supplierevaluationitem.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationstatus", "en-US", "整改状态_us", "整改状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluationitem.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationstatus", "ja-JP", "整改状态_jp", "整改状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluationitem.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationstatus", "zh-CN", "整改状态", "整改状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluationitem.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluationitem.rectificationstatus", "zh-HK", "整改状态_hk", "整改状态（字典 logistics_quality_rectification_status）"),

            // entity.supplierevaluationitem.isobsolete
            new TranslationSeedItem("entity.supplierevaluationitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.supplierevaluationitem.isobsolete
            new TranslationSeedItem("entity.supplierevaluationitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.supplierevaluationitem.isobsolete
            new TranslationSeedItem("entity.supplierevaluationitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.supplierevaluationitem.isobsolete
            new TranslationSeedItem("entity.supplierevaluationitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.supplierevaluationitem.evaluation
            new TranslationSeedItem("entity.supplierevaluationitem.evaluation", "en-US", "评价表主表_us", "评价表主表"),
            // entity.supplierevaluationitem.evaluation
            new TranslationSeedItem("entity.supplierevaluationitem.evaluation", "ja-JP", "评价表主表_jp", "评价表主表"),
            // entity.supplierevaluationitem.evaluation
            new TranslationSeedItem("entity.supplierevaluationitem.evaluation", "zh-CN", "评价表主表", "评价表主表"),
            // entity.supplierevaluationitem.evaluation
            new TranslationSeedItem("entity.supplierevaluationitem.evaluation", "zh-HK", "评价表主表_hk", "评价表主表"),
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
        translation.ResourceGroup = "Complaint";
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
