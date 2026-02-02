// ================================
// Layout.js
// 管理：
// - 登入 Modal
// - Email 登入 API
// - Navbar 登入狀態刷新
// ================================

let sending = false;

    // ---------- DOM ----------
    function el(id) {
        return document.getElementById(id);
    }

    // ---------- Auth ----------
async function sendLoginEmail() {
    if (sending) return;
        const emailInput = el('loginEmail');
        if (!emailInput) return;

        const email = emailInput.value.trim();
        if (!email) {
            alert('請輸入 Email');
            return;
        }
    sending = true;
    const btn = event.target;
    btn.disabled = true;
    btn.innerText = '寄送中...';
        try {
            await axios.post('/Auth/SendLoginLink', {
                email: email
            });

            emailInput.value = '';

            alert('驗證信已寄出，請查看信箱');

            closeAuthModal();

        } catch (err) {
            console.error(err);
            alert('寄送失敗，請稍後再試');
        }
    }

    // ---------- Modal ----------
    function closeAuthModal() {
        const modalEl = el('authModal');
        if (!modalEl) return;

        const modal = bootstrap.Modal.getInstance(modalEl)
            || new bootstrap.Modal(modalEl);

        modal.hide();
    }

    function openAuthModal() {
        const modalEl = el('authModal');
        if (!modalEl) return;

        const modal = new bootstrap.Modal(modalEl);
        modal.show();
    }

    // ---------- Navbar ----------
    async function refreshNavbar() {
        try {
            const res = await axios.get('/Account/AuthStatus');
            const data = res.data;

            // 依登入狀態動態切換
            const loginBtn = document.querySelector('[data-auth="login"]');
            const userMenu = document.querySelector('[data-auth="user"]');

            if (data.isAuthenticated) {
                if (loginBtn) loginBtn.classList.add('d-none');
                if (userMenu) {
                    userMenu.classList.remove('d-none');
                    const nameEl = userMenu.querySelector('[data-auth-name]');
                    if (nameEl) nameEl.textContent = data.displayName;
                }
            } else {
                if (loginBtn) loginBtn.classList.remove('d-none');
                if (userMenu) userMenu.classList.add('d-none');
            }

        } catch (err) {
            console.warn('AuthStatus 取得失敗');
        }
}


    // ---------- Expose ----------
    window.Layout = {
        openAuthModal,
        sendLoginEmail,
        refreshNavbar
    };

