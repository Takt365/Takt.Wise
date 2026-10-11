// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktSerialOutboundItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号出库明细应用服务实现
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
/// 序列号出库明细应用服务
/// </summary>
public class TaktSerialOutboundItemService : TaktServiceBase, ITaktSerialOutboundItemService
{
    private readonly ITaktCompanyRepository<TaktSerialOutboundItem> _serialOutboundItemRepository;
    private readonly ITaktCompanyRepository<TaktSerialOutbound> _serialOutboundRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialOutboundItemRepository">序列号出库明细仓储</param>
    /// <param name="serialOutboundRepository">序列号出库仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSerialOutboundItemService(
        ITaktCompanyRepository<TaktSerialOutboundItem> serialOutboundItemRepository,
        ITaktCompanyRepository<TaktSerialOutbound> serialOutboundRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serialOutboundItemRepository = serialOutboundItemRepository;
        _serialOutboundRepository = serialOutboundRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取序列号出库明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSerialOutboundItemDto>> GetSerialOutboundItemListAsync(TaktSerialOutboundItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSerialOutboundItemDto>.Create(
                new List<TaktSerialOutboundItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serialOutboundItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSerialOutboundItemDto>.Create(
            data.Adapt<List<TaktSerialOutboundItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取序列号出库明细
    /// </summary>
    /// <param name="id">序列号出库明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundItemDto?> GetSerialOutboundItemByIdAsync(long id)
    {
        var entity = await _serialOutboundItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSerialOutboundItemDto>();
    }

    /// <summary>
    /// 获取序列号出库明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSerialOutboundItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _serialOutboundItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.OutboundCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.OutboundCode,
            DictLabel = e.OutboundCode,
        }).ToList();
    }

    /// <summary>
    /// 创建序列号出库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundItemDto> CreateSerialOutboundItemAsync(TaktSerialOutboundItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSerialOutboundItem>();
        entity.IsObsolete = 0;
        await StampSerialOutboundItemSerialOutboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serialOutboundItemRepository,
            x => x.OutboundSerialCode == entity.OutboundSerialCode);
        if (!isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique)
        {
            throw new TaktBusinessException("序列号出库明细的OutboundSerialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _serialOutboundItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OutboundId == entity.OutboundId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.OutboundCode) ? entity.OutboundCode : entity.OutboundId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _serialOutboundItemRepository.CreateAsync(entity);
        return await GetSerialOutboundItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSerialOutboundItemDto>();
    }

    /// <summary>
    /// 更新序列号出库明细
    /// </summary>
    /// <param name="id">序列号出库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundItemDto> UpdateSerialOutboundItemAsync(long id, TaktSerialOutboundItemUpdateDto dto)
    {
        var entity = await _serialOutboundItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号出库明细不存在");
        }
        dto.Adapt(entity);
        await StampSerialOutboundItemSerialOutboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serialOutboundItemRepository,
            x => x.OutboundSerialCode == entity.OutboundSerialCode,
            id);
        if (!isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique)
        {
            throw new TaktBusinessException("序列号出库明细的OutboundSerialCode已存在");
        }
        await _serialOutboundItemRepository.UpdateAsync(entity);
        return await GetSerialOutboundItemByIdAsync(id) ?? throw new TaktBusinessException("序列号出库明细不存在");
    }

    /// <summary>
    /// 删除序列号出库明细
    /// </summary>
    /// <param name="id">序列号出库明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialOutboundItemByIdAsync(long id)
    {
        var entity = await _serialOutboundItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号出库明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("序列号出库明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("序列号出库明细已作废");
        }
        entity.IsObsolete = 1;
        await _serialOutboundItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除序列号出库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSerialOutboundItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSerialOutboundItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新序列号出库明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSerialOutboundItemDto> UpdateSerialOutboundItemObsoleteAsync(TaktSerialOutboundItemObsoleteDto dto)
    {
        var entity = await _serialOutboundItemRepository.GetByIdAsync(dto.SerialOutboundItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("序列号出库明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("序列号出库明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _serialOutboundItemRepository.UpdateAsync(entity);
        return await GetSerialOutboundItemByIdAsync(dto.SerialOutboundItemId) ?? throw new TaktBusinessException("序列号出库明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSerialOutboundItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSerialOutboundItemTemplateDto>(
            sheetName ?? "序列号出库明细导入模板",
            fileName ?? "序列号出库明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入序列号出库明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSerialOutboundItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSerialOutboundItemImportDto>(fileStream, sheetName ?? "序列号出库明细导入模板");
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
                var entity = rows[i].Adapt<TaktSerialOutboundItem>();
                var importDto = rows[i].Adapt<TaktSerialOutboundItemCreateDto>();
                await StampSerialOutboundItemSerialOutboundAsync(entity, importDto);
                var importKey = $"{entity.OutboundSerialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（OutboundSerialCode）");
                }
                var isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _serialOutboundItemRepository,
                    x => x.OutboundSerialCode == entity.OutboundSerialCode);
                if (!isUnique_ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique)
                {
                    throw new TaktBusinessException("序列号出库明细的OutboundSerialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _serialOutboundItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OutboundId == entity.OutboundId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.OutboundCode) ? entity.OutboundCode : entity.OutboundId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _serialOutboundItemRepository.CreateAsync(entity);
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
    /// 导出序列号出库明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSerialOutboundItemAsync(TaktSerialOutboundItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSerialOutboundItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialOutboundItemExportDto>(),
                sheetName ?? "序列号出库明细数据",
                fileName ?? "序列号出库明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _serialOutboundItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSerialOutboundItemExportDto>(),
                sheetName ?? "序列号出库明细数据",
                fileName ?? "序列号出库明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSerialOutboundItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "序列号出库明细数据",
            fileName ?? "序列号出库明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步序列号出库明细主表外键（ManyToOne → 序列号出库）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSerialOutboundItemSerialOutboundAsync(TaktSerialOutboundItem entity, TaktSerialOutboundItemCreateDto dto)
    {
        if (dto.OutboundId <= 0)
        {
            return;
        }
        var master = await _serialOutboundRepository.GetByIdAsync(dto.OutboundId);
        if (master == null)
        {
            throw new TaktBusinessException("序列号出库不存在");
        }
        entity.OutboundId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.OutboundCode))
        {
            entity.OutboundCode = master.OutboundCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建序列号出库明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSerialOutboundItem, bool>> QueryExpression(TaktSerialOutboundItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSerialOutboundItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.OutboundCode != null && x.OutboundCode.Contains(keywords))
                || (x.OutboundSerialCode != null && x.OutboundSerialCode.Contains(keywords))
                || (x.ReferenceInboundCode != null && x.ReferenceInboundCode.Contains(keywords))
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

        if (queryDto?.OutboundId.HasValue == true)
        {
            var outboundId = queryDto.OutboundId.Value;
            exp = exp.And(x => x.OutboundId == outboundId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OutboundCode))
        {
            var outboundCode = queryDto.OutboundCode;
            exp = exp.And(x => x.OutboundCode != null && x.OutboundCode.Contains(outboundCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OutboundSerialCode))
        {
            var outboundSerialCode = queryDto.OutboundSerialCode;
            exp = exp.And(x => x.OutboundSerialCode != null && x.OutboundSerialCode.Contains(outboundSerialCode));
        }

        if (queryDto?.ReferenceInboundId.HasValue == true)
        {
            var referenceInboundId = queryDto.ReferenceInboundId.Value;
            exp = exp.And(x => x.ReferenceInboundId == referenceInboundId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceInboundCode))
        {
            var referenceInboundCode = queryDto.ReferenceInboundCode;
            exp = exp.And(x => x.ReferenceInboundCode != null && x.ReferenceInboundCode.Contains(referenceInboundCode));
        }

        if (queryDto?.ReferenceInboundLineNumber.HasValue == true)
        {
            var referenceInboundLineNumber = queryDto.ReferenceInboundLineNumber.Value;
            exp = exp.And(x => x.ReferenceInboundLineNumber == referenceInboundLineNumber);
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
    private static bool HasAnyListQueryFilter(TaktSerialOutboundItemQueryDto? queryDto)
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
        if (queryDto.OutboundId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OutboundCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OutboundSerialCode))
        {
            return true;
        }
        if (queryDto.ReferenceInboundId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceInboundCode))
        {
            return true;
        }
        if (queryDto.ReferenceInboundLineNumber.HasValue)
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
        if (queryDto.IsObsolete.HasValue)
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
