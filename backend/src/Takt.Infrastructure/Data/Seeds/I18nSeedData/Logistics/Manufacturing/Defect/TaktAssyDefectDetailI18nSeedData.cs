// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyDefectDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect;

/// <summary>
/// TaktAssyDefectDetail 实体国际化翻译种子（键前缀 entity.assydefectdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyDefectDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyDefectDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assydefectdetail 实体翻译...", tenantCode);

        foreach (var item in GetAssyDefectDetailTranslations())
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

        TaktLogger.Information("TaktAssyDefectDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyDefectDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assydefectdetail._self / entity.assydefectdetail.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyDefectDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assydefectdetail._self
            new TranslationSeedItem("entity.assydefectdetail._self", "en-US", "Assy Defect Detail Information_us", "实体名称"),
            // entity.assydefectdetail._self
            new TranslationSeedItem("entity.assydefectdetail._self", "ja-JP", "组立不良明细信息_jp", "实体名称"),
            // entity.assydefectdetail._self
            new TranslationSeedItem("entity.assydefectdetail._self", "zh-CN", "组立不良明细信息", "实体名称"),
            // entity.assydefectdetail._self
            new TranslationSeedItem("entity.assydefectdetail._self", "zh-HK", "组立不良明细信息_hk", "实体名称"),

            // entity.assydefectdetail.assydefectid
            new TranslationSeedItem("entity.assydefectdetail.assydefectid", "en-US", "组立不良ID_us", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assydefectdetail.assydefectid
            new TranslationSeedItem("entity.assydefectdetail.assydefectid", "ja-JP", "组立不良ID_jp", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assydefectdetail.assydefectid
            new TranslationSeedItem("entity.assydefectdetail.assydefectid", "zh-CN", "组立不良ID", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assydefectdetail.assydefectid
            new TranslationSeedItem("entity.assydefectdetail.assydefectid", "zh-HK", "组立不良ID_hk", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.assydefectdetail.prodordercode
            new TranslationSeedItem("entity.assydefectdetail.prodordercode", "en-US", "工单号_us", "工单号（冗余字段,便于查询）"),
            // entity.assydefectdetail.prodordercode
            new TranslationSeedItem("entity.assydefectdetail.prodordercode", "ja-JP", "工单号_jp", "工单号（冗余字段,便于查询）"),
            // entity.assydefectdetail.prodordercode
            new TranslationSeedItem("entity.assydefectdetail.prodordercode", "zh-CN", "工单号", "工单号（冗余字段,便于查询）"),
            // entity.assydefectdetail.prodordercode
            new TranslationSeedItem("entity.assydefectdetail.prodordercode", "zh-HK", "工单号_hk", "工单号（冗余字段,便于查询）"),

            // entity.assydefectdetail.prodactualqty
            new TranslationSeedItem("entity.assydefectdetail.prodactualqty", "en-US", "生实实绩_us", "生实实绩（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.prodactualqty
            new TranslationSeedItem("entity.assydefectdetail.prodactualqty", "ja-JP", "生实实绩_jp", "生实实绩（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.prodactualqty
            new TranslationSeedItem("entity.assydefectdetail.prodactualqty", "zh-CN", "生实实绩", "生实实绩（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.prodactualqty
            new TranslationSeedItem("entity.assydefectdetail.prodactualqty", "zh-HK", "生实实绩_hk", "生实实绩（冗余字段,便于统计/查询）"),

            // entity.assydefectdetail.goodquantity
            new TranslationSeedItem("entity.assydefectdetail.goodquantity", "en-US", "无不良数量_us", "无不良数量（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.goodquantity
            new TranslationSeedItem("entity.assydefectdetail.goodquantity", "ja-JP", "无不良数量_jp", "无不良数量（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.goodquantity
            new TranslationSeedItem("entity.assydefectdetail.goodquantity", "zh-CN", "无不良数量", "无不良数量（冗余字段,便于统计/查询）"),
            // entity.assydefectdetail.goodquantity
            new TranslationSeedItem("entity.assydefectdetail.goodquantity", "zh-HK", "无不良数量_hk", "无不良数量（冗余字段,便于统计/查询）"),

            // entity.assydefectdetail.linenumber
            new TranslationSeedItem("entity.assydefectdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.assydefectdetail.linenumber
            new TranslationSeedItem("entity.assydefectdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.assydefectdetail.linenumber
            new TranslationSeedItem("entity.assydefectdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assydefectdetail.linenumber
            new TranslationSeedItem("entity.assydefectdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.assydefectdetail.defectcategory
            new TranslationSeedItem("entity.assydefectdetail.defectcategory", "en-US", "不良区分_us", "不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）"),
            // entity.assydefectdetail.defectcategory
            new TranslationSeedItem("entity.assydefectdetail.defectcategory", "ja-JP", "不良区分_jp", "不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）"),
            // entity.assydefectdetail.defectcategory
            new TranslationSeedItem("entity.assydefectdetail.defectcategory", "zh-CN", "不良区分", "不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）"),
            // entity.assydefectdetail.defectcategory
            new TranslationSeedItem("entity.assydefectdetail.defectcategory", "zh-HK", "不良区分_hk", "不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）"),

            // entity.assydefectdetail.defectqty
            new TranslationSeedItem("entity.assydefectdetail.defectqty", "en-US", "不良数量_us", "不良数量"),
            // entity.assydefectdetail.defectqty
            new TranslationSeedItem("entity.assydefectdetail.defectqty", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.assydefectdetail.defectqty
            new TranslationSeedItem("entity.assydefectdetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.assydefectdetail.defectqty
            new TranslationSeedItem("entity.assydefectdetail.defectqty", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.assydefectdetail.cumulativedefectqty
            new TranslationSeedItem("entity.assydefectdetail.cumulativedefectqty", "en-US", "累计不良_us", "累计不良"),
            // entity.assydefectdetail.cumulativedefectqty
            new TranslationSeedItem("entity.assydefectdetail.cumulativedefectqty", "ja-JP", "累计不良_jp", "累计不良"),
            // entity.assydefectdetail.cumulativedefectqty
            new TranslationSeedItem("entity.assydefectdetail.cumulativedefectqty", "zh-CN", "累计不良", "累计不良"),
            // entity.assydefectdetail.cumulativedefectqty
            new TranslationSeedItem("entity.assydefectdetail.cumulativedefectqty", "zh-HK", "累计不良_hk", "累计不良"),

            // entity.assydefectdetail.randomcardcode
            new TranslationSeedItem("entity.assydefectdetail.randomcardcode", "en-US", "随机卡号_us", "随机卡号"),
            // entity.assydefectdetail.randomcardcode
            new TranslationSeedItem("entity.assydefectdetail.randomcardcode", "ja-JP", "随机卡号_jp", "随机卡号"),
            // entity.assydefectdetail.randomcardcode
            new TranslationSeedItem("entity.assydefectdetail.randomcardcode", "zh-CN", "随机卡号", "随机卡号"),
            // entity.assydefectdetail.randomcardcode
            new TranslationSeedItem("entity.assydefectdetail.randomcardcode", "zh-HK", "随机卡号_hk", "随机卡号"),

            // entity.assydefectdetail.occurrenceengineering
            new TranslationSeedItem("entity.assydefectdetail.occurrenceengineering", "en-US", "发生工程_us", "发生工程"),
            // entity.assydefectdetail.occurrenceengineering
            new TranslationSeedItem("entity.assydefectdetail.occurrenceengineering", "ja-JP", "发生工程_jp", "发生工程"),
            // entity.assydefectdetail.occurrenceengineering
            new TranslationSeedItem("entity.assydefectdetail.occurrenceengineering", "zh-CN", "发生工程", "发生工程"),
            // entity.assydefectdetail.occurrenceengineering
            new TranslationSeedItem("entity.assydefectdetail.occurrenceengineering", "zh-HK", "发生工程_hk", "发生工程"),

            // entity.assydefectdetail.teststep
            new TranslationSeedItem("entity.assydefectdetail.teststep", "en-US", "测试步骤_us", "测试步骤"),
            // entity.assydefectdetail.teststep
            new TranslationSeedItem("entity.assydefectdetail.teststep", "ja-JP", "测试步骤_jp", "测试步骤"),
            // entity.assydefectdetail.teststep
            new TranslationSeedItem("entity.assydefectdetail.teststep", "zh-CN", "测试步骤", "测试步骤"),
            // entity.assydefectdetail.teststep
            new TranslationSeedItem("entity.assydefectdetail.teststep", "zh-HK", "测试步骤_hk", "测试步骤"),

            // entity.assydefectdetail.defectsymptom
            new TranslationSeedItem("entity.assydefectdetail.defectsymptom", "en-US", "不良症状_us", "不良症状"),
            // entity.assydefectdetail.defectsymptom
            new TranslationSeedItem("entity.assydefectdetail.defectsymptom", "ja-JP", "不良症状_jp", "不良症状"),
            // entity.assydefectdetail.defectsymptom
            new TranslationSeedItem("entity.assydefectdetail.defectsymptom", "zh-CN", "不良症状", "不良症状"),
            // entity.assydefectdetail.defectsymptom
            new TranslationSeedItem("entity.assydefectdetail.defectsymptom", "zh-HK", "不良症状_hk", "不良症状"),

            // entity.assydefectdetail.defectlocation
            new TranslationSeedItem("entity.assydefectdetail.defectlocation", "en-US", "不良个所_us", "不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）"),
            // entity.assydefectdetail.defectlocation
            new TranslationSeedItem("entity.assydefectdetail.defectlocation", "ja-JP", "不良个所_jp", "不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）"),
            // entity.assydefectdetail.defectlocation
            new TranslationSeedItem("entity.assydefectdetail.defectlocation", "zh-CN", "不良个所", "不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）"),
            // entity.assydefectdetail.defectlocation
            new TranslationSeedItem("entity.assydefectdetail.defectlocation", "zh-HK", "不良个所_hk", "不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）"),

            // entity.assydefectdetail.defectreason
            new TranslationSeedItem("entity.assydefectdetail.defectreason", "en-US", "不良原因_us", "不良原因"),
            // entity.assydefectdetail.defectreason
            new TranslationSeedItem("entity.assydefectdetail.defectreason", "ja-JP", "不良原因_jp", "不良原因"),
            // entity.assydefectdetail.defectreason
            new TranslationSeedItem("entity.assydefectdetail.defectreason", "zh-CN", "不良原因", "不良原因"),
            // entity.assydefectdetail.defectreason
            new TranslationSeedItem("entity.assydefectdetail.defectreason", "zh-HK", "不良原因_hk", "不良原因"),

            // entity.assydefectdetail.repairoperatorid
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorid", "en-US", "修理员ID_us", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.assydefectdetail.repairoperatorid
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorid", "ja-JP", "修理员ID_jp", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.assydefectdetail.repairoperatorid
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorid", "zh-CN", "修理员ID", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.assydefectdetail.repairoperatorid
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorid", "zh-HK", "修理员ID_hk", "修理员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.assydefectdetail.repairoperatorname
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorname", "en-US", "修理员名称_us", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.assydefectdetail.repairoperatorname
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorname", "ja-JP", "修理员名称_jp", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.assydefectdetail.repairoperatorname
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorname", "zh-CN", "修理员名称", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.assydefectdetail.repairoperatorname
            new TranslationSeedItem("entity.assydefectdetail.repairoperatorname", "zh-HK", "修理员名称_hk", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.assydefectdetail.isobsolete
            new TranslationSeedItem("entity.assydefectdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assydefectdetail.isobsolete
            new TranslationSeedItem("entity.assydefectdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assydefectdetail.isobsolete
            new TranslationSeedItem("entity.assydefectdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assydefectdetail.isobsolete
            new TranslationSeedItem("entity.assydefectdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.assydefectdetail.assydefect
            new TranslationSeedItem("entity.assydefectdetail.assydefect", "en-US", "组立不良日报_us", "组立不良日报（主表）"),
            // entity.assydefectdetail.assydefect
            new TranslationSeedItem("entity.assydefectdetail.assydefect", "ja-JP", "组立不良日报_jp", "组立不良日报（主表）"),
            // entity.assydefectdetail.assydefect
            new TranslationSeedItem("entity.assydefectdetail.assydefect", "zh-CN", "组立不良日报", "组立不良日报（主表）"),
            // entity.assydefectdetail.assydefect
            new TranslationSeedItem("entity.assydefectdetail.assydefect", "zh-HK", "组立不良日报_hk", "组立不良日报（主表）"),
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
        translation.ResourceGroup = "Defect";
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
