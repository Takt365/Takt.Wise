// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData.Workflow
// 文件名称：TaktExpenseWorkflowSeedData.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单工作流种子（expense_form 表单、Expense 流程方案，关联 takt_accounting_financial_expense）
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData.Workflow;

/// <summary>
/// 费用单工作流种子：流程表单与已发布方案
/// </summary>
public class TaktExpenseWorkflowSeedData : ITaktSeedDataCoordinator
{
    private const string FormCode = "expense_form";
    private const string ProcessKey = "Expense";
    private const string NodeStart = "expense_start";
    private const string NodeFinance = "expense_finance";
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    /// <summary>
    /// 执行顺序
    /// </summary>
    public int Order => 69;

    /// <summary>
    /// 初始化费用单工作流种子
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
        if (adminUser == null)
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
                adminUser.NickName ?? adminUser.UserName);
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

    private static string BuildProcessContent(long financeUserId, string financeUserName)
    {
        var financeNode = new TaktFlowTreeNode
        {
            NodeId = NodeFinance,
            NodeName = "财务审批",
            NodeDisplayName = "财务审批",
            NodeType = 4,
            SetType = 1,
            SignType = 1,
            DirectorLevel = 1,
            NodeApproveList = [new TaktFlowNodeApproveItem { TargetId = financeUserId.ToString(), Name = financeUserName }]
        };
        var root = new TaktFlowTreeNode
        {
            NodeId = NodeStart,
            NodeName = "发起人",
            NodeDisplayName = "发起人",
            NodeType = 1,
            ChildNode = financeNode,
            NodeApproveList = []
        };
        return JsonConvert.SerializeObject(root, JsonSettings);
    }

    private static string BuildFormConfigJson()
    {
        var rules = new object[]
        {
            new { field = "expenseCode", title = "费用单编码", type = "input" },
            new { field = "expenseTitle", title = "费用标题", type = "input" },
            new { field = "expenseType", title = "费用类型", type = "select", props = new { dictType = "accounting_financial_expense_type" } },
            new { field = "expenseAmount", title = "费用金额", type = "inputNumber" },
            new { field = "expenseDate", title = "费用发生日期", type = "date" },
            new { field = "applicationReason", title = "申请原因", type = "textarea", props = new { rows = 4 } }
        };
        return JsonConvert.SerializeObject(rules, JsonSettings);
    }

    private static string BuildRelatedFormFieldJson()
    {
        var root = new
        {
            fields = new object[]
            {
                new { dbColumnName = "expense_code", csharpColumnName = "expenseCode", columnDescription = "费用单编码", dataType = "varchar", displayType = "input" },
                new { dbColumnName = "expense_title", csharpColumnName = "expenseTitle", columnDescription = "费用标题", dataType = "nvarchar", displayType = "input" },
                new { dbColumnName = "expense_type", csharpColumnName = "expenseType", columnDescription = "费用类型", dataType = "int", displayType = "select", dictTypeCode = "accounting_financial_expense_type" },
                new { dbColumnName = "expense_amount", csharpColumnName = "expenseAmount", columnDescription = "费用金额", dataType = "decimal", displayType = "inputNumber" },
                new { dbColumnName = "expense_date", csharpColumnName = "expenseDate", columnDescription = "费用发生日期", dataType = "datetime", displayType = "date" },
                new { dbColumnName = "application_reason", csharpColumnName = "applicationReason", columnDescription = "申请原因", dataType = "nvarchar", displayType = "textarea" }
            },
            business = new
            {
                businessStatusColumn = "expense_status",
                statusInProgress = 1,
                statusApproved = 2,
                statusRejected = 3,
                statusCancelled = 4,
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
                FormName = "费用审批表",
                FormCategory = 1,
                FormType = 1,
                FormConfig = formConfig,
                FormVersion = "v1.0.0",
                IsDatasource = 1,
                RelatedDataBaseName = tenantCode,
                RelatedTableName = "takt_accounting_financial_expense",
                RelatedFormField = relatedField,
                SortOrder = 13,
                FormStatus = 1,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            form = await repository.CreateAsync(form);
            return (form, 1, 0);
        }
        form.FormName = "费用审批表";
        form.FormConfig = formConfig;
        form.RelatedDataBaseName = tenantCode;
        form.RelatedTableName = "takt_accounting_financial_expense";
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
                ProcessName = "费用单审批",
                DefinitionVersion = 1,
                ProcessVersion = "v1.0.0",
                IsLatest = 1,
                ProcessCategory = 1,
                ProcessDescription = "费用单提交后由财务审批",
                ProcessStatus = 1,
                SuspensionState = (int)TaktFlowSuspensionState.Active,
                ProcessContent = processContent,
                DeploymentId = "expense-v1-seed",
                FormId = form.Id,
                FormCode = form.FormCode,
                SortOrder = 13,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            scheme = await repository.CreateAsync(scheme);
            return (scheme, 1, 0);
        }
        scheme.ProcessName = "费用单审批";
        scheme.ProcessContent = processContent;
        scheme.ProcessStatus = 1;
        scheme.FormId = form.Id;
        scheme.FormCode = form.FormCode;
        await repository.UpdateAsync(scheme);
        return (scheme, 0, 1);
    }
}
