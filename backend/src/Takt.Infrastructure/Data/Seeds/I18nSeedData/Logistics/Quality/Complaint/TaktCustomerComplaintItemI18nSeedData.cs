// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerComplaintItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaintItem 实体国际化翻译种子（键前缀 entity.customercomplaintitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerComplaintItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerComplaintItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customercomplaintitem 实体翻译...", tenantCode);

        foreach (var item in GetCustomerComplaintItemTranslations())
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

        TaktLogger.Information("TaktCustomerComplaintItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerComplaintItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customercomplaintitem._self / entity.customercomplaintitem.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customercomplaintitem._self
            new TranslationSeedItem("entity.customercomplaintitem._self", "en-US", "Customer Complaint Item Information_us", "实体名称"),
            // entity.customercomplaintitem._self
            new TranslationSeedItem("entity.customercomplaintitem._self", "ja-JP", "客诉明细信息_jp", "实体名称"),
            // entity.customercomplaintitem._self
            new TranslationSeedItem("entity.customercomplaintitem._self", "zh-CN", "客诉明细信息", "实体名称"),
            // entity.customercomplaintitem._self
            new TranslationSeedItem("entity.customercomplaintitem._self", "zh-HK", "客诉明细信息_hk", "实体名称"),

            // entity.customercomplaintitem.complaintid
            new TranslationSeedItem("entity.customercomplaintitem.complaintid", "en-US", "客诉ID_us", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplaintitem.complaintid
            new TranslationSeedItem("entity.customercomplaintitem.complaintid", "ja-JP", "客诉ID_jp", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplaintitem.complaintid
            new TranslationSeedItem("entity.customercomplaintitem.complaintid", "zh-CN", "客诉ID", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),
            // entity.customercomplaintitem.complaintid
            new TranslationSeedItem("entity.customercomplaintitem.complaintid", "zh-HK", "客诉ID_hk", "客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）"),

            // entity.customercomplaintitem.customercomplaintcode
            new TranslationSeedItem("entity.customercomplaintitem.customercomplaintcode", "en-US", "客诉单号_us", "客诉单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customercomplaintitem.customercomplaintcode
            new TranslationSeedItem("entity.customercomplaintitem.customercomplaintcode", "ja-JP", "客诉单号_jp", "客诉单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customercomplaintitem.customercomplaintcode
            new TranslationSeedItem("entity.customercomplaintitem.customercomplaintcode", "zh-CN", "客诉单号", "客诉单号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customercomplaintitem.customercomplaintcode
            new TranslationSeedItem("entity.customercomplaintitem.customercomplaintcode", "zh-HK", "客诉单号_hk", "客诉单号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customercomplaintitem.linenumber
            new TranslationSeedItem("entity.customercomplaintitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.customercomplaintitem.linenumber
            new TranslationSeedItem("entity.customercomplaintitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.customercomplaintitem.linenumber
            new TranslationSeedItem("entity.customercomplaintitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customercomplaintitem.linenumber
            new TranslationSeedItem("entity.customercomplaintitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.customercomplaintitem.productcode
            new TranslationSeedItem("entity.customercomplaintitem.productcode", "en-US", "产品编码_us", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.customercomplaintitem.productcode
            new TranslationSeedItem("entity.customercomplaintitem.productcode", "ja-JP", "产品编码_jp", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.customercomplaintitem.productcode
            new TranslationSeedItem("entity.customercomplaintitem.productcode", "zh-CN", "产品编码", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.customercomplaintitem.productcode
            new TranslationSeedItem("entity.customercomplaintitem.productcode", "zh-HK", "产品编码_hk", "产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.customercomplaintitem.productname
            new TranslationSeedItem("entity.customercomplaintitem.productname", "en-US", "产品名称_us", "产品名称"),
            // entity.customercomplaintitem.productname
            new TranslationSeedItem("entity.customercomplaintitem.productname", "ja-JP", "产品名称_jp", "产品名称"),
            // entity.customercomplaintitem.productname
            new TranslationSeedItem("entity.customercomplaintitem.productname", "zh-CN", "产品名称", "产品名称"),
            // entity.customercomplaintitem.productname
            new TranslationSeedItem("entity.customercomplaintitem.productname", "zh-HK", "产品名称_hk", "产品名称"),

            // entity.customercomplaintitem.batchcode
            new TranslationSeedItem("entity.customercomplaintitem.batchcode", "en-US", "批次号_us", "批次号"),
            // entity.customercomplaintitem.batchcode
            new TranslationSeedItem("entity.customercomplaintitem.batchcode", "ja-JP", "批次号_jp", "批次号"),
            // entity.customercomplaintitem.batchcode
            new TranslationSeedItem("entity.customercomplaintitem.batchcode", "zh-CN", "批次号", "批次号"),
            // entity.customercomplaintitem.batchcode
            new TranslationSeedItem("entity.customercomplaintitem.batchcode", "zh-HK", "批次号_hk", "批次号"),

            // entity.customercomplaintitem.itemtype
            new TranslationSeedItem("entity.customercomplaintitem.itemtype", "en-US", "不良项目类型_us", "不良项目类型（字典 logistics_quality_complaint_item_type）"),
            // entity.customercomplaintitem.itemtype
            new TranslationSeedItem("entity.customercomplaintitem.itemtype", "ja-JP", "不良项目类型_jp", "不良项目类型（字典 logistics_quality_complaint_item_type）"),
            // entity.customercomplaintitem.itemtype
            new TranslationSeedItem("entity.customercomplaintitem.itemtype", "zh-CN", "不良项目类型", "不良项目类型（字典 logistics_quality_complaint_item_type）"),
            // entity.customercomplaintitem.itemtype
            new TranslationSeedItem("entity.customercomplaintitem.itemtype", "zh-HK", "不良项目类型_hk", "不良项目类型（字典 logistics_quality_complaint_item_type）"),

            // entity.customercomplaintitem.defectdescription
            new TranslationSeedItem("entity.customercomplaintitem.defectdescription", "en-US", "不良现象描述_us", "不良现象描述"),
            // entity.customercomplaintitem.defectdescription
            new TranslationSeedItem("entity.customercomplaintitem.defectdescription", "ja-JP", "不良现象描述_jp", "不良现象描述"),
            // entity.customercomplaintitem.defectdescription
            new TranslationSeedItem("entity.customercomplaintitem.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.customercomplaintitem.defectdescription
            new TranslationSeedItem("entity.customercomplaintitem.defectdescription", "zh-HK", "不良现象描述_hk", "不良现象描述"),

            // entity.customercomplaintitem.defectlevel
            new TranslationSeedItem("entity.customercomplaintitem.defectlevel", "en-US", "缺点等级_us", "缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）"),
            // entity.customercomplaintitem.defectlevel
            new TranslationSeedItem("entity.customercomplaintitem.defectlevel", "ja-JP", "缺点等级_jp", "缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）"),
            // entity.customercomplaintitem.defectlevel
            new TranslationSeedItem("entity.customercomplaintitem.defectlevel", "zh-CN", "缺点等级", "缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）"),
            // entity.customercomplaintitem.defectlevel
            new TranslationSeedItem("entity.customercomplaintitem.defectlevel", "zh-HK", "缺点等级_hk", "缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）"),

            // entity.customercomplaintitem.defectquantity
            new TranslationSeedItem("entity.customercomplaintitem.defectquantity", "en-US", "不良数量_us", "不良数量"),
            // entity.customercomplaintitem.defectquantity
            new TranslationSeedItem("entity.customercomplaintitem.defectquantity", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.customercomplaintitem.defectquantity
            new TranslationSeedItem("entity.customercomplaintitem.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.customercomplaintitem.defectquantity
            new TranslationSeedItem("entity.customercomplaintitem.defectquantity", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.customercomplaintitem.defectrate
            new TranslationSeedItem("entity.customercomplaintitem.defectrate", "en-US", "不良率_us", "不良率（%）"),
            // entity.customercomplaintitem.defectrate
            new TranslationSeedItem("entity.customercomplaintitem.defectrate", "ja-JP", "不良率_jp", "不良率（%）"),
            // entity.customercomplaintitem.defectrate
            new TranslationSeedItem("entity.customercomplaintitem.defectrate", "zh-CN", "不良率", "不良率（%）"),
            // entity.customercomplaintitem.defectrate
            new TranslationSeedItem("entity.customercomplaintitem.defectrate", "zh-HK", "不良率_hk", "不良率（%）"),

            // entity.customercomplaintitem.causeanalysis
            new TranslationSeedItem("entity.customercomplaintitem.causeanalysis", "en-US", "原因分析_us", "原因分析"),
            // entity.customercomplaintitem.causeanalysis
            new TranslationSeedItem("entity.customercomplaintitem.causeanalysis", "ja-JP", "原因分析_jp", "原因分析"),
            // entity.customercomplaintitem.causeanalysis
            new TranslationSeedItem("entity.customercomplaintitem.causeanalysis", "zh-CN", "原因分析", "原因分析"),
            // entity.customercomplaintitem.causeanalysis
            new TranslationSeedItem("entity.customercomplaintitem.causeanalysis", "zh-HK", "原因分析_hk", "原因分析"),

            // entity.customercomplaintitem.improvementaction
            new TranslationSeedItem("entity.customercomplaintitem.improvementaction", "en-US", "改善对策_us", "改善对策"),
            // entity.customercomplaintitem.improvementaction
            new TranslationSeedItem("entity.customercomplaintitem.improvementaction", "ja-JP", "改善对策_jp", "改善对策"),
            // entity.customercomplaintitem.improvementaction
            new TranslationSeedItem("entity.customercomplaintitem.improvementaction", "zh-CN", "改善对策", "改善对策"),
            // entity.customercomplaintitem.improvementaction
            new TranslationSeedItem("entity.customercomplaintitem.improvementaction", "zh-HK", "改善对策_hk", "改善对策"),

            // entity.customercomplaintitem.improvementresponsiblegroupcode
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupcode", "en-US", "改善组编码_us", "改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）"),
            // entity.customercomplaintitem.improvementresponsiblegroupcode
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupcode", "ja-JP", "改善组编码_jp", "改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）"),
            // entity.customercomplaintitem.improvementresponsiblegroupcode
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupcode", "zh-CN", "改善组编码", "改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）"),
            // entity.customercomplaintitem.improvementresponsiblegroupcode
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupcode", "zh-HK", "改善组编码_hk", "改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）"),

            // entity.customercomplaintitem.improvementresponsiblegroupname
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupname", "en-US", "改善组名称_us", "改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customercomplaintitem.improvementresponsiblegroupname
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupname", "ja-JP", "改善组名称_jp", "改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customercomplaintitem.improvementresponsiblegroupname
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupname", "zh-CN", "改善组名称", "改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),
            // entity.customercomplaintitem.improvementresponsiblegroupname
            new TranslationSeedItem("entity.customercomplaintitem.improvementresponsiblegroupname", "zh-HK", "改善组名称_hk", "改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）"),

            // entity.customercomplaintitem.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.plannedcompletiondate", "en-US", "计划完成日期_us", "计划完成日期"),
            // entity.customercomplaintitem.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.plannedcompletiondate", "ja-JP", "计划完成日期_jp", "计划完成日期"),
            // entity.customercomplaintitem.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.plannedcompletiondate", "zh-CN", "计划完成日期", "计划完成日期"),
            // entity.customercomplaintitem.plannedcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.plannedcompletiondate", "zh-HK", "计划完成日期_hk", "计划完成日期"),

            // entity.customercomplaintitem.actualcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.actualcompletiondate", "en-US", "实际完成日期_us", "实际完成日期"),
            // entity.customercomplaintitem.actualcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.actualcompletiondate", "ja-JP", "实际完成日期_jp", "实际完成日期"),
            // entity.customercomplaintitem.actualcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.actualcompletiondate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.customercomplaintitem.actualcompletiondate
            new TranslationSeedItem("entity.customercomplaintitem.actualcompletiondate", "zh-HK", "实际完成日期_hk", "实际完成日期"),

            // entity.customercomplaintitem.filename
            new TranslationSeedItem("entity.customercomplaintitem.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.customercomplaintitem.filename
            new TranslationSeedItem("entity.customercomplaintitem.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.customercomplaintitem.filename
            new TranslationSeedItem("entity.customercomplaintitem.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.customercomplaintitem.filename
            new TranslationSeedItem("entity.customercomplaintitem.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.customercomplaintitem.accessurl
            new TranslationSeedItem("entity.customercomplaintitem.accessurl", "en-US", "访问地址_us", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.customercomplaintitem.accessurl
            new TranslationSeedItem("entity.customercomplaintitem.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.customercomplaintitem.accessurl
            new TranslationSeedItem("entity.customercomplaintitem.accessurl", "zh-CN", "访问地址", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),
            // entity.customercomplaintitem.accessurl
            new TranslationSeedItem("entity.customercomplaintitem.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）"),

            // entity.customercomplaintitem.improvementstatus
            new TranslationSeedItem("entity.customercomplaintitem.improvementstatus", "en-US", "改善状态_us", "改善状态（字典 logistics_quality_improvement_status）"),
            // entity.customercomplaintitem.improvementstatus
            new TranslationSeedItem("entity.customercomplaintitem.improvementstatus", "ja-JP", "改善状态_jp", "改善状态（字典 logistics_quality_improvement_status）"),
            // entity.customercomplaintitem.improvementstatus
            new TranslationSeedItem("entity.customercomplaintitem.improvementstatus", "zh-CN", "改善状态", "改善状态（字典 logistics_quality_improvement_status）"),
            // entity.customercomplaintitem.improvementstatus
            new TranslationSeedItem("entity.customercomplaintitem.improvementstatus", "zh-HK", "改善状态_hk", "改善状态（字典 logistics_quality_improvement_status）"),

            // entity.customercomplaintitem.isobsolete
            new TranslationSeedItem("entity.customercomplaintitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customercomplaintitem.isobsolete
            new TranslationSeedItem("entity.customercomplaintitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customercomplaintitem.isobsolete
            new TranslationSeedItem("entity.customercomplaintitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customercomplaintitem.isobsolete
            new TranslationSeedItem("entity.customercomplaintitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.customercomplaintitem.complaint
            new TranslationSeedItem("entity.customercomplaintitem.complaint", "en-US", "客诉主表_us", "客诉主表"),
            // entity.customercomplaintitem.complaint
            new TranslationSeedItem("entity.customercomplaintitem.complaint", "ja-JP", "客诉主表_jp", "客诉主表"),
            // entity.customercomplaintitem.complaint
            new TranslationSeedItem("entity.customercomplaintitem.complaint", "zh-CN", "客诉主表", "客诉主表"),
            // entity.customercomplaintitem.complaint
            new TranslationSeedItem("entity.customercomplaintitem.complaint", "zh-HK", "客诉主表_hk", "客诉主表"),
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
        translation.ResourceGroup = "Complaint";
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
