// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良日报应用服务
/// </summary>
public class TaktAssyDefectService : TaktServiceBase, ITaktAssyDefectService
{
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">AssyDefectDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyDefectService(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyDefectRepository = assyDefectRepository;
        _assyDefectDetailRepository = assyDefectDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立不良日报列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyDefectDto>> GetAssyDefectListAsync(TaktAssyDefectQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAssyDefectDto>.Create(
                new List<TaktAssyDefectDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyDefectRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyDefectDto>.Create(
            data.Adapt<List<TaktAssyDefectDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto?> GetAssyDefectByIdAsync(long id)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktAssyDefectDto>();
        await FillAssyDefectDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取组立不良日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyDefectOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _assyDefectRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TeamCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TeamCode,
            DictLabel = e.TeamCode,
        }).ToList();
    }

    /// <summary>
    /// 创建组立不良日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto> CreateAssyDefectAsync(TaktAssyDefectCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyDefect>();
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectRepository,
            x => x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
        {
            throw new TaktBusinessException("组立不良日报的ProdDate、ProdOrderCode已存在");
        }
        entity = await _assyDefectRepository.CreateAsync(entity);
                await SaveAssyDefectChildrenAsync(entity, dto);
        return await GetAssyDefectByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyDefectDto>();
    }

    /// <summary>
    /// 更新组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto> UpdateAssyDefectAsync(long id, TaktAssyDefectUpdateDto dto)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectRepository,
            x => x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
        {
            throw new TaktBusinessException("组立不良日报的ProdDate、ProdOrderCode已存在");
        }
        await _assyDefectRepository.UpdateAsync(entity);
                await SaveAssyDefectChildrenAsync(entity, dto);
        return await GetAssyDefectByIdAsync(id) ?? throw new TaktBusinessException("组立不良日报不存在");
    }

    /// <summary>
    /// 删除组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectByIdAsync(long id)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良日报不存在或已删除");
        }
        await _assyDefectDetailRepository.DeleteAsync(x => x.AssyDefectId == entity.Id);
        var deleted = await _assyDefectRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立不良日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除组立不良日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyDefectByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyDefectTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyDefectTemplateDto>(
            sheetName ?? "组立不良日报导入模板",
            fileName ?? "组立不良日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立不良日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyDefectAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyDefectImportDto>(fileStream, sheetName ?? "组立不良日报导入模板");
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
                var entity = rows[i].Adapt<TaktAssyDefect>();
                var importKey = $"{entity.ProdDate}|{entity.ProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdDate、ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyDefectRepository,
                    x => x.ProdDate == entity.ProdDate
                        && x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
                {
                    throw new TaktBusinessException("组立不良日报的ProdDate、ProdOrderCode已存在");
                }
                await _assyDefectRepository.CreateAsync(entity);
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
    /// 导出组立不良日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyDefectAsync(TaktAssyDefectQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAssyDefectQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectExportDto>(),
                sheetName ?? "组立不良日报数据",
                fileName ?? "组立不良日报导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _assyDefectRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectExportDto>(),
                sheetName ?? "组立不良日报数据",
                fileName ?? "组立不良日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyDefectExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立不良日报数据",
            fileName ?? "组立不良日报导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废组立不良明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="assyDefectId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkAssyDefectDetailsObsoleteAsync(long assyDefectId)
    {
        if (assyDefectId <= 0)
        {
            return;
        }
        var rows = await _assyDefectDetailRepository.GetListAsync(
            x => x.AssyDefectId == assyDefectId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _assyDefectDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充组立不良日报详情（加载 OneToMany 子表：组立不良明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillAssyDefectDetailsAsync(TaktAssyDefectDto dto, TaktAssyDefect entity)
    {
        if (dto == null)
        {
            return;
        }
        // 组立不良明细 → dto.AssyDefectDetails（含作废行）
        var assydefectdetails = await _assyDefectDetailRepository.GetListAsync(x => x.AssyDefectId == entity.Id);
        dto.AssyDefectDetails = assydefectdetails.Adapt<List<TaktAssyDefectDetailDto>>();
    }

    /// <summary>
    /// 保存组立不良日报子表级联（组立不良明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveAssyDefectChildrenAsync(TaktAssyDefect entity, TaktAssyDefectCreateDto dto)
    {
        // 组立不良明细（AssyDefectDetails）
        List<TaktAssyDefectDetailUpdateDto>? assyDefectDetailsForSave;
        if (dto is TaktAssyDefectUpdateDto updateDtoForAssyDefectDetails && updateDtoForAssyDefectDetails.AssyDefectDetails != null)
        {
            assyDefectDetailsForSave = updateDtoForAssyDefectDetails.AssyDefectDetails;
        }
        else if (dto.AssyDefectDetails != null)
        {
            assyDefectDetailsForSave = dto.AssyDefectDetails.Adapt<List<TaktAssyDefectDetailUpdateDto>>();
        }
        else
        {
            assyDefectDetailsForSave = null;
        }
        if (assyDefectDetailsForSave is not { Count: > 0 })
        {
            await MarkAssyDefectDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _assyDefectDetailRepository.GetListAsync(x => x.AssyDefectId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktAssyDefectDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < assyDefectDetailsForSave.Count; i++)
            {
                var childDto = assyDefectDetailsForSave[i];
                childDto.AssyDefectId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ProdOrderCode = entity.ProdOrderCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("组立不良明细第{i + 1}项与本次提交的其他项重复（CompanyCode、AssyDefectId、LineNumber）");
                }
                if (childDto.AssyDefectDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.AssyDefectDetailId, out var target))
                    {
                        throw new TaktBusinessException("组立不良明细不存在（AssyDefectDetailId={childDto.AssyDefectDetailId}）");
                    }
                    if (target.AssyDefectId != entity.Id)
                    {
                        throw new TaktBusinessException("组立不良明细不属于当前主表（AssyDefectDetailId={childDto.AssyDefectDetailId}）");
                    }
                    submittedIds.Add(childDto.AssyDefectDetailId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _assyDefectDetailRepository,
                        x => x.AssyDefectId == x.AssyDefectId
                && x.LineNumber == x.LineNumber,
                        childDto.AssyDefectDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
                    {
                        throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.AssyDefectDetailId;
                    target.AssyDefectId = entity.Id;
                    target.IsObsolete = 0;
                    await _assyDefectDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _assyDefectDetailRepository,
                        x => x.AssyDefectId == x.AssyDefectId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
                    {
                        throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktAssyDefectDetail>();
                    child.Id = 0;
                    child.AssyDefectId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _assyDefectDetailRepository.UpdateAsync(removed);
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
                await _assyDefectDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立不良日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyDefect, bool>> QueryExpression(TaktAssyDefectQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyDefect>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdCategory))
        {
            var prodCategory = queryDto.ProdCategory;
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(prodCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCode))
        {
            var teamCode = queryDto.TeamCode;
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(teamCode));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            var shiftNo = queryDto.ShiftNo.Value;
            exp = exp.And(x => x.ShiftNo == shiftNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderType))
        {
            var prodOrderType = queryDto.ProdOrderType;
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(prodOrderType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            var prodOrderQty = queryDto.ProdOrderQty.Value;
            exp = exp.And(x => x.ProdOrderQty == prodOrderQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            var prodActualQty = queryDto.ProdActualQty.Value;
            exp = exp.And(x => x.ProdActualQty == prodActualQty);
        }

        if (queryDto?.GoodQuantity.HasValue == true)
        {
            var goodQuantity = queryDto.GoodQuantity.Value;
            exp = exp.And(x => x.GoodQuantity == goodQuantity);
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

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            var prodDateStart = queryDto.ProdDateStart.Value;
            exp = exp.And(x => x.ProdDate >= prodDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            var prodDateEnd = queryDto.ProdDateEnd.Value;
            exp = exp.And(x => x.ProdDate <= prodDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktAssyDefectQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ProdCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCode))
        {
            return true;
        }
        if (queryDto.ShiftNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
        {
            return true;
        }
        if (queryDto.ProdOrderQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.ProdActualQty.HasValue)
        {
            return true;
        }
        if (queryDto.GoodQuantity.HasValue)
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
        if (queryDto.ProdDateStart.HasValue || queryDto.ProdDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    // ========================================
    // 扩展方法（数据看板）
    // ========================================

    /// <summary>
    /// 获取组立不良统计（数据看板 defect-stat；按生产日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>组立不良统计</returns>
    public async Task<TaktAssyDefectStatDto> GetAssyDefectStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktAssyDefect, bool>> headerPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var headers = await _assyDefectRepository.GetListAsync(headerPredicate);
        var defectIds = headers.Select(h => h.Id).ToList();
        var details = defectIds.Count == 0
            ? new List<TaktAssyDefectDetail>()
            : await _assyDefectDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && defectIds.Contains(x.AssyDefectId));
        var defectQtyByHeaderId = details
            .GroupBy(d => d.AssyDefectId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.DefectQty));
        var teams = headers
            .GroupBy(h => h.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var baseQty = g.Sum(h => h.ProdActualQty);
                var goodQty = g.Sum(h => h.GoodQuantity);
                var defectQty = g.Sum(h => defectQtyByHeaderId.TryGetValue(h.Id, out var qty) ? qty : 0m);
                return new TaktDefectStatTeamItemDto
                {
                    TeamCode = g.Key,
                    BaseQty = baseQty,
                    GoodQty = goodQty,
                    DefectQty = defectQty,
                    DefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(defectQty, baseQty),
                    YieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(goodQty, baseQty),
                };
            })
            .ToList();
        var monthBaseQty = teams.Sum(t => t.BaseQty);
        var monthGoodQty = teams.Sum(t => t.GoodQty);
        var monthDefectQty = teams.Sum(t => t.DefectQty);
        return new TaktAssyDefectStatDto
        {
            StatMonth = statMonth,
            MonthBaseQty = monthBaseQty,
            MonthGoodQty = monthGoodQty,
            MonthDefectQty = monthDefectQty,
            MonthDefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(monthDefectQty, monthBaseQty),
            MonthYieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(monthGoodQty, monthBaseQty),
            Teams = teams,
        };
    }
}
