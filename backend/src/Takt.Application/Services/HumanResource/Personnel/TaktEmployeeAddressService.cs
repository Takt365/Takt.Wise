// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工地址应用服务实现
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
/// 员工地址应用服务
/// </summary>
public class TaktEmployeeAddressService : TaktServiceBase, ITaktEmployeeAddressService
{
    private readonly ITaktCompanyRepository<TaktEmployeeAddress> _employeeAddressRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeAddressRepository">员工地址仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeAddressService(
        ITaktCompanyRepository<TaktEmployeeAddress> employeeAddressRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeAddressRepository = employeeAddressRepository;
        _employeeRepository = employeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工地址列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeAddressDto>> GetEmployeeAddressListAsync(TaktEmployeeAddressQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeAddressDto>.Create(
                new List<TaktEmployeeAddressDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeAddressRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeAddressDto>.Create(
            data.Adapt<List<TaktEmployeeAddressDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto?> GetEmployeeAddressByIdAsync(long id)
    {
        var entity = await _employeeAddressRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeAddressDto>();
    }

    /// <summary>
    /// 获取员工地址选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeAddressOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeAddressRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeName,
            DictLabel = e.EmployeeName,
        }).ToList();
    }

    /// <summary>
    /// 创建员工地址
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto> CreateEmployeeAddressAsync(TaktEmployeeAddressCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeAddress>();
        await StampEmployeeAddressEmployeeAsync(entity, dto);
        var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeAddressRepository,
            x => x.EmployeeId == entity.EmployeeId
                && x.AddressType == entity.AddressType);
        if (!isUnique_ix_employee_address_type_unique)
        {
            throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
        }
        entity = await _employeeAddressRepository.CreateAsync(entity);
        return await GetEmployeeAddressByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeAddressDto>();
    }

    /// <summary>
    /// 更新员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto> UpdateEmployeeAddressAsync(long id, TaktEmployeeAddressUpdateDto dto)
    {
        var entity = await _employeeAddressRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工地址不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeAddressEmployeeAsync(entity, dto);
        var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeAddressRepository,
            x => x.EmployeeId == entity.EmployeeId
                && x.AddressType == entity.AddressType,
            id);
        if (!isUnique_ix_employee_address_type_unique)
        {
            throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
        }
        await _employeeAddressRepository.UpdateAsync(entity);
        return await GetEmployeeAddressByIdAsync(id) ?? throw new TaktBusinessException("员工地址不存在");
    }

    /// <summary>
    /// 删除员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAddressByIdAsync(long id)
    {
        var deleted = await _employeeAddressRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工地址不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工地址
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAddressBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeAddressByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeAddressTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeAddressTemplateDto>(
            sheetName ?? "员工地址导入模板",
            fileName ?? "员工地址导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工地址
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeAddressAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeAddressImportDto>(fileStream, sheetName ?? "员工地址导入模板");
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
                var entity = rows[i].Adapt<TaktEmployeeAddress>();
                var importDto = rows[i].Adapt<TaktEmployeeAddressCreateDto>();
                await StampEmployeeAddressEmployeeAsync(entity, importDto);
                var importKey = $"{entity.EmployeeId}|{entity.AddressType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EmployeeId、AddressType）");
                }
                var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeAddressRepository,
                    x => x.EmployeeId == entity.EmployeeId
                        && x.AddressType == entity.AddressType);
                if (!isUnique_ix_employee_address_type_unique)
                {
                    throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
                }
                await _employeeAddressRepository.CreateAsync(entity);
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
    /// 导出员工地址
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeAddressAsync(TaktEmployeeAddressQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeAddressQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeAddressExportDto>(),
                sheetName ?? "员工地址数据",
                fileName ?? "员工地址导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeAddressRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeAddressExportDto>(),
                sheetName ?? "员工地址数据",
                fileName ?? "员工地址导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeAddressExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工地址数据",
            fileName ?? "员工地址导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步员工地址主表外键（ManyToOne → 员工）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeAddressEmployeeAsync(TaktEmployeeAddress entity, TaktEmployeeAddressCreateDto dto)
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
    /// 构建员工地址查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeAddress, bool>> QueryExpression(TaktEmployeeAddressQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeAddress>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.Country != null && x.Country.Contains(keywords))
                || (x.Province != null && x.Province.Contains(keywords))
                || (x.City != null && x.City.Contains(keywords))
                || (x.District != null && x.District.Contains(keywords))
                || (x.Address1 != null && x.Address1.Contains(keywords))
                || (x.Address2 != null && x.Address2.Contains(keywords))
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

        if (queryDto?.AddressType.HasValue == true)
        {
            var addressType = queryDto.AddressType.Value;
            exp = exp.And(x => x.AddressType == addressType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Country))
        {
            var country = queryDto.Country;
            exp = exp.And(x => x.Country != null && x.Country.Contains(country));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Province))
        {
            var province = queryDto.Province;
            exp = exp.And(x => x.Province != null && x.Province.Contains(province));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.City))
        {
            var city = queryDto.City;
            exp = exp.And(x => x.City != null && x.City.Contains(city));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.District))
        {
            var district = queryDto.District;
            exp = exp.And(x => x.District != null && x.District.Contains(district));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Address1))
        {
            var address1 = queryDto.Address1;
            exp = exp.And(x => x.Address1 != null && x.Address1.Contains(address1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Address2))
        {
            var address2 = queryDto.Address2;
            exp = exp.And(x => x.Address2 != null && x.Address2.Contains(address2));
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
    private static bool HasAnyListQueryFilter(TaktEmployeeAddressQueryDto? queryDto)
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
        if (queryDto.AddressType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Country))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Province))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.City))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.District))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Address1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Address2))
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
