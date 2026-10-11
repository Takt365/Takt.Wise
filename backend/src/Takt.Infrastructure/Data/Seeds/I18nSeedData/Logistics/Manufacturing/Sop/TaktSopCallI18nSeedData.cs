// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCallI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopCall 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopCall 实体国际化翻译种子（键前缀 entity.sopcall.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopCallI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopCall 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopcall 实体翻译...", tenantCode);

        foreach (var item in GetSopCallTranslations())
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

        TaktLogger.Information("TaktSopCall 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopCall 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopcall._self / entity.sopcall.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopCallTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopcall._self
            new TranslationSeedItem("entity.sopcall._self", "en-US", "Sop Call Information_us", "实体名称"),
            // entity.sopcall._self
            new TranslationSeedItem("entity.sopcall._self", "ja-JP", "SOP 安灯呼叫信息_jp", "实体名称"),
            // entity.sopcall._self
            new TranslationSeedItem("entity.sopcall._self", "zh-CN", "SOP 安灯呼叫信息", "实体名称"),
            // entity.sopcall._self
            new TranslationSeedItem("entity.sopcall._self", "zh-HK", "SOP 安灯呼叫信息_hk", "实体名称"),

            // entity.sopcall.workstationid
            new TranslationSeedItem("entity.sopcall.workstationid", "en-US", "工位ID_us", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopcall.workstationid
            new TranslationSeedItem("entity.sopcall.workstationid", "ja-JP", "工位ID_jp", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopcall.workstationid
            new TranslationSeedItem("entity.sopcall.workstationid", "zh-CN", "工位ID", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopcall.workstationid
            new TranslationSeedItem("entity.sopcall.workstationid", "zh-HK", "工位ID_hk", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),

            // entity.sopcall.execid
            new TranslationSeedItem("entity.sopcall.execid", "en-US", "执行追溯ID_us", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopcall.execid
            new TranslationSeedItem("entity.sopcall.execid", "ja-JP", "执行追溯ID_jp", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopcall.execid
            new TranslationSeedItem("entity.sopcall.execid", "zh-CN", "执行追溯ID", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopcall.execid
            new TranslationSeedItem("entity.sopcall.execid", "zh-HK", "执行追溯ID_hk", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),

            // entity.sopcall.calltype
            new TranslationSeedItem("entity.sopcall.calltype", "en-US", "呼叫类型_us", "呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）"),
            // entity.sopcall.calltype
            new TranslationSeedItem("entity.sopcall.calltype", "ja-JP", "呼叫类型_jp", "呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）"),
            // entity.sopcall.calltype
            new TranslationSeedItem("entity.sopcall.calltype", "zh-CN", "呼叫类型", "呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）"),
            // entity.sopcall.calltype
            new TranslationSeedItem("entity.sopcall.calltype", "zh-HK", "呼叫类型_hk", "呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）"),

            // entity.sopcall.callerid
            new TranslationSeedItem("entity.sopcall.callerid", "en-US", "呼叫人ID_us", "呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.callerid
            new TranslationSeedItem("entity.sopcall.callerid", "ja-JP", "呼叫人ID_jp", "呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.callerid
            new TranslationSeedItem("entity.sopcall.callerid", "zh-CN", "呼叫人ID", "呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.callerid
            new TranslationSeedItem("entity.sopcall.callerid", "zh-HK", "呼叫人ID_hk", "呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopcall.callername
            new TranslationSeedItem("entity.sopcall.callername", "en-US", "呼叫人名称_us", "呼叫人名称（冗余：按 CallerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.callername
            new TranslationSeedItem("entity.sopcall.callername", "ja-JP", "呼叫人名称_jp", "呼叫人名称（冗余：按 CallerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.callername
            new TranslationSeedItem("entity.sopcall.callername", "zh-CN", "呼叫人名称", "呼叫人名称（冗余：按 CallerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.callername
            new TranslationSeedItem("entity.sopcall.callername", "zh-HK", "呼叫人名称_hk", "呼叫人名称（冗余：按 CallerId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopcall.calledat
            new TranslationSeedItem("entity.sopcall.calledat", "en-US", "呼叫时间_us", "呼叫时间"),
            // entity.sopcall.calledat
            new TranslationSeedItem("entity.sopcall.calledat", "ja-JP", "呼叫时间_jp", "呼叫时间"),
            // entity.sopcall.calledat
            new TranslationSeedItem("entity.sopcall.calledat", "zh-CN", "呼叫时间", "呼叫时间"),
            // entity.sopcall.calledat
            new TranslationSeedItem("entity.sopcall.calledat", "zh-HK", "呼叫时间_hk", "呼叫时间"),

            // entity.sopcall.respondedby
            new TranslationSeedItem("entity.sopcall.respondedby", "en-US", "响应人ID_us", "响应人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.respondedby
            new TranslationSeedItem("entity.sopcall.respondedby", "ja-JP", "响应人ID_jp", "响应人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.respondedby
            new TranslationSeedItem("entity.sopcall.respondedby", "zh-CN", "响应人ID", "响应人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopcall.respondedby
            new TranslationSeedItem("entity.sopcall.respondedby", "zh-HK", "响应人ID_hk", "响应人 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopcall.respondedbyname
            new TranslationSeedItem("entity.sopcall.respondedbyname", "en-US", "响应人名称_us", "响应人名称（冗余：按 RespondedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.respondedbyname
            new TranslationSeedItem("entity.sopcall.respondedbyname", "ja-JP", "响应人名称_jp", "响应人名称（冗余：按 RespondedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.respondedbyname
            new TranslationSeedItem("entity.sopcall.respondedbyname", "zh-CN", "响应人名称", "响应人名称（冗余：按 RespondedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopcall.respondedbyname
            new TranslationSeedItem("entity.sopcall.respondedbyname", "zh-HK", "响应人名称_hk", "响应人名称（冗余：按 RespondedBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopcall.respondedat
            new TranslationSeedItem("entity.sopcall.respondedat", "en-US", "响应时间_us", "响应时间"),
            // entity.sopcall.respondedat
            new TranslationSeedItem("entity.sopcall.respondedat", "ja-JP", "响应时间_jp", "响应时间"),
            // entity.sopcall.respondedat
            new TranslationSeedItem("entity.sopcall.respondedat", "zh-CN", "响应时间", "响应时间"),
            // entity.sopcall.respondedat
            new TranslationSeedItem("entity.sopcall.respondedat", "zh-HK", "响应时间_hk", "响应时间"),

            // entity.sopcall.responseseconds
            new TranslationSeedItem("entity.sopcall.responseseconds", "en-US", "响应时长秒_us", "响应时长（秒）"),
            // entity.sopcall.responseseconds
            new TranslationSeedItem("entity.sopcall.responseseconds", "ja-JP", "响应时长秒_jp", "响应时长（秒）"),
            // entity.sopcall.responseseconds
            new TranslationSeedItem("entity.sopcall.responseseconds", "zh-CN", "响应时长秒", "响应时长（秒）"),
            // entity.sopcall.responseseconds
            new TranslationSeedItem("entity.sopcall.responseseconds", "zh-HK", "响应时长秒_hk", "响应时长（秒）"),

            // entity.sopcall.callstatus
            new TranslationSeedItem("entity.sopcall.callstatus", "en-US", "呼叫状态_us", "呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）"),
            // entity.sopcall.callstatus
            new TranslationSeedItem("entity.sopcall.callstatus", "ja-JP", "呼叫状态_jp", "呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）"),
            // entity.sopcall.callstatus
            new TranslationSeedItem("entity.sopcall.callstatus", "zh-CN", "呼叫状态", "呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）"),
            // entity.sopcall.callstatus
            new TranslationSeedItem("entity.sopcall.callstatus", "zh-HK", "呼叫状态_hk", "呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）"),

            // entity.sopcall.workstation
            new TranslationSeedItem("entity.sopcall.workstation", "en-US", "工位_us", "工位"),
            // entity.sopcall.workstation
            new TranslationSeedItem("entity.sopcall.workstation", "ja-JP", "工位_jp", "工位"),
            // entity.sopcall.workstation
            new TranslationSeedItem("entity.sopcall.workstation", "zh-CN", "工位", "工位"),
            // entity.sopcall.workstation
            new TranslationSeedItem("entity.sopcall.workstation", "zh-HK", "工位_hk", "工位"),
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
        translation.ResourceGroup = "Sop";
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
