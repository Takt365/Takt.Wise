// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutesI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMeetingMinutes 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMeetingMinutes 实体国际化翻译种子（键前缀 entity.meetingminutes.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMeetingMinutesI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMeetingMinutes 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 meetingminutes 实体翻译...", tenantCode);

        foreach (var item in GetMeetingMinutesTranslations())
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

        TaktLogger.Information("TaktMeetingMinutes 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMeetingMinutes 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.meetingminutes._self / entity.meetingminutes.{{field}}；ResourceGroup=MeetingCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMeetingMinutesTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.meetingminutes._self
            new TranslationSeedItem("entity.meetingminutes._self", "en-US", "Meeting Minutes Information_us", "实体名称"),
            // entity.meetingminutes._self
            new TranslationSeedItem("entity.meetingminutes._self", "ja-JP", "会后纪要信息_jp", "实体名称"),
            // entity.meetingminutes._self
            new TranslationSeedItem("entity.meetingminutes._self", "zh-CN", "会后纪要信息", "实体名称"),
            // entity.meetingminutes._self
            new TranslationSeedItem("entity.meetingminutes._self", "zh-HK", "会后纪要信息_hk", "实体名称"),

            // entity.meetingminutes.meetingid
            new TranslationSeedItem("entity.meetingminutes.meetingid", "en-US", "会议ID_us", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingminutes.meetingid
            new TranslationSeedItem("entity.meetingminutes.meetingid", "ja-JP", "会议ID_jp", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingminutes.meetingid
            new TranslationSeedItem("entity.meetingminutes.meetingid", "zh-CN", "会议ID", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),
            // entity.meetingminutes.meetingid
            new TranslationSeedItem("entity.meetingminutes.meetingid", "zh-HK", "会议ID_hk", "会议 ID（选项 TaktMeetings/options；DictValue=Id）"),

            // entity.meetingminutes.meetingtitle
            new TranslationSeedItem("entity.meetingminutes.meetingtitle", "en-US", "会议标题_us", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingminutes.meetingtitle
            new TranslationSeedItem("entity.meetingminutes.meetingtitle", "ja-JP", "会议标题_jp", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingminutes.meetingtitle
            new TranslationSeedItem("entity.meetingminutes.meetingtitle", "zh-CN", "会议标题", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),
            // entity.meetingminutes.meetingtitle
            new TranslationSeedItem("entity.meetingminutes.meetingtitle", "zh-HK", "会议标题_hk", "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）"),

            // entity.meetingminutes.linenumber
            new TranslationSeedItem("entity.meetingminutes.linenumber", "en-US", "行号_us", "行号（纪要分项序号，固定步长=10；纪要通常为 10）"),
            // entity.meetingminutes.linenumber
            new TranslationSeedItem("entity.meetingminutes.linenumber", "ja-JP", "行号_jp", "行号（纪要分项序号，固定步长=10；纪要通常为 10）"),
            // entity.meetingminutes.linenumber
            new TranslationSeedItem("entity.meetingminutes.linenumber", "zh-CN", "行号", "行号（纪要分项序号，固定步长=10；纪要通常为 10）"),
            // entity.meetingminutes.linenumber
            new TranslationSeedItem("entity.meetingminutes.linenumber", "zh-HK", "行号_hk", "行号（纪要分项序号，固定步长=10；纪要通常为 10）"),

            // entity.meetingminutes.meetingminutes
            new TranslationSeedItem("entity.meetingminutes.meetingminutes", "en-US", "会议纪要_us", "会议纪要（会后纪要富文本 HTML）"),
            // entity.meetingminutes.meetingminutes
            new TranslationSeedItem("entity.meetingminutes.meetingminutes", "ja-JP", "会议纪要_jp", "会议纪要（会后纪要富文本 HTML）"),
            // entity.meetingminutes.meetingminutes
            new TranslationSeedItem("entity.meetingminutes.meetingminutes", "zh-CN", "会议纪要", "会议纪要（会后纪要富文本 HTML）"),
            // entity.meetingminutes.meetingminutes
            new TranslationSeedItem("entity.meetingminutes.meetingminutes", "zh-HK", "会议纪要_hk", "会议纪要（会后纪要富文本 HTML）"),

            // entity.meetingminutes.meetingsummary
            new TranslationSeedItem("entity.meetingminutes.meetingsummary", "en-US", "摘要_us", "摘要（纪要列表展示用）"),
            // entity.meetingminutes.meetingsummary
            new TranslationSeedItem("entity.meetingminutes.meetingsummary", "ja-JP", "摘要_jp", "摘要（纪要列表展示用）"),
            // entity.meetingminutes.meetingsummary
            new TranslationSeedItem("entity.meetingminutes.meetingsummary", "zh-CN", "摘要", "摘要（纪要列表展示用）"),
            // entity.meetingminutes.meetingsummary
            new TranslationSeedItem("entity.meetingminutes.meetingsummary", "zh-HK", "摘要_hk", "摘要（纪要列表展示用）"),

            // entity.meetingminutes.recorderid
            new TranslationSeedItem("entity.meetingminutes.recorderid", "en-US", "记录ID_us", "记录 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingminutes.recorderid
            new TranslationSeedItem("entity.meetingminutes.recorderid", "ja-JP", "记录ID_jp", "记录 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingminutes.recorderid
            new TranslationSeedItem("entity.meetingminutes.recorderid", "zh-CN", "记录ID", "记录 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.meetingminutes.recorderid
            new TranslationSeedItem("entity.meetingminutes.recorderid", "zh-HK", "记录ID_hk", "记录 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.meetingminutes.recordername
            new TranslationSeedItem("entity.meetingminutes.recordername", "en-US", "记录员_us", "记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）"),
            // entity.meetingminutes.recordername
            new TranslationSeedItem("entity.meetingminutes.recordername", "ja-JP", "记录员_jp", "记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）"),
            // entity.meetingminutes.recordername
            new TranslationSeedItem("entity.meetingminutes.recordername", "zh-CN", "记录员", "记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）"),
            // entity.meetingminutes.recordername
            new TranslationSeedItem("entity.meetingminutes.recordername", "zh-HK", "记录员_hk", "记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）"),

            // entity.meetingminutes.filename
            new TranslationSeedItem("entity.meetingminutes.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.meetingminutes.filename
            new TranslationSeedItem("entity.meetingminutes.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.meetingminutes.filename
            new TranslationSeedItem("entity.meetingminutes.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.meetingminutes.filename
            new TranslationSeedItem("entity.meetingminutes.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.meetingminutes.accessurl
            new TranslationSeedItem("entity.meetingminutes.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.meetingminutes.accessurl
            new TranslationSeedItem("entity.meetingminutes.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.meetingminutes.accessurl
            new TranslationSeedItem("entity.meetingminutes.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.meetingminutes.accessurl
            new TranslationSeedItem("entity.meetingminutes.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.meetingminutes.isobsolete
            new TranslationSeedItem("entity.meetingminutes.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingminutes.isobsolete
            new TranslationSeedItem("entity.meetingminutes.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingminutes.isobsolete
            new TranslationSeedItem("entity.meetingminutes.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.meetingminutes.isobsolete
            new TranslationSeedItem("entity.meetingminutes.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.meetingminutes.meeting
            new TranslationSeedItem("entity.meetingminutes.meeting", "en-US", "会议_us", "会议（主表）"),
            // entity.meetingminutes.meeting
            new TranslationSeedItem("entity.meetingminutes.meeting", "ja-JP", "会议_jp", "会议（主表）"),
            // entity.meetingminutes.meeting
            new TranslationSeedItem("entity.meetingminutes.meeting", "zh-CN", "会议", "会议（主表）"),
            // entity.meetingminutes.meeting
            new TranslationSeedItem("entity.meetingminutes.meeting", "zh-HK", "会议_hk", "会议（主表）"),
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
