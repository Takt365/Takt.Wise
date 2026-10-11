// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单应用服务实现
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

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料清单应用服务
/// </summary>
public class TaktBillOfMaterialService : TaktServiceBase, ITaktBillOfMaterialService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _billOfMaterialRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialRepository">物料清单仓储</param>
    /// <param name="billOfMaterialItemRepository">BillOfMaterialItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialService(
        ITaktCompanyRepository<TaktBillOfMaterial> billOfMaterialRepository,
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialRepository = billOfMaterialRepository;
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料清单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialDto>> GetBillOfMaterialListAsync(TaktBillOfMaterialQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBillOfMaterialDto>.Create(
                new List<TaktBillOfMaterialDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialDto>.Create(
            data.Adapt<List<TaktBillOfMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto?> GetBillOfMaterialByIdAsync(long id)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktBillOfMaterialDto>();
        await FillBillOfMaterialDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料清单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BomStatus == 1,
            x => x.BomName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BomCode,
            DictLabel = e.BomName ?? e.BomCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料清单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> CreateBillOfMaterialAsync(TaktBillOfMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterial>();
        var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ParentMaterialCode == entity.ParentMaterialCode
                && x.BomType == entity.BomType
                && x.BomVersion == entity.BomVersion);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
        {
            throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _billOfMaterialRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _billOfMaterialRepository.CreateAsync(entity);
                await SaveBillOfMaterialChildrenAsync(entity, dto);
        return await GetBillOfMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialDto>();
    }

    /// <summary>
    /// 更新物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialAsync(long id, TaktBillOfMaterialUpdateDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ParentMaterialCode == entity.ParentMaterialCode
                && x.BomType == entity.BomType
                && x.BomVersion == entity.BomVersion,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
        {
            throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
        }
        await _billOfMaterialRepository.UpdateAsync(entity);
                await SaveBillOfMaterialChildrenAsync(entity, dto);
        return await GetBillOfMaterialByIdAsync(id) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 删除物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialByIdAsync(long id)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在或已删除");
        }
        await _billOfMaterialItemRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
        var deleted = await _billOfMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料清单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料清单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料清单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialStatusAsync(TaktBillOfMaterialStatusDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.BomStatus = dto.BomStatus;
        await _billOfMaterialRepository.UpdateAsync(entity);
        return await GetBillOfMaterialByIdAsync(dto.BillOfMaterialId) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 更新物料清单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialSortAsync(TaktBillOfMaterialSortDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _billOfMaterialRepository.UpdateAsync(entity);
        return await GetBillOfMaterialByIdAsync(dto.BillOfMaterialId) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialTemplateDto>(
            sheetName ?? "物料清单导入模板",
            fileName ?? "物料清单导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料清单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialImportDto>(fileStream, sheetName ?? "物料清单导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _billOfMaterialRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBillOfMaterial>();
                var importKey = $"{entity.PlantCode}|{entity.ParentMaterialCode}|{entity.BomType}|{entity.BomVersion}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ParentMaterialCode、BomType、BomVersion）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ParentMaterialCode == entity.ParentMaterialCode
                        && x.BomType == entity.BomType
                        && x.BomVersion == entity.BomVersion);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
                {
                    throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _billOfMaterialRepository.CreateAsync(entity);
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
    /// 导出物料清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialAsync(TaktBillOfMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBillOfMaterialQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialExportDto>(),
                sheetName ?? "物料清单数据",
                fileName ?? "物料清单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _billOfMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialExportDto>(),
                sheetName ?? "物料清单数据",
                fileName ?? "物料清单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料清单数据",
            fileName ?? "物料清单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废物料清单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="billOfMaterialId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkBillOfMaterialItemsObsoleteAsync(long billOfMaterialId)
    {
        if (billOfMaterialId <= 0)
        {
            return;
        }
        var rows = await _billOfMaterialItemRepository.GetListAsync(
            x => x.BillOfMaterialId == billOfMaterialId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _billOfMaterialItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充物料清单详情（加载 OneToMany 子表：物料清单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillBillOfMaterialDetailsAsync(TaktBillOfMaterialDto dto, TaktBillOfMaterial entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料清单明细 → dto.Items（含作废行）
        var items = await _billOfMaterialItemRepository.GetListAsync(x => x.BillOfMaterialId == entity.Id);
        dto.Items = items.Adapt<List<TaktBillOfMaterialItemDto>>();
    }

    /// <summary>
    /// 保存物料清单子表级联（物料清单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveBillOfMaterialChildrenAsync(TaktBillOfMaterial entity, TaktBillOfMaterialCreateDto dto)
    {
        // 物料清单明细（Items）
        List<TaktBillOfMaterialItemUpdateDto>? itemsForSave;
        if (dto is TaktBillOfMaterialUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktBillOfMaterialItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkBillOfMaterialItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _billOfMaterialItemRepository.GetListAsync(x => x.BillOfMaterialId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktBillOfMaterialItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.BillOfMaterialId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.BomCode = entity.BomCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("物料清单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、BillOfMaterialId、LineNumber）");
                }
                if (childDto.BillOfMaterialItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.BillOfMaterialItemId, out var target))
                    {
                        throw new TaktBusinessException("物料清单明细不存在（BillOfMaterialItemId={childDto.BillOfMaterialItemId}）");
                    }
                    if (target.BillOfMaterialId != entity.Id)
                    {
                        throw new TaktBusinessException("物料清单明细不属于当前主表（BillOfMaterialItemId={childDto.BillOfMaterialItemId}）");
                    }
                    submittedIds.Add(childDto.BillOfMaterialItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _billOfMaterialItemRepository,
                        x => x.BillOfMaterialId == x.BillOfMaterialId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.BillOfMaterialItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
                    {
                        throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.BillOfMaterialItemId;
                    target.BillOfMaterialId = entity.Id;
                    target.IsObsolete = 0;
                    await _billOfMaterialItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _billOfMaterialItemRepository,
                        x => x.BillOfMaterialId == x.BillOfMaterialId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
                    {
                        throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktBillOfMaterialItem>();
                    child.Id = 0;
                    child.BillOfMaterialId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _billOfMaterialItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.BomCode) ? entity.BomCode : entity.Id.ToString();
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
                await _billOfMaterialItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料清单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterial, bool>> QueryExpression(TaktBillOfMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterial>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.BomName != null && x.BomName.Contains(keywords))
                || (x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(keywords))
                || (x.ParentMaterialDescription != null && x.ParentMaterialDescription.Contains(keywords))
                || (x.BomVersion != null && x.BomVersion.Contains(keywords))
                || (x.AlternativeBomNumber != null && x.AlternativeBomNumber.Contains(keywords))
                || (x.ParentMaterialUnit != null && x.ParentMaterialUnit.Contains(keywords))
                || (x.BomGroupCode != null && x.BomGroupCode.Contains(keywords))
                || (x.BomGroupName != null && x.BomGroupName.Contains(keywords))
                || (x.BomDescription != null && x.BomDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BomCode))
        {
            var bomCode = queryDto.BomCode;
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(bomCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomName))
        {
            var bomName = queryDto.BomName;
            exp = exp.And(x => x.BomName != null && x.BomName.Contains(bomName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ParentMaterialCode))
        {
            var parentMaterialCode = queryDto.ParentMaterialCode;
            exp = exp.And(x => x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(parentMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ParentMaterialDescription))
        {
            var parentMaterialDescription = queryDto.ParentMaterialDescription;
            exp = exp.And(x => x.ParentMaterialDescription != null && x.ParentMaterialDescription.Contains(parentMaterialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomVersion))
        {
            var bomVersion = queryDto.BomVersion;
            exp = exp.And(x => x.BomVersion != null && x.BomVersion.Contains(bomVersion));
        }

        if (queryDto?.BomType.HasValue == true)
        {
            var bomType = queryDto.BomType.Value;
            exp = exp.And(x => x.BomType == bomType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AlternativeBomNumber))
        {
            var alternativeBomNumber = queryDto.AlternativeBomNumber;
            exp = exp.And(x => x.AlternativeBomNumber != null && x.AlternativeBomNumber.Contains(alternativeBomNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ParentMaterialUnit))
        {
            var parentMaterialUnit = queryDto.ParentMaterialUnit;
            exp = exp.And(x => x.ParentMaterialUnit != null && x.ParentMaterialUnit.Contains(parentMaterialUnit));
        }

        if (queryDto?.ParentMaterialQuantity.HasValue == true)
        {
            var parentMaterialQuantity = queryDto.ParentMaterialQuantity.Value;
            exp = exp.And(x => x.ParentMaterialQuantity == parentMaterialQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomGroupCode))
        {
            var bomGroupCode = queryDto.BomGroupCode;
            exp = exp.And(x => x.BomGroupCode == bomGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomGroupName))
        {
            var bomGroupName = queryDto.BomGroupName;
            exp = exp.And(x => x.BomGroupName != null && x.BomGroupName.Contains(bomGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomDescription))
        {
            var bomDescription = queryDto.BomDescription;
            exp = exp.And(x => x.BomDescription != null && x.BomDescription.Contains(bomDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.BomStatus.HasValue == true)
        {
            var bomStatus = queryDto.BomStatus.Value;
            exp = exp.And(x => x.BomStatus == bomStatus);
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
    private static bool HasAnyListQueryFilter(TaktBillOfMaterialQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.BomCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ParentMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ParentMaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomVersion))
        {
            return true;
        }
        if (queryDto.BomType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AlternativeBomNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ParentMaterialUnit))
        {
            return true;
        }
        if (queryDto.ParentMaterialQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomDescription))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.BomStatus.HasValue)
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
