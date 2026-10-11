// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良明细应用服务实现
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
/// 组立不良明细应用服务
/// </summary>
public class TaktAssyDefectDetailService : TaktServiceBase, ITaktAssyDefectDetailService
{
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyDefectDetailService(
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyDefectDetailRepository = assyDefectDetailRepository;
        _assyDefectRepository = assyDefectRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立不良明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyDefectDetailDto>> GetAssyDefectDetailListAsync(TaktAssyDefectDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAssyDefectDetailDto>.Create(
                new List<TaktAssyDefectDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyDefectDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyDefectDetailDto>.Create(
            data.Adapt<List<TaktAssyDefectDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto?> GetAssyDefectDetailByIdAsync(long id)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssyDefectDetailDto>();
    }

    /// <summary>
    /// 获取组立不良明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyDefectDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _assyDefectDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建组立不良明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto> CreateAssyDefectDetailAsync(TaktAssyDefectDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyDefectDetail>();
        entity.IsObsolete = 0;
        await StampAssyDefectDetailAssyDefectAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectDetailRepository,
            x => x.AssyDefectId == entity.AssyDefectId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _assyDefectDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyDefectId == entity.AssyDefectId,
                x => x.LineNumber);
            var businessCode = entity.AssyDefectId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _assyDefectDetailRepository.CreateAsync(entity);
        return await GetAssyDefectDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyDefectDetailDto>();
    }

    /// <summary>
    /// 更新组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto> UpdateAssyDefectDetailAsync(long id, TaktAssyDefectDetailUpdateDto dto)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良明细不存在");
        }
        dto.Adapt(entity);
        await StampAssyDefectDetailAssyDefectAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectDetailRepository,
            x => x.AssyDefectId == entity.AssyDefectId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
        }
        await _assyDefectDetailRepository.UpdateAsync(entity);
        return await GetAssyDefectDetailByIdAsync(id) ?? throw new TaktBusinessException("组立不良明细不存在");
    }

    /// <summary>
    /// 删除组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectDetailByIdAsync(long id)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("组立不良明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("组立不良明细已作废");
        }
        entity.IsObsolete = 1;
        await _assyDefectDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除组立不良明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyDefectDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新组立不良明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto> UpdateAssyDefectDetailObsoleteAsync(TaktAssyDefectDetailObsoleteDto dto)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(dto.AssyDefectDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("组立不良明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _assyDefectDetailRepository.UpdateAsync(entity);
        return await GetAssyDefectDetailByIdAsync(dto.AssyDefectDetailId) ?? throw new TaktBusinessException("组立不良明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyDefectDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyDefectDetailTemplateDto>(
            sheetName ?? "组立不良明细导入模板",
            fileName ?? "组立不良明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立不良明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyDefectDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyDefectDetailImportDto>(fileStream, sheetName ?? "组立不良明细导入模板");
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
                var entity = rows[i].Adapt<TaktAssyDefectDetail>();
                var importDto = rows[i].Adapt<TaktAssyDefectDetailCreateDto>();
                await StampAssyDefectDetailAssyDefectAsync(entity, importDto);
                var importKey = $"{entity.AssyDefectId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssyDefectId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyDefectDetailRepository,
                    x => x.AssyDefectId == entity.AssyDefectId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
                {
                    throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _assyDefectDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyDefectId == entity.AssyDefectId,
                        x => x.LineNumber);
                    var businessCode = entity.AssyDefectId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _assyDefectDetailRepository.CreateAsync(entity);
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
    /// 导出组立不良明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyDefectDetailAsync(TaktAssyDefectDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAssyDefectDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectDetailExportDto>(),
                sheetName ?? "组立不良明细数据",
                fileName ?? "组立不良明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _assyDefectDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectDetailExportDto>(),
                sheetName ?? "组立不良明细数据",
                fileName ?? "组立不良明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyDefectDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立不良明细数据",
            fileName ?? "组立不良明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步组立不良明细主表外键（ManyToOne → 组立不良日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampAssyDefectDetailAssyDefectAsync(TaktAssyDefectDetail entity, TaktAssyDefectDetailCreateDto dto)
    {
        if (dto.AssyDefectId <= 0)
        {
            return;
        }
        var master = await _assyDefectRepository.GetByIdAsync(dto.AssyDefectId);
        if (master == null)
        {
            throw new TaktBusinessException("组立不良日报不存在");
        }
        entity.AssyDefectId = master.Id;
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
        if (string.IsNullOrEmpty(entity.ProdOrderCode))
        {
            entity.ProdOrderCode = master.ProdOrderCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立不良明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyDefectDetail, bool>> QueryExpression(TaktAssyDefectDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyDefectDetail>();

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
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.DefectCategory != null && x.DefectCategory.Contains(keywords))
                || (x.RandomCardCode != null && x.RandomCardCode.Contains(keywords))
                || (x.OccurrenceEngineering != null && x.OccurrenceEngineering.Contains(keywords))
                || (x.TestStep != null && x.TestStep.Contains(keywords))
                || (x.DefectSymptom != null && x.DefectSymptom.Contains(keywords))
                || (x.DefectLocation != null && x.DefectLocation.Contains(keywords))
                || (x.DefectReason != null && x.DefectReason.Contains(keywords))
                || (x.RepairOperatorName != null && x.RepairOperatorName.Contains(keywords))
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

        if (queryDto?.AssyDefectId.HasValue == true)
        {
            var assyDefectId = queryDto.AssyDefectId.Value;
            exp = exp.And(x => x.AssyDefectId == assyDefectId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
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

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectCategory))
        {
            var defectCategory = queryDto.DefectCategory;
            exp = exp.And(x => x.DefectCategory != null && x.DefectCategory.Contains(defectCategory));
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            var defectQty = queryDto.DefectQty.Value;
            exp = exp.And(x => x.DefectQty == defectQty);
        }

        if (queryDto?.CumulativeDefectQty.HasValue == true)
        {
            var cumulativeDefectQty = queryDto.CumulativeDefectQty.Value;
            exp = exp.And(x => x.CumulativeDefectQty == cumulativeDefectQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RandomCardCode))
        {
            var randomCardCode = queryDto.RandomCardCode;
            exp = exp.And(x => x.RandomCardCode != null && x.RandomCardCode.Contains(randomCardCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OccurrenceEngineering))
        {
            var occurrenceEngineering = queryDto.OccurrenceEngineering;
            exp = exp.And(x => x.OccurrenceEngineering != null && x.OccurrenceEngineering.Contains(occurrenceEngineering));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TestStep))
        {
            var testStep = queryDto.TestStep;
            exp = exp.And(x => x.TestStep != null && x.TestStep.Contains(testStep));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectSymptom))
        {
            var defectSymptom = queryDto.DefectSymptom;
            exp = exp.And(x => x.DefectSymptom != null && x.DefectSymptom.Contains(defectSymptom));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectLocation))
        {
            var defectLocation = queryDto.DefectLocation;
            exp = exp.And(x => x.DefectLocation != null && x.DefectLocation.Contains(defectLocation));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectReason))
        {
            var defectReason = queryDto.DefectReason;
            exp = exp.And(x => x.DefectReason != null && x.DefectReason.Contains(defectReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RepairOperatorName))
        {
            var repairOperator = queryDto.RepairOperatorName;
            exp = exp.And(x => x.RepairOperatorName != null && x.RepairOperatorName.Contains(repairOperator));
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
    private static bool HasAnyListQueryFilter(TaktAssyDefectDetailQueryDto? queryDto)
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
        if (queryDto.AssyDefectId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
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
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectCategory))
        {
            return true;
        }
        if (queryDto.DefectQty.HasValue)
        {
            return true;
        }
        if (queryDto.CumulativeDefectQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RandomCardCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OccurrenceEngineering))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TestStep))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectSymptom))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectLocation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RepairOperatorName))
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
