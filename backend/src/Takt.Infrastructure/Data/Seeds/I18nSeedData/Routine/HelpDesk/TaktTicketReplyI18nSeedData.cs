// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktTicketReplyI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTicketReply 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTicketReply 实体国际化翻译种子（键前缀 entity.ticketreply.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTicketReplyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTicketReply 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ticketreply 实体翻译...", tenantCode);

        foreach (var item in GetTicketReplyTranslations())
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

        TaktLogger.Information("TaktTicketReply 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTicketReply 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ticketreply._self / entity.ticketreply.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTicketReplyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ticketreply._self
            new TranslationSeedItem("entity.ticketreply._self", "en-US", "Ticket Reply Information_us", "实体名称"),
            // entity.ticketreply._self
            new TranslationSeedItem("entity.ticketreply._self", "ja-JP", "工单回复信息_jp", "实体名称"),
            // entity.ticketreply._self
            new TranslationSeedItem("entity.ticketreply._self", "zh-CN", "工单回复信息", "实体名称"),
            // entity.ticketreply._self
            new TranslationSeedItem("entity.ticketreply._self", "zh-HK", "工单回复信息_hk", "实体名称"),

            // entity.ticketreply.ticketid
            new TranslationSeedItem("entity.ticketreply.ticketid", "en-US", "工单ID_us", "工单 ID（选项 TaktTickets/options；DictValue=Id）"),
            // entity.ticketreply.ticketid
            new TranslationSeedItem("entity.ticketreply.ticketid", "ja-JP", "工单ID_jp", "工单 ID（选项 TaktTickets/options；DictValue=Id）"),
            // entity.ticketreply.ticketid
            new TranslationSeedItem("entity.ticketreply.ticketid", "zh-CN", "工单ID", "工单 ID（选项 TaktTickets/options；DictValue=Id）"),
            // entity.ticketreply.ticketid
            new TranslationSeedItem("entity.ticketreply.ticketid", "zh-HK", "工单ID_hk", "工单 ID（选项 TaktTickets/options；DictValue=Id）"),

            // entity.ticketreply.authortype
            new TranslationSeedItem("entity.ticketreply.authortype", "en-US", "作者类型_us", "作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）"),
            // entity.ticketreply.authortype
            new TranslationSeedItem("entity.ticketreply.authortype", "ja-JP", "作者类型_jp", "作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）"),
            // entity.ticketreply.authortype
            new TranslationSeedItem("entity.ticketreply.authortype", "zh-CN", "作者类型", "作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）"),
            // entity.ticketreply.authortype
            new TranslationSeedItem("entity.ticketreply.authortype", "zh-HK", "作者类型_hk", "作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）"),

            // entity.ticketreply.authorid
            new TranslationSeedItem("entity.ticketreply.authorid", "en-US", "作者用户ID_us", "作者 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticketreply.authorid
            new TranslationSeedItem("entity.ticketreply.authorid", "ja-JP", "作者用户ID_jp", "作者 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticketreply.authorid
            new TranslationSeedItem("entity.ticketreply.authorid", "zh-CN", "作者用户ID", "作者 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.ticketreply.authorid
            new TranslationSeedItem("entity.ticketreply.authorid", "zh-HK", "作者用户ID_hk", "作者 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.ticketreply.authorname
            new TranslationSeedItem("entity.ticketreply.authorname", "en-US", "作者姓名_us", "作者姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticketreply.authorname
            new TranslationSeedItem("entity.ticketreply.authorname", "ja-JP", "作者姓名_jp", "作者姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticketreply.authorname
            new TranslationSeedItem("entity.ticketreply.authorname", "zh-CN", "作者姓名", "作者姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ticketreply.authorname
            new TranslationSeedItem("entity.ticketreply.authorname", "zh-HK", "作者姓名_hk", "作者姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ticketreply.content
            new TranslationSeedItem("entity.ticketreply.content", "en-US", "回复内容_us", "回复内容"),
            // entity.ticketreply.content
            new TranslationSeedItem("entity.ticketreply.content", "ja-JP", "回复内容_jp", "回复内容"),
            // entity.ticketreply.content
            new TranslationSeedItem("entity.ticketreply.content", "zh-CN", "回复内容", "回复内容"),
            // entity.ticketreply.content
            new TranslationSeedItem("entity.ticketreply.content", "zh-HK", "回复内容_hk", "回复内容"),

            // entity.ticketreply.attachments
            new TranslationSeedItem("entity.ticketreply.attachments", "en-US", "附件JSON_us", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticketreply.attachments
            new TranslationSeedItem("entity.ticketreply.attachments", "ja-JP", "附件JSON_jp", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticketreply.attachments
            new TranslationSeedItem("entity.ticketreply.attachments", "zh-CN", "附件JSON", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.ticketreply.attachments
            new TranslationSeedItem("entity.ticketreply.attachments", "zh-HK", "附件JSON_hk", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),

            // entity.ticketreply.isinternal
            new TranslationSeedItem("entity.ticketreply.isinternal", "en-US", "是否内部备注_us", "是否内部备注（字典 sys_yes_no；0=否 1=是；仅客服可见）"),
            // entity.ticketreply.isinternal
            new TranslationSeedItem("entity.ticketreply.isinternal", "ja-JP", "是否内部备注_jp", "是否内部备注（字典 sys_yes_no；0=否 1=是；仅客服可见）"),
            // entity.ticketreply.isinternal
            new TranslationSeedItem("entity.ticketreply.isinternal", "zh-CN", "是否内部备注", "是否内部备注（字典 sys_yes_no；0=否 1=是；仅客服可见）"),
            // entity.ticketreply.isinternal
            new TranslationSeedItem("entity.ticketreply.isinternal", "zh-HK", "是否内部备注_hk", "是否内部备注（字典 sys_yes_no；0=否 1=是；仅客服可见）"),

            // entity.ticketreply.ticket
            new TranslationSeedItem("entity.ticketreply.ticket", "en-US", "工单_us", "工单（主表）"),
            // entity.ticketreply.ticket
            new TranslationSeedItem("entity.ticketreply.ticket", "ja-JP", "工单_jp", "工单（主表）"),
            // entity.ticketreply.ticket
            new TranslationSeedItem("entity.ticketreply.ticket", "zh-CN", "工单", "工单（主表）"),
            // entity.ticketreply.ticket
            new TranslationSeedItem("entity.ticketreply.ticket", "zh-HK", "工单_hk", "工单（主表）"),
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
