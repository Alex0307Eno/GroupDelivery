// 檔案用途說明：商家方案與付款頁
(function () {
    const plans = [
        { code: 299, summary: '入門開團', weight: '基礎推團權重' },
        { code: 499, summary: '低權重推團 + 簡版數據', weight: '低推團權重' },
        { code: 699, summary: '較高推團權重 + 效率對比', weight: '中高推團權重' },
        { code: 999, summary: '系統建議成團金額/截止時間 + 週摘要 + 最高推團權重', weight: '最高推團權重', recommend: true }
    ];
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], plans, current: null }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.current = (await apiClient.merchant.subscription.get()).data; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            async upgrade(plan) { this.submitting = true; try { await apiClient.merchant.subscription.upsert({ planCode: plan.code }); this.pushToast('方案已更新'); await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
