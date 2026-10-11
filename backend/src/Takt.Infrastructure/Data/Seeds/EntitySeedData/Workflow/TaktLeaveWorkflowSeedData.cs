// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData.Workflow
// 文件名称：TaktLeaveWorkflowSeedData.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：请假工作流种子（leave_form 表单、Leave 流程方案、演示实例/任务/流转及 TaktLeave 关联）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Takt.Application.Services.Workflow.FlowEngine;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData.Workflow;

/// <summary>
/// 请假工作流种子：流程表单、已发布方案、演示实例（草稿 / 审批中 / 已完成）及业务请假单
/// </summary>
public class TaktLeaveWorkflowSeedData : ITaktSeedDataCoordinator
{
    private const string FormCode = "leave_form";
    private const string ProcessKey = "Leave";
    private const string BusinessType = "Leave";
    private const string NodeStart = "leave_start";
    private const string NodeDeptManager = "leave_dept_manager";
    private const string NodeHrConfirm = "leave_hr_confirm";
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    /// <summary>
    /// 执行顺序（在用户-公司、员工-部门关联之后）
    /// </summary>
    public int Order => 66;

    /// <summary>
    /// 初始化请假工作流种子数据（幂等：存在则更新，不存在则创建）
    /// 写入 leave_form 表单、Leave 流程方案、演示实例（草稿/审批中/已完成）、任务、流转及 TaktLeave 业务单
    /// </summary>
    /// <param name="serviceProvider">服务提供者（解析仓储、配置与种子上下文）</param>
    /// <param name="tenantCode">租户编码；为空时跳过</param>
    /// <returns>插入与更新记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化请假工作流种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过请假工作流种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var employeeRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEmployee>>();
        var deptRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDept>>();
        var formRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowForm>>();
        var schemeRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowScheme>>();
        var instanceRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowInstance>>();
        var taskRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowTask>>();
        var transitionRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowTransition>>();
        var leaveRepository = serviceProvider.GetRequiredService<ITaktApprovalSeedRepository<TaktLeave>>();
        var seedContext = serviceProvider.GetRequiredService<TaktSeedContext>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过请假工作流种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过请假工作流种子", tenantCode);
            return (0, 0);
        }
        var adminUser = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == "admin");
        var demoUser = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == "demo");
        var guestUser = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == "guest");
        if (adminUser == null || demoUser == null || guestUser == null)
        {
            TaktLogger.Warning("租户 {TenantCode} 缺少 admin/demo/guest 用户，跳过请假工作流种子", tenantCode);
            return (0, 0);
        }
        int insertCount = 0;
        int updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化请假工作流...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            var demoEmployee = await employeeRepository.FirstAsync(e =>
                e.TenantCode == tenantCode
                && e.CompanyCode == company.CompanyCode
                && e.EmployeeCode == "900003");
            if (demoEmployee == null)
            {
                TaktLogger.Warning("公司 {CompanyCode} 缺少演示员工 900003，跳过该公司请假工作流种子", company.CompanyCode);
                continue;
            }
            var demoDept = await deptRepository.FirstAsync(d =>
                d.TenantCode == tenantCode
                && d.CompanyCode == company.CompanyCode
                && d.DeptCode == "D0820");
            var (form, fi, fu) = await UpsertLeaveFormAsync(
                formRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode);
            insertCount += fi;
            updateCount += fu;
            var processContent = BuildProcessContent(
                adminUser.Id,
                adminUser.NickName ?? adminUser.UserName,
                guestUser.Id,
                guestUser.NickName ?? guestUser.UserName);
            var (scheme, si, su) = await UpsertLeaveSchemeAsync(
                schemeRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                form,
                processContent);
            insertCount += si;
            updateCount += su;
            var frmData = BuildFrmData(demoEmployee, demoDept, "annual", new DateTime(2026, 6, 10), new DateTime(2026, 6, 11), "演示：年假申请");
            var (draftLeave, dli, dlu) = await UpsertLeaveAsync(
                leaveRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                demoEmployee,
                demoDept,
                demoUser,
                "annual",
                new DateTime(2026, 6, 15),
                new DateTime(2026, 6, 16),
                "演示：请假草稿",
                0,
                0,
                null);
            insertCount += dli;
            updateCount += dlu;
            var (runningLeave, rli, rlu) = await UpsertLeaveAsync(
                leaveRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                demoEmployee,
                demoDept,
                demoUser,
                "annual",
                new DateTime(2026, 6, 10),
                new DateTime(2026, 6, 11),
                "演示：年假审批中",
                1,
                1,
                null);
            insertCount += rli;
            updateCount += rlu;
            var (doneLeave, oli, olu) = await UpsertLeaveAsync(
                leaveRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                demoEmployee,
                demoDept,
                demoUser,
                "sick",
                new DateTime(2026, 6, 8),
                new DateTime(2026, 6, 9),
                "演示：病假已通过",
                2,
                2,
                null);
            insertCount += oli;
            updateCount += olu;
            var instanceCodeDraft = $"WFLEAVE-DRAFT-{company.CompanyCode}";
            var instanceCodeRunning = $"WFLEAVE-RUN-{company.CompanyCode}";
            var instanceCodeDone = $"WFLEAVE-DONE-{company.CompanyCode}";
            var (draftInstance, idi, idu) = await UpsertInstanceAsync(
                instanceRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                scheme,
                instanceCodeDraft,
                "演示：请假草稿",
                TaktFlowInstanceStatus.Draft,
                demoUser,
                frmData,
                draftLeave.Id.ToString());
            insertCount += idi;
            updateCount += idu;
            var (runningInstance, iri, iru) = await UpsertInstanceAsync(
                instanceRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                scheme,
                instanceCodeRunning,
                "演示：年假审批中",
                TaktFlowInstanceStatus.Running,
                demoUser,
                frmData,
                runningLeave.Id.ToString(),
                NodeDeptManager,
                "直属主管审批",
                DateTime.Now.AddDays(-1));
            insertCount += iri;
            updateCount += iru;
            var (doneInstance, idi2, idu2) = await UpsertInstanceAsync(
                instanceRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                scheme,
                instanceCodeDone,
                "演示：病假已通过",
                TaktFlowInstanceStatus.Completed,
                demoUser,
                BuildFrmData(demoEmployee, demoDept, "sick", new DateTime(2026, 6, 8), new DateTime(2026, 6, 9), "演示：病假已通过"),
                doneLeave.Id.ToString(),
                null,
                null,
                DateTime.Now.AddDays(-3),
                DateTime.Now.AddDays(-2));
            insertCount += idi2;
            updateCount += idu2;
            await SyncDraftInstanceAsync(seedContext, draftInstance);
            var (rti, rtu) = await SyncRunningInstanceAsync(
                seedContext,
                taskRepository,
                transitionRepository,
                runningInstance,
                demoUser,
                adminUser);
            insertCount += rti;
            updateCount += rtu;
            var (dti, dtu) = await SyncCompletedInstanceAsync(
                seedContext,
                taskRepository,
                transitionRepository,
                doneInstance,
                demoUser,
                adminUser,
                guestUser);
            insertCount += dti;
            updateCount += dtu;
            draftLeave.FlowInstanceId = draftInstance.Id;
            runningLeave.FlowInstanceId = runningInstance.Id;
            doneLeave.FlowInstanceId = doneInstance.Id;
            await leaveRepository.UpdateAsync(draftLeave);
            await leaveRepository.UpdateAsync(runningLeave);
            await leaveRepository.UpdateAsync(doneLeave);
            updateCount += 3;
        }
        TaktLogger.Information("请假工作流种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 构建请假流程设计 JSON（发起人 → 直属主管 → 人事确认）
    /// </summary>
    private static string BuildProcessContent(long managerUserId, string managerName, long hrUserId, string hrName)
    {
        var hrNode = new TaktFlowTreeNode
        {
            NodeId = NodeHrConfirm,
            NodeName = "人事确认",
            NodeDisplayName = "人事确认",
            NodeType = 4,
            SetType = 1,
            SignType = 1,
            DirectorLevel = 1,
            NodeApproveList =
            [
                new TaktFlowNodeApproveItem { TargetId = hrUserId.ToString(), Name = hrName }
            ]
        };
        var managerNode = new TaktFlowTreeNode
        {
            NodeId = NodeDeptManager,
            NodeName = "直属主管审批",
            NodeDisplayName = "直属主管审批",
            NodeType = 4,
            SetType = 1,
            SignType = 1,
            DirectorLevel = 1,
            ChildNode = hrNode,
            NodeApproveList =
            [
                new TaktFlowNodeApproveItem { TargetId = managerUserId.ToString(), Name = managerName }
            ]
        };
        var root = new TaktFlowTreeNode
        {
            NodeId = NodeStart,
            NodeName = "发起人",
            NodeDisplayName = "发起人",
            NodeType = 1,
            ChildNode = managerNode,
            NodeApproveList = []
        };
        return JsonConvert.SerializeObject(root, JsonSettings);
    }

    /// <summary>
    /// 构建流程实例表单 JSON（与 leave_form 字段一致）
    /// </summary>
    private static string BuildFrmData(
        TaktEmployee employee,
        TaktDept? dept,
        string leaveType,
        DateTime startDate,
        DateTime endDate,
        string reason)
    {
        var payload = new
        {
            employeeId = employee.Id.ToString(),
            employeeName = employee.EmployeeName,
            deptId = dept?.Id.ToString(),
            deptName = dept?.DeptName1,
            leaveType,
            startDate = startDate.ToString("yyyy-MM-dd"),
            endDate = endDate.ToString("yyyy-MM-dd"),
            reason
        };
        return JsonConvert.SerializeObject(payload, JsonSettings);
    }

    /// <summary>
    /// 请假表单 formConfig JSON
    /// </summary>
    private static string BuildLeaveFormConfigJson()
    {
        var rules = new object[]
        {
            new { field = "employeeId", title = "员工ID", type = "input", props = new { disabled = true } },
            new { field = "employeeName", title = "员工姓名", type = "input", props = new { disabled = true } },
            new { field = "leaveType", title = "请假类型", type = "select", props = new { dictType = "sys_leave_type" } },
            new { field = "startDate", title = "开始日期", type = "datePicker", props = new { valueFormat = "YYYY-MM-DD" } },
            new { field = "endDate", title = "结束日期", type = "datePicker", props = new { valueFormat = "YYYY-MM-DD" } },
            new { field = "reason", title = "请假事由", type = "textarea", props = new { rows = 3 } }
        };
        return JsonConvert.SerializeObject(rules, JsonSettings);
    }

    /// <summary>
    /// 关联表字段映射 JSON
    /// </summary>
    private static string BuildRelatedFormFieldJson()
    {
        var root = new
        {
            fields = new object[]
            {
                new { dbColumnName = "employee_id", csharpColumnName = "employeeId", columnDescription = "员工ID", dataType = "bigint", displayType = "input" },
                new { dbColumnName = "employee_name", csharpColumnName = "employeeName", columnDescription = "员工姓名", dataType = "nvarchar", displayType = "input" },
                new { dbColumnName = "leave_type", csharpColumnName = "leaveType", columnDescription = "请假类型", dataType = "nvarchar", displayType = "select", dictTypeCode = "sys_leave_type" },
                new { dbColumnName = "start_date", csharpColumnName = "startDate", columnDescription = "开始日期", dataType = "date", displayType = "date" },
                new { dbColumnName = "end_date", csharpColumnName = "endDate", columnDescription = "结束日期", dataType = "date", displayType = "date" },
                new { dbColumnName = "reason", csharpColumnName = "reason", columnDescription = "请假事由", dataType = "nvarchar", displayType = "textarea" }
            },
            business = new
            {
                businessStatusColumn = "leave_status",
                statusInProgress = 1,
                statusApproved = 2,
                statusRejected = 3,
                statusCancelled = 4,
                submitAllowedBusinessStatuses = new[] { 0, 3 }
            }
        };
        return JsonConvert.SerializeObject(root, JsonSettings);
    }

    /// <summary>
    /// 创建或更新请假流程表单
    /// </summary>
    private static async Task<(TaktFlowForm Form, int InsertCount, int UpdateCount)> UpsertLeaveFormAsync(
        ITaktCompanySeedRepository<TaktFlowForm> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var form = await repository.FirstAsync(f =>
            f.TenantCode == tenantCode && f.CompanyCode == companyCode && f.FormCode == FormCode);
        var formConfig = BuildLeaveFormConfigJson();
        var relatedField = BuildRelatedFormFieldJson();
        if (form == null)
        {
            form = new TaktFlowForm
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                FormCode = FormCode,
                FormName = "请假申请表",
                FormCategory = 1,
                FormType = 1,
                FormConfig = formConfig,
                FormVersion = "v1.0.0",
                IsDatasource = 1,
                RelatedDataBaseName = tenantCode,
                RelatedTableName = "takt_human_resource_attendance_leave",
                RelatedFormField = relatedField,
                SortOrder = 10,
                FormStatus = 1,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            form = await repository.CreateAsync(form);
            return (form, 1, 0);
        }
        form.FormName = "请假申请表";
        form.FormCategory = 1;
        form.FormType = 1;
        form.FormConfig = formConfig;
        form.FormVersion = "v1.0.0";
        form.IsDatasource = 1;
        form.RelatedDataBaseName = tenantCode;
        form.RelatedTableName = "takt_human_resource_attendance_leave";
        form.RelatedFormField = relatedField;
        form.SortOrder = 10;
        form.FormStatus = 1;
        form.PlantCode = plantCode;
        form.CultureCode = cultureCode;
        await repository.UpdateAsync(form);
        return (form, 0, 1);
    }

    /// <summary>
    /// 创建或更新已发布的 Leave 流程方案
    /// </summary>
    private static async Task<(TaktFlowScheme Scheme, int InsertCount, int UpdateCount)> UpsertLeaveSchemeAsync(
        ITaktCompanySeedRepository<TaktFlowScheme> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktFlowForm form,
        string processContent)
    {
        var scheme = await repository.FirstAsync(s =>
            s.TenantCode == tenantCode
            && s.CompanyCode == companyCode
            && s.ProcessKey == ProcessKey
            && s.DefinitionVersion == 1);
        if (scheme == null)
        {
            scheme = new TaktFlowScheme
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                ProcessKey = ProcessKey,
                ProcessName = "请假申请",
                DefinitionVersion = 1,
                ProcessVersion = "v1.0.0",
                IsLatest = 1,
                ProcessCategory = 1,
                ProcessDescription = "员工请假：直属主管审批 → 人事确认",
                ProcessStatus = 1,
                SuspensionState = (int)TaktFlowSuspensionState.Active,
                ProcessContent = processContent,
                DeploymentId = "leave-v1-seed",
                FormId = form.Id,
                FormCode = form.FormCode,
                SortOrder = 10,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            scheme = await repository.CreateAsync(scheme);
            return (scheme, 1, 0);
        }
        scheme.ProcessName = "请假申请";
        scheme.ProcessVersion = "v1.0.0";
        scheme.IsLatest = 1;
        scheme.ProcessCategory = 1;
        scheme.ProcessDescription = "员工请假：直属主管审批 → 人事确认";
        scheme.ProcessStatus = 1;
        scheme.SuspensionState = (int)TaktFlowSuspensionState.Active;
        scheme.ProcessContent = processContent;
        scheme.DeploymentId = "leave-v1-seed";
        scheme.FormId = form.Id;
        scheme.FormCode = form.FormCode;
        scheme.SortOrder = 10;
        scheme.PlantCode = plantCode;
        scheme.CultureCode = cultureCode;
        await repository.UpdateAsync(scheme);
        return (scheme, 0, 1);
    }

    /// <summary>
    /// 创建或更新请假业务单
    /// </summary>
    private static async Task<(TaktLeave Leave, int InsertCount, int UpdateCount)> UpsertLeaveAsync(
        ITaktApprovalSeedRepository<TaktLeave> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktEmployee employee,
        TaktDept? dept,
        TaktUser initiator,
        string leaveType,
        DateTime startDate,
        DateTime endDate,
        string reason,
        int leaveStatus,
        int approvalStatus,
        long? flowInstanceId)
    {
        var leave = await repository.FirstAsync(l =>
            l.TenantCode == tenantCode
            && l.CompanyCode == companyCode
            && l.EmployeeId == employee.Id
            && l.StartDate == startDate.Date);
        if (leave == null)
        {
            leave = new TaktLeave
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                EmployeeId = employee.Id,
                EmployeeName = employee.EmployeeName,
                DeptId = dept?.Id,
                DeptName = dept?.DeptName1,
                LeaveType = leaveType,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                Reason = reason,
                HandlingBy = employee.Id,
                HandlingAt = DateTime.Now,
                LeaveStatus = leaveStatus,
                ApprovalStatus = approvalStatus,
                InitiatorId = initiator.Id,
                InitiatedAt = DateTime.Now,
                FlowInstanceId = flowInstanceId,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            leave = await repository.CreateAsync(leave);
            return (leave, 1, 0);
        }
        leave.EmployeeName = employee.EmployeeName;
        leave.DeptId = dept?.Id;
        leave.DeptName = dept?.DeptName1;
        leave.LeaveType = leaveType;
        leave.EndDate = endDate.Date;
        leave.Reason = reason;
        leave.LeaveStatus = leaveStatus;
        leave.ApprovalStatus = approvalStatus;
        leave.InitiatorId = initiator.Id;
        leave.InitiatedAt ??= DateTime.Now;
        leave.FlowInstanceId = flowInstanceId;
        leave.PlantCode = plantCode;
        leave.CultureCode = cultureCode;
        await repository.UpdateAsync(leave);
        return (leave, 0, 1);
    }

    /// <summary>
    /// 创建或更新流程实例
    /// </summary>
    private static async Task<(TaktFlowInstance Instance, int InsertCount, int UpdateCount)> UpsertInstanceAsync(
        ITaktCompanySeedRepository<TaktFlowInstance> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktFlowScheme scheme,
        string instanceCode,
        string processTitle,
        TaktFlowInstanceStatus status,
        TaktUser starter,
        string frmData,
        string businessKey,
        string? currentActivityId = null,
        string? currentActivityName = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        var instance = await repository.FirstAsync(i =>
            i.TenantCode == tenantCode && i.CompanyCode == companyCode && i.InstanceCode == instanceCode);
        if (instance == null)
        {
            instance = new TaktFlowInstance
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                InstanceCode = instanceCode,
                ProcessDefinitionId = scheme.Id,
                ProcessKey = scheme.ProcessKey,
                ProcessName = scheme.ProcessName,
                DefinitionVersion = scheme.DefinitionVersion,
                ProcessTitle = processTitle,
                InstanceStatus = (int)status,
                CurrentActivityId = currentActivityId,
                CurrentActivityName = currentActivityName,
                StartUserId = starter.Id,
                StartUserName = starter.NickName ?? starter.UserName,
                StartTime = startTime ?? DateTime.Now,
                EndTime = endTime,
                BusinessKey = businessKey,
                BusinessType = BusinessType,
                FrmData = frmData,
                FormId = scheme.FormId,
                FormCode = scheme.FormCode,
                ProcessContentSnapshot = scheme.ProcessContent,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            if (status == TaktFlowInstanceStatus.Completed && endTime.HasValue)
            {
                instance.DurationMs = (long)(endTime.Value - (instance.StartTime ?? DateTime.Now)).TotalMilliseconds;
            }
            instance = await repository.CreateAsync(instance);
            return (instance, 1, 0);
        }
        instance.ProcessDefinitionId = scheme.Id;
        instance.ProcessKey = scheme.ProcessKey;
        instance.ProcessName = scheme.ProcessName;
        instance.DefinitionVersion = scheme.DefinitionVersion;
        instance.ProcessTitle = processTitle;
        instance.InstanceStatus = (int)status;
        instance.CurrentActivityId = currentActivityId;
        instance.CurrentActivityName = currentActivityName;
        instance.StartUserId = starter.Id;
        instance.StartUserName = starter.NickName ?? starter.UserName;
        instance.StartTime = startTime ?? instance.StartTime;
        instance.EndTime = endTime;
        instance.BusinessKey = businessKey;
        instance.BusinessType = BusinessType;
        instance.FrmData = frmData;
        instance.FormId = scheme.FormId;
        instance.FormCode = scheme.FormCode;
        instance.ProcessContentSnapshot = scheme.ProcessContent;
        if (status == TaktFlowInstanceStatus.Completed && endTime.HasValue)
        {
            instance.DurationMs = (long)(endTime.Value - (instance.StartTime ?? DateTime.Now)).TotalMilliseconds;
        }
        else if (status != TaktFlowInstanceStatus.Completed)
        {
            instance.DurationMs = null;
            instance.EndTime = null;
        }
        instance.PlantCode = plantCode;
        instance.CultureCode = cultureCode;
        await repository.UpdateAsync(instance);
        return (instance, 0, 1);
    }

    /// <summary>
    /// 草稿实例：无待办、无流转
    /// </summary>
    private static async Task SyncDraftInstanceAsync(TaktSeedContext seedContext, TaktFlowInstance instance)
    {
        await ClearInstanceRuntimeAsync(seedContext, instance.Id);
    }

    /// <summary>
    /// 审批中实例：发起流转 + admin 待办
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SyncRunningInstanceAsync(
        TaktSeedContext seedContext,
        ITaktCompanySeedRepository<TaktFlowTask> taskRepository,
        ITaktCompanySeedRepository<TaktFlowTransition> transitionRepository,
        TaktFlowInstance instance,
        TaktUser starter,
        TaktUser manager)
    {
        await ClearInstanceRuntimeAsync(seedContext, instance.Id);
        var startTime = instance.StartTime ?? DateTime.Now;
        await transitionRepository.CreateAsync(new TaktFlowTransition
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            FromNodeId = null,
            FromNodeName = null,
            ToNodeId = NodeDeptManager,
            ToNodeName = "直属主管审批",
            TransitionUserId = starter.Id,
            TransitionUserName = starter.NickName ?? starter.UserName,
            TransitionTime = startTime,
            TransitionComment = "发起",
            ActionType = (int)TaktFlowActionType.Start,
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        await taskRepository.CreateAsync(new TaktFlowTask
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            TaskDefinitionKey = NodeDeptManager,
            TaskName = "直属主管审批",
            AssigneeUserId = manager.Id,
            AssigneeUserName = manager.NickName ?? manager.UserName,
            TaskStatus = (int)TaktFlowTaskStatus.Pending,
            SignType = (int)TaktFlowSignType.Any,
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        return (2, 0);
    }

    /// <summary>
    /// 已完成实例：全流程任务与流转
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SyncCompletedInstanceAsync(
        TaktSeedContext seedContext,
        ITaktCompanySeedRepository<TaktFlowTask> taskRepository,
        ITaktCompanySeedRepository<TaktFlowTransition> transitionRepository,
        TaktFlowInstance instance,
        TaktUser starter,
        TaktUser manager,
        TaktUser hrUser)
    {
        await ClearInstanceRuntimeAsync(seedContext, instance.Id);
        var startTime = instance.StartTime ?? DateTime.Now.AddDays(-3);
        var managerDone = startTime.AddHours(2);
        var hrDone = instance.EndTime ?? startTime.AddHours(4);
        await transitionRepository.CreateAsync(new TaktFlowTransition
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            FromNodeId = null,
            FromNodeName = null,
            ToNodeId = NodeDeptManager,
            ToNodeName = "直属主管审批",
            TransitionUserId = starter.Id,
            TransitionUserName = starter.NickName ?? starter.UserName,
            TransitionTime = startTime,
            TransitionComment = "发起",
            ActionType = (int)TaktFlowActionType.Start,
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        await transitionRepository.CreateAsync(new TaktFlowTransition
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            FromNodeId = NodeDeptManager,
            FromNodeName = "直属主管审批",
            ToNodeId = NodeHrConfirm,
            ToNodeName = "人事确认",
            TransitionUserId = manager.Id,
            TransitionUserName = manager.NickName ?? manager.UserName,
            TransitionTime = managerDone,
            TransitionComment = "同意",
            ActionType = (int)TaktFlowActionType.Approve,
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        await transitionRepository.CreateAsync(new TaktFlowTransition
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            FromNodeId = NodeHrConfirm,
            FromNodeName = "人事确认",
            ToNodeId = null,
            ToNodeName = null,
            TransitionUserId = hrUser.Id,
            TransitionUserName = hrUser.NickName ?? hrUser.UserName,
            TransitionTime = hrDone,
            TransitionComment = "同意",
            ActionType = (int)TaktFlowActionType.Approve,
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        await taskRepository.CreateAsync(new TaktFlowTask
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            TaskDefinitionKey = NodeDeptManager,
            TaskName = "直属主管审批",
            AssigneeUserId = manager.Id,
            AssigneeUserName = manager.NickName ?? manager.UserName,
            TaskStatus = (int)TaktFlowTaskStatus.Completed,
            SignType = (int)TaktFlowSignType.Any,
            CompletedAt = managerDone,
            Comment = "同意",
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        await taskRepository.CreateAsync(new TaktFlowTask
        {
            TenantCode = instance.TenantCode,
            CompanyCode = instance.CompanyCode,
            InstanceId = instance.Id,
            TaskDefinitionKey = NodeHrConfirm,
            TaskName = "人事确认",
            AssigneeUserId = hrUser.Id,
            AssigneeUserName = hrUser.NickName ?? hrUser.UserName,
            TaskStatus = (int)TaktFlowTaskStatus.Completed,
            SignType = (int)TaktFlowSignType.Any,
            CompletedAt = hrDone,
            Comment = "同意",
            PlantCode = instance.PlantCode,
            CultureCode = instance.CultureCode
        });
        return (5, 0);
    }

    /// <summary>
    /// 清空实例下任务与流转（幂等重建）
    /// </summary>
    private static async Task ClearInstanceRuntimeAsync(TaktSeedContext seedContext, long instanceId)
    {
        var now = DateTime.Now;
        await seedContext.Db.Updateable<TaktFlowTask>()
            .SetColumns(t => new TaktFlowTask
            {
                IsDeleted = 1,
                UpdatedAt = now,
                DeletedAt = now,
            })
            .Where(t => t.InstanceId == instanceId && t.IsDeleted == 0)
            .ExecuteCommandAsync();
        await seedContext.Db.Updateable<TaktFlowTransition>()
            .SetColumns(t => new TaktFlowTransition
            {
                IsDeleted = 1,
                UpdatedAt = now,
                DeletedAt = now,
            })
            .Where(t => t.InstanceId == instanceId && t.IsDeleted == 0)
            .ExecuteCommandAsync();
    }
}
