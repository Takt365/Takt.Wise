// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSellerMaterialService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：销售商物料应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售商物料应用服务
/// </summary>
public class TaktSellerMaterialService : TaktServiceBase, ITaktSellerMaterialService
{
    private readonly ITaktTenantRepository<TaktSellerMaterial> _sellerMaterialRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sellerMaterialRepository">销售商物料仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSellerMaterialService(
        ITaktTenantRepository<TaktSellerMaterial> sellerMaterialRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sellerMaterialRepository = sellerMaterialRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售商物料列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSellerMaterialDto>> GetSellerMaterialListAsync(TaktSellerMaterialQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSellerMaterialDto>.Create(
                new List<TaktSellerMaterialDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sellerMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSellerMaterialDto>.Create(
            data.Adapt<List<TaktSellerMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSellerMaterialDto?> GetSellerMaterialByIdAsync(long id)
    {
        var entity = await _sellerMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktSellerMaterialDto>();
    }

    /// <summary>
    /// 获取销售商物料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSellerMaterialOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _sellerMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.CustomerShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SellerMaterialCode,
            DictLabel = e.CustomerShortName ?? e.SellerMaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售商物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSellerMaterialDto> CreateSellerMaterialAsync(TaktSellerMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktSellerMaterial>();
        var isUnique_ix_takt_logistics_sales_seller_material_unique = await _uniqueValidator.IsUniqueAsync(
            _sellerMaterialRepository,
            x => x.InternalMaterialCode == entity.InternalMaterialCode
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_sales_seller_material_unique)
        {
            throw new TaktBusinessException("销售商物料的InternalMaterialCode、MaterialCode已存在");
        }
        entity = await _sellerMaterialRepository.CreateAsync(entity);
        return await GetSellerMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktSellerMaterialDto>();
    }

    /// <summary>
    /// 更新销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSellerMaterialDto> UpdateSellerMaterialAsync(long id, TaktSellerMaterialUpdateDto dto)
    {
        var entity = await _sellerMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售商物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_seller_material_unique = await _uniqueValidator.IsUniqueAsync(
            _sellerMaterialRepository,
            x => x.InternalMaterialCode == entity.InternalMaterialCode
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_seller_material_unique)
        {
            throw new TaktBusinessException("销售商物料的InternalMaterialCode、MaterialCode已存在");
        }
        await _sellerMaterialRepository.UpdateAsync(entity);
        return await GetSellerMaterialByIdAsync(id) ?? throw new TaktBusinessException("销售商物料不存在");
    }

    /// <summary>
    /// 删除销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSellerMaterialByIdAsync(long id)
    {
        var deleted = await _sellerMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售商物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售商物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSellerMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSellerMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSellerMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSellerMaterialTemplateDto>(
            sheetName ?? "销售商物料导入模板",
            fileName ?? "销售商物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售商物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSellerMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSellerMaterialImportDto>(fileStream, sheetName ?? "销售商物料导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSellerMaterial>();
                var importKey = $"{entity.InternalMaterialCode}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InternalMaterialCode、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_sales_seller_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _sellerMaterialRepository,
                    x => x.InternalMaterialCode == entity.InternalMaterialCode
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_sales_seller_material_unique)
                {
                    throw new TaktBusinessException("销售商物料的InternalMaterialCode、MaterialCode已存在");
                }
                await _sellerMaterialRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出销售商物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSellerMaterialAsync(TaktSellerMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSellerMaterialQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSellerMaterialExportDto>(),
                sheetName ?? "销售商物料数据",
                fileName ?? "销售商物料导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sellerMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSellerMaterialExportDto>(),
                sheetName ?? "销售商物料数据",
                fileName ?? "销售商物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSellerMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售商物料数据",
            fileName ?? "销售商物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售商物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSellerMaterial, bool>> QueryExpression(TaktSellerMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSellerMaterial>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerShortName != null && x.CustomerShortName.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientShortName != null && x.ClientShortName.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.InternalMaterialCode != null && x.InternalMaterialCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.SellerMaterialCode != null && x.SellerMaterialCode.Contains(keywords))
                || (x.SellerMaterialDescription != null && x.SellerMaterialDescription.Contains(keywords))
                || (x.SellerMaterialSpecification != null && x.SellerMaterialSpecification.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerShortName))
        {
            var customerShortName = queryDto.CustomerShortName;
            exp = exp.And(x => x.CustomerShortName != null && x.CustomerShortName.Contains(customerShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientCode))
        {
            var clientCode = queryDto.ClientCode;
            exp = exp.And(x => x.ClientCode != null && x.ClientCode.Contains(clientCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientShortName))
        {
            var clientShortName = queryDto.ClientShortName;
            exp = exp.And(x => x.ClientShortName != null && x.ClientShortName.Contains(clientShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialType))
        {
            var materialType = queryDto.MaterialType;
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(materialType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroup))
        {
            var materialGroup = queryDto.MaterialGroup;
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(materialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InternalMaterialCode))
        {
            var internalMaterialCode = queryDto.InternalMaterialCode;
            exp = exp.And(x => x.InternalMaterialCode != null && x.InternalMaterialCode.Contains(internalMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDescription))
        {
            var materialDescription = queryDto.MaterialDescription;
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(materialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SellerMaterialCode))
        {
            var sellerMaterialCode = queryDto.SellerMaterialCode;
            exp = exp.And(x => x.SellerMaterialCode != null && x.SellerMaterialCode.Contains(sellerMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SellerMaterialDescription))
        {
            var sellerMaterialDescription = queryDto.SellerMaterialDescription;
            exp = exp.And(x => x.SellerMaterialDescription != null && x.SellerMaterialDescription.Contains(sellerMaterialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SellerMaterialSpecification))
        {
            var sellerMaterialSpecification = queryDto.SellerMaterialSpecification;
            exp = exp.And(x => x.SellerMaterialSpecification != null && x.SellerMaterialSpecification.Contains(sellerMaterialSpecification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktSellerMaterialQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InternalMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SellerMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SellerMaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SellerMaterialSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
