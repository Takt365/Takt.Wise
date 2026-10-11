// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Captcha
// 文件名称：TaktCaptchaService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：验证码服务实现（Slider 拼图 / Behavior 行为评分，按 appsettings Captcha 节点；图像合成用 SkiaSharp）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using Takt.Application.Services;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services.Captcha;

/// <summary>
/// 验证码服务实现（ITaktCaptchaService）。
/// 按 <c>Captcha:Type</c> 分支：TaktCaptchaTypeNames.Slider 合成拼图并校验像素偏差；
/// TaktCaptchaTypeNames.Behavior 在内存保存目标百分比并对 <c>position/timeSpent/mouseTrajectory</c> 加权评分。
/// 挑战数据存于进程内 ConcurrentDictionary{TKey,TValue}，一次性校验后移除（Behavior 仅成功时移除）。
/// </summary>
public class TaktCaptchaService : TaktServiceBase, ITaktCaptchaService
{
    /// <summary>
    /// 滑块验证码内存存储（CaptchaId → 目标坐标与创建时间）
    /// </summary>
    private static readonly ConcurrentDictionary<string, TaktSliderCaptchaData> SliderStore = new();

    /// <summary>
    /// 行为验证码内存存储（CaptchaId → 目标位置与创建时间）
    /// </summary>
    private static readonly ConcurrentDictionary<string, TaktBehaviorCaptchaData> BehaviorStore = new();

    private readonly TaktCaptchaOptions _captchaOptions;
    private readonly TaktCaptchaSliderOptions _sliderOptions;
    private readonly TaktCaptchaBehaviorOptions _behaviorOptions;
    private readonly int _expirationMinutes;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// 可用的 Slider 模板组编码（wwwroot 下存在 hole.png 与 slider.png 的组）
    /// </summary>
    private readonly List<int> _availableTemplateGroups = [];

    private readonly Random _random = new();

    /// <summary>
    /// 行为验证码后台清理任务是否已启动
    /// </summary>
    private volatile bool _behaviorCleanupStarted;

    /// <summary>
    /// 滑块验证码后台清理任务是否已启动
    /// </summary>
    private volatile bool _sliderCleanupStarted;

    /// <summary>
    /// 初始化验证码服务
    /// </summary>
    /// <param name="captchaOptions">验证码配置（Captcha 节点）</param>
    /// <param name="environment">Web 宿主环境（解析 wwwroot）</param>
    /// <param name="httpClientFactory">下载背景图 HTTP 客户端工厂</param>
    /// <param name="userContext">当前用户上下文（可选）</param>
    /// <param name="localizationService">本地化服务（可选）</param>
    public TaktCaptchaService(
        IOptions<TaktCaptchaOptions> captchaOptions,
        IWebHostEnvironment environment,
        IHttpClientFactory httpClientFactory,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _captchaOptions = captchaOptions.Value;
        _sliderOptions = _captchaOptions.Slider;
        _behaviorOptions = _captchaOptions.Behavior;
        _expirationMinutes = _captchaOptions.ExpirationMinutes;
        _environment = environment;
        _httpClientFactory = httpClientFactory;

        // Behavior 类型不依赖 Slider 拼图模板，避免每次请求构造服务时刷 ERR 日志
        if (string.Equals(_captchaOptions.Type, TaktCaptchaTypeNames.Slider, StringComparison.OrdinalIgnoreCase))
        {
            InitializeSliderTemplateGroups();
        }
    }

    /// <summary>
    /// 是否启用验证码，读取 <c>Captcha:Enabled</c>。
    /// </summary>
    public bool IsEnabled => _captchaOptions.Enabled;

    /// <summary>
    /// 按 <c>Captcha:Type</c> 生成 Slider 或 Behavior 验证码；未启用时抛出异常。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果（Slider 含图片 data URL，Behavior 的 BackgroundImage 为空）</returns>
    public async Task<TaktCaptchaGenerateResult> GenerateAsync(CancellationToken cancellationToken = default)
    {
        if (!_captchaOptions.Enabled)
        {
            throw new InvalidOperationException("验证码未启用");
        }

        return _captchaOptions.Type.ToLowerInvariant() switch
        {
            "slider" => await GenerateSliderCaptchaAsync(cancellationToken),
            "behavior" => GenerateBehaviorCaptcha(),
            _ => throw new InvalidOperationException($"不支持的验证码类型: {_captchaOptions.Type}"),
        };
    }

    /// <summary>
    /// 校验验证码；未启用时直接返回 Success=true。Slider 校验像素偏差，Behavior 按加权分数与 ScoreThreshold 判定。
    /// </summary>
    /// <param name="request">验证请求（CaptchaId + UserInput）</param>
    /// <param name="cancellationToken">取消令牌（当前实现未使用）</param>
    /// <returns>验证结果，Message 为本地化资源键</returns>
    public Task<TaktCaptchaVerifyResult> VerifyAsync(
        TaktCaptchaVerifyRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!_captchaOptions.Enabled)
        {
            return Task.FromResult(new TaktCaptchaVerifyResult { Success = true, Message = string.Empty });
        }

        return _captchaOptions.Type.ToLowerInvariant() switch
        {
            "slider" => Task.FromResult(VerifySliderCaptcha(request)),
            "behavior" => Task.FromResult(VerifyBehaviorCaptcha(request)),
            _ => throw new InvalidOperationException($"不支持的验证码类型: {_captchaOptions.Type}"),
        };
    }

    #region Slider 生成与校验

    /// <summary>
    /// 生成滑块验证码：随机目标 X/Y，合成背景与滑块图，返回 data URL
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果（含 CaptchaId、背景图、滑块图、目标位置百分比）</returns>
    private async Task<TaktCaptchaGenerateResult> GenerateSliderCaptchaAsync(CancellationToken cancellationToken)
    {
        var captchaId = GenerateCaptchaId();
        var sliderSize = _sliderOptions.SliderHeight > 0 ? _sliderOptions.SliderHeight : _sliderOptions.SliderWidth;
        var margin = sliderSize;
        var minX = margin;
        var maxX = _sliderOptions.Width - sliderSize - margin;
        if (maxX <= minX)
        {
            minX = 0;
            maxX = _sliderOptions.Width - sliderSize;
        }

        var targetX = _random.Next(minX, maxX + 1);
        var targetY = (_sliderOptions.Height - sliderSize) / 2;
        var targetPosition = (int)((double)targetX / _sliderOptions.Width * 100);

        EnsureSliderCleanupStarted();

        SliderStore[captchaId] = new TaktSliderCaptchaData
        {
            TargetX = targetX,
            TargetY = targetY,
            CreatedAt = DateTime.UtcNow,
        };

        var (backgroundImage, sliderImage) = await GenerateCaptchaImagesAsync(targetX, targetY, cancellationToken);

        return new TaktCaptchaGenerateResult
        {
            CaptchaId = captchaId,
            Type = TaktCaptchaTypeNames.Slider,
            BackgroundImage = backgroundImage,
            SliderImage = sliderImage,
            TargetPosition = targetPosition,
        };
    }

    /// <summary>
    /// 校验滑块验证码：校验过期、最短完成时间、用户提交的 position（百分比）与目标像素偏差
    /// </summary>
    /// <param name="request">验证请求（UserInput 为 JSON 或含 position 的对象）</param>
    /// <returns>验证结果（Message 为国际化键）</returns>
    private TaktCaptchaVerifyResult VerifySliderCaptcha(TaktCaptchaVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CaptchaId))
        {
            return FailValidation(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldCaptchaId);
        }

        if (!SliderStore.TryGetValue(request.CaptchaId, out var captchaData))
        {
            return FailValidation(TaktValidationI18nKeys.NotFoundOrExpired, TaktValidationI18nKeys.FieldCaptcha);
        }

        if (DateTime.UtcNow > captchaData.CreatedAt.AddMinutes(_expirationMinutes))
        {
            SliderStore.TryRemove(request.CaptchaId, out _);
            return FailValidation(TaktValidationI18nKeys.Expired, TaktValidationI18nKeys.FieldCaptcha);
        }

        var elapsedSeconds = (DateTime.UtcNow - captchaData.CreatedAt).TotalSeconds;
        if (elapsedSeconds < _sliderOptions.MinCompleteSeconds)
        {
            SliderStore.TryRemove(request.CaptchaId, out _);
            return FailMessage(TaktValidationI18nKeys.TooFastRetry);
        }

        if (request.UserInput == null)
        {
            return FailValidation(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldCaptcha);
        }

        int userPercent;
        if (_sliderOptions.RequireBehaviorData)
        {
            var parsed = ParseSliderBehaviorInput(request.UserInput);
            if (parsed == null)
            {
                return FailMessage(TaktValidationI18nKeys.TipDragWaitSubmit);
            }

            userPercent = parsed.Value.position;
            if (parsed.Value.timeSpent.HasValue && parsed.Value.timeSpent.Value < _sliderOptions.MinTimeSpentSeconds)
            {
                SliderStore.TryRemove(request.CaptchaId, out _);
                return FailMessage(TaktValidationI18nKeys.TooFastRetry);
            }
        }
        else if (!TryParseSliderPosition(request.UserInput, out userPercent))
        {
            return FailValidation(TaktValidationI18nKeys.InvalidFormat, TaktValidationI18nKeys.FieldCaptchaPayload);
        }

        var userPositionX = (int)((double)userPercent / 100 * _sliderOptions.Width);
        var difference = Math.Abs(userPositionX - captchaData.TargetX);
        var success = difference <= _sliderOptions.Tolerance;

        SliderStore.TryRemove(request.CaptchaId, out _);

        return new TaktCaptchaVerifyResult
        {
            Success = success,
            Message = success
                ? GetLocalizedMessage(TaktValidationI18nKeys.FeedbackSuccess)
                : GetValidationMessage(TaktValidationI18nKeys.NotMatch, TaktValidationI18nKeys.FieldSliderPosition),
        };
    }

    /// <summary>
    /// 扫描 wwwroot 模板目录，填充 _availableTemplateGroups
    /// </summary>
    private void InitializeSliderTemplateGroups()
    {
        if (!_sliderOptions.BackgroundImages.Template.UseTemplate)
        {
            return;
        }

        var wwwroot = TaktFileHelper.GetWwwRootPath(_environment.ContentRootPath);
        var templateBase = Path.Combine(wwwroot, NormalizeRelativePath(_sliderOptions.BackgroundImages.Template.TemplatePath));

        for (var i = 1; i <= _sliderOptions.BackgroundImages.Template.GroupCount; i++)
        {
            var groupPath = Path.Combine(templateBase, i.ToString());
            if (File.Exists(Path.Combine(groupPath, "hole.png")) && File.Exists(Path.Combine(groupPath, "slider.png")))
            {
                _availableTemplateGroups.Add(i);
            }
        }

        if (_availableTemplateGroups.Count == 0)
        {
            TaktLogger.Error("[Captcha] 没有可用的模板组: {Path}", templateBase);
        }
    }

    /// <summary>
    /// 合成 Slider 挑战图：背景 + hole 蒙版 + 滑块贴图
    /// </summary>
    /// <param name="targetX">缺口左上角 X（像素）</param>
    /// <param name="targetY">缺口左上角 Y（像素）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>背景与滑块的 data URL Base64 字符串</returns>
    private async Task<(string backgroundImage, string sliderImage)> GenerateCaptchaImagesAsync(
        int targetX,
        int targetY,
        CancellationToken cancellationToken)
    {
        if (_availableTemplateGroups.Count == 0)
        {
            throw new InvalidOperationException("没有可用的模板组");
        }

        var templateGroup = _availableTemplateGroups[_random.Next(_availableTemplateGroups.Count)];
        using var background = await LoadBackgroundImageAsync(cancellationToken);
        var (holeTemplate, sliderTemplate) = await LoadTemplateImagesAsync(templateGroup, cancellationToken);
        using (holeTemplate)
        using (sliderTemplate)
        {
            using var backgroundWithHole = ApplyHoleTemplate(background, holeTemplate, targetX, targetY);
            using var sliderImage = ApplySliderTemplate(sliderTemplate);
            return (
                ImageToBase64(backgroundWithHole, _sliderOptions.BackgroundImages.FileExtension),
                ImageToBase64(sliderImage, ".png"));
        }
    }

    /// <summary>
    /// 从 wwwroot 背景目录随机选取一张图；数量不足时触发下载，仍无则生成随机色块图
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>背景 SKBitmap（调用方负责释放）</returns>
    private async Task<SKBitmap> LoadBackgroundImageAsync(CancellationToken cancellationToken)
    {
        var wwwroot = TaktFileHelper.GetWwwRootPath(_environment.ContentRootPath);
        var storagePath = Path.Combine(wwwroot, NormalizeRelativePath(_sliderOptions.BackgroundImages.StoragePath));
        Directory.CreateDirectory(storagePath);

        var imageFiles = Directory
            .GetFiles(storagePath, $"*{_sliderOptions.BackgroundImages.FileExtension}")
            .Where(f => !Path.GetFileName(f).StartsWith("template_", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (imageFiles.Length < _sliderOptions.BackgroundImages.MinCount)
        {
            await DownloadBackgroundImagesAsync(cancellationToken);
            imageFiles = Directory
                .GetFiles(storagePath, $"*{_sliderOptions.BackgroundImages.FileExtension}")
                .Where(f => !Path.GetFileName(f).StartsWith("template_", StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        if (imageFiles.Length == 0)
        {
            return GenerateRandomImage();
        }

        var bytes = await File.ReadAllBytesAsync(imageFiles[_random.Next(imageFiles.Length)], cancellationToken);
        return DecodeBitmap(bytes);
    }

    /// <summary>
    /// 按配置从 TaktCaptchaBackgroundImagesOptions.DownloadUrl 下载背景图至存储目录
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    private async Task DownloadBackgroundImagesAsync(CancellationToken cancellationToken)
    {
        var wwwroot = TaktFileHelper.GetWwwRootPath(_environment.ContentRootPath);
        var storagePath = Path.Combine(wwwroot, NormalizeRelativePath(_sliderOptions.BackgroundImages.StoragePath));
        Directory.CreateDirectory(storagePath);

        var existing = Directory
            .GetFiles(storagePath, $"*{_sliderOptions.BackgroundImages.FileExtension}")
            .Where(f => !Path.GetFileName(f).StartsWith("template_", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        var need = Math.Max(0, _sliderOptions.BackgroundImages.MinCount - existing.Length);
        if (need <= 0)
        {
            return;
        }

        var baseUrl = _sliderOptions.BackgroundImages.DownloadUrl
            .Replace("{width}", _sliderOptions.Width.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("{height}", _sliderOptions.Height.ToString(), StringComparison.OrdinalIgnoreCase);

        var httpClient = _httpClientFactory.CreateClient(nameof(TaktCaptchaInitializer));
        for (var i = 0; i < need; i++)
        {
            try
            {
                var url = $"{baseUrl}?random={_random.Next(1, 1_000_000)}";
                var bytes = await httpClient.GetByteArrayAsync(url, cancellationToken);
                var fileName = $"bg_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{_random.Next(1000, 9999)}{_sliderOptions.BackgroundImages.FileExtension}";
                await File.WriteAllBytesAsync(Path.Combine(storagePath, fileName), bytes, cancellationToken);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(ex, "[Captcha Slider] 下载背景图失败");
            }
        }
    }

    /// <summary>
    /// 加载指定模板组的 hole.png 与 slider.png
    /// </summary>
    /// <param name="templateGroup">模板组编码（1..GroupCount）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>缺口蒙版与滑块贴图（调用方负责释放）</returns>
    private async Task<(SKBitmap holeTemplate, SKBitmap sliderTemplate)> LoadTemplateImagesAsync(
        int templateGroup,
        CancellationToken cancellationToken)
    {
        var wwwroot = TaktFileHelper.GetWwwRootPath(_environment.ContentRootPath);
        var templatePath = Path.Combine(
            wwwroot,
            NormalizeRelativePath(_sliderOptions.BackgroundImages.Template.TemplatePath),
            templateGroup.ToString());

        var holePath = Path.Combine(templatePath, "hole.png");
        var sliderPath = Path.Combine(templatePath, "slider.png");
        if (!File.Exists(holePath) || !File.Exists(sliderPath))
        {
            throw new FileNotFoundException($"模板图片不存在: Group {templateGroup}");
        }

        var holeBytes = await File.ReadAllBytesAsync(holePath, cancellationToken);
        var sliderBytes = await File.ReadAllBytesAsync(sliderPath, cancellationToken);
        return (DecodeBitmap(holeBytes), DecodeBitmap(sliderBytes));
    }

    /// <summary>
    /// 在背景指定坐标绘制缩放后的 hole 蒙版（半透明叠加）
    /// </summary>
    /// <param name="background">原始背景图</param>
    /// <param name="holeTemplate">缺口模板</param>
    /// <param name="targetX">绘制 X</param>
    /// <param name="targetY">绘制 Y</param>
    /// <returns>带缺口的背景克隆图（调用方负责释放）</returns>
    private SKBitmap ApplyHoleTemplate(SKBitmap background, SKBitmap holeTemplate, int targetX, int targetY)
    {
        var sliderSize = _sliderOptions.SliderHeight > 0 ? _sliderOptions.SliderHeight : _sliderOptions.SliderWidth;
        var result = background.Copy() ?? throw new InvalidOperationException("背景图克隆失败");
        using var resizedHole = ResizeStretch(holeTemplate, sliderSize, sliderSize);
        using var canvas = new SKCanvas(result);
        using var paint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.White.WithAlpha((byte)(255 * 0.8f)),
        };
        canvas.DrawBitmap(
            resizedHole,
            targetX,
            targetY,
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None),
            paint);
        return result;
    }

    /// <summary>
    /// 将滑块模板缩放到配置的滑块尺寸
    /// </summary>
    /// <param name="sliderTemplate">滑块贴图</param>
    /// <returns>缩放后的滑块图（调用方负责释放）</returns>
    private SKBitmap ApplySliderTemplate(SKBitmap sliderTemplate)
    {
        var sliderSize = _sliderOptions.SliderHeight > 0 ? _sliderOptions.SliderHeight : _sliderOptions.SliderWidth;
        return ResizeStretch(sliderTemplate, sliderSize, sliderSize);
    }

    /// <summary>
    /// 无可用背景文件时生成随机浅色块图（兜底）
    /// </summary>
    /// <returns>随机 RGB 图像</returns>
    private SKBitmap GenerateRandomImage()
    {
        var image = new SKBitmap(_sliderOptions.Width, _sliderOptions.Height);
        for (var y = 0; y < image.Height; y++)
        {
            for (var x = 0; x < image.Width; x++)
            {
                image.SetPixel(
                    x,
                    y,
                    new SKColor(
                        (byte)_random.Next(200, 256),
                        (byte)_random.Next(200, 256),
                        (byte)_random.Next(200, 256)));
            }
        }

        return image;
    }

    /// <summary>
    /// 将图像编码为 data URL（PNG 或 JPEG）
    /// </summary>
    /// <param name="image">源图像</param>
    /// <param name="fileExtension">扩展名（.png / .jpg 等）</param>
    /// <returns>形如 data:image/png;base64,... 的字符串</returns>
    private static string ImageToBase64(SKBitmap image, string fileExtension)
    {
        using var skImage = SKImage.FromBitmap(image);
        var isPng = fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase);
        using var data = skImage.Encode(
            isPng ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg,
            isPng ? 100 : 90);
        if (data == null)
        {
            throw new InvalidOperationException("图像编码失败");
        }

        var mime = isPng ? "image/png" : "image/jpeg";
        return $"data:{mime};base64,{Convert.ToBase64String(data.ToArray())}";
    }

    /// <summary>
    /// 从字节解码位图；失败抛 InvalidOperationException
    /// </summary>
    /// <param name="bytes">图像文件字节</param>
    /// <returns>解码后的 SKBitmap</returns>
    private static SKBitmap DecodeBitmap(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        var bitmap = SKBitmap.Decode(bytes);
        if (bitmap == null)
        {
            throw new InvalidOperationException("图像解码失败");
        }

        return bitmap;
    }

    /// <summary>
    /// 按目标宽高拉伸缩放位图
    /// </summary>
    /// <param name="source">源图</param>
    /// <param name="width">目标宽</param>
    /// <param name="height">目标高</param>
    /// <returns>新位图（调用方负责释放）</returns>
    private static SKBitmap ResizeStretch(SKBitmap source, int width, int height)
    {
        ArgumentNullException.ThrowIfNull(source);
        var resized = source.Resize(
            new SKImageInfo(width, height),
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));
        if (resized == null)
        {
            throw new InvalidOperationException("图像缩放失败");
        }

        return resized;
    }

    /// <summary>
    /// 解析滑块行为模式提交（position、timeSpent、mouseTrajectory）
    /// </summary>
    /// <param name="userInput">原始用户输入</param>
    /// <returns>解析成功时返回元组；失败返回 null</returns>
    private (int position, double? timeSpent, object? trajectory)? ParseSliderBehaviorInput(object userInput)
    {
        var jo = ToJObject(userInput);
        if (jo?["position"] == null)
        {
            return null;
        }

        var pos = jo["position"]!.ToObject<int>();
        double? timeSpent = null;
        if (jo["timeSpent"] != null)
        {
            timeSpent = jo["timeSpent"]!.ToObject<double>();
        }

        object? trajectory = jo["mouseTrajectory"] is JArray arr ? arr : null;
        return (pos, timeSpent, trajectory);
    }

    /// <summary>
    /// 解析滑块位置（整数百分比或 JSON 中的 position 字段）
    /// </summary>
    /// <param name="userInput">原始用户输入</param>
    /// <param name="userPercent">输出：0–100 的横向位置百分比</param>
    /// <returns>是否解析成功</returns>
    private static bool TryParseSliderPosition(object userInput, out int userPercent)
    {
        userPercent = 0;
        if (userInput is JValue jv && (jv.Type == JTokenType.Integer || jv.Type == JTokenType.Float))
        {
            userPercent = jv.ToObject<int>();
            return true;
        }

        if (userInput is int i)
        {
            userPercent = i;
            return true;
        }

        var jo = ToJObject(userInput);
        if (jo?["position"] != null)
        {
            userPercent = jo["position"]!.ToObject<int>();
            return true;
        }

        return int.TryParse(userInput?.ToString(), out userPercent);
    }

    #endregion

    #region Behavior 生成与校验

    /// <summary>
    /// 生成行为验证码：随机目标位置百分比（60–90），无图片；按需启动过期清理任务
    /// </summary>
    /// <returns>生成结果（BackgroundImage 为空）</returns>
    private TaktCaptchaGenerateResult GenerateBehaviorCaptcha()
    {
        if (!_behaviorCleanupStarted)
        {
            lock (BehaviorStore)
            {
                if (!_behaviorCleanupStarted)
                {
                    _ = Task.Run(CleanupExpiredBehaviorDataAsync);
                    _behaviorCleanupStarted = true;
                }
            }
        }

        var captchaId = GenerateCaptchaId();
        var targetPosition = _random.Next(60, 91);

        BehaviorStore[captchaId] = new TaktBehaviorCaptchaData
        {
            CreatedAt = DateTime.UtcNow,
            BehaviorData = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                ["targetPosition"] = targetPosition,
            },
        };

        return new TaktCaptchaGenerateResult
        {
            CaptchaId = captchaId,
            Type = TaktCaptchaTypeNames.Behavior,
            BackgroundImage = string.Empty,
            SliderImage = null,
            TargetPosition = targetPosition,
        };
    }

    /// <summary>
    /// 校验行为验证码：校验过期、最短完成时间、必填字段，并按加权规则计算分数
    /// </summary>
    /// <param name="request">验证请求</param>
    /// <returns>验证结果（含 Score）</returns>
    private TaktCaptchaVerifyResult VerifyBehaviorCaptcha(TaktCaptchaVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CaptchaId))
        {
            return FailValidation(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldCaptchaId);
        }

        if (!BehaviorStore.TryGetValue(request.CaptchaId, out var captchaData))
        {
            return FailValidation(TaktValidationI18nKeys.NotFoundOrExpired, TaktValidationI18nKeys.FieldCaptcha);
        }

        if (DateTime.UtcNow > captchaData.CreatedAt.AddMinutes(_expirationMinutes))
        {
            BehaviorStore.TryRemove(request.CaptchaId, out _);
            return FailValidation(TaktValidationI18nKeys.Expired, TaktValidationI18nKeys.FieldCaptcha);
        }

        var elapsedSeconds = (DateTime.UtcNow - captchaData.CreatedAt).TotalSeconds;
        if (elapsedSeconds < _behaviorOptions.MinCompleteSeconds)
        {
            BehaviorStore.TryRemove(request.CaptchaId, out _);
            return FailMessage(TaktValidationI18nKeys.TooFastRetry);
        }

        if (request.UserInput == null)
        {
            return FailValidation(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldCaptcha);
        }

        if (TryGetBehaviorInputFailure(request.UserInput, out var validationKey, out var fieldKey))
        {
            return fieldKey == null
                ? FailMessage(validationKey)
                : FailValidation(validationKey, fieldKey);
        }

        var score = CalculateBehaviorScore(request.UserInput, captchaData);
        var success = score >= _behaviorOptions.ScoreThreshold;

        if (success)
        {
            BehaviorStore.TryRemove(request.CaptchaId, out _);
        }

        return new TaktCaptchaVerifyResult
        {
            Success = success,
            Message = success
                ? GetLocalizedMessage(TaktValidationI18nKeys.FeedbackSuccess)
                : GetLocalizedMessage(TaktValidationI18nKeys.VerifyFailed),
            Score = score,
        };
    }

    /// <summary>
    /// 校验行为提交是否满足 TaktCaptchaBehaviorOptions 中的 RequireTimeSpent / RequireTrajectory
    /// </summary>
    /// <param name="userInput">用户输入</param>
    /// <param name="validationKey">校验失败时的抽象 I18n 键</param>
    /// <param name="fieldKey">字段标签键；无字段时为 null</param>
    /// <returns>true 表示校验失败</returns>
    private bool TryGetBehaviorInputFailure(object userInput, out string validationKey, out string? fieldKey)
    {
        validationKey = string.Empty;
        fieldKey = null;
        var jo = ToJObject(userInput);
        if (jo == null)
        {
            validationKey = TaktValidationI18nKeys.Invalid;
            fieldKey = TaktValidationI18nKeys.FieldBehaviorData;
            return true;
        }

        if (_behaviorOptions.RequireTimeSpent)
        {
            if (jo["timeSpent"] == null)
            {
                validationKey = TaktValidationI18nKeys.TipWaitBeforeSubmit;
                return true;
            }

            var timeSpent = jo["timeSpent"]!.ToObject<double>();
            if (timeSpent < _behaviorOptions.MinTimeSpentSeconds)
            {
                validationKey = TaktValidationI18nKeys.TooFastRetry;
                return true;
            }
        }

        if (_behaviorOptions.RequireTrajectory)
        {
            if (jo["mouseTrajectory"] is not JArray arr)
            {
                validationKey = TaktValidationI18nKeys.Required;
                fieldKey = TaktValidationI18nKeys.FieldMouseTrajectory;
                return true;
            }

            if (arr.Count < _behaviorOptions.MinTrajectoryPoints)
            {
                validationKey = TaktValidationI18nKeys.Insufficient;
                fieldKey = TaktValidationI18nKeys.FieldMouseTrajectory;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 计算行为验证码综合分数（位置 40%、轨迹 30%、耗时 20%、ML 占位 10%）
    /// </summary>
    /// <param name="userInput">用户提交 JSON</param>
    /// <param name="captchaData">服务端保存的挑战数据</param>
    /// <returns>0–1 的分数</returns>
    private double CalculateBehaviorScore(object userInput, TaktBehaviorCaptchaData captchaData)
    {
        var jo = ToJObject(userInput);
        if (jo == null)
        {
            return 0;
        }

        double score = 0;
        var factors = 0;

        if (jo["position"] != null
            && captchaData.BehaviorData.TryGetValue("targetPosition", out var targetObj)
            && int.TryParse(targetObj?.ToString(), out var targetPosition)
            && int.TryParse(jo["position"]!.ToString(), out var userPosition))
        {
            score += Math.Max(0, 1.0 - Math.Abs(userPosition - targetPosition) / 50.0) * 0.4;
            factors++;
        }

        if (jo["mouseTrajectory"] is JArray trajectory)
        {
            score += AnalyzeMouseTrajectory(trajectory) * 0.3;
            factors++;
        }

        if (jo["timeSpent"] != null && double.TryParse(jo["timeSpent"]!.ToString(), out var timeSpent))
        {
            if (timeSpent < _behaviorOptions.MinTimeSpentSeconds)
            {
                timeSpent = 0;
            }

            var timeScore = timeSpent is >= 1.0 and <= 5.0 ? 1.0 :
                timeSpent < 1.0 ? timeSpent :
                Math.Max(0, 1.0 - (timeSpent - 5.0) / 10.0);
            score += timeScore * 0.2;
            factors++;
        }

        if (_behaviorOptions.EnableMachineLearning && jo.Count > 0)
        {
            score += Math.Min(1.0, jo.Count / 10.0) * 0.1;
            factors++;
        }

        return factors == 0 ? 0 : Math.Min(1.0, score);
    }

    /// <summary>
    /// 根据鼠标轨迹点序列计算平滑度分数（转角变化越小分数越高）
    /// </summary>
    /// <param name="trajectory">mouseTrajectory JSON 数组</param>
    /// <returns>0–1 的轨迹分数</returns>
    private double AnalyzeMouseTrajectory(JArray trajectory)
    {
        var points = new List<(double x, double y)>();
        foreach (var item in trajectory.Children())
        {
            if (item is not JObject pointObj)
            {
                continue;
            }

            points.Add((pointObj["x"]?.ToObject<double>() ?? 0, pointObj["y"]?.ToObject<double>() ?? 0));
        }

        if (points.Count < _behaviorOptions.MinTrajectoryPoints || points.Count < 3)
        {
            return points.Count >= 2 ? 0.5 : 0;
        }

        var angleChanges = new List<double>();
        for (var i = 1; i < points.Count - 1; i++)
        {
            var angle1 = Math.Atan2(points[i].y - points[i - 1].y, points[i].x - points[i - 1].x);
            var angle2 = Math.Atan2(points[i + 1].y - points[i].y, points[i + 1].x - points[i].x);
            var angleDiff = Math.Abs(angle2 - angle1);
            if (angleDiff > Math.PI)
            {
                angleDiff = 2 * Math.PI - angleDiff;
            }

            angleChanges.Add(angleDiff);
        }

        return Math.Max(0, 1.0 - angleChanges.Average() / Math.PI);
    }

    /// <summary>
    /// 首次生成滑块验证码时启动过期清理后台任务
    /// </summary>
    private void EnsureSliderCleanupStarted()
    {
        if (_sliderCleanupStarted)
        {
            return;
        }

        lock (SliderStore)
        {
            if (_sliderCleanupStarted)
            {
                return;
            }

            _ = Task.Run(() => CleanupExpiredCaptchaStoreAsync(SliderStore, "Slider"));
            _sliderCleanupStarted = true;
        }
    }

    /// <summary>
    /// 后台循环清理过期的行为验证码内存项（每 5 分钟扫描一次）
    /// </summary>
    private async Task CleanupExpiredBehaviorDataAsync()
    {
        await CleanupExpiredCaptchaStoreAsync(BehaviorStore, "Behavior");
    }

    /// <summary>
    /// 后台循环清理验证码内存存储中的过期项（每 5 分钟扫描一次）
    /// </summary>
    /// <typeparam name="TData">验证码缓存项类型</typeparam>
    /// <param name="store">内存存储</param>
    /// <param name="storeName">存储名称（日志用）</param>
    private async Task CleanupExpiredCaptchaStoreAsync<TData>(
        ConcurrentDictionary<string, TData> store,
        string storeName) where TData : class
    {
        while (true)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
                var expirationTime = DateTime.UtcNow.AddMinutes(-_expirationMinutes);
                foreach (var kvp in store)
                {
                    var createdAt = kvp.Value switch
                    {
                        TaktSliderCaptchaData slider => slider.CreatedAt,
                        TaktBehaviorCaptchaData behavior => behavior.CreatedAt,
                        _ => DateTime.MinValue,
                    };
                    if (createdAt < expirationTime)
                    {
                        store.TryRemove(kvp.Key, out _);
                    }
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Error(ex, "[Captcha {StoreName}] 清理过期数据失败", storeName);
            }
        }
    }

    #endregion

    #region 辅助方法

    /// <summary>
    /// 构造验证失败结果（抽象校验键 + 字段标签键）
    /// </summary>
    private TaktCaptchaVerifyResult FailValidation(string validationKey, string fieldKey) =>
        new() { Success = false, Message = GetValidationMessage(validationKey, fieldKey) };

    /// <summary>
    /// 构造验证失败结果（无字段参数的固定文案键）
    /// </summary>
    private TaktCaptchaVerifyResult FailMessage(string messageKey) =>
        new() { Success = false, Message = GetLocalizedMessage(messageKey) };

    /// <summary>
    /// 生成 URL 安全的验证码 ID（16 字节随机数 Base64Url 编码）
    /// </summary>
    /// <returns>CaptchaId 字符串</returns>
    private static string GenerateCaptchaId()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    /// <summary>
    /// 将配置中的相对路径规范为可与 Path.Combine 拼接的形式
    /// </summary>
    /// <param name="relativePath">如 slide/template</param>
    /// <returns>去掉前导分隔符的路径</returns>
    private static string NormalizeRelativePath(string relativePath) =>
        relativePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);

    /// <summary>
    /// 将登录提交的 UserInput 转为 JObject（支持字符串 JSON、JObject、JsonElement）
    /// </summary>
    /// <param name="userInput">控制器绑定的 CaptchaCode 或对象</param>
    /// <returns>解析后的 JObject；无法解析时返回 null</returns>
    private static JObject? ToJObject(object? userInput)
    {
        if (userInput == null)
        {
            return null;
        }

        if (userInput is JObject jo)
        {
            return jo;
        }

        if (userInput is string s)
        {
            var trimmed = s.Trim();
            if (trimmed.Length == 0 || trimmed[0] != '{')
            {
                return null;
            }

            try
            {
                return JObject.Parse(trimmed);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                return null;
            }
        }

        if (userInput is JsonElement je)
        {
            try
            {
                return JObject.Parse(je.GetRawText());
            }
            catch (Newtonsoft.Json.JsonException)
            {
                return null;
            }
        }

        try
        {
            return JObject.FromObject(userInput);
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion

    #region 内存模型

    /// <summary>
    /// 滑块验证码服务端缓存项
    /// </summary>
    private sealed class TaktSliderCaptchaData
    {
        /// <summary>
        /// 目标缺口左上角 X（像素，相对画布宽度）
        /// </summary>
        public int TargetX { get; init; }

        /// <summary>
        /// 目标缺口左上角 Y（像素）
        /// </summary>
        public int TargetY { get; init; }

        /// <summary>
        /// 创建时间（UTC）
        /// </summary>
        public DateTime CreatedAt { get; init; }
    }

    /// <summary>
    /// 行为验证码服务端缓存项
    /// </summary>
    private sealed class TaktBehaviorCaptchaData
    {
        /// <summary>
        /// 创建时间（UTC）
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 行为挑战数据（含 targetPosition 等键）
        /// </summary>
        public Dictionary<string, object> BehaviorData { get; init; } = new(StringComparer.Ordinal);
    }

    #endregion
}
