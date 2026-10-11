// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowTaskI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowTask 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowTask 实体国际化翻译种子（键前缀 entity.flowtask.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowTaskI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowTask 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowtask 实体翻译...", tenantCode);

        foreach (var item in GetFlowTaskTranslations())
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

        TaktLogger.Information("TaktFlowTask 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowTask 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowtask._self / entity.flowtask.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowTaskTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowtask._self
            new TranslationSeedItem("entity.flowtask._self", "en-US", "Flow Task Information_us", "实体名称"),
            // entity.flowtask._self
            new TranslationSeedItem("entity.flowtask._self", "ja-JP", "流程用户任务信息_jp", "实体名称"),
            // entity.flowtask._self
            new TranslationSeedItem("entity.flowtask._self", "zh-CN", "流程用户任务信息", "实体名称"),
            // entity.flowtask._self
            new TranslationSeedItem("entity.flowtask._self", "zh-HK", "流程用户任务信息_hk", "实体名称"),

            // entity.flowtask.instanceid
            new TranslationSeedItem("entity.flowtask.instanceid", "en-US", "流程实例ID_us", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtask.instanceid
            new TranslationSeedItem("entity.flowtask.instanceid", "ja-JP", "流程实例ID_jp", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtask.instanceid
            new TranslationSeedItem("entity.flowtask.instanceid", "zh-CN", "流程实例ID", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowtask.instanceid
            new TranslationSeedItem("entity.flowtask.instanceid", "zh-HK", "流程实例ID_hk", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),

            // entity.flowtask.taskdefinitionkey
            new TranslationSeedItem("entity.flowtask.taskdefinitionkey", "en-US", "任务定义键_us", "任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowtask.taskdefinitionkey
            new TranslationSeedItem("entity.flowtask.taskdefinitionkey", "ja-JP", "任务定义键_jp", "任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowtask.taskdefinitionkey
            new TranslationSeedItem("entity.flowtask.taskdefinitionkey", "zh-CN", "任务定义键", "任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowtask.taskdefinitionkey
            new TranslationSeedItem("entity.flowtask.taskdefinitionkey", "zh-HK", "任务定义键_hk", "任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）"),

            // entity.flowtask.taskname
            new TranslationSeedItem("entity.flowtask.taskname", "en-US", "任务名称_us", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtask.taskname
            new TranslationSeedItem("entity.flowtask.taskname", "ja-JP", "任务名称_jp", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtask.taskname
            new TranslationSeedItem("entity.flowtask.taskname", "zh-CN", "任务名称", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowtask.taskname
            new TranslationSeedItem("entity.flowtask.taskname", "zh-HK", "任务名称_hk", "任务名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowtask.assigneeuserid
            new TranslationSeedItem("entity.flowtask.assigneeuserid", "en-US", "办理人ID_us", "办理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtask.assigneeuserid
            new TranslationSeedItem("entity.flowtask.assigneeuserid", "ja-JP", "办理人ID_jp", "办理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtask.assigneeuserid
            new TranslationSeedItem("entity.flowtask.assigneeuserid", "zh-CN", "办理人ID", "办理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowtask.assigneeuserid
            new TranslationSeedItem("entity.flowtask.assigneeuserid", "zh-HK", "办理人ID_hk", "办理人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.flowtask.assigneeusername
            new TranslationSeedItem("entity.flowtask.assigneeusername", "en-US", "办理人姓名_us", "办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.assigneeusername
            new TranslationSeedItem("entity.flowtask.assigneeusername", "ja-JP", "办理人姓名_jp", "办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.assigneeusername
            new TranslationSeedItem("entity.flowtask.assigneeusername", "zh-CN", "办理人姓名", "办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.assigneeusername
            new TranslationSeedItem("entity.flowtask.assigneeusername", "zh-HK", "办理人姓名_hk", "办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）"),

            // entity.flowtask.owneruserid
            new TranslationSeedItem("entity.flowtask.owneruserid", "en-US", "任务所有者ID_us", "任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）"),
            // entity.flowtask.owneruserid
            new TranslationSeedItem("entity.flowtask.owneruserid", "ja-JP", "任务所有者ID_jp", "任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）"),
            // entity.flowtask.owneruserid
            new TranslationSeedItem("entity.flowtask.owneruserid", "zh-CN", "任务所有者ID", "任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）"),
            // entity.flowtask.owneruserid
            new TranslationSeedItem("entity.flowtask.owneruserid", "zh-HK", "任务所有者ID_hk", "任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）"),

            // entity.flowtask.ownerusername
            new TranslationSeedItem("entity.flowtask.ownerusername", "en-US", "任务所有者姓名_us", "任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.ownerusername
            new TranslationSeedItem("entity.flowtask.ownerusername", "ja-JP", "任务所有者姓名_jp", "任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.ownerusername
            new TranslationSeedItem("entity.flowtask.ownerusername", "zh-CN", "任务所有者姓名", "任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）"),
            // entity.flowtask.ownerusername
            new TranslationSeedItem("entity.flowtask.ownerusername", "zh-HK", "任务所有者姓名_hk", "任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）"),

            // entity.flowtask.signtype
            new TranslationSeedItem("entity.flowtask.signtype", "en-US", "会签类型_us", "会签类型（字典 sys_flow_sign_type；1=或签 2=会签）"),
            // entity.flowtask.signtype
            new TranslationSeedItem("entity.flowtask.signtype", "ja-JP", "会签类型_jp", "会签类型（字典 sys_flow_sign_type；1=或签 2=会签）"),
            // entity.flowtask.signtype
            new TranslationSeedItem("entity.flowtask.signtype", "zh-CN", "会签类型", "会签类型（字典 sys_flow_sign_type；1=或签 2=会签）"),
            // entity.flowtask.signtype
            new TranslationSeedItem("entity.flowtask.signtype", "zh-HK", "会签类型_hk", "会签类型（字典 sys_flow_sign_type；1=或签 2=会签）"),

            // entity.flowtask.priority
            new TranslationSeedItem("entity.flowtask.priority", "en-US", "优先级_us", "优先级"),
            // entity.flowtask.priority
            new TranslationSeedItem("entity.flowtask.priority", "ja-JP", "优先级_jp", "优先级"),
            // entity.flowtask.priority
            new TranslationSeedItem("entity.flowtask.priority", "zh-CN", "优先级", "优先级"),
            // entity.flowtask.priority
            new TranslationSeedItem("entity.flowtask.priority", "zh-HK", "优先级_hk", "优先级"),

            // entity.flowtask.duedate
            new TranslationSeedItem("entity.flowtask.duedate", "en-US", "到期时间_us", "到期时间"),
            // entity.flowtask.duedate
            new TranslationSeedItem("entity.flowtask.duedate", "ja-JP", "到期时间_jp", "到期时间"),
            // entity.flowtask.duedate
            new TranslationSeedItem("entity.flowtask.duedate", "zh-CN", "到期时间", "到期时间"),
            // entity.flowtask.duedate
            new TranslationSeedItem("entity.flowtask.duedate", "zh-HK", "到期时间_hk", "到期时间"),

            // entity.flowtask.claimtime
            new TranslationSeedItem("entity.flowtask.claimtime", "en-US", "认领时间_us", "认领时间"),
            // entity.flowtask.claimtime
            new TranslationSeedItem("entity.flowtask.claimtime", "ja-JP", "认领时间_jp", "认领时间"),
            // entity.flowtask.claimtime
            new TranslationSeedItem("entity.flowtask.claimtime", "zh-CN", "认领时间", "认领时间"),
            // entity.flowtask.claimtime
            new TranslationSeedItem("entity.flowtask.claimtime", "zh-HK", "认领时间_hk", "认领时间"),

            // entity.flowtask.completedat
            new TranslationSeedItem("entity.flowtask.completedat", "en-US", "办结时间_us", "办结时间"),
            // entity.flowtask.completedat
            new TranslationSeedItem("entity.flowtask.completedat", "ja-JP", "办结时间_jp", "办结时间"),
            // entity.flowtask.completedat
            new TranslationSeedItem("entity.flowtask.completedat", "zh-CN", "办结时间", "办结时间"),
            // entity.flowtask.completedat
            new TranslationSeedItem("entity.flowtask.completedat", "zh-HK", "办结时间_hk", "办结时间"),

            // entity.flowtask.isaddsign
            new TranslationSeedItem("entity.flowtask.isaddsign", "en-US", "是否加签_us", "是否加签任务（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowtask.isaddsign
            new TranslationSeedItem("entity.flowtask.isaddsign", "ja-JP", "是否加签_jp", "是否加签任务（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowtask.isaddsign
            new TranslationSeedItem("entity.flowtask.isaddsign", "zh-CN", "是否加签", "是否加签任务（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowtask.isaddsign
            new TranslationSeedItem("entity.flowtask.isaddsign", "zh-HK", "是否加签_hk", "是否加签任务（字典 sys_yes_no；0=否 1=是）"),

            // entity.flowtask.addsignid
            new TranslationSeedItem("entity.flowtask.addsignid", "en-US", "加签记录ID_us", "加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）"),
            // entity.flowtask.addsignid
            new TranslationSeedItem("entity.flowtask.addsignid", "ja-JP", "加签记录ID_jp", "加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）"),
            // entity.flowtask.addsignid
            new TranslationSeedItem("entity.flowtask.addsignid", "zh-CN", "加签记录ID", "加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）"),
            // entity.flowtask.addsignid
            new TranslationSeedItem("entity.flowtask.addsignid", "zh-HK", "加签记录ID_hk", "加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）"),

            // entity.flowtask.comment
            new TranslationSeedItem("entity.flowtask.comment", "en-US", "审批意见_us", "审批意见"),
            // entity.flowtask.comment
            new TranslationSeedItem("entity.flowtask.comment", "ja-JP", "审批意见_jp", "审批意见"),
            // entity.flowtask.comment
            new TranslationSeedItem("entity.flowtask.comment", "zh-CN", "审批意见", "审批意见"),
            // entity.flowtask.comment
            new TranslationSeedItem("entity.flowtask.comment", "zh-HK", "审批意见_hk", "审批意见"),

            // entity.flowtask.sortorder
            new TranslationSeedItem("entity.flowtask.sortorder", "en-US", "序号_us", "多实例序号"),
            // entity.flowtask.sortorder
            new TranslationSeedItem("entity.flowtask.sortorder", "ja-JP", "序号_jp", "多实例序号"),
            // entity.flowtask.sortorder
            new TranslationSeedItem("entity.flowtask.sortorder", "zh-CN", "序号", "多实例序号"),
            // entity.flowtask.sortorder
            new TranslationSeedItem("entity.flowtask.sortorder", "zh-HK", "序号_hk", "多实例序号"),

            // entity.flowtask.taskstatus
            new TranslationSeedItem("entity.flowtask.taskstatus", "en-US", "任务状态_us", "任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）"),
            // entity.flowtask.taskstatus
            new TranslationSeedItem("entity.flowtask.taskstatus", "ja-JP", "任务状态_jp", "任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）"),
            // entity.flowtask.taskstatus
            new TranslationSeedItem("entity.flowtask.taskstatus", "zh-CN", "任务状态", "任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）"),
            // entity.flowtask.taskstatus
            new TranslationSeedItem("entity.flowtask.taskstatus", "zh-HK", "任务状态_hk", "任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）"),

            // entity.flowtask.instance
            new TranslationSeedItem("entity.flowtask.instance", "en-US", "所属流程实例_us", "所属流程实例"),
            // entity.flowtask.instance
            new TranslationSeedItem("entity.flowtask.instance", "ja-JP", "所属流程实例_jp", "所属流程实例"),
            // entity.flowtask.instance
            new TranslationSeedItem("entity.flowtask.instance", "zh-CN", "所属流程实例", "所属流程实例"),
            // entity.flowtask.instance
            new TranslationSeedItem("entity.flowtask.instance", "zh-HK", "所属流程实例_hk", "所属流程实例"),
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
