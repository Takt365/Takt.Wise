// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeFamilyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeFamily 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeFamily 实体国际化翻译种子（键前缀 entity.employeefamily.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeFamilyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeFamily 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeefamily 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeFamilyTranslations())
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

        TaktLogger.Information("TaktEmployeeFamily 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeFamily 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeefamily._self / entity.employeefamily.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeFamilyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeefamily._self
            new TranslationSeedItem("entity.employeefamily._self", "en-US", "Employee Family Information_us", "实体名称"),
            // entity.employeefamily._self
            new TranslationSeedItem("entity.employeefamily._self", "ja-JP", "员工家庭成员信息_jp", "实体名称"),
            // entity.employeefamily._self
            new TranslationSeedItem("entity.employeefamily._self", "zh-CN", "员工家庭成员信息", "实体名称"),
            // entity.employeefamily._self
            new TranslationSeedItem("entity.employeefamily._self", "zh-HK", "员工家庭成员信息_hk", "实体名称"),

            // entity.employeefamily.employeeid
            new TranslationSeedItem("entity.employeefamily.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeefamily.employeeid
            new TranslationSeedItem("entity.employeefamily.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeefamily.employeeid
            new TranslationSeedItem("entity.employeefamily.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeefamily.employeeid
            new TranslationSeedItem("entity.employeefamily.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeefamily.employeecode
            new TranslationSeedItem("entity.employeefamily.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeefamily.employeecode
            new TranslationSeedItem("entity.employeefamily.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeefamily.employeecode
            new TranslationSeedItem("entity.employeefamily.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeefamily.employeecode
            new TranslationSeedItem("entity.employeefamily.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeefamily.employeename
            new TranslationSeedItem("entity.employeefamily.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeefamily.employeename
            new TranslationSeedItem("entity.employeefamily.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeefamily.employeename
            new TranslationSeedItem("entity.employeefamily.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeefamily.employeename
            new TranslationSeedItem("entity.employeefamily.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeefamily.membername
            new TranslationSeedItem("entity.employeefamily.membername", "en-US", "成员姓名_us", "成员姓名"),
            // entity.employeefamily.membername
            new TranslationSeedItem("entity.employeefamily.membername", "ja-JP", "成员姓名_jp", "成员姓名"),
            // entity.employeefamily.membername
            new TranslationSeedItem("entity.employeefamily.membername", "zh-CN", "成员姓名", "成员姓名"),
            // entity.employeefamily.membername
            new TranslationSeedItem("entity.employeefamily.membername", "zh-HK", "成员姓名_hk", "成员姓名"),

            // entity.employeefamily.relationtype
            new TranslationSeedItem("entity.employeefamily.relationtype", "en-US", "关系类型_us", "与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）"),
            // entity.employeefamily.relationtype
            new TranslationSeedItem("entity.employeefamily.relationtype", "ja-JP", "关系类型_jp", "与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）"),
            // entity.employeefamily.relationtype
            new TranslationSeedItem("entity.employeefamily.relationtype", "zh-CN", "关系类型", "与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）"),
            // entity.employeefamily.relationtype
            new TranslationSeedItem("entity.employeefamily.relationtype", "zh-HK", "关系类型_hk", "与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）"),

            // entity.employeefamily.phonenumber
            new TranslationSeedItem("entity.employeefamily.phonenumber", "en-US", "联系电话_us", "联系电话"),
            // entity.employeefamily.phonenumber
            new TranslationSeedItem("entity.employeefamily.phonenumber", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.employeefamily.phonenumber
            new TranslationSeedItem("entity.employeefamily.phonenumber", "zh-CN", "联系电话", "联系电话"),
            // entity.employeefamily.phonenumber
            new TranslationSeedItem("entity.employeefamily.phonenumber", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.employeefamily.workunit
            new TranslationSeedItem("entity.employeefamily.workunit", "en-US", "工作单位_us", "工作单位"),
            // entity.employeefamily.workunit
            new TranslationSeedItem("entity.employeefamily.workunit", "ja-JP", "工作单位_jp", "工作单位"),
            // entity.employeefamily.workunit
            new TranslationSeedItem("entity.employeefamily.workunit", "zh-CN", "工作单位", "工作单位"),
            // entity.employeefamily.workunit
            new TranslationSeedItem("entity.employeefamily.workunit", "zh-HK", "工作单位_hk", "工作单位"),

            // entity.employeefamily.jobtitle
            new TranslationSeedItem("entity.employeefamily.jobtitle", "en-US", "职务_us", "职务"),
            // entity.employeefamily.jobtitle
            new TranslationSeedItem("entity.employeefamily.jobtitle", "ja-JP", "职务_jp", "职务"),
            // entity.employeefamily.jobtitle
            new TranslationSeedItem("entity.employeefamily.jobtitle", "zh-CN", "职务", "职务"),
            // entity.employeefamily.jobtitle
            new TranslationSeedItem("entity.employeefamily.jobtitle", "zh-HK", "职务_hk", "职务"),

            // entity.employeefamily.birthdate
            new TranslationSeedItem("entity.employeefamily.birthdate", "en-US", "出生日期_us", "出生日期"),
            // entity.employeefamily.birthdate
            new TranslationSeedItem("entity.employeefamily.birthdate", "ja-JP", "出生日期_jp", "出生日期"),
            // entity.employeefamily.birthdate
            new TranslationSeedItem("entity.employeefamily.birthdate", "zh-CN", "出生日期", "出生日期"),
            // entity.employeefamily.birthdate
            new TranslationSeedItem("entity.employeefamily.birthdate", "zh-HK", "出生日期_hk", "出生日期"),

            // entity.employeefamily.isemergencycontact
            new TranslationSeedItem("entity.employeefamily.isemergencycontact", "en-US", "是否紧急联系人_us", "是否紧急联系人（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeefamily.isemergencycontact
            new TranslationSeedItem("entity.employeefamily.isemergencycontact", "ja-JP", "是否紧急联系人_jp", "是否紧急联系人（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeefamily.isemergencycontact
            new TranslationSeedItem("entity.employeefamily.isemergencycontact", "zh-CN", "是否紧急联系人", "是否紧急联系人（字典 sys_yes_no；0=否 1=是）"),
            // entity.employeefamily.isemergencycontact
            new TranslationSeedItem("entity.employeefamily.isemergencycontact", "zh-HK", "是否紧急联系人_hk", "是否紧急联系人（字典 sys_yes_no；0=否 1=是）"),

            // entity.employeefamily.employee
            new TranslationSeedItem("entity.employeefamily.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeefamily.employee
            new TranslationSeedItem("entity.employeefamily.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeefamily.employee
            new TranslationSeedItem("entity.employeefamily.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeefamily.employee
            new TranslationSeedItem("entity.employeefamily.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
