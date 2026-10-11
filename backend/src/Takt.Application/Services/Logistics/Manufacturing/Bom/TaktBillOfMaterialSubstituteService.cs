// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstituteService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM替代料应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM替代料应用服务
/// </summary>
public class TaktBillOfMaterialSubstituteService : TaktServiceBase, ITaktBillOfMaterialSubstituteService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterialSubstitute> _billOfMaterialSubstituteRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialSubstituteRepository">BOM替代料仓储</param>
    /// <param name="billOfMaterialItemRepository">物料清单明细仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialSubstituteService(
        ITaktCompanyRepository<TaktBillOfMaterialSubstitute> billOfMaterialSubstituteRepository,
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialSubstituteRepository = billOfMaterialSubstituteRepository;
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM替代料列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialSubstituteDto>> GetBillOfMaterialSubstituteListAsync(TaktBillOfMaterialSubstituteQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBillOfMaterialSubstituteDto>.Create(
                new List<TaktBillOfMaterialSubstituteDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialSubstituteRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialSubstituteDto>.Create(
            data.Adapt<List<TaktBillOfMaterialSubstituteDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto?> GetBillOfMaterialSubstituteByIdAsync(long id)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBillOfMaterialSubstituteDto>();
    }

    /// <summary>
    /// 获取BOM替代料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialSubstituteOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialSubstituteRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.BomCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BomCode,
            DictLabel = e.BomCode,
        }).ToList();
    }

    /// <summary>
    /// 创建BOM替代料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> CreateBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterialSubstitute>();
        entity.IsObsolete = 0;
        await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, dto);
        await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialSubstituteRepository,
            x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                && x.SubstituteMaterialId == entity.SubstituteMaterialId);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
        {
            throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _billOfMaterialSubstituteRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialItemId == entity.BillOfMaterialItemId,
                x => x.LineNumber);
            var businessCode = entity.BillOfMaterialItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _billOfMaterialSubstituteRepository.CreateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialSubstituteDto>();
    }

    /// <summary>
    /// 更新BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteAsync(long id, TaktBillOfMaterialSubstituteUpdateDto dto)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        dto.Adapt(entity);
        await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, dto);
        await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialSubstituteRepository,
            x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                && x.SubstituteMaterialId == entity.SubstituteMaterialId,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
        {
            throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
        }
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(id) ?? throw new TaktBusinessException("BOM替代料不存在");
    }

    /// <summary>
    /// 删除BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialSubstituteByIdAsync(long id)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("BOM替代料不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("BOM替代料已作废");
        }
        entity.IsObsolete = 1;
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除BOM替代料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialSubstituteBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialSubstituteByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新BOM替代料作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteObsoleteAsync(TaktBillOfMaterialSubstituteObsoleteDto dto)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(dto.BillOfMaterialSubstituteId);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(dto.BillOfMaterialSubstituteId) ?? throw new TaktBusinessException("BOM替代料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialSubstituteTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialSubstituteTemplateDto>(
            sheetName ?? "BOM替代料导入模板",
            fileName ?? "BOM替代料导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM替代料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialSubstituteAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialSubstituteImportDto>(fileStream, sheetName ?? "BOM替代料导入模板");
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
                var entity = rows[i].Adapt<TaktBillOfMaterialSubstitute>();
                var importDto = rows[i].Adapt<TaktBillOfMaterialSubstituteCreateDto>();
                await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, importDto);
                await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, importDto);
                var importKey = $"{entity.BillOfMaterialItemId}|{entity.SubstituteMaterialId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（BillOfMaterialItemId、SubstituteMaterialId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialSubstituteRepository,
                    x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                        && x.SubstituteMaterialId == entity.SubstituteMaterialId);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
                {
                    throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _billOfMaterialSubstituteRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialItemId == entity.BillOfMaterialItemId,
                        x => x.LineNumber);
                    var businessCode = entity.BillOfMaterialItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _billOfMaterialSubstituteRepository.CreateAsync(entity);
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
    /// 导出BOM替代料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBillOfMaterialSubstituteQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialSubstituteExportDto>(),
                sheetName ?? "BOM替代料数据",
                fileName ?? "BOM替代料导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _billOfMaterialSubstituteRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialSubstituteExportDto>(),
                sheetName ?? "BOM替代料数据",
                fileName ?? "BOM替代料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialSubstituteExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM替代料数据",
            fileName ?? "BOM替代料导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步BOM替代料主表外键（ManyToOne → 物料清单明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialSubstituteBillOfMaterialItemAsync(TaktBillOfMaterialSubstitute entity, TaktBillOfMaterialSubstituteCreateDto dto)
    {
        if (dto.BillOfMaterialItemId <= 0)
        {
            return;
        }
        var master = await _billOfMaterialItemRepository.GetByIdAsync(dto.BillOfMaterialItemId);
        if (master == null)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        entity.BillOfMaterialItemId = master.Id;
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
        if (string.IsNullOrEmpty(entity.BomCode))
        {
            entity.BomCode = master.BomCode;
        }
        if (string.IsNullOrEmpty(entity.SubstituteGroup))
        {
            entity.SubstituteGroup = master.SubstituteGroup ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.MaterialUnit))
        {
            entity.MaterialUnit = master.MaterialUnit;
        }
    }

    /// <summary>
    /// 同步BOM替代料主表外键（ManyToOne → 工厂物料）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialSubstituteMaterialPlantAsync(TaktBillOfMaterialSubstitute entity, TaktBillOfMaterialSubstituteCreateDto dto)
    {
        if (dto.SubstituteMaterialId <= 0)
        {
            return;
        }
        var master = await _materialPlantRepository.GetByIdAsync(dto.SubstituteMaterialId);
        if (master == null)
        {
            throw new TaktBusinessException("工厂物料不存在");
        }
        entity.SubstituteMaterialId = master.Id;
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
    /// 构建BOM替代料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterialSubstitute, bool>> QueryExpression(TaktBillOfMaterialSubstituteQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterialSubstitute>();

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
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.PrimaryMaterialCode != null && x.PrimaryMaterialCode.Contains(keywords))
                || (x.SubstituteMaterialCode != null && x.SubstituteMaterialCode.Contains(keywords))
                || (x.SubstituteGroup != null && x.SubstituteGroup.Contains(keywords))
                || (x.MaterialUnit != null && x.MaterialUnit.Contains(keywords))
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

        if (queryDto?.BillOfMaterialItemId.HasValue == true)
        {
            var billOfMaterialItemId = queryDto.BillOfMaterialItemId.Value;
            exp = exp.And(x => x.BillOfMaterialItemId == billOfMaterialItemId);
        }

        if (queryDto?.BillOfMaterialId.HasValue == true)
        {
            var billOfMaterialId = queryDto.BillOfMaterialId.Value;
            exp = exp.And(x => x.BillOfMaterialId == billOfMaterialId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomCode))
        {
            var bomCode = queryDto.BomCode;
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(bomCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PrimaryMaterialCode))
        {
            var primaryMaterialCode = queryDto.PrimaryMaterialCode;
            exp = exp.And(x => x.PrimaryMaterialCode != null && x.PrimaryMaterialCode.Contains(primaryMaterialCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.SubstituteMaterialId.HasValue == true)
        {
            var substituteMaterialId = queryDto.SubstituteMaterialId.Value;
            exp = exp.And(x => x.SubstituteMaterialId == substituteMaterialId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubstituteMaterialCode))
        {
            var substituteMaterialCode = queryDto.SubstituteMaterialCode;
            exp = exp.And(x => x.SubstituteMaterialCode != null && x.SubstituteMaterialCode.Contains(substituteMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubstituteGroup))
        {
            var substituteGroup = queryDto.SubstituteGroup;
            exp = exp.And(x => x.SubstituteGroup != null && x.SubstituteGroup.Contains(substituteGroup));
        }

        if (queryDto?.SubstitutePriority.HasValue == true)
        {
            var substitutePriority = queryDto.SubstitutePriority.Value;
            exp = exp.And(x => x.SubstitutePriority == substitutePriority);
        }

        if (queryDto?.UsageQuantity.HasValue == true)
        {
            var usageQuantity = queryDto.UsageQuantity.Value;
            exp = exp.And(x => x.UsageQuantity == usageQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialUnit))
        {
            var materialUnit = queryDto.MaterialUnit;
            exp = exp.And(x => x.MaterialUnit != null && x.MaterialUnit.Contains(materialUnit));
        }

        if (queryDto?.UsageRatio.HasValue == true)
        {
            var usageRatio = queryDto.UsageRatio.Value;
            exp = exp.And(x => x.UsageRatio == usageRatio);
        }

        if (queryDto?.IsEnabled.HasValue == true)
        {
            var isEnabled = queryDto.IsEnabled.Value;
            exp = exp.And(x => x.IsEnabled == isEnabled);
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

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            var effectiveDateStart = queryDto.EffectiveDateStart.Value;
            exp = exp.And(x => x.EffectiveDate >= effectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            var effectiveDateEnd = queryDto.EffectiveDateEnd.Value;
            exp = exp.And(x => x.EffectiveDate <= effectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            var expiryDateStart = queryDto.ExpiryDateStart.Value;
            exp = exp.And(x => x.ExpiryDate >= expiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            var expiryDateEnd = queryDto.ExpiryDateEnd.Value;
            exp = exp.And(x => x.ExpiryDate <= expiryDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktBillOfMaterialSubstituteQueryDto? queryDto)
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
        if (queryDto.BillOfMaterialItemId.HasValue)
        {
            return true;
        }
        if (queryDto.BillOfMaterialId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PrimaryMaterialCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.SubstituteMaterialId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubstituteMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubstituteGroup))
        {
            return true;
        }
        if (queryDto.SubstitutePriority.HasValue)
        {
            return true;
        }
        if (queryDto.UsageQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialUnit))
        {
            return true;
        }
        if (queryDto.UsageRatio.HasValue)
        {
            return true;
        }
        if (queryDto.IsEnabled.HasValue)
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
        if (queryDto.EffectiveDateStart.HasValue || queryDto.EffectiveDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpiryDateStart.HasValue || queryDto.ExpiryDateEnd.HasValue)
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
