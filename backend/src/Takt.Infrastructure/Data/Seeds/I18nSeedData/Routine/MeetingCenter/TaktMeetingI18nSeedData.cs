// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter
// 文件名称：TaktMeetingI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMeeting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMeeting 实体国际化翻译种子（键前缀 entity.meeting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMeetingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMeeting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 meeting 实体翻译...", tenantCode);

        foreach (var item in GetMeetingTranslations())
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

        TaktLogger.Information("TaktMeeting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMeeting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.meeting._self / entity.meeting.{{field}}；ResourceGroup=MeetingCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMeetingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.meeting._self
            new TranslationSeedItem("entity.meeting._self", "en-US", "Meeting Information_us", "实体名称"),
            // entity.meeting._self
            new TranslationSeedItem("entity.meeting._self", "ja-JP", "会议中心主信息_jp", "实体名称"),
            // entity.meeting._self
            new TranslationSeedItem("entity.meeting._self", "zh-CN", "会议中心主信息", "实体名称"),
            // entity.meeting._self
            new TranslationSeedItem("entity.meeting._self", "zh-HK", "会议中心主信息_hk", "实体名称"),

            // entity.meeting.code
            new TranslationSeedItem("entity.meeting.code", "en-US", "会议编码_us", "会议编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议编码规则生成并展示，非手输；单据类型菜单：会议中心）"),
            // entity.meeting.code
            new TranslationSeedItem("entity.meeting.code", "ja-JP", "会议编码_jp", "会议编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议编码规则生成并展示，非手输；单据类型菜单：会议中心）"),
            // entity.meeting.code
            new TranslationSeedItem("entity.meeting.code", "zh-CN", "会议编码", "会议编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议编码规则生成并展示，非手输；单据类型菜单：会议中心）"),
            // entity.meeting.code
            new TranslationSeedItem("entity.meeting.code", "zh-HK", "会议编码_hk", "会议编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议编码规则生成并展示，非手输；单据类型菜单：会议中心）"),

            // entity.meeting.title
            new TranslationSeedItem("entity.meeting.title", "en-US", "会议标题_us", "会议标题"),
            // entity.meeting.title
            new TranslationSeedItem("entity.meeting.title", "ja-JP", "会议标题_jp", "会议标题"),
            // entity.meeting.title
            new TranslationSeedItem("entity.meeting.title", "zh-CN", "会议标题", "会议标题"),
            // entity.meeting.title
            new TranslationSeedItem("entity.meeting.title", "zh-HK", "会议标题_hk", "会议标题"),

            // entity.meeting.type
            new TranslationSeedItem("entity.meeting.type", "en-US", "会议类型_us", "会议类型（字典 routine_meeting_center_type；0=内部 1=外部 2=视频 3=混合）"),
            // entity.meeting.type
            new TranslationSeedItem("entity.meeting.type", "ja-JP", "会议类型_jp", "会议类型（字典 routine_meeting_center_type；0=内部 1=外部 2=视频 3=混合）"),
            // entity.meeting.type
            new TranslationSeedItem("entity.meeting.type", "zh-CN", "会议类型", "会议类型（字典 routine_meeting_center_type；0=内部 1=外部 2=视频 3=混合）"),
            // entity.meeting.type
            new TranslationSeedItem("entity.meeting.type", "zh-HK", "会议类型_hk", "会议类型（字典 routine_meeting_center_type；0=内部 1=外部 2=视频 3=混合）"),

            // entity.meeting.starttime
            new TranslationSeedItem("entity.meeting.starttime", "en-US", "开始时间_us", "开始时间"),
            // entity.meeting.starttime
            new TranslationSeedItem("entity.meeting.starttime", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.meeting.starttime
            new TranslationSeedItem("entity.meeting.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.meeting.starttime
            new TranslationSeedItem("entity.meeting.starttime", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.meeting.endtime
            new TranslationSeedItem("entity.meeting.endtime", "en-US", "结束时间_us", "结束时间"),
            // entity.meeting.endtime
            new TranslationSeedItem("entity.meeting.endtime", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.meeting.endtime
            new TranslationSeedItem("entity.meeting.endtime", "zh-CN", "结束时间", "结束时间"),
            // entity.meeting.endtime
            new TranslationSeedItem("entity.meeting.endtime", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.meeting.location
            new TranslationSeedItem("entity.meeting.location", "en-US", "会议地点_us", "会议地点（线下会议室名称或地址）"),
            // entity.meeting.location
            new TranslationSeedItem("entity.meeting.location", "ja-JP", "会议地点_jp", "会议地点（线下会议室名称或地址）"),
            // entity.meeting.location
            new TranslationSeedItem("entity.meeting.location", "zh-CN", "会议地点", "会议地点（线下会议室名称或地址）"),
            // entity.meeting.location
            new TranslationSeedItem("entity.meeting.location", "zh-HK", "会议地点_hk", "会议地点（线下会议室名称或地址）"),

            // entity.meeting.link
            new TranslationSeedItem("entity.meeting.link", "en-US", "会议链接_us", "会议链接（线上会议 URL）"),
            // entity.meeting.link
            new TranslationSeedItem("entity.meeting.link", "ja-JP", "会议链接_jp", "会议链接（线上会议 URL）"),
            // entity.meeting.link
            new TranslationSeedItem("entity.meeting.link", "zh-CN", "会议链接", "会议链接（线上会议 URL）"),
            // entity.meeting.link
            new TranslationSeedItem("entity.meeting.link", "zh-HK", "会议链接_hk", "会议链接（线上会议 URL）"),

            // entity.meeting.agenda
            new TranslationSeedItem("entity.meeting.agenda", "en-US", "会议议程_us", "会议议程（会前）"),
            // entity.meeting.agenda
            new TranslationSeedItem("entity.meeting.agenda", "ja-JP", "会议议程_jp", "会议议程（会前）"),
            // entity.meeting.agenda
            new TranslationSeedItem("entity.meeting.agenda", "zh-CN", "会议议程", "会议议程（会前）"),
            // entity.meeting.agenda
            new TranslationSeedItem("entity.meeting.agenda", "zh-HK", "会议议程_hk", "会议议程（会前）"),

            // entity.meeting.tags
            new TranslationSeedItem("entity.meeting.tags", "en-US", "标签_us", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.meeting.tags
            new TranslationSeedItem("entity.meeting.tags", "ja-JP", "标签_jp", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.meeting.tags
            new TranslationSeedItem("entity.meeting.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.meeting.tags
            new TranslationSeedItem("entity.meeting.tags", "zh-HK", "标签_hk", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.meeting.organizerid
            new TranslationSeedItem("entity.meeting.organizerid", "en-US", "组织人ID_us", "组织人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meeting.organizerid
            new TranslationSeedItem("entity.meeting.organizerid", "ja-JP", "组织人ID_jp", "组织人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meeting.organizerid
            new TranslationSeedItem("entity.meeting.organizerid", "zh-CN", "组织人ID", "组织人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meeting.organizerid
            new TranslationSeedItem("entity.meeting.organizerid", "zh-HK", "组织人ID_hk", "组织人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.meeting.organizername
            new TranslationSeedItem("entity.meeting.organizername", "en-US", "组织人姓名_us", "组织人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.organizername
            new TranslationSeedItem("entity.meeting.organizername", "ja-JP", "组织人姓名_jp", "组织人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.organizername
            new TranslationSeedItem("entity.meeting.organizername", "zh-CN", "组织人姓名", "组织人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.organizername
            new TranslationSeedItem("entity.meeting.organizername", "zh-HK", "组织人姓名_hk", "组织人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.meeting.deptid
            new TranslationSeedItem("entity.meeting.deptid", "en-US", "主办部门ID_us", "主办部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.meeting.deptid
            new TranslationSeedItem("entity.meeting.deptid", "ja-JP", "主办部门ID_jp", "主办部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.meeting.deptid
            new TranslationSeedItem("entity.meeting.deptid", "zh-CN", "主办部门ID", "主办部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.meeting.deptid
            new TranslationSeedItem("entity.meeting.deptid", "zh-HK", "主办部门ID_hk", "主办部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.meeting.deptname
            new TranslationSeedItem("entity.meeting.deptname", "en-US", "主办部门名称_us", "主办部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.deptname
            new TranslationSeedItem("entity.meeting.deptname", "ja-JP", "主办部门名称_jp", "主办部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.deptname
            new TranslationSeedItem("entity.meeting.deptname", "zh-CN", "主办部门名称", "主办部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.deptname
            new TranslationSeedItem("entity.meeting.deptname", "zh-HK", "主办部门名称_hk", "主办部门名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.meeting.maxattendees
            new TranslationSeedItem("entity.meeting.maxattendees", "en-US", "最大参会人数_us", "最大参会人数（0 表示不限）"),
            // entity.meeting.maxattendees
            new TranslationSeedItem("entity.meeting.maxattendees", "ja-JP", "最大参会人数_jp", "最大参会人数（0 表示不限）"),
            // entity.meeting.maxattendees
            new TranslationSeedItem("entity.meeting.maxattendees", "zh-CN", "最大参会人数", "最大参会人数（0 表示不限）"),
            // entity.meeting.maxattendees
            new TranslationSeedItem("entity.meeting.maxattendees", "zh-HK", "最大参会人数_hk", "最大参会人数（0 表示不限）"),

            // entity.meeting.reminderminutes
            new TranslationSeedItem("entity.meeting.reminderminutes", "en-US", "提前提醒分钟数_us", "提前提醒分钟数（0 表示不提醒）"),
            // entity.meeting.reminderminutes
            new TranslationSeedItem("entity.meeting.reminderminutes", "ja-JP", "提前提醒分钟数_jp", "提前提醒分钟数（0 表示不提醒）"),
            // entity.meeting.reminderminutes
            new TranslationSeedItem("entity.meeting.reminderminutes", "zh-CN", "提前提醒分钟数", "提前提醒分钟数（0 表示不提醒）"),
            // entity.meeting.reminderminutes
            new TranslationSeedItem("entity.meeting.reminderminutes", "zh-HK", "提前提醒分钟数_hk", "提前提醒分钟数（0 表示不提醒）"),

            // entity.meeting.roomid
            new TranslationSeedItem("entity.meeting.roomid", "en-US", "会议室ID_us", "会议室 ID（选项 TaktMeetingRooms/options；DictValue=Id）"),
            // entity.meeting.roomid
            new TranslationSeedItem("entity.meeting.roomid", "ja-JP", "会议室ID_jp", "会议室 ID（选项 TaktMeetingRooms/options；DictValue=Id）"),
            // entity.meeting.roomid
            new TranslationSeedItem("entity.meeting.roomid", "zh-CN", "会议室ID", "会议室 ID（选项 TaktMeetingRooms/options；DictValue=Id）"),
            // entity.meeting.roomid
            new TranslationSeedItem("entity.meeting.roomid", "zh-HK", "会议室ID_hk", "会议室 ID（选项 TaktMeetingRooms/options；DictValue=Id）"),

            // entity.meeting.roomname
            new TranslationSeedItem("entity.meeting.roomname", "en-US", "会议室名称_us", "会议室名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.roomname
            new TranslationSeedItem("entity.meeting.roomname", "ja-JP", "会议室名称_jp", "会议室名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.roomname
            new TranslationSeedItem("entity.meeting.roomname", "zh-CN", "会议室名称", "会议室名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.meeting.roomname
            new TranslationSeedItem("entity.meeting.roomname", "zh-HK", "会议室名称_hk", "会议室名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.meeting.status
            new TranslationSeedItem("entity.meeting.status", "en-US", "会议状态_us", "会议状态（字典 routine_meeting_center_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）"),
            // entity.meeting.status
            new TranslationSeedItem("entity.meeting.status", "ja-JP", "会议状态_jp", "会议状态（字典 routine_meeting_center_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）"),
            // entity.meeting.status
            new TranslationSeedItem("entity.meeting.status", "zh-CN", "会议状态", "会议状态（字典 routine_meeting_center_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）"),
            // entity.meeting.status
            new TranslationSeedItem("entity.meeting.status", "zh-HK", "会议状态_hk", "会议状态（字典 routine_meeting_center_status；0=草稿 1=已排期 2=进行中 3=已结束 4=已取消）"),

            // entity.meeting.attendees
            new TranslationSeedItem("entity.meeting.attendees", "en-US", "参与人列表_us", "参与人列表（主子表关系）"),
            // entity.meeting.attendees
            new TranslationSeedItem("entity.meeting.attendees", "ja-JP", "参与人列表_jp", "参与人列表（主子表关系）"),
            // entity.meeting.attendees
            new TranslationSeedItem("entity.meeting.attendees", "zh-CN", "参与人列表", "参与人列表（主子表关系）"),
            // entity.meeting.attendees
            new TranslationSeedItem("entity.meeting.attendees", "zh-HK", "参与人列表_hk", "参与人列表（主子表关系）"),

            // entity.meeting.notifications
            new TranslationSeedItem("entity.meeting.notifications", "en-US", "会议通知投递记录_us", "会议通知投递记录（主子表关系）"),
            // entity.meeting.notifications
            new TranslationSeedItem("entity.meeting.notifications", "ja-JP", "会议通知投递记录_jp", "会议通知投递记录（主子表关系）"),
            // entity.meeting.notifications
            new TranslationSeedItem("entity.meeting.notifications", "zh-CN", "会议通知投递记录", "会议通知投递记录（主子表关系）"),
            // entity.meeting.notifications
            new TranslationSeedItem("entity.meeting.notifications", "zh-HK", "会议通知投递记录_hk", "会议通知投递记录（主子表关系）"),
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
