// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenanceWorkOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance;

/// <summary>
/// TaktMaintenanceWorkOrder 实体国际化翻译种子（键前缀 entity.maintenanceworkorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceWorkOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenanceWorkOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenanceworkorder 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceWorkOrderTranslations())
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

        TaktLogger.Information("TaktMaintenanceWorkOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenanceWorkOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenanceworkorder._self / entity.maintenanceworkorder.{{field}}；ResourceGroup=Maintenance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceWorkOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenanceworkorder._self
            new TranslationSeedItem("entity.maintenanceworkorder._self", "en-US", "Maintenance Work Order Information_us", "实体名称"),
            // entity.maintenanceworkorder._self
            new TranslationSeedItem("entity.maintenanceworkorder._self", "ja-JP", "维护工单信息_jp", "实体名称"),
            // entity.maintenanceworkorder._self
            new TranslationSeedItem("entity.maintenanceworkorder._self", "zh-CN", "维护工单信息", "实体名称"),
            // entity.maintenanceworkorder._self
            new TranslationSeedItem("entity.maintenanceworkorder._self", "zh-HK", "维护工单信息_hk", "实体名称"),

            // entity.maintenanceworkorder.workordercode
            new TranslationSeedItem("entity.maintenanceworkorder.workordercode", "en-US", "维护工单号_us", "维护工单号（前端表单选择编码规则后自动通过 TaktNumbering 维护工单号规则生成并展示，非手输；单据类型菜单：维护工单）"),
            // entity.maintenanceworkorder.workordercode
            new TranslationSeedItem("entity.maintenanceworkorder.workordercode", "ja-JP", "维护工单号_jp", "维护工单号（前端表单选择编码规则后自动通过 TaktNumbering 维护工单号规则生成并展示，非手输；单据类型菜单：维护工单）"),
            // entity.maintenanceworkorder.workordercode
            new TranslationSeedItem("entity.maintenanceworkorder.workordercode", "zh-CN", "维护工单号", "维护工单号（前端表单选择编码规则后自动通过 TaktNumbering 维护工单号规则生成并展示，非手输；单据类型菜单：维护工单）"),
            // entity.maintenanceworkorder.workordercode
            new TranslationSeedItem("entity.maintenanceworkorder.workordercode", "zh-HK", "维护工单号_hk", "维护工单号（前端表单选择编码规则后自动通过 TaktNumbering 维护工单号规则生成并展示，非手输；单据类型菜单：维护工单）"),

            // entity.maintenanceworkorder.maintenancenotificationid
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotificationid", "en-US", "来源维护通知单ID_us", "来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.maintenancenotificationid
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotificationid", "ja-JP", "来源维护通知单ID_jp", "来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.maintenancenotificationid
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotificationid", "zh-CN", "来源维护通知单ID", "来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.maintenancenotificationid
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotificationid", "zh-HK", "来源维护通知单ID_hk", "来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorder.notificationcode
            new TranslationSeedItem("entity.maintenanceworkorder.notificationcode", "en-US", "来源通知单号_us", "来源通知单号（冗余）"),
            // entity.maintenanceworkorder.notificationcode
            new TranslationSeedItem("entity.maintenanceworkorder.notificationcode", "ja-JP", "来源通知单号_jp", "来源通知单号（冗余）"),
            // entity.maintenanceworkorder.notificationcode
            new TranslationSeedItem("entity.maintenanceworkorder.notificationcode", "zh-CN", "来源通知单号", "来源通知单号（冗余）"),
            // entity.maintenanceworkorder.notificationcode
            new TranslationSeedItem("entity.maintenanceworkorder.notificationcode", "zh-HK", "来源通知单号_hk", "来源通知单号（冗余）"),

            // entity.maintenanceworkorder.equipmentid
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentid", "en-US", "设备ID_us", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.equipmentid
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentid", "ja-JP", "设备ID_jp", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.equipmentid
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentid", "zh-CN", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.equipmentid
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentid", "zh-HK", "设备ID_hk", "设备ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorder.equipcode
            new TranslationSeedItem("entity.maintenanceworkorder.equipcode", "en-US", "设备编码_us", "设备编码（冗余）"),
            // entity.maintenanceworkorder.equipcode
            new TranslationSeedItem("entity.maintenanceworkorder.equipcode", "ja-JP", "设备编码_jp", "设备编码（冗余）"),
            // entity.maintenanceworkorder.equipcode
            new TranslationSeedItem("entity.maintenanceworkorder.equipcode", "zh-CN", "设备编码", "设备编码（冗余）"),
            // entity.maintenanceworkorder.equipcode
            new TranslationSeedItem("entity.maintenanceworkorder.equipcode", "zh-HK", "设备编码_hk", "设备编码（冗余）"),

            // entity.maintenanceworkorder.equipmentname
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentname", "en-US", "设备名称_us", "设备名称（冗余）"),
            // entity.maintenanceworkorder.equipmentname
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentname", "ja-JP", "设备名称_jp", "设备名称（冗余）"),
            // entity.maintenanceworkorder.equipmentname
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentname", "zh-CN", "设备名称", "设备名称（冗余）"),
            // entity.maintenanceworkorder.equipmentname
            new TranslationSeedItem("entity.maintenanceworkorder.equipmentname", "zh-HK", "设备名称_hk", "设备名称（冗余）"),

            // entity.maintenanceworkorder.maintenancecategory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecategory", "en-US", "维护类别_us", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenanceworkorder.maintenancecategory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecategory", "ja-JP", "维护类别_jp", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenanceworkorder.maintenancecategory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecategory", "zh-CN", "维护类别", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenanceworkorder.maintenancecategory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecategory", "zh-HK", "维护类别_hk", "维护类别（字典 logistics_maintenance_category）"),

            // entity.maintenanceworkorder.maintenancetype
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancetype", "en-US", "维护类型_us", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenanceworkorder.maintenancetype
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancetype", "ja-JP", "维护类型_jp", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenanceworkorder.maintenancetype
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancetype", "zh-CN", "维护类型", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenanceworkorder.maintenancetype
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancetype", "zh-HK", "维护类型_hk", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),

            // entity.maintenanceworkorder.workorderstatus
            new TranslationSeedItem("entity.maintenanceworkorder.workorderstatus", "en-US", "工单状态_us", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.maintenanceworkorder.workorderstatus
            new TranslationSeedItem("entity.maintenanceworkorder.workorderstatus", "ja-JP", "工单状态_jp", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.maintenanceworkorder.workorderstatus
            new TranslationSeedItem("entity.maintenanceworkorder.workorderstatus", "zh-CN", "工单状态", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.maintenanceworkorder.workorderstatus
            new TranslationSeedItem("entity.maintenanceworkorder.workorderstatus", "zh-HK", "工单状态_hk", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),

            // entity.maintenanceworkorder.priority
            new TranslationSeedItem("entity.maintenanceworkorder.priority", "en-US", "优先级_us", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenanceworkorder.priority
            new TranslationSeedItem("entity.maintenanceworkorder.priority", "ja-JP", "优先级_jp", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenanceworkorder.priority
            new TranslationSeedItem("entity.maintenanceworkorder.priority", "zh-CN", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenanceworkorder.priority
            new TranslationSeedItem("entity.maintenanceworkorder.priority", "zh-HK", "优先级_hk", "优先级（1=低，2=中，3=高，4=紧急）"),

            // entity.maintenanceworkorder.workcenter
            new TranslationSeedItem("entity.maintenanceworkorder.workcenter", "en-US", "工作中心_us", "工作中心"),
            // entity.maintenanceworkorder.workcenter
            new TranslationSeedItem("entity.maintenanceworkorder.workcenter", "ja-JP", "工作中心_jp", "工作中心"),
            // entity.maintenanceworkorder.workcenter
            new TranslationSeedItem("entity.maintenanceworkorder.workcenter", "zh-CN", "工作中心", "工作中心"),
            // entity.maintenanceworkorder.workcenter
            new TranslationSeedItem("entity.maintenanceworkorder.workcenter", "zh-HK", "工作中心_hk", "工作中心"),

            // entity.maintenanceworkorder.assignedtechnician
            new TranslationSeedItem("entity.maintenanceworkorder.assignedtechnician", "en-US", "指派技师_us", "指派技师（人员编码）"),
            // entity.maintenanceworkorder.assignedtechnician
            new TranslationSeedItem("entity.maintenanceworkorder.assignedtechnician", "ja-JP", "指派技师_jp", "指派技师（人员编码）"),
            // entity.maintenanceworkorder.assignedtechnician
            new TranslationSeedItem("entity.maintenanceworkorder.assignedtechnician", "zh-CN", "指派技师", "指派技师（人员编码）"),
            // entity.maintenanceworkorder.assignedtechnician
            new TranslationSeedItem("entity.maintenanceworkorder.assignedtechnician", "zh-HK", "指派技师_hk", "指派技师（人员编码）"),

            // entity.maintenanceworkorder.maintenancecompany
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecompany", "en-US", "维护单位_us", "维护单位"),
            // entity.maintenanceworkorder.maintenancecompany
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecompany", "ja-JP", "维护单位_jp", "维护单位"),
            // entity.maintenanceworkorder.maintenancecompany
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecompany", "zh-CN", "维护单位", "维护单位"),
            // entity.maintenanceworkorder.maintenancecompany
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecompany", "zh-HK", "维护单位_hk", "维护单位"),

            // entity.maintenanceworkorder.plannedstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.maintenanceworkorder.plannedstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.maintenanceworkorder.plannedstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.maintenanceworkorder.plannedstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.maintenanceworkorder.plannedendtime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.maintenanceworkorder.plannedendtime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.maintenanceworkorder.plannedendtime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.maintenanceworkorder.plannedendtime
            new TranslationSeedItem("entity.maintenanceworkorder.plannedendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.maintenanceworkorder.actualstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.actualstarttime", "en-US", "实际开始时间_us", "实际开始时间"),
            // entity.maintenanceworkorder.actualstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.actualstarttime", "ja-JP", "实际开始时间_jp", "实际开始时间"),
            // entity.maintenanceworkorder.actualstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.maintenanceworkorder.actualstarttime
            new TranslationSeedItem("entity.maintenanceworkorder.actualstarttime", "zh-HK", "实际开始时间_hk", "实际开始时间"),

            // entity.maintenanceworkorder.actualendtime
            new TranslationSeedItem("entity.maintenanceworkorder.actualendtime", "en-US", "实际结束时间_us", "实际结束时间"),
            // entity.maintenanceworkorder.actualendtime
            new TranslationSeedItem("entity.maintenanceworkorder.actualendtime", "ja-JP", "实际结束时间_jp", "实际结束时间"),
            // entity.maintenanceworkorder.actualendtime
            new TranslationSeedItem("entity.maintenanceworkorder.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.maintenanceworkorder.actualendtime
            new TranslationSeedItem("entity.maintenanceworkorder.actualendtime", "zh-HK", "实际结束时间_hk", "实际结束时间"),

            // entity.maintenanceworkorder.faultdescription
            new TranslationSeedItem("entity.maintenanceworkorder.faultdescription", "en-US", "故障描述_us", "故障描述"),
            // entity.maintenanceworkorder.faultdescription
            new TranslationSeedItem("entity.maintenanceworkorder.faultdescription", "ja-JP", "故障描述_jp", "故障描述"),
            // entity.maintenanceworkorder.faultdescription
            new TranslationSeedItem("entity.maintenanceworkorder.faultdescription", "zh-CN", "故障描述", "故障描述"),
            // entity.maintenanceworkorder.faultdescription
            new TranslationSeedItem("entity.maintenanceworkorder.faultdescription", "zh-HK", "故障描述_hk", "故障描述"),

            // entity.maintenanceworkorder.maintenancecontent
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecontent", "en-US", "维护内容_us", "维护内容"),
            // entity.maintenanceworkorder.maintenancecontent
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecontent", "ja-JP", "维护内容_jp", "维护内容"),
            // entity.maintenanceworkorder.maintenancecontent
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecontent", "zh-CN", "维护内容", "维护内容"),
            // entity.maintenanceworkorder.maintenancecontent
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecontent", "zh-HK", "维护内容_hk", "维护内容"),

            // entity.maintenanceworkorder.solution
            new TranslationSeedItem("entity.maintenanceworkorder.solution", "en-US", "处理方案_us", "处理方案"),
            // entity.maintenanceworkorder.solution
            new TranslationSeedItem("entity.maintenanceworkorder.solution", "ja-JP", "处理方案_jp", "处理方案"),
            // entity.maintenanceworkorder.solution
            new TranslationSeedItem("entity.maintenanceworkorder.solution", "zh-CN", "处理方案", "处理方案"),
            // entity.maintenanceworkorder.solution
            new TranslationSeedItem("entity.maintenanceworkorder.solution", "zh-HK", "处理方案_hk", "处理方案"),

            // entity.maintenanceworkorder.costcenterid
            new TranslationSeedItem("entity.maintenanceworkorder.costcenterid", "en-US", "结算成本中心ID_us", "结算成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costcenterid
            new TranslationSeedItem("entity.maintenanceworkorder.costcenterid", "ja-JP", "结算成本中心ID_jp", "结算成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costcenterid
            new TranslationSeedItem("entity.maintenanceworkorder.costcenterid", "zh-CN", "结算成本中心ID", "结算成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costcenterid
            new TranslationSeedItem("entity.maintenanceworkorder.costcenterid", "zh-HK", "结算成本中心ID_hk", "结算成本中心ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorder.costcentercode
            new TranslationSeedItem("entity.maintenanceworkorder.costcentercode", "en-US", "结算成本中心编码_us", "结算成本中心编码（冗余）"),
            // entity.maintenanceworkorder.costcentercode
            new TranslationSeedItem("entity.maintenanceworkorder.costcentercode", "ja-JP", "结算成本中心编码_jp", "结算成本中心编码（冗余）"),
            // entity.maintenanceworkorder.costcentercode
            new TranslationSeedItem("entity.maintenanceworkorder.costcentercode", "zh-CN", "结算成本中心编码", "结算成本中心编码（冗余）"),
            // entity.maintenanceworkorder.costcentercode
            new TranslationSeedItem("entity.maintenanceworkorder.costcentercode", "zh-HK", "结算成本中心编码_hk", "结算成本中心编码（冗余）"),

            // entity.maintenanceworkorder.costelementid
            new TranslationSeedItem("entity.maintenanceworkorder.costelementid", "en-US", "成本要素ID_us", "成本要素ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costelementid
            new TranslationSeedItem("entity.maintenanceworkorder.costelementid", "ja-JP", "成本要素ID_jp", "成本要素ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costelementid
            new TranslationSeedItem("entity.maintenanceworkorder.costelementid", "zh-CN", "成本要素ID", "成本要素ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkorder.costelementid
            new TranslationSeedItem("entity.maintenanceworkorder.costelementid", "zh-HK", "成本要素ID_hk", "成本要素ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkorder.costelementcode
            new TranslationSeedItem("entity.maintenanceworkorder.costelementcode", "en-US", "成本要素编码_us", "成本要素编码（冗余）"),
            // entity.maintenanceworkorder.costelementcode
            new TranslationSeedItem("entity.maintenanceworkorder.costelementcode", "ja-JP", "成本要素编码_jp", "成本要素编码（冗余）"),
            // entity.maintenanceworkorder.costelementcode
            new TranslationSeedItem("entity.maintenanceworkorder.costelementcode", "zh-CN", "成本要素编码", "成本要素编码（冗余）"),
            // entity.maintenanceworkorder.costelementcode
            new TranslationSeedItem("entity.maintenanceworkorder.costelementcode", "zh-HK", "成本要素编码_hk", "成本要素编码（冗余）"),

            // entity.maintenanceworkorder.totalmaterialcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalmaterialcost", "en-US", "材料成本合计_us", "材料成本合计"),
            // entity.maintenanceworkorder.totalmaterialcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalmaterialcost", "ja-JP", "材料成本合计_jp", "材料成本合计"),
            // entity.maintenanceworkorder.totalmaterialcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalmaterialcost", "zh-CN", "材料成本合计", "材料成本合计"),
            // entity.maintenanceworkorder.totalmaterialcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalmaterialcost", "zh-HK", "材料成本合计_hk", "材料成本合计"),

            // entity.maintenanceworkorder.totallaborcost
            new TranslationSeedItem("entity.maintenanceworkorder.totallaborcost", "en-US", "人工成本合计_us", "人工成本合计"),
            // entity.maintenanceworkorder.totallaborcost
            new TranslationSeedItem("entity.maintenanceworkorder.totallaborcost", "ja-JP", "人工成本合计_jp", "人工成本合计"),
            // entity.maintenanceworkorder.totallaborcost
            new TranslationSeedItem("entity.maintenanceworkorder.totallaborcost", "zh-CN", "人工成本合计", "人工成本合计"),
            // entity.maintenanceworkorder.totallaborcost
            new TranslationSeedItem("entity.maintenanceworkorder.totallaborcost", "zh-HK", "人工成本合计_hk", "人工成本合计"),

            // entity.maintenanceworkorder.totalothercost
            new TranslationSeedItem("entity.maintenanceworkorder.totalothercost", "en-US", "其他成本合计_us", "其他成本合计"),
            // entity.maintenanceworkorder.totalothercost
            new TranslationSeedItem("entity.maintenanceworkorder.totalothercost", "ja-JP", "其他成本合计_jp", "其他成本合计"),
            // entity.maintenanceworkorder.totalothercost
            new TranslationSeedItem("entity.maintenanceworkorder.totalothercost", "zh-CN", "其他成本合计", "其他成本合计"),
            // entity.maintenanceworkorder.totalothercost
            new TranslationSeedItem("entity.maintenanceworkorder.totalothercost", "zh-HK", "其他成本合计_hk", "其他成本合计"),

            // entity.maintenanceworkorder.totalcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalcost", "en-US", "总成本_us", "总成本"),
            // entity.maintenanceworkorder.totalcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalcost", "ja-JP", "总成本_jp", "总成本"),
            // entity.maintenanceworkorder.totalcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalcost", "zh-CN", "总成本", "总成本"),
            // entity.maintenanceworkorder.totalcost
            new TranslationSeedItem("entity.maintenanceworkorder.totalcost", "zh-HK", "总成本_hk", "总成本"),

            // entity.maintenanceworkorder.settlementstatus
            new TranslationSeedItem("entity.maintenanceworkorder.settlementstatus", "en-US", "结算状态_us", "结算状态（0=未结算，1=部分结算，2=已结算）"),
            // entity.maintenanceworkorder.settlementstatus
            new TranslationSeedItem("entity.maintenanceworkorder.settlementstatus", "ja-JP", "结算状态_jp", "结算状态（0=未结算，1=部分结算，2=已结算）"),
            // entity.maintenanceworkorder.settlementstatus
            new TranslationSeedItem("entity.maintenanceworkorder.settlementstatus", "zh-CN", "结算状态", "结算状态（0=未结算，1=部分结算，2=已结算）"),
            // entity.maintenanceworkorder.settlementstatus
            new TranslationSeedItem("entity.maintenanceworkorder.settlementstatus", "zh-HK", "结算状态_hk", "结算状态（0=未结算，1=部分结算，2=已结算）"),

            // entity.maintenanceworkorder.settlementtime
            new TranslationSeedItem("entity.maintenanceworkorder.settlementtime", "en-US", "结算时间_us", "结算时间"),
            // entity.maintenanceworkorder.settlementtime
            new TranslationSeedItem("entity.maintenanceworkorder.settlementtime", "ja-JP", "结算时间_jp", "结算时间"),
            // entity.maintenanceworkorder.settlementtime
            new TranslationSeedItem("entity.maintenanceworkorder.settlementtime", "zh-CN", "结算时间", "结算时间"),
            // entity.maintenanceworkorder.settlementtime
            new TranslationSeedItem("entity.maintenanceworkorder.settlementtime", "zh-HK", "结算时间_hk", "结算时间"),

            // entity.maintenanceworkorder.completedat
            new TranslationSeedItem("entity.maintenanceworkorder.completedat", "en-US", "完工时间_us", "完工时间"),
            // entity.maintenanceworkorder.completedat
            new TranslationSeedItem("entity.maintenanceworkorder.completedat", "ja-JP", "完工时间_jp", "完工时间"),
            // entity.maintenanceworkorder.completedat
            new TranslationSeedItem("entity.maintenanceworkorder.completedat", "zh-CN", "完工时间", "完工时间"),
            // entity.maintenanceworkorder.completedat
            new TranslationSeedItem("entity.maintenanceworkorder.completedat", "zh-HK", "完工时间_hk", "完工时间"),

            // entity.maintenanceworkorder.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeeid", "en-US", "验收人员工ID_us", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenanceworkorder.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeeid", "ja-JP", "验收人员工ID_jp", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenanceworkorder.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeeid", "zh-CN", "验收人员工ID", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenanceworkorder.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeeid", "zh-HK", "验收人员工ID_hk", "验收人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.maintenanceworkorder.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeename", "en-US", "验收人名称_us", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenanceworkorder.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeename", "ja-JP", "验收人名称_jp", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenanceworkorder.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeename", "zh-CN", "验收人名称", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenanceworkorder.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedbyemployeename", "zh-HK", "验收人名称_hk", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.maintenanceworkorder.acceptedat
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedat", "en-US", "验收时间_us", "验收时间"),
            // entity.maintenanceworkorder.acceptedat
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedat", "ja-JP", "验收时间_jp", "验收时间"),
            // entity.maintenanceworkorder.acceptedat
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.maintenanceworkorder.acceptedat
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedat", "zh-HK", "验收时间_hk", "验收时间"),

            // entity.maintenanceworkorder.maintenanceresult
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceresult", "en-US", "维护结果_us", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenanceworkorder.maintenanceresult
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceresult", "ja-JP", "维护结果_jp", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenanceworkorder.maintenanceresult
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceresult", "zh-CN", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenanceworkorder.maintenanceresult
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceresult", "zh-HK", "维护结果_hk", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),

            // entity.maintenanceworkorder.nextmaintenancedate
            new TranslationSeedItem("entity.maintenanceworkorder.nextmaintenancedate", "en-US", "下次维护日期_us", "下次维护日期"),
            // entity.maintenanceworkorder.nextmaintenancedate
            new TranslationSeedItem("entity.maintenanceworkorder.nextmaintenancedate", "ja-JP", "下次维护日期_jp", "下次维护日期"),
            // entity.maintenanceworkorder.nextmaintenancedate
            new TranslationSeedItem("entity.maintenanceworkorder.nextmaintenancedate", "zh-CN", "下次维护日期", "下次维护日期"),
            // entity.maintenanceworkorder.nextmaintenancedate
            new TranslationSeedItem("entity.maintenanceworkorder.nextmaintenancedate", "zh-HK", "下次维护日期_hk", "下次维护日期"),

            // entity.maintenanceworkorder.maintenancecycledays
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecycledays", "en-US", "维护周期（天）_us", "维护周期（天）"),
            // entity.maintenanceworkorder.maintenancecycledays
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecycledays", "ja-JP", "维护周期（天）_jp", "维护周期（天）"),
            // entity.maintenanceworkorder.maintenancecycledays
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecycledays", "zh-CN", "维护周期（天）", "维护周期（天）"),
            // entity.maintenanceworkorder.maintenancecycledays
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancecycledays", "zh-HK", "维护周期（天）_hk", "维护周期（天）"),

            // entity.maintenanceworkorder.maintenanceimages
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceimages", "en-US", "维护图片_us", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenanceworkorder.maintenanceimages
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceimages", "ja-JP", "维护图片_jp", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenanceworkorder.maintenanceimages
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceimages", "zh-CN", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenanceworkorder.maintenanceimages
            new TranslationSeedItem("entity.maintenanceworkorder.maintenanceimages", "zh-HK", "维护图片_hk", "维护图片（JSON格式，存储维护图片URL列表）"),

            // entity.maintenanceworkorder.maintenancedocuments
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancedocuments", "en-US", "维护文档_us", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenanceworkorder.maintenancedocuments
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancedocuments", "ja-JP", "维护文档_jp", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenanceworkorder.maintenancedocuments
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancedocuments", "zh-CN", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenanceworkorder.maintenancedocuments
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancedocuments", "zh-HK", "维护文档_hk", "维护文档（JSON格式，存储维护文档ID列表）"),

            // entity.maintenanceworkorder.acceptedsummary
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedsummary", "en-US", "验收总结_us", "验收总结"),
            // entity.maintenanceworkorder.acceptedsummary
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedsummary", "ja-JP", "验收总结_jp", "验收总结"),
            // entity.maintenanceworkorder.acceptedsummary
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedsummary", "zh-CN", "验收总结", "验收总结"),
            // entity.maintenanceworkorder.acceptedsummary
            new TranslationSeedItem("entity.maintenanceworkorder.acceptedsummary", "zh-HK", "验收总结_hk", "验收总结"),

            // entity.maintenanceworkorder.ishistoryarchived
            new TranslationSeedItem("entity.maintenanceworkorder.ishistoryarchived", "en-US", "是否已归档履历_us", "是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）"),
            // entity.maintenanceworkorder.ishistoryarchived
            new TranslationSeedItem("entity.maintenanceworkorder.ishistoryarchived", "ja-JP", "是否已归档履历_jp", "是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）"),
            // entity.maintenanceworkorder.ishistoryarchived
            new TranslationSeedItem("entity.maintenanceworkorder.ishistoryarchived", "zh-CN", "是否已归档履历", "是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）"),
            // entity.maintenanceworkorder.ishistoryarchived
            new TranslationSeedItem("entity.maintenanceworkorder.ishistoryarchived", "zh-HK", "是否已归档履历_hk", "是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）"),

            // entity.maintenanceworkorder.maintenancenotification
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotification", "en-US", "来源维护通知单_us", "来源维护通知单"),
            // entity.maintenanceworkorder.maintenancenotification
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotification", "ja-JP", "来源维护通知单_jp", "来源维护通知单"),
            // entity.maintenanceworkorder.maintenancenotification
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotification", "zh-CN", "来源维护通知单", "来源维护通知单"),
            // entity.maintenanceworkorder.maintenancenotification
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancenotification", "zh-HK", "来源维护通知单_hk", "来源维护通知单"),

            // entity.maintenanceworkorder.equipment
            new TranslationSeedItem("entity.maintenanceworkorder.equipment", "en-US", "设备_us", "设备（主数据）"),
            // entity.maintenanceworkorder.equipment
            new TranslationSeedItem("entity.maintenanceworkorder.equipment", "ja-JP", "设备_jp", "设备（主数据）"),
            // entity.maintenanceworkorder.equipment
            new TranslationSeedItem("entity.maintenanceworkorder.equipment", "zh-CN", "设备", "设备（主数据）"),
            // entity.maintenanceworkorder.equipment
            new TranslationSeedItem("entity.maintenanceworkorder.equipment", "zh-HK", "设备_hk", "设备（主数据）"),

            // entity.maintenanceworkorder.materials
            new TranslationSeedItem("entity.maintenanceworkorder.materials", "en-US", "领料明细_us", "领料明细"),
            // entity.maintenanceworkorder.materials
            new TranslationSeedItem("entity.maintenanceworkorder.materials", "ja-JP", "领料明细_jp", "领料明细"),
            // entity.maintenanceworkorder.materials
            new TranslationSeedItem("entity.maintenanceworkorder.materials", "zh-CN", "领料明细", "领料明细"),
            // entity.maintenanceworkorder.materials
            new TranslationSeedItem("entity.maintenanceworkorder.materials", "zh-HK", "领料明细_hk", "领料明细"),

            // entity.maintenanceworkorder.labors
            new TranslationSeedItem("entity.maintenanceworkorder.labors", "en-US", "报工明细_us", "报工明细"),
            // entity.maintenanceworkorder.labors
            new TranslationSeedItem("entity.maintenanceworkorder.labors", "ja-JP", "报工明细_jp", "报工明细"),
            // entity.maintenanceworkorder.labors
            new TranslationSeedItem("entity.maintenanceworkorder.labors", "zh-CN", "报工明细", "报工明细"),
            // entity.maintenanceworkorder.labors
            new TranslationSeedItem("entity.maintenanceworkorder.labors", "zh-HK", "报工明细_hk", "报工明细"),

            // entity.maintenanceworkorder.maintenancehistory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancehistory", "en-US", "归档后的维护履历_us", "归档后的维护履历（一工单一条）"),
            // entity.maintenanceworkorder.maintenancehistory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancehistory", "ja-JP", "归档后的维护履历_jp", "归档后的维护履历（一工单一条）"),
            // entity.maintenanceworkorder.maintenancehistory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancehistory", "zh-CN", "归档后的维护履历", "归档后的维护履历（一工单一条）"),
            // entity.maintenanceworkorder.maintenancehistory
            new TranslationSeedItem("entity.maintenanceworkorder.maintenancehistory", "zh-HK", "归档后的维护履历_hk", "归档后的维护履历（一工单一条）"),
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
        translation.ResourceGroup = "Maintenance";
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
