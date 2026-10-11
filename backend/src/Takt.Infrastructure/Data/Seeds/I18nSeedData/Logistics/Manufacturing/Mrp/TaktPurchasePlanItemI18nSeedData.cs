// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlanItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchasePlanItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp;

/// <summary>
/// TaktPurchasePlanItem 实体国际化翻译种子（键前缀 entity.purchaseplanitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchasePlanItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchasePlanItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseplanitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchasePlanItemTranslations())
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

        TaktLogger.Information("TaktPurchasePlanItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchasePlanItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseplanitem._self / entity.purchaseplanitem.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchasePlanItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseplanitem._self
            new TranslationSeedItem("entity.purchaseplanitem._self", "en-US", "Purchase Plan Item Information_us", "实体名称"),
            // entity.purchaseplanitem._self
            new TranslationSeedItem("entity.purchaseplanitem._self", "ja-JP", "Takt采购计划明细信息_jp", "实体名称"),
            // entity.purchaseplanitem._self
            new TranslationSeedItem("entity.purchaseplanitem._self", "zh-CN", "Takt采购计划明细信息", "实体名称"),
            // entity.purchaseplanitem._self
            new TranslationSeedItem("entity.purchaseplanitem._self", "zh-HK", "Takt采购计划明细信息_hk", "实体名称"),

            // entity.purchaseplanitem.purchaseplanid
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplanid", "en-US", "采购计划ID_us", "采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.purchaseplanid
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplanid", "ja-JP", "采购计划ID_jp", "采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.purchaseplanid
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplanid", "zh-CN", "采购计划ID", "采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.purchaseplanid
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplanid", "zh-HK", "采购计划ID_hk", "采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseplanitem.purchaseplancode
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplancode", "en-US", "采购计划编码_us", "采购计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplanitem.purchaseplancode
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplancode", "ja-JP", "采购计划编码_jp", "采购计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplanitem.purchaseplancode
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplancode", "zh-CN", "采购计划编码", "采购计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseplanitem.purchaseplancode
            new TranslationSeedItem("entity.purchaseplanitem.purchaseplancode", "zh-HK", "采购计划编码_hk", "采购计划编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaseplanitem.linenumber
            new TranslationSeedItem("entity.purchaseplanitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseplanitem.linenumber
            new TranslationSeedItem("entity.purchaseplanitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseplanitem.linenumber
            new TranslationSeedItem("entity.purchaseplanitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseplanitem.linenumber
            new TranslationSeedItem("entity.purchaseplanitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseplanitem.productionplanid
            new TranslationSeedItem("entity.purchaseplanitem.productionplanid", "en-US", "来源生产计划ID_us", "来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.productionplanid
            new TranslationSeedItem("entity.purchaseplanitem.productionplanid", "ja-JP", "来源生产计划ID_jp", "来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.productionplanid
            new TranslationSeedItem("entity.purchaseplanitem.productionplanid", "zh-CN", "来源生产计划ID", "来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseplanitem.productionplanid
            new TranslationSeedItem("entity.purchaseplanitem.productionplanid", "zh-HK", "来源生产计划ID_hk", "来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseplanitem.productionplancode
            new TranslationSeedItem("entity.purchaseplanitem.productionplancode", "en-US", "来源生产计划编码_us", "来源生产计划编码"),
            // entity.purchaseplanitem.productionplancode
            new TranslationSeedItem("entity.purchaseplanitem.productionplancode", "ja-JP", "来源生产计划编码_jp", "来源生产计划编码"),
            // entity.purchaseplanitem.productionplancode
            new TranslationSeedItem("entity.purchaseplanitem.productionplancode", "zh-CN", "来源生产计划编码", "来源生产计划编码"),
            // entity.purchaseplanitem.productionplancode
            new TranslationSeedItem("entity.purchaseplanitem.productionplancode", "zh-HK", "来源生产计划编码_hk", "来源生产计划编码"),

            // entity.purchaseplanitem.productionplanlinenumber
            new TranslationSeedItem("entity.purchaseplanitem.productionplanlinenumber", "en-US", "来源生产计划行号_us", "来源生产计划行号"),
            // entity.purchaseplanitem.productionplanlinenumber
            new TranslationSeedItem("entity.purchaseplanitem.productionplanlinenumber", "ja-JP", "来源生产计划行号_jp", "来源生产计划行号"),
            // entity.purchaseplanitem.productionplanlinenumber
            new TranslationSeedItem("entity.purchaseplanitem.productionplanlinenumber", "zh-CN", "来源生产计划行号", "来源生产计划行号"),
            // entity.purchaseplanitem.productionplanlinenumber
            new TranslationSeedItem("entity.purchaseplanitem.productionplanlinenumber", "zh-HK", "来源生产计划行号_hk", "来源生产计划行号"),

            // entity.purchaseplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.purchaseplanitem.materialrequirementsplanningitemid", "en-US", "来源MRP明细ID_us", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.purchaseplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.purchaseplanitem.materialrequirementsplanningitemid", "ja-JP", "来源MRP明细ID_jp", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.purchaseplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.purchaseplanitem.materialrequirementsplanningitemid", "zh-CN", "来源MRP明细ID", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.purchaseplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.purchaseplanitem.materialrequirementsplanningitemid", "zh-HK", "来源MRP明细ID_hk", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),

            // entity.purchaseplanitem.materialcode
            new TranslationSeedItem("entity.purchaseplanitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseplanitem.materialcode
            new TranslationSeedItem("entity.purchaseplanitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseplanitem.materialcode
            new TranslationSeedItem("entity.purchaseplanitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseplanitem.materialcode
            new TranslationSeedItem("entity.purchaseplanitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.purchaseplanitem.materialdescription
            new TranslationSeedItem("entity.purchaseplanitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseplanitem.materialdescription
            new TranslationSeedItem("entity.purchaseplanitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseplanitem.materialdescription
            new TranslationSeedItem("entity.purchaseplanitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseplanitem.materialdescription
            new TranslationSeedItem("entity.purchaseplanitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchaseplanitem.materialspecification
            new TranslationSeedItem("entity.purchaseplanitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.purchaseplanitem.materialspecification
            new TranslationSeedItem("entity.purchaseplanitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.purchaseplanitem.materialspecification
            new TranslationSeedItem("entity.purchaseplanitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.purchaseplanitem.materialspecification
            new TranslationSeedItem("entity.purchaseplanitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.purchaseplanitem.planunit
            new TranslationSeedItem("entity.purchaseplanitem.planunit", "en-US", "计划单位_us", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseplanitem.planunit
            new TranslationSeedItem("entity.purchaseplanitem.planunit", "ja-JP", "计划单位_jp", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseplanitem.planunit
            new TranslationSeedItem("entity.purchaseplanitem.planunit", "zh-CN", "计划单位", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseplanitem.planunit
            new TranslationSeedItem("entity.purchaseplanitem.planunit", "zh-HK", "计划单位_hk", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.purchaseplanitem.planquantity
            new TranslationSeedItem("entity.purchaseplanitem.planquantity", "en-US", "计划数量_us", "计划数量（基本单位数量）"),
            // entity.purchaseplanitem.planquantity
            new TranslationSeedItem("entity.purchaseplanitem.planquantity", "ja-JP", "计划数量_jp", "计划数量（基本单位数量）"),
            // entity.purchaseplanitem.planquantity
            new TranslationSeedItem("entity.purchaseplanitem.planquantity", "zh-CN", "计划数量", "计划数量（基本单位数量）"),
            // entity.purchaseplanitem.planquantity
            new TranslationSeedItem("entity.purchaseplanitem.planquantity", "zh-HK", "计划数量_hk", "计划数量（基本单位数量）"),

            // entity.purchaseplanitem.plannedarrivaldate
            new TranslationSeedItem("entity.purchaseplanitem.plannedarrivaldate", "en-US", "计划到货日期_us", "计划到货日期"),
            // entity.purchaseplanitem.plannedarrivaldate
            new TranslationSeedItem("entity.purchaseplanitem.plannedarrivaldate", "ja-JP", "计划到货日期_jp", "计划到货日期"),
            // entity.purchaseplanitem.plannedarrivaldate
            new TranslationSeedItem("entity.purchaseplanitem.plannedarrivaldate", "zh-CN", "计划到货日期", "计划到货日期"),
            // entity.purchaseplanitem.plannedarrivaldate
            new TranslationSeedItem("entity.purchaseplanitem.plannedarrivaldate", "zh-HK", "计划到货日期_hk", "计划到货日期"),

            // entity.purchaseplanitem.convertedquantity
            new TranslationSeedItem("entity.purchaseplanitem.convertedquantity", "en-US", "已转申请订单数量_us", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplanitem.convertedquantity
            new TranslationSeedItem("entity.purchaseplanitem.convertedquantity", "ja-JP", "已转申请订单数量_jp", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplanitem.convertedquantity
            new TranslationSeedItem("entity.purchaseplanitem.convertedquantity", "zh-CN", "已转申请订单数量", "已转申请/订单数量（基本单位数量）"),
            // entity.purchaseplanitem.convertedquantity
            new TranslationSeedItem("entity.purchaseplanitem.convertedquantity", "zh-HK", "已转申请订单数量_hk", "已转申请/订单数量（基本单位数量）"),

            // entity.purchaseplanitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseplanitem.estimatedunitprice", "en-US", "预计单价_us", "预计单价"),
            // entity.purchaseplanitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseplanitem.estimatedunitprice", "ja-JP", "预计单价_jp", "预计单价"),
            // entity.purchaseplanitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseplanitem.estimatedunitprice", "zh-CN", "预计单价", "预计单价"),
            // entity.purchaseplanitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseplanitem.estimatedunitprice", "zh-HK", "预计单价_hk", "预计单价"),

            // entity.purchaseplanitem.estimatedamount
            new TranslationSeedItem("entity.purchaseplanitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.purchaseplanitem.estimatedamount
            new TranslationSeedItem("entity.purchaseplanitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.purchaseplanitem.estimatedamount
            new TranslationSeedItem("entity.purchaseplanitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.purchaseplanitem.estimatedamount
            new TranslationSeedItem("entity.purchaseplanitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.purchaseplanitem.taxincludedprice
            new TranslationSeedItem("entity.purchaseplanitem.taxincludedprice", "en-US", "含税价格_us", "含税价格（打印用；如 100.00）"),
            // entity.purchaseplanitem.taxincludedprice
            new TranslationSeedItem("entity.purchaseplanitem.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（打印用；如 100.00）"),
            // entity.purchaseplanitem.taxincludedprice
            new TranslationSeedItem("entity.purchaseplanitem.taxincludedprice", "zh-CN", "含税价格", "含税价格（打印用；如 100.00）"),
            // entity.purchaseplanitem.taxincludedprice
            new TranslationSeedItem("entity.purchaseplanitem.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（打印用；如 100.00）"),

            // entity.purchaseplanitem.untaxedprice
            new TranslationSeedItem("entity.purchaseplanitem.untaxedprice", "en-US", "未税价格_us", "未税价格（打印用；如 88.50）"),
            // entity.purchaseplanitem.untaxedprice
            new TranslationSeedItem("entity.purchaseplanitem.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（打印用；如 88.50）"),
            // entity.purchaseplanitem.untaxedprice
            new TranslationSeedItem("entity.purchaseplanitem.untaxedprice", "zh-CN", "未税价格", "未税价格（打印用；如 88.50）"),
            // entity.purchaseplanitem.untaxedprice
            new TranslationSeedItem("entity.purchaseplanitem.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（打印用；如 88.50）"),

            // entity.purchaseplanitem.taxamount
            new TranslationSeedItem("entity.purchaseplanitem.taxamount", "en-US", "税费_us", "税费（打印用；行税额，如 11.50）"),
            // entity.purchaseplanitem.taxamount
            new TranslationSeedItem("entity.purchaseplanitem.taxamount", "ja-JP", "税费_jp", "税费（打印用；行税额，如 11.50）"),
            // entity.purchaseplanitem.taxamount
            new TranslationSeedItem("entity.purchaseplanitem.taxamount", "zh-CN", "税费", "税费（打印用；行税额，如 11.50）"),
            // entity.purchaseplanitem.taxamount
            new TranslationSeedItem("entity.purchaseplanitem.taxamount", "zh-HK", "税费_hk", "税费（打印用；行税额，如 11.50）"),

            // entity.purchaseplanitem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliercode", "en-US", "参考供货商编码_us", "参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseplanitem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliercode", "ja-JP", "参考供货商编码_jp", "参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseplanitem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliercode", "zh-CN", "参考供货商编码", "参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseplanitem.referencesuppliercode
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliercode", "zh-HK", "参考供货商编码_hk", "参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.purchaseplanitem.referencesuppliername1
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliername1", "en-US", "参考供货商名称1_us", "参考供货商名称"),
            // entity.purchaseplanitem.referencesuppliername1
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliername1", "ja-JP", "参考供货商名称1_jp", "参考供货商名称"),
            // entity.purchaseplanitem.referencesuppliername1
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliername1", "zh-CN", "参考供货商名称1", "参考供货商名称"),
            // entity.purchaseplanitem.referencesuppliername1
            new TranslationSeedItem("entity.purchaseplanitem.referencesuppliername1", "zh-HK", "参考供货商名称1_hk", "参考供货商名称"),

            // entity.purchaseplanitem.isobsolete
            new TranslationSeedItem("entity.purchaseplanitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseplanitem.isobsolete
            new TranslationSeedItem("entity.purchaseplanitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseplanitem.isobsolete
            new TranslationSeedItem("entity.purchaseplanitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseplanitem.isobsolete
            new TranslationSeedItem("entity.purchaseplanitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mrp";
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
