// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSourceEcMapper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主/子表映射为设变主/明细创建 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源数据映射为设变创建 DTO
/// </summary>
public static class TaktEcSourceEcMapper
{
    /// <summary>
    /// 将设变来源主表及明细映射为设变创建 DTO
    /// </summary>
    /// <param name="sourceEc">设变来源主表</param>
    /// <param name="sourceDetails">设变来源明细列表</param>
    /// <param name="plantCode">目标工厂代码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="materialsByCode">目标工厂物料字典（物料编码 → TaktMaterialPlant）；为空时不补全物料衍生字段</param>
    /// <param name="modelCodeByRootMaterial">根物料编码 → 机种编码（TaktModelDestination）；为空时 EcModelCode 回退来源主表 SourceModel</param>
    /// <returns>设变创建 DTO</returns>
    public static TaktEcGijutsuCreateDto ToCreateDto(
        TaktSourceEc sourceEc,
        IReadOnlyList<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture,
        IReadOnlyDictionary<string, TaktMaterialPlant>? materialsByCode = null,
        IReadOnlyDictionary<string, string>? modelCodeByRootMaterial = null)
    {
        ArgumentNullException.ThrowIfNull(sourceEc);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var culture = companyDefaultCulture ?? string.Empty;
        var today = DateTime.Today;
        var ecCode = sourceEc.SourceEcCode ?? string.Empty;
        var fallbackModelCode = sourceEc.SourceModel?.Trim() ?? string.Empty;
        var createDto = new TaktEcGijutsuCreateDto
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            CultureCode = culture,
            PlantCode = plantCode,
            EcCode = ecCode,
            EcIssueDate = sourceEc.SourceIssueDate,
            EcTitle = sourceEc.SourceTitle ?? string.Empty,
            EcContent = sourceEc.SourceEcContent ?? string.Empty,
            EcLeader = string.Empty,
            EcLossAmount = 0,
            EcScope = TaktEcScopeConstants.AllDestination,
            EcDeptScope = TaktEcDeptScopeConstants.ByModel,
            EcEntryDate = today,
            ChangeStatus = TaktEcSourceStatusMapper.MapToChangeStatusOrThrow(sourceEc.SourceStatus),
            EcStatus = 1,
            EcDetails = MapDetails(
                sourceDetails,
                plantCode,
                ecCode,
                tenantCode,
                companyCode,
                culture,
                today,
                fallbackModelCode,
                materialsByCode,
                modelCodeByRootMaterial),
        };
        return createDto;
    }

    /// <summary>
    /// 将设变来源明细映射为设变明细创建 DTO 列表，并按工厂物料、型号目的地补全衍生字段
    /// </summary>
    /// <param name="sourceDetails">设变来源明细</param>
    /// <param name="plantCode">目标工厂代码</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="cultureCode">区域文化编码</param>
    /// <param name="defaultBomDate">BOM 生效日期缺省（来源行无 SourceBomEffectiveDate 时使用）</param>
    /// <param name="fallbackModelCode">型号目的地未命中时的机种回退（来源主表 SourceModel）</param>
    /// <param name="materialsByCode">目标工厂物料字典（物料编码 → TaktMaterialPlant）</param>
    /// <param name="modelCodeByRootMaterial">根物料编码 → 机种编码（TaktModelDestination）</param>
    /// <returns>设变明细创建 DTO 列表</returns>
    public static List<TaktEcDetailCreateDto> MapDetailCreateDtos(
        IReadOnlyList<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string ecCode,
        string tenantCode,
        string companyCode,
        string cultureCode,
        DateTime defaultBomDate,
        string fallbackModelCode,
        IReadOnlyDictionary<string, TaktMaterialPlant> materialsByCode,
        IReadOnlyDictionary<string, string>? modelCodeByRootMaterial = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        ArgumentNullException.ThrowIfNull(materialsByCode);
        if (sourceDetails == null || sourceDetails.Count == 0)
        {
            return [];
        }
        var autoLineNumber = 0;
        var result = new List<TaktEcDetailCreateDto>(sourceDetails.Count);
        foreach (var detail in sourceDetails.OrderBy(x => x.LineNumber).ThenBy(x => x.Id))
        {
            autoLineNumber += 10;
            var dto = MapDetailCreateDto(
                detail,
                plantCode,
                ecCode,
                tenantCode,
                companyCode,
                cultureCode,
                defaultBomDate,
                fallbackModelCode,
                detail.LineNumber > 0 ? detail.LineNumber : autoLineNumber);
            TaktEcDetailMaterialPlantMapper.EnrichCreateDto(dto, materialsByCode, modelCodeByRootMaterial);
            result.Add(dto);
        }
        return result;
    }

    /// <summary>
    /// 单条设变来源明细 → 设变明细创建 DTO（不含工厂物料/型号目的地补全）
    /// </summary>
    /// <param name="detail">设变来源明细</param>
    /// <param name="plantCode">目标工厂代码</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="cultureCode">区域文化编码</param>
    /// <param name="defaultBomDate">BOM 生效日期缺省</param>
    /// <param name="fallbackModelCode">机种回退值</param>
    /// <param name="lineNumber">行号</param>
    /// <returns>设变明细创建 DTO</returns>
    private static TaktEcDetailCreateDto MapDetailCreateDto(
        TaktSourceEcDetail detail,
        string plantCode,
        string ecCode,
        string tenantCode,
        string companyCode,
        string cultureCode,
        DateTime defaultBomDate,
        string fallbackModelCode,
        int lineNumber)
    {
        ArgumentNullException.ThrowIfNull(detail);
        return new TaktEcDetailCreateDto
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            CultureCode = cultureCode ?? string.Empty,
            PlantCode = plantCode,
            EcCode = ecCode,
            LineNumber = lineNumber,
            EcModelCode = fallbackModelCode,
            EcRootMaterialCode = detail.SourceRootMaterialCode,
            EcParentMaterialCode = detail.SourceParentMaterialCode,
            EcOldMaterialCode = detail.SourceOldMaterialCode,
            EcOldMaterialDescription = detail.SourceOldMaterialDescription,
            EcOldUsageQuantity = detail.SourceOldUsageQuantity,
            EcOldItemPosition = detail.SourceOldItemPosition,
            EcNewMaterialCode = detail.SourceNewMaterialCode,
            EcNewMaterialDescription = detail.SourceNewMaterialDescription,
            EcNewUsageQuantity = detail.SourceNewUsageQuantity,
            EcNewItemPosition = detail.SourceNewItemPosition,
            EcBomLineCode = detail.SourceBomCode,
            EcIsCompatible = detail.SourceCompatibility,
            Ec2ndVendor = detail.Source2ndVendor,
            EcInstruction = detail.SourceInstruction,
            EcOldPartDisposition = detail.SourceOldPartDisposition,
            EcBomDate = detail.SourceBomEffectiveDate ?? defaultBomDate,
            IsObsolete = detail.IsObsolete,
        };
    }

    /// <summary>
    /// 映射设变来源明细为设变明细创建 DTO 列表
    /// </summary>
    /// <param name="sourceDetails">设变来源明细</param>
    /// <param name="plantCode">目标工厂代码</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="entryDate">录入日期</param>
    /// <param name="fallbackModelCode">型号目的地未命中时的机种回退（来源主表 SourceModel）</param>
    /// <param name="materialsByCode">目标工厂物料字典</param>
    /// <param name="modelCodeByRootMaterial">根物料编码 → 机种</param>
    /// <returns>设变明细创建 DTO 列表</returns>
    private static List<TaktEcDetailCreateDto> MapDetails(
        IReadOnlyList<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string ecCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture,
        DateTime entryDate,
        string fallbackModelCode,
        IReadOnlyDictionary<string, TaktMaterialPlant>? materialsByCode,
        IReadOnlyDictionary<string, string>? modelCodeByRootMaterial)
    {
        return MapDetailCreateDtos(
            sourceDetails,
            plantCode,
            ecCode,
            tenantCode,
            companyCode,
            companyDefaultCulture ?? string.Empty,
            entryDate,
            fallbackModelCode,
            materialsByCode ?? new Dictionary<string, TaktMaterialPlant>(StringComparer.OrdinalIgnoreCase),
            modelCodeByRootMaterial);
    }
}
