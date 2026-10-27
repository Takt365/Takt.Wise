// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionTeam 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps;

/// <summary>
/// TaktProductionTeam 实体国际化翻译种子（键前缀 entity.productionteam.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionTeamI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionTeam 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionteam 实体翻译...", tenantCode);

        foreach (var item in GetProductionTeamTranslations())
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

        TaktLogger.Information("TaktProductionTeam 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionTeam 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionteam._self / entity.productionteam.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionTeamTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "en-US", "Production Team Information_us", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "ja-JP", "生产班组信息_jp", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "zh-CN", "生产班组信息", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "zh-HK", "生产班组信息_hk", "实体名称"),

            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "en-US", "班组编码_us", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等；前端表单选择编码规则后自动通过 TaktNumbering 班组编码规则生成并展示，非手输；单据类型菜单：生产班组）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "ja-JP", "班组编码_jp", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等；前端表单选择编码规则后自动通过 TaktNumbering 班组编码规则生成并展示，非手输；单据类型菜单：生产班组）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "zh-CN", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等；前端表单选择编码规则后自动通过 TaktNumbering 班组编码规则生成并展示，非手输；单据类型菜单：生产班组）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "zh-HK", "班组编码_hk", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等；前端表单选择编码规则后自动通过 TaktNumbering 班组编码规则生成并展示，非手输；单据类型菜单：生产班组）"),

            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "en-US", "班组名称_us", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "ja-JP", "班组名称_jp", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "zh-CN", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "zh-HK", "班组名称_hk", "班组名称（显示名称，如：SMT一班、手插二班等）"),

            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "en-US", "班组分类编码_us", "班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "ja-JP", "班组分类编码_jp", "班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "zh-CN", "班组分类编码", "班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "zh-HK", "班组分类编码_hk", "班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）"),

            // entity.productionteam.teamleaderemployeeid
            new TranslationSeedItem("entity.productionteam.teamleaderemployeeid", "en-US", "班组长ID_us", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionteam.teamleaderemployeeid
            new TranslationSeedItem("entity.productionteam.teamleaderemployeeid", "ja-JP", "班组长ID_jp", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionteam.teamleaderemployeeid
            new TranslationSeedItem("entity.productionteam.teamleaderemployeeid", "zh-CN", "班组长ID", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionteam.teamleaderemployeeid
            new TranslationSeedItem("entity.productionteam.teamleaderemployeeid", "zh-HK", "班组长ID_hk", "班组长（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "en-US", "班组长名称_us", "班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "ja-JP", "班组长名称_jp", "班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "zh-CN", "班组长名称", "班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "zh-HK", "班组长名称_hk", "班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.productionteam.teamstatus
            new TranslationSeedItem("entity.productionteam.teamstatus", "en-US", "启用状态_us", "启用状态（字典 sys_normal_disable；0=禁用，1=启用）"),
            // entity.productionteam.teamstatus
            new TranslationSeedItem("entity.productionteam.teamstatus", "ja-JP", "启用状态_jp", "启用状态（字典 sys_normal_disable；0=禁用，1=启用）"),
            // entity.productionteam.teamstatus
            new TranslationSeedItem("entity.productionteam.teamstatus", "zh-CN", "启用状态", "启用状态（字典 sys_normal_disable；0=禁用，1=启用）"),
            // entity.productionteam.teamstatus
            new TranslationSeedItem("entity.productionteam.teamstatus", "zh-HK", "启用状态_hk", "启用状态（字典 sys_normal_disable；0=禁用，1=启用）"),

            // entity.productionteam.teamequipmentlist
            new TranslationSeedItem("entity.productionteam.teamequipmentlist", "en-US", "设备组明细_us", "设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）"),
            // entity.productionteam.teamequipmentlist
            new TranslationSeedItem("entity.productionteam.teamequipmentlist", "ja-JP", "设备组明细_jp", "设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）"),
            // entity.productionteam.teamequipmentlist
            new TranslationSeedItem("entity.productionteam.teamequipmentlist", "zh-CN", "设备组明细", "设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）"),
            // entity.productionteam.teamequipmentlist
            new TranslationSeedItem("entity.productionteam.teamequipmentlist", "zh-HK", "设备组明细_hk", "设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）"),
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
        translation.ResourceGroup = "Mps";
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
