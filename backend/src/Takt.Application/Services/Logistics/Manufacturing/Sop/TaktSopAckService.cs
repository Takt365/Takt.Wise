// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopAckService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP确认应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP确认应用服务
/// </summary>
public class TaktSopAckService : TaktServiceBase, ITaktSopAckService
{
    private readonly ITaktCompanyRepository<TaktSopAck> _sopAckRepository;
    private readonly ITaktApprovalRepository<TaktSopDoc> _sopDocRepository;
    private readonly ITaktCompanyRepository<TaktSopRevision> _sopRevisionRepository;
    private readonly ITaktCompanyRepository<TaktSopWorkstation> _sopWorkstationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopAckRepository">SOP确认仓储</param>
    /// <param name="sopDocRepository">SOP文档头仓储</param>
    /// <param name="sopRevisionRepository">SOP版本仓储</param>
    /// <param name="sopWorkstationRepository">SOP工位主数据仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopAckService(
        ITaktCompanyRepository<TaktSopAck> sopAckRepository,
        ITaktApprovalRepository<TaktSopDoc> sopDocRepository,
        ITaktCompanyRepository<TaktSopRevision> sopRevisionRepository,
        ITaktCompanyRepository<TaktSopWorkstation> sopWorkstationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopAckRepository = sopAckRepository;
        _sopDocRepository = sopDocRepository;
        _sopRevisionRepository = sopRevisionRepository;
        _sopWorkstationRepository = sopWorkstationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP确认列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopAckDto>> GetSopAckListAsync(TaktSopAckQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopAckRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopAckDto>.Create(
            data.Adapt<List<TaktSopAckDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopAckDto?> GetSopAckByIdAsync(long id)
    {
        var entity = await _sopAckRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopAckDto>();
    }

    /// <summary>
    /// 获取SOP确认选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopAckOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopAckRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AckComment ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AckComment ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP确认
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopAckDto> CreateSopAckAsync(TaktSopAckCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopAck>();
        await StampSopAckSopDocAsync(entity, dto);
        await StampSopAckSopRevisionAsync(entity, dto);
        await StampSopAckSopWorkstationAsync(entity, dto);
        entity = await _sopAckRepository.CreateAsync(entity);
        return await GetSopAckByIdAsync(entity.Id) ?? entity.Adapt<TaktSopAckDto>();
    }

    /// <summary>
    /// 更新SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopAckDto> UpdateSopAckAsync(long id, TaktSopAckUpdateDto dto)
    {
        var entity = await _sopAckRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP确认不存在");
        }
        dto.Adapt(entity);
        await StampSopAckSopDocAsync(entity, dto);
        await StampSopAckSopRevisionAsync(entity, dto);
        await StampSopAckSopWorkstationAsync(entity, dto);
        await _sopAckRepository.UpdateAsync(entity);
        return await GetSopAckByIdAsync(id) ?? throw new TaktBusinessException("SOP确认不存在");
    }

    /// <summary>
    /// 删除SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopAckByIdAsync(long id)
    {
        var deleted = await _sopAckRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP确认不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP确认
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopAckBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopAckByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopAckTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopAckTemplateDto>(
            sheetName ?? "SOP确认导入模板",
            fileName ?? "SOP确认导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP确认
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopAckAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopAckImportDto>(fileStream, sheetName ?? "SOP确认导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopAck>();
                var importDto = rows[i].Adapt<TaktSopAckCreateDto>();
                await StampSopAckSopDocAsync(entity, importDto);
                await StampSopAckSopRevisionAsync(entity, importDto);
                await StampSopAckSopWorkstationAsync(entity, importDto);
                await _sopAckRepository.CreateAsync(entity);
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
    /// 导出SOP确认
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopAckAsync(TaktSopAckQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopAckQueryDto());
        var list = await _sopAckRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopAckExportDto>(),
                sheetName ?? "SOP确认数据",
                fileName ?? "SOP确认导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopAckExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP确认数据",
            fileName ?? "SOP确认导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP确认主表外键（ManyToOne → SOP文档头）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopAckSopDocAsync(TaktSopAck entity, TaktSopAckCreateDto dto)
    {
        if (dto.SopId <= 0)
        {
            return;
        }
        var master = await _sopDocRepository.GetByIdAsync(dto.SopId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP文档头不存在");
        }
        entity.SopId = master.Id;
    }

    /// <summary>
    /// 同步SOP确认主表外键（ManyToOne → SOP版本）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopAckSopRevisionAsync(TaktSopAck entity, TaktSopAckCreateDto dto)
    {
        if (dto.RevisionId <= 0)
        {
            return;
        }
        var master = await _sopRevisionRepository.GetByIdAsync(dto.RevisionId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP版本不存在");
        }
        entity.RevisionId = master.Id;
    }

    /// <summary>
    /// 同步SOP确认主表外键（ManyToOne → SOP工位主数据）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopAckSopWorkstationAsync(TaktSopAck entity, TaktSopAckCreateDto dto)
    {
        if (dto.WorkstationId is not > 0)
        {
            return;
        }
        var master = await _sopWorkstationRepository.GetByIdAsync(dto.WorkstationId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位主数据不存在");
        }
        entity.WorkstationId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP确认查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopAck, bool>> QueryExpression(TaktSopAckQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopAck>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.SopId).Contains(keywords)
                || SqlFunc.ToString(x.RevisionId).Contains(keywords)
                || SqlFunc.ToString(x.WorkstationId).Contains(keywords)
                || SqlFunc.ToString(x.AcknowledgedBy).Contains(keywords)
                || (x.AckComment != null && x.AckComment.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.AcknowledgedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.SopId.HasValue == true)
        {
            exp = exp.And(x => x.SopId == queryDto.SopId);
        }

        if (queryDto?.RevisionId.HasValue == true)
        {
            exp = exp.And(x => x.RevisionId == queryDto.RevisionId);
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationId == queryDto.WorkstationId);
        }

        if (queryDto?.AcknowledgedBy.HasValue == true)
        {
            exp = exp.And(x => x.AcknowledgedBy == queryDto.AcknowledgedBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.AckComment))
        {
            exp = exp.And(x => x.AckComment != null && x.AckComment.Contains(queryDto.AckComment));
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

        if (queryDto?.AcknowledgedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.AcknowledgedAt >= queryDto.AcknowledgedAtStart);
        }

        if (queryDto?.AcknowledgedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.AcknowledgedAt <= queryDto.AcknowledgedAtEnd);
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
