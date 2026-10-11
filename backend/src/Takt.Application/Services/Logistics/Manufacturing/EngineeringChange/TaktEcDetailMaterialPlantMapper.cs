// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailMaterialPlantMapper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：按工厂物料 TaktMaterialPlant、型号目的地 TaktModelDestination 补全设变明细机种/根物料编码与上阶描述/库存/仓库/采购/检验及停产状态（新旧物料描述取自源，不回填）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细与工厂物料、型号目的地字段映射
/// </summary>
public static class TaktEcDetailMaterialPlantMapper
{
    /// <summary>
    /// 按根物料编码从型号目的地列表构建「物料编码 → 机种编码」查找表（同物料取 SortOrder 最小的一条）
    /// </summary>
    /// <param name="destinations">型号目的地列表</param>
    /// <returns>物料编码 → 机种编码（忽略大小写）</returns>
    public static Dictionary<string, string> BuildModelCodeByMaterialLookup(
        IEnumerable<TaktModelDestination> destinations)
    {
        ArgumentNullException.ThrowIfNull(destinations);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in destinations
            .Where(x => !string.IsNullOrWhiteSpace(x.MaterialCode) && !string.IsNullOrWhiteSpace(x.ModelCode))
            .GroupBy(x => x.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            var first = group.OrderBy(x => x.SortOrder).ThenBy(x => x.Id).First();
            result[group.Key] = first.ModelCode.Trim();
        }
        return result;
    }

    /// <summary>
    /// 按物料编码从工厂物料、型号目的地补全设变明细创建 DTO 的衍生字段
    /// </summary>
    /// <param name="dto">设变明细创建 DTO</param>
    /// <param name="materialsByCode">物料编码 → 工厂物料（当前工厂）</param>
    /// <param name="modelCodeByRootMaterial">根物料编码 → 机种编码（TaktModelDestination）；为空时不改 EcModelCode</param>
    public static void EnrichCreateDto(
        TaktEcDetailCreateDto dto,
        IReadOnlyDictionary<string, TaktMaterialPlant> materialsByCode,
        IReadOnlyDictionary<string, string>? modelCodeByRootMaterial = null)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(materialsByCode);
        if (modelCodeByRootMaterial != null
            && TryGetModelCode(modelCodeByRootMaterial, dto.EcRootMaterialCode, out var modelCode))
        {
            dto.EcModelCode = modelCode;
        }
        if (TryGetMaterial(materialsByCode, dto.EcRootMaterialCode, out var rootMaterial))
        {
            dto.EcRootMaterialDescription = ResolveMaterialText(rootMaterial);
            dto.DiscontinuedStatus = ResolveDiscontinuedStatus(rootMaterial);
        }
        if (TryGetMaterial(materialsByCode, dto.EcParentMaterialCode, out var parentMaterial))
        {
            dto.EcParentMaterialDescription = ResolveMaterialText(parentMaterial);
        }
        if (TryGetMaterial(materialsByCode, dto.EcOldMaterialCode, out var oldMaterial))
        {
            dto.EcOldStock = oldMaterial.CurrentStock;
            dto.EcOldWarehouse = ResolveWarehouse(oldMaterial);
            dto.EcOldRequiresInspection = oldMaterial.RequiresInspection;
            dto.EcOldPurchaseType = ResolvePurchaseType(oldMaterial);
        }
        if (TryGetMaterial(materialsByCode, dto.EcNewMaterialCode, out var newMaterial))
        {
            dto.EcNewStock = newMaterial.CurrentStock;
            dto.EcNewWarehouse = ResolveWarehouse(newMaterial);
            dto.EcNewRequiresInspection = newMaterial.RequiresInspection;
            dto.EcNewPurchaseType = ResolvePurchaseType(newMaterial);
        }
    }

    /// <summary>
    /// 收集来源明细行涉及的物料编码（根物料编码、上阶、旧料、新料）
    /// </summary>
    /// <param name="materialCode">单个物料编码</param>
    /// <param name="codes">收集目标</param>
    public static void CollectMaterialCode(string? materialCode, ISet<string> codes)
    {
        ArgumentNullException.ThrowIfNull(codes);
        if (string.IsNullOrWhiteSpace(materialCode))
        {
            return;
        }
        codes.Add(materialCode.Trim());
    }

    /// <summary>
    /// 解析物料显示文本（描述优先，否则物料编码）
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>物料描述</returns>
    private static string ResolveMaterialText(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        if (!string.IsNullOrWhiteSpace(material.MaterialDescription))
        {
            return material.MaterialDescription.Trim();
        }
        return material.MaterialCode;
    }

    /// <summary>
    /// 停产状态：直接取工厂物料 DiscontinuedStatus，空则 Z0
    /// </summary>
    /// <param name="material">根物料编码工厂物料</param>
    /// <returns>字典 DictValue（如 Z0/01）</returns>
    private static string ResolveDiscontinuedStatus(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var status = material.DiscontinuedStatus?.Trim();
        return string.IsNullOrEmpty(status) ? "Z0" : status;
    }

    /// <summary>
    /// 采购类型：取工厂物料 PurchaseType（F/E）
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>F 或 E 或空</returns>
    private static string? ResolvePurchaseType(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var purchaseType = material.PurchaseType?.Trim().ToUpperInvariant();
        if (purchaseType is "F" or "E")
        {
            return purchaseType;
        }
        return string.IsNullOrEmpty(purchaseType) ? null : purchaseType;
    }

    /// <summary>
    /// 仓库：自制取生产仓储，外购取采购仓储
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>仓库编码</returns>
    private static string ResolveWarehouse(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var purchaseType = material.PurchaseType?.Trim().ToUpperInvariant();
        if (purchaseType == "E" && !string.IsNullOrWhiteSpace(material.ProductionLocation))
        {
            return material.ProductionLocation.Trim();
        }
        if (!string.IsNullOrWhiteSpace(material.PurchasingLocation))
        {
            return material.PurchasingLocation.Trim();
        }
        return material.ProductionLocation?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 按物料编码（忽略大小写）查找工厂物料
    /// </summary>
    /// <param name="materialsByCode">物料字典</param>
    /// <param name="materialCode">物料编码</param>
    /// <param name="material">查到的工厂物料</param>
    /// <returns>是否找到</returns>
    private static bool TryGetMaterial(
        IReadOnlyDictionary<string, TaktMaterialPlant> materialsByCode,
        string? materialCode,
        out TaktMaterialPlant material)
    {
        material = null!;
        if (string.IsNullOrWhiteSpace(materialCode))
        {
            return false;
        }
        return materialsByCode.TryGetValue(materialCode.Trim(), out material!);
    }

    /// <summary>
    /// 按根物料编码查找机种编码
    /// </summary>
    /// <param name="modelCodeByRootMaterial">根物料编码 → 机种</param>
    /// <param name="rootMaterial">根物料编码</param>
    /// <param name="modelCode">机种编码</param>
    /// <returns>是否找到</returns>
    private static bool TryGetModelCode(
        IReadOnlyDictionary<string, string> modelCodeByRootMaterial,
        string? rootMaterial,
        out string modelCode)
    {
        modelCode = string.Empty;
        if (string.IsNullOrWhiteSpace(rootMaterial))
        {
            return false;
        }
        if (!modelCodeByRootMaterial.TryGetValue(rootMaterial.Trim(), out var found)
            || string.IsNullOrWhiteSpace(found))
        {
            return false;
        }
        modelCode = found.Trim();
        return true;
    }
}
