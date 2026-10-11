// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktOvertimeItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：加班明细应用服务实现
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
/// 加班明细应用服务
/// </summary>
public class TaktOvertimeItemService : TaktServiceBase, ITaktOvertimeItemService
{
    private readonly ITaktCompanyRepository<TaktOvertimeItem> _overtimeItemRepository;
    private readonly ITaktApprovalRepository<TaktOvertime> _overtimeRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overtimeItemRepository">加班明细仓储</param>
    /// <param name="overtimeRepository">加班信息仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOvertimeItemService(
        ITaktCompanyRepository<TaktOvertimeItem> overtimeItemRepository,
        ITaktApprovalRepository<TaktOvertime> overtimeRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _overtimeItemRepository = overtimeItemRepository;
        _overtimeRepository = overtimeRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取加班明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOvertimeItemDto>> GetOvertimeItemListAsync(TaktOvertimeItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktOvertimeItemDto>.Create(
                new List<TaktOvertimeItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _overtimeItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktOvertimeItemDto>.Create(
            data.Adapt<List<TaktOvertimeItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeItemDto?> GetOvertimeItemByIdAsync(long id)
    {
        var entity = await _overtimeItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktOvertimeItemDto>();
    }

    /// <summary>
    /// 获取加班明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOvertimeItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _overtimeItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeName,
            DictLabel = e.EmployeeName,
        }).ToList();
    }

    /// <summary>
    /// 创建加班明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeItemDto> CreateOvertimeItemAsync(TaktOvertimeItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktOvertimeItem>();
        entity.IsObsolete = 0;
        await StampOvertimeItemOvertimeAsync(entity, dto);
        var isUnique_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeItemRepository,
            x => x.OvertimeId == entity.OvertimeId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_overtime_item_request_line_unique)
        {
            throw new TaktBusinessException("加班明细的OvertimeId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _overtimeItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OvertimeId == entity.OvertimeId,
                x => x.LineNumber);
            var businessCode = entity.OvertimeId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _overtimeItemRepository.CreateAsync(entity);
        return await GetOvertimeItemByIdAsync(entity.Id) ?? entity.Adapt<TaktOvertimeItemDto>();
    }

    /// <summary>
    /// 更新加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeItemDto> UpdateOvertimeItemAsync(long id, TaktOvertimeItemUpdateDto dto)
    {
        var entity = await _overtimeItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班明细不存在");
        }
        dto.Adapt(entity);
        await StampOvertimeItemOvertimeAsync(entity, dto);
        var isUnique_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeItemRepository,
            x => x.OvertimeId == entity.OvertimeId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_overtime_item_request_line_unique)
        {
            throw new TaktBusinessException("加班明细的OvertimeId、LineNumber已存在");
        }
        await _overtimeItemRepository.UpdateAsync(entity);
        return await GetOvertimeItemByIdAsync(id) ?? throw new TaktBusinessException("加班明细不存在");
    }

    /// <summary>
    /// 删除加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeItemByIdAsync(long id)
    {
        var entity = await _overtimeItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("加班明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("加班明细已作废");
        }
        entity.IsObsolete = 1;
        await _overtimeItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除加班明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOvertimeItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新加班明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeItemDto> UpdateOvertimeItemObsoleteAsync(TaktOvertimeItemObsoleteDto dto)
    {
        var entity = await _overtimeItemRepository.GetByIdAsync(dto.OvertimeItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("加班明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("加班明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _overtimeItemRepository.UpdateAsync(entity);
        return await GetOvertimeItemByIdAsync(dto.OvertimeItemId) ?? throw new TaktBusinessException("加班明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetOvertimeItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktOvertimeItemTemplateDto>(
            sheetName ?? "加班明细导入模板",
            fileName ?? "加班明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入加班明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportOvertimeItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktOvertimeItemImportDto>(fileStream, sheetName ?? "加班明细导入模板");
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
                var entity = rows[i].Adapt<TaktOvertimeItem>();
                var importDto = rows[i].Adapt<TaktOvertimeItemCreateDto>();
                await StampOvertimeItemOvertimeAsync(entity, importDto);
                var importKey = $"{entity.OvertimeId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（OvertimeId、LineNumber）");
                }
                var isUnique_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _overtimeItemRepository,
                    x => x.OvertimeId == entity.OvertimeId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_overtime_item_request_line_unique)
                {
                    throw new TaktBusinessException("加班明细的OvertimeId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _overtimeItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OvertimeId == entity.OvertimeId,
                        x => x.LineNumber);
                    var businessCode = entity.OvertimeId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _overtimeItemRepository.CreateAsync(entity);
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
    /// 导出加班明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOvertimeItemAsync(TaktOvertimeItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktOvertimeItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOvertimeItemExportDto>(),
                sheetName ?? "加班明细数据",
                fileName ?? "加班明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _overtimeItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOvertimeItemExportDto>(),
                sheetName ?? "加班明细数据",
                fileName ?? "加班明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOvertimeItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "加班明细数据",
            fileName ?? "加班明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步加班明细主表外键（ManyToOne → 加班信息）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampOvertimeItemOvertimeAsync(TaktOvertimeItem entity, TaktOvertimeItemCreateDto dto)
    {
        if (dto.OvertimeId <= 0)
        {
            return;
        }
        var master = await _overtimeRepository.GetByIdAsync(dto.OvertimeId);
        if (master == null)
        {
            throw new TaktBusinessException("加班信息不存在");
        }
        entity.OvertimeId = master.Id;
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
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建加班明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOvertimeItem, bool>> QueryExpression(TaktOvertimeItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOvertimeItem>();

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
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
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

        if (queryDto?.OvertimeId.HasValue == true)
        {
            var overtimeId = queryDto.OvertimeId.Value;
            exp = exp.And(x => x.OvertimeId == overtimeId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeName))
        {
            var employeeName = queryDto.EmployeeName;
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(employeeName));
        }

        if (queryDto?.PlannedHours.HasValue == true)
        {
            var plannedHours = queryDto.PlannedHours.Value;
            exp = exp.And(x => x.PlannedHours == plannedHours);
        }

        if (queryDto?.ActualHours.HasValue == true)
        {
            var actualHours = queryDto.ActualHours.Value;
            exp = exp.And(x => x.ActualHours == actualHours);
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

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            var actualStartTimeStart = queryDto.ActualStartTimeStart.Value;
            exp = exp.And(x => x.ActualStartTime >= actualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            var actualStartTimeEnd = queryDto.ActualStartTimeEnd.Value;
            exp = exp.And(x => x.ActualStartTime <= actualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            var actualEndTimeStart = queryDto.ActualEndTimeStart.Value;
            exp = exp.And(x => x.ActualEndTime >= actualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            var actualEndTimeEnd = queryDto.ActualEndTimeEnd.Value;
            exp = exp.And(x => x.ActualEndTime <= actualEndTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktOvertimeItemQueryDto? queryDto)
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
        if (queryDto.OvertimeId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.EmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (queryDto.PlannedHours.HasValue)
        {
            return true;
        }
        if (queryDto.ActualHours.HasValue)
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
        if (queryDto.ActualStartTimeStart.HasValue || queryDto.ActualStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualEndTimeStart.HasValue || queryDto.ActualEndTimeEnd.HasValue)
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
