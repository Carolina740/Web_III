// ============================================================
//  El Arca de Noé – Veterinaria
//  Utilidades JavaScript globales
// ============================================================

(function () {
    'use strict';

    // --- Resaltar el ítem de navbar activo según la URL actual ---
    document.addEventListener('DOMContentLoaded', function () {
        const path = window.location.pathname.toLowerCase();
        document.querySelectorAll('.navbar-vet .nav-link').forEach(function (link) {
            const href = link.getAttribute('href');
            if (!href) return;
            const hrefLower = href.toLowerCase();
            // Coincidencia exacta para Dashboard (/), parcial para el resto
            const isActive = (hrefLower === '/' && path === '/')
                || (hrefLower !== '/' && path.startsWith(hrefLower));
            if (isActive) {
                link.classList.add('active');
            }
        });
    });

})();
