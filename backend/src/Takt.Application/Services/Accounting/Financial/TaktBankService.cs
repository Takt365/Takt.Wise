// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktBankService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：银行信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 银行信息应用服务
/// </summary>
public class TaktBankService : TaktServiceBase, ITaktBankService
{
    private readonly ITaktCompanyRepository<TaktBank> _bankRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bankRepository">银行信息仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBankService(
        ITaktCompanyRepository<TaktBank> bankRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bankRepository = bankRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取银行信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBankDto>> GetBankListAsync(TaktBankQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBankDto>.Create(
                new List<TaktBankDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _bankRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBankDto>.Create(
            data.Adapt<List<TaktBankDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto?> GetBankByIdAsync(long id)
    {
        var entity = await _bankRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBankDto>();
    }

    /// <summary>
    /// 获取银行信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBankOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _bankRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.BankCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BankCode,
            DictLabel = e.BankCode,
        }).ToList();
    }

    /// <summary>
    /// 创建银行信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto> CreateBankAsync(TaktBankCreateDto dto)
    {
        var entity = dto.Adapt<TaktBank>();
        var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
            _bankRepository,
            x => x.CountryRegion == entity.CountryRegion
                && x.BankCode == entity.BankCode);
        if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
        {
            throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
        }
        entity = await _bankRepository.CreateAsync(entity);
        return await GetBankByIdAsync(entity.Id) ?? entity.Adapt<TaktBankDto>();
    }

    /// <summary>
    /// 更新银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto> UpdateBankAsync(long id, TaktBankUpdateDto dto)
    {
        var entity = await _bankRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("银行信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
            _bankRepository,
            x => x.CountryRegion == entity.CountryRegion
                && x.BankCode == entity.BankCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
        {
            throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
        }
        await _bankRepository.UpdateAsync(entity);
        return await GetBankByIdAsync(id) ?? throw new TaktBusinessException("银行信息不存在");
    }

    /// <summary>
    /// 删除银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBankByIdAsync(long id)
    {
        var deleted = await _bankRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("银行信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除银行信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBankBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBankByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBankTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBankTemplateDto>(
            sheetName ?? "银行信息导入模板",
            fileName ?? "银行信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入银行信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBankAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBankImportDto>(fileStream, sheetName ?? "银行信息导入模板");
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
                var entity = rows[i].Adapt<TaktBank>();
                var importKey = $"{entity.CountryRegion}|{entity.BankCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountryRegion、BankCode）");
                }
                var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _bankRepository,
                    x => x.CountryRegion == entity.CountryRegion
                        && x.BankCode == entity.BankCode);
                if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
                {
                    throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
                }
                await _bankRepository.CreateAsync(entity);
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
    /// 导出银行信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBankAsync(TaktBankQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBankQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBankExportDto>(),
                sheetName ?? "银行信息数据",
                fileName ?? "银行信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _bankRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBankExportDto>(),
                sheetName ?? "银行信息数据",
                fileName ?? "银行信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBankExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "银行信息数据",
            fileName ?? "银行信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建银行信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBank, bool>> QueryExpression(TaktBankQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBank>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CountryRegion != null && x.CountryRegion.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankName1 != null && x.BankName1.Contains(keywords))
                || (x.BankName2 != null && x.BankName2.Contains(keywords))
                || (x.Province != null && x.Province.Contains(keywords))
                || (x.Prefecture != null && x.Prefecture.Contains(keywords))
                || (x.District != null && x.District.Contains(keywords))
                || (x.Township != null && x.Township.Contains(keywords))
                || (x.Village != null && x.Village.Contains(keywords))
                || (x.Address1 != null && x.Address1.Contains(keywords))
                || (x.Address2 != null && x.Address2.Contains(keywords))
                || (x.SwiftBic != null && x.SwiftBic.Contains(keywords))
                || (x.BankGroup != null && x.BankGroup.Contains(keywords))
                || (x.BankNumber != null && x.BankNumber.Contains(keywords))
                || (x.PostalBank != null && x.PostalBank.Contains(keywords))
                || (x.AddressNumber != null && x.AddressNumber.Contains(keywords))
                || (x.Branch != null && x.Branch.Contains(keywords))
                || (x.BankMethod != null && x.BankMethod.Contains(keywords))
                || (x.BankFormat != null && x.BankFormat.Contains(keywords))
                || (x.IbanRule != null && x.IbanRule.Contains(keywords))
                || (x.BicPlusNumber != null && x.BicPlusNumber.Contains(keywords))
                || (x.PathCode != null && x.PathCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryRegion))
        {
            var countryRegion = queryDto.CountryRegion;
            exp = exp.And(x => x.CountryRegion != null && x.CountryRegion.Contains(countryRegion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankCode))
        {
            var bankCode = queryDto.BankCode;
            exp = exp.And(x => x.BankCode != null && x.BankCode.Contains(bankCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankName1))
        {
            var bankName1 = queryDto.BankName1;
            exp = exp.And(x => x.BankName1 != null && x.BankName1.Contains(bankName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankName2))
        {
            var bankName2 = queryDto.BankName2;
            exp = exp.And(x => x.BankName2 != null && x.BankName2.Contains(bankName2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Province))
        {
            var province = queryDto.Province;
            exp = exp.And(x => x.Province != null && x.Province.Contains(province));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Prefecture))
        {
            var prefecture = queryDto.Prefecture;
            exp = exp.And(x => x.Prefecture != null && x.Prefecture.Contains(prefecture));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.District))
        {
            var district = queryDto.District;
            exp = exp.And(x => x.District != null && x.District.Contains(district));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Township))
        {
            var township = queryDto.Township;
            exp = exp.And(x => x.Township != null && x.Township.Contains(township));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Village))
        {
            var village = queryDto.Village;
            exp = exp.And(x => x.Village != null && x.Village.Contains(village));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SwiftBic))
        {
            var swiftBic = queryDto.SwiftBic;
            exp = exp.And(x => x.SwiftBic != null && x.SwiftBic.Contains(swiftBic));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankGroup))
        {
            var bankGroup = queryDto.BankGroup;
            exp = exp.And(x => x.BankGroup != null && x.BankGroup.Contains(bankGroup));
        }

        if (queryDto?.PobkCurAc.HasValue == true)
        {
            var pobkCurAc = queryDto.PobkCurAc.Value;
            exp = exp.And(x => x.PobkCurAc == pobkCurAc);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankNumber))
        {
            var bankNumber = queryDto.BankNumber;
            exp = exp.And(x => x.BankNumber != null && x.BankNumber.Contains(bankNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostalBank))
        {
            var postalBank = queryDto.PostalBank;
            exp = exp.And(x => x.PostalBank != null && x.PostalBank.Contains(postalBank));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AddressNumber))
        {
            var addressNumber = queryDto.AddressNumber;
            exp = exp.And(x => x.AddressNumber != null && x.AddressNumber.Contains(addressNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Branch))
        {
            var branch = queryDto.Branch;
            exp = exp.And(x => x.Branch != null && x.Branch.Contains(branch));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankMethod))
        {
            var bankMethod = queryDto.BankMethod;
            exp = exp.And(x => x.BankMethod != null && x.BankMethod.Contains(bankMethod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankFormat))
        {
            var bankFormat = queryDto.BankFormat;
            exp = exp.And(x => x.BankFormat != null && x.BankFormat.Contains(bankFormat));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IbanRule))
        {
            var ibanRule = queryDto.IbanRule;
            exp = exp.And(x => x.IbanRule != null && x.IbanRule.Contains(ibanRule));
        }

        if (queryDto?.SddB2b.HasValue == true)
        {
            var sddB2b = queryDto.SddB2b.Value;
            exp = exp.And(x => x.SddB2b == sddB2b);
        }

        if (queryDto?.SddCore.HasValue == true)
        {
            var sddCore = queryDto.SddCore.Value;
            exp = exp.And(x => x.SddCore == sddCore);
        }

        if (queryDto?.SddRtrans.HasValue == true)
        {
            var sddRtrans = queryDto.SddRtrans.Value;
            exp = exp.And(x => x.SddRtrans == sddRtrans);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BicPlusNumber))
        {
            var bicPlusNumber = queryDto.BicPlusNumber;
            exp = exp.And(x => x.BicPlusNumber != null && x.BicPlusNumber.Contains(bicPlusNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PathCode))
        {
            var pathCode = queryDto.PathCode;
            exp = exp.And(x => x.PathCode != null && x.PathCode.Contains(pathCode));
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
    private static bool HasAnyListQueryFilter(TaktBankQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CountryRegion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Province))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Prefecture))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.District))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Township))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Village))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SwiftBic))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankGroup))
        {
            return true;
        }
        if (queryDto.PobkCurAc.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostalBank))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AddressNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Branch))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankMethod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankFormat))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IbanRule))
        {
            return true;
        }
        if (queryDto.SddB2b.HasValue)
        {
            return true;
        }
        if (queryDto.SddCore.HasValue)
        {
            return true;
        }
        if (queryDto.SddRtrans.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BicPlusNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PathCode))
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
