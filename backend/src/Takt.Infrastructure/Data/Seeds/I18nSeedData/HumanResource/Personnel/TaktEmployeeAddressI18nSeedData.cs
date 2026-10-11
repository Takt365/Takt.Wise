// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeAddress 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeAddress 实体国际化翻译种子（键前缀 entity.employeeaddress.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeAddressI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeAddress 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeaddress 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeAddressTranslations())
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

        TaktLogger.Information("TaktEmployeeAddress 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeAddress 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeaddress._self / entity.employeeaddress.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeAddressTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeaddress._self
            new TranslationSeedItem("entity.employeeaddress._self", "en-US", "Employee Address Information_us", "实体名称"),
            // entity.employeeaddress._self
            new TranslationSeedItem("entity.employeeaddress._self", "ja-JP", "员工地址信息_jp", "实体名称"),
            // entity.employeeaddress._self
            new TranslationSeedItem("entity.employeeaddress._self", "zh-CN", "员工地址信息", "实体名称"),
            // entity.employeeaddress._self
            new TranslationSeedItem("entity.employeeaddress._self", "zh-HK", "员工地址信息_hk", "实体名称"),

            // entity.employeeaddress.employeeid
            new TranslationSeedItem("entity.employeeaddress.employeeid", "en-US", "员工ID_us", "员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeaddress.employeeid
            new TranslationSeedItem("entity.employeeaddress.employeeid", "ja-JP", "员工ID_jp", "员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeaddress.employeeid
            new TranslationSeedItem("entity.employeeaddress.employeeid", "zh-CN", "员工ID", "员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeaddress.employeeid
            new TranslationSeedItem("entity.employeeaddress.employeeid", "zh-HK", "员工ID_hk", "员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeaddress.employeecode
            new TranslationSeedItem("entity.employeeaddress.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeaddress.employeecode
            new TranslationSeedItem("entity.employeeaddress.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeaddress.employeecode
            new TranslationSeedItem("entity.employeeaddress.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeaddress.employeecode
            new TranslationSeedItem("entity.employeeaddress.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeaddress.employeename
            new TranslationSeedItem("entity.employeeaddress.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeaddress.employeename
            new TranslationSeedItem("entity.employeeaddress.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeaddress.employeename
            new TranslationSeedItem("entity.employeeaddress.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeaddress.employeename
            new TranslationSeedItem("entity.employeeaddress.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeaddress.addresstype
            new TranslationSeedItem("entity.employeeaddress.addresstype", "en-US", "地址类型_us", "地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）"),
            // entity.employeeaddress.addresstype
            new TranslationSeedItem("entity.employeeaddress.addresstype", "ja-JP", "地址类型_jp", "地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）"),
            // entity.employeeaddress.addresstype
            new TranslationSeedItem("entity.employeeaddress.addresstype", "zh-CN", "地址类型", "地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）"),
            // entity.employeeaddress.addresstype
            new TranslationSeedItem("entity.employeeaddress.addresstype", "zh-HK", "地址类型_hk", "地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）"),

            // entity.employeeaddress.country
            new TranslationSeedItem("entity.employeeaddress.country", "en-US", "国家_us", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.employeeaddress.country
            new TranslationSeedItem("entity.employeeaddress.country", "ja-JP", "国家_jp", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.employeeaddress.country
            new TranslationSeedItem("entity.employeeaddress.country", "zh-CN", "国家", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.employeeaddress.country
            new TranslationSeedItem("entity.employeeaddress.country", "zh-HK", "国家_hk", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.employeeaddress.province
            new TranslationSeedItem("entity.employeeaddress.province", "en-US", "省_us", "省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.employeeaddress.province
            new TranslationSeedItem("entity.employeeaddress.province", "ja-JP", "省_jp", "省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.employeeaddress.province
            new TranslationSeedItem("entity.employeeaddress.province", "zh-CN", "省", "省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.employeeaddress.province
            new TranslationSeedItem("entity.employeeaddress.province", "zh-HK", "省_hk", "省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.employeeaddress.city
            new TranslationSeedItem("entity.employeeaddress.city", "en-US", "市_us", "市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.employeeaddress.city
            new TranslationSeedItem("entity.employeeaddress.city", "ja-JP", "市_jp", "市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.employeeaddress.city
            new TranslationSeedItem("entity.employeeaddress.city", "zh-CN", "市", "市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.employeeaddress.city
            new TranslationSeedItem("entity.employeeaddress.city", "zh-HK", "市_hk", "市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.employeeaddress.district
            new TranslationSeedItem("entity.employeeaddress.district", "en-US", "区县_us", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.employeeaddress.district
            new TranslationSeedItem("entity.employeeaddress.district", "ja-JP", "区县_jp", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.employeeaddress.district
            new TranslationSeedItem("entity.employeeaddress.district", "zh-CN", "区县", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),
            // entity.employeeaddress.district
            new TranslationSeedItem("entity.employeeaddress.district", "zh-HK", "区县_hk", "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）"),

            // entity.employeeaddress.address1
            new TranslationSeedItem("entity.employeeaddress.address1", "en-US", "地址1_us", "地址1（详细地址行1）"),
            // entity.employeeaddress.address1
            new TranslationSeedItem("entity.employeeaddress.address1", "ja-JP", "地址1_jp", "地址1（详细地址行1）"),
            // entity.employeeaddress.address1
            new TranslationSeedItem("entity.employeeaddress.address1", "zh-CN", "地址1", "地址1（详细地址行1）"),
            // entity.employeeaddress.address1
            new TranslationSeedItem("entity.employeeaddress.address1", "zh-HK", "地址1_hk", "地址1（详细地址行1）"),

            // entity.employeeaddress.address2
            new TranslationSeedItem("entity.employeeaddress.address2", "en-US", "地址2_us", "地址2（详细地址行2）"),
            // entity.employeeaddress.address2
            new TranslationSeedItem("entity.employeeaddress.address2", "ja-JP", "地址2_jp", "地址2（详细地址行2）"),
            // entity.employeeaddress.address2
            new TranslationSeedItem("entity.employeeaddress.address2", "zh-CN", "地址2", "地址2（详细地址行2）"),
            // entity.employeeaddress.address2
            new TranslationSeedItem("entity.employeeaddress.address2", "zh-HK", "地址2_hk", "地址2（详细地址行2）"),

            // entity.employeeaddress.employee
            new TranslationSeedItem("entity.employeeaddress.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeaddress.employee
            new TranslationSeedItem("entity.employeeaddress.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeaddress.employee
            new TranslationSeedItem("entity.employeeaddress.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeaddress.employee
            new TranslationSeedItem("entity.employeeaddress.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
