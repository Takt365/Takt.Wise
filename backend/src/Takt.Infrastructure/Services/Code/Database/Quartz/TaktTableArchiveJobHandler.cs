// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Code.Database.Quartz
// 文件名称：TaktTableArchiveJobHandler.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 按年归档处理器（ExecuteParams: { policyIds, archiveYear }）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.Json;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Services.Code.Database.Quartz;

/// <summary>
/// Quartz：按年数据归档处理器
/// </summary>
public sealed class TaktTableArchiveJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktTableArchiveService _tableArchiveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tableArchiveService">数据归档服务</param>
    public TaktTableArchiveJobHandler(ITaktTableArchiveService tableArchiveService)
    {
        ArgumentNullException.ThrowIfNull(tableArchiveService);
        _tableArchiveService = tableArchiveService;
    }

    /// <summary>
    /// 处理器键（与 TaktQuartzTask.ClassName 匹配）
    /// </summary>
    public string HandlerKey => TaktTableArchiveService.QuartzHandlerClassName;

    /// <summary>
    /// 执行任务逻辑
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var (policyIds, archiveYear) = TryParseArchiveParams(context.ExecuteParams);
        if (policyIds.Count == 0 || archiveYear <= 0)
        {
            context.ExecuteMessage = "ExecuteParams 缺少有效 policyIds/archiveYear";
            TaktLogger.Warning("[TableArchive] Quartz 参数无效 Params={Params}", context.ExecuteParams);
            return;
        }
        var result = await _tableArchiveService.ExecuteTableArchiveAsync(new TaktTableArchiveExecuteDto
        {
            PolicyIds = policyIds,
            ArchiveYear = archiveYear,
        });
        var failed = result.Items.Count(x => !x.Success);
        context.ExecuteMessage = $"按年归档完成 year={archiveYear} total={result.Items.Count} failed={failed}";
        TaktLogger.Information(
            "[TableArchive] Quartz 归档完成 Year={Year} Total={Total} Failed={Failed}",
            archiveYear,
            result.Items.Count,
            failed);
    }

    /// <summary>
    /// 解析归档参数 JSON
    /// </summary>
    /// <param name="executeParams">执行参数</param>
    /// <returns>策略 ID 列表与年份</returns>
    private static (List<string> PolicyIds, int ArchiveYear) TryParseArchiveParams(string? executeParams)
    {
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return (new List<string>(), 0);
        }
        try
        {
            using var doc = JsonDocument.Parse(executeParams.Trim());
            var root = doc.RootElement;
            var year = 0;
            if (root.TryGetProperty("archiveYear", out var yearProp))
            {
                if (yearProp.ValueKind == JsonValueKind.Number && yearProp.TryGetInt32(out var y))
                {
                    year = y;
                }
                else if (yearProp.ValueKind == JsonValueKind.String && int.TryParse(yearProp.GetString(), out var ys))
                {
                    year = ys;
                }
            }
            var ids = new List<string>();
            if (root.TryGetProperty("policyIds", out var idsProp) && idsProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in idsProp.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        var s = item.GetString();
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            ids.Add(s.Trim());
                        }
                    }
                    else if (item.ValueKind == JsonValueKind.Number && item.TryGetInt64(out var n))
                    {
                        ids.Add(n.ToString());
                    }
                }
            }
            return (ids, year);
        }
        catch
        {
            return (new List<string>(), 0);
        }
    }
}
