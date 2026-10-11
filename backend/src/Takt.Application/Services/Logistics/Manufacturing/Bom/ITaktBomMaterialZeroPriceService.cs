// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialZeroPriceService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单服务接口（独立模块）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 组件零价格清单服务（FERT+未删除；X+F+移动平均价=0；建议代替末字母逆推）
/// 查询栏工厂/机种选项统一走 ITaktBomMaterialCostAnalysisService。
/// </summary>
public interface ITaktBomMaterialZeroPriceService
{
    /// <summary>
    /// 组件零价格合并清单（工厂+核算月；机种可选多选空=全部；仅主表 MaterialType=FERT 且未删除；X+F 且移动平均价=0；建议代替按末字母逆推）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页合并结果</returns>
    Task<TaktBomMaterialZeroPriceResultDto> GetBomMaterialZeroPriceListAsync(
        TaktBomMaterialZeroPriceQueryDto queryDto);

    /// <summary>
    /// 导出组件零价格合并清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialZeroPriceAsync(
        TaktBomMaterialZeroPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 按当前条件回填移动平均价（PcbSectIndicator=X 或用量≤0 永不回填；ComponentCode 空=批量）
    /// </summary>
    /// <param name="dto">工厂+核算月；组件可选；机种可选</param>
    /// <returns>回填统计</returns>
    Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> BackfillBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceMovingBackfillDto dto);

    /// <summary>
    /// 手工替换更新移动平均价（PcbSectIndicator=X 或用量≤0 永不回填；主表产品/机种月成本+ExtField）
    /// </summary>
    /// <param name="dto">工厂+核算月+原组件+新组件+价/单位/币种</param>
    /// <returns>更新统计</returns>
    Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ManualUpdateBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceManualMovingDto dto);

    /// <summary>
    /// 将当前工厂+核算月（机种可选）范围内 BOM 展开树中 PCB SECT 整树明细写入 PcbSectIndicator=X
    /// </summary>
    /// <param name="dto">工厂+核算月；机种可选</param>
    /// <returns>打标统计</returns>
    Task<TaktBomMaterialZeroPricePcbSectMarkResultDto> MarkBomMaterialZeroPricePcbSectAsync(
        TaktBomMaterialZeroPricePcbSectMarkDto dto);

    /// <summary>
    /// 去掉当前工厂纯数字 ProductCode/ComponentCode 的前导 0（含字母的编码不改；明细含年分表）
    /// </summary>
    /// <param name="dto">工厂</param>
    /// <returns>更新与冲突跳过行数</returns>
    Task<TaktBomMaterialZeroPriceStripLeadingZeroResultDto> StripBomMaterialZeroPriceLeadingZerosAsync(
        TaktBomMaterialZeroPriceStripLeadingZeroDto dto);

    /// <summary>
    /// Quartz：判定日所在自然月，按明细/主表行内 PlantCode 回填零价移动价
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>回填统计；无工厂数据时 null</returns>
    Task<TaktBomMaterialZeroPriceMovingBackfillResultDto?> RunScheduledBomMaterialZeroPriceMovingBackfillAsync(
        DateTime? asOfDate = null);
}
