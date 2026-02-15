// 檔案用途說明：使用者我的訂單列表與取消操作
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], orders: [] }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try { this.orders = (await apiClient.orders.mine()).data || []; }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.loading = false; }
            },
            canCancel(order) { return !order.isClosed && !order.isSuccess; },
            async cancel(order) {
                this.submitting = true;
                try { await apiClient.orders.cancel(order.orderId); this.pushToast('已取消訂單'); await this.load(); }
                catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.submitting = false; }
            }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
