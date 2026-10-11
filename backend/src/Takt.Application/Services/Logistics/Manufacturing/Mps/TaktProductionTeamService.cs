// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组应用服务
/// </summary>
public class TaktProductionTeamService : TaktServiceBase, ITaktProductionTeamService
{
    private readonly ITaktCompanyRepository<TaktProductionTeam> _productionTeamRepository;
    private readonly ITaktCompanyRepository<TaktProductionTeamEquipment> _productionTeamEquipmentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionTeamRepository">生产班组仓储</param>
    /// <param name="productionTeamEquipmentRepository">ProductionTeamEquipment仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionTeamService(
        ITaktCompanyRepository<TaktProductionTeam> productionTeamRepository,
        ITaktCompanyRepository<TaktProductionTeamEquipment> productionTeamEquipmentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionTeamRepository = productionTeamRepository;
        _productionTeamEquipmentRepository = productionTeamEquipmentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产班组列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionTeamDto>> GetProductionTeamListAsync(TaktProductionTeamQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktProductionTeamDto>.Create(
                new List<TaktProductionTeamDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionTeamRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionTeamDto>.Create(
            data.Adapt<List<TaktProductionTeamDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto?> GetProductionTeamByIdAsync(long id)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktProductionTeamDto>();
        await FillProductionTeamDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取生产班组选项列表（DictValue=TeamCode，DictLabel=TeamCode-TeamName，ExtValue=PlantCode；TeamCode 自然序）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="teamCategory">班组分类（字典 logistics_manufacturing_team_category；有值时精确匹配，如 A=组立 P=PCBA）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionTeamOptionsAsync(string? plantCode = null, string? keyword = null, string? teamCategory = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var category = teamCategory?.Trim();
        var predicate = Expressionable.Create<TaktProductionTeam>()
            .And(x => x.TenantCode == CurrentTenantCode)
            .And(x => x.CompanyCode == CurrentCompanyCode)
            .And(x => x.TeamStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(category), x => x.TeamCategory == category)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.TeamCode != null && x.TeamCode.Contains(normalizedKeyword!))
                || (x.TeamName != null && x.TeamName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _productionTeamRepository.GetListAsync(predicate);
        return list
            .OrderBy(e => e.TeamCode, Comparer<string>.Create(TaktStringHelper.CompareNatural))
            .ThenBy(e => e.TeamName, StringComparer.OrdinalIgnoreCase)
            .Select(e => new TaktSelectOption
            {
                DictValue = e.TeamCode,
                DictLabel = $"{e.TeamCode}-{e.TeamName}",
                ExtValue = e.PlantCode,
                ExtLabel = e.TeamCategory,
            }).ToList();
    }

    /// <summary>
    /// 创建生产班组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> CreateProductionTeamAsync(TaktProductionTeamCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionTeam>();
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TeamCategory == entity.TeamCategory);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique)
        {
            throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
        }
        entity = await _productionTeamRepository.CreateAsync(entity);
                await SaveProductionTeamChildrenAsync(entity, dto);
        return await GetProductionTeamByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionTeamDto>();
    }

    /// <summary>
    /// 更新生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> UpdateProductionTeamAsync(long id, TaktProductionTeamUpdateDto dto)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TeamCategory == entity.TeamCategory,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique)
        {
            throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
        }
        await _productionTeamRepository.UpdateAsync(entity);
                await SaveProductionTeamChildrenAsync(entity, dto);
        return await GetProductionTeamByIdAsync(id) ?? throw new TaktBusinessException("生产班组不存在");
    }

    /// <summary>
    /// 删除生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamByIdAsync(long id)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组不存在或已删除");
        }
        await _productionTeamEquipmentRepository.DeleteAsync(x => x.ProdTeamId == entity.Id);
        var deleted = await _productionTeamRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产班组不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产班组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionTeamByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产班组状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> UpdateProductionTeamStatusAsync(TaktProductionTeamStatusDto dto)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(dto.ProductionTeamId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组不存在");
        }
        entity.TeamStatus = dto.TeamStatus;
        await _productionTeamRepository.UpdateAsync(entity);
        return await GetProductionTeamByIdAsync(dto.ProductionTeamId) ?? throw new TaktBusinessException("生产班组不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionTeamTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionTeamTemplateDto>(
            sheetName ?? "生产班组导入模板",
            fileName ?? "生产班组导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产班组
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionTeamAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionTeamImportDto>(fileStream, sheetName ?? "生产班组导入模板");
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
                var entity = rows[i].Adapt<TaktProductionTeam>();
                var importKey = $"{entity.PlantCode}|{entity.TeamCode}|{entity.TeamCategory}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、TeamCode、TeamCategory）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionTeamRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.TeamCode == entity.TeamCode
                        && x.TeamCategory == entity.TeamCategory);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_team_unique)
                {
                    throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
                }
                await _productionTeamRepository.CreateAsync(entity);
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
    /// 导出生产班组
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionTeamAsync(TaktProductionTeamQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktProductionTeamQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionTeamExportDto>(),
                sheetName ?? "生产班组数据",
                fileName ?? "生产班组导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _productionTeamRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionTeamExportDto>(),
                sheetName ?? "生产班组数据",
                fileName ?? "生产班组导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionTeamExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产班组数据",
            fileName ?? "生产班组导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废生产班组设备组标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="prodTeamId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkProductionTeamEquipmentsObsoleteAsync(long prodTeamId)
    {
        if (prodTeamId <= 0)
        {
            return;
        }
        var rows = await _productionTeamEquipmentRepository.GetListAsync(
            x => x.ProdTeamId == prodTeamId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _productionTeamEquipmentRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充生产班组详情（加载 OneToMany 子表：生产班组设备组）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillProductionTeamDetailsAsync(TaktProductionTeamDto dto, TaktProductionTeam entity)
    {
        if (dto == null)
        {
            return;
        }
        // 生产班组设备组 → dto.TeamEquipmentList（含作废行）
        var teamequipmentlist = await _productionTeamEquipmentRepository.GetListAsync(x => x.ProdTeamId == entity.Id);
        dto.TeamEquipmentList = teamequipmentlist.Adapt<List<TaktProductionTeamEquipmentDto>>();
    }

    /// <summary>
    /// 保存生产班组子表级联（生产班组设备组；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveProductionTeamChildrenAsync(TaktProductionTeam entity, TaktProductionTeamCreateDto dto)
    {
        // 生产班组设备组（TeamEquipmentList）
        List<TaktProductionTeamEquipmentUpdateDto>? teamEquipmentListForSave;
        if (dto is TaktProductionTeamUpdateDto updateDtoForTeamEquipmentList && updateDtoForTeamEquipmentList.TeamEquipmentList != null)
        {
            teamEquipmentListForSave = updateDtoForTeamEquipmentList.TeamEquipmentList;
        }
        else if (dto.TeamEquipmentList != null)
        {
            teamEquipmentListForSave = dto.TeamEquipmentList.Adapt<List<TaktProductionTeamEquipmentUpdateDto>>();
        }
        else
        {
            teamEquipmentListForSave = null;
        }
        if (teamEquipmentListForSave is not { Count: > 0 })
        {
            await MarkProductionTeamEquipmentsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _productionTeamEquipmentRepository.GetListAsync(x => x.ProdTeamId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktProductionTeamEquipment>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < teamEquipmentListForSave.Count; i++)
            {
                var childDto = teamEquipmentListForSave[i];
                childDto.ProdTeamId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.TeamCode = entity.TeamCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("生产班组设备组第{i + 1}项与本次提交的其他项重复（CompanyCode、ProdTeamId、LineNumber）");
                }
                if (childDto.ProductionTeamEquipmentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ProductionTeamEquipmentId, out var target))
                    {
                        throw new TaktBusinessException("生产班组设备组不存在（ProductionTeamEquipmentId={childDto.ProductionTeamEquipmentId}）");
                    }
                    if (target.ProdTeamId != entity.Id)
                    {
                        throw new TaktBusinessException("生产班组设备组不属于当前主表（ProductionTeamEquipmentId={childDto.ProductionTeamEquipmentId}）");
                    }
                    submittedIds.Add(childDto.ProductionTeamEquipmentId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _productionTeamEquipmentRepository,
                        x => x.ProdTeamId == x.ProdTeamId
                && x.LineNumber == x.LineNumber
                && x.ProdEquipCode == x.ProdEquipCode,
                        childDto.ProductionTeamEquipmentId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique)
                    {
                        throw new TaktBusinessException("生产班组设备组的ProdTeamId、LineNumber、ProdEquipCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.ProductionTeamEquipmentId;
                    target.ProdTeamId = entity.Id;
                    target.IsObsolete = 0;
                    await _productionTeamEquipmentRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _productionTeamEquipmentRepository,
                        x => x.ProdTeamId == x.ProdTeamId
                && x.LineNumber == x.LineNumber
                && x.ProdEquipCode == x.ProdEquipCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique)
                    {
                        throw new TaktBusinessException("生产班组设备组的ProdTeamId、LineNumber、ProdEquipCode已存在");
                    }
                    var child = childDto.Adapt<TaktProductionTeamEquipment>();
                    child.Id = 0;
                    child.ProdTeamId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _productionTeamEquipmentRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.TeamCode) ? entity.TeamCode : entity.Id.ToString();
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
                await _productionTeamEquipmentRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产班组查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionTeam, bool>> QueryExpression(TaktProductionTeamQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionTeam>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.TeamName != null && x.TeamName.Contains(keywords))
                || (x.TeamCategory != null && x.TeamCategory.Contains(keywords))
                || (x.TeamLeaderName != null && x.TeamLeaderName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCode))
        {
            var teamCode = queryDto.TeamCode;
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(teamCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamName))
        {
            var teamName = queryDto.TeamName;
            exp = exp.And(x => x.TeamName != null && x.TeamName.Contains(teamName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCategory))
        {
            var teamCategory = queryDto.TeamCategory;
            exp = exp.And(x => x.TeamCategory != null && x.TeamCategory.Contains(teamCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamLeaderName))
        {
            var teamLeaderName = queryDto.TeamLeaderName;
            exp = exp.And(x => x.TeamLeaderName != null && x.TeamLeaderName.Contains(teamLeaderName));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            var shiftNo = queryDto.ShiftNo.Value;
            exp = exp.And(x => x.ShiftNo == shiftNo);
        }

        if (queryDto?.TeamStatus.HasValue == true)
        {
            var teamStatus = queryDto.TeamStatus.Value;
            exp = exp.And(x => x.TeamStatus == teamStatus);
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
    private static bool HasAnyListQueryFilter(TaktProductionTeamQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamLeaderName))
        {
            return true;
        }
        if (queryDto.ShiftNo.HasValue)
        {
            return true;
        }
        if (queryDto.TeamStatus.HasValue)
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
