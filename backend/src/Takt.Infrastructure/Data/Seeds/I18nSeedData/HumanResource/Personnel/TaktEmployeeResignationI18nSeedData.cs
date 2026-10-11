// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeResignationI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeResignation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeResignation 实体国际化翻译种子（键前缀 entity.employeeresignation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeResignationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeResignation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeresignation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeResignationTranslations())
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

        TaktLogger.Information("TaktEmployeeResignation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeResignation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeresignation._self / entity.employeeresignation.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeResignationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeresignation._self
            new TranslationSeedItem("entity.employeeresignation._self", "en-US", "Employee Resignation Information_us", "实体名称"),
            // entity.employeeresignation._self
            new TranslationSeedItem("entity.employeeresignation._self", "ja-JP", "员工离职办理记录信息_jp", "实体名称"),
            // entity.employeeresignation._self
            new TranslationSeedItem("entity.employeeresignation._self", "zh-CN", "员工离职办理记录信息", "实体名称"),
            // entity.employeeresignation._self
            new TranslationSeedItem("entity.employeeresignation._self", "zh-HK", "员工离职办理记录信息_hk", "实体名称"),

            // entity.employeeresignation.employeeid
            new TranslationSeedItem("entity.employeeresignation.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeresignation.employeeid
            new TranslationSeedItem("entity.employeeresignation.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeresignation.employeeid
            new TranslationSeedItem("entity.employeeresignation.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeresignation.employeeid
            new TranslationSeedItem("entity.employeeresignation.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeresignation.employeecode
            new TranslationSeedItem("entity.employeeresignation.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeresignation.employeecode
            new TranslationSeedItem("entity.employeeresignation.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeresignation.employeecode
            new TranslationSeedItem("entity.employeeresignation.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeresignation.employeecode
            new TranslationSeedItem("entity.employeeresignation.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeresignation.employeename
            new TranslationSeedItem("entity.employeeresignation.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeresignation.employeename
            new TranslationSeedItem("entity.employeeresignation.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeresignation.employeename
            new TranslationSeedItem("entity.employeeresignation.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeresignation.employeename
            new TranslationSeedItem("entity.employeeresignation.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeresignation.resignationtype
            new TranslationSeedItem("entity.employeeresignation.resignationtype", "en-US", "离职类型_us", "离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employeeresignation.resignationtype
            new TranslationSeedItem("entity.employeeresignation.resignationtype", "ja-JP", "离职类型_jp", "离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employeeresignation.resignationtype
            new TranslationSeedItem("entity.employeeresignation.resignationtype", "zh-CN", "离职类型", "离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employeeresignation.resignationtype
            new TranslationSeedItem("entity.employeeresignation.resignationtype", "zh-HK", "离职类型_hk", "离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),

            // entity.employeeresignation.applydate
            new TranslationSeedItem("entity.employeeresignation.applydate", "en-US", "申请日期_us", "申请日期"),
            // entity.employeeresignation.applydate
            new TranslationSeedItem("entity.employeeresignation.applydate", "ja-JP", "申请日期_jp", "申请日期"),
            // entity.employeeresignation.applydate
            new TranslationSeedItem("entity.employeeresignation.applydate", "zh-CN", "申请日期", "申请日期"),
            // entity.employeeresignation.applydate
            new TranslationSeedItem("entity.employeeresignation.applydate", "zh-HK", "申请日期_hk", "申请日期"),

            // entity.employeeresignation.lastworkdate
            new TranslationSeedItem("entity.employeeresignation.lastworkdate", "en-US", "最后工作日_us", "最后工作日"),
            // entity.employeeresignation.lastworkdate
            new TranslationSeedItem("entity.employeeresignation.lastworkdate", "ja-JP", "最后工作日_jp", "最后工作日"),
            // entity.employeeresignation.lastworkdate
            new TranslationSeedItem("entity.employeeresignation.lastworkdate", "zh-CN", "最后工作日", "最后工作日"),
            // entity.employeeresignation.lastworkdate
            new TranslationSeedItem("entity.employeeresignation.lastworkdate", "zh-HK", "最后工作日_hk", "最后工作日"),

            // entity.employeeresignation.terminationdate
            new TranslationSeedItem("entity.employeeresignation.terminationdate", "en-US", "实际离职日期_us", "实际离职日期"),
            // entity.employeeresignation.terminationdate
            new TranslationSeedItem("entity.employeeresignation.terminationdate", "ja-JP", "实际离职日期_jp", "实际离职日期"),
            // entity.employeeresignation.terminationdate
            new TranslationSeedItem("entity.employeeresignation.terminationdate", "zh-CN", "实际离职日期", "实际离职日期"),
            // entity.employeeresignation.terminationdate
            new TranslationSeedItem("entity.employeeresignation.terminationdate", "zh-HK", "实际离职日期_hk", "实际离职日期"),

            // entity.employeeresignation.reason
            new TranslationSeedItem("entity.employeeresignation.reason", "en-US", "离职原因_us", "离职原因"),
            // entity.employeeresignation.reason
            new TranslationSeedItem("entity.employeeresignation.reason", "ja-JP", "离职原因_jp", "离职原因"),
            // entity.employeeresignation.reason
            new TranslationSeedItem("entity.employeeresignation.reason", "zh-CN", "离职原因", "离职原因"),
            // entity.employeeresignation.reason
            new TranslationSeedItem("entity.employeeresignation.reason", "zh-HK", "离职原因_hk", "离职原因"),

            // entity.employeeresignation.handovernotes
            new TranslationSeedItem("entity.employeeresignation.handovernotes", "en-US", "工作交接说明_us", "工作交接说明"),
            // entity.employeeresignation.handovernotes
            new TranslationSeedItem("entity.employeeresignation.handovernotes", "ja-JP", "工作交接说明_jp", "工作交接说明"),
            // entity.employeeresignation.handovernotes
            new TranslationSeedItem("entity.employeeresignation.handovernotes", "zh-CN", "工作交接说明", "工作交接说明"),
            // entity.employeeresignation.handovernotes
            new TranslationSeedItem("entity.employeeresignation.handovernotes", "zh-HK", "工作交接说明_hk", "工作交接说明"),

            // entity.employeeresignation.employee
            new TranslationSeedItem("entity.employeeresignation.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeresignation.employee
            new TranslationSeedItem("entity.employeeresignation.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeresignation.employee
            new TranslationSeedItem("entity.employeeresignation.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeresignation.employee
            new TranslationSeedItem("entity.employeeresignation.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
        translation.ResourceGroup = "Personnel";
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
