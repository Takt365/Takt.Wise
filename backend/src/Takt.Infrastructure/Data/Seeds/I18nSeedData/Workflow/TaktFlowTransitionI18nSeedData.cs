// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowTransitionI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowTransition 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowTransition 实体国际化翻译种子（键前缀 entity.flowtransition.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowTransitionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowTransition 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowtransition 实体翻译...", tenantCode);

        foreach (var item in GetFlowTransitionTranslations())
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

        TaktLogger.Information("TaktFlowTransition 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowTransition 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowtransition._self / entity.flowtransition.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowTransitionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowtransition._self
            new TranslationSeedItem("entity.flowtransition._self", "en-US", "Flow Transition Information_us", "实体名称"),
            // entity.flowtransition._self
            new TranslationSeedItem("entity.flowtransition._self", "ja-JP", "流程流转历史信息_jp", "实体名称"),
            // entity.flowtransition._self
            new TranslationSeedItem("entity.flowtransition._self", "zh-CN", "流程流转历史信息", "实体名称"),
            // entity.flowtransition._self
            new TranslationSeedItem("entity.flowtransition._self", "zh-HK", "流程流转历史信息_hk", "实体名称"),

            // entity.flowtransition.instanceid
            new TranslationSeedItem("entity.flowtransition.instanceid", "en-US", "流程实例ID_us", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtransition.instanceid
            new TranslationSeedItem("entity.flowtransition.instanceid", "ja-JP", "流程实例ID_jp", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtransition.instanceid
            new TranslationSeedItem("entity.flowtransition.instanceid", "zh-CN", "流程实例ID", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtransition.instanceid
            new TranslationSeedItem("entity.flowtransition.instanceid", "zh-HK", "流程实例ID_hk", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),

            // entity.flowtransition.activityid
            new TranslationSeedItem("entity.flowtransition.activityid", "en-US", "节点ID_us", "节点 ID（设计器 nodeId）"),
            // entity.flowtransition.activityid
            new TranslationSeedItem("entity.flowtransition.activityid", "ja-JP", "节点ID_jp", "节点 ID（设计器 nodeId）"),
            // entity.flowtransition.activityid
            new TranslationSeedItem("entity.flowtransition.activityid", "zh-CN", "节点ID", "节点 ID（设计器 nodeId）"),
            // entity.flowtransition.activityid
            new TranslationSeedItem("entity.flowtransition.activityid", "zh-HK", "节点ID_hk", "节点 ID（设计器 nodeId）"),

            // entity.flowtransition.activityname
            new TranslationSeedItem("entity.flowtransition.activityname", "en-US", "节点名称_us", "节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.activityname
            new TranslationSeedItem("entity.flowtransition.activityname", "ja-JP", "节点名称_jp", "节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.activityname
            new TranslationSeedItem("entity.flowtransition.activityname", "zh-CN", "节点名称", "节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.activityname
            new TranslationSeedItem("entity.flowtransition.activityname", "zh-HK", "节点名称_hk", "节点名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowtransition.activitytype
            new TranslationSeedItem("entity.flowtransition.activitytype", "en-US", "节点类型_us", "节点类型（设计器 nodeType 语义；运行时文本）"),
            // entity.flowtransition.activitytype
            new TranslationSeedItem("entity.flowtransition.activitytype", "ja-JP", "节点类型_jp", "节点类型（设计器 nodeType 语义；运行时文本）"),
            // entity.flowtransition.activitytype
            new TranslationSeedItem("entity.flowtransition.activitytype", "zh-CN", "节点类型", "节点类型（设计器 nodeType 语义；运行时文本）"),
            // entity.flowtransition.activitytype
            new TranslationSeedItem("entity.flowtransition.activitytype", "zh-HK", "节点类型_hk", "节点类型（设计器 nodeType 语义；运行时文本）"),

            // entity.flowtransition.fromnodeid
            new TranslationSeedItem("entity.flowtransition.fromnodeid", "en-US", "源节点ID_us", "源节点 ID（设计器 nodeId）"),
            // entity.flowtransition.fromnodeid
            new TranslationSeedItem("entity.flowtransition.fromnodeid", "ja-JP", "源节点ID_jp", "源节点 ID（设计器 nodeId）"),
            // entity.flowtransition.fromnodeid
            new TranslationSeedItem("entity.flowtransition.fromnodeid", "zh-CN", "源节点ID", "源节点 ID（设计器 nodeId）"),
            // entity.flowtransition.fromnodeid
            new TranslationSeedItem("entity.flowtransition.fromnodeid", "zh-HK", "源节点ID_hk", "源节点 ID（设计器 nodeId）"),

            // entity.flowtransition.fromnodename
            new TranslationSeedItem("entity.flowtransition.fromnodename", "en-US", "源节点名称_us", "源节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.fromnodename
            new TranslationSeedItem("entity.flowtransition.fromnodename", "ja-JP", "源节点名称_jp", "源节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.fromnodename
            new TranslationSeedItem("entity.flowtransition.fromnodename", "zh-CN", "源节点名称", "源节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.fromnodename
            new TranslationSeedItem("entity.flowtransition.fromnodename", "zh-HK", "源节点名称_hk", "源节点名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowtransition.tonodeid
            new TranslationSeedItem("entity.flowtransition.tonodeid", "en-US", "目标节点ID_us", "目标节点 ID（设计器 nodeId）"),
            // entity.flowtransition.tonodeid
            new TranslationSeedItem("entity.flowtransition.tonodeid", "ja-JP", "目标节点ID_jp", "目标节点 ID（设计器 nodeId）"),
            // entity.flowtransition.tonodeid
            new TranslationSeedItem("entity.flowtransition.tonodeid", "zh-CN", "目标节点ID", "目标节点 ID（设计器 nodeId）"),
            // entity.flowtransition.tonodeid
            new TranslationSeedItem("entity.flowtransition.tonodeid", "zh-HK", "目标节点ID_hk", "目标节点 ID（设计器 nodeId）"),

            // entity.flowtransition.tonodename
            new TranslationSeedItem("entity.flowtransition.tonodename", "en-US", "目标节点名称_us", "目标节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.tonodename
            new TranslationSeedItem("entity.flowtransition.tonodename", "ja-JP", "目标节点名称_jp", "目标节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.tonodename
            new TranslationSeedItem("entity.flowtransition.tonodename", "zh-CN", "目标节点名称", "目标节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtransition.tonodename
            new TranslationSeedItem("entity.flowtransition.tonodename", "zh-HK", "目标节点名称_hk", "目标节点名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowtransition.transitionuserid
            new TranslationSeedItem("entity.flowtransition.transitionuserid", "en-US", "操作人ID_us", "操作人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtransition.transitionuserid
            new TranslationSeedItem("entity.flowtransition.transitionuserid", "ja-JP", "操作人ID_jp", "操作人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtransition.transitionuserid
            new TranslationSeedItem("entity.flowtransition.transitionuserid", "zh-CN", "操作人ID", "操作人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtransition.transitionuserid
            new TranslationSeedItem("entity.flowtransition.transitionuserid", "zh-HK", "操作人ID_hk", "操作人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.flowtransition.transitionusername
            new TranslationSeedItem("entity.flowtransition.transitionusername", "en-US", "操作人姓名_us", "操作人姓名（冗余：按 TransitionUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtransition.transitionusername
            new TranslationSeedItem("entity.flowtransition.transitionusername", "ja-JP", "操作人姓名_jp", "操作人姓名（冗余：按 TransitionUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtransition.transitionusername
            new TranslationSeedItem("entity.flowtransition.transitionusername", "zh-CN", "操作人姓名", "操作人姓名（冗余：按 TransitionUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtransition.transitionusername
            new TranslationSeedItem("entity.flowtransition.transitionusername", "zh-HK", "操作人姓名_hk", "操作人姓名（冗余：按 TransitionUserId 取 TaktUser.UserName 联动）"),

            // entity.flowtransition.starttime
            new TranslationSeedItem("entity.flowtransition.starttime", "en-US", "开始时间_us", "开始时间"),
            // entity.flowtransition.starttime
            new TranslationSeedItem("entity.flowtransition.starttime", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.flowtransition.starttime
            new TranslationSeedItem("entity.flowtransition.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.flowtransition.starttime
            new TranslationSeedItem("entity.flowtransition.starttime", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.flowtransition.transitiontime
            new TranslationSeedItem("entity.flowtransition.transitiontime", "en-US", "结束时间_us", "结束时间"),
            // entity.flowtransition.transitiontime
            new TranslationSeedItem("entity.flowtransition.transitiontime", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.flowtransition.transitiontime
            new TranslationSeedItem("entity.flowtransition.transitiontime", "zh-CN", "结束时间", "结束时间"),
            // entity.flowtransition.transitiontime
            new TranslationSeedItem("entity.flowtransition.transitiontime", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.flowtransition.durationms
            new TranslationSeedItem("entity.flowtransition.durationms", "en-US", "历时毫秒_us", "历时毫秒"),
            // entity.flowtransition.durationms
            new TranslationSeedItem("entity.flowtransition.durationms", "ja-JP", "历时毫秒_jp", "历时毫秒"),
            // entity.flowtransition.durationms
            new TranslationSeedItem("entity.flowtransition.durationms", "zh-CN", "历时毫秒", "历时毫秒"),
            // entity.flowtransition.durationms
            new TranslationSeedItem("entity.flowtransition.durationms", "zh-HK", "历时毫秒_hk", "历时毫秒"),

            // entity.flowtransition.transitioncomment
            new TranslationSeedItem("entity.flowtransition.transitioncomment", "en-US", "操作意见_us", "操作意见"),
            // entity.flowtransition.transitioncomment
            new TranslationSeedItem("entity.flowtransition.transitioncomment", "ja-JP", "操作意见_jp", "操作意见"),
            // entity.flowtransition.transitioncomment
            new TranslationSeedItem("entity.flowtransition.transitioncomment", "zh-CN", "操作意见", "操作意见"),
            // entity.flowtransition.transitioncomment
            new TranslationSeedItem("entity.flowtransition.transitioncomment", "zh-HK", "操作意见_hk", "操作意见"),

            // entity.flowtransition.actiontype
            new TranslationSeedItem("entity.flowtransition.actiontype", "en-US", "动作类型_us", "动作类型（字典 sys_flow_action_type；0=发起 1=通过 2=驳回 3=撤回 4=转办 5=加签 6=减签 7=挂起 8=恢复 9=终止 10=抄送）"),
            // entity.flowtransition.actiontype
            new TranslationSeedItem("entity.flowtransition.actiontype", "ja-JP", "动作类型_jp", "动作类型（字典 sys_flow_action_type；0=发起 1=通过 2=驳回 3=撤回 4=转办 5=加签 6=减签 7=挂起 8=恢复 9=终止 10=抄送）"),
            // entity.flowtransition.actiontype
            new TranslationSeedItem("entity.flowtransition.actiontype", "zh-CN", "动作类型", "动作类型（字典 sys_flow_action_type；0=发起 1=通过 2=驳回 3=撤回 4=转办 5=加签 6=减签 7=挂起 8=恢复 9=终止 10=抄送）"),
            // entity.flowtransition.actiontype
            new TranslationSeedItem("entity.flowtransition.actiontype", "zh-HK", "动作类型_hk", "动作类型（字典 sys_flow_action_type；0=发起 1=通过 2=驳回 3=撤回 4=转办 5=加签 6=减签 7=挂起 8=恢复 9=终止 10=抄送）"),

            // entity.flowtransition.instance
            new TranslationSeedItem("entity.flowtransition.instance", "en-US", "所属流程实例_us", "所属流程实例"),
            // entity.flowtransition.instance
            new TranslationSeedItem("entity.flowtransition.instance", "ja-JP", "所属流程实例_jp", "所属流程实例"),
            // entity.flowtransition.instance
            new TranslationSeedItem("entity.flowtransition.instance", "zh-CN", "所属流程实例", "所属流程实例"),
            // entity.flowtransition.instance
            new TranslationSeedItem("entity.flowtransition.instance", "zh-HK", "所属流程实例_hk", "所属流程实例"),
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
