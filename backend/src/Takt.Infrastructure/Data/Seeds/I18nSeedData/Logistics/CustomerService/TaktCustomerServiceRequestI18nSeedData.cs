// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktCustomerServiceRequestI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerServiceRequest 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerServiceRequest 实体国际化翻译种子（键前缀 entity.customerservicerequest.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerServiceRequestI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerServiceRequest 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerservicerequest 实体翻译...", tenantCode);

        foreach (var item in GetCustomerServiceRequestTranslations())
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

        TaktLogger.Information("TaktCustomerServiceRequest 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerServiceRequest 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerservicerequest._self / entity.customerservicerequest.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerServiceRequestTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerservicerequest._self
            new TranslationSeedItem("entity.customerservicerequest._self", "en-US", "Customer Service Request Information_us", "实体名称"),
            // entity.customerservicerequest._self
            new TranslationSeedItem("entity.customerservicerequest._self", "ja-JP", "服务请求信息_jp", "实体名称"),
            // entity.customerservicerequest._self
            new TranslationSeedItem("entity.customerservicerequest._self", "zh-CN", "服务请求信息", "实体名称"),
            // entity.customerservicerequest._self
            new TranslationSeedItem("entity.customerservicerequest._self", "zh-HK", "服务请求信息_hk", "实体名称"),

            // entity.customerservicerequest.servicerequestcode
            new TranslationSeedItem("entity.customerservicerequest.servicerequestcode", "en-US", "服务请求单号_us", "服务请求单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务请求单号规则生成并展示，非手输；单据类型菜单：服务请求）"),
            // entity.customerservicerequest.servicerequestcode
            new TranslationSeedItem("entity.customerservicerequest.servicerequestcode", "ja-JP", "服务请求单号_jp", "服务请求单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务请求单号规则生成并展示，非手输；单据类型菜单：服务请求）"),
            // entity.customerservicerequest.servicerequestcode
            new TranslationSeedItem("entity.customerservicerequest.servicerequestcode", "zh-CN", "服务请求单号", "服务请求单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务请求单号规则生成并展示，非手输；单据类型菜单：服务请求）"),
            // entity.customerservicerequest.servicerequestcode
            new TranslationSeedItem("entity.customerservicerequest.servicerequestcode", "zh-HK", "服务请求单号_hk", "服务请求单号（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务请求单号规则生成并展示，非手输；单据类型菜单：服务请求）"),

            // entity.customerservicerequest.clientid
            new TranslationSeedItem("entity.customerservicerequest.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.clientid
            new TranslationSeedItem("entity.customerservicerequest.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.clientid
            new TranslationSeedItem("entity.customerservicerequest.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.clientid
            new TranslationSeedItem("entity.customerservicerequest.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.customerservicerequest.clientcode
            new TranslationSeedItem("entity.customerservicerequest.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientcode
            new TranslationSeedItem("entity.customerservicerequest.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientcode
            new TranslationSeedItem("entity.customerservicerequest.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientcode
            new TranslationSeedItem("entity.customerservicerequest.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerservicerequest.clientname1
            new TranslationSeedItem("entity.customerservicerequest.clientname1", "en-US", "客户端名称1_us", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientname1
            new TranslationSeedItem("entity.customerservicerequest.clientname1", "ja-JP", "客户端名称1_jp", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientname1
            new TranslationSeedItem("entity.customerservicerequest.clientname1", "zh-CN", "客户端名称1", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.clientname1
            new TranslationSeedItem("entity.customerservicerequest.clientname1", "zh-HK", "客户端名称1_hk", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerservicerequest.servicecontractid
            new TranslationSeedItem("entity.customerservicerequest.servicecontractid", "en-US", "关联服务合同ID_us", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.servicecontractid
            new TranslationSeedItem("entity.customerservicerequest.servicecontractid", "ja-JP", "关联服务合同ID_jp", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.servicecontractid
            new TranslationSeedItem("entity.customerservicerequest.servicecontractid", "zh-CN", "关联服务合同ID", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.servicecontractid
            new TranslationSeedItem("entity.customerservicerequest.servicecontractid", "zh-HK", "关联服务合同ID_hk", "关联服务合同ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerservicerequest.servicecontractcode
            new TranslationSeedItem("entity.customerservicerequest.servicecontractcode", "en-US", "关联服务合同编码_us", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.servicecontractcode
            new TranslationSeedItem("entity.customerservicerequest.servicecontractcode", "ja-JP", "关联服务合同编码_jp", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.servicecontractcode
            new TranslationSeedItem("entity.customerservicerequest.servicecontractcode", "zh-CN", "关联服务合同编码", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicerequest.servicecontractcode
            new TranslationSeedItem("entity.customerservicerequest.servicecontractcode", "zh-HK", "关联服务合同编码_hk", "关联服务合同编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerservicerequest.requestdate
            new TranslationSeedItem("entity.customerservicerequest.requestdate", "en-US", "请求日期_us", "请求日期"),
            // entity.customerservicerequest.requestdate
            new TranslationSeedItem("entity.customerservicerequest.requestdate", "ja-JP", "请求日期_jp", "请求日期"),
            // entity.customerservicerequest.requestdate
            new TranslationSeedItem("entity.customerservicerequest.requestdate", "zh-CN", "请求日期", "请求日期"),
            // entity.customerservicerequest.requestdate
            new TranslationSeedItem("entity.customerservicerequest.requestdate", "zh-HK", "请求日期_hk", "请求日期"),

            // entity.customerservicerequest.expectedservicedate
            new TranslationSeedItem("entity.customerservicerequest.expectedservicedate", "en-US", "期望服务日期_us", "期望服务日期"),
            // entity.customerservicerequest.expectedservicedate
            new TranslationSeedItem("entity.customerservicerequest.expectedservicedate", "ja-JP", "期望服务日期_jp", "期望服务日期"),
            // entity.customerservicerequest.expectedservicedate
            new TranslationSeedItem("entity.customerservicerequest.expectedservicedate", "zh-CN", "期望服务日期", "期望服务日期"),
            // entity.customerservicerequest.expectedservicedate
            new TranslationSeedItem("entity.customerservicerequest.expectedservicedate", "zh-HK", "期望服务日期_hk", "期望服务日期"),

            // entity.customerservicerequest.requesttype
            new TranslationSeedItem("entity.customerservicerequest.requesttype", "en-US", "请求类型_us", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.customerservicerequest.requesttype
            new TranslationSeedItem("entity.customerservicerequest.requesttype", "ja-JP", "请求类型_jp", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.customerservicerequest.requesttype
            new TranslationSeedItem("entity.customerservicerequest.requesttype", "zh-CN", "请求类型", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),
            // entity.customerservicerequest.requesttype
            new TranslationSeedItem("entity.customerservicerequest.requesttype", "zh-HK", "请求类型_hk", "请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）"),

            // entity.customerservicerequest.sourcechannel
            new TranslationSeedItem("entity.customerservicerequest.sourcechannel", "en-US", "请求来源_us", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.customerservicerequest.sourcechannel
            new TranslationSeedItem("entity.customerservicerequest.sourcechannel", "ja-JP", "请求来源_jp", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.customerservicerequest.sourcechannel
            new TranslationSeedItem("entity.customerservicerequest.sourcechannel", "zh-CN", "请求来源", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),
            // entity.customerservicerequest.sourcechannel
            new TranslationSeedItem("entity.customerservicerequest.sourcechannel", "zh-HK", "请求来源_hk", "请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）"),

            // entity.customerservicerequest.priority
            new TranslationSeedItem("entity.customerservicerequest.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level）"),
            // entity.customerservicerequest.priority
            new TranslationSeedItem("entity.customerservicerequest.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level）"),
            // entity.customerservicerequest.priority
            new TranslationSeedItem("entity.customerservicerequest.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level）"),
            // entity.customerservicerequest.priority
            new TranslationSeedItem("entity.customerservicerequest.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level）"),

            // entity.customerservicerequest.requeststatus
            new TranslationSeedItem("entity.customerservicerequest.requeststatus", "en-US", "请求状态_us", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.customerservicerequest.requeststatus
            new TranslationSeedItem("entity.customerservicerequest.requeststatus", "ja-JP", "请求状态_jp", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.customerservicerequest.requeststatus
            new TranslationSeedItem("entity.customerservicerequest.requeststatus", "zh-CN", "请求状态", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),
            // entity.customerservicerequest.requeststatus
            new TranslationSeedItem("entity.customerservicerequest.requeststatus", "zh-HK", "请求状态_hk", "请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）"),

            // entity.customerservicerequest.requestsubject
            new TranslationSeedItem("entity.customerservicerequest.requestsubject", "en-US", "请求主题_us", "请求主题"),
            // entity.customerservicerequest.requestsubject
            new TranslationSeedItem("entity.customerservicerequest.requestsubject", "ja-JP", "请求主题_jp", "请求主题"),
            // entity.customerservicerequest.requestsubject
            new TranslationSeedItem("entity.customerservicerequest.requestsubject", "zh-CN", "请求主题", "请求主题"),
            // entity.customerservicerequest.requestsubject
            new TranslationSeedItem("entity.customerservicerequest.requestsubject", "zh-HK", "请求主题_hk", "请求主题"),

            // entity.customerservicerequest.requestdescription
            new TranslationSeedItem("entity.customerservicerequest.requestdescription", "en-US", "请求描述_us", "请求描述"),
            // entity.customerservicerequest.requestdescription
            new TranslationSeedItem("entity.customerservicerequest.requestdescription", "ja-JP", "请求描述_jp", "请求描述"),
            // entity.customerservicerequest.requestdescription
            new TranslationSeedItem("entity.customerservicerequest.requestdescription", "zh-CN", "请求描述", "请求描述"),
            // entity.customerservicerequest.requestdescription
            new TranslationSeedItem("entity.customerservicerequest.requestdescription", "zh-HK", "请求描述_hk", "请求描述"),

            // entity.customerservicerequest.contactperson
            new TranslationSeedItem("entity.customerservicerequest.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.customerservicerequest.contactperson
            new TranslationSeedItem("entity.customerservicerequest.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.customerservicerequest.contactperson
            new TranslationSeedItem("entity.customerservicerequest.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.customerservicerequest.contactperson
            new TranslationSeedItem("entity.customerservicerequest.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.customerservicerequest.contactphone
            new TranslationSeedItem("entity.customerservicerequest.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.customerservicerequest.contactphone
            new TranslationSeedItem("entity.customerservicerequest.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.customerservicerequest.contactphone
            new TranslationSeedItem("entity.customerservicerequest.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.customerservicerequest.contactphone
            new TranslationSeedItem("entity.customerservicerequest.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.customerservicerequest.contactemail
            new TranslationSeedItem("entity.customerservicerequest.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.customerservicerequest.contactemail
            new TranslationSeedItem("entity.customerservicerequest.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.customerservicerequest.contactemail
            new TranslationSeedItem("entity.customerservicerequest.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.customerservicerequest.contactemail
            new TranslationSeedItem("entity.customerservicerequest.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.customerservicerequest.serviceaddress
            new TranslationSeedItem("entity.customerservicerequest.serviceaddress", "en-US", "服务地址_us", "服务地址"),
            // entity.customerservicerequest.serviceaddress
            new TranslationSeedItem("entity.customerservicerequest.serviceaddress", "ja-JP", "服务地址_jp", "服务地址"),
            // entity.customerservicerequest.serviceaddress
            new TranslationSeedItem("entity.customerservicerequest.serviceaddress", "zh-CN", "服务地址", "服务地址"),
            // entity.customerservicerequest.serviceaddress
            new TranslationSeedItem("entity.customerservicerequest.serviceaddress", "zh-HK", "服务地址_hk", "服务地址"),

            // entity.customerservicerequest.assignedemployeeid
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeeid", "en-US", "受理人员工ID_us", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.assignedemployeeid
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeeid", "ja-JP", "受理人员工ID_jp", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.assignedemployeeid
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeeid", "zh-CN", "受理人员工ID", "受理人员工ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerservicerequest.assignedemployeeid
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeeid", "zh-HK", "受理人员工ID_hk", "受理人员工ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerservicerequest.assignedemployeename
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeename", "en-US", "受理人姓名_us", "受理人姓名"),
            // entity.customerservicerequest.assignedemployeename
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeename", "ja-JP", "受理人姓名_jp", "受理人姓名"),
            // entity.customerservicerequest.assignedemployeename
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeename", "zh-CN", "受理人姓名", "受理人姓名"),
            // entity.customerservicerequest.assignedemployeename
            new TranslationSeedItem("entity.customerservicerequest.assignedemployeename", "zh-HK", "受理人姓名_hk", "受理人姓名"),

            // entity.customerservicerequest.assignedat
            new TranslationSeedItem("entity.customerservicerequest.assignedat", "en-US", "受理时间_us", "受理时间"),
            // entity.customerservicerequest.assignedat
            new TranslationSeedItem("entity.customerservicerequest.assignedat", "ja-JP", "受理时间_jp", "受理时间"),
            // entity.customerservicerequest.assignedat
            new TranslationSeedItem("entity.customerservicerequest.assignedat", "zh-CN", "受理时间", "受理时间"),
            // entity.customerservicerequest.assignedat
            new TranslationSeedItem("entity.customerservicerequest.assignedat", "zh-HK", "受理时间_hk", "受理时间"),

            // entity.customerservicerequest.closedat
            new TranslationSeedItem("entity.customerservicerequest.closedat", "en-US", "关闭时间_us", "关闭时间"),
            // entity.customerservicerequest.closedat
            new TranslationSeedItem("entity.customerservicerequest.closedat", "ja-JP", "关闭时间_jp", "关闭时间"),
            // entity.customerservicerequest.closedat
            new TranslationSeedItem("entity.customerservicerequest.closedat", "zh-CN", "关闭时间", "关闭时间"),
            // entity.customerservicerequest.closedat
            new TranslationSeedItem("entity.customerservicerequest.closedat", "zh-HK", "关闭时间_hk", "关闭时间"),

            // entity.customerservicerequest.sortorder
            new TranslationSeedItem("entity.customerservicerequest.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customerservicerequest.sortorder
            new TranslationSeedItem("entity.customerservicerequest.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customerservicerequest.sortorder
            new TranslationSeedItem("entity.customerservicerequest.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customerservicerequest.sortorder
            new TranslationSeedItem("entity.customerservicerequest.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customerservicerequest.customerservicecontract
            new TranslationSeedItem("entity.customerservicerequest.customerservicecontract", "en-US", "关联服务合同_us", "关联服务合同"),
            // entity.customerservicerequest.customerservicecontract
            new TranslationSeedItem("entity.customerservicerequest.customerservicecontract", "ja-JP", "关联服务合同_jp", "关联服务合同"),
            // entity.customerservicerequest.customerservicecontract
            new TranslationSeedItem("entity.customerservicerequest.customerservicecontract", "zh-CN", "关联服务合同", "关联服务合同"),
            // entity.customerservicerequest.customerservicecontract
            new TranslationSeedItem("entity.customerservicerequest.customerservicecontract", "zh-HK", "关联服务合同_hk", "关联服务合同"),

            // entity.customerservicerequest.serviceorders
            new TranslationSeedItem("entity.customerservicerequest.serviceorders", "en-US", "关联服务订单列表_us", "关联服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceRequestId）"),
            // entity.customerservicerequest.serviceorders
            new TranslationSeedItem("entity.customerservicerequest.serviceorders", "ja-JP", "关联服务订单列表_jp", "关联服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceRequestId）"),
            // entity.customerservicerequest.serviceorders
            new TranslationSeedItem("entity.customerservicerequest.serviceorders", "zh-CN", "关联服务订单列表", "关联服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceRequestId）"),
            // entity.customerservicerequest.serviceorders
            new TranslationSeedItem("entity.customerservicerequest.serviceorders", "zh-HK", "关联服务订单列表_hk", "关联服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceRequestId）"),

            // entity.customerservicerequest.tickets
            new TranslationSeedItem("entity.customerservicerequest.tickets", "en-US", "服务工单列表_us", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceRequestId）"),
            // entity.customerservicerequest.tickets
            new TranslationSeedItem("entity.customerservicerequest.tickets", "ja-JP", "服务工单列表_jp", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceRequestId）"),
            // entity.customerservicerequest.tickets
            new TranslationSeedItem("entity.customerservicerequest.tickets", "zh-CN", "服务工单列表", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceRequestId）"),
            // entity.customerservicerequest.tickets
            new TranslationSeedItem("entity.customerservicerequest.tickets", "zh-HK", "服务工单列表_hk", "服务工单列表（外键在子表 TaktCustomerServiceTicket.ServiceRequestId）"),
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
