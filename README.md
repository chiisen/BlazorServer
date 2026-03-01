# 🚀 BlazorServer

🔥 Blazor 是一個使用 .NET 來建置 Web UI 的架構

📝 安裝了 NLog

🗄️ 安裝了 System.Data.SQLite 與新增一個 SQLite 資料庫

⚡ 安裝了 Dapper

🐬 安裝了 MySQL

⏱️ 安裝了 MiniProfiler

📚 安裝了 NSwag

## 🆕 更新紀錄

- 🚀 **升級 .NET 10**: 將所有專案 (`Client`, `Server`, `Shared`) 的 TargetFramework 升級至 `.NET 10.0`。
- 🔄 **Git 多重遠端同步**: 新增 `setup_git_sync.ps1` 腳本，方便一次推送到多個遠端儲存庫。
- 🧪 **單元測試與涵蓋率**: 建立 `BlazorServer.Tests` xUnit 測試專案，導入 `bUnit` 框架，並設定跨平台程式碼涵蓋率測試 (`coverlet.collector`)。
