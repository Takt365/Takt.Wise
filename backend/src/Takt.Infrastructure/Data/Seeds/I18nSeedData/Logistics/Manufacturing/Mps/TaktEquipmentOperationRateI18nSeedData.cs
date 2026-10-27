// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktEquipmentOperationRateI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEquipmentOperationRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps;

/// <summary>
/// TaktEquipmentOperationRate 实体国际化翻译种子（键前缀 entity.equipmentoperationrate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEquipmentOperationRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEquipmentOperationRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 equipmentoperationrate 实体翻译...", tenantCode);

        foreach (var item in GetEquipmentOperationRateTranslations())
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

        TaktLogger.Information("TaktEquipmentOperationRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEquipmentOperationRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.equipmentoperationrate._self / entity.equipmentoperationrate.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEquipmentOperationRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.equipmentoperationrate._self
            new TranslationSeedItem("entity.equipmentoperationrate._self", "en-US", "Equipment Operation Rate Information_us", "实体名称"),
            // entity.equipmentoperationrate._self
            new TranslationSeedItem("entity.equipmentoperationrate._self", "ja-JP", "机器稼动率信息_jp", "实体名称"),
            // entity.equipmentoperationrate._self
            new TranslationSeedItem("entity.equipmentoperationrate._self", "zh-CN", "机器稼动率信息", "实体名称"),
            // entity.equipmentoperationrate._self
            new TranslationSeedItem("entity.equipmentoperationrate._self", "zh-HK", "机器稼动率信息_hk", "实体名称"),

            // entity.equipmentoperationrate.timecategory
            new TranslationSeedItem("entity.equipmentoperationrate.timecategory", "en-US", "时间类别_us", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentoperationrate.timecategory
            new TranslationSeedItem("entity.equipmentoperationrate.timecategory", "ja-JP", "时间类别_jp", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentoperationrate.timecategory
            new TranslationSeedItem("entity.equipmentoperationrate.timecategory", "zh-CN", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentoperationrate.timecategory
            new TranslationSeedItem("entity.equipmentoperationrate.timecategory", "zh-HK", "时间类别_hk", "时间类别（1=天，2=周，3=月）"),

            // entity.equipmentoperationrate.startdate
            new TranslationSeedItem("entity.equipmentoperationrate.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.equipmentoperationrate.startdate
            new TranslationSeedItem("entity.equipmentoperationrate.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.equipmentoperationrate.startdate
            new TranslationSeedItem("entity.equipmentoperationrate.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.equipmentoperationrate.startdate
            new TranslationSeedItem("entity.equipmentoperationrate.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.equipmentoperationrate.enddate
            new TranslationSeedItem("entity.equipmentoperationrate.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.equipmentoperationrate.enddate
            new TranslationSeedItem("entity.equipmentoperationrate.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.equipmentoperationrate.enddate
            new TranslationSeedItem("entity.equipmentoperationrate.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.equipmentoperationrate.enddate
            new TranslationSeedItem("entity.equipmentoperationrate.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.equipmentoperationrate.weeknumber
            new TranslationSeedItem("entity.equipmentoperationrate.weeknumber", "en-US", "周数_us", "周数（1-53）"),
            // entity.equipmentoperationrate.weeknumber
            new TranslationSeedItem("entity.equipmentoperationrate.weeknumber", "ja-JP", "周数_jp", "周数（1-53）"),
            // entity.equipmentoperationrate.weeknumber
            new TranslationSeedItem("entity.equipmentoperationrate.weeknumber", "zh-CN", "周数", "周数（1-53）"),
            // entity.equipmentoperationrate.weeknumber
            new TranslationSeedItem("entity.equipmentoperationrate.weeknumber", "zh-HK", "周数_hk", "周数（1-53）"),

            // entity.equipmentoperationrate.monthnumber
            new TranslationSeedItem("entity.equipmentoperationrate.monthnumber", "en-US", "月份_us", "月份（1-12）"),
            // entity.equipmentoperationrate.monthnumber
            new TranslationSeedItem("entity.equipmentoperationrate.monthnumber", "ja-JP", "月份_jp", "月份（1-12）"),
            // entity.equipmentoperationrate.monthnumber
            new TranslationSeedItem("entity.equipmentoperationrate.monthnumber", "zh-CN", "月份", "月份（1-12）"),
            // entity.equipmentoperationrate.monthnumber
            new TranslationSeedItem("entity.equipmentoperationrate.monthnumber", "zh-HK", "月份_hk", "月份（1-12）"),

            // entity.equipmentoperationrate.equipcode
            new TranslationSeedItem("entity.equipmentoperationrate.equipcode", "en-US", "设备编码_us", "设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipcode
            new TranslationSeedItem("entity.equipmentoperationrate.equipcode", "ja-JP", "设备编码_jp", "设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipcode
            new TranslationSeedItem("entity.equipmentoperationrate.equipcode", "zh-CN", "设备编码", "设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipcode
            new TranslationSeedItem("entity.equipmentoperationrate.equipcode", "zh-HK", "设备编码_hk", "设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),

            // entity.equipmentoperationrate.equipmentname
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentname", "en-US", "设备名称_us", "设备名称"),
            // entity.equipmentoperationrate.equipmentname
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentname", "ja-JP", "设备名称_jp", "设备名称"),
            // entity.equipmentoperationrate.equipmentname
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentname", "zh-CN", "设备名称", "设备名称"),
            // entity.equipmentoperationrate.equipmentname
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentname", "zh-HK", "设备名称_hk", "设备名称"),

            // entity.equipmentoperationrate.equipmenttype
            new TranslationSeedItem("entity.equipmentoperationrate.equipmenttype", "en-US", "登录设备_us", "登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）"),
            // entity.equipmentoperationrate.equipmenttype
            new TranslationSeedItem("entity.equipmentoperationrate.equipmenttype", "ja-JP", "登录设备_jp", "登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）"),
            // entity.equipmentoperationrate.equipmenttype
            new TranslationSeedItem("entity.equipmentoperationrate.equipmenttype", "zh-CN", "登录设备", "登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）"),
            // entity.equipmentoperationrate.equipmenttype
            new TranslationSeedItem("entity.equipmentoperationrate.equipmenttype", "zh-HK", "登录设备_hk", "登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）"),

            // entity.equipmentoperationrate.teamcode
            new TranslationSeedItem("entity.equipmentoperationrate.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.equipmentoperationrate.teamcode
            new TranslationSeedItem("entity.equipmentoperationrate.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.equipmentoperationrate.teamcode
            new TranslationSeedItem("entity.equipmentoperationrate.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.equipmentoperationrate.teamcode
            new TranslationSeedItem("entity.equipmentoperationrate.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.equipmentoperationrate.shiftno
            new TranslationSeedItem("entity.equipmentoperationrate.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.equipmentoperationrate.shiftno
            new TranslationSeedItem("entity.equipmentoperationrate.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.equipmentoperationrate.shiftno
            new TranslationSeedItem("entity.equipmentoperationrate.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.equipmentoperationrate.shiftno
            new TranslationSeedItem("entity.equipmentoperationrate.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.equipmentoperationrate.plannedruntime
            new TranslationSeedItem("entity.equipmentoperationrate.plannedruntime", "en-US", "负荷时间(分钟)_us", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentoperationrate.plannedruntime
            new TranslationSeedItem("entity.equipmentoperationrate.plannedruntime", "ja-JP", "负荷时间(分钟)_jp", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentoperationrate.plannedruntime
            new TranslationSeedItem("entity.equipmentoperationrate.plannedruntime", "zh-CN", "负荷时间(分钟)", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentoperationrate.plannedruntime
            new TranslationSeedItem("entity.equipmentoperationrate.plannedruntime", "zh-HK", "负荷时间(分钟)_hk", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),

            // entity.equipmentoperationrate.actualruntime
            new TranslationSeedItem("entity.equipmentoperationrate.actualruntime", "en-US", "稼动时间(分钟)_us", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentoperationrate.actualruntime
            new TranslationSeedItem("entity.equipmentoperationrate.actualruntime", "ja-JP", "稼动时间(分钟)_jp", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentoperationrate.actualruntime
            new TranslationSeedItem("entity.equipmentoperationrate.actualruntime", "zh-CN", "稼动时间(分钟)", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentoperationrate.actualruntime
            new TranslationSeedItem("entity.equipmentoperationrate.actualruntime", "zh-HK", "稼动时间(分钟)_hk", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),

            // entity.equipmentoperationrate.downtime
            new TranslationSeedItem("entity.equipmentoperationrate.downtime", "en-US", "停线损失时间(分钟)_us", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentoperationrate.downtime
            new TranslationSeedItem("entity.equipmentoperationrate.downtime", "ja-JP", "停线损失时间(分钟)_jp", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentoperationrate.downtime
            new TranslationSeedItem("entity.equipmentoperationrate.downtime", "zh-CN", "停线损失时间(分钟)", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentoperationrate.downtime
            new TranslationSeedItem("entity.equipmentoperationrate.downtime", "zh-HK", "停线损失时间(分钟)_hk", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),

            // entity.equipmentoperationrate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperationrate", "en-US", "时间稼动率(%)_us", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentoperationrate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperationrate", "ja-JP", "时间稼动率(%)_jp", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentoperationrate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperationrate", "zh-CN", "时间稼动率(%)", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentoperationrate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperationrate", "zh-HK", "时间稼动率(%)_hk", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),

            // entity.equipmentoperationrate.plannedoutput
            new TranslationSeedItem("entity.equipmentoperationrate.plannedoutput", "en-US", "计划产量_us", "计划产量"),
            // entity.equipmentoperationrate.plannedoutput
            new TranslationSeedItem("entity.equipmentoperationrate.plannedoutput", "ja-JP", "计划产量_jp", "计划产量"),
            // entity.equipmentoperationrate.plannedoutput
            new TranslationSeedItem("entity.equipmentoperationrate.plannedoutput", "zh-CN", "计划产量", "计划产量"),
            // entity.equipmentoperationrate.plannedoutput
            new TranslationSeedItem("entity.equipmentoperationrate.plannedoutput", "zh-HK", "计划产量_hk", "计划产量"),

            // entity.equipmentoperationrate.actualoutput
            new TranslationSeedItem("entity.equipmentoperationrate.actualoutput", "en-US", "实际产量_us", "实际产量"),
            // entity.equipmentoperationrate.actualoutput
            new TranslationSeedItem("entity.equipmentoperationrate.actualoutput", "ja-JP", "实际产量_jp", "实际产量"),
            // entity.equipmentoperationrate.actualoutput
            new TranslationSeedItem("entity.equipmentoperationrate.actualoutput", "zh-CN", "实际产量", "实际产量"),
            // entity.equipmentoperationrate.actualoutput
            new TranslationSeedItem("entity.equipmentoperationrate.actualoutput", "zh-HK", "实际产量_hk", "实际产量"),

            // entity.equipmentoperationrate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentoperationrate.qualifiedquantity", "en-US", "合格品数量_us", "合格品数量"),
            // entity.equipmentoperationrate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentoperationrate.qualifiedquantity", "ja-JP", "合格品数量_jp", "合格品数量"),
            // entity.equipmentoperationrate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentoperationrate.qualifiedquantity", "zh-CN", "合格品数量", "合格品数量"),
            // entity.equipmentoperationrate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentoperationrate.qualifiedquantity", "zh-HK", "合格品数量_hk", "合格品数量"),

            // entity.equipmentoperationrate.defectivequantity
            new TranslationSeedItem("entity.equipmentoperationrate.defectivequantity", "en-US", "不良品数量_us", "不良品数量"),
            // entity.equipmentoperationrate.defectivequantity
            new TranslationSeedItem("entity.equipmentoperationrate.defectivequantity", "ja-JP", "不良品数量_jp", "不良品数量"),
            // entity.equipmentoperationrate.defectivequantity
            new TranslationSeedItem("entity.equipmentoperationrate.defectivequantity", "zh-CN", "不良品数量", "不良品数量"),
            // entity.equipmentoperationrate.defectivequantity
            new TranslationSeedItem("entity.equipmentoperationrate.defectivequantity", "zh-HK", "不良品数量_hk", "不良品数量"),

            // entity.equipmentoperationrate.yieldrate
            new TranslationSeedItem("entity.equipmentoperationrate.yieldrate", "en-US", "良品率(%)_us", "良品率（%）"),
            // entity.equipmentoperationrate.yieldrate
            new TranslationSeedItem("entity.equipmentoperationrate.yieldrate", "ja-JP", "良品率(%)_jp", "良品率（%）"),
            // entity.equipmentoperationrate.yieldrate
            new TranslationSeedItem("entity.equipmentoperationrate.yieldrate", "zh-CN", "良品率(%)", "良品率（%）"),
            // entity.equipmentoperationrate.yieldrate
            new TranslationSeedItem("entity.equipmentoperationrate.yieldrate", "zh-HK", "良品率(%)_hk", "良品率（%）"),

            // entity.equipmentoperationrate.downtimereasontype
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereasontype", "en-US", "停机原因类型_us", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentoperationrate.downtimereasontype
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereasontype", "ja-JP", "停机原因类型_jp", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentoperationrate.downtimereasontype
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereasontype", "zh-CN", "停机原因类型", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentoperationrate.downtimereasontype
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereasontype", "zh-HK", "停机原因类型_hk", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),

            // entity.equipmentoperationrate.downtimereason
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereason", "en-US", "停机原因描述_us", "停机原因描述（自由文本，与 DowntimeReasonType 配合）"),
            // entity.equipmentoperationrate.downtimereason
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereason", "ja-JP", "停机原因描述_jp", "停机原因描述（自由文本，与 DowntimeReasonType 配合）"),
            // entity.equipmentoperationrate.downtimereason
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereason", "zh-CN", "停机原因描述", "停机原因描述（自由文本，与 DowntimeReasonType 配合）"),
            // entity.equipmentoperationrate.downtimereason
            new TranslationSeedItem("entity.equipmentoperationrate.downtimereason", "zh-HK", "停机原因描述_hk", "停机原因描述（自由文本，与 DowntimeReasonType 配合）"),

            // entity.equipmentoperationrate.equipmentoperator
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperator", "en-US", "设备操作员_us", "设备操作员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentoperator
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperator", "ja-JP", "设备操作员_jp", "设备操作员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentoperator
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperator", "zh-CN", "设备操作员", "设备操作员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentoperator
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentoperator", "zh-HK", "设备操作员_hk", "设备操作员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.equipmentoperationrate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentmaintainer", "en-US", "设备维护员_us", "设备维护员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentmaintainer", "ja-JP", "设备维护员_jp", "设备维护员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentmaintainer", "zh-CN", "设备维护员", "设备维护员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentoperationrate.equipmentmaintainer", "zh-HK", "设备维护员_hk", "设备维护员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.equipmentoperationrate.teamleader
            new TranslationSeedItem("entity.equipmentoperationrate.teamleader", "en-US", "班组长_us", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.teamleader
            new TranslationSeedItem("entity.equipmentoperationrate.teamleader", "ja-JP", "班组长_jp", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.teamleader
            new TranslationSeedItem("entity.equipmentoperationrate.teamleader", "zh-CN", "班组长", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.equipmentoperationrate.teamleader
            new TranslationSeedItem("entity.equipmentoperationrate.teamleader", "zh-HK", "班组长_hk", "班组长（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.equipmentoperationrate.ratestatus
            new TranslationSeedItem("entity.equipmentoperationrate.ratestatus", "en-US", "状态_us", "状态（0=正常，1=停用）"),
            // entity.equipmentoperationrate.ratestatus
            new TranslationSeedItem("entity.equipmentoperationrate.ratestatus", "ja-JP", "状态_jp", "状态（0=正常，1=停用）"),
            // entity.equipmentoperationrate.ratestatus
            new TranslationSeedItem("entity.equipmentoperationrate.ratestatus", "zh-CN", "状态", "状态（0=正常，1=停用）"),
            // entity.equipmentoperationrate.ratestatus
            new TranslationSeedItem("entity.equipmentoperationrate.ratestatus", "zh-HK", "状态_hk", "状态（0=正常，1=停用）"),
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
        translation.ResourceGroup = "Mps";
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
