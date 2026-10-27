// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMeService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制技执行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制技执行应用服务
/// </summary>
public class TaktEcMeService : TaktServiceBase, ITaktEcMeService
{
    private readonly ITaktCompanyRepository<TaktEcMe> _ecMeRepository;
    private readonly TaktEcTeStatusSynchronizer _ecTeStatusSynchronizer;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecMeRepository">设变制技执行仓储</param>
    /// <param name="ecTeStatusSynchronizer">设变技术课状态同步</param>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcMeService(
        ITaktCompanyRepository<TaktEcMe> ecMeRepository,
        TaktEcTeStatusSynchronizer ecTeStatusSynchronizer,
        TaktEcExecPersistence ecExecPersistence,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecMeRepository = ecMeRepository;
        _ecTeStatusSynchronizer = ecTeStatusSynchronizer;
        _ecExecPersistence = ecExecPersistence;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变制技执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcMeDto>> GetEcMeListAsync(TaktEcMeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecMeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcMeDto>.Create(
            data.Adapt<List<TaktEcMeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcMeDto?> GetEcMeByIdAsync(long id)
    {
        var entity = await _ecMeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcMeDto>();
        await _ecExecPersistence.FillExecViewDetailsAsync(entity, dto);
        return dto;
    }

    /// <summary>
    /// 获取设变制技执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcMeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecMeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.DeptCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变制技执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcMeDto> CreateEcMeAsync(TaktEcMeCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcMe>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_ec_me_unique = await _uniqueValidator.IsUniqueAsync(
            _ecMeRepository,
            x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_me_unique)
        {
            throw new TaktBusinessException("设变制技执行的EcDetailId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecMeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey,
                x => x.LineNumber);
            var businessCode = entity.EcCode;
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecMeRepository.CreateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcMeByIdAsync(entity.Id) ?? entity.Adapt<TaktEcMeDto>();
    }

    /// <summary>
    /// 更新设变制技执行（同设变单号+机种的执行行一并写入可填字段）
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcMeDto> UpdateEcMeAsync(long id, TaktEcMeUpdateDto dto)
    {
        var entity = await _ecMeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变制技执行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_me_unique = await _uniqueValidator.IsUniqueAsync(
            _ecMeRepository,
            x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_me_unique)
        {
            throw new TaktBusinessException("设变制技执行的EcDetailId已存在");
        }
        await _ecMeRepository.UpdateAsync(entity);
        await _ecExecPersistence.FanOutTeFillableByEcModelAndRootMaterialAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcMeByIdAsync(id) ?? throw new TaktBusinessException("设变制技执行不存在");
    }

    /// <summary>
    /// 删除设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcMeByIdAsync(long id)
    {
        var entity = await _ecMeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变制技执行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变制技执行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变制技执行已作废");
        }
        entity.IsObsolete = 1;
        await _ecMeRepository.UpdateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
    }

    /// <summary>
    /// 批量删除设变制技执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcMeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcMeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变制技执行停产状态（同步明细并自动填充/清除执行内容）
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcMeDto> UpdateEcMeDiscontinuedStatusAsync(TaktEcMeDiscontinuedStatusDto dto)
    {
        var entity = await _ecMeRepository.GetByIdAsync(dto.EcMeId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变制技执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变制技执行不存在");
        }
        var status = string.IsNullOrWhiteSpace(dto.DiscontinuedStatus)
            ? TaktEcScopeConstants.PlannedMaterialStatus
            : dto.DiscontinuedStatus.Trim();
        await _ecExecPersistence.ApplyDiscontinuedStatusForExecAsync(entity, status);
        return await GetEcMeByIdAsync(dto.EcMeId) ?? throw new TaktBusinessException("设变制技执行不存在");
    }

    /// <summary>
    /// 更新设变制技执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcMeDto> UpdateEcMeObsoleteAsync(TaktEcMeObsoleteDto dto)
    {
        var entity = await _ecMeRepository.GetByIdAsync(dto.EcMeId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变制技执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变制技执行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecMeRepository.UpdateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcMeByIdAsync(dto.EcMeId) ?? throw new TaktBusinessException("设变制技执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcMeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcMeTemplateDto>(
            sheetName ?? "设变制技执行导入模板",
            fileName ?? "设变制技执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变制技执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcMeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcMeImportDto>(fileStream, sheetName ?? "设变制技执行导入模板");
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
                var entity = rows[i].Adapt<TaktEcMe>();
                var importKey = entity.EcGrainKey;
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcDetailId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_me_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecMeRepository,
                    x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_me_unique)
                {
                    throw new TaktBusinessException("设变制技执行的EcDetailId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecMeRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey,
                        x => x.LineNumber);
                    var businessCode = entity.EcCode;
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecMeRepository.CreateAsync(entity);
                await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
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
    /// 导出设变制技执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcMeAsync(TaktEcMeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcMeQueryDto());
        var list = await _ecMeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcMeExportDto>(),
                sheetName ?? "设变制技执行数据",
                fileName ?? "设变制技执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcMeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变制技执行数据",
            fileName ?? "设变制技执行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变制技执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcMe, bool>> QueryExpression(TaktEcMeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcMe>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }
        exp = exp.And(TaktEcMeQueryHelper.VisibleExecExpression());

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcGrainKey).Contains(keywords)
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.EcRootMaterialCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || SqlFunc.ToString(x.IsSopUpdated).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SopDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcGrainKey))
        {
            exp = exp.And(x => SqlFunc.ToString(x.EcGrainKey) == queryDto.EcGrainKey);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptCode))
        {
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(queryDto.DeptCode));
        }

        // 是否实施不落库。页签按是否更新 IsSopUpdated 过滤：0=未更新，1=已更新
        if (queryDto?.IsSopUpdated.HasValue == true)
        {
            exp = exp.And(x => x.IsSopUpdated == queryDto.IsSopUpdated);
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

        if (queryDto?.SopDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SopDate >= queryDto.SopDateStart);
        }

        if (queryDto?.SopDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SopDate <= queryDto.SopDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        return exp.ToExpression();
    }
}
