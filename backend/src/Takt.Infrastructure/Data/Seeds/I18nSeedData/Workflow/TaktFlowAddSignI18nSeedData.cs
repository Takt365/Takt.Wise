// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowAddSignI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowAddSign 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow;

/// <summary>
/// TaktFlowAddSign 实体国际化翻译种子（键前缀 entity.flowaddsign.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowAddSignI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowAddSign 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowaddsign 实体翻译...", tenantCode);

        foreach (var item in GetFlowAddSignTranslations())
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

        TaktLogger.Information("TaktFlowAddSign 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowAddSign 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowaddsign._self / entity.flowaddsign.{{field}}；ResourceGroup=Workflow；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowAddSignTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowaddsign._self
            new TranslationSeedItem("entity.flowaddsign._self", "en-US", "Flow Add Sign Information_us", "实体名称"),
            // entity.flowaddsign._self
            new TranslationSeedItem("entity.flowaddsign._self", "ja-JP", "流程加签记录信息_jp", "实体名称"),
            // entity.flowaddsign._self
            new TranslationSeedItem("entity.flowaddsign._self", "zh-CN", "流程加签记录信息", "实体名称"),
            // entity.flowaddsign._self
            new TranslationSeedItem("entity.flowaddsign._self", "zh-HK", "流程加签记录信息_hk", "实体名称"),

            // entity.flowaddsign.instanceid
            new TranslationSeedItem("entity.flowaddsign.instanceid", "en-US", "流程实例ID_us", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowaddsign.instanceid
            new TranslationSeedItem("entity.flowaddsign.instanceid", "ja-JP", "流程实例ID_jp", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowaddsign.instanceid
            new TranslationSeedItem("entity.flowaddsign.instanceid", "zh-CN", "流程实例ID", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),
            // entity.flowaddsign.instanceid
            new TranslationSeedItem("entity.flowaddsign.instanceid", "zh-HK", "流程实例ID_hk", "流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）"),

            // entity.flowaddsign.nodeid
            new TranslationSeedItem("entity.flowaddsign.nodeid", "en-US", "节点ID_us", "加签节点 ID（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowaddsign.nodeid
            new TranslationSeedItem("entity.flowaddsign.nodeid", "ja-JP", "节点ID_jp", "加签节点 ID（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowaddsign.nodeid
            new TranslationSeedItem("entity.flowaddsign.nodeid", "zh-CN", "节点ID", "加签节点 ID（设计器 nodeId；与实例 CurrentActivityId 一致）"),
            // entity.flowaddsign.nodeid
            new TranslationSeedItem("entity.flowaddsign.nodeid", "zh-HK", "节点ID_hk", "加签节点 ID（设计器 nodeId；与实例 CurrentActivityId 一致）"),

            // entity.flowaddsign.signuserid
            new TranslationSeedItem("entity.flowaddsign.signuserid", "en-US", "加签人ID_us", "加签人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowaddsign.signuserid
            new TranslationSeedItem("entity.flowaddsign.signuserid", "ja-JP", "加签人ID_jp", "加签人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowaddsign.signuserid
            new TranslationSeedItem("entity.flowaddsign.signuserid", "zh-CN", "加签人ID", "加签人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.flowaddsign.signuserid
            new TranslationSeedItem("entity.flowaddsign.signuserid", "zh-HK", "加签人ID_hk", "加签人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.flowaddsign.signusername
            new TranslationSeedItem("entity.flowaddsign.signusername", "en-US", "加签人姓名_us", "加签人姓名（冗余：按 SignUserId 取 TaktUser.UserName 联动）"),
            // entity.flowaddsign.signusername
            new TranslationSeedItem("entity.flowaddsign.signusername", "ja-JP", "加签人姓名_jp", "加签人姓名（冗余：按 SignUserId 取 TaktUser.UserName 联动）"),
            // entity.flowaddsign.signusername
            new TranslationSeedItem("entity.flowaddsign.signusername", "zh-CN", "加签人姓名", "加签人姓名（冗余：按 SignUserId 取 TaktUser.UserName 联动）"),
            // entity.flowaddsign.signusername
            new TranslationSeedItem("entity.flowaddsign.signusername", "zh-HK", "加签人姓名_hk", "加签人姓名（冗余：按 SignUserId 取 TaktUser.UserName 联动）"),

            // entity.flowaddsign.signtype
            new TranslationSeedItem("entity.flowaddsign.signtype", "en-US", "加签方式_us", "加签方式（字典 sys_flow_add_sign_type；DictValue=sequential、all、one）"),
            // entity.flowaddsign.signtype
            new TranslationSeedItem("entity.flowaddsign.signtype", "ja-JP", "加签方式_jp", "加签方式（字典 sys_flow_add_sign_type；DictValue=sequential、all、one）"),
            // entity.flowaddsign.signtype
            new TranslationSeedItem("entity.flowaddsign.signtype", "zh-CN", "加签方式", "加签方式（字典 sys_flow_add_sign_type；DictValue=sequential、all、one）"),
            // entity.flowaddsign.signtype
            new TranslationSeedItem("entity.flowaddsign.signtype", "zh-HK", "加签方式_hk", "加签方式（字典 sys_flow_add_sign_type；DictValue=sequential、all、one）"),

            // entity.flowaddsign.returntosignnode
            new TranslationSeedItem("entity.flowaddsign.returntosignnode", "en-US", "回到加签节点_us", "完成后是否回到加签节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.returntosignnode
            new TranslationSeedItem("entity.flowaddsign.returntosignnode", "ja-JP", "回到加签节点_jp", "完成后是否回到加签节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.returntosignnode
            new TranslationSeedItem("entity.flowaddsign.returntosignnode", "zh-CN", "回到加签节点", "完成后是否回到加签节点（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.returntosignnode
            new TranslationSeedItem("entity.flowaddsign.returntosignnode", "zh-HK", "回到加签节点_hk", "完成后是否回到加签节点（字典 sys_yes_no；0=否 1=是）"),

            // entity.flowaddsign.reason
            new TranslationSeedItem("entity.flowaddsign.reason", "en-US", "加签原因_us", "加签原因"),
            // entity.flowaddsign.reason
            new TranslationSeedItem("entity.flowaddsign.reason", "ja-JP", "加签原因_jp", "加签原因"),
            // entity.flowaddsign.reason
            new TranslationSeedItem("entity.flowaddsign.reason", "zh-CN", "加签原因", "加签原因"),
            // entity.flowaddsign.reason
            new TranslationSeedItem("entity.flowaddsign.reason", "zh-HK", "加签原因_hk", "加签原因"),

            // entity.flowaddsign.ishandled
            new TranslationSeedItem("entity.flowaddsign.ishandled", "en-US", "是否已处理_us", "是否已处理（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.ishandled
            new TranslationSeedItem("entity.flowaddsign.ishandled", "ja-JP", "是否已处理_jp", "是否已处理（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.ishandled
            new TranslationSeedItem("entity.flowaddsign.ishandled", "zh-CN", "是否已处理", "是否已处理（字典 sys_yes_no；0=否 1=是）"),
            // entity.flowaddsign.ishandled
            new TranslationSeedItem("entity.flowaddsign.ishandled", "zh-HK", "是否已处理_hk", "是否已处理（字典 sys_yes_no；0=否 1=是）"),

            // entity.flowaddsign.instance
            new TranslationSeedItem("entity.flowaddsign.instance", "en-US", "所属流程实例_us", "所属流程实例"),
            // entity.flowaddsign.instance
            new TranslationSeedItem("entity.flowaddsign.instance", "ja-JP", "所属流程实例_jp", "所属流程实例"),
            // entity.flowaddsign.instance
            new TranslationSeedItem("entity.flowaddsign.instance", "zh-CN", "所属流程实例", "所属流程实例"),
            // entity.flowaddsign.instance
            new TranslationSeedItem("entity.flowaddsign.instance", "zh-HK", "所属流程实例_hk", "所属流程实例"),
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
        translation.ResourceGroup = "Workflow";
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
