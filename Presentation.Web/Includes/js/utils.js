/*=============================================================================
*  Info      : App JS - 2019
*  Author    : Aldo Almarza - aldo.almarza@gmail.com
*============================================================================*/


var isPreview = false;
var iFrameGlobal = false;
var MaxColapsableChar = 50;
var maxTextInGrid = 100;
var xJSONResult, DataHTM, xhh, xTmp, xResultJsonData;
var IsEdit = false;
var IsOpen = false;
var CaracteresEspeciales = [8, 9, 37, 39, 46];
var nav4 = window.Event ? true : false;
var IsOpen = store.get("IsOpen") || false;
var minInt = Config.IntMinValue;
var maxInt = Config.IntMaxValue;

var showChar = 50;

var ReturnJson = {
    ErrorCaptcha: -2,
    ErrorModelo: -1,
    SinAccion: 0,
    ActionSuccess: 1,
    UsuarioYaExiste: 2,
    UsuarioNoRegistrado: 3,
    LoginSuccess: 4,
    ErrorCodigoVerificacion: 5,
    Updated: 6,
    ErrorSharePoint: 7,
    FolioYaExiste: 8,
    YaExisteDescripcion: 9,
    DatoNoEncontrado: 10,
    ValidaData: 11
}

function SetOpen(open) {
    store.set("IsOpen", open);
}

window.log = function f() {
    log.history = log.history || [];
    log.history.push(arguments);
    if (this.console) {
        var args = arguments,
			newarr;
        args.callee = args.callee.caller;
        newarr = [].slice.call(args);
        if (typeof console.log === 'object') log.apply.call(console.log, console, newarr);
        else console.log.apply(console, newarr);
    }
};
(function (a) {
    function b() { }
    for (var c = "assert,count,debug,dir,dirxml,error,exception,group,groupCollapsed,groupEnd,info,log,markTimeline,profile,profileEnd,time,timeEnd,trace,warn".split(","), d; !!(d = c.pop()) ;) {
        a[d] = a[d] || b;
    }
})
(function () {
    try {
        //console.log('...');
        return window.console;
    } catch (a) {
        return (window.console = {});
    }
}());

if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}

// prototipo para reemplazar strings
String.prototype.ReplaceAll = function (strBusca, strReplace) {
    var temp = this;
    var index = temp.indexOf(strBusca);

    while (index != -1) {
        temp = temp.replace(strBusca, strReplace);
        index = temp.indexOf(strBusca);
    }
    return temp;
}

if (!String.prototype.includes) {

    Object.defineProperty(String.prototype, 'includes', {
        value: function (substring, searchposition) {
            if (typeof searchposition !== 'number') {
                searchposition = 0
            }
            if (searchposition + substring.length > this.length) {
                return false
            } else {
                return this.indexOf(substring, searchposition) !== -1
            }
        }
    })
}

String.prototype.ReplaceNameFileChars = function (charReplace) {
    var charReplace = charReplace || "_";
    var caracteres = " ,ü'".split("");
    var texto = this;

    for (var i = 0; i < caracteres.length; i++) {
        var index = texto.indexOf(caracteres[i]);

        while (index != -1) {
            texto = texto.replace(caracteres[i], charReplace);
            index = texto.indexOf(caracteres[i]);
        }
    }

    return texto;
}

Date.prototype.getDays = function () {
    return Math.floor(new Date() / 1000 * 60 * 60 * 24);
}

String.prototype.bool = function () {
    return (/^true$/i).test(this);
}

function isEmptyOrSpaces(str) {
    try {
        return str === null || str.match(/^ *$/) !== null;
    } catch (e) {
        return false;
    }
}

$.fn.optVisible = function (show) {
    if (show) {
        this.filter("span > option").unwrap();
    } else {
        this.filter(":not(span > option)").wrap("<span>").parent().hide();
    }
    return this;
}

$.extend($.expr[':'], {
    isEmpty: function (el) {
        return $.trim($(el).val()) === "";
    },
    notEmpty: function (el) {
        return $.trim($(el).val()) !== "";
    },
    isCero: function (el) {
        return $.trim($(el).val()) === "0";
    },
    notCero: function (el) {
        return $.trim($(el).val()) !== "0";
    }
});

$.fn.addError = function () {
    this.addClass("is-invalid");
};

$.fn.removeError = function () {
    this.removeClass("is-invalid");
};

function SetShorten() {
    //$(".Colapsable").not(".NotColapse").shorten({ showChars: MaxColapsableChar });

    //$(".iconinfoAlert").next("p").shorten({ showChars: showChar });
}

function TrimForm() {

    $('form input, form textarea').each(function () {
        $(this).val($.trim($(this).val()));
    });
}

function TrimFormByID(form) {

    $(form + ' input, ' + form + ' textarea').each(function () {
        $(this).val($.trim($(this).val()));
    });
}

function IsFechaMenor(FechaInicio, FechaFin) {
    var d1 = moment(FechaInicio, "DD-MM-YYYY");
    var d2 = moment(FechaFin, "DD-MM-YYYY");

    return (d1 < d2);
}

function IsFechaMayor(FechaInicio, FechaFin) {

    var d1 = moment(FechaInicio, "DD-MM-YYYY");
    var d2 = moment(FechaFin, "DD-MM-YYYY");

    return (d1 > d2);
}

function IsFechaMenorIgual(FechaInicio, FechaFin) {
    var d1 = moment(FechaInicio, "DD-MM-YYYY");
    var d2 = moment(FechaFin, "DD-MM-YYYY");

    return (d1 <= d2);
}

function IsFechaMayorIgual(FechaInicio, FechaFin) {

    var d1 = moment(FechaInicio, "DD-MM-YYYY");
    var d2 = moment(FechaFin, "DD-MM-YYYY");

    return (d1 >= d2);
}

function NumbersValidate() {

    $(".IsInteger").not(".noValidate").attr("maxlength", "10");

    $(".IsInteger").on('keypress', function (event) { return onlyNum(event); });

    $('.IsInteger').not(".noValidate").bind('blur', //keyup
		function (event) {
		    $(this).val($(this).val().replace(/[^0-9]/g, ''));

		    valor = parseInt($(this).val());
		    if (!(valor >= minInt && valor <= maxInt)) {
		        if ($(this).val() != "") {
		            console.log(valor);
		            showBlockUIError("Valor numérico debe ser menor a " + maxInt);
		            $(this).val("");
		            return false;
		        }
		    }

		}
	);

    $(".IsInteger:enabled:not([readonly])").each(function () {

        $(this).on('keypress', function (event) { return onlyNum(event); });
        $(this).on("focus", function () { $(this).select(); });
    });

    $('.IsInteger').bind('blur', //keyup
		function (event) {
		    $(this).val($(this).val().replace(/[^0-9]/g, ''));
		}
	);

    $(".IsNumberFormat:enabled:not([readonly])").each(function () {

        $(this).on('keypress', function (event) { return onlyNumComma(event); });

        $(this).on("focus", function () { $(this).select(); });

        $(this).blur(function () {
            var v = $(this).val();

            if (v === "") {
                return false;
            }

            var num_decimals = parseInt($(this).attr("data-num-decimals")) || 0;
            var number = $.number(v, num_decimals);
            if (number === 0) {
                $(this).empty().val("");
            } else {
                $(this).empty().val(number);
            }
        });

    });
}

function isNumeric32() {

    $('.IsNumeric').bind('blur', function () { //keyup
        $(this).val($(this).val().replace(/[^0-9]/g, ''));
    });
}

function FormatNumbers() {

    $(".IsFloat").each(function () {
        var valor = $(this).val();//attr("data-valor");
        var num_decimals = parseInt($(this).attr("data-num-decimals")) || 0;

        valor = valor.ReplaceAll(Config.SeparadorMiles, "");

        var newValor = $.number(valor, num_decimals, Config.SeparadorDecimales, Config.SeparadorMiles);

        $(this).val(newValor);
    });
}

function NumberFormat(valor, num_decimals) {
    var number = valor.toString().ReplaceAll(Config.SeparadorMiles, "");

    return $.number(number, num_decimals, Config.SeparadorDecimales, Config.SeparadorMiles);
}

function PorcentajeView(valor, decimales) {
    var decimales = decimales || 2;

    if (valor === 0) { return 0; }

    valor = parseFloat(valor * 100);

    //var re = /^\d+(?:\.\d{0,2})?/

    return Number(valor.toString().match(/^\d+(?:\.\d{0,2})?/));

    //var calculo = valor.toFixed(decimales);
    //var sep = calculo.toString().split(".");

    //if (sep.length > 1)
    //{
    //    //console.log(sep[1], Math.floor(sep[1]));
    //    //var IsDecimal = (calculo > Math.floor(calculo));//(calculo % 1 == 0);

    //    if (Math.floor(sep[1]) != 0) {
    //        valor = calculo.toFixed(decimales);//.ReplaceAll(".", Config.SeparadorDecimales);
    //    } else {
    //        valor = sep[0];
    //    }
    //}

    //return valor;


}

function CleanNumber(strNumber) {
    return strNumber.toString().ReplaceAll(Config.SeparadorMiles, "");
}

function IsPorcentaje(msg) {

    var msg = msg || "Porcentaje no debe ser mayor a 100";

    $(".IsPorcentaje").each(function () {

        var num_decimals = parseInt($(this).attr("data-num-decimals")) || 0;

        $(this).on("focus", function () { $(this).select(); });

        $(this).on('keypress', function (event) { return IsPorcentajeApply(event) });
        $(this).bind('keyup blur',
			function (event) {

			    var str = /[^0-9\,.]/

			    var clean = "";

			    if (Config.SeparadorDecimales == ",") {
			        clean = $(this).val().replace(/[^0-9\,]/g, '');
			    } else {
			        clean = $(this).val().replace(/[^0-9\.]/g, '');
			    }

			    $(this).val(clean);

			    var valor = parseInt($(this).val());
			    if (valor > 100) {
			        showBlockUIError(msg);
			        $(this).val("");
			    }
			}
		);
    });

    //$(".IsPorcentaje").on("blur", function () {

    //})
}

function IsPorcentajeApply(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789" + Config.SeparadorDecimales;

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}

function addCloseFunctionToDatePicker(id, funHandler) {
    var $datePicker = $('#' + id);
    var prevhandler = $datePicker.datepicker('option', 'onClose');

    $datePicker.datepicker('option',
        {
            onClose: function (dateText, inst) {
                if (prevhandler) {
                    prevhandler(dateText, inst);
                }

                funHandler(dateText, inst);
            }
        });
}

function DisableDaysoDatePicker(id, funHandler) {
    var $datePicker = $('#' + id);
    var prevhandler = $datePicker.datepicker('option', 'onClose');

    $datePicker.datepicker('option',
        {
            onClose: function (dateText, inst) {
                if (prevhandler) {
                    prevhandler(dateText, inst);
                }

                funHandler(dateText, inst);
            }
        });
}


$.fn.reset = function () {
    $(this).each(function () { this.reset() });
}

$.fn.clean = function () {
    return $(this).val("");
}

$.fn.disable = function () {
    return $(this).attr("disabled", true);
}

$.fn.enable = function () {
    return $(this).attr("disabled", false);
}

$.fn.readonly = function () {
    return $(this).attr("readonly", true);
}

$.fn.readonlyNot = function () {
    return $(this).attr("readonly", false);
}

function IsURL() {

    $(".IsURL").on('blur', function () {

        var str = $.trim($(this).val());
        if (str === "") return false;

        if (!IsURLValidate(str)) {
            $(this).parents(".form-group").addClass("has-error");
            $(this).val("");
            return false;
        }

        $(this).parents(".form-group").removeClass("has-error");
        return true;

    });
}

function IsURLValidate(userInput) {

    var res = userInput.match(/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g);

    if (res == null) {
        return false;
    } else {
        return true;
    }
}

$.datepicker.regional['es'] = {
    clearText: 'Borra',
    clearStatus: 'Borra fecha actual',
    closeText: 'Cerrar',
    closeStatus: 'Cerrar sin guardar',
    prevText: '',
    prevBigText: '<<',
    prevStatus: 'Mostrar mes anterior',
    prevBigStatus: 'Mostrar año anterior',
    nextText: '',
    nextBigText: '>>',
    nextStatus: 'Mostrar mes siguiente',
    nextBigStatus: 'Mostrar año siguiente',
    currentText: 'Hoy',
    currentStatus: 'Mostrar mes actual',
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    monthStatus: 'Seleccionar otro mes',
    yearStatus: 'Seleccionar otro año',
    weekHeader: 'Sm',
    weekStatus: 'Semana del año',
    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
    dayStatus: 'Set DD as first week day',
    dateStatus: 'Select D, M d',
    dateFormat: 'dd/mm/yy',
    firstDay: 1,
    initStatus: 'Seleccionar fecha',
    isRTL: false
};

$.datepicker.setDefaults($.datepicker.regional['es']);
$.datepicker.setDefaults({
    changeYear: true, changeMonth: true, showButtonPanel: false, firstDay: 1,
    showAnim: "drop", dateFormat: 'dd-mm-yy', defaultDate: '+0'
});

function IsEmailMultiple() {
    $(".IsEmailMultiple").on('blur', function () { IsEmailMultipleValidate($(this)); });
}

function IsEmail() {
    $(".IsEmail").on('blur', function () { IsEmailValidate($(this)); });
}
function IsEmailValidate(campo) {
    var str = $.trim($(campo).val());
    if (str === "") {
        $(campo).removeClass("is-valid");
        return false;
    }

    if (!validateEmail(str)) {
        //$(campo).parent().parent().addClass("has-error");
        $(campo).addClass("is-invalid");
        showBlockUIError("Email incorrecto")
        $(campo).val("");
        return false;
    }

    //$(campo).parent().parent().removeClass("has-error");
    $(campo).removeClass("is-invalid").addClass("is-valid");
    return true;
}

function IsEmailMultipleValidate(obj) {

    var str = $.trim($(obj).val());
    if (str === "") return false;

    var valido = true;
    var emails = str.split(new RegExp("\\s*,\\s*", "gi"));

    $.each(emails, function (index, value) {
        if (!validateEmail(value)) {
            $(obj).addClass("is-invalid");
            $(obj).val("");
            valido = false;
        }
    });

    if (valido) $(obj).removeClass("is-invalid").addClass("is-valid");

    return valido;
}

function validateEmail(valor) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(valor);
}

$.extend({
    getUrlVars: function () {
        var vars = [],
			hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});

function getVToken() {

    var obj = $("input[name='__RequestVerificationToken']").eq(0).val();

    return obj;
}

function onlyNum(evt) {
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57));
}

function onlyNumComma(evt) {
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key === 46);
}

function IsFloatValidate() {
    $(".IsFloat:enabled:not([readonly])").each(function () {

        var num_decimals = parseInt($(this).attr("data-num-decimals")) || 0;

        $(this).on("focus", function () { $(this).select(); });

        $(this).on('keypress', function (event) { return IsFloatValidateApply(event) });
        $(this).bind('keyup blur',
			function (event) {

			    var clean = $(this).val().replace(/[^0-9\,.]/g, '');
			    $(this).val(clean);

			    var numero = $(this).val().ReplaceAll(Config.SeparadorMiles, "");
			    numero = numero.ReplaceAll(Config.SeparadorDecimales, ".");

			    var number = $.number(numero, num_decimals, Config.SeparadorDecimales, Config.SeparadorMiles);

			    $(this).empty().val(number);

			}
		);
    });
}

function IsFloatValidateApply(e) {

    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789,.";

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}


function IsAlphabetValidate() {

    $(".IsAlphabet:visible:enabled:not([readonly])").each(function () {

        $(this).on('keypress', function (event) { return IsAlphabetApply(event) });

        $(this).bind('keyup blur',
            function (event) {
                $(this).val($(this).val().replace(/[^a-zA-Z ñÑ áÁ éÉ íÍ óÓ úÚ üÜ\'-]/g, ''));
            }
        );
    });
}

function IsAlphabetApply(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúüabcdefghijklmnñopqrstuvwxyzÁÉÍÓÚÜABCDEFGHIJKLMNÑOPQRSTUVWXYZ-'"; //º°

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}


function isAlphaNumericValidate() {

    $(".isAlphaNumeric:visible:enabled:not([readonly])").each(function () {

        $(this).on("focus", function () { $(this).select(); });
        $(this).on('keypress', function (event) { return isAlphaNumericApply(event) });

        $(this).bind('keyup blur',
			function (event) {
			    $(this).val($(this).val().replace(/[^a-zA-Z0-9 ñÑ áÁ éÉ íÍ óÓ úÚ üÜ\-°º@,.+()]/g, ''));
			}
		);
    });
}

function isAlphaNumericApply(e) {

    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúüabcdefghijklmnñopqrstuvwxyzÁÉÍÓÚÜABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789-°º@,.+()";

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}



function IsIntegerValidate() {
    //$(".IsInteger").attr("maxlength", "10");
    $(".IsInteger").addClass("ar");

    $(".IsInteger").on('keypress', function (event) { return onlyNum(event); });

    $(".IsInteger").on("focus", function () { $(this).select(); });

    $('.IsInteger').bind('keyup blur',
		function (event) {
		    $(this).val($(this).val().replace(/[^0-9]/g, ''));

		    if ($(this).val() == "") {
		        $(this).val("");
		    }

		    valor = parseInt($(this).val());
		    if (!(valor >= Config.IntMinValue && valor <= Config.IntMaxValue)) {
		        if ($(this).val() != "") {
		            console.log(valor);
		            showBlockUIError("Valor numérico debe ser menor a " + maxInt);
		            $(this).val("0");
		            return false;
		        }
		    }

		}
	)
}

function IsNumberFormatValidate() {

    $(".IsNumberFormat:enabled:not([readonly])").each(function () {

        $(this).on('keypress', function (event) { return onlyNumComma(event); });

        $(this).on("focus", function () { $(this).select(); });

        $(this).blur(function () {
            var num_decimals = parseInt($(this).attr("data-num-decimals")) || 0;
            var number = $.number($(this).val(), num_decimals);
            if (number === 0) {
                $(this).empty().val("");
            } else {
                $(this).empty().val(number);
            }
        });
    });
}



function IsNumericValidate() {

    $(".IsNumeric:enabled:not([readonly])").each(function () {

        $(this).on('keypress', function (event) { return onlyNum(event); });
        $(this).on("focus", function () { $(this).select(); });
    });

    $('.IsNumeric').bind('keyup blur',
		function (event) {
		    $(this).val($(this).val().replace(/[^0-9]/g, ''));
		}
	);
}

function IsAlphabet() {

    $(".IsAlphabet:enabled:not([readonly])").each(function () { $(this).on('keypress', function (event) { return IsAlphabetApply(event) }); });
    $('.IsAlphabet').bind('blur',
		function (event) {
		    $(this).val($(this).val().replace(/[^a-zA-Z ñÑ áÁ éÉ íÍ óÓ úÚ üÜ\'-]/g, '')); //º°
		}
	);
}
function IsAlphabetApply(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúüabcdefghijklmnñopqrstuvwxyzÁÉÍÓÚÜABCDEFGHIJKLMNÑOPQRSTUVWXYZ-'"; //º°

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}

function scrollToElement(selector, time, verticalOffset) {
    time = typeof (time) != 'undefined' ? time : 700;
    verticalOffset = typeof (verticalOffset) != 'undefined' ? verticalOffset : 0;
    element = $(selector);
    offset = element.offset();
    offsetTop = offset.top + verticalOffset;
    $('html, body').animate({
        scrollTop: offsetTop
    }, time);
}

var removeValue = function (array, id) {
    return _.reject(array, function (item) {
        return item === id; // or some complex logic
    });
}

function SelfRedirect() {

    //var key = key = "none";

    blockPageUI();
    setTimeout(function () {
        var href = $(location).attr('href');//removeParam(key, $(location).attr('href'));

        window.location.href = href;

    }, 300);

}

function LinkRedirect(URL) {

    blockPageUI();
    window.location.href = URL;
}

function BtnInAction(btn, visible, next) {

    if (visible) {
        $(btn).removeAttr("disabled").removeClass("disabled");
        if (next) {
            $(btn).next().addClass('hide');
        } else {
            $(btn).prev().addClass('hide');
        }
    } else {
        $(btn).attr("disabled", "disabled").addClass("disabled");
        if (next) {
            $(btn).next().removeClass('hide');
        } else {
            $(btn).prev().removeClass('hide');
        }
    }
}

function xUIBtnAjax(btnClick, show_or_hide, indexImg) {

    show_or_hide = show_or_hide || '';
    indexImg = indexImg || 0;

    //boton activo --> imagen hide
    if (show_or_hide === 0) {
        $(btnClick).removeAttr("disabled").removeClass("disabled");
        $(btnClick).next().addClass('hide');
    }

    //boton disabled --> imagen show
    if (show_or_hide == 1) {
        $(btnClick).attr("disabled", "disabled").addClass("disabled");
        $(btnClick).next().removeClass('hide');
    }
}

function getEnterTextBox(obj, callback) {

    var callback = callback || $.noop;

    $(obj).on("keypress", function (e) {
        if (e.keyCode == 13) {
            callback();
            return false;
        }
    });

}

function setTabStorage(index, Identificador) {
    store.set(Identificador, { 'index': index });
}

function getTabStorage(Identificador) {

    var index = store.get(Identificador) || 0;
    if (index === 0) {
        return 0;
    }

    return store.get(Identificador).index;
}

function IsFechaValida(campo) {
    var campo = $("#" + campo).val();

    return moment(campo, 'DD-MM-YYYY', true).isValid();
}

function IsMayorQueHoy(campo) {
    var ahora = moment();
    var campo = moment($("#" + campo).val(), "DD-MM-YYYY"); //$("#" + campo).val();

    return (campo > ahora);
}

function SetVolverLS(opcion) {
    store.set("volver_" + opcion, true);
}

function GetVolverLS(opcion) {

    var Identificador = "volver_" + opcion;

    var valor = store.get(Identificador) || 0;

    if (valor === 0) {
        return false;
    } else {
        return valor;
    }
}

function ReloadCaptcha(obj, key) {

    //var info = obj + " - " + key;
    //alert(obj + " - " + key);
    //console.log(info);

    var obj = obj || "imgCaptcha";

    $("#" + obj).attr("src", "/Includes/css/images/bgCaptcha.png");

    $("#" + obj).attr("src", "/AntiBotImage.ashx?" + $.now() + "&" + "key=" + key);

    //console.log("ReloadCaptcha -> " + obj);
}


function IsDireccion() {

    $(".IsDireccion:enabled:not([readonly])").each(function () { $(this).on('keypress', function (event) { return IsDireccionFieldApply(event) }); });
    $('.IsDireccion').bind('blur', //keyup
		function (event) {
		    $(this).val($(this).val().replace(/[^a-zA-Z0-9 ñÑ áÁ éÉ íÍ óÓ úÚ üÜ\-_,.#º°'%/()"]/g, ''));
		}
	);
}
function IsDireccionFieldApply(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúüabcdefghijklmnñopqrstuvwxyzÁÉÍÓÚÜABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789-_,.#º°\"'%/()";

    tecla_especial = false
    for (var i in CaracteresEspeciales) {
        if (key == CaracteresEspeciales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) return false;
}

function SetCurrentPage(pageNumber, opcion) {
    store.set("page_" + opcion, pageNumber);
}

function GetCurrentPage(opcion) {
    var Identificador = "page_" + opcion;
    var valor = store.get(Identificador) || 0;
    return valor;
}

function createFormDownload(FrmID) {

    FrmID = FrmID || "frmDown";

    $("#" + FrmID).remove();

    jQuery('<form/>', {
        id: FrmID,
        name: FrmID,
        method: 'POST',
        action: '/ES/Uploader/DownloadFile',
        target: '_blank'
    }).appendTo('body');

    $("#frmDown").append('<input name="ObjID1" id="ObjID1" type="hidden" value="0" />');
    $("#frmDown").append('<input name="ObjID2" id="ObjID2" type="hidden" value="0" />');
    $("#frmDown").append('<input name="Hash" id="Hash" type="hidden" value="0" />');
    $("#frmDown").append('<input name="TipoDoc" id="TipoDoc" type="hidden" value="0" />');
    $("#frmDown").append('<input type="hidden" value="" id="VerificationToken" name="__RequestVerificationToken">');
}

function getDownloadFile(ObjID1, ObjID2, Hash, TipoDoc) {

    createFormDownload("frmDown");

    $("#frmDown #ObjID1").val(ObjID1);
    $("#frmDown #ObjID2").val(ObjID2);

    $("#frmDown #Hash").val(Hash);
    $("#frmDown #TipoDoc").val(TipoDoc);
    $("#frmDown #VerificationToken").val(getVToken());

    if ($.trim(Hash) == "") {
        showBlockUIError("Error, Hash inexistente")
    } else {
        $("#frmDown").submit();
    }
}

function getDeleteFile(ObjID1, TipoDoc, VersionEnc, Hash, ObjID2, end) {

    ObjID2 = ObjID2 || 0;
    end = end || 0;

    var Action = "/ES/Uploader/DeleteFile";
    var params = { __RequestVerificationToken: getVToken(), ObjID1: ObjID1, TipoDoc: TipoDoc, VersionEnc: VersionEnc, Hash: Hash, ObjID2: ObjID2 };
    var callback = function () { };

    var msg1 = "¿Confirma <b>eliminar</b> el archivo?";
    var msg2 = "El archivo ha sido eliminado";

    var opts = {
        title: "Eliminar archivo",
        msg1: msg1,
        url: Action,
        params: params,
        alert: 'alert-danger',
        //btn1Hide: true,
        //replaceParrafo: true,
        callback: function () {

            if (end == 0) {
                SelfRedirect();
            } else {
                if ($.isFunction(end)) {
                    end.apply();
                }
            }
            
        }
    }

    xDialogConfirmAction(opts);

    //xDialogConfirmActionAjax("Eliminar archivo", msg1, msg2, Action, params);
}

function OpenDialogIframe(SRC, title, onClose, _width, _height, _top) {
    var _onClose = onClose || $.noop;
    var _width = _width || 500;
    var _height = _height || 300;
    var _top = _top || 40;

    var previewFrame = $('<iframe id="iframeDialog" name="iframeDialog" src="' + SRC + '" style=""  />');

    previewFrame.dialog({
        autoOpen: true,
        title: title,
        width: _width,
        height: _height,
        position: ['center', _top],
        modal: true, resizable: true,
        close: function () {
            if ($.isFunction(_onClose)) {
                _onClose.apply();
            }
        },
        resize: function (event, ui) {

            //console.log($(window).width());
            //FixIframeRespuesta();
        },
        open: function () {
        }
    }).width(_width - 10).height(_height);


}

function enumerateItems(css) {

    $("." + css).each(function (x, y) {

        var number = x + 1;

        $(this).text(number)

    });
}

$.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
    return {
        "iStart": oSettings._iDisplayStart,
        "iEnd": oSettings.fnDisplayEnd(),
        "iLength": oSettings._iDisplayLength,
        "iTotal": oSettings.fnRecordsTotal(),
        "iFilteredTotal": oSettings.fnRecordsDisplay(),
        "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
        "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    };
};


function ValidateRUT(numero, digito) {

    $("#" + numero + ", #" + digito + "").on("blur", function () {

        var rut = $("#" + numero).val();
        var dv = $("#" + digito).val();

        var isRut = $("#" + numero).hasClass("IsRut");

        if (rut !== "" && dv !== "" && isRut) {
            var valido = $.Rut.validar(rut + "-" + dv);
            if (!valido) {
                showBlockUIError("El Rut es incorrecto");
                $("#" + numero).val("").addClass("is-invalid");
                $("#" + digito).val("").addClass("is-invalid");
                return false;
            } else {
                if ($("#" + numero).hasClass("is-invalid")) {
                    $("#" + numero).removeClass("is-invalid").addClass("is-valid");
                    $("#" + digito).removeClass("is-invalid").addClass("is-valid");
                }
            }
        }

    });
}

function SetRutInForm(rut, campoNumero, campoDv) {

    var _RutFormat = parseInt(rut) || 0;
    if (_RutFormat != 0) {
        $("#" + campoNumero).val(_RutFormat);
        $("#" + campoDv).val($.Rut.getDigito(_RutFormat.toString()));
    }
}

function CallSelect2(obj, callback) {

    $('#' + obj).select2({
        language: {
            noResults: function () {
                return "No hay resultados";
            },
            searching: function () {
                return "Buscando...";
            }
        }
    });

    $('#' + obj).on('select2:select', function (e) {

        if ($.isFunction(callback)) {
            callback.apply();
        }

    });
}

function objectifyForm(formArray) {
    //serialize data function
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

var ConfigSemaforo = function (_dias, status1, status2) {

    this.Dias = _dias;

    this.IsGreen = function () {
        return (this.Dias <= status1);
    }

    this.IsYellow = function () {
        return (this.Dias > status1 && this.Dias <= status2);
    }

    this.IsRed = function () {
        return (this.Dias > status2);
    }

}

function GetSemaforo(dias, status1, status2) {

    var strDias = (dias > 1 || dias == 0) ? "días" : "día";

    var color = "rgb(1, 186, 1)";
    var alarma = "";
    var html = "";

    var regla = new ConfigSemaforo(dias, status1, status2);

    if (regla.IsGreen()) {
        color = "rgb(1, 186, 1)";
    }

    if (regla.IsYellow()) {
        color = "rgb(199, 197, 7)";
        alarma = "<br><i class='x-alerta-yellow'></i>";
    }

    if (regla.IsRed()) {
        color = "red";
        alarma = "<br><i class='x-alerta-red'></i>";
    }

    html = "<span style='color: " + color + "'>" + dias + " " + strDias + "</span>" + alarma;

    return html;
}
