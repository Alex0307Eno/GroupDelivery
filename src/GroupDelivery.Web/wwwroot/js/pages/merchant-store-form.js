// 檔案用途說明：商家店家新增與編輯表單
(function () {
    const root = document.getElementById('app');
    const mode = root.dataset.mode;
    const storeId = root.dataset.storeId;
    const app = AppComponents.createPageApp({
        data() { return { loading: false, submitting: false, toasts: [], errors: {}, form: { storeName: root.dataset.storeName || '', phone: root.dataset.phone || '', address: root.dataset.address || '' }, placeResult: '' }; },
        methods: {
            ...AppComponents.baseMethods(),
            validate() {
                this.errors = {};
                if (!this.form.storeName) this.errors.storeName = '請輸入店名';
                if (!this.form.phone) this.errors.phone = '請輸入電話';
                if (!this.form.address) this.errors.address = '請輸入地址';
                return Object.keys(this.errors).length === 0;
            },
            async submit() {
                if (!this.validate()) return;
                this.submitting = true;
                try {
                    const payload = { ...this.form };
                    const res = mode === 'edit' ? await apiClient.merchant.stores.update(storeId, payload) : await apiClient.merchant.stores.create(payload);
                    this.pushToast('儲存成功');
                    this.placeResult = res.data && res.data.googlePlaceId ? `系統已搜尋 GooglePlaceId：${res.data.googlePlaceId}` : '系統將以 店名+地址 自動搜尋 GooglePlaceId';
                    setTimeout(() => { window.location.href = '/Store/MyStores'; }, 1200);
                } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.submitting = false; }
            }
        }
    });
    app.mount('#app');
})();
