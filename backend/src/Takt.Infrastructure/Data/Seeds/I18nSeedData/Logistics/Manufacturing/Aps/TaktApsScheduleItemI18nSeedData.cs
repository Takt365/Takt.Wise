// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsScheduleItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktApsScheduleItem 实体国际化翻译种子（键前缀 entity.apsscheduleitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsScheduleItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsScheduleItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsscheduleitem 实体翻译...", tenantCode);

        foreach (var item in GetApsScheduleItemTranslations())
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

        TaktLogger.Information("TaktApsScheduleItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsScheduleItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsscheduleitem._self / entity.apsscheduleitem.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsScheduleItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsscheduleitem._self
            new TranslationSeedItem("entity.apsscheduleitem._self", "en-US", "Aps Schedule Item Information_us", "实体名称"),
            // entity.apsscheduleitem._self
            new TranslationSeedItem("entity.apsscheduleitem._self", "ja-JP", "APS排程明细信息_jp", "实体名称"),
            // entity.apsscheduleitem._self
            new TranslationSeedItem("entity.apsscheduleitem._self", "zh-CN", "APS排程明细信息", "实体名称"),
            // entity.apsscheduleitem._self
            new TranslationSeedItem("entity.apsscheduleitem._self", "zh-HK", "APS排程明细信息_hk", "实体名称"),

            // entity.apsscheduleitem.apsscheduleid
            new TranslationSeedItem("entity.apsscheduleitem.apsscheduleid", "en-US", "APS排程ID_us", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsscheduleitem.apsscheduleid
            new TranslationSeedItem("entity.apsscheduleitem.apsscheduleid", "ja-JP", "APS排程ID_jp", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsscheduleitem.apsscheduleid
            new TranslationSeedItem("entity.apsscheduleitem.apsscheduleid", "zh-CN", "APS排程ID", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsscheduleitem.apsscheduleid
            new TranslationSeedItem("entity.apsscheduleitem.apsscheduleid", "zh-HK", "APS排程ID_hk", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.apsscheduleitem.apsschedulecode
            new TranslationSeedItem("entity.apsscheduleitem.apsschedulecode", "en-US", "APS排程编码_us", "APS排程编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.apsscheduleitem.apsschedulecode
            new TranslationSeedItem("entity.apsscheduleitem.apsschedulecode", "ja-JP", "APS排程编码_jp", "APS排程编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.apsscheduleitem.apsschedulecode
            new TranslationSeedItem("entity.apsscheduleitem.apsschedulecode", "zh-CN", "APS排程编码", "APS排程编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.apsscheduleitem.apsschedulecode
            new TranslationSeedItem("entity.apsscheduleitem.apsschedulecode", "zh-HK", "APS排程编码_hk", "APS排程编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.apsscheduleitem.apsorderid
            new TranslationSeedItem("entity.apsscheduleitem.apsorderid", "en-US", "APS订单ID_us", "APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsorderid
            new TranslationSeedItem("entity.apsscheduleitem.apsorderid", "ja-JP", "APS订单ID_jp", "APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsorderid
            new TranslationSeedItem("entity.apsscheduleitem.apsorderid", "zh-CN", "APS订单ID", "APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsorderid
            new TranslationSeedItem("entity.apsscheduleitem.apsorderid", "zh-HK", "APS订单ID_hk", "APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）"),

            // entity.apsscheduleitem.apsoperationid
            new TranslationSeedItem("entity.apsscheduleitem.apsoperationid", "en-US", "APS工序排程ID_us", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsoperationid
            new TranslationSeedItem("entity.apsscheduleitem.apsoperationid", "ja-JP", "APS工序排程ID_jp", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsoperationid
            new TranslationSeedItem("entity.apsscheduleitem.apsoperationid", "zh-CN", "APS工序排程ID", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.apsscheduleitem.apsoperationid
            new TranslationSeedItem("entity.apsscheduleitem.apsoperationid", "zh-HK", "APS工序排程ID_hk", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),

            // entity.apsscheduleitem.routingitemid
            new TranslationSeedItem("entity.apsscheduleitem.routingitemid", "en-US", "工艺路线工序ID_us", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsscheduleitem.routingitemid
            new TranslationSeedItem("entity.apsscheduleitem.routingitemid", "ja-JP", "工艺路线工序ID_jp", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsscheduleitem.routingitemid
            new TranslationSeedItem("entity.apsscheduleitem.routingitemid", "zh-CN", "工艺路线工序ID", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsscheduleitem.routingitemid
            new TranslationSeedItem("entity.apsscheduleitem.routingitemid", "zh-HK", "工艺路线工序ID_hk", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),

            // entity.apsscheduleitem.linenumber
            new TranslationSeedItem("entity.apsscheduleitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.apsscheduleitem.linenumber
            new TranslationSeedItem("entity.apsscheduleitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.apsscheduleitem.linenumber
            new TranslationSeedItem("entity.apsscheduleitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.apsscheduleitem.linenumber
            new TranslationSeedItem("entity.apsscheduleitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.apsscheduleitem.workordercode
            new TranslationSeedItem("entity.apsscheduleitem.workordercode", "en-US", "生产工单编码_us", "生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.apsscheduleitem.workordercode
            new TranslationSeedItem("entity.apsscheduleitem.workordercode", "ja-JP", "生产工单编码_jp", "生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.apsscheduleitem.workordercode
            new TranslationSeedItem("entity.apsscheduleitem.workordercode", "zh-CN", "生产工单编码", "生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.apsscheduleitem.workordercode
            new TranslationSeedItem("entity.apsscheduleitem.workordercode", "zh-HK", "生产工单编码_hk", "生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),

            // entity.apsscheduleitem.productcode
            new TranslationSeedItem("entity.apsscheduleitem.productcode", "en-US", "产品编码_us", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsscheduleitem.productcode
            new TranslationSeedItem("entity.apsscheduleitem.productcode", "ja-JP", "产品编码_jp", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsscheduleitem.productcode
            new TranslationSeedItem("entity.apsscheduleitem.productcode", "zh-CN", "产品编码", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsscheduleitem.productcode
            new TranslationSeedItem("entity.apsscheduleitem.productcode", "zh-HK", "产品编码_hk", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.apsscheduleitem.productname
            new TranslationSeedItem("entity.apsscheduleitem.productname", "en-US", "产品名称_us", "产品名称"),
            // entity.apsscheduleitem.productname
            new TranslationSeedItem("entity.apsscheduleitem.productname", "ja-JP", "产品名称_jp", "产品名称"),
            // entity.apsscheduleitem.productname
            new TranslationSeedItem("entity.apsscheduleitem.productname", "zh-CN", "产品名称", "产品名称"),
            // entity.apsscheduleitem.productname
            new TranslationSeedItem("entity.apsscheduleitem.productname", "zh-HK", "产品名称_hk", "产品名称"),

            // entity.apsscheduleitem.workcentercode
            new TranslationSeedItem("entity.apsscheduleitem.workcentercode", "en-US", "工作中心编码_us", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsscheduleitem.workcentercode
            new TranslationSeedItem("entity.apsscheduleitem.workcentercode", "ja-JP", "工作中心编码_jp", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsscheduleitem.workcentercode
            new TranslationSeedItem("entity.apsscheduleitem.workcentercode", "zh-CN", "工作中心编码", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsscheduleitem.workcentercode
            new TranslationSeedItem("entity.apsscheduleitem.workcentercode", "zh-HK", "工作中心编码_hk", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.apsscheduleitem.workcenterdescription
            new TranslationSeedItem("entity.apsscheduleitem.workcenterdescription", "en-US", "工作中心描述_us", "工作中心描述"),
            // entity.apsscheduleitem.workcenterdescription
            new TranslationSeedItem("entity.apsscheduleitem.workcenterdescription", "ja-JP", "工作中心描述_jp", "工作中心描述"),
            // entity.apsscheduleitem.workcenterdescription
            new TranslationSeedItem("entity.apsscheduleitem.workcenterdescription", "zh-CN", "工作中心描述", "工作中心描述"),
            // entity.apsscheduleitem.workcenterdescription
            new TranslationSeedItem("entity.apsscheduleitem.workcenterdescription", "zh-HK", "工作中心描述_hk", "工作中心描述"),

            // entity.apsscheduleitem.processcode
            new TranslationSeedItem("entity.apsscheduleitem.processcode", "en-US", "工序编码_us", "工序编码"),
            // entity.apsscheduleitem.processcode
            new TranslationSeedItem("entity.apsscheduleitem.processcode", "ja-JP", "工序编码_jp", "工序编码"),
            // entity.apsscheduleitem.processcode
            new TranslationSeedItem("entity.apsscheduleitem.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.apsscheduleitem.processcode
            new TranslationSeedItem("entity.apsscheduleitem.processcode", "zh-HK", "工序编码_hk", "工序编码"),

            // entity.apsscheduleitem.processname
            new TranslationSeedItem("entity.apsscheduleitem.processname", "en-US", "工序名称_us", "工序名称"),
            // entity.apsscheduleitem.processname
            new TranslationSeedItem("entity.apsscheduleitem.processname", "ja-JP", "工序名称_jp", "工序名称"),
            // entity.apsscheduleitem.processname
            new TranslationSeedItem("entity.apsscheduleitem.processname", "zh-CN", "工序名称", "工序名称"),
            // entity.apsscheduleitem.processname
            new TranslationSeedItem("entity.apsscheduleitem.processname", "zh-HK", "工序名称_hk", "工序名称"),

            // entity.apsscheduleitem.processsequence
            new TranslationSeedItem("entity.apsscheduleitem.processsequence", "en-US", "工序序号_us", "工序序号"),
            // entity.apsscheduleitem.processsequence
            new TranslationSeedItem("entity.apsscheduleitem.processsequence", "ja-JP", "工序序号_jp", "工序序号"),
            // entity.apsscheduleitem.processsequence
            new TranslationSeedItem("entity.apsscheduleitem.processsequence", "zh-CN", "工序序号", "工序序号"),
            // entity.apsscheduleitem.processsequence
            new TranslationSeedItem("entity.apsscheduleitem.processsequence", "zh-HK", "工序序号_hk", "工序序号"),

            // entity.apsscheduleitem.processstandardst
            new TranslationSeedItem("entity.apsscheduleitem.processstandardst", "en-US", "工序标准ST值_us", "工序标准ST值"),
            // entity.apsscheduleitem.processstandardst
            new TranslationSeedItem("entity.apsscheduleitem.processstandardst", "ja-JP", "工序标准ST值_jp", "工序标准ST值"),
            // entity.apsscheduleitem.processstandardst
            new TranslationSeedItem("entity.apsscheduleitem.processstandardst", "zh-CN", "工序标准ST值", "工序标准ST值"),
            // entity.apsscheduleitem.processstandardst
            new TranslationSeedItem("entity.apsscheduleitem.processstandardst", "zh-HK", "工序标准ST值_hk", "工序标准ST值"),

            // entity.apsscheduleitem.processstandardstunit
            new TranslationSeedItem("entity.apsscheduleitem.processstandardstunit", "en-US", "工序标准ST单位_us", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsscheduleitem.processstandardstunit
            new TranslationSeedItem("entity.apsscheduleitem.processstandardstunit", "ja-JP", "工序标准ST单位_jp", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsscheduleitem.processstandardstunit
            new TranslationSeedItem("entity.apsscheduleitem.processstandardstunit", "zh-CN", "工序标准ST单位", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsscheduleitem.processstandardstunit
            new TranslationSeedItem("entity.apsscheduleitem.processstandardstunit", "zh-HK", "工序标准ST单位_hk", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),

            // entity.apsscheduleitem.extraminutes
            new TranslationSeedItem("entity.apsscheduleitem.extraminutes", "en-US", "额外时间_us", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsscheduleitem.extraminutes
            new TranslationSeedItem("entity.apsscheduleitem.extraminutes", "ja-JP", "额外时间_jp", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsscheduleitem.extraminutes
            new TranslationSeedItem("entity.apsscheduleitem.extraminutes", "zh-CN", "额外时间", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsscheduleitem.extraminutes
            new TranslationSeedItem("entity.apsscheduleitem.extraminutes", "zh-HK", "额外时间_hk", "额外时间（分钟），如换模、调试、清洁等准备时间"),

            // entity.apsscheduleitem.planquantity
            new TranslationSeedItem("entity.apsscheduleitem.planquantity", "en-US", "计划数量_us", "计划数量"),
            // entity.apsscheduleitem.planquantity
            new TranslationSeedItem("entity.apsscheduleitem.planquantity", "ja-JP", "计划数量_jp", "计划数量"),
            // entity.apsscheduleitem.planquantity
            new TranslationSeedItem("entity.apsscheduleitem.planquantity", "zh-CN", "计划数量", "计划数量"),
            // entity.apsscheduleitem.planquantity
            new TranslationSeedItem("entity.apsscheduleitem.planquantity", "zh-HK", "计划数量_hk", "计划数量"),

            // entity.apsscheduleitem.planstarttime
            new TranslationSeedItem("entity.apsscheduleitem.planstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.apsscheduleitem.planstarttime
            new TranslationSeedItem("entity.apsscheduleitem.planstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.apsscheduleitem.planstarttime
            new TranslationSeedItem("entity.apsscheduleitem.planstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsscheduleitem.planstarttime
            new TranslationSeedItem("entity.apsscheduleitem.planstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.apsscheduleitem.planendtime
            new TranslationSeedItem("entity.apsscheduleitem.planendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.apsscheduleitem.planendtime
            new TranslationSeedItem("entity.apsscheduleitem.planendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.apsscheduleitem.planendtime
            new TranslationSeedItem("entity.apsscheduleitem.planendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsscheduleitem.planendtime
            new TranslationSeedItem("entity.apsscheduleitem.planendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.apsscheduleitem.actualstarttime
            new TranslationSeedItem("entity.apsscheduleitem.actualstarttime", "en-US", "实际开始时间_us", "实际开始时间"),
            // entity.apsscheduleitem.actualstarttime
            new TranslationSeedItem("entity.apsscheduleitem.actualstarttime", "ja-JP", "实际开始时间_jp", "实际开始时间"),
            // entity.apsscheduleitem.actualstarttime
            new TranslationSeedItem("entity.apsscheduleitem.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.apsscheduleitem.actualstarttime
            new TranslationSeedItem("entity.apsscheduleitem.actualstarttime", "zh-HK", "实际开始时间_hk", "实际开始时间"),

            // entity.apsscheduleitem.actualendtime
            new TranslationSeedItem("entity.apsscheduleitem.actualendtime", "en-US", "实际结束时间_us", "实际结束时间"),
            // entity.apsscheduleitem.actualendtime
            new TranslationSeedItem("entity.apsscheduleitem.actualendtime", "ja-JP", "实际结束时间_jp", "实际结束时间"),
            // entity.apsscheduleitem.actualendtime
            new TranslationSeedItem("entity.apsscheduleitem.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.apsscheduleitem.actualendtime
            new TranslationSeedItem("entity.apsscheduleitem.actualendtime", "zh-HK", "实际结束时间_hk", "实际结束时间"),

            // entity.apsscheduleitem.processstatus
            new TranslationSeedItem("entity.apsscheduleitem.processstatus", "en-US", "工序状态_us", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsscheduleitem.processstatus
            new TranslationSeedItem("entity.apsscheduleitem.processstatus", "ja-JP", "工序状态_jp", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsscheduleitem.processstatus
            new TranslationSeedItem("entity.apsscheduleitem.processstatus", "zh-CN", "工序状态", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsscheduleitem.processstatus
            new TranslationSeedItem("entity.apsscheduleitem.processstatus", "zh-HK", "工序状态_hk", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),

            // entity.apsscheduleitem.priority
            new TranslationSeedItem("entity.apsscheduleitem.priority", "en-US", "优先级_us", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsscheduleitem.priority
            new TranslationSeedItem("entity.apsscheduleitem.priority", "ja-JP", "优先级_jp", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsscheduleitem.priority
            new TranslationSeedItem("entity.apsscheduleitem.priority", "zh-CN", "优先级", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsscheduleitem.priority
            new TranslationSeedItem("entity.apsscheduleitem.priority", "zh-HK", "优先级_hk", "优先级（0=普通，1=紧急，2=特急）"),

            // entity.apsscheduleitem.isobsolete
            new TranslationSeedItem("entity.apsscheduleitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsscheduleitem.isobsolete
            new TranslationSeedItem("entity.apsscheduleitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsscheduleitem.isobsolete
            new TranslationSeedItem("entity.apsscheduleitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsscheduleitem.isobsolete
            new TranslationSeedItem("entity.apsscheduleitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.apsscheduleitem.schedule
            new TranslationSeedItem("entity.apsscheduleitem.schedule", "en-US", "APS排程主表_us", "APS排程主表（主表）"),
            // entity.apsscheduleitem.schedule
            new TranslationSeedItem("entity.apsscheduleitem.schedule", "ja-JP", "APS排程主表_jp", "APS排程主表（主表）"),
            // entity.apsscheduleitem.schedule
            new TranslationSeedItem("entity.apsscheduleitem.schedule", "zh-CN", "APS排程主表", "APS排程主表（主表）"),
            // entity.apsscheduleitem.schedule
            new TranslationSeedItem("entity.apsscheduleitem.schedule", "zh-HK", "APS排程主表_hk", "APS排程主表（主表）"),
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
