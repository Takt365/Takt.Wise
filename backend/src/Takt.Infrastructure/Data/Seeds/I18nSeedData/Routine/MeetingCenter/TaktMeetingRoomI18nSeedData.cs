// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter
// 文件名称：TaktMeetingRoomI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMeetingRoom 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMeetingRoom 实体国际化翻译种子（键前缀 entity.meetingroom.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMeetingRoomI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMeetingRoom 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 meetingroom 实体翻译...", tenantCode);

        foreach (var item in GetMeetingRoomTranslations())
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

        TaktLogger.Information("TaktMeetingRoom 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMeetingRoom 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.meetingroom._self / entity.meetingroom.{{field}}；ResourceGroup=MeetingCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMeetingRoomTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.meetingroom._self
            new TranslationSeedItem("entity.meetingroom._self", "en-US", "Meeting Room Information_us", "实体名称"),
            // entity.meetingroom._self
            new TranslationSeedItem("entity.meetingroom._self", "ja-JP", "会议室信息_jp", "实体名称"),
            // entity.meetingroom._self
            new TranslationSeedItem("entity.meetingroom._self", "zh-CN", "会议室信息", "实体名称"),
            // entity.meetingroom._self
            new TranslationSeedItem("entity.meetingroom._self", "zh-HK", "会议室信息_hk", "实体名称"),

            // entity.meetingroom.roomcode
            new TranslationSeedItem("entity.meetingroom.roomcode", "en-US", "会议室编码_us", "会议室编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议室编码规则生成并展示，非手输；单据类型菜单：会议室）"),
            // entity.meetingroom.roomcode
            new TranslationSeedItem("entity.meetingroom.roomcode", "ja-JP", "会议室编码_jp", "会议室编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议室编码规则生成并展示，非手输；单据类型菜单：会议室）"),
            // entity.meetingroom.roomcode
            new TranslationSeedItem("entity.meetingroom.roomcode", "zh-CN", "会议室编码", "会议室编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议室编码规则生成并展示，非手输；单据类型菜单：会议室）"),
            // entity.meetingroom.roomcode
            new TranslationSeedItem("entity.meetingroom.roomcode", "zh-HK", "会议室编码_hk", "会议室编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 会议室编码规则生成并展示，非手输；单据类型菜单：会议室）"),

            // entity.meetingroom.roomname
            new TranslationSeedItem("entity.meetingroom.roomname", "en-US", "会议室名称_us", "会议室名称"),
            // entity.meetingroom.roomname
            new TranslationSeedItem("entity.meetingroom.roomname", "ja-JP", "会议室名称_jp", "会议室名称"),
            // entity.meetingroom.roomname
            new TranslationSeedItem("entity.meetingroom.roomname", "zh-CN", "会议室名称", "会议室名称"),
            // entity.meetingroom.roomname
            new TranslationSeedItem("entity.meetingroom.roomname", "zh-HK", "会议室名称_hk", "会议室名称"),

            // entity.meetingroom.building
            new TranslationSeedItem("entity.meetingroom.building", "en-US", "楼栋_us", "楼栋/建筑"),
            // entity.meetingroom.building
            new TranslationSeedItem("entity.meetingroom.building", "ja-JP", "楼栋_jp", "楼栋/建筑"),
            // entity.meetingroom.building
            new TranslationSeedItem("entity.meetingroom.building", "zh-CN", "楼栋", "楼栋/建筑"),
            // entity.meetingroom.building
            new TranslationSeedItem("entity.meetingroom.building", "zh-HK", "楼栋_hk", "楼栋/建筑"),

            // entity.meetingroom.floor
            new TranslationSeedItem("entity.meetingroom.floor", "en-US", "楼层_us", "楼层"),
            // entity.meetingroom.floor
            new TranslationSeedItem("entity.meetingroom.floor", "ja-JP", "楼层_jp", "楼层"),
            // entity.meetingroom.floor
            new TranslationSeedItem("entity.meetingroom.floor", "zh-CN", "楼层", "楼层"),
            // entity.meetingroom.floor
            new TranslationSeedItem("entity.meetingroom.floor", "zh-HK", "楼层_hk", "楼层"),

            // entity.meetingroom.locationdetail
            new TranslationSeedItem("entity.meetingroom.locationdetail", "en-US", "详细位置_us", "详细位置说明"),
            // entity.meetingroom.locationdetail
            new TranslationSeedItem("entity.meetingroom.locationdetail", "ja-JP", "详细位置_jp", "详细位置说明"),
            // entity.meetingroom.locationdetail
            new TranslationSeedItem("entity.meetingroom.locationdetail", "zh-CN", "详细位置", "详细位置说明"),
            // entity.meetingroom.locationdetail
            new TranslationSeedItem("entity.meetingroom.locationdetail", "zh-HK", "详细位置_hk", "详细位置说明"),

            // entity.meetingroom.capacity
            new TranslationSeedItem("entity.meetingroom.capacity", "en-US", "容纳人数_us", "容纳人数（0 表示不限）"),
            // entity.meetingroom.capacity
            new TranslationSeedItem("entity.meetingroom.capacity", "ja-JP", "容纳人数_jp", "容纳人数（0 表示不限）"),
            // entity.meetingroom.capacity
            new TranslationSeedItem("entity.meetingroom.capacity", "zh-CN", "容纳人数", "容纳人数（0 表示不限）"),
            // entity.meetingroom.capacity
            new TranslationSeedItem("entity.meetingroom.capacity", "zh-HK", "容纳人数_hk", "容纳人数（0 表示不限）"),

            // entity.meetingroom.facilities
            new TranslationSeedItem("entity.meetingroom.facilities", "en-US", "设施说明_us", "设施说明（投影、视频会议设备等）"),
            // entity.meetingroom.facilities
            new TranslationSeedItem("entity.meetingroom.facilities", "ja-JP", "设施说明_jp", "设施说明（投影、视频会议设备等）"),
            // entity.meetingroom.facilities
            new TranslationSeedItem("entity.meetingroom.facilities", "zh-CN", "设施说明", "设施说明（投影、视频会议设备等）"),
            // entity.meetingroom.facilities
            new TranslationSeedItem("entity.meetingroom.facilities", "zh-HK", "设施说明_hk", "设施说明（投影、视频会议设备等）"),

            // entity.meetingroom.sortorder
            new TranslationSeedItem("entity.meetingroom.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.meetingroom.sortorder
            new TranslationSeedItem("entity.meetingroom.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.meetingroom.sortorder
            new TranslationSeedItem("entity.meetingroom.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.meetingroom.sortorder
            new TranslationSeedItem("entity.meetingroom.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.meetingroom.roomstatus
            new TranslationSeedItem("entity.meetingroom.roomstatus", "en-US", "会议室状态_us", "会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.meetingroom.roomstatus
            new TranslationSeedItem("entity.meetingroom.roomstatus", "ja-JP", "会议室状态_jp", "会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.meetingroom.roomstatus
            new TranslationSeedItem("entity.meetingroom.roomstatus", "zh-CN", "会议室状态", "会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.meetingroom.roomstatus
            new TranslationSeedItem("entity.meetingroom.roomstatus", "zh-HK", "会议室状态_hk", "会议室状态（字典 routine_meeting_center_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
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
