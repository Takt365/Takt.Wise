// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketCategoryAssignService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单分类默认处理人应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 工单分类默认处理人应用服务
/// </summary>
public class TaktTicketCategoryAssignService : TaktServiceBase, ITaktTicketCategoryAssignService
{
    private readonly ITaktCompanyRepository<TaktTicketCategoryAssign> _ticketCategoryAssignRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketCategoryAssignRepository">工单分类默认处理人仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketCategoryAssignService(
        ITaktCompanyRepository<TaktTicketCategoryAssign> ticketCategoryAssignRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketCategoryAssignRepository = ticketCategoryAssignRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单分类默认处理人列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketCategoryAssignDto>> GetTicketCategoryAssignListAsync(TaktTicketCategoryAssignQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketCategoryAssignRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketCategoryAssignDto>.Create(
            data.Adapt<List<TaktTicketCategoryAssignDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketCategoryAssignDto?> GetTicketCategoryAssignByIdAsync(long id)
    {
        var entity = await _ticketCategoryAssignRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTicketCategoryAssignDto>();
    }

    /// <summary>
    /// 获取工单分类默认处理人选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketCategoryAssignOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ticketCategoryAssignRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AssigneeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AssigneeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单分类默认处理人
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketCategoryAssignDto> CreateTicketCategoryAssignAsync(TaktTicketCategoryAssignCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicketCategoryAssign>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _ticketCategoryAssignRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssigneeId == entity.AssigneeId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.AssigneeId, maxSort);
        }
        entity = await _ticketCategoryAssignRepository.CreateAsync(entity);
        return await GetTicketCategoryAssignByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketCategoryAssignDto>();
    }

    /// <summary>
    /// 更新工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketCategoryAssignDto> UpdateTicketCategoryAssignAsync(long id, TaktTicketCategoryAssignUpdateDto dto)
    {
        var entity = await _ticketCategoryAssignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单分类默认处理人不存在");
        }
        dto.Adapt(entity);
        await _ticketCategoryAssignRepository.UpdateAsync(entity);
        return await GetTicketCategoryAssignByIdAsync(id) ?? throw new TaktBusinessException("工单分类默认处理人不存在");
    }

    /// <summary>
    /// 删除工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketCategoryAssignByIdAsync(long id)
    {
        var deleted = await _ticketCategoryAssignRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单分类默认处理人不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单分类默认处理人
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketCategoryAssignBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketCategoryAssignByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工单分类默认处理人排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketCategoryAssignDto> UpdateTicketCategoryAssignSortAsync(TaktTicketCategoryAssignSortDto dto)
    {
        var entity = await _ticketCategoryAssignRepository.GetByIdAsync(dto.TicketCategoryAssignId);
        if (entity == null)
        {
            throw new TaktBusinessException("工单分类默认处理人不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _ticketCategoryAssignRepository.UpdateAsync(entity);
        return await GetTicketCategoryAssignByIdAsync(dto.TicketCategoryAssignId) ?? throw new TaktBusinessException("工单分类默认处理人不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTicketCategoryAssignTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketCategoryAssignTemplateDto>(
            sheetName ?? "工单分类默认处理人导入模板",
            fileName ?? "工单分类默认处理人导入模板.xlsx");
    }

    /// <summary>
    /// 导入工单分类默认处理人
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTicketCategoryAssignAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTicketCategoryAssignImportDto>(fileStream, sheetName ?? "工单分类默认处理人导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTicketCategoryAssign>();
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _ticketCategoryAssignRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssigneeId == entity.AssigneeId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.AssigneeId, maxSort);
                }
                await _ticketCategoryAssignRepository.CreateAsync(entity);
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
    /// 导出工单分类默认处理人
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketCategoryAssignAsync(TaktTicketCategoryAssignQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTicketCategoryAssignQueryDto());
        var list = await _ticketCategoryAssignRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketCategoryAssignExportDto>(),
                sheetName ?? "工单分类默认处理人数据",
                fileName ?? "工单分类默认处理人导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketCategoryAssignExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单分类默认处理人数据",
            fileName ?? "工单分类默认处理人导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单分类默认处理人查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicketCategoryAssign, bool>> QueryExpression(TaktTicketCategoryAssignQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicketCategoryAssign>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CategoryCode != null && x.CategoryCode.Contains(keywords))
                || SqlFunc.ToString(x.AssigneeId).Contains(keywords)
                || (x.AssigneeName != null && x.AssigneeName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CategoryCode))
        {
            exp = exp.And(x => x.CategoryCode != null && x.CategoryCode.Contains(queryDto.CategoryCode));
        }

        if (queryDto?.AssigneeId.HasValue == true)
        {
            exp = exp.And(x => x.AssigneeId == queryDto.AssigneeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssigneeName))
        {
            exp = exp.And(x => x.AssigneeName != null && x.AssigneeName.Contains(queryDto.AssigneeName));
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
