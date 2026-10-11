// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知单应用服务实现
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
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单应用服务
/// </summary>
public class TaktEcNotificationService : TaktServiceBase, ITaktEcNotificationService
{
    private readonly ITaktApprovalRepository<TaktEcNotification> _ecNotificationRepository;
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecNotificationRepository">工程变更通知单仓储</param>
    /// <param name="ecGijutsuRepository">设变技术课主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcNotificationService(
        ITaktApprovalRepository<TaktEcNotification> ecNotificationRepository,
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecNotificationRepository = ecNotificationRepository;
        _ecGijutsuRepository = ecGijutsuRepository;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取工程变更通知单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcNotificationDto>> GetEcNotificationListAsync(TaktEcNotificationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEcNotificationDto>.Create(
                new List<TaktEcNotificationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecNotificationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcNotificationDto>.Create(
            data.Adapt<List<TaktEcNotificationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNotificationDto?> GetEcNotificationByIdAsync(long id)
    {
        var entity = await _ecNotificationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcNotificationDto>();
    }

    /// <summary>
    /// 获取工程变更通知单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcNotificationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecNotificationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcNotificationStatus == 1,
            x => x.EcNotificationNotifierGroupName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EcNotificationCode,
            DictLabel = e.EcNotificationNotifierGroupName ?? e.EcNotificationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工程变更通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNotificationDto> CreateEcNotificationAsync(TaktEcNotificationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcNotification>();
        await StampEcNotificationEcGijutsuAsync(entity, dto);
        var ruleCode = TaktEcFlowConstants.ResolveNotificationRuleCode(entity.EcNotificationMethod);
        var generated = await _numberingGenerator.GenerateNextAsync(ruleCode);
        if (string.IsNullOrWhiteSpace(generated.BusinessCode))
        {
            throw new TaktBusinessException("工程变更通知单号生成失败");
        }
        entity.EcNotificationCode = generated.BusinessCode.Trim();
        var isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ecNotificationRepository,
            x => x.EcNotificationCode == entity.EcNotificationCode);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique)
        {
            throw new TaktBusinessException("工程变更通知单的EcNotificationCode已存在");
        }
        entity = await _ecNotificationRepository.CreateAsync(entity);
        return await GetEcNotificationByIdAsync(entity.Id) ?? entity.Adapt<TaktEcNotificationDto>();
    }

    /// <summary>
    /// 更新工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNotificationDto> UpdateEcNotificationAsync(long id, TaktEcNotificationUpdateDto dto)
    {
        var entity = await _ecNotificationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工程变更通知单不存在");
        }
        dto.Adapt(entity);
        await StampEcNotificationEcGijutsuAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ecNotificationRepository,
            x => x.EcNotificationCode == entity.EcNotificationCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique)
        {
            throw new TaktBusinessException("工程变更通知单的EcNotificationCode已存在");
        }
        await _ecNotificationRepository.UpdateAsync(entity);
        return await GetEcNotificationByIdAsync(id) ?? throw new TaktBusinessException("工程变更通知单不存在");
    }

    /// <summary>
    /// 删除工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcNotificationByIdAsync(long id)
    {
        var deleted = await _ecNotificationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工程变更通知单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工程变更通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcNotificationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcNotificationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工程变更通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNotificationDto> UpdateEcNotificationStatusAsync(TaktEcNotificationStatusDto dto)
    {
        var entity = await _ecNotificationRepository.GetByIdAsync(dto.EcNotificationId);
        if (entity == null)
        {
            throw new TaktBusinessException("工程变更通知单不存在");
        }
        entity.EcNotificationStatus = dto.EcNotificationStatus;
        await _ecNotificationRepository.UpdateAsync(entity);
        return await GetEcNotificationByIdAsync(dto.EcNotificationId) ?? throw new TaktBusinessException("工程变更通知单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcNotificationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcNotificationTemplateDto>(
            sheetName ?? "工程变更通知单导入模板",
            fileName ?? "工程变更通知单导入模板.xlsx");
    }

    /// <summary>
    /// 导入工程变更通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcNotificationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcNotificationImportDto>(fileStream, sheetName ?? "工程变更通知单导入模板");
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
                var entity = rows[i].Adapt<TaktEcNotification>();
                var importDto = rows[i].Adapt<TaktEcNotificationCreateDto>();
                await StampEcNotificationEcGijutsuAsync(entity, importDto);
                var importKey = $"{entity.EcNotificationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcNotificationCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecNotificationRepository,
                    x => x.EcNotificationCode == entity.EcNotificationCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique)
                {
                    throw new TaktBusinessException("工程变更通知单的EcNotificationCode已存在");
                }
                await _ecNotificationRepository.CreateAsync(entity);
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
    /// 导出工程变更通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcNotificationAsync(TaktEcNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEcNotificationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcNotificationExportDto>(),
                sheetName ?? "工程变更通知单数据",
                fileName ?? "工程变更通知单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _ecNotificationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcNotificationExportDto>(),
                sheetName ?? "工程变更通知单数据",
                fileName ?? "工程变更通知单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcNotificationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工程变更通知单数据",
            fileName ?? "工程变更通知单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工程变更通知单主表外键（ManyToOne → 设变技术课主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcNotificationEcGijutsuAsync(TaktEcNotification entity, TaktEcNotificationCreateDto dto)
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
        if (string.IsNullOrEmpty(entity.EcCode))
        {
            entity.EcCode = master.EcCode;
        }
        if (string.IsNullOrEmpty(entity.EcTitle))
        {
            entity.EcTitle = master.EcTitle;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工程变更通知单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcNotification, bool>> QueryExpression(TaktEcNotificationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcNotification>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcNotificationCode != null && x.EcNotificationCode.Contains(keywords))
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcNotificationDeptCodes != null && x.EcNotificationDeptCodes.Contains(keywords))
                || (x.EcNotificationDeptNames != null && x.EcNotificationDeptNames.Contains(keywords))
                || (x.EcNotificationNotifierGroupName != null && x.EcNotificationNotifierGroupName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNotificationCode))
        {
            var ecNotificationCode = queryDto.EcNotificationCode;
            exp = exp.And(x => x.EcNotificationCode != null && x.EcNotificationCode.Contains(ecNotificationCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.EcTitle))
        {
            var ecTitle = queryDto.EcTitle;
            exp = exp.And(x => x.EcTitle != null && x.EcTitle.Contains(ecTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNotificationDeptCodes))
        {
            var ecNotificationDeptCodes = queryDto.EcNotificationDeptCodes;
            exp = exp.And(x => x.EcNotificationDeptCodes != null && x.EcNotificationDeptCodes.Contains(ecNotificationDeptCodes));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNotificationDeptNames))
        {
            var ecNotificationDeptNames = queryDto.EcNotificationDeptNames;
            exp = exp.And(x => x.EcNotificationDeptNames != null && x.EcNotificationDeptNames.Contains(ecNotificationDeptNames));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNotificationNotifierGroupCode))
        {
            var ecNotificationNotifierGroupCode = queryDto.EcNotificationNotifierGroupCode;
            exp = exp.And(x => x.EcNotificationNotifierGroupCode == ecNotificationNotifierGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcNotificationNotifierGroupName))
        {
            var ecNotificationNotifierGroupName = queryDto.EcNotificationNotifierGroupName;
            exp = exp.And(x => x.EcNotificationNotifierGroupName != null && x.EcNotificationNotifierGroupName.Contains(ecNotificationNotifierGroupName));
        }

        if (queryDto?.EcNotificationMethod.HasValue == true)
        {
            var ecNotificationMethod = queryDto.EcNotificationMethod.Value;
            exp = exp.And(x => x.EcNotificationMethod == ecNotificationMethod);
        }

        if (queryDto?.EcNotificationStatus.HasValue == true)
        {
            var ecNotificationStatus = queryDto.EcNotificationStatus.Value;
            exp = exp.And(x => x.EcNotificationStatus == ecNotificationStatus);
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

        if (queryDto?.EcNotificationDateStart.HasValue == true)
        {
            var ecNotificationDateStart = queryDto.EcNotificationDateStart.Value;
            exp = exp.And(x => x.EcNotificationDate >= ecNotificationDateStart);
        }

        if (queryDto?.EcNotificationDateEnd.HasValue == true)
        {
            var ecNotificationDateEnd = queryDto.EcNotificationDateEnd.Value;
            exp = exp.And(x => x.EcNotificationDate <= ecNotificationDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEcNotificationQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.EcNotificationCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.EcTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNotificationDeptCodes))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNotificationDeptNames))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNotificationNotifierGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcNotificationNotifierGroupName))
        {
            return true;
        }
        if (queryDto.EcNotificationMethod.HasValue)
        {
            return true;
        }
        if (queryDto.EcNotificationStatus.HasValue)
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
        if (queryDto.EcNotificationDateStart.HasValue || queryDto.EcNotificationDateEnd.HasValue)
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
