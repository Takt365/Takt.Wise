// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：TaktPayslipService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工资条应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Domain.Entities.HumanResource.Compensation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 工资条应用服务
/// </summary>
public class TaktPayslipService : TaktServiceBase, ITaktPayslipService
{
    private readonly ITaktCompanyRepository<TaktPayslip> _payslipRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payslipRepository">工资条仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPayslipService(
        ITaktCompanyRepository<TaktPayslip> payslipRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _payslipRepository = payslipRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工资条列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPayslipDto>> GetPayslipListAsync(TaktPayslipQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _payslipRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPayslipDto>.Create(
            data.Adapt<List<TaktPayslipDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayslipDto?> GetPayslipByIdAsync(long id)
    {
        var entity = await _payslipRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPayslipDto>();
    }

    /// <summary>
    /// 获取工资条选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPayslipOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _payslipRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IssueStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工资条
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayslipDto> CreatePayslipAsync(TaktPayslipCreateDto dto)
    {
        var entity = dto.Adapt<TaktPayslip>();
        entity = await _payslipRepository.CreateAsync(entity);
        return await GetPayslipByIdAsync(entity.Id) ?? entity.Adapt<TaktPayslipDto>();
    }

    /// <summary>
    /// 更新工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayslipDto> UpdatePayslipAsync(long id, TaktPayslipUpdateDto dto)
    {
        var entity = await _payslipRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工资条不存在");
        }
        dto.Adapt(entity);
        await _payslipRepository.UpdateAsync(entity);
        return await GetPayslipByIdAsync(id) ?? throw new TaktBusinessException("工资条不存在");
    }

    /// <summary>
    /// 删除工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <returns>任务</returns>
    public async Task DeletePayslipByIdAsync(long id)
    {
        var deleted = await _payslipRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工资条不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工资条
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePayslipBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePayslipByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工资条状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayslipDto> UpdatePayslipStatusAsync(TaktPayslipStatusDto dto)
    {
        var entity = await _payslipRepository.GetByIdAsync(dto.PayslipId);
        if (entity == null)
        {
            throw new TaktBusinessException("工资条不存在");
        }
        entity.IssueStatus = dto.IssueStatus;
        await _payslipRepository.UpdateAsync(entity);
        return await GetPayslipByIdAsync(dto.PayslipId) ?? throw new TaktBusinessException("工资条不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPayslipTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPayslipTemplateDto>(
            sheetName ?? "工资条导入模板",
            fileName ?? "工资条导入模板.xlsx");
    }

    /// <summary>
    /// 导入工资条
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPayslipAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPayslipImportDto>(fileStream, sheetName ?? "工资条导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPayslip>();
                await _payslipRepository.CreateAsync(entity);
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
    /// 导出工资条
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPayslipAsync(TaktPayslipQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPayslipQueryDto());
        var list = await _payslipRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPayslipExportDto>(),
                sheetName ?? "工资条数据",
                fileName ?? "工资条导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPayslipExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工资条数据",
            fileName ?? "工资条导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工资条查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPayslip, bool>> QueryExpression(TaktPayslipQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPayslip>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.PayPeriod != null && x.PayPeriod.Contains(keywords))
                || SqlFunc.ToString(x.BaseSalary).Contains(keywords)
                || SqlFunc.ToString(x.PositionSalary).Contains(keywords)
                || SqlFunc.ToString(x.BonusAmount).Contains(keywords)
                || SqlFunc.ToString(x.OvertimePay).Contains(keywords)
                || SqlFunc.ToString(x.AllowanceTotal).Contains(keywords)
                || SqlFunc.ToString(x.GrossAmount).Contains(keywords)
                || SqlFunc.ToString(x.SocialSecurityDeduction).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundDeduction).Contains(keywords)
                || SqlFunc.ToString(x.TaxDeduction).Contains(keywords)
                || SqlFunc.ToString(x.OtherDeduction).Contains(keywords)
                || SqlFunc.ToString(x.NetAmount).Contains(keywords)
                || (x.FormulaSetCode != null && x.FormulaSetCode.Contains(keywords))
                || SqlFunc.ToString(x.IssueStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.IssueDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
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

        if (queryDto?.BaseSalary.HasValue == true)
        {
            exp = exp.And(x => x.BaseSalary == queryDto.BaseSalary);
        }

        if (queryDto?.PositionSalary.HasValue == true)
        {
            exp = exp.And(x => x.PositionSalary == queryDto.PositionSalary);
        }

        if (queryDto?.BonusAmount.HasValue == true)
        {
            exp = exp.And(x => x.BonusAmount == queryDto.BonusAmount);
        }

        if (queryDto?.OvertimePay.HasValue == true)
        {
            exp = exp.And(x => x.OvertimePay == queryDto.OvertimePay);
        }

        if (queryDto?.AllowanceTotal.HasValue == true)
        {
            exp = exp.And(x => x.AllowanceTotal == queryDto.AllowanceTotal);
        }

        if (queryDto?.GrossAmount.HasValue == true)
        {
            exp = exp.And(x => x.GrossAmount == queryDto.GrossAmount);
        }

        if (queryDto?.SocialSecurityDeduction.HasValue == true)
        {
            exp = exp.And(x => x.SocialSecurityDeduction == queryDto.SocialSecurityDeduction);
        }

        if (queryDto?.HousingFundDeduction.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundDeduction == queryDto.HousingFundDeduction);
        }

        if (queryDto?.TaxDeduction.HasValue == true)
        {
            exp = exp.And(x => x.TaxDeduction == queryDto.TaxDeduction);
        }

        if (queryDto?.OtherDeduction.HasValue == true)
        {
            exp = exp.And(x => x.OtherDeduction == queryDto.OtherDeduction);
        }

        if (queryDto?.NetAmount.HasValue == true)
        {
            exp = exp.And(x => x.NetAmount == queryDto.NetAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormulaSetCode))
        {
            exp = exp.And(x => x.FormulaSetCode != null && x.FormulaSetCode.Contains(queryDto.FormulaSetCode));
        }

        if (queryDto?.IssueStatus.HasValue == true)
        {
            exp = exp.And(x => x.IssueStatus == queryDto.IssueStatus);
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

        if (queryDto?.IssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.IssueDate >= queryDto.IssueDateStart);
        }

        if (queryDto?.IssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.IssueDate <= queryDto.IssueDateEnd);
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
