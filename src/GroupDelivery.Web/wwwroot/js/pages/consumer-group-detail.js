// 檔案用途說明：使用者揪團詳情與購物車
(function () {
    const groupId = document.getElementById('app').dataset.groupId;
    const app = AppComponents.createPageApp({
        data() {
            return { loading: false, submitting: false, toasts: [], detail: null, cart: [], errors: {}, qtyMap: {} };
        },
        computed: {
            totalAmount() { return this.cart.reduce((sum, item) => sum + (item.price * item.qty), 0); }
        },
        methods: {
            ...AppComponents.baseMethods(),
            async load() {
                this.loading = true;
                try {
                    const res = await apiClient.groups.detail(groupId);
                    this.detail = res.data;
                } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.loading = false; }
            },
            addToCart(item) {
                const qty = Number(this.qtyMap[item.productId] || 1);
                const found = this.cart.find(x => x.productId === item.productId);
                if (found) found.qty += qty;
                else this.cart.push({ productId: item.productId, name: item.name, price: item.price, qty });
            },
            async submitOrder() {
                if (!this.cart.length) return;
                this.submitting = true;
                try {
                    await apiClient.orders.create({ groupId: Number(groupId), items: this.cart.map(x => ({ productId: x.productId, quantity: x.qty })) });
                    this.pushToast('下單成功');
                    this.cart = [];
                    await this.load();
                } catch (e) { this.pushToast(apiClient.extractError(e), 'danger'); }
                finally { this.submitting = false; }
            }
        },
        mounted() { this.load(); }
    });
    app.mount('#app');
})();
