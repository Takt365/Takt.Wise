// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialZeroPricesController.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单控制器（FERT+未删除；独立 DTO/服务）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 组件零价格清单控制器（独立菜单：list / export）
/// 查询栏工厂/机种选项走 TaktBomCostOptions。
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM零价格")]
public class TaktBomMaterialZeroPricesController : TaktControllerBase
{
    private readonly ITaktBomMaterialZeroPriceService _bomMaterialZeroPriceService;
    /// <summary>
    /// 在线消息（操作结果落库；导出不落库）
    /// </summary>
    private readonly ITaktMessageService _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialZeroPriceService">零价格清单服务</param>
    /// <param name="messageService">在线消息服务</param>
    public TaktBomMaterialZeroPricesController(
        ITaktBomMaterialZeroPriceService bomMaterialZeroPriceService,
        ITaktMessageService messageService)
    {
        _bomMaterialZeroPriceService = bomMaterialZeroPriceService;
        _messageService = messageService;
    }

    /// <summary>
    /// 组件零价格合并清单（工厂+核算月；机种可选多选；仅 FERT 且未删除；X+F 且移动平均价=0）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>合并结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:list", "BOM零价格")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomMaterialZeroPriceListAsync(
        [FromQuery] TaktBomMaterialZeroPriceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialZeroPriceService.GetBomMaterialZeroPriceListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出组件零价格合并清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:export", "导出BOM零价格")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomMaterialZeroPriceAsync(
        [FromQuery] TaktBomMaterialZeroPriceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialZeroPriceService
                .ExportBomMaterialZeroPriceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 回填移动平均价（ComponentCode 空=按当前条件批量；有值=操作列单条）
    /// </summary>
    /// <param name="dto">回填条件</param>
    /// <returns>统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "回填BOM零价格移动价")]
    [HttpPost("backfill-moving-price")]
    public async Task<IActionResult> BackfillBomMaterialZeroPriceMovingAsync(
        [FromBody] TaktBomMaterialZeroPriceMovingBackfillDto dto)
    {
        try
        {
            var result = await _bomMaterialZeroPriceService.BackfillBomMaterialZeroPriceMovingAsync(dto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildMovingPriceBackfillSuccess(result, isManual: false));
            return Success(result, "回填成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 手工替换更新移动平均价（原组件 ← 新组件价/单位/币种）
    /// </summary>
    /// <param name="dto">原组件 + 新组件 + 价/单位/币种</param>
    /// <returns>统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "手工替换BOM零价格移动价")]
    [HttpPost("manual-moving-price")]
    public async Task<IActionResult> ManualUpdateBomMaterialZeroPriceMovingAsync(
        [FromBody] TaktBomMaterialZeroPriceManualMovingDto dto)
    {
        try
        {
            var result = await _bomMaterialZeroPriceService.ManualUpdateBomMaterialZeroPriceMovingAsync(dto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildMovingPriceBackfillSuccess(result, isManual: true));
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// PCB SECT 整树 ExtField 打标（pcbSect=X；工厂+核算月；机种可选）
    /// </summary>
    /// <param name="dto">打标条件</param>
    /// <returns>统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "标记BOM零价格PCB SECT")]
    [HttpPost("mark-pcb-sect")]
    public async Task<IActionResult> MarkBomMaterialZeroPricePcbSectAsync(
        [FromBody] TaktBomMaterialZeroPricePcbSectMarkDto dto)
    {
        try
        {
            var result = await _bomMaterialZeroPriceService.MarkBomMaterialZeroPricePcbSectAsync(dto);
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                _messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildPcbSectMarkSuccess(result));
            return Success(result, "标记成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 去掉当前工厂纯数字产品/组件编码的前导 0
    /// </summary>
    /// <param name="dto">工厂</param>
    /// <returns>更新统计</returns>
    [TaktPermission("logistics:manufacturing:bom:material:zeroprice:update", "去掉BOM编码前导0")]
    [HttpPost("strip-leading-zeros")]
    public async Task<IActionResult> StripBomMaterialZeroPriceLeadingZerosAsync(
        [FromBody] TaktBomMaterialZeroPriceStripLeadingZeroDto dto)
    {
        try
        {
            var result = await _bomMaterialZeroPriceService.StripBomMaterialZeroPriceLeadingZerosAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
