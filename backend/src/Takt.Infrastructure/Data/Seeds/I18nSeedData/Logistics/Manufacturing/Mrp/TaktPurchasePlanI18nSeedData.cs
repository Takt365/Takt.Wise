// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlanI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchasePlan 实体国际化翻译种子（键前缀 entity.purchaseplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseplan 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePlanTranslations())
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

        TaktLogger.Information("TaktPurchasePlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseplan._self / entity.purchaseplan.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseplan._self
            new TranslationSeedItem("entity.purchaseplan._self", "en-US", "Purchase Plan Information_us", "实体名称"),
            // entity.purchaseplan._self
            new TranslationSeedItem("entity.purchaseplan._self", "ja-JP", "Takt采购计划信息_jp", "实体名称"),
            // entity.purchaseplan._self
            new TranslationSeedItem("entity.purchaseplan._self", "zh-CN", "Takt采购计划信息", "实体名称"),
            // entity.purchaseplan._self
            new TranslationSeedItem("entity.purchaseplan._self", "zh-HK", "Takt采购计划信息_hk", "实体名称"),

            // entity.purchaseplan.code
            new TranslationSeedItem("entity.purchaseplan.code", "en-US", "采购计划编码_us", "采购计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购计划编码规则生成并展示，非手输；单据类型菜单：采购计划）"),
            // entity.purchaseplan.code
            new TranslationSeedItem("entity.purchaseplan.code", "ja-JP", "采购计划编码_jp", "采购计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购计划编码规则生成并展示，非手输；单据类型菜单：采购计划）"),
            // entity.purchaseplan.code
            new TranslationSeedItem("entity.purchaseplan.code", "zh-CN", "采购计划编码", "采购计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购计划编码规则生成并展示，非手输；单据类型菜单：采购计划）"),
            // entity.purchaseplan.code
            new TranslationSeedItem("entity.purchaseplan.code", "zh-HK", "采购计划编码_hk", "采购计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购计划编码规则生成并展示，非手输；单据类型菜单：采购计划）"),

            // entity.purchaseplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningid", "en-US", "来源MRP头表ID_us", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningid", "ja-JP", "来源MRP头表ID_jp", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningid", "zh-CN", "来源MRP头表ID", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.materialrequirementsplanningid
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningid", "zh-HK", "来源MRP头表ID_hk", "来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningcode", "en-US", "来源MRP编码_us", "来源 MRP 编码（冗余）"),
            // entity.purchaseplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningcode", "ja-JP", "来源MRP编码_jp", "来源 MRP 编码（冗余）"),
            // entity.purchaseplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningcode", "zh-CN", "来源MRP编码", "来源 MRP 编码（冗余）"),
            // entity.purchaseplan.materialrequirementsplanningcode
            new TranslationSeedItem("entity.purchaseplan.materialrequirementsplanningcode", "zh-HK", "来源MRP编码_hk", "来源 MRP 编码（冗余）"),

            // entity.purchaseplan.productionplanid
            new TranslationSeedItem("entity.purchaseplan.productionplanid", "en-US", "来源生产计划ID_us", "来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.productionplanid
            new TranslationSeedItem("entity.purchaseplan.productionplanid", "ja-JP", "来源生产计划ID_jp", "来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.productionplanid
            new TranslationSeedItem("entity.purchaseplan.productionplanid", "zh-CN", "来源生产计划ID", "来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplan.productionplanid
            new TranslationSeedItem("entity.purchaseplan.productionplanid", "zh-HK", "来源生产计划ID_hk", "来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseplan.productionplancode
            new TranslationSeedItem("entity.purchaseplan.productionplancode", "en-US", "来源生产计划编码_us", "来源生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplan.productionplancode
            new TranslationSeedItem("entity.purchaseplan.productionplancode", "ja-JP", "来源生产计划编码_jp", "来源生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplan.productionplancode
            new TranslationSeedItem("entity.purchaseplan.productionplancode", "zh-CN", "来源生产计划编码", "来源生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplan.productionplancode
            new TranslationSeedItem("entity.purchaseplan.productionplancode", "zh-HK", "来源生产计划编码_hk", "来源生产计划编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaseplan.plandate
            new TranslationSeedItem("entity.purchaseplan.plandate", "en-US", "计划编制日期_us", "计划编制日期"),
            // entity.purchaseplan.plandate
            new TranslationSeedItem("entity.purchaseplan.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期"),
            // entity.purchaseplan.plandate
            new TranslationSeedItem("entity.purchaseplan.plandate", "zh-CN", "计划编制日期", "计划编制日期"),
            // entity.purchaseplan.plandate
            new TranslationSeedItem("entity.purchaseplan.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期"),

            // entity.purchaseplan.planperiodstart
            new TranslationSeedItem("entity.purchaseplan.planperiodstart", "en-US", "计划周期开始日期_us", "计划周期开始日期"),
            // entity.purchaseplan.planperiodstart
            new TranslationSeedItem("entity.purchaseplan.planperiodstart", "ja-JP", "计划周期开始日期_jp", "计划周期开始日期"),
            // entity.purchaseplan.planperiodstart
            new TranslationSeedItem("entity.purchaseplan.planperiodstart", "zh-CN", "计划周期开始日期", "计划周期开始日期"),
            // entity.purchaseplan.planperiodstart
            new TranslationSeedItem("entity.purchaseplan.planperiodstart", "zh-HK", "计划周期开始日期_hk", "计划周期开始日期"),

            // entity.purchaseplan.planperiodend
            new TranslationSeedItem("entity.purchaseplan.planperiodend", "en-US", "计划周期结束日期_us", "计划周期结束日期"),
            // entity.purchaseplan.planperiodend
            new TranslationSeedItem("entity.purchaseplan.planperiodend", "ja-JP", "计划周期结束日期_jp", "计划周期结束日期"),
            // entity.purchaseplan.planperiodend
            new TranslationSeedItem("entity.purchaseplan.planperiodend", "zh-CN", "计划周期结束日期", "计划周期结束日期"),
            // entity.purchaseplan.planperiodend
            new TranslationSeedItem("entity.purchaseplan.planperiodend", "zh-HK", "计划周期结束日期_hk", "计划周期结束日期"),

            // entity.purchaseplan.purchasegroupcode
            new TranslationSeedItem("entity.purchaseplan.purchasegroupcode", "en-US", "采购组编码_us", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseplan.purchasegroupcode
            new TranslationSeedItem("entity.purchaseplan.purchasegroupcode", "ja-JP", "采购组编码_jp", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseplan.purchasegroupcode
            new TranslationSeedItem("entity.purchaseplan.purchasegroupcode", "zh-CN", "采购组编码", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseplan.purchasegroupcode
            new TranslationSeedItem("entity.purchaseplan.purchasegroupcode", "zh-HK", "采购组编码_hk", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.purchaseplan.plannergroupcode
            new TranslationSeedItem("entity.purchaseplan.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseplan.plannergroupcode
            new TranslationSeedItem("entity.purchaseplan.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseplan.plannergroupcode
            new TranslationSeedItem("entity.purchaseplan.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.purchaseplan.plannergroupcode
            new TranslationSeedItem("entity.purchaseplan.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.purchaseplan.plannergroupname
            new TranslationSeedItem("entity.purchaseplan.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseplan.plannergroupname
            new TranslationSeedItem("entity.purchaseplan.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseplan.plannergroupname
            new TranslationSeedItem("entity.purchaseplan.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.purchaseplan.plannergroupname
            new TranslationSeedItem("entity.purchaseplan.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.purchaseplan.totalquantity
            new TranslationSeedItem("entity.purchaseplan.totalquantity", "en-US", "计划总数量_us", "计划总数量（基本单位数量）"),
            // entity.purchaseplan.totalquantity
            new TranslationSeedItem("entity.purchaseplan.totalquantity", "ja-JP", "计划总数量_jp", "计划总数量（基本单位数量）"),
            // entity.purchaseplan.totalquantity
            new TranslationSeedItem("entity.purchaseplan.totalquantity", "zh-CN", "计划总数量", "计划总数量（基本单位数量）"),
            // entity.purchaseplan.totalquantity
            new TranslationSeedItem("entity.purchaseplan.totalquantity", "zh-HK", "计划总数量_hk", "计划总数量（基本单位数量）"),

            // entity.purchaseplan.totalamount
            new TranslationSeedItem("entity.purchaseplan.totalamount", "en-US", "计划总金额_us", "计划总金额"),
            // entity.purchaseplan.totalamount
            new TranslationSeedItem("entity.purchaseplan.totalamount", "ja-JP", "计划总金额_jp", "计划总金额"),
            // entity.purchaseplan.totalamount
            new TranslationSeedItem("entity.purchaseplan.totalamount", "zh-CN", "计划总金额", "计划总金额"),
            // entity.purchaseplan.totalamount
            new TranslationSeedItem("entity.purchaseplan.totalamount", "zh-HK", "计划总金额_hk", "计划总金额"),

            // entity.purchaseplan.convertedquantity
            new TranslationSeedItem("entity.purchaseplan.convertedquantity", "en-US", "已转申请订单数量_us", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplan.convertedquantity
            new TranslationSeedItem("entity.purchaseplan.convertedquantity", "ja-JP", "已转申请订单数量_jp", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplan.convertedquantity
            new TranslationSeedItem("entity.purchaseplan.convertedquantity", "zh-CN", "已转申请订单数量", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplan.convertedquantity
            new TranslationSeedItem("entity.purchaseplan.convertedquantity", "zh-HK", "已转申请订单数量_hk", "已转申请/订单数量（基本单位数量）"),

            // entity.purchaseplan.convertedamount
            new TranslationSeedItem("entity.purchaseplan.convertedamount", "en-US", "已转申请订单金额_us", "已转申请/订单金额"),
            // entity.purchaseplan.convertedamount
            new TranslationSeedItem("entity.purchaseplan.convertedamount", "ja-JP", "已转申请订单金额_jp", "已转申请/订单金额"),
            // entity.purchaseplan.convertedamount
            new TranslationSeedItem("entity.purchaseplan.convertedamount", "zh-CN", "已转申请订单金额", "已转申请/订单金额"),
            // entity.purchaseplan.convertedamount
            new TranslationSeedItem("entity.purchaseplan.convertedamount", "zh-HK", "已转申请订单金额_hk", "已转申请/订单金额"),

            // entity.purchaseplan.planstatus
            new TranslationSeedItem("entity.purchaseplan.planstatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseplan.planstatus
            new TranslationSeedItem("entity.purchaseplan.planstatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseplan.planstatus
            new TranslationSeedItem("entity.purchaseplan.planstatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),
            // entity.purchaseplan.planstatus
            new TranslationSeedItem("entity.purchaseplan.planstatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）"),

            // entity.purchaseplan.convertedstatus
            new TranslationSeedItem("entity.purchaseplan.convertedstatus", "en-US", "转单状态_us", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseplan.convertedstatus
            new TranslationSeedItem("entity.purchaseplan.convertedstatus", "ja-JP", "转单状态_jp", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseplan.convertedstatus
            new TranslationSeedItem("entity.purchaseplan.convertedstatus", "zh-CN", "转单状态", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseplan.convertedstatus
            new TranslationSeedItem("entity.purchaseplan.convertedstatus", "zh-HK", "转单状态_hk", "转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.purchaseplan.plandescription
            new TranslationSeedItem("entity.purchaseplan.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.purchaseplan.plandescription
            new TranslationSeedItem("entity.purchaseplan.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.purchaseplan.plandescription
            new TranslationSeedItem("entity.purchaseplan.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.purchaseplan.plandescription
            new TranslationSeedItem("entity.purchaseplan.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.purchaseplan.items
            new TranslationSeedItem("entity.purchaseplan.items", "en-US", "采购计划明细列表_us", "采购计划明细列表（主子表关系，一个计划可有多个明细行）"),
            // entity.purchaseplan.items
            new TranslationSeedItem("entity.purchaseplan.items", "ja-JP", "采购计划明细列表_jp", "采购计划明细列表（主子表关系，一个计划可有多个明细行）"),
            // entity.purchaseplan.items
            new TranslationSeedItem("entity.purchaseplan.items", "zh-CN", "采购计划明细列表", "采购计划明细列表（主子表关系，一个计划可有多个明细行）"),
            // entity.purchaseplan.items
            new TranslationSeedItem("entity.purchaseplan.items", "zh-HK", "采购计划明细列表_hk", "采购计划明细列表（主子表关系，一个计划可有多个明细行）"),
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
