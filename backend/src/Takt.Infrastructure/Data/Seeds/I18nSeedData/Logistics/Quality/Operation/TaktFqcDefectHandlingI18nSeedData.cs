// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktFqcDefectHandlingI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFqcDefectHandling 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFqcDefectHandling 实体国际化翻译种子（键前缀 entity.fqcdefecthandling.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFqcDefectHandlingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFqcDefectHandling 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 fqcdefecthandling 实体翻译...", tenantCode);

        foreach (var item in GetFqcDefectHandlingTranslations())
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

        TaktLogger.Information("TaktFqcDefectHandling 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFqcDefectHandling 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.fqcdefecthandling._self / entity.fqcdefecthandling.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFqcDefectHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.fqcdefecthandling._self
            new TranslationSeedItem("entity.fqcdefecthandling._self", "en-US", "Fqc Defect Handling Information_us", "实体名称"),
            // entity.fqcdefecthandling._self
            new TranslationSeedItem("entity.fqcdefecthandling._self", "ja-JP", "FQC出货检验不良处理记录信息_jp", "实体名称"),
            // entity.fqcdefecthandling._self
            new TranslationSeedItem("entity.fqcdefecthandling._self", "zh-CN", "FQC出货检验不良处理记录信息", "实体名称"),
            // entity.fqcdefecthandling._self
            new TranslationSeedItem("entity.fqcdefecthandling._self", "zh-HK", "FQC出货检验不良处理记录信息_hk", "实体名称"),

            // entity.fqcdefecthandling.code
            new TranslationSeedItem("entity.fqcdefecthandling.code", "en-US", "FQC不良处理编码_us", "FQC不良处理编码（前端表单选择编码规则后自动通过 TaktNumbering FQC不良处理编码规则生成并展示，非手输；单据类型菜单：入库检验）"),
            // entity.fqcdefecthandling.code
            new TranslationSeedItem("entity.fqcdefecthandling.code", "ja-JP", "FQC不良处理编码_jp", "FQC不良处理编码（前端表单选择编码规则后自动通过 TaktNumbering FQC不良处理编码规则生成并展示，非手输；单据类型菜单：入库检验）"),
            // entity.fqcdefecthandling.code
            new TranslationSeedItem("entity.fqcdefecthandling.code", "zh-CN", "FQC不良处理编码", "FQC不良处理编码（前端表单选择编码规则后自动通过 TaktNumbering FQC不良处理编码规则生成并展示，非手输；单据类型菜单：入库检验）"),
            // entity.fqcdefecthandling.code
            new TranslationSeedItem("entity.fqcdefecthandling.code", "zh-HK", "FQC不良处理编码_hk", "FQC不良处理编码（前端表单选择编码规则后自动通过 TaktNumbering FQC不良处理编码规则生成并展示，非手输；单据类型菜单：入库检验）"),

            // entity.fqcdefecthandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcdefecthandling.fqcorderitemid", "en-US", "FQC检验单明细ID_us", "FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）"),
            // entity.fqcdefecthandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcdefecthandling.fqcorderitemid", "ja-JP", "FQC检验单明细ID_jp", "FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）"),
            // entity.fqcdefecthandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcdefecthandling.fqcorderitemid", "zh-CN", "FQC检验单明细ID", "FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）"),
            // entity.fqcdefecthandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcdefecthandling.fqcorderitemid", "zh-HK", "FQC检验单明细ID_hk", "FQC检验单明细 ID（选项 TaktFqcOrderItems/options；DictValue=Id）"),

            // entity.fqcdefecthandling.fqcordercode
            new TranslationSeedItem("entity.fqcdefecthandling.fqcordercode", "en-US", "FQC检验单编码_us", "FQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.fqcdefecthandling.fqcordercode
            new TranslationSeedItem("entity.fqcdefecthandling.fqcordercode", "ja-JP", "FQC检验单编码_jp", "FQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.fqcdefecthandling.fqcordercode
            new TranslationSeedItem("entity.fqcdefecthandling.fqcordercode", "zh-CN", "FQC检验单编码", "FQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.fqcdefecthandling.fqcordercode
            new TranslationSeedItem("entity.fqcdefecthandling.fqcordercode", "zh-HK", "FQC检验单编码_hk", "FQC检验单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.fqcdefecthandling.linenumber
            new TranslationSeedItem("entity.fqcdefecthandling.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.fqcdefecthandling.linenumber
            new TranslationSeedItem("entity.fqcdefecthandling.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.fqcdefecthandling.linenumber
            new TranslationSeedItem("entity.fqcdefecthandling.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcdefecthandling.linenumber
            new TranslationSeedItem("entity.fqcdefecthandling.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.fqcdefecthandling.defecttype
            new TranslationSeedItem("entity.fqcdefecthandling.defecttype", "en-US", "不良类型_us", "不良类型（字典 logistics_quality_defect_type）"),
            // entity.fqcdefecthandling.defecttype
            new TranslationSeedItem("entity.fqcdefecthandling.defecttype", "ja-JP", "不良类型_jp", "不良类型（字典 logistics_quality_defect_type）"),
            // entity.fqcdefecthandling.defecttype
            new TranslationSeedItem("entity.fqcdefecthandling.defecttype", "zh-CN", "不良类型", "不良类型（字典 logistics_quality_defect_type）"),
            // entity.fqcdefecthandling.defecttype
            new TranslationSeedItem("entity.fqcdefecthandling.defecttype", "zh-HK", "不良类型_hk", "不良类型（字典 logistics_quality_defect_type）"),

            // entity.fqcdefecthandling.defectcode
            new TranslationSeedItem("entity.fqcdefecthandling.defectcode", "en-US", "不良现象编码_us", "不良现象编码"),
            // entity.fqcdefecthandling.defectcode
            new TranslationSeedItem("entity.fqcdefecthandling.defectcode", "ja-JP", "不良现象编码_jp", "不良现象编码"),
            // entity.fqcdefecthandling.defectcode
            new TranslationSeedItem("entity.fqcdefecthandling.defectcode", "zh-CN", "不良现象编码", "不良现象编码"),
            // entity.fqcdefecthandling.defectcode
            new TranslationSeedItem("entity.fqcdefecthandling.defectcode", "zh-HK", "不良现象编码_hk", "不良现象编码"),

            // entity.fqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.fqcdefecthandling.defectdescription", "en-US", "不良现象描述_us", "不良现象描述"),
            // entity.fqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.fqcdefecthandling.defectdescription", "ja-JP", "不良现象描述_jp", "不良现象描述"),
            // entity.fqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.fqcdefecthandling.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.fqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.fqcdefecthandling.defectdescription", "zh-HK", "不良现象描述_hk", "不良现象描述"),

            // entity.fqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.fqcdefecthandling.defectquantity", "en-US", "不良数量_us", "不良数量"),
            // entity.fqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.fqcdefecthandling.defectquantity", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.fqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.fqcdefecthandling.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.fqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.fqcdefecthandling.defectquantity", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.fqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.fqcdefecthandling.handlingmethod", "en-US", "处理方式_us", "处理方式（字典 logistics_quality_defect_handling_method）"),
            // entity.fqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.fqcdefecthandling.handlingmethod", "ja-JP", "处理方式_jp", "处理方式（字典 logistics_quality_defect_handling_method）"),
            // entity.fqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.fqcdefecthandling.handlingmethod", "zh-CN", "处理方式", "处理方式（字典 logistics_quality_defect_handling_method）"),
            // entity.fqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.fqcdefecthandling.handlingmethod", "zh-HK", "处理方式_hk", "处理方式（字典 logistics_quality_defect_handling_method）"),

            // entity.fqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.fqcdefecthandling.handlingdescription", "en-US", "处理说明_us", "处理说明"),
            // entity.fqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.fqcdefecthandling.handlingdescription", "ja-JP", "处理说明_jp", "处理说明"),
            // entity.fqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.fqcdefecthandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.fqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.fqcdefecthandling.handlingdescription", "zh-HK", "处理说明_hk", "处理说明"),

            // entity.fqcdefecthandling.responsibledeptid
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptid", "en-US", "责任部门ID_us", "责任部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.responsibledeptid
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptid", "ja-JP", "责任部门ID_jp", "责任部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.responsibledeptid
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptid", "zh-CN", "责任部门ID", "责任部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.responsibledeptid
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptid", "zh-HK", "责任部门ID_hk", "责任部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.fqcdefecthandling.responsibledeptname
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptname", "en-US", "责任部门名称_us", "责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.responsibledeptname
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptname", "ja-JP", "责任部门名称_jp", "责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.responsibledeptname
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptname", "zh-CN", "责任部门名称", "责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.responsibledeptname
            new TranslationSeedItem("entity.fqcdefecthandling.responsibledeptname", "zh-HK", "责任部门名称_hk", "责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.fqcdefecthandling.responsiblegroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupcode", "en-US", "责任组编码_us", "责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.responsiblegroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupcode", "ja-JP", "责任组编码_jp", "责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.responsiblegroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupcode", "zh-CN", "责任组编码", "责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.responsiblegroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupcode", "zh-HK", "责任组编码_hk", "责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）"),

            // entity.fqcdefecthandling.responsiblegroupname
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupname", "en-US", "责任组名称_us", "责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.responsiblegroupname
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupname", "ja-JP", "责任组名称_jp", "责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.responsiblegroupname
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupname", "zh-CN", "责任组名称", "责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.responsiblegroupname
            new TranslationSeedItem("entity.fqcdefecthandling.responsiblegroupname", "zh-HK", "责任组名称_hk", "责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),

            // entity.fqcdefecthandling.handlergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupcode", "en-US", "处理组编码_us", "处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.handlergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupcode", "ja-JP", "处理组编码_jp", "处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.handlergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupcode", "zh-CN", "处理组编码", "处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.handlergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupcode", "zh-HK", "处理组编码_hk", "处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）"),

            // entity.fqcdefecthandling.handlergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupname", "en-US", "处理组名称_us", "处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.handlergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupname", "ja-JP", "处理组名称_jp", "处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.handlergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupname", "zh-CN", "处理组名称", "处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.handlergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.handlergroupname", "zh-HK", "处理组名称_hk", "处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),

            // entity.fqcdefecthandling.handlingat
            new TranslationSeedItem("entity.fqcdefecthandling.handlingat", "en-US", "处理时间_us", "处理时间"),
            // entity.fqcdefecthandling.handlingat
            new TranslationSeedItem("entity.fqcdefecthandling.handlingat", "ja-JP", "处理时间_jp", "处理时间"),
            // entity.fqcdefecthandling.handlingat
            new TranslationSeedItem("entity.fqcdefecthandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.fqcdefecthandling.handlingat
            new TranslationSeedItem("entity.fqcdefecthandling.handlingat", "zh-HK", "处理时间_hk", "处理时间"),

            // entity.fqcdefecthandling.verifydeptid
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptid", "en-US", "验证部门ID_us", "验证部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.verifydeptid
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptid", "ja-JP", "验证部门ID_jp", "验证部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.verifydeptid
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptid", "zh-CN", "验证部门ID", "验证部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.fqcdefecthandling.verifydeptid
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptid", "zh-HK", "验证部门ID_hk", "验证部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.fqcdefecthandling.verifydeptname
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptname", "en-US", "验证部门名称_us", "验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.verifydeptname
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptname", "ja-JP", "验证部门名称_jp", "验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.verifydeptname
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptname", "zh-CN", "验证部门名称", "验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）"),
            // entity.fqcdefecthandling.verifydeptname
            new TranslationSeedItem("entity.fqcdefecthandling.verifydeptname", "zh-HK", "验证部门名称_hk", "验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）"),

            // entity.fqcdefecthandling.qualityownergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupcode", "en-US", "质量负责人组编码_us", "质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.qualityownergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupcode", "ja-JP", "质量负责人组编码_jp", "质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.qualityownergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupcode", "zh-CN", "质量负责人组编码", "质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）"),
            // entity.fqcdefecthandling.qualityownergroupcode
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupcode", "zh-HK", "质量负责人组编码_hk", "质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）"),

            // entity.fqcdefecthandling.qualityownergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupname", "en-US", "质量负责人组名称_us", "质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.qualityownergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupname", "ja-JP", "质量负责人组名称_jp", "质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.qualityownergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupname", "zh-CN", "质量负责人组名称", "质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),
            // entity.fqcdefecthandling.qualityownergroupname
            new TranslationSeedItem("entity.fqcdefecthandling.qualityownergroupname", "zh-HK", "质量负责人组名称_hk", "质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）"),

            // entity.fqcdefecthandling.verifyat
            new TranslationSeedItem("entity.fqcdefecthandling.verifyat", "en-US", "验证时间_us", "验证时间"),
            // entity.fqcdefecthandling.verifyat
            new TranslationSeedItem("entity.fqcdefecthandling.verifyat", "ja-JP", "验证时间_jp", "验证时间"),
            // entity.fqcdefecthandling.verifyat
            new TranslationSeedItem("entity.fqcdefecthandling.verifyat", "zh-CN", "验证时间", "验证时间"),
            // entity.fqcdefecthandling.verifyat
            new TranslationSeedItem("entity.fqcdefecthandling.verifyat", "zh-HK", "验证时间_hk", "验证时间"),

            // entity.fqcdefecthandling.verifystatus
            new TranslationSeedItem("entity.fqcdefecthandling.verifystatus", "en-US", "验证结果_us", "验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）"),
            // entity.fqcdefecthandling.verifystatus
            new TranslationSeedItem("entity.fqcdefecthandling.verifystatus", "ja-JP", "验证结果_jp", "验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）"),
            // entity.fqcdefecthandling.verifystatus
            new TranslationSeedItem("entity.fqcdefecthandling.verifystatus", "zh-CN", "验证结果", "验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）"),
            // entity.fqcdefecthandling.verifystatus
            new TranslationSeedItem("entity.fqcdefecthandling.verifystatus", "zh-HK", "验证结果_hk", "验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）"),

            // entity.fqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.fqcdefecthandling.correctiveaction", "en-US", "纠正措施_us", "预防措施/纠正措施"),
            // entity.fqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.fqcdefecthandling.correctiveaction", "ja-JP", "纠正措施_jp", "预防措施/纠正措施"),
            // entity.fqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.fqcdefecthandling.correctiveaction", "zh-CN", "纠正措施", "预防措施/纠正措施"),
            // entity.fqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.fqcdefecthandling.correctiveaction", "zh-HK", "纠正措施_hk", "预防措施/纠正措施"),

            // entity.fqcdefecthandling.defectimages
            new TranslationSeedItem("entity.fqcdefecthandling.defectimages", "en-US", "不良图片_us", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcdefecthandling.defectimages
            new TranslationSeedItem("entity.fqcdefecthandling.defectimages", "ja-JP", "不良图片_jp", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcdefecthandling.defectimages
            new TranslationSeedItem("entity.fqcdefecthandling.defectimages", "zh-CN", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcdefecthandling.defectimages
            new TranslationSeedItem("entity.fqcdefecthandling.defectimages", "zh-HK", "不良图片_hk", "不良图片（JSON格式，存储不良图片URL列表）"),

            // entity.fqcdefecthandling.attachments
            new TranslationSeedItem("entity.fqcdefecthandling.attachments", "en-US", "附件JSON_us", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.fqcdefecthandling.attachments
            new TranslationSeedItem("entity.fqcdefecthandling.attachments", "ja-JP", "附件JSON_jp", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.fqcdefecthandling.attachments
            new TranslationSeedItem("entity.fqcdefecthandling.attachments", "zh-CN", "附件JSON", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.fqcdefecthandling.attachments
            new TranslationSeedItem("entity.fqcdefecthandling.attachments", "zh-HK", "附件JSON_hk", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),

            // entity.fqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.fqcdefecthandling.handlingstatus", "en-US", "处理状态_us", "处理状态（字典 logistics_quality_defect_handling_status）"),
            // entity.fqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.fqcdefecthandling.handlingstatus", "ja-JP", "处理状态_jp", "处理状态（字典 logistics_quality_defect_handling_status）"),
            // entity.fqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.fqcdefecthandling.handlingstatus", "zh-CN", "处理状态", "处理状态（字典 logistics_quality_defect_handling_status）"),
            // entity.fqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.fqcdefecthandling.handlingstatus", "zh-HK", "处理状态_hk", "处理状态（字典 logistics_quality_defect_handling_status）"),

            // entity.fqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.fqcdefecthandling.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.fqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.fqcdefecthandling.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.fqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.fqcdefecthandling.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.fqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.fqcdefecthandling.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.fqcdefecthandling.orderitem
            new TranslationSeedItem("entity.fqcdefecthandling.orderitem", "en-US", "FQC检验单明细_us", "FQC检验单明细（主表）"),
            // entity.fqcdefecthandling.orderitem
            new TranslationSeedItem("entity.fqcdefecthandling.orderitem", "ja-JP", "FQC检验单明细_jp", "FQC检验单明细（主表）"),
            // entity.fqcdefecthandling.orderitem
            new TranslationSeedItem("entity.fqcdefecthandling.orderitem", "zh-CN", "FQC检验单明细", "FQC检验单明细（主表）"),
            // entity.fqcdefecthandling.orderitem
            new TranslationSeedItem("entity.fqcdefecthandling.orderitem", "zh-HK", "FQC检验单明细_hk", "FQC检验单明细（主表）"),
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
