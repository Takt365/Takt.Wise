// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktGeneralMaterialI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktGeneralMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktGeneralMaterial 实体国际化翻译种子（键前缀 entity.generalmaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktGeneralMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktGeneralMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 generalmaterial 实体翻译...", tenantCode);

        foreach (var item in GetGeneralMaterialTranslations())
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

        TaktLogger.Information("TaktGeneralMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktGeneralMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.generalmaterial._self / entity.generalmaterial.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetGeneralMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.generalmaterial._self
            new TranslationSeedItem("entity.generalmaterial._self", "en-US", "General Material Information_us", "实体名称"),
            // entity.generalmaterial._self
            new TranslationSeedItem("entity.generalmaterial._self", "ja-JP", "Takt全局物料信息_jp", "实体名称"),
            // entity.generalmaterial._self
            new TranslationSeedItem("entity.generalmaterial._self", "zh-CN", "Takt全局物料信息", "实体名称"),
            // entity.generalmaterial._self
            new TranslationSeedItem("entity.generalmaterial._self", "zh-HK", "Takt全局物料信息_hk", "实体名称"),

            // entity.generalmaterial.materialcode
            new TranslationSeedItem("entity.generalmaterial.materialcode", "en-US", "物料编码_us", "物料编码（租户内唯一）"),
            // entity.generalmaterial.materialcode
            new TranslationSeedItem("entity.generalmaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（租户内唯一）"),
            // entity.generalmaterial.materialcode
            new TranslationSeedItem("entity.generalmaterial.materialcode", "zh-CN", "物料编码", "物料编码（租户内唯一）"),
            // entity.generalmaterial.materialcode
            new TranslationSeedItem("entity.generalmaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（租户内唯一）"),

            // entity.generalmaterial.completemaintenancestatus
            new TranslationSeedItem("entity.generalmaterial.completemaintenancestatus", "en-US", "完整状态_us", "完整状态"),
            // entity.generalmaterial.completemaintenancestatus
            new TranslationSeedItem("entity.generalmaterial.completemaintenancestatus", "ja-JP", "完整状态_jp", "完整状态"),
            // entity.generalmaterial.completemaintenancestatus
            new TranslationSeedItem("entity.generalmaterial.completemaintenancestatus", "zh-CN", "完整状态", "完整状态"),
            // entity.generalmaterial.completemaintenancestatus
            new TranslationSeedItem("entity.generalmaterial.completemaintenancestatus", "zh-HK", "完整状态_hk", "完整状态"),

            // entity.generalmaterial.maintenancestatus
            new TranslationSeedItem("entity.generalmaterial.maintenancestatus", "en-US", "维护状态_us", "维护状态"),
            // entity.generalmaterial.maintenancestatus
            new TranslationSeedItem("entity.generalmaterial.maintenancestatus", "ja-JP", "维护状态_jp", "维护状态"),
            // entity.generalmaterial.maintenancestatus
            new TranslationSeedItem("entity.generalmaterial.maintenancestatus", "zh-CN", "维护状态", "维护状态"),
            // entity.generalmaterial.maintenancestatus
            new TranslationSeedItem("entity.generalmaterial.maintenancestatus", "zh-HK", "维护状态_hk", "维护状态"),

            // entity.generalmaterial.clientdeletionflag
            new TranslationSeedItem("entity.generalmaterial.clientdeletionflag", "en-US", "客户级删除标记_us", "客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）"),
            // entity.generalmaterial.clientdeletionflag
            new TranslationSeedItem("entity.generalmaterial.clientdeletionflag", "ja-JP", "客户级删除标记_jp", "客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）"),
            // entity.generalmaterial.clientdeletionflag
            new TranslationSeedItem("entity.generalmaterial.clientdeletionflag", "zh-CN", "客户级删除标记", "客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）"),
            // entity.generalmaterial.clientdeletionflag
            new TranslationSeedItem("entity.generalmaterial.clientdeletionflag", "zh-HK", "客户级删除标记_hk", "客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）"),

            // entity.generalmaterial.materialtype
            new TranslationSeedItem("entity.generalmaterial.materialtype", "en-US", "物料类型_us", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.generalmaterial.materialtype
            new TranslationSeedItem("entity.generalmaterial.materialtype", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.generalmaterial.materialtype
            new TranslationSeedItem("entity.generalmaterial.materialtype", "zh-CN", "物料类型", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.generalmaterial.materialtype
            new TranslationSeedItem("entity.generalmaterial.materialtype", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),

            // entity.generalmaterial.industrysector
            new TranslationSeedItem("entity.generalmaterial.industrysector", "en-US", "行业领域_us", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.generalmaterial.industrysector
            new TranslationSeedItem("entity.generalmaterial.industrysector", "ja-JP", "行业领域_jp", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.generalmaterial.industrysector
            new TranslationSeedItem("entity.generalmaterial.industrysector", "zh-CN", "行业领域", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.generalmaterial.industrysector
            new TranslationSeedItem("entity.generalmaterial.industrysector", "zh-HK", "行业领域_hk", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),

            // entity.generalmaterial.materialgroup
            new TranslationSeedItem("entity.generalmaterial.materialgroup", "en-US", "物料组_us", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.generalmaterial.materialgroup
            new TranslationSeedItem("entity.generalmaterial.materialgroup", "ja-JP", "物料组_jp", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.generalmaterial.materialgroup
            new TranslationSeedItem("entity.generalmaterial.materialgroup", "zh-CN", "物料组", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.generalmaterial.materialgroup
            new TranslationSeedItem("entity.generalmaterial.materialgroup", "zh-HK", "物料组_hk", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),

            // entity.generalmaterial.oldmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.oldmaterialnumber", "en-US", "旧物料号_us", "旧物料号"),
            // entity.generalmaterial.oldmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.oldmaterialnumber", "ja-JP", "旧物料号_jp", "旧物料号"),
            // entity.generalmaterial.oldmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.oldmaterialnumber", "zh-CN", "旧物料号", "旧物料号"),
            // entity.generalmaterial.oldmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.oldmaterialnumber", "zh-HK", "旧物料号_hk", "旧物料号"),

            // entity.generalmaterial.baseunit
            new TranslationSeedItem("entity.generalmaterial.baseunit", "en-US", "基本计量单位_us", "基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.generalmaterial.baseunit
            new TranslationSeedItem("entity.generalmaterial.baseunit", "ja-JP", "基本计量单位_jp", "基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.generalmaterial.baseunit
            new TranslationSeedItem("entity.generalmaterial.baseunit", "zh-CN", "基本计量单位", "基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.generalmaterial.baseunit
            new TranslationSeedItem("entity.generalmaterial.baseunit", "zh-HK", "基本计量单位_hk", "基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.generalmaterial.orderunit
            new TranslationSeedItem("entity.generalmaterial.orderunit", "en-US", "采购订单单位_us", "采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.orderunit
            new TranslationSeedItem("entity.generalmaterial.orderunit", "ja-JP", "采购订单单位_jp", "采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.orderunit
            new TranslationSeedItem("entity.generalmaterial.orderunit", "zh-CN", "采购订单单位", "采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.orderunit
            new TranslationSeedItem("entity.generalmaterial.orderunit", "zh-HK", "采购订单单位_hk", "采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.generalmaterial.documentnumber
            new TranslationSeedItem("entity.generalmaterial.documentnumber", "en-US", "单据号_us", "单据号"),
            // entity.generalmaterial.documentnumber
            new TranslationSeedItem("entity.generalmaterial.documentnumber", "ja-JP", "单据号_jp", "单据号"),
            // entity.generalmaterial.documentnumber
            new TranslationSeedItem("entity.generalmaterial.documentnumber", "zh-CN", "单据号", "单据号"),
            // entity.generalmaterial.documentnumber
            new TranslationSeedItem("entity.generalmaterial.documentnumber", "zh-HK", "单据号_hk", "单据号"),

            // entity.generalmaterial.documenttype
            new TranslationSeedItem("entity.generalmaterial.documenttype", "en-US", "单据类型_us", "单据类型（字典 logistics_document_type；DictValue=单据类型编码）"),
            // entity.generalmaterial.documenttype
            new TranslationSeedItem("entity.generalmaterial.documenttype", "ja-JP", "单据类型_jp", "单据类型（字典 logistics_document_type；DictValue=单据类型编码）"),
            // entity.generalmaterial.documenttype
            new TranslationSeedItem("entity.generalmaterial.documenttype", "zh-CN", "单据类型", "单据类型（字典 logistics_document_type；DictValue=单据类型编码）"),
            // entity.generalmaterial.documenttype
            new TranslationSeedItem("entity.generalmaterial.documenttype", "zh-HK", "单据类型_hk", "单据类型（字典 logistics_document_type；DictValue=单据类型编码）"),

            // entity.generalmaterial.documentversion
            new TranslationSeedItem("entity.generalmaterial.documentversion", "en-US", "单据版本_us", "单据版本"),
            // entity.generalmaterial.documentversion
            new TranslationSeedItem("entity.generalmaterial.documentversion", "ja-JP", "单据版本_jp", "单据版本"),
            // entity.generalmaterial.documentversion
            new TranslationSeedItem("entity.generalmaterial.documentversion", "zh-CN", "单据版本", "单据版本"),
            // entity.generalmaterial.documentversion
            new TranslationSeedItem("entity.generalmaterial.documentversion", "zh-HK", "单据版本_hk", "单据版本"),

            // entity.generalmaterial.documentpageformat
            new TranslationSeedItem("entity.generalmaterial.documentpageformat", "en-US", "单据页格式_us", "单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.documentpageformat
            new TranslationSeedItem("entity.generalmaterial.documentpageformat", "ja-JP", "单据页格式_jp", "单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.documentpageformat
            new TranslationSeedItem("entity.generalmaterial.documentpageformat", "zh-CN", "单据页格式", "单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.documentpageformat
            new TranslationSeedItem("entity.generalmaterial.documentpageformat", "zh-HK", "单据页格式_hk", "单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）"),

            // entity.generalmaterial.documentchangenumber
            new TranslationSeedItem("entity.generalmaterial.documentchangenumber", "en-US", "单据更改号_us", "单据更改号"),
            // entity.generalmaterial.documentchangenumber
            new TranslationSeedItem("entity.generalmaterial.documentchangenumber", "ja-JP", "单据更改号_jp", "单据更改号"),
            // entity.generalmaterial.documentchangenumber
            new TranslationSeedItem("entity.generalmaterial.documentchangenumber", "zh-CN", "单据更改号", "单据更改号"),
            // entity.generalmaterial.documentchangenumber
            new TranslationSeedItem("entity.generalmaterial.documentchangenumber", "zh-HK", "单据更改号_hk", "单据更改号"),

            // entity.generalmaterial.documentpagenumber
            new TranslationSeedItem("entity.generalmaterial.documentpagenumber", "en-US", "单据页号_us", "单据页号"),
            // entity.generalmaterial.documentpagenumber
            new TranslationSeedItem("entity.generalmaterial.documentpagenumber", "ja-JP", "单据页号_jp", "单据页号"),
            // entity.generalmaterial.documentpagenumber
            new TranslationSeedItem("entity.generalmaterial.documentpagenumber", "zh-CN", "单据页号", "单据页号"),
            // entity.generalmaterial.documentpagenumber
            new TranslationSeedItem("entity.generalmaterial.documentpagenumber", "zh-HK", "单据页号_hk", "单据页号"),

            // entity.generalmaterial.documentsheetcount
            new TranslationSeedItem("entity.generalmaterial.documentsheetcount", "en-US", "单据页数_us", "单据页数"),
            // entity.generalmaterial.documentsheetcount
            new TranslationSeedItem("entity.generalmaterial.documentsheetcount", "ja-JP", "单据页数_jp", "单据页数"),
            // entity.generalmaterial.documentsheetcount
            new TranslationSeedItem("entity.generalmaterial.documentsheetcount", "zh-CN", "单据页数", "单据页数"),
            // entity.generalmaterial.documentsheetcount
            new TranslationSeedItem("entity.generalmaterial.documentsheetcount", "zh-HK", "单据页数_hk", "单据页数"),

            // entity.generalmaterial.productioninspectionmemo
            new TranslationSeedItem("entity.generalmaterial.productioninspectionmemo", "en-US", "生产/检验备忘_us", "生产/检验备忘"),
            // entity.generalmaterial.productioninspectionmemo
            new TranslationSeedItem("entity.generalmaterial.productioninspectionmemo", "ja-JP", "生产/检验备忘_jp", "生产/检验备忘"),
            // entity.generalmaterial.productioninspectionmemo
            new TranslationSeedItem("entity.generalmaterial.productioninspectionmemo", "zh-CN", "生产/检验备忘", "生产/检验备忘"),
            // entity.generalmaterial.productioninspectionmemo
            new TranslationSeedItem("entity.generalmaterial.productioninspectionmemo", "zh-HK", "生产/检验备忘_hk", "生产/检验备忘"),

            // entity.generalmaterial.productionmemopageformat
            new TranslationSeedItem("entity.generalmaterial.productionmemopageformat", "en-US", "生产备忘页格式_us", "生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.productionmemopageformat
            new TranslationSeedItem("entity.generalmaterial.productionmemopageformat", "ja-JP", "生产备忘页格式_jp", "生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.productionmemopageformat
            new TranslationSeedItem("entity.generalmaterial.productionmemopageformat", "zh-CN", "生产备忘页格式", "生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）"),
            // entity.generalmaterial.productionmemopageformat
            new TranslationSeedItem("entity.generalmaterial.productionmemopageformat", "zh-HK", "生产备忘页格式_hk", "生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）"),

            // entity.generalmaterial.sizedimensions
            new TranslationSeedItem("entity.generalmaterial.sizedimensions", "en-US", "尺寸/规格_us", "尺寸/规格"),
            // entity.generalmaterial.sizedimensions
            new TranslationSeedItem("entity.generalmaterial.sizedimensions", "ja-JP", "尺寸/规格_jp", "尺寸/规格"),
            // entity.generalmaterial.sizedimensions
            new TranslationSeedItem("entity.generalmaterial.sizedimensions", "zh-CN", "尺寸/规格", "尺寸/规格"),
            // entity.generalmaterial.sizedimensions
            new TranslationSeedItem("entity.generalmaterial.sizedimensions", "zh-HK", "尺寸/规格_hk", "尺寸/规格"),

            // entity.generalmaterial.basicmaterial
            new TranslationSeedItem("entity.generalmaterial.basicmaterial", "en-US", "基本物料（材质）_us", "基本物料（材质）"),
            // entity.generalmaterial.basicmaterial
            new TranslationSeedItem("entity.generalmaterial.basicmaterial", "ja-JP", "基本物料（材质）_jp", "基本物料（材质）"),
            // entity.generalmaterial.basicmaterial
            new TranslationSeedItem("entity.generalmaterial.basicmaterial", "zh-CN", "基本物料（材质）", "基本物料（材质）"),
            // entity.generalmaterial.basicmaterial
            new TranslationSeedItem("entity.generalmaterial.basicmaterial", "zh-HK", "基本物料（材质）_hk", "基本物料（材质）"),

            // entity.generalmaterial.industrystandarddescription
            new TranslationSeedItem("entity.generalmaterial.industrystandarddescription", "en-US", "行业标准描述_us", "行业标准描述"),
            // entity.generalmaterial.industrystandarddescription
            new TranslationSeedItem("entity.generalmaterial.industrystandarddescription", "ja-JP", "行业标准描述_jp", "行业标准描述"),
            // entity.generalmaterial.industrystandarddescription
            new TranslationSeedItem("entity.generalmaterial.industrystandarddescription", "zh-CN", "行业标准描述", "行业标准描述"),
            // entity.generalmaterial.industrystandarddescription
            new TranslationSeedItem("entity.generalmaterial.industrystandarddescription", "zh-HK", "行业标准描述_hk", "行业标准描述"),

            // entity.generalmaterial.laboratorydesignoffice
            new TranslationSeedItem("entity.generalmaterial.laboratorydesignoffice", "en-US", "实验室/设计室_us", "实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）"),
            // entity.generalmaterial.laboratorydesignoffice
            new TranslationSeedItem("entity.generalmaterial.laboratorydesignoffice", "ja-JP", "实验室/设计室_jp", "实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）"),
            // entity.generalmaterial.laboratorydesignoffice
            new TranslationSeedItem("entity.generalmaterial.laboratorydesignoffice", "zh-CN", "实验室/设计室", "实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）"),
            // entity.generalmaterial.laboratorydesignoffice
            new TranslationSeedItem("entity.generalmaterial.laboratorydesignoffice", "zh-HK", "实验室/设计室_hk", "实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）"),

            // entity.generalmaterial.purchasingvaluekey
            new TranslationSeedItem("entity.generalmaterial.purchasingvaluekey", "en-US", "采购价值码_us", "采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）"),
            // entity.generalmaterial.purchasingvaluekey
            new TranslationSeedItem("entity.generalmaterial.purchasingvaluekey", "ja-JP", "采购价值码_jp", "采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）"),
            // entity.generalmaterial.purchasingvaluekey
            new TranslationSeedItem("entity.generalmaterial.purchasingvaluekey", "zh-CN", "采购价值码", "采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）"),
            // entity.generalmaterial.purchasingvaluekey
            new TranslationSeedItem("entity.generalmaterial.purchasingvaluekey", "zh-HK", "采购价值码_hk", "采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）"),

            // entity.generalmaterial.grossweight
            new TranslationSeedItem("entity.generalmaterial.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.generalmaterial.grossweight
            new TranslationSeedItem("entity.generalmaterial.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.generalmaterial.grossweight
            new TranslationSeedItem("entity.generalmaterial.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.generalmaterial.grossweight
            new TranslationSeedItem("entity.generalmaterial.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.generalmaterial.netweight
            new TranslationSeedItem("entity.generalmaterial.netweight", "en-US", "净重_us", "净重"),
            // entity.generalmaterial.netweight
            new TranslationSeedItem("entity.generalmaterial.netweight", "ja-JP", "净重_jp", "净重"),
            // entity.generalmaterial.netweight
            new TranslationSeedItem("entity.generalmaterial.netweight", "zh-CN", "净重", "净重"),
            // entity.generalmaterial.netweight
            new TranslationSeedItem("entity.generalmaterial.netweight", "zh-HK", "净重_hk", "净重"),

            // entity.generalmaterial.weightunit
            new TranslationSeedItem("entity.generalmaterial.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.weightunit
            new TranslationSeedItem("entity.generalmaterial.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.weightunit
            new TranslationSeedItem("entity.generalmaterial.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.weightunit
            new TranslationSeedItem("entity.generalmaterial.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.generalmaterial.volume
            new TranslationSeedItem("entity.generalmaterial.volume", "en-US", "体积_us", "体积"),
            // entity.generalmaterial.volume
            new TranslationSeedItem("entity.generalmaterial.volume", "ja-JP", "体积_jp", "体积"),
            // entity.generalmaterial.volume
            new TranslationSeedItem("entity.generalmaterial.volume", "zh-CN", "体积", "体积"),
            // entity.generalmaterial.volume
            new TranslationSeedItem("entity.generalmaterial.volume", "zh-HK", "体积_hk", "体积"),

            // entity.generalmaterial.volumeunit
            new TranslationSeedItem("entity.generalmaterial.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.volumeunit
            new TranslationSeedItem("entity.generalmaterial.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.volumeunit
            new TranslationSeedItem("entity.generalmaterial.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.volumeunit
            new TranslationSeedItem("entity.generalmaterial.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.generalmaterial.containerrequirements
            new TranslationSeedItem("entity.generalmaterial.containerrequirements", "en-US", "容器要求_us", "容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）"),
            // entity.generalmaterial.containerrequirements
            new TranslationSeedItem("entity.generalmaterial.containerrequirements", "ja-JP", "容器要求_jp", "容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）"),
            // entity.generalmaterial.containerrequirements
            new TranslationSeedItem("entity.generalmaterial.containerrequirements", "zh-CN", "容器要求", "容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）"),
            // entity.generalmaterial.containerrequirements
            new TranslationSeedItem("entity.generalmaterial.containerrequirements", "zh-HK", "容器要求_hk", "容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）"),

            // entity.generalmaterial.storageconditions
            new TranslationSeedItem("entity.generalmaterial.storageconditions", "en-US", "仓储条件_us", "仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）"),
            // entity.generalmaterial.storageconditions
            new TranslationSeedItem("entity.generalmaterial.storageconditions", "ja-JP", "仓储条件_jp", "仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）"),
            // entity.generalmaterial.storageconditions
            new TranslationSeedItem("entity.generalmaterial.storageconditions", "zh-CN", "仓储条件", "仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）"),
            // entity.generalmaterial.storageconditions
            new TranslationSeedItem("entity.generalmaterial.storageconditions", "zh-HK", "仓储条件_hk", "仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）"),

            // entity.generalmaterial.temperatureconditions
            new TranslationSeedItem("entity.generalmaterial.temperatureconditions", "en-US", "温度条件_us", "温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）"),
            // entity.generalmaterial.temperatureconditions
            new TranslationSeedItem("entity.generalmaterial.temperatureconditions", "ja-JP", "温度条件_jp", "温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）"),
            // entity.generalmaterial.temperatureconditions
            new TranslationSeedItem("entity.generalmaterial.temperatureconditions", "zh-CN", "温度条件", "温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）"),
            // entity.generalmaterial.temperatureconditions
            new TranslationSeedItem("entity.generalmaterial.temperatureconditions", "zh-HK", "温度条件_hk", "温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）"),

            // entity.generalmaterial.lowlevelcode
            new TranslationSeedItem("entity.generalmaterial.lowlevelcode", "en-US", "低层码_us", "低层码"),
            // entity.generalmaterial.lowlevelcode
            new TranslationSeedItem("entity.generalmaterial.lowlevelcode", "ja-JP", "低层码_jp", "低层码"),
            // entity.generalmaterial.lowlevelcode
            new TranslationSeedItem("entity.generalmaterial.lowlevelcode", "zh-CN", "低层码", "低层码"),
            // entity.generalmaterial.lowlevelcode
            new TranslationSeedItem("entity.generalmaterial.lowlevelcode", "zh-HK", "低层码_hk", "低层码"),

            // entity.generalmaterial.transportationgroup
            new TranslationSeedItem("entity.generalmaterial.transportationgroup", "en-US", "运输组_us", "运输组（字典 logistics_transportation_group；DictValue=运输组编码）"),
            // entity.generalmaterial.transportationgroup
            new TranslationSeedItem("entity.generalmaterial.transportationgroup", "ja-JP", "运输组_jp", "运输组（字典 logistics_transportation_group；DictValue=运输组编码）"),
            // entity.generalmaterial.transportationgroup
            new TranslationSeedItem("entity.generalmaterial.transportationgroup", "zh-CN", "运输组", "运输组（字典 logistics_transportation_group；DictValue=运输组编码）"),
            // entity.generalmaterial.transportationgroup
            new TranslationSeedItem("entity.generalmaterial.transportationgroup", "zh-HK", "运输组_hk", "运输组（字典 logistics_transportation_group；DictValue=运输组编码）"),

            // entity.generalmaterial.hazardousmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.hazardousmaterialnumber", "en-US", "危险品编码_us", "危险品编码"),
            // entity.generalmaterial.hazardousmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.hazardousmaterialnumber", "ja-JP", "危险品编码_jp", "危险品编码"),
            // entity.generalmaterial.hazardousmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.hazardousmaterialnumber", "zh-CN", "危险品编码", "危险品编码"),
            // entity.generalmaterial.hazardousmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.hazardousmaterialnumber", "zh-HK", "危险品编码_hk", "危险品编码"),

            // entity.generalmaterial.division
            new TranslationSeedItem("entity.generalmaterial.division", "en-US", "产品组_us", "产品组（字典 logistics_product_group；DictValue=产品组编码）"),
            // entity.generalmaterial.division
            new TranslationSeedItem("entity.generalmaterial.division", "ja-JP", "产品组_jp", "产品组（字典 logistics_product_group；DictValue=产品组编码）"),
            // entity.generalmaterial.division
            new TranslationSeedItem("entity.generalmaterial.division", "zh-CN", "产品组", "产品组（字典 logistics_product_group；DictValue=产品组编码）"),
            // entity.generalmaterial.division
            new TranslationSeedItem("entity.generalmaterial.division", "zh-HK", "产品组_hk", "产品组（字典 logistics_product_group；DictValue=产品组编码）"),

            // entity.generalmaterial.competitor
            new TranslationSeedItem("entity.generalmaterial.competitor", "en-US", "竞争对手_us", "竞争对手"),
            // entity.generalmaterial.competitor
            new TranslationSeedItem("entity.generalmaterial.competitor", "ja-JP", "竞争对手_jp", "竞争对手"),
            // entity.generalmaterial.competitor
            new TranslationSeedItem("entity.generalmaterial.competitor", "zh-CN", "竞争对手", "竞争对手"),
            // entity.generalmaterial.competitor
            new TranslationSeedItem("entity.generalmaterial.competitor", "zh-HK", "竞争对手_hk", "竞争对手"),

            // entity.generalmaterial.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.generalmaterial.europeanarticlenumberobsolete", "en-US", "欧洲商品号（旧）_us", "欧洲商品号（旧）"),
            // entity.generalmaterial.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.generalmaterial.europeanarticlenumberobsolete", "ja-JP", "欧洲商品号（旧）_jp", "欧洲商品号（旧）"),
            // entity.generalmaterial.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.generalmaterial.europeanarticlenumberobsolete", "zh-CN", "欧洲商品号（旧）", "欧洲商品号（旧）"),
            // entity.generalmaterial.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.generalmaterial.europeanarticlenumberobsolete", "zh-HK", "欧洲商品号（旧）_hk", "欧洲商品号（旧）"),

            // entity.generalmaterial.grgislipquantity
            new TranslationSeedItem("entity.generalmaterial.grgislipquantity", "en-US", "收发货凭证打印数量_us", "收发货凭证打印数量"),
            // entity.generalmaterial.grgislipquantity
            new TranslationSeedItem("entity.generalmaterial.grgislipquantity", "ja-JP", "收发货凭证打印数量_jp", "收发货凭证打印数量"),
            // entity.generalmaterial.grgislipquantity
            new TranslationSeedItem("entity.generalmaterial.grgislipquantity", "zh-CN", "收发货凭证打印数量", "收发货凭证打印数量"),
            // entity.generalmaterial.grgislipquantity
            new TranslationSeedItem("entity.generalmaterial.grgislipquantity", "zh-HK", "收发货凭证打印数量_hk", "收发货凭证打印数量"),

            // entity.generalmaterial.procurementrule
            new TranslationSeedItem("entity.generalmaterial.procurementrule", "en-US", "采购规则_us", "采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）"),
            // entity.generalmaterial.procurementrule
            new TranslationSeedItem("entity.generalmaterial.procurementrule", "ja-JP", "采购规则_jp", "采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）"),
            // entity.generalmaterial.procurementrule
            new TranslationSeedItem("entity.generalmaterial.procurementrule", "zh-CN", "采购规则", "采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）"),
            // entity.generalmaterial.procurementrule
            new TranslationSeedItem("entity.generalmaterial.procurementrule", "zh-HK", "采购规则_hk", "采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）"),

            // entity.generalmaterial.sourceofsupply
            new TranslationSeedItem("entity.generalmaterial.sourceofsupply", "en-US", "货源_us", "货源（字典 logistics_source_of_supply_type；DictValue=货源标识）"),
            // entity.generalmaterial.sourceofsupply
            new TranslationSeedItem("entity.generalmaterial.sourceofsupply", "ja-JP", "货源_jp", "货源（字典 logistics_source_of_supply_type；DictValue=货源标识）"),
            // entity.generalmaterial.sourceofsupply
            new TranslationSeedItem("entity.generalmaterial.sourceofsupply", "zh-CN", "货源", "货源（字典 logistics_source_of_supply_type；DictValue=货源标识）"),
            // entity.generalmaterial.sourceofsupply
            new TranslationSeedItem("entity.generalmaterial.sourceofsupply", "zh-HK", "货源_hk", "货源（字典 logistics_source_of_supply_type；DictValue=货源标识）"),

            // entity.generalmaterial.seasoncategory
            new TranslationSeedItem("entity.generalmaterial.seasoncategory", "en-US", "季节类别_us", "季节类别（字典 logistics_season_category；DictValue=季节类别编码）"),
            // entity.generalmaterial.seasoncategory
            new TranslationSeedItem("entity.generalmaterial.seasoncategory", "ja-JP", "季节类别_jp", "季节类别（字典 logistics_season_category；DictValue=季节类别编码）"),
            // entity.generalmaterial.seasoncategory
            new TranslationSeedItem("entity.generalmaterial.seasoncategory", "zh-CN", "季节类别", "季节类别（字典 logistics_season_category；DictValue=季节类别编码）"),
            // entity.generalmaterial.seasoncategory
            new TranslationSeedItem("entity.generalmaterial.seasoncategory", "zh-HK", "季节类别_hk", "季节类别（字典 logistics_season_category；DictValue=季节类别编码）"),

            // entity.generalmaterial.labeltype
            new TranslationSeedItem("entity.generalmaterial.labeltype", "en-US", "标签类型_us", "标签类型（字典 logistics_label_type；DictValue=标签类型编码）"),
            // entity.generalmaterial.labeltype
            new TranslationSeedItem("entity.generalmaterial.labeltype", "ja-JP", "标签类型_jp", "标签类型（字典 logistics_label_type；DictValue=标签类型编码）"),
            // entity.generalmaterial.labeltype
            new TranslationSeedItem("entity.generalmaterial.labeltype", "zh-CN", "标签类型", "标签类型（字典 logistics_label_type；DictValue=标签类型编码）"),
            // entity.generalmaterial.labeltype
            new TranslationSeedItem("entity.generalmaterial.labeltype", "zh-HK", "标签类型_hk", "标签类型（字典 logistics_label_type；DictValue=标签类型编码）"),

            // entity.generalmaterial.labelform
            new TranslationSeedItem("entity.generalmaterial.labelform", "en-US", "标签格式_us", "标签格式（字典 logistics_label_form；DictValue=标签格式编码）"),
            // entity.generalmaterial.labelform
            new TranslationSeedItem("entity.generalmaterial.labelform", "ja-JP", "标签格式_jp", "标签格式（字典 logistics_label_form；DictValue=标签格式编码）"),
            // entity.generalmaterial.labelform
            new TranslationSeedItem("entity.generalmaterial.labelform", "zh-CN", "标签格式", "标签格式（字典 logistics_label_form；DictValue=标签格式编码）"),
            // entity.generalmaterial.labelform
            new TranslationSeedItem("entity.generalmaterial.labelform", "zh-HK", "标签格式_hk", "标签格式（字典 logistics_label_form；DictValue=标签格式编码）"),

            // entity.generalmaterial.deactivatedfield
            new TranslationSeedItem("entity.generalmaterial.deactivatedfield", "en-US", "已停用字段_us", "已停用字段"),
            // entity.generalmaterial.deactivatedfield
            new TranslationSeedItem("entity.generalmaterial.deactivatedfield", "ja-JP", "已停用字段_jp", "已停用字段"),
            // entity.generalmaterial.deactivatedfield
            new TranslationSeedItem("entity.generalmaterial.deactivatedfield", "zh-CN", "已停用字段", "已停用字段"),
            // entity.generalmaterial.deactivatedfield
            new TranslationSeedItem("entity.generalmaterial.deactivatedfield", "zh-HK", "已停用字段_hk", "已停用字段"),

            // entity.generalmaterial.internationalarticlenumber
            new TranslationSeedItem("entity.generalmaterial.internationalarticlenumber", "en-US", "国际商品编码EAN/UPC_us", "国际商品编码EAN/UPC"),
            // entity.generalmaterial.internationalarticlenumber
            new TranslationSeedItem("entity.generalmaterial.internationalarticlenumber", "ja-JP", "国际商品编码EAN/UPC_jp", "国际商品编码EAN/UPC"),
            // entity.generalmaterial.internationalarticlenumber
            new TranslationSeedItem("entity.generalmaterial.internationalarticlenumber", "zh-CN", "国际商品编码EAN/UPC", "国际商品编码EAN/UPC"),
            // entity.generalmaterial.internationalarticlenumber
            new TranslationSeedItem("entity.generalmaterial.internationalarticlenumber", "zh-HK", "国际商品编码EAN/UPC_hk", "国际商品编码EAN/UPC"),

            // entity.generalmaterial.eancategory
            new TranslationSeedItem("entity.generalmaterial.eancategory", "en-US", "EAN类别_us", "EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）"),
            // entity.generalmaterial.eancategory
            new TranslationSeedItem("entity.generalmaterial.eancategory", "ja-JP", "EAN类别_jp", "EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）"),
            // entity.generalmaterial.eancategory
            new TranslationSeedItem("entity.generalmaterial.eancategory", "zh-CN", "EAN类别", "EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）"),
            // entity.generalmaterial.eancategory
            new TranslationSeedItem("entity.generalmaterial.eancategory", "zh-HK", "EAN类别_hk", "EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）"),

            // entity.generalmaterial.length
            new TranslationSeedItem("entity.generalmaterial.length", "en-US", "长度_us", "长度"),
            // entity.generalmaterial.length
            new TranslationSeedItem("entity.generalmaterial.length", "ja-JP", "长度_jp", "长度"),
            // entity.generalmaterial.length
            new TranslationSeedItem("entity.generalmaterial.length", "zh-CN", "长度", "长度"),
            // entity.generalmaterial.length
            new TranslationSeedItem("entity.generalmaterial.length", "zh-HK", "长度_hk", "长度"),

            // entity.generalmaterial.width
            new TranslationSeedItem("entity.generalmaterial.width", "en-US", "宽度_us", "宽度"),
            // entity.generalmaterial.width
            new TranslationSeedItem("entity.generalmaterial.width", "ja-JP", "宽度_jp", "宽度"),
            // entity.generalmaterial.width
            new TranslationSeedItem("entity.generalmaterial.width", "zh-CN", "宽度", "宽度"),
            // entity.generalmaterial.width
            new TranslationSeedItem("entity.generalmaterial.width", "zh-HK", "宽度_hk", "宽度"),

            // entity.generalmaterial.height
            new TranslationSeedItem("entity.generalmaterial.height", "en-US", "高度_us", "高度"),
            // entity.generalmaterial.height
            new TranslationSeedItem("entity.generalmaterial.height", "ja-JP", "高度_jp", "高度"),
            // entity.generalmaterial.height
            new TranslationSeedItem("entity.generalmaterial.height", "zh-CN", "高度", "高度"),
            // entity.generalmaterial.height
            new TranslationSeedItem("entity.generalmaterial.height", "zh-HK", "高度_hk", "高度"),

            // entity.generalmaterial.dimensionunit
            new TranslationSeedItem("entity.generalmaterial.dimensionunit", "en-US", "长宽高单位_us", "长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.dimensionunit
            new TranslationSeedItem("entity.generalmaterial.dimensionunit", "ja-JP", "长宽高单位_jp", "长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.dimensionunit
            new TranslationSeedItem("entity.generalmaterial.dimensionunit", "zh-CN", "长宽高单位", "长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.dimensionunit
            new TranslationSeedItem("entity.generalmaterial.dimensionunit", "zh-HK", "长宽高单位_hk", "长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),

            // entity.generalmaterial.producthierarchy
            new TranslationSeedItem("entity.generalmaterial.producthierarchy", "en-US", "产品层次_us", "产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）"),
            // entity.generalmaterial.producthierarchy
            new TranslationSeedItem("entity.generalmaterial.producthierarchy", "ja-JP", "产品层次_jp", "产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）"),
            // entity.generalmaterial.producthierarchy
            new TranslationSeedItem("entity.generalmaterial.producthierarchy", "zh-CN", "产品层次", "产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）"),
            // entity.generalmaterial.producthierarchy
            new TranslationSeedItem("entity.generalmaterial.producthierarchy", "zh-HK", "产品层次_hk", "产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）"),

            // entity.generalmaterial.stocktransfernetchangecosting
            new TranslationSeedItem("entity.generalmaterial.stocktransfernetchangecosting", "en-US", "库存调拨净更改成本核算_us", "库存调拨净更改成本核算"),
            // entity.generalmaterial.stocktransfernetchangecosting
            new TranslationSeedItem("entity.generalmaterial.stocktransfernetchangecosting", "ja-JP", "库存调拨净更改成本核算_jp", "库存调拨净更改成本核算"),
            // entity.generalmaterial.stocktransfernetchangecosting
            new TranslationSeedItem("entity.generalmaterial.stocktransfernetchangecosting", "zh-CN", "库存调拨净更改成本核算", "库存调拨净更改成本核算"),
            // entity.generalmaterial.stocktransfernetchangecosting
            new TranslationSeedItem("entity.generalmaterial.stocktransfernetchangecosting", "zh-HK", "库存调拨净更改成本核算_hk", "库存调拨净更改成本核算"),

            // entity.generalmaterial.cadindicator
            new TranslationSeedItem("entity.generalmaterial.cadindicator", "en-US", "CAD标识_us", "CAD标识"),
            // entity.generalmaterial.cadindicator
            new TranslationSeedItem("entity.generalmaterial.cadindicator", "ja-JP", "CAD标识_jp", "CAD标识"),
            // entity.generalmaterial.cadindicator
            new TranslationSeedItem("entity.generalmaterial.cadindicator", "zh-CN", "CAD标识", "CAD标识"),
            // entity.generalmaterial.cadindicator
            new TranslationSeedItem("entity.generalmaterial.cadindicator", "zh-HK", "CAD标识_hk", "CAD标识"),

            // entity.generalmaterial.qminprocurement
            new TranslationSeedItem("entity.generalmaterial.qminprocurement", "en-US", "采购QM激活_us", "采购QM激活"),
            // entity.generalmaterial.qminprocurement
            new TranslationSeedItem("entity.generalmaterial.qminprocurement", "ja-JP", "采购QM激活_jp", "采购QM激活"),
            // entity.generalmaterial.qminprocurement
            new TranslationSeedItem("entity.generalmaterial.qminprocurement", "zh-CN", "采购QM激活", "采购QM激活"),
            // entity.generalmaterial.qminprocurement
            new TranslationSeedItem("entity.generalmaterial.qminprocurement", "zh-HK", "采购QM激活_hk", "采购QM激活"),

            // entity.generalmaterial.allowedpackagingweight
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweight", "en-US", "允许包装重量_us", "允许包装重量"),
            // entity.generalmaterial.allowedpackagingweight
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweight", "ja-JP", "允许包装重量_jp", "允许包装重量"),
            // entity.generalmaterial.allowedpackagingweight
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweight", "zh-CN", "允许包装重量", "允许包装重量"),
            // entity.generalmaterial.allowedpackagingweight
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweight", "zh-HK", "允许包装重量_hk", "允许包装重量"),

            // entity.generalmaterial.allowedpackagingweightunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweightunit", "en-US", "允许包装重量单位_us", "允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.allowedpackagingweightunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweightunit", "ja-JP", "允许包装重量单位_jp", "允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.allowedpackagingweightunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweightunit", "zh-CN", "允许包装重量单位", "允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.generalmaterial.allowedpackagingweightunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingweightunit", "zh-HK", "允许包装重量单位_hk", "允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.generalmaterial.allowedpackagingvolume
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolume", "en-US", "允许包装体积_us", "允许包装体积"),
            // entity.generalmaterial.allowedpackagingvolume
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolume", "ja-JP", "允许包装体积_jp", "允许包装体积"),
            // entity.generalmaterial.allowedpackagingvolume
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolume", "zh-CN", "允许包装体积", "允许包装体积"),
            // entity.generalmaterial.allowedpackagingvolume
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolume", "zh-HK", "允许包装体积_hk", "允许包装体积"),

            // entity.generalmaterial.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolumeunit", "en-US", "允许包装体积单位_us", "允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolumeunit", "ja-JP", "允许包装体积单位_jp", "允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolumeunit", "zh-CN", "允许包装体积单位", "允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.generalmaterial.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.generalmaterial.allowedpackagingvolumeunit", "zh-HK", "允许包装体积单位_hk", "允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.generalmaterial.excessweighttolerance
            new TranslationSeedItem("entity.generalmaterial.excessweighttolerance", "en-US", "超重容差_us", "超重容差"),
            // entity.generalmaterial.excessweighttolerance
            new TranslationSeedItem("entity.generalmaterial.excessweighttolerance", "ja-JP", "超重容差_jp", "超重容差"),
            // entity.generalmaterial.excessweighttolerance
            new TranslationSeedItem("entity.generalmaterial.excessweighttolerance", "zh-CN", "超重容差", "超重容差"),
            // entity.generalmaterial.excessweighttolerance
            new TranslationSeedItem("entity.generalmaterial.excessweighttolerance", "zh-HK", "超重容差_hk", "超重容差"),

            // entity.generalmaterial.excessvolumetolerance
            new TranslationSeedItem("entity.generalmaterial.excessvolumetolerance", "en-US", "超体积容差_us", "超体积容差"),
            // entity.generalmaterial.excessvolumetolerance
            new TranslationSeedItem("entity.generalmaterial.excessvolumetolerance", "ja-JP", "超体积容差_jp", "超体积容差"),
            // entity.generalmaterial.excessvolumetolerance
            new TranslationSeedItem("entity.generalmaterial.excessvolumetolerance", "zh-CN", "超体积容差", "超体积容差"),
            // entity.generalmaterial.excessvolumetolerance
            new TranslationSeedItem("entity.generalmaterial.excessvolumetolerance", "zh-HK", "超体积容差_hk", "超体积容差"),

            // entity.generalmaterial.variablepurchaseorderunit
            new TranslationSeedItem("entity.generalmaterial.variablepurchaseorderunit", "en-US", "可变采购订单单位_us", "可变采购订单单位"),
            // entity.generalmaterial.variablepurchaseorderunit
            new TranslationSeedItem("entity.generalmaterial.variablepurchaseorderunit", "ja-JP", "可变采购订单单位_jp", "可变采购订单单位"),
            // entity.generalmaterial.variablepurchaseorderunit
            new TranslationSeedItem("entity.generalmaterial.variablepurchaseorderunit", "zh-CN", "可变采购订单单位", "可变采购订单单位"),
            // entity.generalmaterial.variablepurchaseorderunit
            new TranslationSeedItem("entity.generalmaterial.variablepurchaseorderunit", "zh-HK", "可变采购订单单位_hk", "可变采购订单单位"),

            // entity.generalmaterial.revisionlevelassigned
            new TranslationSeedItem("entity.generalmaterial.revisionlevelassigned", "en-US", "已分配修订级别_us", "已分配修订级别"),
            // entity.generalmaterial.revisionlevelassigned
            new TranslationSeedItem("entity.generalmaterial.revisionlevelassigned", "ja-JP", "已分配修订级别_jp", "已分配修订级别"),
            // entity.generalmaterial.revisionlevelassigned
            new TranslationSeedItem("entity.generalmaterial.revisionlevelassigned", "zh-CN", "已分配修订级别", "已分配修订级别"),
            // entity.generalmaterial.revisionlevelassigned
            new TranslationSeedItem("entity.generalmaterial.revisionlevelassigned", "zh-HK", "已分配修订级别_hk", "已分配修订级别"),

            // entity.generalmaterial.configurablematerial
            new TranslationSeedItem("entity.generalmaterial.configurablematerial", "en-US", "可配置物料_us", "可配置物料"),
            // entity.generalmaterial.configurablematerial
            new TranslationSeedItem("entity.generalmaterial.configurablematerial", "ja-JP", "可配置物料_jp", "可配置物料"),
            // entity.generalmaterial.configurablematerial
            new TranslationSeedItem("entity.generalmaterial.configurablematerial", "zh-CN", "可配置物料", "可配置物料"),
            // entity.generalmaterial.configurablematerial
            new TranslationSeedItem("entity.generalmaterial.configurablematerial", "zh-HK", "可配置物料_hk", "可配置物料"),

            // entity.generalmaterial.batchmanagementrequired
            new TranslationSeedItem("entity.generalmaterial.batchmanagementrequired", "en-US", "批次管理要求_us", "批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）"),
            // entity.generalmaterial.batchmanagementrequired
            new TranslationSeedItem("entity.generalmaterial.batchmanagementrequired", "ja-JP", "批次管理要求_jp", "批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）"),
            // entity.generalmaterial.batchmanagementrequired
            new TranslationSeedItem("entity.generalmaterial.batchmanagementrequired", "zh-CN", "批次管理要求", "批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）"),
            // entity.generalmaterial.batchmanagementrequired
            new TranslationSeedItem("entity.generalmaterial.batchmanagementrequired", "zh-HK", "批次管理要求_hk", "批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）"),

            // entity.generalmaterial.packagingmaterialtype
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialtype", "en-US", "包装物料类型_us", "包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）"),
            // entity.generalmaterial.packagingmaterialtype
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialtype", "ja-JP", "包装物料类型_jp", "包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）"),
            // entity.generalmaterial.packagingmaterialtype
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialtype", "zh-CN", "包装物料类型", "包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）"),
            // entity.generalmaterial.packagingmaterialtype
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialtype", "zh-HK", "包装物料类型_hk", "包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）"),

            // entity.generalmaterial.maximumlevelbyvolume
            new TranslationSeedItem("entity.generalmaterial.maximumlevelbyvolume", "en-US", "最大装载量（体积）_us", "最大装载量（体积）"),
            // entity.generalmaterial.maximumlevelbyvolume
            new TranslationSeedItem("entity.generalmaterial.maximumlevelbyvolume", "ja-JP", "最大装载量（体积）_jp", "最大装载量（体积）"),
            // entity.generalmaterial.maximumlevelbyvolume
            new TranslationSeedItem("entity.generalmaterial.maximumlevelbyvolume", "zh-CN", "最大装载量（体积）", "最大装载量（体积）"),
            // entity.generalmaterial.maximumlevelbyvolume
            new TranslationSeedItem("entity.generalmaterial.maximumlevelbyvolume", "zh-HK", "最大装载量（体积）_hk", "最大装载量（体积）"),

            // entity.generalmaterial.stackingfactor
            new TranslationSeedItem("entity.generalmaterial.stackingfactor", "en-US", "堆叠因子_us", "堆叠因子"),
            // entity.generalmaterial.stackingfactor
            new TranslationSeedItem("entity.generalmaterial.stackingfactor", "ja-JP", "堆叠因子_jp", "堆叠因子"),
            // entity.generalmaterial.stackingfactor
            new TranslationSeedItem("entity.generalmaterial.stackingfactor", "zh-CN", "堆叠因子", "堆叠因子"),
            // entity.generalmaterial.stackingfactor
            new TranslationSeedItem("entity.generalmaterial.stackingfactor", "zh-HK", "堆叠因子_hk", "堆叠因子"),

            // entity.generalmaterial.packagingmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialgroup", "en-US", "包装物料组_us", "包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）"),
            // entity.generalmaterial.packagingmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialgroup", "ja-JP", "包装物料组_jp", "包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）"),
            // entity.generalmaterial.packagingmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialgroup", "zh-CN", "包装物料组", "包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）"),
            // entity.generalmaterial.packagingmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.packagingmaterialgroup", "zh-HK", "包装物料组_hk", "包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）"),

            // entity.generalmaterial.authorizationgroup
            new TranslationSeedItem("entity.generalmaterial.authorizationgroup", "en-US", "权限组_us", "权限组（字典 logistics_authorization_group；DictValue=权限组编码）"),
            // entity.generalmaterial.authorizationgroup
            new TranslationSeedItem("entity.generalmaterial.authorizationgroup", "ja-JP", "权限组_jp", "权限组（字典 logistics_authorization_group；DictValue=权限组编码）"),
            // entity.generalmaterial.authorizationgroup
            new TranslationSeedItem("entity.generalmaterial.authorizationgroup", "zh-CN", "权限组", "权限组（字典 logistics_authorization_group；DictValue=权限组编码）"),
            // entity.generalmaterial.authorizationgroup
            new TranslationSeedItem("entity.generalmaterial.authorizationgroup", "zh-HK", "权限组_hk", "权限组（字典 logistics_authorization_group；DictValue=权限组编码）"),

            // entity.generalmaterial.validfromdate
            new TranslationSeedItem("entity.generalmaterial.validfromdate", "en-US", "有效起始日期_us", "有效起始日期"),
            // entity.generalmaterial.validfromdate
            new TranslationSeedItem("entity.generalmaterial.validfromdate", "ja-JP", "有效起始日期_jp", "有效起始日期"),
            // entity.generalmaterial.validfromdate
            new TranslationSeedItem("entity.generalmaterial.validfromdate", "zh-CN", "有效起始日期", "有效起始日期"),
            // entity.generalmaterial.validfromdate
            new TranslationSeedItem("entity.generalmaterial.validfromdate", "zh-HK", "有效起始日期_hk", "有效起始日期"),

            // entity.generalmaterial.validtodate
            new TranslationSeedItem("entity.generalmaterial.validtodate", "en-US", "有效至/删除日期_us", "有效至/删除日期"),
            // entity.generalmaterial.validtodate
            new TranslationSeedItem("entity.generalmaterial.validtodate", "ja-JP", "有效至/删除日期_jp", "有效至/删除日期"),
            // entity.generalmaterial.validtodate
            new TranslationSeedItem("entity.generalmaterial.validtodate", "zh-CN", "有效至/删除日期", "有效至/删除日期"),
            // entity.generalmaterial.validtodate
            new TranslationSeedItem("entity.generalmaterial.validtodate", "zh-HK", "有效至/删除日期_hk", "有效至/删除日期"),

            // entity.generalmaterial.seasonyear
            new TranslationSeedItem("entity.generalmaterial.seasonyear", "en-US", "季节年份_us", "季节年份（字典 logistics_season_year；DictValue=季节年份）"),
            // entity.generalmaterial.seasonyear
            new TranslationSeedItem("entity.generalmaterial.seasonyear", "ja-JP", "季节年份_jp", "季节年份（字典 logistics_season_year；DictValue=季节年份）"),
            // entity.generalmaterial.seasonyear
            new TranslationSeedItem("entity.generalmaterial.seasonyear", "zh-CN", "季节年份", "季节年份（字典 logistics_season_year；DictValue=季节年份）"),
            // entity.generalmaterial.seasonyear
            new TranslationSeedItem("entity.generalmaterial.seasonyear", "zh-HK", "季节年份_hk", "季节年份（字典 logistics_season_year；DictValue=季节年份）"),

            // entity.generalmaterial.pricebandcategory
            new TranslationSeedItem("entity.generalmaterial.pricebandcategory", "en-US", "价格带类别_us", "价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）"),
            // entity.generalmaterial.pricebandcategory
            new TranslationSeedItem("entity.generalmaterial.pricebandcategory", "ja-JP", "价格带类别_jp", "价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）"),
            // entity.generalmaterial.pricebandcategory
            new TranslationSeedItem("entity.generalmaterial.pricebandcategory", "zh-CN", "价格带类别", "价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）"),
            // entity.generalmaterial.pricebandcategory
            new TranslationSeedItem("entity.generalmaterial.pricebandcategory", "zh-HK", "价格带类别_hk", "价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）"),

            // entity.generalmaterial.emptiesbillofmaterial
            new TranslationSeedItem("entity.generalmaterial.emptiesbillofmaterial", "en-US", "空容器BOM_us", "空容器BOM"),
            // entity.generalmaterial.emptiesbillofmaterial
            new TranslationSeedItem("entity.generalmaterial.emptiesbillofmaterial", "ja-JP", "空容器BOM_jp", "空容器BOM"),
            // entity.generalmaterial.emptiesbillofmaterial
            new TranslationSeedItem("entity.generalmaterial.emptiesbillofmaterial", "zh-CN", "空容器BOM", "空容器BOM"),
            // entity.generalmaterial.emptiesbillofmaterial
            new TranslationSeedItem("entity.generalmaterial.emptiesbillofmaterial", "zh-HK", "空容器BOM_hk", "空容器BOM"),

            // entity.generalmaterial.externalmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.externalmaterialgroup", "en-US", "外部物料组_us", "外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）"),
            // entity.generalmaterial.externalmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.externalmaterialgroup", "ja-JP", "外部物料组_jp", "外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）"),
            // entity.generalmaterial.externalmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.externalmaterialgroup", "zh-CN", "外部物料组", "外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）"),
            // entity.generalmaterial.externalmaterialgroup
            new TranslationSeedItem("entity.generalmaterial.externalmaterialgroup", "zh-HK", "外部物料组_hk", "外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）"),

            // entity.generalmaterial.crossplantconfigurablematerial
            new TranslationSeedItem("entity.generalmaterial.crossplantconfigurablematerial", "en-US", "跨工厂可配置物料_us", "跨工厂可配置物料"),
            // entity.generalmaterial.crossplantconfigurablematerial
            new TranslationSeedItem("entity.generalmaterial.crossplantconfigurablematerial", "ja-JP", "跨工厂可配置物料_jp", "跨工厂可配置物料"),
            // entity.generalmaterial.crossplantconfigurablematerial
            new TranslationSeedItem("entity.generalmaterial.crossplantconfigurablematerial", "zh-CN", "跨工厂可配置物料", "跨工厂可配置物料"),
            // entity.generalmaterial.crossplantconfigurablematerial
            new TranslationSeedItem("entity.generalmaterial.crossplantconfigurablematerial", "zh-HK", "跨工厂可配置物料_hk", "跨工厂可配置物料"),

            // entity.generalmaterial.materialcategory
            new TranslationSeedItem("entity.generalmaterial.materialcategory", "en-US", "物料类别_us", "物料类别（字典 logistics_material_category；DictValue=物料类别编码）"),
            // entity.generalmaterial.materialcategory
            new TranslationSeedItem("entity.generalmaterial.materialcategory", "ja-JP", "物料类别_jp", "物料类别（字典 logistics_material_category；DictValue=物料类别编码）"),
            // entity.generalmaterial.materialcategory
            new TranslationSeedItem("entity.generalmaterial.materialcategory", "zh-CN", "物料类别", "物料类别（字典 logistics_material_category；DictValue=物料类别编码）"),
            // entity.generalmaterial.materialcategory
            new TranslationSeedItem("entity.generalmaterial.materialcategory", "zh-HK", "物料类别_hk", "物料类别（字典 logistics_material_category；DictValue=物料类别编码）"),

            // entity.generalmaterial.coproductindicator
            new TranslationSeedItem("entity.generalmaterial.coproductindicator", "en-US", "联产品标识_us", "联产品标识"),
            // entity.generalmaterial.coproductindicator
            new TranslationSeedItem("entity.generalmaterial.coproductindicator", "ja-JP", "联产品标识_jp", "联产品标识"),
            // entity.generalmaterial.coproductindicator
            new TranslationSeedItem("entity.generalmaterial.coproductindicator", "zh-CN", "联产品标识", "联产品标识"),
            // entity.generalmaterial.coproductindicator
            new TranslationSeedItem("entity.generalmaterial.coproductindicator", "zh-HK", "联产品标识_hk", "联产品标识"),

            // entity.generalmaterial.followupmaterialindicator
            new TranslationSeedItem("entity.generalmaterial.followupmaterialindicator", "en-US", "后续物料标识_us", "后续物料标识"),
            // entity.generalmaterial.followupmaterialindicator
            new TranslationSeedItem("entity.generalmaterial.followupmaterialindicator", "ja-JP", "后续物料标识_jp", "后续物料标识"),
            // entity.generalmaterial.followupmaterialindicator
            new TranslationSeedItem("entity.generalmaterial.followupmaterialindicator", "zh-CN", "后续物料标识", "后续物料标识"),
            // entity.generalmaterial.followupmaterialindicator
            new TranslationSeedItem("entity.generalmaterial.followupmaterialindicator", "zh-HK", "后续物料标识_hk", "后续物料标识"),

            // entity.generalmaterial.pricingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.pricingreferencematerial", "en-US", "定价参考物料_us", "定价参考物料"),
            // entity.generalmaterial.pricingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.pricingreferencematerial", "ja-JP", "定价参考物料_jp", "定价参考物料"),
            // entity.generalmaterial.pricingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.pricingreferencematerial", "zh-CN", "定价参考物料", "定价参考物料"),
            // entity.generalmaterial.pricingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.pricingreferencematerial", "zh-HK", "定价参考物料_hk", "定价参考物料"),

            // entity.generalmaterial.crossplantmaterialstatus
            new TranslationSeedItem("entity.generalmaterial.crossplantmaterialstatus", "en-US", "跨工厂物料状态_us", "跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossplantmaterialstatus
            new TranslationSeedItem("entity.generalmaterial.crossplantmaterialstatus", "ja-JP", "跨工厂物料状态_jp", "跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossplantmaterialstatus
            new TranslationSeedItem("entity.generalmaterial.crossplantmaterialstatus", "zh-CN", "跨工厂物料状态", "跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossplantmaterialstatus
            new TranslationSeedItem("entity.generalmaterial.crossplantmaterialstatus", "zh-HK", "跨工厂物料状态_hk", "跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）"),

            // entity.generalmaterial.crossdistributionchainstatus
            new TranslationSeedItem("entity.generalmaterial.crossdistributionchainstatus", "en-US", "跨分销链物料状态_us", "跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossdistributionchainstatus
            new TranslationSeedItem("entity.generalmaterial.crossdistributionchainstatus", "ja-JP", "跨分销链物料状态_jp", "跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossdistributionchainstatus
            new TranslationSeedItem("entity.generalmaterial.crossdistributionchainstatus", "zh-CN", "跨分销链物料状态", "跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）"),
            // entity.generalmaterial.crossdistributionchainstatus
            new TranslationSeedItem("entity.generalmaterial.crossdistributionchainstatus", "zh-HK", "跨分销链物料状态_hk", "跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）"),

            // entity.generalmaterial.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossplantstatusvalidfrom", "en-US", "跨工厂状态生效日期_us", "跨工厂状态生效日期"),
            // entity.generalmaterial.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossplantstatusvalidfrom", "ja-JP", "跨工厂状态生效日期_jp", "跨工厂状态生效日期"),
            // entity.generalmaterial.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossplantstatusvalidfrom", "zh-CN", "跨工厂状态生效日期", "跨工厂状态生效日期"),
            // entity.generalmaterial.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossplantstatusvalidfrom", "zh-HK", "跨工厂状态生效日期_hk", "跨工厂状态生效日期"),

            // entity.generalmaterial.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossdistributionstatusvalidfrom", "en-US", "跨分销链状态生效日期_us", "跨分销链状态生效日期"),
            // entity.generalmaterial.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossdistributionstatusvalidfrom", "ja-JP", "跨分销链状态生效日期_jp", "跨分销链状态生效日期"),
            // entity.generalmaterial.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossdistributionstatusvalidfrom", "zh-CN", "跨分销链状态生效日期", "跨分销链状态生效日期"),
            // entity.generalmaterial.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.generalmaterial.crossdistributionstatusvalidfrom", "zh-HK", "跨分销链状态生效日期_hk", "跨分销链状态生效日期"),

            // entity.generalmaterial.taxclassification
            new TranslationSeedItem("entity.generalmaterial.taxclassification", "en-US", "物料税分类_us", "物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）"),
            // entity.generalmaterial.taxclassification
            new TranslationSeedItem("entity.generalmaterial.taxclassification", "ja-JP", "物料税分类_jp", "物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）"),
            // entity.generalmaterial.taxclassification
            new TranslationSeedItem("entity.generalmaterial.taxclassification", "zh-CN", "物料税分类", "物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）"),
            // entity.generalmaterial.taxclassification
            new TranslationSeedItem("entity.generalmaterial.taxclassification", "zh-HK", "物料税分类_hk", "物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）"),

            // entity.generalmaterial.catalogprofile
            new TranslationSeedItem("entity.generalmaterial.catalogprofile", "en-US", "目录参数文件_us", "目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catalogprofile
            new TranslationSeedItem("entity.generalmaterial.catalogprofile", "ja-JP", "目录参数文件_jp", "目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catalogprofile
            new TranslationSeedItem("entity.generalmaterial.catalogprofile", "zh-CN", "目录参数文件", "目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catalogprofile
            new TranslationSeedItem("entity.generalmaterial.catalogprofile", "zh-HK", "目录参数文件_hk", "目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.minimumremainingshelflife
            new TranslationSeedItem("entity.generalmaterial.minimumremainingshelflife", "en-US", "最短剩余货架寿命_us", "最短剩余货架寿命"),
            // entity.generalmaterial.minimumremainingshelflife
            new TranslationSeedItem("entity.generalmaterial.minimumremainingshelflife", "ja-JP", "最短剩余货架寿命_jp", "最短剩余货架寿命"),
            // entity.generalmaterial.minimumremainingshelflife
            new TranslationSeedItem("entity.generalmaterial.minimumremainingshelflife", "zh-CN", "最短剩余货架寿命", "最短剩余货架寿命"),
            // entity.generalmaterial.minimumremainingshelflife
            new TranslationSeedItem("entity.generalmaterial.minimumremainingshelflife", "zh-HK", "最短剩余货架寿命_hk", "最短剩余货架寿命"),

            // entity.generalmaterial.totalshelflife
            new TranslationSeedItem("entity.generalmaterial.totalshelflife", "en-US", "总货架寿命_us", "总货架寿命"),
            // entity.generalmaterial.totalshelflife
            new TranslationSeedItem("entity.generalmaterial.totalshelflife", "ja-JP", "总货架寿命_jp", "总货架寿命"),
            // entity.generalmaterial.totalshelflife
            new TranslationSeedItem("entity.generalmaterial.totalshelflife", "zh-CN", "总货架寿命", "总货架寿命"),
            // entity.generalmaterial.totalshelflife
            new TranslationSeedItem("entity.generalmaterial.totalshelflife", "zh-HK", "总货架寿命_hk", "总货架寿命"),

            // entity.generalmaterial.storagepercentage
            new TranslationSeedItem("entity.generalmaterial.storagepercentage", "en-US", "仓储百分比_us", "仓储百分比"),
            // entity.generalmaterial.storagepercentage
            new TranslationSeedItem("entity.generalmaterial.storagepercentage", "ja-JP", "仓储百分比_jp", "仓储百分比"),
            // entity.generalmaterial.storagepercentage
            new TranslationSeedItem("entity.generalmaterial.storagepercentage", "zh-CN", "仓储百分比", "仓储百分比"),
            // entity.generalmaterial.storagepercentage
            new TranslationSeedItem("entity.generalmaterial.storagepercentage", "zh-HK", "仓储百分比_hk", "仓储百分比"),

            // entity.generalmaterial.contentunit
            new TranslationSeedItem("entity.generalmaterial.contentunit", "en-US", "含量单位_us", "含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）"),
            // entity.generalmaterial.contentunit
            new TranslationSeedItem("entity.generalmaterial.contentunit", "ja-JP", "含量单位_jp", "含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）"),
            // entity.generalmaterial.contentunit
            new TranslationSeedItem("entity.generalmaterial.contentunit", "zh-CN", "含量单位", "含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）"),
            // entity.generalmaterial.contentunit
            new TranslationSeedItem("entity.generalmaterial.contentunit", "zh-HK", "含量单位_hk", "含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）"),

            // entity.generalmaterial.netcontents
            new TranslationSeedItem("entity.generalmaterial.netcontents", "en-US", "净含量_us", "净含量"),
            // entity.generalmaterial.netcontents
            new TranslationSeedItem("entity.generalmaterial.netcontents", "ja-JP", "净含量_jp", "净含量"),
            // entity.generalmaterial.netcontents
            new TranslationSeedItem("entity.generalmaterial.netcontents", "zh-CN", "净含量", "净含量"),
            // entity.generalmaterial.netcontents
            new TranslationSeedItem("entity.generalmaterial.netcontents", "zh-HK", "净含量_hk", "净含量"),

            // entity.generalmaterial.comparisonpriceunit
            new TranslationSeedItem("entity.generalmaterial.comparisonpriceunit", "en-US", "比较价格单位_us", "比较价格单位"),
            // entity.generalmaterial.comparisonpriceunit
            new TranslationSeedItem("entity.generalmaterial.comparisonpriceunit", "ja-JP", "比较价格单位_jp", "比较价格单位"),
            // entity.generalmaterial.comparisonpriceunit
            new TranslationSeedItem("entity.generalmaterial.comparisonpriceunit", "zh-CN", "比较价格单位", "比较价格单位"),
            // entity.generalmaterial.comparisonpriceunit
            new TranslationSeedItem("entity.generalmaterial.comparisonpriceunit", "zh-HK", "比较价格单位_hk", "比较价格单位"),

            // entity.generalmaterial.labelingmaterialgrouping
            new TranslationSeedItem("entity.generalmaterial.labelingmaterialgrouping", "en-US", "标签物料分组_us", "标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）"),
            // entity.generalmaterial.labelingmaterialgrouping
            new TranslationSeedItem("entity.generalmaterial.labelingmaterialgrouping", "ja-JP", "标签物料分组_jp", "标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）"),
            // entity.generalmaterial.labelingmaterialgrouping
            new TranslationSeedItem("entity.generalmaterial.labelingmaterialgrouping", "zh-CN", "标签物料分组", "标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）"),
            // entity.generalmaterial.labelingmaterialgrouping
            new TranslationSeedItem("entity.generalmaterial.labelingmaterialgrouping", "zh-HK", "标签物料分组_hk", "标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）"),

            // entity.generalmaterial.grosscontents
            new TranslationSeedItem("entity.generalmaterial.grosscontents", "en-US", "毛含量_us", "毛含量"),
            // entity.generalmaterial.grosscontents
            new TranslationSeedItem("entity.generalmaterial.grosscontents", "ja-JP", "毛含量_jp", "毛含量"),
            // entity.generalmaterial.grosscontents
            new TranslationSeedItem("entity.generalmaterial.grosscontents", "zh-CN", "毛含量", "毛含量"),
            // entity.generalmaterial.grosscontents
            new TranslationSeedItem("entity.generalmaterial.grosscontents", "zh-HK", "毛含量_hk", "毛含量"),

            // entity.generalmaterial.quantityconversionmethod
            new TranslationSeedItem("entity.generalmaterial.quantityconversionmethod", "en-US", "数量换算方法_us", "数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）"),
            // entity.generalmaterial.quantityconversionmethod
            new TranslationSeedItem("entity.generalmaterial.quantityconversionmethod", "ja-JP", "数量换算方法_jp", "数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）"),
            // entity.generalmaterial.quantityconversionmethod
            new TranslationSeedItem("entity.generalmaterial.quantityconversionmethod", "zh-CN", "数量换算方法", "数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）"),
            // entity.generalmaterial.quantityconversionmethod
            new TranslationSeedItem("entity.generalmaterial.quantityconversionmethod", "zh-HK", "数量换算方法_hk", "数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）"),

            // entity.generalmaterial.internalobjectnumber
            new TranslationSeedItem("entity.generalmaterial.internalobjectnumber", "en-US", "内部对象号_us", "内部对象号"),
            // entity.generalmaterial.internalobjectnumber
            new TranslationSeedItem("entity.generalmaterial.internalobjectnumber", "ja-JP", "内部对象号_jp", "内部对象号"),
            // entity.generalmaterial.internalobjectnumber
            new TranslationSeedItem("entity.generalmaterial.internalobjectnumber", "zh-CN", "内部对象号", "内部对象号"),
            // entity.generalmaterial.internalobjectnumber
            new TranslationSeedItem("entity.generalmaterial.internalobjectnumber", "zh-HK", "内部对象号_hk", "内部对象号"),

            // entity.generalmaterial.environmentallyrelevant
            new TranslationSeedItem("entity.generalmaterial.environmentallyrelevant", "en-US", "环境相关_us", "环境相关"),
            // entity.generalmaterial.environmentallyrelevant
            new TranslationSeedItem("entity.generalmaterial.environmentallyrelevant", "ja-JP", "环境相关_jp", "环境相关"),
            // entity.generalmaterial.environmentallyrelevant
            new TranslationSeedItem("entity.generalmaterial.environmentallyrelevant", "zh-CN", "环境相关", "环境相关"),
            // entity.generalmaterial.environmentallyrelevant
            new TranslationSeedItem("entity.generalmaterial.environmentallyrelevant", "zh-HK", "环境相关_hk", "环境相关"),

            // entity.generalmaterial.productallocationprocedure
            new TranslationSeedItem("entity.generalmaterial.productallocationprocedure", "en-US", "产品分配确定过程_us", "产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）"),
            // entity.generalmaterial.productallocationprocedure
            new TranslationSeedItem("entity.generalmaterial.productallocationprocedure", "ja-JP", "产品分配确定过程_jp", "产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）"),
            // entity.generalmaterial.productallocationprocedure
            new TranslationSeedItem("entity.generalmaterial.productallocationprocedure", "zh-CN", "产品分配确定过程", "产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）"),
            // entity.generalmaterial.productallocationprocedure
            new TranslationSeedItem("entity.generalmaterial.productallocationprocedure", "zh-HK", "产品分配确定过程_hk", "产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）"),

            // entity.generalmaterial.variantpricingprofile
            new TranslationSeedItem("entity.generalmaterial.variantpricingprofile", "en-US", "变式定价参数文件_us", "变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.variantpricingprofile
            new TranslationSeedItem("entity.generalmaterial.variantpricingprofile", "ja-JP", "变式定价参数文件_jp", "变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.variantpricingprofile
            new TranslationSeedItem("entity.generalmaterial.variantpricingprofile", "zh-CN", "变式定价参数文件", "变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.variantpricingprofile
            new TranslationSeedItem("entity.generalmaterial.variantpricingprofile", "zh-HK", "变式定价参数文件_hk", "变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.discountinkind
            new TranslationSeedItem("entity.generalmaterial.discountinkind", "en-US", "实物折扣资格_us", "实物折扣资格"),
            // entity.generalmaterial.discountinkind
            new TranslationSeedItem("entity.generalmaterial.discountinkind", "ja-JP", "实物折扣资格_jp", "实物折扣资格"),
            // entity.generalmaterial.discountinkind
            new TranslationSeedItem("entity.generalmaterial.discountinkind", "zh-CN", "实物折扣资格", "实物折扣资格"),
            // entity.generalmaterial.discountinkind
            new TranslationSeedItem("entity.generalmaterial.discountinkind", "zh-HK", "实物折扣资格_hk", "实物折扣资格"),

            // entity.generalmaterial.manufacturerpartnumber
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartnumber", "en-US", "制造商零件号_us", "制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.generalmaterial.manufacturerpartnumber
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartnumber", "ja-JP", "制造商零件号_jp", "制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.generalmaterial.manufacturerpartnumber
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartnumber", "zh-CN", "制造商零件号", "制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.generalmaterial.manufacturerpartnumber
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartnumber", "zh-HK", "制造商零件号_hk", "制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),

            // entity.generalmaterial.manufacturernumber
            new TranslationSeedItem("entity.generalmaterial.manufacturernumber", "en-US", "制造商编码_us", "制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.generalmaterial.manufacturernumber
            new TranslationSeedItem("entity.generalmaterial.manufacturernumber", "ja-JP", "制造商编码_jp", "制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.generalmaterial.manufacturernumber
            new TranslationSeedItem("entity.generalmaterial.manufacturernumber", "zh-CN", "制造商编码", "制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.generalmaterial.manufacturernumber
            new TranslationSeedItem("entity.generalmaterial.manufacturernumber", "zh-HK", "制造商编码_hk", "制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.generalmaterial.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.inventorymanagedmaterialnumber", "en-US", "自有库存管理物料号_us", "自有库存管理物料号"),
            // entity.generalmaterial.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.inventorymanagedmaterialnumber", "ja-JP", "自有库存管理物料号_jp", "自有库存管理物料号"),
            // entity.generalmaterial.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.inventorymanagedmaterialnumber", "zh-CN", "自有库存管理物料号", "自有库存管理物料号"),
            // entity.generalmaterial.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.inventorymanagedmaterialnumber", "zh-HK", "自有库存管理物料号_hk", "自有库存管理物料号"),

            // entity.generalmaterial.manufacturerpartprofile
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartprofile", "en-US", "制造商零件参数文件_us", "制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.manufacturerpartprofile
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartprofile", "ja-JP", "制造商零件参数文件_jp", "制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.manufacturerpartprofile
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartprofile", "zh-CN", "制造商零件参数文件", "制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.manufacturerpartprofile
            new TranslationSeedItem("entity.generalmaterial.manufacturerpartprofile", "zh-HK", "制造商零件参数文件_hk", "制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.unitsofmeasureusage
            new TranslationSeedItem("entity.generalmaterial.unitsofmeasureusage", "en-US", "计量单位用途_us", "计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）"),
            // entity.generalmaterial.unitsofmeasureusage
            new TranslationSeedItem("entity.generalmaterial.unitsofmeasureusage", "ja-JP", "计量单位用途_jp", "计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）"),
            // entity.generalmaterial.unitsofmeasureusage
            new TranslationSeedItem("entity.generalmaterial.unitsofmeasureusage", "zh-CN", "计量单位用途", "计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）"),
            // entity.generalmaterial.unitsofmeasureusage
            new TranslationSeedItem("entity.generalmaterial.unitsofmeasureusage", "zh-HK", "计量单位用途_hk", "计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）"),

            // entity.generalmaterial.seasonrollout
            new TranslationSeedItem("entity.generalmaterial.seasonrollout", "en-US", "季节推出_us", "季节推出（字典 logistics_season_rollout；DictValue=推出编码）"),
            // entity.generalmaterial.seasonrollout
            new TranslationSeedItem("entity.generalmaterial.seasonrollout", "ja-JP", "季节推出_jp", "季节推出（字典 logistics_season_rollout；DictValue=推出编码）"),
            // entity.generalmaterial.seasonrollout
            new TranslationSeedItem("entity.generalmaterial.seasonrollout", "zh-CN", "季节推出", "季节推出（字典 logistics_season_rollout；DictValue=推出编码）"),
            // entity.generalmaterial.seasonrollout
            new TranslationSeedItem("entity.generalmaterial.seasonrollout", "zh-HK", "季节推出_hk", "季节推出（字典 logistics_season_rollout；DictValue=推出编码）"),

            // entity.generalmaterial.dangerousgoodsprofile
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodsprofile", "en-US", "危险品参数文件_us", "危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.dangerousgoodsprofile
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodsprofile", "ja-JP", "危险品参数文件_jp", "危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.dangerousgoodsprofile
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodsprofile", "zh-CN", "危险品参数文件", "危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.dangerousgoodsprofile
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodsprofile", "zh-HK", "危险品参数文件_hk", "危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.highlyviscous
            new TranslationSeedItem("entity.generalmaterial.highlyviscous", "en-US", "高粘度_us", "高粘度"),
            // entity.generalmaterial.highlyviscous
            new TranslationSeedItem("entity.generalmaterial.highlyviscous", "ja-JP", "高粘度_jp", "高粘度"),
            // entity.generalmaterial.highlyviscous
            new TranslationSeedItem("entity.generalmaterial.highlyviscous", "zh-CN", "高粘度", "高粘度"),
            // entity.generalmaterial.highlyviscous
            new TranslationSeedItem("entity.generalmaterial.highlyviscous", "zh-HK", "高粘度_hk", "高粘度"),

            // entity.generalmaterial.inbulkliquid
            new TranslationSeedItem("entity.generalmaterial.inbulkliquid", "en-US", "散装/液体_us", "散装/液体"),
            // entity.generalmaterial.inbulkliquid
            new TranslationSeedItem("entity.generalmaterial.inbulkliquid", "ja-JP", "散装/液体_jp", "散装/液体"),
            // entity.generalmaterial.inbulkliquid
            new TranslationSeedItem("entity.generalmaterial.inbulkliquid", "zh-CN", "散装/液体", "散装/液体"),
            // entity.generalmaterial.inbulkliquid
            new TranslationSeedItem("entity.generalmaterial.inbulkliquid", "zh-HK", "散装/液体_hk", "散装/液体"),

            // entity.generalmaterial.serialnumberexplicitness
            new TranslationSeedItem("entity.generalmaterial.serialnumberexplicitness", "en-US", "序列号明确级别_us", "序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）"),
            // entity.generalmaterial.serialnumberexplicitness
            new TranslationSeedItem("entity.generalmaterial.serialnumberexplicitness", "ja-JP", "序列号明确级别_jp", "序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）"),
            // entity.generalmaterial.serialnumberexplicitness
            new TranslationSeedItem("entity.generalmaterial.serialnumberexplicitness", "zh-CN", "序列号明确级别", "序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）"),
            // entity.generalmaterial.serialnumberexplicitness
            new TranslationSeedItem("entity.generalmaterial.serialnumberexplicitness", "zh-HK", "序列号明确级别_hk", "序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）"),

            // entity.generalmaterial.closedpackaging
            new TranslationSeedItem("entity.generalmaterial.closedpackaging", "en-US", "封闭包装_us", "封闭包装"),
            // entity.generalmaterial.closedpackaging
            new TranslationSeedItem("entity.generalmaterial.closedpackaging", "ja-JP", "封闭包装_jp", "封闭包装"),
            // entity.generalmaterial.closedpackaging
            new TranslationSeedItem("entity.generalmaterial.closedpackaging", "zh-CN", "封闭包装", "封闭包装"),
            // entity.generalmaterial.closedpackaging
            new TranslationSeedItem("entity.generalmaterial.closedpackaging", "zh-HK", "封闭包装_hk", "封闭包装"),

            // entity.generalmaterial.approvedbatchrecordrequired
            new TranslationSeedItem("entity.generalmaterial.approvedbatchrecordrequired", "en-US", "需批准批次记录_us", "需批准批次记录"),
            // entity.generalmaterial.approvedbatchrecordrequired
            new TranslationSeedItem("entity.generalmaterial.approvedbatchrecordrequired", "ja-JP", "需批准批次记录_jp", "需批准批次记录"),
            // entity.generalmaterial.approvedbatchrecordrequired
            new TranslationSeedItem("entity.generalmaterial.approvedbatchrecordrequired", "zh-CN", "需批准批次记录", "需批准批次记录"),
            // entity.generalmaterial.approvedbatchrecordrequired
            new TranslationSeedItem("entity.generalmaterial.approvedbatchrecordrequired", "zh-HK", "需批准批次记录_hk", "需批准批次记录"),

            // entity.generalmaterial.effectivityparameteroverride
            new TranslationSeedItem("entity.generalmaterial.effectivityparameteroverride", "en-US", "有效性参数覆盖_us", "有效性参数覆盖"),
            // entity.generalmaterial.effectivityparameteroverride
            new TranslationSeedItem("entity.generalmaterial.effectivityparameteroverride", "ja-JP", "有效性参数覆盖_jp", "有效性参数覆盖"),
            // entity.generalmaterial.effectivityparameteroverride
            new TranslationSeedItem("entity.generalmaterial.effectivityparameteroverride", "zh-CN", "有效性参数覆盖", "有效性参数覆盖"),
            // entity.generalmaterial.effectivityparameteroverride
            new TranslationSeedItem("entity.generalmaterial.effectivityparameteroverride", "zh-HK", "有效性参数覆盖_hk", "有效性参数覆盖"),

            // entity.generalmaterial.materialcompletionlevel
            new TranslationSeedItem("entity.generalmaterial.materialcompletionlevel", "en-US", "物料完成级别_us", "物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）"),
            // entity.generalmaterial.materialcompletionlevel
            new TranslationSeedItem("entity.generalmaterial.materialcompletionlevel", "ja-JP", "物料完成级别_jp", "物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）"),
            // entity.generalmaterial.materialcompletionlevel
            new TranslationSeedItem("entity.generalmaterial.materialcompletionlevel", "zh-CN", "物料完成级别", "物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）"),
            // entity.generalmaterial.materialcompletionlevel
            new TranslationSeedItem("entity.generalmaterial.materialcompletionlevel", "zh-HK", "物料完成级别_hk", "物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）"),

            // entity.generalmaterial.shelflifeperiodindicator
            new TranslationSeedItem("entity.generalmaterial.shelflifeperiodindicator", "en-US", "货架寿命期间标识_us", "货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）"),
            // entity.generalmaterial.shelflifeperiodindicator
            new TranslationSeedItem("entity.generalmaterial.shelflifeperiodindicator", "ja-JP", "货架寿命期间标识_jp", "货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）"),
            // entity.generalmaterial.shelflifeperiodindicator
            new TranslationSeedItem("entity.generalmaterial.shelflifeperiodindicator", "zh-CN", "货架寿命期间标识", "货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）"),
            // entity.generalmaterial.shelflifeperiodindicator
            new TranslationSeedItem("entity.generalmaterial.shelflifeperiodindicator", "zh-HK", "货架寿命期间标识_hk", "货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）"),

            // entity.generalmaterial.shelfliferoundingrule
            new TranslationSeedItem("entity.generalmaterial.shelfliferoundingrule", "en-US", "货架寿命舍入规则_us", "货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）"),
            // entity.generalmaterial.shelfliferoundingrule
            new TranslationSeedItem("entity.generalmaterial.shelfliferoundingrule", "ja-JP", "货架寿命舍入规则_jp", "货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）"),
            // entity.generalmaterial.shelfliferoundingrule
            new TranslationSeedItem("entity.generalmaterial.shelfliferoundingrule", "zh-CN", "货架寿命舍入规则", "货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）"),
            // entity.generalmaterial.shelfliferoundingrule
            new TranslationSeedItem("entity.generalmaterial.shelfliferoundingrule", "zh-HK", "货架寿命舍入规则_hk", "货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）"),

            // entity.generalmaterial.productcompositiononpackaging
            new TranslationSeedItem("entity.generalmaterial.productcompositiononpackaging", "en-US", "包装打印产品成分_us", "包装打印产品成分"),
            // entity.generalmaterial.productcompositiononpackaging
            new TranslationSeedItem("entity.generalmaterial.productcompositiononpackaging", "ja-JP", "包装打印产品成分_jp", "包装打印产品成分"),
            // entity.generalmaterial.productcompositiononpackaging
            new TranslationSeedItem("entity.generalmaterial.productcompositiononpackaging", "zh-CN", "包装打印产品成分", "包装打印产品成分"),
            // entity.generalmaterial.productcompositiononpackaging
            new TranslationSeedItem("entity.generalmaterial.productcompositiononpackaging", "zh-HK", "包装打印产品成分_hk", "包装打印产品成分"),

            // entity.generalmaterial.generalitemcategorygroup
            new TranslationSeedItem("entity.generalmaterial.generalitemcategorygroup", "en-US", "通用项目类别组_us", "通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）"),
            // entity.generalmaterial.generalitemcategorygroup
            new TranslationSeedItem("entity.generalmaterial.generalitemcategorygroup", "ja-JP", "通用项目类别组_jp", "通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）"),
            // entity.generalmaterial.generalitemcategorygroup
            new TranslationSeedItem("entity.generalmaterial.generalitemcategorygroup", "zh-CN", "通用项目类别组", "通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）"),
            // entity.generalmaterial.generalitemcategorygroup
            new TranslationSeedItem("entity.generalmaterial.generalitemcategorygroup", "zh-HK", "通用项目类别组_hk", "通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）"),

            // entity.generalmaterial.logisticalvariants
            new TranslationSeedItem("entity.generalmaterial.logisticalvariants", "en-US", "后勤变式通用物料_us", "后勤变式通用物料"),
            // entity.generalmaterial.logisticalvariants
            new TranslationSeedItem("entity.generalmaterial.logisticalvariants", "ja-JP", "后勤变式通用物料_jp", "后勤变式通用物料"),
            // entity.generalmaterial.logisticalvariants
            new TranslationSeedItem("entity.generalmaterial.logisticalvariants", "zh-CN", "后勤变式通用物料", "后勤变式通用物料"),
            // entity.generalmaterial.logisticalvariants
            new TranslationSeedItem("entity.generalmaterial.logisticalvariants", "zh-HK", "后勤变式通用物料_hk", "后勤变式通用物料"),

            // entity.generalmaterial.materiallocked
            new TranslationSeedItem("entity.generalmaterial.materiallocked", "en-US", "物料锁定_us", "物料锁定"),
            // entity.generalmaterial.materiallocked
            new TranslationSeedItem("entity.generalmaterial.materiallocked", "ja-JP", "物料锁定_jp", "物料锁定"),
            // entity.generalmaterial.materiallocked
            new TranslationSeedItem("entity.generalmaterial.materiallocked", "zh-CN", "物料锁定", "物料锁定"),
            // entity.generalmaterial.materiallocked
            new TranslationSeedItem("entity.generalmaterial.materiallocked", "zh-HK", "物料锁定_hk", "物料锁定"),

            // entity.generalmaterial.configurationmanagementrelevant
            new TranslationSeedItem("entity.generalmaterial.configurationmanagementrelevant", "en-US", "配置管理相关_us", "配置管理相关"),
            // entity.generalmaterial.configurationmanagementrelevant
            new TranslationSeedItem("entity.generalmaterial.configurationmanagementrelevant", "ja-JP", "配置管理相关_jp", "配置管理相关"),
            // entity.generalmaterial.configurationmanagementrelevant
            new TranslationSeedItem("entity.generalmaterial.configurationmanagementrelevant", "zh-CN", "配置管理相关", "配置管理相关"),
            // entity.generalmaterial.configurationmanagementrelevant
            new TranslationSeedItem("entity.generalmaterial.configurationmanagementrelevant", "zh-HK", "配置管理相关_hk", "配置管理相关"),

            // entity.generalmaterial.assortmentlisttype
            new TranslationSeedItem("entity.generalmaterial.assortmentlisttype", "en-US", "品种清单类型_us", "品种清单类型"),
            // entity.generalmaterial.assortmentlisttype
            new TranslationSeedItem("entity.generalmaterial.assortmentlisttype", "ja-JP", "品种清单类型_jp", "品种清单类型"),
            // entity.generalmaterial.assortmentlisttype
            new TranslationSeedItem("entity.generalmaterial.assortmentlisttype", "zh-CN", "品种清单类型", "品种清单类型"),
            // entity.generalmaterial.assortmentlisttype
            new TranslationSeedItem("entity.generalmaterial.assortmentlisttype", "zh-HK", "品种清单类型_hk", "品种清单类型"),

            // entity.generalmaterial.expirationdatetype
            new TranslationSeedItem("entity.generalmaterial.expirationdatetype", "en-US", "到期日期类型_us", "到期日期类型"),
            // entity.generalmaterial.expirationdatetype
            new TranslationSeedItem("entity.generalmaterial.expirationdatetype", "ja-JP", "到期日期类型_jp", "到期日期类型"),
            // entity.generalmaterial.expirationdatetype
            new TranslationSeedItem("entity.generalmaterial.expirationdatetype", "zh-CN", "到期日期类型", "到期日期类型"),
            // entity.generalmaterial.expirationdatetype
            new TranslationSeedItem("entity.generalmaterial.expirationdatetype", "zh-HK", "到期日期类型_hk", "到期日期类型"),

            // entity.generalmaterial.gtinvariant
            new TranslationSeedItem("entity.generalmaterial.gtinvariant", "en-US", "GTIN变式_us", "GTIN变式"),
            // entity.generalmaterial.gtinvariant
            new TranslationSeedItem("entity.generalmaterial.gtinvariant", "ja-JP", "GTIN变式_jp", "GTIN变式"),
            // entity.generalmaterial.gtinvariant
            new TranslationSeedItem("entity.generalmaterial.gtinvariant", "zh-CN", "GTIN变式", "GTIN变式"),
            // entity.generalmaterial.gtinvariant
            new TranslationSeedItem("entity.generalmaterial.gtinvariant", "zh-HK", "GTIN变式_hk", "GTIN变式"),

            // entity.generalmaterial.genericmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.genericmaterialnumber", "en-US", "通用物料号_us", "通用物料号"),
            // entity.generalmaterial.genericmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.genericmaterialnumber", "ja-JP", "通用物料号_jp", "通用物料号"),
            // entity.generalmaterial.genericmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.genericmaterialnumber", "zh-CN", "通用物料号", "通用物料号"),
            // entity.generalmaterial.genericmaterialnumber
            new TranslationSeedItem("entity.generalmaterial.genericmaterialnumber", "zh-HK", "通用物料号_hk", "通用物料号"),

            // entity.generalmaterial.samepackingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.samepackingreferencematerial", "en-US", "相同包装参考物料_us", "相同包装参考物料"),
            // entity.generalmaterial.samepackingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.samepackingreferencematerial", "ja-JP", "相同包装参考物料_jp", "相同包装参考物料"),
            // entity.generalmaterial.samepackingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.samepackingreferencematerial", "zh-CN", "相同包装参考物料", "相同包装参考物料"),
            // entity.generalmaterial.samepackingreferencematerial
            new TranslationSeedItem("entity.generalmaterial.samepackingreferencematerial", "zh-HK", "相同包装参考物料_hk", "相同包装参考物料"),

            // entity.generalmaterial.globaldatasyncrelevant
            new TranslationSeedItem("entity.generalmaterial.globaldatasyncrelevant", "en-US", "全球数据同步相关_us", "全球数据同步相关"),
            // entity.generalmaterial.globaldatasyncrelevant
            new TranslationSeedItem("entity.generalmaterial.globaldatasyncrelevant", "ja-JP", "全球数据同步相关_jp", "全球数据同步相关"),
            // entity.generalmaterial.globaldatasyncrelevant
            new TranslationSeedItem("entity.generalmaterial.globaldatasyncrelevant", "zh-CN", "全球数据同步相关", "全球数据同步相关"),
            // entity.generalmaterial.globaldatasyncrelevant
            new TranslationSeedItem("entity.generalmaterial.globaldatasyncrelevant", "zh-HK", "全球数据同步相关_hk", "全球数据同步相关"),

            // entity.generalmaterial.acceptanceatorigin
            new TranslationSeedItem("entity.generalmaterial.acceptanceatorigin", "en-US", "原产地验收_us", "原产地验收"),
            // entity.generalmaterial.acceptanceatorigin
            new TranslationSeedItem("entity.generalmaterial.acceptanceatorigin", "ja-JP", "原产地验收_jp", "原产地验收"),
            // entity.generalmaterial.acceptanceatorigin
            new TranslationSeedItem("entity.generalmaterial.acceptanceatorigin", "zh-CN", "原产地验收", "原产地验收"),
            // entity.generalmaterial.acceptanceatorigin
            new TranslationSeedItem("entity.generalmaterial.acceptanceatorigin", "zh-HK", "原产地验收_hk", "原产地验收"),

            // entity.generalmaterial.standardhutype
            new TranslationSeedItem("entity.generalmaterial.standardhutype", "en-US", "标准HU类型_us", "标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.standardhutype
            new TranslationSeedItem("entity.generalmaterial.standardhutype", "ja-JP", "标准HU类型_jp", "标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.standardhutype
            new TranslationSeedItem("entity.generalmaterial.standardhutype", "zh-CN", "标准HU类型", "标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.standardhutype
            new TranslationSeedItem("entity.generalmaterial.standardhutype", "zh-HK", "标准HU类型_hk", "标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）"),

            // entity.generalmaterial.pilferable
            new TranslationSeedItem("entity.generalmaterial.pilferable", "en-US", "易被盗_us", "易被盗"),
            // entity.generalmaterial.pilferable
            new TranslationSeedItem("entity.generalmaterial.pilferable", "ja-JP", "易被盗_jp", "易被盗"),
            // entity.generalmaterial.pilferable
            new TranslationSeedItem("entity.generalmaterial.pilferable", "zh-CN", "易被盗", "易被盗"),
            // entity.generalmaterial.pilferable
            new TranslationSeedItem("entity.generalmaterial.pilferable", "zh-HK", "易被盗_hk", "易被盗"),

            // entity.generalmaterial.warehousestoragecondition
            new TranslationSeedItem("entity.generalmaterial.warehousestoragecondition", "en-US", "仓储存储条件_us", "仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）"),
            // entity.generalmaterial.warehousestoragecondition
            new TranslationSeedItem("entity.generalmaterial.warehousestoragecondition", "ja-JP", "仓储存储条件_jp", "仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）"),
            // entity.generalmaterial.warehousestoragecondition
            new TranslationSeedItem("entity.generalmaterial.warehousestoragecondition", "zh-CN", "仓储存储条件", "仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）"),
            // entity.generalmaterial.warehousestoragecondition
            new TranslationSeedItem("entity.generalmaterial.warehousestoragecondition", "zh-HK", "仓储存储条件_hk", "仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）"),

            // entity.generalmaterial.warehousematerialgroup
            new TranslationSeedItem("entity.generalmaterial.warehousematerialgroup", "en-US", "仓储物料组_us", "仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）"),
            // entity.generalmaterial.warehousematerialgroup
            new TranslationSeedItem("entity.generalmaterial.warehousematerialgroup", "ja-JP", "仓储物料组_jp", "仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）"),
            // entity.generalmaterial.warehousematerialgroup
            new TranslationSeedItem("entity.generalmaterial.warehousematerialgroup", "zh-CN", "仓储物料组", "仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）"),
            // entity.generalmaterial.warehousematerialgroup
            new TranslationSeedItem("entity.generalmaterial.warehousematerialgroup", "zh-HK", "仓储物料组_hk", "仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）"),

            // entity.generalmaterial.handlingindicator
            new TranslationSeedItem("entity.generalmaterial.handlingindicator", "en-US", "处理标识_us", "处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）"),
            // entity.generalmaterial.handlingindicator
            new TranslationSeedItem("entity.generalmaterial.handlingindicator", "ja-JP", "处理标识_jp", "处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）"),
            // entity.generalmaterial.handlingindicator
            new TranslationSeedItem("entity.generalmaterial.handlingindicator", "zh-CN", "处理标识", "处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）"),
            // entity.generalmaterial.handlingindicator
            new TranslationSeedItem("entity.generalmaterial.handlingindicator", "zh-HK", "处理标识_hk", "处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）"),

            // entity.generalmaterial.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.generalmaterial.hazardoussubstancesrelevant", "en-US", "危险物质相关_us", "危险物质相关"),
            // entity.generalmaterial.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.generalmaterial.hazardoussubstancesrelevant", "ja-JP", "危险物质相关_jp", "危险物质相关"),
            // entity.generalmaterial.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.generalmaterial.hazardoussubstancesrelevant", "zh-CN", "危险物质相关", "危险物质相关"),
            // entity.generalmaterial.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.generalmaterial.hazardoussubstancesrelevant", "zh-HK", "危险物质相关_hk", "危险物质相关"),

            // entity.generalmaterial.handlingunittype
            new TranslationSeedItem("entity.generalmaterial.handlingunittype", "en-US", "处理单元类型_us", "处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.handlingunittype
            new TranslationSeedItem("entity.generalmaterial.handlingunittype", "ja-JP", "处理单元类型_jp", "处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.handlingunittype
            new TranslationSeedItem("entity.generalmaterial.handlingunittype", "zh-CN", "处理单元类型", "处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）"),
            // entity.generalmaterial.handlingunittype
            new TranslationSeedItem("entity.generalmaterial.handlingunittype", "zh-HK", "处理单元类型_hk", "处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）"),

            // entity.generalmaterial.variabletareweight
            new TranslationSeedItem("entity.generalmaterial.variabletareweight", "en-US", "可变皮重_us", "可变皮重"),
            // entity.generalmaterial.variabletareweight
            new TranslationSeedItem("entity.generalmaterial.variabletareweight", "ja-JP", "可变皮重_jp", "可变皮重"),
            // entity.generalmaterial.variabletareweight
            new TranslationSeedItem("entity.generalmaterial.variabletareweight", "zh-CN", "可变皮重", "可变皮重"),
            // entity.generalmaterial.variabletareweight
            new TranslationSeedItem("entity.generalmaterial.variabletareweight", "zh-HK", "可变皮重_hk", "可变皮重"),

            // entity.generalmaterial.maximumallowedcapacity
            new TranslationSeedItem("entity.generalmaterial.maximumallowedcapacity", "en-US", "最大允许容量_us", "最大允许容量"),
            // entity.generalmaterial.maximumallowedcapacity
            new TranslationSeedItem("entity.generalmaterial.maximumallowedcapacity", "ja-JP", "最大允许容量_jp", "最大允许容量"),
            // entity.generalmaterial.maximumallowedcapacity
            new TranslationSeedItem("entity.generalmaterial.maximumallowedcapacity", "zh-CN", "最大允许容量", "最大允许容量"),
            // entity.generalmaterial.maximumallowedcapacity
            new TranslationSeedItem("entity.generalmaterial.maximumallowedcapacity", "zh-HK", "最大允许容量_hk", "最大允许容量"),

            // entity.generalmaterial.overcapacitytolerance
            new TranslationSeedItem("entity.generalmaterial.overcapacitytolerance", "en-US", "超容量容差_us", "超容量容差"),
            // entity.generalmaterial.overcapacitytolerance
            new TranslationSeedItem("entity.generalmaterial.overcapacitytolerance", "ja-JP", "超容量容差_jp", "超容量容差"),
            // entity.generalmaterial.overcapacitytolerance
            new TranslationSeedItem("entity.generalmaterial.overcapacitytolerance", "zh-CN", "超容量容差", "超容量容差"),
            // entity.generalmaterial.overcapacitytolerance
            new TranslationSeedItem("entity.generalmaterial.overcapacitytolerance", "zh-HK", "超容量容差_hk", "超容量容差"),

            // entity.generalmaterial.maximumpackinglength
            new TranslationSeedItem("entity.generalmaterial.maximumpackinglength", "en-US", "最大包装长度_us", "最大包装长度"),
            // entity.generalmaterial.maximumpackinglength
            new TranslationSeedItem("entity.generalmaterial.maximumpackinglength", "ja-JP", "最大包装长度_jp", "最大包装长度"),
            // entity.generalmaterial.maximumpackinglength
            new TranslationSeedItem("entity.generalmaterial.maximumpackinglength", "zh-CN", "最大包装长度", "最大包装长度"),
            // entity.generalmaterial.maximumpackinglength
            new TranslationSeedItem("entity.generalmaterial.maximumpackinglength", "zh-HK", "最大包装长度_hk", "最大包装长度"),

            // entity.generalmaterial.maximumpackingwidth
            new TranslationSeedItem("entity.generalmaterial.maximumpackingwidth", "en-US", "最大包装宽度_us", "最大包装宽度"),
            // entity.generalmaterial.maximumpackingwidth
            new TranslationSeedItem("entity.generalmaterial.maximumpackingwidth", "ja-JP", "最大包装宽度_jp", "最大包装宽度"),
            // entity.generalmaterial.maximumpackingwidth
            new TranslationSeedItem("entity.generalmaterial.maximumpackingwidth", "zh-CN", "最大包装宽度", "最大包装宽度"),
            // entity.generalmaterial.maximumpackingwidth
            new TranslationSeedItem("entity.generalmaterial.maximumpackingwidth", "zh-HK", "最大包装宽度_hk", "最大包装宽度"),

            // entity.generalmaterial.maximumpackingheight
            new TranslationSeedItem("entity.generalmaterial.maximumpackingheight", "en-US", "最大包装高度_us", "最大包装高度"),
            // entity.generalmaterial.maximumpackingheight
            new TranslationSeedItem("entity.generalmaterial.maximumpackingheight", "ja-JP", "最大包装高度_jp", "最大包装高度"),
            // entity.generalmaterial.maximumpackingheight
            new TranslationSeedItem("entity.generalmaterial.maximumpackingheight", "zh-CN", "最大包装高度", "最大包装高度"),
            // entity.generalmaterial.maximumpackingheight
            new TranslationSeedItem("entity.generalmaterial.maximumpackingheight", "zh-HK", "最大包装高度_hk", "最大包装高度"),

            // entity.generalmaterial.maximumpackingdimensionunit
            new TranslationSeedItem("entity.generalmaterial.maximumpackingdimensionunit", "en-US", "最大包装尺寸单位_us", "最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.maximumpackingdimensionunit
            new TranslationSeedItem("entity.generalmaterial.maximumpackingdimensionunit", "ja-JP", "最大包装尺寸单位_jp", "最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.maximumpackingdimensionunit
            new TranslationSeedItem("entity.generalmaterial.maximumpackingdimensionunit", "zh-CN", "最大包装尺寸单位", "最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),
            // entity.generalmaterial.maximumpackingdimensionunit
            new TranslationSeedItem("entity.generalmaterial.maximumpackingdimensionunit", "zh-HK", "最大包装尺寸单位_hk", "最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）"),

            // entity.generalmaterial.countryoforigin
            new TranslationSeedItem("entity.generalmaterial.countryoforigin", "en-US", "原产国_us", "原产国（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.generalmaterial.countryoforigin
            new TranslationSeedItem("entity.generalmaterial.countryoforigin", "ja-JP", "原产国_jp", "原产国（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.generalmaterial.countryoforigin
            new TranslationSeedItem("entity.generalmaterial.countryoforigin", "zh-CN", "原产国", "原产国（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.generalmaterial.countryoforigin
            new TranslationSeedItem("entity.generalmaterial.countryoforigin", "zh-HK", "原产国_hk", "原产国（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.generalmaterial.materialfreightgroup
            new TranslationSeedItem("entity.generalmaterial.materialfreightgroup", "en-US", "物料运费组_us", "物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）"),
            // entity.generalmaterial.materialfreightgroup
            new TranslationSeedItem("entity.generalmaterial.materialfreightgroup", "ja-JP", "物料运费组_jp", "物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）"),
            // entity.generalmaterial.materialfreightgroup
            new TranslationSeedItem("entity.generalmaterial.materialfreightgroup", "zh-CN", "物料运费组", "物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）"),
            // entity.generalmaterial.materialfreightgroup
            new TranslationSeedItem("entity.generalmaterial.materialfreightgroup", "zh-HK", "物料运费组_hk", "物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）"),

            // entity.generalmaterial.quarantineperiod
            new TranslationSeedItem("entity.generalmaterial.quarantineperiod", "en-US", "隔离期_us", "隔离期"),
            // entity.generalmaterial.quarantineperiod
            new TranslationSeedItem("entity.generalmaterial.quarantineperiod", "ja-JP", "隔离期_jp", "隔离期"),
            // entity.generalmaterial.quarantineperiod
            new TranslationSeedItem("entity.generalmaterial.quarantineperiod", "zh-CN", "隔离期", "隔离期"),
            // entity.generalmaterial.quarantineperiod
            new TranslationSeedItem("entity.generalmaterial.quarantineperiod", "zh-HK", "隔离期_hk", "隔离期"),

            // entity.generalmaterial.quarantineperiodunit
            new TranslationSeedItem("entity.generalmaterial.quarantineperiodunit", "en-US", "隔离期单位_us", "隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）"),
            // entity.generalmaterial.quarantineperiodunit
            new TranslationSeedItem("entity.generalmaterial.quarantineperiodunit", "ja-JP", "隔离期单位_jp", "隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）"),
            // entity.generalmaterial.quarantineperiodunit
            new TranslationSeedItem("entity.generalmaterial.quarantineperiodunit", "zh-CN", "隔离期单位", "隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）"),
            // entity.generalmaterial.quarantineperiodunit
            new TranslationSeedItem("entity.generalmaterial.quarantineperiodunit", "zh-HK", "隔离期单位_hk", "隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）"),

            // entity.generalmaterial.qualityinspectiongroup
            new TranslationSeedItem("entity.generalmaterial.qualityinspectiongroup", "en-US", "质检组_us", "质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）"),
            // entity.generalmaterial.qualityinspectiongroup
            new TranslationSeedItem("entity.generalmaterial.qualityinspectiongroup", "ja-JP", "质检组_jp", "质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）"),
            // entity.generalmaterial.qualityinspectiongroup
            new TranslationSeedItem("entity.generalmaterial.qualityinspectiongroup", "zh-CN", "质检组", "质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）"),
            // entity.generalmaterial.qualityinspectiongroup
            new TranslationSeedItem("entity.generalmaterial.qualityinspectiongroup", "zh-HK", "质检组_hk", "质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）"),

            // entity.generalmaterial.serialnumberprofile
            new TranslationSeedItem("entity.generalmaterial.serialnumberprofile", "en-US", "序列号参数文件_us", "序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.serialnumberprofile
            new TranslationSeedItem("entity.generalmaterial.serialnumberprofile", "ja-JP", "序列号参数文件_jp", "序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.serialnumberprofile
            new TranslationSeedItem("entity.generalmaterial.serialnumberprofile", "zh-CN", "序列号参数文件", "序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.serialnumberprofile
            new TranslationSeedItem("entity.generalmaterial.serialnumberprofile", "zh-HK", "序列号参数文件_hk", "序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.formname
            new TranslationSeedItem("entity.generalmaterial.formname", "en-US", "表单名称_us", "表单名称（字典 logistics_form_name；DictValue=表单名称编码）"),
            // entity.generalmaterial.formname
            new TranslationSeedItem("entity.generalmaterial.formname", "ja-JP", "表单名称_jp", "表单名称（字典 logistics_form_name；DictValue=表单名称编码）"),
            // entity.generalmaterial.formname
            new TranslationSeedItem("entity.generalmaterial.formname", "zh-CN", "表单名称", "表单名称（字典 logistics_form_name；DictValue=表单名称编码）"),
            // entity.generalmaterial.formname
            new TranslationSeedItem("entity.generalmaterial.formname", "zh-HK", "表单名称_hk", "表单名称（字典 logistics_form_name；DictValue=表单名称编码）"),

            // entity.generalmaterial.logisticsunitofmeasure
            new TranslationSeedItem("entity.generalmaterial.logisticsunitofmeasure", "en-US", "后勤计量单位_us", "后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.logisticsunitofmeasure
            new TranslationSeedItem("entity.generalmaterial.logisticsunitofmeasure", "ja-JP", "后勤计量单位_jp", "后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.logisticsunitofmeasure
            new TranslationSeedItem("entity.generalmaterial.logisticsunitofmeasure", "zh-CN", "后勤计量单位", "后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.generalmaterial.logisticsunitofmeasure
            new TranslationSeedItem("entity.generalmaterial.logisticsunitofmeasure", "zh-HK", "后勤计量单位_hk", "后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.generalmaterial.catchweightmaterial
            new TranslationSeedItem("entity.generalmaterial.catchweightmaterial", "en-US", "捕捞重量物料_us", "捕捞重量物料"),
            // entity.generalmaterial.catchweightmaterial
            new TranslationSeedItem("entity.generalmaterial.catchweightmaterial", "ja-JP", "捕捞重量物料_jp", "捕捞重量物料"),
            // entity.generalmaterial.catchweightmaterial
            new TranslationSeedItem("entity.generalmaterial.catchweightmaterial", "zh-CN", "捕捞重量物料", "捕捞重量物料"),
            // entity.generalmaterial.catchweightmaterial
            new TranslationSeedItem("entity.generalmaterial.catchweightmaterial", "zh-HK", "捕捞重量物料_hk", "捕捞重量物料"),

            // entity.generalmaterial.catchweightprofile
            new TranslationSeedItem("entity.generalmaterial.catchweightprofile", "en-US", "捕捞重量参数文件_us", "捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catchweightprofile
            new TranslationSeedItem("entity.generalmaterial.catchweightprofile", "ja-JP", "捕捞重量参数文件_jp", "捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catchweightprofile
            new TranslationSeedItem("entity.generalmaterial.catchweightprofile", "zh-CN", "捕捞重量参数文件", "捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.catchweightprofile
            new TranslationSeedItem("entity.generalmaterial.catchweightprofile", "zh-HK", "捕捞重量参数文件_hk", "捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.catchweighttolerancegroup
            new TranslationSeedItem("entity.generalmaterial.catchweighttolerancegroup", "en-US", "捕捞重量容差组_us", "捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）"),
            // entity.generalmaterial.catchweighttolerancegroup
            new TranslationSeedItem("entity.generalmaterial.catchweighttolerancegroup", "ja-JP", "捕捞重量容差组_jp", "捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）"),
            // entity.generalmaterial.catchweighttolerancegroup
            new TranslationSeedItem("entity.generalmaterial.catchweighttolerancegroup", "zh-CN", "捕捞重量容差组", "捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）"),
            // entity.generalmaterial.catchweighttolerancegroup
            new TranslationSeedItem("entity.generalmaterial.catchweighttolerancegroup", "zh-HK", "捕捞重量容差组_hk", "捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）"),

            // entity.generalmaterial.adjustmentprofile
            new TranslationSeedItem("entity.generalmaterial.adjustmentprofile", "en-US", "调整参数文件_us", "调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.adjustmentprofile
            new TranslationSeedItem("entity.generalmaterial.adjustmentprofile", "ja-JP", "调整参数文件_jp", "调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.adjustmentprofile
            new TranslationSeedItem("entity.generalmaterial.adjustmentprofile", "zh-CN", "调整参数文件", "调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）"),
            // entity.generalmaterial.adjustmentprofile
            new TranslationSeedItem("entity.generalmaterial.adjustmentprofile", "zh-HK", "调整参数文件_hk", "调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）"),

            // entity.generalmaterial.intellectualpropertyid
            new TranslationSeedItem("entity.generalmaterial.intellectualpropertyid", "en-US", "知识产权ID_us", "知识产权ID"),
            // entity.generalmaterial.intellectualpropertyid
            new TranslationSeedItem("entity.generalmaterial.intellectualpropertyid", "ja-JP", "知识产权ID_jp", "知识产权ID"),
            // entity.generalmaterial.intellectualpropertyid
            new TranslationSeedItem("entity.generalmaterial.intellectualpropertyid", "zh-CN", "知识产权ID", "知识产权ID"),
            // entity.generalmaterial.intellectualpropertyid
            new TranslationSeedItem("entity.generalmaterial.intellectualpropertyid", "zh-HK", "知识产权ID_hk", "知识产权ID"),

            // entity.generalmaterial.variantpriceallowed
            new TranslationSeedItem("entity.generalmaterial.variantpriceallowed", "en-US", "允许变式价格_us", "允许变式价格"),
            // entity.generalmaterial.variantpriceallowed
            new TranslationSeedItem("entity.generalmaterial.variantpriceallowed", "ja-JP", "允许变式价格_jp", "允许变式价格"),
            // entity.generalmaterial.variantpriceallowed
            new TranslationSeedItem("entity.generalmaterial.variantpriceallowed", "zh-CN", "允许变式价格", "允许变式价格"),
            // entity.generalmaterial.variantpriceallowed
            new TranslationSeedItem("entity.generalmaterial.variantpriceallowed", "zh-HK", "允许变式价格_hk", "允许变式价格"),

            // entity.generalmaterial.medium
            new TranslationSeedItem("entity.generalmaterial.medium", "en-US", "介质_us", "介质（字典 logistics_medium；DictValue=介质编码）"),
            // entity.generalmaterial.medium
            new TranslationSeedItem("entity.generalmaterial.medium", "ja-JP", "介质_jp", "介质（字典 logistics_medium；DictValue=介质编码）"),
            // entity.generalmaterial.medium
            new TranslationSeedItem("entity.generalmaterial.medium", "zh-CN", "介质", "介质（字典 logistics_medium；DictValue=介质编码）"),
            // entity.generalmaterial.medium
            new TranslationSeedItem("entity.generalmaterial.medium", "zh-HK", "介质_hk", "介质（字典 logistics_medium；DictValue=介质编码）"),

            // entity.generalmaterial.physicalcommodity
            new TranslationSeedItem("entity.generalmaterial.physicalcommodity", "en-US", "实物商品_us", "实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）"),
            // entity.generalmaterial.physicalcommodity
            new TranslationSeedItem("entity.generalmaterial.physicalcommodity", "ja-JP", "实物商品_jp", "实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）"),
            // entity.generalmaterial.physicalcommodity
            new TranslationSeedItem("entity.generalmaterial.physicalcommodity", "zh-CN", "实物商品", "实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）"),
            // entity.generalmaterial.physicalcommodity
            new TranslationSeedItem("entity.generalmaterial.physicalcommodity", "zh-HK", "实物商品_hk", "实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）"),

            // entity.generalmaterial.animalorigin
            new TranslationSeedItem("entity.generalmaterial.animalorigin", "en-US", "动物源_us", "动物源"),
            // entity.generalmaterial.animalorigin
            new TranslationSeedItem("entity.generalmaterial.animalorigin", "ja-JP", "动物源_jp", "动物源"),
            // entity.generalmaterial.animalorigin
            new TranslationSeedItem("entity.generalmaterial.animalorigin", "zh-CN", "动物源", "动物源"),
            // entity.generalmaterial.animalorigin
            new TranslationSeedItem("entity.generalmaterial.animalorigin", "zh-HK", "动物源_hk", "动物源"),

            // entity.generalmaterial.textilecompositionfunction
            new TranslationSeedItem("entity.generalmaterial.textilecompositionfunction", "en-US", "纺织成分功能_us", "纺织成分功能"),
            // entity.generalmaterial.textilecompositionfunction
            new TranslationSeedItem("entity.generalmaterial.textilecompositionfunction", "ja-JP", "纺织成分功能_jp", "纺织成分功能"),
            // entity.generalmaterial.textilecompositionfunction
            new TranslationSeedItem("entity.generalmaterial.textilecompositionfunction", "zh-CN", "纺织成分功能", "纺织成分功能"),
            // entity.generalmaterial.textilecompositionfunction
            new TranslationSeedItem("entity.generalmaterial.textilecompositionfunction", "zh-HK", "纺织成分功能_hk", "纺织成分功能"),

            // entity.generalmaterial.segmentationstructure
            new TranslationSeedItem("entity.generalmaterial.segmentationstructure", "en-US", "细分结构_us", "细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）"),
            // entity.generalmaterial.segmentationstructure
            new TranslationSeedItem("entity.generalmaterial.segmentationstructure", "ja-JP", "细分结构_jp", "细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）"),
            // entity.generalmaterial.segmentationstructure
            new TranslationSeedItem("entity.generalmaterial.segmentationstructure", "zh-CN", "细分结构", "细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）"),
            // entity.generalmaterial.segmentationstructure
            new TranslationSeedItem("entity.generalmaterial.segmentationstructure", "zh-HK", "细分结构_hk", "细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）"),

            // entity.generalmaterial.segmentationstrategy
            new TranslationSeedItem("entity.generalmaterial.segmentationstrategy", "en-US", "细分策略_us", "细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）"),
            // entity.generalmaterial.segmentationstrategy
            new TranslationSeedItem("entity.generalmaterial.segmentationstrategy", "ja-JP", "细分策略_jp", "细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）"),
            // entity.generalmaterial.segmentationstrategy
            new TranslationSeedItem("entity.generalmaterial.segmentationstrategy", "zh-CN", "细分策略", "细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）"),
            // entity.generalmaterial.segmentationstrategy
            new TranslationSeedItem("entity.generalmaterial.segmentationstrategy", "zh-HK", "细分策略_hk", "细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）"),

            // entity.generalmaterial.segmentationstatus
            new TranslationSeedItem("entity.generalmaterial.segmentationstatus", "en-US", "细分状态_us", "细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）"),
            // entity.generalmaterial.segmentationstatus
            new TranslationSeedItem("entity.generalmaterial.segmentationstatus", "ja-JP", "细分状态_jp", "细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）"),
            // entity.generalmaterial.segmentationstatus
            new TranslationSeedItem("entity.generalmaterial.segmentationstatus", "zh-CN", "细分状态", "细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）"),
            // entity.generalmaterial.segmentationstatus
            new TranslationSeedItem("entity.generalmaterial.segmentationstatus", "zh-HK", "细分状态_hk", "细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）"),

            // entity.generalmaterial.segmentationscope
            new TranslationSeedItem("entity.generalmaterial.segmentationscope", "en-US", "细分范围_us", "细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）"),
            // entity.generalmaterial.segmentationscope
            new TranslationSeedItem("entity.generalmaterial.segmentationscope", "ja-JP", "细分范围_jp", "细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）"),
            // entity.generalmaterial.segmentationscope
            new TranslationSeedItem("entity.generalmaterial.segmentationscope", "zh-CN", "细分范围", "细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）"),
            // entity.generalmaterial.segmentationscope
            new TranslationSeedItem("entity.generalmaterial.segmentationscope", "zh-HK", "细分范围_hk", "细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）"),

            // entity.generalmaterial.segmentationrelevant
            new TranslationSeedItem("entity.generalmaterial.segmentationrelevant", "en-US", "细分相关_us", "细分相关"),
            // entity.generalmaterial.segmentationrelevant
            new TranslationSeedItem("entity.generalmaterial.segmentationrelevant", "ja-JP", "细分相关_jp", "细分相关"),
            // entity.generalmaterial.segmentationrelevant
            new TranslationSeedItem("entity.generalmaterial.segmentationrelevant", "zh-CN", "细分相关", "细分相关"),
            // entity.generalmaterial.segmentationrelevant
            new TranslationSeedItem("entity.generalmaterial.segmentationrelevant", "zh-HK", "细分相关_hk", "细分相关"),

            // entity.generalmaterial.anpcode
            new TranslationSeedItem("entity.generalmaterial.anpcode", "en-US", "ANP代码_us", "ANP代码（字典 logistics_anp_code；DictValue=ANP代码）"),
            // entity.generalmaterial.anpcode
            new TranslationSeedItem("entity.generalmaterial.anpcode", "ja-JP", "ANP代码_jp", "ANP代码（字典 logistics_anp_code；DictValue=ANP代码）"),
            // entity.generalmaterial.anpcode
            new TranslationSeedItem("entity.generalmaterial.anpcode", "zh-CN", "ANP代码", "ANP代码（字典 logistics_anp_code；DictValue=ANP代码）"),
            // entity.generalmaterial.anpcode
            new TranslationSeedItem("entity.generalmaterial.anpcode", "zh-HK", "ANP代码_hk", "ANP代码（字典 logistics_anp_code；DictValue=ANP代码）"),

            // entity.generalmaterial.fashionattribute1
            new TranslationSeedItem("entity.generalmaterial.fashionattribute1", "en-US", "时装属性1_us", "时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute1
            new TranslationSeedItem("entity.generalmaterial.fashionattribute1", "ja-JP", "时装属性1_jp", "时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute1
            new TranslationSeedItem("entity.generalmaterial.fashionattribute1", "zh-CN", "时装属性1", "时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute1
            new TranslationSeedItem("entity.generalmaterial.fashionattribute1", "zh-HK", "时装属性1_hk", "时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),

            // entity.generalmaterial.fashionattribute2
            new TranslationSeedItem("entity.generalmaterial.fashionattribute2", "en-US", "时装属性2_us", "时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute2
            new TranslationSeedItem("entity.generalmaterial.fashionattribute2", "ja-JP", "时装属性2_jp", "时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute2
            new TranslationSeedItem("entity.generalmaterial.fashionattribute2", "zh-CN", "时装属性2", "时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute2
            new TranslationSeedItem("entity.generalmaterial.fashionattribute2", "zh-HK", "时装属性2_hk", "时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),

            // entity.generalmaterial.fashionattribute3
            new TranslationSeedItem("entity.generalmaterial.fashionattribute3", "en-US", "时装属性3_us", "时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute3
            new TranslationSeedItem("entity.generalmaterial.fashionattribute3", "ja-JP", "时装属性3_jp", "时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute3
            new TranslationSeedItem("entity.generalmaterial.fashionattribute3", "zh-CN", "时装属性3", "时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),
            // entity.generalmaterial.fashionattribute3
            new TranslationSeedItem("entity.generalmaterial.fashionattribute3", "zh-HK", "时装属性3_hk", "时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）"),

            // entity.generalmaterial.seasonusageindicator
            new TranslationSeedItem("entity.generalmaterial.seasonusageindicator", "en-US", "季节使用标识_us", "季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）"),
            // entity.generalmaterial.seasonusageindicator
            new TranslationSeedItem("entity.generalmaterial.seasonusageindicator", "ja-JP", "季节使用标识_jp", "季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）"),
            // entity.generalmaterial.seasonusageindicator
            new TranslationSeedItem("entity.generalmaterial.seasonusageindicator", "zh-CN", "季节使用标识", "季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）"),
            // entity.generalmaterial.seasonusageindicator
            new TranslationSeedItem("entity.generalmaterial.seasonusageindicator", "zh-HK", "季节使用标识_hk", "季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）"),

            // entity.generalmaterial.seasonactiveininventory
            new TranslationSeedItem("entity.generalmaterial.seasonactiveininventory", "en-US", "库存季节激活_us", "库存季节激活"),
            // entity.generalmaterial.seasonactiveininventory
            new TranslationSeedItem("entity.generalmaterial.seasonactiveininventory", "ja-JP", "库存季节激活_jp", "库存季节激活"),
            // entity.generalmaterial.seasonactiveininventory
            new TranslationSeedItem("entity.generalmaterial.seasonactiveininventory", "zh-CN", "库存季节激活", "库存季节激活"),
            // entity.generalmaterial.seasonactiveininventory
            new TranslationSeedItem("entity.generalmaterial.seasonactiveininventory", "zh-HK", "库存季节激活_hk", "库存季节激活"),

            // entity.generalmaterial.characteristicconversionid
            new TranslationSeedItem("entity.generalmaterial.characteristicconversionid", "en-US", "特性转换ID_us", "特性转换ID"),
            // entity.generalmaterial.characteristicconversionid
            new TranslationSeedItem("entity.generalmaterial.characteristicconversionid", "ja-JP", "特性转换ID_jp", "特性转换ID"),
            // entity.generalmaterial.characteristicconversionid
            new TranslationSeedItem("entity.generalmaterial.characteristicconversionid", "zh-CN", "特性转换ID", "特性转换ID"),
            // entity.generalmaterial.characteristicconversionid
            new TranslationSeedItem("entity.generalmaterial.characteristicconversionid", "zh-HK", "特性转换ID_hk", "特性转换ID"),

            // entity.generalmaterial.packagingcode
            new TranslationSeedItem("entity.generalmaterial.packagingcode", "en-US", "包装代码_us", "包装代码（字典 logistics_packaging_code；DictValue=包装代码）"),
            // entity.generalmaterial.packagingcode
            new TranslationSeedItem("entity.generalmaterial.packagingcode", "ja-JP", "包装代码_jp", "包装代码（字典 logistics_packaging_code；DictValue=包装代码）"),
            // entity.generalmaterial.packagingcode
            new TranslationSeedItem("entity.generalmaterial.packagingcode", "zh-CN", "包装代码", "包装代码（字典 logistics_packaging_code；DictValue=包装代码）"),
            // entity.generalmaterial.packagingcode
            new TranslationSeedItem("entity.generalmaterial.packagingcode", "zh-HK", "包装代码_hk", "包装代码（字典 logistics_packaging_code；DictValue=包装代码）"),

            // entity.generalmaterial.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodspackagingstatus", "en-US", "危险品包装状态_us", "危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）"),
            // entity.generalmaterial.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodspackagingstatus", "ja-JP", "危险品包装状态_jp", "危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）"),
            // entity.generalmaterial.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodspackagingstatus", "zh-CN", "危险品包装状态", "危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）"),
            // entity.generalmaterial.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.generalmaterial.dangerousgoodspackagingstatus", "zh-HK", "危险品包装状态_hk", "危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）"),

            // entity.generalmaterial.materialconditionmanagement
            new TranslationSeedItem("entity.generalmaterial.materialconditionmanagement", "en-US", "物料条件管理_us", "物料条件管理"),
            // entity.generalmaterial.materialconditionmanagement
            new TranslationSeedItem("entity.generalmaterial.materialconditionmanagement", "ja-JP", "物料条件管理_jp", "物料条件管理"),
            // entity.generalmaterial.materialconditionmanagement
            new TranslationSeedItem("entity.generalmaterial.materialconditionmanagement", "zh-CN", "物料条件管理", "物料条件管理"),
            // entity.generalmaterial.materialconditionmanagement
            new TranslationSeedItem("entity.generalmaterial.materialconditionmanagement", "zh-HK", "物料条件管理_hk", "物料条件管理"),

            // entity.generalmaterial.returncode
            new TranslationSeedItem("entity.generalmaterial.returncode", "en-US", "退货代码_us", "退货代码（字典 logistics_return_code；DictValue=退货代码）"),
            // entity.generalmaterial.returncode
            new TranslationSeedItem("entity.generalmaterial.returncode", "ja-JP", "退货代码_jp", "退货代码（字典 logistics_return_code；DictValue=退货代码）"),
            // entity.generalmaterial.returncode
            new TranslationSeedItem("entity.generalmaterial.returncode", "zh-CN", "退货代码", "退货代码（字典 logistics_return_code；DictValue=退货代码）"),
            // entity.generalmaterial.returncode
            new TranslationSeedItem("entity.generalmaterial.returncode", "zh-HK", "退货代码_hk", "退货代码（字典 logistics_return_code；DictValue=退货代码）"),

            // entity.generalmaterial.returntologisticslevel
            new TranslationSeedItem("entity.generalmaterial.returntologisticslevel", "en-US", "退回后勤级别_us", "退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）"),
            // entity.generalmaterial.returntologisticslevel
            new TranslationSeedItem("entity.generalmaterial.returntologisticslevel", "ja-JP", "退回后勤级别_jp", "退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）"),
            // entity.generalmaterial.returntologisticslevel
            new TranslationSeedItem("entity.generalmaterial.returntologisticslevel", "zh-CN", "退回后勤级别", "退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）"),
            // entity.generalmaterial.returntologisticslevel
            new TranslationSeedItem("entity.generalmaterial.returntologisticslevel", "zh-HK", "退回后勤级别_hk", "退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）"),

            // entity.generalmaterial.natoitemidentificationnumber
            new TranslationSeedItem("entity.generalmaterial.natoitemidentificationnumber", "en-US", "NATO物料识别号_us", "NATO物料识别号"),
            // entity.generalmaterial.natoitemidentificationnumber
            new TranslationSeedItem("entity.generalmaterial.natoitemidentificationnumber", "ja-JP", "NATO物料识别号_jp", "NATO物料识别号"),
            // entity.generalmaterial.natoitemidentificationnumber
            new TranslationSeedItem("entity.generalmaterial.natoitemidentificationnumber", "zh-CN", "NATO物料识别号", "NATO物料识别号"),
            // entity.generalmaterial.natoitemidentificationnumber
            new TranslationSeedItem("entity.generalmaterial.natoitemidentificationnumber", "zh-HK", "NATO物料识别号_hk", "NATO物料识别号"),

            // entity.generalmaterial.fffclass
            new TranslationSeedItem("entity.generalmaterial.fffclass", "en-US", "FFF类别_us", "FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）"),
            // entity.generalmaterial.fffclass
            new TranslationSeedItem("entity.generalmaterial.fffclass", "ja-JP", "FFF类别_jp", "FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）"),
            // entity.generalmaterial.fffclass
            new TranslationSeedItem("entity.generalmaterial.fffclass", "zh-CN", "FFF类别", "FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）"),
            // entity.generalmaterial.fffclass
            new TranslationSeedItem("entity.generalmaterial.fffclass", "zh-HK", "FFF类别_hk", "FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）"),

            // entity.generalmaterial.supersessionchainnumber
            new TranslationSeedItem("entity.generalmaterial.supersessionchainnumber", "en-US", "替代链编码_us", "替代链编码"),
            // entity.generalmaterial.supersessionchainnumber
            new TranslationSeedItem("entity.generalmaterial.supersessionchainnumber", "ja-JP", "替代链编码_jp", "替代链编码"),
            // entity.generalmaterial.supersessionchainnumber
            new TranslationSeedItem("entity.generalmaterial.supersessionchainnumber", "zh-CN", "替代链编码", "替代链编码"),
            // entity.generalmaterial.supersessionchainnumber
            new TranslationSeedItem("entity.generalmaterial.supersessionchainnumber", "zh-HK", "替代链编码_hk", "替代链编码"),

            // entity.generalmaterial.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.generalmaterial.seasonalprocurementcreationstatus", "en-US", "季节采购创建状态_us", "季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）"),
            // entity.generalmaterial.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.generalmaterial.seasonalprocurementcreationstatus", "ja-JP", "季节采购创建状态_jp", "季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）"),
            // entity.generalmaterial.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.generalmaterial.seasonalprocurementcreationstatus", "zh-CN", "季节采购创建状态", "季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）"),
            // entity.generalmaterial.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.generalmaterial.seasonalprocurementcreationstatus", "zh-HK", "季节采购创建状态_hk", "季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）"),

            // entity.generalmaterial.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.colorcharacteristicinternalnumber", "en-US", "颜色特性内部号_us", "颜色特性内部号"),
            // entity.generalmaterial.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.colorcharacteristicinternalnumber", "ja-JP", "颜色特性内部号_jp", "颜色特性内部号"),
            // entity.generalmaterial.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.colorcharacteristicinternalnumber", "zh-CN", "颜色特性内部号", "颜色特性内部号"),
            // entity.generalmaterial.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.colorcharacteristicinternalnumber", "zh-HK", "颜色特性内部号_hk", "颜色特性内部号"),

            // entity.generalmaterial.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.mainsizecharacteristicinternalnumber", "en-US", "主尺码特性内部号_us", "主尺码特性内部号"),
            // entity.generalmaterial.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.mainsizecharacteristicinternalnumber", "ja-JP", "主尺码特性内部号_jp", "主尺码特性内部号"),
            // entity.generalmaterial.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.mainsizecharacteristicinternalnumber", "zh-CN", "主尺码特性内部号", "主尺码特性内部号"),
            // entity.generalmaterial.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.mainsizecharacteristicinternalnumber", "zh-HK", "主尺码特性内部号_hk", "主尺码特性内部号"),

            // entity.generalmaterial.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.secondsizecharacteristicinternalnumber", "en-US", "次尺码特性内部号_us", "次尺码特性内部号"),
            // entity.generalmaterial.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.secondsizecharacteristicinternalnumber", "ja-JP", "次尺码特性内部号_jp", "次尺码特性内部号"),
            // entity.generalmaterial.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.secondsizecharacteristicinternalnumber", "zh-CN", "次尺码特性内部号", "次尺码特性内部号"),
            // entity.generalmaterial.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.generalmaterial.secondsizecharacteristicinternalnumber", "zh-HK", "次尺码特性内部号_hk", "次尺码特性内部号"),

            // entity.generalmaterial.color
            new TranslationSeedItem("entity.generalmaterial.color", "en-US", "颜色_us", "颜色（字典 logistics_color；DictValue=颜色编码）"),
            // entity.generalmaterial.color
            new TranslationSeedItem("entity.generalmaterial.color", "ja-JP", "颜色_jp", "颜色（字典 logistics_color；DictValue=颜色编码）"),
            // entity.generalmaterial.color
            new TranslationSeedItem("entity.generalmaterial.color", "zh-CN", "颜色", "颜色（字典 logistics_color；DictValue=颜色编码）"),
            // entity.generalmaterial.color
            new TranslationSeedItem("entity.generalmaterial.color", "zh-HK", "颜色_hk", "颜色（字典 logistics_color；DictValue=颜色编码）"),

            // entity.generalmaterial.mainsize
            new TranslationSeedItem("entity.generalmaterial.mainsize", "en-US", "主尺码_us", "主尺码（字典 logistics_main_size；DictValue=尺码编码）"),
            // entity.generalmaterial.mainsize
            new TranslationSeedItem("entity.generalmaterial.mainsize", "ja-JP", "主尺码_jp", "主尺码（字典 logistics_main_size；DictValue=尺码编码）"),
            // entity.generalmaterial.mainsize
            new TranslationSeedItem("entity.generalmaterial.mainsize", "zh-CN", "主尺码", "主尺码（字典 logistics_main_size；DictValue=尺码编码）"),
            // entity.generalmaterial.mainsize
            new TranslationSeedItem("entity.generalmaterial.mainsize", "zh-HK", "主尺码_hk", "主尺码（字典 logistics_main_size；DictValue=尺码编码）"),

            // entity.generalmaterial.secondsize
            new TranslationSeedItem("entity.generalmaterial.secondsize", "en-US", "次尺码_us", "次尺码（字典 logistics_second_size；DictValue=尺码编码）"),
            // entity.generalmaterial.secondsize
            new TranslationSeedItem("entity.generalmaterial.secondsize", "ja-JP", "次尺码_jp", "次尺码（字典 logistics_second_size；DictValue=尺码编码）"),
            // entity.generalmaterial.secondsize
            new TranslationSeedItem("entity.generalmaterial.secondsize", "zh-CN", "次尺码", "次尺码（字典 logistics_second_size；DictValue=尺码编码）"),
            // entity.generalmaterial.secondsize
            new TranslationSeedItem("entity.generalmaterial.secondsize", "zh-HK", "次尺码_hk", "次尺码（字典 logistics_second_size；DictValue=尺码编码）"),

            // entity.generalmaterial.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.generalmaterial.evaluationcharacteristicvalue", "en-US", "评估特性值_us", "评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）"),
            // entity.generalmaterial.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.generalmaterial.evaluationcharacteristicvalue", "ja-JP", "评估特性值_jp", "评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）"),
            // entity.generalmaterial.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.generalmaterial.evaluationcharacteristicvalue", "zh-CN", "评估特性值", "评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）"),
            // entity.generalmaterial.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.generalmaterial.evaluationcharacteristicvalue", "zh-HK", "评估特性值_hk", "评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）"),

            // entity.generalmaterial.carecode
            new TranslationSeedItem("entity.generalmaterial.carecode", "en-US", "护理代码_us", "护理代码（字典 logistics_care_code；DictValue=护理代码）"),
            // entity.generalmaterial.carecode
            new TranslationSeedItem("entity.generalmaterial.carecode", "ja-JP", "护理代码_jp", "护理代码（字典 logistics_care_code；DictValue=护理代码）"),
            // entity.generalmaterial.carecode
            new TranslationSeedItem("entity.generalmaterial.carecode", "zh-CN", "护理代码", "护理代码（字典 logistics_care_code；DictValue=护理代码）"),
            // entity.generalmaterial.carecode
            new TranslationSeedItem("entity.generalmaterial.carecode", "zh-HK", "护理代码_hk", "护理代码（字典 logistics_care_code；DictValue=护理代码）"),

            // entity.generalmaterial.brandid
            new TranslationSeedItem("entity.generalmaterial.brandid", "en-US", "品牌_us", "品牌（字典 logistics_brand_id；DictValue=品牌编码）"),
            // entity.generalmaterial.brandid
            new TranslationSeedItem("entity.generalmaterial.brandid", "ja-JP", "品牌_jp", "品牌（字典 logistics_brand_id；DictValue=品牌编码）"),
            // entity.generalmaterial.brandid
            new TranslationSeedItem("entity.generalmaterial.brandid", "zh-CN", "品牌", "品牌（字典 logistics_brand_id；DictValue=品牌编码）"),
            // entity.generalmaterial.brandid
            new TranslationSeedItem("entity.generalmaterial.brandid", "zh-HK", "品牌_hk", "品牌（字典 logistics_brand_id；DictValue=品牌编码）"),

            // entity.generalmaterial.fibercode1
            new TranslationSeedItem("entity.generalmaterial.fibercode1", "en-US", "纤维代码1_us", "纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode1
            new TranslationSeedItem("entity.generalmaterial.fibercode1", "ja-JP", "纤维代码1_jp", "纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode1
            new TranslationSeedItem("entity.generalmaterial.fibercode1", "zh-CN", "纤维代码1", "纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode1
            new TranslationSeedItem("entity.generalmaterial.fibercode1", "zh-HK", "纤维代码1_hk", "纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）"),

            // entity.generalmaterial.fiberpart1
            new TranslationSeedItem("entity.generalmaterial.fiberpart1", "en-US", "纤维占比1_us", "纤维占比1"),
            // entity.generalmaterial.fiberpart1
            new TranslationSeedItem("entity.generalmaterial.fiberpart1", "ja-JP", "纤维占比1_jp", "纤维占比1"),
            // entity.generalmaterial.fiberpart1
            new TranslationSeedItem("entity.generalmaterial.fiberpart1", "zh-CN", "纤维占比1", "纤维占比1"),
            // entity.generalmaterial.fiberpart1
            new TranslationSeedItem("entity.generalmaterial.fiberpart1", "zh-HK", "纤维占比1_hk", "纤维占比1"),

            // entity.generalmaterial.fibercode2
            new TranslationSeedItem("entity.generalmaterial.fibercode2", "en-US", "纤维代码2_us", "纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode2
            new TranslationSeedItem("entity.generalmaterial.fibercode2", "ja-JP", "纤维代码2_jp", "纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode2
            new TranslationSeedItem("entity.generalmaterial.fibercode2", "zh-CN", "纤维代码2", "纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode2
            new TranslationSeedItem("entity.generalmaterial.fibercode2", "zh-HK", "纤维代码2_hk", "纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）"),

            // entity.generalmaterial.fiberpart2
            new TranslationSeedItem("entity.generalmaterial.fiberpart2", "en-US", "纤维占比2_us", "纤维占比2"),
            // entity.generalmaterial.fiberpart2
            new TranslationSeedItem("entity.generalmaterial.fiberpart2", "ja-JP", "纤维占比2_jp", "纤维占比2"),
            // entity.generalmaterial.fiberpart2
            new TranslationSeedItem("entity.generalmaterial.fiberpart2", "zh-CN", "纤维占比2", "纤维占比2"),
            // entity.generalmaterial.fiberpart2
            new TranslationSeedItem("entity.generalmaterial.fiberpart2", "zh-HK", "纤维占比2_hk", "纤维占比2"),

            // entity.generalmaterial.fibercode3
            new TranslationSeedItem("entity.generalmaterial.fibercode3", "en-US", "纤维代码3_us", "纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode3
            new TranslationSeedItem("entity.generalmaterial.fibercode3", "ja-JP", "纤维代码3_jp", "纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode3
            new TranslationSeedItem("entity.generalmaterial.fibercode3", "zh-CN", "纤维代码3", "纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode3
            new TranslationSeedItem("entity.generalmaterial.fibercode3", "zh-HK", "纤维代码3_hk", "纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）"),

            // entity.generalmaterial.fiberpart3
            new TranslationSeedItem("entity.generalmaterial.fiberpart3", "en-US", "纤维占比3_us", "纤维占比3"),
            // entity.generalmaterial.fiberpart3
            new TranslationSeedItem("entity.generalmaterial.fiberpart3", "ja-JP", "纤维占比3_jp", "纤维占比3"),
            // entity.generalmaterial.fiberpart3
            new TranslationSeedItem("entity.generalmaterial.fiberpart3", "zh-CN", "纤维占比3", "纤维占比3"),
            // entity.generalmaterial.fiberpart3
            new TranslationSeedItem("entity.generalmaterial.fiberpart3", "zh-HK", "纤维占比3_hk", "纤维占比3"),

            // entity.generalmaterial.fibercode4
            new TranslationSeedItem("entity.generalmaterial.fibercode4", "en-US", "纤维代码4_us", "纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode4
            new TranslationSeedItem("entity.generalmaterial.fibercode4", "ja-JP", "纤维代码4_jp", "纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode4
            new TranslationSeedItem("entity.generalmaterial.fibercode4", "zh-CN", "纤维代码4", "纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode4
            new TranslationSeedItem("entity.generalmaterial.fibercode4", "zh-HK", "纤维代码4_hk", "纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）"),

            // entity.generalmaterial.fiberpart4
            new TranslationSeedItem("entity.generalmaterial.fiberpart4", "en-US", "纤维占比4_us", "纤维占比4"),
            // entity.generalmaterial.fiberpart4
            new TranslationSeedItem("entity.generalmaterial.fiberpart4", "ja-JP", "纤维占比4_jp", "纤维占比4"),
            // entity.generalmaterial.fiberpart4
            new TranslationSeedItem("entity.generalmaterial.fiberpart4", "zh-CN", "纤维占比4", "纤维占比4"),
            // entity.generalmaterial.fiberpart4
            new TranslationSeedItem("entity.generalmaterial.fiberpart4", "zh-HK", "纤维占比4_hk", "纤维占比4"),

            // entity.generalmaterial.fibercode5
            new TranslationSeedItem("entity.generalmaterial.fibercode5", "en-US", "纤维代码5_us", "纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode5
            new TranslationSeedItem("entity.generalmaterial.fibercode5", "ja-JP", "纤维代码5_jp", "纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode5
            new TranslationSeedItem("entity.generalmaterial.fibercode5", "zh-CN", "纤维代码5", "纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）"),
            // entity.generalmaterial.fibercode5
            new TranslationSeedItem("entity.generalmaterial.fibercode5", "zh-HK", "纤维代码5_hk", "纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）"),

            // entity.generalmaterial.fiberpart5
            new TranslationSeedItem("entity.generalmaterial.fiberpart5", "en-US", "纤维占比5_us", "纤维占比5"),
            // entity.generalmaterial.fiberpart5
            new TranslationSeedItem("entity.generalmaterial.fiberpart5", "ja-JP", "纤维占比5_jp", "纤维占比5"),
            // entity.generalmaterial.fiberpart5
            new TranslationSeedItem("entity.generalmaterial.fiberpart5", "zh-CN", "纤维占比5", "纤维占比5"),
            // entity.generalmaterial.fiberpart5
            new TranslationSeedItem("entity.generalmaterial.fiberpart5", "zh-HK", "纤维占比5_hk", "纤维占比5"),

            // entity.generalmaterial.fashiongrade
            new TranslationSeedItem("entity.generalmaterial.fashiongrade", "en-US", "时装等级_us", "时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）"),
            // entity.generalmaterial.fashiongrade
            new TranslationSeedItem("entity.generalmaterial.fashiongrade", "ja-JP", "时装等级_jp", "时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）"),
            // entity.generalmaterial.fashiongrade
            new TranslationSeedItem("entity.generalmaterial.fashiongrade", "zh-CN", "时装等级", "时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）"),
            // entity.generalmaterial.fashiongrade
            new TranslationSeedItem("entity.generalmaterial.fashiongrade", "zh-HK", "时装等级_hk", "时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）"),
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
