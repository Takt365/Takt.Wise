// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktRoleService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：角色应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Identity;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 角色应用服务
/// </summary>
public class TaktRoleService : TaktServiceBase, ITaktRoleService
{
    private readonly ITaktTenantRepository<TaktRole> _roleRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleRepository">角色仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoleService(
        ITaktTenantRepository<TaktRole> roleRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _roleRepository = roleRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取角色列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoleDto>> GetRoleListAsync(TaktRoleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _roleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoleDto>.Create(
            data.Adapt<List<TaktRoleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto?> GetRoleByIdAsync(long id)
    {
        var entity = await _roleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktRoleDto>();
        dto.RoleMenus = await _rbacService.GetRoleMenuIdsAsync(entity.Id);
        dto.RoleCompanies = await _rbacService.GetRoleCompanyIdsAsync(entity.Id);
        dto.RoleDepts = await _rbacService.GetRoleDeptIdsAsync(entity.Id);
        return dto;    }

    /// <summary>
    /// 获取角色选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _roleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.RoleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RoleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto> CreateRoleAsync(TaktRoleCreateDto dto)
    {
        var entity = dto.Adapt<TaktRole>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_role_code_unique = await _uniqueValidator.IsUniqueAsync(
            _roleRepository,
            x => x.RoleCode == entity.RoleCode);
        if (!isUnique_ix_role_code_unique)
        {
            throw new TaktBusinessException("角色的RoleCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _roleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _roleRepository.CreateAsync(entity);
        if (dto.RoleMenuIds != null)
        {
            await _rbacService.AssignRoleMenusAsync(entity.Id, dto.RoleMenuIds);
        }
        if (dto.RoleCompanyCodes != null)
        {
            await _rbacService.AssignRoleCompaniesAsync(entity.Id, dto.RoleCompanyCodes);
        }
        if (dto.RoleDeptIds != null)
        {
            await _rbacService.AssignRoleDeptsAsync(entity.Id, dto.RoleDeptIds);
        }
        return await GetRoleByIdAsync(entity.Id) ?? entity.Adapt<TaktRoleDto>();
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto> UpdateRoleAsync(long id, TaktRoleUpdateDto dto)
    {
        var entity = await _roleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("角色不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_role_code_unique = await _uniqueValidator.IsUniqueAsync(
            _roleRepository,
            x => x.RoleCode == entity.RoleCode,
            id);
        if (!isUnique_ix_role_code_unique)
        {
            throw new TaktBusinessException("角色的RoleCode已存在");
        }
        await _roleRepository.UpdateAsync(entity);
        if (dto.RoleMenuIds != null)
        {
            await _rbacService.AssignRoleMenusAsync(id, dto.RoleMenuIds);
        }
        if (dto.RoleCompanyCodes != null)
        {
            await _rbacService.AssignRoleCompaniesAsync(id, dto.RoleCompanyCodes);
        }
        if (dto.RoleDeptIds != null)
        {
            await _rbacService.AssignRoleDeptsAsync(id, dto.RoleDeptIds);
        }
        return await GetRoleByIdAsync(id) ?? throw new TaktBusinessException("角色不存在");
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoleByIdAsync(long id)
    {
        var entity = await _roleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("角色不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置角色不允许删除");
        }
        await _rbacService.AssignRoleMenusAsync(id, Array.Empty<long>());
        await _rbacService.AssignRoleCompaniesAsync(id, Array.Empty<string>());
        await _rbacService.AssignRoleDeptsAsync(id, Array.Empty<long>());
        var deleted = await _roleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("角色不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除角色
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _roleRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置角色不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteRoleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新角色状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto> UpdateRoleStatusAsync(TaktRoleStatusDto dto)
    {
        var entity = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (entity == null)
        {
            throw new TaktBusinessException("角色不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.RoleStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置角色");
        }
        entity.RoleStatus = dto.RoleStatus;
        await _roleRepository.UpdateAsync(entity);
        return await GetRoleByIdAsync(dto.RoleId) ?? throw new TaktBusinessException("角色不存在");
    }

    /// <summary>
    /// 更新角色内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto> UpdateRoleBuiltInAsync(TaktRoleBuiltInDto dto)
    {
        var entity = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (entity == null)
        {
            throw new TaktBusinessException("角色不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置角色标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _roleRepository.UpdateAsync(entity);
        return await GetRoleByIdAsync(dto.RoleId) ?? throw new TaktBusinessException("角色不存在");
    }

    /// <summary>
    /// 更新角色排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoleDto> UpdateRoleSortAsync(TaktRoleSortDto dto)
    {
        var entity = await _roleRepository.GetByIdAsync(dto.RoleId);
        if (entity == null)
        {
            throw new TaktBusinessException("角色不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _roleRepository.UpdateAsync(entity);
        return await GetRoleByIdAsync(dto.RoleId) ?? throw new TaktBusinessException("角色不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoleTemplateDto>(
            sheetName ?? "角色导入模板",
            fileName ?? "角色导入模板.xlsx");
    }

    /// <summary>
    /// 导入角色
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoleImportDto>(fileStream, sheetName ?? "角色导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _roleRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktRole>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.RoleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RoleCode）");
                }
                var isUnique_ix_role_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _roleRepository,
                    x => x.RoleCode == entity.RoleCode);
                if (!isUnique_ix_role_code_unique)
                {
                    throw new TaktBusinessException("角色的RoleCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _roleRepository.CreateAsync(entity);
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
    /// 导出角色
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoleAsync(TaktRoleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktRoleQueryDto());
        var list = await _roleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoleExportDto>(),
                sheetName ?? "角色数据",
                fileName ?? "角色导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "角色数据",
            fileName ?? "角色导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建角色查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRole, bool>> QueryExpression(TaktRoleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRole>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RoleCode != null && x.RoleCode.Contains(keywords))
                || (x.RoleName != null && x.RoleName.Contains(keywords))
                || SqlFunc.ToString(x.DataScope).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.RoleStatus).Contains(keywords)
                || (x.RoleDescription != null && x.RoleDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.RoleCode))
        {
            exp = exp.And(x => x.RoleCode != null && x.RoleCode.Contains(queryDto.RoleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoleName))
        {
            exp = exp.And(x => x.RoleName != null && x.RoleName.Contains(queryDto.RoleName));
        }

        if (queryDto?.DataScope.HasValue == true)
        {
            exp = exp.And(x => x.DataScope == queryDto.DataScope);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.RoleStatus.HasValue == true)
        {
            exp = exp.And(x => x.RoleStatus == queryDto.RoleStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RoleDescription))
        {
            exp = exp.And(x => x.RoleDescription != null && x.RoleDescription.Contains(queryDto.RoleDescription));
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
