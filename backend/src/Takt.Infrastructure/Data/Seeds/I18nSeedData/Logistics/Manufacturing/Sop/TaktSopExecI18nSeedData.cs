// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopExec 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopExec 实体国际化翻译种子（键前缀 entity.sopexec.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopExecI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopExec 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopexec 实体翻译...", tenantCode);

        foreach (var item in GetSopExecTranslations())
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

        TaktLogger.Information("TaktSopExec 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopExec 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopexec._self / entity.sopexec.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopExecTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopexec._self
            new TranslationSeedItem("entity.sopexec._self", "en-US", "Sop Exec Information_us", "实体名称"),
            // entity.sopexec._self
            new TranslationSeedItem("entity.sopexec._self", "ja-JP", "SOP 工位执行追溯信息_jp", "实体名称"),
            // entity.sopexec._self
            new TranslationSeedItem("entity.sopexec._self", "zh-CN", "SOP 工位执行追溯信息", "实体名称"),
            // entity.sopexec._self
            new TranslationSeedItem("entity.sopexec._self", "zh-HK", "SOP 工位执行追溯信息_hk", "实体名称"),

            // entity.sopexec.productionorderid
            new TranslationSeedItem("entity.sopexec.productionorderid", "en-US", "生产工单ID_us", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.sopexec.productionorderid
            new TranslationSeedItem("entity.sopexec.productionorderid", "ja-JP", "生产工单ID_jp", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.sopexec.productionorderid
            new TranslationSeedItem("entity.sopexec.productionorderid", "zh-CN", "生产工单ID", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.sopexec.productionorderid
            new TranslationSeedItem("entity.sopexec.productionorderid", "zh-HK", "生产工单ID_hk", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),

            // entity.sopexec.workordercode
            new TranslationSeedItem("entity.sopexec.workordercode", "en-US", "工单号_us", "MES 工单号（冗余，便于追溯查询）"),
            // entity.sopexec.workordercode
            new TranslationSeedItem("entity.sopexec.workordercode", "ja-JP", "工单号_jp", "MES 工单号（冗余，便于追溯查询）"),
            // entity.sopexec.workordercode
            new TranslationSeedItem("entity.sopexec.workordercode", "zh-CN", "工单号", "MES 工单号（冗余，便于追溯查询）"),
            // entity.sopexec.workordercode
            new TranslationSeedItem("entity.sopexec.workordercode", "zh-HK", "工单号_hk", "MES 工单号（冗余，便于追溯查询）"),

            // entity.sopexec.serialnumber
            new TranslationSeedItem("entity.sopexec.serialnumber", "en-US", "产品序列号_us", "产品序列号 SN"),
            // entity.sopexec.serialnumber
            new TranslationSeedItem("entity.sopexec.serialnumber", "ja-JP", "产品序列号_jp", "产品序列号 SN"),
            // entity.sopexec.serialnumber
            new TranslationSeedItem("entity.sopexec.serialnumber", "zh-CN", "产品序列号", "产品序列号 SN"),
            // entity.sopexec.serialnumber
            new TranslationSeedItem("entity.sopexec.serialnumber", "zh-HK", "产品序列号_hk", "产品序列号 SN"),

            // entity.sopexec.materialcode
            new TranslationSeedItem("entity.sopexec.materialcode", "en-US", "物料编码_us", "产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexec.materialcode
            new TranslationSeedItem("entity.sopexec.materialcode", "ja-JP", "物料编码_jp", "产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexec.materialcode
            new TranslationSeedItem("entity.sopexec.materialcode", "zh-CN", "物料编码", "产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sopexec.materialcode
            new TranslationSeedItem("entity.sopexec.materialcode", "zh-HK", "物料编码_hk", "产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.sopexec.routingitemid
            new TranslationSeedItem("entity.sopexec.routingitemid", "en-US", "工序ID_us", "工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopexec.routingitemid
            new TranslationSeedItem("entity.sopexec.routingitemid", "ja-JP", "工序ID_jp", "工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopexec.routingitemid
            new TranslationSeedItem("entity.sopexec.routingitemid", "zh-CN", "工序ID", "工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.sopexec.routingitemid
            new TranslationSeedItem("entity.sopexec.routingitemid", "zh-HK", "工序ID_hk", "工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),

            // entity.sopexec.processsegmenttype
            new TranslationSeedItem("entity.sopexec.processsegmenttype", "en-US", "工艺段类型_us", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopexec.processsegmenttype
            new TranslationSeedItem("entity.sopexec.processsegmenttype", "ja-JP", "工艺段类型_jp", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopexec.processsegmenttype
            new TranslationSeedItem("entity.sopexec.processsegmenttype", "zh-CN", "工艺段类型", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopexec.processsegmenttype
            new TranslationSeedItem("entity.sopexec.processsegmenttype", "zh-HK", "工艺段类型_hk", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),

            // entity.sopexec.workstationid
            new TranslationSeedItem("entity.sopexec.workstationid", "en-US", "工位ID_us", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopexec.workstationid
            new TranslationSeedItem("entity.sopexec.workstationid", "ja-JP", "工位ID_jp", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopexec.workstationid
            new TranslationSeedItem("entity.sopexec.workstationid", "zh-CN", "工位ID", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),
            // entity.sopexec.workstationid
            new TranslationSeedItem("entity.sopexec.workstationid", "zh-HK", "工位ID_hk", "工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）"),

            // entity.sopexec.employeeid
            new TranslationSeedItem("entity.sopexec.employeeid", "en-US", "员工ID_us", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexec.employeeid
            new TranslationSeedItem("entity.sopexec.employeeid", "ja-JP", "员工ID_jp", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexec.employeeid
            new TranslationSeedItem("entity.sopexec.employeeid", "zh-CN", "员工ID", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexec.employeeid
            new TranslationSeedItem("entity.sopexec.employeeid", "zh-HK", "员工ID_hk", "员工 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopexec.employeename
            new TranslationSeedItem("entity.sopexec.employeename", "en-US", "员工名称_us", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexec.employeename
            new TranslationSeedItem("entity.sopexec.employeename", "ja-JP", "员工名称_jp", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexec.employeename
            new TranslationSeedItem("entity.sopexec.employeename", "zh-CN", "员工名称", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexec.employeename
            new TranslationSeedItem("entity.sopexec.employeename", "zh-HK", "员工名称_hk", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopexec.sopid
            new TranslationSeedItem("entity.sopexec.sopid", "en-US", "SOP主档ID_us", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopexec.sopid
            new TranslationSeedItem("entity.sopexec.sopid", "ja-JP", "SOP主档ID_jp", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopexec.sopid
            new TranslationSeedItem("entity.sopexec.sopid", "zh-CN", "SOP主档ID", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopexec.sopid
            new TranslationSeedItem("entity.sopexec.sopid", "zh-HK", "SOP主档ID_hk", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),

            // entity.sopexec.revisionid
            new TranslationSeedItem("entity.sopexec.revisionid", "en-US", "SOP版本ID_us", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopexec.revisionid
            new TranslationSeedItem("entity.sopexec.revisionid", "ja-JP", "SOP版本ID_jp", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopexec.revisionid
            new TranslationSeedItem("entity.sopexec.revisionid", "zh-CN", "SOP版本ID", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopexec.revisionid
            new TranslationSeedItem("entity.sopexec.revisionid", "zh-HK", "SOP版本ID_hk", "SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),

            // entity.sopexec.revision
            new TranslationSeedItem("entity.sopexec.revision", "en-US", "版本号快照_us", "版本号快照"),
            // entity.sopexec.revision
            new TranslationSeedItem("entity.sopexec.revision", "ja-JP", "版本号快照_jp", "版本号快照"),
            // entity.sopexec.revision
            new TranslationSeedItem("entity.sopexec.revision", "zh-CN", "版本号快照", "版本号快照"),
            // entity.sopexec.revision
            new TranslationSeedItem("entity.sopexec.revision", "zh-HK", "版本号快照_hk", "版本号快照"),

            // entity.sopexec.startedat
            new TranslationSeedItem("entity.sopexec.startedat", "en-US", "开始时间_us", "开始时间"),
            // entity.sopexec.startedat
            new TranslationSeedItem("entity.sopexec.startedat", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.sopexec.startedat
            new TranslationSeedItem("entity.sopexec.startedat", "zh-CN", "开始时间", "开始时间"),
            // entity.sopexec.startedat
            new TranslationSeedItem("entity.sopexec.startedat", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.sopexec.endedat
            new TranslationSeedItem("entity.sopexec.endedat", "en-US", "结束时间_us", "结束时间"),
            // entity.sopexec.endedat
            new TranslationSeedItem("entity.sopexec.endedat", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.sopexec.endedat
            new TranslationSeedItem("entity.sopexec.endedat", "zh-CN", "结束时间", "结束时间"),
            // entity.sopexec.endedat
            new TranslationSeedItem("entity.sopexec.endedat", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.sopexec.selfcheckresult
            new TranslationSeedItem("entity.sopexec.selfcheckresult", "en-US", "自检结果_us", "自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexec.selfcheckresult
            new TranslationSeedItem("entity.sopexec.selfcheckresult", "ja-JP", "自检结果_jp", "自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexec.selfcheckresult
            new TranslationSeedItem("entity.sopexec.selfcheckresult", "zh-CN", "自检结果", "自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexec.selfcheckresult
            new TranslationSeedItem("entity.sopexec.selfcheckresult", "zh-HK", "自检结果_hk", "自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),

            // entity.sopexec.execstatus
            new TranslationSeedItem("entity.sopexec.execstatus", "en-US", "执行状态_us", "执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）"),
            // entity.sopexec.execstatus
            new TranslationSeedItem("entity.sopexec.execstatus", "ja-JP", "执行状态_jp", "执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）"),
            // entity.sopexec.execstatus
            new TranslationSeedItem("entity.sopexec.execstatus", "zh-CN", "执行状态", "执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）"),
            // entity.sopexec.execstatus
            new TranslationSeedItem("entity.sopexec.execstatus", "zh-HK", "执行状态_hk", "执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）"),

            // entity.sopexec.currentstepid
            new TranslationSeedItem("entity.sopexec.currentstepid", "en-US", "当前工步ID_us", "当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexec.currentstepid
            new TranslationSeedItem("entity.sopexec.currentstepid", "ja-JP", "当前工步ID_jp", "当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexec.currentstepid
            new TranslationSeedItem("entity.sopexec.currentstepid", "zh-CN", "当前工步ID", "当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexec.currentstepid
            new TranslationSeedItem("entity.sopexec.currentstepid", "zh-HK", "当前工步ID_hk", "当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),

            // entity.sopexec.workstation
            new TranslationSeedItem("entity.sopexec.workstation", "en-US", "工位_us", "工位"),
            // entity.sopexec.workstation
            new TranslationSeedItem("entity.sopexec.workstation", "ja-JP", "工位_jp", "工位"),
            // entity.sopexec.workstation
            new TranslationSeedItem("entity.sopexec.workstation", "zh-CN", "工位", "工位"),
            // entity.sopexec.workstation
            new TranslationSeedItem("entity.sopexec.workstation", "zh-HK", "工位_hk", "工位"),

            // entity.sopexec.steps
            new TranslationSeedItem("entity.sopexec.steps", "en-US", "工步执行明细_us", "工步执行明细"),
            // entity.sopexec.steps
            new TranslationSeedItem("entity.sopexec.steps", "ja-JP", "工步执行明细_jp", "工步执行明细"),
            // entity.sopexec.steps
            new TranslationSeedItem("entity.sopexec.steps", "zh-CN", "工步执行明细", "工步执行明细"),
            // entity.sopexec.steps
            new TranslationSeedItem("entity.sopexec.steps", "zh-HK", "工步执行明细_hk", "工步执行明细"),

            // entity.sopexec.scans
            new TranslationSeedItem("entity.sopexec.scans", "en-US", "扫码记录_us", "扫码记录"),
            // entity.sopexec.scans
            new TranslationSeedItem("entity.sopexec.scans", "ja-JP", "扫码记录_jp", "扫码记录"),
            // entity.sopexec.scans
            new TranslationSeedItem("entity.sopexec.scans", "zh-CN", "扫码记录", "扫码记录"),
            // entity.sopexec.scans
            new TranslationSeedItem("entity.sopexec.scans", "zh-HK", "扫码记录_hk", "扫码记录"),

            // entity.sopexec.arguments
            new TranslationSeedItem("entity.sopexec.arguments", "en-US", "作业参数_us", "作业参数"),
            // entity.sopexec.arguments
            new TranslationSeedItem("entity.sopexec.arguments", "ja-JP", "作业参数_jp", "作业参数"),
            // entity.sopexec.arguments
            new TranslationSeedItem("entity.sopexec.arguments", "zh-CN", "作业参数", "作业参数"),
            // entity.sopexec.arguments
            new TranslationSeedItem("entity.sopexec.arguments", "zh-HK", "作业参数_hk", "作业参数"),
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
        translation.ResourceGroup = "Sop";
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
