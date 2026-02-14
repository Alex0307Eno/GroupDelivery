// 檔案用途說明：使用者個人資料與角色切換資訊
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, toasts: [], profile: null }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try { this.profile = (await apiClient.me.profile()).data; }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.loading = false; }
            }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
