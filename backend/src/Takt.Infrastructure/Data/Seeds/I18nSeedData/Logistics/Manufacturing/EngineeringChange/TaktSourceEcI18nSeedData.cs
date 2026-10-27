// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSourceEc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktSourceEc 实体国际化翻译种子（键前缀 entity.sourceec.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSourceEcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSourceEc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sourceec 实体翻译...", tenantCode);

        foreach (var item in GetSourceEcTranslations())
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

        TaktLogger.Information("TaktSourceEc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSourceEc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sourceec._self / entity.sourceec.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSourceEcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sourceec._self
            new TranslationSeedItem("entity.sourceec._self", "en-US", "Source Ec Information_us", "实体名称"),
            // entity.sourceec._self
            new TranslationSeedItem("entity.sourceec._self", "ja-JP", "设变来源明细列表信息_jp", "实体名称"),
            // entity.sourceec._self
            new TranslationSeedItem("entity.sourceec._self", "zh-CN", "设变来源明细列表信息", "实体名称"),
            // entity.sourceec._self
            new TranslationSeedItem("entity.sourceec._self", "zh-HK", "设变来源明细列表信息_hk", "实体名称"),

            // entity.sourceec.code
            new TranslationSeedItem("entity.sourceec.code", "en-US", "设变号码_us", "设变号码（前端表单选择编码规则后自动通过 TaktNumbering 设变号码规则生成并展示，非手输；单据类型菜单：设变来源）"),
            // entity.sourceec.code
            new TranslationSeedItem("entity.sourceec.code", "ja-JP", "设变号码_jp", "设变号码（前端表单选择编码规则后自动通过 TaktNumbering 设变号码规则生成并展示，非手输；单据类型菜单：设变来源）"),
            // entity.sourceec.code
            new TranslationSeedItem("entity.sourceec.code", "zh-CN", "设变号码", "设变号码（前端表单选择编码规则后自动通过 TaktNumbering 设变号码规则生成并展示，非手输；单据类型菜单：设变来源）"),
            // entity.sourceec.code
            new TranslationSeedItem("entity.sourceec.code", "zh-HK", "设变号码_hk", "设变号码（前端表单选择编码规则后自动通过 TaktNumbering 设变号码规则生成并展示，非手输；单据类型菜单：设变来源）"),

            // entity.sourceec.sourcemodel
            new TranslationSeedItem("entity.sourceec.sourcemodel", "en-US", "机种_us", "机种"),
            // entity.sourceec.sourcemodel
            new TranslationSeedItem("entity.sourceec.sourcemodel", "ja-JP", "机种_jp", "机种"),
            // entity.sourceec.sourcemodel
            new TranslationSeedItem("entity.sourceec.sourcemodel", "zh-CN", "机种", "机种"),
            // entity.sourceec.sourcemodel
            new TranslationSeedItem("entity.sourceec.sourcemodel", "zh-HK", "机种_hk", "机种"),

            // entity.sourceec.sourcetitle
            new TranslationSeedItem("entity.sourceec.sourcetitle", "en-US", "标题_us", "标题"),
            // entity.sourceec.sourcetitle
            new TranslationSeedItem("entity.sourceec.sourcetitle", "ja-JP", "标题_jp", "标题"),
            // entity.sourceec.sourcetitle
            new TranslationSeedItem("entity.sourceec.sourcetitle", "zh-CN", "标题", "标题"),
            // entity.sourceec.sourcetitle
            new TranslationSeedItem("entity.sourceec.sourcetitle", "zh-HK", "标题_hk", "标题"),

            // entity.sourceec.sourcestatus
            new TranslationSeedItem("entity.sourceec.sourcestatus", "en-US", "状态_us", "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）"),
            // entity.sourceec.sourcestatus
            new TranslationSeedItem("entity.sourceec.sourcestatus", "ja-JP", "状态_jp", "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）"),
            // entity.sourceec.sourcestatus
            new TranslationSeedItem("entity.sourceec.sourcestatus", "zh-CN", "状态", "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）"),
            // entity.sourceec.sourcestatus
            new TranslationSeedItem("entity.sourceec.sourcestatus", "zh-HK", "状态_hk", "状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）"),

            // entity.sourceec.sourceissuedate
            new TranslationSeedItem("entity.sourceec.sourceissuedate", "en-US", "发行日期_us", "发行日期"),
            // entity.sourceec.sourceissuedate
            new TranslationSeedItem("entity.sourceec.sourceissuedate", "ja-JP", "发行日期_jp", "发行日期"),
            // entity.sourceec.sourceissuedate
            new TranslationSeedItem("entity.sourceec.sourceissuedate", "zh-CN", "发行日期", "发行日期"),
            // entity.sourceec.sourceissuedate
            new TranslationSeedItem("entity.sourceec.sourceissuedate", "zh-HK", "发行日期_hk", "发行日期"),

            // entity.sourceec.sourcetcjowner
            new TranslationSeedItem("entity.sourceec.sourcetcjowner", "en-US", "TCJ担当_us", "TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）"),
            // entity.sourceec.sourcetcjowner
            new TranslationSeedItem("entity.sourceec.sourcetcjowner", "ja-JP", "TCJ担当_jp", "TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）"),
            // entity.sourceec.sourcetcjowner
            new TranslationSeedItem("entity.sourceec.sourcetcjowner", "zh-CN", "TCJ担当", "TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）"),
            // entity.sourceec.sourcetcjowner
            new TranslationSeedItem("entity.sourceec.sourcetcjowner", "zh-HK", "TCJ担当_hk", "TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）"),

            // entity.sourceec.sourcetcjdependency
            new TranslationSeedItem("entity.sourceec.sourcetcjdependency", "en-US", "TCJ依赖_us", "TCJ依赖"),
            // entity.sourceec.sourcetcjdependency
            new TranslationSeedItem("entity.sourceec.sourcetcjdependency", "ja-JP", "TCJ依赖_jp", "TCJ依赖"),
            // entity.sourceec.sourcetcjdependency
            new TranslationSeedItem("entity.sourceec.sourcetcjdependency", "zh-CN", "TCJ依赖", "TCJ依赖"),
            // entity.sourceec.sourcetcjdependency
            new TranslationSeedItem("entity.sourceec.sourcetcjdependency", "zh-HK", "TCJ依赖_hk", "TCJ依赖"),

            // entity.sourceec.meeting
            new TranslationSeedItem("entity.sourceec.meeting", "en-US", "设变会议_us", "设变会议"),
            // entity.sourceec.meeting
            new TranslationSeedItem("entity.sourceec.meeting", "ja-JP", "设变会议_jp", "设变会议"),
            // entity.sourceec.meeting
            new TranslationSeedItem("entity.sourceec.meeting", "zh-CN", "设变会议", "设变会议"),
            // entity.sourceec.meeting
            new TranslationSeedItem("entity.sourceec.meeting", "zh-HK", "设变会议_hk", "设变会议"),

            // entity.sourceec.sourceppcode
            new TranslationSeedItem("entity.sourceec.sourceppcode", "en-US", "PP番号_us", "PP番号"),
            // entity.sourceec.sourceppcode
            new TranslationSeedItem("entity.sourceec.sourceppcode", "ja-JP", "PP番号_jp", "PP番号"),
            // entity.sourceec.sourceppcode
            new TranslationSeedItem("entity.sourceec.sourceppcode", "zh-CN", "PP番号", "PP番号"),
            // entity.sourceec.sourceppcode
            new TranslationSeedItem("entity.sourceec.sourceppcode", "zh-HK", "PP番号_hk", "PP番号"),

            // entity.sourceec.sourcetechnicalnoticecode
            new TranslationSeedItem("entity.sourceec.sourcetechnicalnoticecode", "en-US", "技联书_us", "技联书"),
            // entity.sourceec.sourcetechnicalnoticecode
            new TranslationSeedItem("entity.sourceec.sourcetechnicalnoticecode", "ja-JP", "技联书_jp", "技联书"),
            // entity.sourceec.sourcetechnicalnoticecode
            new TranslationSeedItem("entity.sourceec.sourcetechnicalnoticecode", "zh-CN", "技联书", "技联书"),
            // entity.sourceec.sourcetechnicalnoticecode
            new TranslationSeedItem("entity.sourceec.sourcetechnicalnoticecode", "zh-HK", "技联书_hk", "技联书"),

            // entity.sourceec.sourceimplementation
            new TranslationSeedItem("entity.sourceec.sourceimplementation", "en-US", "实施_us", "实施"),
            // entity.sourceec.sourceimplementation
            new TranslationSeedItem("entity.sourceec.sourceimplementation", "ja-JP", "实施_jp", "实施"),
            // entity.sourceec.sourceimplementation
            new TranslationSeedItem("entity.sourceec.sourceimplementation", "zh-CN", "实施", "实施"),
            // entity.sourceec.sourceimplementation
            new TranslationSeedItem("entity.sourceec.sourceimplementation", "zh-HK", "实施_hk", "实施"),

            // entity.sourceec.sourcemainchangereason
            new TranslationSeedItem("entity.sourceec.sourcemainchangereason", "en-US", "主变更理由_us", "主变更理由"),
            // entity.sourceec.sourcemainchangereason
            new TranslationSeedItem("entity.sourceec.sourcemainchangereason", "ja-JP", "主变更理由_jp", "主变更理由"),
            // entity.sourceec.sourcemainchangereason
            new TranslationSeedItem("entity.sourceec.sourcemainchangereason", "zh-CN", "主变更理由", "主变更理由"),
            // entity.sourceec.sourcemainchangereason
            new TranslationSeedItem("entity.sourceec.sourcemainchangereason", "zh-HK", "主变更理由_hk", "主变更理由"),

            // entity.sourceec.sourcesecondarychangereason
            new TranslationSeedItem("entity.sourceec.sourcesecondarychangereason", "en-US", "次变更理由_us", "次变更理由"),
            // entity.sourceec.sourcesecondarychangereason
            new TranslationSeedItem("entity.sourceec.sourcesecondarychangereason", "ja-JP", "次变更理由_jp", "次变更理由"),
            // entity.sourceec.sourcesecondarychangereason
            new TranslationSeedItem("entity.sourceec.sourcesecondarychangereason", "zh-CN", "次变更理由", "次变更理由"),
            // entity.sourceec.sourcesecondarychangereason
            new TranslationSeedItem("entity.sourceec.sourcesecondarychangereason", "zh-HK", "次变更理由_hk", "次变更理由"),

            // entity.sourceec.sourcesafetyregulation
            new TranslationSeedItem("entity.sourceec.sourcesafetyregulation", "en-US", "安规_us", "安规"),
            // entity.sourceec.sourcesafetyregulation
            new TranslationSeedItem("entity.sourceec.sourcesafetyregulation", "ja-JP", "安规_jp", "安规"),
            // entity.sourceec.sourcesafetyregulation
            new TranslationSeedItem("entity.sourceec.sourcesafetyregulation", "zh-CN", "安规", "安规"),
            // entity.sourceec.sourcesafetyregulation
            new TranslationSeedItem("entity.sourceec.sourcesafetyregulation", "zh-HK", "安规_hk", "安规"),

            // entity.sourceec.sourceprogressstatus
            new TranslationSeedItem("entity.sourceec.sourceprogressstatus", "en-US", "进行状况_us", "进行状况"),
            // entity.sourceec.sourceprogressstatus
            new TranslationSeedItem("entity.sourceec.sourceprogressstatus", "ja-JP", "进行状况_jp", "进行状况"),
            // entity.sourceec.sourceprogressstatus
            new TranslationSeedItem("entity.sourceec.sourceprogressstatus", "zh-CN", "进行状况", "进行状况"),
            // entity.sourceec.sourceprogressstatus
            new TranslationSeedItem("entity.sourceec.sourceprogressstatus", "zh-HK", "进行状况_hk", "进行状况"),

            // entity.sourceec.sourceserialnumbercontrol
            new TranslationSeedItem("entity.sourceec.sourceserialnumbercontrol", "en-US", "机番管理_us", "机番管理"),
            // entity.sourceec.sourceserialnumbercontrol
            new TranslationSeedItem("entity.sourceec.sourceserialnumbercontrol", "ja-JP", "机番管理_jp", "机番管理"),
            // entity.sourceec.sourceserialnumbercontrol
            new TranslationSeedItem("entity.sourceec.sourceserialnumbercontrol", "zh-CN", "机番管理", "机番管理"),
            // entity.sourceec.sourceserialnumbercontrol
            new TranslationSeedItem("entity.sourceec.sourceserialnumbercontrol", "zh-HK", "机番管理_hk", "机番管理"),

            // entity.sourceec.sourcecustomerapproval
            new TranslationSeedItem("entity.sourceec.sourcecustomerapproval", "en-US", "客户承认_us", "客户承认"),
            // entity.sourceec.sourcecustomerapproval
            new TranslationSeedItem("entity.sourceec.sourcecustomerapproval", "ja-JP", "客户承认_jp", "客户承认"),
            // entity.sourceec.sourcecustomerapproval
            new TranslationSeedItem("entity.sourceec.sourcecustomerapproval", "zh-CN", "客户承认", "客户承认"),
            // entity.sourceec.sourcecustomerapproval
            new TranslationSeedItem("entity.sourceec.sourcecustomerapproval", "zh-HK", "客户承认_hk", "客户承认"),

            // entity.sourceec.sourceservicemanualrevision
            new TranslationSeedItem("entity.sourceec.sourceservicemanualrevision", "en-US", "服务手册订正_us", "服务手册订正"),
            // entity.sourceec.sourceservicemanualrevision
            new TranslationSeedItem("entity.sourceec.sourceservicemanualrevision", "ja-JP", "服务手册订正_jp", "服务手册订正"),
            // entity.sourceec.sourceservicemanualrevision
            new TranslationSeedItem("entity.sourceec.sourceservicemanualrevision", "zh-CN", "服务手册订正", "服务手册订正"),
            // entity.sourceec.sourceservicemanualrevision
            new TranslationSeedItem("entity.sourceec.sourceservicemanualrevision", "zh-HK", "服务手册订正_hk", "服务手册订正"),

            // entity.sourceec.sourceusermanualrevision
            new TranslationSeedItem("entity.sourceec.sourceusermanualrevision", "en-US", "用户手册订正_us", "用户手册订正"),
            // entity.sourceec.sourceusermanualrevision
            new TranslationSeedItem("entity.sourceec.sourceusermanualrevision", "ja-JP", "用户手册订正_jp", "用户手册订正"),
            // entity.sourceec.sourceusermanualrevision
            new TranslationSeedItem("entity.sourceec.sourceusermanualrevision", "zh-CN", "用户手册订正", "用户手册订正"),
            // entity.sourceec.sourceusermanualrevision
            new TranslationSeedItem("entity.sourceec.sourceusermanualrevision", "zh-HK", "用户手册订正_hk", "用户手册订正"),

            // entity.sourceec.sourcepromotionmanualrevision
            new TranslationSeedItem("entity.sourceec.sourcepromotionmanualrevision", "en-US", "宣传手册订正_us", "宣传手册订正"),
            // entity.sourceec.sourcepromotionmanualrevision
            new TranslationSeedItem("entity.sourceec.sourcepromotionmanualrevision", "ja-JP", "宣传手册订正_jp", "宣传手册订正"),
            // entity.sourceec.sourcepromotionmanualrevision
            new TranslationSeedItem("entity.sourceec.sourcepromotionmanualrevision", "zh-CN", "宣传手册订正", "宣传手册订正"),
            // entity.sourceec.sourcepromotionmanualrevision
            new TranslationSeedItem("entity.sourceec.sourcepromotionmanualrevision", "zh-HK", "宣传手册订正_hk", "宣传手册订正"),

            // entity.sourceec.sourcestandarddocumentrevision
            new TranslationSeedItem("entity.sourceec.sourcestandarddocumentrevision", "en-US", "标准书订正_us", "标准书订正"),
            // entity.sourceec.sourcestandarddocumentrevision
            new TranslationSeedItem("entity.sourceec.sourcestandarddocumentrevision", "ja-JP", "标准书订正_jp", "标准书订正"),
            // entity.sourceec.sourcestandarddocumentrevision
            new TranslationSeedItem("entity.sourceec.sourcestandarddocumentrevision", "zh-CN", "标准书订正", "标准书订正"),
            // entity.sourceec.sourcestandarddocumentrevision
            new TranslationSeedItem("entity.sourceec.sourcestandarddocumentrevision", "zh-HK", "标准书订正_hk", "标准书订正"),

            // entity.sourceec.sourceinformationrelease
            new TranslationSeedItem("entity.sourceec.sourceinformationrelease", "en-US", "情报发行_us", "情报发行"),
            // entity.sourceec.sourceinformationrelease
            new TranslationSeedItem("entity.sourceec.sourceinformationrelease", "ja-JP", "情报发行_jp", "情报发行"),
            // entity.sourceec.sourceinformationrelease
            new TranslationSeedItem("entity.sourceec.sourceinformationrelease", "zh-CN", "情报发行", "情报发行"),
            // entity.sourceec.sourceinformationrelease
            new TranslationSeedItem("entity.sourceec.sourceinformationrelease", "zh-HK", "情报发行_hk", "情报发行"),

            // entity.sourceec.sourcecostchange
            new TranslationSeedItem("entity.sourceec.sourcecostchange", "en-US", "成本变动_us", "成本变动"),
            // entity.sourceec.sourcecostchange
            new TranslationSeedItem("entity.sourceec.sourcecostchange", "ja-JP", "成本变动_jp", "成本变动"),
            // entity.sourceec.sourcecostchange
            new TranslationSeedItem("entity.sourceec.sourcecostchange", "zh-CN", "成本变动", "成本变动"),
            // entity.sourceec.sourcecostchange
            new TranslationSeedItem("entity.sourceec.sourcecostchange", "zh-HK", "成本变动_hk", "成本变动"),

            // entity.sourceec.sourceunitcost
            new TranslationSeedItem("entity.sourceec.sourceunitcost", "en-US", "单位成本_us", "单位成本"),
            // entity.sourceec.sourceunitcost
            new TranslationSeedItem("entity.sourceec.sourceunitcost", "ja-JP", "单位成本_jp", "单位成本"),
            // entity.sourceec.sourceunitcost
            new TranslationSeedItem("entity.sourceec.sourceunitcost", "zh-CN", "单位成本", "单位成本"),
            // entity.sourceec.sourceunitcost
            new TranslationSeedItem("entity.sourceec.sourceunitcost", "zh-HK", "单位成本_hk", "单位成本"),

            // entity.sourceec.sourcemoldmodificationcost
            new TranslationSeedItem("entity.sourceec.sourcemoldmodificationcost", "en-US", "模具改修费_us", "模具改修费"),
            // entity.sourceec.sourcemoldmodificationcost
            new TranslationSeedItem("entity.sourceec.sourcemoldmodificationcost", "ja-JP", "模具改修费_jp", "模具改修费"),
            // entity.sourceec.sourcemoldmodificationcost
            new TranslationSeedItem("entity.sourceec.sourcemoldmodificationcost", "zh-CN", "模具改修费", "模具改修费"),
            // entity.sourceec.sourcemoldmodificationcost
            new TranslationSeedItem("entity.sourceec.sourcemoldmodificationcost", "zh-HK", "模具改修费_hk", "模具改修费"),

            // entity.sourceec.sourcerelateddrawing
            new TranslationSeedItem("entity.sourceec.sourcerelateddrawing", "en-US", "相关图纸_us", "相关图纸"),
            // entity.sourceec.sourcerelateddrawing
            new TranslationSeedItem("entity.sourceec.sourcerelateddrawing", "ja-JP", "相关图纸_jp", "相关图纸"),
            // entity.sourceec.sourcerelateddrawing
            new TranslationSeedItem("entity.sourceec.sourcerelateddrawing", "zh-CN", "相关图纸", "相关图纸"),
            // entity.sourceec.sourcerelateddrawing
            new TranslationSeedItem("entity.sourceec.sourcerelateddrawing", "zh-HK", "相关图纸_hk", "相关图纸"),

            // entity.sourceec.content
            new TranslationSeedItem("entity.sourceec.content", "en-US", "设变内容_us", "设变内容（富文本 HTML）"),
            // entity.sourceec.content
            new TranslationSeedItem("entity.sourceec.content", "ja-JP", "设变内容_jp", "设变内容（富文本 HTML）"),
            // entity.sourceec.content
            new TranslationSeedItem("entity.sourceec.content", "zh-CN", "设变内容", "设变内容（富文本 HTML）"),
            // entity.sourceec.content
            new TranslationSeedItem("entity.sourceec.content", "zh-HK", "设变内容_hk", "设变内容（富文本 HTML）"),

            // entity.sourceec.details
            new TranslationSeedItem("entity.sourceec.details", "en-US", "设变来源明细列表_us", "设变来源明细列表"),
            // entity.sourceec.details
            new TranslationSeedItem("entity.sourceec.details", "ja-JP", "设变来源明细列表_jp", "设变来源明细列表"),
            // entity.sourceec.details
            new TranslationSeedItem("entity.sourceec.details", "zh-CN", "设变来源明细列表", "设变来源明细列表"),
            // entity.sourceec.details
            new TranslationSeedItem("entity.sourceec.details", "zh-HK", "设变来源明细列表_hk", "设变来源明细列表"),
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
        translation.ResourceGroup = "EngineeringChange";
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
