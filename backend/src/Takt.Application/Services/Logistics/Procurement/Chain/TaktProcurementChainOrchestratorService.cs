// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement.Chain
// 文件名称：TaktProcurementChainOrchestratorService.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购全链路编排（三套方案、会签 BusinessType 路由）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Accounting.Financial;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Application.Services.Workflow.FlowEngine.Business;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Procurement.Chain;

/// <summary>
/// 采购全链路编排服务
/// </summary>
public class TaktProcurementChainOrchestratorService : TaktServiceBase, ITaktProcurementChainOrchestrator
{
    private readonly ITaktCompanyRepository<TaktPurchaseInquiry> _purchaseInquiryRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInquiryItem> _purchaseInquiryItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecord> _purchaseInfoRecordRepository;
    private readonly ITaktApprovalRepository<TaktCountersign> _countersignRepository;
    private readonly ITaktCompanyRepository<TaktCountersignDetail> _countersignDetailRepository;
    private readonly ITaktApprovalRepository<TaktPurchaseRequest> _purchaseRequestRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseRequestItem> _purchaseRequestItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrder> _purchaseOrderRepository;
    private readonly ITaktApprovalRepository<TaktExpense> _expenseRepository;
    private readonly ITaktCompanyRepository<TaktExpenseDetail> _expenseDetailRepository;
    private readonly ITaktPurchaseInfoRecordService _purchaseInfoRecordService;
    private readonly ITaktCountersignService _countersignService;
    private readonly ITaktPurchaseRequestService _purchaseRequestService;
    private readonly ITaktPurchaseOrderService _purchaseOrderService;
    private readonly ITaktExpenseService _expenseService;
    private readonly TaktApprovalFlowSubmitService _approvalFlowSubmitService;

    /// <summary>
    /// 初始化采购全链路编排服务
    /// </summary>
    public TaktProcurementChainOrchestratorService(
        ITaktCompanyRepository<TaktPurchaseInquiry> purchaseInquiryRepository,
        ITaktCompanyRepository<TaktPurchaseInquiryItem> purchaseInquiryItemRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecord> purchaseInfoRecordRepository,
        ITaktApprovalRepository<TaktCountersign> countersignRepository,
        ITaktCompanyRepository<TaktCountersignDetail> countersignDetailRepository,
        ITaktApprovalRepository<TaktPurchaseRequest> purchaseRequestRepository,
        ITaktCompanyRepository<TaktPurchaseRequestItem> purchaseRequestItemRepository,
        ITaktCompanyRepository<TaktPurchaseOrder> purchaseOrderRepository,
        ITaktApprovalRepository<TaktExpense> expenseRepository,
        ITaktCompanyRepository<TaktExpenseDetail> expenseDetailRepository,
        ITaktPurchaseInfoRecordService purchaseInfoRecordService,
        ITaktCountersignService countersignService,
        ITaktPurchaseRequestService purchaseRequestService,
        ITaktPurchaseOrderService purchaseOrderService,
        ITaktExpenseService expenseService,
        TaktApprovalFlowSubmitService approvalFlowSubmitService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInquiryRepository = purchaseInquiryRepository;
        _purchaseInquiryItemRepository = purchaseInquiryItemRepository;
        _purchaseInfoRecordRepository = purchaseInfoRecordRepository;
        _countersignRepository = countersignRepository;
        _countersignDetailRepository = countersignDetailRepository;
        _purchaseRequestRepository = purchaseRequestRepository;
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
        _expenseRepository = expenseRepository;
        _expenseDetailRepository = expenseDetailRepository;
        _purchaseInfoRecordService = purchaseInfoRecordService;
        _countersignService = countersignService;
        _purchaseRequestService = purchaseRequestService;
        _purchaseOrderService = purchaseOrderService;
        _expenseService = expenseService;
        _approvalFlowSubmitService = approvalFlowSubmitService;
    }

    /// <summary>
    /// 采购询价提交会签审批（方案一/二入口）
    /// </summary>
    /// <param name="inquiryId">询价主键</param>
    /// <returns>异步任务</returns>
    public async Task SubmitPurchaseInquiryForCountersignAsync(long inquiryId)
    {
        EnsureThreeLayerContext();
        var inquiry = await LoadPurchaseInquiryAsync(inquiryId);
        var items = await LoadPurchaseInquiryItemsAsync(inquiry.Id);
        if (items.Count == 0)
        {
            throw new TaktBusinessException("采购询价无明细，无法提交会签");
        }
        var businessKey = TaktProcurementHelper.BuildBusinessKey(
            TaktProcurementConstants.BusinessTypeInquiry,
            inquiry.Id);
        var countersignId = await EnsureCountersignForInquirySubmitAsync(inquiry, items, businessKey);
        await SubmitCountersignAsync(countersignId);
    }

    /// <summary>
    /// 采购申请提交会签审批
    /// </summary>
    /// <param name="requestId">采购申请主键</param>
    /// <returns>异步任务</returns>
    public async Task SubmitPurchaseRequestForCountersignAsync(long requestId)
    {
        EnsureThreeLayerContext();
        var request = await LoadPurchaseRequestAsync(requestId);
        if (!request.PurchaseInquiryId.HasValue)
        {
            throw new TaktBusinessException("非采购链路生成的申请，请走常规流程");
        }
        var items = await LoadPurchaseRequestItemsAsync(request.Id);
        if (items.Count == 0)
        {
            throw new TaktBusinessException("采购申请无明细，无法提交会签");
        }
        var businessKey = TaktProcurementHelper.BuildBusinessKey(
            TaktProcurementConstants.BusinessTypePurchaseRequest,
            request.Id);
        var countersignId = await EnsureCountersignForPurchaseRequestSubmitAsync(request, items, businessKey);
        await SubmitCountersignAsync(countersignId);
        await PatchPurchaseRequestCountersignAsync(request.Id, countersignId);
    }

    /// <summary>
    /// 方案一：PR 会签通过后人工 PO 决策
    /// </summary>
    /// <param name="requestId">采购申请主键</param>
    /// <param name="generatePo">true=生成 PO 后报销；false=暂不生成 PO 直接报销</param>
    /// <returns>异步任务</returns>
    public async Task ApplyPurchaseRequestPoDecisionAsync(long requestId, bool generatePo)
    {
        EnsureThreeLayerContext();
        var request = await LoadPurchaseRequestAsync(requestId);
        if (request.ChainScheme != TaktProcurementConstants.ChainSchemeWithExpense)
        {
            throw new TaktBusinessException("当前采购申请不属于方案一，无需 PO 决策");
        }
        if (request.PoDecision.HasValue)
        {
            throw new TaktBusinessException("PO 决策已处理，请勿重复操作");
        }
        if (!request.CountersignId.HasValue)
        {
            throw new TaktBusinessException("采购申请尚未完成会签审批");
        }
        var prCountersign = await _countersignRepository.GetByIdAsync(request.CountersignId.Value);
        if (prCountersign == null
            || prCountersign.ApprovalStatus != (int)TaktApprovalStatus.Approved)
        {
            throw new TaktBusinessException("采购申请会签尚未通过，无法决策 PO");
        }
        var inquiry = await LoadPurchaseInquiryAsync(request.PurchaseInquiryId!.Value);
        request.PoDecision = generatePo
            ? TaktProcurementConstants.PoDecisionGeneratePo
            : TaktProcurementConstants.PoDecisionSkipPo;
        await _purchaseRequestRepository.UpdateAsync(request);
        TaktPurchaseOrder? order = null;
        if (generatePo)
        {
            var orderId = await EnsurePurchaseOrderFromRequestAsync(request, inquiry);
            order = await _purchaseOrderRepository.GetByIdAsync(orderId);
        }
        var expenseId = order != null
            ? await EnsureExpenseFromOrderAsync(order, request, inquiry)
            : await EnsureExpenseFromRequestAsync(request, inquiry, prCountersign);
        await EnsureExpenseCountersignAndSubmitAsync(expenseId, inquiry);
    }

    /// <summary>
    /// 会签审批通过后按 BusinessType 分发下游
    /// </summary>
    /// <param name="context">审批回写上下文</param>
    /// <returns>异步任务</returns>
    public async Task OnCountersignApprovedAsync(TaktApprovalFlowCompletedContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var countersign = await LoadCountersignFromContextAsync(context);
        var businessType = countersign.BusinessType?.Trim().ToLowerInvariant()
            ?? string.Empty;
        switch (businessType)
        {
            case TaktProcurementConstants.BusinessTypeInquiry:
                await OnInquiryCountersignApprovedAsync(countersign);
                break;
            case TaktProcurementConstants.BusinessTypePurchaseRequest:
                await OnPurchaseRequestCountersignApprovedAsync(countersign);
                break;
            case TaktProcurementConstants.BusinessTypeStandalone:
                await OnStandaloneCountersignApprovedAsync(countersign);
                break;
            case TaktProcurementConstants.BusinessTypeExpense:
                break;
            default:
                throw new TaktBusinessException($"未识别的会签业务类型：{businessType}");
        }
    }

    /// <summary>
    /// 询价会签通过：生成价格与采购申请草稿
    /// </summary>
    private async Task OnInquiryCountersignApprovedAsync(TaktCountersign countersign)
    {
        if (!TaktProcurementHelper.TryParseBusinessKey(
                countersign.BusinessKey,
                out _,
                out var inquiryId))
        {
            throw new TaktBusinessException("询价会签缺少有效 BusinessKey");
        }
        var inquiry = await LoadPurchaseInquiryAsync(inquiryId);
        var items = await LoadPurchaseInquiryItemsAsync(inquiry.Id);
        var supplierCode = ResolveSupplierCode(inquiry, items);
        await EnsurePurchaseInfoRecordFromInquiryAsync(inquiry, items, supplierCode);
        await EnsurePurchaseRequestFromInquiryAsync(inquiry, items, countersign);
        inquiry.ConvertedStatus = 2;
        inquiry.ConvertedQuantity = inquiry.TotalQuantity;
        inquiry.ConvertedAmount = inquiry.TotalAmount;
        await _purchaseInquiryRepository.UpdateAsync(inquiry);
    }

    /// <summary>
    /// PR 会签通过：方案二自动 PO；方案一等待 PO 决策
    /// </summary>
    private async Task OnPurchaseRequestCountersignApprovedAsync(TaktCountersign countersign)
    {
        if (!TaktProcurementHelper.TryParseBusinessKey(
                countersign.BusinessKey,
                out _,
                out var requestId))
        {
            throw new TaktBusinessException("采购申请会签缺少有效 BusinessKey");
        }
        var request = await LoadPurchaseRequestAsync(requestId);
        request.ApprovalStatus = (int)TaktApprovalStatus.Approved;
        await _purchaseRequestRepository.UpdateAsync(request);
        if (request.ChainScheme == TaktProcurementConstants.ChainSchemePoOnly)
        {
            var inquiry = await LoadPurchaseInquiryAsync(request.PurchaseInquiryId!.Value);
            await EnsurePurchaseOrderFromRequestAsync(request, inquiry);
            return;
        }
        request.PoDecision = null;
        await _purchaseRequestRepository.UpdateAsync(request);
    }

    /// <summary>
    /// 独立会签通过（方案三）：生成费用并提交报销会签
    /// </summary>
    private async Task OnStandaloneCountersignApprovedAsync(TaktCountersign countersign)
    {
        var details = await LoadCountersignDetailsAsync(countersign.Id);
        if (details.Count == 0)
        {
            throw new TaktBusinessException("会签单无明细，无法生成费用");
        }
        var expenseId = await EnsureExpenseFromStandaloneCountersignAsync(countersign, details);
        await EnsureExpenseCountersignAndSubmitAsync(expenseId, null);
    }

    /// <summary>
    /// 提交会签审批
    /// </summary>
    private Task SubmitCountersignAsync(long countersignId)
        => _approvalFlowSubmitService.SubmitForApprovalByTableAsync(
            TaktProcurementConstants.CountersignTableName,
            countersignId,
            TaktProcurementConstants.ProcessKeyCountersign);

    /// <summary>
    /// 费用生成后创建报销会签并提交
    /// </summary>
    private async Task EnsureExpenseCountersignAndSubmitAsync(long expenseId, TaktPurchaseInquiry? inquiry)
    {
        var expense = await _expenseRepository.GetByIdAsync(expenseId)
            ?? throw new TaktBusinessException("费用单不存在");
        var details = await LoadExpenseDetailsAsync(expenseId);
        var businessKey = TaktProcurementHelper.BuildBusinessKey(
            TaktProcurementConstants.BusinessTypeExpense,
            expenseId);
        var existing = await FindCountersignByBusinessKeyAsync(businessKey);
        var countersignId = existing?.Id
            ?? await CreateExpenseCountersignAsync(expense, details, businessKey, inquiry);
        await SubmitCountersignAsync(countersignId);
    }

    /// <summary>
    /// 按 BusinessKey 查找会签
    /// </summary>
    private async Task<TaktCountersign?> FindCountersignByBusinessKeyAsync(string businessKey)
    {
        return await _countersignRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.BusinessKey == businessKey
            && x.IsDeleted == 0);
    }

    /// <summary>
    /// 加载询价主表
    /// </summary>
    private async Task<TaktPurchaseInquiry> LoadPurchaseInquiryAsync(long inquiryId)
    {
        EnsureThreeLayerContext();
        var inquiry = await _purchaseInquiryRepository.GetByIdAsync(inquiryId);
        if (inquiry == null
            || inquiry.TenantCode != CurrentTenantCode
            || inquiry.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购询价不存在");
        }
        return inquiry;
    }

    /// <summary>
    /// 加载采购申请主表
    /// </summary>
    private async Task<TaktPurchaseRequest> LoadPurchaseRequestAsync(long requestId)
    {
        EnsureThreeLayerContext();
        var request = await _purchaseRequestRepository.GetByIdAsync(requestId);
        if (request == null
            || request.TenantCode != CurrentTenantCode
            || request.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购申请不存在");
        }
        return request;
    }

    /// <summary>
    /// 从审批上下文加载会签
    /// </summary>
    private async Task<TaktCountersign> LoadCountersignFromContextAsync(TaktApprovalFlowCompletedContext context)
    {
        var countersign = await _countersignRepository.GetByIdAsync(context.EntityId);
        if (countersign == null
            || countersign.TenantCode != context.TenantCode
            || countersign.CompanyCode != context.CompanyCode)
        {
            throw new TaktBusinessException("会签单不存在");
        }
        return countersign;
    }

    /// <summary>
    /// 加载询价明细
    /// </summary>
    private async Task<List<TaktPurchaseInquiryItem>> LoadPurchaseInquiryItemsAsync(long inquiryId)
    {
        var list = await _purchaseInquiryItemRepository.GetListAsync(x => x.PurchaseInquiryId == inquiryId);
        return list.OrderBy(x => x.LineNumber).ToList();
    }

    /// <summary>
    /// 加载采购申请明细
    /// </summary>
    private async Task<List<TaktPurchaseRequestItem>> LoadPurchaseRequestItemsAsync(long requestId)
    {
        var list = await _purchaseRequestItemRepository.GetListAsync(x => x.PurchaseRequestId == requestId);
        return list.OrderBy(x => x.LineNumber).ToList();
    }

    /// <summary>
    /// 加载会签明细
    /// </summary>
    private async Task<List<TaktCountersignDetail>> LoadCountersignDetailsAsync(long countersignId)
    {
        var list = await _countersignDetailRepository.GetListAsync(x => x.CountersignId == countersignId);
        return list.OrderBy(x => x.LineNumber).ToList();
    }

    /// <summary>
    /// 加载费用明细
    /// </summary>
    private async Task<List<TaktExpenseDetail>> LoadExpenseDetailsAsync(long expenseId)
    {
        var list = await _expenseDetailRepository.GetListAsync(x => x.ExpenseId == expenseId);
        return list.OrderBy(x => x.LineNumber).ToList();
    }

    /// <summary>
    /// 解析供应商编码（询价一单一供应商，仅取抬头 SupplierCode）
    /// </summary>
    private static string ResolveSupplierCode(TaktPurchaseInquiry inquiry, IReadOnlyList<TaktPurchaseInquiryItem> items)
    {
        _ = items;
        if (!string.IsNullOrWhiteSpace(inquiry.SupplierCode))
        {
            return inquiry.SupplierCode;
        }
        throw new TaktBusinessException("询价单缺少供应商编码，无法生成采购价格");
    }

    /// <summary>
    /// 幂等创建询价提交用会签
    /// </summary>
    private async Task<long> EnsureCountersignForInquirySubmitAsync(
        TaktPurchaseInquiry inquiry,
        IReadOnlyList<TaktPurchaseInquiryItem> items,
        string businessKey)
    {
        var existing = await FindCountersignByBusinessKeyAsync(businessKey);
        if (existing != null)
        {
            return existing.Id;
        }
        return await CreateInquiryCountersignAsync(inquiry, items, businessKey);
    }

    /// <summary>
    /// 幂等创建 PR 提交用会签
    /// </summary>
    private async Task<long> EnsureCountersignForPurchaseRequestSubmitAsync(
        TaktPurchaseRequest request,
        IReadOnlyList<TaktPurchaseRequestItem> items,
        string businessKey)
    {
        var existing = await FindCountersignByBusinessKeyAsync(businessKey);
        if (existing != null)
        {
            return existing.Id;
        }
        return await CreatePurchaseRequestCountersignAsync(request, items, businessKey);
    }

    /// <summary>
    /// 创建询价会签单
    /// </summary>
    private async Task<long> CreateInquiryCountersignAsync(
        TaktPurchaseInquiry inquiry,
        IReadOnlyList<TaktPurchaseInquiryItem> items,
        string businessKey)
    {
        if (!inquiry.InquiryEmployeeId.HasValue || inquiry.InquiryEmployeeId.Value <= 0)
        {
            throw new TaktBusinessException("询价单缺少询价人员工，无法提交会签");
        }
        var countersignCode = TaktProcurementHelper.DeriveCountersignCode(
            inquiry.PurchaseInquiryCode,
            inquiry.Id);
        var createDto = new TaktCountersignCreateDto
        {
            CountersignCode = countersignCode,
            ApplicantBy = inquiry.InquiryEmployeeId.Value,
            ApplicationAmount = inquiry.TotalAmount,
            BudgetAmount = inquiry.TotalAmount,
            CountersignTitle = $"采购询价审批-{inquiry.PurchaseInquiryCode}",
            ApplicationReason = inquiry.InquiryReason,
            CountersignStatus = 0,
            CountersignDetails = items.Select(item => new TaktCountersignDetailCreateDto
            {
                LineNumber = item.LineNumber,
                AllocationCategory = item.AllocationCategory,
                ItemName = item.MaterialDescription,
                ItemDescription = $"{TaktProcurementConstants.CountersignMaterialCodePrefix}{item.MaterialCode}",
                ItemQuantity = item.InquiryQuantity,
                ItemAmount = item.TaxIncludedAmount
            }).ToList()
        };
        var created = await _countersignService.CreateCountersignAsync(createDto);
        await PatchCountersignChainFieldsAsync(
            created.CountersignId,
            TaktProcurementConstants.BusinessTypeInquiry,
            businessKey,
            TaktProcurementConstants.StepNoInquiry,
            inquiry.Id,
            inquiry.PurchaseInquiryCode);
        return created.CountersignId;
    }

    /// <summary>
    /// 创建 PR 会签单
    /// </summary>
    private async Task<long> CreatePurchaseRequestCountersignAsync(
        TaktPurchaseRequest request,
        IReadOnlyList<TaktPurchaseRequestItem> items,
        string businessKey)
    {
        if (!request.RequestEmployeeId.HasValue || request.RequestEmployeeId.Value <= 0)
        {
            throw new TaktBusinessException("采购申请缺少申请人员工，无法提交会签");
        }
        var countersignCode = TaktProcurementHelper.DeriveCountersignCode(
            request.PurchaseRequestCode,
            request.Id);
        var createDto = new TaktCountersignCreateDto
        {
            CountersignCode = countersignCode,
            ApplicantBy = request.RequestEmployeeId.Value,
            ApplicationAmount = request.TotalAmount,
            BudgetAmount = request.TotalAmount,
            CountersignTitle = $"采购申请审批-{request.PurchaseRequestCode}",
            ApplicationReason = request.RequestReason,
            CountersignStatus = 0,
            CountersignDetails = items.Select(item => new TaktCountersignDetailCreateDto
            {
                LineNumber = item.LineNumber,
                AllocationCategory = item.AllocationCategory,
                ItemName = item.MaterialDescription,
                ItemDescription = $"{TaktProcurementConstants.CountersignMaterialCodePrefix}{item.MaterialCode}",
                ItemQuantity = item.RequestQuantity,
                ItemAmount = item.TaxIncludedAmount
            }).ToList()
        };
        var created = await _countersignService.CreateCountersignAsync(createDto);
        await PatchCountersignChainFieldsAsync(
            created.CountersignId,
            TaktProcurementConstants.BusinessTypePurchaseRequest,
            businessKey,
            TaktProcurementConstants.StepNoPurchaseRequest,
            request.PurchaseInquiryId,
            request.PurchaseInquiryCode);
        return created.CountersignId;
    }

    /// <summary>
    /// 创建费用报销会签单
    /// </summary>
    private async Task<long> CreateExpenseCountersignAsync(
        TaktExpense expense,
        IReadOnlyList<TaktExpenseDetail> details,
        string businessKey,
        TaktPurchaseInquiry? inquiry)
    {
        var countersignCode = TaktProcurementHelper.DeriveCountersignCode(
            expense.ExpenseCode,
            expense.Id);
        var createDto = new TaktCountersignCreateDto
        {
            CountersignCode = countersignCode,
            ApplicantBy = expense.ApplicantBy,
            ApplicantName = expense.ApplicantName,
            ApplicationAmount = expense.ExpenseAmount,
            BudgetAmount = expense.ExpenseAmount,
            ApplicationDeptId = expense.ApplicationDeptId,
            ApplicationDeptName = expense.ApplicationDeptName,
            CostBearerDeptId = expense.CostBearerDeptId,
            CostBearerDeptName = expense.CostBearerDeptName,
            FileName = expense.FileName,
            AccessUrl = expense.AccessUrl,
            CountersignTitle = $"费用报销审批-{expense.ExpenseCode}",
            ApplicationReason = expense.ApplicationReason,
            CountersignStatus = 0,
            CountersignDetails = details.Select(detail => new TaktCountersignDetailCreateDto
            {
                LineNumber = detail.LineNumber,
                AllocationCategory = detail.AllocationCategory,
                ItemName = detail.ItemName,
                ItemDescription = detail.ItemDescription,
                ItemQuantity = detail.ItemQuantity,
                ItemAmount = detail.ItemAmount
            }).ToList()
        };
        var created = await _countersignService.CreateCountersignAsync(createDto);
        await PatchCountersignChainFieldsAsync(
            created.CountersignId,
            TaktProcurementConstants.BusinessTypeExpense,
            businessKey,
            TaktProcurementConstants.StepNoExpense,
            inquiry?.Id,
            inquiry?.PurchaseInquiryCode);
        return created.CountersignId;
    }

    /// <summary>
    /// 幂等生成采购价格
    /// </summary>
    private async Task EnsurePurchaseInfoRecordFromInquiryAsync(
        TaktPurchaseInquiry inquiry,
        IReadOnlyList<TaktPurchaseInquiryItem> items,
        string supplierCode)
    {
        var existing = await _purchaseInfoRecordRepository.FirstAsync(x =>
            x.PurchaseInquiryId == inquiry.Id && x.IsDeleted == 0);
        if (existing != null)
        {
            return;
        }
        var priceCode = TaktProcurementHelper.DeriveShortCode("P", inquiry.Id);
        var firstItem = items.FirstOrDefault();
        if (firstItem == null || string.IsNullOrWhiteSpace(firstItem.MaterialCode))
        {
            return;
        }
        // 新定价模型：一头一物料；询价多物料时以首行物料建档，条件行承载报价
        var createDto = new TaktPurchaseInfoRecordCreateDto
        {
            PurchaseInfoRecordCode = priceCode,
            SupplierCode = supplierCode,
            MaterialCode = firstItem.MaterialCode,
            PriceType = "PB00",
            ValidFrom = inquiry.InquiryDate,
            ValidTo = inquiry.QuoteDeadlineDate ?? new DateTime(9999, 12, 31, 23, 59, 59),
            PurchaseInquiryId = inquiry.Id,
            PurchaseInquiryCode = inquiry.PurchaseInquiryCode,
            Items = items.Select((item, index) => new TaktPurchaseInfoRecordItemCreateDto
            {
                PurchaseInfoRecordCode = priceCode,
                PurchaseInfoRecordSeq = (index + 1) * 10,
                PriceType = "PB00",
                ScaleBasis = "C",
                ScaleUnit = item.InquiryUnit,
                CalculationType = "A",
                Price = item.QuotedUnitPrice,
            }).ToList()
        };
        var created = await _purchaseInfoRecordService.CreatePurchaseInfoRecordAsync(createDto);
        await PatchPurchaseInfoRecordSourceAsync(created.PurchaseInfoRecordId, inquiry.Id, inquiry.PurchaseInquiryCode);
    }

    /// <summary>
    /// 幂等从询价生成采购申请
    /// </summary>
    private async Task EnsurePurchaseRequestFromInquiryAsync(
        TaktPurchaseInquiry inquiry,
        IReadOnlyList<TaktPurchaseInquiryItem> items,
        TaktCountersign countersign)
    {
        var existing = await _purchaseRequestRepository.FirstAsync(x =>
            x.PurchaseInquiryId == inquiry.Id && x.IsDeleted == 0);
        if (existing != null)
        {
            return;
        }
        var requestCode = TaktProcurementHelper.DeriveShortCode("R", inquiry.Id);
        var createDto = new TaktPurchaseRequestCreateDto
        {
            PlantCode = inquiry.PlantCode,
            PurchaseRequestCode = requestCode,
            RequestDate = DateTime.Now,
            RequiredArrivalDate = inquiry.QuoteDeadlineDate,
            RequestEmployeeId = inquiry.InquiryEmployeeId,
            RequestEmployeeName = inquiry.InquiryEmployeeName ?? string.Empty,
            TotalQuantity = inquiry.TotalQuantity,
            TotalAmount = inquiry.TotalAmount,
            RequestReason = inquiry.InquiryReason ?? countersign.ApplicationReason,
            RequestStatus = 1,
            SupplierCode = inquiry.SupplierCode,
            SupplierName1 = inquiry.SupplierName1,
            Items = items.Select(item => new TaktPurchaseRequestItemCreateDto
            {
                LineNumber = item.LineNumber,
                AllocationCategory = item.AllocationCategory,
                MaterialCode = item.MaterialCode ?? string.Empty,
                MaterialDescription = item.MaterialDescription,
                MaterialSpecification = item.MaterialSpecification,
                RequestUnit = item.InquiryUnit,
                RequestQuantity = item.InquiryQuantity,
                PurchaseRequestUnitPrice = item.QuotedUnitPrice,
                TaxIncludedAmount = item.TaxIncludedAmount,
                UntaxedAmount = item.UntaxedAmount,
                TaxAmount = item.TaxAmount
            }).ToList()
        };
        var created = await _purchaseRequestService.CreatePurchaseRequestAsync(createDto);
        await PatchPurchaseRequestFromInquiryAsync(created.PurchaseRequestId, inquiry);
    }

    /// <summary>
    /// 幂等生成采购订单
    /// </summary>
    private async Task<long> EnsurePurchaseOrderFromRequestAsync(
        TaktPurchaseRequest request,
        TaktPurchaseInquiry inquiry)
    {
        var existing = await _purchaseOrderRepository.FirstAsync(x =>
            x.PurchaseRequestId == request.Id && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing.Id;
        }
        var supplierCode = inquiry.SupplierCode ?? string.Empty;
        var supplierName = string.IsNullOrWhiteSpace(inquiry.SupplierName1)
            ? supplierCode
            : inquiry.SupplierName1;
        if (string.IsNullOrWhiteSpace(supplierCode))
        {
            throw new TaktBusinessException("来源询价缺少供应商，无法生成采购订单");
        }
        var orderCode = TaktProcurementHelper.DeriveShortCode("O", request.Id);
        var requestDto = await _purchaseRequestService.GetPurchaseRequestByIdAsync(request.Id)
            ?? throw new TaktBusinessException("采购申请不存在");
        var createDto = new TaktPurchaseOrderCreateDto
        {
            PlantCode = request.PlantCode,
            PurchaseOrderCode = orderCode,
            SupplierCode = supplierCode,
            SupplierName1 = supplierName,
            OrderDate = DateTime.Now,
            RequiredArrivalDate = request.RequiredArrivalDate,
            TotalQuantity = request.TotalQuantity,
            TotalAmount = request.TotalAmount,
            ActualAmount = request.TotalAmount,
            TaxAmount = 0,
            OrderStatus = 1,
            Items = (requestDto.Items ?? []).Select(item => new TaktPurchaseOrderItemCreateDto
            {
                LineNumber = item.LineNumber,
                RequestCode = request.PurchaseRequestCode,
                RequestLineNumber = item.LineNumber,
                MaterialCode = item.MaterialCode ?? string.Empty,
                MaterialDescription = item.MaterialDescription,
                MaterialSpecification = item.MaterialSpecification,
                PurchaseUnit = item.RequestUnit,
                OrderQuantity = item.RequestQuantity,
                PurchaseUnitPrice = item.PurchaseRequestUnitPrice,
                TaxIncludedAmount = item.TaxIncludedAmount,
                UntaxedAmount = item.UntaxedAmount,
                TaxAmount = item.TaxAmount
            }).ToList()
        };
        var created = await _purchaseOrderService.CreatePurchaseOrderAsync(createDto);
        await PatchPurchaseOrderSourceAsync(created.PurchaseOrderId, request.Id, request.PurchaseRequestCode);
        return created.PurchaseOrderId;
    }

    /// <summary>
    /// 幂等从订单生成费用
    /// </summary>
    private async Task<long> EnsureExpenseFromOrderAsync(
        TaktPurchaseOrder order,
        TaktPurchaseRequest request,
        TaktPurchaseInquiry inquiry)
    {
        var existing = await _expenseRepository.FirstAsync(x =>
            x.PurchaseOrderCode == order.PurchaseOrderCode && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing.Id;
        }
        var expenseCode = TaktProcurementHelper.DeriveExpenseCode(order.PurchaseOrderCode, order.Id);
        var orderDto = await _purchaseOrderService.GetPurchaseOrderByIdAsync(order.Id)
            ?? throw new TaktBusinessException("采购订单不存在");
        var expenseType = ResolveExpenseType(inquiry.PaymentMode);
        var createDto = new TaktExpenseCreateDto
        {
            ExpenseCode = expenseCode,
            ExpenseTitle = $"采购订单费用-{order.PurchaseOrderCode}",
            ExpenseType = expenseType,
            SupplierCode = order.SupplierCode,
            SupplierName1 = order.SupplierName1,
            ApplicantBy = request.RequestEmployeeId ?? inquiry.InquiryEmployeeId ?? 0,
            ExpenseAmount = order.TotalAmount,
            TaxAmount = order.TaxAmount,
            ExpenseDate = DateTime.Now,
            ApplicationReason = request.RequestReason,
            ExpenseStatus = 0,
            ExpenseDetails = (orderDto.Items ?? []).Select((item, index) => new TaktExpenseDetailCreateDto
            {
                LineNumber = item.LineNumber > 0 ? item.LineNumber : (index + 1) * 10,
                AllocationCategory = "K",
                ItemName = item.MaterialDescription,
                ItemDescription = item.MaterialSpecification,
                ItemQuantity = item.OrderQuantity,
                ItemAmount = item.TaxIncludedAmount
            }).ToList()
        };
        var created = await _expenseService.CreateExpenseAsync(createDto);
        await PatchExpenseSourceAsync(created.ExpenseId, request, order);
        return created.ExpenseId;
    }

    /// <summary>
    /// 幂等从 PR 直接生成费用（不经过 PO）
    /// </summary>
    private async Task<long> EnsureExpenseFromRequestAsync(
        TaktPurchaseRequest request,
        TaktPurchaseInquiry inquiry,
        TaktCountersign prCountersign)
    {
        var existing = await _expenseRepository.FirstAsync(x =>
            x.PurchaseRequestCode == request.PurchaseRequestCode
            && x.PurchaseOrderCode == null
            && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing.Id;
        }
        var expenseCode = TaktProcurementHelper.DeriveExpenseCode(request.PurchaseRequestCode, request.Id);
        var requestDto = await _purchaseRequestService.GetPurchaseRequestByIdAsync(request.Id)
            ?? throw new TaktBusinessException("采购申请不存在");
        var expenseType = ResolveExpenseType(inquiry.PaymentMode);
        var createDto = new TaktExpenseCreateDto
        {
            ExpenseCode = expenseCode,
            ExpenseTitle = $"采购申请费用-{request.PurchaseRequestCode}",
            ExpenseType = expenseType,
            SupplierCode = inquiry.SupplierCode,
            SupplierName1 = inquiry.SupplierName1,
            ApplicantBy = request.RequestEmployeeId ?? inquiry.InquiryEmployeeId ?? 0,
            ApplicationDeptId = prCountersign.ApplicationDeptId,
            ApplicationDeptName = prCountersign.ApplicationDeptName,
            CostBearerDeptId = prCountersign.CostBearerDeptId,
            CostBearerDeptName = prCountersign.CostBearerDeptName,
            CountersignId = prCountersign.Id,
            ExpenseAmount = request.TotalAmount,
            TaxAmount = 0,
            ExpenseDate = DateTime.Now,
            ApplicationReason = request.RequestReason,
            ExpenseStatus = 0,
            ExpenseDetails = (requestDto.Items ?? []).Select((item, index) => new TaktExpenseDetailCreateDto
            {
                LineNumber = item.LineNumber > 0 ? item.LineNumber : (index + 1) * 10,
                AllocationCategory = item.AllocationCategory ?? "K",
                ItemName = item.MaterialDescription,
                ItemDescription = item.MaterialSpecification,
                ItemQuantity = item.RequestQuantity,
                ItemAmount = item.TaxIncludedAmount
            }).ToList()
        };
        var created = await _expenseService.CreateExpenseAsync(createDto);
        await PatchExpenseSourceAsync(created.ExpenseId, request, null);
        return created.ExpenseId;
    }

    /// <summary>
    /// 方案三：从独立会签生成费用
    /// </summary>
    private async Task<long> EnsureExpenseFromStandaloneCountersignAsync(
        TaktCountersign countersign,
        IReadOnlyList<TaktCountersignDetail> details)
    {
        var existing = await _expenseRepository.FirstAsync(x =>
            x.CountersignId == countersign.Id && x.IsDeleted == 0);
        if (existing != null)
        {
            return existing.Id;
        }
        var expenseCode = TaktProcurementHelper.DeriveExpenseCode(countersign.CountersignCode, countersign.Id);
        var createDto = new TaktExpenseCreateDto
        {
            ExpenseCode = expenseCode,
            ExpenseTitle = countersign.CountersignTitle ?? $"会签费用-{countersign.CountersignCode}",
            ExpenseType = TaktProcurementConstants.ExpenseTypeMiscPurchase,
            ApplicantBy = countersign.ApplicantBy,
            ApplicantName = countersign.ApplicantName,
            ApplicationDeptId = countersign.ApplicationDeptId,
            ApplicationDeptName = countersign.ApplicationDeptName,
            CostBearerDeptId = countersign.CostBearerDeptId,
            CostBearerDeptName = countersign.CostBearerDeptName,
            FileName = countersign.FileName,
            AccessUrl = countersign.AccessUrl,
            CountersignId = countersign.Id,
            ExpenseAmount = countersign.ApplicationAmount,
            TaxAmount = 0,
            ExpenseDate = DateTime.Now,
            ApplicationReason = countersign.ApplicationReason,
            ExpenseStatus = 0,
            ExpenseDetails = details.Select(detail => new TaktExpenseDetailCreateDto
            {
                LineNumber = detail.LineNumber,
                AllocationCategory = detail.AllocationCategory,
                ItemName = detail.ItemName,
                ItemDescription = detail.ItemDescription,
                ItemQuantity = detail.ItemQuantity,
                ItemAmount = detail.ItemAmount
            }).ToList()
        };
        var created = await _expenseService.CreateExpenseAsync(createDto);
        var entity = await _expenseRepository.GetByIdAsync(created.ExpenseId);
        if (entity != null)
        {
            entity.CountersignId = countersign.Id;
            await _expenseRepository.UpdateAsync(entity);
        }
        return created.ExpenseId;
    }

    /// <summary>
    /// 按付款方式解析费用类型
    /// </summary>
    private static int ResolveExpenseType(string paymentMode)
    {
        return string.Equals(
            paymentMode,
            TaktProcurementConstants.PaymentModeEmployeeReimburse,
            StringComparison.OrdinalIgnoreCase)
            ? TaktProcurementConstants.ExpenseTypeMiscPurchase
            : TaktProcurementConstants.ExpenseTypeSupplierPayment;
    }

    /// <summary>
    /// 回写会签链路字段
    /// </summary>
    private async Task PatchCountersignChainFieldsAsync(
        long countersignId,
        string businessType,
        string businessKey,
        int stepNo,
        long? purchaseInquiryId,
        string? purchaseInquiryCode)
    {
        var entity = await _countersignRepository.GetByIdAsync(countersignId);
        if (entity == null)
        {
            return;
        }
        entity.BusinessType = businessType;
        entity.BusinessKey = businessKey;
        entity.StepNo = stepNo;
        if (purchaseInquiryId.HasValue)
        {
            entity.PurchaseInquiryId = purchaseInquiryId;
            entity.PurchaseInquiryCode = purchaseInquiryCode;
        }
        await _countersignRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 回写采购价格来源询价
    /// </summary>
    private async Task PatchPurchaseInfoRecordSourceAsync(long priceId, long inquiryId, string inquiryCode)
    {
        var entity = await _purchaseInfoRecordRepository.GetByIdAsync(priceId);
        if (entity == null)
        {
            return;
        }
        entity.PurchaseInquiryId = inquiryId;
        entity.PurchaseInquiryCode = inquiryCode;
        await _purchaseInfoRecordRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 回写采购申请来源询价与会签
    /// </summary>
    private async Task PatchPurchaseRequestFromInquiryAsync(
        long requestId,
        TaktPurchaseInquiry inquiry)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(requestId);
        if (entity == null)
        {
            return;
        }
        entity.PurchaseInquiryId = inquiry.Id;
        entity.PurchaseInquiryCode = inquiry.PurchaseInquiryCode;
        entity.ChainScheme = inquiry.ChainScheme;
        await _purchaseRequestRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 回写 PR 会签关联
    /// </summary>
    private async Task PatchPurchaseRequestCountersignAsync(long requestId, long countersignId)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(requestId);
        if (entity == null)
        {
            return;
        }
        var countersign = await _countersignRepository.GetByIdAsync(countersignId);
        entity.CountersignId = countersignId;
        entity.CountersignCode = countersign?.CountersignCode;
        await _purchaseRequestRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 回写采购订单来源申请
    /// </summary>
    private async Task PatchPurchaseOrderSourceAsync(long orderId, long requestId, string requestCode)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(orderId);
        if (entity == null)
        {
            return;
        }
        entity.PurchaseRequestId = requestId;
        entity.PurchaseRequestCode = requestCode;
        await _purchaseOrderRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 回写费用单来源 PR/PO
    /// </summary>
    private async Task PatchExpenseSourceAsync(
        long expenseId,
        TaktPurchaseRequest request,
        TaktPurchaseOrder? order)
    {
        var entity = await _expenseRepository.GetByIdAsync(expenseId);
        if (entity == null)
        {
            return;
        }
        entity.PurchaseRequestCode = request.PurchaseRequestCode;
        if (order != null)
        {
            entity.PurchaseOrderCode = order.PurchaseOrderCode;
        }
        await _expenseRepository.UpdateAsync(entity);
    }
}
