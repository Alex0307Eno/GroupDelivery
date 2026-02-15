// 檔案用途說明：商家揪團管理與訂單檢視
(function () {
    const groupId = document.getElementById('app').dataset.groupId;
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], detail: null }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.detail = (await apiClient.merchant.groups.manage(groupId)).data; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            async closeGroup() { this.submitting = true; try { await apiClient.merchant.groups.close(groupId); this.pushToast('已手動關團'); await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } },
            exportOrders() { window.location.href = `/api/merchant/groups/${groupId}/export`; }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
