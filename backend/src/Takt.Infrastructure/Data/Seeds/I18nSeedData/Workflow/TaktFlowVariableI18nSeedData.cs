// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowVariableI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowVariable 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowVariable 实体国际化翻译种子（键前缀 entity.flowvariable.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowVariableI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowVariable 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowvariable 实体翻译...", tenantCode);

        foreach (var item in GetFlowVariableTranslations())
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

        TaktLogger.Information("TaktFlowVariable 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowVariable 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowvariable._self / entity.flowvariable.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowVariableTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowvariable._self
            new TranslationSeedItem("entity.flowvariable._self", "en-US", "Flow Variable Information_us", "实体名称"),
            // entity.flowvariable._self
            new TranslationSeedItem("entity.flowvariable._self", "ja-JP", "流程变量信息_jp", "实体名称"),
            // entity.flowvariable._self
            new TranslationSeedItem("entity.flowvariable._self", "zh-CN", "流程变量信息", "实体名称"),
            // entity.flowvariable._self
            new TranslationSeedItem("entity.flowvariable._self", "zh-HK", "流程变量信息_hk", "实体名称"),

            // entity.flowvariable.instanceid
            new TranslationSeedItem("entity.flowvariable.instanceid", "en-US", "流程实例ID_us", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowvariable.instanceid
            new TranslationSeedItem("entity.flowvariable.instanceid", "ja-JP", "流程实例ID_jp", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowvariable.instanceid
            new TranslationSeedItem("entity.flowvariable.instanceid", "zh-CN", "流程实例ID", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowvariable.instanceid
            new TranslationSeedItem("entity.flowvariable.instanceid", "zh-HK", "流程实例ID_hk", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),

            // entity.flowvariable.taskid
            new TranslationSeedItem("entity.flowvariable.taskid", "en-US", "任务ID_us", "任务 ID（选项 TaktFlowTasks/options；DictValue=Id；任务级变量时填写）"),
            // entity.flowvariable.taskid
            new TranslationSeedItem("entity.flowvariable.taskid", "ja-JP", "任务ID_jp", "任务 ID（选项 TaktFlowTasks/options；DictValue=Id；任务级变量时填写）"),
            // entity.flowvariable.taskid
            new TranslationSeedItem("entity.flowvariable.taskid", "zh-CN", "任务ID", "任务 ID（选项 TaktFlowTasks/options；DictValue=Id；任务级变量时填写）"),
            // entity.flowvariable.taskid
            new TranslationSeedItem("entity.flowvariable.taskid", "zh-HK", "任务ID_hk", "任务 ID（选项 TaktFlowTasks/options；DictValue=Id；任务级变量时填写）"),

            // entity.flowvariable.variablename
            new TranslationSeedItem("entity.flowvariable.variablename", "en-US", "变量名_us", "变量名"),
            // entity.flowvariable.variablename
            new TranslationSeedItem("entity.flowvariable.variablename", "ja-JP", "变量名_jp", "变量名"),
            // entity.flowvariable.variablename
            new TranslationSeedItem("entity.flowvariable.variablename", "zh-CN", "变量名", "变量名"),
            // entity.flowvariable.variablename
            new TranslationSeedItem("entity.flowvariable.variablename", "zh-HK", "变量名_hk", "变量名"),

            // entity.flowvariable.variabletype
            new TranslationSeedItem("entity.flowvariable.variabletype", "en-US", "变量类型_us", "变量类型（字典 sys_flow_variable_type；0=字符串 1=长整型 2=双精度 3=布尔 4=JSON）"),
            // entity.flowvariable.variabletype
            new TranslationSeedItem("entity.flowvariable.variabletype", "ja-JP", "变量类型_jp", "变量类型（字典 sys_flow_variable_type；0=字符串 1=长整型 2=双精度 3=布尔 4=JSON）"),
            // entity.flowvariable.variabletype
            new TranslationSeedItem("entity.flowvariable.variabletype", "zh-CN", "变量类型", "变量类型（字典 sys_flow_variable_type；0=字符串 1=长整型 2=双精度 3=布尔 4=JSON）"),
            // entity.flowvariable.variabletype
            new TranslationSeedItem("entity.flowvariable.variabletype", "zh-HK", "变量类型_hk", "变量类型（字典 sys_flow_variable_type；0=字符串 1=长整型 2=双精度 3=布尔 4=JSON）"),

            // entity.flowvariable.textvalue
            new TranslationSeedItem("entity.flowvariable.textvalue", "en-US", "文本值_us", "文本值（JSON 变量存此列）"),
            // entity.flowvariable.textvalue
            new TranslationSeedItem("entity.flowvariable.textvalue", "ja-JP", "文本值_jp", "文本值（JSON 变量存此列）"),
            // entity.flowvariable.textvalue
            new TranslationSeedItem("entity.flowvariable.textvalue", "zh-CN", "文本值", "文本值（JSON 变量存此列）"),
            // entity.flowvariable.textvalue
            new TranslationSeedItem("entity.flowvariable.textvalue", "zh-HK", "文本值_hk", "文本值（JSON 变量存此列）"),

            // entity.flowvariable.longvalue
            new TranslationSeedItem("entity.flowvariable.longvalue", "en-US", "长整型值_us", "长整型值"),
            // entity.flowvariable.longvalue
            new TranslationSeedItem("entity.flowvariable.longvalue", "ja-JP", "长整型值_jp", "长整型值"),
            // entity.flowvariable.longvalue
            new TranslationSeedItem("entity.flowvariable.longvalue", "zh-CN", "长整型值", "长整型值"),
            // entity.flowvariable.longvalue
            new TranslationSeedItem("entity.flowvariable.longvalue", "zh-HK", "长整型值_hk", "长整型值"),

            // entity.flowvariable.doublevalue
            new TranslationSeedItem("entity.flowvariable.doublevalue", "en-US", "双精度值_us", "双精度值"),
            // entity.flowvariable.doublevalue
            new TranslationSeedItem("entity.flowvariable.doublevalue", "ja-JP", "双精度值_jp", "双精度值"),
            // entity.flowvariable.doublevalue
            new TranslationSeedItem("entity.flowvariable.doublevalue", "zh-CN", "双精度值", "双精度值"),
            // entity.flowvariable.doublevalue
            new TranslationSeedItem("entity.flowvariable.doublevalue", "zh-HK", "双精度值_hk", "双精度值"),

            // entity.flowvariable.instance
            new TranslationSeedItem("entity.flowvariable.instance", "en-US", "所属流程实例_us", "所属流程实例"),
            // entity.flowvariable.instance
            new TranslationSeedItem("entity.flowvariable.instance", "ja-JP", "所属流程实例_jp", "所属流程实例"),
            // entity.flowvariable.instance
            new TranslationSeedItem("entity.flowvariable.instance", "zh-CN", "所属流程实例", "所属流程实例"),
            // entity.flowvariable.instance
            new TranslationSeedItem("entity.flowvariable.instance", "zh-HK", "所属流程实例_hk", "所属流程实例"),
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
