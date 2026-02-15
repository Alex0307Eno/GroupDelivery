// 檔案用途說明：管理端儀表板
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, toasts: [], stats: {} }; },
        methods: { ...AppComponents.baseMethods(), async load() { this.loading = true; try { this.stats = (await apiClient.admin.dashboard()).data || {}; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } } },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
