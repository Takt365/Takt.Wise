// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktWiseController.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：平台级公开配置（分页等，来源 appsettings，供前端 bootstrap）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 平台公开配置（无需登录）
/// </summary>
[AllowAnonymous]
public class TaktWiseController : TaktControllerBase
{
    /// <summary>
    /// 获取分页全局配置（与 appsettings Paged 节一致）
    /// </summary>
    /// <returns>分页配置 DTO</returns>
    [HttpGet("pagination")]
    public IActionResult GetPaginationConfig()
    {
        var dto = new TaktPagedConfigDto
        {
            DefaultPageIndex = TaktPagedClamp.DefaultPageIndex,
            DefaultPageSize = TaktPagedClamp.DefaultPageSize,
            MaxPageSize = TaktPagedClamp.DefaultMaxPageSize,
            PageSizeOptions = TaktPagedClamp.PageSizeOptions,
        };
        return Success(dto, "查询成功");
    }
}
