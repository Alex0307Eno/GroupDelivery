// 檔案用途說明：使用者首頁與附近揪團列表
(function () {
    const app = AppComponents.createPageApp({
        data() {
            return { loading: false, submitting: false, toasts: [], groups: [], filters: { distance: '', hot: false, store: '', deadline: '', minAmount: '', maxAmount: '' } };
        },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try {
                    const res = await apiClient.groups.list(this.filters);
                    this.groups = res.data || [];
                } catch (e) {
                    this.pushToast(apiClient.extractError(e), 'danger');
                } finally { this.loading = false; }
            },
            goDetail(id) { window.location.href = `/Group/GroupDetail/${id}`; }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
