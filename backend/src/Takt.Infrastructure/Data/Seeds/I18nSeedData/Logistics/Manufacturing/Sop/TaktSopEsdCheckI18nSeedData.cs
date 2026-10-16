// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopEsdCheckI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopEsdCheck 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSopEsdCheck 实体国际化翻译种子（键前缀 entity.sopesdcheck.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopEsdCheckI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopEsdCheck 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopesdcheck 实体翻译...", tenantCode);

        foreach (var item in GetSopEsdCheckTranslations())
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

        TaktLogger.Information("TaktSopEsdCheck 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopEsdCheck 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopesdcheck._self / entity.sopesdcheck.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopEsdCheckTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopesdcheck._self
            new TranslationSeedItem("entity.sopesdcheck._self", "en-US", "Sop Esd Check Information_us", "实体名称"),
            // entity.sopesdcheck._self
            new TranslationSeedItem("entity.sopesdcheck._self", "ja-JP", "SOP ESD 检查信息_jp", "实体名称"),
            // entity.sopesdcheck._self
            new TranslationSeedItem("entity.sopesdcheck._self", "zh-CN", "SOP ESD 检查信息", "实体名称"),
            // entity.sopesdcheck._self
            new TranslationSeedItem("entity.sopesdcheck._self", "zh-HK", "SOP ESD 检查信息_hk", "实体名称"),

            // entity.sopesdcheck.workstationid
            new TranslationSeedItem("entity.sopesdcheck.workstationid", "en-US", "工位ID_us", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopesdcheck.workstationid
            new TranslationSeedItem("entity.sopesdcheck.workstationid", "ja-JP", "工位ID_jp", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopesdcheck.workstationid
            new TranslationSeedItem("entity.sopesdcheck.workstationid", "zh-CN", "工位ID", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopesdcheck.workstationid
            new TranslationSeedItem("entity.sopesdcheck.workstationid", "zh-HK", "工位ID_hk", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),

            // entity.sopesdcheck.execid
            new TranslationSeedItem("entity.sopesdcheck.execid", "en-US", "执行追溯ID_us", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopesdcheck.execid
            new TranslationSeedItem("entity.sopesdcheck.execid", "ja-JP", "执行追溯ID_jp", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopesdcheck.execid
            new TranslationSeedItem("entity.sopesdcheck.execid", "zh-CN", "执行追溯ID", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopesdcheck.execid
            new TranslationSeedItem("entity.sopesdcheck.execid", "zh-HK", "执行追溯ID_hk", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),

            // entity.sopesdcheck.employeeid
            new TranslationSeedItem("entity.sopesdcheck.employeeid", "en-US", "员工ID_us", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopesdcheck.employeeid
            new TranslationSeedItem("entity.sopesdcheck.employeeid", "ja-JP", "员工ID_jp", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopesdcheck.employeeid
            new TranslationSeedItem("entity.sopesdcheck.employeeid", "zh-CN", "员工ID", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopesdcheck.employeeid
            new TranslationSeedItem("entity.sopesdcheck.employeeid", "zh-HK", "员工ID_hk", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopesdcheck.employeename
            new TranslationSeedItem("entity.sopesdcheck.employeename", "en-US", "员工名称_us", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopesdcheck.employeename
            new TranslationSeedItem("entity.sopesdcheck.employeename", "ja-JP", "员工名称_jp", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopesdcheck.employeename
            new TranslationSeedItem("entity.sopesdcheck.employeename", "zh-CN", "员工名称", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopesdcheck.employeename
            new TranslationSeedItem("entity.sopesdcheck.employeename", "zh-HK", "员工名称_hk", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopesdcheck.devicecode
            new TranslationSeedItem("entity.sopesdcheck.devicecode", "en-US", "监测设备编码_us", "监测设备编码"),
            // entity.sopesdcheck.devicecode
            new TranslationSeedItem("entity.sopesdcheck.devicecode", "ja-JP", "监测设备编码_jp", "监测设备编码"),
            // entity.sopesdcheck.devicecode
            new TranslationSeedItem("entity.sopesdcheck.devicecode", "zh-CN", "监测设备编码", "监测设备编码"),
            // entity.sopesdcheck.devicecode
            new TranslationSeedItem("entity.sopesdcheck.devicecode", "zh-HK", "监测设备编码_hk", "监测设备编码"),

            // entity.sopesdcheck.resistancevalue
            new TranslationSeedItem("entity.sopesdcheck.resistancevalue", "en-US", "阻值兆欧_us", "阻值（兆欧）"),
            // entity.sopesdcheck.resistancevalue
            new TranslationSeedItem("entity.sopesdcheck.resistancevalue", "ja-JP", "阻值兆欧_jp", "阻值（兆欧）"),
            // entity.sopesdcheck.resistancevalue
            new TranslationSeedItem("entity.sopesdcheck.resistancevalue", "zh-CN", "阻值兆欧", "阻值（兆欧）"),
            // entity.sopesdcheck.resistancevalue
            new TranslationSeedItem("entity.sopesdcheck.resistancevalue", "zh-HK", "阻值兆欧_hk", "阻值（兆欧）"),

            // entity.sopesdcheck.iscompliant
            new TranslationSeedItem("entity.sopesdcheck.iscompliant", "en-US", "达标_us", "达标（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.iscompliant
            new TranslationSeedItem("entity.sopesdcheck.iscompliant", "ja-JP", "达标_jp", "达标（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.iscompliant
            new TranslationSeedItem("entity.sopesdcheck.iscompliant", "zh-CN", "达标", "达标（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.iscompliant
            new TranslationSeedItem("entity.sopesdcheck.iscompliant", "zh-HK", "达标_hk", "达标（字典 sys_yes_no；0=否，1=是）"),

            // entity.sopesdcheck.lockscreentriggered
            new TranslationSeedItem("entity.sopesdcheck.lockscreentriggered", "en-US", "锁屏_us", "锁屏（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.lockscreentriggered
            new TranslationSeedItem("entity.sopesdcheck.lockscreentriggered", "ja-JP", "锁屏_jp", "锁屏（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.lockscreentriggered
            new TranslationSeedItem("entity.sopesdcheck.lockscreentriggered", "zh-CN", "锁屏", "锁屏（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopesdcheck.lockscreentriggered
            new TranslationSeedItem("entity.sopesdcheck.lockscreentriggered", "zh-HK", "锁屏_hk", "锁屏（字典 sys_yes_no；0=否，1=是）"),

            // entity.sopesdcheck.checkedat
            new TranslationSeedItem("entity.sopesdcheck.checkedat", "en-US", "检查时间_us", "检查时间"),
            // entity.sopesdcheck.checkedat
            new TranslationSeedItem("entity.sopesdcheck.checkedat", "ja-JP", "检查时间_jp", "检查时间"),
            // entity.sopesdcheck.checkedat
            new TranslationSeedItem("entity.sopesdcheck.checkedat", "zh-CN", "检查时间", "检查时间"),
            // entity.sopesdcheck.checkedat
            new TranslationSeedItem("entity.sopesdcheck.checkedat", "zh-HK", "检查时间_hk", "检查时间"),

            // entity.sopesdcheck.workstation
            new TranslationSeedItem("entity.sopesdcheck.workstation", "en-US", "工位_us", "工位"),
            // entity.sopesdcheck.workstation
            new TranslationSeedItem("entity.sopesdcheck.workstation", "ja-JP", "工位_jp", "工位"),
            // entity.sopesdcheck.workstation
            new TranslationSeedItem("entity.sopesdcheck.workstation", "zh-CN", "工位", "工位"),
            // entity.sopesdcheck.workstation
            new TranslationSeedItem("entity.sopesdcheck.workstation", "zh-HK", "工位_hk", "工位"),
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
