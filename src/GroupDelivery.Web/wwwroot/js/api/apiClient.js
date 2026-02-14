// 檔案用途說明：集中管理前台/後台 API 呼叫，若實際端點不同請優先調整本檔案
(function (window) {
    // 建立 axios instance，統一 base 與 timeout
    const http = axios.create({
        baseURL: '/',
        timeout: 15000,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });

    // 統一錯誤訊息提取，避免每頁重複寫判斷
    function extractError(error) {
        if (error && error.response && error.response.data) {
            return error.response.data.message || error.response.data.error || '系統忙碌中，請稍後再試';
        }
        return error && error.message ? error.message : '網路連線異常，請稍後再試';
    }

    // 提供常用 CRUD 封裝，讓頁面層呼叫更直覺
    const apiClient = {
        extractError,
        groups: {
            list: (params) => http.get('/api/groups', { params }),
            detail: (id) => http.get(`/api/groups/${id}`)
        },
        orders: {
            create: (payload) => http.post('/api/orders', payload),
            mine: (params) => http.get('/api/me/orders', { params }),
            detail: (id) => http.get(`/api/me/orders/${id}`),
            cancel: (id) => http.post(`/api/me/orders/${id}/cancel`)
        },
        me: {
            profile: () => http.get('/api/me/profile')
        },
        merchant: {
            dashboard: () => http.get('/api/merchant/dashboard'),
            stores: {
                list: () => http.get('/api/merchant/stores'),
                create: (payload) => http.post('/api/merchant/stores', payload),
                update: (id, payload) => http.put(`/api/merchant/stores/${id}`, payload),
                toggle: (id, payload) => http.post(`/api/merchant/stores/${id}/toggle`, payload)
            },
            products: {
                list: (storeId) => http.get('/api/merchant/products', { params: { storeId } }),
                create: (payload) => http.post('/api/merchant/products', payload),
                update: (id, payload) => http.put(`/api/merchant/products/${id}`, payload),
                batch: (payload) => http.post('/api/merchant/products/batch', payload)
            },
            groups: {
                create: (payload) => http.post('/api/merchant/groups', payload),
                manage: (id) => http.get(`/api/merchant/groups/${id}/manage`),
                close: (id) => http.post(`/api/merchant/groups/${id}/close`)
            },
            subscription: {
                get: () => http.get('/api/merchant/subscription'),
                upsert: (payload) => http.post('/api/merchant/subscription', payload)
            }
        },
        admin: {
            dashboard: () => http.get('/api/admin/dashboard'),
            stores: (params) => http.get('/api/admin/stores', { params }),
            groups: (params) => http.get('/api/admin/groups', { params }),
            closeGroup: (id) => http.post(`/api/admin/groups/${id}/close`),
            suspendStore: (id, payload) => http.post(`/api/admin/stores/${id}/suspend`, payload)
        }
    };

    window.apiClient = apiClient;
})(window);
