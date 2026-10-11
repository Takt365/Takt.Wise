// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomCalculateService.cs
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 计算服务接口（计算成本 / 重算成本 / 计算平均成本 / 回填采购价 / 计算最近采购成本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 计算服务（计算成本 / 重算成本 / 计算平均成本 / 回填采购价 / 计算最近采购成本；与成本分析 CRUD 分离）
/// </summary>
public interface ITaktBomCalculateService
{
    /// <summary>
    /// 查询栏工厂选项：当前公司 RelatedPlant ∩ 成本主表 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomCalculatePlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 计算成本：明细按工厂+产品+核算月合计写入主表（有则更新；明细有而主表无则回填新建；按查询所选物料类型，空=全部类型），再刷新机种月均
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>合计统计</returns>
    Task<TaktBomCalculateCostResultDto> SumBomCalculateCostAsync(TaktBomCalculateQueryDto queryDto);

    /// <summary>
    /// 重算成本：将主表旧产品月成本追加到 ExtField JSON（核算日 yyyy/M/d → 成本）后按明细重写（有则更新；明细有而主表无则回填新建；按查询所选物料类型，空=全部类型），再刷新机种月均
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>重算统计</returns>
    Task<TaktBomCalculateCostResultDto> RecalculateBomCalculateCostAsync(TaktBomCalculateQueryDto queryDto);

    /// <summary>
    /// 计算平均成本：先回填空机种/空物料类型，再按工厂+物料类型+机种+月份写机种月均（始终全部 MaterialType）
    /// </summary>
    /// <param name="queryDto">工厂 + 核算期间；机种可选；MaterialType 忽略</param>
    /// <returns>平均结果</returns>
    Task<TaktBomCalculateAverageResultDto> CalculateBomCalculateAverageAsync(
        TaktBomCalculateAverageQueryDto queryDto);

    /// <summary>
    /// 按核算日回填 BOM 明细采购组织/采购组/供应商/净价/采购货币/采购价格单位（最近有效采购价格）
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>回填统计</returns>
    Task<TaktBomCalculatePurchaseInfoRecordBackfillResultDto> BackfillBomCalculatePurchaseInfoRecordAsync(
        TaktBomCalculateQueryDto queryDto);

    /// <summary>
    /// 计算最近采购成本：与产品月成本同一快照口径，行金额=组件数量×(净价÷采购价格单位)，写入主表 LatestPurchaseCost
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>合计统计</returns>
    Task<TaktBomCalculateCostResultDto> SumBomCalculateLatestPurchaseCostAsync(
        TaktBomCalculateQueryDto queryDto);

    /// <summary>
    /// Quartz 计算成本：判定日所在自然月（按明细表 PlantCode 分组处理；不限定物料类型）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>合计统计</returns>
    Task<TaktBomCalculateCostResultDto?> RunScheduledBomCalculateSumAsync(DateTime? asOfDate = null);

    /// <summary>
    /// Quartz 重算成本：判定日所在自然月（按明细表 PlantCode 分组处理）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>重算统计</returns>
    Task<TaktBomCalculateCostResultDto?> RunScheduledBomCalculateRecalculateAsync(DateTime? asOfDate = null);

    /// <summary>
    /// Quartz 计算平均成本：判定日所在自然月（按主表 PlantCode 去重工厂）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>各工厂汇总；当月无主表行时返回 null</returns>
    Task<TaktBomCalculateAverageResultDto?> RunScheduledBomCalculateAverageAsync(DateTime? asOfDate = null);
}
