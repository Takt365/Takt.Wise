// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotificationI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenanceNotification 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaintenanceNotification 实体国际化翻译种子（键前缀 entity.maintenancenotification.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceNotificationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenanceNotification 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenancenotification 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceNotificationTranslations())
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

        TaktLogger.Information("TaktMaintenanceNotification 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenanceNotification 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenancenotification._self / entity.maintenancenotification.{{field}}；ResourceGroup=Maintenance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceNotificationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenancenotification._self
            new TranslationSeedItem("entity.maintenancenotification._self", "en-US", "Maintenance Notification Information_us", "实体名称"),
            // entity.maintenancenotification._self
            new TranslationSeedItem("entity.maintenancenotification._self", "ja-JP", "维护通知单信息_jp", "实体名称"),
            // entity.maintenancenotification._self
            new TranslationSeedItem("entity.maintenancenotification._self", "zh-CN", "维护通知单信息", "实体名称"),
            // entity.maintenancenotification._self
            new TranslationSeedItem("entity.maintenancenotification._self", "zh-HK", "维护通知单信息_hk", "实体名称"),

            // entity.maintenancenotification.notificationcode
            new TranslationSeedItem("entity.maintenancenotification.notificationcode", "en-US", "通知单号_us", "通知单号"),
            // entity.maintenancenotification.notificationcode
            new TranslationSeedItem("entity.maintenancenotification.notificationcode", "ja-JP", "通知单号_jp", "通知单号"),
            // entity.maintenancenotification.notificationcode
            new TranslationSeedItem("entity.maintenancenotification.notificationcode", "zh-CN", "通知单号", "通知单号"),
            // entity.maintenancenotification.notificationcode
            new TranslationSeedItem("entity.maintenancenotification.notificationcode", "zh-HK", "通知单号_hk", "通知单号"),

            // entity.maintenancenotification.equipmentid
            new TranslationSeedItem("entity.maintenancenotification.equipmentid", "en-US", "设备ID_us", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.equipmentid
            new TranslationSeedItem("entity.maintenancenotification.equipmentid", "ja-JP", "设备ID_jp", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.equipmentid
            new TranslationSeedItem("entity.maintenancenotification.equipmentid", "zh-CN", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.equipmentid
            new TranslationSeedItem("entity.maintenancenotification.equipmentid", "zh-HK", "设备ID_hk", "设备ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenancenotification.equipcode
            new TranslationSeedItem("entity.maintenancenotification.equipcode", "en-US", "设备编码_us", "设备编码（冗余，便于查询）"),
            // entity.maintenancenotification.equipcode
            new TranslationSeedItem("entity.maintenancenotification.equipcode", "ja-JP", "设备编码_jp", "设备编码（冗余，便于查询）"),
            // entity.maintenancenotification.equipcode
            new TranslationSeedItem("entity.maintenancenotification.equipcode", "zh-CN", "设备编码", "设备编码（冗余，便于查询）"),
            // entity.maintenancenotification.equipcode
            new TranslationSeedItem("entity.maintenancenotification.equipcode", "zh-HK", "设备编码_hk", "设备编码（冗余，便于查询）"),

            // entity.maintenancenotification.equipmentname
            new TranslationSeedItem("entity.maintenancenotification.equipmentname", "en-US", "设备名称_us", "设备名称（冗余）"),
            // entity.maintenancenotification.equipmentname
            new TranslationSeedItem("entity.maintenancenotification.equipmentname", "ja-JP", "设备名称_jp", "设备名称（冗余）"),
            // entity.maintenancenotification.equipmentname
            new TranslationSeedItem("entity.maintenancenotification.equipmentname", "zh-CN", "设备名称", "设备名称（冗余）"),
            // entity.maintenancenotification.equipmentname
            new TranslationSeedItem("entity.maintenancenotification.equipmentname", "zh-HK", "设备名称_hk", "设备名称（冗余）"),

            // entity.maintenancenotification.maintenancecategory
            new TranslationSeedItem("entity.maintenancenotification.maintenancecategory", "en-US", "维护类别_us", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancenotification.maintenancecategory
            new TranslationSeedItem("entity.maintenancenotification.maintenancecategory", "ja-JP", "维护类别_jp", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancenotification.maintenancecategory
            new TranslationSeedItem("entity.maintenancenotification.maintenancecategory", "zh-CN", "维护类别", "维护类别（字典 logistics_maintenance_category）"),
            // entity.maintenancenotification.maintenancecategory
            new TranslationSeedItem("entity.maintenancenotification.maintenancecategory", "zh-HK", "维护类别_hk", "维护类别（字典 logistics_maintenance_category）"),

            // entity.maintenancenotification.priority
            new TranslationSeedItem("entity.maintenancenotification.priority", "en-US", "优先级_us", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenancenotification.priority
            new TranslationSeedItem("entity.maintenancenotification.priority", "ja-JP", "优先级_jp", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenancenotification.priority
            new TranslationSeedItem("entity.maintenancenotification.priority", "zh-CN", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.maintenancenotification.priority
            new TranslationSeedItem("entity.maintenancenotification.priority", "zh-HK", "优先级_hk", "优先级（1=低，2=中，3=高，4=紧急）"),

            // entity.maintenancenotification.notificationstatus
            new TranslationSeedItem("entity.maintenancenotification.notificationstatus", "en-US", "通知单状态_us", "通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）"),
            // entity.maintenancenotification.notificationstatus
            new TranslationSeedItem("entity.maintenancenotification.notificationstatus", "ja-JP", "通知单状态_jp", "通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）"),
            // entity.maintenancenotification.notificationstatus
            new TranslationSeedItem("entity.maintenancenotification.notificationstatus", "zh-CN", "通知单状态", "通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）"),
            // entity.maintenancenotification.notificationstatus
            new TranslationSeedItem("entity.maintenancenotification.notificationstatus", "zh-HK", "通知单状态_hk", "通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）"),

            // entity.maintenancenotification.faultdescription
            new TranslationSeedItem("entity.maintenancenotification.faultdescription", "en-US", "异常描述_us", "异常/故障描述"),
            // entity.maintenancenotification.faultdescription
            new TranslationSeedItem("entity.maintenancenotification.faultdescription", "ja-JP", "异常描述_jp", "异常/故障描述"),
            // entity.maintenancenotification.faultdescription
            new TranslationSeedItem("entity.maintenancenotification.faultdescription", "zh-CN", "异常描述", "异常/故障描述"),
            // entity.maintenancenotification.faultdescription
            new TranslationSeedItem("entity.maintenancenotification.faultdescription", "zh-HK", "异常描述_hk", "异常/故障描述"),

            // entity.maintenancenotification.discoveredat
            new TranslationSeedItem("entity.maintenancenotification.discoveredat", "en-US", "发现时间_us", "发现时间"),
            // entity.maintenancenotification.discoveredat
            new TranslationSeedItem("entity.maintenancenotification.discoveredat", "ja-JP", "发现时间_jp", "发现时间"),
            // entity.maintenancenotification.discoveredat
            new TranslationSeedItem("entity.maintenancenotification.discoveredat", "zh-CN", "发现时间", "发现时间"),
            // entity.maintenancenotification.discoveredat
            new TranslationSeedItem("entity.maintenancenotification.discoveredat", "zh-HK", "发现时间_hk", "发现时间"),

            // entity.maintenancenotification.breakdownstarttime
            new TranslationSeedItem("entity.maintenancenotification.breakdownstarttime", "en-US", "故障开始时间_us", "故障开始时间"),
            // entity.maintenancenotification.breakdownstarttime
            new TranslationSeedItem("entity.maintenancenotification.breakdownstarttime", "ja-JP", "故障开始时间_jp", "故障开始时间"),
            // entity.maintenancenotification.breakdownstarttime
            new TranslationSeedItem("entity.maintenancenotification.breakdownstarttime", "zh-CN", "故障开始时间", "故障开始时间"),
            // entity.maintenancenotification.breakdownstarttime
            new TranslationSeedItem("entity.maintenancenotification.breakdownstarttime", "zh-HK", "故障开始时间_hk", "故障开始时间"),

            // entity.maintenancenotification.breakdownendtime
            new TranslationSeedItem("entity.maintenancenotification.breakdownendtime", "en-US", "故障结束时间_us", "故障结束时间"),
            // entity.maintenancenotification.breakdownendtime
            new TranslationSeedItem("entity.maintenancenotification.breakdownendtime", "ja-JP", "故障结束时间_jp", "故障结束时间"),
            // entity.maintenancenotification.breakdownendtime
            new TranslationSeedItem("entity.maintenancenotification.breakdownendtime", "zh-CN", "故障结束时间", "故障结束时间"),
            // entity.maintenancenotification.breakdownendtime
            new TranslationSeedItem("entity.maintenancenotification.breakdownendtime", "zh-HK", "故障结束时间_hk", "故障结束时间"),

            // entity.maintenancenotification.reportedbyemployeeid
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeeid", "en-US", "报告人员工ID_us", "报告人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancenotification.reportedbyemployeeid
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeeid", "ja-JP", "报告人员工ID_jp", "报告人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancenotification.reportedbyemployeeid
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeeid", "zh-CN", "报告人员工ID", "报告人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.maintenancenotification.reportedbyemployeeid
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeeid", "zh-HK", "报告人员工ID_hk", "报告人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.maintenancenotification.reportedbyemployeename
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeename", "en-US", "报告人名称_us", "报告人名称（冗余：按 ReportedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancenotification.reportedbyemployeename
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeename", "ja-JP", "报告人名称_jp", "报告人名称（冗余：按 ReportedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancenotification.reportedbyemployeename
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeename", "zh-CN", "报告人名称", "报告人名称（冗余：按 ReportedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.maintenancenotification.reportedbyemployeename
            new TranslationSeedItem("entity.maintenancenotification.reportedbyemployeename", "zh-HK", "报告人名称_hk", "报告人名称（冗余：按 ReportedByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.maintenancenotification.costcenterid
            new TranslationSeedItem("entity.maintenancenotification.costcenterid", "en-US", "责任成本中心ID_us", "责任成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.costcenterid
            new TranslationSeedItem("entity.maintenancenotification.costcenterid", "ja-JP", "责任成本中心ID_jp", "责任成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.costcenterid
            new TranslationSeedItem("entity.maintenancenotification.costcenterid", "zh-CN", "责任成本中心ID", "责任成本中心ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.costcenterid
            new TranslationSeedItem("entity.maintenancenotification.costcenterid", "zh-HK", "责任成本中心ID_hk", "责任成本中心ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenancenotification.costcentercode
            new TranslationSeedItem("entity.maintenancenotification.costcentercode", "en-US", "责任成本中心编码_us", "责任成本中心编码（冗余）"),
            // entity.maintenancenotification.costcentercode
            new TranslationSeedItem("entity.maintenancenotification.costcentercode", "ja-JP", "责任成本中心编码_jp", "责任成本中心编码（冗余）"),
            // entity.maintenancenotification.costcentercode
            new TranslationSeedItem("entity.maintenancenotification.costcentercode", "zh-CN", "责任成本中心编码", "责任成本中心编码（冗余）"),
            // entity.maintenancenotification.costcentercode
            new TranslationSeedItem("entity.maintenancenotification.costcentercode", "zh-HK", "责任成本中心编码_hk", "责任成本中心编码（冗余）"),

            // entity.maintenancenotification.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorderid", "en-US", "关联维护工单ID_us", "关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorderid", "ja-JP", "关联维护工单ID_jp", "关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorderid", "zh-CN", "关联维护工单ID", "关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）"),
            // entity.maintenancenotification.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorderid", "zh-HK", "关联维护工单ID_hk", "关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）"),

            // entity.maintenancenotification.maintenanceworkordercode
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkordercode", "en-US", "关联维护工单号_us", "关联维护工单号（冗余）"),
            // entity.maintenancenotification.maintenanceworkordercode
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkordercode", "ja-JP", "关联维护工单号_jp", "关联维护工单号（冗余）"),
            // entity.maintenancenotification.maintenanceworkordercode
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkordercode", "zh-CN", "关联维护工单号", "关联维护工单号（冗余）"),
            // entity.maintenancenotification.maintenanceworkordercode
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkordercode", "zh-HK", "关联维护工单号_hk", "关联维护工单号（冗余）"),

            // entity.maintenancenotification.notificationimages
            new TranslationSeedItem("entity.maintenancenotification.notificationimages", "en-US", "通知图片_us", "通知图片（JSON格式，存储图片URL列表）"),
            // entity.maintenancenotification.notificationimages
            new TranslationSeedItem("entity.maintenancenotification.notificationimages", "ja-JP", "通知图片_jp", "通知图片（JSON格式，存储图片URL列表）"),
            // entity.maintenancenotification.notificationimages
            new TranslationSeedItem("entity.maintenancenotification.notificationimages", "zh-CN", "通知图片", "通知图片（JSON格式，存储图片URL列表）"),
            // entity.maintenancenotification.notificationimages
            new TranslationSeedItem("entity.maintenancenotification.notificationimages", "zh-HK", "通知图片_hk", "通知图片（JSON格式，存储图片URL列表）"),

            // entity.maintenancenotification.equipment
            new TranslationSeedItem("entity.maintenancenotification.equipment", "en-US", "设备_us", "设备（主数据）"),
            // entity.maintenancenotification.equipment
            new TranslationSeedItem("entity.maintenancenotification.equipment", "ja-JP", "设备_jp", "设备（主数据）"),
            // entity.maintenancenotification.equipment
            new TranslationSeedItem("entity.maintenancenotification.equipment", "zh-CN", "设备", "设备（主数据）"),
            // entity.maintenancenotification.equipment
            new TranslationSeedItem("entity.maintenancenotification.equipment", "zh-HK", "设备_hk", "设备（主数据）"),

            // entity.maintenancenotification.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorder", "en-US", "关联维护工单_us", "关联维护工单"),
            // entity.maintenancenotification.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorder", "ja-JP", "关联维护工单_jp", "关联维护工单"),
            // entity.maintenancenotification.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorder", "zh-CN", "关联维护工单", "关联维护工单"),
            // entity.maintenancenotification.maintenanceworkorder
            new TranslationSeedItem("entity.maintenancenotification.maintenanceworkorder", "zh-HK", "关联维护工单_hk", "关联维护工单"),
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
