// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktStaticFilesCollectionExtensions.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：非 wwwroot 本地文件静态映射（默认 wwwroot 时跳过，与 TaktFileUploadEngine 一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 本地文件静态资源扩展
/// </summary>
public static class TaktStaticFilesCollectionExtensions
{
    /// <summary>
    /// 当 UploadStorageRootPath 显式指向 wwwroot 之外时，映射 /uploads 等根相对 URL
    /// </summary>
    /// <param name="app">Web 应用</param>
    /// <returns>Web 应用</returns>
    public static WebApplication UseTaktLocalUploadStaticFiles(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        var uploadOptions = app.Configuration
            .GetSection(TaktFileUploadOptions.SectionName)
            .Get<TaktFileUploadOptions>() ?? new TaktFileUploadOptions();
        var contentRoot = app.Environment.ContentRootPath;
        var uploadRoot = TaktFileHelper.ResolveLocalUploadStorageRootPath(
            contentRoot,
            uploadOptions.UploadStorageRootPath);
        var wwwroot = TaktFileHelper.GetWwwRootPath(contentRoot);
        if (string.Equals(
            Path.GetFullPath(uploadRoot),
            Path.GetFullPath(wwwroot),
            StringComparison.OrdinalIgnoreCase))
        {
            return app;
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadRoot),
            RequestPath = "",
        });
        return app;
    }
}
