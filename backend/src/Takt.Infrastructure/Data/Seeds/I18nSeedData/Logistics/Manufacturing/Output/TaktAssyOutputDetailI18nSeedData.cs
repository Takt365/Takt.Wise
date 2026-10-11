// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyOutputDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssyOutputDetail 实体国际化翻译种子（键前缀 entity.assyoutputdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyOutputDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyOutputDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assyoutputdetail 实体翻译...", tenantCode);

        foreach (var item in GetAssyOutputDetailTranslations())
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

        TaktLogger.Information("TaktAssyOutputDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyOutputDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assyoutputdetail._self / entity.assyoutputdetail.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyOutputDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assyoutputdetail._self
            new TranslationSeedItem("entity.assyoutputdetail._self", "en-US", "Assy Output Detail Information_us", "实体名称"),
            // entity.assyoutputdetail._self
            new TranslationSeedItem("entity.assyoutputdetail._self", "ja-JP", "组立日报明细信息_jp", "实体名称"),
            // entity.assyoutputdetail._self
            new TranslationSeedItem("entity.assyoutputdetail._self", "zh-CN", "组立日报明细信息", "实体名称"),
            // entity.assyoutputdetail._self
            new TranslationSeedItem("entity.assyoutputdetail._self", "zh-HK", "组立日报明细信息_hk", "实体名称"),

            // entity.assyoutputdetail.assyoutputid
            new TranslationSeedItem("entity.assyoutputdetail.assyoutputid", "en-US", "组立日报ID_us", "组立日报ID（主表主键；与 TimePeriod、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assyoutputdetail.assyoutputid
            new TranslationSeedItem("entity.assyoutputdetail.assyoutputid", "ja-JP", "组立日报ID_jp", "组立日报ID（主表主键；与 TimePeriod、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assyoutputdetail.assyoutputid
            new TranslationSeedItem("entity.assyoutputdetail.assyoutputid", "zh-CN", "组立日报ID", "组立日报ID（主表主键；与 TimePeriod、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),
            // entity.assyoutputdetail.assyoutputid
            new TranslationSeedItem("entity.assyoutputdetail.assyoutputid", "zh-HK", "组立日报ID_hk", "组立日报ID（主表主键；与 TimePeriod、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）"),

            // entity.assyoutputdetail.prodordercode
            new TranslationSeedItem("entity.assyoutputdetail.prodordercode", "en-US", "工单号_us", "工单号（冗余字段,便于查询）"),
            // entity.assyoutputdetail.prodordercode
            new TranslationSeedItem("entity.assyoutputdetail.prodordercode", "ja-JP", "工单号_jp", "工单号（冗余字段,便于查询）"),
            // entity.assyoutputdetail.prodordercode
            new TranslationSeedItem("entity.assyoutputdetail.prodordercode", "zh-CN", "工单号", "工单号（冗余字段,便于查询）"),
            // entity.assyoutputdetail.prodordercode
            new TranslationSeedItem("entity.assyoutputdetail.prodordercode", "zh-HK", "工单号_hk", "工单号（冗余字段,便于查询）"),

            // entity.assyoutputdetail.linenumber
            new TranslationSeedItem("entity.assyoutputdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10；与 AssyOutputId、TimePeriod 组成唯一键）"),
            // entity.assyoutputdetail.linenumber
            new TranslationSeedItem("entity.assyoutputdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10；与 AssyOutputId、TimePeriod 组成唯一键）"),
            // entity.assyoutputdetail.linenumber
            new TranslationSeedItem("entity.assyoutputdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10；与 AssyOutputId、TimePeriod 组成唯一键）"),
            // entity.assyoutputdetail.linenumber
            new TranslationSeedItem("entity.assyoutputdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10；与 AssyOutputId、TimePeriod 组成唯一键）"),

            // entity.assyoutputdetail.timeperiod
            new TranslationSeedItem("entity.assyoutputdetail.timeperiod", "en-US", "生产时段_us", "生产时段（固定值；与 AssyOutputId、LineNumber 组成唯一键）"),
            // entity.assyoutputdetail.timeperiod
            new TranslationSeedItem("entity.assyoutputdetail.timeperiod", "ja-JP", "生产时段_jp", "生产时段（固定值；与 AssyOutputId、LineNumber 组成唯一键）"),
            // entity.assyoutputdetail.timeperiod
            new TranslationSeedItem("entity.assyoutputdetail.timeperiod", "zh-CN", "生产时段", "生产时段（固定值；与 AssyOutputId、LineNumber 组成唯一键）"),
            // entity.assyoutputdetail.timeperiod
            new TranslationSeedItem("entity.assyoutputdetail.timeperiod", "zh-HK", "生产时段_hk", "生产时段（固定值；与 AssyOutputId、LineNumber 组成唯一键）"),

            // entity.assyoutputdetail.stdcapacity
            new TranslationSeedItem("entity.assyoutputdetail.stdcapacity", "en-US", "标准产能_us", "标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）"),
            // entity.assyoutputdetail.stdcapacity
            new TranslationSeedItem("entity.assyoutputdetail.stdcapacity", "ja-JP", "标准产能_jp", "标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）"),
            // entity.assyoutputdetail.stdcapacity
            new TranslationSeedItem("entity.assyoutputdetail.stdcapacity", "zh-CN", "标准产能", "标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）"),
            // entity.assyoutputdetail.stdcapacity
            new TranslationSeedItem("entity.assyoutputdetail.stdcapacity", "zh-HK", "标准产能_hk", "标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）"),

            // entity.assyoutputdetail.prodactualqty
            new TranslationSeedItem("entity.assyoutputdetail.prodactualqty", "en-US", "实际生产数量_us", "实际生产数量"),
            // entity.assyoutputdetail.prodactualqty
            new TranslationSeedItem("entity.assyoutputdetail.prodactualqty", "ja-JP", "实际生产数量_jp", "实际生产数量"),
            // entity.assyoutputdetail.prodactualqty
            new TranslationSeedItem("entity.assyoutputdetail.prodactualqty", "zh-CN", "实际生产数量", "实际生产数量"),
            // entity.assyoutputdetail.prodactualqty
            new TranslationSeedItem("entity.assyoutputdetail.prodactualqty", "zh-HK", "实际生产数量_hk", "实际生产数量"),

            // entity.assyoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.assyoutputdetail.downtimeminutes", "en-US", "停线时间_us", "停线时间(分钟)"),
            // entity.assyoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.assyoutputdetail.downtimeminutes", "ja-JP", "停线时间_jp", "停线时间(分钟)"),
            // entity.assyoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.assyoutputdetail.downtimeminutes", "zh-CN", "停线时间", "停线时间(分钟)"),
            // entity.assyoutputdetail.downtimeminutes
            new TranslationSeedItem("entity.assyoutputdetail.downtimeminutes", "zh-HK", "停线时间_hk", "停线时间(分钟)"),

            // entity.assyoutputdetail.downtimereason
            new TranslationSeedItem("entity.assyoutputdetail.downtimereason", "en-US", "停线原因_us", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.assyoutputdetail.downtimereason
            new TranslationSeedItem("entity.assyoutputdetail.downtimereason", "ja-JP", "停线原因_jp", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.assyoutputdetail.downtimereason
            new TranslationSeedItem("entity.assyoutputdetail.downtimereason", "zh-CN", "停线原因", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),
            // entity.assyoutputdetail.downtimereason
            new TranslationSeedItem("entity.assyoutputdetail.downtimereason", "zh-HK", "停线原因_hk", "停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）"),

            // entity.assyoutputdetail.downtimedescription
            new TranslationSeedItem("entity.assyoutputdetail.downtimedescription", "en-US", "停线说明_us", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.downtimedescription
            new TranslationSeedItem("entity.assyoutputdetail.downtimedescription", "ja-JP", "停线说明_jp", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.downtimedescription
            new TranslationSeedItem("entity.assyoutputdetail.downtimedescription", "zh-CN", "停线说明", "停线说明（停线原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.downtimedescription
            new TranslationSeedItem("entity.assyoutputdetail.downtimedescription", "zh-HK", "停线说明_hk", "停线说明（停线原因选定后的补充详细说明）"),

            // entity.assyoutputdetail.unachievedreason
            new TranslationSeedItem("entity.assyoutputdetail.unachievedreason", "en-US", "未达成原因_us", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.assyoutputdetail.unachievedreason
            new TranslationSeedItem("entity.assyoutputdetail.unachievedreason", "ja-JP", "未达成原因_jp", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.assyoutputdetail.unachievedreason
            new TranslationSeedItem("entity.assyoutputdetail.unachievedreason", "zh-CN", "未达成原因", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),
            // entity.assyoutputdetail.unachievedreason
            new TranslationSeedItem("entity.assyoutputdetail.unachievedreason", "zh-HK", "未达成原因_hk", "未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）"),

            // entity.assyoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.assyoutputdetail.unachieveddescription", "en-US", "未达成说明_us", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.assyoutputdetail.unachieveddescription", "ja-JP", "未达成说明_jp", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.assyoutputdetail.unachieveddescription", "zh-CN", "未达成说明", "未达成说明（未达成原因选定后的补充详细说明）"),
            // entity.assyoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.assyoutputdetail.unachieveddescription", "zh-HK", "未达成说明_hk", "未达成说明（未达成原因选定后的补充详细说明）"),

            // entity.assyoutputdetail.inputminutes
            new TranslationSeedItem("entity.assyoutputdetail.inputminutes", "en-US", "投入工时_us", "投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）"),
            // entity.assyoutputdetail.inputminutes
            new TranslationSeedItem("entity.assyoutputdetail.inputminutes", "ja-JP", "投入工时_jp", "投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）"),
            // entity.assyoutputdetail.inputminutes
            new TranslationSeedItem("entity.assyoutputdetail.inputminutes", "zh-CN", "投入工时", "投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）"),
            // entity.assyoutputdetail.inputminutes
            new TranslationSeedItem("entity.assyoutputdetail.inputminutes", "zh-HK", "投入工时_hk", "投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）"),

            // entity.assyoutputdetail.actualminutes
            new TranslationSeedItem("entity.assyoutputdetail.actualminutes", "en-US", "实际工时_us", "实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）"),
            // entity.assyoutputdetail.actualminutes
            new TranslationSeedItem("entity.assyoutputdetail.actualminutes", "ja-JP", "实际工时_jp", "实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）"),
            // entity.assyoutputdetail.actualminutes
            new TranslationSeedItem("entity.assyoutputdetail.actualminutes", "zh-CN", "实际工时", "实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）"),
            // entity.assyoutputdetail.actualminutes
            new TranslationSeedItem("entity.assyoutputdetail.actualminutes", "zh-HK", "实际工时_hk", "实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）"),

            // entity.assyoutputdetail.indirectminutes
            new TranslationSeedItem("entity.assyoutputdetail.indirectminutes", "en-US", "间接工时_us", "间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）"),
            // entity.assyoutputdetail.indirectminutes
            new TranslationSeedItem("entity.assyoutputdetail.indirectminutes", "ja-JP", "间接工时_jp", "间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）"),
            // entity.assyoutputdetail.indirectminutes
            new TranslationSeedItem("entity.assyoutputdetail.indirectminutes", "zh-CN", "间接工时", "间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）"),
            // entity.assyoutputdetail.indirectminutes
            new TranslationSeedItem("entity.assyoutputdetail.indirectminutes", "zh-HK", "间接工时_hk", "间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）"),

            // entity.assyoutputdetail.confirmminutes
            new TranslationSeedItem("entity.assyoutputdetail.confirmminutes", "en-US", "报工工时_us", "报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）"),
            // entity.assyoutputdetail.confirmminutes
            new TranslationSeedItem("entity.assyoutputdetail.confirmminutes", "ja-JP", "报工工时_jp", "报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）"),
            // entity.assyoutputdetail.confirmminutes
            new TranslationSeedItem("entity.assyoutputdetail.confirmminutes", "zh-CN", "报工工时", "报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）"),
            // entity.assyoutputdetail.confirmminutes
            new TranslationSeedItem("entity.assyoutputdetail.confirmminutes", "zh-HK", "报工工时_hk", "报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）"),

            // entity.assyoutputdetail.mixedprod
            new TranslationSeedItem("entity.assyoutputdetail.mixedprod", "en-US", "混合生产_us", "混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）"),
            // entity.assyoutputdetail.mixedprod
            new TranslationSeedItem("entity.assyoutputdetail.mixedprod", "ja-JP", "混合生产_jp", "混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）"),
            // entity.assyoutputdetail.mixedprod
            new TranslationSeedItem("entity.assyoutputdetail.mixedprod", "zh-CN", "混合生产", "混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）"),
            // entity.assyoutputdetail.mixedprod
            new TranslationSeedItem("entity.assyoutputdetail.mixedprod", "zh-HK", "混合生产_hk", "混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）"),

            // entity.assyoutputdetail.achievementrate
            new TranslationSeedItem("entity.assyoutputdetail.achievementrate", "en-US", "达成率_us", "达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）"),
            // entity.assyoutputdetail.achievementrate
            new TranslationSeedItem("entity.assyoutputdetail.achievementrate", "ja-JP", "达成率_jp", "达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）"),
            // entity.assyoutputdetail.achievementrate
            new TranslationSeedItem("entity.assyoutputdetail.achievementrate", "zh-CN", "达成率", "达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）"),
            // entity.assyoutputdetail.achievementrate
            new TranslationSeedItem("entity.assyoutputdetail.achievementrate", "zh-HK", "达成率_hk", "达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）"),

            // entity.assyoutputdetail.isobsolete
            new TranslationSeedItem("entity.assyoutputdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assyoutputdetail.isobsolete
            new TranslationSeedItem("entity.assyoutputdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assyoutputdetail.isobsolete
            new TranslationSeedItem("entity.assyoutputdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.assyoutputdetail.isobsolete
            new TranslationSeedItem("entity.assyoutputdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.assyoutputdetail.assyoutput
            new TranslationSeedItem("entity.assyoutputdetail.assyoutput", "en-US", "组立日报_us", "组立日报（主表）"),
            // entity.assyoutputdetail.assyoutput
            new TranslationSeedItem("entity.assyoutputdetail.assyoutput", "ja-JP", "组立日报_jp", "组立日报（主表）"),
            // entity.assyoutputdetail.assyoutput
            new TranslationSeedItem("entity.assyoutputdetail.assyoutput", "zh-CN", "组立日报", "组立日报（主表）"),
            // entity.assyoutputdetail.assyoutput
            new TranslationSeedItem("entity.assyoutputdetail.assyoutput", "zh-HK", "组立日报_hk", "组立日报（主表）"),
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
