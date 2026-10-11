// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Benefits
// 文件名称：TaktSocialInsuranceService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：社保公积金应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Benefits;
using Takt.Domain.Entities.HumanResource.Benefits;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Benefits;

/// <summary>
/// 社保公积金应用服务
/// </summary>
public class TaktSocialInsuranceService : TaktServiceBase, ITaktSocialInsuranceService
{
    private readonly ITaktCompanyRepository<TaktSocialInsurance> _socialInsuranceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="socialInsuranceRepository">社保公积金仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSocialInsuranceService(
        ITaktCompanyRepository<TaktSocialInsurance> socialInsuranceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _socialInsuranceRepository = socialInsuranceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取社保公积金列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSocialInsuranceDto>> GetSocialInsuranceListAsync(TaktSocialInsuranceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _socialInsuranceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSocialInsuranceDto>.Create(
            data.Adapt<List<TaktSocialInsuranceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialInsuranceDto?> GetSocialInsuranceByIdAsync(long id)
    {
        var entity = await _socialInsuranceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSocialInsuranceDto>();
    }

    /// <summary>
    /// 获取社保公积金选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSocialInsuranceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _socialInsuranceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PayStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建社保公积金
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialInsuranceDto> CreateSocialInsuranceAsync(TaktSocialInsuranceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSocialInsurance>();
        entity = await _socialInsuranceRepository.CreateAsync(entity);
        return await GetSocialInsuranceByIdAsync(entity.Id) ?? entity.Adapt<TaktSocialInsuranceDto>();
    }

    /// <summary>
    /// 更新社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialInsuranceDto> UpdateSocialInsuranceAsync(long id, TaktSocialInsuranceUpdateDto dto)
    {
        var entity = await _socialInsuranceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("社保公积金不存在");
        }
        dto.Adapt(entity);
        await _socialInsuranceRepository.UpdateAsync(entity);
        return await GetSocialInsuranceByIdAsync(id) ?? throw new TaktBusinessException("社保公积金不存在");
    }

    /// <summary>
    /// 删除社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSocialInsuranceByIdAsync(long id)
    {
        var deleted = await _socialInsuranceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("社保公积金不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除社保公积金
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSocialInsuranceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSocialInsuranceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新社保公积金状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialInsuranceDto> UpdateSocialInsuranceStatusAsync(TaktSocialInsuranceStatusDto dto)
    {
        var entity = await _socialInsuranceRepository.GetByIdAsync(dto.SocialInsuranceId);
        if (entity == null)
        {
            throw new TaktBusinessException("社保公积金不存在");
        }
        entity.PayStatus = dto.PayStatus;
        await _socialInsuranceRepository.UpdateAsync(entity);
        return await GetSocialInsuranceByIdAsync(dto.SocialInsuranceId) ?? throw new TaktBusinessException("社保公积金不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSocialInsuranceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSocialInsuranceTemplateDto>(
            sheetName ?? "社保公积金导入模板",
            fileName ?? "社保公积金导入模板.xlsx");
    }

    /// <summary>
    /// 导入社保公积金
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSocialInsuranceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSocialInsuranceImportDto>(fileStream, sheetName ?? "社保公积金导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSocialInsurance>();
                await _socialInsuranceRepository.CreateAsync(entity);
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
    /// 导出社保公积金
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSocialInsuranceAsync(TaktSocialInsuranceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSocialInsuranceQueryDto());
        var list = await _socialInsuranceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSocialInsuranceExportDto>(),
                sheetName ?? "社保公积金数据",
                fileName ?? "社保公积金导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSocialInsuranceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "社保公积金数据",
            fileName ?? "社保公积金导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建社保公积金查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSocialInsurance, bool>> QueryExpression(TaktSocialInsuranceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSocialInsurance>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.BenefitItemId).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.PayPeriod != null && x.PayPeriod.Contains(keywords))
                || SqlFunc.ToString(x.SocialSecurityBase).Contains(keywords)
                || SqlFunc.ToString(x.PensionAmount).Contains(keywords)
                || SqlFunc.ToString(x.MedicalAmount).Contains(keywords)
                || SqlFunc.ToString(x.UnemploymentAmount).Contains(keywords)
                || SqlFunc.ToString(x.InjuryAmount).Contains(keywords)
                || SqlFunc.ToString(x.MaternityAmount).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundBase).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundAmount).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.PayStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.BenefitItemId.HasValue == true)
        {
            exp = exp.And(x => x.BenefitItemId == queryDto.BenefitItemId);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PayPeriod))
        {
            exp = exp.And(x => x.PayPeriod != null && x.PayPeriod.Contains(queryDto.PayPeriod));
        }

        if (queryDto?.SocialSecurityBase.HasValue == true)
        {
            exp = exp.And(x => x.SocialSecurityBase == queryDto.SocialSecurityBase);
        }

        if (queryDto?.PensionAmount.HasValue == true)
        {
            exp = exp.And(x => x.PensionAmount == queryDto.PensionAmount);
        }

        if (queryDto?.MedicalAmount.HasValue == true)
        {
            exp = exp.And(x => x.MedicalAmount == queryDto.MedicalAmount);
        }

        if (queryDto?.UnemploymentAmount.HasValue == true)
        {
            exp = exp.And(x => x.UnemploymentAmount == queryDto.UnemploymentAmount);
        }

        if (queryDto?.InjuryAmount.HasValue == true)
        {
            exp = exp.And(x => x.InjuryAmount == queryDto.InjuryAmount);
        }

        if (queryDto?.MaternityAmount.HasValue == true)
        {
            exp = exp.And(x => x.MaternityAmount == queryDto.MaternityAmount);
        }

        if (queryDto?.HousingFundBase.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundBase == queryDto.HousingFundBase);
        }

        if (queryDto?.HousingFundAmount.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundAmount == queryDto.HousingFundAmount);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.PayStatus.HasValue == true)
        {
            exp = exp.And(x => x.PayStatus == queryDto.PayStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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

        return exp.ToExpression();
    }
}
