// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialExplosionService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 递归展开服务（与物料清单 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 递归展开服务（读 BOM/明细；与 TaktBillOfMaterialService 分离）
/// </summary>
public class TaktBillOfMaterialExplosionService : TaktServiceBase, ITaktBillOfMaterialExplosionService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _billOfMaterialRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialRepository">物料清单仓储</param>
    /// <param name="billOfMaterialItemRepository">物料清单明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialExplosionService(
        ITaktCompanyRepository<TaktBillOfMaterial> billOfMaterialRepository,
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialRepository = billOfMaterialRepository;
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
    }

    /// <inheritdoc />
    public async Task<TaktBillOfMaterialExplosionDto?> GetBillOfMaterialExplosionAsync(TaktBillOfMaterialExplosionQueryDto query)
    {
        ArgumentNullException.ThrowIfNull(query);
        if (query.BillOfMaterialId <= 0)
        {
            throw new TaktBusinessException("物料清单 ID 无效");
        }
        var root = await _billOfMaterialRepository.GetByIdAsync(query.BillOfMaterialId);
        if (root == null || root.TenantCode != CurrentTenantCode || root.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var quantity = query.Quantity <= 0 ? 1 : query.Quantity;
        var result = new TaktBillOfMaterialExplosionDto
        {
            BillOfMaterialId = root.Id,
            BomCode = root.BomCode,
            ParentMaterialCode = root.ParentMaterialCode,
            ParentMaterialName = root.BomName,
            ParentMaterialDescription = root.ParentMaterialDescription,
            Quantity = quantity,
            Lines = new List<TaktBillOfMaterialExplosionLineDto>()
        };
        if (query.IncludeLevelZero)
        {
            result.Lines.Add(BuildExplosionLevelZeroLine(root, quantity));
        }
        var pathStack = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { root.ParentMaterialCode };
        await ExplodeBillOfMaterialAsync(root, quantity, 1, query.MaxLevel, root.ParentMaterialCode, pathStack, result.Lines);
        return result;
    }

    /// <summary>
    /// 构建层级 0 父件行
    /// </summary>
    /// <param name="bom">BOM 头</param>
    /// <param name="quantity">需求数量</param>
    /// <returns>层级 0 行</returns>
    private static TaktBillOfMaterialExplosionLineDto BuildExplosionLevelZeroLine(TaktBillOfMaterial bom, decimal quantity)
    {
        return new TaktBillOfMaterialExplosionLineDto
        {
            HierarchyLevel = 0,
            LevelPrefix = string.Empty,
            SourceBillOfMaterialId = bom.Id,
            LineNumber = 0,
            MaterialId = 0,
            MaterialCode = bom.ParentMaterialCode,
            MaterialName = bom.BomName,
            MaterialDescription = bom.ParentMaterialDescription ?? string.Empty,
            ImmediateParentMaterialCode = string.Empty,
            UsageQuantity = bom.ParentMaterialQuantity <= 0 ? 1 : bom.ParentMaterialQuantity,
            MaterialUnit = bom.ParentMaterialUnit,
            ScrapRate = 0,
            CumulativeQuantity = quantity,
            OperationSeq = 0,
            HasChildBom = 1
        };
    }

    /// <summary>
    /// 递归展开 BOM 直接子件及下级 BOM
    /// </summary>
    /// <param name="bom">当前 BOM 头</param>
    /// <param name="parentQuantity">父件累计数量</param>
    /// <param name="level">当前层级</param>
    /// <param name="maxLevel">最大层级</param>
    /// <param name="immediateParentMaterialCode">直接父件物料编码</param>
    /// <param name="pathStack">展开路径（环检测）</param>
    /// <param name="lines">输出行</param>
    private async Task ExplodeBillOfMaterialAsync(
        TaktBillOfMaterial bom,
        decimal parentQuantity,
        int level,
        int maxLevel,
        string immediateParentMaterialCode,
        HashSet<string> pathStack,
        List<TaktBillOfMaterialExplosionLineDto> lines)
    {
        if (level > maxLevel)
        {
            return;
        }
        var items = await _billOfMaterialItemRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.BillOfMaterialId == bom.Id
            && x.IsObsolete == 0);
        foreach (var item in items.OrderBy(x => x.LineNumber))
        {
            var cumulativeQuantity = CalcExplosionComponentQuantity(parentQuantity, bom, item);
            var childBom = await FindPublishedChildBomAsync(bom.PlantCode, item.MaterialCode, bom.BomType);
            var isCircular = pathStack.Contains(item.MaterialCode);
            lines.Add(new TaktBillOfMaterialExplosionLineDto
            {
                HierarchyLevel = level,
                LevelPrefix = new string('.', level),
                SourceBillOfMaterialId = bom.Id,
                SourceBillOfMaterialItemId = item.Id,
                LineNumber = item.LineNumber,
                MaterialId = item.Id,
                MaterialCode = item.MaterialCode,
                MaterialName = item.MaterialDescription,
                MaterialDescription = item.MaterialDescription ?? string.Empty,
                ImmediateParentMaterialCode = immediateParentMaterialCode,
                UsageQuantity = item.UsageQuantity,
                MaterialUnit = item.MaterialUnit,
                ScrapRate = item.ScrapRate,
                CumulativeQuantity = cumulativeQuantity,
                OperationSeq = item.OperationSeq,
                WorkCenter = item.WorkCenter,
                Position = item.Position,
                IsPhantom = item.IsPhantom,
                IsOptional = item.IsOptional,
                SubstituteGroup = item.SubstituteGroup,
                HasChildBom = childBom == null ? 0 : 1,
                IsCircular = isCircular ? 1 : 0
            });
            if (childBom == null || isCircular)
            {
                continue;
            }
            pathStack.Add(item.MaterialCode);
            await ExplodeBillOfMaterialAsync(
                childBom,
                cumulativeQuantity,
                level + 1,
                maxLevel,
                item.MaterialCode,
                pathStack,
                lines);
            pathStack.Remove(item.MaterialCode);
        }
    }

    /// <summary>
    /// 计算子件累计需求量
    /// </summary>
    /// <param name="parentQuantity">父件累计数量</param>
    /// <param name="bom">BOM 头</param>
    /// <param name="item">明细行</param>
    /// <returns>累计需求量</returns>
    private static decimal CalcExplosionComponentQuantity(decimal parentQuantity, TaktBillOfMaterial bom, TaktBillOfMaterialItem item)
    {
        var baseQty = bom.ParentMaterialQuantity <= 0 ? 1 : bom.ParentMaterialQuantity;
        var usage = item.ActualUsageQuantity > 0
            ? item.ActualUsageQuantity
            : item.UsageQuantity * (1 + item.ScrapRate / 100m);
        return parentQuantity * usage / baseQty;
    }

    /// <summary>
    /// 查找子件已发布 BOM
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="materialCode">物料编码</param>
    /// <param name="bomType">BOM 类型</param>
    /// <returns>已发布 BOM；无则 null</returns>
    private async Task<TaktBillOfMaterial?> FindPublishedChildBomAsync(string plantCode, string materialCode, int bomType)
    {
        var now = DateTime.Now;
        var candidates = await _billOfMaterialRepository.GetListAsync(x =>
            x.PlantCode == plantCode
            && x.ParentMaterialCode == materialCode
            && x.BomStatus == 1
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EffectiveDate <= now
            && (x.ExpiryDate == null || x.ExpiryDate >= now));
        return candidates
            .Where(x => x.BomType == bomType)
            .OrderByDescending(x => x.BomVersion)
            .FirstOrDefault()
            ?? candidates.OrderByDescending(x => x.BomVersion).FirstOrDefault();
    }
}
