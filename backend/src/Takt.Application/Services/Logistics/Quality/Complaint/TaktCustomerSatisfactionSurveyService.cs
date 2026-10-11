// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查应用服务实现
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
/// 客户满意度调查应用服务
/// </summary>
public class TaktCustomerSatisfactionSurveyService : TaktServiceBase, ITaktCustomerSatisfactionSurveyService
{
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> _customerSatisfactionSurveyRepository;
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> _customerSatisfactionSurveyItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerSatisfactionSurveyRepository">客户满意度调查仓储</param>
    /// <param name="customerSatisfactionSurveyItemRepository">CustomerSatisfactionSurveyItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerSatisfactionSurveyService(
        ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> customerSatisfactionSurveyRepository,
        ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> customerSatisfactionSurveyItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerSatisfactionSurveyRepository = customerSatisfactionSurveyRepository;
        _customerSatisfactionSurveyItemRepository = customerSatisfactionSurveyItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户满意度调查列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerSatisfactionSurveyDto>> GetCustomerSatisfactionSurveyListAsync(TaktCustomerSatisfactionSurveyQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerSatisfactionSurveyDto>.Create(
                new List<TaktCustomerSatisfactionSurveyDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerSatisfactionSurveyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerSatisfactionSurveyDto>.Create(
            data.Adapt<List<TaktCustomerSatisfactionSurveyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto?> GetCustomerSatisfactionSurveyByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCustomerSatisfactionSurveyDto>();
        await FillCustomerSatisfactionSurveyDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取客户满意度调查选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerSatisfactionSurveyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerSatisfactionSurveyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SurveyStatus == 1,
            x => x.CustomerSatisfactionSurveyCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerSatisfactionSurveyCode,
            DictLabel = e.CustomerSatisfactionSurveyCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客户满意度调查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> CreateCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerSatisfactionSurvey>();
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
        {
            throw new TaktBusinessException("客户满意度调查的PlantCode、CustomerSatisfactionSurveyCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerSatisfactionSurveyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
        }
        entity = await _customerSatisfactionSurveyRepository.CreateAsync(entity);
                await SaveCustomerSatisfactionSurveyChildrenAsync(entity, dto);
        return await GetCustomerSatisfactionSurveyByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerSatisfactionSurveyDto>();
    }

    /// <summary>
    /// 更新客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyAsync(long id, TaktCustomerSatisfactionSurveyUpdateDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
        {
            throw new TaktBusinessException("客户满意度调查的PlantCode、CustomerSatisfactionSurveyCode已存在");
        }
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
                await SaveCustomerSatisfactionSurveyChildrenAsync(entity, dto);
        return await GetCustomerSatisfactionSurveyByIdAsync(id) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 删除客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在或已删除");
        }
        await _customerSatisfactionSurveyItemRepository.DeleteAsync(x => x.SurveyId == entity.Id);
        var deleted = await _customerSatisfactionSurveyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户满意度调查不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户满意度调查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerSatisfactionSurveyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户满意度调查状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyStatusAsync(TaktCustomerSatisfactionSurveyStatusDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        entity.SurveyStatus = dto.SurveyStatus;
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyByIdAsync(dto.CustomerSatisfactionSurveyId) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 更新客户满意度调查排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveySortAsync(TaktCustomerSatisfactionSurveySortDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyByIdAsync(dto.CustomerSatisfactionSurveyId) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerSatisfactionSurveyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerSatisfactionSurveyTemplateDto>(
            sheetName ?? "客户满意度调查导入模板",
            fileName ?? "客户满意度调查导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户满意度调查
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerSatisfactionSurveyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerSatisfactionSurveyImportDto>(fileStream, sheetName ?? "客户满意度调查导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerSatisfactionSurvey>();
                var importKey = $"{entity.PlantCode}|{entity.CustomerSatisfactionSurveyCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、CustomerSatisfactionSurveyCode）");
                }
                var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerSatisfactionSurveyRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode);
                if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
                {
                    throw new TaktBusinessException("客户满意度调查的PlantCode、CustomerSatisfactionSurveyCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerSatisfactionSurveyRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
                }
                await _customerSatisfactionSurveyRepository.CreateAsync(entity);
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
    /// 导出客户满意度调查
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerSatisfactionSurveyQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerSatisfactionSurveyExportDto>(),
                sheetName ?? "客户满意度调查数据",
                fileName ?? "客户满意度调查导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerSatisfactionSurveyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerSatisfactionSurveyExportDto>(),
                sheetName ?? "客户满意度调查数据",
                fileName ?? "客户满意度调查导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerSatisfactionSurveyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户满意度调查数据",
            fileName ?? "客户满意度调查导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废客户满意度调查项目明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="surveyId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkCustomerSatisfactionSurveyItemsObsoleteAsync(long surveyId)
    {
        if (surveyId <= 0)
        {
            return;
        }
        var rows = await _customerSatisfactionSurveyItemRepository.GetListAsync(
            x => x.SurveyId == surveyId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _customerSatisfactionSurveyItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充客户满意度调查详情（加载 OneToMany 子表：客户满意度调查项目明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCustomerSatisfactionSurveyDetailsAsync(TaktCustomerSatisfactionSurveyDto dto, TaktCustomerSatisfactionSurvey entity)
    {
        if (dto == null)
        {
            return;
        }
        // 客户满意度调查项目明细 → dto.Items（含作废行）
        var items = await _customerSatisfactionSurveyItemRepository.GetListAsync(x => x.SurveyId == entity.Id);
        dto.Items = items.Adapt<List<TaktCustomerSatisfactionSurveyItemDto>>();
    }

    /// <summary>
    /// 保存客户满意度调查子表级联（客户满意度调查项目明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCustomerSatisfactionSurveyChildrenAsync(TaktCustomerSatisfactionSurvey entity, TaktCustomerSatisfactionSurveyCreateDto dto)
    {
        // 客户满意度调查项目明细（Items）
        List<TaktCustomerSatisfactionSurveyItemUpdateDto>? itemsForSave;
        if (dto is TaktCustomerSatisfactionSurveyUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktCustomerSatisfactionSurveyItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkCustomerSatisfactionSurveyItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _customerSatisfactionSurveyItemRepository.GetListAsync(x => x.SurveyId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCustomerSatisfactionSurveyItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SurveyId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.CustomerSatisfactionSurveyCode = entity.CustomerSatisfactionSurveyCode;
                childDto.CustomerFeedback = entity.CustomerFeedback;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("客户满意度调查项目明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SurveyId、LineNumber）");
                }
                if (childDto.CustomerSatisfactionSurveyItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CustomerSatisfactionSurveyItemId, out var target))
                    {
                        throw new TaktBusinessException("客户满意度调查项目明细不存在（CustomerSatisfactionSurveyItemId={childDto.CustomerSatisfactionSurveyItemId}）");
                    }
                    if (target.SurveyId != entity.Id)
                    {
                        throw new TaktBusinessException("客户满意度调查项目明细不属于当前主表（CustomerSatisfactionSurveyItemId={childDto.CustomerSatisfactionSurveyItemId}）");
                    }
                    submittedIds.Add(childDto.CustomerSatisfactionSurveyItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerSatisfactionSurveyItemRepository,
                        x => x.SurveyId == x.SurveyId
                && x.LineNumber == x.LineNumber,
                        childDto.CustomerSatisfactionSurveyItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
                    {
                        throw new TaktBusinessException("客户满意度调查项目明细的SurveyId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.CustomerSatisfactionSurveyItemId;
                    target.SurveyId = entity.Id;
                    target.IsObsolete = 0;
                    await _customerSatisfactionSurveyItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerSatisfactionSurveyItemRepository,
                        x => x.SurveyId == x.SurveyId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
                    {
                        throw new TaktBusinessException("客户满意度调查项目明细的SurveyId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktCustomerSatisfactionSurveyItem>();
                    child.Id = 0;
                    child.SurveyId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _customerSatisfactionSurveyItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CustomerSatisfactionSurveyCode) ? entity.CustomerSatisfactionSurveyCode : entity.Id.ToString();
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
                await _customerSatisfactionSurveyItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户满意度调查查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerSatisfactionSurvey, bool>> QueryExpression(TaktCustomerSatisfactionSurveyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerSatisfactionSurvey>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.SurveyorByGroupName != null && x.SurveyorByGroupName.Contains(keywords))
                || (x.CustomerContact != null && x.CustomerContact.Contains(keywords))
                || (x.CustomerPhone != null && x.CustomerPhone.Contains(keywords))
                || (x.CustomerPraise != null && x.CustomerPraise.Contains(keywords))
                || (x.CustomerFeedback != null && x.CustomerFeedback.Contains(keywords))
                || (x.ImprovementPlan != null && x.ImprovementPlan.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerSatisfactionSurveyCode))
        {
            var customerSatisfactionSurveyCode = queryDto.CustomerSatisfactionSurveyCode;
            exp = exp.And(x => x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(customerSatisfactionSurveyCode));
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

        if (queryDto?.SurveyMethod.HasValue == true)
        {
            var surveyMethod = queryDto.SurveyMethod.Value;
            exp = exp.And(x => x.SurveyMethod == surveyMethod);
        }

        if (queryDto?.SurveyType.HasValue == true)
        {
            var surveyType = queryDto.SurveyType.Value;
            exp = exp.And(x => x.SurveyType == surveyType);
        }

        if (queryDto?.SurveyPeriod.HasValue == true)
        {
            var surveyPeriod = queryDto.SurveyPeriod.Value;
            exp = exp.And(x => x.SurveyPeriod == surveyPeriod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SurveyorByGroupName))
        {
            var surveyorBy = queryDto.SurveyorByGroupName;
            exp = exp.And(x => x.SurveyorByGroupName != null && x.SurveyorByGroupName.Contains(surveyorBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerContact))
        {
            var customerContact = queryDto.CustomerContact;
            exp = exp.And(x => x.CustomerContact != null && x.CustomerContact.Contains(customerContact));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerPhone))
        {
            var customerPhone = queryDto.CustomerPhone;
            exp = exp.And(x => x.CustomerPhone != null && x.CustomerPhone.Contains(customerPhone));
        }

        if (queryDto?.OverallSatisfaction.HasValue == true)
        {
            var overallSatisfaction = queryDto.OverallSatisfaction.Value;
            exp = exp.And(x => x.OverallSatisfaction == overallSatisfaction);
        }

        if (queryDto?.TotalScore.HasValue == true)
        {
            var totalScore = queryDto.TotalScore.Value;
            exp = exp.And(x => x.TotalScore == totalScore);
        }

        if (queryDto?.QualityScore.HasValue == true)
        {
            var qualityScore = queryDto.QualityScore.Value;
            exp = exp.And(x => x.QualityScore == qualityScore);
        }

        if (queryDto?.DeliveryScore.HasValue == true)
        {
            var deliveryScore = queryDto.DeliveryScore.Value;
            exp = exp.And(x => x.DeliveryScore == deliveryScore);
        }

        if (queryDto?.ServiceScore.HasValue == true)
        {
            var serviceScore = queryDto.ServiceScore.Value;
            exp = exp.And(x => x.ServiceScore == serviceScore);
        }

        if (queryDto?.PriceScore.HasValue == true)
        {
            var priceScore = queryDto.PriceScore.Value;
            exp = exp.And(x => x.PriceScore == priceScore);
        }

        if (queryDto?.TechnicalScore.HasValue == true)
        {
            var technicalScore = queryDto.TechnicalScore.Value;
            exp = exp.And(x => x.TechnicalScore == technicalScore);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerPraise))
        {
            var customerPraise = queryDto.CustomerPraise;
            exp = exp.And(x => x.CustomerPraise != null && x.CustomerPraise.Contains(customerPraise));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerFeedback))
        {
            var customerFeedback = queryDto.CustomerFeedback;
            exp = exp.And(x => x.CustomerFeedback != null && x.CustomerFeedback.Contains(customerFeedback));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ImprovementPlan))
        {
            var improvementPlan = queryDto.ImprovementPlan;
            exp = exp.And(x => x.ImprovementPlan != null && x.ImprovementPlan.Contains(improvementPlan));
        }

        if (queryDto?.RelatedComplaintId.HasValue == true)
        {
            var relatedComplaintId = queryDto.RelatedComplaintId.Value;
            exp = exp.And(x => x.RelatedComplaintId == relatedComplaintId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Attachments))
        {
            var attachments = queryDto.Attachments;
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(attachments));
        }

        if (queryDto?.SurveyStatus.HasValue == true)
        {
            var surveyStatus = queryDto.SurveyStatus.Value;
            exp = exp.And(x => x.SurveyStatus == surveyStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.FollowUpStatus.HasValue == true)
        {
            var followUpStatus = queryDto.FollowUpStatus.Value;
            exp = exp.And(x => x.FollowUpStatus == followUpStatus);
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

        if (queryDto?.SurveyDateStart.HasValue == true)
        {
            var surveyDateStart = queryDto.SurveyDateStart.Value;
            exp = exp.And(x => x.SurveyDate >= surveyDateStart);
        }

        if (queryDto?.SurveyDateEnd.HasValue == true)
        {
            var surveyDateEnd = queryDto.SurveyDateEnd.Value;
            exp = exp.And(x => x.SurveyDate <= surveyDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktCustomerSatisfactionSurveyQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerSatisfactionSurveyCode))
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
        if (queryDto.SurveyMethod.HasValue)
        {
            return true;
        }
        if (queryDto.SurveyType.HasValue)
        {
            return true;
        }
        if (queryDto.SurveyPeriod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SurveyorByGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerContact))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerPhone))
        {
            return true;
        }
        if (queryDto.OverallSatisfaction.HasValue)
        {
            return true;
        }
        if (queryDto.TotalScore.HasValue)
        {
            return true;
        }
        if (queryDto.QualityScore.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryScore.HasValue)
        {
            return true;
        }
        if (queryDto.ServiceScore.HasValue)
        {
            return true;
        }
        if (queryDto.PriceScore.HasValue)
        {
            return true;
        }
        if (queryDto.TechnicalScore.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerPraise))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerFeedback))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ImprovementPlan))
        {
            return true;
        }
        if (queryDto.RelatedComplaintId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Attachments))
        {
            return true;
        }
        if (queryDto.SurveyStatus.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.FollowUpStatus.HasValue)
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
        if (queryDto.SurveyDateStart.HasValue || queryDto.SurveyDateEnd.HasValue)
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
