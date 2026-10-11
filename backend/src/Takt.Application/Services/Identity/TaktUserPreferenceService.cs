// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktUserPreferenceService.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：用户个人偏好应用服务实现
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

namespace Takt.Application.Services.Identity;

/// <summary>
/// 用户个人偏好应用服务
/// </summary>
public class TaktUserPreferenceService : TaktServiceBase, ITaktUserPreferenceService
{
    private readonly ITaktCompanyRepository<TaktUserPreference> _userPreferenceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userPreferenceRepository">用户个人偏好仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktUserPreferenceService(
        ITaktCompanyRepository<TaktUserPreference> userPreferenceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _userPreferenceRepository = userPreferenceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取用户个人偏好列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktUserPreferenceDto>> GetUserPreferenceListAsync(TaktUserPreferenceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktUserPreferenceDto>.Create(
                new List<TaktUserPreferenceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _userPreferenceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktUserPreferenceDto>.Create(
            data.Adapt<List<TaktUserPreferenceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktUserPreferenceDto?> GetUserPreferenceByIdAsync(long id)
    {
        var entity = await _userPreferenceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktUserPreferenceDto>();
    }

    /// <summary>
    /// 获取用户个人偏好选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetUserPreferenceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktUserPreference>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.UserPreferenceStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.PreferenceJson != null && x.PreferenceJson.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _userPreferenceRepository.GetListAsync(
            predicate,
            x => x.PreferenceJson ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PreferenceJson,
            DictLabel = e.PreferenceJson,
        }).ToList();
    }

    /// <summary>
    /// 创建用户个人偏好
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktUserPreferenceDto> CreateUserPreferenceAsync(TaktUserPreferenceCreateDto dto)
    {
        var entity = dto.Adapt<TaktUserPreference>();
        var isUnique_ix_user_preference_user_id_unique = await _uniqueValidator.IsUniqueAsync(
            _userPreferenceRepository,
            x => x.UserId == entity.UserId);
        if (!isUnique_ix_user_preference_user_id_unique)
        {
            throw new TaktBusinessException("用户个人偏好的UserId已存在");
        }
        entity = await _userPreferenceRepository.CreateAsync(entity);
        return await GetUserPreferenceByIdAsync(entity.Id) ?? entity.Adapt<TaktUserPreferenceDto>();
    }

    /// <summary>
    /// 更新用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktUserPreferenceDto> UpdateUserPreferenceAsync(long id, TaktUserPreferenceUpdateDto dto)
    {
        var entity = await _userPreferenceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用户个人偏好不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_user_preference_user_id_unique = await _uniqueValidator.IsUniqueAsync(
            _userPreferenceRepository,
            x => x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_user_preference_user_id_unique)
        {
            throw new TaktBusinessException("用户个人偏好的UserId已存在");
        }
        await _userPreferenceRepository.UpdateAsync(entity);
        return await GetUserPreferenceByIdAsync(id) ?? throw new TaktBusinessException("用户个人偏好不存在");
    }

    /// <summary>
    /// 删除用户个人偏好
    /// </summary>
    /// <param name="id">用户个人偏好ID</param>
    /// <returns>任务</returns>
    public async Task DeleteUserPreferenceByIdAsync(long id)
    {
        var deleted = await _userPreferenceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("用户个人偏好不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除用户个人偏好
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteUserPreferenceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteUserPreferenceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新用户个人偏好状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktUserPreferenceDto> UpdateUserPreferenceStatusAsync(TaktUserPreferenceStatusDto dto)
    {
        var entity = await _userPreferenceRepository.GetByIdAsync(dto.UserPreferenceId);
        if (entity == null)
        {
            throw new TaktBusinessException("用户个人偏好不存在");
        }
        entity.UserPreferenceStatus = dto.UserPreferenceStatus;
        await _userPreferenceRepository.UpdateAsync(entity);
        return await GetUserPreferenceByIdAsync(dto.UserPreferenceId) ?? throw new TaktBusinessException("用户个人偏好不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetUserPreferenceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktUserPreferenceTemplateDto>(
            sheetName ?? "用户个人偏好导入模板",
            fileName ?? "用户个人偏好导入模板.xlsx");
    }

    /// <summary>
    /// 导入用户个人偏好
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportUserPreferenceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktUserPreferenceImportDto>(fileStream, sheetName ?? "用户个人偏好导入模板");
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
                var entity = rows[i].Adapt<TaktUserPreference>();
                var importKey = $"{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（UserId）");
                }
                var isUnique_ix_user_preference_user_id_unique = await _uniqueValidator.IsUniqueAsync(
                    _userPreferenceRepository,
                    x => x.UserId == entity.UserId);
                if (!isUnique_ix_user_preference_user_id_unique)
                {
                    throw new TaktBusinessException("用户个人偏好的UserId已存在");
                }
                await _userPreferenceRepository.CreateAsync(entity);
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
    /// 导出用户个人偏好
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportUserPreferenceAsync(TaktUserPreferenceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktUserPreferenceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktUserPreferenceExportDto>(),
                sheetName ?? "用户个人偏好数据",
                fileName ?? "用户个人偏好导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _userPreferenceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktUserPreferenceExportDto>(),
                sheetName ?? "用户个人偏好数据",
                fileName ?? "用户个人偏好导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktUserPreferenceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用户个人偏好数据",
            fileName ?? "用户个人偏好导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建用户个人偏好查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktUserPreference, bool>> QueryExpression(TaktUserPreferenceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktUserPreference>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PreferenceJson != null && x.PreferenceJson.Contains(keywords))
                || (x.UserLayout != null && x.UserLayout.Contains(keywords))
                || (x.UserTheme != null && x.UserTheme.Contains(keywords))
                || (x.UserColor != null && x.UserColor.Contains(keywords))
                || (x.UserLocale != null && x.UserLocale.Contains(keywords))
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

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PreferenceJson))
        {
            var preferenceJson = queryDto.PreferenceJson;
            exp = exp.And(x => x.PreferenceJson != null && x.PreferenceJson.Contains(preferenceJson));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserLayout))
        {
            var layout = queryDto.UserLayout;
            exp = exp.And(x => x.UserLayout != null && x.UserLayout.Contains(layout));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserTheme))
        {
            var theme = queryDto.UserTheme;
            exp = exp.And(x => x.UserTheme != null && x.UserTheme.Contains(theme));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserColor))
        {
            var color = queryDto.UserColor;
            exp = exp.And(x => x.UserColor != null && x.UserColor.Contains(color));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserLocale))
        {
            var locale = queryDto.UserLocale;
            exp = exp.And(x => x.UserLocale != null && x.UserLocale.Contains(locale));
        }

        if (queryDto?.UserPreferenceStatus.HasValue == true)
        {
            var userPreferenceStatus = queryDto.UserPreferenceStatus.Value;
            exp = exp.And(x => x.UserPreferenceStatus == userPreferenceStatus);
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
    private static bool HasAnyListQueryFilter(TaktUserPreferenceQueryDto? queryDto)
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
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PreferenceJson))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserLayout))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserTheme))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserColor))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserLocale))
        {
            return true;
        }
        if (queryDto.UserPreferenceStatus.HasValue)
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
    /// 获取当前登录用户的个人偏好（无记录返回 null）
    /// </summary>
    /// <returns>DTO 或 null</returns>
    public async Task<TaktUserPreferenceDto?> GetMyUserPreferenceAsync()
    {
        EnsureThreeLayerContext();
        if (!IsAuthenticated || CurrentUserId is null or <= 0)
        {
            ThrowBusinessException("用户未登录");
        }

        var userId = CurrentUserId.Value;
        var entity = await _userPreferenceRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.UserId == userId
            && x.IsDeleted == 0);
        return entity?.Adapt<TaktUserPreferenceDto>();
    }

    /// <summary>
    /// 按当前登录用户 Upsert 个人偏好（忽略客户端传入的 UserId）
    /// </summary>
    /// <param name="dto">Upsert DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktUserPreferenceDto> UpsertMyUserPreferenceAsync(TaktUserPreferenceMeUpsertDto dto)
    {
        EnsureThreeLayerContext();
        if (!IsAuthenticated || CurrentUserId is null or <= 0)
        {
            ThrowBusinessException("用户未登录");
        }

        ArgumentNullException.ThrowIfNull(dto);
        var json = ClampPreferenceJson(dto.PreferenceJson);
        ApplyHotFieldsFromJson(json, out var layout, out var theme, out var color, out var locale);

        var userId = CurrentUserId.Value;
        var entity = await _userPreferenceRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.UserId == userId
            && x.IsDeleted == 0);

        if (entity == null)
        {
            entity = new TaktUserPreference
            {
                UserId = userId,
                PreferenceJson = json,
                UserLayout = layout,
                UserTheme = theme,
                UserColor = color,
                UserLocale = locale,
                UserPreferenceStatus = 1,
            };
            entity = await _userPreferenceRepository.CreateAsync(entity);
        }
        else
        {
            entity.PreferenceJson = json;
            entity.UserLayout = layout;
            entity.UserTheme = theme;
            entity.UserColor = color;
            entity.UserLocale = locale;
            await _userPreferenceRepository.UpdateAsync(entity);
        }

        return await GetUserPreferenceByIdAsync(entity.Id)
            ?? entity.Adapt<TaktUserPreferenceDto>();
    }

    /// <summary>
    /// 规范化偏好 JSON（ntext；软上限防 DoS）
    /// </summary>
    /// <param name="preferenceJson">原始 JSON</param>
    /// <returns>规范化后的 JSON</returns>
    private static string ClampPreferenceJson(string? preferenceJson)
    {
        const int maxLength = 524_288;
        var json = string.IsNullOrWhiteSpace(preferenceJson) ? "{}" : preferenceJson.Trim();
        if (json.Length > maxLength)
        {
            throw new TaktBusinessException("偏好JSON过大");
        }

        return json;
    }

    /// <summary>
    /// 从 PreferenceJson 解析冗余 UserLayout / UserTheme / UserColor / UserLocale
    /// </summary>
    /// <param name="preferenceJson">偏好 JSON</param>
    /// <param name="layout">布局</param>
    /// <param name="theme">主题</param>
    /// <param name="color">颜色（themeColor.type）</param>
    /// <param name="locale">界面语言</param>
    private static void ApplyHotFieldsFromJson(string preferenceJson, out string layout, out string theme, out string color, out string locale)
    {
        layout = "side";
        theme = "light";
        color = "blue";
        locale = "zh-CN";
        try
        {
            var token = Newtonsoft.Json.Linq.JToken.Parse(preferenceJson);
            var layoutToken = token.SelectToken("layout")?.ToString()?.Trim();
            var themeToken = token.SelectToken("theme")?.ToString()?.Trim();
            var colorToken = token.SelectToken("themeColor.type")?.ToString()?.Trim();
            var localeToken = token.SelectToken("locale")?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(layoutToken))
            {
                layout = layoutToken.Length > 40 ? layoutToken[..40] : layoutToken;
            }

            if (!string.IsNullOrWhiteSpace(themeToken))
            {
                theme = themeToken.Length > 40 ? themeToken[..40] : themeToken;
            }

            if (!string.IsNullOrWhiteSpace(colorToken))
            {
                color = colorToken.Length > 40 ? colorToken[..40] : colorToken;
            }

            if (!string.IsNullOrWhiteSpace(localeToken))
            {
                locale = localeToken.Length > 5 ? localeToken[..5] : localeToken;
            }
        }
        catch (Newtonsoft.Json.JsonException)
        {
            throw new TaktBusinessException("偏好JSON格式无效");
        }
    }
}
