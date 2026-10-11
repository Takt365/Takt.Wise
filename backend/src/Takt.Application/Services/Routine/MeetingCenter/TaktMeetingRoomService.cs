// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingRoomService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：会议室应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Domain.Entities.Routine.MeetingCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议室应用服务
/// </summary>
public class TaktMeetingRoomService : TaktServiceBase, ITaktMeetingRoomService
{
    private readonly ITaktCompanyRepository<TaktMeetingRoom> _meetingRoomRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingRoomRepository">会议室仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingRoomService(
        ITaktCompanyRepository<TaktMeetingRoom> meetingRoomRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingRoomRepository = meetingRoomRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会议室列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMeetingRoomDto>> GetMeetingRoomListAsync(TaktMeetingRoomQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _meetingRoomRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMeetingRoomDto>.Create(
            data.Adapt<List<TaktMeetingRoomDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingRoomDto?> GetMeetingRoomByIdAsync(long id)
    {
        var entity = await _meetingRoomRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMeetingRoomDto>();
    }

    /// <summary>
    /// 获取会议室选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMeetingRoomOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _meetingRoomRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoomStatus == 1,
            x => x.RoomName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RoomName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会议室
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingRoomDto> CreateMeetingRoomAsync(TaktMeetingRoomCreateDto dto)
    {
        var entity = dto.Adapt<TaktMeetingRoom>();
        var isUnique_ix_meeting_room_code_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingRoomRepository,
            x => x.RoomCode == entity.RoomCode);
        if (!isUnique_ix_meeting_room_code_unique)
        {
            throw new TaktBusinessException("会议室的RoomCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _meetingRoomRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _meetingRoomRepository.CreateAsync(entity);
        return await GetMeetingRoomByIdAsync(entity.Id) ?? entity.Adapt<TaktMeetingRoomDto>();
    }

    /// <summary>
    /// 更新会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingRoomDto> UpdateMeetingRoomAsync(long id, TaktMeetingRoomUpdateDto dto)
    {
        var entity = await _meetingRoomRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议室不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_meeting_room_code_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingRoomRepository,
            x => x.RoomCode == entity.RoomCode,
            id);
        if (!isUnique_ix_meeting_room_code_unique)
        {
            throw new TaktBusinessException("会议室的RoomCode已存在");
        }
        await _meetingRoomRepository.UpdateAsync(entity);
        return await GetMeetingRoomByIdAsync(id) ?? throw new TaktBusinessException("会议室不存在");
    }

    /// <summary>
    /// 删除会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingRoomByIdAsync(long id)
    {
        var deleted = await _meetingRoomRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会议室不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会议室
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingRoomBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMeetingRoomByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议室状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingRoomDto> UpdateMeetingRoomStatusAsync(TaktMeetingRoomStatusDto dto)
    {
        var entity = await _meetingRoomRepository.GetByIdAsync(dto.MeetingRoomId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议室不存在");
        }
        entity.RoomStatus = dto.RoomStatus;
        await _meetingRoomRepository.UpdateAsync(entity);
        return await GetMeetingRoomByIdAsync(dto.MeetingRoomId) ?? throw new TaktBusinessException("会议室不存在");
    }

    /// <summary>
    /// 更新会议室排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingRoomDto> UpdateMeetingRoomSortAsync(TaktMeetingRoomSortDto dto)
    {
        var entity = await _meetingRoomRepository.GetByIdAsync(dto.MeetingRoomId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议室不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _meetingRoomRepository.UpdateAsync(entity);
        return await GetMeetingRoomByIdAsync(dto.MeetingRoomId) ?? throw new TaktBusinessException("会议室不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMeetingRoomTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMeetingRoomTemplateDto>(
            sheetName ?? "会议室导入模板",
            fileName ?? "会议室导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议室
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMeetingRoomAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMeetingRoomImportDto>(fileStream, sheetName ?? "会议室导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _meetingRoomRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktMeetingRoom>();
                var importKey = $"{entity.RoomCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RoomCode）");
                }
                var isUnique_ix_meeting_room_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _meetingRoomRepository,
                    x => x.RoomCode == entity.RoomCode);
                if (!isUnique_ix_meeting_room_code_unique)
                {
                    throw new TaktBusinessException("会议室的RoomCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _meetingRoomRepository.CreateAsync(entity);
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
    /// 导出会议室
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMeetingRoomAsync(TaktMeetingRoomQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMeetingRoomQueryDto());
        var list = await _meetingRoomRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingRoomExportDto>(),
                sheetName ?? "会议室数据",
                fileName ?? "会议室导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMeetingRoomExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议室数据",
            fileName ?? "会议室导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议室查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMeetingRoom, bool>> QueryExpression(TaktMeetingRoomQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMeetingRoom>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RoomCode != null && x.RoomCode.Contains(keywords))
                || (x.RoomName != null && x.RoomName.Contains(keywords))
                || (x.Building != null && x.Building.Contains(keywords))
                || (x.Floor != null && x.Floor.Contains(keywords))
                || (x.LocationDetail != null && x.LocationDetail.Contains(keywords))
                || SqlFunc.ToString(x.Capacity).Contains(keywords)
                || (x.Facilities != null && x.Facilities.Contains(keywords))
                || SqlFunc.ToString(x.RoomStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.RoomCode))
        {
            exp = exp.And(x => x.RoomCode != null && x.RoomCode.Contains(queryDto.RoomCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoomName))
        {
            exp = exp.And(x => x.RoomName != null && x.RoomName.Contains(queryDto.RoomName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Building))
        {
            exp = exp.And(x => x.Building != null && x.Building.Contains(queryDto.Building));
        }

        if (!string.IsNullOrEmpty(queryDto?.Floor))
        {
            exp = exp.And(x => x.Floor != null && x.Floor.Contains(queryDto.Floor));
        }

        if (!string.IsNullOrEmpty(queryDto?.LocationDetail))
        {
            exp = exp.And(x => x.LocationDetail != null && x.LocationDetail.Contains(queryDto.LocationDetail));
        }

        if (queryDto?.Capacity.HasValue == true)
        {
            exp = exp.And(x => x.Capacity == queryDto.Capacity);
        }

        if (!string.IsNullOrEmpty(queryDto?.Facilities))
        {
            exp = exp.And(x => x.Facilities != null && x.Facilities.Contains(queryDto.Facilities));
        }

        if (queryDto?.RoomStatus.HasValue == true)
        {
            exp = exp.And(x => x.RoomStatus == queryDto.RoomStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
