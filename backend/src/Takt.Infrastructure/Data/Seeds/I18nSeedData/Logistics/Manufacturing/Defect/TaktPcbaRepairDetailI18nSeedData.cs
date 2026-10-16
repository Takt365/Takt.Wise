// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaRepairDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPcbaRepairDetail 实体国际化翻译种子（键前缀 entity.pcbarepairdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaRepairDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaRepairDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbarepairdetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaRepairDetailTranslations())
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

        TaktLogger.Information("TaktPcbaRepairDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaRepairDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbarepairdetail._self / entity.pcbarepairdetail.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaRepairDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbarepairdetail._self
            new TranslationSeedItem("entity.pcbarepairdetail._self", "en-US", "Pcba Repair Detail Information_us", "实体名称"),
            // entity.pcbarepairdetail._self
            new TranslationSeedItem("entity.pcbarepairdetail._self", "ja-JP", "PCBA改修明细信息_jp", "实体名称"),
            // entity.pcbarepairdetail._self
            new TranslationSeedItem("entity.pcbarepairdetail._self", "zh-CN", "PCBA改修明细信息", "实体名称"),
            // entity.pcbarepairdetail._self
            new TranslationSeedItem("entity.pcbarepairdetail._self", "zh-HK", "PCBA改修明细信息_hk", "实体名称"),

            // entity.pcbarepairdetail.pcbarepairid
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepairid", "en-US", "PCBA改修ID_us", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbarepairdetail.pcbarepairid
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepairid", "ja-JP", "PCBA改修ID_jp", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbarepairdetail.pcbarepairid
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepairid", "zh-CN", "PCBA改修ID", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbarepairdetail.pcbarepairid
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepairid", "zh-HK", "PCBA改修ID_hk", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbarepairdetail.prodordercode
            new TranslationSeedItem("entity.pcbarepairdetail.prodordercode", "en-US", "工单号_us", "工单号（冗余字段,便于查询）"),
            // entity.pcbarepairdetail.prodordercode
            new TranslationSeedItem("entity.pcbarepairdetail.prodordercode", "ja-JP", "工单号_jp", "工单号（冗余字段,便于查询）"),
            // entity.pcbarepairdetail.prodordercode
            new TranslationSeedItem("entity.pcbarepairdetail.prodordercode", "zh-CN", "工单号", "工单号（冗余字段,便于查询）"),
            // entity.pcbarepairdetail.prodordercode
            new TranslationSeedItem("entity.pcbarepairdetail.prodordercode", "zh-HK", "工单号_hk", "工单号（冗余字段,便于查询）"),

            // entity.pcbarepairdetail.linenumber
            new TranslationSeedItem("entity.pcbarepairdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.pcbarepairdetail.linenumber
            new TranslationSeedItem("entity.pcbarepairdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.pcbarepairdetail.linenumber
            new TranslationSeedItem("entity.pcbarepairdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbarepairdetail.linenumber
            new TranslationSeedItem("entity.pcbarepairdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.pcbarepairdetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbarepairdetail.pcbaboardtype", "en-US", "PCBA板别_us", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbarepairdetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbarepairdetail.pcbaboardtype", "ja-JP", "PCBA板别_jp", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbarepairdetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbarepairdetail.pcbaboardtype", "zh-CN", "PCBA板别", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbarepairdetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbarepairdetail.pcbaboardtype", "zh-HK", "PCBA板别_hk", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),

            // entity.pcbarepairdetail.prodactualqty
            new TranslationSeedItem("entity.pcbarepairdetail.prodactualqty", "en-US", "生产实绩_us", "生产实绩"),
            // entity.pcbarepairdetail.prodactualqty
            new TranslationSeedItem("entity.pcbarepairdetail.prodactualqty", "ja-JP", "生产实绩_jp", "生产实绩"),
            // entity.pcbarepairdetail.prodactualqty
            new TranslationSeedItem("entity.pcbarepairdetail.prodactualqty", "zh-CN", "生产实绩", "生产实绩"),
            // entity.pcbarepairdetail.prodactualqty
            new TranslationSeedItem("entity.pcbarepairdetail.prodactualqty", "zh-HK", "生产实绩_hk", "生产实绩"),

            // entity.pcbarepairdetail.teamcode
            new TranslationSeedItem("entity.pcbarepairdetail.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairdetail.teamcode
            new TranslationSeedItem("entity.pcbarepairdetail.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairdetail.teamcode
            new TranslationSeedItem("entity.pcbarepairdetail.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbarepairdetail.teamcode
            new TranslationSeedItem("entity.pcbarepairdetail.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.pcbarepairdetail.cardcode
            new TranslationSeedItem("entity.pcbarepairdetail.cardcode", "en-US", "卡号_us", "卡号"),
            // entity.pcbarepairdetail.cardcode
            new TranslationSeedItem("entity.pcbarepairdetail.cardcode", "ja-JP", "卡号_jp", "卡号"),
            // entity.pcbarepairdetail.cardcode
            new TranslationSeedItem("entity.pcbarepairdetail.cardcode", "zh-CN", "卡号", "卡号"),
            // entity.pcbarepairdetail.cardcode
            new TranslationSeedItem("entity.pcbarepairdetail.cardcode", "zh-HK", "卡号_hk", "卡号"),

            // entity.pcbarepairdetail.defectsymptom
            new TranslationSeedItem("entity.pcbarepairdetail.defectsymptom", "en-US", "不良症状_us", "不良症状"),
            // entity.pcbarepairdetail.defectsymptom
            new TranslationSeedItem("entity.pcbarepairdetail.defectsymptom", "ja-JP", "不良症状_jp", "不良症状"),
            // entity.pcbarepairdetail.defectsymptom
            new TranslationSeedItem("entity.pcbarepairdetail.defectsymptom", "zh-CN", "不良症状", "不良症状"),
            // entity.pcbarepairdetail.defectsymptom
            new TranslationSeedItem("entity.pcbarepairdetail.defectsymptom", "zh-HK", "不良症状_hk", "不良症状"),

            // entity.pcbarepairdetail.defectengineering
            new TranslationSeedItem("entity.pcbarepairdetail.defectengineering", "en-US", "检出工程_us", "检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）"),
            // entity.pcbarepairdetail.defectengineering
            new TranslationSeedItem("entity.pcbarepairdetail.defectengineering", "ja-JP", "检出工程_jp", "检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）"),
            // entity.pcbarepairdetail.defectengineering
            new TranslationSeedItem("entity.pcbarepairdetail.defectengineering", "zh-CN", "检出工程", "检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）"),
            // entity.pcbarepairdetail.defectengineering
            new TranslationSeedItem("entity.pcbarepairdetail.defectengineering", "zh-HK", "检出工程_hk", "检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）"),

            // entity.pcbarepairdetail.defectreason
            new TranslationSeedItem("entity.pcbarepairdetail.defectreason", "en-US", "不良原因_us", "不良原因"),
            // entity.pcbarepairdetail.defectreason
            new TranslationSeedItem("entity.pcbarepairdetail.defectreason", "ja-JP", "不良原因_jp", "不良原因"),
            // entity.pcbarepairdetail.defectreason
            new TranslationSeedItem("entity.pcbarepairdetail.defectreason", "zh-CN", "不良原因", "不良原因"),
            // entity.pcbarepairdetail.defectreason
            new TranslationSeedItem("entity.pcbarepairdetail.defectreason", "zh-HK", "不良原因_hk", "不良原因"),

            // entity.pcbarepairdetail.defectqty
            new TranslationSeedItem("entity.pcbarepairdetail.defectqty", "en-US", "不良数量_us", "不良数量"),
            // entity.pcbarepairdetail.defectqty
            new TranslationSeedItem("entity.pcbarepairdetail.defectqty", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.pcbarepairdetail.defectqty
            new TranslationSeedItem("entity.pcbarepairdetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.pcbarepairdetail.defectqty
            new TranslationSeedItem("entity.pcbarepairdetail.defectqty", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.pcbarepairdetail.defectresponsibility
            new TranslationSeedItem("entity.pcbarepairdetail.defectresponsibility", "en-US", "责任归属_us", "责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）"),
            // entity.pcbarepairdetail.defectresponsibility
            new TranslationSeedItem("entity.pcbarepairdetail.defectresponsibility", "ja-JP", "责任归属_jp", "责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）"),
            // entity.pcbarepairdetail.defectresponsibility
            new TranslationSeedItem("entity.pcbarepairdetail.defectresponsibility", "zh-CN", "责任归属", "责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）"),
            // entity.pcbarepairdetail.defectresponsibility
            new TranslationSeedItem("entity.pcbarepairdetail.defectresponsibility", "zh-HK", "责任归属_hk", "责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）"),

            // entity.pcbarepairdetail.defectnature
            new TranslationSeedItem("entity.pcbarepairdetail.defectnature", "en-US", "不良性质_us", "不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）"),
            // entity.pcbarepairdetail.defectnature
            new TranslationSeedItem("entity.pcbarepairdetail.defectnature", "ja-JP", "不良性质_jp", "不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）"),
            // entity.pcbarepairdetail.defectnature
            new TranslationSeedItem("entity.pcbarepairdetail.defectnature", "zh-CN", "不良性质", "不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）"),
            // entity.pcbarepairdetail.defectnature
            new TranslationSeedItem("entity.pcbarepairdetail.defectnature", "zh-HK", "不良性质_hk", "不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）"),

            // entity.pcbarepairdetail.repairoperatorid
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorid", "en-US", "修理员ID_us", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbarepairdetail.repairoperatorid
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorid", "ja-JP", "修理员ID_jp", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbarepairdetail.repairoperatorid
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorid", "zh-CN", "修理员ID", "修理员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbarepairdetail.repairoperatorid
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorid", "zh-HK", "修理员ID_hk", "修理员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.pcbarepairdetail.repairoperatorname
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorname", "en-US", "修理员名称_us", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbarepairdetail.repairoperatorname
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorname", "ja-JP", "修理员名称_jp", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbarepairdetail.repairoperatorname
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorname", "zh-CN", "修理员名称", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbarepairdetail.repairoperatorname
            new TranslationSeedItem("entity.pcbarepairdetail.repairoperatorname", "zh-HK", "修理员名称_hk", "修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.pcbarepairdetail.isobsolete
            new TranslationSeedItem("entity.pcbarepairdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbarepairdetail.isobsolete
            new TranslationSeedItem("entity.pcbarepairdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbarepairdetail.isobsolete
            new TranslationSeedItem("entity.pcbarepairdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbarepairdetail.isobsolete
            new TranslationSeedItem("entity.pcbarepairdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.pcbarepairdetail.pcbarepair
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepair", "en-US", "PCBA改修日报_us", "PCBA改修日报（主表）"),
            // entity.pcbarepairdetail.pcbarepair
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepair", "ja-JP", "PCBA改修日报_jp", "PCBA改修日报（主表）"),
            // entity.pcbarepairdetail.pcbarepair
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepair", "zh-CN", "PCBA改修日报", "PCBA改修日报（主表）"),
            // entity.pcbarepairdetail.pcbarepair
            new TranslationSeedItem("entity.pcbarepairdetail.pcbarepair", "zh-HK", "PCBA改修日报_hk", "PCBA改修日报（主表）"),
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
