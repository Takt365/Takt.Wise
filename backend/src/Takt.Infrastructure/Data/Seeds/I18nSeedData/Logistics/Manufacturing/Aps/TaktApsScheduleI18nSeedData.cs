// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktApsSchedule 实体国际化翻译种子（键前缀 entity.apsschedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsschedule 实体翻译...", tenantCode);

        foreach (var item in GetApsScheduleTranslations())
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

        TaktLogger.Information("TaktApsSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsschedule._self / entity.apsschedule.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsschedule._self
            new TranslationSeedItem("entity.apsschedule._self", "en-US", "Aps Schedule Information_us", "实体名称"),
            // entity.apsschedule._self
            new TranslationSeedItem("entity.apsschedule._self", "ja-JP", "APS排程主表信息_jp", "实体名称"),
            // entity.apsschedule._self
            new TranslationSeedItem("entity.apsschedule._self", "zh-CN", "APS排程主表信息", "实体名称"),
            // entity.apsschedule._self
            new TranslationSeedItem("entity.apsschedule._self", "zh-HK", "APS排程主表信息_hk", "实体名称"),

            // entity.apsschedule.materialrequirementsplanningid
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningid", "en-US", "来源MRP头表ID_us", "来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）"),
            // entity.apsschedule.materialrequirementsplanningid
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningid", "ja-JP", "来源MRP头表ID_jp", "来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）"),
            // entity.apsschedule.materialrequirementsplanningid
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningid", "zh-CN", "来源MRP头表ID", "来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）"),
            // entity.apsschedule.materialrequirementsplanningid
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningid", "zh-HK", "来源MRP头表ID_hk", "来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）"),

            // entity.apsschedule.materialrequirementsplanningcode
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningcode", "en-US", "来源MRP编码_us", "来源 MRP 编码（冗余）"),
            // entity.apsschedule.materialrequirementsplanningcode
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningcode", "ja-JP", "来源MRP编码_jp", "来源 MRP 编码（冗余）"),
            // entity.apsschedule.materialrequirementsplanningcode
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningcode", "zh-CN", "来源MRP编码", "来源 MRP 编码（冗余）"),
            // entity.apsschedule.materialrequirementsplanningcode
            new TranslationSeedItem("entity.apsschedule.materialrequirementsplanningcode", "zh-HK", "来源MRP编码_hk", "来源 MRP 编码（冗余）"),

            // entity.apsschedule.schedulecode
            new TranslationSeedItem("entity.apsschedule.schedulecode", "en-US", "排程编码_us", "排程编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 排程编码规则生成并展示，非手输；单据类型菜单：高级排程）"),
            // entity.apsschedule.schedulecode
            new TranslationSeedItem("entity.apsschedule.schedulecode", "ja-JP", "排程编码_jp", "排程编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 排程编码规则生成并展示，非手输；单据类型菜单：高级排程）"),
            // entity.apsschedule.schedulecode
            new TranslationSeedItem("entity.apsschedule.schedulecode", "zh-CN", "排程编码", "排程编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 排程编码规则生成并展示，非手输；单据类型菜单：高级排程）"),
            // entity.apsschedule.schedulecode
            new TranslationSeedItem("entity.apsschedule.schedulecode", "zh-HK", "排程编码_hk", "排程编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 排程编码规则生成并展示，非手输；单据类型菜单：高级排程）"),

            // entity.apsschedule.schedulename
            new TranslationSeedItem("entity.apsschedule.schedulename", "en-US", "排程名称_us", "排程名称"),
            // entity.apsschedule.schedulename
            new TranslationSeedItem("entity.apsschedule.schedulename", "ja-JP", "排程名称_jp", "排程名称"),
            // entity.apsschedule.schedulename
            new TranslationSeedItem("entity.apsschedule.schedulename", "zh-CN", "排程名称", "排程名称"),
            // entity.apsschedule.schedulename
            new TranslationSeedItem("entity.apsschedule.schedulename", "zh-HK", "排程名称_hk", "排程名称"),

            // entity.apsschedule.scheduletype
            new TranslationSeedItem("entity.apsschedule.scheduletype", "en-US", "排程类型_us", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsschedule.scheduletype
            new TranslationSeedItem("entity.apsschedule.scheduletype", "ja-JP", "排程类型_jp", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsschedule.scheduletype
            new TranslationSeedItem("entity.apsschedule.scheduletype", "zh-CN", "排程类型", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsschedule.scheduletype
            new TranslationSeedItem("entity.apsschedule.scheduletype", "zh-HK", "排程类型_hk", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),

            // entity.apsschedule.plandate
            new TranslationSeedItem("entity.apsschedule.plandate", "en-US", "计划日期_us", "计划日期"),
            // entity.apsschedule.plandate
            new TranslationSeedItem("entity.apsschedule.plandate", "ja-JP", "计划日期_jp", "计划日期"),
            // entity.apsschedule.plandate
            new TranslationSeedItem("entity.apsschedule.plandate", "zh-CN", "计划日期", "计划日期"),
            // entity.apsschedule.plandate
            new TranslationSeedItem("entity.apsschedule.plandate", "zh-HK", "计划日期_hk", "计划日期"),

            // entity.apsschedule.planstarttime
            new TranslationSeedItem("entity.apsschedule.planstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.apsschedule.planstarttime
            new TranslationSeedItem("entity.apsschedule.planstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.apsschedule.planstarttime
            new TranslationSeedItem("entity.apsschedule.planstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsschedule.planstarttime
            new TranslationSeedItem("entity.apsschedule.planstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.apsschedule.planendtime
            new TranslationSeedItem("entity.apsschedule.planendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.apsschedule.planendtime
            new TranslationSeedItem("entity.apsschedule.planendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.apsschedule.planendtime
            new TranslationSeedItem("entity.apsschedule.planendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsschedule.planendtime
            new TranslationSeedItem("entity.apsschedule.planendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.apsschedule.plancycle
            new TranslationSeedItem("entity.apsschedule.plancycle", "en-US", "计划周期_us", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsschedule.plancycle
            new TranslationSeedItem("entity.apsschedule.plancycle", "ja-JP", "计划周期_jp", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsschedule.plancycle
            new TranslationSeedItem("entity.apsschedule.plancycle", "zh-CN", "计划周期", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsschedule.plancycle
            new TranslationSeedItem("entity.apsschedule.plancycle", "zh-HK", "计划周期_hk", "计划周期（0=日计划，1=周计划，2=月计划）"),

            // entity.apsschedule.workshopcode
            new TranslationSeedItem("entity.apsschedule.workshopcode", "en-US", "车间编码_us", "车间编码"),
            // entity.apsschedule.workshopcode
            new TranslationSeedItem("entity.apsschedule.workshopcode", "ja-JP", "车间编码_jp", "车间编码"),
            // entity.apsschedule.workshopcode
            new TranslationSeedItem("entity.apsschedule.workshopcode", "zh-CN", "车间编码", "车间编码"),
            // entity.apsschedule.workshopcode
            new TranslationSeedItem("entity.apsschedule.workshopcode", "zh-HK", "车间编码_hk", "车间编码"),

            // entity.apsschedule.workshopname
            new TranslationSeedItem("entity.apsschedule.workshopname", "en-US", "车间名称_us", "车间名称"),
            // entity.apsschedule.workshopname
            new TranslationSeedItem("entity.apsschedule.workshopname", "ja-JP", "车间名称_jp", "车间名称"),
            // entity.apsschedule.workshopname
            new TranslationSeedItem("entity.apsschedule.workshopname", "zh-CN", "车间名称", "车间名称"),
            // entity.apsschedule.workshopname
            new TranslationSeedItem("entity.apsschedule.workshopname", "zh-HK", "车间名称_hk", "车间名称"),

            // entity.apsschedule.productionlinecode
            new TranslationSeedItem("entity.apsschedule.productionlinecode", "en-US", "生产线编码_us", "生产班组编码"),
            // entity.apsschedule.productionlinecode
            new TranslationSeedItem("entity.apsschedule.productionlinecode", "ja-JP", "生产线编码_jp", "生产班组编码"),
            // entity.apsschedule.productionlinecode
            new TranslationSeedItem("entity.apsschedule.productionlinecode", "zh-CN", "生产线编码", "生产班组编码"),
            // entity.apsschedule.productionlinecode
            new TranslationSeedItem("entity.apsschedule.productionlinecode", "zh-HK", "生产线编码_hk", "生产班组编码"),

            // entity.apsschedule.productionlinename
            new TranslationSeedItem("entity.apsschedule.productionlinename", "en-US", "生产班组名称_us", "生产班组名称"),
            // entity.apsschedule.productionlinename
            new TranslationSeedItem("entity.apsschedule.productionlinename", "ja-JP", "生产班组名称_jp", "生产班组名称"),
            // entity.apsschedule.productionlinename
            new TranslationSeedItem("entity.apsschedule.productionlinename", "zh-CN", "生产班组名称", "生产班组名称"),
            // entity.apsschedule.productionlinename
            new TranslationSeedItem("entity.apsschedule.productionlinename", "zh-HK", "生产班组名称_hk", "生产班组名称"),

            // entity.apsschedule.schedulestrategy
            new TranslationSeedItem("entity.apsschedule.schedulestrategy", "en-US", "排程策略_us", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsschedule.schedulestrategy
            new TranslationSeedItem("entity.apsschedule.schedulestrategy", "ja-JP", "排程策略_jp", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsschedule.schedulestrategy
            new TranslationSeedItem("entity.apsschedule.schedulestrategy", "zh-CN", "排程策略", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsschedule.schedulestrategy
            new TranslationSeedItem("entity.apsschedule.schedulestrategy", "zh-HK", "排程策略_hk", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),

            // entity.apsschedule.schedulealgorithm
            new TranslationSeedItem("entity.apsschedule.schedulealgorithm", "en-US", "排程算法_us", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsschedule.schedulealgorithm
            new TranslationSeedItem("entity.apsschedule.schedulealgorithm", "ja-JP", "排程算法_jp", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsschedule.schedulealgorithm
            new TranslationSeedItem("entity.apsschedule.schedulealgorithm", "zh-CN", "排程算法", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsschedule.schedulealgorithm
            new TranslationSeedItem("entity.apsschedule.schedulealgorithm", "zh-HK", "排程算法_hk", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),

            // entity.apsschedule.optimizationobjective
            new TranslationSeedItem("entity.apsschedule.optimizationobjective", "en-US", "优化目标_us", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsschedule.optimizationobjective
            new TranslationSeedItem("entity.apsschedule.optimizationobjective", "ja-JP", "优化目标_jp", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsschedule.optimizationobjective
            new TranslationSeedItem("entity.apsschedule.optimizationobjective", "zh-CN", "优化目标", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsschedule.optimizationobjective
            new TranslationSeedItem("entity.apsschedule.optimizationobjective", "zh-HK", "优化目标_hk", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),

            // entity.apsschedule.schedulestatus
            new TranslationSeedItem("entity.apsschedule.schedulestatus", "en-US", "排程状态_us", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsschedule.schedulestatus
            new TranslationSeedItem("entity.apsschedule.schedulestatus", "ja-JP", "排程状态_jp", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsschedule.schedulestatus
            new TranslationSeedItem("entity.apsschedule.schedulestatus", "zh-CN", "排程状态", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsschedule.schedulestatus
            new TranslationSeedItem("entity.apsschedule.schedulestatus", "zh-HK", "排程状态_hk", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),

            // entity.apsschedule.plannergroupcode
            new TranslationSeedItem("entity.apsschedule.plannergroupcode", "en-US", "计划组编码_us", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.apsschedule.plannergroupcode
            new TranslationSeedItem("entity.apsschedule.plannergroupcode", "ja-JP", "计划组编码_jp", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.apsschedule.plannergroupcode
            new TranslationSeedItem("entity.apsschedule.plannergroupcode", "zh-CN", "计划组编码", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),
            // entity.apsschedule.plannergroupcode
            new TranslationSeedItem("entity.apsschedule.plannergroupcode", "zh-HK", "计划组编码_hk", "计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）"),

            // entity.apsschedule.plannergroupname
            new TranslationSeedItem("entity.apsschedule.plannergroupname", "en-US", "计划组名称_us", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.apsschedule.plannergroupname
            new TranslationSeedItem("entity.apsschedule.plannergroupname", "ja-JP", "计划组名称_jp", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.apsschedule.plannergroupname
            new TranslationSeedItem("entity.apsschedule.plannergroupname", "zh-CN", "计划组名称", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),
            // entity.apsschedule.plannergroupname
            new TranslationSeedItem("entity.apsschedule.plannergroupname", "zh-HK", "计划组名称_hk", "计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）"),

            // entity.apsschedule.publishtime
            new TranslationSeedItem("entity.apsschedule.publishtime", "en-US", "发布时间_us", "发布时间"),
            // entity.apsschedule.publishtime
            new TranslationSeedItem("entity.apsschedule.publishtime", "ja-JP", "发布时间_jp", "发布时间"),
            // entity.apsschedule.publishtime
            new TranslationSeedItem("entity.apsschedule.publishtime", "zh-CN", "发布时间", "发布时间"),
            // entity.apsschedule.publishtime
            new TranslationSeedItem("entity.apsschedule.publishtime", "zh-HK", "发布时间_hk", "发布时间"),

            // entity.apsschedule.publishuserid
            new TranslationSeedItem("entity.apsschedule.publishuserid", "en-US", "发布人ID_us", "发布人（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）"),
            // entity.apsschedule.publishuserid
            new TranslationSeedItem("entity.apsschedule.publishuserid", "ja-JP", "发布人ID_jp", "发布人（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）"),
            // entity.apsschedule.publishuserid
            new TranslationSeedItem("entity.apsschedule.publishuserid", "zh-CN", "发布人ID", "发布人（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）"),
            // entity.apsschedule.publishuserid
            new TranslationSeedItem("entity.apsschedule.publishuserid", "zh-HK", "发布人ID_hk", "发布人（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）"),

            // entity.apsschedule.publishusername
            new TranslationSeedItem("entity.apsschedule.publishusername", "en-US", "发布人姓名_us", "发布人姓名（冗余字段,便于查询）"),
            // entity.apsschedule.publishusername
            new TranslationSeedItem("entity.apsschedule.publishusername", "ja-JP", "发布人姓名_jp", "发布人姓名（冗余字段,便于查询）"),
            // entity.apsschedule.publishusername
            new TranslationSeedItem("entity.apsschedule.publishusername", "zh-CN", "发布人姓名", "发布人姓名（冗余字段,便于查询）"),
            // entity.apsschedule.publishusername
            new TranslationSeedItem("entity.apsschedule.publishusername", "zh-HK", "发布人姓名_hk", "发布人姓名（冗余字段,便于查询）"),

            // entity.apsschedule.scheduledescription
            new TranslationSeedItem("entity.apsschedule.scheduledescription", "en-US", "排程说明_us", "排程说明"),
            // entity.apsschedule.scheduledescription
            new TranslationSeedItem("entity.apsschedule.scheduledescription", "ja-JP", "排程说明_jp", "排程说明"),
            // entity.apsschedule.scheduledescription
            new TranslationSeedItem("entity.apsschedule.scheduledescription", "zh-CN", "排程说明", "排程说明"),
            // entity.apsschedule.scheduledescription
            new TranslationSeedItem("entity.apsschedule.scheduledescription", "zh-HK", "排程说明_hk", "排程说明"),

            // entity.apsschedule.orders
            new TranslationSeedItem("entity.apsschedule.orders", "en-US", "APS 排程订单列表_us", "APS 排程订单列表（排程批次关联的订单）"),
            // entity.apsschedule.orders
            new TranslationSeedItem("entity.apsschedule.orders", "ja-JP", "APS 排程订单列表_jp", "APS 排程订单列表（排程批次关联的订单）"),
            // entity.apsschedule.orders
            new TranslationSeedItem("entity.apsschedule.orders", "zh-CN", "APS 排程订单列表", "APS 排程订单列表（排程批次关联的订单）"),
            // entity.apsschedule.orders
            new TranslationSeedItem("entity.apsschedule.orders", "zh-HK", "APS 排程订单列表_hk", "APS 排程订单列表（排程批次关联的订单）"),

            // entity.apsschedule.items
            new TranslationSeedItem("entity.apsschedule.items", "en-US", "排程明细列表_us", "排程明细列表（主子表关系）"),
            // entity.apsschedule.items
            new TranslationSeedItem("entity.apsschedule.items", "ja-JP", "排程明细列表_jp", "排程明细列表（主子表关系）"),
            // entity.apsschedule.items
            new TranslationSeedItem("entity.apsschedule.items", "zh-CN", "排程明细列表", "排程明细列表（主子表关系）"),
            // entity.apsschedule.items
            new TranslationSeedItem("entity.apsschedule.items", "zh-HK", "排程明细列表_hk", "排程明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Aps";
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
