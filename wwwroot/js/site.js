// Funcionalidades principales del sitio
$(document).ready(function() {
    // Actualizar contador del carrito al cargar la página
    actualizarContadorCarrito();
    
    // Inicializar tooltips de Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    // Confirmar eliminaciones
    $('.btn-delete').click(function(e) {
        if (!confirm('¿Estás seguro de que deseas eliminar este elemento?')) {
            e.preventDefault();
        }
    });
    
    // Smooth scroll para enlaces internos
    $('a[href^="#"]').click(function(e) {
        e.preventDefault();
        var target = this.hash;
        var $target = $(target);
        
        $('html, body').animate({
            scrollTop: $target.offset().top - 70
        }, 300, 'swing');
    });
    
    // Validación de formularios
    $('.needs-validation').each(function() {
        $(this).on('submit', function(e) {
            if (!this.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            $(this).addClass('was-validated');
        });
    });
    
    // Previsualización de imágenes
    $('input[type="file"]').change(function(e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                const preview = $(this).siblings('.image-preview');
                if (preview.length) {
                    preview.attr('src', e.target.result).show();
                }
            }.bind(this);
            reader.readAsDataURL(file);
        }
    });
    
    // Filtro en tiempo real para tablas
    $('#search-input').on('keyup', function() {
        const value = $(this).val().toLowerCase();
        $('#data-table tbody tr').filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
    
    // Auto-hide alerts
    $('.alert').each(function() {
        const alert = $(this);
        if (alert.hasClass('alert-dismissible')) {
            setTimeout(function() {
                alert.fadeOut();
            }, 5000);
        }
    });
});

// Función para actualizar contador del carrito
function actualizarContadorCarrito() {
    $.get('/Carrito/GetCount')
        .done(function(count) {
            $('#cart-count').text(count);
        })
        .fail(function() {
            console.log('Error al obtener el contador del carrito');
        });
}

// Función para mostrar notificaciones toast
function mostrarToast(message, type = 'success') {
    const icon = type === 'success' ? 'fas fa-check-circle' : 'fas fa-exclamation-triangle';
    const bgClass = type === 'success' ? 'bg-success' : 'bg-danger';
    
    const toast = `
        <div class="toast align-items-center text-white ${bgClass} border-0 position-fixed top-0 end-0 m-3" 
             role="alert" style="z-index: 1056;">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="${icon} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" 
                        data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    $('body').append(toast);
    const toastElement = new bootstrap.Toast($('.toast').last());
    toastElement.show();
    
    setTimeout(function() {
        $('.toast').last().remove();
    }, 3000);
}

// Función para formatear números como moneda CLP
function formatearMoneda(numero) {
    return new Intl.NumberFormat('es-CL', {
        style: 'currency',
        currency: 'CLP'
    }).format(numero);
}

// Función específica para formatear precios en pesos chilenos
function formatearPrecioCLP(numero) {
    return new Intl.NumberFormat('es-CL', {
        style: 'currency',
        currency: 'CLP',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(numero);
}

// Función para manejar errores AJAX
function manejarErrorAjax(xhr, status, error) {
    console.error('Error AJAX:', error);
    
    let mensaje = 'Ocurrió un error inesperado';
    
    if (xhr.status === 401) {
        mensaje = 'No tienes permisos para realizar esta acción';
    } else if (xhr.status === 404) {
        mensaje = 'Recurso no encontrado';
    } else if (xhr.status === 500) {
        mensaje = 'Error interno del servidor';
    }
    
    mostrarToast(mensaje, 'error');
}

// Configurar AJAX para incluir token antifalsificación
$.ajaxSetup({
    beforeSend: function(xhr, settings) {
        if (settings.type === 'POST' || settings.type === 'PUT' || settings.type === 'DELETE') {
            const token = $('input[name="__RequestVerificationToken"]').val();
            if (token) {
                xhr.setRequestHeader('RequestVerificationToken', token);
            }
        }
    },
    error: manejarErrorAjax
});

// Función para loading spinner
function mostrarLoading(elemento) {
    const originalText = elemento.html();
    elemento.data('original-text', originalText);
    elemento.html('<span class="loading-spinner me-2"></span>Cargando...');
    elemento.prop('disabled', true);
}

function ocultarLoading(elemento) {
    const originalText = elemento.data('original-text');
    elemento.html(originalText);
    elemento.prop('disabled', false);
}

// Función para validar formularios
function validarFormulario(formulario) {
    let esValido = true;
    
    // Validar campos requeridos
    formulario.find('[required]').each(function() {
        if (!$(this).val().trim()) {
            $(this).addClass('is-invalid');
            esValido = false;
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    
    // Validar email
    formulario.find('input[type="email"]').each(function() {
        const email = $(this).val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (email && !emailRegex.test(email)) {
            $(this).addClass('is-invalid');
            esValido = false;
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    
    // Validar números
    formulario.find('input[type="number"]').each(function() {
        const valor = parseFloat($(this).val());
        const min = parseFloat($(this).attr('min'));
        const max = parseFloat($(this).attr('max'));
        
        if (isNaN(valor) || (min && valor < min) || (max && valor > max)) {
            $(this).addClass('is-invalid');
            esValido = false;
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    
    return esValido;
}

// Función para hacer peticiones AJAX seguras
function peticionAjax(url, data, metodo = 'POST') {
    return $.ajax({
        url: url,
        type: metodo,
        data: data,
        dataType: 'json',
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });
}

// Función para confirmar acciones
function confirmarAccion(mensaje, callback) {
    if (confirm(mensaje)) {
        callback();
    }
}

// Función para escapar HTML
function escaparHtml(texto) {
    const div = document.createElement('div');
    div.textContent = texto;
    return div.innerHTML;
}

// Función para truncar texto
function truncarTexto(texto, longitud = 100) {
    if (texto.length <= longitud) return texto;
    return texto.substring(0, longitud) + '...';
}

// Función para mostrar/ocultar contraseña
function togglePassword(inputId, iconId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(iconId);
    
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}

// Función para copiar al portapapeles
function copiarAlPortapapeles(texto) {
    navigator.clipboard.writeText(texto).then(function() {
        mostrarToast('Texto copiado al portapapeles');
    }, function(err) {
        console.error('Error al copiar: ', err);
        mostrarToast('Error al copiar texto', 'error');
    });
}

// Función para scroll suave
function scrollSuave(elemento) {
    elemento.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
    });
}

// Función para lazy loading de imágenes
function inicializarLazyLoading() {
    const images = document.querySelectorAll('img[data-src]');
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });
    
    images.forEach(img => imageObserver.observe(img));
}

// Función para detectar modo oscuro
function detectarModoOscuro() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
}

// Función para cambiar tema
function cambiarTema(tema) {
    document.documentElement.setAttribute('data-theme', tema);
    localStorage.setItem('theme', tema);
}

// Función para inicializar tema
function inicializarTema() {
    const temaGuardado = localStorage.getItem('theme');
    if (temaGuardado) {
        cambiarTema(temaGuardado);
    } else if (detectarModoOscuro()) {
        cambiarTema('dark');
    }
} 