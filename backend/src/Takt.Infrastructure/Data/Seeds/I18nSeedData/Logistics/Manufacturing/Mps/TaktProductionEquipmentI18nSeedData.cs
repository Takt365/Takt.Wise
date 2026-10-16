// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipmentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionEquipment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktProductionEquipment 实体国际化翻译种子（键前缀 entity.productionequipment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionEquipmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionEquipment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionequipment 实体翻译...", tenantCode);

        foreach (var item in GetProductionEquipmentTranslations())
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

        TaktLogger.Information("TaktProductionEquipment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionEquipment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionequipment._self / entity.productionequipment.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionEquipmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionequipment._self
            new TranslationSeedItem("entity.productionequipment._self", "en-US", "Production Equipment Information_us", "实体名称"),
            // entity.productionequipment._self
            new TranslationSeedItem("entity.productionequipment._self", "ja-JP", "生产设备主数据信息_jp", "实体名称"),
            // entity.productionequipment._self
            new TranslationSeedItem("entity.productionequipment._self", "zh-CN", "生产设备主数据信息", "实体名称"),
            // entity.productionequipment._self
            new TranslationSeedItem("entity.productionequipment._self", "zh-HK", "生产设备主数据信息_hk", "实体名称"),

            // entity.productionequipment.equipcategory
            new TranslationSeedItem("entity.productionequipment.equipcategory", "en-US", "设备类别_us", "设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）"),
            // entity.productionequipment.equipcategory
            new TranslationSeedItem("entity.productionequipment.equipcategory", "ja-JP", "设备类别_jp", "设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）"),
            // entity.productionequipment.equipcategory
            new TranslationSeedItem("entity.productionequipment.equipcategory", "zh-CN", "设备类别", "设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）"),
            // entity.productionequipment.equipcategory
            new TranslationSeedItem("entity.productionequipment.equipcategory", "zh-HK", "设备类别_hk", "设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）"),

            // entity.productionequipment.prodequipcode
            new TranslationSeedItem("entity.productionequipment.prodequipcode", "en-US", "生产设备编码_us", "生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码；前端表单选择编码规则后自动通过 TaktNumbering 生产设备编码规则生成并展示，非手输；单据类型菜单：生产设备）"),
            // entity.productionequipment.prodequipcode
            new TranslationSeedItem("entity.productionequipment.prodequipcode", "ja-JP", "生产设备编码_jp", "生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码；前端表单选择编码规则后自动通过 TaktNumbering 生产设备编码规则生成并展示，非手输；单据类型菜单：生产设备）"),
            // entity.productionequipment.prodequipcode
            new TranslationSeedItem("entity.productionequipment.prodequipcode", "zh-CN", "生产设备编码", "生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码；前端表单选择编码规则后自动通过 TaktNumbering 生产设备编码规则生成并展示，非手输；单据类型菜单：生产设备）"),
            // entity.productionequipment.prodequipcode
            new TranslationSeedItem("entity.productionequipment.prodequipcode", "zh-HK", "生产设备编码_hk", "生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码；前端表单选择编码规则后自动通过 TaktNumbering 生产设备编码规则生成并展示，非手输；单据类型菜单：生产设备）"),

            // entity.productionequipment.prodequipname
            new TranslationSeedItem("entity.productionequipment.prodequipname", "en-US", "生产设备名称_us", "生产设备名称（列表展示名）"),
            // entity.productionequipment.prodequipname
            new TranslationSeedItem("entity.productionequipment.prodequipname", "ja-JP", "生产设备名称_jp", "生产设备名称（列表展示名）"),
            // entity.productionequipment.prodequipname
            new TranslationSeedItem("entity.productionequipment.prodequipname", "zh-CN", "生产设备名称", "生产设备名称（列表展示名）"),
            // entity.productionequipment.prodequipname
            new TranslationSeedItem("entity.productionequipment.prodequipname", "zh-HK", "生产设备名称_hk", "生产设备名称（列表展示名）"),

            // entity.productionequipment.manufacturer
            new TranslationSeedItem("entity.productionequipment.manufacturer", "en-US", "制造商_us", "制造商"),
            // entity.productionequipment.manufacturer
            new TranslationSeedItem("entity.productionequipment.manufacturer", "ja-JP", "制造商_jp", "制造商"),
            // entity.productionequipment.manufacturer
            new TranslationSeedItem("entity.productionequipment.manufacturer", "zh-CN", "制造商", "制造商"),
            // entity.productionequipment.manufacturer
            new TranslationSeedItem("entity.productionequipment.manufacturer", "zh-HK", "制造商_hk", "制造商"),

            // entity.productionequipment.equipbrand
            new TranslationSeedItem("entity.productionequipment.equipbrand", "en-US", "设备品牌_us", "设备品牌（铭牌 Brand）"),
            // entity.productionequipment.equipbrand
            new TranslationSeedItem("entity.productionequipment.equipbrand", "ja-JP", "设备品牌_jp", "设备品牌（铭牌 Brand）"),
            // entity.productionequipment.equipbrand
            new TranslationSeedItem("entity.productionequipment.equipbrand", "zh-CN", "设备品牌", "设备品牌（铭牌 Brand）"),
            // entity.productionequipment.equipbrand
            new TranslationSeedItem("entity.productionequipment.equipbrand", "zh-HK", "设备品牌_hk", "设备品牌（铭牌 Brand）"),

            // entity.productionequipment.machinetype
            new TranslationSeedItem("entity.productionequipment.machinetype", "en-US", "机型名称_us", "机型名称（铭牌 Machine Type，如 SP18P-L）"),
            // entity.productionequipment.machinetype
            new TranslationSeedItem("entity.productionequipment.machinetype", "ja-JP", "机型名称_jp", "机型名称（铭牌 Machine Type，如 SP18P-L）"),
            // entity.productionequipment.machinetype
            new TranslationSeedItem("entity.productionequipment.machinetype", "zh-CN", "机型名称", "机型名称（铭牌 Machine Type，如 SP18P-L）"),
            // entity.productionequipment.machinetype
            new TranslationSeedItem("entity.productionequipment.machinetype", "zh-HK", "机型名称_hk", "机型名称（铭牌 Machine Type，如 SP18P-L）"),

            // entity.productionequipment.modelcode
            new TranslationSeedItem("entity.productionequipment.modelcode", "en-US", "型号_us", "型号（铭牌 Model No，如 NM-EJP1A）"),
            // entity.productionequipment.modelcode
            new TranslationSeedItem("entity.productionequipment.modelcode", "ja-JP", "型号_jp", "型号（铭牌 Model No，如 NM-EJP1A）"),
            // entity.productionequipment.modelcode
            new TranslationSeedItem("entity.productionequipment.modelcode", "zh-CN", "型号", "型号（铭牌 Model No，如 NM-EJP1A）"),
            // entity.productionequipment.modelcode
            new TranslationSeedItem("entity.productionequipment.modelcode", "zh-HK", "型号_hk", "型号（铭牌 Model No，如 NM-EJP1A）"),

            // entity.productionequipment.serialcode
            new TranslationSeedItem("entity.productionequipment.serialcode", "en-US", "序列号_us", "序列号（铭牌 Serial No，如 1P8V0336）"),
            // entity.productionequipment.serialcode
            new TranslationSeedItem("entity.productionequipment.serialcode", "ja-JP", "序列号_jp", "序列号（铭牌 Serial No，如 1P8V0336）"),
            // entity.productionequipment.serialcode
            new TranslationSeedItem("entity.productionequipment.serialcode", "zh-CN", "序列号", "序列号（铭牌 Serial No，如 1P8V0336）"),
            // entity.productionequipment.serialcode
            new TranslationSeedItem("entity.productionequipment.serialcode", "zh-HK", "序列号_hk", "序列号（铭牌 Serial No，如 1P8V0336）"),

            // entity.productionequipment.manufacturingdate
            new TranslationSeedItem("entity.productionequipment.manufacturingdate", "en-US", "出厂日期_us", "出厂日期（Manufacturing Date）"),
            // entity.productionequipment.manufacturingdate
            new TranslationSeedItem("entity.productionequipment.manufacturingdate", "ja-JP", "出厂日期_jp", "出厂日期（Manufacturing Date）"),
            // entity.productionequipment.manufacturingdate
            new TranslationSeedItem("entity.productionequipment.manufacturingdate", "zh-CN", "出厂日期", "出厂日期（Manufacturing Date）"),
            // entity.productionequipment.manufacturingdate
            new TranslationSeedItem("entity.productionequipment.manufacturingdate", "zh-HK", "出厂日期_hk", "出厂日期（Manufacturing Date）"),

            // entity.productionequipment.equipspecification
            new TranslationSeedItem("entity.productionequipment.equipspecification", "en-US", "设备规格_us", "设备规格"),
            // entity.productionequipment.equipspecification
            new TranslationSeedItem("entity.productionequipment.equipspecification", "ja-JP", "设备规格_jp", "设备规格"),
            // entity.productionequipment.equipspecification
            new TranslationSeedItem("entity.productionequipment.equipspecification", "zh-CN", "设备规格", "设备规格"),
            // entity.productionequipment.equipspecification
            new TranslationSeedItem("entity.productionequipment.equipspecification", "zh-HK", "设备规格_hk", "设备规格"),

            // entity.productionequipment.stdcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.stdcycletimeseconds", "en-US", "理论周期时间秒_us", "理论周期时间（秒/模次；StdCycleTime，SPM 倒数）"),
            // entity.productionequipment.stdcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.stdcycletimeseconds", "ja-JP", "理论周期时间秒_jp", "理论周期时间（秒/模次；StdCycleTime，SPM 倒数）"),
            // entity.productionequipment.stdcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.stdcycletimeseconds", "zh-CN", "理论周期时间秒", "理论周期时间（秒/模次；StdCycleTime，SPM 倒数）"),
            // entity.productionequipment.stdcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.stdcycletimeseconds", "zh-HK", "理论周期时间秒_hk", "理论周期时间（秒/模次；StdCycleTime，SPM 倒数）"),

            // entity.productionequipment.stdminutesperunit
            new TranslationSeedItem("entity.productionequipment.stdminutesperunit", "en-US", "标准分钟每件_us", "标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）"),
            // entity.productionequipment.stdminutesperunit
            new TranslationSeedItem("entity.productionequipment.stdminutesperunit", "ja-JP", "标准分钟每件_jp", "标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）"),
            // entity.productionequipment.stdminutesperunit
            new TranslationSeedItem("entity.productionequipment.stdminutesperunit", "zh-CN", "标准分钟每件", "标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）"),
            // entity.productionequipment.stdminutesperunit
            new TranslationSeedItem("entity.productionequipment.stdminutesperunit", "zh-HK", "标准分钟每件_hk", "标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）"),

            // entity.productionequipment.stdminutespercycle
            new TranslationSeedItem("entity.productionequipment.stdminutespercycle", "en-US", "标准分钟每周期_us", "标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）"),
            // entity.productionequipment.stdminutespercycle
            new TranslationSeedItem("entity.productionequipment.stdminutespercycle", "ja-JP", "标准分钟每周期_jp", "标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）"),
            // entity.productionequipment.stdminutespercycle
            new TranslationSeedItem("entity.productionequipment.stdminutespercycle", "zh-CN", "标准分钟每周期", "标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）"),
            // entity.productionequipment.stdminutespercycle
            new TranslationSeedItem("entity.productionequipment.stdminutespercycle", "zh-HK", "标准分钟每周期_hk", "标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）"),

            // entity.productionequipment.theoreticalspm
            new TranslationSeedItem("entity.productionequipment.theoreticalspm", "en-US", "理论模次每小时_us", "理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）"),
            // entity.productionequipment.theoreticalspm
            new TranslationSeedItem("entity.productionequipment.theoreticalspm", "ja-JP", "理论模次每小时_jp", "理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）"),
            // entity.productionequipment.theoreticalspm
            new TranslationSeedItem("entity.productionequipment.theoreticalspm", "zh-CN", "理论模次每小时", "理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）"),
            // entity.productionequipment.theoreticalspm
            new TranslationSeedItem("entity.productionequipment.theoreticalspm", "zh-HK", "理论模次每小时_hk", "理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）"),

            // entity.productionequipment.theoreticalcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.theoreticalcycletimeseconds", "en-US", "理论成型周期秒_us", "理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）"),
            // entity.productionequipment.theoreticalcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.theoreticalcycletimeseconds", "ja-JP", "理论成型周期秒_jp", "理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）"),
            // entity.productionequipment.theoreticalcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.theoreticalcycletimeseconds", "zh-CN", "理论成型周期秒", "理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）"),
            // entity.productionequipment.theoreticalcycletimeseconds
            new TranslationSeedItem("entity.productionequipment.theoreticalcycletimeseconds", "zh-HK", "理论成型周期秒_hk", "理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）"),

            // entity.productionequipment.stdequiphourlycapacity
            new TranslationSeedItem("entity.productionequipment.stdequiphourlycapacity", "en-US", "设备标准小时产能_us", "设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）"),
            // entity.productionequipment.stdequiphourlycapacity
            new TranslationSeedItem("entity.productionequipment.stdequiphourlycapacity", "ja-JP", "设备标准小时产能_jp", "设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）"),
            // entity.productionequipment.stdequiphourlycapacity
            new TranslationSeedItem("entity.productionequipment.stdequiphourlycapacity", "zh-CN", "设备标准小时产能", "设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）"),
            // entity.productionequipment.stdequiphourlycapacity
            new TranslationSeedItem("entity.productionequipment.stdequiphourlycapacity", "zh-HK", "设备标准小时产能_hk", "设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）"),

            // entity.productionequipment.availabilityrate
            new TranslationSeedItem("entity.productionequipment.availabilityrate", "en-US", "设备时间稼动率_us", "设备时间稼动率（AvailabilityRate，0–1）"),
            // entity.productionequipment.availabilityrate
            new TranslationSeedItem("entity.productionequipment.availabilityrate", "ja-JP", "设备时间稼动率_jp", "设备时间稼动率（AvailabilityRate，0–1）"),
            // entity.productionequipment.availabilityrate
            new TranslationSeedItem("entity.productionequipment.availabilityrate", "zh-CN", "设备时间稼动率", "设备时间稼动率（AvailabilityRate，0–1）"),
            // entity.productionequipment.availabilityrate
            new TranslationSeedItem("entity.productionequipment.availabilityrate", "zh-HK", "设备时间稼动率_hk", "设备时间稼动率（AvailabilityRate，0–1）"),

            // entity.productionequipment.performancerate
            new TranslationSeedItem("entity.productionequipment.performancerate", "en-US", "性能稼动率_us", "性能稼动率（PerformanceRate，0–1；实际可达产能乘数）"),
            // entity.productionequipment.performancerate
            new TranslationSeedItem("entity.productionequipment.performancerate", "ja-JP", "性能稼动率_jp", "性能稼动率（PerformanceRate，0–1；实际可达产能乘数）"),
            // entity.productionequipment.performancerate
            new TranslationSeedItem("entity.productionequipment.performancerate", "zh-CN", "性能稼动率", "性能稼动率（PerformanceRate，0–1；实际可达产能乘数）"),
            // entity.productionequipment.performancerate
            new TranslationSeedItem("entity.productionequipment.performancerate", "zh-HK", "性能稼动率_hk", "性能稼动率（PerformanceRate，0–1；实际可达产能乘数）"),

            // entity.productionequipment.setupminutes
            new TranslationSeedItem("entity.productionequipment.setupminutes", "en-US", "准备时间分钟_us", "准备时间（分钟；通用调试）"),
            // entity.productionequipment.setupminutes
            new TranslationSeedItem("entity.productionequipment.setupminutes", "ja-JP", "准备时间分钟_jp", "准备时间（分钟；通用调试）"),
            // entity.productionequipment.setupminutes
            new TranslationSeedItem("entity.productionequipment.setupminutes", "zh-CN", "准备时间分钟", "准备时间（分钟；通用调试）"),
            // entity.productionequipment.setupminutes
            new TranslationSeedItem("entity.productionequipment.setupminutes", "zh-HK", "准备时间分钟_hk", "准备时间（分钟；通用调试）"),

            // entity.productionequipment.moldchangeminutes
            new TranslationSeedItem("entity.productionequipment.moldchangeminutes", "en-US", "换模时间分钟_us", "换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）"),
            // entity.productionequipment.moldchangeminutes
            new TranslationSeedItem("entity.productionequipment.moldchangeminutes", "ja-JP", "换模时间分钟_jp", "换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）"),
            // entity.productionequipment.moldchangeminutes
            new TranslationSeedItem("entity.productionequipment.moldchangeminutes", "zh-CN", "换模时间分钟", "换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）"),
            // entity.productionequipment.moldchangeminutes
            new TranslationSeedItem("entity.productionequipment.moldchangeminutes", "zh-HK", "换模时间分钟_hk", "换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）"),

            // entity.productionequipment.materialchangeminutes
            new TranslationSeedItem("entity.productionequipment.materialchangeminutes", "en-US", "换料时间分钟_us", "换料时间（分钟；注塑料筒清洗等）"),
            // entity.productionequipment.materialchangeminutes
            new TranslationSeedItem("entity.productionequipment.materialchangeminutes", "ja-JP", "换料时间分钟_jp", "换料时间（分钟；注塑料筒清洗等）"),
            // entity.productionequipment.materialchangeminutes
            new TranslationSeedItem("entity.productionequipment.materialchangeminutes", "zh-CN", "换料时间分钟", "换料时间（分钟；注塑料筒清洗等）"),
            // entity.productionequipment.materialchangeminutes
            new TranslationSeedItem("entity.productionequipment.materialchangeminutes", "zh-HK", "换料时间分钟_hk", "换料时间（分钟；注塑料筒清洗等）"),

            // entity.productionequipment.mtbfhours
            new TranslationSeedItem("entity.productionequipment.mtbfhours", "en-US", "平均无故障时间小时_us", "平均无故障时间 MTBF（小时）"),
            // entity.productionequipment.mtbfhours
            new TranslationSeedItem("entity.productionequipment.mtbfhours", "ja-JP", "平均无故障时间小时_jp", "平均无故障时间 MTBF（小时）"),
            // entity.productionequipment.mtbfhours
            new TranslationSeedItem("entity.productionequipment.mtbfhours", "zh-CN", "平均无故障时间小时", "平均无故障时间 MTBF（小时）"),
            // entity.productionequipment.mtbfhours
            new TranslationSeedItem("entity.productionequipment.mtbfhours", "zh-HK", "平均无故障时间小时_hk", "平均无故障时间 MTBF（小时）"),

            // entity.productionequipment.mttrhours
            new TranslationSeedItem("entity.productionequipment.mttrhours", "en-US", "平均修复时间小时_us", "平均修复时间 MTTR（小时）"),
            // entity.productionequipment.mttrhours
            new TranslationSeedItem("entity.productionequipment.mttrhours", "ja-JP", "平均修复时间小时_jp", "平均修复时间 MTTR（小时）"),
            // entity.productionequipment.mttrhours
            new TranslationSeedItem("entity.productionequipment.mttrhours", "zh-CN", "平均修复时间小时", "平均修复时间 MTTR（小时）"),
            // entity.productionequipment.mttrhours
            new TranslationSeedItem("entity.productionequipment.mttrhours", "zh-HK", "平均修复时间小时_hk", "平均修复时间 MTTR（小时）"),

            // entity.productionequipment.repeatabilityaccuracy
            new TranslationSeedItem("entity.productionequipment.repeatabilityaccuracy", "en-US", "重复定位精度_us", "重复定位精度（mm；滑块/模板 ±0.01~0.05）"),
            // entity.productionequipment.repeatabilityaccuracy
            new TranslationSeedItem("entity.productionequipment.repeatabilityaccuracy", "ja-JP", "重复定位精度_jp", "重复定位精度（mm；滑块/模板 ±0.01~0.05）"),
            // entity.productionequipment.repeatabilityaccuracy
            new TranslationSeedItem("entity.productionequipment.repeatabilityaccuracy", "zh-CN", "重复定位精度", "重复定位精度（mm；滑块/模板 ±0.01~0.05）"),
            // entity.productionequipment.repeatabilityaccuracy
            new TranslationSeedItem("entity.productionequipment.repeatabilityaccuracy", "zh-HK", "重复定位精度_hk", "重复定位精度（mm；滑块/模板 ±0.01~0.05）"),

            // entity.productionequipment.shutheightaccuracy
            new TranslationSeedItem("entity.productionequipment.shutheightaccuracy", "en-US", "闭合高度精度_us", "闭合高度精度（mm；冲压）"),
            // entity.productionequipment.shutheightaccuracy
            new TranslationSeedItem("entity.productionequipment.shutheightaccuracy", "ja-JP", "闭合高度精度_jp", "闭合高度精度（mm；冲压）"),
            // entity.productionequipment.shutheightaccuracy
            new TranslationSeedItem("entity.productionequipment.shutheightaccuracy", "zh-CN", "闭合高度精度", "闭合高度精度（mm；冲压）"),
            // entity.productionequipment.shutheightaccuracy
            new TranslationSeedItem("entity.productionequipment.shutheightaccuracy", "zh-HK", "闭合高度精度_hk", "闭合高度精度（mm；冲压）"),

            // entity.productionequipment.injectionaccuracy
            new TranslationSeedItem("entity.productionequipment.injectionaccuracy", "en-US", "注射精度_us", "注射精度（%；注塑计量 ±0.5%）"),
            // entity.productionequipment.injectionaccuracy
            new TranslationSeedItem("entity.productionequipment.injectionaccuracy", "ja-JP", "注射精度_jp", "注射精度（%；注塑计量 ±0.5%）"),
            // entity.productionequipment.injectionaccuracy
            new TranslationSeedItem("entity.productionequipment.injectionaccuracy", "zh-CN", "注射精度", "注射精度（%；注塑计量 ±0.5%）"),
            // entity.productionequipment.injectionaccuracy
            new TranslationSeedItem("entity.productionequipment.injectionaccuracy", "zh-HK", "注射精度_hk", "注射精度（%；注塑计量 ±0.5%）"),

            // entity.productionequipment.temperaturecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.temperaturecontrolaccuracy", "en-US", "温控精度_us", "温控精度（℃；注塑 ±1℃）"),
            // entity.productionequipment.temperaturecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.temperaturecontrolaccuracy", "ja-JP", "温控精度_jp", "温控精度（℃；注塑 ±1℃）"),
            // entity.productionequipment.temperaturecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.temperaturecontrolaccuracy", "zh-CN", "温控精度", "温控精度（℃；注塑 ±1℃）"),
            // entity.productionequipment.temperaturecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.temperaturecontrolaccuracy", "zh-HK", "温控精度_hk", "温控精度（℃；注塑 ±1℃）"),

            // entity.productionequipment.pressurecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.pressurecontrolaccuracy", "en-US", "压力控制精度_us", "压力控制精度（%；冲压/注塑 ±1–2%）"),
            // entity.productionequipment.pressurecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.pressurecontrolaccuracy", "ja-JP", "压力控制精度_jp", "压力控制精度（%；冲压/注塑 ±1–2%）"),
            // entity.productionequipment.pressurecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.pressurecontrolaccuracy", "zh-CN", "压力控制精度", "压力控制精度（%；冲压/注塑 ±1–2%）"),
            // entity.productionequipment.pressurecontrolaccuracy
            new TranslationSeedItem("entity.productionequipment.pressurecontrolaccuracy", "zh-HK", "压力控制精度_hk", "压力控制精度（%；冲压/注塑 ±1–2%）"),

            // entity.productionequipment.processcapabilitycpk
            new TranslationSeedItem("entity.productionequipment.processcapabilitycpk", "en-US", "工艺能力Cpk_us", "工艺能力 Cpk（关键尺寸）"),
            // entity.productionequipment.processcapabilitycpk
            new TranslationSeedItem("entity.productionequipment.processcapabilitycpk", "ja-JP", "工艺能力Cpk_jp", "工艺能力 Cpk（关键尺寸）"),
            // entity.productionequipment.processcapabilitycpk
            new TranslationSeedItem("entity.productionequipment.processcapabilitycpk", "zh-CN", "工艺能力Cpk", "工艺能力 Cpk（关键尺寸）"),
            // entity.productionequipment.processcapabilitycpk
            new TranslationSeedItem("entity.productionequipment.processcapabilitycpk", "zh-HK", "工艺能力Cpk_hk", "工艺能力 Cpk（关键尺寸）"),

            // entity.productionequipment.maxdimensionaltolerance
            new TranslationSeedItem("entity.productionequipment.maxdimensionaltolerance", "en-US", "最大成型公差_us", "最大成型公差（mm）"),
            // entity.productionequipment.maxdimensionaltolerance
            new TranslationSeedItem("entity.productionequipment.maxdimensionaltolerance", "ja-JP", "最大成型公差_jp", "最大成型公差（mm）"),
            // entity.productionequipment.maxdimensionaltolerance
            new TranslationSeedItem("entity.productionequipment.maxdimensionaltolerance", "zh-CN", "最大成型公差", "最大成型公差（mm）"),
            // entity.productionequipment.maxdimensionaltolerance
            new TranslationSeedItem("entity.productionequipment.maxdimensionaltolerance", "zh-HK", "最大成型公差_hk", "最大成型公差（mm）"),

            // entity.productionequipment.maxmolddimension
            new TranslationSeedItem("entity.productionequipment.maxmolddimension", "en-US", "最大模具尺寸_us", "最大模具尺寸（L×W×H）"),
            // entity.productionequipment.maxmolddimension
            new TranslationSeedItem("entity.productionequipment.maxmolddimension", "ja-JP", "最大模具尺寸_jp", "最大模具尺寸（L×W×H）"),
            // entity.productionequipment.maxmolddimension
            new TranslationSeedItem("entity.productionequipment.maxmolddimension", "zh-CN", "最大模具尺寸", "最大模具尺寸（L×W×H）"),
            // entity.productionequipment.maxmolddimension
            new TranslationSeedItem("entity.productionequipment.maxmolddimension", "zh-HK", "最大模具尺寸_hk", "最大模具尺寸（L×W×H）"),

            // entity.productionequipment.minmolddimension
            new TranslationSeedItem("entity.productionequipment.minmolddimension", "en-US", "最小模具尺寸_us", "最小模具尺寸（L×W×H）"),
            // entity.productionequipment.minmolddimension
            new TranslationSeedItem("entity.productionequipment.minmolddimension", "ja-JP", "最小模具尺寸_jp", "最小模具尺寸（L×W×H）"),
            // entity.productionequipment.minmolddimension
            new TranslationSeedItem("entity.productionequipment.minmolddimension", "zh-CN", "最小模具尺寸", "最小模具尺寸（L×W×H）"),
            // entity.productionequipment.minmolddimension
            new TranslationSeedItem("entity.productionequipment.minmolddimension", "zh-HK", "最小模具尺寸_hk", "最小模具尺寸（L×W×H）"),

            // entity.productionequipment.maxmoldweightton
            new TranslationSeedItem("entity.productionequipment.maxmoldweightton", "en-US", "模具重量上限吨_us", "模具重量上限（ton）"),
            // entity.productionequipment.maxmoldweightton
            new TranslationSeedItem("entity.productionequipment.maxmoldweightton", "ja-JP", "模具重量上限吨_jp", "模具重量上限（ton）"),
            // entity.productionequipment.maxmoldweightton
            new TranslationSeedItem("entity.productionequipment.maxmoldweightton", "zh-CN", "模具重量上限吨", "模具重量上限（ton）"),
            // entity.productionequipment.maxmoldweightton
            new TranslationSeedItem("entity.productionequipment.maxmoldweightton", "zh-HK", "模具重量上限吨_hk", "模具重量上限（ton）"),

            // entity.productionequipment.moldheightrange
            new TranslationSeedItem("entity.productionequipment.moldheightrange", "en-US", "模具厚度范围_us", "模具厚度范围（冲压闭合高度/注塑模板间距）"),
            // entity.productionequipment.moldheightrange
            new TranslationSeedItem("entity.productionequipment.moldheightrange", "ja-JP", "模具厚度范围_jp", "模具厚度范围（冲压闭合高度/注塑模板间距）"),
            // entity.productionequipment.moldheightrange
            new TranslationSeedItem("entity.productionequipment.moldheightrange", "zh-CN", "模具厚度范围", "模具厚度范围（冲压闭合高度/注塑模板间距）"),
            // entity.productionequipment.moldheightrange
            new TranslationSeedItem("entity.productionequipment.moldheightrange", "zh-HK", "模具厚度范围_hk", "模具厚度范围（冲压闭合高度/注塑模板间距）"),

            // entity.productionequipment.ejectiontype
            new TranslationSeedItem("entity.productionequipment.ejectiontype", "en-US", "顶出方式_us", "顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）"),
            // entity.productionequipment.ejectiontype
            new TranslationSeedItem("entity.productionequipment.ejectiontype", "ja-JP", "顶出方式_jp", "顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）"),
            // entity.productionequipment.ejectiontype
            new TranslationSeedItem("entity.productionequipment.ejectiontype", "zh-CN", "顶出方式", "顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）"),
            // entity.productionequipment.ejectiontype
            new TranslationSeedItem("entity.productionequipment.ejectiontype", "zh-HK", "顶出方式_hk", "顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）"),

            // entity.productionequipment.ejectionstrokemm
            new TranslationSeedItem("entity.productionequipment.ejectionstrokemm", "en-US", "顶出行程毫米_us", "顶出行程（mm）"),
            // entity.productionequipment.ejectionstrokemm
            new TranslationSeedItem("entity.productionequipment.ejectionstrokemm", "ja-JP", "顶出行程毫米_jp", "顶出行程（mm）"),
            // entity.productionequipment.ejectionstrokemm
            new TranslationSeedItem("entity.productionequipment.ejectionstrokemm", "zh-CN", "顶出行程毫米", "顶出行程（mm）"),
            // entity.productionequipment.ejectionstrokemm
            new TranslationSeedItem("entity.productionequipment.ejectionstrokemm", "zh-HK", "顶出行程毫米_hk", "顶出行程（mm）"),

            // entity.productionequipment.cavitycount
            new TranslationSeedItem("entity.productionequipment.cavitycount", "en-US", "工位穴数_us", "工位数/穴数（CavityCount；一出几，产能折算关键）"),
            // entity.productionequipment.cavitycount
            new TranslationSeedItem("entity.productionequipment.cavitycount", "ja-JP", "工位穴数_jp", "工位数/穴数（CavityCount；一出几，产能折算关键）"),
            // entity.productionequipment.cavitycount
            new TranslationSeedItem("entity.productionequipment.cavitycount", "zh-CN", "工位穴数", "工位数/穴数（CavityCount；一出几，产能折算关键）"),
            // entity.productionequipment.cavitycount
            new TranslationSeedItem("entity.productionequipment.cavitycount", "zh-HK", "工位穴数_hk", "工位数/穴数（CavityCount；一出几，产能折算关键）"),

            // entity.productionequipment.quickmoldchange
            new TranslationSeedItem("entity.productionequipment.quickmoldchange", "en-US", "快速换模_us", "快速换模（字典 sys_yes_no）"),
            // entity.productionequipment.quickmoldchange
            new TranslationSeedItem("entity.productionequipment.quickmoldchange", "ja-JP", "快速换模_jp", "快速换模（字典 sys_yes_no）"),
            // entity.productionequipment.quickmoldchange
            new TranslationSeedItem("entity.productionequipment.quickmoldchange", "zh-CN", "快速换模", "快速换模（字典 sys_yes_no）"),
            // entity.productionequipment.quickmoldchange
            new TranslationSeedItem("entity.productionequipment.quickmoldchange", "zh-HK", "快速换模_hk", "快速换模（字典 sys_yes_no）"),

            // entity.productionequipment.moldcode
            new TranslationSeedItem("entity.productionequipment.moldcode", "en-US", "模具编码_us", "模具编码（模具主数据关联）"),
            // entity.productionequipment.moldcode
            new TranslationSeedItem("entity.productionequipment.moldcode", "ja-JP", "模具编码_jp", "模具编码（模具主数据关联）"),
            // entity.productionequipment.moldcode
            new TranslationSeedItem("entity.productionequipment.moldcode", "zh-CN", "模具编码", "模具编码（模具主数据关联）"),
            // entity.productionequipment.moldcode
            new TranslationSeedItem("entity.productionequipment.moldcode", "zh-HK", "模具编码_hk", "模具编码（模具主数据关联）"),

            // entity.productionequipment.ratedtonnage
            new TranslationSeedItem("entity.productionequipment.ratedtonnage", "en-US", "额定吨位_us", "额定吨位（ton；冲压）"),
            // entity.productionequipment.ratedtonnage
            new TranslationSeedItem("entity.productionequipment.ratedtonnage", "ja-JP", "额定吨位_jp", "额定吨位（ton；冲压）"),
            // entity.productionequipment.ratedtonnage
            new TranslationSeedItem("entity.productionequipment.ratedtonnage", "zh-CN", "额定吨位", "额定吨位（ton；冲压）"),
            // entity.productionequipment.ratedtonnage
            new TranslationSeedItem("entity.productionequipment.ratedtonnage", "zh-HK", "额定吨位_hk", "额定吨位（ton；冲压）"),

            // entity.productionequipment.clampingforcekn
            new TranslationSeedItem("entity.productionequipment.clampingforcekn", "en-US", "锁模力千牛_us", "锁模力（kN；注塑）"),
            // entity.productionequipment.clampingforcekn
            new TranslationSeedItem("entity.productionequipment.clampingforcekn", "ja-JP", "锁模力千牛_jp", "锁模力（kN；注塑）"),
            // entity.productionequipment.clampingforcekn
            new TranslationSeedItem("entity.productionequipment.clampingforcekn", "zh-CN", "锁模力千牛", "锁模力（kN；注塑）"),
            // entity.productionequipment.clampingforcekn
            new TranslationSeedItem("entity.productionequipment.clampingforcekn", "zh-HK", "锁模力千牛_hk", "锁模力（kN；注塑）"),

            // entity.productionequipment.maxstrokemm
            new TranslationSeedItem("entity.productionequipment.maxstrokemm", "en-US", "最大行程毫米_us", "最大行程（mm）"),
            // entity.productionequipment.maxstrokemm
            new TranslationSeedItem("entity.productionequipment.maxstrokemm", "ja-JP", "最大行程毫米_jp", "最大行程（mm）"),
            // entity.productionequipment.maxstrokemm
            new TranslationSeedItem("entity.productionequipment.maxstrokemm", "zh-CN", "最大行程毫米", "最大行程（mm）"),
            // entity.productionequipment.maxstrokemm
            new TranslationSeedItem("entity.productionequipment.maxstrokemm", "zh-HK", "最大行程毫米_hk", "最大行程（mm）"),

            // entity.productionequipment.openstrokemm
            new TranslationSeedItem("entity.productionequipment.openstrokemm", "en-US", "开模行程毫米_us", "开模行程（mm；注塑）"),
            // entity.productionequipment.openstrokemm
            new TranslationSeedItem("entity.productionequipment.openstrokemm", "ja-JP", "开模行程毫米_jp", "开模行程（mm；注塑）"),
            // entity.productionequipment.openstrokemm
            new TranslationSeedItem("entity.productionequipment.openstrokemm", "zh-CN", "开模行程毫米", "开模行程（mm；注塑）"),
            // entity.productionequipment.openstrokemm
            new TranslationSeedItem("entity.productionequipment.openstrokemm", "zh-HK", "开模行程毫米_hk", "开模行程（mm；注塑）"),

            // entity.productionequipment.platensize
            new TranslationSeedItem("entity.productionequipment.platensize", "en-US", "模板尺寸_us", "模板尺寸（mm）"),
            // entity.productionequipment.platensize
            new TranslationSeedItem("entity.productionequipment.platensize", "ja-JP", "模板尺寸_jp", "模板尺寸（mm）"),
            // entity.productionequipment.platensize
            new TranslationSeedItem("entity.productionequipment.platensize", "zh-CN", "模板尺寸", "模板尺寸（mm）"),
            // entity.productionequipment.platensize
            new TranslationSeedItem("entity.productionequipment.platensize", "zh-HK", "模板尺寸_hk", "模板尺寸（mm）"),

            // entity.productionequipment.ratedvoltage
            new TranslationSeedItem("entity.productionequipment.ratedvoltage", "en-US", "额定电压_us", "使用电压（V）"),
            // entity.productionequipment.ratedvoltage
            new TranslationSeedItem("entity.productionequipment.ratedvoltage", "ja-JP", "额定电压_jp", "使用电压（V）"),
            // entity.productionequipment.ratedvoltage
            new TranslationSeedItem("entity.productionequipment.ratedvoltage", "zh-CN", "额定电压", "使用电压（V）"),
            // entity.productionequipment.ratedvoltage
            new TranslationSeedItem("entity.productionequipment.ratedvoltage", "zh-HK", "额定电压_hk", "使用电压（V）"),

            // entity.productionequipment.ratedpowerkw
            new TranslationSeedItem("entity.productionequipment.ratedpowerkw", "en-US", "额定功率千瓦_us", "额定功率（kW）"),
            // entity.productionequipment.ratedpowerkw
            new TranslationSeedItem("entity.productionequipment.ratedpowerkw", "ja-JP", "额定功率千瓦_jp", "额定功率（kW）"),
            // entity.productionequipment.ratedpowerkw
            new TranslationSeedItem("entity.productionequipment.ratedpowerkw", "zh-CN", "额定功率千瓦", "额定功率（kW）"),
            // entity.productionequipment.ratedpowerkw
            new TranslationSeedItem("entity.productionequipment.ratedpowerkw", "zh-HK", "额定功率千瓦_hk", "额定功率（kW）"),

            // entity.productionequipment.airconsumptionlpm
            new TranslationSeedItem("entity.productionequipment.airconsumptionlpm", "en-US", "耗气量升每分钟_us", "耗气量（L/min）"),
            // entity.productionequipment.airconsumptionlpm
            new TranslationSeedItem("entity.productionequipment.airconsumptionlpm", "ja-JP", "耗气量升每分钟_jp", "耗气量（L/min）"),
            // entity.productionequipment.airconsumptionlpm
            new TranslationSeedItem("entity.productionequipment.airconsumptionlpm", "zh-CN", "耗气量升每分钟", "耗气量（L/min）"),
            // entity.productionequipment.airconsumptionlpm
            new TranslationSeedItem("entity.productionequipment.airconsumptionlpm", "zh-HK", "耗气量升每分钟_hk", "耗气量（L/min）"),

            // entity.productionequipment.coolingwaterflowlpm
            new TranslationSeedItem("entity.productionequipment.coolingwaterflowlpm", "en-US", "冷却水流量升每分钟_us", "冷却水流量（L/min）"),
            // entity.productionequipment.coolingwaterflowlpm
            new TranslationSeedItem("entity.productionequipment.coolingwaterflowlpm", "ja-JP", "冷却水流量升每分钟_jp", "冷却水流量（L/min）"),
            // entity.productionequipment.coolingwaterflowlpm
            new TranslationSeedItem("entity.productionequipment.coolingwaterflowlpm", "zh-CN", "冷却水流量升每分钟", "冷却水流量（L/min）"),
            // entity.productionequipment.coolingwaterflowlpm
            new TranslationSeedItem("entity.productionequipment.coolingwaterflowlpm", "zh-HK", "冷却水流量升每分钟_hk", "冷却水流量（L/min）"),

            // entity.productionequipment.operatorcount
            new TranslationSeedItem("entity.productionequipment.operatorcount", "en-US", "操作人员数_us", "操作人员数（标准配人）"),
            // entity.productionequipment.operatorcount
            new TranslationSeedItem("entity.productionequipment.operatorcount", "ja-JP", "操作人员数_jp", "操作人员数（标准配人）"),
            // entity.productionequipment.operatorcount
            new TranslationSeedItem("entity.productionequipment.operatorcount", "zh-CN", "操作人员数", "操作人员数（标准配人）"),
            // entity.productionequipment.operatorcount
            new TranslationSeedItem("entity.productionequipment.operatorcount", "zh-HK", "操作人员数_hk", "操作人员数（标准配人）"),

            // entity.productionequipment.iscriticalresource
            new TranslationSeedItem("entity.productionequipment.iscriticalresource", "en-US", "是否关键设备_us", "是否关键设备（字典 sys_yes_no；RCCP/粗能力）"),
            // entity.productionequipment.iscriticalresource
            new TranslationSeedItem("entity.productionequipment.iscriticalresource", "ja-JP", "是否关键设备_jp", "是否关键设备（字典 sys_yes_no；RCCP/粗能力）"),
            // entity.productionequipment.iscriticalresource
            new TranslationSeedItem("entity.productionequipment.iscriticalresource", "zh-CN", "是否关键设备", "是否关键设备（字典 sys_yes_no；RCCP/粗能力）"),
            // entity.productionequipment.iscriticalresource
            new TranslationSeedItem("entity.productionequipment.iscriticalresource", "zh-HK", "是否关键设备_hk", "是否关键设备（字典 sys_yes_no；RCCP/粗能力）"),

            // entity.productionequipment.parallelcapacity
            new TranslationSeedItem("entity.productionequipment.parallelcapacity", "en-US", "并行能力_us", "并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）"),
            // entity.productionequipment.parallelcapacity
            new TranslationSeedItem("entity.productionequipment.parallelcapacity", "ja-JP", "并行能力_jp", "并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）"),
            // entity.productionequipment.parallelcapacity
            new TranslationSeedItem("entity.productionequipment.parallelcapacity", "zh-CN", "并行能力", "并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）"),
            // entity.productionequipment.parallelcapacity
            new TranslationSeedItem("entity.productionequipment.parallelcapacity", "zh-HK", "并行能力_hk", "并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）"),

            // entity.productionequipment.allowrushorder
            new TranslationSeedItem("entity.productionequipment.allowrushorder", "en-US", "是否允许插单_us", "是否允许插单（字典 sys_yes_no）"),
            // entity.productionequipment.allowrushorder
            new TranslationSeedItem("entity.productionequipment.allowrushorder", "ja-JP", "是否允许插单_jp", "是否允许插单（字典 sys_yes_no）"),
            // entity.productionequipment.allowrushorder
            new TranslationSeedItem("entity.productionequipment.allowrushorder", "zh-CN", "是否允许插单", "是否允许插单（字典 sys_yes_no）"),
            // entity.productionequipment.allowrushorder
            new TranslationSeedItem("entity.productionequipment.allowrushorder", "zh-HK", "是否允许插单_hk", "是否允许插单（字典 sys_yes_no）"),

            // entity.productionequipment.warmupminutes
            new TranslationSeedItem("entity.productionequipment.warmupminutes", "en-US", "开机预热时间分钟_us", "开机预热时间（分钟；注塑螺杆预热）"),
            // entity.productionequipment.warmupminutes
            new TranslationSeedItem("entity.productionequipment.warmupminutes", "ja-JP", "开机预热时间分钟_jp", "开机预热时间（分钟；注塑螺杆预热）"),
            // entity.productionequipment.warmupminutes
            new TranslationSeedItem("entity.productionequipment.warmupminutes", "zh-CN", "开机预热时间分钟", "开机预热时间（分钟；注塑螺杆预热）"),
            // entity.productionequipment.warmupminutes
            new TranslationSeedItem("entity.productionequipment.warmupminutes", "zh-HK", "开机预热时间分钟_hk", "开机预热时间（分钟；注塑螺杆预热）"),

            // entity.productionequipment.operatingtemprange
            new TranslationSeedItem("entity.productionequipment.operatingtemprange", "en-US", "工作温度范围_us", "工作温度范围（℃）"),
            // entity.productionequipment.operatingtemprange
            new TranslationSeedItem("entity.productionequipment.operatingtemprange", "ja-JP", "工作温度范围_jp", "工作温度范围（℃）"),
            // entity.productionequipment.operatingtemprange
            new TranslationSeedItem("entity.productionequipment.operatingtemprange", "zh-CN", "工作温度范围", "工作温度范围（℃）"),
            // entity.productionequipment.operatingtemprange
            new TranslationSeedItem("entity.productionequipment.operatingtemprange", "zh-HK", "工作温度范围_hk", "工作温度范围（℃）"),

            // entity.productionequipment.operatinghumidityrange
            new TranslationSeedItem("entity.productionequipment.operatinghumidityrange", "en-US", "工作湿度范围_us", "湿度范围（%RH）"),
            // entity.productionequipment.operatinghumidityrange
            new TranslationSeedItem("entity.productionequipment.operatinghumidityrange", "ja-JP", "工作湿度范围_jp", "湿度范围（%RH）"),
            // entity.productionequipment.operatinghumidityrange
            new TranslationSeedItem("entity.productionequipment.operatinghumidityrange", "zh-CN", "工作湿度范围", "湿度范围（%RH）"),
            // entity.productionequipment.operatinghumidityrange
            new TranslationSeedItem("entity.productionequipment.operatinghumidityrange", "zh-HK", "工作湿度范围_hk", "湿度范围（%RH）"),

            // entity.productionequipment.noiseleveldb
            new TranslationSeedItem("entity.productionequipment.noiseleveldb", "en-US", "噪音水平分贝_us", "噪音水平（dB）"),
            // entity.productionequipment.noiseleveldb
            new TranslationSeedItem("entity.productionequipment.noiseleveldb", "ja-JP", "噪音水平分贝_jp", "噪音水平（dB）"),
            // entity.productionequipment.noiseleveldb
            new TranslationSeedItem("entity.productionequipment.noiseleveldb", "zh-CN", "噪音水平分贝", "噪音水平（dB）"),
            // entity.productionequipment.noiseleveldb
            new TranslationSeedItem("entity.productionequipment.noiseleveldb", "zh-HK", "噪音水平分贝_hk", "噪音水平（dB）"),

            // entity.productionequipment.equipmentrunstatus
            new TranslationSeedItem("entity.productionequipment.equipmentrunstatus", "en-US", "设备运行状态_us", "设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）"),
            // entity.productionequipment.equipmentrunstatus
            new TranslationSeedItem("entity.productionequipment.equipmentrunstatus", "ja-JP", "设备运行状态_jp", "设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）"),
            // entity.productionequipment.equipmentrunstatus
            new TranslationSeedItem("entity.productionequipment.equipmentrunstatus", "zh-CN", "设备运行状态", "设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）"),
            // entity.productionequipment.equipmentrunstatus
            new TranslationSeedItem("entity.productionequipment.equipmentrunstatus", "zh-HK", "设备运行状态_hk", "设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）"),

            // entity.productionequipment.maintenanceintervalhours
            new TranslationSeedItem("entity.productionequipment.maintenanceintervalhours", "en-US", "保养周期小时_us", "保养周期（小时）"),
            // entity.productionequipment.maintenanceintervalhours
            new TranslationSeedItem("entity.productionequipment.maintenanceintervalhours", "ja-JP", "保养周期小时_jp", "保养周期（小时）"),
            // entity.productionequipment.maintenanceintervalhours
            new TranslationSeedItem("entity.productionequipment.maintenanceintervalhours", "zh-CN", "保养周期小时", "保养周期（小时）"),
            // entity.productionequipment.maintenanceintervalhours
            new TranslationSeedItem("entity.productionequipment.maintenanceintervalhours", "zh-HK", "保养周期小时_hk", "保养周期（小时）"),

            // entity.productionequipment.cumulativerunhours
            new TranslationSeedItem("entity.productionequipment.cumulativerunhours", "en-US", "累计运行时间小时_us", "累计运行时间（小时；寿命/PM）"),
            // entity.productionequipment.cumulativerunhours
            new TranslationSeedItem("entity.productionequipment.cumulativerunhours", "ja-JP", "累计运行时间小时_jp", "累计运行时间（小时；寿命/PM）"),
            // entity.productionequipment.cumulativerunhours
            new TranslationSeedItem("entity.productionequipment.cumulativerunhours", "zh-CN", "累计运行时间小时", "累计运行时间（小时；寿命/PM）"),
            // entity.productionequipment.cumulativerunhours
            new TranslationSeedItem("entity.productionequipment.cumulativerunhours", "zh-HK", "累计运行时间小时_hk", "累计运行时间（小时；寿命/PM）"),

            // entity.productionequipment.interfacetype
            new TranslationSeedItem("entity.productionequipment.interfacetype", "en-US", "车间集成接口_us", "车间集成接口（SMEMA/PLC 等）"),
            // entity.productionequipment.interfacetype
            new TranslationSeedItem("entity.productionequipment.interfacetype", "ja-JP", "车间集成接口_jp", "车间集成接口（SMEMA/PLC 等）"),
            // entity.productionequipment.interfacetype
            new TranslationSeedItem("entity.productionequipment.interfacetype", "zh-CN", "车间集成接口", "车间集成接口（SMEMA/PLC 等）"),
            // entity.productionequipment.interfacetype
            new TranslationSeedItem("entity.productionequipment.interfacetype", "zh-HK", "车间集成接口_hk", "车间集成接口（SMEMA/PLC 等）"),

            // entity.productionequipment.commissioningdate
            new TranslationSeedItem("entity.productionequipment.commissioningdate", "en-US", "投产日期_us", "投产日期（Commissioning Date；设备正式投产日期）"),
            // entity.productionequipment.commissioningdate
            new TranslationSeedItem("entity.productionequipment.commissioningdate", "ja-JP", "投产日期_jp", "投产日期（Commissioning Date；设备正式投产日期）"),
            // entity.productionequipment.commissioningdate
            new TranslationSeedItem("entity.productionequipment.commissioningdate", "zh-CN", "投产日期", "投产日期（Commissioning Date；设备正式投产日期）"),
            // entity.productionequipment.commissioningdate
            new TranslationSeedItem("entity.productionequipment.commissioningdate", "zh-HK", "投产日期_hk", "投产日期（Commissioning Date；设备正式投产日期）"),

            // entity.productionequipment.decommissioningdate
            new TranslationSeedItem("entity.productionequipment.decommissioningdate", "en-US", "停产日期_us", "停产日期（Decommissioning Date；设备停止生产日期）"),
            // entity.productionequipment.decommissioningdate
            new TranslationSeedItem("entity.productionequipment.decommissioningdate", "ja-JP", "停产日期_jp", "停产日期（Decommissioning Date；设备停止生产日期）"),
            // entity.productionequipment.decommissioningdate
            new TranslationSeedItem("entity.productionequipment.decommissioningdate", "zh-CN", "停产日期", "停产日期（Decommissioning Date；设备停止生产日期）"),
            // entity.productionequipment.decommissioningdate
            new TranslationSeedItem("entity.productionequipment.decommissioningdate", "zh-HK", "停产日期_hk", "停产日期（Decommissioning Date；设备停止生产日期）"),

            // entity.productionequipment.scrapdate
            new TranslationSeedItem("entity.productionequipment.scrapdate", "en-US", "报废日期_us", "报废日期（资产注销 / Scrap Date）"),
            // entity.productionequipment.scrapdate
            new TranslationSeedItem("entity.productionequipment.scrapdate", "ja-JP", "报废日期_jp", "报废日期（资产注销 / Scrap Date）"),
            // entity.productionequipment.scrapdate
            new TranslationSeedItem("entity.productionequipment.scrapdate", "zh-CN", "报废日期", "报废日期（资产注销 / Scrap Date）"),
            // entity.productionequipment.scrapdate
            new TranslationSeedItem("entity.productionequipment.scrapdate", "zh-HK", "报废日期_hk", "报废日期（资产注销 / Scrap Date）"),

            // entity.productionequipment.storagelocation
            new TranslationSeedItem("entity.productionequipment.storagelocation", "en-US", "存放位置_us", "存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）"),
            // entity.productionequipment.storagelocation
            new TranslationSeedItem("entity.productionequipment.storagelocation", "ja-JP", "存放位置_jp", "存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）"),
            // entity.productionequipment.storagelocation
            new TranslationSeedItem("entity.productionequipment.storagelocation", "zh-CN", "存放位置", "存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）"),
            // entity.productionequipment.storagelocation
            new TranslationSeedItem("entity.productionequipment.storagelocation", "zh-HK", "存放位置_hk", "存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）"),

            // entity.productionequipment.equipadministratorid
            new TranslationSeedItem("entity.productionequipment.equipadministratorid", "en-US", "设备管理员ID_us", "设备管理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionequipment.equipadministratorid
            new TranslationSeedItem("entity.productionequipment.equipadministratorid", "ja-JP", "设备管理员ID_jp", "设备管理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionequipment.equipadministratorid
            new TranslationSeedItem("entity.productionequipment.equipadministratorid", "zh-CN", "设备管理员ID", "设备管理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.productionequipment.equipadministratorid
            new TranslationSeedItem("entity.productionequipment.equipadministratorid", "zh-HK", "设备管理员ID_hk", "设备管理员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.productionequipment.equipadministratorname
            new TranslationSeedItem("entity.productionequipment.equipadministratorname", "en-US", "设备管理员名称_us", "设备管理员名称（冗余：按 EquipAdministratorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionequipment.equipadministratorname
            new TranslationSeedItem("entity.productionequipment.equipadministratorname", "ja-JP", "设备管理员名称_jp", "设备管理员名称（冗余：按 EquipAdministratorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionequipment.equipadministratorname
            new TranslationSeedItem("entity.productionequipment.equipadministratorname", "zh-CN", "设备管理员名称", "设备管理员名称（冗余：按 EquipAdministratorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.productionequipment.equipadministratorname
            new TranslationSeedItem("entity.productionequipment.equipadministratorname", "zh-HK", "设备管理员名称_hk", "设备管理员名称（冗余：按 EquipAdministratorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.productionequipment.sortorder
            new TranslationSeedItem("entity.productionequipment.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.productionequipment.sortorder
            new TranslationSeedItem("entity.productionequipment.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.productionequipment.sortorder
            new TranslationSeedItem("entity.productionequipment.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.productionequipment.sortorder
            new TranslationSeedItem("entity.productionequipment.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.productionequipment.prodequipstatus
            new TranslationSeedItem("entity.productionequipment.prodequipstatus", "en-US", "生产设备状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionequipment.prodequipstatus
            new TranslationSeedItem("entity.productionequipment.prodequipstatus", "ja-JP", "生产设备状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionequipment.prodequipstatus
            new TranslationSeedItem("entity.productionequipment.prodequipstatus", "zh-CN", "生产设备状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionequipment.prodequipstatus
            new TranslationSeedItem("entity.productionequipment.prodequipstatus", "zh-HK", "生产设备状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
