// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeExperienceI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeExperience 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeExperience 实体国际化翻译种子（键前缀 entity.employeeexperience.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeExperienceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeExperience 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeexperience 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeExperienceTranslations())
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

        TaktLogger.Information("TaktEmployeeExperience 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeExperience 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeexperience._self / entity.employeeexperience.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeExperienceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeexperience._self
            new TranslationSeedItem("entity.employeeexperience._self", "en-US", "Employee Experience Information_us", "实体名称"),
            // entity.employeeexperience._self
            new TranslationSeedItem("entity.employeeexperience._self", "ja-JP", "员工外部工作经历信息_jp", "实体名称"),
            // entity.employeeexperience._self
            new TranslationSeedItem("entity.employeeexperience._self", "zh-CN", "员工外部工作经历信息", "实体名称"),
            // entity.employeeexperience._self
            new TranslationSeedItem("entity.employeeexperience._self", "zh-HK", "员工外部工作经历信息_hk", "实体名称"),

            // entity.employeeexperience.employeeid
            new TranslationSeedItem("entity.employeeexperience.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeexperience.employeeid
            new TranslationSeedItem("entity.employeeexperience.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeexperience.employeeid
            new TranslationSeedItem("entity.employeeexperience.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeexperience.employeeid
            new TranslationSeedItem("entity.employeeexperience.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeexperience.employeecode
            new TranslationSeedItem("entity.employeeexperience.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeexperience.employeecode
            new TranslationSeedItem("entity.employeeexperience.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeexperience.employeecode
            new TranslationSeedItem("entity.employeeexperience.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeexperience.employeecode
            new TranslationSeedItem("entity.employeeexperience.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeexperience.employeename
            new TranslationSeedItem("entity.employeeexperience.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeexperience.employeename
            new TranslationSeedItem("entity.employeeexperience.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeexperience.employeename
            new TranslationSeedItem("entity.employeeexperience.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeexperience.employeename
            new TranslationSeedItem("entity.employeeexperience.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeexperience.companyname
            new TranslationSeedItem("entity.employeeexperience.companyname", "en-US", "工作单位_us", "工作单位名称"),
            // entity.employeeexperience.companyname
            new TranslationSeedItem("entity.employeeexperience.companyname", "ja-JP", "工作单位_jp", "工作单位名称"),
            // entity.employeeexperience.companyname
            new TranslationSeedItem("entity.employeeexperience.companyname", "zh-CN", "工作单位", "工作单位名称"),
            // entity.employeeexperience.companyname
            new TranslationSeedItem("entity.employeeexperience.companyname", "zh-HK", "工作单位_hk", "工作单位名称"),

            // entity.employeeexperience.positionname
            new TranslationSeedItem("entity.employeeexperience.positionname", "en-US", "职位名称_us", "职位名称"),
            // entity.employeeexperience.positionname
            new TranslationSeedItem("entity.employeeexperience.positionname", "ja-JP", "职位名称_jp", "职位名称"),
            // entity.employeeexperience.positionname
            new TranslationSeedItem("entity.employeeexperience.positionname", "zh-CN", "职位名称", "职位名称"),
            // entity.employeeexperience.positionname
            new TranslationSeedItem("entity.employeeexperience.positionname", "zh-HK", "职位名称_hk", "职位名称"),

            // entity.employeeexperience.jobcontent
            new TranslationSeedItem("entity.employeeexperience.jobcontent", "en-US", "工作内容_us", "工作内容"),
            // entity.employeeexperience.jobcontent
            new TranslationSeedItem("entity.employeeexperience.jobcontent", "ja-JP", "工作内容_jp", "工作内容"),
            // entity.employeeexperience.jobcontent
            new TranslationSeedItem("entity.employeeexperience.jobcontent", "zh-CN", "工作内容", "工作内容"),
            // entity.employeeexperience.jobcontent
            new TranslationSeedItem("entity.employeeexperience.jobcontent", "zh-HK", "工作内容_hk", "工作内容"),

            // entity.employeeexperience.startdate
            new TranslationSeedItem("entity.employeeexperience.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.employeeexperience.startdate
            new TranslationSeedItem("entity.employeeexperience.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.employeeexperience.startdate
            new TranslationSeedItem("entity.employeeexperience.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.employeeexperience.startdate
            new TranslationSeedItem("entity.employeeexperience.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.employeeexperience.enddate
            new TranslationSeedItem("entity.employeeexperience.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.employeeexperience.enddate
            new TranslationSeedItem("entity.employeeexperience.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.employeeexperience.enddate
            new TranslationSeedItem("entity.employeeexperience.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.employeeexperience.enddate
            new TranslationSeedItem("entity.employeeexperience.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.employeeexperience.witnessname
            new TranslationSeedItem("entity.employeeexperience.witnessname", "en-US", "证明人姓名_us", "证明人姓名"),
            // entity.employeeexperience.witnessname
            new TranslationSeedItem("entity.employeeexperience.witnessname", "ja-JP", "证明人姓名_jp", "证明人姓名"),
            // entity.employeeexperience.witnessname
            new TranslationSeedItem("entity.employeeexperience.witnessname", "zh-CN", "证明人姓名", "证明人姓名"),
            // entity.employeeexperience.witnessname
            new TranslationSeedItem("entity.employeeexperience.witnessname", "zh-HK", "证明人姓名_hk", "证明人姓名"),

            // entity.employeeexperience.witnessphone
            new TranslationSeedItem("entity.employeeexperience.witnessphone", "en-US", "证明人电话_us", "证明人电话"),
            // entity.employeeexperience.witnessphone
            new TranslationSeedItem("entity.employeeexperience.witnessphone", "ja-JP", "证明人电话_jp", "证明人电话"),
            // entity.employeeexperience.witnessphone
            new TranslationSeedItem("entity.employeeexperience.witnessphone", "zh-CN", "证明人电话", "证明人电话"),
            // entity.employeeexperience.witnessphone
            new TranslationSeedItem("entity.employeeexperience.witnessphone", "zh-HK", "证明人电话_hk", "证明人电话"),

            // entity.employeeexperience.employee
            new TranslationSeedItem("entity.employeeexperience.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeexperience.employee
            new TranslationSeedItem("entity.employeeexperience.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeexperience.employee
            new TranslationSeedItem("entity.employeeexperience.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeexperience.employee
            new TranslationSeedItem("entity.employeeexperience.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
