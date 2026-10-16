// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPackagingMaterialI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPackagingMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPackagingMaterial 实体国际化翻译种子（键前缀 entity.packagingmaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPackagingMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPackagingMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 packagingmaterial 实体翻译...", tenantCode);

        foreach (var item in GetPackagingMaterialTranslations())
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

        TaktLogger.Information("TaktPackagingMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPackagingMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.packagingmaterial._self / entity.packagingmaterial.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPackagingMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.packagingmaterial._self
            new TranslationSeedItem("entity.packagingmaterial._self", "en-US", "Packaging Material Information_us", "实体名称"),
            // entity.packagingmaterial._self
            new TranslationSeedItem("entity.packagingmaterial._self", "ja-JP", "Takt包装物料信息_jp", "实体名称"),
            // entity.packagingmaterial._self
            new TranslationSeedItem("entity.packagingmaterial._self", "zh-CN", "Takt包装物料信息", "实体名称"),
            // entity.packagingmaterial._self
            new TranslationSeedItem("entity.packagingmaterial._self", "zh-HK", "Takt包装物料信息_hk", "实体名称"),

            // entity.packagingmaterial.code
            new TranslationSeedItem("entity.packagingmaterial.code", "en-US", "包装物料编码_us", "包装物料编码（前端表单选择编码规则后自动通过 TaktNumbering 包装物料编码规则生成并展示，非手输；单据类型菜单：包装物料）"),
            // entity.packagingmaterial.code
            new TranslationSeedItem("entity.packagingmaterial.code", "ja-JP", "包装物料编码_jp", "包装物料编码（前端表单选择编码规则后自动通过 TaktNumbering 包装物料编码规则生成并展示，非手输；单据类型菜单：包装物料）"),
            // entity.packagingmaterial.code
            new TranslationSeedItem("entity.packagingmaterial.code", "zh-CN", "包装物料编码", "包装物料编码（前端表单选择编码规则后自动通过 TaktNumbering 包装物料编码规则生成并展示，非手输；单据类型菜单：包装物料）"),
            // entity.packagingmaterial.code
            new TranslationSeedItem("entity.packagingmaterial.code", "zh-HK", "包装物料编码_hk", "包装物料编码（前端表单选择编码规则后自动通过 TaktNumbering 包装物料编码规则生成并展示，非手输；单据类型菜单：包装物料）"),

            // entity.packagingmaterial.materialcode
            new TranslationSeedItem("entity.packagingmaterial.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.packagingmaterial.materialcode
            new TranslationSeedItem("entity.packagingmaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.packagingmaterial.materialcode
            new TranslationSeedItem("entity.packagingmaterial.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.packagingmaterial.materialcode
            new TranslationSeedItem("entity.packagingmaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.packagingmaterial.materialdescription
            new TranslationSeedItem("entity.packagingmaterial.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.packagingmaterial.materialdescription
            new TranslationSeedItem("entity.packagingmaterial.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.packagingmaterial.materialdescription
            new TranslationSeedItem("entity.packagingmaterial.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.packagingmaterial.materialdescription
            new TranslationSeedItem("entity.packagingmaterial.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.packagingmaterial.hscode
            new TranslationSeedItem("entity.packagingmaterial.hscode", "en-US", "海关商品编码_us", "海关商品编码（HS Code）"),
            // entity.packagingmaterial.hscode
            new TranslationSeedItem("entity.packagingmaterial.hscode", "ja-JP", "海关商品编码_jp", "海关商品编码（HS Code）"),
            // entity.packagingmaterial.hscode
            new TranslationSeedItem("entity.packagingmaterial.hscode", "zh-CN", "海关商品编码", "海关商品编码（HS Code）"),
            // entity.packagingmaterial.hscode
            new TranslationSeedItem("entity.packagingmaterial.hscode", "zh-HK", "海关商品编码_hk", "海关商品编码（HS Code）"),

            // entity.packagingmaterial.hsname
            new TranslationSeedItem("entity.packagingmaterial.hsname", "en-US", "商品名称_us", "商品名称（HS Name；海关申报完整品名，可超默认短串）"),
            // entity.packagingmaterial.hsname
            new TranslationSeedItem("entity.packagingmaterial.hsname", "ja-JP", "商品名称_jp", "商品名称（HS Name；海关申报完整品名，可超默认短串）"),
            // entity.packagingmaterial.hsname
            new TranslationSeedItem("entity.packagingmaterial.hsname", "zh-CN", "商品名称", "商品名称（HS Name；海关申报完整品名，可超默认短串）"),
            // entity.packagingmaterial.hsname
            new TranslationSeedItem("entity.packagingmaterial.hsname", "zh-HK", "商品名称_hk", "商品名称（HS Name；海关申报完整品名，可超默认短串）"),

            // entity.packagingmaterial.additionalcode
            new TranslationSeedItem("entity.packagingmaterial.additionalcode", "en-US", "附加编码_us", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packagingmaterial.additionalcode
            new TranslationSeedItem("entity.packagingmaterial.additionalcode", "ja-JP", "附加编码_jp", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packagingmaterial.additionalcode
            new TranslationSeedItem("entity.packagingmaterial.additionalcode", "zh-CN", "附加编码", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packagingmaterial.additionalcode
            new TranslationSeedItem("entity.packagingmaterial.additionalcode", "zh-HK", "附加编码_hk", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),

            // entity.packagingmaterial.origincountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.origincountryregioncode", "en-US", "原产国/地区编码_us", "原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.origincountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.origincountryregioncode", "ja-JP", "原产国/地区编码_jp", "原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.origincountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.origincountryregioncode", "zh-CN", "原产国/地区编码", "原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.origincountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.origincountryregioncode", "zh-HK", "原产国/地区编码_hk", "原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),

            // entity.packagingmaterial.origincountryregionname
            new TranslationSeedItem("entity.packagingmaterial.origincountryregionname", "en-US", "原产国/地区名称_us", "原产国/地区名称"),
            // entity.packagingmaterial.origincountryregionname
            new TranslationSeedItem("entity.packagingmaterial.origincountryregionname", "ja-JP", "原产国/地区名称_jp", "原产国/地区名称"),
            // entity.packagingmaterial.origincountryregionname
            new TranslationSeedItem("entity.packagingmaterial.origincountryregionname", "zh-CN", "原产国/地区名称", "原产国/地区名称"),
            // entity.packagingmaterial.origincountryregionname
            new TranslationSeedItem("entity.packagingmaterial.origincountryregionname", "zh-HK", "原产国/地区名称_hk", "原产国/地区名称"),

            // entity.packagingmaterial.destinationcountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregioncode", "en-US", "目的国/地区编码_us", "目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.destinationcountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregioncode", "ja-JP", "目的国/地区编码_jp", "目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.destinationcountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregioncode", "zh-CN", "目的国/地区编码", "目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),
            // entity.packagingmaterial.destinationcountryregioncode
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregioncode", "zh-HK", "目的国/地区编码_hk", "目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）"),

            // entity.packagingmaterial.destinationcountryregionname
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregionname", "en-US", "目的国/地区名称_us", "目的国/地区名称"),
            // entity.packagingmaterial.destinationcountryregionname
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregionname", "ja-JP", "目的国/地区名称_jp", "目的国/地区名称"),
            // entity.packagingmaterial.destinationcountryregionname
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregionname", "zh-CN", "目的国/地区名称", "目的国/地区名称"),
            // entity.packagingmaterial.destinationcountryregionname
            new TranslationSeedItem("entity.packagingmaterial.destinationcountryregionname", "zh-HK", "目的国/地区名称_hk", "目的国/地区名称"),

            // entity.packagingmaterial.regulatoryconditioncode
            new TranslationSeedItem("entity.packagingmaterial.regulatoryconditioncode", "en-US", "监管条件代码_us", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packagingmaterial.regulatoryconditioncode
            new TranslationSeedItem("entity.packagingmaterial.regulatoryconditioncode", "ja-JP", "监管条件代码_jp", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packagingmaterial.regulatoryconditioncode
            new TranslationSeedItem("entity.packagingmaterial.regulatoryconditioncode", "zh-CN", "监管条件代码", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packagingmaterial.regulatoryconditioncode
            new TranslationSeedItem("entity.packagingmaterial.regulatoryconditioncode", "zh-HK", "监管条件代码_hk", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),

            // entity.packagingmaterial.tariffratetype
            new TranslationSeedItem("entity.packagingmaterial.tariffratetype", "en-US", "税率/协定税率标识_us", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packagingmaterial.tariffratetype
            new TranslationSeedItem("entity.packagingmaterial.tariffratetype", "ja-JP", "税率/协定税率标识_jp", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packagingmaterial.tariffratetype
            new TranslationSeedItem("entity.packagingmaterial.tariffratetype", "zh-CN", "税率/协定税率标识", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packagingmaterial.tariffratetype
            new TranslationSeedItem("entity.packagingmaterial.tariffratetype", "zh-HK", "税率/协定税率标识_hk", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),

            // entity.packagingmaterial.grossweight
            new TranslationSeedItem("entity.packagingmaterial.grossweight", "en-US", "毛重_us", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packagingmaterial.grossweight
            new TranslationSeedItem("entity.packagingmaterial.grossweight", "ja-JP", "毛重_jp", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packagingmaterial.grossweight
            new TranslationSeedItem("entity.packagingmaterial.grossweight", "zh-CN", "毛重", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packagingmaterial.grossweight
            new TranslationSeedItem("entity.packagingmaterial.grossweight", "zh-HK", "毛重_hk", "毛重（包含包装物的总重量，单位：千克）"),

            // entity.packagingmaterial.netweight
            new TranslationSeedItem("entity.packagingmaterial.netweight", "en-US", "净重_us", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packagingmaterial.netweight
            new TranslationSeedItem("entity.packagingmaterial.netweight", "ja-JP", "净重_jp", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packagingmaterial.netweight
            new TranslationSeedItem("entity.packagingmaterial.netweight", "zh-CN", "净重", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packagingmaterial.netweight
            new TranslationSeedItem("entity.packagingmaterial.netweight", "zh-HK", "净重_hk", "净重（不含包装物的净重量，单位：千克）"),

            // entity.packagingmaterial.weightunit
            new TranslationSeedItem("entity.packagingmaterial.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）"),
            // entity.packagingmaterial.weightunit
            new TranslationSeedItem("entity.packagingmaterial.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）"),
            // entity.packagingmaterial.weightunit
            new TranslationSeedItem("entity.packagingmaterial.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）"),
            // entity.packagingmaterial.weightunit
            new TranslationSeedItem("entity.packagingmaterial.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）"),

            // entity.packagingmaterial.businessvolume
            new TranslationSeedItem("entity.packagingmaterial.businessvolume", "en-US", "业务量_us", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packagingmaterial.businessvolume
            new TranslationSeedItem("entity.packagingmaterial.businessvolume", "ja-JP", "业务量_jp", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packagingmaterial.businessvolume
            new TranslationSeedItem("entity.packagingmaterial.businessvolume", "zh-CN", "业务量", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packagingmaterial.businessvolume
            new TranslationSeedItem("entity.packagingmaterial.businessvolume", "zh-HK", "业务量_hk", "业务量/容积（一个包装单位的体积，单位：立方米）"),

            // entity.packagingmaterial.volumeunit
            new TranslationSeedItem("entity.packagingmaterial.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packagingmaterial.volumeunit
            new TranslationSeedItem("entity.packagingmaterial.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packagingmaterial.volumeunit
            new TranslationSeedItem("entity.packagingmaterial.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packagingmaterial.volumeunit
            new TranslationSeedItem("entity.packagingmaterial.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）"),

            // entity.packagingmaterial.sizedimension
            new TranslationSeedItem("entity.packagingmaterial.sizedimension", "en-US", "大小/量纲_us", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packagingmaterial.sizedimension
            new TranslationSeedItem("entity.packagingmaterial.sizedimension", "ja-JP", "大小/量纲_jp", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packagingmaterial.sizedimension
            new TranslationSeedItem("entity.packagingmaterial.sizedimension", "zh-CN", "大小/量纲", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packagingmaterial.sizedimension
            new TranslationSeedItem("entity.packagingmaterial.sizedimension", "zh-HK", "大小/量纲_hk", "大小/量纲（尺寸量纲或大小规格）"),

            // entity.packagingmaterial.packagingtype
            new TranslationSeedItem("entity.packagingmaterial.packagingtype", "en-US", "包装类型_us", "包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）"),
            // entity.packagingmaterial.packagingtype
            new TranslationSeedItem("entity.packagingmaterial.packagingtype", "ja-JP", "包装类型_jp", "包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）"),
            // entity.packagingmaterial.packagingtype
            new TranslationSeedItem("entity.packagingmaterial.packagingtype", "zh-CN", "包装类型", "包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）"),
            // entity.packagingmaterial.packagingtype
            new TranslationSeedItem("entity.packagingmaterial.packagingtype", "zh-HK", "包装类型_hk", "包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）"),

            // entity.packagingmaterial.packingunit
            new TranslationSeedItem("entity.packagingmaterial.packingunit", "en-US", "包装单位_us", "包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packagingmaterial.packingunit
            new TranslationSeedItem("entity.packagingmaterial.packingunit", "ja-JP", "包装单位_jp", "包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packagingmaterial.packingunit
            new TranslationSeedItem("entity.packagingmaterial.packingunit", "zh-CN", "包装单位", "包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packagingmaterial.packingunit
            new TranslationSeedItem("entity.packagingmaterial.packingunit", "zh-HK", "包装单位_hk", "包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）"),

            // entity.packagingmaterial.quantityperpacking
            new TranslationSeedItem("entity.packagingmaterial.quantityperpacking", "en-US", "每包装数量_us", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packagingmaterial.quantityperpacking
            new TranslationSeedItem("entity.packagingmaterial.quantityperpacking", "ja-JP", "每包装数量_jp", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packagingmaterial.quantityperpacking
            new TranslationSeedItem("entity.packagingmaterial.quantityperpacking", "zh-CN", "每包装数量", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packagingmaterial.quantityperpacking
            new TranslationSeedItem("entity.packagingmaterial.quantityperpacking", "zh-HK", "每包装数量_hk", "每包装数量（一个包装包含的基本单位数量）"),

            // entity.packagingmaterial.packagingspec
            new TranslationSeedItem("entity.packagingmaterial.packagingspec", "en-US", "包装规格_us", "包装规格（含多段规格说明，可超默认短串）"),
            // entity.packagingmaterial.packagingspec
            new TranslationSeedItem("entity.packagingmaterial.packagingspec", "ja-JP", "包装规格_jp", "包装规格（含多段规格说明，可超默认短串）"),
            // entity.packagingmaterial.packagingspec
            new TranslationSeedItem("entity.packagingmaterial.packagingspec", "zh-CN", "包装规格", "包装规格（含多段规格说明，可超默认短串）"),
            // entity.packagingmaterial.packagingspec
            new TranslationSeedItem("entity.packagingmaterial.packagingspec", "zh-HK", "包装规格_hk", "包装规格（含多段规格说明，可超默认短串）"),

            // entity.packagingmaterial.packagingdescription
            new TranslationSeedItem("entity.packagingmaterial.packagingdescription", "en-US", "包装描述_us", "包装描述（超长说明，可超默认短串）"),
            // entity.packagingmaterial.packagingdescription
            new TranslationSeedItem("entity.packagingmaterial.packagingdescription", "ja-JP", "包装描述_jp", "包装描述（超长说明，可超默认短串）"),
            // entity.packagingmaterial.packagingdescription
            new TranslationSeedItem("entity.packagingmaterial.packagingdescription", "zh-CN", "包装描述", "包装描述（超长说明，可超默认短串）"),
            // entity.packagingmaterial.packagingdescription
            new TranslationSeedItem("entity.packagingmaterial.packagingdescription", "zh-HK", "包装描述_hk", "包装描述（超长说明，可超默认短串）"),

            // entity.packagingmaterial.sortorder
            new TranslationSeedItem("entity.packagingmaterial.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.packagingmaterial.sortorder
            new TranslationSeedItem("entity.packagingmaterial.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.packagingmaterial.sortorder
            new TranslationSeedItem("entity.packagingmaterial.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.packagingmaterial.sortorder
            new TranslationSeedItem("entity.packagingmaterial.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),
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
        translation.ResourceGroup = "Materials";
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
