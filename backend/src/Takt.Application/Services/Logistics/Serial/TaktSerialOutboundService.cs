// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktSerialOutboundService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号出库应用服务实现
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
/// 序列号出库应用服务
/// </summary>
public class TaktSerialOutboundService : TaktServiceBase, ITaktSerialOutboundService
{
    private readonly ITaktCompanyRepository<TaktSerialOutbound> _serialOutboundRepository;
    private readonly ITaktCompanyRepository<TaktSerialOutboundItem> _serialOutboundItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialOutboundRepository">序列号出库仓储</param>
    /// <param name="serialOutboundItemRepository">SerialOutboundItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSerialOutboundService(
        ITaktCompanyRepository<TaktSerialOutbound> serialOutboundRepository,
        ITaktCompanyRepository<TaktSerialOutboundItem> serialOutboundItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serialOutboundRepository = serialOutboundRepository;
        _serialOutboundItemRepository = serialOutboundItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取序列号出库列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSerialOutboundDto>> GetSerialOutboundListAsync(TaktSerialOutboundQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSerialOutboundDto>.Create(
                new List<TaktSerialOutboundDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serialOutboundRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSerialOutboundDto>.Create(
            data.Adapt<List<TaktSerialOutboundDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundDto?> GetSerialOutboundByIdAsync(long id)
    {
        var entity = await _serialOutboundRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSerialOutboundDto>();
        await FillSerialOutboundDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取序列号出库选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSerialOutboundOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _serialOutboundRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.OutboundCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.OutboundCode,
            DictLabel = e.OutboundCode,
        }).ToList();
    }

    /// <summary>
    /// 创建序列号出库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundDto> CreateSerialOutboundAsync(TaktSerialOutboundCreateDto dto)
    {
        var entity = dto.Adapt<TaktSerialOutbound>();
        var isUnique_ix_takt_logistics_serial_outbound_outbound_unique = await _uniqueValidator.IsUniqueAsync(
            _serialOutboundRepository,
            x => x.PlantCode == entity.PlantCode
                && x.OutboundCode == entity.OutboundCode);
        if (!isUnique_ix_takt_logistics_serial_outbound_outbound_unique)
        {
            throw new TaktBusinessException("序列号出库的PlantCode、OutboundCode已存在");
        }
        entity = await _serialOutboundRepository.CreateAsync(entity);
                await SaveSerialOutboundChildrenAsync(entity, dto);
        return await GetSerialOutboundByIdAsync(entity.Id) ?? entity.Adapt<TaktSerialOutboundDto>();
    }

    /// <summary>
    /// 更新序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundDto> UpdateSerialOutboundAsync(long id, TaktSerialOutboundUpdateDto dto)
    {
        var entity = await _serialOutboundRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号出库不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_serial_outbound_outbound_unique = await _uniqueValidator.IsUniqueAsync(
            _serialOutboundRepository,
            x => x.PlantCode == entity.PlantCode
                && x.OutboundCode == entity.OutboundCode,
            id);
        if (!isUnique_ix_takt_logistics_serial_outbound_outbound_unique)
        {
            throw new TaktBusinessException("序列号出库的PlantCode、OutboundCode已存在");
        }
        await _serialOutboundRepository.UpdateAsync(entity);
                await SaveSerialOutboundChildrenAsync(entity, dto);
        return await GetSerialOutboundByIdAsync(id) ?? throw new TaktBusinessException("序列号出库不存在");
    }

    /// <summary>
    /// 删除序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialOutboundByIdAsync(long id)
    {
        var entity = await _serialOutboundRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号出库不存在或已删除");
        }
        await _serialOutboundItemRepository.DeleteAsync(x => x.OutboundId == entity.Id);
        var deleted = await _serialOutboundRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("序列号出库不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除序列号出库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialOutboundBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSerialOutboundByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSerialOutboundTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSerialOutboundTemplateDto>(
            sheetName ?? "序列号出库导入模板",
            fileName ?? "序列号出库导入模板.xlsx");
    }

    /// <summary>
    /// 导入序列号出库
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSerialOutboundAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSerialOutboundImportDto>(fileStream, sheetName ?? "序列号出库导入模板");
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
                var entity = rows[i].Adapt<TaktSerialOutbound>();
                var importKey = $"{entity.PlantCode}|{entity.OutboundCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、OutboundCode）");
                }
                var isUnique_ix_takt_logistics_serial_outbound_outbound_unique = await _uniqueValidator.IsUniqueAsync(
                    _serialOutboundRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.OutboundCode == entity.OutboundCode);
                if (!isUnique_ix_takt_logistics_serial_outbound_outbound_unique)
                {
                    throw new TaktBusinessException("序列号出库的PlantCode、OutboundCode已存在");
                }
                await _serialOutboundRepository.CreateAsync(entity);
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
    /// 导出序列号出库
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSerialOutboundAsync(TaktSerialOutboundQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSerialOutboundQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialOutboundExportDto>(),
                sheetName ?? "序列号出库数据",
                fileName ?? "序列号出库导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _serialOutboundRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialOutboundExportDto>(),
                sheetName ?? "序列号出库数据",
                fileName ?? "序列号出库导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSerialOutboundExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "序列号出库数据",
            fileName ?? "序列号出库导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废序列号出库明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="outboundId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSerialOutboundItemsObsoleteAsync(long outboundId)
    {
        if (outboundId <= 0)
        {
            return;
        }
        var rows = await _serialOutboundItemRepository.GetListAsync(
            x => x.OutboundId == outboundId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _serialOutboundItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充序列号出库详情（加载 OneToMany 子表：序列号出库明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSerialOutboundDetailsAsync(TaktSerialOutboundDto dto, TaktSerialOutbound entity)
    {
        if (dto == null)
        {
            return;
        }
        // 序列号出库明细 → dto.Items（含作废行）
        var items = await _serialOutboundItemRepository.GetListAsync(x => x.OutboundId == entity.Id);
        dto.Items = items.Adapt<List<TaktSerialOutboundItemDto>>();
    }

    /// <summary>
    /// 保存序列号出库子表级联（序列号出库明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSerialOutboundChildrenAsync(TaktSerialOutbound entity, TaktSerialOutboundCreateDto dto)
    {
        // 序列号出库明细（Items）
        List<TaktSerialOutboundItemUpdateDto>? itemsForSave;
        if (dto is TaktSerialOutboundUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSerialOutboundItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSerialOutboundItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _serialOutboundItemRepository.GetListAsync(x => x.OutboundId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSerialOutboundItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.OutboundId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.OutboundCode = entity.OutboundCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("序列号出库明细第{i + 1}项与本次提交的其他项重复（CompanyCode、OutboundId、LineNumber）");
                }
                if (childDto.SerialOutboundItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SerialOutboundItemId, out var target))
                    {
                        throw new TaktBusinessException("序列号出库明细不存在（SerialOutboundItemId={childDto.SerialOutboundItemId}）");
                    }
                    if (target.OutboundId != entity.Id)
                    {
                        throw new TaktBusinessException("序列号出库明细不属于当前主表（SerialOutboundItemId={childDto.SerialOutboundItemId}）");
                    }
                    submittedIds.Add(childDto.SerialOutboundItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SerialOutboundItemId;
                    target.OutboundId = entity.Id;
                    target.IsObsolete = 0;
                    await _serialOutboundItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSerialOutboundItem>();
                    child.Id = 0;
                    child.OutboundId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _serialOutboundItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.OutboundCode) ? entity.OutboundCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _serialOutboundItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建序列号出库查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSerialOutbound, bool>> QueryExpression(TaktSerialOutboundQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSerialOutbound>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.OutboundCode != null && x.OutboundCode.Contains(keywords))
                || (x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(keywords))
                || (x.Destination != null && x.Destination.Contains(keywords))
                || (x.DestinationPort != null && x.DestinationPort.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.LocationCode != null && x.LocationCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OutboundCode))
        {
            var outboundCode = queryDto.OutboundCode;
            exp = exp.And(x => x.OutboundCode != null && x.OutboundCode.Contains(outboundCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShippingInvoiceCode))
        {
            var shippingInvoiceCode = queryDto.ShippingInvoiceCode;
            exp = exp.And(x => x.ShippingInvoiceCode != null && x.ShippingInvoiceCode.Contains(shippingInvoiceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Destination))
        {
            var destination = queryDto.Destination;
            exp = exp.And(x => x.Destination != null && x.Destination.Contains(destination));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DestinationPort))
        {
            var destinationPort = queryDto.DestinationPort;
            exp = exp.And(x => x.DestinationPort != null && x.DestinationPort.Contains(destinationPort));
        }

        if (queryDto?.OutboundType.HasValue == true)
        {
            var outboundType = queryDto.OutboundType.Value;
            exp = exp.And(x => x.OutboundType == outboundType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LocationCode))
        {
            var locationCode = queryDto.LocationCode;
            exp = exp.And(x => x.LocationCode != null && x.LocationCode.Contains(locationCode));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            var totalQuantity = queryDto.TotalQuantity.Value;
            exp = exp.And(x => x.TotalQuantity == totalQuantity);
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

        if (queryDto?.OutboundDateStart.HasValue == true)
        {
            var outboundDateStart = queryDto.OutboundDateStart.Value;
            exp = exp.And(x => x.OutboundDate >= outboundDateStart);
        }

        if (queryDto?.OutboundDateEnd.HasValue == true)
        {
            var outboundDateEnd = queryDto.OutboundDateEnd.Value;
            exp = exp.And(x => x.OutboundDate <= outboundDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSerialOutboundQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OutboundCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShippingInvoiceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Destination))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DestinationPort))
        {
            return true;
        }
        if (queryDto.OutboundType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LocationCode))
        {
            return true;
        }
        if (queryDto.TotalQuantity.HasValue)
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
        if (queryDto.OutboundDateStart.HasValue || queryDto.OutboundDateEnd.HasValue)
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
