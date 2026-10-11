// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktTenantService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：租户应用服务实现
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
/// 租户应用服务
/// </summary>
public class TaktTenantService : TaktServiceBase, ITaktTenantService
{
    private readonly ITaktTenantRepository<TaktTenant> _tenantRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tenantRepository">租户仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTenantService(
        ITaktTenantRepository<TaktTenant> tenantRepository,
        ITaktRbacService rbacService,

        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _tenantRepository = tenantRepository;
        _rbacService = rbacService;

        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取租户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTenantDto>> GetTenantListAsync(TaktTenantQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _tenantRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTenantDto>.Create(
            data.Adapt<List<TaktTenantDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTenantDto?> GetTenantByIdAsync(long id)
    {
        var entity = await _tenantRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTenantDto>();
        return dto;    }

    /// <summary>
    /// 获取租户选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTenantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _tenantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.TenantStatus == 1,
            x => x.TenantName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TenantName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTenantDto> CreateTenantAsync(TaktTenantCreateDto dto)
    {
        var entity = dto.Adapt<TaktTenant>();
        entity.IsBuiltIn = 0;
        entity = await _tenantRepository.CreateAsync(entity);
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserTenantIdsAsync(userId);
                var codes = links.Select(x => x.TenantCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.TenantCode))
                {
                    codes.Add(entity.TenantCode);
                }
                await _rbacService.AssignUserTenantsAsync(userId, codes.ToArray());
            }
        }
        return await GetTenantByIdAsync(entity.Id) ?? entity.Adapt<TaktTenantDto>();
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTenantDto> UpdateTenantAsync(long id, TaktTenantUpdateDto dto)
    {
        var entity = await _tenantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("租户不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        await _tenantRepository.UpdateAsync(entity);
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserTenantIdsAsync(userId);
                var codes = links.Select(x => x.TenantCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.TenantCode))
                {
                    codes.Add(entity.TenantCode);
                }
                await _rbacService.AssignUserTenantsAsync(userId, codes.ToArray());
            }
        }
        return await GetTenantByIdAsync(id) ?? throw new TaktBusinessException("租户不存在");
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTenantByIdAsync(long id)
    {
        var entity = await _tenantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("租户不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置租户不允许删除");
        }
        var deleted = await _tenantRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("租户不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除租户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTenantBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _tenantRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置租户不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteTenantByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新租户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTenantDto> UpdateTenantStatusAsync(TaktTenantStatusDto dto)
    {
        var entity = await _tenantRepository.GetByIdAsync(dto.TenantId);
        if (entity == null)
        {
            throw new TaktBusinessException("租户不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.TenantStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置租户");
        }
        entity.TenantStatus = dto.TenantStatus;
        await _tenantRepository.UpdateAsync(entity);
        return await GetTenantByIdAsync(dto.TenantId) ?? throw new TaktBusinessException("租户不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTenantTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTenantTemplateDto>(
            sheetName ?? "租户导入模板",
            fileName ?? "租户导入模板.xlsx");
    }

    /// <summary>
    /// 导入租户
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTenantAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTenantImportDto>(fileStream, sheetName ?? "租户导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTenant>();
                entity.IsBuiltIn = 0;
                await _tenantRepository.CreateAsync(entity);
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
    /// 导出租户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTenantAsync(TaktTenantQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTenantQueryDto());
        var list = await _tenantRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTenantExportDto>(),
                sheetName ?? "租户数据",
                fileName ?? "租户导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTenantExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "租户数据",
            fileName ?? "租户导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建租户查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTenant, bool>> QueryExpression(TaktTenantQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTenant>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TenantName != null && x.TenantName.Contains(keywords))
                || (x.ContactName != null && x.ContactName.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.TenantStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SubscriptionStartTime).Contains(keywords)
                || SqlFunc.ToString(x.SubscriptionEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TenantName))
        {
            exp = exp.And(x => x.TenantName != null && x.TenantName.Contains(queryDto.TenantName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactName))
        {
            exp = exp.And(x => x.ContactName != null && x.ContactName.Contains(queryDto.ContactName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.TenantStatus.HasValue == true)
        {
            exp = exp.And(x => x.TenantStatus == queryDto.TenantStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.SubscriptionStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.SubscriptionStartTime >= queryDto.SubscriptionStartTimeStart);
        }

        if (queryDto?.SubscriptionStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.SubscriptionStartTime <= queryDto.SubscriptionStartTimeEnd);
        }

        if (queryDto?.SubscriptionEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.SubscriptionEndTime >= queryDto.SubscriptionEndTimeStart);
        }

        if (queryDto?.SubscriptionEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.SubscriptionEndTime <= queryDto.SubscriptionEndTimeEnd);
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
