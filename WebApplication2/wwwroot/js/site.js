// ============================================================
//  El Arca de Noé – Veterinaria
//  Utilidades JavaScript globales
// ============================================================

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {

        // ── 1. Resaltar ítem activo en el navbar ──────────────
        const path = window.location.pathname.toLowerCase();
        document.querySelectorAll('.navbar-vet .nav-link').forEach(function (link) {
            const href = (link.getAttribute('href') || '').toLowerCase();
            if (!href) return;
            const isActive = (href === '/' && (path === '/' || path === ''))
                || (href !== '/' && path.startsWith(href));
            if (isActive) link.classList.add('active');
        });

        // ── 2. Auto-cierre de alertas TempData (4 segundos) ───
        setTimeout(function () {
            document.querySelectorAll('.alert-dismissible').forEach(function (alert) {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                if (bsAlert) bsAlert.close();
            });
        }, 4000);

        // ── 3. Activar tooltips de Bootstrap ──────────────────
        const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"], [title]');
        tooltipElements.forEach(function (el) {
            // Solo activar como tooltip en botones pequeños (acciones de tabla)
            if (el.classList.contains('btn') && el.title) {
                new bootstrap.Tooltip(el, { trigger: 'hover', placement: 'top' });
            }
        });

        // ── 4. Confirmar eliminaciones (forms en página Delete) ─
        const deleteForms = document.querySelectorAll('form[data-confirm]');
        deleteForms.forEach(function (form) {
            form.addEventListener('submit', function (e) {
                const msg = form.getAttribute('data-confirm') || '¿Confirmas esta acción?';
                if (!window.confirm(msg)) e.preventDefault();
            });
        });

        // ── 5. Resaltar fila de tabla al hacer click ───────────
        document.querySelectorAll('.table-vet tbody tr').forEach(function (row) {
            row.style.cursor = 'default';
        });

        // ── 6. Contador de caracteres para textareas ──────────
        document.querySelectorAll('textarea[maxlength]').forEach(function (ta) {
            const max     = parseInt(ta.getAttribute('maxlength'));
            const counter = document.createElement('div');
            counter.className = 'form-text text-end text-muted';
            counter.style.fontSize = '.75rem';
            counter.textContent = `0 / ${max} caracteres`;
            ta.insertAdjacentElement('afterend', counter);

            ta.addEventListener('input', function () {
                const len = ta.value.length;
                counter.textContent = `${len} / ${max} caracteres`;
                counter.style.color = len > max * 0.9
                    ? 'var(--vet-red)'
                    : 'var(--vet-text-muted)';
            });
        });

        // ── 7. Scroll suave al top en navegación ──────────────
        window.scrollTo({ top: 0, behavior: 'smooth' });

    });

})();
