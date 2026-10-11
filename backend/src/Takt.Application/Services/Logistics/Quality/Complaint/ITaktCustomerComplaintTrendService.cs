// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：ITaktCustomerComplaintTrendService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉月度推移转置分析服务接口（与客诉 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 顾客投诉月度推移转置分析服务（读客诉主表；与 TaktCustomerComplaintService 分离）
/// </summary>
public interface ITaktCustomerComplaintTrendService
{
    /// <summary>
    /// 顾客投诉月度推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    Task<TaktCustomerComplaintMonthlyTrendResultDto> GetCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出顾客投诉月度推移转置分析
    /// </summary>
    /// <param name="query">查询 DTO</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
