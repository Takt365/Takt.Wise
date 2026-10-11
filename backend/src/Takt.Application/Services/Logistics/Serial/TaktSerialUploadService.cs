// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktSerialUploadService.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号上传应用服务实现
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
/// 序列号上传应用服务
/// </summary>
public class TaktSerialUploadService : TaktServiceBase, ITaktSerialUploadService
{
    private readonly ITaktCompanyRepository<TaktSerialUpload> _serialUploadRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialUploadRepository">序列号上传仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSerialUploadService(
        ITaktCompanyRepository<TaktSerialUpload> serialUploadRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serialUploadRepository = serialUploadRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取序列号上传列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSerialUploadDto>> GetSerialUploadListAsync(TaktSerialUploadQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serialUploadRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSerialUploadDto>.Create(
            data.Adapt<List<TaktSerialUploadDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialUploadDto?> GetSerialUploadByIdAsync(long id)
    {
        var entity = await _serialUploadRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSerialUploadDto>();
    }

    /// <summary>
    /// 获取序列号上传选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSerialUploadOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _serialUploadRepository.GetListAsync(
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
    /// 创建序列号上传
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialUploadDto> CreateSerialUploadAsync(TaktSerialUploadCreateDto dto)
    {
        var entity = dto.Adapt<TaktSerialUpload>();
        var isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _serialUploadRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ShippingInvoiceCode == entity.ShippingInvoiceCode
                && x.SequenceCode == entity.SequenceCode);
        if (!isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique)
        {
            throw new TaktBusinessException("序列号上传的PlantCode、ShippingInvoiceCode、SequenceCode已存在");
        }
        entity = await _serialUploadRepository.CreateAsync(entity);
        return await GetSerialUploadByIdAsync(entity.Id) ?? entity.Adapt<TaktSerialUploadDto>();
    }

    /// <summary>
    /// 更新序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialUploadDto> UpdateSerialUploadAsync(long id, TaktSerialUploadUpdateDto dto)
    {
        var entity = await _serialUploadRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号上传不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _serialUploadRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ShippingInvoiceCode == entity.ShippingInvoiceCode
                && x.SequenceCode == entity.SequenceCode,
            id);
        if (!isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique)
        {
            throw new TaktBusinessException("序列号上传的PlantCode、ShippingInvoiceCode、SequenceCode已存在");
        }
        await _serialUploadRepository.UpdateAsync(entity);
        return await GetSerialUploadByIdAsync(id) ?? throw new TaktBusinessException("序列号上传不存在");
    }

    /// <summary>
    /// 删除序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialUploadByIdAsync(long id)
    {
        var deleted = await _serialUploadRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("序列号上传不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除序列号上传
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialUploadBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSerialUploadByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSerialUploadTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSerialUploadTemplateDto>(
            sheetName ?? "序列号上传导入模板",
            fileName ?? "序列号上传导入模板.xlsx");
    }

    /// <summary>
    /// 导入序列号上传
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSerialUploadAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSerialUploadImportDto>(fileStream, sheetName ?? "序列号上传导入模板");
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
                var entity = rows[i].Adapt<TaktSerialUpload>();
                var importKey = $"{entity.PlantCode}|{entity.ShippingInvoiceCode}|{entity.SequenceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ShippingInvoiceCode、SequenceCode）");
                }
                var isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique = await _uniqueValidator.IsUniqueAsync(
                    _serialUploadRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ShippingInvoiceCode == entity.ShippingInvoiceCode
                        && x.SequenceCode == entity.SequenceCode);
                if (!isUnique_ix_takt_logistics_serial_upload_invoice_seq_unique)
                {
                    throw new TaktBusinessException("序列号上传的PlantCode、ShippingInvoiceCode、SequenceCode已存在");
                }
                await _serialUploadRepository.CreateAsync(entity);
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
    /// 导出序列号上传
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSerialUploadAsync(TaktSerialUploadQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSerialUploadQueryDto());
        var list = await _serialUploadRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialUploadExportDto>(),
                sheetName ?? "序列号上传数据",
                fileName ?? "序列号上传导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSerialUploadExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "序列号上传数据",
            fileName ?? "序列号上传导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建序列号上传查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSerialUpload, bool>> QueryExpression(TaktSerialUploadQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSerialUpload>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(keywords))
                || SqlFunc.ToString(x.SequenceCode).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || (x.SerialCode != null && x.SerialCode.Contains(keywords))
                || SqlFunc.ToString(x.PackingQuantity).Contains(keywords)
                || (x.TransportMode != null && x.TransportMode.Contains(keywords))
                || (x.MaterialText != null && x.MaterialText.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OutboundDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShippingInvoiceCode))
        {
            exp = exp.And(x => x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(queryDto.ShippingInvoiceCode));
        }

        if (queryDto?.SequenceCode.HasValue == true)
        {
            exp = exp.And(x => x.SequenceCode == queryDto.SequenceCode);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialCode))
        {
            exp = exp.And(x => x.SerialCode != null && x.SerialCode.Contains(queryDto.SerialCode));
        }

        if (queryDto?.PackingQuantity.HasValue == true)
        {
            exp = exp.And(x => x.PackingQuantity == queryDto.PackingQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.TransportMode))
        {
            exp = exp.And(x => x.TransportMode != null && x.TransportMode.Contains(queryDto.TransportMode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialText))
        {
            exp = exp.And(x => x.MaterialText != null && x.MaterialText.Contains(queryDto.MaterialText));
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
