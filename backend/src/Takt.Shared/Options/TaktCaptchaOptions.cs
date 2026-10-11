// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktCaptchaOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：验证码配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 验证码配置选项
/// </summary>
public class TaktCaptchaOptions
{
    public const string SectionName = "Captcha";

    /// <summary>
    /// 是否启用验证码
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 验证码类型（Slider/Behavior）
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// 验证码过期时间（分钟）
    /// </summary>
    public int ExpirationMinutes { get; set; }

    /// <summary>
    /// 滑块验证码配置
    /// </summary>
    public TaktCaptchaSliderOptions Slider { get; set; } = null!;

    /// <summary>
    /// 行为验证码配置
    /// </summary>
    public TaktCaptchaBehaviorOptions Behavior { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Type))
        {
            throw new InvalidOperationException($"{SectionName}:Type 不能为空");
        }

        if (Type != "Slider" && Type != "Behavior")
        {
            throw new InvalidOperationException($"{SectionName}:Type 必须是 Slider 或 Behavior");
        }

        if (ExpirationMinutes <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:ExpirationMinutes 必须大于 0");
        }

        if (Slider == null)
        {
            throw new InvalidOperationException($"{SectionName}:Slider 配置不能为空");
        }

        Slider.Validate();

        if (Behavior == null)
        {
            throw new InvalidOperationException($"{SectionName}:Behavior 配置不能为空");
        }

        Behavior.Validate();
    }
}

/// <summary>
/// 滑块验证码配置（对应 appsettings Captcha:Slider）
/// </summary>
public class TaktCaptchaSliderOptions
{
    /// <summary>
    /// 拼图画布宽度（像素）
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 拼图画布高度（像素）
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 滑块宽度（像素）
    /// </summary>
    public int SliderWidth { get; set; }

    /// <summary>
    /// 滑块高度（像素）；为 0 时回退使用 SliderWidth
    /// </summary>
    public int SliderHeight { get; set; }

    /// <summary>
    /// 允许的位置误差（像素，用户提交 position 换算后与 TargetX 比较）
    /// </summary>
    public int Tolerance { get; set; }

    /// <summary>
    /// 从生成到提交的最短间隔（秒），防机器人
    /// </summary>
    public double MinCompleteSeconds { get; set; }

    /// <summary>
    /// 是否要求提交 position、timeSpent、mouseTrajectory 等行为字段
    /// </summary>
    public bool RequireBehaviorData { get; set; }

    /// <summary>
    /// 最短有效拖动耗时（秒，RequireBehaviorData 为 true 时校验）
    /// </summary>
    public double MinTimeSpentSeconds { get; set; }

    /// <summary>
    /// 背景图与模板配置
    /// </summary>
    public TaktCaptchaBackgroundImagesOptions BackgroundImages { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (Width <= 0 || Height <= 0 || SliderWidth <= 0 || SliderHeight <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider 宽高尺寸必须大于 0");
        }

        if (Tolerance < 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:Tolerance 不能小于 0");
        }

        if (MinCompleteSeconds <= 0 || MinTimeSpentSeconds <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider 时间阈值必须大于 0");
        }

        if (BackgroundImages == null)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages 配置不能为空");
        }

        BackgroundImages.Validate();
    }
}

/// <summary>
/// 行为验证码配置（对应 appsettings Captcha:Behavior）
/// </summary>
public class TaktCaptchaBehaviorOptions
{
    /// <summary>
    /// 通过阈值（0–1，加权得分不低于此值则验证成功）
    /// </summary>
    public double ScoreThreshold { get; set; }

    /// <summary>
    /// 是否启用 ML 占位评分（启发式，非真实模型）
    /// </summary>
    public bool EnableMachineLearning { get; set; }

    /// <summary>
    /// 从生成到提交的最短间隔（秒）
    /// </summary>
    public double MinCompleteSeconds { get; set; }

    /// <summary>
    /// 提交 timeSpent 的最小值（秒）
    /// </summary>
    public double MinTimeSpentSeconds { get; set; }

    /// <summary>
    /// mouseTrajectory 最少点数（RequireTrajectory 为 true 时）
    /// </summary>
    public int MinTrajectoryPoints { get; set; }

    /// <summary>
    /// 是否必须提交 timeSpent 字段
    /// </summary>
    public bool RequireTimeSpent { get; set; }

    /// <summary>
    /// 是否必须提交 mouseTrajectory 数组
    /// </summary>
    public bool RequireTrajectory { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (ScoreThreshold < 0 || ScoreThreshold > 1)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Behavior:ScoreThreshold 必须在 0 到 1 之间");
        }

        if (MinCompleteSeconds <= 0 || MinTimeSpentSeconds <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Behavior 时间阈值必须大于 0");
        }

        if (MinTrajectoryPoints <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Behavior:MinTrajectoryPoints 必须大于 0");
        }
    }
}

/// <summary>
/// 滑块背景图配置（对应 Captcha:Slider:BackgroundImages）
/// </summary>
public class TaktCaptchaBackgroundImagesOptions
{
    /// <summary>
    /// 应用启动时是否删除已有背景图并重新下载
    /// </summary>
    public bool RedownloadOnStartup { get; set; }

    /// <summary>
    /// wwwroot 下至少保留的背景图数量
    /// </summary>
    public int MinCount { get; set; }

    /// <summary>
    /// 下载地址模板，占位符 {width}、{height}
    /// </summary>
    public string DownloadUrl { get; set; } = null!;

    /// <summary>
    /// 相对 wwwroot 的存储目录（如 slide/background）
    /// </summary>
    public string StoragePath { get; set; } = null!;

    /// <summary>
    /// 背景图文件扩展名（如 .jpg）
    /// </summary>
    public string FileExtension { get; set; } = null!;

    /// <summary>
    /// 缺口与滑块贴图模板配置
    /// </summary>
    public TaktCaptchaTemplateOptions Template { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (MinCount <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:MinCount 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(DownloadUrl))
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:DownloadUrl 不能为空");
        }

        if (string.IsNullOrWhiteSpace(StoragePath))
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:StoragePath 不能为空");
        }

        if (string.IsNullOrWhiteSpace(FileExtension))
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:FileExtension 不能为空");
        }

        if (Template == null)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:Template 配置不能为空");
        }

        Template.Validate();
    }
}

/// <summary>
/// 滑块模板配置（对应 Captcha:Slider:BackgroundImages:Template）
/// </summary>
public class TaktCaptchaTemplateOptions
{
    /// <summary>
    /// 是否使用 hole.png / slider.png 模板合成拼图
    /// </summary>
    public bool UseTemplate { get; set; }

    /// <summary>
    /// 相对 wwwroot 的模板根目录（如 slide/template，下含 1..GroupCount 子目录）
    /// </summary>
    public string TemplatePath { get; set; } = null!;

    /// <summary>
    /// 模板组数量（子目录 1、2、…、GroupCount）
    /// </summary>
    public int GroupCount { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (UseTemplate && string.IsNullOrWhiteSpace(TemplatePath))
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:Template:TemplatePath 不能为空");
        }

        if (GroupCount <= 0)
        {
            throw new InvalidOperationException($"{TaktCaptchaOptions.SectionName}:Slider:BackgroundImages:Template:GroupCount 必须大于 0");
        }
    }
}

/// <summary>
/// 验证码类型名称（与 Captcha:Type 配置一致）
/// </summary>
public static class TaktCaptchaTypeNames
{
    /// <summary>
    /// 滑块拼图
    /// </summary>
    public const string Slider = "Slider";

    /// <summary>
    /// 行为轨迹
    /// </summary>
    public const string Behavior = "Behavior";
}

/// <summary>
/// 验证码生成结果
/// </summary>
public class TaktCaptchaGenerateResult
{
    /// <summary>
    /// 验证码 ID
    /// </summary>
    public string CaptchaId { get; set; } = string.Empty;

    /// <summary>
    /// 验证码类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 背景图（data URL 或 Base64）
    /// </summary>
    public string BackgroundImage { get; set; } = string.Empty;

    /// <summary>
    /// 滑块图（仅 Slider）
    /// </summary>
    public string? SliderImage { get; set; }

    /// <summary>
    /// 目标位置（百分比 0–100）
    /// </summary>
    public int? TargetPosition { get; set; }
}

/// <summary>
/// 验证码验证请求
/// </summary>
public class TaktCaptchaVerifyRequest
{
    /// <summary>
    /// 验证码 ID
    /// </summary>
    public string CaptchaId { get; set; } = string.Empty;

    /// <summary>
    /// 用户输入（JSON 字符串或对象：position、timeSpent、mouseTrajectory）
    /// </summary>
    public object? UserInput { get; set; }
}

/// <summary>
/// 验证码验证结果
/// </summary>
public class TaktCaptchaVerifyResult
{
    /// <summary>
    /// 是否通过
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 消息键或文案
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 行为分数（Behavior）
    /// </summary>
    public double? Score { get; set; }
}

/// <summary>
/// 验证码挑战 DTO（API 返回前端）
/// </summary>
public class TaktCaptchaChallengeDto
{
    /// <summary>
    /// 验证码 ID
    /// </summary>
    public string CaptchaId { get; set; } = string.Empty;

    /// <summary>
    /// 验证码类型
    /// </summary>
    public string CaptchaType { get; set; } = string.Empty;

    /// <summary>
    /// 画布宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 画布高度
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 滑块宽度
    /// </summary>
    public int SliderWidth { get; set; }

    /// <summary>
    /// 滑块高度
    /// </summary>
    public int SliderHeight { get; set; }

    /// <summary>
    /// 是否要求行为数据
    /// </summary>
    public bool RequireBehaviorData { get; set; }

    /// <summary>
    /// 背景图（data URL 或纯 Base64）
    /// </summary>
    public string? BackgroundImage { get; set; }

    /// <summary>
    /// 滑块图（data URL 或纯 Base64）
    /// </summary>
    public string? SliderImage { get; set; }

    /// <summary>
    /// 目标位置百分比（Behavior 类型返回给前端用于目标指示；Slider 不返回）
    /// </summary>
    public int? TargetPosition { get; set; }
}
