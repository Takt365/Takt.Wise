// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine.Business
// 文件名称：TaktApprovalFlowSubmitService.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：通用「业务表 + 已发布方案」提交审批（按 RelatedTableName 发现表单/流程）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Workflow;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Workflow;

namespace Takt.Application.Services.Workflow.FlowEngine.Business;

/// <summary>
/// 按物理表名提交审批（新增流程只需配置表单 RelatedTableName + 方案 FormId，无需 C# 枚举）
/// </summary>
public class TaktApprovalFlowSubmitService : TaktServiceBase
{
    private readonly ITaktCompanyRepository<TaktFlowForm> _flowFormRepository;
    private readonly ITaktCompanyRepository<TaktFlowScheme> _flowSchemeRepository;
    private readonly ITaktApprovalFlowDataGateway _approvalFlowDataGateway;
    private readonly ITaktFlowEngineService _flowEngineService;

    /// <summary>
    /// 初始化提交服务
    /// </summary>
    public TaktApprovalFlowSubmitService(
        ITaktCompanyRepository<TaktFlowForm> flowFormRepository,
        ITaktCompanyRepository<TaktFlowScheme> flowSchemeRepository,
        ITaktApprovalFlowDataGateway approvalFlowDataGateway,
        ITaktFlowEngineService flowEngineService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowFormRepository = flowFormRepository;
        _flowSchemeRepository = flowSchemeRepository;
        _approvalFlowDataGateway = approvalFlowDataGateway;
        _flowEngineService = flowEngineService;
    }

    /// <summary>
    /// 按关联表提交审批
    /// </summary>
    /// <param name="relatedTableName">表单 RelatedTableName（审批实体物理表名）</param>
    /// <param name="entityId">业务主键</param>
    /// <param name="processKey">可选流程键；未传时取绑定该表单且已发布的方案</param>
    /// <returns>流程实例详情</returns>
    public async Task<TaktFlowInstanceDetailDto> SubmitForApprovalByTableAsync(
        string relatedTableName,
        long entityId,
        string? processKey = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relatedTableName);
        EnsureThreeLayerContext();
        var form = await _flowFormRepository.FirstAsync(f =>
            f.TenantCode == CurrentTenantCode
            && f.CompanyCode == CurrentCompanyCode
            && f.RelatedTableName == relatedTableName
            && f.IsDatasource == 1
            && f.FormStatus == 1);
        if (form == null)
        {
            throw new TaktBusinessException($"未找到表「{relatedTableName}」对应的数据源流程表单");
        }
        var scheme = await ResolvePublishedSchemeAsync(form, processKey);
        var row = await _approvalFlowDataGateway.GetRowByIdAsync(
            relatedTableName,
            entityId,
            CurrentTenantCode,
            CurrentCompanyCode);
        if (row == null)
        {
            throw new TaktBusinessException("业务单据不存在");
        }
        var binding = TaktFlowFormBindingHelper.ParseBinding(form.RelatedFormField);
        ValidateSubmitAllowed(row, binding.Business);
        var frmData = TaktFlowFormBindingHelper.BuildFrmDataFromDbRow(row, binding);
        var processTitle = BuildDefaultProcessTitle(row, scheme.ProcessName);
        return await _flowEngineService.StartFlowInstanceAsync(new TaktFlowStartDto
        {
            ProcessKey = scheme.ProcessKey,
            BusinessType = scheme.ProcessKey,
            BusinessKey = entityId.ToString(),
            ProcessTitle = processTitle,
            FrmData = frmData
        });
    }

    /// <summary>
    /// 解析已发布方案
    /// </summary>
    private async Task<TaktFlowScheme> ResolvePublishedSchemeAsync(TaktFlowForm form, string? processKey)
    {
        TaktFlowScheme? scheme;
        if (!string.IsNullOrWhiteSpace(processKey))
        {
            scheme = await _flowSchemeRepository.FirstAsync(s =>
                s.TenantCode == CurrentTenantCode
                && s.CompanyCode == CurrentCompanyCode
                && s.ProcessKey == processKey
                && s.IsLatest == 1
                && s.ProcessStatus == 1
                && s.SuspensionState == (int)TaktFlowSuspensionState.Active);
        }
        else
        {
            scheme = await _flowSchemeRepository.FirstAsync(s =>
                s.TenantCode == CurrentTenantCode
                && s.CompanyCode == CurrentCompanyCode
                && s.FormId == form.Id
                && s.IsLatest == 1
                && s.ProcessStatus == 1
                && s.SuspensionState == (int)TaktFlowSuspensionState.Active);
        }
        if (scheme == null)
        {
            throw new TaktBusinessException("未找到已发布的流程方案");
        }
        return scheme;
    }

    /// <summary>
    /// 校验是否允许提交
    /// </summary>
    private static void ValidateSubmitAllowed(IReadOnlyDictionary<string, object?> row, TaktFlowFormBusinessBinding? business)
    {
        var approvalObj = row.TryGetValue("approval_status", out var a) ? a : null;
        if (approvalObj != null && int.TryParse(approvalObj.ToString(), out var approvalStatus)
            && approvalStatus == (int)1)
        {
            throw new TaktBusinessException("审批进行中，请勿重复提交");
        }
        if (business?.SubmitAllowedBusinessStatuses == null
            || business.SubmitAllowedBusinessStatuses.Count == 0
            || string.IsNullOrWhiteSpace(business.BusinessStatusColumn))
        {
            return;
        }
        var statusObj = row.TryGetValue(business.BusinessStatusColumn, out var s) ? s : null;
        if (statusObj == null || !int.TryParse(statusObj.ToString(), out var businessStatus))
        {
            throw new TaktBusinessException("当前业务状态不允许提交审批");
        }
        if (!business.SubmitAllowedBusinessStatuses.Contains(businessStatus))
        {
            throw new TaktBusinessException("当前业务状态不允许提交审批");
        }
    }

    /// <summary>
    /// 默认申请标题
    /// </summary>
    private static string BuildDefaultProcessTitle(IReadOnlyDictionary<string, object?> row, string processName)
    {
        if (row.TryGetValue("title", out var title) && title != null && !string.IsNullOrWhiteSpace(title.ToString()))
        {
            return title.ToString()!;
        }
        if (row.TryGetValue("employee_name", out var emp) && emp != null && !string.IsNullOrWhiteSpace(emp.ToString()))
        {
            return $"{emp} - {processName}";
        }
        return processName;
    }
}
