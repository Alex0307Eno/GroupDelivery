// 檔案用途說明：管理端揪團監控頁
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], groups: [] }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.groups = (await apiClient.admin.groups()).data || []; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            async closeGroup(group) { this.submitting = true; try { await apiClient.admin.closeGroup(group.groupId); this.pushToast('已關閉揪團'); await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
