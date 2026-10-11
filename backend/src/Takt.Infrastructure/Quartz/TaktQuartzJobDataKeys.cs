// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobDataKeys.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz JobDataMap 键名常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz JobDataMap 键名
/// </summary>
public static class TaktQuartzJobDataKeys
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public const string TenantCode = "TenantCode";

    /// <summary>
    /// 公司代码
    /// </summary>
    public const string CompanyCode = "CompanyCode";

    /// <summary>
    /// 定时任务 ID
    /// </summary>
    public const string QuartzTaskId = "QuartzTaskId";

    /// <summary>
    /// 任务类型
    /// </summary>
    public const string TaskType = "TaskType";

    /// <summary>
    /// 程序集名称
    /// </summary>
    public const string AssemblyName = "AssemblyName";

    /// <summary>
    /// 任务类名
    /// </summary>
    public const string ClassName = "ClassName";

    /// <summary>
    /// API 执行地址
    /// </summary>
    public const string ApiUrl = "ApiUrl";

    /// <summary>
    /// 网络请求方式
    /// </summary>
    public const string RequestMethod = "RequestMethod";

    /// <summary>
    /// SQL 语句
    /// </summary>
    public const string SqlScript = "SqlScript";

    /// <summary>
    /// 触发器类型
    /// </summary>
    public const string TriggerType = "TriggerType";

    /// <summary>
    /// 执行间隔时间（秒）
    /// </summary>
    public const string IntervalSeconds = "IntervalSeconds";

    /// <summary>
    /// 执行参数
    /// </summary>
    public const string ExecuteParams = "ExecuteParams";

    /// <summary>
    /// 触发用户
    /// </summary>
    public const string UserName = "user_name";

    /// <summary>
    /// 是否手动立即触发（1=是，允许暂停态任务执行）
    /// </summary>
    public const string ManualTrigger = "ManualTrigger";
}
