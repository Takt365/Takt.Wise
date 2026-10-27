// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcNotification 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec-notification）
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
/// TaktEcNotification 实体国际化翻译种子（键前缀 entity.ecnotification.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcNotificationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcNotification 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecnotification 实体翻译...", tenantCode);

        foreach (var item in GetEcNotificationTranslations())
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

        TaktLogger.Information("TaktEcNotification 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcNotification 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecnotification._self / entity.ecnotification.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcNotificationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecnotification._self
            new TranslationSeedItem("entity.ecnotification._self", "en-US", "Ec Notification Information_us", "实体名称"),
            // entity.ecnotification._self
            new TranslationSeedItem("entity.ecnotification._self", "ja-JP", "工程变更通知单信息_jp", "实体名称"),
            // entity.ecnotification._self
            new TranslationSeedItem("entity.ecnotification._self", "zh-CN", "工程变更通知单信息", "实体名称"),
            // entity.ecnotification._self
            new TranslationSeedItem("entity.ecnotification._self", "zh-HK", "工程变更通知单信息_hk", "实体名称"),

            // entity.ecnotification.code
            new TranslationSeedItem("entity.ecnotification.code", "en-US", "通知单号_us", "通知单号（按通知方式经编码规则生成；前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：设变通知）"),
            // entity.ecnotification.code
            new TranslationSeedItem("entity.ecnotification.code", "ja-JP", "通知单号_jp", "通知单号（按通知方式经编码规则生成；前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：设变通知）"),
            // entity.ecnotification.code
            new TranslationSeedItem("entity.ecnotification.code", "zh-CN", "通知单号", "通知单号（按通知方式经编码规则生成；前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：设变通知）"),
            // entity.ecnotification.code
            new TranslationSeedItem("entity.ecnotification.code", "zh-HK", "通知单号_hk", "通知单号（按通知方式经编码规则生成；前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：设变通知）"),

            // entity.ecnotification.ecteid
            new TranslationSeedItem("entity.ecnotification.ecteid", "en-US", "技术课主表ID_us", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecnotification.ecteid
            new TranslationSeedItem("entity.ecnotification.ecteid", "ja-JP", "技术课主表ID_jp", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecnotification.ecteid
            new TranslationSeedItem("entity.ecnotification.ecteid", "zh-CN", "技术课主表ID", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecnotification.ecteid
            new TranslationSeedItem("entity.ecnotification.ecteid", "zh-HK", "技术课主表ID_hk", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),

            // entity.ecnotification.eccode
            new TranslationSeedItem("entity.ecnotification.eccode", "en-US", "设变单号_us", "设变单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ecnotification.eccode
            new TranslationSeedItem("entity.ecnotification.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ecnotification.eccode
            new TranslationSeedItem("entity.ecnotification.eccode", "zh-CN", "设变单号", "设变单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ecnotification.eccode
            new TranslationSeedItem("entity.ecnotification.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ecnotification.ectitle
            new TranslationSeedItem("entity.ecnotification.ectitle", "en-US", "设变标题_us", "设变标题（冗余字段）"),
            // entity.ecnotification.ectitle
            new TranslationSeedItem("entity.ecnotification.ectitle", "ja-JP", "设变标题_jp", "设变标题（冗余字段）"),
            // entity.ecnotification.ectitle
            new TranslationSeedItem("entity.ecnotification.ectitle", "zh-CN", "设变标题", "设变标题（冗余字段）"),
            // entity.ecnotification.ectitle
            new TranslationSeedItem("entity.ecnotification.ectitle", "zh-HK", "设变标题_hk", "设变标题（冗余字段）"),

            // entity.ecnotification.date
            new TranslationSeedItem("entity.ecnotification.date", "en-US", "通知日期_us", "通知日期"),
            // entity.ecnotification.date
            new TranslationSeedItem("entity.ecnotification.date", "ja-JP", "通知日期_jp", "通知日期"),
            // entity.ecnotification.date
            new TranslationSeedItem("entity.ecnotification.date", "zh-CN", "通知日期", "通知日期"),
            // entity.ecnotification.date
            new TranslationSeedItem("entity.ecnotification.date", "zh-HK", "通知日期_hk", "通知日期"),

            // entity.ecnotification.deptcodes
            new TranslationSeedItem("entity.ecnotification.deptcodes", "en-US", "通知部门组编码_us", "通知部门编码（多选，逗号分隔；选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1；联动 TaktEcGroups/options?ecGroupCategory=；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.deptcodes
            new TranslationSeedItem("entity.ecnotification.deptcodes", "ja-JP", "通知部门组编码_jp", "通知部门编码（多选，逗号分隔；选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1；联动 TaktEcGroups/options?ecGroupCategory=；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.deptcodes
            new TranslationSeedItem("entity.ecnotification.deptcodes", "zh-CN", "通知部门组编码", "通知部门编码（多选，逗号分隔；选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1；联动 TaktEcGroups/options?ecGroupCategory=；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.deptcodes
            new TranslationSeedItem("entity.ecnotification.deptcodes", "zh-HK", "通知部门组编码_hk", "通知部门编码（多选，逗号分隔；选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1；联动 TaktEcGroups/options?ecGroupCategory=；Length=500 容纳多个 6 位编码）"),

            // entity.ecnotification.deptnames
            new TranslationSeedItem("entity.ecnotification.deptnames", "en-US", "通知部门组名称_us", "通知部门名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationDeptCodes 取 TaktDept.DeptName1 联动；Length=500）"),
            // entity.ecnotification.deptnames
            new TranslationSeedItem("entity.ecnotification.deptnames", "ja-JP", "通知部门组名称_jp", "通知部门名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationDeptCodes 取 TaktDept.DeptName1 联动；Length=500）"),
            // entity.ecnotification.deptnames
            new TranslationSeedItem("entity.ecnotification.deptnames", "zh-CN", "通知部门组名称", "通知部门名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationDeptCodes 取 TaktDept.DeptName1 联动；Length=500）"),
            // entity.ecnotification.deptnames
            new TranslationSeedItem("entity.ecnotification.deptnames", "zh-HK", "通知部门组名称_hk", "通知部门名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationDeptCodes 取 TaktDept.DeptName1 联动；Length=500）"),

            // entity.ecnotification.notifiergroupcode
            new TranslationSeedItem("entity.ecnotification.notifiergroupcode", "en-US", "通知组编码_us", "通知组编码（多选，逗号分隔；选项 TaktEcGroups/options?ecGroupCategory=EcNotificationDeptCodes；DictValue=EcGroupCode，DictLabel=EcGroupName；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.notifiergroupcode
            new TranslationSeedItem("entity.ecnotification.notifiergroupcode", "ja-JP", "通知组编码_jp", "通知组编码（多选，逗号分隔；选项 TaktEcGroups/options?ecGroupCategory=EcNotificationDeptCodes；DictValue=EcGroupCode，DictLabel=EcGroupName；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.notifiergroupcode
            new TranslationSeedItem("entity.ecnotification.notifiergroupcode", "zh-CN", "通知组编码", "通知组编码（多选，逗号分隔；选项 TaktEcGroups/options?ecGroupCategory=EcNotificationDeptCodes；DictValue=EcGroupCode，DictLabel=EcGroupName；Length=500 容纳多个 6 位编码）"),
            // entity.ecnotification.notifiergroupcode
            new TranslationSeedItem("entity.ecnotification.notifiergroupcode", "zh-HK", "通知组编码_hk", "通知组编码（多选，逗号分隔；选项 TaktEcGroups/options?ecGroupCategory=EcNotificationDeptCodes；DictValue=EcGroupCode，DictLabel=EcGroupName；Length=500 容纳多个 6 位编码）"),

            // entity.ecnotification.notifiergroupname
            new TranslationSeedItem("entity.ecnotification.notifiergroupname", "en-US", "通知组名称_us", "通知组名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动；Length=500）"),
            // entity.ecnotification.notifiergroupname
            new TranslationSeedItem("entity.ecnotification.notifiergroupname", "ja-JP", "通知组名称_jp", "通知组名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动；Length=500）"),
            // entity.ecnotification.notifiergroupname
            new TranslationSeedItem("entity.ecnotification.notifiergroupname", "zh-CN", "通知组名称", "通知组名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动；Length=500）"),
            // entity.ecnotification.notifiergroupname
            new TranslationSeedItem("entity.ecnotification.notifiergroupname", "zh-HK", "通知组名称_hk", "通知组名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动；Length=500）"),

            // entity.ecnotification.method
            new TranslationSeedItem("entity.ecnotification.method", "en-US", "通知方式_us", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotification.method
            new TranslationSeedItem("entity.ecnotification.method", "ja-JP", "通知方式_jp", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotification.method
            new TranslationSeedItem("entity.ecnotification.method", "zh-CN", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotification.method
            new TranslationSeedItem("entity.ecnotification.method", "zh-HK", "通知方式_hk", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),

            // entity.ecnotification.status
            new TranslationSeedItem("entity.ecnotification.status", "en-US", "通知状态_us", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotification.status
            new TranslationSeedItem("entity.ecnotification.status", "ja-JP", "通知状态_jp", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotification.status
            new TranslationSeedItem("entity.ecnotification.status", "zh-CN", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotification.status
            new TranslationSeedItem("entity.ecnotification.status", "zh-HK", "通知状态_hk", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),

            // entity.ecnotification.ecte
            new TranslationSeedItem("entity.ecnotification.ecte", "en-US", "关联的设变主表_us", "关联的设变主表"),
            // entity.ecnotification.ecte
            new TranslationSeedItem("entity.ecnotification.ecte", "ja-JP", "关联的设变主表_jp", "关联的设变主表"),
            // entity.ecnotification.ecte
            new TranslationSeedItem("entity.ecnotification.ecte", "zh-CN", "关联的设变主表", "关联的设变主表"),
            // entity.ecnotification.ecte
            new TranslationSeedItem("entity.ecnotification.ecte", "zh-HK", "关联的设变主表_hk", "关联的设变主表"),
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
