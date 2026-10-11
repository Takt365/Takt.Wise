// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktAccountTitleService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会计科目应用服务
/// </summary>
public class TaktAccountTitleService : TaktServiceBase, ITaktAccountTitleService
{
    private readonly ITaktCompanyRepository<TaktAccountTitle> _accountTitleRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="accountTitleRepository">会计科目仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAccountTitleService(
        ITaktCompanyRepository<TaktAccountTitle> accountTitleRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _accountTitleRepository = accountTitleRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会计科目列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAccountTitleDto>> GetAccountTitleListAsync(TaktAccountTitleQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAccountTitleDto>.Create(
                new List<TaktAccountTitleDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _accountTitleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAccountTitleDto>.Create(
            data.Adapt<List<TaktAccountTitleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto?> GetAccountTitleByIdAsync(long id)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAccountTitleDto>();
    }

    /// <summary>
    /// 获取会计科目平铺选项（DictValue=AccountTitleCode；供统驭科目等业务字段下拉）。
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配编码/名称）</param>
    /// <param name="reconciliationOnly">为 true 时仅辅助核算科目（IsAuxiliary=1）</param>
    /// <param name="auxiliaryType">辅助核算类型（可选；D=客户 K=供应商 等）</param>
    /// <returns>下拉选项（有上限，禁止全表）</returns>
    public async Task<List<TaktSelectOption>> GetAccountTitleOptionsAsync(string? plantCode = null, string? keyword = null, bool reconciliationOnly = false, string? auxiliaryType = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var normalizedAuxiliaryType = auxiliaryType?.Trim();
        var predicate = Expressionable.Create<TaktAccountTitle>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AccountTitleStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(reconciliationOnly, x => x.IsAuxiliary == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedAuxiliaryType), x => x.AuxiliaryType == normalizedAuxiliaryType)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.AccountTitleCode != null && x.AccountTitleCode.Contains(normalizedKeyword!))
                || (x.AccountTitleName != null && x.AccountTitleName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _accountTitleRepository.GetListForExportAsync(predicate, 500);
        return list
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.AccountTitleCode)
            .Select(e => new TaktSelectOption
            {
                DictValue = e.AccountTitleCode,
                DictLabel = string.IsNullOrWhiteSpace(e.AccountTitleName) ? e.AccountTitleCode : e.AccountTitleName,
                ExtLabel = e.AccountTitleCode,
                SortOrder = e.SortOrder,
            })
            .ToList();
    }

    /// <summary>
    /// 获取会计科目树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 AccountTitleCode）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetAccountTitleTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _accountTitleRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ParentId == parentId
            && x.AccountTitleStatus == 1);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                return new TaktTreeSelectOption
                {
                    DictValue = item.AccountTitleCode,
                    DictLabel = string.IsNullOrWhiteSpace(item.AccountTitleName) ? item.AccountTitleCode : item.AccountTitleName,
                    ExtLabel = item.AccountTitleCode,
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取会计科目树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktAccountTitleTreeDto>> GetAccountTitleTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktAccountTitle, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.AccountTitleStatus == 1);
        var list = await _accountTitleRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktAccountTitleTreeDto>();
                treeDto.Children = new List<TaktAccountTitleTreeDto>();
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建会计科目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> CreateAccountTitleAsync(TaktAccountTitleCreateDto dto)
    {
        var entity = dto.Adapt<TaktAccountTitle>();
        var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
            _accountTitleRepository,
            x => x.AccountTitleCode == entity.AccountTitleCode);
        if (!isUnique_ix_account_title_code_unique)
        {
            throw new TaktBusinessException("会计科目的AccountTitleCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _accountTitleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _accountTitleRepository.CreateAsync(entity);
        return await GetAccountTitleByIdAsync(entity.Id) ?? entity.Adapt<TaktAccountTitleDto>();
    }

    /// <summary>
    /// 更新会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleAsync(long id, TaktAccountTitleUpdateDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
            _accountTitleRepository,
            x => x.AccountTitleCode == entity.AccountTitleCode,
            id);
        if (!isUnique_ix_account_title_code_unique)
        {
            throw new TaktBusinessException("会计科目的AccountTitleCode已存在");
        }
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(id) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 删除会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleByIdAsync(long id)
    {

        var hasChildren = await _accountTitleRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _accountTitleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会计科目不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会计科目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAccountTitleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会计科目状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleStatusAsync(TaktAccountTitleStatusDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(dto.AccountTitleId);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        entity.AccountTitleStatus = dto.AccountTitleStatus;
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(dto.AccountTitleId) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 更新会计科目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleSortAsync(TaktAccountTitleSortDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(dto.AccountTitleId);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(dto.AccountTitleId) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAccountTitleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAccountTitleTemplateDto>(
            sheetName ?? "会计科目导入模板",
            fileName ?? "会计科目导入模板.xlsx");
    }

    /// <summary>
    /// 导入会计科目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAccountTitleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAccountTitleImportDto>(fileStream, sheetName ?? "会计科目导入模板");
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
                var entity = rows[i].Adapt<TaktAccountTitle>();
                var importKey = $"{entity.AccountTitleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AccountTitleCode）");
                }
                var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _accountTitleRepository,
                    x => x.AccountTitleCode == entity.AccountTitleCode);
                if (!isUnique_ix_account_title_code_unique)
                {
                    throw new TaktBusinessException("会计科目的AccountTitleCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _accountTitleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _accountTitleRepository.CreateAsync(entity);
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
    /// 导出会计科目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAccountTitleAsync(TaktAccountTitleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAccountTitleQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAccountTitleExportDto>(),
                sheetName ?? "会计科目数据",
                fileName ?? "会计科目导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _accountTitleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAccountTitleExportDto>(),
                sheetName ?? "会计科目数据",
                fileName ?? "会计科目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAccountTitleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会计科目数据",
            fileName ?? "会计科目导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会计科目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAccountTitle, bool>> QueryExpression(TaktAccountTitleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAccountTitle>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.AccountTitleCode != null && x.AccountTitleCode.Contains(keywords))
                || (x.AccountTitleName != null && x.AccountTitleName.Contains(keywords))
                || (x.AccountTitleType != null && x.AccountTitleType.Contains(keywords))
                || (x.AuxiliaryType != null && x.AuxiliaryType.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountTitleCode))
        {
            var accountTitleCode = queryDto.AccountTitleCode;
            exp = exp.And(x => x.AccountTitleCode != null && x.AccountTitleCode.Contains(accountTitleCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountTitleName))
        {
            var accountTitleName = queryDto.AccountTitleName;
            exp = exp.And(x => x.AccountTitleName != null && x.AccountTitleName.Contains(accountTitleName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountTitleType))
        {
            var accountTitleType = queryDto.AccountTitleType;
            exp = exp.And(x => x.AccountTitleType != null && x.AccountTitleType.Contains(accountTitleType));
        }

        if (queryDto?.BalanceDirection.HasValue == true)
        {
            var balanceDirection = queryDto.BalanceDirection.Value;
            exp = exp.And(x => x.BalanceDirection == balanceDirection);
        }

        if (queryDto?.AccountTitleLevel.HasValue == true)
        {
            var accountTitleLevel = queryDto.AccountTitleLevel.Value;
            exp = exp.And(x => x.AccountTitleLevel == accountTitleLevel);
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            var isLeaf = queryDto.IsLeaf.Value;
            exp = exp.And(x => x.IsLeaf == isLeaf);
        }

        if (queryDto?.IsAuxiliary.HasValue == true)
        {
            var isAuxiliary = queryDto.IsAuxiliary.Value;
            exp = exp.And(x => x.IsAuxiliary == isAuxiliary);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AuxiliaryType))
        {
            var auxiliaryType = queryDto.AuxiliaryType;
            exp = exp.And(x => x.AuxiliaryType != null && x.AuxiliaryType.Contains(auxiliaryType));
        }

        if (queryDto?.IsQuantity.HasValue == true)
        {
            var isQuantity = queryDto.IsQuantity.Value;
            exp = exp.And(x => x.IsQuantity == isQuantity);
        }

        if (queryDto?.IsCurrency.HasValue == true)
        {
            var isCurrency = queryDto.IsCurrency.Value;
            exp = exp.And(x => x.IsCurrency == isCurrency);
        }

        if (queryDto?.IsCash.HasValue == true)
        {
            var isCash = queryDto.IsCash.Value;
            exp = exp.And(x => x.IsCash == isCash);
        }

        if (queryDto?.IsBank.HasValue == true)
        {
            var isBank = queryDto.IsBank.Value;
            exp = exp.And(x => x.IsBank == isBank);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.AccountTitleStatus.HasValue == true)
        {
            var accountTitleStatus = queryDto.AccountTitleStatus.Value;
            exp = exp.And(x => x.AccountTitleStatus == accountTitleStatus);
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            var validFromStart = queryDto.ValidFromStart.Value;
            exp = exp.And(x => x.ValidFrom >= validFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            var validFromEnd = queryDto.ValidFromEnd.Value;
            exp = exp.And(x => x.ValidFrom <= validFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            var validToStart = queryDto.ValidToStart.Value;
            exp = exp.And(x => x.ValidTo >= validToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            var validToEnd = queryDto.ValidToEnd.Value;
            exp = exp.And(x => x.ValidTo <= validToEnd);
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
    private static bool HasAnyListQueryFilter(TaktAccountTitleQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.AccountTitleCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountTitleName))
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountTitleType))
        {
            return true;
        }
        if (queryDto.BalanceDirection.HasValue)
        {
            return true;
        }
        if (queryDto.AccountTitleLevel.HasValue)
        {
            return true;
        }
        if (queryDto.IsLeaf.HasValue)
        {
            return true;
        }
        if (queryDto.IsAuxiliary.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AuxiliaryType))
        {
            return true;
        }
        if (queryDto.IsQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.IsCurrency.HasValue)
        {
            return true;
        }
        if (queryDto.IsCash.HasValue)
        {
            return true;
        }
        if (queryDto.IsBank.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.AccountTitleStatus.HasValue)
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
        if (queryDto.ValidFromStart.HasValue || queryDto.ValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidToStart.HasValue || queryDto.ValidToEnd.HasValue)
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
