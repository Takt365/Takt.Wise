// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉处理记录应用服务实现
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
/// 客诉处理记录应用服务
/// </summary>
public class TaktCustomerComplaintHandlingService : TaktServiceBase, ITaktCustomerComplaintHandlingService
{
    private readonly ITaktCompanyRepository<TaktCustomerComplaintHandling> _customerComplaintHandlingRepository;
    private readonly ITaktCompanyRepository<TaktCustomerComplaint> _customerComplaintRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintHandlingRepository">客诉处理记录仓储</param>
    /// <param name="customerComplaintRepository">客诉主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerComplaintHandlingService(
        ITaktCompanyRepository<TaktCustomerComplaintHandling> customerComplaintHandlingRepository,
        ITaktCompanyRepository<TaktCustomerComplaint> customerComplaintRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerComplaintHandlingRepository = customerComplaintHandlingRepository;
        _customerComplaintRepository = customerComplaintRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客诉处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerComplaintHandlingDto>> GetCustomerComplaintHandlingListAsync(TaktCustomerComplaintHandlingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerComplaintHandlingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerComplaintHandlingDto>.Create(
            data.Adapt<List<TaktCustomerComplaintHandlingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintHandlingDto?> GetCustomerComplaintHandlingByIdAsync(long id)
    {
        var entity = await _customerComplaintHandlingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerComplaintHandlingDto>();
    }

    /// <summary>
    /// 获取客诉处理记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerComplaintHandlingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerComplaintHandlingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.HandlingStatus == 1,
            x => x.ComplaintHandlingCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ComplaintHandlingCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建客诉处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintHandlingDto> CreateCustomerComplaintHandlingAsync(TaktCustomerComplaintHandlingCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerComplaintHandling>();
        await StampCustomerComplaintHandlingCustomerComplaintAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintHandlingRepository,
            x => x.ComplaintHandlingCode == entity.ComplaintHandlingCode);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique)
        {
            throw new TaktBusinessException("客诉处理记录的ComplaintHandlingCode已存在");
        }
        entity = await _customerComplaintHandlingRepository.CreateAsync(entity);
        return await GetCustomerComplaintHandlingByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerComplaintHandlingDto>();
    }

    /// <summary>
    /// 更新客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintHandlingDto> UpdateCustomerComplaintHandlingAsync(long id, TaktCustomerComplaintHandlingUpdateDto dto)
    {
        var entity = await _customerComplaintHandlingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉处理记录不存在");
        }
        dto.Adapt(entity);
        await StampCustomerComplaintHandlingCustomerComplaintAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintHandlingRepository,
            x => x.ComplaintHandlingCode == entity.ComplaintHandlingCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique)
        {
            throw new TaktBusinessException("客诉处理记录的ComplaintHandlingCode已存在");
        }
        await _customerComplaintHandlingRepository.UpdateAsync(entity);
        return await GetCustomerComplaintHandlingByIdAsync(id) ?? throw new TaktBusinessException("客诉处理记录不存在");
    }

    /// <summary>
    /// 删除客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintHandlingByIdAsync(long id)
    {
        var deleted = await _customerComplaintHandlingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客诉处理记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客诉处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintHandlingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerComplaintHandlingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客诉处理记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintHandlingDto> UpdateCustomerComplaintHandlingStatusAsync(TaktCustomerComplaintHandlingStatusDto dto)
    {
        var entity = await _customerComplaintHandlingRepository.GetByIdAsync(dto.CustomerComplaintHandlingId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉处理记录不存在");
        }
        entity.HandlingStatus = dto.HandlingStatus;
        await _customerComplaintHandlingRepository.UpdateAsync(entity);
        return await GetCustomerComplaintHandlingByIdAsync(dto.CustomerComplaintHandlingId) ?? throw new TaktBusinessException("客诉处理记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerComplaintHandlingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerComplaintHandlingTemplateDto>(
            sheetName ?? "客诉处理记录导入模板",
            fileName ?? "客诉处理记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入客诉处理记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerComplaintHandlingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerComplaintHandlingImportDto>(fileStream, sheetName ?? "客诉处理记录导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerComplaintHandling>();
                var importDto = rows[i].Adapt<TaktCustomerComplaintHandlingCreateDto>();
                await StampCustomerComplaintHandlingCustomerComplaintAsync(entity, importDto);
                var importKey = $"{entity.ComplaintHandlingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ComplaintHandlingCode）");
                }
                var isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerComplaintHandlingRepository,
                    x => x.ComplaintHandlingCode == entity.ComplaintHandlingCode);
                if (!isUnique_ix_takt_logistics_quality_customer_complaint_handling_code_unique)
                {
                    throw new TaktBusinessException("客诉处理记录的ComplaintHandlingCode已存在");
                }
                await _customerComplaintHandlingRepository.CreateAsync(entity);
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
    /// 导出客诉处理记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintHandlingAsync(TaktCustomerComplaintHandlingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerComplaintHandlingQueryDto());
        var list = await _customerComplaintHandlingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerComplaintHandlingExportDto>(),
                sheetName ?? "客诉处理记录数据",
                fileName ?? "客诉处理记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerComplaintHandlingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客诉处理记录数据",
            fileName ?? "客诉处理记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步客诉处理记录主表外键（ManyToOne → 客诉主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerComplaintHandlingCustomerComplaintAsync(TaktCustomerComplaintHandling entity, TaktCustomerComplaintHandlingCreateDto dto)
    {
        if (dto.ComplaintId <= 0)
        {
            return;
        }
        var master = await _customerComplaintRepository.GetByIdAsync(dto.ComplaintId);
        if (master == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.ComplaintId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客诉处理记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerComplaintHandling, bool>> QueryExpression(TaktCustomerComplaintHandlingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerComplaintHandling>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ComplaintHandlingCode != null && x.ComplaintHandlingCode.Contains(keywords))
                || SqlFunc.ToString(x.ComplaintId).Contains(keywords)
                || (x.ComplaintCode != null && x.ComplaintCode.Contains(keywords))
                || SqlFunc.ToString(x.ComplaintItemId).Contains(keywords)
                || SqlFunc.ToString(x.HandlingStage).Contains(keywords)
                || SqlFunc.ToString(x.HandlingMethod).Contains(keywords)
                || (x.HandlingDescription != null && x.HandlingDescription.Contains(keywords))
                || (x.CauseAnalysis != null && x.CauseAnalysis.Contains(keywords))
                || (x.CorrectiveAction != null && x.CorrectiveAction.Contains(keywords))
                || (x.PreventiveAction != null && x.PreventiveAction.Contains(keywords))
                || (x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(keywords))
                || (x.ResponsibleGroupName != null && x.ResponsibleGroupName.Contains(keywords))
                || (x.HandlerGroupName != null && x.HandlerGroupName.Contains(keywords))
                || SqlFunc.ToString(x.HandlingStatus).Contains(keywords)
                || SqlFunc.ToString(x.HandlingCost).Contains(keywords)
                || (x.CustomerFeedback != null && x.CustomerFeedback.Contains(keywords))
                || SqlFunc.ToString(x.CustomerSatisfaction).Contains(keywords)
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.HandlingAt).Contains(keywords)
                || SqlFunc.ToString(x.PlannedCompletionDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualCompletionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComplaintHandlingCode))
        {
            exp = exp.And(x => x.ComplaintHandlingCode != null && x.ComplaintHandlingCode.Contains(queryDto.ComplaintHandlingCode));
        }

        if (queryDto?.ComplaintId.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintId == queryDto.ComplaintId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ComplaintCode))
        {
            exp = exp.And(x => x.ComplaintCode != null && x.ComplaintCode.Contains(queryDto.ComplaintCode));
        }

        if (queryDto?.ComplaintItemId.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintItemId == queryDto.ComplaintItemId);
        }

        if (queryDto?.HandlingStage.HasValue == true)
        {
            exp = exp.And(x => x.HandlingStage == queryDto.HandlingStage);
        }

        if (queryDto?.HandlingMethod.HasValue == true)
        {
            exp = exp.And(x => x.HandlingMethod == queryDto.HandlingMethod);
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingDescription))
        {
            exp = exp.And(x => x.HandlingDescription != null && x.HandlingDescription.Contains(queryDto.HandlingDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.CauseAnalysis))
        {
            exp = exp.And(x => x.CauseAnalysis != null && x.CauseAnalysis.Contains(queryDto.CauseAnalysis));
        }

        if (!string.IsNullOrEmpty(queryDto?.CorrectiveAction))
        {
            exp = exp.And(x => x.CorrectiveAction != null && x.CorrectiveAction.Contains(queryDto.CorrectiveAction));
        }

        if (!string.IsNullOrEmpty(queryDto?.PreventiveAction))
        {
            exp = exp.And(x => x.PreventiveAction != null && x.PreventiveAction.Contains(queryDto.PreventiveAction));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleDeptName))
        {
            exp = exp.And(x => x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(queryDto.ResponsibleDeptName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleGroupName))
        {
            exp = exp.And(x => x.ResponsibleGroupName != null && x.ResponsibleGroupName.Contains(queryDto.ResponsibleGroupName));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlerGroupName))
        {
            exp = exp.And(x => x.HandlerGroupName != null && x.HandlerGroupName.Contains(queryDto.HandlerGroupName));
        }

        if (queryDto?.HandlingStatus.HasValue == true)
        {
            exp = exp.And(x => x.HandlingStatus == queryDto.HandlingStatus);
        }

        if (queryDto?.HandlingCost.HasValue == true)
        {
            exp = exp.And(x => x.HandlingCost == queryDto.HandlingCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerFeedback))
        {
            exp = exp.And(x => x.CustomerFeedback != null && x.CustomerFeedback.Contains(queryDto.CustomerFeedback));
        }

        if (queryDto?.CustomerSatisfaction.HasValue == true)
        {
            exp = exp.And(x => x.CustomerSatisfaction == queryDto.CustomerSatisfaction);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FileName))
        {
            var fileName = queryDto.FileName;
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(fileName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccessUrl))
        {
            var accessUrl = queryDto.AccessUrl;
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(accessUrl));
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

        if (queryDto?.HandlingAtStart.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt >= queryDto.HandlingAtStart);
        }

        if (queryDto?.HandlingAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt <= queryDto.HandlingAtEnd);
        }

        if (queryDto?.PlannedCompletionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedCompletionDate >= queryDto.PlannedCompletionDateStart);
        }

        if (queryDto?.PlannedCompletionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedCompletionDate <= queryDto.PlannedCompletionDateEnd);
        }

        if (queryDto?.ActualCompletionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualCompletionDate >= queryDto.ActualCompletionDateStart);
        }

        if (queryDto?.ActualCompletionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualCompletionDate <= queryDto.ActualCompletionDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
