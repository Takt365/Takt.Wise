// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktPersonnelOperationRateI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPersonnelOperationRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPersonnelOperationRate 实体国际化翻译种子（键前缀 entity.personneloperationrate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPersonnelOperationRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPersonnelOperationRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 personneloperationrate 实体翻译...", tenantCode);

        foreach (var item in GetPersonnelOperationRateTranslations())
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

        TaktLogger.Information("TaktPersonnelOperationRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPersonnelOperationRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.personneloperationrate._self / entity.personneloperationrate.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPersonnelOperationRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.personneloperationrate._self
            new TranslationSeedItem("entity.personneloperationrate._self", "en-US", "Personnel Operation Rate Information_us", "实体名称"),
            // entity.personneloperationrate._self
            new TranslationSeedItem("entity.personneloperationrate._self", "ja-JP", "人员稼动率信息_jp", "实体名称"),
            // entity.personneloperationrate._self
            new TranslationSeedItem("entity.personneloperationrate._self", "zh-CN", "人员稼动率信息", "实体名称"),
            // entity.personneloperationrate._self
            new TranslationSeedItem("entity.personneloperationrate._self", "zh-HK", "人员稼动率信息_hk", "实体名称"),

            // entity.personneloperationrate.timecategory
            new TranslationSeedItem("entity.personneloperationrate.timecategory", "en-US", "时间类别_us", "时间类别（1=天，2=周，3=月）"),
            // entity.personneloperationrate.timecategory
            new TranslationSeedItem("entity.personneloperationrate.timecategory", "ja-JP", "时间类别_jp", "时间类别（1=天，2=周，3=月）"),
            // entity.personneloperationrate.timecategory
            new TranslationSeedItem("entity.personneloperationrate.timecategory", "zh-CN", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.personneloperationrate.timecategory
            new TranslationSeedItem("entity.personneloperationrate.timecategory", "zh-HK", "时间类别_hk", "时间类别（1=天，2=周，3=月）"),

            // entity.personneloperationrate.startdate
            new TranslationSeedItem("entity.personneloperationrate.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.personneloperationrate.startdate
            new TranslationSeedItem("entity.personneloperationrate.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.personneloperationrate.startdate
            new TranslationSeedItem("entity.personneloperationrate.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.personneloperationrate.startdate
            new TranslationSeedItem("entity.personneloperationrate.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.personneloperationrate.enddate
            new TranslationSeedItem("entity.personneloperationrate.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.personneloperationrate.enddate
            new TranslationSeedItem("entity.personneloperationrate.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.personneloperationrate.enddate
            new TranslationSeedItem("entity.personneloperationrate.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.personneloperationrate.enddate
            new TranslationSeedItem("entity.personneloperationrate.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.personneloperationrate.weeknumber
            new TranslationSeedItem("entity.personneloperationrate.weeknumber", "en-US", "周数_us", "周数（1-53）"),
            // entity.personneloperationrate.weeknumber
            new TranslationSeedItem("entity.personneloperationrate.weeknumber", "ja-JP", "周数_jp", "周数（1-53）"),
            // entity.personneloperationrate.weeknumber
            new TranslationSeedItem("entity.personneloperationrate.weeknumber", "zh-CN", "周数", "周数（1-53）"),
            // entity.personneloperationrate.weeknumber
            new TranslationSeedItem("entity.personneloperationrate.weeknumber", "zh-HK", "周数_hk", "周数（1-53）"),

            // entity.personneloperationrate.monthnumber
            new TranslationSeedItem("entity.personneloperationrate.monthnumber", "en-US", "月份_us", "月份（1-12）"),
            // entity.personneloperationrate.monthnumber
            new TranslationSeedItem("entity.personneloperationrate.monthnumber", "ja-JP", "月份_jp", "月份（1-12）"),
            // entity.personneloperationrate.monthnumber
            new TranslationSeedItem("entity.personneloperationrate.monthnumber", "zh-CN", "月份", "月份（1-12）"),
            // entity.personneloperationrate.monthnumber
            new TranslationSeedItem("entity.personneloperationrate.monthnumber", "zh-HK", "月份_hk", "月份（1-12）"),

            // entity.personneloperationrate.teamcode
            new TranslationSeedItem("entity.personneloperationrate.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.personneloperationrate.teamcode
            new TranslationSeedItem("entity.personneloperationrate.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.personneloperationrate.teamcode
            new TranslationSeedItem("entity.personneloperationrate.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.personneloperationrate.teamcode
            new TranslationSeedItem("entity.personneloperationrate.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.personneloperationrate.teamcodename
            new TranslationSeedItem("entity.personneloperationrate.teamcodename", "en-US", "生产班组名称_us", "生产班组名称"),
            // entity.personneloperationrate.teamcodename
            new TranslationSeedItem("entity.personneloperationrate.teamcodename", "ja-JP", "生产班组名称_jp", "生产班组名称"),
            // entity.personneloperationrate.teamcodename
            new TranslationSeedItem("entity.personneloperationrate.teamcodename", "zh-CN", "生产班组名称", "生产班组名称"),
            // entity.personneloperationrate.teamcodename
            new TranslationSeedItem("entity.personneloperationrate.teamcodename", "zh-HK", "生产班组名称_hk", "生产班组名称"),

            // entity.personneloperationrate.shiftno
            new TranslationSeedItem("entity.personneloperationrate.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.personneloperationrate.shiftno
            new TranslationSeedItem("entity.personneloperationrate.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.personneloperationrate.shiftno
            new TranslationSeedItem("entity.personneloperationrate.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.personneloperationrate.shiftno
            new TranslationSeedItem("entity.personneloperationrate.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.personneloperationrate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.planneddirectpersonnelcount", "en-US", "计划直接人员数量_us", "计划直接人员数量"),
            // entity.personneloperationrate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.planneddirectpersonnelcount", "ja-JP", "计划直接人员数量_jp", "计划直接人员数量"),
            // entity.personneloperationrate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.planneddirectpersonnelcount", "zh-CN", "计划直接人员数量", "计划直接人员数量"),
            // entity.personneloperationrate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.planneddirectpersonnelcount", "zh-HK", "计划直接人员数量_hk", "计划直接人员数量"),

            // entity.personneloperationrate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualdirectpersonnelcount", "en-US", "实际直接人员数量_us", "实际直接人员数量"),
            // entity.personneloperationrate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualdirectpersonnelcount", "ja-JP", "实际直接人员数量_jp", "实际直接人员数量"),
            // entity.personneloperationrate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualdirectpersonnelcount", "zh-CN", "实际直接人员数量", "实际直接人员数量"),
            // entity.personneloperationrate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualdirectpersonnelcount", "zh-HK", "实际直接人员数量_hk", "实际直接人员数量"),

            // entity.personneloperationrate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.plannedindirectpersonnelcount", "en-US", "计划间接人员数量_us", "计划间接人员数量"),
            // entity.personneloperationrate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.plannedindirectpersonnelcount", "ja-JP", "计划间接人员数量_jp", "计划间接人员数量"),
            // entity.personneloperationrate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.plannedindirectpersonnelcount", "zh-CN", "计划间接人员数量", "计划间接人员数量"),
            // entity.personneloperationrate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.plannedindirectpersonnelcount", "zh-HK", "计划间接人员数量_hk", "计划间接人员数量"),

            // entity.personneloperationrate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualindirectpersonnelcount", "en-US", "实际间接人员数量_us", "实际间接人员数量"),
            // entity.personneloperationrate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualindirectpersonnelcount", "ja-JP", "实际间接人员数量_jp", "实际间接人员数量"),
            // entity.personneloperationrate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualindirectpersonnelcount", "zh-CN", "实际间接人员数量", "实际间接人员数量"),
            // entity.personneloperationrate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personneloperationrate.actualindirectpersonnelcount", "zh-HK", "实际间接人员数量_hk", "实际间接人员数量"),

            // entity.personneloperationrate.plannedworktime
            new TranslationSeedItem("entity.personneloperationrate.plannedworktime", "en-US", "出勤时间(分钟)_us", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personneloperationrate.plannedworktime
            new TranslationSeedItem("entity.personneloperationrate.plannedworktime", "ja-JP", "出勤时间(分钟)_jp", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personneloperationrate.plannedworktime
            new TranslationSeedItem("entity.personneloperationrate.plannedworktime", "zh-CN", "出勤时间(分钟)", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personneloperationrate.plannedworktime
            new TranslationSeedItem("entity.personneloperationrate.plannedworktime", "zh-HK", "出勤时间(分钟)_hk", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),

            // entity.personneloperationrate.actualworktime
            new TranslationSeedItem("entity.personneloperationrate.actualworktime", "en-US", "在岗作业时间(分钟)_us", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personneloperationrate.actualworktime
            new TranslationSeedItem("entity.personneloperationrate.actualworktime", "ja-JP", "在岗作业时间(分钟)_jp", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personneloperationrate.actualworktime
            new TranslationSeedItem("entity.personneloperationrate.actualworktime", "zh-CN", "在岗作业时间(分钟)", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personneloperationrate.actualworktime
            new TranslationSeedItem("entity.personneloperationrate.actualworktime", "zh-HK", "在岗作业时间(分钟)_hk", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),

            // entity.personneloperationrate.breaktime
            new TranslationSeedItem("entity.personneloperationrate.breaktime", "en-US", "休息时间(分钟)_us", "休息时间（分钟）"),
            // entity.personneloperationrate.breaktime
            new TranslationSeedItem("entity.personneloperationrate.breaktime", "ja-JP", "休息时间(分钟)_jp", "休息时间（分钟）"),
            // entity.personneloperationrate.breaktime
            new TranslationSeedItem("entity.personneloperationrate.breaktime", "zh-CN", "休息时间(分钟)", "休息时间（分钟）"),
            // entity.personneloperationrate.breaktime
            new TranslationSeedItem("entity.personneloperationrate.breaktime", "zh-HK", "休息时间(分钟)_hk", "休息时间（分钟）"),

            // entity.personneloperationrate.idletime
            new TranslationSeedItem("entity.personneloperationrate.idletime", "en-US", "空闲时间(分钟)_us", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personneloperationrate.idletime
            new TranslationSeedItem("entity.personneloperationrate.idletime", "ja-JP", "空闲时间(分钟)_jp", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personneloperationrate.idletime
            new TranslationSeedItem("entity.personneloperationrate.idletime", "zh-CN", "空闲时间(分钟)", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personneloperationrate.idletime
            new TranslationSeedItem("entity.personneloperationrate.idletime", "zh-HK", "空闲时间(分钟)_hk", "空闲时间（分钟）。等料、设备调试等非作业时间。"),

            // entity.personneloperationrate.personneloperationrate
            new TranslationSeedItem("entity.personneloperationrate.personneloperationrate", "en-US", "人员稼动率(%)_us", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personneloperationrate.personneloperationrate
            new TranslationSeedItem("entity.personneloperationrate.personneloperationrate", "ja-JP", "人员稼动率(%)_jp", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personneloperationrate.personneloperationrate
            new TranslationSeedItem("entity.personneloperationrate.personneloperationrate", "zh-CN", "人员稼动率(%)", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personneloperationrate.personneloperationrate
            new TranslationSeedItem("entity.personneloperationrate.personneloperationrate", "zh-HK", "人员稼动率(%)_hk", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),

            // entity.personneloperationrate.plannedoutput
            new TranslationSeedItem("entity.personneloperationrate.plannedoutput", "en-US", "计划产量_us", "计划产量"),
            // entity.personneloperationrate.plannedoutput
            new TranslationSeedItem("entity.personneloperationrate.plannedoutput", "ja-JP", "计划产量_jp", "计划产量"),
            // entity.personneloperationrate.plannedoutput
            new TranslationSeedItem("entity.personneloperationrate.plannedoutput", "zh-CN", "计划产量", "计划产量"),
            // entity.personneloperationrate.plannedoutput
            new TranslationSeedItem("entity.personneloperationrate.plannedoutput", "zh-HK", "计划产量_hk", "计划产量"),

            // entity.personneloperationrate.actualoutput
            new TranslationSeedItem("entity.personneloperationrate.actualoutput", "en-US", "实际产量_us", "实际产量"),
            // entity.personneloperationrate.actualoutput
            new TranslationSeedItem("entity.personneloperationrate.actualoutput", "ja-JP", "实际产量_jp", "实际产量"),
            // entity.personneloperationrate.actualoutput
            new TranslationSeedItem("entity.personneloperationrate.actualoutput", "zh-CN", "实际产量", "实际产量"),
            // entity.personneloperationrate.actualoutput
            new TranslationSeedItem("entity.personneloperationrate.actualoutput", "zh-HK", "实际产量_hk", "实际产量"),

            // entity.personneloperationrate.qualifiedquantity
            new TranslationSeedItem("entity.personneloperationrate.qualifiedquantity", "en-US", "合格品数量_us", "合格品数量"),
            // entity.personneloperationrate.qualifiedquantity
            new TranslationSeedItem("entity.personneloperationrate.qualifiedquantity", "ja-JP", "合格品数量_jp", "合格品数量"),
            // entity.personneloperationrate.qualifiedquantity
            new TranslationSeedItem("entity.personneloperationrate.qualifiedquantity", "zh-CN", "合格品数量", "合格品数量"),
            // entity.personneloperationrate.qualifiedquantity
            new TranslationSeedItem("entity.personneloperationrate.qualifiedquantity", "zh-HK", "合格品数量_hk", "合格品数量"),

            // entity.personneloperationrate.defectivequantity
            new TranslationSeedItem("entity.personneloperationrate.defectivequantity", "en-US", "不良品数量_us", "不良品数量"),
            // entity.personneloperationrate.defectivequantity
            new TranslationSeedItem("entity.personneloperationrate.defectivequantity", "ja-JP", "不良品数量_jp", "不良品数量"),
            // entity.personneloperationrate.defectivequantity
            new TranslationSeedItem("entity.personneloperationrate.defectivequantity", "zh-CN", "不良品数量", "不良品数量"),
            // entity.personneloperationrate.defectivequantity
            new TranslationSeedItem("entity.personneloperationrate.defectivequantity", "zh-HK", "不良品数量_hk", "不良品数量"),

            // entity.personneloperationrate.yieldrate
            new TranslationSeedItem("entity.personneloperationrate.yieldrate", "en-US", "良品率(%)_us", "良品率（%）"),
            // entity.personneloperationrate.yieldrate
            new TranslationSeedItem("entity.personneloperationrate.yieldrate", "ja-JP", "良品率(%)_jp", "良品率（%）"),
            // entity.personneloperationrate.yieldrate
            new TranslationSeedItem("entity.personneloperationrate.yieldrate", "zh-CN", "良品率(%)", "良品率（%）"),
            // entity.personneloperationrate.yieldrate
            new TranslationSeedItem("entity.personneloperationrate.yieldrate", "zh-HK", "良品率(%)_hk", "良品率（%）"),

            // entity.personneloperationrate.workefficiency
            new TranslationSeedItem("entity.personneloperationrate.workefficiency", "en-US", "工作效率(%)_us", "工作效率（%）"),
            // entity.personneloperationrate.workefficiency
            new TranslationSeedItem("entity.personneloperationrate.workefficiency", "ja-JP", "工作效率(%)_jp", "工作效率（%）"),
            // entity.personneloperationrate.workefficiency
            new TranslationSeedItem("entity.personneloperationrate.workefficiency", "zh-CN", "工作效率(%)", "工作效率（%）"),
            // entity.personneloperationrate.workefficiency
            new TranslationSeedItem("entity.personneloperationrate.workefficiency", "zh-HK", "工作效率(%)_hk", "工作效率（%）"),

            // entity.personneloperationrate.idlereasontype
            new TranslationSeedItem("entity.personneloperationrate.idlereasontype", "en-US", "空闲原因类型_us", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personneloperationrate.idlereasontype
            new TranslationSeedItem("entity.personneloperationrate.idlereasontype", "ja-JP", "空闲原因类型_jp", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personneloperationrate.idlereasontype
            new TranslationSeedItem("entity.personneloperationrate.idlereasontype", "zh-CN", "空闲原因类型", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personneloperationrate.idlereasontype
            new TranslationSeedItem("entity.personneloperationrate.idlereasontype", "zh-HK", "空闲原因类型_hk", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),

            // entity.personneloperationrate.idlereason
            new TranslationSeedItem("entity.personneloperationrate.idlereason", "en-US", "空闲原因描述_us", "空闲原因描述"),
            // entity.personneloperationrate.idlereason
            new TranslationSeedItem("entity.personneloperationrate.idlereason", "ja-JP", "空闲原因描述_jp", "空闲原因描述"),
            // entity.personneloperationrate.idlereason
            new TranslationSeedItem("entity.personneloperationrate.idlereason", "zh-CN", "空闲原因描述", "空闲原因描述"),
            // entity.personneloperationrate.idlereason
            new TranslationSeedItem("entity.personneloperationrate.idlereason", "zh-HK", "空闲原因描述_hk", "空闲原因描述"),

            // entity.personneloperationrate.overtimehours
            new TranslationSeedItem("entity.personneloperationrate.overtimehours", "en-US", "加班时间(分钟)_us", "加班时间（分钟）"),
            // entity.personneloperationrate.overtimehours
            new TranslationSeedItem("entity.personneloperationrate.overtimehours", "ja-JP", "加班时间(分钟)_jp", "加班时间（分钟）"),
            // entity.personneloperationrate.overtimehours
            new TranslationSeedItem("entity.personneloperationrate.overtimehours", "zh-CN", "加班时间(分钟)", "加班时间（分钟）"),
            // entity.personneloperationrate.overtimehours
            new TranslationSeedItem("entity.personneloperationrate.overtimehours", "zh-HK", "加班时间(分钟)_hk", "加班时间（分钟）"),

            // entity.personneloperationrate.teamleader
            new TranslationSeedItem("entity.personneloperationrate.teamleader", "en-US", "班组长_us", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.teamleader
            new TranslationSeedItem("entity.personneloperationrate.teamleader", "ja-JP", "班组长_jp", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.teamleader
            new TranslationSeedItem("entity.personneloperationrate.teamleader", "zh-CN", "班组长", "班组长（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.teamleader
            new TranslationSeedItem("entity.personneloperationrate.teamleader", "zh-HK", "班组长_hk", "班组长（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.personneloperationrate.supervisor
            new TranslationSeedItem("entity.personneloperationrate.supervisor", "en-US", "主管_us", "主管（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.supervisor
            new TranslationSeedItem("entity.personneloperationrate.supervisor", "ja-JP", "主管_jp", "主管（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.supervisor
            new TranslationSeedItem("entity.personneloperationrate.supervisor", "zh-CN", "主管", "主管（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.personneloperationrate.supervisor
            new TranslationSeedItem("entity.personneloperationrate.supervisor", "zh-HK", "主管_hk", "主管（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.personneloperationrate.ratestatus
            new TranslationSeedItem("entity.personneloperationrate.ratestatus", "en-US", "状态_us", "状态（0=正常，1=停用）"),
            // entity.personneloperationrate.ratestatus
            new TranslationSeedItem("entity.personneloperationrate.ratestatus", "ja-JP", "状态_jp", "状态（0=正常，1=停用）"),
            // entity.personneloperationrate.ratestatus
            new TranslationSeedItem("entity.personneloperationrate.ratestatus", "zh-CN", "状态", "状态（0=正常，1=停用）"),
            // entity.personneloperationrate.ratestatus
            new TranslationSeedItem("entity.personneloperationrate.ratestatus", "zh-HK", "状态_hk", "状态（0=正常，1=停用）"),
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
