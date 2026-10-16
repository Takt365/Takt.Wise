// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeSkillI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeSkill 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeSkill 实体国际化翻译种子（键前缀 entity.employeeskill.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeSkillI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeSkill 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeskill 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeSkillTranslations())
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

        TaktLogger.Information("TaktEmployeeSkill 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeSkill 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeskill._self / entity.employeeskill.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeSkillTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeskill._self
            new TranslationSeedItem("entity.employeeskill._self", "en-US", "Employee Skill Information_us", "实体名称"),
            // entity.employeeskill._self
            new TranslationSeedItem("entity.employeeskill._self", "ja-JP", "员工技能与证书信息_jp", "实体名称"),
            // entity.employeeskill._self
            new TranslationSeedItem("entity.employeeskill._self", "zh-CN", "员工技能与证书信息", "实体名称"),
            // entity.employeeskill._self
            new TranslationSeedItem("entity.employeeskill._self", "zh-HK", "员工技能与证书信息_hk", "实体名称"),

            // entity.employeeskill.employeeid
            new TranslationSeedItem("entity.employeeskill.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeskill.employeeid
            new TranslationSeedItem("entity.employeeskill.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeskill.employeeid
            new TranslationSeedItem("entity.employeeskill.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeskill.employeeid
            new TranslationSeedItem("entity.employeeskill.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeskill.employeecode
            new TranslationSeedItem("entity.employeeskill.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeskill.employeecode
            new TranslationSeedItem("entity.employeeskill.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeskill.employeecode
            new TranslationSeedItem("entity.employeeskill.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeskill.employeecode
            new TranslationSeedItem("entity.employeeskill.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeskill.employeename
            new TranslationSeedItem("entity.employeeskill.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeskill.employeename
            new TranslationSeedItem("entity.employeeskill.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeskill.employeename
            new TranslationSeedItem("entity.employeeskill.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeskill.employeename
            new TranslationSeedItem("entity.employeeskill.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeskill.skillname
            new TranslationSeedItem("entity.employeeskill.skillname", "en-US", "技能名称_us", "技能名称"),
            // entity.employeeskill.skillname
            new TranslationSeedItem("entity.employeeskill.skillname", "ja-JP", "技能名称_jp", "技能名称"),
            // entity.employeeskill.skillname
            new TranslationSeedItem("entity.employeeskill.skillname", "zh-CN", "技能名称", "技能名称"),
            // entity.employeeskill.skillname
            new TranslationSeedItem("entity.employeeskill.skillname", "zh-HK", "技能名称_hk", "技能名称"),

            // entity.employeeskill.skilllevel
            new TranslationSeedItem("entity.employeeskill.skilllevel", "en-US", "技能等级_us", "技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）"),
            // entity.employeeskill.skilllevel
            new TranslationSeedItem("entity.employeeskill.skilllevel", "ja-JP", "技能等级_jp", "技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）"),
            // entity.employeeskill.skilllevel
            new TranslationSeedItem("entity.employeeskill.skilllevel", "zh-CN", "技能等级", "技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）"),
            // entity.employeeskill.skilllevel
            new TranslationSeedItem("entity.employeeskill.skilllevel", "zh-HK", "技能等级_hk", "技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）"),

            // entity.employeeskill.certificatename
            new TranslationSeedItem("entity.employeeskill.certificatename", "en-US", "证书名称_us", "证书名称"),
            // entity.employeeskill.certificatename
            new TranslationSeedItem("entity.employeeskill.certificatename", "ja-JP", "证书名称_jp", "证书名称"),
            // entity.employeeskill.certificatename
            new TranslationSeedItem("entity.employeeskill.certificatename", "zh-CN", "证书名称", "证书名称"),
            // entity.employeeskill.certificatename
            new TranslationSeedItem("entity.employeeskill.certificatename", "zh-HK", "证书名称_hk", "证书名称"),

            // entity.employeeskill.certificatecode
            new TranslationSeedItem("entity.employeeskill.certificatecode", "en-US", "证书编码_us", "证书编码"),
            // entity.employeeskill.certificatecode
            new TranslationSeedItem("entity.employeeskill.certificatecode", "ja-JP", "证书编码_jp", "证书编码"),
            // entity.employeeskill.certificatecode
            new TranslationSeedItem("entity.employeeskill.certificatecode", "zh-CN", "证书编码", "证书编码"),
            // entity.employeeskill.certificatecode
            new TranslationSeedItem("entity.employeeskill.certificatecode", "zh-HK", "证书编码_hk", "证书编码"),

            // entity.employeeskill.obtaineddate
            new TranslationSeedItem("entity.employeeskill.obtaineddate", "en-US", "取得日期_us", "取得日期"),
            // entity.employeeskill.obtaineddate
            new TranslationSeedItem("entity.employeeskill.obtaineddate", "ja-JP", "取得日期_jp", "取得日期"),
            // entity.employeeskill.obtaineddate
            new TranslationSeedItem("entity.employeeskill.obtaineddate", "zh-CN", "取得日期", "取得日期"),
            // entity.employeeskill.obtaineddate
            new TranslationSeedItem("entity.employeeskill.obtaineddate", "zh-HK", "取得日期_hk", "取得日期"),

            // entity.employeeskill.expirydate
            new TranslationSeedItem("entity.employeeskill.expirydate", "en-US", "到期日期_us", "到期日期"),
            // entity.employeeskill.expirydate
            new TranslationSeedItem("entity.employeeskill.expirydate", "ja-JP", "到期日期_jp", "到期日期"),
            // entity.employeeskill.expirydate
            new TranslationSeedItem("entity.employeeskill.expirydate", "zh-CN", "到期日期", "到期日期"),
            // entity.employeeskill.expirydate
            new TranslationSeedItem("entity.employeeskill.expirydate", "zh-HK", "到期日期_hk", "到期日期"),

            // entity.employeeskill.employee
            new TranslationSeedItem("entity.employeeskill.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeskill.employee
            new TranslationSeedItem("entity.employeeskill.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeskill.employee
            new TranslationSeedItem("entity.employeeskill.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeskill.employee
            new TranslationSeedItem("entity.employeeskill.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
