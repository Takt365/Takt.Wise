// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine.Business
// 文件名称：TaktApprovalFlowBusinessService.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：通用审批业务回写（按表单 RelatedTableName + RelatedFormField 元数据，无流程枚举）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Workflow;

namespace Takt.Application.Services.Workflow.FlowEngine.Business;

/// <summary>
/// 流程与审批业务表通用集成（数据源表单 IsDatasource=1 时按 RelatedTableName 回写）
/// </summary>
public class TaktApprovalFlowBusinessService : TaktServiceBase
{
    private readonly ITaktCompanyRepository<TaktFlowForm> _flowFormRepository;
    private readonly ITaktApprovalFlowDataGateway _approvalFlowDataGateway;
    private readonly IReadOnlyDictionary<string, ITaktApprovalFlowCompletedContributor> _flowCompletedContributorMap;

    /// <summary>
    /// 初始化通用审批流程集成服务
    /// </summary>
    /// <param name="flowFormRepository">流程表单仓储</param>
    /// <param name="approvalFlowDataGateway">审批表数据网关</param>
    /// <param name="flowCompletedContributors">审批通过后业务回写贡献点集合</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    /// <exception cref="InvalidOperationException">存在重复 RelatedTableName 的 Contributor</exception>
    public TaktApprovalFlowBusinessService(
        ITaktCompanyRepository<TaktFlowForm> flowFormRepository,
        ITaktApprovalFlowDataGateway approvalFlowDataGateway,
        IEnumerable<ITaktApprovalFlowCompletedContributor> flowCompletedContributors,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowFormRepository = flowFormRepository;
        _approvalFlowDataGateway = approvalFlowDataGateway;
        _flowCompletedContributorMap = BuildFlowCompletedContributorMap(flowCompletedContributors);
    }

    /// <summary>
    /// 流程启动后：关联 FlowInstanceId 或从 FrmData 落库
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <returns>异步任务</returns>
    public async Task OnFlowStartedAsync(TaktFlowInstance instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        var form = await LoadDatasourceFormAsync(instance);
        if (form == null)
        {
            return;
        }
        var binding = TaktFlowFormBindingHelper.ParseBinding(form.RelatedFormField);
        if (!string.IsNullOrWhiteSpace(instance.BusinessKey) && long.TryParse(instance.BusinessKey, out var entityId))
        {
            await ApplyStartedPatchAsync(form.RelatedTableName!, entityId, instance, binding.Business);
            return;
        }
        var columns = TaktFlowFormBindingHelper.BuildDbColumnsFromFrmData(instance.FrmData, binding);
        var newId = await _approvalFlowDataGateway.InsertRowAsync(
            form.RelatedTableName!,
            columns,
            instance.TenantCode,
            instance.CompanyCode,
            instance.StartUserId);
        instance.BusinessKey = newId.ToString();
        if (string.IsNullOrWhiteSpace(instance.BusinessType))
        {
            instance.BusinessType = instance.ProcessKey;
        }
        await ApplyStartedPatchAsync(form.RelatedTableName!, newId, instance, binding.Business);
    }

    /// <summary>
    /// 流程终态回写审批表
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <returns>异步任务</returns>
    public async Task OnFlowTerminalAsync(TaktFlowInstance instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        var instanceStatus = (TaktFlowInstanceStatus)instance.InstanceStatus;
        if (instanceStatus is not (
            TaktFlowInstanceStatus.Completed
            or TaktFlowInstanceStatus.Rejected
            or TaktFlowInstanceStatus.Terminated))
        {
            return;
        }
        var form = await LoadDatasourceFormAsync(instance);
        if (form == null || string.IsNullOrWhiteSpace(instance.BusinessKey) || !long.TryParse(instance.BusinessKey, out var entityId))
        {
            return;
        }
        var binding = TaktFlowFormBindingHelper.ParseBinding(form.RelatedFormField);
        var approvalStatus = MapApprovalStatus(instanceStatus);
        var businessStatus = TaktFlowFormBindingHelper.ResolveBusinessStatusValue(binding.Business, instanceStatus);
        var patch = new TaktApprovalFlowStatePatch
        {
            ApprovalStatus = approvalStatus,
            BusinessStatusColumn = binding.Business?.BusinessStatusColumn,
            BusinessStatusValue = businessStatus
        };
        await _approvalFlowDataGateway.UpdateFlowStateAsync(
            form.RelatedTableName!,
            entityId,
            instance.TenantCode,
            instance.CompanyCode,
            instance.StartUserId,
            patch);
        if ((TaktFlowInstanceStatus)instance.InstanceStatus == TaktFlowInstanceStatus.Completed
            && !string.IsNullOrWhiteSpace(instance.FrmData))
        {
            var dataColumns = TaktFlowFormBindingHelper.BuildDbColumnsFromFrmData(instance.FrmData, binding);
            if (dataColumns.Count > 0)
            {
                await _approvalFlowDataGateway.UpdateRowColumnsAsync(
                    form.RelatedTableName!,
                    entityId,
                    instance.TenantCode,
                    instance.CompanyCode,
                    instance.StartUserId,
                    dataColumns);
            }
        }
        if ((TaktFlowInstanceStatus)instance.InstanceStatus == TaktFlowInstanceStatus.Completed)
        {
            await DispatchFlowCompletedContributorsAsync(instance, form.RelatedTableName!, entityId);
        }
    }

    /// <summary>
    /// 构建审批通过后回写 Contributor 索引（启动时校验 RelatedTableName 全应用唯一）
    /// </summary>
    /// <param name="flowCompletedContributors">Contributor 集合</param>
    /// <returns>表名到 Contributor 的只读字典</returns>
    /// <exception cref="InvalidOperationException">存在重复 RelatedTableName</exception>
    private static IReadOnlyDictionary<string, ITaktApprovalFlowCompletedContributor> BuildFlowCompletedContributorMap(
        IEnumerable<ITaktApprovalFlowCompletedContributor> flowCompletedContributors)
    {
        ArgumentNullException.ThrowIfNull(flowCompletedContributors);
        var map = new Dictionary<string, ITaktApprovalFlowCompletedContributor>(StringComparer.OrdinalIgnoreCase);
        foreach (var contributor in flowCompletedContributors)
        {
            ArgumentNullException.ThrowIfNull(contributor);
            var tableName = contributor.RelatedTableName;
            ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
            if (map.TryGetValue(tableName, out var existing))
            {
                throw new InvalidOperationException(
                    $"审批通过后回写 Contributor 表名重复：{tableName}（{existing.GetType().Name} 与 {contributor.GetType().Name}）");
            }
            map[tableName] = contributor;
        }
        return map;
    }

    /// <summary>
    /// 分发审批通过后业务回写（各审批实体应用服务实现 ITaktApprovalFlowCompletedContributor）
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="relatedTableName">数据源物理表名</param>
    /// <param name="entityId">业务单据主键</param>
    /// <returns>异步任务</returns>
    private async Task DispatchFlowCompletedContributorsAsync(
        TaktFlowInstance instance,
        string relatedTableName,
        long entityId)
    {
        if (!_flowCompletedContributorMap.TryGetValue(relatedTableName, out var contributor))
        {
            return;
        }
        var context = new TaktApprovalFlowCompletedContext
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            EntityId = entityId,
            OperatorUserId = instance.StartUserId
        };
        await contributor.OnApprovalFlowCompletedAsync(context);
    }

    /// <summary>
    /// 加载数据源表单
    /// </summary>
    private async Task<TaktFlowForm?> LoadDatasourceFormAsync(TaktFlowInstance instance)
    {
        if (!instance.FormId.HasValue || instance.FormId.Value <= 0)
        {
            return null;
        }
        var form = await _flowFormRepository.GetByIdAsync(instance.FormId.Value);
        if (form == null
            || form.TenantCode != instance.TenantCode
            || form.CompanyCode != instance.CompanyCode
            || form.IsDatasource != 1
            || string.IsNullOrWhiteSpace(form.RelatedTableName))
        {
            return null;
        }
        return form;
    }

    /// <summary>
    /// 启动后补丁（审批中 + 关联实例）
    /// </summary>
    private async Task ApplyStartedPatchAsync(
        string tableName,
        long entityId,
        TaktFlowInstance instance,
        TaktFlowFormBusinessBinding? business)
    {
        var patch = new TaktApprovalFlowStatePatch
        {
            FlowInstanceId = instance.Id,
            ApprovalStatus = 1,
            InitiatorId = instance.StartUserId,
            InitiatedAt = instance.StartTime,
            BusinessStatusColumn = business?.BusinessStatusColumn,
            BusinessStatusValue = business?.StatusInProgress
        };
        await _approvalFlowDataGateway.UpdateFlowStateAsync(
            tableName,
            entityId,
            instance.TenantCode,
            instance.CompanyCode,
            instance.StartUserId,
            patch);
    }

    /// <summary>
    /// 流程终态映射审批状态
    /// </summary>
    private static int MapApprovalStatus(TaktFlowInstanceStatus instanceStatus)
    {
        switch (instanceStatus)
        {
            case TaktFlowInstanceStatus.Completed:
                return 2;
            case TaktFlowInstanceStatus.Rejected:
                return 3;
            case TaktFlowInstanceStatus.Terminated:
                return 4;
            default:
                return 0;
        }
    }
}
