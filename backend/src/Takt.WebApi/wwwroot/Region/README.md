# IP定位数据库文件说明

## 文件位置

- **IPv4数据库**: `ip2region_v4.xdb`
- **IPv6数据库**: `ip2region_v6.xdb`（可选）

## 数据格式

XDB格式是IP2Region的专用数据格式，具有以下特点：
- 支持亿级别的IP数据段管理
- 查询性能达到微秒级别（10微秒左右）
- 自动去重压缩，数据文件体积小
- 支持离线查询，无需网络连接

## 数据来源

### 官方数据源
- **官方网站**: https://ip2region.net/
- **GitHub仓库**: https://github.com/lionsoul2014/ip2region
- **Gitee镜像**: https://gitee.com/lionsoul/ip2region
- **C#绑定实现**: https://gitee.com/lionsoul/ip2region/tree/master/binding/csharp

### 下载方式

#### 方式1：从GitHub下载
```bash
# 克隆仓库
git clone https://github.com/lionsoul2014/ip2region.git

# 进入数据目录
cd ip2region/data

# 复制XDB文件到项目目录
# ip2region.xdb -> wwwroot/Region/ip2region_v4.xdb
```

#### 方式2：直接下载XDB文件
- 访问 https://github.com/lionsoul2014/ip2region/tree/master/data
- 下载 `ip2region.xdb` 文件
- 重命名为 `ip2region_v4.xdb` 并放置到 `wwwroot/Region/` 目录

#### 方式3：使用官方工具生成
如果需要自定义数据，可以使用官方提供的工具生成XDB文件：
- **Golang工具**: https://github.com/lionsoul2014/ip2region/tree/master/maker/golang
- **Java工具**: https://github.com/lionsoul2014/ip2region/tree/master/maker/java
- **C#工具**: https://github.com/lionsoul2014/ip2region/tree/master/maker/csharp
- **C#绑定**: https://gitee.com/lionsoul/ip2region/tree/master/binding/csharp

## 数据更新

### 更新步骤
1. 从官方仓库下载最新的XDB文件
2. 备份当前数据库文件
3. 替换 `wwwroot/Region/` 目录下的文件
4. 重启应用程序（会自动重新加载）

### 更新频率建议
- **生产环境**: 建议每月更新一次
- **开发环境**: 可根据需要随时更新

## 文件大小

- **IPv4数据库**: 约 10-15 MB
- **IPv6数据库**: 约 50-100 MB（如果使用）

## XDB 结构（官方稳定格式）

文件按指针从小到大四层：

| 层段 | 大小 | 说明 |
|------|------|------|
| Header | 256 Bytes | 版本、缓存策略、生成时间、索引起止、IP 版本、指针字节数 |
| Vector 索引 | 512 KiB | 256×256 二级索引，加速二分 |
| 地域信息 | 动态 | utf-8 原文去重存储 |
| 二分索引 | 动态 | IPv4 每项 14B / IPv6 每项 38B |

`Searcher.Search(ip)` 返回的是第三层「地域信息」字符串，**不是**自造字段。

### 地域信息标准格式（官方）

```text
国家|省份|城市|ISP
```

官方示例：

```text
0|0|内网IP|内网IP
澳大利亚|0|0|0
中国|福建省|福州市|电信
中国|上海|上海市|阿里云
```

新版社区数据可能在末尾多一段 ISO alpha-2（本仓库解析兼容）：

```text
中国|江苏省|南京市|0|CN
United States|California|0|Google LLC|US
```

本项目 `TaktLocationHelper.ParseLocationResult` 按 **`[0]国家 [1]省份 [2]城市 [3]ISP [4]ISO?`** 切分；❌ 禁止再按「国家|区域|省份|城市|ISP」错位。

## IP2Region.Net（NuGet 官方客户端）

包：`IP2Region.Net`（本仓库 `Takt.Shared` → `PackageReference`，当前 3.0.2）。Apache 2.0。

官方用法（与 [NuGet / 仓库 README](https://www.nuget.org/packages/IP2Region.Net) 一致）：

```csharp
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;

ISearcher searcher = new Searcher(CachePolicy.Content, "your xdb file path");
string? region = searcher.Search("58.251.27.201");
```

### CachePolicy（三档，均线程安全）

| 策略 | 说明 | 本仓库 |
|------|------|--------|
| **Content** | 整库载入内存，查询最快（Benchmark IPv4 ~54ns） | ✅ **采用** |
| VectorIndex | 仅缓存 vector，少一次固定 IO | 未用 |
| File | 完全按文件 IO 查询 | 未用 |

官方也提供 ASP.NET Core 扩展 `services.AddIP2RegionService(path, CachePolicy.Content)`（NET8+ keyed：`GetRequiredKeyedService<ISearcher>("IP2Region.Net")`）。  
本仓库**未**走该扩展，原因：需同时挂 IPv4/IPv6 两个 xdb、中间件/日志层从 Shared 静态入口调用、以及 `ResolveClientIp` / 内网占位 / 地域串切分等业务包装。底层仍是同一个 `new Searcher(CachePolicy.Content, path)`。

### 官方查询四步（`Searcher` 内部实现，本仓库不自研二分）

1. **IP → 大端字节数组**：IPv4 4 字节 / IPv6 16 字节
2. **Vector 索引**：用 `ip_bytes[0]`、`ip_bytes[1]` 定位 256×256 分区，读出该区二分索引 `sPtr`～`ePtr`（小端 u32）
3. **区内拆半查找**：IPv4 索引项 14B、IPv6 38B；命中后得到 `dataLen` + `dataPtr`  
   - 备注：IPv4 为兼容 2.0，**待查 IP 大端、索引内 sip/eip 小端**，比较在 binding 的 `IPv4Compare` 中处理
4. **读地域串**：从 `dataPtr` 读 `dataLen` 字节 → UTF-8 字符串（即上节「地域信息」）

本仓库调用链：

```text
InitializeTaktIpLocationDatabase()
  → TaktLocationHelper.Initialize(v4[, v6])
  → new Searcher(CachePolicy.Content, path)   // 官方 API
TaktLocationHelper.Search(ip)
  → searcher.Search(ip)                       // 上列四步
  → ParseLocationResult(原文)                 // 仅切分地域串
```

❌ 禁止在业务层重写 vector/二分；❌ 禁止改写官方 `Searcher` 比较字节序；❌ 禁止降级为自研 File IO。换库只替换 `wwwroot/Region/ip2region_v*.xdb` 后重启。

说明：开源免费 `ipv4_source` 与 ip138 等商用库数据源不同，同一 IP 归属可能不一致（例如 `45.199.182.3` 免费库现为 `中国|台湾省|0|0|CN`，ip138 可能显示香港）。高精度请用 [Ip2Region 社区商用离线库](https://ip2region.net/products/offline)。

## 内网 vs 公网

| IP 类型 | 行为 |
|--------|------|
| 回环 / RFC1918 私网（如 `192.168.*`、`10.*`） | **不依赖** xdb，直接返回「本地 / 内网」占位 |
| 公网 IPv4 / IPv6 | **必须**加载 `ip2region_v4.xdb`（IPv6 另需 `ip2region_v6.xdb`） |

若只放了说明文档、未放 `.xdb`，或运行目录找不到 `wwwroot/Region`，会出现「只能查内网、公网查不到」的现象。启动日志中应有 `[TaktLocationHelper] …加载离线库`；缺失时会有警告。

## 使用说明

### 初始化
数据库在应用启动时自动初始化（会依次尝试 WebRoot/Region、ContentRoot/wwwroot/Region、BaseDirectory）。无需手动操作。

### 查询示例
```csharp
// 查询IP地址位置
string ip = "114.114.114.114";
IpLocationResult? result = TaktLocationHelper.Search(ip);

if (result != null)
{
    Console.WriteLine($"位置: {result.FormattedAddress}");
}
```

详细使用说明请查看 `使用示例.md` 文件。

## 注意事项

1. **文件权限**: 确保应用程序有读取XDB文件的权限
2. **文件路径**: 文件必须放置在 `wwwroot/Region/` 目录下
3. **文件格式**: 必须使用XDB格式，不支持其他格式
4. **IPv6支持**: IPv6数据库是可选的，如果不存在，IPv6地址查询会返回null
5. **性能**: 使用Content缓存策略，首次加载会将整个数据库加载到内存，查询速度最快

## 技术支持

- **官方文档**: https://ip2region.net/
- **GitHub Issues**: https://github.com/lionsoul2014/ip2region/issues
- **Gitee Issues**: https://gitee.com/lionsoul/ip2region/issues
- **C#绑定文档**: https://gitee.com/lionsoul/ip2region/tree/master/binding/csharp
- **社区支持**: 访问官方社区获取帮助

## 许可证

IP2Region使用Apache 2.0许可证，可以自由使用和修改。
