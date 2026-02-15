// 檔案用途說明：管理端商家管理列表
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], stores: [], filters: { keyword: '', status: '', plan: '' } }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.stores = (await apiClient.admin.stores(this.filters)).data || []; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            async suspend(store) { this.submitting = true; try { await apiClient.admin.suspendStore(store.storeId, { suspended: !store.suspended }); this.pushToast('狀態已更新'); await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
