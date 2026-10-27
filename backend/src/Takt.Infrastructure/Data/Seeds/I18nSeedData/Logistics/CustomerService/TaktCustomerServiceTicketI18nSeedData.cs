// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktCustomerServiceTicketI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerServiceTicket 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService;

/// <summary>
/// TaktCustomerServiceTicket 实体国际化翻译种子（键前缀 entity.customerserviceticket.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerServiceTicketI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerServiceTicket 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerserviceticket 实体翻译...", tenantCode);

        foreach (var item in GetCustomerServiceTicketTranslations())
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

        TaktLogger.Information("TaktCustomerServiceTicket 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerServiceTicket 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerserviceticket._self / entity.customerserviceticket.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerServiceTicketTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerserviceticket._self
            new TranslationSeedItem("entity.customerserviceticket._self", "en-US", "Customer Service Ticket Information_us", "实体名称"),
            // entity.customerserviceticket._self
            new TranslationSeedItem("entity.customerserviceticket._self", "ja-JP", "服务工单信息_jp", "实体名称"),
            // entity.customerserviceticket._self
            new TranslationSeedItem("entity.customerserviceticket._self", "zh-CN", "服务工单信息", "实体名称"),
            // entity.customerserviceticket._self
            new TranslationSeedItem("entity.customerserviceticket._self", "zh-HK", "服务工单信息_hk", "实体名称"),

            // entity.customerserviceticket.serviceticketcode
            new TranslationSeedItem("entity.customerserviceticket.serviceticketcode", "en-US", "服务工单编码_us", "服务工单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务工单编码规则生成并展示，非手输；单据类型菜单：服务工单）"),
            // entity.customerserviceticket.serviceticketcode
            new TranslationSeedItem("entity.customerserviceticket.serviceticketcode", "ja-JP", "服务工单编码_jp", "服务工单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务工单编码规则生成并展示，非手输；单据类型菜单：服务工单）"),
            // entity.customerserviceticket.serviceticketcode
            new TranslationSeedItem("entity.customerserviceticket.serviceticketcode", "zh-CN", "服务工单编码", "服务工单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务工单编码规则生成并展示，非手输；单据类型菜单：服务工单）"),
            // entity.customerserviceticket.serviceticketcode
            new TranslationSeedItem("entity.customerserviceticket.serviceticketcode", "zh-HK", "服务工单编码_hk", "服务工单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务工单编码规则生成并展示，非手输；单据类型菜单：服务工单）"),

            // entity.customerserviceticket.clientid
            new TranslationSeedItem("entity.customerserviceticket.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.clientid
            new TranslationSeedItem("entity.customerserviceticket.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.clientid
            new TranslationSeedItem("entity.customerserviceticket.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.clientid
            new TranslationSeedItem("entity.customerserviceticket.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceticket.clientcode
            new TranslationSeedItem("entity.customerserviceticket.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientcode
            new TranslationSeedItem("entity.customerserviceticket.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientcode
            new TranslationSeedItem("entity.customerserviceticket.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientcode
            new TranslationSeedItem("entity.customerserviceticket.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceticket.clientname1
            new TranslationSeedItem("entity.customerserviceticket.clientname1", "en-US", "客户端名称1_us", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientname1
            new TranslationSeedItem("entity.customerserviceticket.clientname1", "ja-JP", "客户端名称1_jp", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientname1
            new TranslationSeedItem("entity.customerserviceticket.clientname1", "zh-CN", "客户端名称1", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.clientname1
            new TranslationSeedItem("entity.customerserviceticket.clientname1", "zh-HK", "客户端名称1_hk", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceticket.servicerequestid
            new TranslationSeedItem("entity.customerserviceticket.servicerequestid", "en-US", "关联服务请求ID_us", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicerequestid
            new TranslationSeedItem("entity.customerserviceticket.servicerequestid", "ja-JP", "关联服务请求ID_jp", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicerequestid
            new TranslationSeedItem("entity.customerserviceticket.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicerequestid
            new TranslationSeedItem("entity.customerserviceticket.servicerequestid", "zh-HK", "关联服务请求ID_hk", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceticket.servicerequestcode
            new TranslationSeedItem("entity.customerserviceticket.servicerequestcode", "en-US", "关联服务请求单号_us", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicerequestcode
            new TranslationSeedItem("entity.customerserviceticket.servicerequestcode", "ja-JP", "关联服务请求单号_jp", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicerequestcode
            new TranslationSeedItem("entity.customerserviceticket.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicerequestcode
            new TranslationSeedItem("entity.customerserviceticket.servicerequestcode", "zh-HK", "关联服务请求单号_hk", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceticket.serviceorderid
            new TranslationSeedItem("entity.customerserviceticket.serviceorderid", "en-US", "关联服务订单ID_us", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.serviceorderid
            new TranslationSeedItem("entity.customerserviceticket.serviceorderid", "ja-JP", "关联服务订单ID_jp", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.serviceorderid
            new TranslationSeedItem("entity.customerserviceticket.serviceorderid", "zh-CN", "关联服务订单ID", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.serviceorderid
            new TranslationSeedItem("entity.customerserviceticket.serviceorderid", "zh-HK", "关联服务订单ID_hk", "关联服务订单ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceticket.serviceordercode
            new TranslationSeedItem("entity.customerserviceticket.serviceordercode", "en-US", "关联服务订单编码_us", "关联服务订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.serviceordercode
            new TranslationSeedItem("entity.customerserviceticket.serviceordercode", "ja-JP", "关联服务订单编码_jp", "关联服务订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.serviceordercode
            new TranslationSeedItem("entity.customerserviceticket.serviceordercode", "zh-CN", "关联服务订单编码", "关联服务订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.serviceordercode
            new TranslationSeedItem("entity.customerserviceticket.serviceordercode", "zh-HK", "关联服务订单编码_hk", "关联服务订单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceticket.servicecontractid
            new TranslationSeedItem("entity.customerserviceticket.servicecontractid", "en-US", "关联服务合同ID_us", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicecontractid
            new TranslationSeedItem("entity.customerserviceticket.servicecontractid", "ja-JP", "关联服务合同ID_jp", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicecontractid
            new TranslationSeedItem("entity.customerserviceticket.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.servicecontractid
            new TranslationSeedItem("entity.customerserviceticket.servicecontractid", "zh-HK", "关联服务合同ID_hk", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceticket.servicecontractcode
            new TranslationSeedItem("entity.customerserviceticket.servicecontractcode", "en-US", "关联服务合同编码_us", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicecontractcode
            new TranslationSeedItem("entity.customerserviceticket.servicecontractcode", "ja-JP", "关联服务合同编码_jp", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicecontractcode
            new TranslationSeedItem("entity.customerserviceticket.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceticket.servicecontractcode
            new TranslationSeedItem("entity.customerserviceticket.servicecontractcode", "zh-HK", "关联服务合同编码_hk", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceticket.tickettype
            new TranslationSeedItem("entity.customerserviceticket.tickettype", "en-US", "工单类型_us", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.customerserviceticket.tickettype
            new TranslationSeedItem("entity.customerserviceticket.tickettype", "ja-JP", "工单类型_jp", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.customerserviceticket.tickettype
            new TranslationSeedItem("entity.customerserviceticket.tickettype", "zh-CN", "工单类型", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),
            // entity.customerserviceticket.tickettype
            new TranslationSeedItem("entity.customerserviceticket.tickettype", "zh-HK", "工单类型_hk", "工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）"),

            // entity.customerserviceticket.priority
            new TranslationSeedItem("entity.customerserviceticket.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level）"),
            // entity.customerserviceticket.priority
            new TranslationSeedItem("entity.customerserviceticket.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level）"),
            // entity.customerserviceticket.priority
            new TranslationSeedItem("entity.customerserviceticket.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level）"),
            // entity.customerserviceticket.priority
            new TranslationSeedItem("entity.customerserviceticket.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level）"),

            // entity.customerserviceticket.ticketstatus
            new TranslationSeedItem("entity.customerserviceticket.ticketstatus", "en-US", "工单状态_us", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.customerserviceticket.ticketstatus
            new TranslationSeedItem("entity.customerserviceticket.ticketstatus", "ja-JP", "工单状态_jp", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.customerserviceticket.ticketstatus
            new TranslationSeedItem("entity.customerserviceticket.ticketstatus", "zh-CN", "工单状态", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),
            // entity.customerserviceticket.ticketstatus
            new TranslationSeedItem("entity.customerserviceticket.ticketstatus", "zh-HK", "工单状态_hk", "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）"),

            // entity.customerserviceticket.ticketsubject
            new TranslationSeedItem("entity.customerserviceticket.ticketsubject", "en-US", "工单主题_us", "工单主题"),
            // entity.customerserviceticket.ticketsubject
            new TranslationSeedItem("entity.customerserviceticket.ticketsubject", "ja-JP", "工单主题_jp", "工单主题"),
            // entity.customerserviceticket.ticketsubject
            new TranslationSeedItem("entity.customerserviceticket.ticketsubject", "zh-CN", "工单主题", "工单主题"),
            // entity.customerserviceticket.ticketsubject
            new TranslationSeedItem("entity.customerserviceticket.ticketsubject", "zh-HK", "工单主题_hk", "工单主题"),

            // entity.customerserviceticket.faultdescription
            new TranslationSeedItem("entity.customerserviceticket.faultdescription", "en-US", "故障描述_us", "故障/问题描述"),
            // entity.customerserviceticket.faultdescription
            new TranslationSeedItem("entity.customerserviceticket.faultdescription", "ja-JP", "故障描述_jp", "故障/问题描述"),
            // entity.customerserviceticket.faultdescription
            new TranslationSeedItem("entity.customerserviceticket.faultdescription", "zh-CN", "故障描述", "故障/问题描述"),
            // entity.customerserviceticket.faultdescription
            new TranslationSeedItem("entity.customerserviceticket.faultdescription", "zh-HK", "故障描述_hk", "故障/问题描述"),

            // entity.customerserviceticket.solutiondescription
            new TranslationSeedItem("entity.customerserviceticket.solutiondescription", "en-US", "处理方案_us", "处理方案/解决说明"),
            // entity.customerserviceticket.solutiondescription
            new TranslationSeedItem("entity.customerserviceticket.solutiondescription", "ja-JP", "处理方案_jp", "处理方案/解决说明"),
            // entity.customerserviceticket.solutiondescription
            new TranslationSeedItem("entity.customerserviceticket.solutiondescription", "zh-CN", "处理方案", "处理方案/解决说明"),
            // entity.customerserviceticket.solutiondescription
            new TranslationSeedItem("entity.customerserviceticket.solutiondescription", "zh-HK", "处理方案_hk", "处理方案/解决说明"),

            // entity.customerserviceticket.servicelocation
            new TranslationSeedItem("entity.customerserviceticket.servicelocation", "en-US", "服务地点_us", "服务地点"),
            // entity.customerserviceticket.servicelocation
            new TranslationSeedItem("entity.customerserviceticket.servicelocation", "ja-JP", "服务地点_jp", "服务地点"),
            // entity.customerserviceticket.servicelocation
            new TranslationSeedItem("entity.customerserviceticket.servicelocation", "zh-CN", "服务地点", "服务地点"),
            // entity.customerserviceticket.servicelocation
            new TranslationSeedItem("entity.customerserviceticket.servicelocation", "zh-HK", "服务地点_hk", "服务地点"),

            // entity.customerserviceticket.assignedemployeeid
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeeid", "en-US", "指派服务人员工ID_us", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.assignedemployeeid
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeeid", "ja-JP", "指派服务人员工ID_jp", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.assignedemployeeid
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeeid", "zh-CN", "指派服务人员工ID", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceticket.assignedemployeeid
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeeid", "zh-HK", "指派服务人员工ID_hk", "指派服务人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceticket.assignedemployeename
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeename", "en-US", "指派服务人员姓名_us", "指派服务人员姓名"),
            // entity.customerserviceticket.assignedemployeename
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeename", "ja-JP", "指派服务人员姓名_jp", "指派服务人员姓名"),
            // entity.customerserviceticket.assignedemployeename
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeename", "zh-CN", "指派服务人员姓名", "指派服务人员姓名"),
            // entity.customerserviceticket.assignedemployeename
            new TranslationSeedItem("entity.customerserviceticket.assignedemployeename", "zh-HK", "指派服务人员姓名_hk", "指派服务人员姓名"),

            // entity.customerserviceticket.scheduledstarttime
            new TranslationSeedItem("entity.customerserviceticket.scheduledstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.customerserviceticket.scheduledstarttime
            new TranslationSeedItem("entity.customerserviceticket.scheduledstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.customerserviceticket.scheduledstarttime
            new TranslationSeedItem("entity.customerserviceticket.scheduledstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.customerserviceticket.scheduledstarttime
            new TranslationSeedItem("entity.customerserviceticket.scheduledstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.customerserviceticket.scheduledendtime
            new TranslationSeedItem("entity.customerserviceticket.scheduledendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.customerserviceticket.scheduledendtime
            new TranslationSeedItem("entity.customerserviceticket.scheduledendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.customerserviceticket.scheduledendtime
            new TranslationSeedItem("entity.customerserviceticket.scheduledendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.customerserviceticket.scheduledendtime
            new TranslationSeedItem("entity.customerserviceticket.scheduledendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.customerserviceticket.actualstarttime
            new TranslationSeedItem("entity.customerserviceticket.actualstarttime", "en-US", "实际开始时间_us", "实际开始时间"),
            // entity.customerserviceticket.actualstarttime
            new TranslationSeedItem("entity.customerserviceticket.actualstarttime", "ja-JP", "实际开始时间_jp", "实际开始时间"),
            // entity.customerserviceticket.actualstarttime
            new TranslationSeedItem("entity.customerserviceticket.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.customerserviceticket.actualstarttime
            new TranslationSeedItem("entity.customerserviceticket.actualstarttime", "zh-HK", "实际开始时间_hk", "实际开始时间"),

            // entity.customerserviceticket.actualendtime
            new TranslationSeedItem("entity.customerserviceticket.actualendtime", "en-US", "实际结束时间_us", "实际结束时间"),
            // entity.customerserviceticket.actualendtime
            new TranslationSeedItem("entity.customerserviceticket.actualendtime", "ja-JP", "实际结束时间_jp", "实际结束时间"),
            // entity.customerserviceticket.actualendtime
            new TranslationSeedItem("entity.customerserviceticket.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.customerserviceticket.actualendtime
            new TranslationSeedItem("entity.customerserviceticket.actualendtime", "zh-HK", "实际结束时间_hk", "实际结束时间"),

            // entity.customerserviceticket.acceptanceresult
            new TranslationSeedItem("entity.customerserviceticket.acceptanceresult", "en-US", "验收结果_us", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.customerserviceticket.acceptanceresult
            new TranslationSeedItem("entity.customerserviceticket.acceptanceresult", "ja-JP", "验收结果_jp", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.customerserviceticket.acceptanceresult
            new TranslationSeedItem("entity.customerserviceticket.acceptanceresult", "zh-CN", "验收结果", "验收结果（0=不合格，1=合格，2=部分合格）"),
            // entity.customerserviceticket.acceptanceresult
            new TranslationSeedItem("entity.customerserviceticket.acceptanceresult", "zh-HK", "验收结果_hk", "验收结果（0=不合格，1=合格，2=部分合格）"),

            // entity.customerserviceticket.acceptedbyemployeeid
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeeid", "en-US", "验收人员工ID_us", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceticket.acceptedbyemployeeid
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeeid", "ja-JP", "验收人员工ID_jp", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceticket.acceptedbyemployeeid
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeeid", "zh-CN", "验收人员工ID", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceticket.acceptedbyemployeeid
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeeid", "zh-HK", "验收人员工ID_hk", "验收人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.customerserviceticket.acceptedbyemployeename
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeename", "en-US", "验收人名称_us", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceticket.acceptedbyemployeename
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeename", "ja-JP", "验收人名称_jp", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceticket.acceptedbyemployeename
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeename", "zh-CN", "验收人名称", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceticket.acceptedbyemployeename
            new TranslationSeedItem("entity.customerserviceticket.acceptedbyemployeename", "zh-HK", "验收人名称_hk", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.customerserviceticket.acceptedat
            new TranslationSeedItem("entity.customerserviceticket.acceptedat", "en-US", "验收时间_us", "验收时间"),
            // entity.customerserviceticket.acceptedat
            new TranslationSeedItem("entity.customerserviceticket.acceptedat", "ja-JP", "验收时间_jp", "验收时间"),
            // entity.customerserviceticket.acceptedat
            new TranslationSeedItem("entity.customerserviceticket.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.customerserviceticket.acceptedat
            new TranslationSeedItem("entity.customerserviceticket.acceptedat", "zh-HK", "验收时间_hk", "验收时间"),

            // entity.customerserviceticket.sortorder
            new TranslationSeedItem("entity.customerserviceticket.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceticket.sortorder
            new TranslationSeedItem("entity.customerserviceticket.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceticket.sortorder
            new TranslationSeedItem("entity.customerserviceticket.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceticket.sortorder
            new TranslationSeedItem("entity.customerserviceticket.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customerserviceticket.customerservicerequest
            new TranslationSeedItem("entity.customerserviceticket.customerservicerequest", "en-US", "关联服务请求_us", "关联服务请求"),
            // entity.customerserviceticket.customerservicerequest
            new TranslationSeedItem("entity.customerserviceticket.customerservicerequest", "ja-JP", "关联服务请求_jp", "关联服务请求"),
            // entity.customerserviceticket.customerservicerequest
            new TranslationSeedItem("entity.customerserviceticket.customerservicerequest", "zh-CN", "关联服务请求", "关联服务请求"),
            // entity.customerserviceticket.customerservicerequest
            new TranslationSeedItem("entity.customerserviceticket.customerservicerequest", "zh-HK", "关联服务请求_hk", "关联服务请求"),

            // entity.customerserviceticket.customerserviceorder
            new TranslationSeedItem("entity.customerserviceticket.customerserviceorder", "en-US", "关联服务订单_us", "关联服务订单"),
            // entity.customerserviceticket.customerserviceorder
            new TranslationSeedItem("entity.customerserviceticket.customerserviceorder", "ja-JP", "关联服务订单_jp", "关联服务订单"),
            // entity.customerserviceticket.customerserviceorder
            new TranslationSeedItem("entity.customerserviceticket.customerserviceorder", "zh-CN", "关联服务订单", "关联服务订单"),
            // entity.customerserviceticket.customerserviceorder
            new TranslationSeedItem("entity.customerserviceticket.customerserviceorder", "zh-HK", "关联服务订单_hk", "关联服务订单"),

            // entity.customerserviceticket.customerservicecontract
            new TranslationSeedItem("entity.customerserviceticket.customerservicecontract", "en-US", "关联服务合同_us", "关联服务合同"),
            // entity.customerserviceticket.customerservicecontract
            new TranslationSeedItem("entity.customerserviceticket.customerservicecontract", "ja-JP", "关联服务合同_jp", "关联服务合同"),
            // entity.customerserviceticket.customerservicecontract
            new TranslationSeedItem("entity.customerserviceticket.customerservicecontract", "zh-CN", "关联服务合同", "关联服务合同"),
            // entity.customerserviceticket.customerservicecontract
            new TranslationSeedItem("entity.customerserviceticket.customerservicecontract", "zh-HK", "关联服务合同_hk", "关联服务合同"),
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
        translation.ResourceGroup = "CustomerService";
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
