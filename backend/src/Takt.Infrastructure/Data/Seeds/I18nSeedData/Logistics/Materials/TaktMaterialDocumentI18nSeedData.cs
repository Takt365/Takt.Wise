// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialDocumentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialDocument 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterialDocument 实体国际化翻译种子（键前缀 entity.materialdocument.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialDocumentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialDocument 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialdocument 实体翻译...", tenantCode);

        foreach (var item in GetMaterialDocumentTranslations())
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

        TaktLogger.Information("TaktMaterialDocument 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialDocument 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialdocument._self / entity.materialdocument.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialDocumentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "en-US", "Material Document Information_us", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "ja-JP", "Takt物料凭证主表信息_jp", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "zh-CN", "Takt物料凭证主表信息", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "zh-HK", "Takt物料凭证主表信息_hk", "实体名称"),

            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "en-US", "物料凭证_us", "物料凭证（前端表单选择编码规则后自动通过 TaktNumbering 物料凭证规则生成并展示，非手输；单据类型菜单：物料凭证）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "ja-JP", "物料凭证_jp", "物料凭证（前端表单选择编码规则后自动通过 TaktNumbering 物料凭证规则生成并展示，非手输；单据类型菜单：物料凭证）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "zh-CN", "物料凭证", "物料凭证（前端表单选择编码规则后自动通过 TaktNumbering 物料凭证规则生成并展示，非手输；单据类型菜单：物料凭证）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "zh-HK", "物料凭证_hk", "物料凭证（前端表单选择编码规则后自动通过 TaktNumbering 物料凭证规则生成并展示，非手输；单据类型菜单：物料凭证）"),

            // entity.materialdocument.year
            new TranslationSeedItem("entity.materialdocument.year", "en-US", "物料凭证的年份_us", "物料凭证的年份"),
            // entity.materialdocument.year
            new TranslationSeedItem("entity.materialdocument.year", "ja-JP", "物料凭证的年份_jp", "物料凭证的年份"),
            // entity.materialdocument.year
            new TranslationSeedItem("entity.materialdocument.year", "zh-CN", "物料凭证的年份", "物料凭证的年份"),
            // entity.materialdocument.year
            new TranslationSeedItem("entity.materialdocument.year", "zh-HK", "物料凭证的年份_hk", "物料凭证的年份"),

            // entity.materialdocument.transactioneventtype
            new TranslationSeedItem("entity.materialdocument.transactioneventtype", "en-US", "交易/事件类型_us", "交易/事件类型（字典 logistics_material_document_transaction_event_type）"),
            // entity.materialdocument.transactioneventtype
            new TranslationSeedItem("entity.materialdocument.transactioneventtype", "ja-JP", "交易/事件类型_jp", "交易/事件类型（字典 logistics_material_document_transaction_event_type）"),
            // entity.materialdocument.transactioneventtype
            new TranslationSeedItem("entity.materialdocument.transactioneventtype", "zh-CN", "交易/事件类型", "交易/事件类型（字典 logistics_material_document_transaction_event_type）"),
            // entity.materialdocument.transactioneventtype
            new TranslationSeedItem("entity.materialdocument.transactioneventtype", "zh-HK", "交易/事件类型_hk", "交易/事件类型（字典 logistics_material_document_transaction_event_type）"),

            // entity.materialdocument.documenttype
            new TranslationSeedItem("entity.materialdocument.documenttype", "en-US", "凭证类型_us", "凭证类型（字典 logistics_material_document_type）"),
            // entity.materialdocument.documenttype
            new TranslationSeedItem("entity.materialdocument.documenttype", "ja-JP", "凭证类型_jp", "凭证类型（字典 logistics_material_document_type）"),
            // entity.materialdocument.documenttype
            new TranslationSeedItem("entity.materialdocument.documenttype", "zh-CN", "凭证类型", "凭证类型（字典 logistics_material_document_type）"),
            // entity.materialdocument.documenttype
            new TranslationSeedItem("entity.materialdocument.documenttype", "zh-HK", "凭证类型_hk", "凭证类型（字典 logistics_material_document_type）"),

            // entity.materialdocument.revaluationtype
            new TranslationSeedItem("entity.materialdocument.revaluationtype", "en-US", "凭证类型重新评估_us", "凭证类型重新评估"),
            // entity.materialdocument.revaluationtype
            new TranslationSeedItem("entity.materialdocument.revaluationtype", "ja-JP", "凭证类型重新评估_jp", "凭证类型重新评估"),
            // entity.materialdocument.revaluationtype
            new TranslationSeedItem("entity.materialdocument.revaluationtype", "zh-CN", "凭证类型重新评估", "凭证类型重新评估"),
            // entity.materialdocument.revaluationtype
            new TranslationSeedItem("entity.materialdocument.revaluationtype", "zh-HK", "凭证类型重新评估_hk", "凭证类型重新评估"),

            // entity.materialdocument.documentdate
            new TranslationSeedItem("entity.materialdocument.documentdate", "en-US", "凭证日期_us", "凭证日期"),
            // entity.materialdocument.documentdate
            new TranslationSeedItem("entity.materialdocument.documentdate", "ja-JP", "凭证日期_jp", "凭证日期"),
            // entity.materialdocument.documentdate
            new TranslationSeedItem("entity.materialdocument.documentdate", "zh-CN", "凭证日期", "凭证日期"),
            // entity.materialdocument.documentdate
            new TranslationSeedItem("entity.materialdocument.documentdate", "zh-HK", "凭证日期_hk", "凭证日期"),

            // entity.materialdocument.postingdate
            new TranslationSeedItem("entity.materialdocument.postingdate", "en-US", "过帐日期_us", "过帐日期"),
            // entity.materialdocument.postingdate
            new TranslationSeedItem("entity.materialdocument.postingdate", "ja-JP", "过帐日期_jp", "过帐日期"),
            // entity.materialdocument.postingdate
            new TranslationSeedItem("entity.materialdocument.postingdate", "zh-CN", "过帐日期", "过帐日期"),
            // entity.materialdocument.postingdate
            new TranslationSeedItem("entity.materialdocument.postingdate", "zh-HK", "过帐日期_hk", "过帐日期"),

            // entity.materialdocument.referencecode
            new TranslationSeedItem("entity.materialdocument.referencecode", "en-US", "参照_us", "参照（最长 16，故 Length=16）"),
            // entity.materialdocument.referencecode
            new TranslationSeedItem("entity.materialdocument.referencecode", "ja-JP", "参照_jp", "参照（最长 16，故 Length=16）"),
            // entity.materialdocument.referencecode
            new TranslationSeedItem("entity.materialdocument.referencecode", "zh-CN", "参照", "参照（最长 16，故 Length=16）"),
            // entity.materialdocument.referencecode
            new TranslationSeedItem("entity.materialdocument.referencecode", "zh-HK", "参照_hk", "参照（最长 16，故 Length=16）"),

            // entity.materialdocument.headertext
            new TranslationSeedItem("entity.materialdocument.headertext", "en-US", "凭证抬头文本_us", "凭证抬头文本（最长 25，故 Length=25）"),
            // entity.materialdocument.headertext
            new TranslationSeedItem("entity.materialdocument.headertext", "ja-JP", "凭证抬头文本_jp", "凭证抬头文本（最长 25，故 Length=25）"),
            // entity.materialdocument.headertext
            new TranslationSeedItem("entity.materialdocument.headertext", "zh-CN", "凭证抬头文本", "凭证抬头文本（最长 25，故 Length=25）"),
            // entity.materialdocument.headertext
            new TranslationSeedItem("entity.materialdocument.headertext", "zh-HK", "凭证抬头文本_hk", "凭证抬头文本（最长 25，故 Length=25）"),

            // entity.materialdocument.billofladingcode
            new TranslationSeedItem("entity.materialdocument.billofladingcode", "en-US", "提货单_us", "提货单（最长 16，故 Length=16）"),
            // entity.materialdocument.billofladingcode
            new TranslationSeedItem("entity.materialdocument.billofladingcode", "ja-JP", "提货单_jp", "提货单（最长 16，故 Length=16）"),
            // entity.materialdocument.billofladingcode
            new TranslationSeedItem("entity.materialdocument.billofladingcode", "zh-CN", "提货单", "提货单（最长 16，故 Length=16）"),
            // entity.materialdocument.billofladingcode
            new TranslationSeedItem("entity.materialdocument.billofladingcode", "zh-HK", "提货单_hk", "提货单（最长 16，故 Length=16）"),

            // entity.materialdocument.deliverycode
            new TranslationSeedItem("entity.materialdocument.deliverycode", "en-US", "交货单_us", "交货单"),
            // entity.materialdocument.deliverycode
            new TranslationSeedItem("entity.materialdocument.deliverycode", "ja-JP", "交货单_jp", "交货单"),
            // entity.materialdocument.deliverycode
            new TranslationSeedItem("entity.materialdocument.deliverycode", "zh-CN", "交货单", "交货单"),
            // entity.materialdocument.deliverycode
            new TranslationSeedItem("entity.materialdocument.deliverycode", "zh-HK", "交货单_hk", "交货单"),

            // entity.materialdocument.transactioncode
            new TranslationSeedItem("entity.materialdocument.transactioncode", "en-US", "事务代码_us", "事务代码"),
            // entity.materialdocument.transactioncode
            new TranslationSeedItem("entity.materialdocument.transactioncode", "ja-JP", "事务代码_jp", "事务代码"),
            // entity.materialdocument.transactioncode
            new TranslationSeedItem("entity.materialdocument.transactioncode", "zh-CN", "事务代码", "事务代码"),
            // entity.materialdocument.transactioncode
            new TranslationSeedItem("entity.materialdocument.transactioncode", "zh-HK", "事务代码_hk", "事务代码"),

            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "en-US", "过账人_us", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "ja-JP", "过账人_jp", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "zh-CN", "过账人", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "zh-HK", "过账人_hk", "过账人（当前登录用户对应的 EmployeeCode）"),

            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "en-US", "物料凭证行项目列表_us", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "ja-JP", "物料凭证行项目列表_jp", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "zh-CN", "物料凭证行项目列表", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "zh-HK", "物料凭证行项目列表_hk", "物料凭证行项目列表（主子表关系）"),
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
        translation.ResourceGroup = "Materials";
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
