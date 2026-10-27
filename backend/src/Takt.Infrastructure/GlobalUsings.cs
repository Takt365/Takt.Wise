// ========================================
// 项目名称：TaktWise
// 命名空间：Takt.Infrastructure
// 文件名称：GlobalUsings.cs
// 创建时间：2024-01-15 10:00:00
// 创建人：Davis.Chte
// 功能描述：Takt.Infrastructure 项目全局 using 声明
// 
// 版权所有 (C) TaktWise. 保留所有权利。
// 本代码仅供内部使用，未经授权不得复制或分发。
// ========================================

// SqlSugar ORM
global using SqlSugar;

// Microsoft.Extensions
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

// ASP.NET Core
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;

// System 命名空间
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;

global using Takt.Shared.Constants;
global using Takt.Shared.Enums;
global using Takt.Shared.Exceptions;
global using Takt.Shared.Extensions;
global using Takt.Shared.Helpers;
global using Takt.Shared.Options;
global using Takt.Shared.Validation;

global using Takt.Domain.Interfaces;
global using Takt.Domain.Repositories;

global using Takt.Domain.Entities.Accounting;
global using Takt.Domain.Entities.Accounting.Controlling;
global using Takt.Domain.Entities.Accounting.Financial;
global using Takt.Domain.Entities.Code;
global using Takt.Domain.Entities.Code.Database;
global using Takt.Domain.Entities.Code.Generator;
global using Takt.Domain.Entities.Foundation;
global using Takt.Domain.Entities.HumanResource;
global using Takt.Domain.Entities.HumanResource.Attendance;
global using Takt.Domain.Entities.HumanResource.Benefits;
global using Takt.Domain.Entities.HumanResource.Compensation;
global using Takt.Domain.Entities.HumanResource.Organization;
global using Takt.Domain.Entities.HumanResource.Performance;
global using Takt.Domain.Entities.HumanResource.Personnel;
global using Takt.Domain.Entities.HumanResource.Talent;
global using Takt.Domain.Entities.HumanResource.Training;
global using Takt.Domain.Entities.Identity;
global using Takt.Domain.Entities.Logistics;
global using Takt.Domain.Entities.Logistics.CustomerService;
global using Takt.Domain.Entities.Logistics.Maintenance;
global using Takt.Domain.Entities.Logistics.Manufacturing;
global using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
global using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
global using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
global using Takt.Domain.Entities.Logistics.Manufacturing.Output;
global using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
global using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
global using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
global using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
global using Takt.Domain.Entities.Logistics.Materials;
global using Takt.Domain.Entities.Logistics.Procurement;
global using Takt.Domain.Entities.Logistics.Quality;
global using Takt.Domain.Entities.Logistics.Quality.Complaint;
global using Takt.Domain.Entities.Logistics.Quality.Cost;
global using Takt.Domain.Entities.Logistics.Quality.Operation;
global using Takt.Domain.Entities.Logistics.Sales;
global using Takt.Domain.Entities.Logistics.Serial;
global using Takt.Domain.Entities.Routine;
global using Takt.Domain.Entities.Routine.Announcement;
global using Takt.Domain.Entities.Routine.MeetingCenter;
global using Takt.Domain.Entities.Routine.DocumentCenter;
global using Takt.Domain.Entities.Routine.HelpDesk;
global using Takt.Domain.Entities.Routine.NewsCenter;
global using Takt.Domain.Entities.Routine.VisitorCenter;
global using Takt.Domain.Entities.Statistics;
global using Takt.Domain.Entities.Statistics.Logging;
global using Takt.Domain.Entities.Statistics.QuickQuery;
global using Takt.Domain.Entities.Workflow;

global using Takt.Application.Dtos.Accounting;
global using Takt.Application.Dtos.Accounting.Controlling;
global using Takt.Application.Dtos.Accounting.Financial;
global using Takt.Application.Dtos.Code;
global using Takt.Application.Dtos.Code.Database;
global using Takt.Application.Dtos.Code.Generator;
global using Takt.Application.Dtos.Foundation;
global using Takt.Application.Dtos.HumanResource;
global using Takt.Application.Dtos.HumanResource.Attendance;
global using Takt.Application.Dtos.HumanResource.Benefits;
global using Takt.Application.Dtos.HumanResource.Compensation;
global using Takt.Application.Dtos.HumanResource.Organization;
global using Takt.Application.Dtos.HumanResource.Performance;
global using Takt.Application.Dtos.HumanResource.Personnel;
global using Takt.Application.Dtos.HumanResource.Talent;
global using Takt.Application.Dtos.HumanResource.Training;
global using Takt.Application.Dtos.Identity;
global using Takt.Application.Dtos.Logistics;
global using Takt.Application.Dtos.Logistics.CustomerService;
global using Takt.Application.Dtos.Logistics.Maintenance;
global using Takt.Application.Dtos.Logistics.Manufacturing;
global using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
global using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
global using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
global using Takt.Application.Dtos.Logistics.Manufacturing.Output;
global using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
global using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
global using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
global using Takt.Application.Dtos.Logistics.Materials;
global using Takt.Application.Dtos.Logistics.Procurement;
global using Takt.Application.Dtos.Logistics.Quality;
global using Takt.Application.Dtos.Logistics.Quality.Complaint;
global using Takt.Application.Dtos.Logistics.Quality.Cost;
global using Takt.Application.Dtos.Logistics.Quality.Operation;
global using Takt.Application.Dtos.Logistics.Sales;
global using Takt.Application.Dtos.Logistics.Serial;
global using Takt.Application.Dtos.Routine;
global using Takt.Application.Dtos.Routine.Announcement;
global using Takt.Application.Dtos.Routine.MeetingCenter;
global using Takt.Application.Dtos.Routine.DocumentCenter;
global using Takt.Application.Dtos.Routine.HelpDesk;
global using Takt.Application.Dtos.Routine.NewsCenter;
global using Takt.Application.Dtos.Routine.VisitorCenter;
global using Takt.Application.Dtos.Statistics;
global using Takt.Application.Dtos.Statistics.Logging;
global using Takt.Application.Dtos.Statistics.QuickQuery;
global using Takt.Application.Dtos.Workflow;

global using Takt.Application.Services.Accounting;
global using Takt.Application.Services.Accounting.Controlling;
global using Takt.Application.Services.Accounting.Financial;
global using Takt.Application.Services.Code;
global using Takt.Application.Services.Code.Database;
global using Takt.Application.Services.Code.Generator;
global using Takt.Application.Services.Foundation;
global using Takt.Application.Services.HumanResource;
global using Takt.Application.Services.HumanResource.Attendance;
global using Takt.Application.Services.HumanResource.Benefits;
global using Takt.Application.Services.HumanResource.Compensation;
global using Takt.Application.Services.HumanResource.Organization;
global using Takt.Application.Services.HumanResource.Performance;
global using Takt.Application.Services.HumanResource.Personnel;
global using Takt.Application.Services.HumanResource.Talent;
global using Takt.Application.Services.HumanResource.Training;
global using Takt.Application.Services.Identity;
global using Takt.Application.Services.Logistics;
global using Takt.Application.Services.Logistics.CustomerService;
global using Takt.Application.Services.Logistics.Maintenance;
global using Takt.Application.Services.Logistics.Manufacturing;
global using Takt.Application.Services.Logistics.Manufacturing.Bom;
global using Takt.Application.Services.Logistics.Manufacturing.Defect;
global using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
global using Takt.Application.Services.Logistics.Manufacturing.Output;
global using Takt.Application.Services.Logistics.Manufacturing.Aps;
global using Takt.Application.Services.Logistics.Manufacturing.Mds;
global using Takt.Application.Services.Logistics.Manufacturing.Sop;
global using Takt.Application.Services.Logistics.Materials;
global using Takt.Application.Services.Logistics.Procurement;
global using Takt.Application.Services.Logistics.Quality;
global using Takt.Application.Services.Logistics.Quality.Complaint;
global using Takt.Application.Services.Logistics.Quality.Cost;
global using Takt.Application.Services.Logistics.Quality.Operation;
global using Takt.Application.Services.Logistics.Sales;
global using Takt.Application.Services.Logistics.Serial;
global using Takt.Application.Services.Routine;
global using Takt.Application.Services.Routine.Announcement;
global using Takt.Application.Services.Routine.MeetingCenter;
global using Takt.Application.Services.Routine.DocumentCenter;
global using Takt.Application.Services.Routine.HelpDesk;
global using Takt.Application.Services.Routine.NewsCenter;
global using Takt.Application.Services.Routine.VisitorCenter;
global using Takt.Application.Services.Statistics;
global using Takt.Application.Services.Statistics.Logging;
global using Takt.Application.Services.Statistics.QuickQuery;
global using Takt.Application.Services.Workflow;

global using Takt.Infrastructure.Extensions;
global using Takt.Infrastructure.Data.Context;