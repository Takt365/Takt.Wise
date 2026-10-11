// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaOutputDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktPcbaOutputDetail 实体国际化翻译种子（键前缀 entity.pcbaoutputdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaOutputDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaOutputDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaoutputdetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaOutputDetailTranslations())
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

        TaktLogger.Information("TaktPcbaOutputDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaOutputDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaoutputdetail._self / entity.pcbaoutputdetail.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaOutputDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "en-US", "Pcba Output Detail Information_us", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "ja-JP", "PCBA明细信息_jp", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "zh-CN", "PCBA明细信息", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "zh-HK", "PCBA明细信息_hk", "实体名称"),

            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "en-US", "PCBA日报ID_us", "PCBA日报ID（主表主键；与 TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "ja-JP", "PCBA日报ID_jp", "PCBA日报ID（主表主键；与 TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "zh-CN", "PCBA日报ID", "PCBA日报ID（主表主键；与 TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "zh-HK", "PCBA日报ID_hk", "PCBA日报ID（主表主键；与 TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),

            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "en-US", "工单号_us", "工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "ja-JP", "工单号_jp", "工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "zh-CN", "工单号", "工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "zh-HK", "工单号_hk", "工单号（冗余字段,便于查询）"),

            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide 组成唯一键）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide 组成唯一键）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide 组成唯一键）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide 组成唯一键）"),

            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "en-US", "生产时段_us", "生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "ja-JP", "生产时段_jp", "生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "zh-CN", "生产时段", "生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "zh-HK", "生产时段_hk", "生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）"),

            // entity.pcbaoutputdetail.teamcode
            new TranslationSeedItem("entity.pcbaoutputdetail.teamcode", "en-US", "生产班组_us", "生产班组（选项 TaktProductionTeams/options?teamCategory=P；DictValue=TeamCode，ExtValue=PlantCode；与 PcbaOutputId、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.teamcode
            new TranslationSeedItem("entity.pcbaoutputdetail.teamcode", "ja-JP", "生产班组_jp", "生产班组（选项 TaktProductionTeams/options?teamCategory=P；DictValue=TeamCode，ExtValue=PlantCode；与 PcbaOutputId、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.teamcode
            new TranslationSeedItem("entity.pcbaoutputdetail.teamcode", "zh-CN", "生产班组", "生产班组（选项 TaktProductionTeams/options?teamCategory=P；DictValue=TeamCode，ExtValue=PlantCode；与 PcbaOutputId、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.teamcode
            new TranslationSeedItem("entity.pcbaoutputdetail.teamcode", "zh-HK", "生产班组_hk", "生产班组（选项 TaktProductionTeams/options?teamCategory=P；DictValue=TeamCode，ExtValue=PlantCode；与 PcbaOutputId、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),

            // entity.pcbaoutputdetail.prodequipcode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodequipcode", "en-US", "生产设备_us", "生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.pcbaoutputdetail.prodequipcode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodequipcode", "ja-JP", "生产设备_jp", "生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.pcbaoutputdetail.prodequipcode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodequipcode", "zh-CN", "生产设备", "生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),
            // entity.pcbaoutputdetail.prodequipcode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodequipcode", "zh-HK", "生产设备_hk", "生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）"),

            // entity.pcbaoutputdetail.directlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.directlabor", "en-US", "直接人员_us", "直接人员"),
            // entity.pcbaoutputdetail.directlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.directlabor", "ja-JP", "直接人员_jp", "直接人员"),
            // entity.pcbaoutputdetail.directlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.directlabor", "zh-CN", "直接人员", "直接人员"),
            // entity.pcbaoutputdetail.directlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.directlabor", "zh-HK", "直接人员_hk", "直接人员"),

            // entity.pcbaoutputdetail.indirectlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.indirectlabor", "en-US", "间接人员_us", "间接人员"),
            // entity.pcbaoutputdetail.indirectlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.indirectlabor", "ja-JP", "间接人员_jp", "间接人员"),
            // entity.pcbaoutputdetail.indirectlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.indirectlabor", "zh-CN", "间接人员", "间接人员"),
            // entity.pcbaoutputdetail.indirectlabor
            new TranslationSeedItem("entity.pcbaoutputdetail.indirectlabor", "zh-HK", "间接人员_hk", "间接人员"),

            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "en-US", "班次_us", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班；与 PcbaOutputId、TeamCode、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "ja-JP", "班次_jp", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班；与 PcbaOutputId、TeamCode、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "zh-CN", "班次", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班；与 PcbaOutputId、TeamCode、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "zh-HK", "班次_hk", "班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班；与 PcbaOutputId、TeamCode、PcbBoardType、PanelSide、LineNumber 组成唯一键）"),

            // entity.pcbaoutputdetail.stdminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.stdminutes", "en-US", "标准工时_us", "标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）"),
            // entity.pcbaoutputdetail.stdminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.stdminutes", "ja-JP", "标准工时_jp", "标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）"),
            // entity.pcbaoutputdetail.stdminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.stdminutes", "zh-CN", "标准工时", "标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）"),
            // entity.pcbaoutputdetail.stdminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.stdminutes", "zh-HK", "标准工时_hk", "标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）"),

            // entity.pcbaoutputdetail.stdlaborcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdlaborcapacity", "en-US", "人员标准产能_us", "人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）"),
            // entity.pcbaoutputdetail.stdlaborcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdlaborcapacity", "ja-JP", "人员标准产能_jp", "人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）"),
            // entity.pcbaoutputdetail.stdlaborcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdlaborcapacity", "zh-CN", "人员标准产能", "人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）"),
            // entity.pcbaoutputdetail.stdlaborcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdlaborcapacity", "zh-HK", "人员标准产能_hk", "人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）"),

            // entity.pcbaoutputdetail.stdshorts
            new TranslationSeedItem("entity.pcbaoutputdetail.stdshorts", "en-US", "标准点数_us", "标准点数（PCBA 专用，按工作中心回填）"),
            // entity.pcbaoutputdetail.stdshorts
            new TranslationSeedItem("entity.pcbaoutputdetail.stdshorts", "ja-JP", "标准点数_jp", "标准点数（PCBA 专用，按工作中心回填）"),
            // entity.pcbaoutputdetail.stdshorts
            new TranslationSeedItem("entity.pcbaoutputdetail.stdshorts", "zh-CN", "标准点数", "标准点数（PCBA 专用，按工作中心回填）"),
            // entity.pcbaoutputdetail.stdshorts
            new TranslationSeedItem("entity.pcbaoutputdetail.stdshorts", "zh-HK", "标准点数_hk", "标准点数（PCBA 专用，按工作中心回填）"),

            // entity.pcbaoutputdetail.stdequipmentcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdequipmentcapacity", "en-US", "设备标准产能_us", "设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）"),
            // entity.pcbaoutputdetail.stdequipmentcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdequipmentcapacity", "ja-JP", "设备标准产能_jp", "设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）"),
            // entity.pcbaoutputdetail.stdequipmentcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdequipmentcapacity", "zh-CN", "设备标准产能", "设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）"),
            // entity.pcbaoutputdetail.stdequipmentcapacity
            new TranslationSeedItem("entity.pcbaoutputdetail.stdequipmentcapacity", "zh-HK", "设备标准产能_hk", "设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）"),

            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "en-US", "PCB板别_us", "PCB板别（字典 logistics_manufacturing_pcba_function；存 DictLabel；UI 提交由前端 dict-type 转换；与 PcbaOutputId、TeamCode、ShiftNo、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "ja-JP", "PCB板别_jp", "PCB板别（字典 logistics_manufacturing_pcba_function；存 DictLabel；UI 提交由前端 dict-type 转换；与 PcbaOutputId、TeamCode、ShiftNo、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "zh-CN", "PCB板别", "PCB板别（字典 logistics_manufacturing_pcba_function；存 DictLabel；UI 提交由前端 dict-type 转换；与 PcbaOutputId、TeamCode、ShiftNo、PanelSide、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "zh-HK", "PCB板别_hk", "PCB板别（字典 logistics_manufacturing_pcba_function；存 DictLabel；UI 提交由前端 dict-type 转换；与 PcbaOutputId、TeamCode、ShiftNo、PanelSide、LineNumber 组成唯一键）"),

            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "en-US", "面板别_us", "面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "ja-JP", "面板别_jp", "面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "zh-CN", "面板别", "面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、LineNumber 组成唯一键）"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "zh-HK", "面板别_hk", "面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面；与 PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、LineNumber 组成唯一键）"),

            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "en-US", "批次数量_us", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "ja-JP", "批次数量_jp", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "zh-CN", "批次数量", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "zh-HK", "批次数量_hk", "批次数量"),

            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "en-US", "当日完成数_us", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "ja-JP", "当日完成数_jp", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "zh-CN", "当日完成数", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "zh-HK", "当日完成数_hk", "当日完成数"),

            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "en-US", "累计完成数_us", "累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "ja-JP", "累计完成数_jp", "累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "zh-CN", "累计完成数", "累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "zh-HK", "累计完成数_hk", "累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）"),

            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "en-US", "完成状态_us", "完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "ja-JP", "完成状态_jp", "完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "zh-CN", "完成状态", "完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "zh-HK", "完成状态_hk", "完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）"),

            // entity.pcbaoutputdetail.serialcode
            new TranslationSeedItem("entity.pcbaoutputdetail.serialcode", "en-US", "序列号_us", "序列号（明细级）"),
            // entity.pcbaoutputdetail.serialcode
            new TranslationSeedItem("entity.pcbaoutputdetail.serialcode", "ja-JP", "序列号_jp", "序列号（明细级）"),
            // entity.pcbaoutputdetail.serialcode
            new TranslationSeedItem("entity.pcbaoutputdetail.serialcode", "zh-CN", "序列号", "序列号（明细级）"),
            // entity.pcbaoutputdetail.serialcode
            new TranslationSeedItem("entity.pcbaoutputdetail.serialcode", "zh-HK", "序列号_hk", "序列号（明细级）"),

            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "en-US", "不良台数_us", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "ja-JP", "不良台数_jp", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "zh-CN", "不良台数", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "zh-HK", "不良台数_hk", "不良台数"),

            // entity.pcbaoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimeminutes", "en-US", "停线时间_us", "停线时间(分钟)"),
            // entity.pcbaoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimeminutes", "ja-JP", "停线时间_jp", "停线时间(分钟)"),
            // entity.pcbaoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimeminutes", "zh-CN", "停线时间", "停线时间(分钟)"),
            // entity.pcbaoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimeminutes", "zh-HK", "停线时间_hk", "停线时间(分钟)"),

            // entity.pcbaoutputdetail.downtimereason
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimereason", "en-US", "停线原因_us", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.pcbaoutputdetail.downtimereason
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimereason", "ja-JP", "停线原因_jp", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.pcbaoutputdetail.downtimereason
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimereason", "zh-CN", "停线原因", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.pcbaoutputdetail.downtimereason
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimereason", "zh-HK", "停线原因_hk", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),

            // entity.pcbaoutputdetail.downtimedescription
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimedescription", "en-US", "停线说明_us", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.downtimedescription
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimedescription", "ja-JP", "停线说明_jp", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.downtimedescription
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimedescription", "zh-CN", "停线说明", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.downtimedescription
            new TranslationSeedItem("entity.pcbaoutputdetail.downtimedescription", "zh-HK", "停线说明_hk", "停线说明（停线原因选定后的补充详细说明）"),

            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "en-US", "投入工数_us", "投入工数(分钟)（计算结果：明细 DirectLabor×60）"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "ja-JP", "投入工数_jp", "投入工数(分钟)（计算结果：明细 DirectLabor×60）"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "zh-CN", "投入工数", "投入工数(分钟)（计算结果：明细 DirectLabor×60）"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "zh-HK", "投入工数_hk", "投入工数(分钟)（计算结果：明细 DirectLabor×60）"),

            // entity.pcbaoutputdetail.actualminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.actualminutes", "en-US", "实际工时_us", "实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）"),
            // entity.pcbaoutputdetail.actualminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.actualminutes", "ja-JP", "实际工时_jp", "实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）"),
            // entity.pcbaoutputdetail.actualminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.actualminutes", "zh-CN", "实际工时", "实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）"),
            // entity.pcbaoutputdetail.actualminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.actualminutes", "zh-HK", "实际工时_hk", "实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）"),

            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "en-US", "修工数_us", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "ja-JP", "修工数_jp", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "zh-CN", "修工数", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "zh-HK", "修工数_hk", "修工数(分钟)"),

            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "en-US", "切换次数_us", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "ja-JP", "切换次数_jp", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "zh-CN", "切换次数", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "zh-HK", "切换次数_hk", "切换次数"),

            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "en-US", "切换时间_us", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "ja-JP", "切换时间_jp", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "zh-CN", "切换时间", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "zh-HK", "切换时间_hk", "切换时间(分钟)"),

            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "en-US", "切停机时间_us", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "ja-JP", "切停机时间_jp", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "zh-CN", "切停机时间", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "zh-HK", "切停机时间_hk", "切停机时间(分钟)"),

            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "en-US", "总工数_us", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "ja-JP", "总工数_jp", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "zh-CN", "总工数", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "zh-HK", "总工数_hk", "总工数(分钟)"),

            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "en-US", "未达成原因_us", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "ja-JP", "未达成原因_jp", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "zh-CN", "未达成原因", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "zh-HK", "未达成原因_hk", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),

            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "en-US", "未达成说明_us", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "ja-JP", "未达成说明_jp", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "zh-CN", "未达成说明", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "zh-HK", "未达成说明_hk", "未达成说明（未达成原因选定后的补充详细说明）"),

            // entity.pcbaoutputdetail.confirmminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.confirmminutes", "en-US", "报工工时_us", "报工工时(分钟)"),
            // entity.pcbaoutputdetail.confirmminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.confirmminutes", "ja-JP", "报工工时_jp", "报工工时(分钟)"),
            // entity.pcbaoutputdetail.confirmminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.confirmminutes", "zh-CN", "报工工时", "报工工时(分钟)"),
            // entity.pcbaoutputdetail.confirmminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.confirmminutes", "zh-HK", "报工工时_hk", "报工工时(分钟)"),

            // entity.pcbaoutputdetail.mixedprod
            new TranslationSeedItem("entity.pcbaoutputdetail.mixedprod", "en-US", "混合生产_us", "混合生产（0=非混合；N=此生产时段内另有N笔报工）"),
            // entity.pcbaoutputdetail.mixedprod
            new TranslationSeedItem("entity.pcbaoutputdetail.mixedprod", "ja-JP", "混合生产_jp", "混合生产（0=非混合；N=此生产时段内另有N笔报工）"),
            // entity.pcbaoutputdetail.mixedprod
            new TranslationSeedItem("entity.pcbaoutputdetail.mixedprod", "zh-CN", "混合生产", "混合生产（0=非混合；N=此生产时段内另有N笔报工）"),
            // entity.pcbaoutputdetail.mixedprod
            new TranslationSeedItem("entity.pcbaoutputdetail.mixedprod", "zh-HK", "混合生产_hk", "混合生产（0=非混合；N=此生产时段内另有N笔报工）"),

            // entity.pcbaoutputdetail.achievementrate
            new TranslationSeedItem("entity.pcbaoutputdetail.achievementrate", "en-US", "达成率_us", "达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）"),
            // entity.pcbaoutputdetail.achievementrate
            new TranslationSeedItem("entity.pcbaoutputdetail.achievementrate", "ja-JP", "达成率_jp", "达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）"),
            // entity.pcbaoutputdetail.achievementrate
            new TranslationSeedItem("entity.pcbaoutputdetail.achievementrate", "zh-CN", "达成率", "达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）"),
            // entity.pcbaoutputdetail.achievementrate
            new TranslationSeedItem("entity.pcbaoutputdetail.achievementrate", "zh-HK", "达成率_hk", "达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）"),

            // entity.pcbaoutputdetail.isobsolete
            new TranslationSeedItem("entity.pcbaoutputdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbaoutputdetail.isobsolete
            new TranslationSeedItem("entity.pcbaoutputdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbaoutputdetail.isobsolete
            new TranslationSeedItem("entity.pcbaoutputdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.pcbaoutputdetail.isobsolete
            new TranslationSeedItem("entity.pcbaoutputdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "en-US", "PCBA日报_us", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "ja-JP", "PCBA日报_jp", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "zh-CN", "PCBA日报", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "zh-HK", "PCBA日报_hk", "PCBA日报（主表）"),
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
        translation.ResourceGroup = "Output";
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
