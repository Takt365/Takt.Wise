// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowSchemeI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowScheme 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowScheme 实体国际化翻译种子（键前缀 entity.flowscheme.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowSchemeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowScheme 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowscheme 实体翻译...", tenantCode);

        foreach (var item in GetFlowSchemeTranslations())
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

        TaktLogger.Information("TaktFlowScheme 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowScheme 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowscheme._self / entity.flowscheme.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowSchemeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowscheme._self
            new TranslationSeedItem("entity.flowscheme._self", "en-US", "Flow Scheme Information_us", "实体名称"),
            // entity.flowscheme._self
            new TranslationSeedItem("entity.flowscheme._self", "ja-JP", "流程定义信息_jp", "实体名称"),
            // entity.flowscheme._self
            new TranslationSeedItem("entity.flowscheme._self", "zh-CN", "流程定义信息", "实体名称"),
            // entity.flowscheme._self
            new TranslationSeedItem("entity.flowscheme._self", "zh-HK", "流程定义信息_hk", "实体名称"),

            // entity.flowscheme.processkey
            new TranslationSeedItem("entity.flowscheme.processkey", "en-US", "流程键_us", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowscheme.processkey
            new TranslationSeedItem("entity.flowscheme.processkey", "ja-JP", "流程键_jp", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowscheme.processkey
            new TranslationSeedItem("entity.flowscheme.processkey", "zh-CN", "流程键", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowscheme.processkey
            new TranslationSeedItem("entity.flowscheme.processkey", "zh-HK", "流程键_hk", "流程键（公司内业务唯一标识，如 leave）"),

            // entity.flowscheme.processname
            new TranslationSeedItem("entity.flowscheme.processname", "en-US", "流程名称_us", "流程名称"),
            // entity.flowscheme.processname
            new TranslationSeedItem("entity.flowscheme.processname", "ja-JP", "流程名称_jp", "流程名称"),
            // entity.flowscheme.processname
            new TranslationSeedItem("entity.flowscheme.processname", "zh-CN", "流程名称", "流程名称"),
            // entity.flowscheme.processname
            new TranslationSeedItem("entity.flowscheme.processname", "zh-HK", "流程名称_hk", "流程名称"),

            // entity.flowscheme.definitionversion
            new TranslationSeedItem("entity.flowscheme.definitionversion", "en-US", "定义版本号_us", "定义版本号（同流程键可多版本）"),
            // entity.flowscheme.definitionversion
            new TranslationSeedItem("entity.flowscheme.definitionversion", "ja-JP", "定义版本号_jp", "定义版本号（同流程键可多版本）"),
            // entity.flowscheme.definitionversion
            new TranslationSeedItem("entity.flowscheme.definitionversion", "zh-CN", "定义版本号", "定义版本号（同流程键可多版本）"),
            // entity.flowscheme.definitionversion
            new TranslationSeedItem("entity.flowscheme.definitionversion", "zh-HK", "定义版本号_hk", "定义版本号（同流程键可多版本）"),

            // entity.flowscheme.processversion
            new TranslationSeedItem("entity.flowscheme.processversion", "en-US", "版本标签_us", "版本标签（如 v1.0.0）"),
            // entity.flowscheme.processversion
            new TranslationSeedItem("entity.flowscheme.processversion", "ja-JP", "版本标签_jp", "版本标签（如 v1.0.0）"),
            // entity.flowscheme.processversion
            new TranslationSeedItem("entity.flowscheme.processversion", "zh-CN", "版本标签", "版本标签（如 v1.0.0）"),
            // entity.flowscheme.processversion
            new TranslationSeedItem("entity.flowscheme.processversion", "zh-HK", "版本标签_hk", "版本标签（如 v1.0.0）"),

            // entity.flowscheme.islatest
            new TranslationSeedItem("entity.flowscheme.islatest", "en-US", "是否最新版_us", "是否当前最新版（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowscheme.islatest
            new TranslationSeedItem("entity.flowscheme.islatest", "ja-JP", "是否最新版_jp", "是否当前最新版（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowscheme.islatest
            new TranslationSeedItem("entity.flowscheme.islatest", "zh-CN", "是否最新版", "是否当前最新版（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowscheme.islatest
            new TranslationSeedItem("entity.flowscheme.islatest", "zh-HK", "是否最新版_hk", "是否当前最新版（字典 sys_yes_no；0=否 1=是）"),

            // entity.flowscheme.processcategory
            new TranslationSeedItem("entity.flowscheme.processcategory", "en-US", "流程分类_us", "流程分类（字典 sys_flow_category；0=通用流程 1=业务流程 2=系统流程）"),
            // entity.flowscheme.processcategory
            new TranslationSeedItem("entity.flowscheme.processcategory", "ja-JP", "流程分类_jp", "流程分类（字典 sys_flow_category；0=通用流程 1=业务流程 2=系统流程）"),
            // entity.flowscheme.processcategory
            new TranslationSeedItem("entity.flowscheme.processcategory", "zh-CN", "流程分类", "流程分类（字典 sys_flow_category；0=通用流程 1=业务流程 2=系统流程）"),
            // entity.flowscheme.processcategory
            new TranslationSeedItem("entity.flowscheme.processcategory", "zh-HK", "流程分类_hk", "流程分类（字典 sys_flow_category；0=通用流程 1=业务流程 2=系统流程）"),

            // entity.flowscheme.processdescription
            new TranslationSeedItem("entity.flowscheme.processdescription", "en-US", "流程说明_us", "流程说明"),
            // entity.flowscheme.processdescription
            new TranslationSeedItem("entity.flowscheme.processdescription", "ja-JP", "流程说明_jp", "流程说明"),
            // entity.flowscheme.processdescription
            new TranslationSeedItem("entity.flowscheme.processdescription", "zh-CN", "流程说明", "流程说明"),
            // entity.flowscheme.processdescription
            new TranslationSeedItem("entity.flowscheme.processdescription", "zh-HK", "流程说明_hk", "流程说明"),

            // entity.flowscheme.suspensionstate
            new TranslationSeedItem("entity.flowscheme.suspensionstate", "en-US", "挂起状态_us", "挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）"),
            // entity.flowscheme.suspensionstate
            new TranslationSeedItem("entity.flowscheme.suspensionstate", "ja-JP", "挂起状态_jp", "挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）"),
            // entity.flowscheme.suspensionstate
            new TranslationSeedItem("entity.flowscheme.suspensionstate", "zh-CN", "挂起状态", "挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）"),
            // entity.flowscheme.suspensionstate
            new TranslationSeedItem("entity.flowscheme.suspensionstate", "zh-HK", "挂起状态_hk", "挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）"),

            // entity.flowscheme.processcontent
            new TranslationSeedItem("entity.flowscheme.processcontent", "en-US", "流程设计_us", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowscheme.processcontent
            new TranslationSeedItem("entity.flowscheme.processcontent", "ja-JP", "流程设计_jp", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowscheme.processcontent
            new TranslationSeedItem("entity.flowscheme.processcontent", "zh-CN", "流程设计", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowscheme.processcontent
            new TranslationSeedItem("entity.flowscheme.processcontent", "zh-HK", "流程设计_hk", "流程设计 JSON（节点、网关、条件、审批人配置）"),

            // entity.flowscheme.deploymentid
            new TranslationSeedItem("entity.flowscheme.deploymentid", "en-US", "部署批次号_us", "部署批次号（引擎发布时生成）"),
            // entity.flowscheme.deploymentid
            new TranslationSeedItem("entity.flowscheme.deploymentid", "ja-JP", "部署批次号_jp", "部署批次号（引擎发布时生成）"),
            // entity.flowscheme.deploymentid
            new TranslationSeedItem("entity.flowscheme.deploymentid", "zh-CN", "部署批次号", "部署批次号（引擎发布时生成）"),
            // entity.flowscheme.deploymentid
            new TranslationSeedItem("entity.flowscheme.deploymentid", "zh-HK", "部署批次号_hk", "部署批次号（引擎发布时生成）"),

            // entity.flowscheme.formid
            new TranslationSeedItem("entity.flowscheme.formid", "en-US", "关联表单ID_us", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowscheme.formid
            new TranslationSeedItem("entity.flowscheme.formid", "ja-JP", "关联表单ID_jp", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowscheme.formid
            new TranslationSeedItem("entity.flowscheme.formid", "zh-CN", "关联表单ID", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowscheme.formid
            new TranslationSeedItem("entity.flowscheme.formid", "zh-HK", "关联表单ID_hk", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),

            // entity.flowscheme.formcode
            new TranslationSeedItem("entity.flowscheme.formcode", "en-US", "关联表单编码_us", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowscheme.formcode
            new TranslationSeedItem("entity.flowscheme.formcode", "ja-JP", "关联表单编码_jp", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowscheme.formcode
            new TranslationSeedItem("entity.flowscheme.formcode", "zh-CN", "关联表单编码", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowscheme.formcode
            new TranslationSeedItem("entity.flowscheme.formcode", "zh-HK", "关联表单编码_hk", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowscheme.sortorder
            new TranslationSeedItem("entity.flowscheme.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.flowscheme.sortorder
            new TranslationSeedItem("entity.flowscheme.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.flowscheme.sortorder
            new TranslationSeedItem("entity.flowscheme.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.flowscheme.sortorder
            new TranslationSeedItem("entity.flowscheme.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.flowscheme.processstatus
            new TranslationSeedItem("entity.flowscheme.processstatus", "en-US", "发布状态_us", "发布状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowscheme.processstatus
            new TranslationSeedItem("entity.flowscheme.processstatus", "ja-JP", "发布状态_jp", "发布状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowscheme.processstatus
            new TranslationSeedItem("entity.flowscheme.processstatus", "zh-CN", "发布状态", "发布状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),
            // entity.flowscheme.processstatus
            new TranslationSeedItem("entity.flowscheme.processstatus", "zh-HK", "发布状态_hk", "发布状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）"),

            // entity.flowscheme.form
            new TranslationSeedItem("entity.flowscheme.form", "en-US", "关联表单_us", "关联表单"),
            // entity.flowscheme.form
            new TranslationSeedItem("entity.flowscheme.form", "ja-JP", "关联表单_jp", "关联表单"),
            // entity.flowscheme.form
            new TranslationSeedItem("entity.flowscheme.form", "zh-CN", "关联表单", "关联表单"),
            // entity.flowscheme.form
            new TranslationSeedItem("entity.flowscheme.form", "zh-HK", "关联表单_hk", "关联表单"),
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
