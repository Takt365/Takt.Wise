// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowFormService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Workflow;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程表单应用服务
/// </summary>
public class TaktFlowFormService : TaktServiceBase, ITaktFlowFormService
{
    private readonly ITaktCompanyRepository<TaktFlowForm> _flowFormRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowFormRepository">流程表单仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowFormService(
        ITaktCompanyRepository<TaktFlowForm> flowFormRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowFormRepository = flowFormRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取流程表单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowFormDto>> GetFlowFormListAsync(TaktFlowFormQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowFormRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowFormDto>.Create(
            data.Adapt<List<TaktFlowFormDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowFormDto?> GetFlowFormByIdAsync(long id)
    {
        var entity = await _flowFormRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowFormDto>();
    }

    /// <summary>
    /// 获取流程表单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowFormOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowFormRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FormName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FormName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建流程表单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowFormDto> CreateFlowFormAsync(TaktFlowFormCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowForm>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _flowFormRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.FormCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.FormCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.FormCode))
        {
            throw new TaktBusinessException("表单编码不能为空");
        }
        var isUnique_ix_flow_form_code_unique = await _uniqueValidator.IsUniqueAsync(
            _flowFormRepository,
            x => x.FormCode == entity.FormCode);
        if (!isUnique_ix_flow_form_code_unique)
        {
            throw new TaktBusinessException("流程表单的FormCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _flowFormRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _flowFormRepository.CreateAsync(entity);
        return await GetFlowFormByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowFormDto>();
    }

    /// <summary>
    /// 更新流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowFormDto> UpdateFlowFormAsync(long id, TaktFlowFormUpdateDto dto)
    {
        var entity = await _flowFormRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程表单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_flow_form_code_unique = await _uniqueValidator.IsUniqueAsync(
            _flowFormRepository,
            x => x.FormCode == entity.FormCode,
            id);
        if (!isUnique_ix_flow_form_code_unique)
        {
            throw new TaktBusinessException("流程表单的FormCode已存在");
        }
        await _flowFormRepository.UpdateAsync(entity);
        return await GetFlowFormByIdAsync(id) ?? throw new TaktBusinessException("流程表单不存在");
    }

    /// <summary>
    /// 删除流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowFormByIdAsync(long id)
    {
        var deleted = await _flowFormRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程表单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程表单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowFormBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowFormByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新流程表单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowFormDto> UpdateFlowFormStatusAsync(TaktFlowFormStatusDto dto)
    {
        var entity = await _flowFormRepository.GetByIdAsync(dto.FlowFormId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程表单不存在");
        }
        entity.FormStatus = dto.FormStatus;
        await _flowFormRepository.UpdateAsync(entity);
        return await GetFlowFormByIdAsync(dto.FlowFormId) ?? throw new TaktBusinessException("流程表单不存在");
    }

    /// <summary>
    /// 更新流程表单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowFormDto> UpdateFlowFormSortAsync(TaktFlowFormSortDto dto)
    {
        var entity = await _flowFormRepository.GetByIdAsync(dto.FlowFormId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程表单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _flowFormRepository.UpdateAsync(entity);
        return await GetFlowFormByIdAsync(dto.FlowFormId) ?? throw new TaktBusinessException("流程表单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowFormTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowFormTemplateDto>(
            sheetName ?? "流程表单导入模板",
            fileName ?? "流程表单导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程表单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowFormAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowFormImportDto>(fileStream, sheetName ?? "流程表单导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _flowFormRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowForm>();
                var importKey = $"{entity.FormCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FormCode）");
                }
                var isUnique_ix_flow_form_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _flowFormRepository,
                    x => x.FormCode == entity.FormCode);
                if (!isUnique_ix_flow_form_code_unique)
                {
                    throw new TaktBusinessException("流程表单的FormCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _flowFormRepository.CreateAsync(entity);
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
    /// 导出流程表单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowFormAsync(TaktFlowFormQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowFormQueryDto());
        var list = await _flowFormRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowFormExportDto>(),
                sheetName ?? "流程表单数据",
                fileName ?? "流程表单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowFormExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程表单数据",
            fileName ?? "流程表单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程表单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowForm, bool>> QueryExpression(TaktFlowFormQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowForm>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.FormCode != null && x.FormCode.Contains(keywords))
                || (x.FormName != null && x.FormName.Contains(keywords))
                || SqlFunc.ToString(x.FormCategory).Contains(keywords)
                || SqlFunc.ToString(x.FormType).Contains(keywords)
                || (x.FormConfig != null && x.FormConfig.Contains(keywords))
                || (x.FormTemplate != null && x.FormTemplate.Contains(keywords))
                || (x.FormVersion != null && x.FormVersion.Contains(keywords))
                || SqlFunc.ToString(x.IsDatasource).Contains(keywords)
                || (x.RelatedDataBaseName != null && x.RelatedDataBaseName.Contains(keywords))
                || (x.RelatedTableName != null && x.RelatedTableName.Contains(keywords))
                || (x.RelatedFormField != null && x.RelatedFormField.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.FormStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FormCode))
        {
            exp = exp.And(x => x.FormCode != null && x.FormCode.Contains(queryDto.FormCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormName))
        {
            exp = exp.And(x => x.FormName != null && x.FormName.Contains(queryDto.FormName));
        }

        if (queryDto?.FormCategory.HasValue == true)
        {
            exp = exp.And(x => x.FormCategory == queryDto.FormCategory);
        }

        if (queryDto?.FormType.HasValue == true)
        {
            exp = exp.And(x => x.FormType == queryDto.FormType);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormConfig))
        {
            exp = exp.And(x => x.FormConfig != null && x.FormConfig.Contains(queryDto.FormConfig));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormTemplate))
        {
            exp = exp.And(x => x.FormTemplate != null && x.FormTemplate.Contains(queryDto.FormTemplate));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormVersion))
        {
            exp = exp.And(x => x.FormVersion != null && x.FormVersion.Contains(queryDto.FormVersion));
        }

        if (queryDto?.IsDatasource.HasValue == true)
        {
            exp = exp.And(x => x.IsDatasource == queryDto.IsDatasource);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedDataBaseName))
        {
            exp = exp.And(x => x.RelatedDataBaseName != null && x.RelatedDataBaseName.Contains(queryDto.RelatedDataBaseName));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedTableName))
        {
            exp = exp.And(x => x.RelatedTableName != null && x.RelatedTableName.Contains(queryDto.RelatedTableName));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedFormField))
        {
            exp = exp.And(x => x.RelatedFormField != null && x.RelatedFormField.Contains(queryDto.RelatedFormField));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.FormStatus.HasValue == true)
        {
            exp = exp.And(x => x.FormStatus == queryDto.FormStatus);
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
