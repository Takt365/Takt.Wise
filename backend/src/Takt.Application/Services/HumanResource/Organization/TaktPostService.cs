// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Organization
// 文件名称：TaktPostService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位应用服务实现
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
/// 岗位应用服务
/// </summary>
public class TaktPostService : TaktServiceBase, ITaktPostService
{
    private readonly ITaktCompanyRepository<TaktPost> _postRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="postRepository">岗位仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPostService(
        ITaktCompanyRepository<TaktPost> postRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _postRepository = postRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取岗位列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPostDto>> GetPostListAsync(TaktPostQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPostDto>.Create(
                new List<TaktPostDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _postRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPostDto>.Create(
            data.Adapt<List<TaktPostDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto?> GetPostByIdAsync(long id)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPostDto>();
        return dto;    }

    /// <summary>
    /// 获取岗位选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPostOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _postRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PostStatus == 1,
            x => x.PostName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PostCode,
            DictLabel = e.PostName ?? e.PostCode,
        }).ToList();
    }

    /// <summary>
    /// 创建岗位
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> CreatePostAsync(TaktPostCreateDto dto)
    {
        var entity = dto.Adapt<TaktPost>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
            _postRepository,
            x => x.PostCode == entity.PostCode);
        if (!isUnique_ix_post_code_unique)
        {
            throw new TaktBusinessException("岗位的PostCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _postRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId, maxSort);
        }
        entity = await _postRepository.CreateAsync(entity);
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeePostIdsAsync(employeeId);
                var ids = links.Select(x => x.PostId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeePostsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetPostByIdAsync(entity.Id) ?? entity.Adapt<TaktPostDto>();
    }

    /// <summary>
    /// 更新岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostAsync(long id, TaktPostUpdateDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
            _postRepository,
            x => x.PostCode == entity.PostCode,
            id);
        if (!isUnique_ix_post_code_unique)
        {
            throw new TaktBusinessException("岗位的PostCode已存在");
        }
        await _postRepository.UpdateAsync(entity);
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeePostIdsAsync(employeeId);
                var ids = links.Select(x => x.PostId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeePostsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetPostByIdAsync(id) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 删除岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>任务</returns>
    public async Task DeletePostByIdAsync(long id)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置岗位不允许删除");
        }
        var deleted = await _postRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("岗位不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePostBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _postRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置岗位不允许删除");
        }
        foreach (var id in idList)
        {
            await DeletePostByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新岗位状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostStatusAsync(TaktPostStatusDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(dto.PostId);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.PostStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置岗位");
        }
        entity.PostStatus = dto.PostStatus;
        await _postRepository.UpdateAsync(entity);
        return await GetPostByIdAsync(dto.PostId) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 更新岗位排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostSortAsync(TaktPostSortDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(dto.PostId);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _postRepository.UpdateAsync(entity);
        return await GetPostByIdAsync(dto.PostId) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPostTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPostTemplateDto>(
            sheetName ?? "岗位导入模板",
            fileName ?? "岗位导入模板.xlsx");
    }

    /// <summary>
    /// 导入岗位
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPostAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPostImportDto>(fileStream, sheetName ?? "岗位导入模板");
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
                var entity = rows[i].Adapt<TaktPost>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PostCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PostCode）");
                }
                var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _postRepository,
                    x => x.PostCode == entity.PostCode);
                if (!isUnique_ix_post_code_unique)
                {
                    throw new TaktBusinessException("岗位的PostCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _postRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId, maxSort);
                }
                await _postRepository.CreateAsync(entity);
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
    /// 导出岗位
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPostAsync(TaktPostQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPostQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPostExportDto>(),
                sheetName ?? "岗位数据",
                fileName ?? "岗位导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _postRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPostExportDto>(),
                sheetName ?? "岗位数据",
                fileName ?? "岗位导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPostExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "岗位数据",
            fileName ?? "岗位导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建岗位查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPost, bool>> QueryExpression(TaktPostQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPost>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PostCode != null && x.PostCode.Contains(keywords))
                || (x.PostName != null && x.PostName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.PostCategory != null && x.PostCategory.Contains(keywords))
                || (x.PostLevel != null && x.PostLevel.Contains(keywords))
                || (x.Responsibilities != null && x.Responsibilities.Contains(keywords))
                || (x.Requirements != null && x.Requirements.Contains(keywords))
                || (x.PostDescription != null && x.PostDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PostCode))
        {
            var postCode = queryDto.PostCode;
            exp = exp.And(x => x.PostCode != null && x.PostCode.Contains(postCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostName))
        {
            var postName = queryDto.PostName;
            exp = exp.And(x => x.PostName != null && x.PostName.Contains(postName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostCategory))
        {
            var postCategory = queryDto.PostCategory;
            exp = exp.And(x => x.PostCategory != null && x.PostCategory.Contains(postCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostLevel))
        {
            var postLevel = queryDto.PostLevel;
            exp = exp.And(x => x.PostLevel != null && x.PostLevel.Contains(postLevel));
        }

        if (queryDto?.Headcount.HasValue == true)
        {
            var headcount = queryDto.Headcount.Value;
            exp = exp.And(x => x.Headcount == headcount);
        }

        if (queryDto?.CurrentCount.HasValue == true)
        {
            var currentCount = queryDto.CurrentCount.Value;
            exp = exp.And(x => x.CurrentCount == currentCount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Responsibilities))
        {
            var responsibilities = queryDto.Responsibilities;
            exp = exp.And(x => x.Responsibilities != null && x.Responsibilities.Contains(responsibilities));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Requirements))
        {
            var requirements = queryDto.Requirements;
            exp = exp.And(x => x.Requirements != null && x.Requirements.Contains(requirements));
        }

        if (queryDto?.EducationRequired.HasValue == true)
        {
            var educationRequired = queryDto.EducationRequired.Value;
            exp = exp.And(x => x.EducationRequired == educationRequired);
        }

        if (queryDto?.ExperienceYears.HasValue == true)
        {
            var experienceYears = queryDto.ExperienceYears.Value;
            exp = exp.And(x => x.ExperienceYears == experienceYears);
        }

        if (queryDto?.SalaryMin.HasValue == true)
        {
            var salaryMin = queryDto.SalaryMin.Value;
            exp = exp.And(x => x.SalaryMin == salaryMin);
        }

        if (queryDto?.SalaryMax.HasValue == true)
        {
            var salaryMax = queryDto.SalaryMax.Value;
            exp = exp.And(x => x.SalaryMax == salaryMax);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostDescription))
        {
            var postDescription = queryDto.PostDescription;
            exp = exp.And(x => x.PostDescription != null && x.PostDescription.Contains(postDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.PostStatus.HasValue == true)
        {
            var postStatus = queryDto.PostStatus.Value;
            exp = exp.And(x => x.PostStatus == postStatus);
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
    private static bool HasAnyListQueryFilter(TaktPostQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PostCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostLevel))
        {
            return true;
        }
        if (queryDto.Headcount.HasValue)
        {
            return true;
        }
        if (queryDto.CurrentCount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Responsibilities))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Requirements))
        {
            return true;
        }
        if (queryDto.EducationRequired.HasValue)
        {
            return true;
        }
        if (queryDto.ExperienceYears.HasValue)
        {
            return true;
        }
        if (queryDto.SalaryMin.HasValue)
        {
            return true;
        }
        if (queryDto.SalaryMax.HasValue)
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostDescription))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.PostStatus.HasValue)
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
