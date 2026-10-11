// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHandoffMailService.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施顺序邮件：按 TaktEcGroup 类别发信，并写入 TaktEcNotification / TaktEcNotificationDelivery
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变实施顺序通知。技术保存后通知生管与制技；部门全部已实施后通知下一环节；品管完成后通知全部类别。
/// 发信结果写入工程变更通知单与按部门投递记录，不改设变主表 ExtField。
/// </summary>
public class TaktEcHandoffMailService
{
    private const string LedgerMarker = "ec-handoff:";
    private const int NotificationMethodEmail = 2;
    private const int NotificationStatusNotified = 1;
    private static string FillPrompt(string ecCode) => $"【待办】您有设计变更{ecCode}的实施内容尚未填写，请及时处理";
    private static string BomBeforePrompt(string ecCode) => $"【提醒】设计变更{ecCode}将于30天后生效，请填写实施内容";
    private static string BomAfterPrompt(string ecCode) => $"【逾期】设计变更{ecCode}生效已逾期30天，请尽快补填实施内容";
    private const string MailFooter = "本邮件由系统自动发送，无需回复。如需帮助，请联系管理员。";

    private readonly ITaktCompanyRepository<TaktEcGijutsu> _gijutsuRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _detailRepository;
    private readonly ITaktCompanyRepository<TaktEcGroup> _groupRepository;
    private readonly ITaktApprovalRepository<TaktEcNotification> _notificationRepository;
    private readonly ITaktCompanyRepository<TaktEcNotificationDelivery> _deliveryRepository;
    private readonly ITaktNumberingGenerator _numberingGenerator;
    private readonly TaktEcExecDeptAccess _deptAccess;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="gijutsuRepository">设变技术课主仓储</param>
    /// <param name="detailRepository">设变明细仓储</param>
    /// <param name="groupRepository">设变组仓储</param>
    /// <param name="notificationRepository">工程变更通知单仓储</param>
    /// <param name="deliveryRepository">工程变更通知投递仓储</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="deptAccess">部门执行跨表访问</param>
    /// <param name="configuration">邮件配置</param>
    public TaktEcHandoffMailService(
        ITaktCompanyRepository<TaktEcGijutsu> gijutsuRepository,
        ITaktCompanyRepository<TaktEcDetail> detailRepository,
        ITaktCompanyRepository<TaktEcGroup> groupRepository,
        ITaktApprovalRepository<TaktEcNotification> notificationRepository,
        ITaktCompanyRepository<TaktEcNotificationDelivery> deliveryRepository,
        ITaktNumberingGenerator numberingGenerator,
        TaktEcExecDeptAccess deptAccess,
        IConfiguration configuration)
    {
        _gijutsuRepository = gijutsuRepository;
        _detailRepository = detailRepository;
        _groupRepository = groupRepository;
        _notificationRepository = notificationRepository;
        _deliveryRepository = deliveryRepository;
        _numberingGenerator = numberingGenerator;
        _deptAccess = deptAccess;
        _configuration = configuration;
    }

    /// <summary>
    /// 按设变单号加载明细与执行行后派发尚未发送的环节邮件
    /// </summary>
    /// <param name="gijutsu">设变主表</param>
    /// <returns>任务</returns>
    public async Task TryDispatchAsync(TaktEcGijutsu gijutsu)
    {
        ArgumentNullException.ThrowIfNull(gijutsu);
        if (string.IsNullOrWhiteSpace(gijutsu.EcCode))
        {
            return;
        }
        var code = gijutsu.EcCode.Trim();
        var details = await _detailRepository.GetListAsync(x => x.EcCode == code && x.IsObsolete == 0);
        var execRows = await _deptAccess.ListBaseByEcCodeAsync(code);
        await TryDispatchAsync(gijutsu, details, execRows);
    }

    /// <summary>
    /// BOM 生效日前 30 天、后 30 天，对仍未实施的部门按设变组类别再发一封邮件
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>成功发出的邮件封数</returns>
    public async Task<int> SendBomDateRemindersAsync(
        string tenantCode,
        string companyCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var today = DateTime.Today;
        var before = today.AddDays(30);
        var after = today.AddDays(-30);
        var beforeEnd = before.AddDays(1);
        var afterEnd = after.AddDays(1);
        var details = await _detailRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && ((x.EcBomDate >= before && x.EcBomDate < beforeEnd)
                || (x.EcBomDate >= after && x.EcBomDate < afterEnd)));
        if (details.Count == 0)
        {
            return 0;
        }
        var sentCount = 0;
        foreach (var group in details.GroupBy(x => x.EcCode?.Trim() ?? string.Empty, StringComparer.OrdinalIgnoreCase))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(group.Key))
            {
                continue;
            }
            var gijutsu = await _gijutsuRepository.FirstAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.EcCode == group.Key);
            if (gijutsu == null)
            {
                continue;
            }
            var execRows = await _deptAccess.ListBaseByEcCodeAsync(group.Key);
            var notification = await EnsureNotificationAsync(gijutsu);
            var sent = ReadMergedTokens(notification, gijutsu);
            var changed = false;
            foreach (var phase in new[] { (Token: "bomb", Day: before, After: false), (Token: "boma", Day: after, After: true) })
            {
                var phaseDetails = group.Where(x => x.EcBomDate >= phase.Day && x.EcBomDate < phase.Day.AddDays(1)).ToList();
                if (phaseDetails.Count == 0)
                {
                    continue;
                }
                var detailIds = phaseDetails.Select(x => x.Id).ToHashSet();
                var openDepts = execRows
                    .Where(x => x.IsObsolete == 0 && x.IsImplemented != 1 && detailIds.Contains(x.EcDetailId))
                    .Select(x => x.DeptCode)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                var bomDate = phaseDetails[0].EcBomDate.Date;
                foreach (var deptCode in openDepts)
                {
                    if (!TryMapDeptCategory(deptCode, out var category))
                    {
                        continue;
                    }
                    var token = $"{phase.Token}:{bomDate:yyyyMMdd}:{deptCode}";
                    if (sent.Contains(token))
                    {
                        continue;
                    }
                    var emails = await ResolveEmailsAsync(gijutsu.PlantCode, [category]);
                    if (emails.Count == 0)
                    {
                        TaktLogger.Warning(
                            "[EcBomRemind] 无收件邮箱 EcCode={EcCode} Dept={Dept} Date={Date}",
                            group.Key,
                            deptCode,
                            bomDate.ToString("yyyy-MM-dd"));
                        continue;
                    }
                    var bodyText = phase.After ? BomAfterPrompt(group.Key) : BomBeforePrompt(group.Key);
                    var subject = bodyText;
                    var body = WithFooter(bodyText);
                    var failed = false;
                    foreach (var email in emails)
                    {
                        try
                        {
                            await TaktMailHelper.SendEmailAsync(_configuration, email, subject, body, false);
                            sentCount++;
                        }
                        catch (Exception ex)
                        {
                            failed = true;
                            TaktLogger.Error(ex, "[EcBomRemind] 发送失败 EcCode={EcCode} Email={Email}", group.Key, email);
                        }
                    }
                    if (failed)
                    {
                        continue;
                    }
                    sent.Add(token);
                    await MarkDeptSentAsync(notification, deptCode);
                    changed = true;
                }
            }
            if (!changed)
            {
                continue;
            }
            await SaveNotificationLedgerAsync(notification, sent);
        }
        return sentCount;
    }

    /// <summary>
    /// 执行部门对应的设变组类别
    /// </summary>
    private static bool TryMapDeptCategory(string deptCode, out int category)
    {
        category = deptCode switch
        {
            TaktEcDeptCodes.Pmc => TaktEcGroupCategoryConstants.Pmc,
            TaktEcDeptCodes.Te => TaktEcGroupCategoryConstants.Sop,
            TaktEcDeptCodes.Mp => TaktEcGroupCategoryConstants.Mp,
            TaktEcDeptCodes.Mc => TaktEcGroupCategoryConstants.Mc,
            TaktEcDeptCodes.Iqc => TaktEcGroupCategoryConstants.Iqc,
            TaktEcDeptCodes.Smt => TaktEcGroupCategoryConstants.Smt,
            TaktEcDeptCodes.Pcba => TaktEcGroupCategoryConstants.Pcba,
            TaktEcDeptCodes.Assy => TaktEcGroupCategoryConstants.Assy,
            TaktEcDeptCodes.Qa => TaktEcGroupCategoryConstants.Qa,
            _ => 0,
        };
        return category > 0;
    }

    /// <summary>
    /// 用已加载的明细与执行行派发尚未发送的环节邮件
    /// </summary>
    /// <param name="gijutsu">设变主表</param>
    /// <param name="details">未作废明细</param>
    /// <param name="execRows">部门执行行</param>
    /// <returns>任务</returns>
    public async Task TryDispatchAsync(
        TaktEcGijutsu gijutsu,
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<TaktEcExecBaseRow> execRows)
    {
        ArgumentNullException.ThrowIfNull(gijutsu);
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(execRows);
        if (string.IsNullOrWhiteSpace(gijutsu.EcCode))
        {
            return;
        }
        var notification = await EnsureNotificationAsync(gijutsu);
        var sent = ReadMergedTokens(notification, gijutsu);
        var changed = false;
        foreach (var step in BuildSteps(gijutsu, details, execRows))
        {
            if (sent.Contains(step.Token))
            {
                continue;
            }
            var emails = await ResolveEmailsAsync(gijutsu.PlantCode, step.Categories);
            if (emails.Count == 0)
            {
                TaktLogger.Warning(
                    "[EcHandoff] 无收件邮箱，本步留待下次重试 EcCode={EcCode} Step={Step}",
                    gijutsu.EcCode,
                    step.Token);
                continue;
            }
            var failed = false;
            foreach (var email in emails)
            {
                try
                {
                    await TaktMailHelper.SendEmailAsync(_configuration, email, step.Subject, step.Body, false);
                }
                catch (Exception ex)
                {
                    failed = true;
                    TaktLogger.Error(ex, "[EcHandoff] 发送失败 EcCode={EcCode} Step={Step} Email={Email}", gijutsu.EcCode, step.Token, email);
                }
            }
            if (failed)
            {
                continue;
            }
            sent.Add(step.Token);
            await MarkCategoriesSentAsync(notification, step.Categories);
            changed = true;
            TaktLogger.Information(
                "[EcHandoff] 已发送 EcCode={EcCode} Step={Step} Count={Count}",
                gijutsu.EcCode,
                step.Token,
                emails.Count);
        }
        if (!changed)
        {
            return;
        }
        await SaveNotificationLedgerAsync(notification, sent);
    }

    /// <summary>
    /// 当前应发送、且前置部门已全部实施的步骤
    /// </summary>
    private static List<HandoffStep> BuildSteps(
        TaktEcGijutsu gijutsu,
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<TaktEcExecBaseRow> execRows)
    {
        var code = gijutsu.EcCode.Trim();
        var title = string.IsNullOrWhiteSpace(gijutsu.EcTitle) ? code : $"{code} {gijutsu.EcTitle.Trim()}";
        var steps = new List<HandoffStep>
        {
            Step(
                "tech",
                [TaktEcGroupCategoryConstants.Pmc, TaktEcGroupCategoryConstants.Sop],
                FillPrompt(code),
                WithFooter(FillPrompt(code))),
        };
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Pmc))
        {
            steps.Add(Step(
                "pmc",
                [TaktEcGroupCategoryConstants.Mp],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Mp))
        {
            AddRouteSteps(steps, details, code, "mp");
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Iqc))
        {
            var inspected = details.Where(x => x.EcNewRequiresInspection == 1).ToList();
            AddRouteSteps(steps, inspected.Count > 0 ? inspected : details, code, "iqc");
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Mc))
        {
            steps.Add(Step(
                "mc",
                [TaktEcGroupCategoryConstants.Assy, TaktEcGroupCategoryConstants.Pcba],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Smt))
        {
            steps.Add(Step(
                "smt",
                [TaktEcGroupCategoryConstants.Pcba],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Pcba))
        {
            steps.Add(Step(
                "pcba",
                [TaktEcGroupCategoryConstants.Assy],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Assy))
        {
            steps.Add(Step(
                "assy",
                [TaktEcGroupCategoryConstants.Qa],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (IsDeptImplemented(execRows, TaktEcDeptCodes.Qa))
        {
            steps.Add(Step(
                "qa",
                [
                    TaktEcGroupCategoryConstants.EcOwner,
                    TaktEcGroupCategoryConstants.Sop,
                    TaktEcGroupCategoryConstants.Pmc,
                    TaktEcGroupCategoryConstants.Mp,
                    TaktEcGroupCategoryConstants.Mc,
                    TaktEcGroupCategoryConstants.Iqc,
                    TaktEcGroupCategoryConstants.Smt,
                    TaktEcGroupCategoryConstants.Pcba,
                    TaktEcGroupCategoryConstants.Assy,
                    TaktEcGroupCategoryConstants.Qa,
                ],
                $"设变 {code}：实施完成",
                WithFooter($"设变 {title} 品管已实施，实施完成。")));
        }
        return steps;
    }

    /// <summary>
    /// 采购或受检完成后，按外购仓是否 C003 分别通知部管或 SMT
    /// </summary>
    private static void AddRouteSteps(
        List<HandoffStep> steps,
        IReadOnlyList<TaktEcDetail> details,
        string code,
        string tokenPrefix)
    {
        if (details.Any(x => TaktEcScopeConstants.IsBukanVisible(x.EcNewPurchaseType, x.EcNewWarehouse)))
        {
            steps.Add(Step(
                tokenPrefix + "-mc",
                [TaktEcGroupCategoryConstants.Mc],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
        if (details.Any(x => TaktEcScopeConstants.IsPcbaC003ExternalGroup(x.EcNewPurchaseType, x.EcNewWarehouse)))
        {
            steps.Add(Step(
                tokenPrefix + "-smt",
                [TaktEcGroupCategoryConstants.Smt],
                FillPrompt(code),
                WithFooter(FillPrompt(code))));
        }
    }

    /// <summary>
    /// 该部门存在未作废执行行且全部已实施
    /// </summary>
    private static bool IsDeptImplemented(IReadOnlyList<TaktEcExecBaseRow> execRows, string deptCode)
    {
        var rows = execRows.Where(x => x.IsObsolete == 0 && string.Equals(x.DeptCode, deptCode, StringComparison.OrdinalIgnoreCase)).ToList();
        return rows.Count > 0 && rows.All(x => x.IsImplemented == 1);
    }

    /// <summary>
    /// 同工厂、启用设变组中指定类别的联系邮箱（去重）
    /// </summary>
    private async Task<List<string>> ResolveEmailsAsync(string plantCode, int[] categories)
    {
        var groups = await _groupRepository.GetListAsync(x =>
            x.PlantCode == plantCode
            && x.GroupStatus == 1
            && x.ContactEmail != null
            && x.ContactEmail != "");
        return groups
            .Where(x => categories.Select(TaktEcGroupCategoryConstants.ToDeptCode).Contains(x.EcGroupCategory, StringComparer.OrdinalIgnoreCase))
            .Select(x => x.ContactEmail!.Trim())
            .Where(x => x.Contains('@'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 取已有通知单；没有则按邮件通知编码规则创建一笔
    /// </summary>
    private async Task<TaktEcNotification> EnsureNotificationAsync(TaktEcGijutsu gijutsu)
    {
        var existing = await _notificationRepository.FirstAsync(x =>
            x.EcGijutsuId == gijutsu.Id && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing;
        }
        var generated = await _numberingGenerator.GenerateNextAsync(TaktEcFlowConstants.NotificationRuleEmail);
        if (string.IsNullOrWhiteSpace(generated.BusinessCode))
        {
            throw new TaktBusinessException("工程变更通知单号生成失败");
        }
        var notification = new TaktEcNotification
        {
            TenantCode = gijutsu.TenantCode,
            CompanyCode = gijutsu.CompanyCode,
            PlantCode = gijutsu.PlantCode,
            EcNotificationCode = generated.BusinessCode.Trim(),
            EcGijutsuId = gijutsu.Id,
            EcCode = gijutsu.EcCode.Trim(),
            EcTitle = gijutsu.EcTitle,
            EcNotificationDate = DateTime.Today,
            EcNotificationMethod = NotificationMethodEmail,
            EcNotificationStatus = 0,
        };
        return await _notificationRepository.CreateAsync(notification);
    }

    /// <summary>
    /// 已发送步骤：通知单 ExtField，并合并主表上的历史标记
    /// </summary>
    private static HashSet<string> ReadMergedTokens(TaktEcNotification notification, TaktEcGijutsu gijutsu)
    {
        var sent = ReadTokens(notification.ExtField);
        foreach (var token in ReadTokens(gijutsu.ExtField))
        {
            sent.Add(token);
        }
        return sent;
    }

    /// <summary>
    /// 把已发送步骤写回通知单，并把状态改为已通知
    /// </summary>
    private async Task SaveNotificationLedgerAsync(TaktEcNotification notification, HashSet<string> tokens)
    {
        notification.ExtField = WriteTokens(notification.ExtField, tokens);
        notification.EcNotificationMethod = NotificationMethodEmail;
        if (notification.EcNotificationStatus == 0)
        {
            notification.EcNotificationStatus = NotificationStatusNotified;
        }
        await _notificationRepository.UpdateAsync(notification);
    }

    /// <summary>
    /// 设变组类别对应的执行部门
    /// </summary>
    private static string? CategoryToDeptCode(int category) => category switch
    {
        TaktEcGroupCategoryConstants.EcOwner => TaktEcDeptCodes.Eng,
        TaktEcGroupCategoryConstants.Sop => TaktEcDeptCodes.Te,
        TaktEcGroupCategoryConstants.Pmc => TaktEcDeptCodes.Pmc,
        TaktEcGroupCategoryConstants.Mp => TaktEcDeptCodes.Mp,
        TaktEcGroupCategoryConstants.Mc => TaktEcDeptCodes.Mc,
        TaktEcGroupCategoryConstants.Iqc => TaktEcDeptCodes.Iqc,
        TaktEcGroupCategoryConstants.Smt => TaktEcDeptCodes.Smt,
        TaktEcGroupCategoryConstants.Pcba => TaktEcDeptCodes.Pcba,
        TaktEcGroupCategoryConstants.Assy => TaktEcDeptCodes.Assy,
        TaktEcGroupCategoryConstants.Qa => TaktEcDeptCodes.Qa,
        _ => null,
    };

    /// <summary>
    /// 本步各类别对应部门写入投递记录
    /// </summary>
    private async Task MarkCategoriesSentAsync(TaktEcNotification notification, int[] categories)
    {
        foreach (var category in categories)
        {
            var deptCode = CategoryToDeptCode(category);
            if (string.IsNullOrWhiteSpace(deptCode))
            {
                continue;
            }
            await MarkDeptSentAsync(notification, deptCode);
        }
    }

    /// <summary>
    /// 部门投递记为已发送；通知单部门清单补上该部门
    /// </summary>
    private async Task MarkDeptSentAsync(TaktEcNotification notification, string deptCode)
    {
        var code = deptCode.Trim();
        var name = TaktEcDeptCodes.GetDisplayName(code);
        AppendDept(notification, code, name);
        var delivery = await _deliveryRepository.FirstAsync(x =>
            x.EcNotificationId == notification.Id
            && x.EcDeptGroupCode == code
            && x.IsDeleted == 0);
        var now = DateTime.Now;
        if (delivery == null)
        {
            delivery = new TaktEcNotificationDelivery
            {
                TenantCode = notification.TenantCode,
                CompanyCode = notification.CompanyCode,
                PlantCode = notification.PlantCode,
                EcNotificationId = notification.Id,
                EcNotificationCode = notification.EcNotificationCode,
                EcGijutsuId = notification.EcGijutsuId,
                EcCode = notification.EcCode,
                EcDeptGroupCode = code,
                EcDeptGroupName = name,
                Priority = TaktEcFlowConstants.PriorityNormal,
                DeliveryStatus = TaktEcFlowConstants.DeliveryStatusSent,
                SentAt = now,
            };
            await _deliveryRepository.CreateAsync(delivery);
            return;
        }
        if (delivery.DeliveryStatus >= TaktEcFlowConstants.DeliveryStatusConfirmed)
        {
            return;
        }
        delivery.DeliveryStatus = TaktEcFlowConstants.DeliveryStatusSent;
        delivery.SentAt = now;
        if (string.IsNullOrWhiteSpace(delivery.EcDeptGroupName))
        {
            delivery.EcDeptGroupName = name;
        }
        await _deliveryRepository.UpdateAsync(delivery);
    }

    /// <summary>
    /// 通知单部门编码、名称按逗号追加
    /// </summary>
    private static void AppendDept(TaktEcNotification notification, string deptCode, string deptName)
    {
        var codes = (notification.EcNotificationDeptCodes ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        if (codes.Contains(deptCode, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }
        codes.Add(deptCode);
        var names = (notification.EcNotificationDeptNames ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        names.Add(deptName);
        notification.EcNotificationDeptCodes = string.Join(",", codes);
        notification.EcNotificationDeptNames = string.Join(",", names);
    }

    private static string WithFooter(string content) => $"{content}\n\n{MailFooter}";

    private static HandoffStep Step(string token, int[] categories, string subject, string body) =>
        new(token, categories, subject, body);

    private static HashSet<string> ReadTokens(string? extField)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(extField))
        {
            return set;
        }
        var index = extField.IndexOf(LedgerMarker, StringComparison.Ordinal);
        if (index < 0)
        {
            return set;
        }
        var rest = extField[(index + LedgerMarker.Length)..];
        var end = rest.IndexOf('|');
        if (end >= 0)
        {
            rest = rest[..end];
        }
        foreach (var part in rest.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            set.Add(part);
        }
        return set;
    }

    private static string WriteTokens(string? extField, HashSet<string> tokens)
    {
        var payload = LedgerMarker + string.Join(',', tokens.OrderBy(x => x, StringComparer.Ordinal));
        if (string.IsNullOrEmpty(extField))
        {
            return payload;
        }
        var index = extField.IndexOf(LedgerMarker, StringComparison.Ordinal);
        if (index < 0)
        {
            return extField.TrimEnd('|') + "|" + payload;
        }
        var end = extField.IndexOf('|', index + LedgerMarker.Length);
        var before = extField[..index].TrimEnd('|');
        var after = end < 0 ? string.Empty : extField[end..];
        return string.IsNullOrEmpty(before) ? payload + after : before + "|" + payload + after;
    }

    private sealed record HandoffStep(string Token, int[] Categories, string Subject, string Body);
}
