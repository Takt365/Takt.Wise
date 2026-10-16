## [unreleased]

### Bug Fixes

- *(backend)* 内存与导出溢出防护，Options 三层配置绑定 — 2026-06-06

### Documentation

- *(readme)* 按当前仓库规模与模块更新四语 README — 2026-08-28
- *(cursor)* 更新主子表 Skill 对齐 LR/TB 布局与 panel 模式 — 2026-06-22
- *(cursor)* 完善 generate-all 流水线规则并禁止 PowerShell 查找替换 — 2026-06-18

### Features

- *(logistics)* 采购信息与销售条件更名，并补齐税码税条件与工程变更 EMTL — 2026-10-16
- *(platform)* 工程变更 SMT/执行与各域 Stat 栈，同步代码生成与 CRUD 前后端 — 2026-09-09
- *(controlling)* 成本中心类型改为 F/G/H/L/S 并同步字典与登录展示 — 2026-08-28
- *(platform)* Vite 产物按领域分目录，Trend/Stat/Explosion 独立栈与前端构建修复 — 2026-08-28
- *(platform)* 统一 UserName 口径、消息附件与 OpenIddict claim — 2026-08-24
- *(platform)* 统一冗余联动字段口径并清理 SAP 注释 — 2026-08-21
- *(bom)* PCB SECT 整树标识、成本口径与 Quartz 立即执行参数弹窗 — 2026-08-19
- *(platform)* 接入可观测性并统一 CultureCode/Plant 与字段长度规范 — 2026-08-12
- *(logistics)* Rename customer service to TaktCustomerService* and align menus — 2026-07-24
- *(platform)* 全量对齐行政区划/银行/员工地址与质量推移等全栈能力 — 2026-07-23
- *(foundation)* 行政区划改由 Sap_Data 同步（sap_sync_ad） — 2026-07-23
- *(platform)* 扩展序列号汇总/上传，重构制造计划域并精简价格阶梯 — 2026-07-20
- *(platform)* 全栈 DTO/权限对齐，移除 ChangeLog 模块并扩展 ECN/计划/工时 — 2026-07-09
- *(platform)* 全栈实体字段对齐、ECN 技术变更扩展与 RelatedPlant 统一 — 2026-07-02
- *(backend)* 统一数据库类型映射与仓储聚合统计 — 2026-06-22
- *(platform)* 精简人才/入职/库位模块，ECN 主子表扩展与菜单审计脚本 — 2026-06-22
- *(platform)* 全栈 DTO 对齐、ECN 部门视图与维护模块扩展 — 2026-06-22
- *(platform)* 工作流与 Quartz SignalR 实时推送及全栈模块扩展 — 2026-06-12
- *(humanresource)* 人事调动/入职模块重命名并同步全栈 — 2026-06-07
- *(logistics)* 品质成本重命名、菜单修正与物流模块扩展 — 2026-06-06
- *(humanresource)* 扩展绩效培训薪酬模块并修复部门更新 DTO — 2026-06-06
- *(seeds)* 扩展树表/主子表通用按钮与统计模块按钮种子 — 2026-06-05
- 初始提交 Takt.Plat 全栈代码库 — 2026-06-05

### Miscellaneous

- *(changelog)* 恢复孤立分支 changelog — 2026-10-11
- *(changelog)* 使 changelog 分支与 master 共祖，修复 GitHub 文件页无法打开 — 2026-10-11
- *(changelog)* 恢复孤立分支 changelog — 2026-10-11
- *(changelog)* 将 CHANGELOG 提交回当前分支 — 2026-10-11
- *(scripts)* 移除误提交的临时恢复脚本 — 2026-07-23
- *(backend)* 忽略临时构建目录 backend/_build_out — 2026-06-12
- *(scripts)* 清理一次性脚本并扩展代码生成流水线 — 2026-06-08
- 全栈统一产品命名为 Takt Plat — 2026-06-06
- 扩展 Cursor 规则 Skill 并强化前后端 utils 溢出边界 — 2026-06-06
- 同步 C# 工具类规范、changelog 提交日期与 WebApi 启动配置 — 2026-06-06
- 合并远程初始提交并保留本地完整 README/LICENSE — 2026-06-05

### Other

- Initial commit — 2026-05-31

### Refactor

- *(brand)* 产品名统一为节拍智控·TaktWise — 2026-10-11
- *(ecn)* Distinction 改为 Scope，清理临时脚本并同步前后端 — 2026-09-10
- *(webapi)* 扩展迁移 Infrastructure 并重命名 OpenIddict/权限过滤器 — 2026-06-06
