// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNews 实体字段国际化种子（已对齐前端 locales：src/locales/routine/news-center/news）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter;

/// <summary>
/// TaktNews 实体国际化翻译种子（键前缀 entity.news.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNews 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 news 实体翻译...", tenantCode);

        foreach (var item in GetNewsTranslations())
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

        TaktLogger.Information("TaktNews 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNews 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.news._self / entity.news.{{field}}；ResourceGroup=NewsCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "en-US", "News Information_us", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "ja-JP", "新闻中心主信息_jp", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "zh-CN", "新闻中心主信息", "实体名称"),
            // entity.news._self
            new TranslationSeedItem("entity.news._self", "zh-HK", "新闻中心主信息_hk", "实体名称"),

            // entity.news.code
            new TranslationSeedItem("entity.news.code", "en-US", "新闻编码_us", "新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻中心）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "ja-JP", "新闻编码_jp", "新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻中心）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "zh-CN", "新闻编码", "新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻中心）"),
            // entity.news.code
            new TranslationSeedItem("entity.news.code", "zh-HK", "新闻编码_hk", "新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻中心）"),

            // entity.news.category
            new TranslationSeedItem("entity.news.category", "en-US", "新闻分类_us", "新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "ja-JP", "新闻分类_jp", "新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "zh-CN", "新闻分类", "新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）"),
            // entity.news.category
            new TranslationSeedItem("entity.news.category", "zh-HK", "新闻分类_hk", "新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）"),

            // entity.news.title
            new TranslationSeedItem("entity.news.title", "en-US", "新闻标题_us", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "ja-JP", "新闻标题_jp", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "zh-CN", "新闻标题", "新闻标题"),
            // entity.news.title
            new TranslationSeedItem("entity.news.title", "zh-HK", "新闻标题_hk", "新闻标题"),

            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "en-US", "新闻摘要_us", "新闻摘要（用于列表展示）"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "ja-JP", "新闻摘要_jp", "新闻摘要（用于列表展示）"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "zh-CN", "新闻摘要", "新闻摘要（用于列表展示）"),
            // entity.news.summary
            new TranslationSeedItem("entity.news.summary", "zh-HK", "新闻摘要_hk", "新闻摘要（用于列表展示）"),

            // entity.news.tags
            new TranslationSeedItem("entity.news.tags", "en-US", "标签_us", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.news.tags
            new TranslationSeedItem("entity.news.tags", "ja-JP", "标签_jp", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.news.tags
            new TranslationSeedItem("entity.news.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.news.tags
            new TranslationSeedItem("entity.news.tags", "zh-HK", "标签_hk", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.news.content
            new TranslationSeedItem("entity.news.content", "en-US", "新闻内容_us", "新闻内容（富文本 HTML；插图随正文存储，无独立附件）"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "ja-JP", "新闻内容_jp", "新闻内容（富文本 HTML；插图随正文存储，无独立附件）"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "zh-CN", "新闻内容", "新闻内容（富文本 HTML；插图随正文存储，无独立附件）"),
            // entity.news.content
            new TranslationSeedItem("entity.news.content", "zh-HK", "新闻内容_hk", "新闻内容（富文本 HTML；插图随正文存储，无独立附件）"),

            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "en-US", "新闻封面图片URL_us", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "ja-JP", "新闻封面图片URL_jp", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "zh-CN", "新闻封面图片URL", "新闻封面图片 URL"),
            // entity.news.coverimage
            new TranslationSeedItem("entity.news.coverimage", "zh-HK", "新闻封面图片URL_hk", "新闻封面图片 URL"),

            // entity.news.istop
            new TranslationSeedItem("entity.news.istop", "en-US", "置顶_us", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.istop
            new TranslationSeedItem("entity.news.istop", "ja-JP", "置顶_jp", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.istop
            new TranslationSeedItem("entity.news.istop", "zh-CN", "置顶", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.istop
            new TranslationSeedItem("entity.news.istop", "zh-HK", "置顶_hk", "置顶（字典 sys_yes_no；0=否 1=是）"),

            // entity.news.isrecommended
            new TranslationSeedItem("entity.news.isrecommended", "en-US", "推荐_us", "推荐（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.isrecommended
            new TranslationSeedItem("entity.news.isrecommended", "ja-JP", "推荐_jp", "推荐（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.isrecommended
            new TranslationSeedItem("entity.news.isrecommended", "zh-CN", "推荐", "推荐（字典 sys_yes_no；0=否 1=是）"),
            // entity.news.isrecommended
            new TranslationSeedItem("entity.news.isrecommended", "zh-HK", "推荐_hk", "推荐（字典 sys_yes_no；0=否 1=是）"),

            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "en-US", "生效时间_us", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "ja-JP", "生效时间_jp", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "zh-CN", "生效时间", "生效时间"),
            // entity.news.effectivetime
            new TranslationSeedItem("entity.news.effectivetime", "zh-HK", "生效时间_hk", "生效时间"),

            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "en-US", "失效时间_us", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "ja-JP", "失效时间_jp", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "zh-CN", "失效时间", "失效时间"),
            // entity.news.expiretime
            new TranslationSeedItem("entity.news.expiretime", "zh-HK", "失效时间_hk", "失效时间"),

            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "en-US", "阅读次数_us", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "ja-JP", "阅读次数_jp", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "zh-CN", "阅读次数", "阅读次数"),
            // entity.news.readcount
            new TranslationSeedItem("entity.news.readcount", "zh-HK", "阅读次数_hk", "阅读次数"),

            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "en-US", "点赞次数_us", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "ja-JP", "点赞次数_jp", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "zh-CN", "点赞次数", "点赞次数"),
            // entity.news.likecount
            new TranslationSeedItem("entity.news.likecount", "zh-HK", "点赞次数_hk", "点赞次数"),

            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "en-US", "评论次数_us", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "ja-JP", "评论次数_jp", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "zh-CN", "评论次数", "评论次数"),
            // entity.news.commentcount
            new TranslationSeedItem("entity.news.commentcount", "zh-HK", "评论次数_hk", "评论次数"),

            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "en-US", "收藏次数_us", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "ja-JP", "收藏次数_jp", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "zh-CN", "收藏次数", "收藏次数"),
            // entity.news.favoritecount
            new TranslationSeedItem("entity.news.favoritecount", "zh-HK", "收藏次数_hk", "收藏次数"),

            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "en-US", "分享次数_us", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "ja-JP", "分享次数_jp", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "zh-CN", "分享次数", "分享次数"),
            // entity.news.sharecount
            new TranslationSeedItem("entity.news.sharecount", "zh-HK", "分享次数_hk", "分享次数"),

            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "en-US", "发布部门ID_us", "发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "ja-JP", "发布部门ID_jp", "发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "zh-CN", "发布部门ID", "发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.news.deptid
            new TranslationSeedItem("entity.news.deptid", "zh-HK", "发布部门ID_hk", "发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "en-US", "发布部门名称_us", "发布部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "ja-JP", "发布部门名称_jp", "发布部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "zh-CN", "发布部门名称", "发布部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.deptname
            new TranslationSeedItem("entity.news.deptname", "zh-HK", "发布部门名称_hk", "发布部门名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "en-US", "发布人ID_us", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "ja-JP", "发布人ID_jp", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "zh-CN", "发布人ID", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.news.publisherid
            new TranslationSeedItem("entity.news.publisherid", "zh-HK", "发布人ID_hk", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "en-US", "发布人姓名_us", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "ja-JP", "发布人姓名_jp", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "zh-CN", "发布人姓名", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.news.publishername
            new TranslationSeedItem("entity.news.publishername", "zh-HK", "发布人姓名_hk", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "en-US", "发布时间_us", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "ja-JP", "发布时间_jp", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "zh-CN", "发布时间", "发布时间"),
            // entity.news.publishtime
            new TranslationSeedItem("entity.news.publishtime", "zh-HK", "发布时间_hk", "发布时间"),

            // entity.news.targetscope
            new TranslationSeedItem("entity.news.targetscope", "en-US", "目标范围_us", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.news.targetscope
            new TranslationSeedItem("entity.news.targetscope", "ja-JP", "目标范围_jp", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.news.targetscope
            new TranslationSeedItem("entity.news.targetscope", "zh-CN", "目标范围", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.news.targetscope
            new TranslationSeedItem("entity.news.targetscope", "zh-HK", "目标范围_hk", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),

            // entity.news.targetdepartments
            new TranslationSeedItem("entity.news.targetdepartments", "en-US", "目标部门编码_us", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.news.targetdepartments
            new TranslationSeedItem("entity.news.targetdepartments", "ja-JP", "目标部门编码_jp", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.news.targetdepartments
            new TranslationSeedItem("entity.news.targetdepartments", "zh-CN", "目标部门编码", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.news.targetdepartments
            new TranslationSeedItem("entity.news.targetdepartments", "zh-HK", "目标部门编码_hk", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),

            // entity.news.targetusers
            new TranslationSeedItem("entity.news.targetusers", "en-US", "目标用户名_us", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.news.targetusers
            new TranslationSeedItem("entity.news.targetusers", "ja-JP", "目标用户名_jp", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.news.targetusers
            new TranslationSeedItem("entity.news.targetusers", "zh-CN", "目标用户名", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.news.targetusers
            new TranslationSeedItem("entity.news.targetusers", "zh-HK", "目标用户名_hk", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),

            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.news.sortorder
            new TranslationSeedItem("entity.news.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.news.status
            new TranslationSeedItem("entity.news.status", "en-US", "新闻状态_us", "新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "ja-JP", "新闻状态_jp", "新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "zh-CN", "新闻状态", "新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.news.status
            new TranslationSeedItem("entity.news.status", "zh-HK", "新闻状态_hk", "新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),

            // entity.news.comments
            new TranslationSeedItem("entity.news.comments", "en-US", "新闻评论列表_us", "新闻评论列表（主子表关系）"),
            // entity.news.comments
            new TranslationSeedItem("entity.news.comments", "ja-JP", "新闻评论列表_jp", "新闻评论列表（主子表关系）"),
            // entity.news.comments
            new TranslationSeedItem("entity.news.comments", "zh-CN", "新闻评论列表", "新闻评论列表（主子表关系）"),
            // entity.news.comments
            new TranslationSeedItem("entity.news.comments", "zh-HK", "新闻评论列表_hk", "新闻评论列表（主子表关系）"),

            // entity.news.likes
            new TranslationSeedItem("entity.news.likes", "en-US", "新闻点赞记录列表_us", "新闻点赞记录列表（主子表关系）"),
            // entity.news.likes
            new TranslationSeedItem("entity.news.likes", "ja-JP", "新闻点赞记录列表_jp", "新闻点赞记录列表（主子表关系）"),
            // entity.news.likes
            new TranslationSeedItem("entity.news.likes", "zh-CN", "新闻点赞记录列表", "新闻点赞记录列表（主子表关系）"),
            // entity.news.likes
            new TranslationSeedItem("entity.news.likes", "zh-HK", "新闻点赞记录列表_hk", "新闻点赞记录列表（主子表关系）"),

            // entity.news.reads
            new TranslationSeedItem("entity.news.reads", "en-US", "新闻阅读记录列表_us", "新闻阅读记录列表（主子表关系）"),
            // entity.news.reads
            new TranslationSeedItem("entity.news.reads", "ja-JP", "新闻阅读记录列表_jp", "新闻阅读记录列表（主子表关系）"),
            // entity.news.reads
            new TranslationSeedItem("entity.news.reads", "zh-CN", "新闻阅读记录列表", "新闻阅读记录列表（主子表关系）"),
            // entity.news.reads
            new TranslationSeedItem("entity.news.reads", "zh-HK", "新闻阅读记录列表_hk", "新闻阅读记录列表（主子表关系）"),

            // entity.news.favorites
            new TranslationSeedItem("entity.news.favorites", "en-US", "新闻收藏记录列表_us", "新闻收藏记录列表（主子表关系）"),
            // entity.news.favorites
            new TranslationSeedItem("entity.news.favorites", "ja-JP", "新闻收藏记录列表_jp", "新闻收藏记录列表（主子表关系）"),
            // entity.news.favorites
            new TranslationSeedItem("entity.news.favorites", "zh-CN", "新闻收藏记录列表", "新闻收藏记录列表（主子表关系）"),
            // entity.news.favorites
            new TranslationSeedItem("entity.news.favorites", "zh-HK", "新闻收藏记录列表_hk", "新闻收藏记录列表（主子表关系）"),

            // entity.news.shares
            new TranslationSeedItem("entity.news.shares", "en-US", "新闻分享记录列表_us", "新闻分享记录列表（主子表关系）"),
            // entity.news.shares
            new TranslationSeedItem("entity.news.shares", "ja-JP", "新闻分享记录列表_jp", "新闻分享记录列表（主子表关系）"),
            // entity.news.shares
            new TranslationSeedItem("entity.news.shares", "zh-CN", "新闻分享记录列表", "新闻分享记录列表（主子表关系）"),
            // entity.news.shares
            new TranslationSeedItem("entity.news.shares", "zh-HK", "新闻分享记录列表_hk", "新闻分享记录列表（主子表关系）"),
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
        translation.ResourceGroup = "NewsCenter";
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
