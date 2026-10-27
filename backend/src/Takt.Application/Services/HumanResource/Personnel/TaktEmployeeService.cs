// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Application.Services.Identity;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工应用服务
/// </summary>
public class TaktEmployeeService : TaktServiceBase, ITaktEmployeeService
{
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktCompanyRepository<TaktEmployeeAddress> _employeeAddressRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeEducation> _employeeEducationRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeFamily> _employeeFamilyRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeExperience> _employeeExperienceRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeSkill> _employeeSkillRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeContract> _employeeContractRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeOnboarding> _employeeOnboardingRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeReassignment> _employeeReassignmentRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeResignation> _employeeResignationRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeAttachment> _employeeAttachmentRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeDelegation> _employeeDelegationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="employeeAddressRepository">EmployeeAddress仓储</param>
    /// <param name="employeeEducationRepository">EmployeeEducation仓储</param>
    /// <param name="employeeFamilyRepository">EmployeeFamily仓储</param>
    /// <param name="employeeExperienceRepository">EmployeeExperience仓储</param>
    /// <param name="employeeSkillRepository">EmployeeSkill仓储</param>
    /// <param name="employeeContractRepository">EmployeeContract仓储</param>
    /// <param name="employeeJoinedRepository">EmployeeJoined仓储</param>
    /// <param name="employeeOnboardingRepository">EmployeeOnboarding仓储</param>
    /// <param name="employeeReassignmentRepository">EmployeeReassignment仓储</param>
    /// <param name="employeeResignationRepository">EmployeeResignation仓储</param>
    /// <param name="employeeAttachmentRepository">EmployeeAttachment仓储</param>
    /// <param name="employeeDelegationRepository">EmployeeDelegation仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeService(
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktRbacService rbacService,

        ITaktCompanyRepository<TaktEmployeeAddress> employeeAddressRepository,
        ITaktCompanyRepository<TaktEmployeeEducation> employeeEducationRepository,
        ITaktCompanyRepository<TaktEmployeeFamily> employeeFamilyRepository,
        ITaktCompanyRepository<TaktEmployeeExperience> employeeExperienceRepository,
        ITaktCompanyRepository<TaktEmployeeSkill> employeeSkillRepository,
        ITaktCompanyRepository<TaktEmployeeContract> employeeContractRepository,
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktCompanyRepository<TaktEmployeeOnboarding> employeeOnboardingRepository,
        ITaktApprovalRepository<TaktEmployeeReassignment> employeeReassignmentRepository,
        ITaktApprovalRepository<TaktEmployeeResignation> employeeResignationRepository,
        ITaktCompanyRepository<TaktEmployeeAttachment> employeeAttachmentRepository,
        ITaktCompanyRepository<TaktEmployeeDelegation> employeeDelegationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeRepository = employeeRepository;
        _rbacService = rbacService;

        _employeeAddressRepository = employeeAddressRepository;
        _employeeEducationRepository = employeeEducationRepository;
        _employeeFamilyRepository = employeeFamilyRepository;
        _employeeExperienceRepository = employeeExperienceRepository;
        _employeeSkillRepository = employeeSkillRepository;
        _employeeContractRepository = employeeContractRepository;
        _employeeJoinedRepository = employeeJoinedRepository;
        _employeeOnboardingRepository = employeeOnboardingRepository;
        _employeeReassignmentRepository = employeeReassignmentRepository;
        _employeeResignationRepository = employeeResignationRepository;
        _employeeAttachmentRepository = employeeAttachmentRepository;
        _employeeDelegationRepository = employeeDelegationRepository;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取员工列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeDto>> GetEmployeeListAsync(TaktEmployeeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEmployeeDto>.Create(
                new List<TaktEmployeeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeDto>.Create(
            data.Adapt<List<TaktEmployeeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto?> GetEmployeeByIdAsync(long id)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEmployeeDto>();
        dto.EmployeeDepts = await _rbacService.GetEmployeeDeptIdsAsync(entity.Id);
        dto.EmployeePosts = await _rbacService.GetEmployeePostIdsAsync(entity.Id);
        return dto;    }

    /// <summary>
    /// 获取员工选项列表（DictValue=Id，ExtValue=EmployeeName）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _employeeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EmployeeStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.EmployeeCode,
            ExtValue = e.EmployeeName,
        }).ToList();
    }

    /// <summary>
    /// 创建员工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> CreateEmployeeAsync(TaktEmployeeCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployee>();
        entity.IsBuiltIn = 0;
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _employeeRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.EmployeeCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.EmployeeCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.EmployeeCode))
        {
            throw new TaktBusinessException("员工编码不能为空");
        }
        var isUnique_ix_employee_code_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeRepository,
            x => x.EmployeeCode == entity.EmployeeCode);
        if (!isUnique_ix_employee_code_unique)
        {
            throw new TaktBusinessException("员工的EmployeeCode已存在");
        }
        entity = await _employeeRepository.CreateAsync(entity);
        if (dto.EmployeeDeptIds != null)
        {
            await _rbacService.AssignEmployeeDeptsAsync(entity.Id, dto.EmployeeDeptIds);
        }
        if (dto.EmployeePostIds != null)
        {
            await _rbacService.AssignEmployeePostsAsync(entity.Id, dto.EmployeePostIds);
        }
                await SaveEmployeeChildrenAsync(entity, dto);
        return await GetEmployeeByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeDto>();
    }

    /// <summary>
    /// 更新员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> UpdateEmployeeAsync(long id, TaktEmployeeUpdateDto dto)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        var originalEmployeeStatus = entity.EmployeeStatus;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        if (entity.IsBuiltIn == 1 && entity.EmployeeStatus != originalEmployeeStatus
            && (entity.EmployeeStatus == 3 || entity.EmployeeStatus == 4))
        {
            throw new TaktBusinessException("不允许将内置员工设为离职或退休");
        }
        var isUnique_ix_employee_code_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeRepository,
            x => x.EmployeeCode == entity.EmployeeCode,
            id);
        if (!isUnique_ix_employee_code_unique)
        {
            throw new TaktBusinessException("员工的EmployeeCode已存在");
        }
        await _employeeRepository.UpdateAsync(entity);
        if (dto.EmployeeDeptIds != null)
        {
            await _rbacService.AssignEmployeeDeptsAsync(id, dto.EmployeeDeptIds);
        }
        if (dto.EmployeePostIds != null)
        {
            await _rbacService.AssignEmployeePostsAsync(id, dto.EmployeePostIds);
        }
                await SaveEmployeeChildrenAsync(entity, dto);
        return await GetEmployeeByIdAsync(id) ?? throw new TaktBusinessException("员工不存在");
    }

    /// <summary>
    /// 删除员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeByIdAsync(long id)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置员工不允许删除");
        }
        await _rbacService.AssignEmployeeDeptsAsync(id, Array.Empty<long>());
        await _rbacService.AssignEmployeePostsAsync(id, Array.Empty<long>());
        var deleted = await _employeeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _employeeRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置员工不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新员工状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> UpdateEmployeeStatusAsync(TaktEmployeeStatusDto dto)
    {
        var entity = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        entity.MaritalStatus = dto.MaritalStatus;
        await _employeeRepository.UpdateAsync(entity);
        return await GetEmployeeByIdAsync(dto.EmployeeId) ?? throw new TaktBusinessException("员工不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeTemplateDto>(
            sheetName ?? "员工导入模板",
            fileName ?? "员工导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeImportDto>(fileStream, sheetName ?? "员工导入模板");
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
                var entity = rows[i].Adapt<TaktEmployee>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.EmployeeCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EmployeeCode）");
                }
                var isUnique_ix_employee_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeRepository,
                    x => x.EmployeeCode == entity.EmployeeCode);
                if (!isUnique_ix_employee_code_unique)
                {
                    throw new TaktBusinessException("员工的EmployeeCode已存在");
                }
                await _employeeRepository.CreateAsync(entity);
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
    /// 导出员工
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeAsync(TaktEmployeeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEmployeeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeExportDto>(),
                sheetName ?? "员工数据",
                fileName ?? "员工导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _employeeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeExportDto>(),
                sheetName ?? "员工数据",
                fileName ?? "员工导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工数据",
            fileName ?? "员工导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充员工详情（加载 OneToMany 子表：员工地址、员工教育经历、员工家庭成员、员工工作经历、员工技能、员工劳动合同、员工入职上岗、入职待办、员工调动、员工离职、员工附件、员工代理关系）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEmployeeDetailsAsync(TaktEmployeeDto dto, TaktEmployee entity)
    {
        if (dto == null)
        {
            return;
        }
        // 员工地址 → dto.EmployeeAddresses
        var employeeaddresses = await _employeeAddressRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeAddresses = employeeaddresses.Adapt<List<TaktEmployeeAddressDto>>();
        // 员工教育经历 → dto.EmployeeEducations
        var employeeeducations = await _employeeEducationRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeEducations = employeeeducations.Adapt<List<TaktEmployeeEducationDto>>();
        // 员工家庭成员 → dto.EmployeeFamilies
        var employeefamilies = await _employeeFamilyRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeFamilies = employeefamilies.Adapt<List<TaktEmployeeFamilyDto>>();
        // 员工工作经历 → dto.EmployeeExperiences
        var employeeexperiences = await _employeeExperienceRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeExperiences = employeeexperiences.Adapt<List<TaktEmployeeExperienceDto>>();
        // 员工技能 → dto.EmployeeSkills
        var employeeskills = await _employeeSkillRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeSkills = employeeskills.Adapt<List<TaktEmployeeSkillDto>>();
        // 员工劳动合同 → dto.EmployeeContracts
        var employeecontracts = await _employeeContractRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeContracts = employeecontracts.Adapt<List<TaktEmployeeContractDto>>();
        // 员工入职上岗 → dto.EmployeeJoineds
        var employeejoineds = await _employeeJoinedRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeJoineds = employeejoineds.Adapt<List<TaktEmployeeJoinedDto>>();
        // 入职待办 → dto.EmployeeOnboardings
        var employeeonboardings = await _employeeOnboardingRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeOnboardings = employeeonboardings.Adapt<List<TaktEmployeeOnboardingDto>>();
        // 员工调动 → dto.EmployeeReassignments
        var employeereassignments = await _employeeReassignmentRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeReassignments = employeereassignments.Adapt<List<TaktEmployeeReassignmentDto>>();
        // 员工离职 → dto.EmployeeResignations
        var employeeresignations = await _employeeResignationRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeResignations = employeeresignations.Adapt<List<TaktEmployeeResignationDto>>();
        // 员工附件 → dto.EmployeeAttachments
        var employeeattachments = await _employeeAttachmentRepository.GetListAsync(x => x.EmployeeId == entity.Id);
        dto.EmployeeAttachments = employeeattachments.Adapt<List<TaktEmployeeAttachmentDto>>();
        // 员工代理关系 → dto.EmployeeDelegations
        var employeedelegations = await _employeeDelegationRepository.GetListAsync(x => x.OriginalEmployeeId == entity.Id);
        dto.EmployeeDelegations = employeedelegations.Adapt<List<TaktEmployeeDelegationDto>>();
    }

    /// <summary>
    /// 保存员工子表级联（员工地址、员工教育经历、员工家庭成员、员工工作经历、员工技能、员工劳动合同、员工入职上岗、入职待办、员工调动、员工离职、员工附件、员工代理关系；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEmployeeChildrenAsync(TaktEmployee entity, TaktEmployeeCreateDto dto)
    {
        // 员工地址（EmployeeAddresses）
        List<TaktEmployeeAddressUpdateDto>? employeeAddressesForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeAddresses && updateDtoForEmployeeAddresses.EmployeeAddresses != null)
        {
            employeeAddressesForSave = updateDtoForEmployeeAddresses.EmployeeAddresses;
        }
        else if (dto.EmployeeAddresses != null)
        {
            employeeAddressesForSave = dto.EmployeeAddresses.Adapt<List<TaktEmployeeAddressUpdateDto>>();
        }
        else
        {
            employeeAddressesForSave = null;
        }
        if (employeeAddressesForSave is not { Count: > 0 })
        {
            await _employeeAddressRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeAddressRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeAddress>();
            for (var i = 0; i < employeeAddressesForSave.Count; i++)
            {
                var childDto = employeeAddressesForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeAddressId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeAddressId, out var target))
                    {
                        throw new TaktBusinessException("员工地址不存在（EmployeeAddressId={childDto.EmployeeAddressId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工地址不属于当前主表（EmployeeAddressId={childDto.EmployeeAddressId}）");
                    }
                    submittedIds.Add(childDto.EmployeeAddressId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeAddressId;
                    target.EmployeeId = entity.Id;
                    await _employeeAddressRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeAddress>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeAddressRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeAddressRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工教育经历（EmployeeEducations）
        List<TaktEmployeeEducationUpdateDto>? employeeEducationsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeEducations && updateDtoForEmployeeEducations.EmployeeEducations != null)
        {
            employeeEducationsForSave = updateDtoForEmployeeEducations.EmployeeEducations;
        }
        else if (dto.EmployeeEducations != null)
        {
            employeeEducationsForSave = dto.EmployeeEducations.Adapt<List<TaktEmployeeEducationUpdateDto>>();
        }
        else
        {
            employeeEducationsForSave = null;
        }
        if (employeeEducationsForSave is not { Count: > 0 })
        {
            await _employeeEducationRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeEducationRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeEducation>();
            for (var i = 0; i < employeeEducationsForSave.Count; i++)
            {
                var childDto = employeeEducationsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeEducationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeEducationId, out var target))
                    {
                        throw new TaktBusinessException("员工教育经历不存在（EmployeeEducationId={childDto.EmployeeEducationId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工教育经历不属于当前主表（EmployeeEducationId={childDto.EmployeeEducationId}）");
                    }
                    submittedIds.Add(childDto.EmployeeEducationId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeEducationId;
                    target.EmployeeId = entity.Id;
                    await _employeeEducationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeEducation>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeEducationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeEducationRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工家庭成员（EmployeeFamilies）
        List<TaktEmployeeFamilyUpdateDto>? employeeFamiliesForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeFamilies && updateDtoForEmployeeFamilies.EmployeeFamilies != null)
        {
            employeeFamiliesForSave = updateDtoForEmployeeFamilies.EmployeeFamilies;
        }
        else if (dto.EmployeeFamilies != null)
        {
            employeeFamiliesForSave = dto.EmployeeFamilies.Adapt<List<TaktEmployeeFamilyUpdateDto>>();
        }
        else
        {
            employeeFamiliesForSave = null;
        }
        if (employeeFamiliesForSave is not { Count: > 0 })
        {
            await _employeeFamilyRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeFamilyRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeFamily>();
            for (var i = 0; i < employeeFamiliesForSave.Count; i++)
            {
                var childDto = employeeFamiliesForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeFamilyId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeFamilyId, out var target))
                    {
                        throw new TaktBusinessException("员工家庭成员不存在（EmployeeFamilyId={childDto.EmployeeFamilyId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工家庭成员不属于当前主表（EmployeeFamilyId={childDto.EmployeeFamilyId}）");
                    }
                    submittedIds.Add(childDto.EmployeeFamilyId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeFamilyId;
                    target.EmployeeId = entity.Id;
                    await _employeeFamilyRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeFamily>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeFamilyRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeFamilyRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工工作经历（EmployeeExperiences）
        List<TaktEmployeeExperienceUpdateDto>? employeeExperiencesForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeExperiences && updateDtoForEmployeeExperiences.EmployeeExperiences != null)
        {
            employeeExperiencesForSave = updateDtoForEmployeeExperiences.EmployeeExperiences;
        }
        else if (dto.EmployeeExperiences != null)
        {
            employeeExperiencesForSave = dto.EmployeeExperiences.Adapt<List<TaktEmployeeExperienceUpdateDto>>();
        }
        else
        {
            employeeExperiencesForSave = null;
        }
        if (employeeExperiencesForSave is not { Count: > 0 })
        {
            await _employeeExperienceRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeExperienceRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeExperience>();
            for (var i = 0; i < employeeExperiencesForSave.Count; i++)
            {
                var childDto = employeeExperiencesForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeExperienceId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeExperienceId, out var target))
                    {
                        throw new TaktBusinessException("员工工作经历不存在（EmployeeExperienceId={childDto.EmployeeExperienceId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工工作经历不属于当前主表（EmployeeExperienceId={childDto.EmployeeExperienceId}）");
                    }
                    submittedIds.Add(childDto.EmployeeExperienceId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeExperienceId;
                    target.EmployeeId = entity.Id;
                    await _employeeExperienceRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeExperience>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeExperienceRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeExperienceRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工技能（EmployeeSkills）
        List<TaktEmployeeSkillUpdateDto>? employeeSkillsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeSkills && updateDtoForEmployeeSkills.EmployeeSkills != null)
        {
            employeeSkillsForSave = updateDtoForEmployeeSkills.EmployeeSkills;
        }
        else if (dto.EmployeeSkills != null)
        {
            employeeSkillsForSave = dto.EmployeeSkills.Adapt<List<TaktEmployeeSkillUpdateDto>>();
        }
        else
        {
            employeeSkillsForSave = null;
        }
        if (employeeSkillsForSave is not { Count: > 0 })
        {
            await _employeeSkillRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeSkillRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeSkill>();
            for (var i = 0; i < employeeSkillsForSave.Count; i++)
            {
                var childDto = employeeSkillsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeSkillId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeSkillId, out var target))
                    {
                        throw new TaktBusinessException("员工技能不存在（EmployeeSkillId={childDto.EmployeeSkillId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工技能不属于当前主表（EmployeeSkillId={childDto.EmployeeSkillId}）");
                    }
                    submittedIds.Add(childDto.EmployeeSkillId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeSkillId;
                    target.EmployeeId = entity.Id;
                    await _employeeSkillRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeSkill>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeSkillRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeSkillRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工劳动合同（EmployeeContracts）
        List<TaktEmployeeContractUpdateDto>? employeeContractsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeContracts && updateDtoForEmployeeContracts.EmployeeContracts != null)
        {
            employeeContractsForSave = updateDtoForEmployeeContracts.EmployeeContracts;
        }
        else if (dto.EmployeeContracts != null)
        {
            employeeContractsForSave = dto.EmployeeContracts.Adapt<List<TaktEmployeeContractUpdateDto>>();
        }
        else
        {
            employeeContractsForSave = null;
        }
        if (employeeContractsForSave is not { Count: > 0 })
        {
            await _employeeContractRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeContractRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeContract>();
            for (var i = 0; i < employeeContractsForSave.Count; i++)
            {
                var childDto = employeeContractsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeContractId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeContractId, out var target))
                    {
                        throw new TaktBusinessException("员工劳动合同不存在（EmployeeContractId={childDto.EmployeeContractId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工劳动合同不属于当前主表（EmployeeContractId={childDto.EmployeeContractId}）");
                    }
                    submittedIds.Add(childDto.EmployeeContractId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeContractId;
                    target.EmployeeId = entity.Id;
                    await _employeeContractRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeContract>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeContractRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeContractRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工入职上岗（EmployeeJoineds）
        List<TaktEmployeeJoinedUpdateDto>? employeeJoinedsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeJoineds && updateDtoForEmployeeJoineds.EmployeeJoineds != null)
        {
            employeeJoinedsForSave = updateDtoForEmployeeJoineds.EmployeeJoineds;
        }
        else if (dto.EmployeeJoineds != null)
        {
            employeeJoinedsForSave = dto.EmployeeJoineds.Adapt<List<TaktEmployeeJoinedUpdateDto>>();
        }
        else
        {
            employeeJoinedsForSave = null;
        }
        if (employeeJoinedsForSave is not { Count: > 0 })
        {
            await _employeeJoinedRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeJoinedRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeJoined>();
            for (var i = 0; i < employeeJoinedsForSave.Count; i++)
            {
                var childDto = employeeJoinedsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeJoinedId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeJoinedId, out var target))
                    {
                        throw new TaktBusinessException("员工入职上岗不存在（EmployeeJoinedId={childDto.EmployeeJoinedId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工入职上岗不属于当前主表（EmployeeJoinedId={childDto.EmployeeJoinedId}）");
                    }
                    submittedIds.Add(childDto.EmployeeJoinedId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeJoinedId;
                    target.EmployeeId = entity.Id;
                    await _employeeJoinedRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeJoined>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeJoinedRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeJoinedRepository.CreateRangeAsync(toCreate);
            }
        }
        // 入职待办（EmployeeOnboardings）
        List<TaktEmployeeOnboardingUpdateDto>? employeeOnboardingsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeOnboardings && updateDtoForEmployeeOnboardings.EmployeeOnboardings != null)
        {
            employeeOnboardingsForSave = updateDtoForEmployeeOnboardings.EmployeeOnboardings;
        }
        else if (dto.EmployeeOnboardings != null)
        {
            employeeOnboardingsForSave = dto.EmployeeOnboardings.Adapt<List<TaktEmployeeOnboardingUpdateDto>>();
        }
        else
        {
            employeeOnboardingsForSave = null;
        }
        if (employeeOnboardingsForSave is not { Count: > 0 })
        {
            await _employeeOnboardingRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeOnboardingRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeOnboarding>();
            for (var i = 0; i < employeeOnboardingsForSave.Count; i++)
            {
                var childDto = employeeOnboardingsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.Mobile = entity.Mobile;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeOnboardingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeOnboardingId, out var target))
                    {
                        throw new TaktBusinessException("入职待办不存在（EmployeeOnboardingId={childDto.EmployeeOnboardingId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("入职待办不属于当前主表（EmployeeOnboardingId={childDto.EmployeeOnboardingId}）");
                    }
                    submittedIds.Add(childDto.EmployeeOnboardingId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeOnboardingId;
                    target.EmployeeId = entity.Id;
                    await _employeeOnboardingRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeOnboarding>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeOnboardingRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeOnboardingRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工调动（EmployeeReassignments）
        List<TaktEmployeeReassignmentUpdateDto>? employeeReassignmentsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeReassignments && updateDtoForEmployeeReassignments.EmployeeReassignments != null)
        {
            employeeReassignmentsForSave = updateDtoForEmployeeReassignments.EmployeeReassignments;
        }
        else if (dto.EmployeeReassignments != null)
        {
            employeeReassignmentsForSave = dto.EmployeeReassignments.Adapt<List<TaktEmployeeReassignmentUpdateDto>>();
        }
        else
        {
            employeeReassignmentsForSave = null;
        }
        if (employeeReassignmentsForSave is not { Count: > 0 })
        {
            await _employeeReassignmentRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeReassignmentRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeReassignment>();
            for (var i = 0; i < employeeReassignmentsForSave.Count; i++)
            {
                var childDto = employeeReassignmentsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeReassignmentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeReassignmentId, out var target))
                    {
                        throw new TaktBusinessException("员工调动不存在（EmployeeReassignmentId={childDto.EmployeeReassignmentId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工调动不属于当前主表（EmployeeReassignmentId={childDto.EmployeeReassignmentId}）");
                    }
                    submittedIds.Add(childDto.EmployeeReassignmentId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeReassignmentId;
                    target.EmployeeId = entity.Id;
                    await _employeeReassignmentRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeReassignment>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeReassignmentRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeReassignmentRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工离职（EmployeeResignations）
        List<TaktEmployeeResignationUpdateDto>? employeeResignationsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeResignations && updateDtoForEmployeeResignations.EmployeeResignations != null)
        {
            employeeResignationsForSave = updateDtoForEmployeeResignations.EmployeeResignations;
        }
        else if (dto.EmployeeResignations != null)
        {
            employeeResignationsForSave = dto.EmployeeResignations.Adapt<List<TaktEmployeeResignationUpdateDto>>();
        }
        else
        {
            employeeResignationsForSave = null;
        }
        if (employeeResignationsForSave is not { Count: > 0 })
        {
            await _employeeResignationRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeResignationRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeResignation>();
            for (var i = 0; i < employeeResignationsForSave.Count; i++)
            {
                var childDto = employeeResignationsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeResignationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeResignationId, out var target))
                    {
                        throw new TaktBusinessException("员工离职不存在（EmployeeResignationId={childDto.EmployeeResignationId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工离职不属于当前主表（EmployeeResignationId={childDto.EmployeeResignationId}）");
                    }
                    submittedIds.Add(childDto.EmployeeResignationId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeResignationId;
                    target.EmployeeId = entity.Id;
                    await _employeeResignationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeResignation>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeResignationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeResignationRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工附件（EmployeeAttachments）
        List<TaktEmployeeAttachmentUpdateDto>? employeeAttachmentsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeAttachments && updateDtoForEmployeeAttachments.EmployeeAttachments != null)
        {
            employeeAttachmentsForSave = updateDtoForEmployeeAttachments.EmployeeAttachments;
        }
        else if (dto.EmployeeAttachments != null)
        {
            employeeAttachmentsForSave = dto.EmployeeAttachments.Adapt<List<TaktEmployeeAttachmentUpdateDto>>();
        }
        else
        {
            employeeAttachmentsForSave = null;
        }
        if (employeeAttachmentsForSave is not { Count: > 0 })
        {
            await _employeeAttachmentRepository.DeleteAsync(x => x.EmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeAttachmentRepository.GetListAsync(x => x.EmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeAttachment>();
            for (var i = 0; i < employeeAttachmentsForSave.Count; i++)
            {
                var childDto = employeeAttachmentsForSave[i];
                childDto.EmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EmployeeCode = entity.EmployeeCode;
                childDto.EmployeeName = entity.EmployeeName;
                if (childDto.EmployeeAttachmentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeAttachmentId, out var target))
                    {
                        throw new TaktBusinessException("员工附件不存在（EmployeeAttachmentId={childDto.EmployeeAttachmentId}）");
                    }
                    if (target.EmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工附件不属于当前主表（EmployeeAttachmentId={childDto.EmployeeAttachmentId}）");
                    }
                    submittedIds.Add(childDto.EmployeeAttachmentId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeAttachmentId;
                    target.EmployeeId = entity.Id;
                    await _employeeAttachmentRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeAttachment>();
                    child.Id = 0;
                    child.EmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeAttachmentRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeAttachmentRepository.CreateRangeAsync(toCreate);
            }
        }
        // 员工代理关系（EmployeeDelegations）
        List<TaktEmployeeDelegationUpdateDto>? employeeDelegationsForSave;
        if (dto is TaktEmployeeUpdateDto updateDtoForEmployeeDelegations && updateDtoForEmployeeDelegations.EmployeeDelegations != null)
        {
            employeeDelegationsForSave = updateDtoForEmployeeDelegations.EmployeeDelegations;
        }
        else if (dto.EmployeeDelegations != null)
        {
            employeeDelegationsForSave = dto.EmployeeDelegations.Adapt<List<TaktEmployeeDelegationUpdateDto>>();
        }
        else
        {
            employeeDelegationsForSave = null;
        }
        if (employeeDelegationsForSave is not { Count: > 0 })
        {
            await _employeeDelegationRepository.DeleteAsync(x => x.OriginalEmployeeId == entity.Id);
        }
        else
        {
            var existingList = await _employeeDelegationRepository.GetListAsync(x => x.OriginalEmployeeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktEmployeeDelegation>();
            for (var i = 0; i < employeeDelegationsForSave.Count; i++)
            {
                var childDto = employeeDelegationsForSave[i];
                childDto.OriginalEmployeeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.OriginalEmployeeCode = entity.EmployeeCode;
                childDto.OriginalEmployeeName = entity.EmployeeName;
                if (childDto.EmployeeDelegationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.EmployeeDelegationId, out var target))
                    {
                        throw new TaktBusinessException("员工代理关系不存在（EmployeeDelegationId={childDto.EmployeeDelegationId}）");
                    }
                    if (target.OriginalEmployeeId != entity.Id)
                    {
                        throw new TaktBusinessException("员工代理关系不属于当前主表（EmployeeDelegationId={childDto.EmployeeDelegationId}）");
                    }
                    submittedIds.Add(childDto.EmployeeDelegationId);
                    childDto.Adapt(target);
                    target.Id = childDto.EmployeeDelegationId;
                    target.OriginalEmployeeId = entity.Id;
                    await _employeeDelegationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktEmployeeDelegation>();
                    child.Id = 0;
                    child.OriginalEmployeeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _employeeDelegationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _employeeDelegationRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployee, bool>> QueryExpression(TaktEmployeeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployee>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.IdCardCode != null && x.IdCardCode.Contains(keywords))
                || (x.Mobile != null && x.Mobile.Contains(keywords))
                || (x.Email != null && x.Email.Contains(keywords))
                || (x.NativePlace != null && x.NativePlace.Contains(keywords))
                || (x.Avatar != null && x.Avatar.Contains(keywords))
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

        if (queryDto?.Gender.HasValue == true)
        {
            var gender = queryDto.Gender.Value;
            exp = exp.And(x => x.Gender == gender);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IdCardCode))
        {
            var idCardCode = queryDto.IdCardCode;
            exp = exp.And(x => x.IdCardCode != null && x.IdCardCode.Contains(idCardCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Mobile))
        {
            var mobile = queryDto.Mobile;
            exp = exp.And(x => x.Mobile != null && x.Mobile.Contains(mobile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Email))
        {
            var email = queryDto.Email;
            exp = exp.And(x => x.Email != null && x.Email.Contains(email));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NativePlace))
        {
            var nativePlace = queryDto.NativePlace;
            exp = exp.And(x => x.NativePlace != null && x.NativePlace.Contains(nativePlace));
        }

        if (queryDto?.Ethnicity.HasValue == true)
        {
            var ethnicity = queryDto.Ethnicity.Value;
            exp = exp.And(x => x.Ethnicity == ethnicity);
        }

        if (queryDto?.PoliticalAffiliation.HasValue == true)
        {
            var politicalAffiliation = queryDto.PoliticalAffiliation.Value;
            exp = exp.And(x => x.PoliticalAffiliation == politicalAffiliation);
        }

        if (queryDto?.MaritalStatus.HasValue == true)
        {
            var maritalStatus = queryDto.MaritalStatus.Value;
            exp = exp.And(x => x.MaritalStatus == maritalStatus);
        }

        if (queryDto?.EmployeeStatus.HasValue == true)
        {
            var employeeStatus = queryDto.EmployeeStatus.Value;
            exp = exp.And(x => x.EmployeeStatus == employeeStatus);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Avatar))
        {
            var avatar = queryDto.Avatar;
            exp = exp.And(x => x.Avatar != null && x.Avatar.Contains(avatar));
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

        if (queryDto?.BirthDateStart.HasValue == true)
        {
            var birthDateStart = queryDto.BirthDateStart.Value;
            exp = exp.And(x => x.BirthDate >= birthDateStart);
        }

        if (queryDto?.BirthDateEnd.HasValue == true)
        {
            var birthDateEnd = queryDto.BirthDateEnd.Value;
            exp = exp.And(x => x.BirthDate <= birthDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEmployeeQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (queryDto.Gender.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IdCardCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Mobile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Email))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NativePlace))
        {
            return true;
        }
        if (queryDto.Ethnicity.HasValue)
        {
            return true;
        }
        if (queryDto.PoliticalAffiliation.HasValue)
        {
            return true;
        }
        if (queryDto.MaritalStatus.HasValue)
        {
            return true;
        }
        if (queryDto.EmployeeStatus.HasValue)
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Avatar))
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
        if (queryDto.BirthDateStart.HasValue || queryDto.BirthDateEnd.HasValue)
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
