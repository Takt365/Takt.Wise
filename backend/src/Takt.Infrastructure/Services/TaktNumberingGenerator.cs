// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktNumberingGenerator.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：ITaktNumberingGenerator 实现（按规则编码递增流水并写回编码规则表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

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
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编码规则仓储</param>
    /// <param name="userContext">用户上下文</param>
    public TaktNumberingGenerator(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktUserContext userContext)
    {
        _numberingRepository = numberingRepository;
        _userContext = userContext;
    }

    /// <summary>
    /// 生成下一个业务编码（递增流水并持久化）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果</returns>
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
        return await PersistNextCodeAsync(rule, cancellationToken);
    }

    /// <summary>
    /// 预览下一个业务编码（不占用流水号、不写库）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
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
        var now = DateTime.Now;
        var model = ToModel(rule);
        var sequence = TaktNumberingHelper.ComputePreviewSequence(model, now);
        return new TaktNumberingModel
        {
            BusinessCode = TaktNumberingHelper.FormatBusinessCode(model, sequence, now),
            CurrentSequence = sequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 尝试生成下一个业务编码；规则不存在或已禁用时返回 null
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果；不可用时为 null</returns>
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
        return await PersistNextCodeAsync(rule, cancellationToken);
    }

    /// <summary>
    /// 递增流水并写回编码规则
    /// </summary>
    /// <param name="rule">编码规则实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果</returns>
    private async Task<TaktNumberingModel> PersistNextCodeAsync(
        TaktNumbering rule,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTime.Now;
        var model = ToModel(rule);
        var (code, nextSequence) = TaktNumberingHelper.BuildNextBusinessCode(model, now);
        rule.CurrentSequence = nextSequence;
        rule.ExampleCode = code;
        rule.UpdatedAt = now;
        await _numberingRepository.UpdateAsync(rule);
        return new TaktNumberingModel
        {
            BusinessCode = code,
            CurrentSequence = nextSequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 按规则编码加载当前租户/公司下已启用的编码规则
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="throwIfMissing">不存在或禁用时是否抛异常</param>
    /// <returns>编码规则实体；不可用时可能为 null</returns>
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
    /// 实体转编码模型（供 TaktNumberingHelper 纯计算）
    /// </summary>
    /// <param name="entity">编码规则实体</param>
    /// <returns>编码模型</returns>
    private static TaktNumberingModel ToModel(TaktNumbering entity) => new()
    {
        RuleCode = entity.RuleCode,
        BusinessCode = entity.ExampleCode,
        CompanyCode = entity.CompanyCode,
        DeptCode = entity.DeptCode,
        PrefixCode = entity.PrefixCode,
        DateFormat = entity.DateFormat,
        SequenceLength = entity.SequenceLength,
        SequenceStep = entity.SequenceStep,
        SuffixCode = entity.SuffixCode,
        ResetPeriod = entity.ResetPeriod,
        CurrentSequence = entity.CurrentSequence,
        Separator = entity.Separator,
        Description = entity.NumberingDescription,
        DocumentType = entity.DocumentType,
        UpdatedAt = entity.UpdatedAt,
    };
}
