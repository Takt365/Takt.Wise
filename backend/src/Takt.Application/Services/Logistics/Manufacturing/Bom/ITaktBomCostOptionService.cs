// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomCostOptionService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项服务接口（工厂 / 期间 / 机种 / 产品 / 物料）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本查询栏共用选项（机种推移 / 差异推移 / 零价格）
/// </summary>
public interface ITaktBomCostOptionService
{
    /// <summary>
    /// 工厂选项：当前公司 RelatedPlant ∩ 头表未删除 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项（通常 0～1 项）</returns>
    Task<List<TaktSelectOption>> GetBomCostOptionPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 物料类型去重（头表；须工厂+期间；仅 IsDeleted=0）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>DictValue/DictLabel=MaterialType</returns>
    Task<List<TaktSelectOption>> GetBomCostOptionMaterialTypeOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null);

    /// <summary>
    /// 机种去重（头表 ModelCode；须工厂+期间；仅 IsDeleted=0）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType 可选</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>DictValue=ModelCode</returns>
    Task<List<TaktSelectOption>> GetBomCostOptionModelOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null);

    /// <summary>
    /// 产品去重（头表 ProductCode；须工厂+期间；仅 IsDeleted=0）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType/ModelCode 可选</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>DictValue=ProductCode</returns>
    Task<List<TaktSelectOption>> GetBomCostOptionProductOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null);

    /// <summary>
    /// 物料/组件去重（明细表；须工厂+期间；X+F+未删除；keyword 远程）
    /// 机种/产品可空：空则不过滤
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；ModelCode/ModelCodes/ProductCode/Keyword 均可空</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>DictValue=ComponentCode</returns>
    Task<List<TaktSelectOption>> GetBomCostOptionMaterialOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null);
}
