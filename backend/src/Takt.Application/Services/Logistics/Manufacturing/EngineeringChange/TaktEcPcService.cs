// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcPcService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变生管执行应用服务实现
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
/// 设变生管执行应用服务
/// </summary>
public class TaktEcPcService : TaktServiceBase, ITaktEcPcService
{
    private readonly ITaktCompanyRepository<TaktEcPc> _ecPcRepository;
    private readonly TaktEcTeStatusSynchronizer _ecTeStatusSynchronizer;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecPcRepository">设变生管执行仓储</param>
    /// <param name="ecTeStatusSynchronizer">设变技术课状态同步</param>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcPcService(
        ITaktCompanyRepository<TaktEcPc> ecPcRepository,
        TaktEcTeStatusSynchronizer ecTeStatusSynchronizer,
        TaktEcExecPersistence ecExecPersistence,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecPcRepository = ecPcRepository;
        _ecTeStatusSynchronizer = ecTeStatusSynchronizer;
        _ecExecPersistence = ecExecPersistence;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变生管执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcPcDto>> GetEcPcListAsync(TaktEcPcQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecPcRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcPcDto>.Create(
            data.Adapt<List<TaktEcPcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变生管执行
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcPcDto?> GetEcPcByIdAsync(long id)
    {
        var entity = await _ecPcRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcPcDto>();
        await _ecExecPersistence.FillExecViewDetailsAsync(entity, dto);
        return dto;
    }

    /// <summary>
    /// 获取设变生管执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcPcOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecPcRepository.GetListAsync(
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
    /// 创建设变生管执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcPcDto> CreateEcPcAsync(TaktEcPcCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcPc>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_ec_pc_unique = await _uniqueValidator.IsUniqueAsync(
            _ecPcRepository,
            x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey
);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_pc_unique)
        {
            throw new TaktBusinessException("设变生管执行的EcDetailId、EcRootMaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecPcRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey,
                x => x.LineNumber);
            var businessCode = entity.EcCode;
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecPcRepository.CreateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcPcByIdAsync(entity.Id) ?? entity.Adapt<TaktEcPcDto>();
    }

    /// <summary>
    /// 更新设变生管执行（同去重键执行行一并写入可填字段；旧品处理按表单字典值保存）
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcPcDto> UpdateEcPcAsync(long id, TaktEcPcUpdateDto dto)
    {
        var entity = await _ecPcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        dto.ScheduledBatch = TaktStringHelper.NormalizeBatchCode(dto.ScheduledBatch);
        if (!string.IsNullOrWhiteSpace(dto.PoRemainder))
        {
            dto.PoRemainder = dto.PoRemainder.Trim();
        }
        if (!TaktRegexHelper.IsValidEcPoRemainder(dto.PoRemainder))
        {
            throw new TaktBusinessException("Po残须为空或「采购单10位-序号4位:数量」，如 4500123456-0010:18.5");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_pc_unique = await _uniqueValidator.IsUniqueAsync(
            _ecPcRepository,
            x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey
,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_pc_unique)
        {
            throw new TaktBusinessException("设变生管执行的EcDetailId、EcRootMaterialCode已存在");
        }
        await _ecPcRepository.UpdateAsync(entity);
        await _ecExecPersistence.FanOutPcFillableByEcModelAndRootMaterialAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcPcByIdAsync(id) ?? throw new TaktBusinessException("设变生管执行不存在");
    }

    /// <summary>
    /// 删除设变生管执行
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcPcByIdAsync(long id)
    {
        var entity = await _ecPcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变生管执行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变生管执行已作废");
        }
        entity.IsObsolete = 1;
        await _ecPcRepository.UpdateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
    }

    /// <summary>
    /// 批量删除设变生管执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcPcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcPcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变生管执行停产状态（同步明细并自动填充/清除执行内容）
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcPcDto> UpdateEcPcDiscontinuedStatusAsync(TaktEcPcDiscontinuedStatusDto dto)
    {
        var entity = await _ecPcRepository.GetByIdAsync(dto.EcPcId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        var status = string.IsNullOrWhiteSpace(dto.DiscontinuedStatus)
            ? TaktEcScopeConstants.PlannedMaterialStatus
            : dto.DiscontinuedStatus.Trim();
        await _ecExecPersistence.ApplyDiscontinuedStatusForExecAsync(entity, status);
        return await GetEcPcByIdAsync(dto.EcPcId) ?? throw new TaktBusinessException("设变生管执行不存在");
    }

    /// <summary>
    /// 更新设变生管执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcPcDto> UpdateEcPcObsoleteAsync(TaktEcPcObsoleteDto dto)
    {
        var entity = await _ecPcRepository.GetByIdAsync(dto.EcPcId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecPcRepository.UpdateAsync(entity);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcPcByIdAsync(dto.EcPcId) ?? throw new TaktBusinessException("设变生管执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcPcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcPcTemplateDto>(
            sheetName ?? "设变生管执行导入模板",
            fileName ?? "设变生管执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变生管执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcPcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcPcImportDto>(fileStream, sheetName ?? "设变生管执行导入模板");
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
                var entity = rows[i].Adapt<TaktEcPc>();
                var importKey = entity.EcGrainKey;
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcDetailId、EcRootMaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_pc_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecPcRepository,
                    x => x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey
        );
                if (!isUnique_ix_takt_logistics_manufacturing_ec_pc_unique)
                {
                    throw new TaktBusinessException("设变生管执行的EcDetailId、EcRootMaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecPcRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcCode == entity.EcCode && SqlFunc.ToString(x.EcGrainKey) == entity.EcGrainKey,
                        x => x.LineNumber);
                    var businessCode = entity.EcCode;
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecPcRepository.CreateAsync(entity);
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
    /// 导出设变生管执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcPcAsync(TaktEcPcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcPcQueryDto());
        var list = await _ecPcRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcPcExportDto>(),
                sheetName ?? "设变生管执行数据",
                fileName ?? "设变生管执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcPcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变生管执行数据",
            fileName ?? "设变生管执行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变生管执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcPc, bool>> QueryExpression(TaktEcPcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcPc>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }
        exp = exp.And(TaktEcPcQueryHelper.VisibleExecExpression());

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcGrainKey).Contains(keywords)
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.EcModelCodeGroup).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || SqlFunc.ToString(x.IsImplemented).Contains(keywords)
                || (x.ExecContent != null && x.ExecContent.Contains(keywords))
                || (x.ScheduledBatch != null && x.ScheduledBatch.Contains(keywords))
                || (x.PoRemainder != null && x.PoRemainder.Contains(keywords))
                || SqlFunc.ToString(x.BalanceQuantity).Contains(keywords)
                || (x.EcOldPartDisposition != null && x.EcOldPartDisposition.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScheduledDate).Contains(keywords)
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

        if (queryDto?.IsImplemented.HasValue == true)
        {
            exp = exp.And(x => x.IsImplemented == queryDto.IsImplemented);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecContent))
        {
            exp = exp.And(x => x.ExecContent != null && x.ExecContent.Contains(queryDto.ExecContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScheduledBatch))
        {
            exp = exp.And(x => x.ScheduledBatch != null && x.ScheduledBatch.Contains(queryDto.ScheduledBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.PoRemainder))
        {
            exp = exp.And(x => x.PoRemainder != null && x.PoRemainder.Contains(queryDto.PoRemainder));
        }

        if (queryDto?.BalanceQuantity.HasValue == true)
        {
            exp = exp.And(x => x.BalanceQuantity == queryDto.BalanceQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldPartDisposition))
        {
            exp = exp.And(x => x.EcOldPartDisposition != null && x.EcOldPartDisposition.Contains(queryDto.EcOldPartDisposition));
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

        if (queryDto?.ScheduledDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledDate >= queryDto.ScheduledDateStart);
        }

        if (queryDto?.ScheduledDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledDate <= queryDto.ScheduledDateEnd);
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
