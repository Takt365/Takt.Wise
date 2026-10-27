// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeContractI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeContract 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeContract 实体国际化翻译种子（键前缀 entity.employeecontract.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeContractI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeContract 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeecontract 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeContractTranslations())
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

        TaktLogger.Information("TaktEmployeeContract 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeContract 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeecontract._self / entity.employeecontract.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeContractTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeecontract._self
            new TranslationSeedItem("entity.employeecontract._self", "en-US", "Employee Contract Information_us", "实体名称"),
            // entity.employeecontract._self
            new TranslationSeedItem("entity.employeecontract._self", "ja-JP", "员工劳动合同信息_jp", "实体名称"),
            // entity.employeecontract._self
            new TranslationSeedItem("entity.employeecontract._self", "zh-CN", "员工劳动合同信息", "实体名称"),
            // entity.employeecontract._self
            new TranslationSeedItem("entity.employeecontract._self", "zh-HK", "员工劳动合同信息_hk", "实体名称"),

            // entity.employeecontract.employeeid
            new TranslationSeedItem("entity.employeecontract.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeecontract.employeeid
            new TranslationSeedItem("entity.employeecontract.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeecontract.employeeid
            new TranslationSeedItem("entity.employeecontract.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeecontract.employeeid
            new TranslationSeedItem("entity.employeecontract.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeecontract.employeecode
            new TranslationSeedItem("entity.employeecontract.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeecontract.employeecode
            new TranslationSeedItem("entity.employeecontract.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeecontract.employeecode
            new TranslationSeedItem("entity.employeecontract.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeecontract.employeecode
            new TranslationSeedItem("entity.employeecontract.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeecontract.employeename
            new TranslationSeedItem("entity.employeecontract.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeecontract.employeename
            new TranslationSeedItem("entity.employeecontract.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeecontract.employeename
            new TranslationSeedItem("entity.employeecontract.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeecontract.employeename
            new TranslationSeedItem("entity.employeecontract.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeecontract.contractcode
            new TranslationSeedItem("entity.employeecontract.contractcode", "en-US", "合同编码_us", "合同编码（前端表单选择编码规则后自动通过 TaktNumbering 合同编码规则生成并展示，非手输；单据类型菜单：员工合同）"),
            // entity.employeecontract.contractcode
            new TranslationSeedItem("entity.employeecontract.contractcode", "ja-JP", "合同编码_jp", "合同编码（前端表单选择编码规则后自动通过 TaktNumbering 合同编码规则生成并展示，非手输；单据类型菜单：员工合同）"),
            // entity.employeecontract.contractcode
            new TranslationSeedItem("entity.employeecontract.contractcode", "zh-CN", "合同编码", "合同编码（前端表单选择编码规则后自动通过 TaktNumbering 合同编码规则生成并展示，非手输；单据类型菜单：员工合同）"),
            // entity.employeecontract.contractcode
            new TranslationSeedItem("entity.employeecontract.contractcode", "zh-HK", "合同编码_hk", "合同编码（前端表单选择编码规则后自动通过 TaktNumbering 合同编码规则生成并展示，非手输；单据类型菜单：员工合同）"),

            // entity.employeecontract.contracttype
            new TranslationSeedItem("entity.employeecontract.contracttype", "en-US", "合同类型_us", "合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）"),
            // entity.employeecontract.contracttype
            new TranslationSeedItem("entity.employeecontract.contracttype", "ja-JP", "合同类型_jp", "合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）"),
            // entity.employeecontract.contracttype
            new TranslationSeedItem("entity.employeecontract.contracttype", "zh-CN", "合同类型", "合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）"),
            // entity.employeecontract.contracttype
            new TranslationSeedItem("entity.employeecontract.contracttype", "zh-HK", "合同类型_hk", "合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）"),

            // entity.employeecontract.startdate
            new TranslationSeedItem("entity.employeecontract.startdate", "en-US", "合同开始日期_us", "合同开始日期"),
            // entity.employeecontract.startdate
            new TranslationSeedItem("entity.employeecontract.startdate", "ja-JP", "合同开始日期_jp", "合同开始日期"),
            // entity.employeecontract.startdate
            new TranslationSeedItem("entity.employeecontract.startdate", "zh-CN", "合同开始日期", "合同开始日期"),
            // entity.employeecontract.startdate
            new TranslationSeedItem("entity.employeecontract.startdate", "zh-HK", "合同开始日期_hk", "合同开始日期"),

            // entity.employeecontract.enddate
            new TranslationSeedItem("entity.employeecontract.enddate", "en-US", "合同结束日期_us", "合同结束日期"),
            // entity.employeecontract.enddate
            new TranslationSeedItem("entity.employeecontract.enddate", "ja-JP", "合同结束日期_jp", "合同结束日期"),
            // entity.employeecontract.enddate
            new TranslationSeedItem("entity.employeecontract.enddate", "zh-CN", "合同结束日期", "合同结束日期"),
            // entity.employeecontract.enddate
            new TranslationSeedItem("entity.employeecontract.enddate", "zh-HK", "合同结束日期_hk", "合同结束日期"),

            // entity.employeecontract.probationenddate
            new TranslationSeedItem("entity.employeecontract.probationenddate", "en-US", "试用期结束日期_us", "试用期结束日期"),
            // entity.employeecontract.probationenddate
            new TranslationSeedItem("entity.employeecontract.probationenddate", "ja-JP", "试用期结束日期_jp", "试用期结束日期"),
            // entity.employeecontract.probationenddate
            new TranslationSeedItem("entity.employeecontract.probationenddate", "zh-CN", "试用期结束日期", "试用期结束日期"),
            // entity.employeecontract.probationenddate
            new TranslationSeedItem("entity.employeecontract.probationenddate", "zh-HK", "试用期结束日期_hk", "试用期结束日期"),

            // entity.employeecontract.signdate
            new TranslationSeedItem("entity.employeecontract.signdate", "en-US", "签订日期_us", "签订日期"),
            // entity.employeecontract.signdate
            new TranslationSeedItem("entity.employeecontract.signdate", "ja-JP", "签订日期_jp", "签订日期"),
            // entity.employeecontract.signdate
            new TranslationSeedItem("entity.employeecontract.signdate", "zh-CN", "签订日期", "签订日期"),
            // entity.employeecontract.signdate
            new TranslationSeedItem("entity.employeecontract.signdate", "zh-HK", "签订日期_hk", "签订日期"),

            // entity.employeecontract.signcompany
            new TranslationSeedItem("entity.employeecontract.signcompany", "en-US", "签约单位_us", "签约单位"),
            // entity.employeecontract.signcompany
            new TranslationSeedItem("entity.employeecontract.signcompany", "ja-JP", "签约单位_jp", "签约单位"),
            // entity.employeecontract.signcompany
            new TranslationSeedItem("entity.employeecontract.signcompany", "zh-CN", "签约单位", "签约单位"),
            // entity.employeecontract.signcompany
            new TranslationSeedItem("entity.employeecontract.signcompany", "zh-HK", "签约单位_hk", "签约单位"),

            // entity.employeecontract.contractstatus
            new TranslationSeedItem("entity.employeecontract.contractstatus", "en-US", "合同状态_us", "合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）"),
            // entity.employeecontract.contractstatus
            new TranslationSeedItem("entity.employeecontract.contractstatus", "ja-JP", "合同状态_jp", "合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）"),
            // entity.employeecontract.contractstatus
            new TranslationSeedItem("entity.employeecontract.contractstatus", "zh-CN", "合同状态", "合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）"),
            // entity.employeecontract.contractstatus
            new TranslationSeedItem("entity.employeecontract.contractstatus", "zh-HK", "合同状态_hk", "合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）"),

            // entity.employeecontract.employee
            new TranslationSeedItem("entity.employeecontract.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeecontract.employee
            new TranslationSeedItem("entity.employeecontract.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeecontract.employee
            new TranslationSeedItem("entity.employeecontract.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeecontract.employee
            new TranslationSeedItem("entity.employeecontract.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
