// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktNumberingService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：编码规则应用服务实现（CRUD + 导入导出）
// · 新增/导入/更新：服务端按规则自动生成或刷新起始编码 StartingNumber
// · 运行时取号：业务模块注入 ITaktNumberingGenerator（按业务表编码 max；本服务不维护流水计数）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 编码规则应用服务
/// </summary>
public class TaktNumberingService : TaktServiceBase, ITaktNumberingService
{
    private readonly ITaktCompanyRepository<TaktNumbering> _numberingRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编码规则仓储</param>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">运行时编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNumberingService(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _numberingRepository = numberingRepository;
        _companyRepository = companyRepository;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取编码规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNumberingDto>> GetNumberingListAsync(TaktNumberingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _numberingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNumberingDto>.Create(
            data.Adapt<List<TaktNumberingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto?> GetNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 预览下一个业务编码（不占用流水号、不写库）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    public async Task<TaktNumberingPreviewDto> PreviewNumberingNextAsync(
        string ruleCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        var model = await _numberingGenerator.PreviewNextAsync(ruleCode.Trim(), cancellationToken);
        return new TaktNumberingPreviewDto
        {
            RuleCode = model.RuleCode ?? ruleCode.Trim(),
            BusinessCode = model.BusinessCode ?? string.Empty,
        };
    }

    /// <summary>
    /// 获取编码规则选项列表
    /// </summary>
    /// <param name="documentType">单据类型（TaktMenu.MenuName）；有值时仅返回该类型下启用规则</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNumberingOptionsAsync(string? plantCode = null, string? keyword = null, string? documentType = null)
    {
        EnsureThreeLayerContext();
        var docType = documentType?.Trim();
        var list = await _numberingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.NumberingStatus == 1
                && (string.IsNullOrEmpty(docType) || x.DocumentType == docType),
            x => x.RuleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.RuleCode,
            DictLabel = string.IsNullOrWhiteSpace(e.RuleName) ? e.RuleCode : $"{e.RuleName} ({e.RuleCode})",
            ExtValue = e.StartingNumber,
        }).ToList();
    }

    /// <summary>
    /// 创建编码规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> CreateNumberingAsync(TaktNumberingCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktNumbering>();
        entity.TenantCode = CurrentTenantCode;
        entity.CompanyCode = CurrentCompanyCode;
        entity.IsBuiltIn = 0;
        NormalizeNumberingEntity(entity);
        entity.StartingNumber = TaktNumberingHelper.BuildInitialStartingNumber(
            await ToModelAsync(entity),
            DateTime.Now);
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编码规则的RuleCode已存在");
        }
        entity = await _numberingRepository.CreateAsync(entity);
        return await GetNumberingByIdAsync(entity.Id) ?? entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 更新编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingAsync(long id, TaktNumberingUpdateDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编码规则不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        NormalizeNumberingEntity(entity);
        var step = entity.SequenceStep <= 0 ? 1 : entity.SequenceStep;
        entity.StartingNumber = TaktNumberingHelper.FormatBusinessCode(
            await ToModelAsync(entity),
            step,
            DateTime.Now);
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode,
            id);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编码规则的RuleCode已存在");
        }
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(id) ?? throw new TaktBusinessException("编码规则不存在");
    }

    /// <summary>
    /// 删除编码规则
    /// </summary>
    /// <param name="id">编码规则ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编码规则不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置编码规则不允许删除");
        }
        var deleted = await _numberingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("编码规则不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除编码规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _numberingRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置编码规则不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteNumberingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新编码规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingStatusAsync(TaktNumberingStatusDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(dto.NumberingId);
        if (entity == null)
        {
            throw new TaktBusinessException("编码规则不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.NumberingStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置编码规则");
        }
        entity.NumberingStatus = dto.NumberingStatus;
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(dto.NumberingId) ?? throw new TaktBusinessException("编码规则不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNumberingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNumberingTemplateDto>(
            sheetName ?? "编码规则导入模板",
            fileName ?? "编码规则导入模板.xlsx");
    }

    /// <summary>
    /// 导入编码规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNumberingAsync(Stream fileStream, string? sheetName = null)
    {
        EnsureThreeLayerContext();
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNumberingImportDto>(fileStream, sheetName ?? "编码规则导入模板");
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
                var entity = rows[i].Adapt<TaktNumbering>();
                entity.IsBuiltIn = 0;
                entity.TenantCode = CurrentTenantCode;
                entity.CompanyCode = CurrentCompanyCode;
                NormalizeNumberingEntity(entity);
                await ApplyStartingNumberOnCreateAsync(entity, rows[i].StartingNumber, autoGenerateWhenEmpty: true);
                var importKey = $"{entity.RuleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RuleCode）");
                }
                var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _numberingRepository,
                    x => x.RuleCode == entity.RuleCode);
                if (!isUnique_ix_numbering_code_unique)
                {
                    throw new TaktBusinessException("编码规则的RuleCode已存在");
                }
                await _numberingRepository.CreateAsync(entity);
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
    /// 导出编码规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNumberingAsync(TaktNumberingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNumberingQueryDto());
        var list = await _numberingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNumberingExportDto>(),
                sheetName ?? "编码规则数据",
                fileName ?? "编码规则导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNumberingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "编码规则数据",
            fileName ?? "编码规则导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建编码规则查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNumbering, bool>> QueryExpression(TaktNumberingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNumbering>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RuleCode != null && x.RuleCode.Contains(keywords))
                || (x.RuleName != null && x.RuleName.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || (x.PrefixCode != null && x.PrefixCode.Contains(keywords))
                || (x.DateFormat != null && x.DateFormat.Contains(keywords))
                || SqlFunc.ToString(x.SequenceLength).Contains(keywords)
                || SqlFunc.ToString(x.SequenceStep).Contains(keywords)
                || (x.SuffixCode != null && x.SuffixCode.Contains(keywords))
                || (x.ResetPeriod != null && x.ResetPeriod.Contains(keywords))
                || x.StartingNumber.Contains(keywords)
                || (x.Separator != null && x.Separator.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.NumberingStatus).Contains(keywords)
                || (x.NumberingDescription != null && x.NumberingDescription.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleCode))
        {
            exp = exp.And(x => x.RuleCode != null && x.RuleCode.Contains(queryDto.RuleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleName))
        {
            exp = exp.And(x => x.RuleName != null && x.RuleName.Contains(queryDto.RuleName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentType))
        {
            exp = exp.And(x => x.DocumentType == queryDto.DocumentType.Trim());
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptCode))
        {
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(queryDto.DeptCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PrefixCode))
        {
            exp = exp.And(x => x.PrefixCode != null && x.PrefixCode.Contains(queryDto.PrefixCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DateFormat))
        {
            exp = exp.And(x => x.DateFormat != null && x.DateFormat.Contains(queryDto.DateFormat));
        }

        if (queryDto?.SequenceLength.HasValue == true)
        {
            exp = exp.And(x => x.SequenceLength == queryDto.SequenceLength);
        }

        if (queryDto?.SequenceStep.HasValue == true)
        {
            exp = exp.And(x => x.SequenceStep == queryDto.SequenceStep);
        }

        if (!string.IsNullOrEmpty(queryDto?.SuffixCode))
        {
            exp = exp.And(x => x.SuffixCode != null && x.SuffixCode.Contains(queryDto.SuffixCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResetPeriod))
        {
            exp = exp.And(x => x.ResetPeriod != null && x.ResetPeriod.Contains(queryDto.ResetPeriod));
        }

        if (!string.IsNullOrEmpty(queryDto?.StartingNumber))
        {
            exp = exp.And(x => x.StartingNumber.Contains(queryDto.StartingNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.Separator))
        {
            exp = exp.And(x => x.Separator != null && x.Separator.Contains(queryDto.Separator));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.NumberingStatus.HasValue == true)
        {
            exp = exp.And(x => x.NumberingStatus == queryDto.NumberingStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.NumberingDescription))
        {
            exp = exp.And(x => x.NumberingDescription != null && x.NumberingDescription.Contains(queryDto.NumberingDescription));
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

    // ========================================
    // 编码规则规范化（Create/Update/Import 共用）
    // ========================================

    /// <summary>
    /// 实体转编码模型（不含公司简称；仅供 Normalize）
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    /// <returns>编码模型</returns>
    private static TaktNumberingModel ToModel(TaktNumbering entity) => new()
    {
        RuleCode = entity.RuleCode,
        BusinessCode = entity.StartingNumber,
        CompanyCode = entity.CompanyCode,
        DeptCode = entity.DeptCode,
        PrefixCode = entity.PrefixCode,
        DateFormat = entity.DateFormat,
        SequenceLength = entity.SequenceLength,
        SequenceStep = entity.SequenceStep,
        SuffixCode = entity.SuffixCode,
        ResetPeriod = entity.ResetPeriod,
        Separator = entity.Separator,
        Description = entity.NumberingDescription,
        DocumentType = entity.DocumentType,
        UpdatedAt = entity.UpdatedAt,
    };

    /// <summary>
    /// 实体转编码模型（含当前公司简称，供起始编码/取号样例拼接）
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    /// <returns>编码模型</returns>
    private async Task<TaktNumberingModel> ToModelAsync(TaktNumbering entity)
    {
        var model = ToModel(entity);
        model.CompanyShortName = await ResolveCompanyShortNameAsync(entity.CompanyCode);
        return model;
    }

    /// <summary>
    /// 按公司代码解析公司简称（无简称时回退公司代码）
    /// </summary>
    /// <param name="companyCode">公司代码</param>
    /// <returns>公司简称或公司代码</returns>
    private async Task<string> ResolveCompanyShortNameAsync(string? companyCode)
    {
        var code = companyCode?.Trim();
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return code ?? string.Empty;
        }
        var list = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == code);
        var company = list.FirstOrDefault();
        if (company == null || string.IsNullOrWhiteSpace(company.CompanyShortName))
        {
            return code;
        }
        return company.CompanyShortName.Trim();
    }

    /// <summary>
    /// 规范化编码规则实体默认值
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    private static void NormalizeNumberingEntity(TaktNumbering entity)
    {
        var model = ToModel(entity);
        TaktNumberingHelper.NormalizeNumberingModel(model);
        entity.SequenceLength = model.SequenceLength;
        entity.SequenceStep = model.SequenceStep;
        entity.ResetPeriod = model.ResetPeriod;
        entity.DateFormat = model.DateFormat;
        entity.NumberingDescription = model.Description;
        entity.DocumentType = model.DocumentType;
        entity.Separator = model.Separator;
    }

    /// <summary>
    /// 创建/导入时写入起始编码样例（公司段用简称）
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    /// <param name="startingNumberInput">起始编码</param>
    /// <param name="autoGenerateWhenEmpty">为空时是否自动生成</param>
    private async Task ApplyStartingNumberOnCreateAsync(
        TaktNumbering entity,
        string? startingNumberInput,
        bool autoGenerateWhenEmpty)
    {
        entity.StartingNumber = TaktNumberingHelper.ResolveStartingNumberOnCreate(
            await ToModelAsync(entity),
            startingNumberInput,
            autoGenerateWhenEmpty);
    }
}