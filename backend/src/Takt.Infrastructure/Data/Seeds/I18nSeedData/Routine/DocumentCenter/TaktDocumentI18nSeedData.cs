// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter
// 文件名称：TaktDocumentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDocument 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter;

/// <summary>
/// TaktDocument 实体国际化翻译种子（键前缀 entity.document.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDocumentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDocument 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 document 实体翻译...", tenantCode);

        foreach (var item in GetDocumentTranslations())
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

        TaktLogger.Information("TaktDocument 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDocument 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.document._self / entity.document.{{field}}；ResourceGroup=DocumentCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.document._self
            new TranslationSeedItem("entity.document._self", "en-US", "Document Information_us", "实体名称"),
            // entity.document._self
            new TranslationSeedItem("entity.document._self", "ja-JP", "文管中心主信息_jp", "实体名称"),
            // entity.document._self
            new TranslationSeedItem("entity.document._self", "zh-CN", "文管中心主信息", "实体名称"),
            // entity.document._self
            new TranslationSeedItem("entity.document._self", "zh-HK", "文管中心主信息_hk", "实体名称"),

            // entity.document.code
            new TranslationSeedItem("entity.document.code", "en-US", "文档编码_us", "文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文管中心）"),
            // entity.document.code
            new TranslationSeedItem("entity.document.code", "ja-JP", "文档编码_jp", "文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文管中心）"),
            // entity.document.code
            new TranslationSeedItem("entity.document.code", "zh-CN", "文档编码", "文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文管中心）"),
            // entity.document.code
            new TranslationSeedItem("entity.document.code", "zh-HK", "文档编码_hk", "文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文管中心）"),

            // entity.document.title
            new TranslationSeedItem("entity.document.title", "en-US", "文档标题_us", "文档标题"),
            // entity.document.title
            new TranslationSeedItem("entity.document.title", "ja-JP", "文档标题_jp", "文档标题"),
            // entity.document.title
            new TranslationSeedItem("entity.document.title", "zh-CN", "文档标题", "文档标题"),
            // entity.document.title
            new TranslationSeedItem("entity.document.title", "zh-HK", "文档标题_hk", "文档标题"),

            // entity.document.category
            new TranslationSeedItem("entity.document.category", "en-US", "文档分类_us", "文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）"),
            // entity.document.category
            new TranslationSeedItem("entity.document.category", "ja-JP", "文档分类_jp", "文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）"),
            // entity.document.category
            new TranslationSeedItem("entity.document.category", "zh-CN", "文档分类", "文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）"),
            // entity.document.category
            new TranslationSeedItem("entity.document.category", "zh-HK", "文档分类_hk", "文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）"),

            // entity.document.confidentiallevel
            new TranslationSeedItem("entity.document.confidentiallevel", "en-US", "密级_us", "密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）"),
            // entity.document.confidentiallevel
            new TranslationSeedItem("entity.document.confidentiallevel", "ja-JP", "密级_jp", "密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）"),
            // entity.document.confidentiallevel
            new TranslationSeedItem("entity.document.confidentiallevel", "zh-CN", "密级", "密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）"),
            // entity.document.confidentiallevel
            new TranslationSeedItem("entity.document.confidentiallevel", "zh-HK", "密级_hk", "密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）"),

            // entity.document.version
            new TranslationSeedItem("entity.document.version", "en-US", "当前版本号_us", "当前版本号"),
            // entity.document.version
            new TranslationSeedItem("entity.document.version", "ja-JP", "当前版本号_jp", "当前版本号"),
            // entity.document.version
            new TranslationSeedItem("entity.document.version", "zh-CN", "当前版本号", "当前版本号"),
            // entity.document.version
            new TranslationSeedItem("entity.document.version", "zh-HK", "当前版本号_hk", "当前版本号"),

            // entity.document.content
            new TranslationSeedItem("entity.document.content", "en-US", "文档内容_us", "文档内容（富文本 HTML）"),
            // entity.document.content
            new TranslationSeedItem("entity.document.content", "ja-JP", "文档内容_jp", "文档内容（富文本 HTML）"),
            // entity.document.content
            new TranslationSeedItem("entity.document.content", "zh-CN", "文档内容", "文档内容（富文本 HTML）"),
            // entity.document.content
            new TranslationSeedItem("entity.document.content", "zh-HK", "文档内容_hk", "文档内容（富文本 HTML）"),

            // entity.document.summary
            new TranslationSeedItem("entity.document.summary", "en-US", "文档摘要_us", "文档摘要（用于列表展示）"),
            // entity.document.summary
            new TranslationSeedItem("entity.document.summary", "ja-JP", "文档摘要_jp", "文档摘要（用于列表展示）"),
            // entity.document.summary
            new TranslationSeedItem("entity.document.summary", "zh-CN", "文档摘要", "文档摘要（用于列表展示）"),
            // entity.document.summary
            new TranslationSeedItem("entity.document.summary", "zh-HK", "文档摘要_hk", "文档摘要（用于列表展示）"),

            // entity.document.tags
            new TranslationSeedItem("entity.document.tags", "en-US", "标签_us", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.document.tags
            new TranslationSeedItem("entity.document.tags", "ja-JP", "标签_jp", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.document.tags
            new TranslationSeedItem("entity.document.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.document.tags
            new TranslationSeedItem("entity.document.tags", "zh-HK", "标签_hk", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.document.filename
            new TranslationSeedItem("entity.document.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.document.filename
            new TranslationSeedItem("entity.document.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.document.filename
            new TranslationSeedItem("entity.document.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.document.filename
            new TranslationSeedItem("entity.document.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.document.accessurl
            new TranslationSeedItem("entity.document.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.document.accessurl
            new TranslationSeedItem("entity.document.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.document.accessurl
            new TranslationSeedItem("entity.document.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.document.accessurl
            new TranslationSeedItem("entity.document.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.document.effectivetime
            new TranslationSeedItem("entity.document.effectivetime", "en-US", "生效时间_us", "生效时间"),
            // entity.document.effectivetime
            new TranslationSeedItem("entity.document.effectivetime", "ja-JP", "生效时间_jp", "生效时间"),
            // entity.document.effectivetime
            new TranslationSeedItem("entity.document.effectivetime", "zh-CN", "生效时间", "生效时间"),
            // entity.document.effectivetime
            new TranslationSeedItem("entity.document.effectivetime", "zh-HK", "生效时间_hk", "生效时间"),

            // entity.document.expiretime
            new TranslationSeedItem("entity.document.expiretime", "en-US", "失效时间_us", "失效时间"),
            // entity.document.expiretime
            new TranslationSeedItem("entity.document.expiretime", "ja-JP", "失效时间_jp", "失效时间"),
            // entity.document.expiretime
            new TranslationSeedItem("entity.document.expiretime", "zh-CN", "失效时间", "失效时间"),
            // entity.document.expiretime
            new TranslationSeedItem("entity.document.expiretime", "zh-HK", "失效时间_hk", "失效时间"),

            // entity.document.publishtime
            new TranslationSeedItem("entity.document.publishtime", "en-US", "发布时间_us", "发布时间"),
            // entity.document.publishtime
            new TranslationSeedItem("entity.document.publishtime", "ja-JP", "发布时间_jp", "发布时间"),
            // entity.document.publishtime
            new TranslationSeedItem("entity.document.publishtime", "zh-CN", "发布时间", "发布时间"),
            // entity.document.publishtime
            new TranslationSeedItem("entity.document.publishtime", "zh-HK", "发布时间_hk", "发布时间"),

            // entity.document.publisherid
            new TranslationSeedItem("entity.document.publisherid", "en-US", "发布人ID_us", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.document.publisherid
            new TranslationSeedItem("entity.document.publisherid", "ja-JP", "发布人ID_jp", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.document.publisherid
            new TranslationSeedItem("entity.document.publisherid", "zh-CN", "发布人ID", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.document.publisherid
            new TranslationSeedItem("entity.document.publisherid", "zh-HK", "发布人ID_hk", "发布人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.document.publishername
            new TranslationSeedItem("entity.document.publishername", "en-US", "发布人姓名_us", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.publishername
            new TranslationSeedItem("entity.document.publishername", "ja-JP", "发布人姓名_jp", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.publishername
            new TranslationSeedItem("entity.document.publishername", "zh-CN", "发布人姓名", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.publishername
            new TranslationSeedItem("entity.document.publishername", "zh-HK", "发布人姓名_hk", "发布人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.document.deptid
            new TranslationSeedItem("entity.document.deptid", "en-US", "归属部门ID_us", "归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.document.deptid
            new TranslationSeedItem("entity.document.deptid", "ja-JP", "归属部门ID_jp", "归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.document.deptid
            new TranslationSeedItem("entity.document.deptid", "zh-CN", "归属部门ID", "归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.document.deptid
            new TranslationSeedItem("entity.document.deptid", "zh-HK", "归属部门ID_hk", "归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.document.deptname
            new TranslationSeedItem("entity.document.deptname", "en-US", "归属部门名称_us", "归属部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.deptname
            new TranslationSeedItem("entity.document.deptname", "ja-JP", "归属部门名称_jp", "归属部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.deptname
            new TranslationSeedItem("entity.document.deptname", "zh-CN", "归属部门名称", "归属部门名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.document.deptname
            new TranslationSeedItem("entity.document.deptname", "zh-HK", "归属部门名称_hk", "归属部门名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.document.istop
            new TranslationSeedItem("entity.document.istop", "en-US", "置顶_us", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.document.istop
            new TranslationSeedItem("entity.document.istop", "ja-JP", "置顶_jp", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.document.istop
            new TranslationSeedItem("entity.document.istop", "zh-CN", "置顶", "置顶（字典 sys_yes_no；0=否 1=是）"),
            // entity.document.istop
            new TranslationSeedItem("entity.document.istop", "zh-HK", "置顶_hk", "置顶（字典 sys_yes_no；0=否 1=是）"),

            // entity.document.viewcount
            new TranslationSeedItem("entity.document.viewcount", "en-US", "浏览次数_us", "浏览次数"),
            // entity.document.viewcount
            new TranslationSeedItem("entity.document.viewcount", "ja-JP", "浏览次数_jp", "浏览次数"),
            // entity.document.viewcount
            new TranslationSeedItem("entity.document.viewcount", "zh-CN", "浏览次数", "浏览次数"),
            // entity.document.viewcount
            new TranslationSeedItem("entity.document.viewcount", "zh-HK", "浏览次数_hk", "浏览次数"),

            // entity.document.targetscope
            new TranslationSeedItem("entity.document.targetscope", "en-US", "目标范围_us", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.document.targetscope
            new TranslationSeedItem("entity.document.targetscope", "ja-JP", "目标范围_jp", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.document.targetscope
            new TranslationSeedItem("entity.document.targetscope", "zh-CN", "目标范围", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),
            // entity.document.targetscope
            new TranslationSeedItem("entity.document.targetscope", "zh-HK", "目标范围_hk", "目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）"),

            // entity.document.targetdepartments
            new TranslationSeedItem("entity.document.targetdepartments", "en-US", "目标部门编码_us", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.document.targetdepartments
            new TranslationSeedItem("entity.document.targetdepartments", "ja-JP", "目标部门编码_jp", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.document.targetdepartments
            new TranslationSeedItem("entity.document.targetdepartments", "zh-CN", "目标部门编码", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),
            // entity.document.targetdepartments
            new TranslationSeedItem("entity.document.targetdepartments", "zh-HK", "目标部门编码_hk", "目标部门编码（多个用逗号分隔；TargetScope=1 时使用）"),

            // entity.document.targetusers
            new TranslationSeedItem("entity.document.targetusers", "en-US", "目标用户名_us", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.document.targetusers
            new TranslationSeedItem("entity.document.targetusers", "ja-JP", "目标用户名_jp", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.document.targetusers
            new TranslationSeedItem("entity.document.targetusers", "zh-CN", "目标用户名", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),
            // entity.document.targetusers
            new TranslationSeedItem("entity.document.targetusers", "zh-HK", "目标用户名_hk", "目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）"),

            // entity.document.sortorder
            new TranslationSeedItem("entity.document.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.document.sortorder
            new TranslationSeedItem("entity.document.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.document.sortorder
            new TranslationSeedItem("entity.document.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.document.sortorder
            new TranslationSeedItem("entity.document.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.document.status
            new TranslationSeedItem("entity.document.status", "en-US", "文档状态_us", "文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.document.status
            new TranslationSeedItem("entity.document.status", "ja-JP", "文档状态_jp", "文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.document.status
            new TranslationSeedItem("entity.document.status", "zh-CN", "文档状态", "文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),
            // entity.document.status
            new TranslationSeedItem("entity.document.status", "zh-HK", "文档状态_hk", "文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）"),

            // entity.document.versions
            new TranslationSeedItem("entity.document.versions", "en-US", "版本历史列表_us", "版本历史列表（主子表关系）"),
            // entity.document.versions
            new TranslationSeedItem("entity.document.versions", "ja-JP", "版本历史列表_jp", "版本历史列表（主子表关系）"),
            // entity.document.versions
            new TranslationSeedItem("entity.document.versions", "zh-CN", "版本历史列表", "版本历史列表（主子表关系）"),
            // entity.document.versions
            new TranslationSeedItem("entity.document.versions", "zh-HK", "版本历史列表_hk", "版本历史列表（主子表关系）"),
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
        translation.ResourceGroup = "DocumentCenter";
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
