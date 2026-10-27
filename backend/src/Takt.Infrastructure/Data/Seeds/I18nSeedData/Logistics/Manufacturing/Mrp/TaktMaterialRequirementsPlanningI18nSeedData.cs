// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialRequirementsPlanning 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialRequirementsPlanning 实体国际化翻译种子（键前缀 entity.materialrequirementsplanning.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialRequirementsPlanningI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialRequirementsPlanning 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialrequirementsplanning 实体翻译...", tenantCode);

        foreach (var item in GetMaterialRequirementsPlanningTranslations())
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

        TaktLogger.Information("TaktMaterialRequirementsPlanning 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialRequirementsPlanning 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialrequirementsplanning._self / entity.materialrequirementsplanning.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialRequirementsPlanningTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialrequirementsplanning._self
            new TranslationSeedItem("entity.materialrequirementsplanning._self", "en-US", "Material Requirements Planning Information_us", "实体名称"),
            // entity.materialrequirementsplanning._self
            new TranslationSeedItem("entity.materialrequirementsplanning._self", "ja-JP", "物料需求计划 MRP 头表信息_jp", "实体名称"),
            // entity.materialrequirementsplanning._self
            new TranslationSeedItem("entity.materialrequirementsplanning._self", "zh-CN", "物料需求计划 MRP 头表信息", "实体名称"),
            // entity.materialrequirementsplanning._self
            new TranslationSeedItem("entity.materialrequirementsplanning._self", "zh-HK", "物料需求计划 MRP 头表信息_hk", "实体名称"),

            // entity.materialrequirementsplanning.code
            new TranslationSeedItem("entity.materialrequirementsplanning.code", "en-US", "MRP编码_us", "MRP 编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering MRP编码规则生成并展示，非手输；单据类型菜单：MRP运算向导）"),
            // entity.materialrequirementsplanning.code
            new TranslationSeedItem("entity.materialrequirementsplanning.code", "ja-JP", "MRP编码_jp", "MRP 编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering MRP编码规则生成并展示，非手输；单据类型菜单：MRP运算向导）"),
            // entity.materialrequirementsplanning.code
            new TranslationSeedItem("entity.materialrequirementsplanning.code", "zh-CN", "MRP编码", "MRP 编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering MRP编码规则生成并展示，非手输；单据类型菜单：MRP运算向导）"),
            // entity.materialrequirementsplanning.code
            new TranslationSeedItem("entity.materialrequirementsplanning.code", "zh-HK", "MRP编码_hk", "MRP 编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering MRP编码规则生成并展示，非手输；单据类型菜单：MRP运算向导）"),

            // entity.materialrequirementsplanning.masterproductionscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterproductionscheduleid", "en-US", "来源MPS头表ID_us", "来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）"),
            // entity.materialrequirementsplanning.masterproductionscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterproductionscheduleid", "ja-JP", "来源MPS头表ID_jp", "来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）"),
            // entity.materialrequirementsplanning.masterproductionscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterproductionscheduleid", "zh-CN", "来源MPS头表ID", "来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）"),
            // entity.materialrequirementsplanning.masterproductionscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterproductionscheduleid", "zh-HK", "来源MPS头表ID_hk", "来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）"),

            // entity.materialrequirementsplanning.mpscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mpscode", "en-US", "来源MPS编码_us", "来源 MPS 编码（冗余）"),
            // entity.materialrequirementsplanning.mpscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mpscode", "ja-JP", "来源MPS编码_jp", "来源 MPS 编码（冗余）"),
            // entity.materialrequirementsplanning.mpscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mpscode", "zh-CN", "来源MPS编码", "来源 MPS 编码（冗余）"),
            // entity.materialrequirementsplanning.mpscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mpscode", "zh-HK", "来源MPS编码_hk", "来源 MPS 编码（冗余）"),

            // entity.materialrequirementsplanning.masterdemandscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterdemandscheduleid", "en-US", "来源MDS头表ID_us", "来源 MDS 头表 ID（Demand 层追溯，可选）"),
            // entity.materialrequirementsplanning.masterdemandscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterdemandscheduleid", "ja-JP", "来源MDS头表ID_jp", "来源 MDS 头表 ID（Demand 层追溯，可选）"),
            // entity.materialrequirementsplanning.masterdemandscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterdemandscheduleid", "zh-CN", "来源MDS头表ID", "来源 MDS 头表 ID（Demand 层追溯，可选）"),
            // entity.materialrequirementsplanning.masterdemandscheduleid
            new TranslationSeedItem("entity.materialrequirementsplanning.masterdemandscheduleid", "zh-HK", "来源MDS头表ID_hk", "来源 MDS 头表 ID（Demand 层追溯，可选）"),

            // entity.materialrequirementsplanning.mdscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mdscode", "en-US", "来源MDS编码_us", "来源 MDS 编码（冗余）"),
            // entity.materialrequirementsplanning.mdscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mdscode", "ja-JP", "来源MDS编码_jp", "来源 MDS 编码（冗余）"),
            // entity.materialrequirementsplanning.mdscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mdscode", "zh-CN", "来源MDS编码", "来源 MDS 编码（冗余）"),
            // entity.materialrequirementsplanning.mdscode
            new TranslationSeedItem("entity.materialrequirementsplanning.mdscode", "zh-HK", "来源MDS编码_hk", "来源 MDS 编码（冗余）"),

            // entity.materialrequirementsplanning.plandate
            new TranslationSeedItem("entity.materialrequirementsplanning.plandate", "en-US", "计划编制日期_us", "计划编制日期"),
            // entity.materialrequirementsplanning.plandate
            new TranslationSeedItem("entity.materialrequirementsplanning.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期"),
            // entity.materialrequirementsplanning.plandate
            new TranslationSeedItem("entity.materialrequirementsplanning.plandate", "zh-CN", "计划编制日期", "计划编制日期"),
            // entity.materialrequirementsplanning.plandate
            new TranslationSeedItem("entity.materialrequirementsplanning.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期"),

            // entity.materialrequirementsplanning.planperiodstart
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodstart", "en-US", "计划周期开始日期_us", "计划周期开始日期"),
            // entity.materialrequirementsplanning.planperiodstart
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodstart", "ja-JP", "计划周期开始日期_jp", "计划周期开始日期"),
            // entity.materialrequirementsplanning.planperiodstart
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodstart", "zh-CN", "计划周期开始日期", "计划周期开始日期"),
            // entity.materialrequirementsplanning.planperiodstart
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodstart", "zh-HK", "计划周期开始日期_hk", "计划周期开始日期"),

            // entity.materialrequirementsplanning.planperiodend
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodend", "en-US", "计划周期结束日期_us", "计划周期结束日期"),
            // entity.materialrequirementsplanning.planperiodend
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodend", "ja-JP", "计划周期结束日期_jp", "计划周期结束日期"),
            // entity.materialrequirementsplanning.planperiodend
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodend", "zh-CN", "计划周期结束日期", "计划周期结束日期"),
            // entity.materialrequirementsplanning.planperiodend
            new TranslationSeedItem("entity.materialrequirementsplanning.planperiodend", "zh-HK", "计划周期结束日期_hk", "计划周期结束日期"),

            // entity.materialrequirementsplanning.plannergroupcode
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.materialrequirementsplanning.plannergroupcode
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.materialrequirementsplanning.plannergroupcode
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.materialrequirementsplanning.plannergroupcode
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.materialrequirementsplanning.plannergroupname
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.materialrequirementsplanning.plannergroupname
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.materialrequirementsplanning.plannergroupname
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.materialrequirementsplanning.plannergroupname
            new TranslationSeedItem("entity.materialrequirementsplanning.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.materialrequirementsplanning.runstatus
            new TranslationSeedItem("entity.materialrequirementsplanning.runstatus", "en-US", "运算状态_us", "运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）"),
            // entity.materialrequirementsplanning.runstatus
            new TranslationSeedItem("entity.materialrequirementsplanning.runstatus", "ja-JP", "运算状态_jp", "运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）"),
            // entity.materialrequirementsplanning.runstatus
            new TranslationSeedItem("entity.materialrequirementsplanning.runstatus", "zh-CN", "运算状态", "运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）"),
            // entity.materialrequirementsplanning.runstatus
            new TranslationSeedItem("entity.materialrequirementsplanning.runstatus", "zh-HK", "运算状态_hk", "运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）"),

            // entity.materialrequirementsplanning.productionplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplanid", "en-US", "产出生产计划ID_us", "产出生产计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.productionplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplanid", "ja-JP", "产出生产计划ID_jp", "产出生产计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.productionplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplanid", "zh-CN", "产出生产计划ID", "产出生产计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.productionplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplanid", "zh-HK", "产出生产计划ID_hk", "产出生产计划 ID（运算完成后回写）"),

            // entity.materialrequirementsplanning.productionplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplancode", "en-US", "产出生产计划编码_us", "产出生产计划编码（冗余）"),
            // entity.materialrequirementsplanning.productionplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplancode", "ja-JP", "产出生产计划编码_jp", "产出生产计划编码（冗余）"),
            // entity.materialrequirementsplanning.productionplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplancode", "zh-CN", "产出生产计划编码", "产出生产计划编码（冗余）"),
            // entity.materialrequirementsplanning.productionplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.productionplancode", "zh-HK", "产出生产计划编码_hk", "产出生产计划编码（冗余）"),

            // entity.materialrequirementsplanning.purchaseplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplanid", "en-US", "产出采购计划ID_us", "产出采购计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.purchaseplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplanid", "ja-JP", "产出采购计划ID_jp", "产出采购计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.purchaseplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplanid", "zh-CN", "产出采购计划ID", "产出采购计划 ID（运算完成后回写）"),
            // entity.materialrequirementsplanning.purchaseplanid
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplanid", "zh-HK", "产出采购计划ID_hk", "产出采购计划 ID（运算完成后回写）"),

            // entity.materialrequirementsplanning.purchaseplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplancode", "en-US", "产出采购计划编码_us", "产出采购计划编码（冗余）"),
            // entity.materialrequirementsplanning.purchaseplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplancode", "ja-JP", "产出采购计划编码_jp", "产出采购计划编码（冗余）"),
            // entity.materialrequirementsplanning.purchaseplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplancode", "zh-CN", "产出采购计划编码", "产出采购计划编码（冗余）"),
            // entity.materialrequirementsplanning.purchaseplancode
            new TranslationSeedItem("entity.materialrequirementsplanning.purchaseplancode", "zh-HK", "产出采购计划编码_hk", "产出采购计划编码（冗余）"),

            // entity.materialrequirementsplanning.plandescription
            new TranslationSeedItem("entity.materialrequirementsplanning.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.materialrequirementsplanning.plandescription
            new TranslationSeedItem("entity.materialrequirementsplanning.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.materialrequirementsplanning.plandescription
            new TranslationSeedItem("entity.materialrequirementsplanning.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.materialrequirementsplanning.plandescription
            new TranslationSeedItem("entity.materialrequirementsplanning.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.materialrequirementsplanning.items
            new TranslationSeedItem("entity.materialrequirementsplanning.items", "en-US", "MRP 需求明细列表_us", "MRP 需求明细列表（主子表关系）"),
            // entity.materialrequirementsplanning.items
            new TranslationSeedItem("entity.materialrequirementsplanning.items", "ja-JP", "MRP 需求明细列表_jp", "MRP 需求明细列表（主子表关系）"),
            // entity.materialrequirementsplanning.items
            new TranslationSeedItem("entity.materialrequirementsplanning.items", "zh-CN", "MRP 需求明细列表", "MRP 需求明细列表（主子表关系）"),
            // entity.materialrequirementsplanning.items
            new TranslationSeedItem("entity.materialrequirementsplanning.items", "zh-HK", "MRP 需求明细列表_hk", "MRP 需求明细列表（主子表关系）"),
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
