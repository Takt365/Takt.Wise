// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktSelfServiceService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：自助服务应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 自助服务应用服务
/// </summary>
public class TaktSelfServiceService : TaktServiceBase, ITaktSelfServiceService
{
    private readonly ITaktCompanyRepository<TaktSelfService> _selfServiceRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="selfServiceRepository">自助服务仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSelfServiceService(
        ITaktCompanyRepository<TaktSelfService> selfServiceRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _selfServiceRepository = selfServiceRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取自助服务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSelfServiceDto>> GetSelfServiceListAsync(TaktSelfServiceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _selfServiceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSelfServiceDto>.Create(
            data.Adapt<List<TaktSelfServiceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSelfServiceDto?> GetSelfServiceByIdAsync(long id)
    {
        var entity = await _selfServiceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSelfServiceDto>();
    }

    /// <summary>
    /// 获取自助服务选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSelfServiceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _selfServiceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SelfServiceStatus == 1,
            x => x.ServiceName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ServiceName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建自助服务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSelfServiceDto> CreateSelfServiceAsync(TaktSelfServiceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSelfService>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _selfServiceRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _selfServiceRepository.CreateAsync(entity);
        return await GetSelfServiceByIdAsync(entity.Id) ?? entity.Adapt<TaktSelfServiceDto>();
    }

    /// <summary>
    /// 更新自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSelfServiceDto> UpdateSelfServiceAsync(long id, TaktSelfServiceUpdateDto dto)
    {
        var entity = await _selfServiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("自助服务不存在");
        }
        dto.Adapt(entity);
        await _selfServiceRepository.UpdateAsync(entity);
        return await GetSelfServiceByIdAsync(id) ?? throw new TaktBusinessException("自助服务不存在");
    }

    /// <summary>
    /// 删除自助服务
    /// </summary>
    /// <param name="id">自助服务ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSelfServiceByIdAsync(long id)
    {
        var deleted = await _selfServiceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("自助服务不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除自助服务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSelfServiceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSelfServiceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新自助服务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSelfServiceDto> UpdateSelfServiceStatusAsync(TaktSelfServiceStatusDto dto)
    {
        var entity = await _selfServiceRepository.GetByIdAsync(dto.SelfServiceId);
        if (entity == null)
        {
            throw new TaktBusinessException("自助服务不存在");
        }
        entity.SelfServiceStatus = dto.SelfServiceStatus;
        await _selfServiceRepository.UpdateAsync(entity);
        return await GetSelfServiceByIdAsync(dto.SelfServiceId) ?? throw new TaktBusinessException("自助服务不存在");
    }

    /// <summary>
    /// 更新自助服务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSelfServiceDto> UpdateSelfServiceSortAsync(TaktSelfServiceSortDto dto)
    {
        var entity = await _selfServiceRepository.GetByIdAsync(dto.SelfServiceId);
        if (entity == null)
        {
            throw new TaktBusinessException("自助服务不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _selfServiceRepository.UpdateAsync(entity);
        return await GetSelfServiceByIdAsync(dto.SelfServiceId) ?? throw new TaktBusinessException("自助服务不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSelfServiceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSelfServiceTemplateDto>(
            sheetName ?? "自助服务导入模板",
            fileName ?? "自助服务导入模板.xlsx");
    }

    /// <summary>
    /// 导入自助服务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSelfServiceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSelfServiceImportDto>(fileStream, sheetName ?? "自助服务导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSortMax = await _selfServiceRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSelfService>();
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _selfServiceRepository.CreateAsync(entity);
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
    /// 导出自助服务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSelfServiceAsync(TaktSelfServiceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSelfServiceQueryDto());
        var list = await _selfServiceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSelfServiceExportDto>(),
                sheetName ?? "自助服务数据",
                fileName ?? "自助服务导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSelfServiceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "自助服务数据",
            fileName ?? "自助服务导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建自助服务查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSelfService, bool>> QueryExpression(TaktSelfServiceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSelfService>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ServiceName != null && x.ServiceName.Contains(keywords))
                || SqlFunc.ToString(x.ServiceType).Contains(keywords)
                || (x.SelfServiceDescription != null && x.SelfServiceDescription.Contains(keywords))
                || (x.LinkOrCode != null && x.LinkOrCode.Contains(keywords))
                || (x.IconUrl != null && x.IconUrl.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.SelfServiceStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceName))
        {
            exp = exp.And(x => x.ServiceName != null && x.ServiceName.Contains(queryDto.ServiceName));
        }

        if (queryDto?.ServiceType.HasValue == true)
        {
            exp = exp.And(x => x.ServiceType == queryDto.ServiceType);
        }

        if (!string.IsNullOrEmpty(queryDto?.SelfServiceDescription))
        {
            exp = exp.And(x => x.SelfServiceDescription != null && x.SelfServiceDescription.Contains(queryDto.SelfServiceDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.LinkOrCode))
        {
            exp = exp.And(x => x.LinkOrCode != null && x.LinkOrCode.Contains(queryDto.LinkOrCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.IconUrl))
        {
            exp = exp.And(x => x.IconUrl != null && x.IconUrl.Contains(queryDto.IconUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.SelfServiceStatus.HasValue == true)
        {
            exp = exp.And(x => x.SelfServiceStatus == queryDto.SelfServiceStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
