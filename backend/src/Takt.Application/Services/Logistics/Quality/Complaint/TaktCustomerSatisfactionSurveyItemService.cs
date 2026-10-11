// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查项目明细应用服务实现
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
/// 客户满意度调查项目明细应用服务
/// </summary>
public class TaktCustomerSatisfactionSurveyItemService : TaktServiceBase, ITaktCustomerSatisfactionSurveyItemService
{
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> _customerSatisfactionSurveyItemRepository;
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> _customerSatisfactionSurveyRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerSatisfactionSurveyItemRepository">客户满意度调查项目明细仓储</param>
    /// <param name="customerSatisfactionSurveyRepository">客户满意度调查仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerSatisfactionSurveyItemService(
        ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> customerSatisfactionSurveyItemRepository,
        ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> customerSatisfactionSurveyRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerSatisfactionSurveyItemRepository = customerSatisfactionSurveyItemRepository;
        _customerSatisfactionSurveyRepository = customerSatisfactionSurveyRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户满意度调查项目明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerSatisfactionSurveyItemDto>> GetCustomerSatisfactionSurveyItemListAsync(TaktCustomerSatisfactionSurveyItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerSatisfactionSurveyItemDto>.Create(
                new List<TaktCustomerSatisfactionSurveyItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerSatisfactionSurveyItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerSatisfactionSurveyItemDto>.Create(
            data.Adapt<List<TaktCustomerSatisfactionSurveyItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyItemDto?> GetCustomerSatisfactionSurveyItemByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerSatisfactionSurveyItemDto>();
    }

    /// <summary>
    /// 获取客户满意度调查项目明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerSatisfactionSurveyItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerSatisfactionSurveyItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FollowUpStatus == 1 && x.IsObsolete == 0,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerSatisfactionSurveyCode,
            DictLabel = e.ItemName ?? e.CustomerSatisfactionSurveyCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客户满意度调查项目明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyItemDto> CreateCustomerSatisfactionSurveyItemAsync(TaktCustomerSatisfactionSurveyItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerSatisfactionSurveyItem>();
        entity.IsObsolete = 0;
        await StampCustomerSatisfactionSurveyItemCustomerSatisfactionSurveyAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyItemRepository,
            x => x.SurveyId == entity.SurveyId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
        {
            throw new TaktBusinessException("客户满意度调查项目明细的SurveyId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _customerSatisfactionSurveyItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SurveyId == entity.SurveyId,
                x => x.LineNumber);
            var businessCode = entity.SurveyId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _customerSatisfactionSurveyItemRepository.CreateAsync(entity);
        return await GetCustomerSatisfactionSurveyItemByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerSatisfactionSurveyItemDto>();
    }

    /// <summary>
    /// 更新客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyItemDto> UpdateCustomerSatisfactionSurveyItemAsync(long id, TaktCustomerSatisfactionSurveyItemUpdateDto dto)
    {
        var entity = await _customerSatisfactionSurveyItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在");
        }
        dto.Adapt(entity);
        await StampCustomerSatisfactionSurveyItemCustomerSatisfactionSurveyAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyItemRepository,
            x => x.SurveyId == entity.SurveyId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
        {
            throw new TaktBusinessException("客户满意度调查项目明细的SurveyId、LineNumber已存在");
        }
        await _customerSatisfactionSurveyItemRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyItemByIdAsync(id) ?? throw new TaktBusinessException("客户满意度调查项目明细不存在");
    }

    /// <summary>
    /// 删除客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyItemByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("客户满意度调查项目明细已作废");
        }
        entity.IsObsolete = 1;
        await _customerSatisfactionSurveyItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除客户满意度调查项目明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerSatisfactionSurveyItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户满意度调查项目明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyItemDto> UpdateCustomerSatisfactionSurveyItemStatusAsync(TaktCustomerSatisfactionSurveyItemStatusDto dto)
    {
        var entity = await _customerSatisfactionSurveyItemRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在");
        }
        entity.FollowUpStatus = dto.FollowUpStatus;
        await _customerSatisfactionSurveyItemRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyItemByIdAsync(dto.CustomerSatisfactionSurveyItemId) ?? throw new TaktBusinessException("客户满意度调查项目明细不存在");
    }

    /// <summary>
    /// 更新客户满意度调查项目明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyItemDto> UpdateCustomerSatisfactionSurveyItemObsoleteAsync(TaktCustomerSatisfactionSurveyItemObsoleteDto dto)
    {
        var entity = await _customerSatisfactionSurveyItemRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("客户满意度调查项目明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _customerSatisfactionSurveyItemRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyItemByIdAsync(dto.CustomerSatisfactionSurveyItemId) ?? throw new TaktBusinessException("客户满意度调查项目明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerSatisfactionSurveyItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerSatisfactionSurveyItemTemplateDto>(
            sheetName ?? "客户满意度调查项目明细导入模板",
            fileName ?? "客户满意度调查项目明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户满意度调查项目明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerSatisfactionSurveyItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerSatisfactionSurveyItemImportDto>(fileStream, sheetName ?? "客户满意度调查项目明细导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerSatisfactionSurveyItem>();
                var importDto = rows[i].Adapt<TaktCustomerSatisfactionSurveyItemCreateDto>();
                await StampCustomerSatisfactionSurveyItemCustomerSatisfactionSurveyAsync(entity, importDto);
                var importKey = $"{entity.SurveyId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SurveyId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerSatisfactionSurveyItemRepository,
                    x => x.SurveyId == entity.SurveyId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
                {
                    throw new TaktBusinessException("客户满意度调查项目明细的SurveyId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _customerSatisfactionSurveyItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SurveyId == entity.SurveyId,
                        x => x.LineNumber);
                    var businessCode = entity.SurveyId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _customerSatisfactionSurveyItemRepository.CreateAsync(entity);
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
    /// 导出客户满意度调查项目明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerSatisfactionSurveyItemAsync(TaktCustomerSatisfactionSurveyItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerSatisfactionSurveyItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerSatisfactionSurveyItemExportDto>(),
                sheetName ?? "客户满意度调查项目明细数据",
                fileName ?? "客户满意度调查项目明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerSatisfactionSurveyItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerSatisfactionSurveyItemExportDto>(),
                sheetName ?? "客户满意度调查项目明细数据",
                fileName ?? "客户满意度调查项目明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerSatisfactionSurveyItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户满意度调查项目明细数据",
            fileName ?? "客户满意度调查项目明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步客户满意度调查项目明细主表外键（ManyToOne → 客户满意度调查）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerSatisfactionSurveyItemCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyItem entity, TaktCustomerSatisfactionSurveyItemCreateDto dto)
    {
        if (dto.SurveyId <= 0)
        {
            return;
        }
        var master = await _customerSatisfactionSurveyRepository.GetByIdAsync(dto.SurveyId);
        if (master == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        entity.SurveyId = master.Id;
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
        if (string.IsNullOrEmpty(entity.CustomerSatisfactionSurveyCode))
        {
            entity.CustomerSatisfactionSurveyCode = master.CustomerSatisfactionSurveyCode;
        }
        if (string.IsNullOrEmpty(entity.CustomerFeedback))
        {
            entity.CustomerFeedback = master.CustomerFeedback;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户满意度调查项目明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerSatisfactionSurveyItem, bool>> QueryExpression(TaktCustomerSatisfactionSurveyItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerSatisfactionSurveyItem>();

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
                || (x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.ItemDescription != null && x.ItemDescription.Contains(keywords))
                || (x.CustomerFeedback != null && x.CustomerFeedback.Contains(keywords))
                || (x.ImprovementSuggestion != null && x.ImprovementSuggestion.Contains(keywords))
                || (x.FollowUpAction != null && x.FollowUpAction.Contains(keywords))
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

        if (queryDto?.SurveyId.HasValue == true)
        {
            var surveyId = queryDto.SurveyId.Value;
            exp = exp.And(x => x.SurveyId == surveyId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerSatisfactionSurveyCode))
        {
            var customerSatisfactionSurveyCode = queryDto.CustomerSatisfactionSurveyCode;
            exp = exp.And(x => x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(customerSatisfactionSurveyCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.CategoryType.HasValue == true)
        {
            var categoryType = queryDto.CategoryType.Value;
            exp = exp.And(x => x.CategoryType == categoryType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemName))
        {
            var itemName = queryDto.ItemName;
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(itemName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemDescription))
        {
            var itemDescription = queryDto.ItemDescription;
            exp = exp.And(x => x.ItemDescription != null && x.ItemDescription.Contains(itemDescription));
        }

        if (queryDto?.Weight.HasValue == true)
        {
            var weight = queryDto.Weight.Value;
            exp = exp.And(x => x.Weight == weight);
        }

        if (queryDto?.Score.HasValue == true)
        {
            var score = queryDto.Score.Value;
            exp = exp.And(x => x.Score == score);
        }

        if (queryDto?.SatisfactionLevel.HasValue == true)
        {
            var satisfactionLevel = queryDto.SatisfactionLevel.Value;
            exp = exp.And(x => x.SatisfactionLevel == satisfactionLevel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerFeedback))
        {
            var customerFeedback = queryDto.CustomerFeedback;
            exp = exp.And(x => x.CustomerFeedback != null && x.CustomerFeedback.Contains(customerFeedback));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ImprovementSuggestion))
        {
            var improvementSuggestion = queryDto.ImprovementSuggestion;
            exp = exp.And(x => x.ImprovementSuggestion != null && x.ImprovementSuggestion.Contains(improvementSuggestion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FollowUpAction))
        {
            var followUpAction = queryDto.FollowUpAction;
            exp = exp.And(x => x.FollowUpAction != null && x.FollowUpAction.Contains(followUpAction));
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
    private static bool HasAnyListQueryFilter(TaktCustomerSatisfactionSurveyItemQueryDto? queryDto)
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
        if (queryDto.SurveyId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerSatisfactionSurveyCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.CategoryType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemDescription))
        {
            return true;
        }
        if (queryDto.Weight.HasValue)
        {
            return true;
        }
        if (queryDto.Score.HasValue)
        {
            return true;
        }
        if (queryDto.SatisfactionLevel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerFeedback))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ImprovementSuggestion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FollowUpAction))
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
        if (queryDto.IsObsolete.HasValue)
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
