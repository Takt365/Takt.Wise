// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData.Workflow
// 文件名称：TaktOvertimeWorkflowSeedData.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：加班工作流种子（overtime_form 表单、Overtime 流程方案）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Takt.Application.Services.Workflow.FlowEngine;
using Takt.Domain.Entities.Accounting.Financial;
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
/// 加班工作流种子：流程表单与已发布方案
/// </summary>
public class TaktOvertimeWorkflowSeedData : ITaktSeedDataCoordinator
{
    private const string FormCode = "overtime_form";
    private const string ProcessKey = "Overtime";
    private const string NodeStart = "overtime_start";
    private const string NodeDeptManager = "overtime_dept_manager";
    private const string NodeHrConfirm = "overtime_hr_confirm";
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    /// <summary>
    /// 执行顺序（在请假工作流之后）
    /// </summary>
    public int Order => 67;

    /// <summary>
    /// 初始化加班工作流种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        if (string.IsNullOrEmpty(tenantCode))
        {
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var formRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowForm>>();
        var schemeRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktFlowScheme>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        var adminUser = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == "admin");
        var guestUser = await userRepository.FirstAsync(u => u.TenantCode == tenantCode && u.UserName == "guest");
        if (adminUser == null || guestUser == null)
        {
            return (0, 0);
        }
        int insertCount = 0;
        int updateCount = 0;
        foreach (var company in orderedCompanies)
        {
            var (form, fi, fu) = await UpsertFormAsync(
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
            var (_, si, su) = await UpsertSchemeAsync(
                schemeRepository,
                tenantCode,
                company.CompanyCode,
                database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                form,
                processContent);
            insertCount += si;
            updateCount += su;
        }
        return (insertCount, updateCount);
    }

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
            NodeApproveList = [new TaktFlowNodeApproveItem { TargetId = hrUserId.ToString(), Name = hrName }]
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
            NodeApproveList = [new TaktFlowNodeApproveItem { TargetId = managerUserId.ToString(), Name = managerName }]
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

    private static string BuildFormConfigJson()
    {
        var rules = new object[]
        {
            new { field = "deptId", title = "部门ID", type = "input", props = new { disabled = true } },
            new { field = "deptName", title = "部门名称", type = "input", props = new { disabled = true } },
            new { field = "overtimeDate", title = "加班日期", type = "datePicker", props = new { valueFormat = "YYYY-MM-DD" } },
            new { field = "plannedStartTime", title = "计划开始", type = "input" },
            new { field = "plannedEndTime", title = "计划结束", type = "input" },
            new { field = "overtimeType", title = "加班类型", type = "select", props = new { dictType = "humanresource_attendance_overtime_type" } },
            new { field = "totalEmployees", title = "总人数", type = "input" },
            new { field = "totalPlannedHours", title = "计划总小时", type = "input" },
            new { field = "reason", title = "加班原因", type = "textarea", props = new { rows = 3 } }
        };
        return JsonConvert.SerializeObject(rules, JsonSettings);
    }

    private static string BuildRelatedFormFieldJson()
    {
        var root = new
        {
            fields = new object[]
            {
                new { dbColumnName = "dept_id", csharpColumnName = "deptId", columnDescription = "部门ID", dataType = "bigint", displayType = "input" },
                new { dbColumnName = "dept_name", csharpColumnName = "deptName", columnDescription = "部门名称", dataType = "nvarchar", displayType = "input" },
                new { dbColumnName = "overtime_date", csharpColumnName = "overtimeDate", columnDescription = "加班日期", dataType = "date", displayType = "date" },
                new { dbColumnName = "planned_start_time", csharpColumnName = "plannedStartTime", columnDescription = "计划开始", dataType = "datetime", displayType = "input" },
                new { dbColumnName = "planned_end_time", csharpColumnName = "plannedEndTime", columnDescription = "计划结束", dataType = "datetime", displayType = "input" },
                new { dbColumnName = "overtime_type", csharpColumnName = "overtimeType", columnDescription = "加班类型", dataType = "int", displayType = "select", dictTypeCode = "humanresource_attendance_overtime_type" },
                new { dbColumnName = "total_employees", csharpColumnName = "totalEmployees", columnDescription = "总人数", dataType = "int", displayType = "input" },
                new { dbColumnName = "total_planned_hours", csharpColumnName = "totalPlannedHours", columnDescription = "计划总小时", dataType = "decimal", displayType = "input" },
                new { dbColumnName = "reason", csharpColumnName = "reason", columnDescription = "加班原因", dataType = "nvarchar", displayType = "textarea" }
            },
            business = new
            {
                businessStatusColumn = "overtime_status",
                statusInProgress = 1,
                statusApproved = 2,
                statusRejected = 3,
                statusCancelled = 0,
                submitAllowedBusinessStatuses = new[] { 0, 3 }
            }
        };
        return JsonConvert.SerializeObject(root, JsonSettings);
    }

    private static async Task<(TaktFlowForm Form, int InsertCount, int UpdateCount)> UpsertFormAsync(
        ITaktCompanySeedRepository<TaktFlowForm> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var form = await repository.FirstAsync(f =>
            f.TenantCode == tenantCode && f.CompanyCode == companyCode && f.FormCode == FormCode);
        var formConfig = BuildFormConfigJson();
        var relatedField = BuildRelatedFormFieldJson();
        if (form == null)
        {
            form = new TaktFlowForm
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                FormCode = FormCode,
                FormName = "加班申请表",
                FormCategory = 1,
                FormType = 1,
                FormConfig = formConfig,
                FormVersion = "v1.0.0",
                IsDatasource = 1,
                RelatedDataBaseName = tenantCode,
                RelatedTableName = "takt_human_resource_attendance_overtime",
                RelatedFormField = relatedField,
                SortOrder = 11,
                FormStatus = 1,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            form = await repository.CreateAsync(form);
            return (form, 1, 0);
        }
        form.FormName = "加班申请表";
        form.FormConfig = formConfig;
        form.RelatedDataBaseName = tenantCode;
        form.RelatedTableName = "takt_human_resource_attendance_overtime";
        form.RelatedFormField = relatedField;
        form.IsDatasource = 1;
        form.FormStatus = 1;
        form.PlantCode = plantCode;
        form.CultureCode = cultureCode;
        await repository.UpdateAsync(form);
        return (form, 0, 1);
    }

    private static async Task<(TaktFlowScheme Scheme, int InsertCount, int UpdateCount)> UpsertSchemeAsync(
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
                ProcessName = "加班申请",
                DefinitionVersion = 1,
                ProcessVersion = "v1.0.0",
                IsLatest = 1,
                ProcessCategory = 1,
                ProcessDescription = "部门加班：直属主管审批 → 人事确认",
                ProcessStatus = 1,
                SuspensionState = (int)TaktFlowSuspensionState.Active,
                ProcessContent = processContent,
                DeploymentId = "overtime-v1-seed",
                FormId = form.Id,
                FormCode = form.FormCode,
                SortOrder = 11,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            scheme = await repository.CreateAsync(scheme);
            return (scheme, 1, 0);
        }
        scheme.ProcessName = "加班申请";
        scheme.ProcessContent = processContent;
        scheme.ProcessStatus = 1;
        scheme.FormId = form.Id;
        scheme.FormCode = form.FormCode;
        await repository.UpdateAsync(scheme);
        return (scheme, 0, 1);
    }
}
