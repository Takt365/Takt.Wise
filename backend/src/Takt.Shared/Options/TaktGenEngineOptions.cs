// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktGenEngineOptions.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成引擎配置（wwwroot/Generator 模板根路径）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 代码生成引擎配置选项
/// </summary>
public class TaktGenEngineOptions
{
    /// <summary>
    /// Web 内容根路径（用于定位 wwwroot/Generator；由 WebApi 启动时注入 ContentRootPath）
    /// </summary>
    public string? ContentRootPath { get; set; }
}
