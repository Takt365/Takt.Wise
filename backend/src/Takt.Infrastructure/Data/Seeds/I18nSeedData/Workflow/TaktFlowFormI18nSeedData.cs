// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowFormI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowForm 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow;

/// <summary>
/// TaktFlowForm 实体国际化翻译种子（键前缀 entity.flowform.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowFormI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowForm 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowform 实体翻译...", tenantCode);

        foreach (var item in GetFlowFormTranslations())
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

        TaktLogger.Information("TaktFlowForm 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowForm 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowform._self / entity.flowform.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowFormTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowform._self
            new TranslationSeedItem("entity.flowform._self", "en-US", "Flow Form Information_us", "实体名称"),
            // entity.flowform._self
            new TranslationSeedItem("entity.flowform._self", "ja-JP", "流程表单定义信息_jp", "实体名称"),
            // entity.flowform._self
            new TranslationSeedItem("entity.flowform._self", "zh-CN", "流程表单定义信息", "实体名称"),
            // entity.flowform._self
            new TranslationSeedItem("entity.flowform._self", "zh-HK", "流程表单定义信息_hk", "实体名称"),

            // entity.flowform.formcode
            new TranslationSeedItem("entity.flowform.formcode", "en-US", "表单编码_us", "表单编码（租户+公司内唯一；前端表单按表单分类选择编码规则后自动通过 TaktNumbering 表单编码规则生成并展示，非手输；单据类型菜单：表单管理）"),
            // entity.flowform.formcode
            new TranslationSeedItem("entity.flowform.formcode", "ja-JP", "表单编码_jp", "表单编码（租户+公司内唯一；前端表单按表单分类选择编码规则后自动通过 TaktNumbering 表单编码规则生成并展示，非手输；单据类型菜单：表单管理）"),
            // entity.flowform.formcode
            new TranslationSeedItem("entity.flowform.formcode", "zh-CN", "表单编码", "表单编码（租户+公司内唯一；前端表单按表单分类选择编码规则后自动通过 TaktNumbering 表单编码规则生成并展示，非手输；单据类型菜单：表单管理）"),
            // entity.flowform.formcode
            new TranslationSeedItem("entity.flowform.formcode", "zh-HK", "表单编码_hk", "表单编码（租户+公司内唯一；前端表单按表单分类选择编码规则后自动通过 TaktNumbering 表单编码规则生成并展示，非手输；单据类型菜单：表单管理）"),

            // entity.flowform.formname
            new TranslationSeedItem("entity.flowform.formname", "en-US", "表单名称_us", "表单名称"),
            // entity.flowform.formname
            new TranslationSeedItem("entity.flowform.formname", "ja-JP", "表单名称_jp", "表单名称"),
            // entity.flowform.formname
            new TranslationSeedItem("entity.flowform.formname", "zh-CN", "表单名称", "表单名称"),
            // entity.flowform.formname
            new TranslationSeedItem("entity.flowform.formname", "zh-HK", "表单名称_hk", "表单名称"),

            // entity.flowform.formcategory
            new TranslationSeedItem("entity.flowform.formcategory", "en-US", "表单分类_us", "表单分类（字典 sys_form_category）"),
            // entity.flowform.formcategory
            new TranslationSeedItem("entity.flowform.formcategory", "ja-JP", "表单分类_jp", "表单分类（字典 sys_form_category）"),
            // entity.flowform.formcategory
            new TranslationSeedItem("entity.flowform.formcategory", "zh-CN", "表单分类", "表单分类（字典 sys_form_category）"),
            // entity.flowform.formcategory
            new TranslationSeedItem("entity.flowform.formcategory", "zh-HK", "表单分类_hk", "表单分类（字典 sys_form_category）"),

            // entity.flowform.formtype
            new TranslationSeedItem("entity.flowform.formtype", "en-US", "表单类型_us", "表单类型（字典 sys_form_type）"),
            // entity.flowform.formtype
            new TranslationSeedItem("entity.flowform.formtype", "ja-JP", "表单类型_jp", "表单类型（字典 sys_form_type）"),
            // entity.flowform.formtype
            new TranslationSeedItem("entity.flowform.formtype", "zh-CN", "表单类型", "表单类型（字典 sys_form_type）"),
            // entity.flowform.formtype
            new TranslationSeedItem("entity.flowform.formtype", "zh-HK", "表单类型_hk", "表单类型（字典 sys_form_type）"),

            // entity.flowform.formconfig
            new TranslationSeedItem("entity.flowform.formconfig", "en-US", "表单配置_us", "表单设计 JSON"),
            // entity.flowform.formconfig
            new TranslationSeedItem("entity.flowform.formconfig", "ja-JP", "表单配置_jp", "表单设计 JSON"),
            // entity.flowform.formconfig
            new TranslationSeedItem("entity.flowform.formconfig", "zh-CN", "表单配置", "表单设计 JSON"),
            // entity.flowform.formconfig
            new TranslationSeedItem("entity.flowform.formconfig", "zh-HK", "表单配置_hk", "表单设计 JSON"),

            // entity.flowform.formtemplate
            new TranslationSeedItem("entity.flowform.formtemplate", "en-US", "表单模板_us", "表单模板 JSON"),
            // entity.flowform.formtemplate
            new TranslationSeedItem("entity.flowform.formtemplate", "ja-JP", "表单模板_jp", "表单模板 JSON"),
            // entity.flowform.formtemplate
            new TranslationSeedItem("entity.flowform.formtemplate", "zh-CN", "表单模板", "表单模板 JSON"),
            // entity.flowform.formtemplate
            new TranslationSeedItem("entity.flowform.formtemplate", "zh-HK", "表单模板_hk", "表单模板 JSON"),

            // entity.flowform.formversion
            new TranslationSeedItem("entity.flowform.formversion", "en-US", "表单版本_us", "表单版本标签"),
            // entity.flowform.formversion
            new TranslationSeedItem("entity.flowform.formversion", "ja-JP", "表单版本_jp", "表单版本标签"),
            // entity.flowform.formversion
            new TranslationSeedItem("entity.flowform.formversion", "zh-CN", "表单版本", "表单版本标签"),
            // entity.flowform.formversion
            new TranslationSeedItem("entity.flowform.formversion", "zh-HK", "表单版本_hk", "表单版本标签"),

            // entity.flowform.isdatasource
            new TranslationSeedItem("entity.flowform.isdatasource", "en-US", "数据源表单_us", "是否绑定数据源（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowform.isdatasource
            new TranslationSeedItem("entity.flowform.isdatasource", "ja-JP", "数据源表单_jp", "是否绑定数据源（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowform.isdatasource
            new TranslationSeedItem("entity.flowform.isdatasource", "zh-CN", "数据源表单", "是否绑定数据源（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowform.isdatasource
            new TranslationSeedItem("entity.flowform.isdatasource", "zh-HK", "数据源表单_hk", "是否绑定数据源（字典 sys_yes_no；0=否 1=是）"),

            // entity.flowform.relateddatabasename
            new TranslationSeedItem("entity.flowform.relateddatabasename", "en-US", "关联库名_us", "关联库名（选项 TaktDatabaseInfos/list；DictValue=TenantCode）"),
            // entity.flowform.relateddatabasename
            new TranslationSeedItem("entity.flowform.relateddatabasename", "ja-JP", "关联库名_jp", "关联库名（选项 TaktDatabaseInfos/list；DictValue=TenantCode）"),
            // entity.flowform.relateddatabasename
            new TranslationSeedItem("entity.flowform.relateddatabasename", "zh-CN", "关联库名", "关联库名（选项 TaktDatabaseInfos/list；DictValue=TenantCode）"),
            // entity.flowform.relateddatabasename
            new TranslationSeedItem("entity.flowform.relateddatabasename", "zh-HK", "关联库名_hk", "关联库名（选项 TaktDatabaseInfos/list；DictValue=TenantCode）"),

            // entity.flowform.relatedtablename
            new TranslationSeedItem("entity.flowform.relatedtablename", "en-US", "关联表名_us", "关联表名（选项 TaktDatabaseInfos/tables；DictValue=TableName）"),
            // entity.flowform.relatedtablename
            new TranslationSeedItem("entity.flowform.relatedtablename", "ja-JP", "关联表名_jp", "关联表名（选项 TaktDatabaseInfos/tables；DictValue=TableName）"),
            // entity.flowform.relatedtablename
            new TranslationSeedItem("entity.flowform.relatedtablename", "zh-CN", "关联表名", "关联表名（选项 TaktDatabaseInfos/tables；DictValue=TableName）"),
            // entity.flowform.relatedtablename
            new TranslationSeedItem("entity.flowform.relatedtablename", "zh-HK", "关联表名_hk", "关联表名（选项 TaktDatabaseInfos/tables；DictValue=TableName）"),

            // entity.flowform.relatedformfield
            new TranslationSeedItem("entity.flowform.relatedformfield", "en-US", "关联映射_us", "关联映射 JSON"),
            // entity.flowform.relatedformfield
            new TranslationSeedItem("entity.flowform.relatedformfield", "ja-JP", "关联映射_jp", "关联映射 JSON"),
            // entity.flowform.relatedformfield
            new TranslationSeedItem("entity.flowform.relatedformfield", "zh-CN", "关联映射", "关联映射 JSON"),
            // entity.flowform.relatedformfield
            new TranslationSeedItem("entity.flowform.relatedformfield", "zh-HK", "关联映射_hk", "关联映射 JSON"),

            // entity.flowform.sortorder
            new TranslationSeedItem("entity.flowform.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.flowform.sortorder
            new TranslationSeedItem("entity.flowform.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.flowform.sortorder
            new TranslationSeedItem("entity.flowform.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.flowform.sortorder
            new TranslationSeedItem("entity.flowform.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.flowform.formstatus
            new TranslationSeedItem("entity.flowform.formstatus", "en-US", "表单状态_us", "表单状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowform.formstatus
            new TranslationSeedItem("entity.flowform.formstatus", "ja-JP", "表单状态_jp", "表单状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowform.formstatus
            new TranslationSeedItem("entity.flowform.formstatus", "zh-CN", "表单状态", "表单状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowform.formstatus
            new TranslationSeedItem("entity.flowform.formstatus", "zh-HK", "表单状态_hk", "表单状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
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
        translation.ResourceGroup = "Workflow";
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
