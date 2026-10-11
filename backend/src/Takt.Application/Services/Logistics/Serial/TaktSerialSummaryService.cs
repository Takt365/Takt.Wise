// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktSerialSummaryService.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号汇总应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Domain.Entities.Logistics.Serial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Serial;

/// <summary>
/// 序列号汇总应用服务
/// </summary>
public class TaktSerialSummaryService : TaktServiceBase, ITaktSerialSummaryService
{
    private readonly ITaktCompanyRepository<TaktSerialSummary> _serialSummaryRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialSummaryRepository">序列号汇总仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSerialSummaryService(
        ITaktCompanyRepository<TaktSerialSummary> serialSummaryRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serialSummaryRepository = serialSummaryRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取序列号汇总列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSerialSummaryDto>> GetSerialSummaryListAsync(TaktSerialSummaryQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serialSummaryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSerialSummaryDto>.Create(
            data.Adapt<List<TaktSerialSummaryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialSummaryDto?> GetSerialSummaryByIdAsync(long id)
    {
        var entity = await _serialSummaryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSerialSummaryDto>();
    }

    /// <summary>
    /// 获取序列号汇总选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSerialSummaryOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _serialSummaryRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建序列号汇总
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialSummaryDto> CreateSerialSummaryAsync(TaktSerialSummaryCreateDto dto)
    {
        var entity = dto.Adapt<TaktSerialSummary>();
        var isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique = await _uniqueValidator.IsUniqueAsync(
            _serialSummaryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.InboundSerialCode == entity.InboundSerialCode);
        if (!isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique)
        {
            throw new TaktBusinessException("序列号汇总的PlantCode、InboundSerialCode已存在");
        }
        entity = await _serialSummaryRepository.CreateAsync(entity);
        return await GetSerialSummaryByIdAsync(entity.Id) ?? entity.Adapt<TaktSerialSummaryDto>();
    }

    /// <summary>
    /// 更新序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialSummaryDto> UpdateSerialSummaryAsync(long id, TaktSerialSummaryUpdateDto dto)
    {
        var entity = await _serialSummaryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号汇总不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique = await _uniqueValidator.IsUniqueAsync(
            _serialSummaryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.InboundSerialCode == entity.InboundSerialCode,
            id);
        if (!isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique)
        {
            throw new TaktBusinessException("序列号汇总的PlantCode、InboundSerialCode已存在");
        }
        await _serialSummaryRepository.UpdateAsync(entity);
        return await GetSerialSummaryByIdAsync(id) ?? throw new TaktBusinessException("序列号汇总不存在");
    }

    /// <summary>
    /// 删除序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialSummaryByIdAsync(long id)
    {
        var deleted = await _serialSummaryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("序列号汇总不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除序列号汇总
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialSummaryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSerialSummaryByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSerialSummaryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSerialSummaryTemplateDto>(
            sheetName ?? "序列号汇总导入模板",
            fileName ?? "序列号汇总导入模板.xlsx");
    }

    /// <summary>
    /// 导入序列号汇总
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSerialSummaryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSerialSummaryImportDto>(fileStream, sheetName ?? "序列号汇总导入模板");
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
                var entity = rows[i].Adapt<TaktSerialSummary>();
                var importKey = $"{entity.PlantCode}|{entity.InboundSerialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、InboundSerialCode）");
                }
                var isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique = await _uniqueValidator.IsUniqueAsync(
                    _serialSummaryRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.InboundSerialCode == entity.InboundSerialCode);
                if (!isUnique_ix_takt_logistics_serial_summary_inbound_serial_unique)
                {
                    throw new TaktBusinessException("序列号汇总的PlantCode、InboundSerialCode已存在");
                }
                await _serialSummaryRepository.CreateAsync(entity);
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
    /// 导出序列号汇总
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSerialSummaryAsync(TaktSerialSummaryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSerialSummaryQueryDto());
        var list = await _serialSummaryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialSummaryExportDto>(),
                sheetName ?? "序列号汇总数据",
                fileName ?? "序列号汇总导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSerialSummaryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "序列号汇总数据",
            fileName ?? "序列号汇总导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建序列号汇总查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSerialSummary, bool>> QueryExpression(TaktSerialSummaryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSerialSummary>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.InboundCode != null && x.InboundCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.InboundSerialCode != null && x.InboundSerialCode.Contains(keywords))
                || SqlFunc.ToString(x.InboundQuantity).Contains(keywords)
                || (x.ProductInboundSerialCode != null && x.ProductInboundSerialCode.Contains(keywords))
                || (x.OutboundCode != null && x.OutboundCode.Contains(keywords))
                || (x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(keywords))
                || (x.Destination != null && x.Destination.Contains(keywords))
                || (x.DestinationPort != null && x.DestinationPort.Contains(keywords))
                || (x.OutboundSerialCode != null && x.OutboundSerialCode.Contains(keywords))
                || SqlFunc.ToString(x.OutboundQuantity).Contains(keywords)
                || (x.ProductOutboundSerialCode != null && x.ProductOutboundSerialCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InboundDate).Contains(keywords)
                || SqlFunc.ToString(x.LoadingDate).Contains(keywords)
                || SqlFunc.ToString(x.OutboundDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.InboundCode))
        {
            exp = exp.And(x => x.InboundCode != null && x.InboundCode.Contains(queryDto.InboundCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.InboundSerialCode))
        {
            exp = exp.And(x => x.InboundSerialCode != null && x.InboundSerialCode.Contains(queryDto.InboundSerialCode));
        }

        if (queryDto?.InboundQuantity.HasValue == true)
        {
            exp = exp.And(x => x.InboundQuantity == queryDto.InboundQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductInboundSerialCode))
        {
            exp = exp.And(x => x.ProductInboundSerialCode != null && x.ProductInboundSerialCode.Contains(queryDto.ProductInboundSerialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundCode))
        {
            exp = exp.And(x => x.OutboundCode != null && x.OutboundCode.Contains(queryDto.OutboundCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShippingInvoiceCode))
        {
            exp = exp.And(x => x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(queryDto.ShippingInvoiceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Destination))
        {
            exp = exp.And(x => x.Destination != null && x.Destination.Contains(queryDto.Destination));
        }

        if (!string.IsNullOrEmpty(queryDto?.DestinationPort))
        {
            exp = exp.And(x => x.DestinationPort != null && x.DestinationPort.Contains(queryDto.DestinationPort));
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundSerialCode))
        {
            exp = exp.And(x => x.OutboundSerialCode != null && x.OutboundSerialCode.Contains(queryDto.OutboundSerialCode));
        }

        if (queryDto?.OutboundQuantity.HasValue == true)
        {
            exp = exp.And(x => x.OutboundQuantity == queryDto.OutboundQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductOutboundSerialCode))
        {
            exp = exp.And(x => x.ProductOutboundSerialCode != null && x.ProductOutboundSerialCode.Contains(queryDto.ProductOutboundSerialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InboundDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InboundDate >= queryDto.InboundDateStart);
        }

        if (queryDto?.InboundDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InboundDate <= queryDto.InboundDateEnd);
        }

        if (queryDto?.LoadingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.LoadingDate >= queryDto.LoadingDateStart);
        }

        if (queryDto?.LoadingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.LoadingDate <= queryDto.LoadingDateEnd);
        }

        if (queryDto?.OutboundDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate >= queryDto.OutboundDateStart);
        }

        if (queryDto?.OutboundDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate <= queryDto.OutboundDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
