// 檔案用途說明：集中註冊共用 Vue 元件與頁面可重用工具
(function (window) {
    function formatMoney(value) {
        const num = Number(value || 0);
        if (Number.isNaN(num)) return '$0';
        return `$${num.toLocaleString('zh-TW', { minimumFractionDigits: 0, maximumFractionDigits: 2 })}`;
    }

    const AppToast = {
        props: ['items'],
        template: `
<div class="toast-container position-fixed top-0 end-0 p-3" style="z-index:1080;">
  <div v-for="item in items" :key="item.id" class="toast show border-0 mb-2" :class="item.type==='success' ? 'text-bg-success' : 'text-bg-danger'">
    <div class="d-flex">
      <div class="toast-body">{{ item.message }}</div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" @click="$emit('remove', item.id)"></button>
    </div>
  </div>
</div>`
    };

    const AppModal = {
        props: ['modelValue', 'title'],
        emits: ['update:modelValue', 'confirm'],
        template: `
<div class="modal fade" :class="{show:modelValue}" :style="modelValue ? 'display:block;background:rgba(0,0,0,.4);' : ''">
  <div class="modal-dialog modal-dialog-centered"><div class="modal-content">
    <div class="modal-header"><h5 class="modal-title">{{ title }}</h5>
      <button type="button" class="btn-close" @click="$emit('update:modelValue', false)"></button>
    </div>
    <div class="modal-body"><slot></slot></div>
    <div class="modal-footer">
      <button class="btn btn-outline-secondary" @click="$emit('update:modelValue', false)">取消</button>
      <button class="btn btn-primary" @click="$emit('confirm')">確認</button>
    </div>
  </div></div>
</div>`
    };

    const AppLoading = {
        props: ['show'],
        template: `<div v-if="show" class="app-loading-mask"><div class="spinner-border text-light" role="status"></div></div>`
    };

    const AppEmptyState = {
        props: ['text'],
        template: `<div class="text-center py-5 text-muted"><i class="fa-regular fa-folder-open fs-2 d-block mb-2"></i>{{ text || '目前沒有資料' }}</div>`
    };

    const AppPagination = {
        props: ['page', 'totalPages'],
        emits: ['change'],
        template: `
<nav v-if="totalPages > 1"><ul class="pagination justify-content-center">
<li class="page-item" :class="{disabled:page<=1}"><button class="page-link" @click="$emit('change', page-1)">上一頁</button></li>
<li class="page-item disabled"><span class="page-link">{{ page }} / {{ totalPages }}</span></li>
<li class="page-item" :class="{disabled:page>=totalPages}"><button class="page-link" @click="$emit('change', page+1)">下一頁</button></li>
</ul></nav>`
    };

    const AppFormError = {
        props: ['errors', 'field'],
        computed: {
            message() {
                if (!this.errors) return '';
                if (Array.isArray(this.errors[this.field])) return this.errors[this.field][0];
                return this.errors[this.field] || '';
            }
        },
        template: `<div v-if="message" class="invalid-feedback d-block">{{ message }}</div>`
    };

    const AppMoney = {
        props: ['value'],
        computed: {
            text() { return formatMoney(this.value); }
        },
        template: `<span>{{ text }}</span>`
    };

    function createPageApp(options) {
        return Vue.createApp(options)
            .component('app-toast', AppToast)
            .component('app-modal', AppModal)
            .component('app-loading', AppLoading)
            .component('app-empty-state', AppEmptyState)
            .component('app-pagination', AppPagination)
            .component('app-form-error', AppFormError)
            .component('app-money', AppMoney);
    }

    function baseMethods() {
        return {
            pushToast(message, type) {
                this.toasts.push({ id: Date.now() + Math.random(), message, type: type || 'success' });
            },
            removeToast(id) {
                this.toasts = this.toasts.filter(x => x.id !== id);
            },
            toNumberInput(value) {
                return String(value || '').replace(/[^\d.]/g, '').replace(/(\..*)\./g, '$1');
            },
            moneyBlur(field) {
                const num = Number(this.form[field] || 0);
                this.form[field] = Number.isNaN(num) ? '0' : num.toFixed(2).replace(/\.00$/, '');
            }
        };
    }

    window.AppComponents = { createPageApp, baseMethods, formatMoney };
})(window);
