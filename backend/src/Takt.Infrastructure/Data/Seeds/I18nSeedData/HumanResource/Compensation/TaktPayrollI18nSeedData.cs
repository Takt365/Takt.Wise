// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktPayrollI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPayroll 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation;

/// <summary>
/// TaktPayroll 实体国际化翻译种子（键前缀 entity.payroll.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPayrollI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPayroll 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 payroll 实体翻译...", tenantCode);

        foreach (var item in GetPayrollTranslations())
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

        TaktLogger.Information("TaktPayroll 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPayroll 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.payroll._self / entity.payroll.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPayrollTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.payroll._self
            new TranslationSeedItem("entity.payroll._self", "en-US", "Payroll Information_us", "实体名称"),
            // entity.payroll._self
            new TranslationSeedItem("entity.payroll._self", "ja-JP", "薪酬体系信息_jp", "实体名称"),
            // entity.payroll._self
            new TranslationSeedItem("entity.payroll._self", "zh-CN", "薪酬体系信息", "实体名称"),
            // entity.payroll._self
            new TranslationSeedItem("entity.payroll._self", "zh-HK", "薪酬体系信息_hk", "实体名称"),

            // entity.payroll.code
            new TranslationSeedItem("entity.payroll.code", "en-US", "薪酬体系编码_us", "薪酬体系编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪酬体系编码规则生成并展示，非手输；单据类型菜单：薪酬体系）"),
            // entity.payroll.code
            new TranslationSeedItem("entity.payroll.code", "ja-JP", "薪酬体系编码_jp", "薪酬体系编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪酬体系编码规则生成并展示，非手输；单据类型菜单：薪酬体系）"),
            // entity.payroll.code
            new TranslationSeedItem("entity.payroll.code", "zh-CN", "薪酬体系编码", "薪酬体系编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪酬体系编码规则生成并展示，非手输；单据类型菜单：薪酬体系）"),
            // entity.payroll.code
            new TranslationSeedItem("entity.payroll.code", "zh-HK", "薪酬体系编码_hk", "薪酬体系编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪酬体系编码规则生成并展示，非手输；单据类型菜单：薪酬体系）"),

            // entity.payroll.name
            new TranslationSeedItem("entity.payroll.name", "en-US", "薪酬体系名称_us", "薪酬体系名称"),
            // entity.payroll.name
            new TranslationSeedItem("entity.payroll.name", "ja-JP", "薪酬体系名称_jp", "薪酬体系名称"),
            // entity.payroll.name
            new TranslationSeedItem("entity.payroll.name", "zh-CN", "薪酬体系名称", "薪酬体系名称"),
            // entity.payroll.name
            new TranslationSeedItem("entity.payroll.name", "zh-HK", "薪酬体系名称_hk", "薪酬体系名称"),

            // entity.payroll.payscaleid
            new TranslationSeedItem("entity.payroll.payscaleid", "en-US", "薪级表ID_us", "薪级表（选项 TaktPayScales/options；DictValue=Id）"),
            // entity.payroll.payscaleid
            new TranslationSeedItem("entity.payroll.payscaleid", "ja-JP", "薪级表ID_jp", "薪级表（选项 TaktPayScales/options；DictValue=Id）"),
            // entity.payroll.payscaleid
            new TranslationSeedItem("entity.payroll.payscaleid", "zh-CN", "薪级表ID", "薪级表（选项 TaktPayScales/options；DictValue=Id）"),
            // entity.payroll.payscaleid
            new TranslationSeedItem("entity.payroll.payscaleid", "zh-HK", "薪级表ID_hk", "薪级表（选项 TaktPayScales/options；DictValue=Id）"),

            // entity.payroll.formulasetcode
            new TranslationSeedItem("entity.payroll.formulasetcode", "en-US", "公式方案编码_us", "公式方案编码（关联 TaktSalaryFormula.SetCode，同编码多行步骤；整单工资核算时使用）"),
            // entity.payroll.formulasetcode
            new TranslationSeedItem("entity.payroll.formulasetcode", "ja-JP", "公式方案编码_jp", "公式方案编码（关联 TaktSalaryFormula.SetCode，同编码多行步骤；整单工资核算时使用）"),
            // entity.payroll.formulasetcode
            new TranslationSeedItem("entity.payroll.formulasetcode", "zh-CN", "公式方案编码", "公式方案编码（关联 TaktSalaryFormula.SetCode，同编码多行步骤；整单工资核算时使用）"),
            // entity.payroll.formulasetcode
            new TranslationSeedItem("entity.payroll.formulasetcode", "zh-HK", "公式方案编码_hk", "公式方案编码（关联 TaktSalaryFormula.SetCode，同编码多行步骤；整单工资核算时使用）"),

            // entity.payroll.effectivedate
            new TranslationSeedItem("entity.payroll.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.payroll.effectivedate
            new TranslationSeedItem("entity.payroll.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.payroll.effectivedate
            new TranslationSeedItem("entity.payroll.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.payroll.effectivedate
            new TranslationSeedItem("entity.payroll.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.payroll.expirydate
            new TranslationSeedItem("entity.payroll.expirydate", "en-US", "失效日期_us", "失效日期"),
            // entity.payroll.expirydate
            new TranslationSeedItem("entity.payroll.expirydate", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.payroll.expirydate
            new TranslationSeedItem("entity.payroll.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.payroll.expirydate
            new TranslationSeedItem("entity.payroll.expirydate", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.payroll.description
            new TranslationSeedItem("entity.payroll.description", "en-US", "说明_us", "说明"),
            // entity.payroll.description
            new TranslationSeedItem("entity.payroll.description", "ja-JP", "说明_jp", "说明"),
            // entity.payroll.description
            new TranslationSeedItem("entity.payroll.description", "zh-CN", "说明", "说明"),
            // entity.payroll.description
            new TranslationSeedItem("entity.payroll.description", "zh-HK", "说明_hk", "说明"),

            // entity.payroll.status
            new TranslationSeedItem("entity.payroll.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payroll.status
            new TranslationSeedItem("entity.payroll.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payroll.status
            new TranslationSeedItem("entity.payroll.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.payroll.status
            new TranslationSeedItem("entity.payroll.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
        translation.ResourceGroup = "Compensation";
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
