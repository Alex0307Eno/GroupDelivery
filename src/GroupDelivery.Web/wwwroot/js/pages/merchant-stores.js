// 檔案用途說明：商家店家列表管理
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, toasts: [], stores: [] }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try { this.stores = (await apiClient.merchant.stores.list()).data || []; }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.loading = false; }
            },
            async toggle(store) {
                try { await apiClient.merchant.stores.toggle(store.storeId, { isActive: !store.isActive }); this.pushToast('狀態已更新'); await this.load(); }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
            }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
