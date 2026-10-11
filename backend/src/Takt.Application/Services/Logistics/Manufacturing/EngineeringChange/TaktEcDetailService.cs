// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细应用服务
/// </summary>
public class TaktEcDetailService : TaktServiceBase, ITaktEcDetailService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecGijutsuRepository">设变技术课主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDetailService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
        _ecGijutsuRepository = ecGijutsuRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDetailDto>> GetEcDetailListAsync(TaktEcDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEcDetailDto>.Create(
                new List<TaktEcDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcDetailDto>.Create(
            data.Adapt<List<TaktEcDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto?> GetEcDetailByIdAsync(long id)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcDetailDto>();
    }

    /// <summary>
    /// 获取设变明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.EcCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EcCode,
            DictLabel = e.EcCode,
        }).ToList();
    }

    /// <summary>
    /// 创建设变明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> CreateEcDetailAsync(TaktEcDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcDetail>();
        entity.IsObsolete = 0;
        await StampEcDetailEcGijutsuAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDetailRepository,
            x => x.EcGijutsuId == entity.EcGijutsuId
                && x.EcRootMaterialCode == entity.EcRootMaterialCode
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
        {
            throw new TaktBusinessException("设变明细的EcGijutsuId、EcRootMaterialCode、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.EcGijutsuId == entity.EcGijutsuId
                    && x.EcRootMaterialCode == entity.EcRootMaterialCode,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.EcCode) ? entity.EcCode : entity.EcGijutsuId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecDetailRepository.CreateAsync(entity);
        return await GetEcDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktEcDetailDto>();
    }

    /// <summary>
    /// 更新设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> UpdateEcDetailAsync(long id, TaktEcDetailUpdateDto dto)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        var keepIsCompatible = entity.EcIsCompatible;
        var keep2ndVendor = entity.Ec2ndVendor;
        var keepInstruction = entity.EcInstruction;
        var keepOldPartDisposition = entity.EcOldPartDisposition;
        dto.Adapt(entity);
        // 技术课明细弹窗未绑定这四列时 UpdateDto 默认空串，Adapt 会覆盖库内已导入的值
        if (string.IsNullOrWhiteSpace(dto.EcIsCompatible))
        {
            entity.EcIsCompatible = keepIsCompatible;
        }
        if (string.IsNullOrWhiteSpace(dto.Ec2ndVendor))
        {
            entity.Ec2ndVendor = keep2ndVendor;
        }
        if (string.IsNullOrWhiteSpace(dto.EcInstruction))
        {
            entity.EcInstruction = keepInstruction;
        }
        if (string.IsNullOrWhiteSpace(dto.EcOldPartDisposition))
        {
            entity.EcOldPartDisposition = keepOldPartDisposition;
        }
        await StampEcDetailEcGijutsuAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDetailRepository,
            x => x.EcGijutsuId == entity.EcGijutsuId
                && x.EcRootMaterialCode == entity.EcRootMaterialCode
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
        {
            throw new TaktBusinessException("设变明细的EcGijutsuId、EcRootMaterialCode、LineNumber已存在");
        }
        await _ecDetailRepository.UpdateAsync(entity);
        return await GetEcDetailByIdAsync(id) ?? throw new TaktBusinessException("设变明细不存在");
    }

    /// <summary>
    /// 删除设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDetailByIdAsync(long id)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变明细已作废");
        }
        entity.IsObsolete = 1;
        await _ecDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除设变明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> UpdateEcDetailObsoleteAsync(TaktEcDetailObsoleteDto dto)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(dto.EcDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecDetailRepository.UpdateAsync(entity);
        return await GetEcDetailByIdAsync(dto.EcDetailId) ?? throw new TaktBusinessException("设变明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcDetailTemplateDto>(
            sheetName ?? "设变明细导入模板",
            fileName ?? "设变明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcDetailImportDto>(fileStream, sheetName ?? "设变明细导入模板");
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
                var entity = rows[i].Adapt<TaktEcDetail>();
                var importDto = rows[i].Adapt<TaktEcDetailCreateDto>();
                await StampEcDetailEcGijutsuAsync(entity, importDto);
                var importKey = $"{entity.EcGijutsuId}|{entity.EcRootMaterialCode}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcGijutsuId、EcRootMaterialCode、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecDetailRepository,
                    x => x.EcGijutsuId == entity.EcGijutsuId
                        && x.EcRootMaterialCode == entity.EcRootMaterialCode
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
                {
                    throw new TaktBusinessException("设变明细的EcGijutsuId、EcRootMaterialCode、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode
                            && x.CompanyCode == CurrentCompanyCode
                            && x.EcGijutsuId == entity.EcGijutsuId
                            && x.EcRootMaterialCode == entity.EcRootMaterialCode,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.EcCode) ? entity.EcCode : entity.EcGijutsuId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecDetailRepository.CreateAsync(entity);
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
    /// 导出设变明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcDetailAsync(TaktEcDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEcDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcDetailExportDto>(),
                sheetName ?? "设变明细数据",
                fileName ?? "设变明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _ecDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcDetailExportDto>(),
                sheetName ?? "设变明细数据",
                fileName ?? "设变明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变明细数据",
            fileName ?? "设变明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设变明细主表外键（ManyToOne → 设变技术课主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcDetailEcGijutsuAsync(TaktEcDetail entity, TaktEcDetailCreateDto dto)
    {
        if (dto.EcGijutsuId <= 0)
        {
            return;
        }
        var master = await _ecGijutsuRepository.GetByIdAsync(dto.EcGijutsuId);
        if (master == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        entity.EcGijutsuId = master.Id;
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
        // 冗余：始终由主表带入，禁止客户端默认值
        entity.EcCode = master.EcCode;
        entity.EcScope = master.EcScope;
        entity.EcDeptScope = master.EcDeptScope;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> QueryExpression(TaktEcDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();

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
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcBomLineCode != null && x.EcBomLineCode.Contains(keywords))
                || (x.EcModelCode != null && x.EcModelCode.Contains(keywords))
                || (x.EcRootMaterialCode != null && x.EcRootMaterialCode.Contains(keywords))
                || (x.EcRootMaterialDescription != null && x.EcRootMaterialDescription.Contains(keywords))
                || (x.EcParentMaterialCode != null && x.EcParentMaterialCode.Contains(keywords))
                || (x.EcParentMaterialDescription != null && x.EcParentMaterialDescription.Contains(keywords))
                || (x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(keywords))
                || (x.EcOldMaterialDescription != null && x.EcOldMaterialDescription.Contains(keywords))
                || (x.EcOldItemPosition != null && x.EcOldItemPosition.Contains(keywords))
                || (x.EcOldWarehouse != null && x.EcOldWarehouse.Contains(keywords))
                || (x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(keywords))
                || (x.EcNewMaterialDescription != null && x.EcNewMaterialDescription.Contains(keywords))
                || (x.EcNewItemPosition != null && x.EcNewItemPosition.Contains(keywords))
                || (x.EcNewWarehouse != null && x.EcNewWarehouse.Contains(keywords))
                || (x.EcIsCompatible != null && x.EcIsCompatible.Contains(keywords))
                || (x.Ec2ndVendor != null && x.Ec2ndVendor.Contains(keywords))
                || (x.EcInstruction != null && x.EcInstruction.Contains(keywords))
                || (x.EcOldPartDisposition != null && x.EcOldPartDisposition.Contains(keywords))
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

        if (queryDto?.EcGijutsuId.HasValue == true)
        {
            var ecGijutsuId = queryDto.EcGijutsuId.Value;
            exp = exp.And(x => x.EcGijutsuId == ecGijutsuId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcBomLineCode))
        {
            var ecBomLineCode = queryDto.EcBomLineCode;
            exp = exp.And(x => x.EcBomLineCode != null && x.EcBomLineCode.Contains(ecBomLineCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcModelCode))
        {
            var ecModelCode = queryDto.EcModelCode;
            exp = exp.And(x => x.EcModelCode != null && x.EcModelCode.Contains(ecModelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcRootMaterialCode))
        {
            var ecRootMaterialCode = queryDto.EcRootMaterialCode;
            exp = exp.And(x => x.EcRootMaterialCode != null && x.EcRootMaterialCode.Contains(ecRootMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcRootMaterialDescription))
        {
            var ecRootMaterialDescription = queryDto.EcRootMaterialDescription;
            exp = exp.And(x => x.EcRootMaterialDescription != null && x.EcRootMaterialDescription.Contains(ecRootMaterialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcParentMaterialCode))
        {
            var ecParentMaterialCode = queryDto.EcParentMaterialCode;
            exp = exp.And(x => x.EcParentMaterialCode != null && x.EcParentMaterialCode.Contains(ecParentMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcParentMaterialDescription))
        {
            var ecParentMaterialDescription = queryDto.EcParentMaterialDescription;
            exp = exp.And(x => x.EcParentMaterialDescription != null && x.EcParentMaterialDescription.Contains(ecParentMaterialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DiscontinuedStatus))
        {
            var discontinuedStatus = queryDto.DiscontinuedStatus;
            exp = exp.And(x => x.DiscontinuedStatus != null && x.DiscontinuedStatus.Contains(discontinuedStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldMaterialCode))
        {
            var ecOldMaterialCode = queryDto.EcOldMaterialCode;
            exp = exp.And(x => x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(ecOldMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldMaterialDescription))
        {
            var ecOldMaterialDescription = queryDto.EcOldMaterialDescription;
            exp = exp.And(x => x.EcOldMaterialDescription != null && x.EcOldMaterialDescription.Contains(ecOldMaterialDescription));
        }

        if (queryDto?.EcOldUsageQuantity.HasValue == true)
        {
            var ecOldUsageQuantity = queryDto.EcOldUsageQuantity.Value;
            exp = exp.And(x => x.EcOldUsageQuantity == ecOldUsageQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldItemPosition))
        {
            var ecOldItemPosition = queryDto.EcOldItemPosition;
            exp = exp.And(x => x.EcOldItemPosition != null && x.EcOldItemPosition.Contains(ecOldItemPosition));
        }

        if (queryDto?.EcOldStock.HasValue == true)
        {
            var ecOldStock = queryDto.EcOldStock.Value;
            exp = exp.And(x => x.EcOldStock == ecOldStock);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldWarehouse))
        {
            var ecOldWarehouse = queryDto.EcOldWarehouse;
            exp = exp.And(x => x.EcOldWarehouse != null && x.EcOldWarehouse.Contains(ecOldWarehouse));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldPurchaseType))
        {
            var ecOldPurchaseType = queryDto.EcOldPurchaseType;
            exp = exp.And(x => x.EcOldPurchaseType != null && x.EcOldPurchaseType == ecOldPurchaseType);
        }

        if (queryDto?.EcOldRequiresInspection.HasValue == true)
        {
            var ecOldRequiresInspection = queryDto.EcOldRequiresInspection.Value;
            exp = exp.And(x => x.EcOldRequiresInspection == ecOldRequiresInspection);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNewMaterialCode))
        {
            var ecNewMaterialCode = queryDto.EcNewMaterialCode;
            exp = exp.And(x => x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(ecNewMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNewMaterialDescription))
        {
            var ecNewMaterialDescription = queryDto.EcNewMaterialDescription;
            exp = exp.And(x => x.EcNewMaterialDescription != null && x.EcNewMaterialDescription.Contains(ecNewMaterialDescription));
        }

        if (queryDto?.EcNewUsageQuantity.HasValue == true)
        {
            var ecNewUsageQuantity = queryDto.EcNewUsageQuantity.Value;
            exp = exp.And(x => x.EcNewUsageQuantity == ecNewUsageQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNewItemPosition))
        {
            var ecNewItemPosition = queryDto.EcNewItemPosition;
            exp = exp.And(x => x.EcNewItemPosition != null && x.EcNewItemPosition.Contains(ecNewItemPosition));
        }

        if (queryDto?.EcNewStock.HasValue == true)
        {
            var ecNewStock = queryDto.EcNewStock.Value;
            exp = exp.And(x => x.EcNewStock == ecNewStock);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNewWarehouse))
        {
            var ecNewWarehouse = queryDto.EcNewWarehouse;
            exp = exp.And(x => x.EcNewWarehouse != null && x.EcNewWarehouse.Contains(ecNewWarehouse));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNewPurchaseType))
        {
            var ecNewPurchaseType = queryDto.EcNewPurchaseType;
            exp = exp.And(x => x.EcNewPurchaseType != null && x.EcNewPurchaseType == ecNewPurchaseType);
        }

        if (queryDto?.EcNewRequiresInspection.HasValue == true)
        {
            var ecNewRequiresInspection = queryDto.EcNewRequiresInspection.Value;
            exp = exp.And(x => x.EcNewRequiresInspection == ecNewRequiresInspection);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcIsCompatible))
        {
            var ecIsCompatible = queryDto.EcIsCompatible;
            exp = exp.And(x => x.EcIsCompatible != null && x.EcIsCompatible.Contains(ecIsCompatible));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Ec2ndVendor))
        {
            var ec2ndVendor = queryDto.Ec2ndVendor;
            exp = exp.And(x => x.Ec2ndVendor != null && x.Ec2ndVendor.Contains(ec2ndVendor));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcInstruction))
        {
            var ecInstruction = queryDto.EcInstruction;
            exp = exp.And(x => x.EcInstruction != null && x.EcInstruction.Contains(ecInstruction));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcOldPartDisposition))
        {
            var ecOldPartDisposition = queryDto.EcOldPartDisposition;
            exp = exp.And(x => x.EcOldPartDisposition != null && x.EcOldPartDisposition.Contains(ecOldPartDisposition));
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

        if (queryDto?.EcBomDateStart.HasValue == true)
        {
            var ecBomDateStart = queryDto.EcBomDateStart.Value;
            exp = exp.And(x => x.EcBomDate >= ecBomDateStart);
        }

        if (queryDto?.EcBomDateEnd.HasValue == true)
        {
            var ecBomDateEnd = queryDto.EcBomDateEnd.Value;
            exp = exp.And(x => x.EcBomDate <= ecBomDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEcDetailQueryDto? queryDto)
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
        if (queryDto.EcGijutsuId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcBomLineCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcModelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcRootMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcRootMaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcParentMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcParentMaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DiscontinuedStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldMaterialDescription))
        {
            return true;
        }
        if (queryDto.EcOldUsageQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldItemPosition))
        {
            return true;
        }
        if (queryDto.EcOldStock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldWarehouse))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldPurchaseType))
        {
            return true;
        }
        if (queryDto.EcOldRequiresInspection.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewMaterialDescription))
        {
            return true;
        }
        if (queryDto.EcNewUsageQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewItemPosition))
        {
            return true;
        }
        if (queryDto.EcNewStock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewWarehouse))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNewPurchaseType))
        {
            return true;
        }
        if (queryDto.EcNewRequiresInspection.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcIsCompatible))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Ec2ndVendor))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcInstruction))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcOldPartDisposition))
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
        if (queryDto.EcBomDateStart.HasValue || queryDto.EcBomDateEnd.HasValue)
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
