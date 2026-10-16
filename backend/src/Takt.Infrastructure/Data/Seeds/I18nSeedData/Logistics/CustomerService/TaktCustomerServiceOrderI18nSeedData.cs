// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktCustomerServiceOrderI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerServiceOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerServiceOrder 实体国际化翻译种子（键前缀 entity.customerserviceorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerServiceOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerServiceOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerserviceorder 实体翻译...", tenantCode);

        foreach (var item in GetCustomerServiceOrderTranslations())
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

        TaktLogger.Information("TaktCustomerServiceOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerServiceOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerserviceorder._self / entity.customerserviceorder.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerServiceOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerserviceorder._self
            new TranslationSeedItem("entity.customerserviceorder._self", "en-US", "Customer Service Order Information_us", "实体名称"),
            // entity.customerserviceorder._self
            new TranslationSeedItem("entity.customerserviceorder._self", "ja-JP", "服务订单信息_jp", "实体名称"),
            // entity.customerserviceorder._self
            new TranslationSeedItem("entity.customerserviceorder._self", "zh-CN", "服务订单信息", "实体名称"),
            // entity.customerserviceorder._self
            new TranslationSeedItem("entity.customerserviceorder._self", "zh-HK", "服务订单信息_hk", "实体名称"),

            // entity.customerserviceorder.serviceordercode
            new TranslationSeedItem("entity.customerserviceorder.serviceordercode", "en-US", "服务订单编码_us", "服务订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务订单编码规则生成并展示，非手输；单据类型菜单：服务订单）"),
            // entity.customerserviceorder.serviceordercode
            new TranslationSeedItem("entity.customerserviceorder.serviceordercode", "ja-JP", "服务订单编码_jp", "服务订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务订单编码规则生成并展示，非手输；单据类型菜单：服务订单）"),
            // entity.customerserviceorder.serviceordercode
            new TranslationSeedItem("entity.customerserviceorder.serviceordercode", "zh-CN", "服务订单编码", "服务订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务订单编码规则生成并展示，非手输；单据类型菜单：服务订单）"),
            // entity.customerserviceorder.serviceordercode
            new TranslationSeedItem("entity.customerserviceorder.serviceordercode", "zh-HK", "服务订单编码_hk", "服务订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务订单编码规则生成并展示，非手输；单据类型菜单：服务订单）"),

            // entity.customerserviceorder.clientid
            new TranslationSeedItem("entity.customerserviceorder.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.clientid
            new TranslationSeedItem("entity.customerserviceorder.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.clientid
            new TranslationSeedItem("entity.customerserviceorder.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.clientid
            new TranslationSeedItem("entity.customerserviceorder.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceorder.clientcode
            new TranslationSeedItem("entity.customerserviceorder.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientcode
            new TranslationSeedItem("entity.customerserviceorder.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientcode
            new TranslationSeedItem("entity.customerserviceorder.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientcode
            new TranslationSeedItem("entity.customerserviceorder.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceorder.clientname1
            new TranslationSeedItem("entity.customerserviceorder.clientname1", "en-US", "客户端名称1_us", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientname1
            new TranslationSeedItem("entity.customerserviceorder.clientname1", "ja-JP", "客户端名称1_jp", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientname1
            new TranslationSeedItem("entity.customerserviceorder.clientname1", "zh-CN", "客户端名称1", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.clientname1
            new TranslationSeedItem("entity.customerserviceorder.clientname1", "zh-HK", "客户端名称1_hk", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceorder.servicecontractid
            new TranslationSeedItem("entity.customerserviceorder.servicecontractid", "en-US", "关联服务合同ID_us", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicecontractid
            new TranslationSeedItem("entity.customerserviceorder.servicecontractid", "ja-JP", "关联服务合同ID_jp", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicecontractid
            new TranslationSeedItem("entity.customerserviceorder.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicecontractid
            new TranslationSeedItem("entity.customerserviceorder.servicecontractid", "zh-HK", "关联服务合同ID_hk", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceorder.servicecontractcode
            new TranslationSeedItem("entity.customerserviceorder.servicecontractcode", "en-US", "关联服务合同编码_us", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicecontractcode
            new TranslationSeedItem("entity.customerserviceorder.servicecontractcode", "ja-JP", "关联服务合同编码_jp", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicecontractcode
            new TranslationSeedItem("entity.customerserviceorder.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicecontractcode
            new TranslationSeedItem("entity.customerserviceorder.servicecontractcode", "zh-HK", "关联服务合同编码_hk", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceorder.servicerequestid
            new TranslationSeedItem("entity.customerserviceorder.servicerequestid", "en-US", "关联服务请求ID_us", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicerequestid
            new TranslationSeedItem("entity.customerserviceorder.servicerequestid", "ja-JP", "关联服务请求ID_jp", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicerequestid
            new TranslationSeedItem("entity.customerserviceorder.servicerequestid", "zh-CN", "关联服务请求ID", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerserviceorder.servicerequestid
            new TranslationSeedItem("entity.customerserviceorder.servicerequestid", "zh-HK", "关联服务请求ID_hk", "关联服务请求ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerserviceorder.servicerequestcode
            new TranslationSeedItem("entity.customerserviceorder.servicerequestcode", "en-US", "关联服务请求单号_us", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicerequestcode
            new TranslationSeedItem("entity.customerserviceorder.servicerequestcode", "ja-JP", "关联服务请求单号_jp", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicerequestcode
            new TranslationSeedItem("entity.customerserviceorder.servicerequestcode", "zh-CN", "关联服务请求单号", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerserviceorder.servicerequestcode
            new TranslationSeedItem("entity.customerserviceorder.servicerequestcode", "zh-HK", "关联服务请求单号_hk", "关联服务请求单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerserviceorder.orderdate
            new TranslationSeedItem("entity.customerserviceorder.orderdate", "en-US", "订单日期_us", "订单日期"),
            // entity.customerserviceorder.orderdate
            new TranslationSeedItem("entity.customerserviceorder.orderdate", "ja-JP", "订单日期_jp", "订单日期"),
            // entity.customerserviceorder.orderdate
            new TranslationSeedItem("entity.customerserviceorder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.customerserviceorder.orderdate
            new TranslationSeedItem("entity.customerserviceorder.orderdate", "zh-HK", "订单日期_hk", "订单日期"),

            // entity.customerserviceorder.ordertype
            new TranslationSeedItem("entity.customerserviceorder.ordertype", "en-US", "订单类型_us", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.customerserviceorder.ordertype
            new TranslationSeedItem("entity.customerserviceorder.ordertype", "ja-JP", "订单类型_jp", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.customerserviceorder.ordertype
            new TranslationSeedItem("entity.customerserviceorder.ordertype", "zh-CN", "订单类型", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),
            // entity.customerserviceorder.ordertype
            new TranslationSeedItem("entity.customerserviceorder.ordertype", "zh-HK", "订单类型_hk", "订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）"),

            // entity.customerserviceorder.orderstatus
            new TranslationSeedItem("entity.customerserviceorder.orderstatus", "en-US", "订单状态_us", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.customerserviceorder.orderstatus
            new TranslationSeedItem("entity.customerserviceorder.orderstatus", "ja-JP", "订单状态_jp", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.customerserviceorder.orderstatus
            new TranslationSeedItem("entity.customerserviceorder.orderstatus", "zh-CN", "订单状态", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),
            // entity.customerserviceorder.orderstatus
            new TranslationSeedItem("entity.customerserviceorder.orderstatus", "zh-HK", "订单状态_hk", "订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）"),

            // entity.customerserviceorder.totalamount
            new TranslationSeedItem("entity.customerserviceorder.totalamount", "en-US", "订单总金额_us", "订单总金额"),
            // entity.customerserviceorder.totalamount
            new TranslationSeedItem("entity.customerserviceorder.totalamount", "ja-JP", "订单总金额_jp", "订单总金额"),
            // entity.customerserviceorder.totalamount
            new TranslationSeedItem("entity.customerserviceorder.totalamount", "zh-CN", "订单总金额", "订单总金额"),
            // entity.customerserviceorder.totalamount
            new TranslationSeedItem("entity.customerserviceorder.totalamount", "zh-HK", "订单总金额_hk", "订单总金额"),

            // entity.customerserviceorder.discountamount
            new TranslationSeedItem("entity.customerserviceorder.discountamount", "en-US", "折扣金额_us", "折扣金额"),
            // entity.customerserviceorder.discountamount
            new TranslationSeedItem("entity.customerserviceorder.discountamount", "ja-JP", "折扣金额_jp", "折扣金额"),
            // entity.customerserviceorder.discountamount
            new TranslationSeedItem("entity.customerserviceorder.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.customerserviceorder.discountamount
            new TranslationSeedItem("entity.customerserviceorder.discountamount", "zh-HK", "折扣金额_hk", "折扣金额"),

            // entity.customerserviceorder.taxamount
            new TranslationSeedItem("entity.customerserviceorder.taxamount", "en-US", "税费_us", "税费"),
            // entity.customerserviceorder.taxamount
            new TranslationSeedItem("entity.customerserviceorder.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.customerserviceorder.taxamount
            new TranslationSeedItem("entity.customerserviceorder.taxamount", "zh-CN", "税费", "税费"),
            // entity.customerserviceorder.taxamount
            new TranslationSeedItem("entity.customerserviceorder.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.customerserviceorder.actualamount
            new TranslationSeedItem("entity.customerserviceorder.actualamount", "en-US", "订单实付金额_us", "订单实付金额"),
            // entity.customerserviceorder.actualamount
            new TranslationSeedItem("entity.customerserviceorder.actualamount", "ja-JP", "订单实付金额_jp", "订单实付金额"),
            // entity.customerserviceorder.actualamount
            new TranslationSeedItem("entity.customerserviceorder.actualamount", "zh-CN", "订单实付金额", "订单实付金额"),
            // entity.customerserviceorder.actualamount
            new TranslationSeedItem("entity.customerserviceorder.actualamount", "zh-HK", "订单实付金额_hk", "订单实付金额"),

            // entity.customerserviceorder.currencycode
            new TranslationSeedItem("entity.customerserviceorder.currencycode", "en-US", "结算币种代码_us", "结算币种代码"),
            // entity.customerserviceorder.currencycode
            new TranslationSeedItem("entity.customerserviceorder.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码"),
            // entity.customerserviceorder.currencycode
            new TranslationSeedItem("entity.customerserviceorder.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.customerserviceorder.currencycode
            new TranslationSeedItem("entity.customerserviceorder.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码"),

            // entity.customerserviceorder.plannedstartdate
            new TranslationSeedItem("entity.customerserviceorder.plannedstartdate", "en-US", "计划开始日期_us", "计划开始日期"),
            // entity.customerserviceorder.plannedstartdate
            new TranslationSeedItem("entity.customerserviceorder.plannedstartdate", "ja-JP", "计划开始日期_jp", "计划开始日期"),
            // entity.customerserviceorder.plannedstartdate
            new TranslationSeedItem("entity.customerserviceorder.plannedstartdate", "zh-CN", "计划开始日期", "计划开始日期"),
            // entity.customerserviceorder.plannedstartdate
            new TranslationSeedItem("entity.customerserviceorder.plannedstartdate", "zh-HK", "计划开始日期_hk", "计划开始日期"),

            // entity.customerserviceorder.plannedenddate
            new TranslationSeedItem("entity.customerserviceorder.plannedenddate", "en-US", "计划结束日期_us", "计划结束日期"),
            // entity.customerserviceorder.plannedenddate
            new TranslationSeedItem("entity.customerserviceorder.plannedenddate", "ja-JP", "计划结束日期_jp", "计划结束日期"),
            // entity.customerserviceorder.plannedenddate
            new TranslationSeedItem("entity.customerserviceorder.plannedenddate", "zh-CN", "计划结束日期", "计划结束日期"),
            // entity.customerserviceorder.plannedenddate
            new TranslationSeedItem("entity.customerserviceorder.plannedenddate", "zh-HK", "计划结束日期_hk", "计划结束日期"),

            // entity.customerserviceorder.actualstartdate
            new TranslationSeedItem("entity.customerserviceorder.actualstartdate", "en-US", "实际开始日期_us", "实际开始日期"),
            // entity.customerserviceorder.actualstartdate
            new TranslationSeedItem("entity.customerserviceorder.actualstartdate", "ja-JP", "实际开始日期_jp", "实际开始日期"),
            // entity.customerserviceorder.actualstartdate
            new TranslationSeedItem("entity.customerserviceorder.actualstartdate", "zh-CN", "实际开始日期", "实际开始日期"),
            // entity.customerserviceorder.actualstartdate
            new TranslationSeedItem("entity.customerserviceorder.actualstartdate", "zh-HK", "实际开始日期_hk", "实际开始日期"),

            // entity.customerserviceorder.actualenddate
            new TranslationSeedItem("entity.customerserviceorder.actualenddate", "en-US", "实际结束日期_us", "实际结束日期"),
            // entity.customerserviceorder.actualenddate
            new TranslationSeedItem("entity.customerserviceorder.actualenddate", "ja-JP", "实际结束日期_jp", "实际结束日期"),
            // entity.customerserviceorder.actualenddate
            new TranslationSeedItem("entity.customerserviceorder.actualenddate", "zh-CN", "实际结束日期", "实际结束日期"),
            // entity.customerserviceorder.actualenddate
            new TranslationSeedItem("entity.customerserviceorder.actualenddate", "zh-HK", "实际结束日期_hk", "实际结束日期"),

            // entity.customerserviceorder.serviceemployeeid
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeeid", "en-US", "服务负责人员工ID_us", "服务负责人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceorder.serviceemployeeid
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeeid", "ja-JP", "服务负责人员工ID_jp", "服务负责人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceorder.serviceemployeeid
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeeid", "zh-CN", "服务负责人员工ID", "服务负责人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerserviceorder.serviceemployeeid
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeeid", "zh-HK", "服务负责人员工ID_hk", "服务负责人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.customerserviceorder.serviceemployeename
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeename", "en-US", "服务负责人名称_us", "服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceorder.serviceemployeename
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeename", "ja-JP", "服务负责人名称_jp", "服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceorder.serviceemployeename
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeename", "zh-CN", "服务负责人名称", "服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerserviceorder.serviceemployeename
            new TranslationSeedItem("entity.customerserviceorder.serviceemployeename", "zh-HK", "服务负责人名称_hk", "服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.customerserviceorder.sortorder
            new TranslationSeedItem("entity.customerserviceorder.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceorder.sortorder
            new TranslationSeedItem("entity.customerserviceorder.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceorder.sortorder
            new TranslationSeedItem("entity.customerserviceorder.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customerserviceorder.sortorder
            new TranslationSeedItem("entity.customerserviceorder.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customerserviceorder.customerservicecontract
            new TranslationSeedItem("entity.customerserviceorder.customerservicecontract", "en-US", "关联服务合同_us", "关联服务合同"),
            // entity.customerserviceorder.customerservicecontract
            new TranslationSeedItem("entity.customerserviceorder.customerservicecontract", "ja-JP", "关联服务合同_jp", "关联服务合同"),
            // entity.customerserviceorder.customerservicecontract
            new TranslationSeedItem("entity.customerserviceorder.customerservicecontract", "zh-CN", "关联服务合同", "关联服务合同"),
            // entity.customerserviceorder.customerservicecontract
            new TranslationSeedItem("entity.customerserviceorder.customerservicecontract", "zh-HK", "关联服务合同_hk", "关联服务合同"),

            // entity.customerserviceorder.customerservicerequest
            new TranslationSeedItem("entity.customerserviceorder.customerservicerequest", "en-US", "关联服务请求_us", "关联服务请求"),
            // entity.customerserviceorder.customerservicerequest
            new TranslationSeedItem("entity.customerserviceorder.customerservicerequest", "ja-JP", "关联服务请求_jp", "关联服务请求"),
            // entity.customerserviceorder.customerservicerequest
            new TranslationSeedItem("entity.customerserviceorder.customerservicerequest", "zh-CN", "关联服务请求", "关联服务请求"),
            // entity.customerserviceorder.customerservicerequest
            new TranslationSeedItem("entity.customerserviceorder.customerservicerequest", "zh-HK", "关联服务请求_hk", "关联服务请求"),

            // entity.customerserviceorder.tickets
            new TranslationSeedItem("entity.customerserviceorder.tickets", "en-US", "服务工单列表_us", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceOrderId）"),
            // entity.customerserviceorder.tickets
            new TranslationSeedItem("entity.customerserviceorder.tickets", "ja-JP", "服务工单列表_jp", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceOrderId）"),
            // entity.customerserviceorder.tickets
            new TranslationSeedItem("entity.customerserviceorder.tickets", "zh-CN", "服务工单列表", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceOrderId）"),
            // entity.customerserviceorder.tickets
            new TranslationSeedItem("entity.customerserviceorder.tickets", "zh-HK", "服务工单列表_hk", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceOrderId）"),
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
