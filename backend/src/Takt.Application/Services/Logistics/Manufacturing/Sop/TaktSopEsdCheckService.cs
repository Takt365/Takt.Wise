// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopEsdCheckService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP ESD检查应用服务实现
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
/// SOP ESD检查应用服务
/// </summary>
public class TaktSopEsdCheckService : TaktServiceBase, ITaktSopEsdCheckService
{
    private readonly ITaktCompanyRepository<TaktSopEsdCheck> _sopEsdCheckRepository;
    private readonly ITaktCompanyRepository<TaktSopWorkstation> _sopWorkstationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopEsdCheckRepository">SOP ESD检查仓储</param>
    /// <param name="sopWorkstationRepository">SOP工位主数据仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopEsdCheckService(
        ITaktCompanyRepository<TaktSopEsdCheck> sopEsdCheckRepository,
        ITaktCompanyRepository<TaktSopWorkstation> sopWorkstationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopEsdCheckRepository = sopEsdCheckRepository;
        _sopWorkstationRepository = sopWorkstationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP ESD检查列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopEsdCheckDto>> GetSopEsdCheckListAsync(TaktSopEsdCheckQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopEsdCheckRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopEsdCheckDto>.Create(
            data.Adapt<List<TaktSopEsdCheckDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopEsdCheckDto?> GetSopEsdCheckByIdAsync(long id)
    {
        var entity = await _sopEsdCheckRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopEsdCheckDto>();
    }

    /// <summary>
    /// 获取SOP ESD检查选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopEsdCheckOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopEsdCheckRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeviceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeviceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP ESD检查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopEsdCheckDto> CreateSopEsdCheckAsync(TaktSopEsdCheckCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopEsdCheck>();
        await StampSopEsdCheckSopWorkstationAsync(entity, dto);
        entity = await _sopEsdCheckRepository.CreateAsync(entity);
        return await GetSopEsdCheckByIdAsync(entity.Id) ?? entity.Adapt<TaktSopEsdCheckDto>();
    }

    /// <summary>
    /// 更新SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopEsdCheckDto> UpdateSopEsdCheckAsync(long id, TaktSopEsdCheckUpdateDto dto)
    {
        var entity = await _sopEsdCheckRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP ESD检查不存在");
        }
        dto.Adapt(entity);
        await StampSopEsdCheckSopWorkstationAsync(entity, dto);
        await _sopEsdCheckRepository.UpdateAsync(entity);
        return await GetSopEsdCheckByIdAsync(id) ?? throw new TaktBusinessException("SOP ESD检查不存在");
    }

    /// <summary>
    /// 删除SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopEsdCheckByIdAsync(long id)
    {
        var deleted = await _sopEsdCheckRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP ESD检查不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP ESD检查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopEsdCheckBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopEsdCheckByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopEsdCheckTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopEsdCheckTemplateDto>(
            sheetName ?? "SOP ESD检查导入模板",
            fileName ?? "SOP ESD检查导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP ESD检查
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopEsdCheckAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopEsdCheckImportDto>(fileStream, sheetName ?? "SOP ESD检查导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopEsdCheck>();
                var importDto = rows[i].Adapt<TaktSopEsdCheckCreateDto>();
                await StampSopEsdCheckSopWorkstationAsync(entity, importDto);
                await _sopEsdCheckRepository.CreateAsync(entity);
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
    /// 导出SOP ESD检查
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopEsdCheckAsync(TaktSopEsdCheckQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopEsdCheckQueryDto());
        var list = await _sopEsdCheckRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopEsdCheckExportDto>(),
                sheetName ?? "SOP ESD检查数据",
                fileName ?? "SOP ESD检查导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopEsdCheckExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP ESD检查数据",
            fileName ?? "SOP ESD检查导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP ESD检查主表外键（ManyToOne → SOP工位主数据）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopEsdCheckSopWorkstationAsync(TaktSopEsdCheck entity, TaktSopEsdCheckCreateDto dto)
    {
        if (dto.WorkstationId <= 0)
        {
            return;
        }
        var master = await _sopWorkstationRepository.GetByIdAsync(dto.WorkstationId);
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
    /// 构建SOP ESD检查查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopEsdCheck, bool>> QueryExpression(TaktSopEsdCheckQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopEsdCheck>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.WorkstationId).Contains(keywords)
                || SqlFunc.ToString(x.ExecId).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.DeviceCode != null && x.DeviceCode.Contains(keywords))
                || SqlFunc.ToString(x.ResistanceValue).Contains(keywords)
                || SqlFunc.ToString(x.IsCompliant).Contains(keywords)
                || SqlFunc.ToString(x.LockScreenTriggered).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CheckedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationId == queryDto.WorkstationId);
        }

        if (queryDto?.ExecId.HasValue == true)
        {
            exp = exp.And(x => x.ExecId == queryDto.ExecId);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeviceCode))
        {
            exp = exp.And(x => x.DeviceCode != null && x.DeviceCode.Contains(queryDto.DeviceCode));
        }

        if (queryDto?.ResistanceValue.HasValue == true)
        {
            exp = exp.And(x => x.ResistanceValue == queryDto.ResistanceValue);
        }

        if (queryDto?.IsCompliant.HasValue == true)
        {
            exp = exp.And(x => x.IsCompliant == queryDto.IsCompliant);
        }

        if (queryDto?.LockScreenTriggered.HasValue == true)
        {
            exp = exp.And(x => x.LockScreenTriggered == queryDto.LockScreenTriggered);
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

        if (queryDto?.CheckedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CheckedAt >= queryDto.CheckedAtStart);
        }

        if (queryDto?.CheckedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CheckedAt <= queryDto.CheckedAtEnd);
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
