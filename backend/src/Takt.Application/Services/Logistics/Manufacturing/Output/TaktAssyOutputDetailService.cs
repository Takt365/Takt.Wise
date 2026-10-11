// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报明细应用服务
/// </summary>
public class TaktAssyOutputDetailService : TaktServiceBase, ITaktAssyOutputDetailService
{
    private readonly ITaktCompanyRepository<TaktAssyOutputDetail> _assyOutputDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyOutput> _assyOutputRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyOutputDetailService(
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyOutputDetailRepository = assyOutputDetailRepository;
        _assyOutputRepository = assyOutputRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立日报明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyOutputDetailDto>> GetAssyOutputDetailListAsync(TaktAssyOutputDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAssyOutputDetailDto>.Create(
                new List<TaktAssyOutputDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyOutputDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyOutputDetailDto>.Create(
            data.Adapt<List<TaktAssyOutputDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto?> GetAssyOutputDetailByIdAsync(long id)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssyOutputDetailDto>();
    }

    /// <summary>
    /// 获取组立日报明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyOutputDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _assyOutputDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建组立日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto> CreateAssyOutputDetailAsync(TaktAssyOutputDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyOutputDetail>();
        entity.IsObsolete = 0;
        await StampAssyOutputDetailAssyOutputAsync(entity, dto);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _assyOutputDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyOutputId == entity.AssyOutputId,
                x => x.LineNumber);
            var businessCode = entity.AssyOutputId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        var timePeriod = (entity.TimePeriod ?? string.Empty).Trim();
        entity.TimePeriod = timePeriod;
        var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputDetailRepository,
            x => x.AssyOutputId == entity.AssyOutputId
                && x.TimePeriod == timePeriod
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique)
        {
            throw new TaktBusinessException("组立日报明细的AssyOutputId、TimePeriod、LineNumber已存在");
        }
        entity = await _assyOutputDetailRepository.CreateAsync(entity);
        return await GetAssyOutputDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyOutputDetailDto>();
    }

    /// <summary>
    /// 更新组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto> UpdateAssyOutputDetailAsync(long id, TaktAssyOutputDetailUpdateDto dto)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报明细不存在");
        }
        dto.Adapt(entity);
        await StampAssyOutputDetailAssyOutputAsync(entity, dto);
        var timePeriod = (entity.TimePeriod ?? string.Empty).Trim();
        entity.TimePeriod = timePeriod;
        var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputDetailRepository,
            x => x.AssyOutputId == entity.AssyOutputId
                && x.TimePeriod == timePeriod
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique)
        {
            throw new TaktBusinessException("组立日报明细的AssyOutputId、TimePeriod、LineNumber已存在");
        }
        await _assyOutputDetailRepository.UpdateAsync(entity);
        return await GetAssyOutputDetailByIdAsync(id) ?? throw new TaktBusinessException("组立日报明细不存在");
    }

    /// <summary>
    /// 删除组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputDetailByIdAsync(long id)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("组立日报明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("组立日报明细已作废");
        }
        entity.IsObsolete = 1;
        await _assyOutputDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除组立日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyOutputDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新组立日报明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto> UpdateAssyOutputDetailObsoleteAsync(TaktAssyOutputDetailObsoleteDto dto)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(dto.AssyOutputDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("组立日报明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _assyOutputDetailRepository.UpdateAsync(entity);
        return await GetAssyOutputDetailByIdAsync(dto.AssyOutputDetailId) ?? throw new TaktBusinessException("组立日报明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyOutputDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyOutputDetailTemplateDto>(
            sheetName ?? "组立日报明细导入模板",
            fileName ?? "组立日报明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立日报明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyOutputDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyOutputDetailImportDto>(fileStream, sheetName ?? "组立日报明细导入模板");
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
                var entity = rows[i].Adapt<TaktAssyOutputDetail>();
                var importDto = rows[i].Adapt<TaktAssyOutputDetailCreateDto>();
                await StampAssyOutputDetailAssyOutputAsync(entity, importDto);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _assyOutputDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyOutputId == entity.AssyOutputId,
                        x => x.LineNumber);
                    var businessCode = entity.AssyOutputId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                var timePeriod = (entity.TimePeriod ?? string.Empty).Trim();
                entity.TimePeriod = timePeriod;
                var importKey = $"{entity.AssyOutputId}|{timePeriod}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssyOutputId、TimePeriod、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyOutputDetailRepository,
                    x => x.AssyOutputId == entity.AssyOutputId
                        && x.TimePeriod == timePeriod
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_unique)
                {
                    throw new TaktBusinessException("组立日报明细的AssyOutputId、TimePeriod、LineNumber已存在");
                }
                await _assyOutputDetailRepository.CreateAsync(entity);
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
    /// 导出组立日报明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyOutputDetailAsync(TaktAssyOutputDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAssyOutputDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyOutputDetailExportDto>(),
                sheetName ?? "组立日报明细数据",
                fileName ?? "组立日报明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _assyOutputDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyOutputDetailExportDto>(),
                sheetName ?? "组立日报明细数据",
                fileName ?? "组立日报明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyOutputDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立日报明细数据",
            fileName ?? "组立日报明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步组立日报明细主表外键（ManyToOne → 组立日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampAssyOutputDetailAssyOutputAsync(TaktAssyOutputDetail entity, TaktAssyOutputDetailCreateDto dto)
    {
        if (dto.AssyOutputId <= 0)
        {
            return;
        }
        var master = await _assyOutputRepository.GetByIdAsync(dto.AssyOutputId);
        if (master == null)
        {
            throw new TaktBusinessException("组立日报不存在");
        }
        entity.AssyOutputId = master.Id;
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
        if (string.IsNullOrEmpty(entity.ProdOrderCode))
        {
            entity.ProdOrderCode = master.ProdOrderCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立日报明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyOutputDetail, bool>> QueryExpression(TaktAssyOutputDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyOutputDetail>();

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
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.TimePeriod != null && x.TimePeriod.Contains(keywords))
                || (x.DowntimeReason != null && x.DowntimeReason.Contains(keywords))
                || (x.DowntimeDescription != null && x.DowntimeDescription.Contains(keywords))
                || (x.UnachievedReason != null && x.UnachievedReason.Contains(keywords))
                || (x.UnachievedDescription != null && x.UnachievedDescription.Contains(keywords))
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

        if (queryDto?.AssyOutputId.HasValue == true)
        {
            var assyOutputId = queryDto.AssyOutputId.Value;
            exp = exp.And(x => x.AssyOutputId == assyOutputId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TimePeriod))
        {
            var timePeriod = queryDto.TimePeriod;
            exp = exp.And(x => x.TimePeriod != null && x.TimePeriod.Contains(timePeriod));
        }

        if (queryDto?.StdCapacity.HasValue == true)
        {
            var stdCapacity = queryDto.StdCapacity.Value;
            exp = exp.And(x => x.StdCapacity == stdCapacity);
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            var prodActualQty = queryDto.ProdActualQty.Value;
            exp = exp.And(x => x.ProdActualQty == prodActualQty);
        }

        if (queryDto?.DowntimeMinutes.HasValue == true)
        {
            var downtimeMinutes = queryDto.DowntimeMinutes.Value;
            exp = exp.And(x => x.DowntimeMinutes == downtimeMinutes);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DowntimeReason))
        {
            var downtimeReason = queryDto.DowntimeReason;
            exp = exp.And(x => x.DowntimeReason != null && x.DowntimeReason.Contains(downtimeReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DowntimeDescription))
        {
            var downtimeDescription = queryDto.DowntimeDescription;
            exp = exp.And(x => x.DowntimeDescription != null && x.DowntimeDescription.Contains(downtimeDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnachievedReason))
        {
            var unachievedReason = queryDto.UnachievedReason;
            exp = exp.And(x => x.UnachievedReason != null && x.UnachievedReason.Contains(unachievedReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnachievedDescription))
        {
            var unachievedDescription = queryDto.UnachievedDescription;
            exp = exp.And(x => x.UnachievedDescription != null && x.UnachievedDescription.Contains(unachievedDescription));
        }

        if (queryDto?.InputMinutes.HasValue == true)
        {
            var inputMinutes = queryDto.InputMinutes.Value;
            exp = exp.And(x => x.InputMinutes == inputMinutes);
        }

        if (queryDto?.ActualMinutes.HasValue == true)
        {
            var actualMinutes = queryDto.ActualMinutes.Value;
            exp = exp.And(x => x.ActualMinutes == actualMinutes);
        }

        if (queryDto?.IndirectMinutes.HasValue == true)
        {
            var indirectMinutes = queryDto.IndirectMinutes.Value;
            exp = exp.And(x => x.IndirectMinutes == indirectMinutes);
        }

        if (queryDto?.ConfirmMinutes.HasValue == true)
        {
            var confirmMinutes = queryDto.ConfirmMinutes.Value;
            exp = exp.And(x => x.ConfirmMinutes == confirmMinutes);
        }

        if (queryDto?.MixedProd.HasValue == true)
        {
            var mixedProd = queryDto.MixedProd.Value;
            exp = exp.And(x => x.MixedProd == mixedProd);
        }

        if (queryDto?.AchievementRate.HasValue == true)
        {
            var achievementRate = queryDto.AchievementRate.Value;
            exp = exp.And(x => x.AchievementRate == achievementRate);
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
    private static bool HasAnyListQueryFilter(TaktAssyOutputDetailQueryDto? queryDto)
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
        if (queryDto.AssyOutputId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TimePeriod))
        {
            return true;
        }
        if (queryDto.StdCapacity.HasValue)
        {
            return true;
        }
        if (queryDto.ProdActualQty.HasValue)
        {
            return true;
        }
        if (queryDto.DowntimeMinutes.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DowntimeReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DowntimeDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnachievedReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnachievedDescription))
        {
            return true;
        }
        if (queryDto.InputMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.ActualMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.IndirectMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.ConfirmMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.MixedProd.HasValue)
        {
            return true;
        }
        if (queryDto.AchievementRate.HasValue)
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
