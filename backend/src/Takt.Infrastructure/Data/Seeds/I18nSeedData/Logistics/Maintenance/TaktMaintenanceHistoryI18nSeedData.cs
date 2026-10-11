// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoryI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenanceHistory 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaintenanceHistory 实体国际化翻译种子（键前缀 entity.maintenancehistory.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceHistoryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenanceHistory 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenancehistory 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceHistoryTranslations())
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

        TaktLogger.Information("TaktMaintenanceHistory 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenanceHistory 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenancehistory._self / entity.maintenancehistory.{{field}}；ResourceGroup=Maintenance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceHistoryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenancehistory._self
            new TranslationSeedItem("entity.maintenancehistory._self", "en-US", "Maintenance History Information_us", "实体名称"),
            // entity.maintenancehistory._self
            new TranslationSeedItem("entity.maintenancehistory._self", "ja-JP", "设备维护履历信息_jp", "实体名称"),
            // entity.maintenancehistory._self
            new TranslationSeedItem("entity.maintenancehistory._self", "zh-CN", "设备维护履历信息", "实体名称"),
            // entity.maintenancehistory._self
            new TranslationSeedItem("entity.maintenancehistory._self", "zh-HK", "设备维护履历信息_hk", "实体名称"),

            // entity.maintenancehistory.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorderid", "en-US", "来源维护工单ID_us", "来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorderid", "ja-JP", "来源维护工单ID_jp", "来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorderid", "zh-CN", "来源维护工单ID", "来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorderid", "zh-HK", "来源维护工单ID_hk", "来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）"),

            // entity.maintenancehistory.workordercode
            new TranslationSeedItem("entity.maintenancehistory.workordercode", "en-US", "来源维护工单号_us", "来源维护工单号（冗余）"),
            // entity.maintenancehistory.workordercode
            new TranslationSeedItem("entity.maintenancehistory.workordercode", "ja-JP", "来源维护工单号_jp", "来源维护工单号（冗余）"),
            // entity.maintenancehistory.workordercode
            new TranslationSeedItem("entity.maintenancehistory.workordercode", "zh-CN", "来源维护工单号", "来源维护工单号（冗余）"),
            // entity.maintenancehistory.workordercode
            new TranslationSeedItem("entity.maintenancehistory.workordercode", "zh-HK", "来源维护工单号_hk", "来源维护工单号（冗余）"),

            // entity.maintenancehistory.equipmentid
            new TranslationSeedItem("entity.maintenancehistory.equipmentid", "en-US", "设备ID_us", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.equipmentid
            new TranslationSeedItem("entity.maintenancehistory.equipmentid", "ja-JP", "设备ID_jp", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.equipmentid
            new TranslationSeedItem("entity.maintenancehistory.equipmentid", "zh-CN", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancehistory.equipmentid
            new TranslationSeedItem("entity.maintenancehistory.equipmentid", "zh-HK", "设备ID_hk", "设备ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenancehistory.equipcode
            new TranslationSeedItem("entity.maintenancehistory.equipcode", "en-US", "设备编码_us", "设备编码（冗余字段,便于查询）"),
            // entity.maintenancehistory.equipcode
            new TranslationSeedItem("entity.maintenancehistory.equipcode", "ja-JP", "设备编码_jp", "设备编码（冗余字段,便于查询）"),
            // entity.maintenancehistory.equipcode
            new TranslationSeedItem("entity.maintenancehistory.equipcode", "zh-CN", "设备编码", "设备编码（冗余字段,便于查询）"),
            // entity.maintenancehistory.equipcode
            new TranslationSeedItem("entity.maintenancehistory.equipcode", "zh-HK", "设备编码_hk", "设备编码（冗余字段,便于查询）"),

            // entity.maintenancehistory.maintenancetype
            new TranslationSeedItem("entity.maintenancehistory.maintenancetype", "en-US", "维护类型_us", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenancehistory.maintenancetype
            new TranslationSeedItem("entity.maintenancehistory.maintenancetype", "ja-JP", "维护类型_jp", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenancehistory.maintenancetype
            new TranslationSeedItem("entity.maintenancehistory.maintenancetype", "zh-CN", "维护类型", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenancehistory.maintenancetype
            new TranslationSeedItem("entity.maintenancehistory.maintenancetype", "zh-HK", "维护类型_hk", "维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),

            // entity.maintenancehistory.maintenancecategory
            new TranslationSeedItem("entity.maintenancehistory.maintenancecategory", "en-US", "维护类别_us", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancehistory.maintenancecategory
            new TranslationSeedItem("entity.maintenancehistory.maintenancecategory", "ja-JP", "维护类别_jp", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancehistory.maintenancecategory
            new TranslationSeedItem("entity.maintenancehistory.maintenancecategory", "zh-CN", "维护类别", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancehistory.maintenancecategory
            new TranslationSeedItem("entity.maintenancehistory.maintenancecategory", "zh-HK", "维护类别_hk", "维护类别（字典 logistics_maintenance_category）"),

            // entity.maintenancehistory.maintenancecompany
            new TranslationSeedItem("entity.maintenancehistory.maintenancecompany", "en-US", "维护单位_us", "维护单位"),
            // entity.maintenancehistory.maintenancecompany
            new TranslationSeedItem("entity.maintenancehistory.maintenancecompany", "ja-JP", "维护单位_jp", "维护单位"),
            // entity.maintenancehistory.maintenancecompany
            new TranslationSeedItem("entity.maintenancehistory.maintenancecompany", "zh-CN", "维护单位", "维护单位"),
            // entity.maintenancehistory.maintenancecompany
            new TranslationSeedItem("entity.maintenancehistory.maintenancecompany", "zh-HK", "维护单位_hk", "维护单位"),

            // entity.maintenancehistory.maintenancetechnician
            new TranslationSeedItem("entity.maintenancehistory.maintenancetechnician", "en-US", "维护技师_us", "维护技师（人员编码）"),
            // entity.maintenancehistory.maintenancetechnician
            new TranslationSeedItem("entity.maintenancehistory.maintenancetechnician", "ja-JP", "维护技师_jp", "维护技师（人员编码）"),
            // entity.maintenancehistory.maintenancetechnician
            new TranslationSeedItem("entity.maintenancehistory.maintenancetechnician", "zh-CN", "维护技师", "维护技师（人员编码）"),
            // entity.maintenancehistory.maintenancetechnician
            new TranslationSeedItem("entity.maintenancehistory.maintenancetechnician", "zh-HK", "维护技师_hk", "维护技师（人员编码）"),

            // entity.maintenancehistory.maintenancedate
            new TranslationSeedItem("entity.maintenancehistory.maintenancedate", "en-US", "维护日期_us", "维护日期（归档基准日，通常取工单完工时间）"),
            // entity.maintenancehistory.maintenancedate
            new TranslationSeedItem("entity.maintenancehistory.maintenancedate", "ja-JP", "维护日期_jp", "维护日期（归档基准日，通常取工单完工时间）"),
            // entity.maintenancehistory.maintenancedate
            new TranslationSeedItem("entity.maintenancehistory.maintenancedate", "zh-CN", "维护日期", "维护日期（归档基准日，通常取工单完工时间）"),
            // entity.maintenancehistory.maintenancedate
            new TranslationSeedItem("entity.maintenancehistory.maintenancedate", "zh-HK", "维护日期_hk", "维护日期（归档基准日，通常取工单完工时间）"),

            // entity.maintenancehistory.maintenancestarttime
            new TranslationSeedItem("entity.maintenancehistory.maintenancestarttime", "en-US", "维护开始时间_us", "维护开始时间"),
            // entity.maintenancehistory.maintenancestarttime
            new TranslationSeedItem("entity.maintenancehistory.maintenancestarttime", "ja-JP", "维护开始时间_jp", "维护开始时间"),
            // entity.maintenancehistory.maintenancestarttime
            new TranslationSeedItem("entity.maintenancehistory.maintenancestarttime", "zh-CN", "维护开始时间", "维护开始时间"),
            // entity.maintenancehistory.maintenancestarttime
            new TranslationSeedItem("entity.maintenancehistory.maintenancestarttime", "zh-HK", "维护开始时间_hk", "维护开始时间"),

            // entity.maintenancehistory.maintenanceendtime
            new TranslationSeedItem("entity.maintenancehistory.maintenanceendtime", "en-US", "维护结束时间_us", "维护结束时间"),
            // entity.maintenancehistory.maintenanceendtime
            new TranslationSeedItem("entity.maintenancehistory.maintenanceendtime", "ja-JP", "维护结束时间_jp", "维护结束时间"),
            // entity.maintenancehistory.maintenanceendtime
            new TranslationSeedItem("entity.maintenancehistory.maintenanceendtime", "zh-CN", "维护结束时间", "维护结束时间"),
            // entity.maintenancehistory.maintenanceendtime
            new TranslationSeedItem("entity.maintenancehistory.maintenanceendtime", "zh-HK", "维护结束时间_hk", "维护结束时间"),

            // entity.maintenancehistory.maintenancecontent
            new TranslationSeedItem("entity.maintenancehistory.maintenancecontent", "en-US", "维护内容_us", "维护内容描述"),
            // entity.maintenancehistory.maintenancecontent
            new TranslationSeedItem("entity.maintenancehistory.maintenancecontent", "ja-JP", "维护内容_jp", "维护内容描述"),
            // entity.maintenancehistory.maintenancecontent
            new TranslationSeedItem("entity.maintenancehistory.maintenancecontent", "zh-CN", "维护内容", "维护内容描述"),
            // entity.maintenancehistory.maintenancecontent
            new TranslationSeedItem("entity.maintenancehistory.maintenancecontent", "zh-HK", "维护内容_hk", "维护内容描述"),

            // entity.maintenancehistory.faultdescription
            new TranslationSeedItem("entity.maintenancehistory.faultdescription", "en-US", "故障描述_us", "故障描述"),
            // entity.maintenancehistory.faultdescription
            new TranslationSeedItem("entity.maintenancehistory.faultdescription", "ja-JP", "故障描述_jp", "故障描述"),
            // entity.maintenancehistory.faultdescription
            new TranslationSeedItem("entity.maintenancehistory.faultdescription", "zh-CN", "故障描述", "故障描述"),
            // entity.maintenancehistory.faultdescription
            new TranslationSeedItem("entity.maintenancehistory.faultdescription", "zh-HK", "故障描述_hk", "故障描述"),

            // entity.maintenancehistory.solution
            new TranslationSeedItem("entity.maintenancehistory.solution", "en-US", "处理方案_us", "处理方案"),
            // entity.maintenancehistory.solution
            new TranslationSeedItem("entity.maintenancehistory.solution", "ja-JP", "处理方案_jp", "处理方案"),
            // entity.maintenancehistory.solution
            new TranslationSeedItem("entity.maintenancehistory.solution", "zh-CN", "处理方案", "处理方案"),
            // entity.maintenancehistory.solution
            new TranslationSeedItem("entity.maintenancehistory.solution", "zh-HK", "处理方案_hk", "处理方案"),

            // entity.maintenancehistory.usedparts
            new TranslationSeedItem("entity.maintenancehistory.usedparts", "en-US", "使用配件_us", "使用配件（JSON，由工单领料明细汇总）"),
            // entity.maintenancehistory.usedparts
            new TranslationSeedItem("entity.maintenancehistory.usedparts", "ja-JP", "使用配件_jp", "使用配件（JSON，由工单领料明细汇总）"),
            // entity.maintenancehistory.usedparts
            new TranslationSeedItem("entity.maintenancehistory.usedparts", "zh-CN", "使用配件", "使用配件（JSON，由工单领料明细汇总）"),
            // entity.maintenancehistory.usedparts
            new TranslationSeedItem("entity.maintenancehistory.usedparts", "zh-HK", "使用配件_hk", "使用配件（JSON，由工单领料明细汇总）"),

            // entity.maintenancehistory.maintenancecost
            new TranslationSeedItem("entity.maintenancehistory.maintenancecost", "en-US", "维护费用_us", "维护费用（工单总成本快照）"),
            // entity.maintenancehistory.maintenancecost
            new TranslationSeedItem("entity.maintenancehistory.maintenancecost", "ja-JP", "维护费用_jp", "维护费用（工单总成本快照）"),
            // entity.maintenancehistory.maintenancecost
            new TranslationSeedItem("entity.maintenancehistory.maintenancecost", "zh-CN", "维护费用", "维护费用（工单总成本快照）"),
            // entity.maintenancehistory.maintenancecost
            new TranslationSeedItem("entity.maintenancehistory.maintenancecost", "zh-HK", "维护费用_hk", "维护费用（工单总成本快照）"),

            // entity.maintenancehistory.maintenanceresult
            new TranslationSeedItem("entity.maintenancehistory.maintenanceresult", "en-US", "维护结果_us", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenancehistory.maintenanceresult
            new TranslationSeedItem("entity.maintenancehistory.maintenanceresult", "ja-JP", "维护结果_jp", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenancehistory.maintenanceresult
            new TranslationSeedItem("entity.maintenancehistory.maintenanceresult", "zh-CN", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenancehistory.maintenanceresult
            new TranslationSeedItem("entity.maintenancehistory.maintenanceresult", "zh-HK", "维护结果_hk", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),

            // entity.maintenancehistory.maintenancestatus
            new TranslationSeedItem("entity.maintenancehistory.maintenancestatus", "en-US", "履历状态_us", "履历状态（固定为 2=已完成，归档写入）"),
            // entity.maintenancehistory.maintenancestatus
            new TranslationSeedItem("entity.maintenancehistory.maintenancestatus", "ja-JP", "履历状态_jp", "履历状态（固定为 2=已完成，归档写入）"),
            // entity.maintenancehistory.maintenancestatus
            new TranslationSeedItem("entity.maintenancehistory.maintenancestatus", "zh-CN", "履历状态", "履历状态（固定为 2=已完成，归档写入）"),
            // entity.maintenancehistory.maintenancestatus
            new TranslationSeedItem("entity.maintenancehistory.maintenancestatus", "zh-HK", "履历状态_hk", "履历状态（固定为 2=已完成，归档写入）"),

            // entity.maintenancehistory.nextmaintenancedate
            new TranslationSeedItem("entity.maintenancehistory.nextmaintenancedate", "en-US", "下次维护日期_us", "下次维护日期"),
            // entity.maintenancehistory.nextmaintenancedate
            new TranslationSeedItem("entity.maintenancehistory.nextmaintenancedate", "ja-JP", "下次维护日期_jp", "下次维护日期"),
            // entity.maintenancehistory.nextmaintenancedate
            new TranslationSeedItem("entity.maintenancehistory.nextmaintenancedate", "zh-CN", "下次维护日期", "下次维护日期"),
            // entity.maintenancehistory.nextmaintenancedate
            new TranslationSeedItem("entity.maintenancehistory.nextmaintenancedate", "zh-HK", "下次维护日期_hk", "下次维护日期"),

            // entity.maintenancehistory.maintenancecycledays
            new TranslationSeedItem("entity.maintenancehistory.maintenancecycledays", "en-US", "维护周期（天）_us", "维护周期（天）"),
            // entity.maintenancehistory.maintenancecycledays
            new TranslationSeedItem("entity.maintenancehistory.maintenancecycledays", "ja-JP", "维护周期（天）_jp", "维护周期（天）"),
            // entity.maintenancehistory.maintenancecycledays
            new TranslationSeedItem("entity.maintenancehistory.maintenancecycledays", "zh-CN", "维护周期（天）", "维护周期（天）"),
            // entity.maintenancehistory.maintenancecycledays
            new TranslationSeedItem("entity.maintenancehistory.maintenancecycledays", "zh-HK", "维护周期（天）_hk", "维护周期（天）"),

            // entity.maintenancehistory.maintenancedocuments
            new TranslationSeedItem("entity.maintenancehistory.maintenancedocuments", "en-US", "维护文档_us", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenancehistory.maintenancedocuments
            new TranslationSeedItem("entity.maintenancehistory.maintenancedocuments", "ja-JP", "维护文档_jp", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenancehistory.maintenancedocuments
            new TranslationSeedItem("entity.maintenancehistory.maintenancedocuments", "zh-CN", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenancehistory.maintenancedocuments
            new TranslationSeedItem("entity.maintenancehistory.maintenancedocuments", "zh-HK", "维护文档_hk", "维护文档（JSON格式，存储维护文档ID列表）"),

            // entity.maintenancehistory.maintenanceimages
            new TranslationSeedItem("entity.maintenancehistory.maintenanceimages", "en-US", "维护图片_us", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenancehistory.maintenanceimages
            new TranslationSeedItem("entity.maintenancehistory.maintenanceimages", "ja-JP", "维护图片_jp", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenancehistory.maintenanceimages
            new TranslationSeedItem("entity.maintenancehistory.maintenanceimages", "zh-CN", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenancehistory.maintenanceimages
            new TranslationSeedItem("entity.maintenancehistory.maintenanceimages", "zh-HK", "维护图片_hk", "维护图片（JSON格式，存储维护图片URL列表）"),

            // entity.maintenancehistory.acceptedsummary
            new TranslationSeedItem("entity.maintenancehistory.acceptedsummary", "en-US", "验收总结_us", "验收总结"),
            // entity.maintenancehistory.acceptedsummary
            new TranslationSeedItem("entity.maintenancehistory.acceptedsummary", "ja-JP", "验收总结_jp", "验收总结"),
            // entity.maintenancehistory.acceptedsummary
            new TranslationSeedItem("entity.maintenancehistory.acceptedsummary", "zh-CN", "验收总结", "验收总结"),
            // entity.maintenancehistory.acceptedsummary
            new TranslationSeedItem("entity.maintenancehistory.acceptedsummary", "zh-HK", "验收总结_hk", "验收总结"),

            // entity.maintenancehistory.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeeid", "en-US", "验收人员工ID_us", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancehistory.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeeid", "ja-JP", "验收人员工ID_jp", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancehistory.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeeid", "zh-CN", "验收人员工ID", "验收人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancehistory.acceptedbyemployeeid
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeeid", "zh-HK", "验收人员工ID_hk", "验收人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.maintenancehistory.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeename", "en-US", "验收人名称_us", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancehistory.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeename", "ja-JP", "验收人名称_jp", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancehistory.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeename", "zh-CN", "验收人名称", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancehistory.acceptedbyemployeename
            new TranslationSeedItem("entity.maintenancehistory.acceptedbyemployeename", "zh-HK", "验收人名称_hk", "验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.maintenancehistory.acceptedat
            new TranslationSeedItem("entity.maintenancehistory.acceptedat", "en-US", "验收时间_us", "验收时间"),
            // entity.maintenancehistory.acceptedat
            new TranslationSeedItem("entity.maintenancehistory.acceptedat", "ja-JP", "验收时间_jp", "验收时间"),
            // entity.maintenancehistory.acceptedat
            new TranslationSeedItem("entity.maintenancehistory.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.maintenancehistory.acceptedat
            new TranslationSeedItem("entity.maintenancehistory.acceptedat", "zh-HK", "验收时间_hk", "验收时间"),

            // entity.maintenancehistory.archivedat
            new TranslationSeedItem("entity.maintenancehistory.archivedat", "en-US", "归档时间_us", "归档时间"),
            // entity.maintenancehistory.archivedat
            new TranslationSeedItem("entity.maintenancehistory.archivedat", "ja-JP", "归档时间_jp", "归档时间"),
            // entity.maintenancehistory.archivedat
            new TranslationSeedItem("entity.maintenancehistory.archivedat", "zh-CN", "归档时间", "归档时间"),
            // entity.maintenancehistory.archivedat
            new TranslationSeedItem("entity.maintenancehistory.archivedat", "zh-HK", "归档时间_hk", "归档时间"),

            // entity.maintenancehistory.equipment
            new TranslationSeedItem("entity.maintenancehistory.equipment", "en-US", "设备_us", "设备（主表）"),
            // entity.maintenancehistory.equipment
            new TranslationSeedItem("entity.maintenancehistory.equipment", "ja-JP", "设备_jp", "设备（主表）"),
            // entity.maintenancehistory.equipment
            new TranslationSeedItem("entity.maintenancehistory.equipment", "zh-CN", "设备", "设备（主表）"),
            // entity.maintenancehistory.equipment
            new TranslationSeedItem("entity.maintenancehistory.equipment", "zh-HK", "设备_hk", "设备（主表）"),

            // entity.maintenancehistory.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorder", "en-US", "来源维护工单_us", "来源维护工单"),
            // entity.maintenancehistory.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorder", "ja-JP", "来源维护工单_jp", "来源维护工单"),
            // entity.maintenancehistory.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorder", "zh-CN", "来源维护工单", "来源维护工单"),
            // entity.maintenancehistory.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancehistory.maintenanceworkorder", "zh-HK", "来源维护工单_hk", "来源维护工单"),
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
