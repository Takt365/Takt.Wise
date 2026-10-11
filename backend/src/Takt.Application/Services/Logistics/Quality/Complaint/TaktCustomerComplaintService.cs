// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Domain.Entities.Logistics.Quality.Complaint;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 客诉主应用服务
/// </summary>
public class TaktCustomerComplaintService : TaktServiceBase, ITaktCustomerComplaintService
{
    private readonly ITaktCompanyRepository<TaktCustomerComplaint> _customerComplaintRepository;
    private readonly ITaktCompanyRepository<TaktCustomerComplaintItem> _customerComplaintItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintRepository">客诉主仓储</param>
    /// <param name="customerComplaintItemRepository">CustomerComplaintItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerComplaintService(
        ITaktCompanyRepository<TaktCustomerComplaint> customerComplaintRepository,
        ITaktCompanyRepository<TaktCustomerComplaintItem> customerComplaintItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerComplaintRepository = customerComplaintRepository;
        _customerComplaintItemRepository = customerComplaintItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客诉主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerComplaintDto>> GetCustomerComplaintListAsync(TaktCustomerComplaintQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerComplaintDto>.Create(
                new List<TaktCustomerComplaintDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerComplaintRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerComplaintDto>.Create(
            data.Adapt<List<TaktCustomerComplaintDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto?> GetCustomerComplaintByIdAsync(long id)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCustomerComplaintDto>();
        await FillCustomerComplaintDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取客诉主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerComplaintOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerComplaintRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ComplaintStatus == 1,
            x => x.ResponsibleDeptName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerComplaintCode,
            DictLabel = e.ResponsibleDeptName ?? e.CustomerComplaintCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客诉主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> CreateCustomerComplaintAsync(TaktCustomerComplaintCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerComplaint>();
        var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerComplaintCode == entity.CustomerComplaintCode);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
        {
            throw new TaktBusinessException("客诉主的PlantCode、CustomerComplaintCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerComplaintRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
        }
        entity = await _customerComplaintRepository.CreateAsync(entity);
                await SaveCustomerComplaintChildrenAsync(entity, dto);
        return await GetCustomerComplaintByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerComplaintDto>();
    }

    /// <summary>
    /// 更新客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintAsync(long id, TaktCustomerComplaintUpdateDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerComplaintCode == entity.CustomerComplaintCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
        {
            throw new TaktBusinessException("客诉主的PlantCode、CustomerComplaintCode已存在");
        }
        await _customerComplaintRepository.UpdateAsync(entity);
                await SaveCustomerComplaintChildrenAsync(entity, dto);
        return await GetCustomerComplaintByIdAsync(id) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 删除客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintByIdAsync(long id)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在或已删除");
        }
        await _customerComplaintItemRepository.DeleteAsync(x => x.ComplaintId == entity.Id);
        var deleted = await _customerComplaintRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客诉主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客诉主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerComplaintByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客诉主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintStatusAsync(TaktCustomerComplaintStatusDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(dto.CustomerComplaintId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.ComplaintStatus = dto.ComplaintStatus;
        await _customerComplaintRepository.UpdateAsync(entity);
        return await GetCustomerComplaintByIdAsync(dto.CustomerComplaintId) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 更新客诉主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintSortAsync(TaktCustomerComplaintSortDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(dto.CustomerComplaintId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerComplaintRepository.UpdateAsync(entity);
        return await GetCustomerComplaintByIdAsync(dto.CustomerComplaintId) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerComplaintTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerComplaintTemplateDto>(
            sheetName ?? "客诉主导入模板",
            fileName ?? "客诉主导入模板.xlsx");
    }

    /// <summary>
    /// 导入客诉主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerComplaintAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerComplaintImportDto>(fileStream, sheetName ?? "客诉主导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerComplaint>();
                var importKey = $"{entity.PlantCode}|{entity.CustomerComplaintCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、CustomerComplaintCode）");
                }
                var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerComplaintRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.CustomerComplaintCode == entity.CustomerComplaintCode);
                if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
                {
                    throw new TaktBusinessException("客诉主的PlantCode、CustomerComplaintCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerComplaintRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
                }
                await _customerComplaintRepository.CreateAsync(entity);
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
    /// 导出客诉主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintAsync(TaktCustomerComplaintQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerComplaintQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerComplaintExportDto>(),
                sheetName ?? "客诉主数据",
                fileName ?? "客诉主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerComplaintRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerComplaintExportDto>(),
                sheetName ?? "客诉主数据",
                fileName ?? "客诉主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerComplaintExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客诉主数据",
            fileName ?? "客诉主导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取客诉件数统计（数据看板；按投诉日期与 ComplaintStatus）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>客诉件数统计</returns>
    public async Task<TaktCustomerComplaintStatDto> GetCustomerComplaintStatAsync(TaktCustomerComplaintStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.ComplaintDateStart,
            queryDto.ComplaintDateEnd,
            queryDto.StatMonth);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        // 字典 logistics_quality_complaint_status：0=待处理 1=处理中 2=已回复 3=已关闭 4=已驳回
        const int statusPending = 0;
        const int statusInProgress = 1;
        Expression<Func<TaktCustomerComplaint, bool>> inMonth = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ComplaintDate >= start
            && x.ComplaintDate <= end;
        Expression<Func<TaktCustomerComplaint, bool>> pending = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ComplaintDate >= start
            && x.ComplaintDate <= end
            && x.ComplaintStatus == statusPending;
        Expression<Func<TaktCustomerComplaint, bool>> inProgress = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ComplaintDate >= start
            && x.ComplaintDate <= end
            && x.ComplaintStatus == statusInProgress;
        Expression<Func<TaktCustomerComplaint, bool>> completed = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ComplaintDate >= start
            && x.ComplaintDate <= end
            && x.ComplaintStatus >= 2;
        var monthComplaintCount = await _customerComplaintRepository.CountAsync(inMonth);
        var monthPendingCount = await _customerComplaintRepository.CountAsync(pending);
        var monthInProgressCount = await _customerComplaintRepository.CountAsync(inProgress);
        var monthCompletedCount = await _customerComplaintRepository.CountAsync(completed);
        return new TaktCustomerComplaintStatDto
        {
            StatMonth = statMonth,
            MonthComplaintCount = monthComplaintCount,
            MonthPendingCount = monthPendingCount,
            MonthInProgressCount = monthInProgressCount,
            MonthCompletedCount = monthCompletedCount,
        };
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废客诉明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="complaintId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkCustomerComplaintItemsObsoleteAsync(long complaintId)
    {
        if (complaintId <= 0)
        {
            return;
        }
        var rows = await _customerComplaintItemRepository.GetListAsync(
            x => x.ComplaintId == complaintId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _customerComplaintItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充客诉主详情（加载 OneToMany 子表：客诉明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCustomerComplaintDetailsAsync(TaktCustomerComplaintDto dto, TaktCustomerComplaint entity)
    {
        if (dto == null)
        {
            return;
        }
        // 客诉明细 → dto.Items（含作废行）
        var items = await _customerComplaintItemRepository.GetListAsync(x => x.ComplaintId == entity.Id);
        dto.Items = items.Adapt<List<TaktCustomerComplaintItemDto>>();
    }

    /// <summary>
    /// 保存客诉主子表级联（客诉明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCustomerComplaintChildrenAsync(TaktCustomerComplaint entity, TaktCustomerComplaintCreateDto dto)
    {
        // 客诉明细（Items）
        List<TaktCustomerComplaintItemUpdateDto>? itemsForSave;
        if (dto is TaktCustomerComplaintUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktCustomerComplaintItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkCustomerComplaintItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _customerComplaintItemRepository.GetListAsync(x => x.ComplaintId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCustomerComplaintItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.ComplaintId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.CustomerComplaintCode = entity.CustomerComplaintCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("客诉明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ComplaintId、LineNumber）");
                }
                if (childDto.CustomerComplaintItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CustomerComplaintItemId, out var target))
                    {
                        throw new TaktBusinessException("客诉明细不存在（CustomerComplaintItemId={childDto.CustomerComplaintItemId}）");
                    }
                    if (target.ComplaintId != entity.Id)
                    {
                        throw new TaktBusinessException("客诉明细不属于当前主表（CustomerComplaintItemId={childDto.CustomerComplaintItemId}）");
                    }
                    submittedIds.Add(childDto.CustomerComplaintItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerComplaintItemRepository,
                        x => x.ComplaintId == x.ComplaintId
                && x.LineNumber == x.LineNumber,
                        childDto.CustomerComplaintItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_customer_complaint_item_line_unique)
                    {
                        throw new TaktBusinessException("客诉明细的ComplaintId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.CustomerComplaintItemId;
                    target.ComplaintId = entity.Id;
                    target.IsObsolete = 0;
                    await _customerComplaintItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerComplaintItemRepository,
                        x => x.ComplaintId == x.ComplaintId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_customer_complaint_item_line_unique)
                    {
                        throw new TaktBusinessException("客诉明细的ComplaintId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktCustomerComplaintItem>();
                    child.Id = 0;
                    child.ComplaintId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _customerComplaintItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CustomerComplaintCode) ? entity.CustomerComplaintCode : entity.Id.ToString();
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
                await _customerComplaintItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客诉主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerComplaint, bool>> QueryExpression(TaktCustomerComplaintQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerComplaint>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(keywords))
                || (x.ResponsibleGroupName != null && x.ResponsibleGroupName.Contains(keywords))
                || (x.ComplaintDescription != null && x.ComplaintDescription.Contains(keywords))
                || (x.HandlingResult != null && x.HandlingResult.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerComplaintCode))
        {
            var customerComplaintCode = queryDto.CustomerComplaintCode;
            exp = exp.And(x => x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(customerComplaintCode));
        }

        if (queryDto?.CustomerId.HasValue == true)
        {
            var customerId = queryDto.CustomerId.Value;
            exp = exp.And(x => x.CustomerId == customerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerName1))
        {
            var customerName1 = queryDto.CustomerName1;
            exp = exp.And(x => x.CustomerName1 != null && x.CustomerName1.Contains(customerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (queryDto?.ComplaintMethod.HasValue == true)
        {
            var complaintMethod = queryDto.ComplaintMethod.Value;
            exp = exp.And(x => x.ComplaintMethod == complaintMethod);
        }

        if (queryDto?.ComplaintType.HasValue == true)
        {
            var complaintType = queryDto.ComplaintType.Value;
            exp = exp.And(x => x.ComplaintType == complaintType);
        }

        if (queryDto?.ComplaintLevel.HasValue == true)
        {
            var complaintLevel = queryDto.ComplaintLevel.Value;
            exp = exp.And(x => x.ComplaintLevel == complaintLevel);
        }

        if (queryDto?.ResponsibleDeptId.HasValue == true)
        {
            var responsibleDeptId = queryDto.ResponsibleDeptId.Value;
            exp = exp.And(x => x.ResponsibleDeptId == responsibleDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponsibleDeptName))
        {
            var responsibleDeptName = queryDto.ResponsibleDeptName;
            exp = exp.And(x => x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(responsibleDeptName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponsibleGroupCode))
        {
            var responsibleGroupCode = queryDto.ResponsibleGroupCode;
            exp = exp.And(x => x.ResponsibleGroupCode == responsibleGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponsibleGroupName))
        {
            var responsibleGroupName = queryDto.ResponsibleGroupName;
            exp = exp.And(x => x.ResponsibleGroupName != null && x.ResponsibleGroupName.Contains(responsibleGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ComplaintDescription))
        {
            var complaintDescription = queryDto.ComplaintDescription;
            exp = exp.And(x => x.ComplaintDescription != null && x.ComplaintDescription.Contains(complaintDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandlingResult))
        {
            var handlingResult = queryDto.HandlingResult;
            exp = exp.And(x => x.HandlingResult != null && x.HandlingResult.Contains(handlingResult));
        }

        if (queryDto?.CustomerSatisfaction.HasValue == true)
        {
            var customerSatisfaction = queryDto.CustomerSatisfaction.Value;
            exp = exp.And(x => x.CustomerSatisfaction == customerSatisfaction);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Attachments))
        {
            var attachments = queryDto.Attachments;
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(attachments));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.ComplaintStatus.HasValue == true)
        {
            var complaintStatus = queryDto.ComplaintStatus.Value;
            exp = exp.And(x => x.ComplaintStatus == complaintStatus);
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

        if (queryDto?.ComplaintDateStart.HasValue == true)
        {
            var complaintDateStart = queryDto.ComplaintDateStart.Value;
            exp = exp.And(x => x.ComplaintDate >= complaintDateStart);
        }

        if (queryDto?.ComplaintDateEnd.HasValue == true)
        {
            var complaintDateEnd = queryDto.ComplaintDateEnd.Value;
            exp = exp.And(x => x.ComplaintDate <= complaintDateEnd);
        }

        if (queryDto?.RequiredReplyDateStart.HasValue == true)
        {
            var requiredReplyDateStart = queryDto.RequiredReplyDateStart.Value;
            exp = exp.And(x => x.RequiredReplyDate >= requiredReplyDateStart);
        }

        if (queryDto?.RequiredReplyDateEnd.HasValue == true)
        {
            var requiredReplyDateEnd = queryDto.RequiredReplyDateEnd.Value;
            exp = exp.And(x => x.RequiredReplyDate <= requiredReplyDateEnd);
        }

        if (queryDto?.ActualReplyDateStart.HasValue == true)
        {
            var actualReplyDateStart = queryDto.ActualReplyDateStart.Value;
            exp = exp.And(x => x.ActualReplyDate >= actualReplyDateStart);
        }

        if (queryDto?.ActualReplyDateEnd.HasValue == true)
        {
            var actualReplyDateEnd = queryDto.ActualReplyDateEnd.Value;
            exp = exp.And(x => x.ActualReplyDate <= actualReplyDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktCustomerComplaintQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerComplaintCode))
        {
            return true;
        }
        if (queryDto.CustomerId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (queryDto.ComplaintMethod.HasValue)
        {
            return true;
        }
        if (queryDto.ComplaintType.HasValue)
        {
            return true;
        }
        if (queryDto.ComplaintLevel.HasValue)
        {
            return true;
        }
        if (queryDto.ResponsibleDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResponsibleDeptName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResponsibleGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResponsibleGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ComplaintDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingResult))
        {
            return true;
        }
        if (queryDto.CustomerSatisfaction.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Attachments))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.ComplaintStatus.HasValue)
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
        if (queryDto.ComplaintDateStart.HasValue || queryDto.ComplaintDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.RequiredReplyDateStart.HasValue || queryDto.RequiredReplyDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualReplyDateStart.HasValue || queryDto.ActualReplyDateEnd.HasValue)
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
