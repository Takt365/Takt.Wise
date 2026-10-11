// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIqcOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktIqcOrderItem 实体国际化翻译种子（键前缀 entity.iqcorderitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIqcOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIqcOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 iqcorderitem 实体翻译...", tenantCode);

        foreach (var item in GetIqcOrderItemTranslations())
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

        TaktLogger.Information("TaktIqcOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIqcOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.iqcorderitem._self / entity.iqcorderitem.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIqcOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.iqcorderitem._self
            new TranslationSeedItem("entity.iqcorderitem._self", "en-US", "Iqc Order Item Information_us", "实体名称"),
            // entity.iqcorderitem._self
            new TranslationSeedItem("entity.iqcorderitem._self", "ja-JP", "IQC进货检验单明细信息_jp", "实体名称"),
            // entity.iqcorderitem._self
            new TranslationSeedItem("entity.iqcorderitem._self", "zh-CN", "IQC进货检验单明细信息", "实体名称"),
            // entity.iqcorderitem._self
            new TranslationSeedItem("entity.iqcorderitem._self", "zh-HK", "IQC进货检验单明细信息_hk", "实体名称"),

            // entity.iqcorderitem.iqcorderid
            new TranslationSeedItem("entity.iqcorderitem.iqcorderid", "en-US", "IQC检验单ID_us", "IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）"),
            // entity.iqcorderitem.iqcorderid
            new TranslationSeedItem("entity.iqcorderitem.iqcorderid", "ja-JP", "IQC检验单ID_jp", "IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）"),
            // entity.iqcorderitem.iqcorderid
            new TranslationSeedItem("entity.iqcorderitem.iqcorderid", "zh-CN", "IQC检验单ID", "IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）"),
            // entity.iqcorderitem.iqcorderid
            new TranslationSeedItem("entity.iqcorderitem.iqcorderid", "zh-HK", "IQC检验单ID_hk", "IQC检验单 ID（选项 TaktIqcOrders/options，DictValue=Id）"),

            // entity.iqcorderitem.iqcordercode
            new TranslationSeedItem("entity.iqcorderitem.iqcordercode", "en-US", "IQC检验单编码_us", "IQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.iqcorderitem.iqcordercode
            new TranslationSeedItem("entity.iqcorderitem.iqcordercode", "ja-JP", "IQC检验单编码_jp", "IQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.iqcorderitem.iqcordercode
            new TranslationSeedItem("entity.iqcorderitem.iqcordercode", "zh-CN", "IQC检验单编码", "IQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.iqcorderitem.iqcordercode
            new TranslationSeedItem("entity.iqcorderitem.iqcordercode", "zh-HK", "IQC检验单编码_hk", "IQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.iqcorderitem.linenumber
            new TranslationSeedItem("entity.iqcorderitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.iqcorderitem.linenumber
            new TranslationSeedItem("entity.iqcorderitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.iqcorderitem.linenumber
            new TranslationSeedItem("entity.iqcorderitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcorderitem.linenumber
            new TranslationSeedItem("entity.iqcorderitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.iqcorderitem.materialcode
            new TranslationSeedItem("entity.iqcorderitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.iqcorderitem.materialcode
            new TranslationSeedItem("entity.iqcorderitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.iqcorderitem.materialcode
            new TranslationSeedItem("entity.iqcorderitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.iqcorderitem.materialcode
            new TranslationSeedItem("entity.iqcorderitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.iqcorderitem.materialdescription
            new TranslationSeedItem("entity.iqcorderitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.iqcorderitem.materialdescription
            new TranslationSeedItem("entity.iqcorderitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.iqcorderitem.materialdescription
            new TranslationSeedItem("entity.iqcorderitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.iqcorderitem.materialdescription
            new TranslationSeedItem("entity.iqcorderitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.iqcorderitem.batchcode
            new TranslationSeedItem("entity.iqcorderitem.batchcode", "en-US", "批次号_us", "批次号"),
            // entity.iqcorderitem.batchcode
            new TranslationSeedItem("entity.iqcorderitem.batchcode", "ja-JP", "批次号_jp", "批次号"),
            // entity.iqcorderitem.batchcode
            new TranslationSeedItem("entity.iqcorderitem.batchcode", "zh-CN", "批次号", "批次号"),
            // entity.iqcorderitem.batchcode
            new TranslationSeedItem("entity.iqcorderitem.batchcode", "zh-HK", "批次号_hk", "批次号"),

            // entity.iqcorderitem.purchasequantity
            new TranslationSeedItem("entity.iqcorderitem.purchasequantity", "en-US", "进货数量_us", "进货数量"),
            // entity.iqcorderitem.purchasequantity
            new TranslationSeedItem("entity.iqcorderitem.purchasequantity", "ja-JP", "进货数量_jp", "进货数量"),
            // entity.iqcorderitem.purchasequantity
            new TranslationSeedItem("entity.iqcorderitem.purchasequantity", "zh-CN", "进货数量", "进货数量"),
            // entity.iqcorderitem.purchasequantity
            new TranslationSeedItem("entity.iqcorderitem.purchasequantity", "zh-HK", "进货数量_hk", "进货数量"),

            // entity.iqcorderitem.standardcode
            new TranslationSeedItem("entity.iqcorderitem.standardcode", "en-US", "检验标准编码_us", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.iqcorderitem.standardcode
            new TranslationSeedItem("entity.iqcorderitem.standardcode", "ja-JP", "检验标准编码_jp", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.iqcorderitem.standardcode
            new TranslationSeedItem("entity.iqcorderitem.standardcode", "zh-CN", "检验标准编码", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.iqcorderitem.standardcode
            new TranslationSeedItem("entity.iqcorderitem.standardcode", "zh-HK", "检验标准编码_hk", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),

            // entity.iqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.iqcorderitem.samplingschemecode", "en-US", "抽样方案编码_us", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.iqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.iqcorderitem.samplingschemecode", "ja-JP", "抽样方案编码_jp", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.iqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.iqcorderitem.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.iqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.iqcorderitem.samplingschemecode", "zh-HK", "抽样方案编码_hk", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),

            // entity.iqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.iqcorderitem.inspectionmethod", "en-US", "检验方式_us", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.iqcorderitem.inspectionmethod", "ja-JP", "检验方式_jp", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.iqcorderitem.inspectionmethod", "zh-CN", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.iqcorderitem.inspectionmethod", "zh-HK", "检验方式_hk", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),

            // entity.iqcorderitem.samplequantity
            new TranslationSeedItem("entity.iqcorderitem.samplequantity", "en-US", "抽样数量_us", "抽样数量"),
            // entity.iqcorderitem.samplequantity
            new TranslationSeedItem("entity.iqcorderitem.samplequantity", "ja-JP", "抽样数量_jp", "抽样数量"),
            // entity.iqcorderitem.samplequantity
            new TranslationSeedItem("entity.iqcorderitem.samplequantity", "zh-CN", "抽样数量", "抽样数量"),
            // entity.iqcorderitem.samplequantity
            new TranslationSeedItem("entity.iqcorderitem.samplequantity", "zh-HK", "抽样数量_hk", "抽样数量"),

            // entity.iqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.qualifiedquantity", "en-US", "合格数量_us", "合格数量"),
            // entity.iqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.qualifiedquantity", "ja-JP", "合格数量_jp", "合格数量"),
            // entity.iqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.qualifiedquantity", "zh-CN", "合格数量", "合格数量"),
            // entity.iqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.qualifiedquantity", "zh-HK", "合格数量_hk", "合格数量"),

            // entity.iqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.unqualifiedquantity", "en-US", "不合格数量_us", "不合格数量"),
            // entity.iqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.unqualifiedquantity", "ja-JP", "不合格数量_jp", "不合格数量"),
            // entity.iqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.unqualifiedquantity", "zh-CN", "不合格数量", "不合格数量"),
            // entity.iqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcorderitem.unqualifiedquantity", "zh-HK", "不合格数量_hk", "不合格数量"),

            // entity.iqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcorderitem.inspectionreturnquantity", "en-US", "验退数量_us", "验退数量"),
            // entity.iqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcorderitem.inspectionreturnquantity", "ja-JP", "验退数量_jp", "验退数量"),
            // entity.iqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcorderitem.inspectionreturnquantity", "zh-CN", "验退数量", "验退数量"),
            // entity.iqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcorderitem.inspectionreturnquantity", "zh-HK", "验退数量_hk", "验退数量"),

            // entity.iqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.iqcorderitem.sampleserialcode", "en-US", "抽检序列号_us", "抽检序列号"),
            // entity.iqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.iqcorderitem.sampleserialcode", "ja-JP", "抽检序列号_jp", "抽检序列号"),
            // entity.iqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.iqcorderitem.sampleserialcode", "zh-CN", "抽检序列号", "抽检序列号"),
            // entity.iqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.iqcorderitem.sampleserialcode", "zh-HK", "抽检序列号_hk", "抽检序列号"),

            // entity.iqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.iqcorderitem.inspectiondescription", "en-US", "检验说明_us", "检验说明"),
            // entity.iqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.iqcorderitem.inspectiondescription", "ja-JP", "检验说明_jp", "检验说明"),
            // entity.iqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.iqcorderitem.inspectiondescription", "zh-CN", "检验说明", "检验说明"),
            // entity.iqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.iqcorderitem.inspectiondescription", "zh-HK", "检验说明_hk", "检验说明"),

            // entity.iqcorderitem.inspectorid
            new TranslationSeedItem("entity.iqcorderitem.inspectorid", "en-US", "检验员ID_us", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.iqcorderitem.inspectorid
            new TranslationSeedItem("entity.iqcorderitem.inspectorid", "ja-JP", "检验员ID_jp", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.iqcorderitem.inspectorid
            new TranslationSeedItem("entity.iqcorderitem.inspectorid", "zh-CN", "检验员ID", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.iqcorderitem.inspectorid
            new TranslationSeedItem("entity.iqcorderitem.inspectorid", "zh-HK", "检验员ID_hk", "检验员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.iqcorderitem.inspectorname
            new TranslationSeedItem("entity.iqcorderitem.inspectorname", "en-US", "检验员名称_us", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.iqcorderitem.inspectorname
            new TranslationSeedItem("entity.iqcorderitem.inspectorname", "ja-JP", "检验员名称_jp", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.iqcorderitem.inspectorname
            new TranslationSeedItem("entity.iqcorderitem.inspectorname", "zh-CN", "检验员名称", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.iqcorderitem.inspectorname
            new TranslationSeedItem("entity.iqcorderitem.inspectorname", "zh-HK", "检验员名称_hk", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.iqcorderitem.inspectiondate
            new TranslationSeedItem("entity.iqcorderitem.inspectiondate", "en-US", "检验日期_us", "检验日期"),
            // entity.iqcorderitem.inspectiondate
            new TranslationSeedItem("entity.iqcorderitem.inspectiondate", "ja-JP", "检验日期_jp", "检验日期"),
            // entity.iqcorderitem.inspectiondate
            new TranslationSeedItem("entity.iqcorderitem.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.iqcorderitem.inspectiondate
            new TranslationSeedItem("entity.iqcorderitem.inspectiondate", "zh-HK", "检验日期_hk", "检验日期"),

            // entity.iqcorderitem.judgestatus
            new TranslationSeedItem("entity.iqcorderitem.judgestatus", "en-US", "判定状态_us", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcorderitem.judgestatus
            new TranslationSeedItem("entity.iqcorderitem.judgestatus", "ja-JP", "判定状态_jp", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcorderitem.judgestatus
            new TranslationSeedItem("entity.iqcorderitem.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcorderitem.judgestatus
            new TranslationSeedItem("entity.iqcorderitem.judgestatus", "zh-HK", "判定状态_hk", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.iqcorderitem.isobsolete
            new TranslationSeedItem("entity.iqcorderitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.iqcorderitem.isobsolete
            new TranslationSeedItem("entity.iqcorderitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.iqcorderitem.isobsolete
            new TranslationSeedItem("entity.iqcorderitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.iqcorderitem.isobsolete
            new TranslationSeedItem("entity.iqcorderitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.iqcorderitem.order
            new TranslationSeedItem("entity.iqcorderitem.order", "en-US", "IQC检验单_us", "IQC检验单（主表）"),
            // entity.iqcorderitem.order
            new TranslationSeedItem("entity.iqcorderitem.order", "ja-JP", "IQC检验单_jp", "IQC检验单（主表）"),
            // entity.iqcorderitem.order
            new TranslationSeedItem("entity.iqcorderitem.order", "zh-CN", "IQC检验单", "IQC检验单（主表）"),
            // entity.iqcorderitem.order
            new TranslationSeedItem("entity.iqcorderitem.order", "zh-HK", "IQC检验单_hk", "IQC检验单（主表）"),

            // entity.iqcorderitem.defecthandlings
            new TranslationSeedItem("entity.iqcorderitem.defecthandlings", "en-US", "不良处理记录列表_us", "不良处理记录列表（主子表关系）"),
            // entity.iqcorderitem.defecthandlings
            new TranslationSeedItem("entity.iqcorderitem.defecthandlings", "ja-JP", "不良处理记录列表_jp", "不良处理记录列表（主子表关系）"),
            // entity.iqcorderitem.defecthandlings
            new TranslationSeedItem("entity.iqcorderitem.defecthandlings", "zh-CN", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
            // entity.iqcorderitem.defecthandlings
            new TranslationSeedItem("entity.iqcorderitem.defecthandlings", "zh-HK", "不良处理记录列表_hk", "不良处理记录列表（主子表关系）"),
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
        translation.ResourceGroup = "Operation";
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
