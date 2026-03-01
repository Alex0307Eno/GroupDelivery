// ================================
// Layout.js
// 管理：
// - 登入 Modal
// - Email 登入 API
// - Navbar 登入狀態刷新
// - 全站 Toast 通知
// ================================

let sending = false;

// ---------- DOM ----------
function el(id) {
    return document.getElementById(id);
}

// ---------- Toast ----------
function showToast(message, type) {

    const toastEl = el("globalToast");
    const msgEl = el("globalToastMessage");

    if (!toastEl || !msgEl) return;

    toastEl.className = "toast align-items-center text-bg-" + (type || "dark") + " border-0";

    msgEl.textContent = message;

    const toast = new bootstrap.Toast(toastEl, {
        delay: 2500
    });

    toast.show();
}

// ---------- Auth ----------
async function sendLoginEmail(event) {

    if (sending) return;

    const emailInput = el('loginEmail');
    if (!emailInput) return;

    const email = emailInput.value.trim();

    if (!email) {
        Layout.showToast('請輸入 Email', 'warning');
        return;
    }

    sending = true;

    try {

        await axios.post('/Auth/SendLoginLink', {
            email: email
        });

        emailInput.value = '';

        Layout.showToast('驗證信已寄出，請前往信箱完成登入', 'success');

        // 關閉 modal
        closeAuthModal();

        // 可選：3 秒後導回首頁
        setTimeout(() => {
            window.location.href = "/";
        }, 3000);

    } catch (err) {

        Layout.showToast('寄送失敗，請稍後再試', 'danger');

    } finally {

        sending = false;
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
    refreshNavbar,
    showToast
};