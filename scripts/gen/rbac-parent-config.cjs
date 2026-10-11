// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：rbac-parent-config.cjs
// 创建时间：2026-06-01
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 八表单一配置源（实体导航、DTO、服务委托；User 服务/DTO 手工）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');

const NAVIGATION_REGION_MARKER = '导航属性区域';

/** 关联表实体 → 命名空间（用于主实体文件 using） */
const RBAC_JUNCTION_NAMESPACE = {
  UserRole: 'Takt.Domain.Entities.Identity',
  UserTenant: 'Takt.Domain.Entities.Identity',
  UserCompany: 'Takt.Domain.Entities.Identity',
  RoleMenu: 'Takt.Domain.Entities.Identity',
  RoleCompany: 'Takt.Domain.Entities.Identity',
  RoleDept: 'Takt.Domain.Entities.HumanResource.Organization',
  EmployeeDept: 'Takt.Domain.Entities.HumanResource.Organization',
  EmployeePost: 'Takt.Domain.Entities.HumanResource.Organization',
};

/**
 * 主实体 RBAC OneToMany 导航（与八表、ITaktRbacService 一致）
 * assignFromParent：Create/Update DTO 写入 createIdsProp，服务侧 AssignXxxAsync(主实体.Id, …)
 * includeOnCreate：false 时仅响应 DTO List<关联Dto>，不写 Create 字段（如 Role.UserRoles）
 */
const RBAC_PARENT_NAVIGATIONS = {
  User: [
    {
      navProp: 'UserRoles',
      junction: 'UserRole',
      fkOnChild: 'UserId',
      table: 'takt_identity_user_role',
      summary: '用户角色关联',
      createIdsProp: 'RoleIds',
      createIdsType: 'long[]?',
      assignFromParent: true,
    },
    {
      navProp: 'UserTenants',
      junction: 'UserTenant',
      fkOnChild: 'UserId',
      table: 'takt_identity_user_tenant',
      summary: '用户可访问租户关联',
      createIdsProp: 'TenantCodes',
      createIdsType: 'string[]?',
      assignFromParent: true,
    },
    {
      navProp: 'UserCompanies',
      junction: 'UserCompany',
      fkOnChild: 'UserId',
      table: 'takt_identity_user_company',
      summary: '用户可访问公司关联',
      createIdsProp: 'CompanyCodes',
      createIdsType: 'string[]?',
      assignFromParent: true,
    },
  ],
  Tenant: [
    {
      navProp: 'UserTenants',
      junction: 'UserTenant',
      fkOnChild: 'TenantCode',
      table: 'takt_identity_user_tenant',
      summary: '可访问该租户的用户关联',
    },
  ],
  Role: [
    {
      navProp: 'RoleMenus',
      junction: 'RoleMenu',
      fkOnChild: 'RoleId',
      table: 'takt_identity_role_menu',
      summary: '角色菜单权限关联',
      createIdsProp: 'RoleMenuIds',
      createIdsType: 'long[]?',
      assignFromParent: true,
      getListMethod: 'GetRoleMenuIdsAsync',
      assignMethod: 'AssignRoleMenusAsync',
    },
    {
      navProp: 'RoleCompanies',
      junction: 'RoleCompany',
      fkOnChild: 'RoleId',
      table: 'takt_identity_role_company',
      summary: '角色可访问公司关联',
      createIdsProp: 'RoleCompanyCodes',
      createIdsType: 'string[]?',
      assignFromParent: true,
      getListMethod: 'GetRoleCompanyIdsAsync',
      assignMethod: 'AssignRoleCompaniesAsync',
    },
    {
      navProp: 'RoleDepts',
      junction: 'RoleDept',
      fkOnChild: 'RoleId',
      table: 'takt_human_resource_organization_roledept',
      summary: '自定义数据权限关联部门',
      createIdsProp: 'RoleDeptIds',
      createIdsType: 'long[]?',
      assignFromParent: true,
      getListMethod: 'GetRoleDeptIdsAsync',
      assignMethod: 'AssignRoleDeptsAsync',
    },
    {
      navProp: 'UserRoles',
      junction: 'UserRole',
      fkOnChild: 'RoleId',
      table: 'takt_identity_user_role',
      summary: '拥有该角色的用户关联',
      includeOnCreate: false,
    },
  ],
  Menu: [
    {
      navProp: 'RoleMenus',
      junction: 'RoleMenu',
      fkOnChild: 'MenuId',
      table: 'takt_identity_role_menu',
      summary: '拥有该菜单权限的角色关联',
    },
  ],
  Company: [
    {
      navProp: 'RoleCompanies',
      junction: 'RoleCompany',
      fkOnChild: 'CompanyCode',
      table: 'takt_identity_role_company',
      summary: '可访问该公司的角色关联',
    },
    {
      navProp: 'UserCompanies',
      junction: 'UserCompany',
      fkOnChild: 'CompanyCode',
      table: 'takt_identity_user_company',
      summary: '可访问该公司的用户关联',
    },
  ],
  Employee: [
    {
      navProp: 'EmployeeDepts',
      junction: 'EmployeeDept',
      fkOnChild: 'EmployeeId',
      table: 'takt_human_resource_organization_employeedept',
      summary: '员工部门关联',
      createIdsProp: 'EmployeeDeptIds',
      createIdsType: 'long[]?',
      assignFromParent: true,
      getListMethod: 'GetEmployeeDeptIdsAsync',
      assignMethod: 'AssignEmployeeDeptsAsync',
    },
    {
      navProp: 'EmployeePosts',
      junction: 'EmployeePost',
      fkOnChild: 'EmployeeId',
      table: 'takt_human_resource_organization_employeepost',
      summary: '员工岗位关联',
      createIdsProp: 'EmployeePostIds',
      createIdsType: 'long[]?',
      assignFromParent: true,
      getListMethod: 'GetEmployeePostIdsAsync',
      assignMethod: 'AssignEmployeePostsAsync',
    },
  ],
  Dept: [
    {
      navProp: 'RoleDepts',
      junction: 'RoleDept',
      fkOnChild: 'DeptId',
      table: 'takt_human_resource_organization_roledept',
      summary: '角色数据权限关联该部门',
    },
    {
      navProp: 'EmployeeDepts',
      junction: 'EmployeeDept',
      fkOnChild: 'DeptId',
      table: 'takt_human_resource_organization_employeedept',
      summary: '员工部门关联',
    },
  ],
  Post: [
    {
      navProp: 'EmployeePosts',
      junction: 'EmployeePost',
      fkOnChild: 'PostId',
      table: 'takt_human_resource_organization_employeepost',
      summary: '员工岗位关联',
    },
  ],
};

/** 主实体 Create/Update：反向合并到对端（无 assignFromParent 导航时由 DTO/服务写入） */
const RBAC_INVERSE_CREATE_FIELDS = {
  Tenant: [
    {
      prop: 'UserIds',
      type: 'long[]?',
      summary: '可访问该租户的用户 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）',
    },
  ],
  Menu: [
    {
      prop: 'RoleIds',
      type: 'long[]?',
      summary: '拥有该菜单权限的角色 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）',
    },
  ],
  Company: [
    {
      prop: 'RoleIds',
      type: 'long[]?',
      summary: '可访问该公司的角色 ID 列表（RBAC 反向合并）',
    },
    {
      prop: 'UserIds',
      type: 'long[]?',
      summary: '可访问该公司的用户 ID 列表（RBAC 反向合并）',
    },
  ],
  Dept: [
    {
      prop: 'RoleIds',
      type: 'long[]?',
      summary: '数据权限关联该部门的角色 ID 列表（RBAC 反向合并）',
    },
    {
      prop: 'EmployeeIds',
      type: 'long[]?',
      summary: '关联该部门的员工 ID 列表（RBAC 反向合并）',
    },
  ],
  Post: [
    {
      prop: 'EmployeeIds',
      type: 'long[]?',
      summary: '关联该岗位的员工 ID 列表（RBAC 反向合并）',
    },
  ],
};

/**
 * 服务层 direct / inverse（与 RBAC_PARENT_NAVIGATIONS、RBAC_INVERSE_CREATE_FIELDS 对齐）
 */
const RBAC_PARENT_CONFIG = {
  Role: {
    direct: [
      {
        assignIdsProp: 'RoleMenuIds',
        getListMethod: 'GetRoleMenuIdsAsync',
        assignMethod: 'AssignRoleMenusAsync',
        responseListProp: 'RoleMenus',
        assignClearArg: 'Array.Empty<long>()',
      },
      {
        assignIdsProp: 'RoleCompanyCodes',
        getListMethod: 'GetRoleCompanyIdsAsync',
        assignMethod: 'AssignRoleCompaniesAsync',
        responseListProp: 'RoleCompanies',
        assignClearArg: 'Array.Empty<string>()',
      },
      {
        assignIdsProp: 'RoleDeptIds',
        getListMethod: 'GetRoleDeptIdsAsync',
        assignMethod: 'AssignRoleDeptsAsync',
        responseListProp: 'RoleDepts',
        assignClearArg: 'Array.Empty<long>()',
      },
    ],
  },
  Employee: {
    direct: [
      {
        assignIdsProp: 'EmployeeDeptIds',
        getListMethod: 'GetEmployeeDeptIdsAsync',
        assignMethod: 'AssignEmployeeDeptsAsync',
        responseListProp: 'EmployeeDepts',
        assignClearArg: 'Array.Empty<long>()',
      },
      {
        assignIdsProp: 'EmployeePostIds',
        getListMethod: 'GetEmployeePostIdsAsync',
        assignMethod: 'AssignEmployeePostsAsync',
        responseListProp: 'EmployeePosts',
        assignClearArg: 'Array.Empty<long>()',
      },
    ],
  },
  Tenant: {
    inverse: [
      {
        kind: 'mergeUserTenants',
        peerIdsProp: 'UserIds',
        linkCodeProp: 'TenantCode',
        getListMethod: 'GetUserTenantIdsAsync',
        assignMethod: 'AssignUserTenantsAsync',
        codeSelector: 'x => x.TenantCode',
      },
    ],
  },
  Menu: {
    inverse: [
      {
        kind: 'mergeRoleMenus',
        peerIdsProp: 'RoleIds',
        linkIdExpr: 'entity.Id',
        getListMethod: 'GetRoleMenuIdsAsync',
        assignMethod: 'AssignRoleMenusAsync',
        linkIdSelector: 'x => x.MenuId',
      },
    ],
  },
  Company: {
    inverse: [
      {
        kind: 'mergeRoleCompanies',
        peerIdsProp: 'RoleIds',
        linkCodeProp: 'CompanyCode',
        getListMethod: 'GetRoleCompanyIdsAsync',
        assignMethod: 'AssignRoleCompaniesAsync',
        codeSelector: 'x => x.CompanyCode',
      },
      {
        kind: 'mergeUserCompanies',
        peerIdsProp: 'UserIds',
        linkCodeProp: 'CompanyCode',
        getListMethod: 'GetUserCompanyIdsAsync',
        assignMethod: 'AssignUserCompaniesAsync',
        codeSelector: 'x => x.CompanyCode',
      },
    ],
  },
  Dept: {
    inverse: [
      {
        kind: 'mergeRoleDepts',
        peerIdsProp: 'RoleIds',
        linkIdExpr: 'entity.Id',
        getListMethod: 'GetRoleDeptIdsAsync',
        assignMethod: 'AssignRoleDeptsAsync',
        linkIdSelector: 'x => x.DeptId',
      },
      {
        kind: 'mergeEmployeeDepts',
        peerIdsProp: 'EmployeeIds',
        linkIdExpr: 'entity.Id',
        getListMethod: 'GetEmployeeDeptIdsAsync',
        assignMethod: 'AssignEmployeeDeptsAsync',
        linkIdSelector: 'x => x.DeptId',
      },
    ],
  },
  Post: {
    inverse: [
      {
        kind: 'mergeEmployeePosts',
        peerIdsProp: 'EmployeeIds',
        linkIdExpr: 'entity.Id',
        getListMethod: 'GetEmployeePostIdsAsync',
        assignMethod: 'AssignEmployeePostsAsync',
        linkIdSelector: 'x => x.PostId',
      },
    ],
  },
};

function entityHasIsBuiltInLocal(entityFile) {
  if (!entityFile) {
    return false;
  }
  return fs.readFileSync(entityFile, 'utf8').includes('IsBuiltIn');
}

function getRbacParentNavigations(entityShort) {
  return RBAC_PARENT_NAVIGATIONS[entityShort] ?? [];
}

function hasRbacParentNavigations(entityShort) {
  return getRbacParentNavigations(entityShort).length > 0;
}

function hasRbacParentConfig(entityShort) {
  return Boolean(RBAC_PARENT_CONFIG[entityShort]);
}

function getRbacParentConfig(entityShort) {
  return RBAC_PARENT_CONFIG[entityShort] ?? null;
}

function getInverseCreateFields(entityShort) {
  return RBAC_INVERSE_CREATE_FIELDS[entityShort] ?? [];
}

/**
 * 根据实体导航项解析 Create/Update DTO 字段（与 ITaktRbacService 参数类型一致）
 * @param {string} entityShort
 * @param {object} nav parseNavigationProperties 项
 */
function resolveRbacCreateFieldFromNav(entityShort, nav) {
  const specs = getRbacParentNavigations(entityShort);
  const spec = specs.find((s) => s.navProp === nav.name);
  if (!spec || spec.includeOnCreate === false || !spec.assignFromParent) {
    return null;
  }
  return {
    prop: spec.createIdsProp,
    type: spec.createIdsType,
    summary: `${spec.summary}（RBAC 全量覆盖，分配走 ITaktRbacService）`,
  };
}

function appendInverseRbacCreateFields(lines, entityShort) {
  const fields = getInverseCreateFields(entityShort);
  fields.forEach((f) => {
    lines.push('    /// <summary>');
    lines.push(`    /// ${f.summary}`);
    lines.push('    /// </summary>');
    lines.push(`    public ${f.type} ${f.prop} { get; set; }`);
    lines.push('');
  });
}

function collectRbacJunctionUsings(entityNamespace, entityShort) {
  const usings = new Set();
  getRbacParentNavigations(entityShort).forEach((spec) => {
    const ns = RBAC_JUNCTION_NAMESPACE[spec.junction];
    if (ns && ns !== entityNamespace) {
      usings.add(ns);
    }
  });
  return [...usings].sort();
}

function genDirectAssignBlock(d, idExpr) {
  return [
    `        if (dto.${d.assignIdsProp} != null)`,
    '        {',
    `            await _rbacService.${d.assignMethod}(${idExpr}, dto.${d.assignIdsProp});`,
    '        }',
  ].join('\n');
}

/**
 * 规范 RBAC inverse 块中 Select 的 lambda（配置可写 x.Prop 或 x => x.Prop）
 * @param {string} selector
 * @returns {string}
 */
function toRbacSelectLambda(selector) {
  const s = (selector || '').trim();
  if (!s) {
    throw new Error('rbac-parent-config: codeSelector/linkIdSelector 不能为空');
  }
  if (s.includes('=>')) {
    return s;
  }
  const m = s.match(/^x\.(\w+)$/);
  if (m) {
    return `x => x.${m[1]}`;
  }
  throw new Error(
    `rbac-parent-config: 无效的 Select 表达式 "${selector}"，须为 x => x.Prop 或 x.Prop`,
  );
}

function genInverseAssignBlock(inv) {
  const codeSelector = inv.codeSelector ? toRbacSelectLambda(inv.codeSelector) : null;
  const linkIdSelector = inv.linkIdSelector ? toRbacSelectLambda(inv.linkIdSelector) : null;
  if (inv.kind === 'mergeUserTenants') {
    return [
      `        if (dto.${inv.peerIdsProp} != null)`,
      '        {',
      `            foreach (var userId in dto.${inv.peerIdsProp}.Distinct())`,
      '            {',
      `                var links = await _rbacService.${inv.getListMethod}(userId);`,
      `                var codes = links.Select(${codeSelector}).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();`,
      `                if (!codes.Contains(entity.${inv.linkCodeProp}))`,
      '                {',
      `                    codes.Add(entity.${inv.linkCodeProp});`,
      '                }',
      `                await _rbacService.${inv.assignMethod}(userId, codes.ToArray());`,
      '            }',
      '        }',
    ].join('\n');
  }
  if (inv.kind === 'mergeRoleMenus' || inv.kind === 'mergeRoleDepts') {
    return [
      `        if (dto.${inv.peerIdsProp} != null)`,
      '        {',
      `            foreach (var roleId in dto.${inv.peerIdsProp}.Distinct())`,
      '            {',
      `                var links = await _rbacService.${inv.getListMethod}(roleId);`,
      `                var ids = links.Select(${linkIdSelector}).Distinct().ToList();`,
      `                if (!ids.Contains(${inv.linkIdExpr}))`,
      '                {',
      `                    ids.Add(${inv.linkIdExpr});`,
      '                }',
      `                await _rbacService.${inv.assignMethod}(roleId, ids.ToArray());`,
      '            }',
      '        }',
    ].join('\n');
  }
  if (inv.kind === 'mergeRoleCompanies' || inv.kind === 'mergeUserCompanies') {
    const peerVar = inv.kind === 'mergeRoleCompanies' ? 'roleId' : 'userId';
    return [
      `        if (dto.${inv.peerIdsProp} != null)`,
      '        {',
      `            foreach (var ${peerVar} in dto.${inv.peerIdsProp}.Distinct())`,
      '            {',
      `                var links = await _rbacService.${inv.getListMethod}(${peerVar});`,
      `                var codes = links.Select(${codeSelector}).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();`,
      `                if (!codes.Contains(entity.${inv.linkCodeProp}))`,
      '                {',
      `                    codes.Add(entity.${inv.linkCodeProp});`,
      '                }',
      `                await _rbacService.${inv.assignMethod}(${peerVar}, codes.ToArray());`,
      '            }',
      '        }',
    ].join('\n');
  }
  if (inv.kind === 'mergeEmployeeDepts' || inv.kind === 'mergeEmployeePosts') {
    return [
      `        if (dto.${inv.peerIdsProp} != null)`,
      '        {',
      `            foreach (var employeeId in dto.${inv.peerIdsProp}.Distinct())`,
      '            {',
      `                var links = await _rbacService.${inv.getListMethod}(employeeId);`,
      `                var ids = links.Select(${linkIdSelector}).Distinct().ToList();`,
      `                if (!ids.Contains(${inv.linkIdExpr}))`,
      '                {',
      `                    ids.Add(${inv.linkIdExpr});`,
      '                }',
      `                await _rbacService.${inv.assignMethod}(employeeId, ids.ToArray());`,
      '            }',
      '        }',
    ].join('\n');
  }
  return '';
}

function generateRbacParentDelegationExtras(
  entityShort,
  dtoInfo,
  masterRepoField,
  entityFile,
  desc,
  buildBuiltInDeleteGuardLines,
) {
  const cfg = getRbacParentConfig(entityShort);
  if (!cfg) {
    return null;
  }
  const direct = cfg.direct ?? [];
  const inverse = cfg.inverse ?? [];
  if (!direct.length && !inverse.length) {
    return null;
  }

  const getByIdLines = ['        var dto = entity.Adapt<' + `${dtoInfo.base}>();`];
  for (const d of direct) {
    if (d.responseListProp) {
      getByIdLines.push(
        `        dto.${d.responseListProp} = await _rbacService.${d.getListMethod}(entity.Id);`,
      );
    }
  }
  getByIdLines.push('        return dto;');

  const createBlocks = [];
  direct.forEach((d) => createBlocks.push(genDirectAssignBlock(d, 'entity.Id')));
  inverse.forEach((inv) => createBlocks.push(genInverseAssignBlock(inv)));
  const createAfterSave = createBlocks.filter(Boolean).join('\n');

  const updateBlocks = [];
  direct.forEach((d) => updateBlocks.push(genDirectAssignBlock(d, 'id')));
  inverse.forEach((inv) => updateBlocks.push(genInverseAssignBlock(inv)));
  const updateAfterSave = updateBlocks.filter(Boolean).join('\n');

  const deletePrefixLines = [];
  deletePrefixLines.push(`        var entity = await ${masterRepoField}.GetByIdAsync(id);`);
  deletePrefixLines.push('        if (entity == null)');
  deletePrefixLines.push('        {');
  deletePrefixLines.push(`            throw new TaktBusinessException("${desc}不存在或已删除");`);
  deletePrefixLines.push('        }');
  if (entityHasIsBuiltInLocal(entityFile)) {
    deletePrefixLines.push(buildBuiltInDeleteGuardLines(desc).trimEnd());
  }
  for (const d of direct) {
    const clearArg = d.assignClearArg ?? 'Array.Empty<long>()';
    deletePrefixLines.push(`        await _rbacService.${d.assignMethod}(id, ${clearArg});`);
  }

  return {
    ctorFields: '    private readonly ITaktRbacService _rbacService;\n',
    ctorParams: '        ITaktRbacService rbacService,\n',
    ctorParamDocs: '    /// <param name="rbacService">RBAC 关联分配服务</param>\n',
    ctorAssigns: '        _rbacService = rbacService;\n',
    extraUsings: ['Takt.Application.Services.Identity'],
    getByIdReturn: getByIdLines.join('\n'),
    createAfterSave,
    updateAfterSave,
    deletePrefix: direct.length ? deletePrefixLines.join('\n') : null,
    skipDirectDelete: Boolean(direct.length),
  };
}

module.exports = {
  NAVIGATION_REGION_MARKER,
  RBAC_JUNCTION_NAMESPACE,
  RBAC_PARENT_NAVIGATIONS,
  RBAC_INVERSE_CREATE_FIELDS,
  RBAC_PARENT_CONFIG,
  getRbacParentNavigations,
  hasRbacParentNavigations,
  hasRbacParentConfig,
  getRbacParentConfig,
  getInverseCreateFields,
  resolveRbacCreateFieldFromNav,
  appendInverseRbacCreateFields,
  collectRbacJunctionUsings,
  generateRbacParentDelegationExtras,
};
