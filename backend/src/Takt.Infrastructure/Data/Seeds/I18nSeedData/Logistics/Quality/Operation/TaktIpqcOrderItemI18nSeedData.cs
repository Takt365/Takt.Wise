// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIpqcOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktIpqcOrderItem 实体国际化翻译种子（键前缀 entity.ipqcorderitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIpqcOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIpqcOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ipqcorderitem 实体翻译...", tenantCode);

        foreach (var item in GetIpqcOrderItemTranslations())
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

        TaktLogger.Information("TaktIpqcOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIpqcOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ipqcorderitem._self / entity.ipqcorderitem.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIpqcOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ipqcorderitem._self
            new TranslationSeedItem("entity.ipqcorderitem._self", "en-US", "Ipqc Order Item Information_us", "实体名称"),
            // entity.ipqcorderitem._self
            new TranslationSeedItem("entity.ipqcorderitem._self", "ja-JP", "IPQC制程检验单明细信息_jp", "实体名称"),
            // entity.ipqcorderitem._self
            new TranslationSeedItem("entity.ipqcorderitem._self", "zh-CN", "IPQC制程检验单明细信息", "实体名称"),
            // entity.ipqcorderitem._self
            new TranslationSeedItem("entity.ipqcorderitem._self", "zh-HK", "IPQC制程检验单明细信息_hk", "实体名称"),

            // entity.ipqcorderitem.ipqcorderid
            new TranslationSeedItem("entity.ipqcorderitem.ipqcorderid", "en-US", "IPQC检验单ID_us", "IPQC检验单 ID（选项 TaktIpqcOrders/options，DictValue=Id）"),
            // entity.ipqcorderitem.ipqcorderid
            new TranslationSeedItem("entity.ipqcorderitem.ipqcorderid", "ja-JP", "IPQC检验单ID_jp", "IPQC检验单 ID（选项 TaktIpqcOrders/options，DictValue=Id）"),
            // entity.ipqcorderitem.ipqcorderid
            new TranslationSeedItem("entity.ipqcorderitem.ipqcorderid", "zh-CN", "IPQC检验单ID", "IPQC检验单 ID（选项 TaktIpqcOrders/options，DictValue=Id）"),
            // entity.ipqcorderitem.ipqcorderid
            new TranslationSeedItem("entity.ipqcorderitem.ipqcorderid", "zh-HK", "IPQC检验单ID_hk", "IPQC检验单 ID（选项 TaktIpqcOrders/options，DictValue=Id）"),

            // entity.ipqcorderitem.ipqcordercode
            new TranslationSeedItem("entity.ipqcorderitem.ipqcordercode", "en-US", "IPQC检验单编码_us", "IPQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ipqcorderitem.ipqcordercode
            new TranslationSeedItem("entity.ipqcorderitem.ipqcordercode", "ja-JP", "IPQC检验单编码_jp", "IPQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ipqcorderitem.ipqcordercode
            new TranslationSeedItem("entity.ipqcorderitem.ipqcordercode", "zh-CN", "IPQC检验单编码", "IPQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.ipqcorderitem.ipqcordercode
            new TranslationSeedItem("entity.ipqcorderitem.ipqcordercode", "zh-HK", "IPQC检验单编码_hk", "IPQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.ipqcorderitem.linenumber
            new TranslationSeedItem("entity.ipqcorderitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcorderitem.linenumber
            new TranslationSeedItem("entity.ipqcorderitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcorderitem.linenumber
            new TranslationSeedItem("entity.ipqcorderitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcorderitem.linenumber
            new TranslationSeedItem("entity.ipqcorderitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ipqcorderitem.materialcode
            new TranslationSeedItem("entity.ipqcorderitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.ipqcorderitem.materialcode
            new TranslationSeedItem("entity.ipqcorderitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.ipqcorderitem.materialcode
            new TranslationSeedItem("entity.ipqcorderitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.ipqcorderitem.materialcode
            new TranslationSeedItem("entity.ipqcorderitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.ipqcorderitem.materialdescription
            new TranslationSeedItem("entity.ipqcorderitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.ipqcorderitem.materialdescription
            new TranslationSeedItem("entity.ipqcorderitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.ipqcorderitem.materialdescription
            new TranslationSeedItem("entity.ipqcorderitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.ipqcorderitem.materialdescription
            new TranslationSeedItem("entity.ipqcorderitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.ipqcorderitem.batchcode
            new TranslationSeedItem("entity.ipqcorderitem.batchcode", "en-US", "批次号_us", "批次号"),
            // entity.ipqcorderitem.batchcode
            new TranslationSeedItem("entity.ipqcorderitem.batchcode", "ja-JP", "批次号_jp", "批次号"),
            // entity.ipqcorderitem.batchcode
            new TranslationSeedItem("entity.ipqcorderitem.batchcode", "zh-CN", "批次号", "批次号"),
            // entity.ipqcorderitem.batchcode
            new TranslationSeedItem("entity.ipqcorderitem.batchcode", "zh-HK", "批次号_hk", "批次号"),

            // entity.ipqcorderitem.productionquantity
            new TranslationSeedItem("entity.ipqcorderitem.productionquantity", "en-US", "生产数量_us", "生产数量"),
            // entity.ipqcorderitem.productionquantity
            new TranslationSeedItem("entity.ipqcorderitem.productionquantity", "ja-JP", "生产数量_jp", "生产数量"),
            // entity.ipqcorderitem.productionquantity
            new TranslationSeedItem("entity.ipqcorderitem.productionquantity", "zh-CN", "生产数量", "生产数量"),
            // entity.ipqcorderitem.productionquantity
            new TranslationSeedItem("entity.ipqcorderitem.productionquantity", "zh-HK", "生产数量_hk", "生产数量"),

            // entity.ipqcorderitem.standardcode
            new TranslationSeedItem("entity.ipqcorderitem.standardcode", "en-US", "检验标准编码_us", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.ipqcorderitem.standardcode
            new TranslationSeedItem("entity.ipqcorderitem.standardcode", "ja-JP", "检验标准编码_jp", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.ipqcorderitem.standardcode
            new TranslationSeedItem("entity.ipqcorderitem.standardcode", "zh-CN", "检验标准编码", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),
            // entity.ipqcorderitem.standardcode
            new TranslationSeedItem("entity.ipqcorderitem.standardcode", "zh-HK", "检验标准编码_hk", "检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）"),

            // entity.ipqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.ipqcorderitem.samplingschemecode", "en-US", "抽样方案编码_us", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.ipqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.ipqcorderitem.samplingschemecode", "ja-JP", "抽样方案编码_jp", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.ipqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.ipqcorderitem.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),
            // entity.ipqcorderitem.samplingschemecode
            new TranslationSeedItem("entity.ipqcorderitem.samplingschemecode", "zh-HK", "抽样方案编码_hk", "抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）"),

            // entity.ipqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.ipqcorderitem.inspectionmethod", "en-US", "检验方式_us", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.ipqcorderitem.inspectionmethod", "ja-JP", "检验方式_jp", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.ipqcorderitem.inspectionmethod", "zh-CN", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcorderitem.inspectionmethod
            new TranslationSeedItem("entity.ipqcorderitem.inspectionmethod", "zh-HK", "检验方式_hk", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),

            // entity.ipqcorderitem.samplequantity
            new TranslationSeedItem("entity.ipqcorderitem.samplequantity", "en-US", "抽样数量_us", "抽样数量"),
            // entity.ipqcorderitem.samplequantity
            new TranslationSeedItem("entity.ipqcorderitem.samplequantity", "ja-JP", "抽样数量_jp", "抽样数量"),
            // entity.ipqcorderitem.samplequantity
            new TranslationSeedItem("entity.ipqcorderitem.samplequantity", "zh-CN", "抽样数量", "抽样数量"),
            // entity.ipqcorderitem.samplequantity
            new TranslationSeedItem("entity.ipqcorderitem.samplequantity", "zh-HK", "抽样数量_hk", "抽样数量"),

            // entity.ipqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.qualifiedquantity", "en-US", "合格数量_us", "合格数量"),
            // entity.ipqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.qualifiedquantity", "ja-JP", "合格数量_jp", "合格数量"),
            // entity.ipqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.qualifiedquantity", "zh-CN", "合格数量", "合格数量"),
            // entity.ipqcorderitem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.qualifiedquantity", "zh-HK", "合格数量_hk", "合格数量"),

            // entity.ipqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.unqualifiedquantity", "en-US", "不合格数量_us", "不合格数量"),
            // entity.ipqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.unqualifiedquantity", "ja-JP", "不合格数量_jp", "不合格数量"),
            // entity.ipqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.unqualifiedquantity", "zh-CN", "不合格数量", "不合格数量"),
            // entity.ipqcorderitem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcorderitem.unqualifiedquantity", "zh-HK", "不合格数量_hk", "不合格数量"),

            // entity.ipqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorderitem.inspectionreturnquantity", "en-US", "验退数量_us", "验退数量"),
            // entity.ipqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorderitem.inspectionreturnquantity", "ja-JP", "验退数量_jp", "验退数量"),
            // entity.ipqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorderitem.inspectionreturnquantity", "zh-CN", "验退数量", "验退数量"),
            // entity.ipqcorderitem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorderitem.inspectionreturnquantity", "zh-HK", "验退数量_hk", "验退数量"),

            // entity.ipqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.ipqcorderitem.sampleserialcode", "en-US", "抽检序列号_us", "抽检序列号"),
            // entity.ipqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.ipqcorderitem.sampleserialcode", "ja-JP", "抽检序列号_jp", "抽检序列号"),
            // entity.ipqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.ipqcorderitem.sampleserialcode", "zh-CN", "抽检序列号", "抽检序列号"),
            // entity.ipqcorderitem.sampleserialcode
            new TranslationSeedItem("entity.ipqcorderitem.sampleserialcode", "zh-HK", "抽检序列号_hk", "抽检序列号"),

            // entity.ipqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondescription", "en-US", "检验说明_us", "检验说明"),
            // entity.ipqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondescription", "ja-JP", "检验说明_jp", "检验说明"),
            // entity.ipqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondescription", "zh-CN", "检验说明", "检验说明"),
            // entity.ipqcorderitem.inspectiondescription
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondescription", "zh-HK", "检验说明_hk", "检验说明"),

            // entity.ipqcorderitem.inspectorid
            new TranslationSeedItem("entity.ipqcorderitem.inspectorid", "en-US", "检验员ID_us", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorderitem.inspectorid
            new TranslationSeedItem("entity.ipqcorderitem.inspectorid", "ja-JP", "检验员ID_jp", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorderitem.inspectorid
            new TranslationSeedItem("entity.ipqcorderitem.inspectorid", "zh-CN", "检验员ID", "检验员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorderitem.inspectorid
            new TranslationSeedItem("entity.ipqcorderitem.inspectorid", "zh-HK", "检验员ID_hk", "检验员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.ipqcorderitem.inspectorname
            new TranslationSeedItem("entity.ipqcorderitem.inspectorname", "en-US", "检验员名称_us", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorderitem.inspectorname
            new TranslationSeedItem("entity.ipqcorderitem.inspectorname", "ja-JP", "检验员名称_jp", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorderitem.inspectorname
            new TranslationSeedItem("entity.ipqcorderitem.inspectorname", "zh-CN", "检验员名称", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorderitem.inspectorname
            new TranslationSeedItem("entity.ipqcorderitem.inspectorname", "zh-HK", "检验员名称_hk", "检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.ipqcorderitem.inspectiondate
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondate", "en-US", "检验日期_us", "检验日期"),
            // entity.ipqcorderitem.inspectiondate
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondate", "ja-JP", "检验日期_jp", "检验日期"),
            // entity.ipqcorderitem.inspectiondate
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.ipqcorderitem.inspectiondate
            new TranslationSeedItem("entity.ipqcorderitem.inspectiondate", "zh-HK", "检验日期_hk", "检验日期"),

            // entity.ipqcorderitem.judgestatus
            new TranslationSeedItem("entity.ipqcorderitem.judgestatus", "en-US", "判定状态_us", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorderitem.judgestatus
            new TranslationSeedItem("entity.ipqcorderitem.judgestatus", "ja-JP", "判定状态_jp", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorderitem.judgestatus
            new TranslationSeedItem("entity.ipqcorderitem.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorderitem.judgestatus
            new TranslationSeedItem("entity.ipqcorderitem.judgestatus", "zh-HK", "判定状态_hk", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),

            // entity.ipqcorderitem.isobsolete
            new TranslationSeedItem("entity.ipqcorderitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcorderitem.isobsolete
            new TranslationSeedItem("entity.ipqcorderitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcorderitem.isobsolete
            new TranslationSeedItem("entity.ipqcorderitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcorderitem.isobsolete
            new TranslationSeedItem("entity.ipqcorderitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ipqcorderitem.order
            new TranslationSeedItem("entity.ipqcorderitem.order", "en-US", "IPQC检验单_us", "IPQC检验单（主表）"),
            // entity.ipqcorderitem.order
            new TranslationSeedItem("entity.ipqcorderitem.order", "ja-JP", "IPQC检验单_jp", "IPQC检验单（主表）"),
            // entity.ipqcorderitem.order
            new TranslationSeedItem("entity.ipqcorderitem.order", "zh-CN", "IPQC检验单", "IPQC检验单（主表）"),
            // entity.ipqcorderitem.order
            new TranslationSeedItem("entity.ipqcorderitem.order", "zh-HK", "IPQC检验单_hk", "IPQC检验单（主表）"),

            // entity.ipqcorderitem.defecthandlings
            new TranslationSeedItem("entity.ipqcorderitem.defecthandlings", "en-US", "不良处理记录列表_us", "不良处理记录列表（主子表关系）"),
            // entity.ipqcorderitem.defecthandlings
            new TranslationSeedItem("entity.ipqcorderitem.defecthandlings", "ja-JP", "不良处理记录列表_jp", "不良处理记录列表（主子表关系）"),
            // entity.ipqcorderitem.defecthandlings
            new TranslationSeedItem("entity.ipqcorderitem.defecthandlings", "zh-CN", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
            // entity.ipqcorderitem.defecthandlings
            new TranslationSeedItem("entity.ipqcorderitem.defecthandlings", "zh-HK", "不良处理记录列表_hk", "不良处理记录列表（主子表关系）"),
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
