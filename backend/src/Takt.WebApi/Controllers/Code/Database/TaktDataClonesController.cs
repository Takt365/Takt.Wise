// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Database
// 文件名称：TaktDataClonesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆控制器（备份预览 + 执行；库表元数据见 TaktDatabaseInfosController）
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
/// 公司级数据克隆控制器
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "数据克隆")]
public class TaktDataClonesController : TaktControllerBase
{
    private readonly ITaktDataCloneService _dataCloneService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dataCloneService">公司级数据克隆服务</param>
    public TaktDataClonesController(ITaktDataCloneService dataCloneService)
    {
        _dataCloneService = dataCloneService;
    }

    /// <summary>
    /// 获取公司级数据克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>备份与清空预览</returns>
    [TaktPermission("code:database:data:preview", "数据克隆预览")]
    [HttpPost("preview")]
    public async Task<IActionResult> GetDataClonePreviewAsync([FromBody] TaktDataCloneDto dto)
    {
        try
        {
            var result = await _dataCloneService.GetDataClonePreviewAsync(dto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按公司范围克隆数据（一次一个源公司、一张表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>克隆结果</returns>
    [TaktPermission("code:database:data:clone", "数据克隆")]
    [HttpPost("clone")]
    public async Task<IActionResult> CloneDataAsync([FromBody] TaktDataCloneDto dto)
    {
        try
        {
            var result = await _dataCloneService.CloneDataAsync(dto);
            return Success(result, "克隆成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
