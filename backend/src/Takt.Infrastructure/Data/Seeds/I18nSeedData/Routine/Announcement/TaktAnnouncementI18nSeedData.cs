// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.Announcement
// 文件名称：TaktAnnouncementI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAnnouncement 实体字段国际化种子（已对齐前端 locales：src/locales/routine/announcement）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.Announcement;

/// <summary>
/// TaktAnnouncement 实体国际化翻译种子（键前缀 entity.announcement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAnnouncementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAnnouncement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 announcement 实体翻译...", tenantCode);

        foreach (var item in GetAnnouncementTranslations())
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

        TaktLogger.Information("TaktAnnouncement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAnnouncement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.announcement._self / entity.announcement.{{field}}；ResourceGroup=Announcement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAnnouncementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "en-US", "Announcement Information_us", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "ja-JP", "公告通知信息_jp", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "zh-CN", "公告通知信息", "实体名称"),
            // entity.announcement._self
            new TranslationSeedItem("entity.announcement._self", "zh-HK", "公告通知信息_hk", "实体名称"),

            // entity.announcement.code
            new TranslationSeedItem("entity.announcement.code", "en-US", "公告编码_us", "公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）"),
            // entity.announcement.code
            new TranslationSeedItem("entity.announcement.code", "ja-JP", "公告编码_jp", "公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）"),
            // entity.announcement.code
            new TranslationSeedItem("entity.announcement.code", "zh-CN", "公告编码", "公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）"),
            // entity.announcement.code
            new TranslationSeedItem("entity.announcement.code", "zh-HK", "公告编码_hk", "公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）"),

            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "en-US", "公告标题_us", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "ja-JP", "公告标题_jp", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "zh-CN", "公告标题", "公告标题"),
            // entity.announcement.title
            new TranslationSeedItem("entity.announcement.title", "zh-HK", "公告标题_hk", "公告标题"),

            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "en-US", "公告类型_us", "公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "ja-JP", "公告类型_jp", "公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "zh-CN", "公告类型", "公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）"),
            // entity.announcement.type
            new TranslationSeedItem("entity.announcement.type", "zh-HK", "公告类型_hk", "公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）"),

            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "en-US", "公告内容_us", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "ja-JP", "公告内容_jp", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "zh-CN", "公告内容", "公告内容（富文本 HTML）"),
            // entity.announcement.content
            new TranslationSeedItem("entity.announcement.content", "zh-HK", "公告内容_hk", "公告内容（富文本 HTML）"),

            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "en-US", "公告摘要_us", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "ja-JP", "公告摘要_jp", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "zh-CN", "公告摘要", "公告摘要（用于列表展示）"),
            // entity.announcement.summary
            new TranslationSeedItem("entity.announcement.summary", "zh-HK", "公告摘要_hk", "公告摘要（用于列表展示）"),

            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "en-US", "标签_us", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "ja-JP", "标签_jp", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.announcement.tags
            new TranslationSeedItem("entity.announcement.tags", "zh-HK", "标签_hk", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.announcement.filename
            new TranslationSeedItem("entity.announcement.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.announcement.filename
            new TranslationSeedItem("entity.announcement.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.announcement.filename
            new TranslationSeedItem("entity.announcement.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.announcement.filename
            new TranslationSeedItem("entity.announcement.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.announcement.accessurl
            new TranslationSeedItem("entity.announcement.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.announcement.accessurl
            new TranslationSeedItem("entity.announcement.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.announcement.accessurl
            new TranslationSeedItem("entity.announcement.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.announcement.accessurl
            new TranslationSeedItem("entity.announcement.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "en-US", "发布时间_us", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "ja-JP", "发布时间_jp", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "zh-CN", "发布时间", "发布时间（定时发布时使用）"),
            // entity.announcement.publishtime
            new TranslationSeedItem("entity.announcement.publishtime", "zh-HK", "发布时间_hk", "发布时间（定时发布时使用）"),

            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "en-US", "定时发布_us", "定时发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "ja-JP", "定时发布_jp", "定时发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "zh-CN", "定时发布", "定时发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.isscheduled
            new TranslationSeedItem("entity.announcement.isscheduled", "zh-HK", "定时发布_hk", "定时发布（字典 sys_yes_no；0=否 1=是）"),

            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "en-US", "置顶_us", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "ja-JP", "置顶_jp", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "zh-CN", "置顶", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.announcement.istop
            new TranslationSeedItem("entity.announcement.istop", "zh-HK", "置顶_hk", "置顶（字典 sys_yes_no；0=否 1=是）"),

            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "en-US", "置顶优先级_us", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "ja-JP", "置顶优先级_jp", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "zh-CN", "置顶优先级", "置顶优先级（数字越大越靠前）"),
            // entity.announcement.toppriority
            new TranslationSeedItem("entity.announcement.toppriority", "zh-HK", "置顶优先级_hk", "置顶优先级（数字越大越靠前）"),

            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "en-US", "过期时间_us", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "ja-JP", "过期时间_jp", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "zh-CN", "过期时间", "过期时间（过期后自动隐藏）"),
            // entity.announcement.expiretime
            new TranslationSeedItem("entity.announcement.expiretime", "zh-HK", "过期时间_hk", "过期时间（过期后自动隐藏）"),

            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "en-US", "查看次数_us", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "ja-JP", "查看次数_jp", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "zh-CN", "查看次数", "查看次数"),
            // entity.announcement.viewcount
            new TranslationSeedItem("entity.announcement.viewcount", "zh-HK", "查看次数_hk", "查看次数"),

            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "en-US", "目标范围_us", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "ja-JP", "目标范围_jp", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "zh-CN", "目标范围", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.announcement.targetscope
            new TranslationSeedItem("entity.announcement.targetscope", "zh-HK", "目标范围_hk", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),

            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "en-US", "目标部门编码_us", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "ja-JP", "目标部门编码_jp", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "zh-CN", "目标部门编码", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.announcement.targetdepartments
            new TranslationSeedItem("entity.announcement.targetdepartments", "zh-HK", "目标部门编码_hk", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),

            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "en-US", "目标用户名_us", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "ja-JP", "目标用户名_jp", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "zh-CN", "目标用户名", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.announcement.targetusers
            new TranslationSeedItem("entity.announcement.targetusers", "zh-HK", "目标用户名_hk", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),

            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "en-US", "状态_us", "状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "ja-JP", "状态_jp", "状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "zh-CN", "状态", "状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.announcement.status
            new TranslationSeedItem("entity.announcement.status", "zh-HK", "状态_hk", "状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
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
        translation.ResourceGroup = "Announcement";
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
