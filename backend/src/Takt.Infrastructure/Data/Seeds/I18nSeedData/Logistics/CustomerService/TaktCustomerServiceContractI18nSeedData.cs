// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContractI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerServiceContract 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomerServiceContract 实体国际化翻译种子（键前缀 entity.customerservicecontract.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerServiceContractI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerServiceContract 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerservicecontract 实体翻译...", tenantCode);

        foreach (var item in GetCustomerServiceContractTranslations())
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

        TaktLogger.Information("TaktCustomerServiceContract 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerServiceContract 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerservicecontract._self / entity.customerservicecontract.{{field}}；ResourceGroup=CustomerService；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerServiceContractTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerservicecontract._self
            new TranslationSeedItem("entity.customerservicecontract._self", "en-US", "Customer Service Contract Information_us", "实体名称"),
            // entity.customerservicecontract._self
            new TranslationSeedItem("entity.customerservicecontract._self", "ja-JP", "服务合同信息_jp", "实体名称"),
            // entity.customerservicecontract._self
            new TranslationSeedItem("entity.customerservicecontract._self", "zh-CN", "服务合同信息", "实体名称"),
            // entity.customerservicecontract._self
            new TranslationSeedItem("entity.customerservicecontract._self", "zh-HK", "服务合同信息_hk", "实体名称"),

            // entity.customerservicecontract.servicecontractcode
            new TranslationSeedItem("entity.customerservicecontract.servicecontractcode", "en-US", "服务合同编码_us", "服务合同编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务合同编码规则生成并展示，非手输；单据类型菜单：服务合同）"),
            // entity.customerservicecontract.servicecontractcode
            new TranslationSeedItem("entity.customerservicecontract.servicecontractcode", "ja-JP", "服务合同编码_jp", "服务合同编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务合同编码规则生成并展示，非手输；单据类型菜单：服务合同）"),
            // entity.customerservicecontract.servicecontractcode
            new TranslationSeedItem("entity.customerservicecontract.servicecontractcode", "zh-CN", "服务合同编码", "服务合同编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务合同编码规则生成并展示，非手输；单据类型菜单：服务合同）"),
            // entity.customerservicecontract.servicecontractcode
            new TranslationSeedItem("entity.customerservicecontract.servicecontractcode", "zh-HK", "服务合同编码_hk", "服务合同编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 服务合同编码规则生成并展示，非手输；单据类型菜单：服务合同）"),

            // entity.customerservicecontract.contractname
            new TranslationSeedItem("entity.customerservicecontract.contractname", "en-US", "合同名称_us", "合同名称"),
            // entity.customerservicecontract.contractname
            new TranslationSeedItem("entity.customerservicecontract.contractname", "ja-JP", "合同名称_jp", "合同名称"),
            // entity.customerservicecontract.contractname
            new TranslationSeedItem("entity.customerservicecontract.contractname", "zh-CN", "合同名称", "合同名称"),
            // entity.customerservicecontract.contractname
            new TranslationSeedItem("entity.customerservicecontract.contractname", "zh-HK", "合同名称_hk", "合同名称"),

            // entity.customerservicecontract.clientid
            new TranslationSeedItem("entity.customerservicecontract.clientid", "en-US", "客户端ID_us", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicecontract.clientid
            new TranslationSeedItem("entity.customerservicecontract.clientid", "ja-JP", "客户端ID_jp", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicecontract.clientid
            new TranslationSeedItem("entity.customerservicecontract.clientid", "zh-CN", "客户端ID", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),
            // entity.customerservicecontract.clientid
            new TranslationSeedItem("entity.customerservicecontract.clientid", "zh-HK", "客户端ID_hk", "客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）"),

            // entity.customerservicecontract.clientcode
            new TranslationSeedItem("entity.customerservicecontract.clientcode", "en-US", "客户端编码_us", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientcode
            new TranslationSeedItem("entity.customerservicecontract.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientcode
            new TranslationSeedItem("entity.customerservicecontract.clientcode", "zh-CN", "客户端编码", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientcode
            new TranslationSeedItem("entity.customerservicecontract.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerservicecontract.clientname1
            new TranslationSeedItem("entity.customerservicecontract.clientname1", "en-US", "客户端名称1_us", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientname1
            new TranslationSeedItem("entity.customerservicecontract.clientname1", "ja-JP", "客户端名称1_jp", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientname1
            new TranslationSeedItem("entity.customerservicecontract.clientname1", "zh-CN", "客户端名称1", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customerservicecontract.clientname1
            new TranslationSeedItem("entity.customerservicecontract.clientname1", "zh-HK", "客户端名称1_hk", "客户端名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customerservicecontract.contracttype
            new TranslationSeedItem("entity.customerservicecontract.contracttype", "en-US", "合同类型_us", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.customerservicecontract.contracttype
            new TranslationSeedItem("entity.customerservicecontract.contracttype", "ja-JP", "合同类型_jp", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.customerservicecontract.contracttype
            new TranslationSeedItem("entity.customerservicecontract.contracttype", "zh-CN", "合同类型", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),
            // entity.customerservicecontract.contracttype
            new TranslationSeedItem("entity.customerservicecontract.contracttype", "zh-HK", "合同类型_hk", "合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）"),

            // entity.customerservicecontract.contractstatus
            new TranslationSeedItem("entity.customerservicecontract.contractstatus", "en-US", "合同状态_us", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.customerservicecontract.contractstatus
            new TranslationSeedItem("entity.customerservicecontract.contractstatus", "ja-JP", "合同状态_jp", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.customerservicecontract.contractstatus
            new TranslationSeedItem("entity.customerservicecontract.contractstatus", "zh-CN", "合同状态", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),
            // entity.customerservicecontract.contractstatus
            new TranslationSeedItem("entity.customerservicecontract.contractstatus", "zh-HK", "合同状态_hk", "合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）"),

            // entity.customerservicecontract.signdate
            new TranslationSeedItem("entity.customerservicecontract.signdate", "en-US", "签订日期_us", "签订日期"),
            // entity.customerservicecontract.signdate
            new TranslationSeedItem("entity.customerservicecontract.signdate", "ja-JP", "签订日期_jp", "签订日期"),
            // entity.customerservicecontract.signdate
            new TranslationSeedItem("entity.customerservicecontract.signdate", "zh-CN", "签订日期", "签订日期"),
            // entity.customerservicecontract.signdate
            new TranslationSeedItem("entity.customerservicecontract.signdate", "zh-HK", "签订日期_hk", "签订日期"),

            // entity.customerservicecontract.effectivedate
            new TranslationSeedItem("entity.customerservicecontract.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.customerservicecontract.effectivedate
            new TranslationSeedItem("entity.customerservicecontract.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.customerservicecontract.effectivedate
            new TranslationSeedItem("entity.customerservicecontract.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.customerservicecontract.effectivedate
            new TranslationSeedItem("entity.customerservicecontract.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.customerservicecontract.expirydate
            new TranslationSeedItem("entity.customerservicecontract.expirydate", "en-US", "到期日期_us", "到期日期"),
            // entity.customerservicecontract.expirydate
            new TranslationSeedItem("entity.customerservicecontract.expirydate", "ja-JP", "到期日期_jp", "到期日期"),
            // entity.customerservicecontract.expirydate
            new TranslationSeedItem("entity.customerservicecontract.expirydate", "zh-CN", "到期日期", "到期日期"),
            // entity.customerservicecontract.expirydate
            new TranslationSeedItem("entity.customerservicecontract.expirydate", "zh-HK", "到期日期_hk", "到期日期"),

            // entity.customerservicecontract.contractamount
            new TranslationSeedItem("entity.customerservicecontract.contractamount", "en-US", "合同金额_us", "合同金额"),
            // entity.customerservicecontract.contractamount
            new TranslationSeedItem("entity.customerservicecontract.contractamount", "ja-JP", "合同金额_jp", "合同金额"),
            // entity.customerservicecontract.contractamount
            new TranslationSeedItem("entity.customerservicecontract.contractamount", "zh-CN", "合同金额", "合同金额"),
            // entity.customerservicecontract.contractamount
            new TranslationSeedItem("entity.customerservicecontract.contractamount", "zh-HK", "合同金额_hk", "合同金额"),

            // entity.customerservicecontract.currencycode
            new TranslationSeedItem("entity.customerservicecontract.currencycode", "en-US", "结算币种代码_us", "结算币种代码"),
            // entity.customerservicecontract.currencycode
            new TranslationSeedItem("entity.customerservicecontract.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码"),
            // entity.customerservicecontract.currencycode
            new TranslationSeedItem("entity.customerservicecontract.currencycode", "zh-CN", "结算币种代码", "结算币种代码"),
            // entity.customerservicecontract.currencycode
            new TranslationSeedItem("entity.customerservicecontract.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码"),

            // entity.customerservicecontract.paymentterms
            new TranslationSeedItem("entity.customerservicecontract.paymentterms", "en-US", "付款条件_us", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.customerservicecontract.paymentterms
            new TranslationSeedItem("entity.customerservicecontract.paymentterms", "ja-JP", "付款条件_jp", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.customerservicecontract.paymentterms
            new TranslationSeedItem("entity.customerservicecontract.paymentterms", "zh-CN", "付款条件", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),
            // entity.customerservicecontract.paymentterms
            new TranslationSeedItem("entity.customerservicecontract.paymentterms", "zh-HK", "付款条件_hk", "付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）"),

            // entity.customerservicecontract.servicescope
            new TranslationSeedItem("entity.customerservicecontract.servicescope", "en-US", "服务范围描述_us", "服务范围描述"),
            // entity.customerservicecontract.servicescope
            new TranslationSeedItem("entity.customerservicecontract.servicescope", "ja-JP", "服务范围描述_jp", "服务范围描述"),
            // entity.customerservicecontract.servicescope
            new TranslationSeedItem("entity.customerservicecontract.servicescope", "zh-CN", "服务范围描述", "服务范围描述"),
            // entity.customerservicecontract.servicescope
            new TranslationSeedItem("entity.customerservicecontract.servicescope", "zh-HK", "服务范围描述_hk", "服务范围描述"),

            // entity.customerservicecontract.slaresponsehours
            new TranslationSeedItem("entity.customerservicecontract.slaresponsehours", "en-US", "SLA响应时限（小时）_us", "SLA 响应时限（小时）"),
            // entity.customerservicecontract.slaresponsehours
            new TranslationSeedItem("entity.customerservicecontract.slaresponsehours", "ja-JP", "SLA响应时限（小时）_jp", "SLA 响应时限（小时）"),
            // entity.customerservicecontract.slaresponsehours
            new TranslationSeedItem("entity.customerservicecontract.slaresponsehours", "zh-CN", "SLA响应时限（小时）", "SLA 响应时限（小时）"),
            // entity.customerservicecontract.slaresponsehours
            new TranslationSeedItem("entity.customerservicecontract.slaresponsehours", "zh-HK", "SLA响应时限（小时）_hk", "SLA 响应时限（小时）"),

            // entity.customerservicecontract.slaresolvehours
            new TranslationSeedItem("entity.customerservicecontract.slaresolvehours", "en-US", "SLA解决时限（小时）_us", "SLA 解决时限（小时）"),
            // entity.customerservicecontract.slaresolvehours
            new TranslationSeedItem("entity.customerservicecontract.slaresolvehours", "ja-JP", "SLA解决时限（小时）_jp", "SLA 解决时限（小时）"),
            // entity.customerservicecontract.slaresolvehours
            new TranslationSeedItem("entity.customerservicecontract.slaresolvehours", "zh-CN", "SLA解决时限（小时）", "SLA 解决时限（小时）"),
            // entity.customerservicecontract.slaresolvehours
            new TranslationSeedItem("entity.customerservicecontract.slaresolvehours", "zh-HK", "SLA解决时限（小时）_hk", "SLA 解决时限（小时）"),

            // entity.customerservicecontract.accountmanageremployeeid
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeeid", "en-US", "客户经理员工ID_us", "客户经理（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerservicecontract.accountmanageremployeeid
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeeid", "ja-JP", "客户经理员工ID_jp", "客户经理（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerservicecontract.accountmanageremployeeid
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeeid", "zh-CN", "客户经理员工ID", "客户经理（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.customerservicecontract.accountmanageremployeeid
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeeid", "zh-HK", "客户经理员工ID_hk", "客户经理（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.customerservicecontract.accountmanageremployeename
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeename", "en-US", "客户经理名称_us", "客户经理名称（冗余：按 AccountManagerEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerservicecontract.accountmanageremployeename
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeename", "ja-JP", "客户经理名称_jp", "客户经理名称（冗余：按 AccountManagerEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerservicecontract.accountmanageremployeename
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeename", "zh-CN", "客户经理名称", "客户经理名称（冗余：按 AccountManagerEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.customerservicecontract.accountmanageremployeename
            new TranslationSeedItem("entity.customerservicecontract.accountmanageremployeename", "zh-HK", "客户经理名称_hk", "客户经理名称（冗余：按 AccountManagerEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.customerservicecontract.sortorder
            new TranslationSeedItem("entity.customerservicecontract.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customerservicecontract.sortorder
            new TranslationSeedItem("entity.customerservicecontract.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customerservicecontract.sortorder
            new TranslationSeedItem("entity.customerservicecontract.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customerservicecontract.sortorder
            new TranslationSeedItem("entity.customerservicecontract.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customerservicecontract.serviceorders
            new TranslationSeedItem("entity.customerservicecontract.serviceorders", "en-US", "服务订单列表_us", "服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceContractId）"),
            // entity.customerservicecontract.serviceorders
            new TranslationSeedItem("entity.customerservicecontract.serviceorders", "ja-JP", "服务订单列表_jp", "服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceContractId）"),
            // entity.customerservicecontract.serviceorders
            new TranslationSeedItem("entity.customerservicecontract.serviceorders", "zh-CN", "服务订单列表", "服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceContractId）"),
            // entity.customerservicecontract.serviceorders
            new TranslationSeedItem("entity.customerservicecontract.serviceorders", "zh-HK", "服务订单列表_hk", "服务订单列表（外键在子表 TaktCustomerServiceOrder.ServiceContractId）"),

            // entity.customerservicecontract.servicerequests
            new TranslationSeedItem("entity.customerservicecontract.servicerequests", "en-US", "服务请求列表_us", "服务请求列表（外键在子表 TaktCustomerServiceRequest.ServiceContractId）"),
            // entity.customerservicecontract.servicerequests
            new TranslationSeedItem("entity.customerservicecontract.servicerequests", "ja-JP", "服务请求列表_jp", "服务请求列表（外键在子表 TaktCustomerServiceRequest.ServiceContractId）"),
            // entity.customerservicecontract.servicerequests
            new TranslationSeedItem("entity.customerservicecontract.servicerequests", "zh-CN", "服务请求列表", "服务请求列表（外键在子表 TaktCustomerServiceRequest.ServiceContractId）"),
            // entity.customerservicecontract.servicerequests
            new TranslationSeedItem("entity.customerservicecontract.servicerequests", "zh-HK", "服务请求列表_hk", "服务请求列表（外键在子表 TaktCustomerServiceRequest.ServiceContractId）"),
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
