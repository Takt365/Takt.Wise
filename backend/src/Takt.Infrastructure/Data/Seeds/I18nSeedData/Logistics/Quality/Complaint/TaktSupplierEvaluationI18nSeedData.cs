// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplierEvaluation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktSupplierEvaluation 实体国际化翻译种子（键前缀 entity.supplierevaluation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierEvaluationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplierEvaluation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplierevaluation 实体翻译...", tenantCode);

        foreach (var item in GetSupplierEvaluationTranslations())
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

        TaktLogger.Information("TaktSupplierEvaluation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplierEvaluation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplierevaluation._self / entity.supplierevaluation.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierEvaluationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplierevaluation._self
            new TranslationSeedItem("entity.supplierevaluation._self", "en-US", "Supplier Evaluation Information_us", "实体名称"),
            // entity.supplierevaluation._self
            new TranslationSeedItem("entity.supplierevaluation._self", "ja-JP", "供应商评价考核主表信息_jp", "实体名称"),
            // entity.supplierevaluation._self
            new TranslationSeedItem("entity.supplierevaluation._self", "zh-CN", "供应商评价考核主表信息", "实体名称"),
            // entity.supplierevaluation._self
            new TranslationSeedItem("entity.supplierevaluation._self", "zh-HK", "供应商评价考核主表信息_hk", "实体名称"),

            // entity.supplierevaluation.code
            new TranslationSeedItem("entity.supplierevaluation.code", "en-US", "评价表编码_us", "评价表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 评价表编码规则生成并展示，非手输；单据类型菜单：供应商评价考核）"),
            // entity.supplierevaluation.code
            new TranslationSeedItem("entity.supplierevaluation.code", "ja-JP", "评价表编码_jp", "评价表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 评价表编码规则生成并展示，非手输；单据类型菜单：供应商评价考核）"),
            // entity.supplierevaluation.code
            new TranslationSeedItem("entity.supplierevaluation.code", "zh-CN", "评价表编码", "评价表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 评价表编码规则生成并展示，非手输；单据类型菜单：供应商评价考核）"),
            // entity.supplierevaluation.code
            new TranslationSeedItem("entity.supplierevaluation.code", "zh-HK", "评价表编码_hk", "评价表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 评价表编码规则生成并展示，非手输；单据类型菜单：供应商评价考核）"),

            // entity.supplierevaluation.supplierid
            new TranslationSeedItem("entity.supplierevaluation.supplierid", "en-US", "供应商ID_us", "供应商 ID（选项 TaktSuppliers/options；DictValue=Id）"),
            // entity.supplierevaluation.supplierid
            new TranslationSeedItem("entity.supplierevaluation.supplierid", "ja-JP", "供应商ID_jp", "供应商 ID（选项 TaktSuppliers/options；DictValue=Id）"),
            // entity.supplierevaluation.supplierid
            new TranslationSeedItem("entity.supplierevaluation.supplierid", "zh-CN", "供应商ID", "供应商 ID（选项 TaktSuppliers/options；DictValue=Id）"),
            // entity.supplierevaluation.supplierid
            new TranslationSeedItem("entity.supplierevaluation.supplierid", "zh-HK", "供应商ID_hk", "供应商 ID（选项 TaktSuppliers/options；DictValue=Id）"),

            // entity.supplierevaluation.suppliername1
            new TranslationSeedItem("entity.supplierevaluation.suppliername1", "en-US", "供应商名称1_us", "供应商名称"),
            // entity.supplierevaluation.suppliername1
            new TranslationSeedItem("entity.supplierevaluation.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称"),
            // entity.supplierevaluation.suppliername1
            new TranslationSeedItem("entity.supplierevaluation.suppliername1", "zh-CN", "供应商名称1", "供应商名称"),
            // entity.supplierevaluation.suppliername1
            new TranslationSeedItem("entity.supplierevaluation.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称"),

            // entity.supplierevaluation.suppliercode
            new TranslationSeedItem("entity.supplierevaluation.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.supplierevaluation.suppliercode
            new TranslationSeedItem("entity.supplierevaluation.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.supplierevaluation.suppliercode
            new TranslationSeedItem("entity.supplierevaluation.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.supplierevaluation.suppliercode
            new TranslationSeedItem("entity.supplierevaluation.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.supplierevaluation.evaluationdate
            new TranslationSeedItem("entity.supplierevaluation.evaluationdate", "en-US", "评价日期_us", "评价日期"),
            // entity.supplierevaluation.evaluationdate
            new TranslationSeedItem("entity.supplierevaluation.evaluationdate", "ja-JP", "评价日期_jp", "评价日期"),
            // entity.supplierevaluation.evaluationdate
            new TranslationSeedItem("entity.supplierevaluation.evaluationdate", "zh-CN", "评价日期", "评价日期"),
            // entity.supplierevaluation.evaluationdate
            new TranslationSeedItem("entity.supplierevaluation.evaluationdate", "zh-HK", "评价日期_hk", "评价日期"),

            // entity.supplierevaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierevaluation.evaluationperiod", "en-US", "评价周期_us", "评价周期（字典 logistics_quality_period）"),
            // entity.supplierevaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierevaluation.evaluationperiod", "ja-JP", "评价周期_jp", "评价周期（字典 logistics_quality_period）"),
            // entity.supplierevaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierevaluation.evaluationperiod", "zh-CN", "评价周期", "评价周期（字典 logistics_quality_period）"),
            // entity.supplierevaluation.evaluationperiod
            new TranslationSeedItem("entity.supplierevaluation.evaluationperiod", "zh-HK", "评价周期_hk", "评价周期（字典 logistics_quality_period）"),

            // entity.supplierevaluation.evaluationtype
            new TranslationSeedItem("entity.supplierevaluation.evaluationtype", "en-US", "评价类型_us", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierevaluation.evaluationtype
            new TranslationSeedItem("entity.supplierevaluation.evaluationtype", "ja-JP", "评价类型_jp", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierevaluation.evaluationtype
            new TranslationSeedItem("entity.supplierevaluation.evaluationtype", "zh-CN", "评价类型", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),
            // entity.supplierevaluation.evaluationtype
            new TranslationSeedItem("entity.supplierevaluation.evaluationtype", "zh-HK", "评价类型_hk", "评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）"),

            // entity.supplierevaluation.evaluatorbygroupcode
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupcode", "en-US", "评价组编码_us", "评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）"),
            // entity.supplierevaluation.evaluatorbygroupcode
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupcode", "ja-JP", "评价组编码_jp", "评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）"),
            // entity.supplierevaluation.evaluatorbygroupcode
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupcode", "zh-CN", "评价组编码", "评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）"),
            // entity.supplierevaluation.evaluatorbygroupcode
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupcode", "zh-HK", "评价组编码_hk", "评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）"),

            // entity.supplierevaluation.evaluatorbygroupname
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupname", "en-US", "评价组名称_us", "评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.supplierevaluation.evaluatorbygroupname
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupname", "ja-JP", "评价组名称_jp", "评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.supplierevaluation.evaluatorbygroupname
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupname", "zh-CN", "评价组名称", "评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.supplierevaluation.evaluatorbygroupname
            new TranslationSeedItem("entity.supplierevaluation.evaluatorbygroupname", "zh-HK", "评价组名称_hk", "评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),

            // entity.supplierevaluation.evaluationdeptid
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptid", "en-US", "评价部门ID_us", "评价部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.supplierevaluation.evaluationdeptid
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptid", "ja-JP", "评价部门ID_jp", "评价部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.supplierevaluation.evaluationdeptid
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptid", "zh-CN", "评价部门ID", "评价部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.supplierevaluation.evaluationdeptid
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptid", "zh-HK", "评价部门ID_hk", "评价部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.supplierevaluation.evaluationdeptname
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptname", "en-US", "评价部门名称_us", "评价部门名称（冗余：按 EvaluationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.supplierevaluation.evaluationdeptname
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptname", "ja-JP", "评价部门名称_jp", "评价部门名称（冗余：按 EvaluationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.supplierevaluation.evaluationdeptname
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptname", "zh-CN", "评价部门名称", "评价部门名称（冗余：按 EvaluationDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.supplierevaluation.evaluationdeptname
            new TranslationSeedItem("entity.supplierevaluation.evaluationdeptname", "zh-HK", "评价部门名称_hk", "评价部门名称（冗余：按 EvaluationDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.supplierevaluation.overallrating
            new TranslationSeedItem("entity.supplierevaluation.overallrating", "en-US", "总体评级_us", "总体评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluation.overallrating
            new TranslationSeedItem("entity.supplierevaluation.overallrating", "ja-JP", "总体评级_jp", "总体评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluation.overallrating
            new TranslationSeedItem("entity.supplierevaluation.overallrating", "zh-CN", "总体评级", "总体评级（字典 logistics_quality_supplier_rating）"),
            // entity.supplierevaluation.overallrating
            new TranslationSeedItem("entity.supplierevaluation.overallrating", "zh-HK", "总体评级_hk", "总体评级（字典 logistics_quality_supplier_rating）"),

            // entity.supplierevaluation.totalscore
            new TranslationSeedItem("entity.supplierevaluation.totalscore", "en-US", "综合评分_us", "综合评分（0-100分）"),
            // entity.supplierevaluation.totalscore
            new TranslationSeedItem("entity.supplierevaluation.totalscore", "ja-JP", "综合评分_jp", "综合评分（0-100分）"),
            // entity.supplierevaluation.totalscore
            new TranslationSeedItem("entity.supplierevaluation.totalscore", "zh-CN", "综合评分", "综合评分（0-100分）"),
            // entity.supplierevaluation.totalscore
            new TranslationSeedItem("entity.supplierevaluation.totalscore", "zh-HK", "综合评分_hk", "综合评分（0-100分）"),

            // entity.supplierevaluation.qualityscore
            new TranslationSeedItem("entity.supplierevaluation.qualityscore", "en-US", "质量评分_us", "质量评分（0-100分）"),
            // entity.supplierevaluation.qualityscore
            new TranslationSeedItem("entity.supplierevaluation.qualityscore", "ja-JP", "质量评分_jp", "质量评分（0-100分）"),
            // entity.supplierevaluation.qualityscore
            new TranslationSeedItem("entity.supplierevaluation.qualityscore", "zh-CN", "质量评分", "质量评分（0-100分）"),
            // entity.supplierevaluation.qualityscore
            new TranslationSeedItem("entity.supplierevaluation.qualityscore", "zh-HK", "质量评分_hk", "质量评分（0-100分）"),

            // entity.supplierevaluation.deliveryscore
            new TranslationSeedItem("entity.supplierevaluation.deliveryscore", "en-US", "交付评分_us", "交付评分（0-100分）"),
            // entity.supplierevaluation.deliveryscore
            new TranslationSeedItem("entity.supplierevaluation.deliveryscore", "ja-JP", "交付评分_jp", "交付评分（0-100分）"),
            // entity.supplierevaluation.deliveryscore
            new TranslationSeedItem("entity.supplierevaluation.deliveryscore", "zh-CN", "交付评分", "交付评分（0-100分）"),
            // entity.supplierevaluation.deliveryscore
            new TranslationSeedItem("entity.supplierevaluation.deliveryscore", "zh-HK", "交付评分_hk", "交付评分（0-100分）"),

            // entity.supplierevaluation.pricescore
            new TranslationSeedItem("entity.supplierevaluation.pricescore", "en-US", "价格评分_us", "价格评分（0-100分）"),
            // entity.supplierevaluation.pricescore
            new TranslationSeedItem("entity.supplierevaluation.pricescore", "ja-JP", "价格评分_jp", "价格评分（0-100分）"),
            // entity.supplierevaluation.pricescore
            new TranslationSeedItem("entity.supplierevaluation.pricescore", "zh-CN", "价格评分", "价格评分（0-100分）"),
            // entity.supplierevaluation.pricescore
            new TranslationSeedItem("entity.supplierevaluation.pricescore", "zh-HK", "价格评分_hk", "价格评分（0-100分）"),

            // entity.supplierevaluation.servicescore
            new TranslationSeedItem("entity.supplierevaluation.servicescore", "en-US", "服务评分_us", "服务评分（0-100分）"),
            // entity.supplierevaluation.servicescore
            new TranslationSeedItem("entity.supplierevaluation.servicescore", "ja-JP", "服务评分_jp", "服务评分（0-100分）"),
            // entity.supplierevaluation.servicescore
            new TranslationSeedItem("entity.supplierevaluation.servicescore", "zh-CN", "服务评分", "服务评分（0-100分）"),
            // entity.supplierevaluation.servicescore
            new TranslationSeedItem("entity.supplierevaluation.servicescore", "zh-HK", "服务评分_hk", "服务评分（0-100分）"),

            // entity.supplierevaluation.technicalscore
            new TranslationSeedItem("entity.supplierevaluation.technicalscore", "en-US", "技术能力评分_us", "技术能力评分（0-100分）"),
            // entity.supplierevaluation.technicalscore
            new TranslationSeedItem("entity.supplierevaluation.technicalscore", "ja-JP", "技术能力评分_jp", "技术能力评分（0-100分）"),
            // entity.supplierevaluation.technicalscore
            new TranslationSeedItem("entity.supplierevaluation.technicalscore", "zh-CN", "技术能力评分", "技术能力评分（0-100分）"),
            // entity.supplierevaluation.technicalscore
            new TranslationSeedItem("entity.supplierevaluation.technicalscore", "zh-HK", "技术能力评分_hk", "技术能力评分（0-100分）"),

            // entity.supplierevaluation.mainstrengths
            new TranslationSeedItem("entity.supplierevaluation.mainstrengths", "en-US", "主要优点_us", "主要优点"),
            // entity.supplierevaluation.mainstrengths
            new TranslationSeedItem("entity.supplierevaluation.mainstrengths", "ja-JP", "主要优点_jp", "主要优点"),
            // entity.supplierevaluation.mainstrengths
            new TranslationSeedItem("entity.supplierevaluation.mainstrengths", "zh-CN", "主要优点", "主要优点"),
            // entity.supplierevaluation.mainstrengths
            new TranslationSeedItem("entity.supplierevaluation.mainstrengths", "zh-HK", "主要优点_hk", "主要优点"),

            // entity.supplierevaluation.mainissues
            new TranslationSeedItem("entity.supplierevaluation.mainissues", "en-US", "主要问题_us", "主要问题/不足"),
            // entity.supplierevaluation.mainissues
            new TranslationSeedItem("entity.supplierevaluation.mainissues", "ja-JP", "主要问题_jp", "主要问题/不足"),
            // entity.supplierevaluation.mainissues
            new TranslationSeedItem("entity.supplierevaluation.mainissues", "zh-CN", "主要问题", "主要问题/不足"),
            // entity.supplierevaluation.mainissues
            new TranslationSeedItem("entity.supplierevaluation.mainissues", "zh-HK", "主要问题_hk", "主要问题/不足"),

            // entity.supplierevaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierevaluation.improvementrequirements", "en-US", "改进要求_us", "改进要求/建议"),
            // entity.supplierevaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierevaluation.improvementrequirements", "ja-JP", "改进要求_jp", "改进要求/建议"),
            // entity.supplierevaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierevaluation.improvementrequirements", "zh-CN", "改进要求", "改进要求/建议"),
            // entity.supplierevaluation.improvementrequirements
            new TranslationSeedItem("entity.supplierevaluation.improvementrequirements", "zh-HK", "改进要求_hk", "改进要求/建议"),

            // entity.supplierevaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierevaluation.evaluationconclusion", "en-US", "考核结论_us", "考核结论（字典 logistics_quality_evaluation_conclusion）"),
            // entity.supplierevaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierevaluation.evaluationconclusion", "ja-JP", "考核结论_jp", "考核结论（字典 logistics_quality_evaluation_conclusion）"),
            // entity.supplierevaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierevaluation.evaluationconclusion", "zh-CN", "考核结论", "考核结论（字典 logistics_quality_evaluation_conclusion）"),
            // entity.supplierevaluation.evaluationconclusion
            new TranslationSeedItem("entity.supplierevaluation.evaluationconclusion", "zh-HK", "考核结论_hk", "考核结论（字典 logistics_quality_evaluation_conclusion）"),

            // entity.supplierevaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluation.rectificationdeadline", "en-US", "整改期限_us", "整改期限（要求完成日期）"),
            // entity.supplierevaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluation.rectificationdeadline", "ja-JP", "整改期限_jp", "整改期限（要求完成日期）"),
            // entity.supplierevaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluation.rectificationdeadline", "zh-CN", "整改期限", "整改期限（要求完成日期）"),
            // entity.supplierevaluation.rectificationdeadline
            new TranslationSeedItem("entity.supplierevaluation.rectificationdeadline", "zh-HK", "整改期限_hk", "整改期限（要求完成日期）"),

            // entity.supplierevaluation.attachments
            new TranslationSeedItem("entity.supplierevaluation.attachments", "en-US", "附件JSON_us", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.supplierevaluation.attachments
            new TranslationSeedItem("entity.supplierevaluation.attachments", "ja-JP", "附件JSON_jp", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.supplierevaluation.attachments
            new TranslationSeedItem("entity.supplierevaluation.attachments", "zh-CN", "附件JSON", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.supplierevaluation.attachments
            new TranslationSeedItem("entity.supplierevaluation.attachments", "zh-HK", "附件JSON_hk", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),

            // entity.supplierevaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierevaluation.evaluationstatus", "en-US", "评价状态_us", "评价状态（字典 logistics_quality_evaluation_status）"),
            // entity.supplierevaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierevaluation.evaluationstatus", "ja-JP", "评价状态_jp", "评价状态（字典 logistics_quality_evaluation_status）"),
            // entity.supplierevaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierevaluation.evaluationstatus", "zh-CN", "评价状态", "评价状态（字典 logistics_quality_evaluation_status）"),
            // entity.supplierevaluation.evaluationstatus
            new TranslationSeedItem("entity.supplierevaluation.evaluationstatus", "zh-HK", "评价状态_hk", "评价状态（字典 logistics_quality_evaluation_status）"),

            // entity.supplierevaluation.sortorder
            new TranslationSeedItem("entity.supplierevaluation.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.supplierevaluation.sortorder
            new TranslationSeedItem("entity.supplierevaluation.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.supplierevaluation.sortorder
            new TranslationSeedItem("entity.supplierevaluation.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.supplierevaluation.sortorder
            new TranslationSeedItem("entity.supplierevaluation.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.supplierevaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluation.rectificationstatus", "en-US", "整改跟进状态_us", "整改跟进状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluation.rectificationstatus", "ja-JP", "整改跟进状态_jp", "整改跟进状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluation.rectificationstatus", "zh-CN", "整改跟进状态", "整改跟进状态（字典 logistics_quality_rectification_status）"),
            // entity.supplierevaluation.rectificationstatus
            new TranslationSeedItem("entity.supplierevaluation.rectificationstatus", "zh-HK", "整改跟进状态_hk", "整改跟进状态（字典 logistics_quality_rectification_status）"),

            // entity.supplierevaluation.items
            new TranslationSeedItem("entity.supplierevaluation.items", "en-US", "评价项目明细列表_us", "评价项目明细列表（主子表关系）"),
            // entity.supplierevaluation.items
            new TranslationSeedItem("entity.supplierevaluation.items", "ja-JP", "评价项目明细列表_jp", "评价项目明细列表（主子表关系）"),
            // entity.supplierevaluation.items
            new TranslationSeedItem("entity.supplierevaluation.items", "zh-CN", "评价项目明细列表", "评价项目明细列表（主子表关系）"),
            // entity.supplierevaluation.items
            new TranslationSeedItem("entity.supplierevaluation.items", "zh-HK", "评价项目明细列表_hk", "评价项目明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Complaint";
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
