// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeEducation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeEducation 实体国际化翻译种子（键前缀 entity.employeeeducation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeEducationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeEducation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeeducation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeEducationTranslations())
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

        TaktLogger.Information("TaktEmployeeEducation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeEducation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeeducation._self / entity.employeeeducation.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeEducationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeeducation._self
            new TranslationSeedItem("entity.employeeeducation._self", "en-US", "Employee Education Information_us", "实体名称"),
            // entity.employeeeducation._self
            new TranslationSeedItem("entity.employeeeducation._self", "ja-JP", "员工教育经历信息_jp", "实体名称"),
            // entity.employeeeducation._self
            new TranslationSeedItem("entity.employeeeducation._self", "zh-CN", "员工教育经历信息", "实体名称"),
            // entity.employeeeducation._self
            new TranslationSeedItem("entity.employeeeducation._self", "zh-HK", "员工教育经历信息_hk", "实体名称"),

            // entity.employeeeducation.employeeid
            new TranslationSeedItem("entity.employeeeducation.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeeducation.employeeid
            new TranslationSeedItem("entity.employeeeducation.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeeducation.employeeid
            new TranslationSeedItem("entity.employeeeducation.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeeducation.employeeid
            new TranslationSeedItem("entity.employeeeducation.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeeducation.employeecode
            new TranslationSeedItem("entity.employeeeducation.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeeducation.employeecode
            new TranslationSeedItem("entity.employeeeducation.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeeducation.employeecode
            new TranslationSeedItem("entity.employeeeducation.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeeducation.employeecode
            new TranslationSeedItem("entity.employeeeducation.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeeducation.employeename
            new TranslationSeedItem("entity.employeeeducation.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeeducation.employeename
            new TranslationSeedItem("entity.employeeeducation.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeeducation.employeename
            new TranslationSeedItem("entity.employeeeducation.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeeducation.employeename
            new TranslationSeedItem("entity.employeeeducation.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeeducation.schoolname
            new TranslationSeedItem("entity.employeeeducation.schoolname", "en-US", "学校名称_us", "学校名称"),
            // entity.employeeeducation.schoolname
            new TranslationSeedItem("entity.employeeeducation.schoolname", "ja-JP", "学校名称_jp", "学校名称"),
            // entity.employeeeducation.schoolname
            new TranslationSeedItem("entity.employeeeducation.schoolname", "zh-CN", "学校名称", "学校名称"),
            // entity.employeeeducation.schoolname
            new TranslationSeedItem("entity.employeeeducation.schoolname", "zh-HK", "学校名称_hk", "学校名称"),

            // entity.employeeeducation.educationlevel
            new TranslationSeedItem("entity.employeeeducation.educationlevel", "en-US", "学历层次_us", "学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）"),
            // entity.employeeeducation.educationlevel
            new TranslationSeedItem("entity.employeeeducation.educationlevel", "ja-JP", "学历层次_jp", "学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）"),
            // entity.employeeeducation.educationlevel
            new TranslationSeedItem("entity.employeeeducation.educationlevel", "zh-CN", "学历层次", "学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）"),
            // entity.employeeeducation.educationlevel
            new TranslationSeedItem("entity.employeeeducation.educationlevel", "zh-HK", "学历层次_hk", "学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）"),

            // entity.employeeeducation.degreelevel
            new TranslationSeedItem("entity.employeeeducation.degreelevel", "en-US", "学位层次_us", "学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）"),
            // entity.employeeeducation.degreelevel
            new TranslationSeedItem("entity.employeeeducation.degreelevel", "ja-JP", "学位层次_jp", "学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）"),
            // entity.employeeeducation.degreelevel
            new TranslationSeedItem("entity.employeeeducation.degreelevel", "zh-CN", "学位层次", "学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）"),
            // entity.employeeeducation.degreelevel
            new TranslationSeedItem("entity.employeeeducation.degreelevel", "zh-HK", "学位层次_hk", "学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）"),

            // entity.employeeeducation.majorname
            new TranslationSeedItem("entity.employeeeducation.majorname", "en-US", "专业名称_us", "专业名称"),
            // entity.employeeeducation.majorname
            new TranslationSeedItem("entity.employeeeducation.majorname", "ja-JP", "专业名称_jp", "专业名称"),
            // entity.employeeeducation.majorname
            new TranslationSeedItem("entity.employeeeducation.majorname", "zh-CN", "专业名称", "专业名称"),
            // entity.employeeeducation.majorname
            new TranslationSeedItem("entity.employeeeducation.majorname", "zh-HK", "专业名称_hk", "专业名称"),

            // entity.employeeeducation.certificatecode
            new TranslationSeedItem("entity.employeeeducation.certificatecode", "en-US", "证书编码_us", "证书编码"),
            // entity.employeeeducation.certificatecode
            new TranslationSeedItem("entity.employeeeducation.certificatecode", "ja-JP", "证书编码_jp", "证书编码"),
            // entity.employeeeducation.certificatecode
            new TranslationSeedItem("entity.employeeeducation.certificatecode", "zh-CN", "证书编码", "证书编码"),
            // entity.employeeeducation.certificatecode
            new TranslationSeedItem("entity.employeeeducation.certificatecode", "zh-HK", "证书编码_hk", "证书编码"),

            // entity.employeeeducation.startdate
            new TranslationSeedItem("entity.employeeeducation.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.employeeeducation.startdate
            new TranslationSeedItem("entity.employeeeducation.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.employeeeducation.startdate
            new TranslationSeedItem("entity.employeeeducation.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.employeeeducation.startdate
            new TranslationSeedItem("entity.employeeeducation.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.employeeeducation.enddate
            new TranslationSeedItem("entity.employeeeducation.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.employeeeducation.enddate
            new TranslationSeedItem("entity.employeeeducation.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.employeeeducation.enddate
            new TranslationSeedItem("entity.employeeeducation.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.employeeeducation.enddate
            new TranslationSeedItem("entity.employeeeducation.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.employeeeducation.ishighest
            new TranslationSeedItem("entity.employeeeducation.ishighest", "en-US", "是否最高学历_us", "是否最高学历（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeeeducation.ishighest
            new TranslationSeedItem("entity.employeeeducation.ishighest", "ja-JP", "是否最高学历_jp", "是否最高学历（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeeeducation.ishighest
            new TranslationSeedItem("entity.employeeeducation.ishighest", "zh-CN", "是否最高学历", "是否最高学历（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeeeducation.ishighest
            new TranslationSeedItem("entity.employeeeducation.ishighest", "zh-HK", "是否最高学历_hk", "是否最高学历（字典 sys_yes_no；0=否 1=是）"),

            // entity.employeeeducation.employee
            new TranslationSeedItem("entity.employeeeducation.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeeducation.employee
            new TranslationSeedItem("entity.employeeeducation.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeeducation.employee
            new TranslationSeedItem("entity.employeeeducation.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeeducation.employee
            new TranslationSeedItem("entity.employeeeducation.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
