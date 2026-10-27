// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrderFormFillService.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单表单回填服务（独立非实体 CRUD；generate-services 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单表单回填服务实现
/// </summary>
public class TaktProductionOrderFormFillService : TaktServiceBase, ITaktProductionOrderFormFillService
{
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktApprovalRepository<TaktStandardOperationTime> _standardOperationTimeRepository;
    private readonly ITaktCompanyRepository<TaktStandardOperationRate> _standardOperationRateRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionOrderFormFillService(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionOrderRepository = productionOrderRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _standardOperationTimeRepository = standardOperationTimeRepository;
        _standardOperationRateRepository = standardOperationRateRepository;
    }

    /// <inheritdoc />
    public async Task<TaktProductionOrderFormFillDto?> GetProductionOrderFormFillAsync(
        TaktProductionOrderFormFillQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var prodOrderCode = queryDto.ProdOrderCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return null;
        }
        var plantFilter = queryDto.PlantCode?.Trim();
        var order = await _productionOrderRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ProdOrderCode == prodOrderCode
            && (string.IsNullOrWhiteSpace(plantFilter) || x.PlantCode == plantFilter));
        if (order == null)
        {
            return null;
        }
        string? modelCode = null;
        if (!string.IsNullOrWhiteSpace(order.MaterialCode))
        {
            var model = await _modelDestinationRepository.FirstAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.MaterialCode == order.MaterialCode);
            if (!string.IsNullOrWhiteSpace(model?.ModelCode))
            {
                modelCode = model.ModelCode;
            }
        }
        var plantCode = string.IsNullOrWhiteSpace(order.PlantCode)
            ? (plantFilter ?? string.Empty)
            : order.PlantCode;
        var prodDate = (queryDto.ProdDate ?? DateTime.Today).Date;
        var operationTimes = await TaktAssyOutputDerivedFieldsHelper.ResolveStandardOperationTimesByMaterialAsync(
            _standardOperationTimeRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            order.MaterialCode,
            plantCode,
            prodDate);
        var effectiveRows = TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(operationTimes);
        var stdMinutes = TaktAssyOutputDerivedFieldsHelper.CalculateStdMinutesFromOperationTimes(effectiveRows);
        var operationRatePercent = await TaktAssyOutputDerivedFieldsHelper.ResolvePersonnelOperationRatePercentAsync(
            _standardOperationRateRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            plantCode,
            prodDate);
        decimal? stdCapacity = null;
        if (queryDto.DirectLabor is > 0)
        {
            stdCapacity = TaktProductionStatHelper.CalculateAssyStdCapacity(
                queryDto.DirectLabor.Value,
                stdMinutes,
                operationRatePercent);
        }
        List<TaktProductionOrderFormFillDefaultDetailDto>? defaultDetails = null;
        if (queryDto.IncludeDefaultDetails)
        {
            var preview = TaktPcbaOutputDetailSeedHelper.BuildDefaultDetailPreview(effectiveRows);
            defaultDetails = preview
                .Select(x => new TaktProductionOrderFormFillDefaultDetailDto
                {
                    LineNumber = x.LineNumber,
                    WorkCenter = x.WorkCenter,
                    OperationDesc = x.OperationDesc,
                    StandardShorts = x.StandardShorts,
                    StandardMinutes = x.StandardMinutes,
                })
                .ToList();
        }
        return new TaktProductionOrderFormFillDto
        {
            ProdOrderCode = order.ProdOrderCode,
            PlantCode = plantCode,
            ProdOrderType = order.ProdOrderType,
            ModelCode = modelCode,
            MaterialCode = order.MaterialCode,
            BatchCode = order.ProdBatch,
            ProdOrderQty = order.ProdOrderQty,
            SerialCode = order.SerialCode,
            StdMinutes = stdMinutes,
            OperationRatePercent = operationRatePercent,
            StdCapacity = stdCapacity,
            DefaultDetails = defaultDetails,
        };
    }
}
