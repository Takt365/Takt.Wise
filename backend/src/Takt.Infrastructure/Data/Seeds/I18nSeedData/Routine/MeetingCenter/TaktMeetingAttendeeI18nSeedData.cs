// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter
// 文件名称：TaktMeetingAttendeeI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMeetingAttendee 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMeetingAttendee 实体国际化翻译种子（键前缀 entity.meetingattendee.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMeetingAttendeeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMeetingAttendee 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 meetingattendee 实体翻译...", tenantCode);

        foreach (var item in GetMeetingAttendeeTranslations())
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

        TaktLogger.Information("TaktMeetingAttendee 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMeetingAttendee 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.meetingattendee._self / entity.meetingattendee.{{field}}；ResourceGroup=MeetingCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMeetingAttendeeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.meetingattendee._self
            new TranslationSeedItem("entity.meetingattendee._self", "en-US", "Meeting Attendee Information_us", "实体名称"),
            // entity.meetingattendee._self
            new TranslationSeedItem("entity.meetingattendee._self", "ja-JP", "参会人员子信息_jp", "实体名称"),
            // entity.meetingattendee._self
            new TranslationSeedItem("entity.meetingattendee._self", "zh-CN", "参会人员子信息", "实体名称"),
            // entity.meetingattendee._self
            new TranslationSeedItem("entity.meetingattendee._self", "zh-HK", "参会人员子信息_hk", "实体名称"),

            // entity.meetingattendee.meetingid
            new TranslationSeedItem("entity.meetingattendee.meetingid", "en-US", "会议ID_us", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingattendee.meetingid
            new TranslationSeedItem("entity.meetingattendee.meetingid", "ja-JP", "会议ID_jp", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingattendee.meetingid
            new TranslationSeedItem("entity.meetingattendee.meetingid", "zh-CN", "会议ID", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingattendee.meetingid
            new TranslationSeedItem("entity.meetingattendee.meetingid", "zh-HK", "会议ID_hk", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),

            // entity.meetingattendee.meetingtitle
            new TranslationSeedItem("entity.meetingattendee.meetingtitle", "en-US", "会议标题_us", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingattendee.meetingtitle
            new TranslationSeedItem("entity.meetingattendee.meetingtitle", "ja-JP", "会议标题_jp", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingattendee.meetingtitle
            new TranslationSeedItem("entity.meetingattendee.meetingtitle", "zh-CN", "会议标题", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingattendee.meetingtitle
            new TranslationSeedItem("entity.meetingattendee.meetingtitle", "zh-HK", "会议标题_hk", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),

            // entity.meetingattendee.linenumber
            new TranslationSeedItem("entity.meetingattendee.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.meetingattendee.linenumber
            new TranslationSeedItem("entity.meetingattendee.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.meetingattendee.linenumber
            new TranslationSeedItem("entity.meetingattendee.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.meetingattendee.linenumber
            new TranslationSeedItem("entity.meetingattendee.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.meetingattendee.userid
            new TranslationSeedItem("entity.meetingattendee.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingattendee.userid
            new TranslationSeedItem("entity.meetingattendee.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingattendee.userid
            new TranslationSeedItem("entity.meetingattendee.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingattendee.userid
            new TranslationSeedItem("entity.meetingattendee.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.meetingattendee.username
            new TranslationSeedItem("entity.meetingattendee.username", "en-US", "用户姓名_us", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingattendee.username
            new TranslationSeedItem("entity.meetingattendee.username", "ja-JP", "用户姓名_jp", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingattendee.username
            new TranslationSeedItem("entity.meetingattendee.username", "zh-CN", "用户姓名", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meetingattendee.username
            new TranslationSeedItem("entity.meetingattendee.username", "zh-HK", "用户姓名_hk", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.meetingattendee.attendeerole
            new TranslationSeedItem("entity.meetingattendee.attendeerole", "en-US", "参与角色_us", "参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）"),
            // entity.meetingattendee.attendeerole
            new TranslationSeedItem("entity.meetingattendee.attendeerole", "ja-JP", "参与角色_jp", "参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）"),
            // entity.meetingattendee.attendeerole
            new TranslationSeedItem("entity.meetingattendee.attendeerole", "zh-CN", "参与角色", "参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）"),
            // entity.meetingattendee.attendeerole
            new TranslationSeedItem("entity.meetingattendee.attendeerole", "zh-HK", "参与角色_hk", "参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）"),

            // entity.meetingattendee.checkintime
            new TranslationSeedItem("entity.meetingattendee.checkintime", "en-US", "签到时间_us", "签到时间"),
            // entity.meetingattendee.checkintime
            new TranslationSeedItem("entity.meetingattendee.checkintime", "ja-JP", "签到时间_jp", "签到时间"),
            // entity.meetingattendee.checkintime
            new TranslationSeedItem("entity.meetingattendee.checkintime", "zh-CN", "签到时间", "签到时间"),
            // entity.meetingattendee.checkintime
            new TranslationSeedItem("entity.meetingattendee.checkintime", "zh-HK", "签到时间_hk", "签到时间"),

            // entity.meetingattendee.checkouttime
            new TranslationSeedItem("entity.meetingattendee.checkouttime", "en-US", "签退时间_us", "签退时间"),
            // entity.meetingattendee.checkouttime
            new TranslationSeedItem("entity.meetingattendee.checkouttime", "ja-JP", "签退时间_jp", "签退时间"),
            // entity.meetingattendee.checkouttime
            new TranslationSeedItem("entity.meetingattendee.checkouttime", "zh-CN", "签退时间", "签退时间"),
            // entity.meetingattendee.checkouttime
            new TranslationSeedItem("entity.meetingattendee.checkouttime", "zh-HK", "签退时间_hk", "签退时间"),

            // entity.meetingattendee.checkinmethod
            new TranslationSeedItem("entity.meetingattendee.checkinmethod", "en-US", "签到方式_us", "签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）"),
            // entity.meetingattendee.checkinmethod
            new TranslationSeedItem("entity.meetingattendee.checkinmethod", "ja-JP", "签到方式_jp", "签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）"),
            // entity.meetingattendee.checkinmethod
            new TranslationSeedItem("entity.meetingattendee.checkinmethod", "zh-CN", "签到方式", "签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）"),
            // entity.meetingattendee.checkinmethod
            new TranslationSeedItem("entity.meetingattendee.checkinmethod", "zh-HK", "签到方式_hk", "签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）"),

            // entity.meetingattendee.attendancestatus
            new TranslationSeedItem("entity.meetingattendee.attendancestatus", "en-US", "出席状态_us", "出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）"),
            // entity.meetingattendee.attendancestatus
            new TranslationSeedItem("entity.meetingattendee.attendancestatus", "ja-JP", "出席状态_jp", "出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）"),
            // entity.meetingattendee.attendancestatus
            new TranslationSeedItem("entity.meetingattendee.attendancestatus", "zh-CN", "出席状态", "出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）"),
            // entity.meetingattendee.attendancestatus
            new TranslationSeedItem("entity.meetingattendee.attendancestatus", "zh-HK", "出席状态_hk", "出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）"),

            // entity.meetingattendee.isobsolete
            new TranslationSeedItem("entity.meetingattendee.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingattendee.isobsolete
            new TranslationSeedItem("entity.meetingattendee.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingattendee.isobsolete
            new TranslationSeedItem("entity.meetingattendee.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingattendee.isobsolete
            new TranslationSeedItem("entity.meetingattendee.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.meetingattendee.meeting
            new TranslationSeedItem("entity.meetingattendee.meeting", "en-US", "会议_us", "会议（主表）"),
            // entity.meetingattendee.meeting
            new TranslationSeedItem("entity.meetingattendee.meeting", "ja-JP", "会议_jp", "会议（主表）"),
            // entity.meetingattendee.meeting
            new TranslationSeedItem("entity.meetingattendee.meeting", "zh-CN", "会议", "会议（主表）"),
            // entity.meetingattendee.meeting
            new TranslationSeedItem("entity.meetingattendee.meeting", "zh-HK", "会议_hk", "会议（主表）"),
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
