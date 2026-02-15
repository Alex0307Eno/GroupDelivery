// 檔案用途說明：商家開團表單
(function () {
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], errors: {}, stores: [], form: { storeId: '', targetAmount: '', deadline: '', deliveryRangeKm: '', remark: '' } }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.stores = (await apiClient.merchant.stores.list()).data || []; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            validate() { this.errors = {}; if (!this.form.storeId) this.errors.storeId = '請選擇店家'; if (Number(this.form.targetAmount) < 0) this.errors.targetAmount = '目標金額不可小於 0'; if (!this.form.deadline || new Date(this.form.deadline) <= new Date()) this.errors.deadline = '截止時間必須大於現在'; return Object.keys(this.errors).length === 0; },
            async submit() { if (!this.validate()) return; this.submitting = true; try { await apiClient.merchant.groups.create({ ...this.form, targetAmount: Number(this.form.targetAmount), deliveryRangeKm: Number(this.form.deliveryRangeKm || 0) }); this.pushToast('開團成功'); setTimeout(() => { window.location.href = '/Store/MyGroups'; }, 1200); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
