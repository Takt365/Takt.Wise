// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Database
// 文件名称：TaktTableClonesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表克隆控制器（备份预览 + 执行；库表元数据见 TaktDatabaseInfosController）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Code.Database;

/// <summary>
/// 数据表克隆控制器
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "数据表克隆")]
public class TaktTableClonesController : TaktControllerBase
{
    private readonly ITaktTableCloneService _tableCloneService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tableCloneService">整表克隆服务</param>
    public TaktTableClonesController(ITaktTableCloneService tableCloneService)
    {
        _tableCloneService = tableCloneService;
    }

    /// <summary>
    /// 获取跨租户整表克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>备份与清空预览</returns>
    [TaktPermission("code:database:table:preview", "表克隆预览")]
    [HttpPost("preview")]
    public async Task<IActionResult> GetTableClonePreviewAsync([FromBody] TaktTableCloneDto dto)
    {
        try
        {
            var result = await _tableCloneService.GetTableClonePreviewAsync(dto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 跨租户批量克隆源表数据到目标表（一次 1~5 张表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>克隆结果</returns>
    [TaktPermission("code:database:table:clone", "表克隆")]
    [HttpPost("clone")]
    public async Task<IActionResult> CloneTableAsync([FromBody] TaktTableCloneDto dto)
    {
        try
        {
            var result = await _tableCloneService.CloneTableAsync(dto);
            return Success(result, "克隆成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
