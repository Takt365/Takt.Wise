// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktOvertimeService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：加班信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 加班信息应用服务
/// </summary>
public class TaktOvertimeService : TaktServiceBase, ITaktOvertimeService
{
    private readonly ITaktApprovalRepository<TaktOvertime> _overtimeRepository;
    private readonly ITaktCompanyRepository<TaktOvertimeItem> _overtimeItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overtimeRepository">加班信息仓储</param>
    /// <param name="overtimeItemRepository">OvertimeItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOvertimeService(
        ITaktApprovalRepository<TaktOvertime> overtimeRepository,
        ITaktCompanyRepository<TaktOvertimeItem> overtimeItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _overtimeRepository = overtimeRepository;
        _overtimeItemRepository = overtimeItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取加班信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOvertimeDto>> GetOvertimeListAsync(TaktOvertimeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktOvertimeDto>.Create(
                new List<TaktOvertimeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _overtimeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktOvertimeDto>.Create(
            data.Adapt<List<TaktOvertimeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto?> GetOvertimeByIdAsync(long id)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktOvertimeDto>();
        await FillOvertimeDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取加班信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOvertimeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _overtimeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OvertimeStatus == 1,
            x => x.DeptName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DeptName ?? string.Empty,
            DictLabel = e.DeptName ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建加班信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> CreateOvertimeAsync(TaktOvertimeCreateDto dto)
    {
        var entity = dto.Adapt<TaktOvertime>();
        var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeRepository,
            x => x.DeptId == entity.DeptId
                && x.OvertimeDate == entity.OvertimeDate);
        if (!isUnique_ix_overtime_dept_date_unique)
        {
            throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
        }
        entity = await _overtimeRepository.CreateAsync(entity);
                await SaveOvertimeChildrenAsync(entity, dto);
        return await GetOvertimeByIdAsync(entity.Id) ?? entity.Adapt<TaktOvertimeDto>();
    }

    /// <summary>
    /// 更新加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> UpdateOvertimeAsync(long id, TaktOvertimeUpdateDto dto)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeRepository,
            x => x.DeptId == entity.DeptId
                && x.OvertimeDate == entity.OvertimeDate,
            id);
        if (!isUnique_ix_overtime_dept_date_unique)
        {
            throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
        }
        await _overtimeRepository.UpdateAsync(entity);
                await SaveOvertimeChildrenAsync(entity, dto);
        return await GetOvertimeByIdAsync(id) ?? throw new TaktBusinessException("加班信息不存在");
    }

    /// <summary>
    /// 删除加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeByIdAsync(long id)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在或已删除");
        }
        await _overtimeItemRepository.DeleteAsync(x => x.OvertimeId == entity.Id);
        var deleted = await _overtimeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("加班信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除加班信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOvertimeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新加班信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> UpdateOvertimeStatusAsync(TaktOvertimeStatusDto dto)
    {
        var entity = await _overtimeRepository.GetByIdAsync(dto.OvertimeId);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在");
        }
        entity.OvertimeStatus = dto.OvertimeStatus;
        await _overtimeRepository.UpdateAsync(entity);
        return await GetOvertimeByIdAsync(dto.OvertimeId) ?? throw new TaktBusinessException("加班信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetOvertimeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktOvertimeTemplateDto>(
            sheetName ?? "加班信息导入模板",
            fileName ?? "加班信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入加班信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportOvertimeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktOvertimeImportDto>(fileStream, sheetName ?? "加班信息导入模板");
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
                var entity = rows[i].Adapt<TaktOvertime>();
                var importKey = $"{entity.DeptId}|{entity.OvertimeDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DeptId、OvertimeDate）");
                }
                var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
                    _overtimeRepository,
                    x => x.DeptId == entity.DeptId
                        && x.OvertimeDate == entity.OvertimeDate);
                if (!isUnique_ix_overtime_dept_date_unique)
                {
                    throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
                }
                await _overtimeRepository.CreateAsync(entity);
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
    /// 导出加班信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOvertimeAsync(TaktOvertimeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktOvertimeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOvertimeExportDto>(),
                sheetName ?? "加班信息数据",
                fileName ?? "加班信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _overtimeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOvertimeExportDto>(),
                sheetName ?? "加班信息数据",
                fileName ?? "加班信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOvertimeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "加班信息数据",
            fileName ?? "加班信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废加班明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="overtimeId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkOvertimeItemsObsoleteAsync(long overtimeId)
    {
        if (overtimeId <= 0)
        {
            return;
        }
        var rows = await _overtimeItemRepository.GetListAsync(
            x => x.OvertimeId == overtimeId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _overtimeItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充加班信息详情（加载 OneToMany 子表：加班明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillOvertimeDetailsAsync(TaktOvertimeDto dto, TaktOvertime entity)
    {
        if (dto == null)
        {
            return;
        }
        // 加班明细 → dto.Items（含作废行）
        var items = await _overtimeItemRepository.GetListAsync(x => x.OvertimeId == entity.Id);
        dto.Items = items.Adapt<List<TaktOvertimeItemDto>>();
    }

    /// <summary>
    /// 保存加班信息子表级联（加班明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveOvertimeChildrenAsync(TaktOvertime entity, TaktOvertimeCreateDto dto)
    {
        // 加班明细（Items）
        List<TaktOvertimeItemUpdateDto>? itemsForSave;
        if (dto is TaktOvertimeUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktOvertimeItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkOvertimeItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _overtimeItemRepository.GetListAsync(x => x.OvertimeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktOvertimeItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.OvertimeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("加班明细第{i + 1}项与本次提交的其他项重复（CompanyCode、OvertimeId、LineNumber）");
                }
                if (childDto.OvertimeItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.OvertimeItemId, out var target))
                    {
                        throw new TaktBusinessException("加班明细不存在（OvertimeItemId={childDto.OvertimeItemId}）");
                    }
                    if (target.OvertimeId != entity.Id)
                    {
                        throw new TaktBusinessException("加班明细不属于当前主表（OvertimeItemId={childDto.OvertimeItemId}）");
                    }
                    submittedIds.Add(childDto.OvertimeItemId);
                    var isUniqueUpdate_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _overtimeItemRepository,
                        x => x.OvertimeId == x.OvertimeId
                && x.LineNumber == x.LineNumber,
                        childDto.OvertimeItemId);
                    if (!isUniqueUpdate_ix_overtime_item_request_line_unique)
                    {
                        throw new TaktBusinessException("加班明细的OvertimeId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.OvertimeItemId;
                    target.OvertimeId = entity.Id;
                    target.IsObsolete = 0;
                    await _overtimeItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _overtimeItemRepository,
                        x => x.OvertimeId == x.OvertimeId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_overtime_item_request_line_unique)
                    {
                        throw new TaktBusinessException("加班明细的OvertimeId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktOvertimeItem>();
                    child.Id = 0;
                    child.OvertimeId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _overtimeItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = entity.Id.ToString();
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
                await _overtimeItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建加班信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOvertime, bool>> QueryExpression(TaktOvertimeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOvertime>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.HandlingByName != null && x.HandlingByName.Contains(keywords))
                || (x.HandlingComment != null && x.HandlingComment.Contains(keywords))
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

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.TotalEmployees.HasValue == true)
        {
            var totalEmployees = queryDto.TotalEmployees.Value;
            exp = exp.And(x => x.TotalEmployees == totalEmployees);
        }

        if (queryDto?.TotalPlannedHours.HasValue == true)
        {
            var totalPlannedHours = queryDto.TotalPlannedHours.Value;
            exp = exp.And(x => x.TotalPlannedHours == totalPlannedHours);
        }

        if (queryDto?.TotalActualHours.HasValue == true)
        {
            var totalActualHours = queryDto.TotalActualHours.Value;
            exp = exp.And(x => x.TotalActualHours == totalActualHours);
        }

        if (queryDto?.OvertimeType.HasValue == true)
        {
            var overtimeType = queryDto.OvertimeType.Value;
            exp = exp.And(x => x.OvertimeType == overtimeType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
        }

        if (queryDto?.HandlingBy.HasValue == true)
        {
            var handlingBy = queryDto.HandlingBy.Value;
            exp = exp.And(x => x.HandlingBy == handlingBy);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandlingByName))
        {
            var handlingByName = queryDto.HandlingByName;
            exp = exp.And(x => x.HandlingByName != null && x.HandlingByName.Contains(handlingByName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandlingComment))
        {
            var handlingComment = queryDto.HandlingComment;
            exp = exp.And(x => x.HandlingComment != null && x.HandlingComment.Contains(handlingComment));
        }

        if (queryDto?.OvertimeStatus.HasValue == true)
        {
            var overtimeStatus = queryDto.OvertimeStatus.Value;
            exp = exp.And(x => x.OvertimeStatus == overtimeStatus);
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

        if (queryDto?.OvertimeDateStart.HasValue == true)
        {
            var overtimeDateStart = queryDto.OvertimeDateStart.Value;
            exp = exp.And(x => x.OvertimeDate >= overtimeDateStart);
        }

        if (queryDto?.OvertimeDateEnd.HasValue == true)
        {
            var overtimeDateEnd = queryDto.OvertimeDateEnd.Value;
            exp = exp.And(x => x.OvertimeDate <= overtimeDateEnd);
        }

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            var plannedStartTimeStart = queryDto.PlannedStartTimeStart.Value;
            exp = exp.And(x => x.PlannedStartTime >= plannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            var plannedStartTimeEnd = queryDto.PlannedStartTimeEnd.Value;
            exp = exp.And(x => x.PlannedStartTime <= plannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            var plannedEndTimeStart = queryDto.PlannedEndTimeStart.Value;
            exp = exp.And(x => x.PlannedEndTime >= plannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            var plannedEndTimeEnd = queryDto.PlannedEndTimeEnd.Value;
            exp = exp.And(x => x.PlannedEndTime <= plannedEndTimeEnd);
        }

        if (queryDto?.HandlingAtStart.HasValue == true)
        {
            var handlingAtStart = queryDto.HandlingAtStart.Value;
            exp = exp.And(x => x.HandlingAt >= handlingAtStart);
        }

        if (queryDto?.HandlingAtEnd.HasValue == true)
        {
            var handlingAtEnd = queryDto.HandlingAtEnd.Value;
            exp = exp.And(x => x.HandlingAt <= handlingAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktOvertimeQueryDto? queryDto)
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
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.TotalEmployees.HasValue)
        {
            return true;
        }
        if (queryDto.TotalPlannedHours.HasValue)
        {
            return true;
        }
        if (queryDto.TotalActualHours.HasValue)
        {
            return true;
        }
        if (queryDto.OvertimeType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
        {
            return true;
        }
        if (queryDto.HandlingBy.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingComment))
        {
            return true;
        }
        if (queryDto.OvertimeStatus.HasValue)
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
        if (queryDto.OvertimeDateStart.HasValue || queryDto.OvertimeDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedStartTimeStart.HasValue || queryDto.PlannedStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedEndTimeStart.HasValue || queryDto.PlannedEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.HandlingAtStart.HasValue || queryDto.HandlingAtEnd.HasValue)
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
