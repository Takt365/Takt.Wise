// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktNumberingGenerator.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：ITaktNumberingGenerator 实现（取号只认业务表编码 max；公司段用当前登录公司 CompanyShortName；仅刷新 StartingNumber）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Infrastructure.Services;

/// <summary>
/// 业务编码生成器实现
/// </summary>
public sealed class TaktNumberingGenerator : ITaktNumberingGenerator
{
    private readonly ITaktCompanyRepository<TaktNumbering> _numberingRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编码规则仓储</param>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="userContext">用户上下文</param>
    public TaktNumberingGenerator(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext userContext)
    {
        _numberingRepository = numberingRepository;
        _companyRepository = companyRepository;
        _userContext = userContext;
    }

    /// <inheritdoc />
    public async Task<TaktNumberingModel> GenerateNextAsync(
        string ruleCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        var rule = await LoadActiveRuleByCodeAsync(ruleCode.Trim(), throwIfMissing: true);
        if (rule == null)
        {
            throw new TaktBusinessException($"编码规则「{ruleCode}」不存在");
        }
        return await PersistNextCodeAsync(rule, businessMaxSequence: 0, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TaktNumberingModel> GenerateNextAsync(
        string ruleCode,
        IReadOnlyList<string?> existingBusinessCodes,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        ArgumentNullException.ThrowIfNull(existingBusinessCodes);
        var rule = await LoadActiveRuleByCodeAsync(ruleCode.Trim(), throwIfMissing: true);
        if (rule == null)
        {
            throw new TaktBusinessException($"编码规则「{ruleCode}」不存在");
        }
        var now = DateTime.Now;
        var model = await ToModelAsync(rule, cancellationToken);
        var businessMax = TaktNumberingHelper.ResolveMaxSequenceFromBusinessCodes(
            existingBusinessCodes,
            model.SequenceLength,
            model.Separator,
            model.DateFormat,
            now);
        return await PersistNextCodeAsync(rule, businessMax, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TaktNumberingModel> PreviewNextAsync(
        string ruleCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        var rule = await LoadActiveRuleByCodeAsync(ruleCode.Trim(), throwIfMissing: true);
        if (rule == null)
        {
            throw new TaktBusinessException($"编码规则「{ruleCode}」不存在");
        }
        return await BuildPreviewAsync(rule, businessMaxSequence: 0, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TaktNumberingModel> PreviewNextAsync(
        string ruleCode,
        IReadOnlyList<string?> existingBusinessCodes,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ruleCode);
        ArgumentNullException.ThrowIfNull(existingBusinessCodes);
        var rule = await LoadActiveRuleByCodeAsync(ruleCode.Trim(), throwIfMissing: true);
        if (rule == null)
        {
            throw new TaktBusinessException($"编码规则「{ruleCode}」不存在");
        }
        var now = DateTime.Now;
        var model = await ToModelAsync(rule, cancellationToken);
        var businessMax = TaktNumberingHelper.ResolveMaxSequenceFromBusinessCodes(
            existingBusinessCodes,
            model.SequenceLength,
            model.Separator,
            model.DateFormat,
            now);
        return await BuildPreviewAsync(rule, businessMax, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TaktNumberingModel?> TryGenerateNextAsync(
        string ruleCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ruleCode))
        {
            return null;
        }
        var rule = await LoadActiveRuleByCodeAsync(ruleCode.Trim(), throwIfMissing: false);
        if (rule == null)
        {
            return null;
        }
        return await PersistNextCodeAsync(rule, businessMaxSequence: 0, cancellationToken);
    }

    /// <summary>
    /// 组装预览结果（不写库）
    /// </summary>
    /// <param name="rule">编码规则</param>
    /// <param name="businessMaxSequence">业务表 max</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览模型</returns>
    private async Task<TaktNumberingModel> BuildPreviewAsync(
        TaktNumbering rule,
        int businessMaxSequence,
        CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var model = await ToModelAsync(rule, cancellationToken);
        var sequence = TaktNumberingHelper.ComputePreviewSequence(model, now, businessMaxSequence);
        return new TaktNumberingModel
        {
            BusinessCode = TaktNumberingHelper.FormatBusinessCode(model, sequence, now),
            IssuedSequence = sequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 取号并写回规则 StartingNumber（最近发出码；流水只认业务表 max；公司段用简称）
    /// </summary>
    /// <param name="rule">编码规则实体</param>
    /// <param name="businessMaxSequence">业务表 max；无数据传 0</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果</returns>
    private async Task<TaktNumberingModel> PersistNextCodeAsync(
        TaktNumbering rule,
        int businessMaxSequence,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTime.Now;
        var model = await ToModelAsync(rule, cancellationToken);
        var (code, issuedSequence) = TaktNumberingHelper.BuildNextBusinessCode(model, now, businessMaxSequence);
        rule.StartingNumber = code;
        rule.UpdatedAt = now;
        await _numberingRepository.UpdateAsync(rule);
        return new TaktNumberingModel
        {
            BusinessCode = code,
            IssuedSequence = issuedSequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 按规则编码加载当前租户/公司下已启用的编码规则
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="throwIfMissing">缺失或禁用时是否抛业务异常</param>
    /// <returns>规则实体；不抛时可为 null</returns>
    private async Task<TaktNumbering?> LoadActiveRuleByCodeAsync(string ruleCode, bool throwIfMissing)
    {
        var tenantCode = _userContext.TenantCode;
        var companyCode = _userContext.CompanyCode;
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
        {
            if (throwIfMissing)
            {
                throw new TaktBusinessException("租户或公司上下文无效");
            }
            return null;
        }
        var list = await _numberingRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.RuleCode == ruleCode);
        var rule = list.FirstOrDefault();
        if (rule == null)
        {
            if (throwIfMissing)
            {
                throw new TaktBusinessException($"编码规则「{ruleCode}」不存在");
            }
            return null;
        }
        if (rule.NumberingStatus != 1)
        {
            if (throwIfMissing)
            {
                throw new TaktBusinessException($"编码规则「{ruleCode}」已禁用");
            }
            return null;
        }
        return rule;
    }

    /// <summary>
    /// 实体转编码模型（公司段填入当前登录公司简称）
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>编码模型</returns>
    private async Task<TaktNumberingModel> ToModelAsync(TaktNumbering entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var companyShortName = await ResolveCurrentCompanyShortNameAsync(cancellationToken);
        return new TaktNumberingModel
        {
            RuleCode = entity.RuleCode,
            BusinessCode = entity.StartingNumber,
            CompanyCode = entity.CompanyCode,
            CompanyShortName = companyShortName,
            DeptCode = entity.DeptCode,
            PrefixCode = entity.PrefixCode,
            DateFormat = entity.DateFormat,
            SequenceLength = entity.SequenceLength,
            SequenceStep = entity.SequenceStep,
            SuffixCode = entity.SuffixCode,
            ResetPeriod = entity.ResetPeriod,
            Separator = entity.Separator,
            Description = entity.NumberingDescription,
            DocumentType = entity.DocumentType,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    /// <summary>
    /// 解析当前登录公司简称（无简称时回退公司代码）
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公司简称或公司代码</returns>
    private async Task<string> ResolveCurrentCompanyShortNameAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var tenantCode = _userContext.TenantCode?.Trim();
        var companyCode = _userContext.CompanyCode?.Trim();
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
        {
            return companyCode ?? string.Empty;
        }
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == tenantCode && x.CompanyCode == companyCode);
        var company = companies.FirstOrDefault();
        if (company == null)
        {
            return companyCode;
        }
        return string.IsNullOrWhiteSpace(company.CompanyShortName)
            ? companyCode
            : company.CompanyShortName.Trim();
    }
}
