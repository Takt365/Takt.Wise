// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMeetingNotification 实体字段国际化种子（已对齐前端 locales：src/locales/routine/meeting-center/meeting-notification）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter;

/// <summary>
/// TaktMeetingNotification 实体国际化翻译种子（键前缀 entity.meetingnotification.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMeetingNotificationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMeetingNotification 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 meetingnotification 实体翻译...", tenantCode);

        foreach (var item in GetMeetingNotificationTranslations())
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

        TaktLogger.Information("TaktMeetingNotification 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMeetingNotification 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.meetingnotification._self / entity.meetingnotification.{{field}}；ResourceGroup=MeetingCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMeetingNotificationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.meetingnotification._self
            new TranslationSeedItem("entity.meetingnotification._self", "en-US", "Meeting Notification Information_us", "实体名称"),
            // entity.meetingnotification._self
            new TranslationSeedItem("entity.meetingnotification._self", "ja-JP", "会议通知投递记录信息_jp", "实体名称"),
            // entity.meetingnotification._self
            new TranslationSeedItem("entity.meetingnotification._self", "zh-CN", "会议通知投递记录信息", "实体名称"),
            // entity.meetingnotification._self
            new TranslationSeedItem("entity.meetingnotification._self", "zh-HK", "会议通知投递记录信息_hk", "实体名称"),

            // entity.meetingnotification.meetingid
            new TranslationSeedItem("entity.meetingnotification.meetingid", "en-US", "会议ID_us", "会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingid
            new TranslationSeedItem("entity.meetingnotification.meetingid", "ja-JP", "会议ID_jp", "会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingid
            new TranslationSeedItem("entity.meetingnotification.meetingid", "zh-CN", "会议ID", "会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingid
            new TranslationSeedItem("entity.meetingnotification.meetingid", "zh-HK", "会议ID_hk", "会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）"),

            // entity.meetingnotification.meetingattendeeid
            new TranslationSeedItem("entity.meetingnotification.meetingattendeeid", "en-US", "参会人员ID_us", "参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingattendeeid
            new TranslationSeedItem("entity.meetingnotification.meetingattendeeid", "ja-JP", "参会人员ID_jp", "参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingattendeeid
            new TranslationSeedItem("entity.meetingnotification.meetingattendeeid", "zh-CN", "参会人员ID", "参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）"),
            // entity.meetingnotification.meetingattendeeid
            new TranslationSeedItem("entity.meetingnotification.meetingattendeeid", "zh-HK", "参会人员ID_hk", "参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）"),

            // entity.meetingnotification.meetingtitle
            new TranslationSeedItem("entity.meetingnotification.meetingtitle", "en-US", "会议标题_us", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingnotification.meetingtitle
            new TranslationSeedItem("entity.meetingnotification.meetingtitle", "ja-JP", "会议标题_jp", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingnotification.meetingtitle
            new TranslationSeedItem("entity.meetingnotification.meetingtitle", "zh-CN", "会议标题", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingnotification.meetingtitle
            new TranslationSeedItem("entity.meetingnotification.meetingtitle", "zh-HK", "会议标题_hk", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),

            // entity.meetingnotification.meetingcode
            new TranslationSeedItem("entity.meetingnotification.meetingcode", "en-US", "会议编码_us", "会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）"),
            // entity.meetingnotification.meetingcode
            new TranslationSeedItem("entity.meetingnotification.meetingcode", "ja-JP", "会议编码_jp", "会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）"),
            // entity.meetingnotification.meetingcode
            new TranslationSeedItem("entity.meetingnotification.meetingcode", "zh-CN", "会议编码", "会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）"),
            // entity.meetingnotification.meetingcode
            new TranslationSeedItem("entity.meetingnotification.meetingcode", "zh-HK", "会议编码_hk", "会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）"),

            // entity.meetingnotification.userid
            new TranslationSeedItem("entity.meetingnotification.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingnotification.userid
            new TranslationSeedItem("entity.meetingnotification.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingnotification.userid
            new TranslationSeedItem("entity.meetingnotification.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingnotification.userid
            new TranslationSeedItem("entity.meetingnotification.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.meetingnotification.username
            new TranslationSeedItem("entity.meetingnotification.username", "en-US", "用户姓名_us", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingnotification.username
            new TranslationSeedItem("entity.meetingnotification.username", "ja-JP", "用户姓名_jp", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingnotification.username
            new TranslationSeedItem("entity.meetingnotification.username", "zh-CN", "用户姓名", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingnotification.username
            new TranslationSeedItem("entity.meetingnotification.username", "zh-HK", "用户姓名_hk", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.meetingnotification.recipientemail
            new TranslationSeedItem("entity.meetingnotification.recipientemail", "en-US", "收件邮箱_us", "收件邮箱（员工档案 Email）"),
            // entity.meetingnotification.recipientemail
            new TranslationSeedItem("entity.meetingnotification.recipientemail", "ja-JP", "收件邮箱_jp", "收件邮箱（员工档案 Email）"),
            // entity.meetingnotification.recipientemail
            new TranslationSeedItem("entity.meetingnotification.recipientemail", "zh-CN", "收件邮箱", "收件邮箱（员工档案 Email）"),
            // entity.meetingnotification.recipientemail
            new TranslationSeedItem("entity.meetingnotification.recipientemail", "zh-HK", "收件邮箱_hk", "收件邮箱（员工档案 Email）"),

            // entity.meetingnotification.notificationtype
            new TranslationSeedItem("entity.meetingnotification.notificationtype", "en-US", "通知类型_us", "通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）"),
            // entity.meetingnotification.notificationtype
            new TranslationSeedItem("entity.meetingnotification.notificationtype", "ja-JP", "通知类型_jp", "通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）"),
            // entity.meetingnotification.notificationtype
            new TranslationSeedItem("entity.meetingnotification.notificationtype", "zh-CN", "通知类型", "通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）"),
            // entity.meetingnotification.notificationtype
            new TranslationSeedItem("entity.meetingnotification.notificationtype", "zh-HK", "通知类型_hk", "通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）"),

            // entity.meetingnotification.notificationchannel
            new TranslationSeedItem("entity.meetingnotification.notificationchannel", "en-US", "通知渠道_us", "通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）"),
            // entity.meetingnotification.notificationchannel
            new TranslationSeedItem("entity.meetingnotification.notificationchannel", "ja-JP", "通知渠道_jp", "通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）"),
            // entity.meetingnotification.notificationchannel
            new TranslationSeedItem("entity.meetingnotification.notificationchannel", "zh-CN", "通知渠道", "通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）"),
            // entity.meetingnotification.notificationchannel
            new TranslationSeedItem("entity.meetingnotification.notificationchannel", "zh-HK", "通知渠道_hk", "通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）"),

            // entity.meetingnotification.deliverystatus
            new TranslationSeedItem("entity.meetingnotification.deliverystatus", "en-US", "投递状态_us", "投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）"),
            // entity.meetingnotification.deliverystatus
            new TranslationSeedItem("entity.meetingnotification.deliverystatus", "ja-JP", "投递状态_jp", "投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）"),
            // entity.meetingnotification.deliverystatus
            new TranslationSeedItem("entity.meetingnotification.deliverystatus", "zh-CN", "投递状态", "投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）"),
            // entity.meetingnotification.deliverystatus
            new TranslationSeedItem("entity.meetingnotification.deliverystatus", "zh-HK", "投递状态_hk", "投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）"),

            // entity.meetingnotification.notificationsubject
            new TranslationSeedItem("entity.meetingnotification.notificationsubject", "en-US", "邮件主题_us", "邮件主题"),
            // entity.meetingnotification.notificationsubject
            new TranslationSeedItem("entity.meetingnotification.notificationsubject", "ja-JP", "邮件主题_jp", "邮件主题"),
            // entity.meetingnotification.notificationsubject
            new TranslationSeedItem("entity.meetingnotification.notificationsubject", "zh-CN", "邮件主题", "邮件主题"),
            // entity.meetingnotification.notificationsubject
            new TranslationSeedItem("entity.meetingnotification.notificationsubject", "zh-HK", "邮件主题_hk", "邮件主题"),

            // entity.meetingnotification.confirmreceipttoken
            new TranslationSeedItem("entity.meetingnotification.confirmreceipttoken", "en-US", "回执确认令牌_us", "回执确认令牌（邮件链接参数；租户+公司内唯一）"),
            // entity.meetingnotification.confirmreceipttoken
            new TranslationSeedItem("entity.meetingnotification.confirmreceipttoken", "ja-JP", "回执确认令牌_jp", "回执确认令牌（邮件链接参数；租户+公司内唯一）"),
            // entity.meetingnotification.confirmreceipttoken
            new TranslationSeedItem("entity.meetingnotification.confirmreceipttoken", "zh-CN", "回执确认令牌", "回执确认令牌（邮件链接参数；租户+公司内唯一）"),
            // entity.meetingnotification.confirmreceipttoken
            new TranslationSeedItem("entity.meetingnotification.confirmreceipttoken", "zh-HK", "回执确认令牌_hk", "回执确认令牌（邮件链接参数；租户+公司内唯一）"),

            // entity.meetingnotification.sentat
            new TranslationSeedItem("entity.meetingnotification.sentat", "en-US", "发送时间_us", "发送时间"),
            // entity.meetingnotification.sentat
            new TranslationSeedItem("entity.meetingnotification.sentat", "ja-JP", "发送时间_jp", "发送时间"),
            // entity.meetingnotification.sentat
            new TranslationSeedItem("entity.meetingnotification.sentat", "zh-CN", "发送时间", "发送时间"),
            // entity.meetingnotification.sentat
            new TranslationSeedItem("entity.meetingnotification.sentat", "zh-HK", "发送时间_hk", "发送时间"),

            // entity.meetingnotification.confirmedat
            new TranslationSeedItem("entity.meetingnotification.confirmedat", "en-US", "回执确认时间_us", "回执确认时间"),
            // entity.meetingnotification.confirmedat
            new TranslationSeedItem("entity.meetingnotification.confirmedat", "ja-JP", "回执确认时间_jp", "回执确认时间"),
            // entity.meetingnotification.confirmedat
            new TranslationSeedItem("entity.meetingnotification.confirmedat", "zh-CN", "回执确认时间", "回执确认时间"),
            // entity.meetingnotification.confirmedat
            new TranslationSeedItem("entity.meetingnotification.confirmedat", "zh-HK", "回执确认时间_hk", "回执确认时间"),

            // entity.meetingnotification.confirmedbyuserid
            new TranslationSeedItem("entity.meetingnotification.confirmedbyuserid", "en-US", "确认人用户ID_us", "确认人用户 ID"),
            // entity.meetingnotification.confirmedbyuserid
            new TranslationSeedItem("entity.meetingnotification.confirmedbyuserid", "ja-JP", "确认人用户ID_jp", "确认人用户 ID"),
            // entity.meetingnotification.confirmedbyuserid
            new TranslationSeedItem("entity.meetingnotification.confirmedbyuserid", "zh-CN", "确认人用户ID", "确认人用户 ID"),
            // entity.meetingnotification.confirmedbyuserid
            new TranslationSeedItem("entity.meetingnotification.confirmedbyuserid", "zh-HK", "确认人用户ID_hk", "确认人用户 ID"),

            // entity.meetingnotification.confirmedbyusername
            new TranslationSeedItem("entity.meetingnotification.confirmedbyusername", "en-US", "确认人用户名_us", "确认人用户名"),
            // entity.meetingnotification.confirmedbyusername
            new TranslationSeedItem("entity.meetingnotification.confirmedbyusername", "ja-JP", "确认人用户名_jp", "确认人用户名"),
            // entity.meetingnotification.confirmedbyusername
            new TranslationSeedItem("entity.meetingnotification.confirmedbyusername", "zh-CN", "确认人用户名", "确认人用户名"),
            // entity.meetingnotification.confirmedbyusername
            new TranslationSeedItem("entity.meetingnotification.confirmedbyusername", "zh-HK", "确认人用户名_hk", "确认人用户名"),

            // entity.meetingnotification.senderrormessage
            new TranslationSeedItem("entity.meetingnotification.senderrormessage", "en-US", "发送失败原因_us", "发送失败原因（SMTP 或校验错误摘要）"),
            // entity.meetingnotification.senderrormessage
            new TranslationSeedItem("entity.meetingnotification.senderrormessage", "ja-JP", "发送失败原因_jp", "发送失败原因（SMTP 或校验错误摘要）"),
            // entity.meetingnotification.senderrormessage
            new TranslationSeedItem("entity.meetingnotification.senderrormessage", "zh-CN", "发送失败原因", "发送失败原因（SMTP 或校验错误摘要）"),
            // entity.meetingnotification.senderrormessage
            new TranslationSeedItem("entity.meetingnotification.senderrormessage", "zh-HK", "发送失败原因_hk", "发送失败原因（SMTP 或校验错误摘要）"),

            // entity.meetingnotification.meeting
            new TranslationSeedItem("entity.meetingnotification.meeting", "en-US", "会议_us", "会议（主表）"),
            // entity.meetingnotification.meeting
            new TranslationSeedItem("entity.meetingnotification.meeting", "ja-JP", "会议_jp", "会议（主表）"),
            // entity.meetingnotification.meeting
            new TranslationSeedItem("entity.meetingnotification.meeting", "zh-CN", "会议", "会议（主表）"),
            // entity.meetingnotification.meeting
            new TranslationSeedItem("entity.meetingnotification.meeting", "zh-HK", "会议_hk", "会议（主表）"),

            // entity.meetingnotification.meetingattendee
            new TranslationSeedItem("entity.meetingnotification.meetingattendee", "en-US", "参会人员_us", "参会人员（主子表关系）"),
            // entity.meetingnotification.meetingattendee
            new TranslationSeedItem("entity.meetingnotification.meetingattendee", "ja-JP", "参会人员_jp", "参会人员（主子表关系）"),
            // entity.meetingnotification.meetingattendee
            new TranslationSeedItem("entity.meetingnotification.meetingattendee", "zh-CN", "参会人员", "参会人员（主子表关系）"),
            // entity.meetingnotification.meetingattendee
            new TranslationSeedItem("entity.meetingnotification.meetingattendee", "zh-HK", "参会人员_hk", "参会人员（主子表关系）"),
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
        translation.ResourceGroup = "MeetingCenter";
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
