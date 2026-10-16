// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktSalaryFormulaI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalaryFormula 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalaryFormula 实体国际化翻译种子（键前缀 entity.salaryformula.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalaryFormulaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalaryFormula 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salaryformula 实体翻译...", tenantCode);

        foreach (var item in GetSalaryFormulaTranslations())
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

        TaktLogger.Information("TaktSalaryFormula 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalaryFormula 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salaryformula._self / entity.salaryformula.{{field}}；ResourceGroup=Compensation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalaryFormulaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salaryformula._self
            new TranslationSeedItem("entity.salaryformula._self", "en-US", "Salary Formula Information_us", "实体名称"),
            // entity.salaryformula._self
            new TranslationSeedItem("entity.salaryformula._self", "ja-JP", "薪资计算公式信息_jp", "实体名称"),
            // entity.salaryformula._self
            new TranslationSeedItem("entity.salaryformula._self", "zh-CN", "薪资计算公式信息", "实体名称"),
            // entity.salaryformula._self
            new TranslationSeedItem("entity.salaryformula._self", "zh-HK", "薪资计算公式信息_hk", "实体名称"),

            // entity.salaryformula.setcode
            new TranslationSeedItem("entity.salaryformula.setcode", "en-US", "公式方案编码_us", "公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识；前端表单选择编码规则后自动通过 TaktNumbering 公式方案编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.setcode
            new TranslationSeedItem("entity.salaryformula.setcode", "ja-JP", "公式方案编码_jp", "公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识；前端表单选择编码规则后自动通过 TaktNumbering 公式方案编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.setcode
            new TranslationSeedItem("entity.salaryformula.setcode", "zh-CN", "公式方案编码", "公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识；前端表单选择编码规则后自动通过 TaktNumbering 公式方案编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.setcode
            new TranslationSeedItem("entity.salaryformula.setcode", "zh-HK", "公式方案编码_hk", "公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识；前端表单选择编码规则后自动通过 TaktNumbering 公式方案编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),

            // entity.salaryformula.setname
            new TranslationSeedItem("entity.salaryformula.setname", "en-US", "公式方案名称_us", "公式方案名称"),
            // entity.salaryformula.setname
            new TranslationSeedItem("entity.salaryformula.setname", "ja-JP", "公式方案名称_jp", "公式方案名称"),
            // entity.salaryformula.setname
            new TranslationSeedItem("entity.salaryformula.setname", "zh-CN", "公式方案名称", "公式方案名称"),
            // entity.salaryformula.setname
            new TranslationSeedItem("entity.salaryformula.setname", "zh-HK", "公式方案名称_hk", "公式方案名称"),

            // entity.salaryformula.payrollid
            new TranslationSeedItem("entity.salaryformula.payrollid", "en-US", "薪酬体系ID_us", "薪酬体系（选项 TaktPayrolls/options；同 set_code 各行取值应一致，DictValue=Id）"),
            // entity.salaryformula.payrollid
            new TranslationSeedItem("entity.salaryformula.payrollid", "ja-JP", "薪酬体系ID_jp", "薪酬体系（选项 TaktPayrolls/options；同 set_code 各行取值应一致，DictValue=Id）"),
            // entity.salaryformula.payrollid
            new TranslationSeedItem("entity.salaryformula.payrollid", "zh-CN", "薪酬体系ID", "薪酬体系（选项 TaktPayrolls/options；同 set_code 各行取值应一致，DictValue=Id）"),
            // entity.salaryformula.payrollid
            new TranslationSeedItem("entity.salaryformula.payrollid", "zh-HK", "薪酬体系ID_hk", "薪酬体系（选项 TaktPayrolls/options；同 set_code 各行取值应一致，DictValue=Id）"),

            // entity.salaryformula.formulacode
            new TranslationSeedItem("entity.salaryformula.formulacode", "en-US", "步骤编码_us", "步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET；前端表单选择编码规则后自动通过 TaktNumbering 步骤编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.formulacode
            new TranslationSeedItem("entity.salaryformula.formulacode", "ja-JP", "步骤编码_jp", "步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET；前端表单选择编码规则后自动通过 TaktNumbering 步骤编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.formulacode
            new TranslationSeedItem("entity.salaryformula.formulacode", "zh-CN", "步骤编码", "步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET；前端表单选择编码规则后自动通过 TaktNumbering 步骤编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),
            // entity.salaryformula.formulacode
            new TranslationSeedItem("entity.salaryformula.formulacode", "zh-HK", "步骤编码_hk", "步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET；前端表单选择编码规则后自动通过 TaktNumbering 步骤编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）"),

            // entity.salaryformula.formulaname
            new TranslationSeedItem("entity.salaryformula.formulaname", "en-US", "步骤名称_us", "步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）"),
            // entity.salaryformula.formulaname
            new TranslationSeedItem("entity.salaryformula.formulaname", "ja-JP", "步骤名称_jp", "步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）"),
            // entity.salaryformula.formulaname
            new TranslationSeedItem("entity.salaryformula.formulaname", "zh-CN", "步骤名称", "步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）"),
            // entity.salaryformula.formulaname
            new TranslationSeedItem("entity.salaryformula.formulaname", "zh-HK", "步骤名称_hk", "步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）"),

            // entity.salaryformula.formulastep
            new TranslationSeedItem("entity.salaryformula.formulastep", "en-US", "公式步骤_us", "公式步骤类型（字典 humanresource_compensation_salary_formula_step_type；1=应发 2=社保个人 3=公积金个人 4=个税 5=实发）"),
            // entity.salaryformula.formulastep
            new TranslationSeedItem("entity.salaryformula.formulastep", "ja-JP", "公式步骤_jp", "公式步骤类型（字典 humanresource_compensation_salary_formula_step_type；1=应发 2=社保个人 3=公积金个人 4=个税 5=实发）"),
            // entity.salaryformula.formulastep
            new TranslationSeedItem("entity.salaryformula.formulastep", "zh-CN", "公式步骤", "公式步骤类型（字典 humanresource_compensation_salary_formula_step_type；1=应发 2=社保个人 3=公积金个人 4=个税 5=实发）"),
            // entity.salaryformula.formulastep
            new TranslationSeedItem("entity.salaryformula.formulastep", "zh-HK", "公式步骤_hk", "公式步骤类型（字典 humanresource_compensation_salary_formula_step_type；1=应发 2=社保个人 3=公积金个人 4=个税 5=实发）"),

            // entity.salaryformula.targetfield
            new TranslationSeedItem("entity.salaryformula.targetfield", "en-US", "结果字段_us", "结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）"),
            // entity.salaryformula.targetfield
            new TranslationSeedItem("entity.salaryformula.targetfield", "ja-JP", "结果字段_jp", "结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）"),
            // entity.salaryformula.targetfield
            new TranslationSeedItem("entity.salaryformula.targetfield", "zh-CN", "结果字段", "结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）"),
            // entity.salaryformula.targetfield
            new TranslationSeedItem("entity.salaryformula.targetfield", "zh-HK", "结果字段_hk", "结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）"),

            // entity.salaryformula.formulaexpression
            new TranslationSeedItem("entity.salaryformula.formulaexpression", "en-US", "计算公式_us", "计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）"),
            // entity.salaryformula.formulaexpression
            new TranslationSeedItem("entity.salaryformula.formulaexpression", "ja-JP", "计算公式_jp", "计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）"),
            // entity.salaryformula.formulaexpression
            new TranslationSeedItem("entity.salaryformula.formulaexpression", "zh-CN", "计算公式", "计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）"),
            // entity.salaryformula.formulaexpression
            new TranslationSeedItem("entity.salaryformula.formulaexpression", "zh-HK", "计算公式_hk", "计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）"),

            // entity.salaryformula.stepdescription
            new TranslationSeedItem("entity.salaryformula.stepdescription", "en-US", "步骤说明_us", "步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）"),
            // entity.salaryformula.stepdescription
            new TranslationSeedItem("entity.salaryformula.stepdescription", "ja-JP", "步骤说明_jp", "步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）"),
            // entity.salaryformula.stepdescription
            new TranslationSeedItem("entity.salaryformula.stepdescription", "zh-CN", "步骤说明", "步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）"),
            // entity.salaryformula.stepdescription
            new TranslationSeedItem("entity.salaryformula.stepdescription", "zh-HK", "步骤说明_hk", "步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）"),

            // entity.salaryformula.effectivedate
            new TranslationSeedItem("entity.salaryformula.effectivedate", "en-US", "生效日期_us", "方案生效日期（同 set_code 各行应一致）"),
            // entity.salaryformula.effectivedate
            new TranslationSeedItem("entity.salaryformula.effectivedate", "ja-JP", "生效日期_jp", "方案生效日期（同 set_code 各行应一致）"),
            // entity.salaryformula.effectivedate
            new TranslationSeedItem("entity.salaryformula.effectivedate", "zh-CN", "生效日期", "方案生效日期（同 set_code 各行应一致）"),
            // entity.salaryformula.effectivedate
            new TranslationSeedItem("entity.salaryformula.effectivedate", "zh-HK", "生效日期_hk", "方案生效日期（同 set_code 各行应一致）"),

            // entity.salaryformula.expirydate
            new TranslationSeedItem("entity.salaryformula.expirydate", "en-US", "失效日期_us", "方案失效日期"),
            // entity.salaryformula.expirydate
            new TranslationSeedItem("entity.salaryformula.expirydate", "ja-JP", "失效日期_jp", "方案失效日期"),
            // entity.salaryformula.expirydate
            new TranslationSeedItem("entity.salaryformula.expirydate", "zh-CN", "失效日期", "方案失效日期"),
            // entity.salaryformula.expirydate
            new TranslationSeedItem("entity.salaryformula.expirydate", "zh-HK", "失效日期_hk", "方案失效日期"),

            // entity.salaryformula.sortorder
            new TranslationSeedItem("entity.salaryformula.sortorder", "en-US", "执行顺序_us", "执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）"),
            // entity.salaryformula.sortorder
            new TranslationSeedItem("entity.salaryformula.sortorder", "ja-JP", "执行顺序_jp", "执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）"),
            // entity.salaryformula.sortorder
            new TranslationSeedItem("entity.salaryformula.sortorder", "zh-CN", "执行顺序", "执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）"),
            // entity.salaryformula.sortorder
            new TranslationSeedItem("entity.salaryformula.sortorder", "zh-HK", "执行顺序_hk", "执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）"),

            // entity.salaryformula.formulastatus
            new TranslationSeedItem("entity.salaryformula.formulastatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryformula.formulastatus
            new TranslationSeedItem("entity.salaryformula.formulastatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryformula.formulastatus
            new TranslationSeedItem("entity.salaryformula.formulastatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
            // entity.salaryformula.formulastatus
            new TranslationSeedItem("entity.salaryformula.formulastatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）"),
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
