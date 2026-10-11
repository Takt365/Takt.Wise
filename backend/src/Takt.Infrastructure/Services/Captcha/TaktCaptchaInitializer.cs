// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Captcha
// 文件名称：TaktCaptchaInitializer.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：应用启动时初始化验证码（Slider 模板/背景资源校验与下载，试生成）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services.Captcha;

/// <summary>
/// 验证码启动初始化服务（IHostedService）。
/// 当 <c>Captcha:Enabled</c> 为 true 且 <c>Captcha:Type</c> 为 Slider 时，校验 wwwroot 下模板并补足背景图；
/// 最后试生成一条验证码以验证 ITaktCaptchaService 可用。失败不阻断应用启动。
/// </summary>
public class TaktCaptchaInitializer : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly TaktCaptchaOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// 初始化验证码启动服务
    /// </summary>
    /// <param name="serviceScopeFactory">用于解析 Scoped 验证码服务</param>
    /// <param name="webHostEnvironment">宿主环境（解析 wwwroot 路径）</param>
    /// <param name="options">验证码配置（Captcha 节点）</param>
    /// <param name="httpClientFactory">下载背景图 HTTP 客户端工厂</param>
    public TaktCaptchaInitializer(
        IServiceScopeFactory serviceScopeFactory,
        IWebHostEnvironment webHostEnvironment,
        IOptions<TaktCaptchaOptions> options,
        IHttpClientFactory httpClientFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _webHostEnvironment = webHostEnvironment;
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 应用启动时执行：Captcha 未启用则跳过；Slider 类型时校验模板并下载背景图；最后试生成一条验证码。异常仅记日志，不阻止启动。
    /// </summary>
    /// <param name="cancellationToken">宿主停止时取消下载与试生成</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
        {
            return;
        }

        var captchaType = _options.Type;
        TaktLogger.Information("[Captcha Initializer] 开始初始化验证码（类型: {Type}）", captchaType);

        try
        {
            if (string.Equals(captchaType, TaktCaptchaTypeNames.Slider, StringComparison.OrdinalIgnoreCase))
            {
                await InitializeSliderResourcesAsync(cancellationToken);
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var captchaService = scope.ServiceProvider.GetRequiredService<ITaktCaptchaService>();
            var result = await captchaService.GenerateAsync(cancellationToken);
            TaktLogger.Information(
                "[Captcha Initializer] 试生成成功: CaptchaId={CaptchaId}, Type={Type}",
                result.CaptchaId,
                result.Type);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[Captcha Initializer] 初始化失败，应用继续启动");
        }
    }

    /// <summary>
    /// 应用关闭时调用；本服务无需要释放的后台资源，立即返回已完成任务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已完成的任务</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// 初始化 Slider 资源：校验 <c>slide/template/{n}/hole.png|slider.png</c>，并确保背景图数量满足 <c>MinCount</c>
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    private async Task InitializeSliderResourcesAsync(CancellationToken cancellationToken)
    {
        var wwwrootPath = TaktFileHelper.GetWwwRootPath(_webHostEnvironment.ContentRootPath);
        if (!Directory.Exists(wwwrootPath))
        {
            Directory.CreateDirectory(wwwrootPath);
        }

        var templatePath = Path.Combine(wwwrootPath, _options.Slider.BackgroundImages.Template.TemplatePath);
        var backgroundPath = Path.Combine(wwwrootPath, _options.Slider.BackgroundImages.StoragePath);
        Directory.CreateDirectory(templatePath);
        Directory.CreateDirectory(backgroundPath);

        var validGroups = 0;
        for (var i = 1; i <= _options.Slider.BackgroundImages.Template.GroupCount; i++)
        {
            var groupDir = Path.Combine(templatePath, i.ToString());
            var hole = Path.Combine(groupDir, "hole.png");
            var slider = Path.Combine(groupDir, "slider.png");
            if (File.Exists(hole) && File.Exists(slider))
            {
                validGroups++;
            }
            else
            {
                TaktLogger.Warning("[Captcha Initializer] 模板组 {Group} 缺失 hole.png/slider.png", i);
            }
        }

        if (validGroups == 0)
        {
            throw new InvalidOperationException(
                $"验证码模板无效：请在 {templatePath} 下放置 1..{_options.Slider.BackgroundImages.Template.GroupCount}/hole.png 与 slider.png");
        }

        await EnsureBackgroundImagesAsync(backgroundPath, cancellationToken);
    }

    /// <summary>
    /// 确保背景图目录中图片数量不少于 TaktCaptchaBackgroundImagesOptions.MinCount；
    /// 若 TaktCaptchaBackgroundImagesOptions.RedownloadOnStartup 为 true 则先删除已有文件再下载
    /// </summary>
    /// <param name="backgroundPath">背景图目录绝对路径（wwwroot/slide/background）</param>
    /// <param name="cancellationToken">取消令牌</param>
    private async Task EnsureBackgroundImagesAsync(string backgroundPath, CancellationToken cancellationToken)
    {
        var extension = _options.Slider.BackgroundImages.FileExtension;
        var existing = Directory.GetFiles(backgroundPath, $"*{extension}");

        if (_options.Slider.BackgroundImages.RedownloadOnStartup && existing.Length > 0)
        {
            foreach (var file in existing)
            {
                File.Delete(file);
            }

            existing = [];
        }

        var need = Math.Max(0, _options.Slider.BackgroundImages.MinCount - existing.Length);
        if (need == 0)
        {
            return;
        }

        var httpClient = _httpClientFactory.CreateClient(nameof(TaktCaptchaInitializer));
        var baseUrl = _options.Slider.BackgroundImages.DownloadUrl
            .Replace("{width}", _options.Slider.Width.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("{height}", _options.Slider.Height.ToString(), StringComparison.OrdinalIgnoreCase);

        for (var i = 0; i < need; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                var url = $"{baseUrl}?random={Random.Shared.Next(1, 1_000_000)}";
                var bytes = await httpClient.GetByteArrayAsync(url, cancellationToken);
                var fileName = $"bg_{Guid.NewGuid():N}{extension}";
                await File.WriteAllBytesAsync(Path.Combine(backgroundPath, fileName), bytes, cancellationToken);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(ex, "[Captcha Initializer] 下载背景图失败");
            }
        }
    }
}
