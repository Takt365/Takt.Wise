// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationDeliveryI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcNotificationDelivery 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcNotificationDelivery 实体国际化翻译种子（键前缀 entity.ecnotificationdelivery.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcNotificationDeliveryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcNotificationDelivery 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecnotificationdelivery 实体翻译...", tenantCode);

        foreach (var item in GetEcNotificationDeliveryTranslations())
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

        TaktLogger.Information("TaktEcNotificationDelivery 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcNotificationDelivery 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecnotificationdelivery._self / entity.ecnotificationdelivery.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcNotificationDeliveryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecnotificationdelivery._self
            new TranslationSeedItem("entity.ecnotificationdelivery._self", "en-US", "Ec Notification Delivery Information_us", "实体名称"),
            // entity.ecnotificationdelivery._self
            new TranslationSeedItem("entity.ecnotificationdelivery._self", "ja-JP", "确认时间信息_jp", "实体名称"),
            // entity.ecnotificationdelivery._self
            new TranslationSeedItem("entity.ecnotificationdelivery._self", "zh-CN", "确认时间信息", "实体名称"),
            // entity.ecnotificationdelivery._self
            new TranslationSeedItem("entity.ecnotificationdelivery._self", "zh-HK", "确认时间信息_hk", "实体名称"),

            // entity.ecnotificationdelivery.ecnotificationid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationid", "en-US", "通知单ID_us", "通知单 ID"),
            // entity.ecnotificationdelivery.ecnotificationid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationid", "ja-JP", "通知单ID_jp", "通知单 ID"),
            // entity.ecnotificationdelivery.ecnotificationid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationid", "zh-CN", "通知单ID", "通知单 ID"),
            // entity.ecnotificationdelivery.ecnotificationid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationid", "zh-HK", "通知单ID_hk", "通知单 ID"),

            // entity.ecnotificationdelivery.ecnotificationcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationcode", "en-US", "通知单号_us", "通知单号（冗余）"),
            // entity.ecnotificationdelivery.ecnotificationcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationcode", "ja-JP", "通知单号_jp", "通知单号（冗余）"),
            // entity.ecnotificationdelivery.ecnotificationcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationcode", "zh-CN", "通知单号", "通知单号（冗余）"),
            // entity.ecnotificationdelivery.ecnotificationcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecnotificationcode", "zh-HK", "通知单号_hk", "通知单号（冗余）"),

            // entity.ecnotificationdelivery.ecteid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecteid", "en-US", "技术课主表ID_us", "技术课主表 ID（TaktEcTe 主键）"),
            // entity.ecnotificationdelivery.ecteid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecteid", "ja-JP", "技术课主表ID_jp", "技术课主表 ID（TaktEcTe 主键）"),
            // entity.ecnotificationdelivery.ecteid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecteid", "zh-CN", "技术课主表ID", "技术课主表 ID（TaktEcTe 主键）"),
            // entity.ecnotificationdelivery.ecteid
            new TranslationSeedItem("entity.ecnotificationdelivery.ecteid", "zh-HK", "技术课主表ID_hk", "技术课主表 ID（TaktEcTe 主键）"),

            // entity.ecnotificationdelivery.eccode
            new TranslationSeedItem("entity.ecnotificationdelivery.eccode", "en-US", "设变单号_us", "设变单号（冗余）"),
            // entity.ecnotificationdelivery.eccode
            new TranslationSeedItem("entity.ecnotificationdelivery.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余）"),
            // entity.ecnotificationdelivery.eccode
            new TranslationSeedItem("entity.ecnotificationdelivery.eccode", "zh-CN", "设变单号", "设变单号（冗余）"),
            // entity.ecnotificationdelivery.eccode
            new TranslationSeedItem("entity.ecnotificationdelivery.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余）"),

            // entity.ecnotificationdelivery.ecdeptgroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupcode", "en-US", "设变部门编码_us", "设变部门编码（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecnotificationdelivery.ecdeptgroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupcode", "ja-JP", "设变部门编码_jp", "设变部门编码（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecnotificationdelivery.ecdeptgroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupcode", "zh-CN", "设变部门编码", "设变部门编码（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),
            // entity.ecnotificationdelivery.ecdeptgroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupcode", "zh-HK", "设变部门编码_hk", "设变部门编码（选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1）"),

            // entity.ecnotificationdelivery.ecdeptgroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupname", "en-US", "设变部门名称_us", "设变部门名称（冗余：按 EcDeptGroupCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecnotificationdelivery.ecdeptgroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupname", "ja-JP", "设变部门名称_jp", "设变部门名称（冗余：按 EcDeptGroupCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecnotificationdelivery.ecdeptgroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupname", "zh-CN", "设变部门名称", "设变部门名称（冗余：按 EcDeptGroupCode 取 TaktDept.DeptName1 联动）"),
            // entity.ecnotificationdelivery.ecdeptgroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.ecdeptgroupname", "zh-HK", "设变部门名称_hk", "设变部门名称（冗余：按 EcDeptGroupCode 取 TaktDept.DeptName1 联动）"),

            // entity.ecnotificationdelivery.priority
            new TranslationSeedItem("entity.ecnotificationdelivery.priority", "en-US", "优先级_us", "优先级（1=普通，2=高，3=紧急）"),
            // entity.ecnotificationdelivery.priority
            new TranslationSeedItem("entity.ecnotificationdelivery.priority", "ja-JP", "优先级_jp", "优先级（1=普通，2=高，3=紧急）"),
            // entity.ecnotificationdelivery.priority
            new TranslationSeedItem("entity.ecnotificationdelivery.priority", "zh-CN", "优先级", "优先级（1=普通，2=高，3=紧急）"),
            // entity.ecnotificationdelivery.priority
            new TranslationSeedItem("entity.ecnotificationdelivery.priority", "zh-HK", "优先级_hk", "优先级（1=普通，2=高，3=紧急）"),

            // entity.ecnotificationdelivery.deliverystatus
            new TranslationSeedItem("entity.ecnotificationdelivery.deliverystatus", "en-US", "投递状态_us", "投递状态（0=待发送，1=已发送，2=已确认）"),
            // entity.ecnotificationdelivery.deliverystatus
            new TranslationSeedItem("entity.ecnotificationdelivery.deliverystatus", "ja-JP", "投递状态_jp", "投递状态（0=待发送，1=已发送，2=已确认）"),
            // entity.ecnotificationdelivery.deliverystatus
            new TranslationSeedItem("entity.ecnotificationdelivery.deliverystatus", "zh-CN", "投递状态", "投递状态（0=待发送，1=已发送，2=已确认）"),
            // entity.ecnotificationdelivery.deliverystatus
            new TranslationSeedItem("entity.ecnotificationdelivery.deliverystatus", "zh-HK", "投递状态_hk", "投递状态（0=待发送，1=已发送，2=已确认）"),

            // entity.ecnotificationdelivery.sentat
            new TranslationSeedItem("entity.ecnotificationdelivery.sentat", "en-US", "发送时间_us", "发送时间"),
            // entity.ecnotificationdelivery.sentat
            new TranslationSeedItem("entity.ecnotificationdelivery.sentat", "ja-JP", "发送时间_jp", "发送时间"),
            // entity.ecnotificationdelivery.sentat
            new TranslationSeedItem("entity.ecnotificationdelivery.sentat", "zh-CN", "发送时间", "发送时间"),
            // entity.ecnotificationdelivery.sentat
            new TranslationSeedItem("entity.ecnotificationdelivery.sentat", "zh-HK", "发送时间_hk", "发送时间"),

            // entity.ecnotificationdelivery.confirmedbygroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupcode", "en-US", "确认组编码_us", "确认组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecnotificationdelivery.confirmedbygroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupcode", "ja-JP", "确认组编码_jp", "确认组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecnotificationdelivery.confirmedbygroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupcode", "zh-CN", "确认组编码", "确认组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）"),
            // entity.ecnotificationdelivery.confirmedbygroupcode
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupcode", "zh-HK", "确认组编码_hk", "确认组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）"),

            // entity.ecnotificationdelivery.confirmedbygroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupname", "en-US", "确认组名称_us", "确认组名称（冗余：按 ConfirmedByGroupCode 取 TaktEcGroups.EcGroupName 联动）"),
            // entity.ecnotificationdelivery.confirmedbygroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupname", "ja-JP", "确认组名称_jp", "确认组名称（冗余：按 ConfirmedByGroupCode 取 TaktEcGroups.EcGroupName 联动）"),
            // entity.ecnotificationdelivery.confirmedbygroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupname", "zh-CN", "确认组名称", "确认组名称（冗余：按 ConfirmedByGroupCode 取 TaktEcGroups.EcGroupName 联动）"),
            // entity.ecnotificationdelivery.confirmedbygroupname
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedbygroupname", "zh-HK", "确认组名称_hk", "确认组名称（冗余：按 ConfirmedByGroupCode 取 TaktEcGroups.EcGroupName 联动）"),

            // entity.ecnotificationdelivery.confirmedat
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedat", "en-US", "确认时间_us", "确认时间"),
            // entity.ecnotificationdelivery.confirmedat
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedat", "ja-JP", "确认时间_jp", "确认时间"),
            // entity.ecnotificationdelivery.confirmedat
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedat", "zh-CN", "确认时间", "确认时间"),
            // entity.ecnotificationdelivery.confirmedat
            new TranslationSeedItem("entity.ecnotificationdelivery.confirmedat", "zh-HK", "确认时间_hk", "确认时间"),
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
