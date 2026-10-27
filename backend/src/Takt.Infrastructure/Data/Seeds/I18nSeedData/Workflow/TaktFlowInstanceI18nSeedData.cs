// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowInstanceI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowInstance 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowInstance 实体国际化翻译种子（键前缀 entity.flowinstance.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowInstanceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowInstance 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowinstance 实体翻译...", tenantCode);

        foreach (var item in GetFlowInstanceTranslations())
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

        TaktLogger.Information("TaktFlowInstance 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowInstance 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowinstance._self / entity.flowinstance.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowInstanceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowinstance._self
            new TranslationSeedItem("entity.flowinstance._self", "en-US", "Flow Instance Information_us", "实体名称"),
            // entity.flowinstance._self
            new TranslationSeedItem("entity.flowinstance._self", "ja-JP", "流程实例信息_jp", "实体名称"),
            // entity.flowinstance._self
            new TranslationSeedItem("entity.flowinstance._self", "zh-CN", "流程实例信息", "实体名称"),
            // entity.flowinstance._self
            new TranslationSeedItem("entity.flowinstance._self", "zh-HK", "流程实例信息_hk", "实体名称"),

            // entity.flowinstance.instancecode
            new TranslationSeedItem("entity.flowinstance.instancecode", "en-US", "实例编码_us", "实例编码（对外业务单号；前端表单选择编码规则后自动通过 TaktNumbering 实例编码规则生成并展示，非手输；单据类型菜单：流程实例）"),
            // entity.flowinstance.instancecode
            new TranslationSeedItem("entity.flowinstance.instancecode", "ja-JP", "实例编码_jp", "实例编码（对外业务单号；前端表单选择编码规则后自动通过 TaktNumbering 实例编码规则生成并展示，非手输；单据类型菜单：流程实例）"),
            // entity.flowinstance.instancecode
            new TranslationSeedItem("entity.flowinstance.instancecode", "zh-CN", "实例编码", "实例编码（对外业务单号；前端表单选择编码规则后自动通过 TaktNumbering 实例编码规则生成并展示，非手输；单据类型菜单：流程实例）"),
            // entity.flowinstance.instancecode
            new TranslationSeedItem("entity.flowinstance.instancecode", "zh-HK", "实例编码_hk", "实例编码（对外业务单号；前端表单选择编码规则后自动通过 TaktNumbering 实例编码规则生成并展示，非手输；单据类型菜单：流程实例）"),

            // entity.flowinstance.processdefinitionid
            new TranslationSeedItem("entity.flowinstance.processdefinitionid", "en-US", "流程定义ID_us", "流程定义 ID（选项 TaktFlowSchemes/options；DictValue=Id）"),
            // entity.flowinstance.processdefinitionid
            new TranslationSeedItem("entity.flowinstance.processdefinitionid", "ja-JP", "流程定义ID_jp", "流程定义 ID（选项 TaktFlowSchemes/options；DictValue=Id）"),
            // entity.flowinstance.processdefinitionid
            new TranslationSeedItem("entity.flowinstance.processdefinitionid", "zh-CN", "流程定义ID", "流程定义 ID（选项 TaktFlowSchemes/options；DictValue=Id）"),
            // entity.flowinstance.processdefinitionid
            new TranslationSeedItem("entity.flowinstance.processdefinitionid", "zh-HK", "流程定义ID_hk", "流程定义 ID（选项 TaktFlowSchemes/options；DictValue=Id）"),

            // entity.flowinstance.processkey
            new TranslationSeedItem("entity.flowinstance.processkey", "en-US", "流程键_us", "流程键（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processkey
            new TranslationSeedItem("entity.flowinstance.processkey", "ja-JP", "流程键_jp", "流程键（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processkey
            new TranslationSeedItem("entity.flowinstance.processkey", "zh-CN", "流程键", "流程键（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processkey
            new TranslationSeedItem("entity.flowinstance.processkey", "zh-HK", "流程键_hk", "流程键（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowinstance.processname
            new TranslationSeedItem("entity.flowinstance.processname", "en-US", "流程名称_us", "流程名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processname
            new TranslationSeedItem("entity.flowinstance.processname", "ja-JP", "流程名称_jp", "流程名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processname
            new TranslationSeedItem("entity.flowinstance.processname", "zh-CN", "流程名称", "流程名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.processname
            new TranslationSeedItem("entity.flowinstance.processname", "zh-HK", "流程名称_hk", "流程名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowinstance.definitionversion
            new TranslationSeedItem("entity.flowinstance.definitionversion", "en-US", "定义版本号_us", "发起时锁定的定义版本号"),
            // entity.flowinstance.definitionversion
            new TranslationSeedItem("entity.flowinstance.definitionversion", "ja-JP", "定义版本号_jp", "发起时锁定的定义版本号"),
            // entity.flowinstance.definitionversion
            new TranslationSeedItem("entity.flowinstance.definitionversion", "zh-CN", "定义版本号", "发起时锁定的定义版本号"),
            // entity.flowinstance.definitionversion
            new TranslationSeedItem("entity.flowinstance.definitionversion", "zh-HK", "定义版本号_hk", "发起时锁定的定义版本号"),

            // entity.flowinstance.processtitle
            new TranslationSeedItem("entity.flowinstance.processtitle", "en-US", "申请标题_us", "申请标题"),
            // entity.flowinstance.processtitle
            new TranslationSeedItem("entity.flowinstance.processtitle", "ja-JP", "申请标题_jp", "申请标题"),
            // entity.flowinstance.processtitle
            new TranslationSeedItem("entity.flowinstance.processtitle", "zh-CN", "申请标题", "申请标题"),
            // entity.flowinstance.processtitle
            new TranslationSeedItem("entity.flowinstance.processtitle", "zh-HK", "申请标题_hk", "申请标题"),

            // entity.flowinstance.currentactivityid
            new TranslationSeedItem("entity.flowinstance.currentactivityid", "en-US", "当前节点ID_us", "当前节点 ID（设计器 nodeId）"),
            // entity.flowinstance.currentactivityid
            new TranslationSeedItem("entity.flowinstance.currentactivityid", "ja-JP", "当前节点ID_jp", "当前节点 ID（设计器 nodeId）"),
            // entity.flowinstance.currentactivityid
            new TranslationSeedItem("entity.flowinstance.currentactivityid", "zh-CN", "当前节点ID", "当前节点 ID（设计器 nodeId）"),
            // entity.flowinstance.currentactivityid
            new TranslationSeedItem("entity.flowinstance.currentactivityid", "zh-HK", "当前节点ID_hk", "当前节点 ID（设计器 nodeId）"),

            // entity.flowinstance.currentactivityname
            new TranslationSeedItem("entity.flowinstance.currentactivityname", "en-US", "当前节点名称_us", "当前节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.currentactivityname
            new TranslationSeedItem("entity.flowinstance.currentactivityname", "ja-JP", "当前节点名称_jp", "当前节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.currentactivityname
            new TranslationSeedItem("entity.flowinstance.currentactivityname", "zh-CN", "当前节点名称", "当前节点名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.currentactivityname
            new TranslationSeedItem("entity.flowinstance.currentactivityname", "zh-HK", "当前节点名称_hk", "当前节点名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowinstance.startuserid
            new TranslationSeedItem("entity.flowinstance.startuserid", "en-US", "发起人ID_us", "发起人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowinstance.startuserid
            new TranslationSeedItem("entity.flowinstance.startuserid", "ja-JP", "发起人ID_jp", "发起人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowinstance.startuserid
            new TranslationSeedItem("entity.flowinstance.startuserid", "zh-CN", "发起人ID", "发起人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowinstance.startuserid
            new TranslationSeedItem("entity.flowinstance.startuserid", "zh-HK", "发起人ID_hk", "发起人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.flowinstance.startusername
            new TranslationSeedItem("entity.flowinstance.startusername", "en-US", "发起人姓名_us", "发起人姓名（冗余：按 StartUserId 取 TaktUser.UserName 联动）"),
            // entity.flowinstance.startusername
            new TranslationSeedItem("entity.flowinstance.startusername", "ja-JP", "发起人姓名_jp", "发起人姓名（冗余：按 StartUserId 取 TaktUser.UserName 联动）"),
            // entity.flowinstance.startusername
            new TranslationSeedItem("entity.flowinstance.startusername", "zh-CN", "发起人姓名", "发起人姓名（冗余：按 StartUserId 取 TaktUser.UserName 联动）"),
            // entity.flowinstance.startusername
            new TranslationSeedItem("entity.flowinstance.startusername", "zh-HK", "发起人姓名_hk", "发起人姓名（冗余：按 StartUserId 取 TaktUser.UserName 联动）"),

            // entity.flowinstance.starttime
            new TranslationSeedItem("entity.flowinstance.starttime", "en-US", "开始时间_us", "开始时间"),
            // entity.flowinstance.starttime
            new TranslationSeedItem("entity.flowinstance.starttime", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.flowinstance.starttime
            new TranslationSeedItem("entity.flowinstance.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.flowinstance.starttime
            new TranslationSeedItem("entity.flowinstance.starttime", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.flowinstance.endtime
            new TranslationSeedItem("entity.flowinstance.endtime", "en-US", "结束时间_us", "结束时间"),
            // entity.flowinstance.endtime
            new TranslationSeedItem("entity.flowinstance.endtime", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.flowinstance.endtime
            new TranslationSeedItem("entity.flowinstance.endtime", "zh-CN", "结束时间", "结束时间"),
            // entity.flowinstance.endtime
            new TranslationSeedItem("entity.flowinstance.endtime", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.flowinstance.durationms
            new TranslationSeedItem("entity.flowinstance.durationms", "en-US", "历时毫秒_us", "历时毫秒"),
            // entity.flowinstance.durationms
            new TranslationSeedItem("entity.flowinstance.durationms", "ja-JP", "历时毫秒_jp", "历时毫秒"),
            // entity.flowinstance.durationms
            new TranslationSeedItem("entity.flowinstance.durationms", "zh-CN", "历时毫秒", "历时毫秒"),
            // entity.flowinstance.durationms
            new TranslationSeedItem("entity.flowinstance.durationms", "zh-HK", "历时毫秒_hk", "历时毫秒"),

            // entity.flowinstance.businesskey
            new TranslationSeedItem("entity.flowinstance.businesskey", "en-US", "业务主键_us", "业务主键（业务单据 Id 字符串，与 BusinessType 联合回写）"),
            // entity.flowinstance.businesskey
            new TranslationSeedItem("entity.flowinstance.businesskey", "ja-JP", "业务主键_jp", "业务主键（业务单据 Id 字符串，与 BusinessType 联合回写）"),
            // entity.flowinstance.businesskey
            new TranslationSeedItem("entity.flowinstance.businesskey", "zh-CN", "业务主键", "业务主键（业务单据 Id 字符串，与 BusinessType 联合回写）"),
            // entity.flowinstance.businesskey
            new TranslationSeedItem("entity.flowinstance.businesskey", "zh-HK", "业务主键_hk", "业务主键（业务单据 Id 字符串，与 BusinessType 联合回写）"),

            // entity.flowinstance.businesstype
            new TranslationSeedItem("entity.flowinstance.businesstype", "en-US", "业务类型_us", "业务类型（业务模块约定标识，默认与 ProcessKey 一致）"),
            // entity.flowinstance.businesstype
            new TranslationSeedItem("entity.flowinstance.businesstype", "ja-JP", "业务类型_jp", "业务类型（业务模块约定标识，默认与 ProcessKey 一致）"),
            // entity.flowinstance.businesstype
            new TranslationSeedItem("entity.flowinstance.businesstype", "zh-CN", "业务类型", "业务类型（业务模块约定标识，默认与 ProcessKey 一致）"),
            // entity.flowinstance.businesstype
            new TranslationSeedItem("entity.flowinstance.businesstype", "zh-HK", "业务类型_hk", "业务类型（业务模块约定标识，默认与 ProcessKey 一致）"),

            // entity.flowinstance.superinstanceid
            new TranslationSeedItem("entity.flowinstance.superinstanceid", "en-US", "父流程实例ID_us", "父流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowinstance.superinstanceid
            new TranslationSeedItem("entity.flowinstance.superinstanceid", "ja-JP", "父流程实例ID_jp", "父流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowinstance.superinstanceid
            new TranslationSeedItem("entity.flowinstance.superinstanceid", "zh-CN", "父流程实例ID", "父流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowinstance.superinstanceid
            new TranslationSeedItem("entity.flowinstance.superinstanceid", "zh-HK", "父流程实例ID_hk", "父流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),

            // entity.flowinstance.deletereason
            new TranslationSeedItem("entity.flowinstance.deletereason", "en-US", "终止原因_us", "终止原因（实例终止时填写）"),
            // entity.flowinstance.deletereason
            new TranslationSeedItem("entity.flowinstance.deletereason", "ja-JP", "终止原因_jp", "终止原因（实例终止时填写）"),
            // entity.flowinstance.deletereason
            new TranslationSeedItem("entity.flowinstance.deletereason", "zh-CN", "终止原因", "终止原因（实例终止时填写）"),
            // entity.flowinstance.deletereason
            new TranslationSeedItem("entity.flowinstance.deletereason", "zh-HK", "终止原因_hk", "终止原因（实例终止时填写）"),

            // entity.flowinstance.frmdata
            new TranslationSeedItem("entity.flowinstance.frmdata", "en-US", "表单数据_us", "表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable 表）"),
            // entity.flowinstance.frmdata
            new TranslationSeedItem("entity.flowinstance.frmdata", "ja-JP", "表单数据_jp", "表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable 表）"),
            // entity.flowinstance.frmdata
            new TranslationSeedItem("entity.flowinstance.frmdata", "zh-CN", "表单数据", "表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable 表）"),
            // entity.flowinstance.frmdata
            new TranslationSeedItem("entity.flowinstance.frmdata", "zh-HK", "表单数据_hk", "表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable 表）"),

            // entity.flowinstance.formid
            new TranslationSeedItem("entity.flowinstance.formid", "en-US", "表单ID_us", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowinstance.formid
            new TranslationSeedItem("entity.flowinstance.formid", "ja-JP", "表单ID_jp", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowinstance.formid
            new TranslationSeedItem("entity.flowinstance.formid", "zh-CN", "表单ID", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),
            // entity.flowinstance.formid
            new TranslationSeedItem("entity.flowinstance.formid", "zh-HK", "表单ID_hk", "关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）"),

            // entity.flowinstance.formcode
            new TranslationSeedItem("entity.flowinstance.formcode", "en-US", "表单编码_us", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.formcode
            new TranslationSeedItem("entity.flowinstance.formcode", "ja-JP", "表单编码_jp", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.formcode
            new TranslationSeedItem("entity.flowinstance.formcode", "zh-CN", "表单编码", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.flowinstance.formcode
            new TranslationSeedItem("entity.flowinstance.formcode", "zh-HK", "表单编码_hk", "关联表单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.flowinstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowinstance.processcontentsnapshot", "en-US", "流程设计快照_us", "流程设计快照（启动时复制 ProcessContent，避免定义变更影响在途实例）"),
            // entity.flowinstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowinstance.processcontentsnapshot", "ja-JP", "流程设计快照_jp", "流程设计快照（启动时复制 ProcessContent，避免定义变更影响在途实例）"),
            // entity.flowinstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowinstance.processcontentsnapshot", "zh-CN", "流程设计快照", "流程设计快照（启动时复制 ProcessContent，避免定义变更影响在途实例）"),
            // entity.flowinstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowinstance.processcontentsnapshot", "zh-HK", "流程设计快照_hk", "流程设计快照（启动时复制 ProcessContent，避免定义变更影响在途实例）"),

            // entity.flowinstance.instancestatus
            new TranslationSeedItem("entity.flowinstance.instancestatus", "en-US", "实例状态_us", "实例状态（字典 sys_flow_status；0=运行中 1=已完成 2=已驳回 3=已挂起 4=已终止 5=草稿）"),
            // entity.flowinstance.instancestatus
            new TranslationSeedItem("entity.flowinstance.instancestatus", "ja-JP", "实例状态_jp", "实例状态（字典 sys_flow_status；0=运行中 1=已完成 2=已驳回 3=已挂起 4=已终止 5=草稿）"),
            // entity.flowinstance.instancestatus
            new TranslationSeedItem("entity.flowinstance.instancestatus", "zh-CN", "实例状态", "实例状态（字典 sys_flow_status；0=运行中 1=已完成 2=已驳回 3=已挂起 4=已终止 5=草稿）"),
            // entity.flowinstance.instancestatus
            new TranslationSeedItem("entity.flowinstance.instancestatus", "zh-HK", "实例状态_hk", "实例状态（字典 sys_flow_status；0=运行中 1=已完成 2=已驳回 3=已挂起 4=已终止 5=草稿）"),

            // entity.flowinstance.processdefinition
            new TranslationSeedItem("entity.flowinstance.processdefinition", "en-US", "流程定义_us", "流程定义"),
            // entity.flowinstance.processdefinition
            new TranslationSeedItem("entity.flowinstance.processdefinition", "ja-JP", "流程定义_jp", "流程定义"),
            // entity.flowinstance.processdefinition
            new TranslationSeedItem("entity.flowinstance.processdefinition", "zh-CN", "流程定义", "流程定义"),
            // entity.flowinstance.processdefinition
            new TranslationSeedItem("entity.flowinstance.processdefinition", "zh-HK", "流程定义_hk", "流程定义"),

            // entity.flowinstance.tasks
            new TranslationSeedItem("entity.flowinstance.tasks", "en-US", "待办任务_us", "待办任务"),
            // entity.flowinstance.tasks
            new TranslationSeedItem("entity.flowinstance.tasks", "ja-JP", "待办任务_jp", "待办任务"),
            // entity.flowinstance.tasks
            new TranslationSeedItem("entity.flowinstance.tasks", "zh-CN", "待办任务", "待办任务"),
            // entity.flowinstance.tasks
            new TranslationSeedItem("entity.flowinstance.tasks", "zh-HK", "待办任务_hk", "待办任务"),

            // entity.flowinstance.historicactivities
            new TranslationSeedItem("entity.flowinstance.historicactivities", "en-US", "流转历史_us", "流转历史"),
            // entity.flowinstance.historicactivities
            new TranslationSeedItem("entity.flowinstance.historicactivities", "ja-JP", "流转历史_jp", "流转历史"),
            // entity.flowinstance.historicactivities
            new TranslationSeedItem("entity.flowinstance.historicactivities", "zh-CN", "流转历史", "流转历史"),
            // entity.flowinstance.historicactivities
            new TranslationSeedItem("entity.flowinstance.historicactivities", "zh-HK", "流转历史_hk", "流转历史"),

            // entity.flowinstance.variables
            new TranslationSeedItem("entity.flowinstance.variables", "en-US", "流程变量_us", "流程变量"),
            // entity.flowinstance.variables
            new TranslationSeedItem("entity.flowinstance.variables", "ja-JP", "流程变量_jp", "流程变量"),
            // entity.flowinstance.variables
            new TranslationSeedItem("entity.flowinstance.variables", "zh-CN", "流程变量", "流程变量"),
            // entity.flowinstance.variables
            new TranslationSeedItem("entity.flowinstance.variables", "zh-HK", "流程变量_hk", "流程变量"),

            // entity.flowinstance.addsigns
            new TranslationSeedItem("entity.flowinstance.addsigns", "en-US", "加签记录_us", "加签记录"),
            // entity.flowinstance.addsigns
            new TranslationSeedItem("entity.flowinstance.addsigns", "ja-JP", "加签记录_jp", "加签记录"),
            // entity.flowinstance.addsigns
            new TranslationSeedItem("entity.flowinstance.addsigns", "zh-CN", "加签记录", "加签记录"),
            // entity.flowinstance.addsigns
            new TranslationSeedItem("entity.flowinstance.addsigns", "zh-HK", "加签记录_hk", "加签记录"),
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
