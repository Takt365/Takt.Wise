// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktTicketI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTicket 实体字段国际化种子（已对齐前端 locales：src/locales/routine/help-desk/ticket）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktTicket 实体国际化翻译种子（键前缀 entity.ticket.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTicketI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTicket 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ticket 实体翻译...", tenantCode);

        foreach (var item in GetTicketTranslations())
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

        TaktLogger.Information("TaktTicket 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTicket 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ticket._self / entity.ticket.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTicketTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "en-US", "Ticket Information_us", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "ja-JP", "服务台工单信息_jp", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "zh-CN", "服务台工单信息", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "zh-HK", "服务台工单信息_hk", "实体名称"),

            // entity.ticket.code
            new TranslationSeedItem("entity.ticket.code", "en-US", "工单编码_us", "工单编码（唯一）"),
            // entity.ticket.code
            new TranslationSeedItem("entity.ticket.code", "ja-JP", "工单编码_jp", "工单编码（唯一）"),
            // entity.ticket.code
            new TranslationSeedItem("entity.ticket.code", "zh-CN", "工单编码", "工单编码（唯一）"),
            // entity.ticket.code
            new TranslationSeedItem("entity.ticket.code", "zh-HK", "工单编码_hk", "工单编码（唯一）"),

            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "en-US", "工单标题_us", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "ja-JP", "工单标题_jp", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "zh-CN", "工单标题", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "zh-HK", "工单标题_hk", "工单标题"),

            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "en-US", "工单内容_us", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "ja-JP", "工单内容_jp", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "zh-CN", "工单内容", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "zh-HK", "工单内容_hk", "工单内容描述"),

            // entity.ticket.attachments
            new TranslationSeedItem("entity.ticket.attachments", "en-US", "附件列表JSON_us", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticket.attachments
            new TranslationSeedItem("entity.ticket.attachments", "ja-JP", "附件列表JSON_jp", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticket.attachments
            new TranslationSeedItem("entity.ticket.attachments", "zh-CN", "附件列表JSON", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticket.attachments
            new TranslationSeedItem("entity.ticket.attachments", "zh-HK", "附件列表JSON_hk", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),

            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),

            // entity.ticket.urgency
            new TranslationSeedItem("entity.ticket.urgency", "en-US", "紧急度_us", "紧急度（字典 sys_urgency_level；1=高 2=中 3=低）"),
            // entity.ticket.urgency
            new TranslationSeedItem("entity.ticket.urgency", "ja-JP", "紧急度_jp", "紧急度（字典 sys_urgency_level；1=高 2=中 3=低）"),
            // entity.ticket.urgency
            new TranslationSeedItem("entity.ticket.urgency", "zh-CN", "紧急度", "紧急度（字典 sys_urgency_level；1=高 2=中 3=低）"),
            // entity.ticket.urgency
            new TranslationSeedItem("entity.ticket.urgency", "zh-HK", "紧急度_hk", "紧急度（字典 sys_urgency_level；1=高 2=中 3=低）"),

            // entity.ticket.impact
            new TranslationSeedItem("entity.ticket.impact", "en-US", "影响范围_us", "影响范围（字典 sys_impact_level；1=高 2=中 3=低）"),
            // entity.ticket.impact
            new TranslationSeedItem("entity.ticket.impact", "ja-JP", "影响范围_jp", "影响范围（字典 sys_impact_level；1=高 2=中 3=低）"),
            // entity.ticket.impact
            new TranslationSeedItem("entity.ticket.impact", "zh-CN", "影响范围", "影响范围（字典 sys_impact_level；1=高 2=中 3=低）"),
            // entity.ticket.impact
            new TranslationSeedItem("entity.ticket.impact", "zh-HK", "影响范围_hk", "影响范围（字典 sys_impact_level；1=高 2=中 3=低）"),

            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "en-US", "分类编码_us", "分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "ja-JP", "分类编码_jp", "分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "zh-CN", "分类编码", "分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "zh-HK", "分类编码_hk", "分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）"),

            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "en-US", "工单来源_us", "工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "ja-JP", "工单来源_jp", "工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "zh-CN", "工单来源", "工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "zh-HK", "工单来源_hk", "工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）"),

            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "en-US", "提交人ID_us", "提交人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "ja-JP", "提交人ID_jp", "提交人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "zh-CN", "提交人ID", "提交人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "zh-HK", "提交人ID_hk", "提交人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "en-US", "提交人姓名_us", "提交人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "ja-JP", "提交人姓名_jp", "提交人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "zh-CN", "提交人姓名", "提交人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "zh-HK", "提交人姓名_hk", "提交人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "en-US", "处理人ID_us", "处理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "ja-JP", "处理人ID_jp", "处理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "zh-CN", "处理人ID", "处理人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "zh-HK", "处理人ID_hk", "处理人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "en-US", "处理人姓名_us", "处理人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "ja-JP", "处理人姓名_jp", "处理人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "zh-CN", "处理人姓名", "处理人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "zh-HK", "处理人姓名_hk", "处理人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "en-US", "关联知识ID_us", "关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "ja-JP", "关联知识ID_jp", "关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "zh-CN", "关联知识ID", "关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "zh-HK", "关联知识ID_hk", "关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）"),

            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "en-US", "父工单ID_us", "父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "ja-JP", "父工单ID_jp", "父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "zh-CN", "父工单ID", "父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "zh-HK", "父工单ID_hk", "父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）"),

            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "en-US", "首次响应时间_us", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "ja-JP", "首次响应时间_jp", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "zh-CN", "首次响应时间", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "zh-HK", "首次响应时间_hk", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),

            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "en-US", "首次响应期限_us", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "ja-JP", "首次响应期限_jp", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "zh-CN", "首次响应期限", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "zh-HK", "首次响应期限_hk", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),

            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "en-US", "解决时间_us", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "ja-JP", "解决时间_jp", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "zh-CN", "解决时间", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "zh-HK", "解决时间_hk", "解决时间（问题被标记为已解决的时间）"),

            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "en-US", "解决期限_us", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "ja-JP", "解决期限_jp", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "zh-CN", "解决期限", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "zh-HK", "解决期限_hk", "解决期限（根据 SLA 计算出的解决截止时间）"),

            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "en-US", "关闭时间_us", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "ja-JP", "关闭时间_jp", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "zh-CN", "关闭时间", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "zh-HK", "关闭时间_hk", "关闭时间（工单最终关闭的时间）"),

            // entity.ticket.itassetid
            new TranslationSeedItem("entity.ticket.itassetid", "en-US", "IT设备ID_us", "IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）"),
            // entity.ticket.itassetid
            new TranslationSeedItem("entity.ticket.itassetid", "ja-JP", "IT设备ID_jp", "IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）"),
            // entity.ticket.itassetid
            new TranslationSeedItem("entity.ticket.itassetid", "zh-CN", "IT设备ID", "IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）"),
            // entity.ticket.itassetid
            new TranslationSeedItem("entity.ticket.itassetid", "zh-HK", "IT设备ID_hk", "IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）"),

            // entity.ticket.assetcode
            new TranslationSeedItem("entity.ticket.assetcode", "en-US", "资产号码_us", "资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）"),
            // entity.ticket.assetcode
            new TranslationSeedItem("entity.ticket.assetcode", "ja-JP", "资产号码_jp", "资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）"),
            // entity.ticket.assetcode
            new TranslationSeedItem("entity.ticket.assetcode", "zh-CN", "资产号码", "资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）"),
            // entity.ticket.assetcode
            new TranslationSeedItem("entity.ticket.assetcode", "zh-HK", "资产号码_hk", "资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）"),

            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "en-US", "流程实例ID_us", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id；BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "ja-JP", "流程实例ID_jp", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id；BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id；BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "zh-HK", "流程实例ID_hk", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id；BusinessType=Ticket、BusinessKey=本表 Id）"),

            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "en-US", "申请部门ID_us", "申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "ja-JP", "申请部门ID_jp", "申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "zh-CN", "申请部门ID", "申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "zh-HK", "申请部门ID_hk", "申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "en-US", "申请部门名称_us", "申请部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "ja-JP", "申请部门名称_jp", "申请部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "zh-CN", "申请部门名称", "申请部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "zh-HK", "申请部门名称_hk", "申请部门名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "en-US", "申请人ID_us", "申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "ja-JP", "申请人ID_jp", "申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "zh-CN", "申请人ID", "申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "zh-HK", "申请人ID_hk", "申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）"),

            // entity.ticket.applicantname
            new TranslationSeedItem("entity.ticket.applicantname", "en-US", "申请人名称_us", "申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）"),
            // entity.ticket.applicantname
            new TranslationSeedItem("entity.ticket.applicantname", "ja-JP", "申请人名称_jp", "申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）"),
            // entity.ticket.applicantname
            new TranslationSeedItem("entity.ticket.applicantname", "zh-CN", "申请人名称", "申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）"),
            // entity.ticket.applicantname
            new TranslationSeedItem("entity.ticket.applicantname", "zh-HK", "申请人名称_hk", "申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）"),

            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "en-US", "工单状态_us", "工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "ja-JP", "工单状态_jp", "工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "zh-CN", "工单状态", "工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "zh-HK", "工单状态_hk", "工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）"),

            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "en-US", "子工单列表_us", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "ja-JP", "子工单列表_jp", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "zh-CN", "子工单列表", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "zh-HK", "子工单列表_hk", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),

            // entity.ticket.evaluation
            new TranslationSeedItem("entity.ticket.evaluation", "en-US", "服务评价_us", "服务评价（工单关闭后的评价，一对一）"),
            // entity.ticket.evaluation
            new TranslationSeedItem("entity.ticket.evaluation", "ja-JP", "服务评价_jp", "服务评价（工单关闭后的评价，一对一）"),
            // entity.ticket.evaluation
            new TranslationSeedItem("entity.ticket.evaluation", "zh-CN", "服务评价", "服务评价（工单关闭后的评价，一对一）"),
            // entity.ticket.evaluation
            new TranslationSeedItem("entity.ticket.evaluation", "zh-HK", "服务评价_hk", "服务评价（工单关闭后的评价，一对一）"),
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
        translation.ResourceGroup = "HelpDesk";
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
