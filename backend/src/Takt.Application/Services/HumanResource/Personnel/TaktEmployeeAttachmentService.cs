// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工附件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工附件应用服务
/// </summary>
public class TaktEmployeeAttachmentService : TaktServiceBase, ITaktEmployeeAttachmentService
{
    private readonly ITaktCompanyRepository<TaktEmployeeAttachment> _employeeAttachmentRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeAttachmentRepository">员工附件仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeAttachmentService(
        ITaktCompanyRepository<TaktEmployeeAttachment> employeeAttachmentRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeAttachmentRepository = employeeAttachmentRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工附件列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeAttachmentDto>> GetEmployeeAttachmentListAsync(TaktEmployeeAttachmentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeAttachmentDto>.Create(
                new List<TaktEmployeeAttachmentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeAttachmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeAttachmentDto>.Create(
            data.Adapt<List<TaktEmployeeAttachmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAttachmentDto?> GetEmployeeAttachmentByIdAsync(long id)
    {
        var entity = await _employeeAttachmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeAttachmentDto>();
    }

    /// <summary>
    /// 获取员工附件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeAttachmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeAttachmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeCode,
            DictLabel = e.EmployeeName ?? e.EmployeeCode,
        }).ToList();
    }

    /// <summary>
    /// 创建员工附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAttachmentDto> CreateEmployeeAttachmentAsync(TaktEmployeeAttachmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeAttachment>();
        await StampEmployeeAttachmentEmployeeAsync(entity, dto);
        entity = await _employeeAttachmentRepository.CreateAsync(entity);
        return await GetEmployeeAttachmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeAttachmentDto>();
    }

    /// <summary>
    /// 更新员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAttachmentDto> UpdateEmployeeAttachmentAsync(long id, TaktEmployeeAttachmentUpdateDto dto)
    {
        var entity = await _employeeAttachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工附件不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeAttachmentEmployeeAsync(entity, dto);
        await _employeeAttachmentRepository.UpdateAsync(entity);
        return await GetEmployeeAttachmentByIdAsync(id) ?? throw new TaktBusinessException("员工附件不存在");
    }

    /// <summary>
    /// 删除员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAttachmentByIdAsync(long id)
    {
        var deleted = await _employeeAttachmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工附件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAttachmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeAttachmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeAttachmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeAttachmentTemplateDto>(
            sheetName ?? "员工附件导入模板",
            fileName ?? "员工附件导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeAttachmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeAttachmentImportDto>(fileStream, sheetName ?? "员工附件导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeAttachment>();
                var importDto = rows[i].Adapt<TaktEmployeeAttachmentCreateDto>();
                await StampEmployeeAttachmentEmployeeAsync(entity, importDto);
                await _employeeAttachmentRepository.CreateAsync(entity);
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
    /// 导出员工附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeAttachmentAsync(TaktEmployeeAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeAttachmentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeAttachmentExportDto>(),
                sheetName ?? "员工附件数据",
                fileName ?? "员工附件导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeAttachmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeAttachmentExportDto>(),
                sheetName ?? "员工附件数据",
                fileName ?? "员工附件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeAttachmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工附件数据",
            fileName ?? "员工附件导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工附件主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeAttachmentEmployeeAsync(TaktEmployeeAttachment entity, TaktEmployeeAttachmentCreateDto dto)
    {
        if (dto.EmployeeId <= 0)
        {
            return;
        }
        var master = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (master == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        entity.EmployeeId = master.Id;
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
        if (string.IsNullOrEmpty(entity.EmployeeCode))
        {
            entity.EmployeeCode = master.EmployeeCode;
        }
        if (string.IsNullOrEmpty(entity.EmployeeName))
        {
            entity.EmployeeName = master.EmployeeName;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工附件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeAttachment, bool>> QueryExpression(TaktEmployeeAttachmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeAttachment>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
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

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeCode))
        {
            var employeeCode = queryDto.EmployeeCode;
            exp = exp.And(x => x.EmployeeCode != null && x.EmployeeCode.Contains(employeeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeName))
        {
            var employeeName = queryDto.EmployeeName;
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(employeeName));
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
    private static bool HasAnyListQueryFilter(TaktEmployeeAttachmentQueryDto? queryDto)
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
        if (queryDto.EmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FileName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccessUrl))
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
