// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp;

/// <summary>
/// TaktProductionPlan 实体国际化翻译种子（键前缀 entity.productionplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionplan 实体翻译...", tenantCode);

        foreach (var item in GetProductionPlanTranslations())
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

        TaktLogger.Information("TaktProductionPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionplan._self / entity.productionplan.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionplan._self
            new TranslationSeedItem("entity.productionplan._self", "en-US", "Production Plan Information_us", "实体名称"),
            // entity.productionplan._self
            new TranslationSeedItem("entity.productionplan._self", "ja-JP", "Takt生产计划信息_jp", "实体名称"),
            // entity.productionplan._self
            new TranslationSeedItem("entity.productionplan._self", "zh-CN", "Takt生产计划信息", "实体名称"),
            // entity.productionplan._self
            new TranslationSeedItem("entity.productionplan._self", "zh-HK", "Takt生产计划信息_hk", "实体名称"),

            // entity.productionplan.code
            new TranslationSeedItem("entity.productionplan.code", "en-US", "生产计划编码_us", "生产计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 生产计划编码规则生成并展示，非手输；单据类型菜单：生产计划）"),
            // entity.productionplan.code
            new TranslationSeedItem("entity.productionplan.code", "ja-JP", "生产计划编码_jp", "生产计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 生产计划编码规则生成并展示，非手输；单据类型菜单：生产计划）"),
            // entity.productionplan.code
            new TranslationSeedItem("entity.productionplan.code", "zh-CN", "生产计划编码", "生产计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 生产计划编码规则生成并展示，非手输；单据类型菜单：生产计划）"),
            // entity.productionplan.code
            new TranslationSeedItem("entity.productionplan.code", "zh-HK", "生产计划编码_hk", "生产计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 生产计划编码规则生成并展示，非手输；单据类型菜单：生产计划）"),

            // entity.productionplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningid", "en-US", "来源MRP头表ID_us", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningid", "ja-JP", "来源MRP头表ID_jp", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningid", "zh-CN", "来源MRP头表ID", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningid", "zh-HK", "来源MRP头表ID_hk", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningcode", "en-US", "来源MRP编码_us", "来源 MRP 编码（冗余）"),
            // entity.productionplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningcode", "ja-JP", "来源MRP编码_jp", "来源 MRP 编码（冗余）"),
            // entity.productionplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningcode", "zh-CN", "来源MRP编码", "来源 MRP 编码（冗余）"),
            // entity.productionplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.productionplan.materialrequirementsplanningcode", "zh-HK", "来源MRP编码_hk", "来源 MRP 编码（冗余）"),

            // entity.productionplan.salesforecastid
            new TranslationSeedItem("entity.productionplan.salesforecastid", "en-US", "来源销售预测ID_us", "来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.salesforecastid
            new TranslationSeedItem("entity.productionplan.salesforecastid", "ja-JP", "来源销售预测ID_jp", "来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.salesforecastid
            new TranslationSeedItem("entity.productionplan.salesforecastid", "zh-CN", "来源销售预测ID", "来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplan.salesforecastid
            new TranslationSeedItem("entity.productionplan.salesforecastid", "zh-HK", "来源销售预测ID_hk", "来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplan.salesforecastcode
            new TranslationSeedItem("entity.productionplan.salesforecastcode", "en-US", "来源销售预测编码_us", "来源销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplan.salesforecastcode
            new TranslationSeedItem("entity.productionplan.salesforecastcode", "ja-JP", "来源销售预测编码_jp", "来源销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplan.salesforecastcode
            new TranslationSeedItem("entity.productionplan.salesforecastcode", "zh-CN", "来源销售预测编码", "来源销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplan.salesforecastcode
            new TranslationSeedItem("entity.productionplan.salesforecastcode", "zh-HK", "来源销售预测编码_hk", "来源销售预测编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.productionplan.plandate
            new TranslationSeedItem("entity.productionplan.plandate", "en-US", "计划编制日期_us", "计划编制日期"),
            // entity.productionplan.plandate
            new TranslationSeedItem("entity.productionplan.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期"),
            // entity.productionplan.plandate
            new TranslationSeedItem("entity.productionplan.plandate", "zh-CN", "计划编制日期", "计划编制日期"),
            // entity.productionplan.plandate
            new TranslationSeedItem("entity.productionplan.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期"),

            // entity.productionplan.planperiodstart
            new TranslationSeedItem("entity.productionplan.planperiodstart", "en-US", "计划周期开始日期_us", "计划周期开始日期"),
            // entity.productionplan.planperiodstart
            new TranslationSeedItem("entity.productionplan.planperiodstart", "ja-JP", "计划周期开始日期_jp", "计划周期开始日期"),
            // entity.productionplan.planperiodstart
            new TranslationSeedItem("entity.productionplan.planperiodstart", "zh-CN", "计划周期开始日期", "计划周期开始日期"),
            // entity.productionplan.planperiodstart
            new TranslationSeedItem("entity.productionplan.planperiodstart", "zh-HK", "计划周期开始日期_hk", "计划周期开始日期"),

            // entity.productionplan.planperiodend
            new TranslationSeedItem("entity.productionplan.planperiodend", "en-US", "计划周期结束日期_us", "计划周期结束日期"),
            // entity.productionplan.planperiodend
            new TranslationSeedItem("entity.productionplan.planperiodend", "ja-JP", "计划周期结束日期_jp", "计划周期结束日期"),
            // entity.productionplan.planperiodend
            new TranslationSeedItem("entity.productionplan.planperiodend", "zh-CN", "计划周期结束日期", "计划周期结束日期"),
            // entity.productionplan.planperiodend
            new TranslationSeedItem("entity.productionplan.planperiodend", "zh-HK", "计划周期结束日期_hk", "计划周期结束日期"),

            // entity.productionplan.plannergroupcode
            new TranslationSeedItem("entity.productionplan.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.productionplan.plannergroupcode
            new TranslationSeedItem("entity.productionplan.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.productionplan.plannergroupcode
            new TranslationSeedItem("entity.productionplan.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.productionplan.plannergroupcode
            new TranslationSeedItem("entity.productionplan.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.productionplan.plannergroupname
            new TranslationSeedItem("entity.productionplan.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.productionplan.plannergroupname
            new TranslationSeedItem("entity.productionplan.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.productionplan.plannergroupname
            new TranslationSeedItem("entity.productionplan.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.productionplan.plannergroupname
            new TranslationSeedItem("entity.productionplan.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.productionplan.totalquantity
            new TranslationSeedItem("entity.productionplan.totalquantity", "en-US", "计划总数量_us", "计划总数量（基本单位数量）"),
            // entity.productionplan.totalquantity
            new TranslationSeedItem("entity.productionplan.totalquantity", "ja-JP", "计划总数量_jp", "计划总数量（基本单位数量）"),
            // entity.productionplan.totalquantity
            new TranslationSeedItem("entity.productionplan.totalquantity", "zh-CN", "计划总数量", "计划总数量（基本单位数量）"),
            // entity.productionplan.totalquantity
            new TranslationSeedItem("entity.productionplan.totalquantity", "zh-HK", "计划总数量_hk", "计划总数量（基本单位数量）"),

            // entity.productionplan.totalamount
            new TranslationSeedItem("entity.productionplan.totalamount", "en-US", "计划总金额_us", "计划总金额"),
            // entity.productionplan.totalamount
            new TranslationSeedItem("entity.productionplan.totalamount", "ja-JP", "计划总金额_jp", "计划总金额"),
            // entity.productionplan.totalamount
            new TranslationSeedItem("entity.productionplan.totalamount", "zh-CN", "计划总金额", "计划总金额"),
            // entity.productionplan.totalamount
            new TranslationSeedItem("entity.productionplan.totalamount", "zh-HK", "计划总金额_hk", "计划总金额"),

            // entity.productionplan.convertedquantity
            new TranslationSeedItem("entity.productionplan.convertedquantity", "en-US", "已转工单采购数量_us", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplan.convertedquantity
            new TranslationSeedItem("entity.productionplan.convertedquantity", "ja-JP", "已转工单采购数量_jp", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplan.convertedquantity
            new TranslationSeedItem("entity.productionplan.convertedquantity", "zh-CN", "已转工单采购数量", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplan.convertedquantity
            new TranslationSeedItem("entity.productionplan.convertedquantity", "zh-HK", "已转工单采购数量_hk", "已转工单/采购数量（基本单位数量）"),

            // entity.productionplan.convertedamount
            new TranslationSeedItem("entity.productionplan.convertedamount", "en-US", "已转工单采购金额_us", "已转工单/采购金额"),
            // entity.productionplan.convertedamount
            new TranslationSeedItem("entity.productionplan.convertedamount", "ja-JP", "已转工单采购金额_jp", "已转工单/采购金额"),
            // entity.productionplan.convertedamount
            new TranslationSeedItem("entity.productionplan.convertedamount", "zh-CN", "已转工单采购金额", "已转工单/采购金额"),
            // entity.productionplan.convertedamount
            new TranslationSeedItem("entity.productionplan.convertedamount", "zh-HK", "已转工单采购金额_hk", "已转工单/采购金额"),

            // entity.productionplan.planstatus
            new TranslationSeedItem("entity.productionplan.planstatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.productionplan.planstatus
            new TranslationSeedItem("entity.productionplan.planstatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.productionplan.planstatus
            new TranslationSeedItem("entity.productionplan.planstatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.productionplan.planstatus
            new TranslationSeedItem("entity.productionplan.planstatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.productionplan.convertedstatus
            new TranslationSeedItem("entity.productionplan.convertedstatus", "en-US", "转单状态_us", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.productionplan.convertedstatus
            new TranslationSeedItem("entity.productionplan.convertedstatus", "ja-JP", "转单状态_jp", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.productionplan.convertedstatus
            new TranslationSeedItem("entity.productionplan.convertedstatus", "zh-CN", "转单状态", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.productionplan.convertedstatus
            new TranslationSeedItem("entity.productionplan.convertedstatus", "zh-HK", "转单状态_hk", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.productionplan.plandescription
            new TranslationSeedItem("entity.productionplan.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.productionplan.plandescription
            new TranslationSeedItem("entity.productionplan.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.productionplan.plandescription
            new TranslationSeedItem("entity.productionplan.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.productionplan.plandescription
            new TranslationSeedItem("entity.productionplan.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.productionplan.items
            new TranslationSeedItem("entity.productionplan.items", "en-US", "生产计划明细列表_us", "生产计划明细列表（主子表关系）"),
            // entity.productionplan.items
            new TranslationSeedItem("entity.productionplan.items", "ja-JP", "生产计划明细列表_jp", "生产计划明细列表（主子表关系）"),
            // entity.productionplan.items
            new TranslationSeedItem("entity.productionplan.items", "zh-CN", "生产计划明细列表", "生产计划明细列表（主子表关系）"),
            // entity.productionplan.items
            new TranslationSeedItem("entity.productionplan.items", "zh-HK", "生产计划明细列表_hk", "生产计划明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Mrp";
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
