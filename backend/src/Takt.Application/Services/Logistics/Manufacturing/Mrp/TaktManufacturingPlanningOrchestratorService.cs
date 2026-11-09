// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktManufacturingPlanningOrchestratorService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路编排实现（MDS→MPS→MRP→APS→工单 / 采购计划→PR）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement.Chain;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mrp;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 制造计划全链路编排服务
/// </summary>
public class TaktManufacturingPlanningOrchestratorService : TaktServiceBase, ITaktManufacturingPlanningOrchestrator
{
    private const int PublishedBomStatus = 1;
    private const int MrpRunStatusDraft = 0;
    private const int MrpRunStatusRunning = 1;
    private const int MrpRunStatusCompleted = 2;
    private const int MrpRunStatusPublished = 3;
    private const int MrpRunStatusFailed = 4;
    private const int ProcurementTypeMake = 0;
    private const int ProcurementTypeBuy = 1;
    private const int PlannedOrderStatusReleased = 2;

    private readonly ITaktApprovalRepository<TaktMasterDemandSchedule> _mdsRepository;
    private readonly ITaktCompanyRepository<TaktMasterDemandScheduleLine> _mdsLineRepository;
    private readonly ITaktApprovalRepository<TaktSalesForecast> _salesForecastRepository;
    private readonly ITaktCompanyRepository<TaktSalesForecastItem> _salesForecastItemRepository;
    private readonly ITaktTenantRepository<TaktFinancialPeriod> _financialPeriodRepository;
    private readonly ITaktApprovalRepository<TaktRouting> _routingRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktCompanyRepository<TaktApsOperation> _apsOperationRepository;
    private readonly ITaktApprovalRepository<TaktMasterProductionSchedule> _mpsRepository;
    private readonly ITaktCompanyRepository<TaktMasterProductionScheduleLine> _mpsLineRepository;
    private readonly ITaktApprovalRepository<TaktMaterialRequirementsPlanning> _mrpRepository;
    private readonly ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> _mrpItemRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _bomRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktPlannedOrder> _plannedOrderRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ITaktApprovalRepository<TaktProductionPlan> _productionPlanRepository;
    private readonly ITaktCompanyRepository<TaktProductionPlanItem> _productionPlanItemRepository;
    private readonly ITaktApprovalRepository<TaktPurchasePlan> _purchasePlanRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePlanItem> _purchasePlanItemRepository;
    private readonly ITaktCompanyRepository<TaktApsOrder> _apsOrderRepository;
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktCompanyRepository<TaktApsScheduleItem> _apsScheduleItemRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktBillOfMaterialExplosionService _billOfMaterialExplosionService;
    private readonly ITaktPurchaseRequestService _purchaseRequestService;
    private readonly ITaktProcurementChainOrchestrator _procurementChainOrchestrator;

    /// <summary>
    /// 初始化制造计划编排服务
    /// </summary>
    public TaktManufacturingPlanningOrchestratorService(
        ITaktApprovalRepository<TaktMasterDemandSchedule> mdsRepository,
        ITaktCompanyRepository<TaktMasterDemandScheduleLine> mdsLineRepository,
        ITaktApprovalRepository<TaktSalesForecast> salesForecastRepository,
        ITaktCompanyRepository<TaktSalesForecastItem> salesForecastItemRepository,
        ITaktTenantRepository<TaktFinancialPeriod> financialPeriodRepository,
        ITaktApprovalRepository<TaktRouting> routingRepository,
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktCompanyRepository<TaktApsOperation> apsOperationRepository,
        ITaktApprovalRepository<TaktMasterProductionSchedule> mpsRepository,
        ITaktCompanyRepository<TaktMasterProductionScheduleLine> mpsLineRepository,
        ITaktApprovalRepository<TaktMaterialRequirementsPlanning> mrpRepository,
        ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> mrpItemRepository,
        ITaktCompanyRepository<TaktBillOfMaterial> bomRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktPlannedOrder> plannedOrderRepository,
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktApprovalRepository<TaktProductionPlan> productionPlanRepository,
        ITaktCompanyRepository<TaktProductionPlanItem> productionPlanItemRepository,
        ITaktApprovalRepository<TaktPurchasePlan> purchasePlanRepository,
        ITaktCompanyRepository<TaktPurchasePlanItem> purchasePlanItemRepository,
        ITaktCompanyRepository<TaktApsOrder> apsOrderRepository,
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktCompanyRepository<TaktApsScheduleItem> apsScheduleItemRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktBillOfMaterialExplosionService billOfMaterialExplosionService,
        ITaktPurchaseRequestService purchaseRequestService,
        ITaktProcurementChainOrchestrator procurementChainOrchestrator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _mdsRepository = mdsRepository;
        _mdsLineRepository = mdsLineRepository;
        _salesForecastRepository = salesForecastRepository;
        _salesForecastItemRepository = salesForecastItemRepository;
        _financialPeriodRepository = financialPeriodRepository;
        _routingRepository = routingRepository;
        _routingItemRepository = routingItemRepository;
        _apsOperationRepository = apsOperationRepository;
        _mpsRepository = mpsRepository;
        _mpsLineRepository = mpsLineRepository;
        _mrpRepository = mrpRepository;
        _mrpItemRepository = mrpItemRepository;
        _bomRepository = bomRepository;
        _materialPlantRepository = materialPlantRepository;
        _plannedOrderRepository = plannedOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _productionPlanRepository = productionPlanRepository;
        _productionPlanItemRepository = productionPlanItemRepository;
        _purchasePlanRepository = purchasePlanRepository;
        _purchasePlanItemRepository = purchasePlanItemRepository;
        _apsOrderRepository = apsOrderRepository;
        _apsScheduleRepository = apsScheduleRepository;
        _apsScheduleItemRepository = apsScheduleItemRepository;
        _productionOrderRepository = productionOrderRepository;
        _billOfMaterialExplosionService = billOfMaterialExplosionService;
        _purchaseRequestService = purchaseRequestService;
        _procurementChainOrchestrator = procurementChainOrchestrator;
    }

    /// <summary>
    /// 按销售预测各月最新版本生成或刷新 MDS。同一工厂、同一物料、同一月份的数量合并为一条需求
    /// </summary>
    /// <param name="dto">销售预测主键列表</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> RunMdsFromSalesForecastAsync(TaktMdsRunFromSalesForecastDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var ids = ParseIdList(dto.SalesForecastIds);
        var forecasts = new List<TaktSalesForecast>(ids.Count);
        foreach (var id in ids)
        {
            var forecast = await _salesForecastRepository.GetByIdAsync(id)
                ?? throw new TaktBusinessException($"销售预测不存在：{id}");
            forecasts.Add(forecast);
        }

        var plantCode = forecasts[0].PlantCode;
        if (forecasts.Any(x => !string.Equals(x.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)))
        {
            throw new TaktBusinessException("生成 MDS 的销售预测必须属于同一工厂");
        }

        var merged = new Dictionary<string, TaktMasterDemandScheduleLine>(StringComparer.OrdinalIgnoreCase);
        foreach (var forecast in forecasts)
        {
            var items = await _salesForecastItemRepository.GetListAsync(x => x.SalesForecastId == forecast.Id && x.IsObsolete == 0);
            foreach (var group in items.GroupBy(x => $"{x.FiscalYear}|{x.PlanMonth}"))
            {
                var latest = group.OrderBy(x => x.VersionNo).ThenBy(x => x.LineNumber).Last();
                if (latest.PlanQuantity <= 0)
                {
                    continue;
                }

                var bucket = await ResolveForecastBucketAsync(latest.FiscalYear, latest.PlanMonth);
                var key = $"{forecast.MaterialCode}|{bucket.Start:yyyyMMdd}";
                if (!merged.TryGetValue(key, out var line))
                {
                    line = new TaktMasterDemandScheduleLine
                    {
                        TenantCode = CurrentTenantCode,
                        CompanyCode = CurrentCompanyCode,
                        PlantCode = plantCode,
                        DemandSourceType = 1,
                        SalesForecastId = forecast.Id,
                        SalesForecastLineNumber = latest.LineNumber,
                        SalesForecastVersionNo = latest.VersionNo,
                        MaterialCode = forecast.MaterialCode,
                        PlannedBatch = forecast.PlannedBatch,
                        BucketStart = bucket.Start,
                        BucketEnd = bucket.End,
                        UnitOfMeasure = "PC"
                    };
                    merged[key] = line;
                }
                else if (string.IsNullOrWhiteSpace(line.PlannedBatch) && !string.IsNullOrWhiteSpace(forecast.PlannedBatch))
                {
                    line.PlannedBatch = forecast.PlannedBatch;
                }

                line.DemandQuantity += latest.PlanQuantity;
            }
        }

        if (merged.Count == 0)
        {
            throw new TaktBusinessException("销售预测没有可转入 MDS 的版本数量");
        }

        TaktMasterDemandSchedule mds;
        if (dto.MasterDemandScheduleId is > 0)
        {
            mds = await _mdsRepository.GetByIdAsync(dto.MasterDemandScheduleId.Value)
                ?? throw new TaktBusinessException("主需求计划不存在");
            await _mdsLineRepository.DeleteAsync(x => x.MasterDemandScheduleId == mds.Id);
        }
        else
        {
            mds = new TaktMasterDemandSchedule
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = plantCode,
                MdsCode = BuildFlowCode("MD"),
                PlanPeriodStart = merged.Values.Min(x => x.BucketStart),
                PlanPeriodEnd = merged.Values.Max(x => x.BucketEnd),
                BucketType = 2,
                ScheduleStatus = 1,
                ApprovalStatus = 0
            };
            mds = await _mdsRepository.CreateAsync(mds);
        }

        mds.PlanPeriodStart = merged.Values.Min(x => x.BucketStart);
        mds.PlanPeriodEnd = merged.Values.Max(x => x.BucketEnd);
        await _mdsRepository.UpdateAsync(mds);

        var lineNo = 10;
        foreach (var line in merged.Values.OrderBy(x => x.BucketStart).ThenBy(x => x.MaterialCode))
        {
            line.MasterDemandScheduleId = mds.Id;
            line.MdsCode = mds.MdsCode;
            line.LineNumber = lineNo;
            await _mdsLineRepository.CreateAsync(line);
            lineNo = checked(lineNo + 10);
        }

        return new TaktManufacturingPlanningFlowResultDto
        {
            EntityId = mds.Id,
            EntityCode = mds.MdsCode,
            ProcessedCount = merged.Count
        };
    }

    /// <summary>
    /// 从 MDS 生成或刷新 MPS 明细
    /// </summary>
    /// <param name="dto">MDS 下推参数</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> RunMpsFromMdsAsync(TaktMpsRunFromMdsDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var mds = await _mdsRepository.GetByIdAsync(dto.MasterDemandScheduleId)
            ?? throw new TaktBusinessException("主需求计划不存在");
        if (mds.TenantCode != CurrentTenantCode || mds.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("主需求计划不存在");
        }

        var mdsLines = await _mdsLineRepository.GetListAsync(x =>
            x.MasterDemandScheduleId == mds.Id
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        if (mdsLines.Count == 0)
        {
            throw new TaktBusinessException("MDS 无明细行，无法生成 MPS");
        }

        TaktMasterProductionSchedule mps;
        if (dto.MasterProductionScheduleId is > 0)
        {
            mps = await _mpsRepository.GetByIdAsync(dto.MasterProductionScheduleId.Value)
                ?? throw new TaktBusinessException("主生产计划不存在");
            await _mpsLineRepository.DeleteAsync(x => x.MasterProductionScheduleId == mps.Id);
        }
        else
        {
            mps = new TaktMasterProductionSchedule
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = mds.PlantCode,
                MpsCode = BuildFlowCode("MPS"),
                MasterDemandScheduleId = mds.Id,
                MdsCode = mds.MdsCode,
                PlanPeriodStart = mdsLines.Min(l => l.BucketStart),
                PlanPeriodEnd = mdsLines.Max(l => l.BucketEnd),
                BucketType = dto.BucketType ?? 1,
                ScheduleStatus = 1,
                ApprovalStatus = 0
            };
            mps = await _mpsRepository.CreateAsync(mps);
        }

        var forecastBatchById = await LoadSalesForecastPlannedBatchAsync(mdsLines);
        foreach (var mdsLine in mdsLines.OrderBy(l => l.BucketStart).ThenBy(l => l.MaterialCode))
        {
            var gross = mdsLine.DemandQuantity;
            var mpsLine = new TaktMasterProductionScheduleLine
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                MasterProductionScheduleId = mps.Id,
                MpsCode = mps.MpsCode,
                MasterDemandScheduleLineId = mdsLine.Id,
                MaterialCode = mdsLine.MaterialCode,
                PlannedBatch = ResolvePlannedBatch(mdsLine, forecastBatchById),
                BucketStart = mdsLine.BucketStart,
                BucketEnd = mdsLine.BucketEnd,
                GrossRequirement = gross,
                ScheduledReceipts = 0,
                ProjectedOnHand = 0,
                NetRequirement = gross,
                PlannedOrderQuantity = gross,
                AtpQuantity = 0,
                UnitOfMeasure = mdsLine.UnitOfMeasure
            };
            await _mpsLineRepository.CreateAsync(mpsLine);
        }

        return new TaktManufacturingPlanningFlowResultDto
        {
            EntityId = mps.Id,
            EntityCode = mps.MpsCode,
            ProcessedCount = mdsLines.Count
        };
    }

    /// <summary>
    /// 从 MPS 执行完整 MRP 运算（BOM 展开 + 库存/在途抵扣）
    /// </summary>
    /// <param name="dto">MRP 运算参数</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> RunMrpFromMpsAsync(TaktMrpRunDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var options = dto.Options ?? new TaktMrpRunOptionsDto();
        var mrp = await LoadMrpHeaderAsync(dto.MaterialRequirementsPlanningId);
        if (mrp.RunStatus != MrpRunStatusDraft && mrp.RunStatus != MrpRunStatusCompleted)
        {
            throw new TaktBusinessException("MRP 当前状态不允许运算");
        }

        if (mrp.MasterProductionScheduleId is not > 0)
        {
            throw new TaktBusinessException("MRP 未关联 MPS，无法运算");
        }

        mrp.RunStatus = MrpRunStatusRunning;
        await _mrpRepository.UpdateAsync(mrp);

        try
        {
            var mpsLines = await _mpsLineRepository.GetListAsync(x =>
                x.MasterProductionScheduleId == mrp.MasterProductionScheduleId
                && x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
            var demandMap = new Dictionary<string, MrpDemandAccumulator>(StringComparer.OrdinalIgnoreCase);

            foreach (var mpsLine in mpsLines)
            {
                var rootQty = mpsLine.PlannedOrderQuantity > 0 ? mpsLine.PlannedOrderQuantity : mpsLine.NetRequirement;
                if (rootQty <= 0)
                {
                    continue;
                }

                await AccumulateBomDemandAsync(
                    mrp.PlantCode,
                    mpsLine.MaterialCode,
                    rootQty,
                    mpsLine.BucketStart,
                    mpsLine.PlannedBatch,
                    options,
                    demandMap);
            }

            var materialPlants = await _materialPlantRepository.GetListAsync(x =>
                x.PlantCode == mrp.PlantCode
                && x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
            var onHandByMaterial = materialPlants.ToDictionary(x => x.MaterialCode, x => x.CurrentStock, StringComparer.OrdinalIgnoreCase);
            var materialNameByCode = materialPlants.ToDictionary(x => x.MaterialCode, x => x.MaterialDescription, StringComparer.OrdinalIgnoreCase);
            var procurementByMaterial = materialPlants.ToDictionary(x => x.MaterialCode, x => ResolveProcurementType(x.PurchaseType), StringComparer.OrdinalIgnoreCase);

            var scheduledByMaterial = await BuildScheduledReceiptsByMaterialAsync(mrp.PlantCode, options);

            await _mrpItemRepository.DeleteAsync(x => x.MaterialRequirementsPlanningId == mrp.Id);

            var lineNumber = 10;
            var itemCount = 0;
            foreach (var bucket in demandMap.Values.OrderBy(x => x.RequirementDate).ThenBy(x => x.MaterialCode))
            {
                onHandByMaterial.TryGetValue(bucket.MaterialCode, out var onHand);
                scheduledByMaterial.TryGetValue(bucket.MaterialCode, out var receipts);
                var (net, poh) = TaktMrpNettingCalculator.Calculate(bucket.GrossRequirement, onHand, receipts);
                onHandByMaterial[bucket.MaterialCode] = poh;
                scheduledByMaterial[bucket.MaterialCode] = 0;
                procurementByMaterial.TryGetValue(bucket.MaterialCode, out var procurementType);

                var item = new TaktMaterialRequirementsPlanningItem
                {
                    TenantCode = CurrentTenantCode,
                    CompanyCode = CurrentCompanyCode,
                    MaterialRequirementsPlanningId = mrp.Id,
                    MaterialRequirementsPlanningCode = mrp.MaterialRequirementsPlanningCode,
                    LineNumber = lineNumber,
                    MaterialCode = bucket.MaterialCode,
                    PlannedBatch = bucket.PlannedBatch,
                    MaterialDescription = materialNameByCode.GetValueOrDefault(bucket.MaterialCode) ?? bucket.MaterialCode,
                    ParentMaterialCode = bucket.ParentMaterialCode,
                    BomLevel = bucket.BomLevel,
                    RequirementDate = bucket.RequirementDate,
                    PlanUnit = "PC",
                    GrossRequirement = bucket.GrossRequirement,
                    ScheduledReceipts = receipts,
                    OnHandQuantity = onHand,
                    ProjectedOnHand = poh,
                    NetRequirement = net,
                    ProcurementType = procurementType,
                    IsObsolete = 0
                };
                lineNumber = checked(lineNumber + 10);
                itemCount++;
                await _mrpItemRepository.CreateAsync(item);
            }

            mrp.RunStatus = MrpRunStatusCompleted;
            await _mrpRepository.UpdateAsync(mrp);

            return new TaktManufacturingPlanningFlowResultDto
            {
                EntityId = mrp.Id,
                EntityCode = mrp.MaterialRequirementsPlanningCode,
                ProcessedCount = itemCount
            };
        }
        catch (Exception ex)
        {
            mrp.RunStatus = MrpRunStatusFailed;
            await _mrpRepository.UpdateAsync(mrp);
            TaktLogger.Error(ex, "MRP 运算失败 MRP={MrpId}", mrp.Id);
            throw;
        }
    }

    /// <summary>
    /// 发布 MRP 运算结果（自制→计划订单+生产计划，外购→采购计划）
    /// </summary>
    /// <param name="materialRequirementsPlanningId">MRP 头表 ID</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> PublishMrpAsync(long materialRequirementsPlanningId)
    {
        EnsureThreeLayerContext();
        var mrp = await LoadMrpHeaderAsync(materialRequirementsPlanningId);
        if (mrp.RunStatus != MrpRunStatusCompleted)
        {
            throw new TaktBusinessException("MRP 须先完成运算才能发布");
        }

        var items = await _mrpItemRepository.GetListAsync(x =>
            x.MaterialRequirementsPlanningId == mrp.Id
            && x.IsObsolete == 0
            && x.NetRequirement > 0);
        var makeItems = items.Where(x => x.ProcurementType == ProcurementTypeMake).ToList();
        var buyItems = items.Where(x => x.ProcurementType == ProcurementTypeBuy).ToList();
        var createdIds = new List<string>();

        if (makeItems.Count > 0)
        {
            var productionPlan = new TaktProductionPlan
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = mrp.PlantCode,
                ProductionPlanCode = BuildFlowCode("PP"),
                MaterialRequirementsPlanningId = mrp.Id,
                MaterialRequirementsPlanningCode = mrp.MaterialRequirementsPlanningCode,
                PlanDate = DateTime.Now,
                PlanPeriodStart = mrp.PlanPeriodStart,
                PlanPeriodEnd = mrp.PlanPeriodEnd,
                PlanStatus = 1,
                ConvertedStatus = 0,
                ApprovalStatus = 0
            };
            productionPlan = await _productionPlanRepository.CreateAsync(productionPlan);
            mrp.ProductionPlanId = productionPlan.Id;
            createdIds.Add(productionPlan.Id.ToString());

            var ppLine = 10;
            foreach (var item in makeItems)
            {
                await _productionPlanItemRepository.CreateAsync(new TaktProductionPlanItem
                {
                    TenantCode = CurrentTenantCode,
                    CompanyCode = CurrentCompanyCode,
                    ProductionPlanId = productionPlan.Id,
                    ProductionPlanCode = productionPlan.ProductionPlanCode,
                    LineNumber = ppLine,
                    MaterialCode = item.MaterialCode,
                    PlannedBatch = item.PlannedBatch,
                    MaterialDescription = item.MaterialDescription,
                    MaterialSpecification = item.MaterialSpecification,
                    PlanUnit = item.PlanUnit,
                    PlanQuantity = item.NetRequirement,
                    PlannedStartDate = item.RequirementDate,
                    PlannedEndDate = item.RequirementDate,
                    MaterialRequirementsPlanningItemId = item.Id,
                    IsObsolete = 0
                });
                ppLine = checked(ppLine + 10);

                var plannedOrder = new TaktPlannedOrder
                {
                    TenantCode = CurrentTenantCode,
                    CompanyCode = CurrentCompanyCode,
                    PlantCode = mrp.PlantCode,
                    PlannedOrderCode = BuildFlowCode("PO"),
                    MaterialRequirementsPlanningId = mrp.Id,
                    MaterialRequirementsPlanningCode = mrp.MaterialRequirementsPlanningCode,
                    MaterialRequirementsPlanningItemId = item.Id,
                    MaterialCode = item.MaterialCode,
                    PlannedBatch = item.PlannedBatch,
                    PlannedQuantity = item.NetRequirement,
                    UnitOfMeasure = item.PlanUnit,
                    PlannedStartTime = item.RequirementDate,
                    PlannedEndTime = item.RequirementDate,
                    OrderStatus = 0
                };
                plannedOrder = await _plannedOrderRepository.CreateAsync(plannedOrder);
                createdIds.Add(plannedOrder.Id.ToString());
            }
        }

        if (buyItems.Count > 0)
        {
            var purchasePlan = new TaktPurchasePlan
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = mrp.PlantCode,
                PurchasePlanCode = BuildFlowCode("BP"),
                MaterialRequirementsPlanningId = mrp.Id,
                MaterialRequirementsPlanningCode = mrp.MaterialRequirementsPlanningCode,
                ProductionPlanId = mrp.ProductionPlanId,
                PlanDate = DateTime.Now,
                PlanPeriodStart = mrp.PlanPeriodStart,
                PlanPeriodEnd = mrp.PlanPeriodEnd,
                PlanStatus = 1,
                ConvertedStatus = 0,
                ApprovalStatus = 0
            };
            purchasePlan = await _purchasePlanRepository.CreateAsync(purchasePlan);
            mrp.PurchasePlanId = purchasePlan.Id;
            createdIds.Add(purchasePlan.Id.ToString());

            var bpLine = 10;
            foreach (var item in buyItems)
            {
                await _purchasePlanItemRepository.CreateAsync(new TaktPurchasePlanItem
                {
                    TenantCode = CurrentTenantCode,
                    CompanyCode = CurrentCompanyCode,
                    PurchasePlanId = purchasePlan.Id,
                    PurchasePlanCode = purchasePlan.PurchasePlanCode,
                    LineNumber = bpLine,
                    MaterialCode = item.MaterialCode,
                    MaterialDescription = item.MaterialDescription,
                    MaterialSpecification = item.MaterialSpecification,
                    PlanUnit = item.PlanUnit,
                    PlanQuantity = item.NetRequirement,
                    PlannedArrivalDate = item.RequirementDate,
                    MaterialRequirementsPlanningItemId = item.Id,
                    IsObsolete = 0
                });
                bpLine = checked(bpLine + 10);
            }
        }

        mrp.RunStatus = MrpRunStatusPublished;
        await _mrpRepository.UpdateAsync(mrp);

        return new TaktManufacturingPlanningFlowResultDto
        {
            EntityId = mrp.Id,
            EntityCode = mrp.MaterialRequirementsPlanningCode,
            ProcessedCount = items.Count,
            CreatedEntityIds = createdIds
        };
    }

    /// <summary>
    /// 计划订单释放到 APS 订单
    /// </summary>
    /// <param name="dto">计划订单 ID 列表</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> ReleasePlannedOrdersToApsAsync(TaktReleasePlannedOrdersToApsDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var ids = ParseIdList(dto.PlannedOrderIds);
        var created = new List<string>();

        foreach (var id in ids)
        {
            var planned = await _plannedOrderRepository.GetByIdAsync(id)
                ?? throw new TaktBusinessException($"计划订单不存在：{id}");
            if (planned.OrderStatus == PlannedOrderStatusReleased)
            {
                continue;
            }

            var apsOrder = new TaktApsOrder
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = planned.PlantCode,
                ApsOrderCode = BuildFlowCode("AO"),
                PlannedOrderId = planned.Id,
                PlannedOrderCode = planned.PlannedOrderCode,
                MaterialCode = planned.MaterialCode,
                PlannedBatch = planned.PlannedBatch,
                OrderQuantity = planned.PlannedQuantity,
                UnitOfMeasure = planned.UnitOfMeasure,
                RoutingCode = planned.RoutingCode,
                PlannedStartTime = planned.PlannedStartTime,
                PlannedEndTime = planned.PlannedEndTime,
                OrderStatus = 0
            };
            apsOrder = await _apsOrderRepository.CreateAsync(apsOrder);
            await CreateApsOperationsFromRoutingAsync(apsOrder);
            planned.OrderStatus = PlannedOrderStatusReleased;
            await _plannedOrderRepository.UpdateAsync(planned);
            created.Add(apsOrder.Id.ToString());
        }

        return new TaktManufacturingPlanningFlowResultDto
        {
            ProcessedCount = created.Count,
            CreatedEntityIds = created
        };
    }

    /// <summary>
    /// APS 订单排程（无限产能按日期排序）
    /// </summary>
    /// <param name="dto">APS 排程参数</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> RunApsSchedulingAsync(TaktApsScheduleRunDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var orderIds = ParseIdList(dto.ApsOrderIds);
        var orders = new List<TaktApsOrder>();
        foreach (var id in orderIds)
        {
            var order = await _apsOrderRepository.GetByIdAsync(id)
                ?? throw new TaktBusinessException($"APS 订单不存在：{id}");
            orders.Add(order);
        }

        if (orders.Count == 0)
        {
            throw new TaktBusinessException("无 APS 订单可排程");
        }

        TaktApsSchedule schedule;
        if (dto.ApsScheduleId is > 0)
        {
            schedule = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId.Value)
                ?? throw new TaktBusinessException("APS 排程批次不存在");
        }
        else
        {
            var first = orders[0];
            schedule = new TaktApsSchedule
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = first.PlantCode,
                ScheduleCode = BuildFlowCode("AS"),
                ScheduleName = string.IsNullOrWhiteSpace(dto.ScheduleName) ? $"APS排程-{DateTime.Now:yyyy-MM-dd}" : dto.ScheduleName!,
                ScheduleType = 1,
                PlanDate = DateTime.Now,
                PlanStartTime = orders.Min(o => o.PlannedStartTime ?? DateTime.Now),
                PlanEndTime = orders.Max(o => o.PlannedEndTime ?? DateTime.Now.AddDays(7)),
                ScheduleStatus = 2
            };
            schedule = await _apsScheduleRepository.CreateAsync(schedule);
        }

        await _apsScheduleItemRepository.DeleteAsync(x => x.ApsScheduleId == schedule.Id);
        var line = 10;
        DateTime? scheduleEnd = null;
        foreach (var order in orders.OrderBy(o => o.PlannedStartTime ?? DateTime.MaxValue).ThenBy(o => o.ApsOrderCode))
        {
            var operations = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == order.Id && x.IsObsolete == 0);
            if (operations.Count == 0)
            {
                await CreateApsOperationsFromRoutingAsync(order);
                operations = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == order.Id && x.IsObsolete == 0);
            }

            var cursor = order.PlannedStartTime ?? schedule.PlanStartTime;
            var sequence = 1;
            if (operations.Count == 0)
            {
                var end = order.PlannedEndTime ?? cursor.AddDays(1);
                await _apsScheduleItemRepository.CreateAsync(BuildScheduleItem(schedule, order, null, line, sequence, cursor, end));
                cursor = end;
                line = checked(line + 10);
            }
            else
            {
                foreach (var operation in operations.OrderBy(x => x.LineNumber))
                {
                    var minutes = operation.PlannedDurationMinutes + operation.ChangeoverMinutes;
                    if (minutes < 0)
                    {
                        minutes = 0;
                    }

                    var end = cursor.AddMinutes((double)minutes);
                    operation.PlannedStartTime = cursor;
                    operation.PlannedEndTime = end;
                    operation.OperationStatus = 1;
                    await _apsOperationRepository.UpdateAsync(operation);
                    await _apsScheduleItemRepository.CreateAsync(BuildScheduleItem(schedule, order, operation, line, sequence, cursor, end));
                    cursor = end;
                    line = checked(line + 10);
                    sequence++;
                }
            }

            order.PlannedStartTime = order.PlannedStartTime ?? schedule.PlanStartTime;
            order.PlannedEndTime = cursor;
            order.ApsScheduleId = schedule.Id;
            order.OrderStatus = 1;
            await _apsOrderRepository.UpdateAsync(order);
            scheduleEnd = cursor;
        }

        if (scheduleEnd.HasValue)
        {
            schedule.PlanEndTime = scheduleEnd.Value;
            await _apsScheduleRepository.UpdateAsync(schedule);
        }

        return new TaktManufacturingPlanningFlowResultDto
        {
            EntityId = schedule.Id,
            EntityCode = schedule.ScheduleCode,
            ProcessedCount = orders.Count
        };
    }

    /// <summary>
    /// APS 订单释放为生产工单
    /// </summary>
    /// <param name="dto">APS 订单 ID 列表</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> ReleaseApsToProductionOrdersAsync(TaktReleaseApsToProductionDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var orderIds = ParseIdList(dto.ApsOrderIds);
        var created = new List<string>();

        foreach (var id in orderIds)
        {
            var apsOrder = await _apsOrderRepository.GetByIdAsync(id)
                ?? throw new TaktBusinessException($"APS 订单不存在：{id}");
            var prodOrder = new TaktProductionOrder
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                PlantCode = apsOrder.PlantCode,
                ProdOrderType = "ZDTA",
                ProdOrderCode = BuildFlowCode("WO"),
                MaterialCode = apsOrder.MaterialCode,
                ProdBatch = apsOrder.PlannedBatch,
                ProdOrderQty = apsOrder.OrderQuantity,
                UnitOfMeasure = apsOrder.UnitOfMeasure,
                RoutingCode = apsOrder.RoutingCode,
                PlannedOrderId = apsOrder.PlannedOrderId,
                ApsOrderId = apsOrder.Id,
                PlannedStartTime = apsOrder.PlannedStartTime,
                PlannedEndTime = apsOrder.PlannedEndTime,
                Priority = 3
            };
            prodOrder = await _productionOrderRepository.CreateAsync(prodOrder);
            apsOrder.OrderStatus = 2;
            await _apsOrderRepository.UpdateAsync(apsOrder);
            created.Add(prodOrder.Id.ToString());
        }

        return new TaktManufacturingPlanningFlowResultDto
        {
            ProcessedCount = created.Count,
            CreatedEntityIds = created
        };
    }

    /// <summary>
    /// 采购计划转采购申请
    /// </summary>
    /// <param name="purchasePlanId">采购计划 ID</param>
    /// <param name="dto">转 PR 选项</param>
    /// <returns>编排结果</returns>
    public async Task<TaktManufacturingPlanningFlowResultDto> ConvertPurchasePlanToPurchaseRequestAsync(long purchasePlanId, TaktConvertPurchasePlanToPrDto? dto = null)
    {
        EnsureThreeLayerContext();
        dto ??= new TaktConvertPurchasePlanToPrDto();
        var plan = await _purchasePlanRepository.GetByIdAsync(purchasePlanId)
            ?? throw new TaktBusinessException("采购计划不存在");
        var planItems = await _purchasePlanItemRepository.GetListAsync(x =>
            x.PurchasePlanId == plan.Id && x.IsObsolete == 0 && x.PlanQuantity > 0);
        if (planItems.Count == 0)
        {
            throw new TaktBusinessException("采购计划无有效明细");
        }

        var requestCode = BuildFlowCode("PR");
        var supplierCode = planItems
            .Select(x => x.ReferenceSupplierCode)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?.Trim() ?? string.Empty;
        var supplierName1 = planItems
            .Select(x => x.ReferenceSupplierName1)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?.Trim() ?? supplierCode;
        var createDto = new TaktPurchaseRequestCreateDto
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            PlantCode = plan.PlantCode,
            PurchaseRequestCode = requestCode,
            PurchasePlanId = plan.Id,
            PurchasePlanCode = plan.PurchasePlanCode,
            RequestDate = DateTime.Now,
            RequestEmployeeName = CurrentUserName ?? "system",
            RequestStatus = 1,
            ChainScheme = 1,
            SupplierCode = supplierCode,
            SupplierName1 = supplierName1,
            Items = planItems.Select(item => new TaktPurchaseRequestItemCreateDto
            {
                LineNumber = item.LineNumber,
                AllocationCategory = "K",
                MaterialCode = item.MaterialCode,
                MaterialDescription = item.MaterialDescription,
                MaterialSpecification = item.MaterialSpecification,
                RequestUnit = item.PlanUnit,
                RequestQuantity = item.PlanQuantity - item.ConvertedQuantity,
                PurchaseRequestUnitPrice = item.EstimatedUnitPrice,
                TaxIncludedAmount = item.EstimatedAmount,
                PurchasePlanItemId = item.Id
            }).ToList()
        };
        createDto.TotalQuantity = createDto.Items?.Sum(x => x.RequestQuantity) ?? 0;
        createDto.TotalAmount = createDto.Items?.Sum(x => x.TaxIncludedAmount) ?? 0;

        var created = await _purchaseRequestService.CreatePurchaseRequestAsync(createDto);
        if (dto.SubmitForCountersign)
        {
            await _procurementChainOrchestrator.SubmitPurchaseRequestForCountersignAsync(created.PurchaseRequestId);
        }

        plan.ConvertedStatus = 1;
        await _purchasePlanRepository.UpdateAsync(plan);

        return new TaktManufacturingPlanningFlowResultDto
        {
            EntityId = created.PurchaseRequestId,
            EntityCode = created.PurchaseRequestCode,
            ProcessedCount = planItems.Count
        };
    }

    // ========================================
    // 私有辅助
    // ========================================

    private async Task<TaktMaterialRequirementsPlanning> LoadMrpHeaderAsync(long mrpId)
    {
        var mrp = await _mrpRepository.GetByIdAsync(mrpId)
            ?? throw new TaktBusinessException("物料需求计划不存在");
        if (mrp.TenantCode != CurrentTenantCode || mrp.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("物料需求计划不存在");
        }

        return mrp;
    }

    private async Task AccumulateBomDemandAsync(
        string plantCode,
        string materialCode,
        decimal quantity,
        DateTime requirementDate,
        string? plannedBatch,
        TaktMrpRunOptionsDto options,
        Dictionary<string, MrpDemandAccumulator> demandMap)
    {
        var bom = await FindPublishedBomAsync(plantCode, materialCode, options.BomType);
        if (bom == null)
        {
            AddDemand(demandMap, materialCode, null, 1, requirementDate, quantity, plannedBatch);
            return;
        }

        var explosion = await _billOfMaterialExplosionService.GetBillOfMaterialExplosionAsync(new TaktBillOfMaterialExplosionQueryDto
        {
            BillOfMaterialId = bom.Id,
            Quantity = quantity,
            MaxLevel = options.MaxBomLevel,
            IncludeLevelZero = false
        });
        if (explosion?.Lines == null || explosion.Lines.Count == 0)
        {
            AddDemand(demandMap, materialCode, null, 1, requirementDate, quantity, plannedBatch);
            return;
        }

        foreach (var line in explosion.Lines.Where(l => l.HierarchyLevel > 0))
        {
            AddDemand(
                demandMap,
                line.MaterialCode,
                line.ImmediateParentMaterialCode,
                line.HierarchyLevel,
                requirementDate,
                line.CumulativeQuantity,
                plannedBatch);
        }
    }

    private async Task<TaktBillOfMaterial?> FindPublishedBomAsync(string plantCode, string materialCode, int bomType)
    {
        var now = DateTime.Now;
        var candidates = await _bomRepository.GetListAsync(x =>
            x.PlantCode == plantCode
            && x.ParentMaterialCode == materialCode
            && x.BomStatus == PublishedBomStatus
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EffectiveDate <= now
            && (x.ExpiryDate == null || x.ExpiryDate >= now));
        var match = candidates
            .Where(x => x.BomType == bomType)
            .OrderByDescending(x => x.BomVersion)
            .FirstOrDefault();
        return match ?? candidates.OrderByDescending(x => x.BomVersion).FirstOrDefault();
    }

    private async Task<Dictionary<string, decimal>> BuildScheduledReceiptsByMaterialAsync(string plantCode, TaktMrpRunOptionsDto options)
    {
        var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        if (options.IncludePlannedOrders)
        {
            var planned = await _plannedOrderRepository.GetListAsync(x =>
                x.PlantCode == plantCode
                && x.OrderStatus >= 1
                && x.OrderStatus < 3
                && x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
            foreach (var row in planned)
            {
                result[row.MaterialCode] = result.GetValueOrDefault(row.MaterialCode) + row.PlannedQuantity;
            }
        }

        if (options.IncludeOpenPurchaseOrders)
        {
            var poItems = await _purchaseOrderItemRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
            foreach (var row in poItems)
            {
                if (string.IsNullOrWhiteSpace(row.MaterialCode))
                {
                    continue;
                }

                var openQty = row.OrderQuantity - row.ReceivedQuantity;
                if (openQty > 0)
                {
                    result[row.MaterialCode] = result.GetValueOrDefault(row.MaterialCode) + openQty;
                }
            }
        }

        return result;
    }

    private static void AddDemand(
        Dictionary<string, MrpDemandAccumulator> map,
        string materialCode,
        string? parentMaterialCode,
        int bomLevel,
        DateTime requirementDate,
        decimal quantity,
        string? plannedBatch)
    {
        if (quantity <= 0 || string.IsNullOrWhiteSpace(materialCode))
        {
            return;
        }

        var batch = string.IsNullOrWhiteSpace(plannedBatch) ? string.Empty : plannedBatch.Trim();
        var key = $"{materialCode}|{requirementDate:yyyyMMdd}|{parentMaterialCode ?? ""}|{bomLevel}|{batch}";
        if (!map.TryGetValue(key, out var acc))
        {
            acc = new MrpDemandAccumulator
            {
                MaterialCode = materialCode,
                ParentMaterialCode = parentMaterialCode,
                BomLevel = bomLevel,
                RequirementDate = requirementDate,
                PlannedBatch = string.IsNullOrEmpty(batch) ? null : batch
            };
            map[key] = acc;
        }

        acc.GrossRequirement += quantity;
    }

    private static int ResolveProcurementType(string purchaseType)
    {
        if (string.Equals(purchaseType, "F", StringComparison.OrdinalIgnoreCase))
        {
            return ProcurementTypeBuy;
        }

        if (string.Equals(purchaseType, "E", StringComparison.OrdinalIgnoreCase))
        {
            return ProcurementTypeMake;
        }

        return ProcurementTypeMake;
    }

    private static List<long> ParseIdList(IReadOnlyList<string> ids)
    {
        ArgumentNullException.ThrowIfNull(ids);
        if (ids.Count == 0)
        {
            throw new TaktBusinessException("ID 列表不能为空");
        }

        var result = new List<long>(ids.Count);
        foreach (var raw in ids)
        {
            if (!long.TryParse(raw, out var id) || id <= 0)
            {
                throw new TaktBusinessException($"无效 ID：{raw}");
            }

            result.Add(id);
        }

        return result;
    }

    /// <summary>
    /// 按 MDS 行上的销售预测主键取计划批次，供行上未填批次时回退
    /// </summary>
    /// <param name="mdsLines">本次参与 MPS 的 MDS 行</param>
    /// <returns>销售预测 Id → 计划批次</returns>
    private async Task<Dictionary<long, string?>> LoadSalesForecastPlannedBatchAsync(IReadOnlyList<TaktMasterDemandScheduleLine> mdsLines)
    {
        var forecastIds = mdsLines
            .Where(line => line.SalesForecastId is > 0 && string.IsNullOrWhiteSpace(line.PlannedBatch))
            .Select(line => line.SalesForecastId!.Value)
            .Distinct()
            .ToList();
        var result = new Dictionary<long, string?>();
        if (forecastIds.Count == 0)
        {
            return result;
        }

        var forecasts = await _salesForecastRepository.GetListAsync(x => forecastIds.Contains(x.Id));
        foreach (var forecast in forecasts)
        {
            result[forecast.Id] = forecast.PlannedBatch;
        }

        return result;
    }

    /// <summary>
    /// 计划批次：MDS 行优先，空则取关联销售预测
    /// </summary>
    /// <param name="line">MDS 行</param>
    /// <param name="forecastBatchById">销售预测计划批次</param>
    /// <returns>计划批次</returns>
    private static string? ResolvePlannedBatch(
        TaktMasterDemandScheduleLine line,
        IReadOnlyDictionary<long, string?> forecastBatchById)
    {
        if (!string.IsNullOrWhiteSpace(line.PlannedBatch))
        {
            return line.PlannedBatch.Trim();
        }

        if (line.SalesForecastId is > 0
            && forecastBatchById.TryGetValue(line.SalesForecastId.Value, out var forecastBatch)
            && !string.IsNullOrWhiteSpace(forecastBatch))
        {
            return forecastBatch.Trim();
        }

        return null;
    }

    /// <summary>
    /// 按工艺路线工序展开 APS 订单工序。标准工时按订单数量 / 基本数量折算，换型含准备与清理
    /// </summary>
    /// <param name="order">APS 订单</param>
    /// <returns>任务</returns>
    private async Task CreateApsOperationsFromRoutingAsync(TaktApsOrder order)
    {
        var existing = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == order.Id && x.IsObsolete == 0);
        if (existing.Count > 0 || string.IsNullOrWhiteSpace(order.RoutingCode))
        {
            return;
        }

        var routings = await _routingRepository.GetListAsync(x =>
            x.PlantCode == order.PlantCode
            && x.RoutingCode == order.RoutingCode
            && (x.RoutingStatus == 2 || x.RoutingStatus == 4));
        var routing = routings.OrderByDescending(x => x.RoutingStatus).FirstOrDefault();
        if (routing == null)
        {
            return;
        }

        var items = await _routingItemRepository.GetListAsync(x => x.RoutingId == routing.Id && x.IsObsolete == 0);
        var line = 10;
        foreach (var item in items.OrderBy(x => x.SortOrder).ThenBy(x => x.LineNumber))
        {
            var baseQuantity = item.BaseQuantity <= 0 ? 1 : item.BaseQuantity;
            var runMinutes = item.ConvertedMinutes > 0 ? item.ConvertedMinutes : item.StandardMinutes;
            var duration = decimal.Round(order.OrderQuantity / baseQuantity * runMinutes, 2, MidpointRounding.AwayFromZero);
            var processCode = item.LineNumber.ToString("D4");
            if (processCode.Length > 4)
            {
                processCode = processCode[^4..];
            }

            var processName = string.IsNullOrWhiteSpace(item.ProcessDescription) ? $"工序{processCode}" : item.ProcessDescription.Trim();
            if (processName.Length > 70)
            {
                processName = processName[..70];
            }

            var orderCode = order.ApsOrderCode.Length <= 12 ? order.ApsOrderCode : order.ApsOrderCode[..12];
            await _apsOperationRepository.CreateAsync(new TaktApsOperation
            {
                TenantCode = order.TenantCode,
                CompanyCode = order.CompanyCode,
                PlantCode = order.PlantCode,
                ApsOrderId = order.Id,
                ApsOrderCode = orderCode,
                LineNumber = line,
                RoutingItemId = item.Id,
                ProcessCode = processCode,
                ProcessName = processName,
                PlannedDurationMinutes = duration,
                ChangeoverMinutes = item.SetupMinutes + item.TeardownMinutes
            });
            line = checked(line + 10);
        }
    }

    /// <summary>
    /// 组装一条 APS 排程工序
    /// </summary>
    private static TaktApsScheduleItem BuildScheduleItem(
        TaktApsSchedule schedule,
        TaktApsOrder order,
        TaktApsOperation? operation,
        int lineNumber,
        int sequence,
        DateTime start,
        DateTime end)
    {
        var processCode = string.IsNullOrWhiteSpace(operation?.ProcessCode) ? "0000" : operation.ProcessCode;
        var processName = string.IsNullOrWhiteSpace(operation?.ProcessName) ? "主工序" : operation.ProcessName;
        var orderCode = order.ApsOrderCode.Length <= 12 ? order.ApsOrderCode : order.ApsOrderCode[..12];
        var perPiece = operation == null || order.OrderQuantity <= 0
            ? 0
            : decimal.Round(operation.PlannedDurationMinutes / order.OrderQuantity, 2, MidpointRounding.AwayFromZero);
        return new TaktApsScheduleItem
        {
            TenantCode = order.TenantCode,
            CompanyCode = order.CompanyCode,
            PlantCode = order.PlantCode,
            ApsScheduleId = schedule.Id,
            ApsScheduleCode = schedule.ScheduleCode,
            ApsOrderId = order.Id,
            ApsOperationId = operation?.Id,
            RoutingItemId = operation?.RoutingItemId,
            LineNumber = lineNumber,
            WorkOrderCode = orderCode,
            ProductCode = order.MaterialCode,
            ProductName = order.MaterialCode,
            ProcessCode = processCode,
            ProcessName = processName,
            ProcessSequence = sequence,
            ProcessStandardST = perPiece,
            ProcessStandardSTUnit = 3,
            ExtraMinutes = operation?.ChangeoverMinutes ?? 0,
            PlanQuantity = order.OrderQuantity,
            PlanStartTime = start,
            PlanEndTime = end,
            ProcessStatus = 0
        };
    }

    /// <summary>
    /// 财年+计划月份转为自然月时间桶。优先财务期间主数据，没有期间时用财年编码中的年份
    /// </summary>
    /// <param name="fiscalYear">财务年度编码</param>
    /// <param name="planMonth">计划月份</param>
    /// <returns>月初与月末</returns>
    private async Task<(DateTime Start, DateTime End)> ResolveForecastBucketAsync(string fiscalYear, int planMonth)
    {
        var month = planMonth is >= 1 and <= 12 ? planMonth : 1;
        var period = await _financialPeriodRepository.FirstAsync(x => x.FinancialYearCode == fiscalYear && x.CalendarMonth == month);
        DateTime start;
        if (period != null && period.CalendarYear > 0 && period.CalendarMonth is >= 1 and <= 12)
        {
            start = new DateTime(period.CalendarYear, period.CalendarMonth, 1);
        }
        else
        {
            var year = DateTime.Now.Year;
            if (!string.IsNullOrWhiteSpace(fiscalYear) && fiscalYear.Length >= 6 && int.TryParse(fiscalYear.AsSpan(2), out var parsedYear))
            {
                year = parsedYear;
            }

            start = new DateTime(year, month, 1);
        }

        return (start, start.AddMonths(1).AddSeconds(-1));
    }

    private static string BuildFlowCode(string prefix) =>
        $"{prefix}{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";

    private sealed class MrpDemandAccumulator
    {
        public string MaterialCode { get; set; } = string.Empty;
        public string? ParentMaterialCode { get; set; }
        public int BomLevel { get; set; } = 1;
        public DateTime RequirementDate { get; set; }
        public string? PlannedBatch { get; set; }
        public decimal GrossRequirement { get; set; }
    }
}
