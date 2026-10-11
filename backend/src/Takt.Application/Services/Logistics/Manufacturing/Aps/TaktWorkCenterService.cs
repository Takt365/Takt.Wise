// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 工作中心应用服务
/// </summary>
public class TaktWorkCenterService : TaktServiceBase, ITaktWorkCenterService
{
    private readonly ITaktCompanyRepository<TaktWorkCenter> _workCenterRepository;
    private readonly ITaktCompanyRepository<TaktWorkCenterResource> _workCenterResourceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workCenterRepository">工作中心仓储</param>
    /// <param name="workCenterResourceRepository">WorkCenterResource仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktWorkCenterService(
        ITaktCompanyRepository<TaktWorkCenter> workCenterRepository,
        ITaktCompanyRepository<TaktWorkCenterResource> workCenterResourceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workCenterRepository = workCenterRepository;
        _workCenterResourceRepository = workCenterResourceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工作中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktWorkCenterDto>> GetWorkCenterListAsync(TaktWorkCenterQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktWorkCenterDto>.Create(
                new List<TaktWorkCenterDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _workCenterRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktWorkCenterDto>.Create(
            data.Adapt<List<TaktWorkCenterDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto?> GetWorkCenterByIdAsync(long id)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktWorkCenterDto>();
        await FillWorkCenterDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工作中心选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetWorkCenterOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _workCenterRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WorkCenterStatus == 1,
            x => x.WorkCenterCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkCenterCode,
            DictLabel = e.WorkCenterCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工作中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> CreateWorkCenterAsync(TaktWorkCenterCreateDto dto)
    {
        var entity = dto.Adapt<TaktWorkCenter>();
        var isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique)
        {
            throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
        }
        entity = await _workCenterRepository.CreateAsync(entity);
                await SaveWorkCenterChildrenAsync(entity, dto);
        return await GetWorkCenterByIdAsync(entity.Id) ?? entity.Adapt<TaktWorkCenterDto>();
    }

    /// <summary>
    /// 更新工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> UpdateWorkCenterAsync(long id, TaktWorkCenterUpdateDto dto)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique)
        {
            throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
        }
        await _workCenterRepository.UpdateAsync(entity);
                await SaveWorkCenterChildrenAsync(entity, dto);
        return await GetWorkCenterByIdAsync(id) ?? throw new TaktBusinessException("工作中心不存在");
    }

    /// <summary>
    /// 删除工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterByIdAsync(long id)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在或已删除");
        }
        await _workCenterResourceRepository.DeleteAsync(x => x.WorkCenterId == entity.Id);
        var deleted = await _workCenterRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工作中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工作中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteWorkCenterByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工作中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> UpdateWorkCenterStatusAsync(TaktWorkCenterStatusDto dto)
    {
        var entity = await _workCenterRepository.GetByIdAsync(dto.WorkCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在");
        }
        entity.WorkCenterStatus = dto.WorkCenterStatus;
        await _workCenterRepository.UpdateAsync(entity);
        return await GetWorkCenterByIdAsync(dto.WorkCenterId) ?? throw new TaktBusinessException("工作中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetWorkCenterTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktWorkCenterTemplateDto>(
            sheetName ?? "工作中心导入模板",
            fileName ?? "工作中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入工作中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportWorkCenterAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktWorkCenterImportDto>(fileStream, sheetName ?? "工作中心导入模板");
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
                var entity = rows[i].Adapt<TaktWorkCenter>();
                var importKey = $"{entity.PlantCode}|{entity.WorkCenterCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WorkCenterCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique = await _uniqueValidator.IsUniqueAsync(
                    _workCenterRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WorkCenterCode == entity.WorkCenterCode);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_work_center_unique)
                {
                    throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
                }
                await _workCenterRepository.CreateAsync(entity);
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
    /// 导出工作中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportWorkCenterAsync(TaktWorkCenterQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktWorkCenterQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWorkCenterExportDto>(),
                sheetName ?? "工作中心数据",
                fileName ?? "工作中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _workCenterRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWorkCenterExportDto>(),
                sheetName ?? "工作中心数据",
                fileName ?? "工作中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktWorkCenterExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工作中心数据",
            fileName ?? "工作中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工作中心详情（加载 OneToMany 子表：工作中心资源）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillWorkCenterDetailsAsync(TaktWorkCenterDto dto, TaktWorkCenter entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工作中心资源 → dto.Resources
        var resources = await _workCenterResourceRepository.GetListAsync(x => x.WorkCenterId == entity.Id);
        dto.Resources = resources.Adapt<List<TaktWorkCenterResourceDto>>();
    }

    /// <summary>
    /// 保存工作中心子表级联（工作中心资源；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveWorkCenterChildrenAsync(TaktWorkCenter entity, TaktWorkCenterCreateDto dto)
    {
        // 工作中心资源（Resources）
        List<TaktWorkCenterResourceUpdateDto>? resourcesForSave;
        if (dto is TaktWorkCenterUpdateDto updateDtoForResources && updateDtoForResources.Resources != null)
        {
            resourcesForSave = updateDtoForResources.Resources;
        }
        else if (dto.Resources != null)
        {
            resourcesForSave = dto.Resources.Adapt<List<TaktWorkCenterResourceUpdateDto>>();
        }
        else
        {
            resourcesForSave = null;
        }
        if (resourcesForSave is not { Count: > 0 })
        {
            await _workCenterResourceRepository.DeleteAsync(x => x.WorkCenterId == entity.Id);
        }
        else
        {
            var existingList = await _workCenterResourceRepository.GetListAsync(x => x.WorkCenterId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktWorkCenterResource>();
            for (var i = 0; i < resourcesForSave.Count; i++)
            {
                var childDto = resourcesForSave[i];
                childDto.WorkCenterId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.WorkCenterCode = entity.WorkCenterCode;
                if (childDto.WorkCenterResourceId > 0)
                {
                    if (!existingById.TryGetValue(childDto.WorkCenterResourceId, out var target))
                    {
                        throw new TaktBusinessException("工作中心资源不存在（WorkCenterResourceId={childDto.WorkCenterResourceId}）");
                    }
                    if (target.WorkCenterId != entity.Id)
                    {
                        throw new TaktBusinessException("工作中心资源不属于当前主表（WorkCenterResourceId={childDto.WorkCenterResourceId}）");
                    }
                    submittedIds.Add(childDto.WorkCenterResourceId);
                    childDto.Adapt(target);
                    target.Id = childDto.WorkCenterResourceId;
                    target.WorkCenterId = entity.Id;
                    await _workCenterResourceRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktWorkCenterResource>();
                    child.Id = 0;
                    child.WorkCenterId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _workCenterResourceRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _workCenterResourceRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工作中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktWorkCenter, bool>> QueryExpression(TaktWorkCenterQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktWorkCenter>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.WorkCenterDescription != null && x.WorkCenterDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenterCode))
        {
            var workCenterCode = queryDto.WorkCenterCode;
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(workCenterCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenterDescription))
        {
            var workCenterDescription = queryDto.WorkCenterDescription;
            exp = exp.And(x => x.WorkCenterDescription != null && x.WorkCenterDescription.Contains(workCenterDescription));
        }

        if (queryDto?.WorkCenterStatus.HasValue == true)
        {
            var workCenterStatus = queryDto.WorkCenterStatus.Value;
            exp = exp.And(x => x.WorkCenterStatus == workCenterStatus);
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
    private static bool HasAnyListQueryFilter(TaktWorkCenterQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenterCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenterDescription))
        {
            return true;
        }
        if (queryDto.WorkCenterStatus.HasValue)
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
