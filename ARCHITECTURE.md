# GroupDelivery 系統架構說明

## 分層設計

### Controller 層
- 負責接收 HTTP Request、驗證基本輸入格式、回傳 HTTP Response。
- 不直接實作商業邏輯，只呼叫 Service。
- 需從 `Claim/JWT` 取得登入者識別，避免信任前端傳入敏感欄位。

### Service 層
- 負責商業規則與安全驗證。
- 驗證 ownership（例如團主才能關團、訂單建立者才能修改取餐方式）。
- 針對價格、金額等欄位以資料庫資料重算，避免前端竄改。
- 整合 Repository 查詢結果並產出對外 DTO。

### Repository 層
- 僅負責資料庫存取，不承載商業流程。
- 以 `DbContext` 執行查詢與寫入。
- 提供 Service 所需的聚合資料（例如菜單品項含客製化選項）。

## 主要資料流程

1. 前端呼叫 API 送出請求。
2. Controller 取得目前登入者 Claim，做基本 request 檢查後呼叫 Service。
3. Service 透過 Repository 讀取資料，執行商業驗證與安全檢查：
   - 使用者與資源 ownership
   - 團單狀態與截止時間
   - 價格與加價重新計算
4. Service 呼叫 Repository 寫入異動資料。
5. Controller 回傳結果給前端。

## 安全設計原則

- 使用者身分以伺服器端 Claim 為唯一來源，不信任 request 的 userId。
- 訂單金額、加價、總價由後端依資料庫重新計算。
- 修改類 API 必須做 ownership 驗證，避免越權操作。
- 優先使用 PublicId/Guid 作為對外識別，降低遞增主鍵暴露風險。
- Controller/Service/Repository 明確分工，減少邏輯散落造成的安全缺口。
