// 檔案用途說明：商家菜單管理頁
(function () {
    const storeId = document.getElementById('app').dataset.storeId;
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], products: [], selected: [], errors: {}, form: { name: '', price: '', isActive: true } }; },
        methods: {
            ...AppComponents.baseMethods(),
            async load() { this.loading = true; try { this.products = (await apiClient.merchant.products.list(storeId)).data || []; } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.loading = false; } },
            validate() { this.errors = {}; if (!this.form.name) this.errors.name = '請輸入品名'; if (Number(this.form.price) < 0 || this.form.price === '') this.errors.price = '價格不可為負'; return Object.keys(this.errors).length === 0; },
            async save() { if (!this.validate()) return; this.submitting = true; try { await apiClient.merchant.products.create({ ...this.form, storeId: Number(storeId), price: Number(this.form.price) }); this.pushToast('新增成功'); this.form = { name: '', price: '', isActive: true }; await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } finally { this.submitting = false; } },
            async batch(active) { if (!this.selected.length) return; try { await apiClient.merchant.products.batch({ ids: this.selected, isActive: active }); this.pushToast('批次更新成功'); await this.load(); } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); } }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
