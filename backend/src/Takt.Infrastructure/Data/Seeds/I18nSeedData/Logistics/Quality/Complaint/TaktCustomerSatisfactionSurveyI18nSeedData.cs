// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerSatisfactionSurvey 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerSatisfactionSurvey 实体国际化翻译种子（键前缀 entity.customersatisfactionsurvey.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerSatisfactionSurveyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerSatisfactionSurvey 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customersatisfactionsurvey 实体翻译...", tenantCode);

        foreach (var item in GetCustomerSatisfactionSurveyTranslations())
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

        TaktLogger.Information("TaktCustomerSatisfactionSurvey 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerSatisfactionSurvey 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customersatisfactionsurvey._self / entity.customersatisfactionsurvey.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerSatisfactionSurveyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customersatisfactionsurvey._self
            new TranslationSeedItem("entity.customersatisfactionsurvey._self", "en-US", "Customer Satisfaction Survey Information_us", "实体名称"),
            // entity.customersatisfactionsurvey._self
            new TranslationSeedItem("entity.customersatisfactionsurvey._self", "ja-JP", "客户满意度调查表主表信息_jp", "实体名称"),
            // entity.customersatisfactionsurvey._self
            new TranslationSeedItem("entity.customersatisfactionsurvey._self", "zh-CN", "客户满意度调查表主表信息", "实体名称"),
            // entity.customersatisfactionsurvey._self
            new TranslationSeedItem("entity.customersatisfactionsurvey._self", "zh-HK", "客户满意度调查表主表信息_hk", "实体名称"),

            // entity.customersatisfactionsurvey.code
            new TranslationSeedItem("entity.customersatisfactionsurvey.code", "en-US", "调查表编码_us", "调查表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 调查表编码规则生成并展示，非手输；单据类型菜单：客户满意度调查）"),
            // entity.customersatisfactionsurvey.code
            new TranslationSeedItem("entity.customersatisfactionsurvey.code", "ja-JP", "调查表编码_jp", "调查表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 调查表编码规则生成并展示，非手输；单据类型菜单：客户满意度调查）"),
            // entity.customersatisfactionsurvey.code
            new TranslationSeedItem("entity.customersatisfactionsurvey.code", "zh-CN", "调查表编码", "调查表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 调查表编码规则生成并展示，非手输；单据类型菜单：客户满意度调查）"),
            // entity.customersatisfactionsurvey.code
            new TranslationSeedItem("entity.customersatisfactionsurvey.code", "zh-HK", "调查表编码_hk", "调查表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 调查表编码规则生成并展示，非手输；单据类型菜单：客户满意度调查）"),

            // entity.customersatisfactionsurvey.customerid
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerid", "en-US", "客户ID_us", "客户 ID（选项 TaktCustomers/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.customerid
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerid", "ja-JP", "客户ID_jp", "客户 ID（选项 TaktCustomers/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.customerid
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerid", "zh-CN", "客户ID", "客户 ID（选项 TaktCustomers/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.customerid
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerid", "zh-HK", "客户ID_hk", "客户 ID（选项 TaktCustomers/options；DictValue=Id）"),

            // entity.customersatisfactionsurvey.customername1
            new TranslationSeedItem("entity.customersatisfactionsurvey.customername1", "en-US", "客户名称1_us", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.customersatisfactionsurvey.customername1
            new TranslationSeedItem("entity.customersatisfactionsurvey.customername1", "ja-JP", "客户名称1_jp", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.customersatisfactionsurvey.customername1
            new TranslationSeedItem("entity.customersatisfactionsurvey.customername1", "zh-CN", "客户名称1", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),
            // entity.customersatisfactionsurvey.customername1
            new TranslationSeedItem("entity.customersatisfactionsurvey.customername1", "zh-HK", "客户名称1_hk", "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）"),

            // entity.customersatisfactionsurvey.customercode
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customersatisfactionsurvey.customercode
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customersatisfactionsurvey.customercode
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customersatisfactionsurvey.customercode
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.customersatisfactionsurvey.surveydate
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveydate", "en-US", "调查日期_us", "调查日期"),
            // entity.customersatisfactionsurvey.surveydate
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveydate", "ja-JP", "调查日期_jp", "调查日期"),
            // entity.customersatisfactionsurvey.surveydate
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveydate", "zh-CN", "调查日期", "调查日期"),
            // entity.customersatisfactionsurvey.surveydate
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveydate", "zh-HK", "调查日期_hk", "调查日期"),

            // entity.customersatisfactionsurvey.surveymethod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveymethod", "en-US", "调查方式_us", "调查方式（字典 logistics_quality_survey_method）"),
            // entity.customersatisfactionsurvey.surveymethod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveymethod", "ja-JP", "调查方式_jp", "调查方式（字典 logistics_quality_survey_method）"),
            // entity.customersatisfactionsurvey.surveymethod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveymethod", "zh-CN", "调查方式", "调查方式（字典 logistics_quality_survey_method）"),
            // entity.customersatisfactionsurvey.surveymethod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveymethod", "zh-HK", "调查方式_hk", "调查方式（字典 logistics_quality_survey_method）"),

            // entity.customersatisfactionsurvey.surveytype
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveytype", "en-US", "调查类型_us", "调查类型（字典 logistics_quality_survey_type）"),
            // entity.customersatisfactionsurvey.surveytype
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveytype", "ja-JP", "调查类型_jp", "调查类型（字典 logistics_quality_survey_type）"),
            // entity.customersatisfactionsurvey.surveytype
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveytype", "zh-CN", "调查类型", "调查类型（字典 logistics_quality_survey_type）"),
            // entity.customersatisfactionsurvey.surveytype
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveytype", "zh-HK", "调查类型_hk", "调查类型（字典 logistics_quality_survey_type）"),

            // entity.customersatisfactionsurvey.surveyperiod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyperiod", "en-US", "调查周期_us", "调查周期（字典 logistics_quality_period）"),
            // entity.customersatisfactionsurvey.surveyperiod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyperiod", "ja-JP", "调查周期_jp", "调查周期（字典 logistics_quality_period）"),
            // entity.customersatisfactionsurvey.surveyperiod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyperiod", "zh-CN", "调查周期", "调查周期（字典 logistics_quality_period）"),
            // entity.customersatisfactionsurvey.surveyperiod
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyperiod", "zh-HK", "调查周期_hk", "调查周期（字典 logistics_quality_period）"),

            // entity.customersatisfactionsurvey.surveyorbygroupcode
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupcode", "en-US", "调查组编码_us", "调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）"),
            // entity.customersatisfactionsurvey.surveyorbygroupcode
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupcode", "ja-JP", "调查组编码_jp", "调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）"),
            // entity.customersatisfactionsurvey.surveyorbygroupcode
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupcode", "zh-CN", "调查组编码", "调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）"),
            // entity.customersatisfactionsurvey.surveyorbygroupcode
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupcode", "zh-HK", "调查组编码_hk", "调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）"),

            // entity.customersatisfactionsurvey.surveyorbygroupname
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupname", "en-US", "调查组名称_us", "调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customersatisfactionsurvey.surveyorbygroupname
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupname", "ja-JP", "调查组名称_jp", "调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customersatisfactionsurvey.surveyorbygroupname
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupname", "zh-CN", "调查组名称", "调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customersatisfactionsurvey.surveyorbygroupname
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveyorbygroupname", "zh-HK", "调查组名称_hk", "调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),

            // entity.customersatisfactionsurvey.customercontact
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercontact", "en-US", "客户联系人_us", "客户联系人"),
            // entity.customersatisfactionsurvey.customercontact
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercontact", "ja-JP", "客户联系人_jp", "客户联系人"),
            // entity.customersatisfactionsurvey.customercontact
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercontact", "zh-CN", "客户联系人", "客户联系人"),
            // entity.customersatisfactionsurvey.customercontact
            new TranslationSeedItem("entity.customersatisfactionsurvey.customercontact", "zh-HK", "客户联系人_hk", "客户联系人"),

            // entity.customersatisfactionsurvey.customerphone
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerphone", "en-US", "客户联系电话_us", "客户联系电话"),
            // entity.customersatisfactionsurvey.customerphone
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerphone", "ja-JP", "客户联系电话_jp", "客户联系电话"),
            // entity.customersatisfactionsurvey.customerphone
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerphone", "zh-CN", "客户联系电话", "客户联系电话"),
            // entity.customersatisfactionsurvey.customerphone
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerphone", "zh-HK", "客户联系电话_hk", "客户联系电话"),

            // entity.customersatisfactionsurvey.overallsatisfaction
            new TranslationSeedItem("entity.customersatisfactionsurvey.overallsatisfaction", "en-US", "整体满意度_us", "整体满意度（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurvey.overallsatisfaction
            new TranslationSeedItem("entity.customersatisfactionsurvey.overallsatisfaction", "ja-JP", "整体满意度_jp", "整体满意度（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurvey.overallsatisfaction
            new TranslationSeedItem("entity.customersatisfactionsurvey.overallsatisfaction", "zh-CN", "整体满意度", "整体满意度（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurvey.overallsatisfaction
            new TranslationSeedItem("entity.customersatisfactionsurvey.overallsatisfaction", "zh-HK", "整体满意度_hk", "整体满意度（字典 logistics_quality_satisfaction_level）"),

            // entity.customersatisfactionsurvey.totalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.totalscore", "en-US", "综合评分_us", "综合评分（0-100分）"),
            // entity.customersatisfactionsurvey.totalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.totalscore", "ja-JP", "综合评分_jp", "综合评分（0-100分）"),
            // entity.customersatisfactionsurvey.totalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.totalscore", "zh-CN", "综合评分", "综合评分（0-100分）"),
            // entity.customersatisfactionsurvey.totalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.totalscore", "zh-HK", "综合评分_hk", "综合评分（0-100分）"),

            // entity.customersatisfactionsurvey.qualityscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.qualityscore", "en-US", "产品质量评分_us", "产品质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.qualityscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.qualityscore", "ja-JP", "产品质量评分_jp", "产品质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.qualityscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.qualityscore", "zh-CN", "产品质量评分", "产品质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.qualityscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.qualityscore", "zh-HK", "产品质量评分_hk", "产品质量评分（0-100分）"),

            // entity.customersatisfactionsurvey.deliveryscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.deliveryscore", "en-US", "交付准时率评分_us", "交付准时率评分（0-100分）"),
            // entity.customersatisfactionsurvey.deliveryscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.deliveryscore", "ja-JP", "交付准时率评分_jp", "交付准时率评分（0-100分）"),
            // entity.customersatisfactionsurvey.deliveryscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.deliveryscore", "zh-CN", "交付准时率评分", "交付准时率评分（0-100分）"),
            // entity.customersatisfactionsurvey.deliveryscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.deliveryscore", "zh-HK", "交付准时率评分_hk", "交付准时率评分（0-100分）"),

            // entity.customersatisfactionsurvey.servicescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.servicescore", "en-US", "服务质量评分_us", "服务质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.servicescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.servicescore", "ja-JP", "服务质量评分_jp", "服务质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.servicescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.servicescore", "zh-CN", "服务质量评分", "服务质量评分（0-100分）"),
            // entity.customersatisfactionsurvey.servicescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.servicescore", "zh-HK", "服务质量评分_hk", "服务质量评分（0-100分）"),

            // entity.customersatisfactionsurvey.pricescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.pricescore", "en-US", "价格竞争力评分_us", "价格竞争力评分（0-100分）"),
            // entity.customersatisfactionsurvey.pricescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.pricescore", "ja-JP", "价格竞争力评分_jp", "价格竞争力评分（0-100分）"),
            // entity.customersatisfactionsurvey.pricescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.pricescore", "zh-CN", "价格竞争力评分", "价格竞争力评分（0-100分）"),
            // entity.customersatisfactionsurvey.pricescore
            new TranslationSeedItem("entity.customersatisfactionsurvey.pricescore", "zh-HK", "价格竞争力评分_hk", "价格竞争力评分（0-100分）"),

            // entity.customersatisfactionsurvey.technicalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.technicalscore", "en-US", "技术支持评分_us", "技术支持评分（0-100分）"),
            // entity.customersatisfactionsurvey.technicalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.technicalscore", "ja-JP", "技术支持评分_jp", "技术支持评分（0-100分）"),
            // entity.customersatisfactionsurvey.technicalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.technicalscore", "zh-CN", "技术支持评分", "技术支持评分（0-100分）"),
            // entity.customersatisfactionsurvey.technicalscore
            new TranslationSeedItem("entity.customersatisfactionsurvey.technicalscore", "zh-HK", "技术支持评分_hk", "技术支持评分（0-100分）"),

            // entity.customersatisfactionsurvey.customerpraise
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerpraise", "en-US", "客户主要表扬_us", "客户主要表扬"),
            // entity.customersatisfactionsurvey.customerpraise
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerpraise", "ja-JP", "客户主要表扬_jp", "客户主要表扬"),
            // entity.customersatisfactionsurvey.customerpraise
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerpraise", "zh-CN", "客户主要表扬", "客户主要表扬"),
            // entity.customersatisfactionsurvey.customerpraise
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerpraise", "zh-HK", "客户主要表扬_hk", "客户主要表扬"),

            // entity.customersatisfactionsurvey.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerfeedback", "en-US", "客户意见_us", "客户主要意见/建议"),
            // entity.customersatisfactionsurvey.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerfeedback", "ja-JP", "客户意见_jp", "客户主要意见/建议"),
            // entity.customersatisfactionsurvey.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerfeedback", "zh-CN", "客户意见", "客户主要意见/建议"),
            // entity.customersatisfactionsurvey.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurvey.customerfeedback", "zh-HK", "客户意见_hk", "客户主要意见/建议"),

            // entity.customersatisfactionsurvey.improvementplan
            new TranslationSeedItem("entity.customersatisfactionsurvey.improvementplan", "en-US", "改进计划_us", "改进计划/措施"),
            // entity.customersatisfactionsurvey.improvementplan
            new TranslationSeedItem("entity.customersatisfactionsurvey.improvementplan", "ja-JP", "改进计划_jp", "改进计划/措施"),
            // entity.customersatisfactionsurvey.improvementplan
            new TranslationSeedItem("entity.customersatisfactionsurvey.improvementplan", "zh-CN", "改进计划", "改进计划/措施"),
            // entity.customersatisfactionsurvey.improvementplan
            new TranslationSeedItem("entity.customersatisfactionsurvey.improvementplan", "zh-HK", "改进计划_hk", "改进计划/措施"),

            // entity.customersatisfactionsurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customersatisfactionsurvey.relatedcomplaintid", "en-US", "关联客诉ID_us", "关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customersatisfactionsurvey.relatedcomplaintid", "ja-JP", "关联客诉ID_jp", "关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customersatisfactionsurvey.relatedcomplaintid", "zh-CN", "关联客诉ID", "关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customersatisfactionsurvey.relatedcomplaintid
            new TranslationSeedItem("entity.customersatisfactionsurvey.relatedcomplaintid", "zh-HK", "关联客诉ID_hk", "关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),

            // entity.customersatisfactionsurvey.attachments
            new TranslationSeedItem("entity.customersatisfactionsurvey.attachments", "en-US", "附件JSON_us", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.customersatisfactionsurvey.attachments
            new TranslationSeedItem("entity.customersatisfactionsurvey.attachments", "ja-JP", "附件JSON_jp", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.customersatisfactionsurvey.attachments
            new TranslationSeedItem("entity.customersatisfactionsurvey.attachments", "zh-CN", "附件JSON", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.customersatisfactionsurvey.attachments
            new TranslationSeedItem("entity.customersatisfactionsurvey.attachments", "zh-HK", "附件JSON_hk", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),

            // entity.customersatisfactionsurvey.surveystatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveystatus", "en-US", "调查状态_us", "调查状态（字典 logistics_quality_survey_status）"),
            // entity.customersatisfactionsurvey.surveystatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveystatus", "ja-JP", "调查状态_jp", "调查状态（字典 logistics_quality_survey_status）"),
            // entity.customersatisfactionsurvey.surveystatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveystatus", "zh-CN", "调查状态", "调查状态（字典 logistics_quality_survey_status）"),
            // entity.customersatisfactionsurvey.surveystatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.surveystatus", "zh-HK", "调查状态_hk", "调查状态（字典 logistics_quality_survey_status）"),

            // entity.customersatisfactionsurvey.sortorder
            new TranslationSeedItem("entity.customersatisfactionsurvey.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customersatisfactionsurvey.sortorder
            new TranslationSeedItem("entity.customersatisfactionsurvey.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customersatisfactionsurvey.sortorder
            new TranslationSeedItem("entity.customersatisfactionsurvey.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customersatisfactionsurvey.sortorder
            new TranslationSeedItem("entity.customersatisfactionsurvey.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customersatisfactionsurvey.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.followupstatus", "en-US", "跟进状态_us", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurvey.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.followupstatus", "ja-JP", "跟进状态_jp", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurvey.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.followupstatus", "zh-CN", "跟进状态", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurvey.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurvey.followupstatus", "zh-HK", "跟进状态_hk", "跟进状态（字典 logistics_quality_follow_up_status）"),

            // entity.customersatisfactionsurvey.items
            new TranslationSeedItem("entity.customersatisfactionsurvey.items", "en-US", "调查项目明细列表_us", "调查项目明细列表（主子表关系）"),
            // entity.customersatisfactionsurvey.items
            new TranslationSeedItem("entity.customersatisfactionsurvey.items", "ja-JP", "调查项目明细列表_jp", "调查项目明细列表（主子表关系）"),
            // entity.customersatisfactionsurvey.items
            new TranslationSeedItem("entity.customersatisfactionsurvey.items", "zh-CN", "调查项目明细列表", "调查项目明细列表（主子表关系）"),
            // entity.customersatisfactionsurvey.items
            new TranslationSeedItem("entity.customersatisfactionsurvey.items", "zh-HK", "调查项目明细列表_hk", "调查项目明细列表（主子表关系）"),
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
