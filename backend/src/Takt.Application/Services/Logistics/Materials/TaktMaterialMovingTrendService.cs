// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialMovingTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移分析服务（门面）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料移动价格推移分析服务
/// </summary>
public class TaktMaterialMovingTrendService : TaktServiceBase, ITaktMaterialMovingTrendService
{
    private readonly ITaktMaterialTrendAnalysisService _analysisService;

    public TaktMaterialMovingTrendService(
        ITaktMaterialTrendAnalysisService analysisService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _analysisService = analysisService;
    }

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null) =>
        _analysisService.GetMaterialMovingTrendPlantOptionsAsync(plantCode, keyword);

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string? plantCode = null, string? keyword = null) =>
        _analysisService.GetMaterialMovingTrendValuationOptionsAsync(plantCode, keyword);

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? valuation = null) =>
        _analysisService.GetMaterialMovingTrendMaterialOptionsAsync(plantCode, keyword, valuation);

    public Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(TaktMaterialMovingTrendQueryDto queryDto) =>
        _analysisService.GetMaterialMovingTrendAnalysisAsync(queryDto);

    public Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query, string? sheetName = null, string? fileName = null) =>
        _analysisService.ExportMaterialMovingTrendAnalysisAsync(query, sheetName, fileName);
}
