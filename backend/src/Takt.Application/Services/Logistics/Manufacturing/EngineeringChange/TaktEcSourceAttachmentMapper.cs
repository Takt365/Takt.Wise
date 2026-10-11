// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSourceAttachmentMapper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主表字段映射为设变附件创建 DTO（技联书/PP 番等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源主表 → 设变附件创建 DTO 映射
/// </summary>
public static class TaktEcSourceAttachmentMapper
{
    private const string PlaceholderAccessUrl = "-";

    /// <summary>
    /// 将来源设变主表上的文档编码映射为设变附件行（技联书→TL，PP番号→FPP）
    /// </summary>
    /// <param name="sourceEc">来源设变主</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <returns>附件创建 DTO 列表（无对应编码时为空列表）</returns>
    public static List<TaktEcAttachmentCreateDto> MapAttachments(
        TaktSourceEc sourceEc,
        string ecCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture)
    {
        ArgumentNullException.ThrowIfNull(sourceEc);
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var culture = companyDefaultCulture ?? string.Empty;
        var lineNumber = 0;
        var result = new List<TaktEcAttachmentCreateDto>(2);
        AppendIfPresent(
            result,
            ref lineNumber,
            sourceEc.SourceTechnicalNoticeCode,
            TaktEcAttachmentTypeConstants.Liaison,
            "技联书",
            ecCode,
            tenantCode,
            companyCode,
            culture);
        AppendIfPresent(
            result,
            ref lineNumber,
            sourceEc.SourcePpCode,
            TaktEcAttachmentTypeConstants.Fpp,
            "PP番号",
            ecCode,
            tenantCode,
            companyCode,
            culture);
        return result;
    }

    /// <summary>
    /// 文档编码非空时追加一行附件 DTO
    /// </summary>
    private static void AppendIfPresent(
        List<TaktEcAttachmentCreateDto> target,
        ref int lineNumber,
        string? docCode,
        string attachmentType,
        string defaultFileName,
        string ecCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture)
    {
        if (string.IsNullOrWhiteSpace(docCode))
        {
            return;
        }
        lineNumber += 10;
        var trimmedDocCode = docCode.Trim();
        target.Add(new TaktEcAttachmentCreateDto
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            CultureCode = companyDefaultCulture,
            EcCode = ecCode,
            LineNumber = lineNumber,
            AttachmentType = attachmentType,
            DocCode = trimmedDocCode,
            FileName = TaktEcAttachmentDocCodeHelper.BuildFileNameFromDocCode(trimmedDocCode, defaultFileName),
            AccessUrl = PlaceholderAccessUrl,
        });
    }
}
