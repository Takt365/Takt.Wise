// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaInspectionDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPcbaInspectionDetail 实体国际化翻译种子（键前缀 entity.pcbainspectiondetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaInspectionDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaInspectionDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbainspectiondetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaInspectionDetailTranslations())
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

        TaktLogger.Information("TaktPcbaInspectionDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaInspectionDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbainspectiondetail._self / entity.pcbainspectiondetail.{{field}}；ResourceGroup=Defect；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaInspectionDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbainspectiondetail._self
            new TranslationSeedItem("entity.pcbainspectiondetail._self", "en-US", "Pcba Inspection Detail Information_us", "实体名称"),
            // entity.pcbainspectiondetail._self
            new TranslationSeedItem("entity.pcbainspectiondetail._self", "ja-JP", "PCBA检查明细信息_jp", "实体名称"),
            // entity.pcbainspectiondetail._self
            new TranslationSeedItem("entity.pcbainspectiondetail._self", "zh-CN", "PCBA检查明细信息", "实体名称"),
            // entity.pcbainspectiondetail._self
            new TranslationSeedItem("entity.pcbainspectiondetail._self", "zh-HK", "PCBA检查明细信息_hk", "实体名称"),

            // entity.pcbainspectiondetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspectionid", "en-US", "PCBA检查ID_us", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbainspectiondetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspectionid", "ja-JP", "PCBA检查ID_jp", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbainspectiondetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspectionid", "zh-CN", "PCBA检查ID", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbainspectiondetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspectionid", "zh-HK", "PCBA检查ID_hk", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbainspectiondetail.prodordercode
            new TranslationSeedItem("entity.pcbainspectiondetail.prodordercode", "en-US", "工单号_us", "工单号（冗余字段,便于查询）"),
            // entity.pcbainspectiondetail.prodordercode
            new TranslationSeedItem("entity.pcbainspectiondetail.prodordercode", "ja-JP", "工单号_jp", "工单号（冗余字段,便于查询）"),
            // entity.pcbainspectiondetail.prodordercode
            new TranslationSeedItem("entity.pcbainspectiondetail.prodordercode", "zh-CN", "工单号", "工单号（冗余字段,便于查询）"),
            // entity.pcbainspectiondetail.prodordercode
            new TranslationSeedItem("entity.pcbainspectiondetail.prodordercode", "zh-HK", "工单号_hk", "工单号（冗余字段,便于查询）"),

            // entity.pcbainspectiondetail.linenumber
            new TranslationSeedItem("entity.pcbainspectiondetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.pcbainspectiondetail.linenumber
            new TranslationSeedItem("entity.pcbainspectiondetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.pcbainspectiondetail.linenumber
            new TranslationSeedItem("entity.pcbainspectiondetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbainspectiondetail.linenumber
            new TranslationSeedItem("entity.pcbainspectiondetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.pcbainspectiondetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbaboardtype", "en-US", "PCBA板别_us", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbainspectiondetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbaboardtype", "ja-JP", "PCBA板别_jp", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbainspectiondetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbaboardtype", "zh-CN", "PCBA板别", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),
            // entity.pcbainspectiondetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbaboardtype", "zh-HK", "PCBA板别_hk", "PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）"),

            // entity.pcbainspectiondetail.visualinspectionline
            new TranslationSeedItem("entity.pcbainspectiondetail.visualinspectionline", "en-US", "目视线别_us", "目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.visualinspectionline
            new TranslationSeedItem("entity.pcbainspectiondetail.visualinspectionline", "ja-JP", "目视线别_jp", "目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.visualinspectionline
            new TranslationSeedItem("entity.pcbainspectiondetail.visualinspectionline", "zh-CN", "目视线别", "目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.visualinspectionline
            new TranslationSeedItem("entity.pcbainspectiondetail.visualinspectionline", "zh-HK", "目视线别_hk", "目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）"),

            // entity.pcbainspectiondetail.aoiline
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiline", "en-US", "AOI线别_us", "AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.aoiline
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiline", "ja-JP", "AOI线别_jp", "AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.aoiline
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiline", "zh-CN", "AOI线别", "AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）"),
            // entity.pcbainspectiondetail.aoiline
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiline", "zh-HK", "AOI线别_hk", "AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）"),

            // entity.pcbainspectiondetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.bsideassemblydate", "en-US", "B面实装日期_us", "B面实装日期"),
            // entity.pcbainspectiondetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.bsideassemblydate", "ja-JP", "B面实装日期_jp", "B面实装日期"),
            // entity.pcbainspectiondetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.bsideassemblydate", "zh-CN", "B面实装日期", "B面实装日期"),
            // entity.pcbainspectiondetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.bsideassemblydate", "zh-HK", "B面实装日期_hk", "B面实装日期"),

            // entity.pcbainspectiondetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.tsideassemblydate", "en-US", "T面实装日期_us", "T面实装日期"),
            // entity.pcbainspectiondetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.tsideassemblydate", "ja-JP", "T面实装日期_jp", "T面实装日期"),
            // entity.pcbainspectiondetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.tsideassemblydate", "zh-CN", "T面实装日期", "T面实装日期"),
            // entity.pcbainspectiondetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbainspectiondetail.tsideassemblydate", "zh-HK", "T面实装日期_hk", "T面实装日期"),

            // entity.pcbainspectiondetail.shiftno
            new TranslationSeedItem("entity.pcbainspectiondetail.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbainspectiondetail.shiftno
            new TranslationSeedItem("entity.pcbainspectiondetail.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbainspectiondetail.shiftno
            new TranslationSeedItem("entity.pcbainspectiondetail.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),
            // entity.pcbainspectiondetail.shiftno
            new TranslationSeedItem("entity.pcbainspectiondetail.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）"),

            // entity.pcbainspectiondetail.inspectorid
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorid", "en-US", "检查员ID_us", "检查员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbainspectiondetail.inspectorid
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorid", "ja-JP", "检查员ID_jp", "检查员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbainspectiondetail.inspectorid
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorid", "zh-CN", "检查员ID", "检查员（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.pcbainspectiondetail.inspectorid
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorid", "zh-HK", "检查员ID_hk", "检查员（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.pcbainspectiondetail.inspectorname
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorname", "en-US", "检查员名称_us", "检查员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbainspectiondetail.inspectorname
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorname", "ja-JP", "检查员名称_jp", "检查员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbainspectiondetail.inspectorname
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorname", "zh-CN", "检查员名称", "检查员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.pcbainspectiondetail.inspectorname
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectorname", "zh-HK", "检查员名称_hk", "检查员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.pcbainspectiondetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbainspectiondetail.dailycompletedqty", "en-US", "当日完成数量_us", "当日完成数量"),
            // entity.pcbainspectiondetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbainspectiondetail.dailycompletedqty", "ja-JP", "当日完成数量_jp", "当日完成数量"),
            // entity.pcbainspectiondetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbainspectiondetail.dailycompletedqty", "zh-CN", "当日完成数量", "当日完成数量"),
            // entity.pcbainspectiondetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbainspectiondetail.dailycompletedqty", "zh-HK", "当日完成数量_hk", "当日完成数量"),

            // entity.pcbainspectiondetail.inspectionqty
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionqty", "en-US", "检查数量_us", "检查数量"),
            // entity.pcbainspectiondetail.inspectionqty
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionqty", "ja-JP", "检查数量_jp", "检查数量"),
            // entity.pcbainspectiondetail.inspectionqty
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionqty", "zh-CN", "检查数量", "检查数量"),
            // entity.pcbainspectiondetail.inspectionqty
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionqty", "zh-HK", "检查数量_hk", "检查数量"),

            // entity.pcbainspectiondetail.inspectionstatus
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionstatus", "en-US", "检查状态_us", "检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）"),
            // entity.pcbainspectiondetail.inspectionstatus
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionstatus", "ja-JP", "检查状态_jp", "检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）"),
            // entity.pcbainspectiondetail.inspectionstatus
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionstatus", "zh-CN", "检查状态", "检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）"),
            // entity.pcbainspectiondetail.inspectionstatus
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionstatus", "zh-HK", "检查状态_hk", "检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）"),

            // entity.pcbainspectiondetail.teamcode
            new TranslationSeedItem("entity.pcbainspectiondetail.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbainspectiondetail.teamcode
            new TranslationSeedItem("entity.pcbainspectiondetail.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbainspectiondetail.teamcode
            new TranslationSeedItem("entity.pcbainspectiondetail.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),
            // entity.pcbainspectiondetail.teamcode
            new TranslationSeedItem("entity.pcbainspectiondetail.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）"),

            // entity.pcbainspectiondetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionworkhours", "en-US", "检查工数_us", "检查工数"),
            // entity.pcbainspectiondetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionworkhours", "ja-JP", "检查工数_jp", "检查工数"),
            // entity.pcbainspectiondetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionworkhours", "zh-CN", "检查工数", "检查工数"),
            // entity.pcbainspectiondetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.inspectionworkhours", "zh-HK", "检查工数_hk", "检查工数"),

            // entity.pcbainspectiondetail.aoiworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiworkhours", "en-US", "AOI工数_us", "AOI工数"),
            // entity.pcbainspectiondetail.aoiworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiworkhours", "ja-JP", "AOI工数_jp", "AOI工数"),
            // entity.pcbainspectiondetail.aoiworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiworkhours", "zh-CN", "AOI工数", "AOI工数"),
            // entity.pcbainspectiondetail.aoiworkhours
            new TranslationSeedItem("entity.pcbainspectiondetail.aoiworkhours", "zh-HK", "AOI工数_hk", "AOI工数"),

            // entity.pcbainspectiondetail.defectqty
            new TranslationSeedItem("entity.pcbainspectiondetail.defectqty", "en-US", "不良数量_us", "不良数量"),
            // entity.pcbainspectiondetail.defectqty
            new TranslationSeedItem("entity.pcbainspectiondetail.defectqty", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.pcbainspectiondetail.defectqty
            new TranslationSeedItem("entity.pcbainspectiondetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.pcbainspectiondetail.defectqty
            new TranslationSeedItem("entity.pcbainspectiondetail.defectqty", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.pcbainspectiondetail.handplacement
            new TranslationSeedItem("entity.pcbainspectiondetail.handplacement", "en-US", "手贴_us", "手贴"),
            // entity.pcbainspectiondetail.handplacement
            new TranslationSeedItem("entity.pcbainspectiondetail.handplacement", "ja-JP", "手贴_jp", "手贴"),
            // entity.pcbainspectiondetail.handplacement
            new TranslationSeedItem("entity.pcbainspectiondetail.handplacement", "zh-CN", "手贴", "手贴"),
            // entity.pcbainspectiondetail.handplacement
            new TranslationSeedItem("entity.pcbainspectiondetail.handplacement", "zh-HK", "手贴_hk", "手贴"),

            // entity.pcbainspectiondetail.serialnumber
            new TranslationSeedItem("entity.pcbainspectiondetail.serialnumber", "en-US", "流水号_us", "流水号"),
            // entity.pcbainspectiondetail.serialnumber
            new TranslationSeedItem("entity.pcbainspectiondetail.serialnumber", "ja-JP", "流水号_jp", "流水号"),
            // entity.pcbainspectiondetail.serialnumber
            new TranslationSeedItem("entity.pcbainspectiondetail.serialnumber", "zh-CN", "流水号", "流水号"),
            // entity.pcbainspectiondetail.serialnumber
            new TranslationSeedItem("entity.pcbainspectiondetail.serialnumber", "zh-HK", "流水号_hk", "流水号"),

            // entity.pcbainspectiondetail.content
            new TranslationSeedItem("entity.pcbainspectiondetail.content", "en-US", "内容_us", "内容"),
            // entity.pcbainspectiondetail.content
            new TranslationSeedItem("entity.pcbainspectiondetail.content", "ja-JP", "内容_jp", "内容"),
            // entity.pcbainspectiondetail.content
            new TranslationSeedItem("entity.pcbainspectiondetail.content", "zh-CN", "内容", "内容"),
            // entity.pcbainspectiondetail.content
            new TranslationSeedItem("entity.pcbainspectiondetail.content", "zh-HK", "内容_hk", "内容"),

            // entity.pcbainspectiondetail.defectlocation
            new TranslationSeedItem("entity.pcbainspectiondetail.defectlocation", "en-US", "不良个所_us", "不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）"),
            // entity.pcbainspectiondetail.defectlocation
            new TranslationSeedItem("entity.pcbainspectiondetail.defectlocation", "ja-JP", "不良个所_jp", "不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）"),
            // entity.pcbainspectiondetail.defectlocation
            new TranslationSeedItem("entity.pcbainspectiondetail.defectlocation", "zh-CN", "不良个所", "不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）"),
            // entity.pcbainspectiondetail.defectlocation
            new TranslationSeedItem("entity.pcbainspectiondetail.defectlocation", "zh-HK", "不良个所_hk", "不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）"),

            // entity.pcbainspectiondetail.isobsolete
            new TranslationSeedItem("entity.pcbainspectiondetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbainspectiondetail.isobsolete
            new TranslationSeedItem("entity.pcbainspectiondetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbainspectiondetail.isobsolete
            new TranslationSeedItem("entity.pcbainspectiondetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbainspectiondetail.isobsolete
            new TranslationSeedItem("entity.pcbainspectiondetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.pcbainspectiondetail.pcbainspection
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspection", "en-US", "PCBA检查日报_us", "PCBA检查日报（主表）"),
            // entity.pcbainspectiondetail.pcbainspection
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspection", "ja-JP", "PCBA检查日报_jp", "PCBA检查日报（主表）"),
            // entity.pcbainspectiondetail.pcbainspection
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspection", "zh-CN", "PCBA检查日报", "PCBA检查日报（主表）"),
            // entity.pcbainspectiondetail.pcbainspection
            new TranslationSeedItem("entity.pcbainspectiondetail.pcbainspection", "zh-HK", "PCBA检查日报_hk", "PCBA检查日报（主表）"),
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
