// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktMenuService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单应用服务实现
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
/// 菜单应用服务
/// </summary>
public class TaktMenuService : TaktServiceBase, ITaktMenuService
{
    private readonly ITaktTenantRepository<TaktMenu> _menuRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="menuRepository">菜单仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMenuService(
        ITaktTenantRepository<TaktMenu> menuRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _menuRepository = menuRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取菜单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMenuDto>> GetMenuListAsync(TaktMenuQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _menuRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMenuDto>.Create(
            data.Adapt<List<TaktMenuDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto?> GetMenuByIdAsync(long id)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMenuDto>();
        return dto;    }

    /// <summary>
    /// 获取菜单树形选项（一律懒加载一层 parentId；valueBy=name 时 DictValue=MenuName、ExtValue=Id 供展开）
    /// </summary>
    /// <param name="valueBy">选项值：id=菜单Id（默认）；name=菜单名称 MenuName（仍按 parentId 懒加载）</param>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配当前层 MenuName）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetMenuTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null, string? valueBy = null)
    {
        var useMenuName = string.Equals(valueBy?.Trim(), "name", StringComparison.OrdinalIgnoreCase);
        Expression<Func<TaktMenu, bool>> predicate = useMenuName
            ? (x => x.TenantCode == CurrentTenantCode
                && x.ParentId == parentId
                && x.MenuStatus == 1
                && x.MenuType == 1)
            : (x => x.TenantCode == CurrentTenantCode
                && x.ParentId == parentId
                && x.MenuStatus == 1);
        var list = await _menuRepository.GetListAsync(predicate);
        var kw = keyword?.Trim();
        if (!string.IsNullOrEmpty(kw))
        {
            list = list
                .Where(x => (x.MenuName ?? string.Empty).Contains(kw, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var label = item.MenuName ?? item.Id.ToString();
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                return new TaktTreeSelectOption
                {
                    DictValue = useMenuName ? label : item.Id.ToString(),
                    DictLabel = label,
                    ExtValue = useMenuName ? item.Id.ToString() : null,
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取菜单树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktMenuTreeDto>> GetMenuTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        Expression<Func<TaktMenu, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId && x.MenuStatus == 1);
        var list = await _menuRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktMenuTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 角色菜单分配：互斥懒加载树（一层）。
    /// 性能：一律先按 ParentId 取一层再内存过滤，禁止把上万 AssignedIds 丢进 SQL IN。
    /// 根层提升节点分批按 ParentId 查询。
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>一层树节点</returns>
    public async Task<List<TaktMenuTreeDto>> GetMenuAssignTreeAsync(TaktMenuAssignTreeQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);

        var assignedSet = new HashSet<long>();
        if (queryDto.AssignedMenuIds != null)
        {
            foreach (var raw in queryDto.AssignedMenuIds)
            {
                if (long.TryParse(raw, out var id) && id > 0)
                {
                    assignedSet.Add(id);
                }
            }
        }

        var parentId = queryDto.ParentId;
        var assignedSide = queryDto.AssignedSide;
        var tenantCode = CurrentTenantCode;

        if (parentId != 0)
        {
            var parentAssigned = assignedSet.Contains(parentId);
            if (assignedSide && !parentAssigned)
            {
                return new List<TaktMenuTreeDto>();
            }
            if (!assignedSide && parentAssigned)
            {
                return new List<TaktMenuTreeDto>();
            }

            // 【懒加载】只查当前父下一层，再内存按已分配集过滤
            var children = await _menuRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.MenuStatus == 1
                && x.ParentId == parentId);
            var filtered = assignedSide
                ? children.Where(x => assignedSet.Contains(x.Id)).ToList()
                : children.Where(x => !assignedSet.Contains(x.Id)).ToList();
            return await MapAssignTreeLayerAsync(filtered, assignedSet, assignedSide, tenantCode);
        }

        // ========== 根层 ==========
        var rootMenus = await _menuRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.MenuStatus == 1
            && x.ParentId == 0);

        List<TaktMenu> layer;
        if (assignedSide)
        {
            if (assignedSet.Count == 0)
            {
                return new List<TaktMenuTreeDto>();
            }

            layer = rootMenus.Where(x => assignedSet.Contains(x.Id)).ToList();

            // 提升根：已分配且父未分配 — 按已分配 Id 分批取实体，内存判断
            const int batchSize = 500;
            var assignedIdList = assignedSet.ToList();
            for (var i = 0; i < assignedIdList.Count; i = checked(i + batchSize))
            {
                var batch = assignedIdList.Skip(i).Take(batchSize).ToList();
                var menus = await _menuRepository.GetListAsync(m =>
                    m.TenantCode == tenantCode
                    && m.MenuStatus == 1
                    && batch.Contains(m.Id));
                foreach (var m in menus)
                {
                    if (m.ParentId != 0 && !assignedSet.Contains(m.ParentId))
                    {
                        layer.Add(m);
                    }
                }
            }

            layer = layer
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .OrderBy(x => x.SortOrder)
                .ToList();
        }
        else
        {
            layer = rootMenus.Where(x => !assignedSet.Contains(x.Id)).ToList();

            // 提升根：未分配且父已分配 — 分批 ParentId IN (assigned)
            if (assignedSet.Count > 0)
            {
                const int batchSize = 500;
                var assignedIdList = assignedSet.ToList();
                var promoted = new List<TaktMenu>();
                for (var i = 0; i < assignedIdList.Count; i = checked(i + batchSize))
                {
                    var batch = assignedIdList.Skip(i).Take(batchSize).ToList();
                    var children = await _menuRepository.GetListAsync(m =>
                        m.TenantCode == tenantCode
                        && m.MenuStatus == 1
                        && batch.Contains(m.ParentId));
                    foreach (var m in children)
                    {
                        if (!assignedSet.Contains(m.Id))
                        {
                            promoted.Add(m);
                        }
                    }
                }

                layer = layer
                    .Concat(promoted)
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            }
        }

        return await MapAssignTreeLayerAsync(layer, assignedSet, assignedSide, tenantCode);
    }

    /// <summary>
    /// 映射分配树一层，并按本侧是否仍有子设置 IsLeaf（子查询仅按 ParentId，内存过滤）
    /// </summary>
    /// <param name="layer">本层实体</param>
    /// <param name="assignedSet">已分配集</param>
    /// <param name="assignedSide">是否已分配侧</param>
    /// <param name="tenantCode">租户</param>
    /// <returns>DTO 一层</returns>
    private async Task<List<TaktMenuTreeDto>> MapAssignTreeLayerAsync(
        List<TaktMenu> layer,
        HashSet<long> assignedSet,
        bool assignedSide,
        string tenantCode)
    {
        if (layer.Count == 0)
        {
            return new List<TaktMenuTreeDto>();
        }

        var layerIds = layer.Select(x => x.Id).ToList();
        var allChildren = await _menuRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.MenuStatus == 1
            && layerIds.Contains(x.ParentId));

        var parentsWithSideChildren = allChildren
            .Where(c => assignedSide ? assignedSet.Contains(c.Id) : !assignedSet.Contains(c.Id))
            .Select(c => c.ParentId)
            .ToHashSet();

        return layer
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktMenuTreeDto>();
                treeDto.IsLeaf = parentsWithSideChildren.Contains(item.Id) ? 0 : 1;
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> CreateMenuAsync(TaktMenuCreateDto dto)
    {
        var entity = dto.Adapt<TaktMenu>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
            _menuRepository,
            x => x.MenuCode == entity.MenuCode);
        if (!isUnique_ix_menu_code_unique)
        {
            throw new TaktBusinessException("菜单的MenuCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _menuRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _menuRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleMenuIdsAsync(roleId);
                var ids = links.Select(x => x.MenuId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleMenusAsync(roleId, ids.ToArray());
            }
        }
        return await GetMenuByIdAsync(entity.Id) ?? entity.Adapt<TaktMenuDto>();
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuAsync(long id, TaktMenuUpdateDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
            _menuRepository,
            x => x.MenuCode == entity.MenuCode,
            id);
        if (!isUnique_ix_menu_code_unique)
        {
            throw new TaktBusinessException("菜单的MenuCode已存在");
        }
        await _menuRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleMenuIdsAsync(roleId);
                var ids = links.Select(x => x.MenuId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleMenusAsync(roleId, ids.ToArray());
            }
        }
        return await GetMenuByIdAsync(id) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMenuByIdAsync(long id)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置菜单不允许删除");
        }
        var deleted = await _menuRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("菜单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMenuBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _menuRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置菜单不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteMenuByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新菜单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuStatusAsync(TaktMenuStatusDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(dto.MenuId);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.MenuStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置菜单");
        }
        entity.MenuStatus = dto.MenuStatus;
        await _menuRepository.UpdateAsync(entity);
        return await GetMenuByIdAsync(dto.MenuId) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 更新菜单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuSortAsync(TaktMenuSortDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(dto.MenuId);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _menuRepository.UpdateAsync(entity);
        return await GetMenuByIdAsync(dto.MenuId) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMenuTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMenuTemplateDto>(
            sheetName ?? "菜单导入模板",
            fileName ?? "菜单导入模板.xlsx");
    }

    /// <summary>
    /// 导入菜单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMenuAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMenuImportDto>(fileStream, sheetName ?? "菜单导入模板");
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
                var entity = rows[i].Adapt<TaktMenu>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.MenuCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MenuCode）");
                }
                var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _menuRepository,
                    x => x.MenuCode == entity.MenuCode);
                if (!isUnique_ix_menu_code_unique)
                {
                    throw new TaktBusinessException("菜单的MenuCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _menuRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _menuRepository.CreateAsync(entity);
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
    /// 导出菜单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMenuAsync(TaktMenuQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMenuQueryDto());
        var list = await _menuRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMenuExportDto>(),
                sheetName ?? "菜单数据",
                fileName ?? "菜单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMenuExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "菜单数据",
            fileName ?? "菜单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建菜单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMenu, bool>> QueryExpression(TaktMenuQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMenu>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MenuCode != null && x.MenuCode.Contains(keywords))
                || (x.MenuName != null && x.MenuName.Contains(keywords))
                || (x.I18nKey != null && x.I18nKey.Contains(keywords))
                || (x.Icon != null && x.Icon.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.Level).Contains(keywords)
                || (x.MenuPath != null && x.MenuPath.Contains(keywords))
                || SqlFunc.ToString(x.IsLeaf).Contains(keywords)
                || SqlFunc.ToString(x.MenuType).Contains(keywords)
                || (x.Permission != null && x.Permission.Contains(keywords))
                || (x.RoutePath != null && x.RoutePath.Contains(keywords))
                || (x.ComponentPath != null && x.ComponentPath.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsExternal).Contains(keywords)
                || (x.ExternalUrl != null && x.ExternalUrl.Contains(keywords))
                || SqlFunc.ToString(x.IsCached).Contains(keywords)
                || SqlFunc.ToString(x.IsVisible).Contains(keywords)
                || SqlFunc.ToString(x.MenuStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || (x.MenuDescription != null && x.MenuDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuCode))
        {
            exp = exp.And(x => x.MenuCode != null && x.MenuCode.Contains(queryDto.MenuCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuName))
        {
            exp = exp.And(x => x.MenuName != null && x.MenuName.Contains(queryDto.MenuName));
        }

        if (!string.IsNullOrEmpty(queryDto?.I18nKey))
        {
            exp = exp.And(x => x.I18nKey != null && x.I18nKey.Contains(queryDto.I18nKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.Icon))
        {
            exp = exp.And(x => x.Icon != null && x.Icon.Contains(queryDto.Icon));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            exp = exp.And(x => x.Level == queryDto.Level);
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuPath))
        {
            exp = exp.And(x => x.MenuPath != null && x.MenuPath.Contains(queryDto.MenuPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            exp = exp.And(x => x.IsLeaf == queryDto.IsLeaf);
        }

        if (queryDto?.MenuType.HasValue == true)
        {
            exp = exp.And(x => x.MenuType == queryDto.MenuType);
        }

        if (!string.IsNullOrEmpty(queryDto?.Permission))
        {
            exp = exp.And(x => x.Permission != null && x.Permission.Contains(queryDto.Permission));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutePath))
        {
            exp = exp.And(x => x.RoutePath != null && x.RoutePath.Contains(queryDto.RoutePath));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComponentPath))
        {
            exp = exp.And(x => x.ComponentPath != null && x.ComponentPath.Contains(queryDto.ComponentPath));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsExternal.HasValue == true)
        {
            exp = exp.And(x => x.IsExternal == queryDto.IsExternal);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExternalUrl))
        {
            exp = exp.And(x => x.ExternalUrl != null && x.ExternalUrl.Contains(queryDto.ExternalUrl));
        }

        if (queryDto?.IsCached.HasValue == true)
        {
            exp = exp.And(x => x.IsCached == queryDto.IsCached);
        }

        if (queryDto?.IsVisible.HasValue == true)
        {
            exp = exp.And(x => x.IsVisible == queryDto.IsVisible);
        }

        if (queryDto?.MenuStatus.HasValue == true)
        {
            exp = exp.And(x => x.MenuStatus == queryDto.MenuStatus);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuDescription))
        {
            exp = exp.And(x => x.MenuDescription != null && x.MenuDescription.Contains(queryDto.MenuDescription));
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
