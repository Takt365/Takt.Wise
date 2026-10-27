// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Organization
// 文件名称：TaktDeptService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：部门应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Application.Services.Identity;

namespace Takt.Application.Services.HumanResource.Organization;

/// <summary>
/// 部门应用服务
/// </summary>
public class TaktDeptService : TaktServiceBase, ITaktDeptService
{
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDeptService(
        ITaktCompanyRepository<TaktDept> deptRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _deptRepository = deptRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取部门列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDeptDto>> GetDeptListAsync(TaktDeptQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktDeptDto>.Create(
                new List<TaktDeptDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _deptRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDeptDto>.Create(
            data.Adapt<List<TaktDeptDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto?> GetDeptByIdAsync(long id)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDeptDto>();
        return dto;    }

    /// <summary>
    /// 获取部门树形选项列表（懒加载：仅 parentId 直接子级一层；强制当前登录公司隔离）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetDeptTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktDept>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.DeptStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.DeptCode != null && x.DeptCode.Contains(normalizedKeyword!))
                || (x.DeptName1 != null && x.DeptName1.Contains(normalizedKeyword!))
                || (x.IsoCode != null && x.IsoCode.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _deptRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                return new TaktTreeSelectOption
                {
                    DictValue = item.Id.ToString(),
                    DictLabel = item.DeptName1,
                    ExtValue = item.Id.ToString(),
                    I18nKey = string.IsNullOrWhiteSpace(item.DeptCode)
                        ? null
                        : TaktNamingHelper.OrgDeptResourceKey(item.DeptCode),
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取部门选项（DictValue=DeptCode，DictLabel=DeptName1）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDeptOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktDept>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.DeptCode != null && x.DeptCode.Contains(normalizedKeyword!))
                || (x.DeptName1 != null && x.DeptName1.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _deptRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.DeptCode)
            .Select(item => new TaktSelectOption
            {
                DictValue = item.DeptCode,
                DictLabel = string.IsNullOrWhiteSpace(item.DeptName1) ? item.DeptCode : item.DeptName1,
                ExtLabel = item.DeptCode,
                SortOrder = item.SortOrder,
            })
            .ToList();
    }

    /// <summary>
    /// 获取部门 ISO 编码树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue=IsoCode；ExtValue=实体 Id 供展开；强制当前登录公司隔离）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载展开时由前端传 ExtValue=实体 Id）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetDeptIsoTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktDept>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.DeptStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.DeptCode != null && x.DeptCode.Contains(normalizedKeyword!))
                || (x.DeptName1 != null && x.DeptName1.Contains(normalizedKeyword!))
                || (x.IsoCode != null && x.IsoCode.Contains(normalizedKeyword!))
                || (x.DeptShortName != null && x.DeptShortName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _deptRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var iso = (item.IsoCode ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(iso))
                {
                    iso = (item.DeptShortName ?? string.Empty).Trim();
                }
                if (string.IsNullOrEmpty(iso))
                {
                    iso = (item.DeptCode ?? string.Empty).Trim();
                }
                var label = string.IsNullOrEmpty(iso)
                    ? item.DeptName1
                    : $"{iso} - {item.DeptName1}";
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                return new TaktTreeSelectOption
                {
                    // 编码规则等存 IsoCode；展开 parentId 必须用实体 Id（勿把 DeptCode/IsoCode 当 parentId）
                    DictValue = iso,
                    DictLabel = label,
                    ExtLabel = item.DeptCode,
                    ExtValue = item.Id.ToString(),
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取部门树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktDeptTreeDto>> GetDeptTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktDept, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.DeptStatus == 1);
        var list = await _deptRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktDeptTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> CreateDeptAsync(TaktDeptCreateDto dto)
    {
        SyncDeptShortNameAndIsoCode(dto);
        var entity = dto.Adapt<TaktDept>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
            _deptRepository,
            x => x.DeptCode == entity.DeptCode);
        if (!isUnique_ix_dept_code_unique)
        {
            throw new TaktBusinessException("部门的DeptCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _deptRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _deptRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleDeptIdsAsync(roleId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleDeptsAsync(roleId, ids.ToArray());
            }
        }
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeeDeptIdsAsync(employeeId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeeDeptsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetDeptByIdAsync(entity.Id) ?? entity.Adapt<TaktDeptDto>();
    }

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptAsync(long id, TaktDeptUpdateDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        SyncDeptShortNameAndIsoCode(dto);
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
            _deptRepository,
            x => x.DeptCode == entity.DeptCode,
            id);
        if (!isUnique_ix_dept_code_unique)
        {
            throw new TaktBusinessException("部门的DeptCode已存在");
        }
        await _deptRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleDeptIdsAsync(roleId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleDeptsAsync(roleId, ids.ToArray());
            }
        }
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeeDeptIdsAsync(employeeId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeeDeptsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetDeptByIdAsync(id) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDeptByIdAsync(long id)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置部门不允许删除");
        }

        var hasChildren = await _deptRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _deptRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("部门不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDeptBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _deptRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置部门不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteDeptByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptStatusAsync(TaktDeptStatusDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(dto.DeptId);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.DeptStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置部门");
        }
        entity.DeptStatus = dto.DeptStatus;
        await _deptRepository.UpdateAsync(entity);
        return await GetDeptByIdAsync(dto.DeptId) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 更新部门排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptSortAsync(TaktDeptSortDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(dto.DeptId);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _deptRepository.UpdateAsync(entity);
        return await GetDeptByIdAsync(dto.DeptId) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 更新部门内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptBuiltInAsync(TaktDeptBuiltInDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(dto.DeptId);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置部门标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _deptRepository.UpdateAsync(entity);
        return await GetDeptByIdAsync(dto.DeptId) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDeptTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDeptTemplateDto>(
            sheetName ?? "部门导入模板",
            fileName ?? "部门导入模板.xlsx");
    }

    /// <summary>
    /// 导入部门
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDeptAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDeptImportDto>(fileStream, sheetName ?? "部门导入模板");
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
                SyncDeptShortNameAndIsoCode(rows[i]);
                var entity = rows[i].Adapt<TaktDept>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.DeptCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DeptCode）");
                }
                var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _deptRepository,
                    x => x.DeptCode == entity.DeptCode);
                if (!isUnique_ix_dept_code_unique)
                {
                    throw new TaktBusinessException("部门的DeptCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _deptRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _deptRepository.CreateAsync(entity);
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
    /// 导出部门
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeptAsync(TaktDeptQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktDeptQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDeptExportDto>(),
                sheetName ?? "部门数据",
                fileName ?? "部门导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _deptRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDeptExportDto>(),
                sheetName ?? "部门数据",
                fileName ?? "部门导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDeptExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "部门数据",
            fileName ?? "部门导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建部门查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDept, bool>> QueryExpression(TaktDeptQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDept>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || (x.DeptShortName != null && x.DeptShortName.Contains(keywords))
                || (x.DeptName1 != null && x.DeptName1.Contains(keywords))
                || (x.DeptName2 != null && x.DeptName2.Contains(keywords))
                || (x.DeptPath != null && x.DeptPath.Contains(keywords))
                || (x.IsoCode != null && x.IsoCode.Contains(keywords))
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.HeadUserName != null && x.HeadUserName.Contains(keywords))
                || (x.Phone != null && x.Phone.Contains(keywords))
                || (x.Email != null && x.Email.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.DeptDescription != null && x.DeptDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptCode))
        {
            var deptCode = queryDto.DeptCode;
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(deptCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptShortName))
        {
            var deptShortName = queryDto.DeptShortName;
            exp = exp.And(x => x.DeptShortName != null && x.DeptShortName.Contains(deptShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName1))
        {
            var deptName1 = queryDto.DeptName1;
            exp = exp.And(x => x.DeptName1 != null && x.DeptName1.Contains(deptName1));
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName2))
        {
            var deptName2 = queryDto.DeptName2;
            exp = exp.And(x => x.DeptName2 != null && x.DeptName2.Contains(deptName2));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            var level = queryDto.Level.Value;
            exp = exp.And(x => x.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptPath))
        {
            var deptPath = queryDto.DeptPath;
            exp = exp.And(x => x.DeptPath != null && x.DeptPath.Contains(deptPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            var isLeaf = queryDto.IsLeaf.Value;
            exp = exp.And(x => x.IsLeaf == isLeaf);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IsoCode))
        {
            var isoCode = queryDto.IsoCode;
            exp = exp.And(x => x.IsoCode != null && x.IsoCode.Contains(isoCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterCode))
        {
            var costCenterCode = queryDto.CostCenterCode;
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(costCenterCode));
        }

        if (queryDto?.CostCategory.HasValue == true)
        {
            var costCategory = queryDto.CostCategory.Value;
            exp = exp.And(x => x.CostCategory == costCategory);
        }

        if (queryDto?.HeadUserId.HasValue == true)
        {
            var headUserId = queryDto.HeadUserId.Value;
            exp = exp.And(x => x.HeadUserId == headUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HeadUserName))
        {
            var headUserName = queryDto.HeadUserName;
            exp = exp.And(x => x.HeadUserName != null && x.HeadUserName.Contains(headUserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Phone))
        {
            var phone = queryDto.Phone;
            exp = exp.And(x => x.Phone != null && x.Phone.Contains(phone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Email))
        {
            var email = queryDto.Email;
            exp = exp.And(x => x.Email != null && x.Email.Contains(email));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Location))
        {
            var location = queryDto.Location;
            exp = exp.And(x => x.Location != null && x.Location.Contains(location));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptDescription))
        {
            var deptDescription = queryDto.DeptDescription;
            exp = exp.And(x => x.DeptDescription != null && x.DeptDescription.Contains(deptDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.DeptStatus.HasValue == true)
        {
            var deptStatus = queryDto.DeptStatus.Value;
            exp = exp.And(x => x.DeptStatus == deptStatus);
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
    private static bool HasAnyListQueryFilter(TaktDeptQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.DeptCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName2))
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (queryDto.Level.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptPath))
        {
            return true;
        }
        if (queryDto.IsLeaf.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IsoCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterCode))
        {
            return true;
        }
        if (queryDto.CostCategory.HasValue)
        {
            return true;
        }
        if (queryDto.HeadUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HeadUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Phone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Email))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Location))
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptDescription))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.DeptStatus.HasValue)
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

    /// <summary>
    /// 部门简称与 ISO 编码保持一致（实体约定二者同值）。
    /// </summary>
    /// <param name="dto">创建 DTO</param>
    private static void SyncDeptShortNameAndIsoCode(TaktDeptCreateDto dto)
    {
        var value = !string.IsNullOrWhiteSpace(dto.DeptShortName) ? dto.DeptShortName.Trim() : (dto.IsoCode ?? string.Empty).Trim();
        dto.DeptShortName = value;
        dto.IsoCode = value;
    }

    /// <summary>
    /// 部门简称与 ISO 编码保持一致（实体约定二者同值）。
    /// </summary>
    /// <param name="dto">更新 DTO</param>
    private static void SyncDeptShortNameAndIsoCode(TaktDeptUpdateDto dto)
    {
        var value = !string.IsNullOrWhiteSpace(dto.DeptShortName) ? dto.DeptShortName.Trim() : (dto.IsoCode ?? string.Empty).Trim();
        dto.DeptShortName = value;
        dto.IsoCode = value;
    }

    /// <summary>
    /// 部门简称与 ISO 编码保持一致（实体约定二者同值）。
    /// </summary>
    /// <param name="dto">导入 DTO</param>
    private static void SyncDeptShortNameAndIsoCode(TaktDeptImportDto dto)
    {
        var value = !string.IsNullOrWhiteSpace(dto.DeptShortName) ? dto.DeptShortName.Trim() : (dto.IsoCode ?? string.Empty).Trim();
        dto.DeptShortName = value;
        dto.IsoCode = value;
    }
}
