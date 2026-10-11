// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktConstants.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：系统内置常量（落库字段值权威来源；非字典项，前后端对齐 TaktConstants）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 系统内置常量（登录/操作/Quartz/设备等；写库与校验唯一引用点）
/// </summary>
public static class TaktConstants
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public static class LoginType
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const string Unknown = "unknown";
        /// <summary>
        /// 账号密码
        /// </summary>
        public const string Password = "password";
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public const string RefreshToken = "refreshtoken";
        /// <summary>
        /// 客户端凭证
        /// </summary>
        public const string ClientCredentials = "clientcredentials";
        /// <summary>
        /// 授权码换令牌
        /// </summary>
        public const string AuthorizationCode = "authorizationcode";
        /// <summary>
        /// OAuth 授权页登录
        /// </summary>
        public const string OAuthAuthorize = "oauthauthorize";
        /// <summary>
        /// 登录预检验密
        /// </summary>
        public const string VerifyPassword = "verifypassword";
        /// <summary>
        /// 注销会话
        /// </summary>
        public const string SignOut = "signout";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Unknown, Password, RefreshToken, ClientCredentials, AuthorizationCode, OAuthAuthorize, VerifyPassword, SignOut,
        };

        /// <summary>
        /// 是否为允许的登录方式常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// 登录结果
    /// </summary>
    public static class LoginResult
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        public const string Success = "success";
        /// <summary>
        /// 密码错误
        /// </summary>
        public const string PasswordError = "passworderror";
        /// <summary>
        /// 用户不存在
        /// </summary>
        public const string UserNotFound = "usernotfound";
        /// <summary>
        /// 用户已禁用
        /// </summary>
        public const string UserDisabled = "userdisabled";
        /// <summary>
        /// 用户已锁定
        /// </summary>
        public const string UserLocked = "userlocked";
        /// <summary>
        /// 验证码错误
        /// </summary>
        public const string CaptchaError = "captchaerror";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Success, PasswordError, UserNotFound, UserDisabled, UserLocked, CaptchaError,
        };

        /// <summary>
        /// 是否为允许的登录结果常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public static class OperType
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const string Unknown = "unknown";
        /// <summary>
        /// 新增
        /// </summary>
        public const string Create = "create";
        /// <summary>
        /// 修改
        /// </summary>
        public const string Update = "update";
        /// <summary>
        /// 删除
        /// </summary>
        public const string Delete = "delete";
        /// <summary>
        /// 查询
        /// </summary>
        public const string Query = "query";
        /// <summary>
        /// 导出
        /// </summary>
        public const string Export = "export";
        /// <summary>
        /// 导入
        /// </summary>
        public const string Import = "import";
        /// <summary>
        /// 授权
        /// </summary>
        public const string Grant = "grant";
        /// <summary>
        /// 强退
        /// </summary>
        public const string ForceOut = "forceout";
        /// <summary>
        /// 生成代码
        /// </summary>
        public const string CodeGen = "codegen";
        /// <summary>
        /// 清空数据
        /// </summary>
        public const string ClearData = "cleardata";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Unknown, Create, Update, Delete, Query, Export, Import, Grant, ForceOut, CodeGen, ClearData,
        };

        /// <summary>
        /// 是否为允许的操作类型常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// Quartz 任务类型
    /// </summary>
    public static class QuartzTaskType
    {
        /// <summary>
        /// 程序集
        /// </summary>
        public const string Assembly = "assembly";
        /// <summary>
        /// 网络请求
        /// </summary>
        public const string Http = "http";
        /// <summary>
        /// SQL 语句
        /// </summary>
        public const string Sql = "sql";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Assembly, Http, Sql,
        };

        /// <summary>
        /// 是否为允许的任务类型常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// Quartz 触发器类型（对齐 TaktQuartzSchedulerManager：0=Simple 1=Cron）
    /// </summary>
    public static class QuartzTriggerType
    {
        /// <summary>
        /// Simple 间隔触发
        /// </summary>
        public const int Simple = 0;
        /// <summary>
        /// Cron 表达式触发
        /// </summary>
        public const int Cron = 1;

        private static readonly HashSet<int> Allowed = new()
        {
            Simple, Cron,
        };

        /// <summary>
        /// 是否为允许的触发器类型
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(int value) => Allowed.Contains(value);
    }

    /// <summary>
    /// Quartz Misfire 策略（对齐 TaktQuartzSchedulerManager：0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public static class QuartzMisfirePolicy
    {
        /// <summary>
        /// 默认策略
        /// </summary>
        public const int Default = 0;
        /// <summary>
        /// 忽略 Misfire
        /// </summary>
        public const int Ignore = 1;
        /// <summary>
        /// 立即触发
        /// </summary>
        public const int FireAndProceed = 2;
        /// <summary>
        /// 不触发
        /// </summary>
        public const int DoNothing = 3;

        private static readonly HashSet<int> Allowed = new()
        {
            Default, Ignore, FireAndProceed, DoNothing,
        };

        /// <summary>
        /// 是否为允许的 Misfire 策略
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(int value) => Allowed.Contains(value);
    }

    /// <summary>
    /// 登录设备
    /// </summary>
    public static class DeviceType
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const string Unknown = "unknown";
        /// <summary>
        /// PC
        /// </summary>
        public const string Pc = "pc";
        /// <summary>
        /// 手机
        /// </summary>
        public const string Mobile = "mobile";
        /// <summary>
        /// 平板
        /// </summary>
        public const string Tablet = "tablet";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Unknown, Pc, Mobile, Tablet,
        };

        /// <summary>
        /// 是否为允许的登录设备常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// 浏览器
    /// </summary>
    public static class BrowserType
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const string Unknown = "unknown";
        /// <summary>
        /// Chrome
        /// </summary>
        public const string Chrome = "chrome";
        /// <summary>
        /// Firefox
        /// </summary>
        public const string Firefox = "firefox";
        /// <summary>
        /// Safari
        /// </summary>
        public const string Safari = "safari";
        /// <summary>
        /// Edge
        /// </summary>
        public const string Edge = "edge";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Unknown, Chrome, Firefox, Safari, Edge,
        };

        /// <summary>
        /// 是否为允许的浏览器常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }

    /// <summary>
    /// 审计日志操作人登录名（无法解析时落库值）
    /// </summary>
    public static class AuditUserName
    {
        /// <summary>
        /// 未知操作人
        /// </summary>
        public const string Unknown = "unknown";
    }

    /// <summary>
    /// 无登录上下文时的审计操作人 ID（与种子 admin 工号 900001、菜单种子 CreatedBy 一致）
    /// </summary>
    public static class SystemAuditUser
    {
        /// <summary>
        /// 系统默认审计用户 ID
        /// </summary>
        public const long Id = 900001L;
    }

    /// <summary>
    /// 操作系统
    /// </summary>
    public static class OperatingSystem
    {
        /// <summary>
        /// 未知
        /// </summary>
        public const string Unknown = "unknown";
        /// <summary>
        /// Windows
        /// </summary>
        public const string Windows = "windows";
        /// <summary>
        /// macOS
        /// </summary>
        public const string MacOs = "macos";
        /// <summary>
        /// Linux
        /// </summary>
        public const string Linux = "linux";
        /// <summary>
        /// Android
        /// </summary>
        public const string Android = "android";
        /// <summary>
        /// iOS
        /// </summary>
        public const string Ios = "ios";

        private static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
        {
            Unknown, Windows, MacOs, Linux, Android, Ios,
        };

        /// <summary>
        /// 是否为允许的操作系统常量值
        /// </summary>
        /// <param name="value">待校验值</param>
        /// <returns>是否允许</returns>
        public static bool IsValid(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && Allowed.Contains(value.Trim());
        }
    }
}
