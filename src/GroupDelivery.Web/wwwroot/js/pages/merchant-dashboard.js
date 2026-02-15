// 檔案用途說明：商家儀表板數據與快捷入口
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, toasts: [], dashboard: {} }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try { this.dashboard = (await apiClient.merchant.dashboard()).data || {}; }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.loading = false; }
            }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
