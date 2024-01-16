/*=============================================================================
*  Info      : App JS - 2019
*  Author    : Aldo Almarza - aldo.almarza@gmail.com
*============================================================================*/

$(document).ajaxStart(
        function () {
            if (Config.IsBlock) {
                addAjaxBlock();
            }
        }).ajaxStop(
        function () {
            $("#ajax-block").remove();
        });

function addAjaxBlock() {
    $("<div id='ajax-block' class='ajax-block' style='' />").appendTo("body").block({ message: null });
    $(".blockOverlay").addClass("blockNone");
}

$.ajaxSetup({
    "error": function (XMLHttpRequest, textStatus, errorThrown) {
        xhh = XMLHttpRequest;
        if (IsDebug) {
            console.log(textStatus);
            console.log(errorThrown);
            console.log(XMLHttpRequest.responseText);

            if (XMLHttpRequest.status === 403) {
                window.location.href = ExceptionPages.ExceptionPage403;
                return false;
            }
        }
        else {
            //window.location.href = ExceptionPage;
            RedirectExceptionPage(XMLHttpRequest.status);
        }
    }
});


; (function ($) {
    $(function () {

        callToolTips();
        IsIntegerValidate();
        callCalendars();
        IsNumericValidate();
        IsAlphabetValidate();
        IsDireccion();
        IsEmail();

        //IsURL();
        //SetShorten();
        //noSumbit();
        //

        //TrimForm();
        //
        //isAlphaNumericValidate();
        //isAlphabetValidate();

        //$(".accordionFilters").accordion({
        //    collapsible: true
        //});

        //$(document).on('show.bs.modal', '.modal', function () {
        //    console.log("k");
        //    var zIndex = 140 + (10 * $('.modal:visible').length);
        //    $(this).css('z-index', zIndex);
        //    setTimeout(function () {
        //        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        //    }, 0);
        //});

    });
})(jQuery);

var charBlock = ["<", ">", "=", "{", "}", "*"];

function fixFakePath(obj) {

    $('#' + obj).on('change', function () {

        var fileName = $(this).val();
        var bloqueo = false;
        var charMessage = "";

        for (var i in charBlock) {
            if (fileName.includes(charBlock[i])) {
                bloqueo = true;
                charMessage = charBlock[i];
                break;
            }
        }

        if (bloqueo) {
            showBlockUIError("El nombre del archivo contiene caracteres no permitidos: " + charMessage + "");
            $('#' + obj).val("");
            return false;
        }
        var texto = fileName.replace('fakepath', "").replace(/[/\\*]/g, "").replace("C:", "");

        $(this).next('.custom-file-label').html(texto);
    })
}

function RedirectExceptionPage(status) {

    if (status === 400) {
        window.location.href = ExceptionPages.ExceptionPage400;
        return false;
    }

    if (status === 403) {
        window.location.href = ExceptionPages.ExceptionPage403;
        return false;
    }

    if (status === 404) {
        window.location.href = ExceptionPages.ExceptionPage404;
        return false;
    }

    if (status === 500) {
        window.location.href = ExceptionPages.ExceptionPage500;
        return false;
    }
}

function callCalendars() {

    $(".IsCalendar").attr("data-inputmask", "'alias': 'date'");
    $(".IsCalendar").attr("maxlength", "10");
    $(".IsCalendar").inputmask();

    $(".IsCalendar").on("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(".IsCalendar").on("blur", function (e) {
        var valor = $.trim($(this).val());
        if (valor == "") {
            return false;
        }

        var _maxDate = $(this).attr("data-max-date") || "";
        if (_maxDate == "+0d") {
            
            var valid = moment(valor, 'DD-MM-YYYY', true).isValid();
            if (valid) {

                var ahora = moment();
                var campo = moment(valor, "DD-MM-YYYY");

                if (campo > ahora) {
                    showBlockUIError("Debe ingresar una fecha menor o igual a la actual");
                    $(this).val("");
                    return false;
                }

            } else {
                showBlockUIError("Debe ingresar una fecha válida");
                $(this).val("");
                return false;
            }
        }

        var old_date = $(this).attr("data-old-date") || "";
        if (old_date != "") {
            var v = old_date.toString().bool();
            if (!v) {
                var ahora = moment();
                var campo = moment(valor, "DD-MM-YYYY");
                if (campo < ahora) {
                    showBlockUIError("Debe ingresar una fecha mayor o igual a la actual");
                    $(this).val("");
                    return false;
                }
            }
        }

    });

    $(".IsCalendar").each(function () {

        var _yearRange = $(this).attr("data-year-range") || "-100:+1"; //yearRange: '1999:2012',
        var _maxDate = $(this).attr("data-max-date") || null;
        var old_date = $(this).attr("data-old-date") || "";

        $(this).datepicker({
            yearRange: _yearRange,
            maxDate: _maxDate,
            beforeShow: function (i, e) {
                var z = jQuery(i).closest(".ui-dialog").css("z-index") + 4;
                e.dpDiv.css('z-index', z);
            },
            onSelect: function (dateText, inst) {

                var old_date = $(this).attr("data-old-date") || "";
                if (old_date != "") {
                    var v = old_date.toString().bool();
                    if (!v) {
                        var ahora = moment();
                        var campo = moment(valor, "DD-MM-YYYY");
                        if (campo < ahora) {
                            showBlockUIError("Debe ingresar una fecha mayor o igual a la actual");
                            $(this).val("");
                            return false;
                        }
                    }
                }

                $(this).blur();
            }
        });

    });

    $(".x-icon-calendar").on('click', function () {
        var d = $(this).data("dateinput");

        var isDisabled = $("#" + d).is(":disabled");

        if (!isDisabled) {
            $("#" + d).focus()
        }
    });
}

function callCalendarPopup(form) {

    $("#" + form + " .IsCalendar").attr("data-inputmask", "'alias': 'date'");
    $("#" + form + " .IsCalendar").attr("maxlength", "10");
    $("#" + form + " .IsCalendar").inputmask();

    $("#" + form + " .IsCalendar").each(function () {

        var _yearRange = $(this).attr("data-year-range") || "-100:+1"; //yearRange: '1999:2012',
        var _maxDate = $(this).attr("data-max-date") || null;
        var old_date = $(this).attr("data-old-date") || "";

        $(this).datepicker({
            yearRange: _yearRange,
            maxDate: _maxDate,
            beforeShow: function (i, e) {
                var z = jQuery(i).closest(".ui-dialog").css("z-index") + 4;
                e.dpDiv.css('z-index', z);
            },
            onSelect: function (dateText, inst) {

                var old_date = $(this).attr("data-old-date") || "";
                if (old_date != "") {
                    var v = old_date.toString().bool();
                    if (!v) {
                        var ahora = moment();
                        var campo = moment(valor, "DD-MM-YYYY");
                        if (campo < ahora) {
                            showBlockUIError("Debe ingresar una fecha mayor o igual a la actual");
                            $(this).val("");
                            return false;
                        }
                    }
                }

                $(this).blur();
            }
        });

    });

    $("#" + form + " .x-icon-calendar").on('click', function () {
        var d = $(this).data("dateinput");
        var isDisabled = $("#" + d).is(":disabled");

        if (!isDisabled) {
            $("#" + d).focus()
        }
        //console.log(isDisabled, d);  
    });
}

function setCounter(obj, idCounter) {

    $(obj).simplyCountable({
        counter: $(obj).next(),
        countType: 'characters',
        maxCount: parseInt($(obj).attr("maxlength")),
        strictMax: false,
        countDirection: 'down',
        safeClass: 'safe',
        overClass: 'over',
        thousandSeparator: ',',
        onOverCount: function (count, countable, counter) { },
        onSafeCount: function (count, countable, counter) { },
        onMaxCount: function (count, countable, counter) { }
    });
}

function callToolTips() {

    $(".x-title-t").tooltip({ placement: 'top', trigger: 'hover', html: true });
    $(".x-title-l").tooltip({ placement: 'left', trigger: 'hover', html: true });
    //$(".x-title-r").tooltip({ placement: 'right', trigger: 'hover', html: true });

    //$(".x-title-t").tooltip({
    //    template: '<div class="tooltip" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>',
    //    delay: { "show": 5000, "hide": 1000 },
    //    html: true
    //});

    //$(".x-title-l").tooltip({ placement: 'left' });
}

function callToolTipsGrid(grid, place) {
    $("#" + grid + " .x-title-t").tooltip({ placement: 'top', trigger: 'hover', html: true, container: 'body' });
    $("#" + grid + " .x-title-l").tooltip({ placement: 'left', trigger: 'hover', html: true, container: 'body' });
}


function showBlockUI(callback, OkMessage) {

    var callback = callback || function () { };

    var html = "Operación realizada!";

    //if (getSiteLang() == 1) html = "Operation succeeded!";

    OkMessage = OkMessage || "";
    if (OkMessage != "") html = OkMessage;

    $.blockUI({
        message: "<h4 style='font-size:1.2rem'><i class='x-icon x-icon-accept fix-top-icon rel' style='top: 5px'></i> " + html + "</h4>",
        fadeIn: 500,
        fadeOut: 500,
        timeout: 1800,
        showOverlay: false,
        centerY: true,
        baseZ: 1051,
        css: {
            border: 'none',
            padding: '14px 5px',
            backgroundColor: '#000',
            '-webkit-border-radius': '8px',
            '-moz-border-radius': '8px',
            '-ms-border-radius': '8px',
            '-o-border-radius': '8px',
            'border-radius': '8px',
            'box-shadow': '0 2px 6px rgba(0, 0, 0, 0.2)',
            opacity: .7,
            color: '#fff'
        }
    });

    setTimeout(function () {
        $.unblockUI({
            onUnblock: function () {
                try {
                    callback();
                } catch (e) {

                }
            }
        });
    }, 1200);


}



function blockPageUI(message) {

    var msg = message || "";

    if (msg == "") {
        msg = "<br><i class='x-icon-loading'></i>";
    }
    else {
        msg = "<br><i class='x-icon-loading'></i><span class='ac rel' style='top: -5px;'>" + msg + "</span>";
    }

    $.blockUI({
        message: msg,
        baseZ: 1051,
        css: {
            width: '90px',
            opacity: .9,
            height: '75px',
            left: '45%',
            '-webkit-border-radius': '8px',
            '-moz-border-radius': '8px',
            '-ms-border-radius': '8px',
            '-o-border-radius': '8px',
            'border-radius': '8px',
            'box-shadow': '0 2px 6px rgba(0, 0, 0, 0.2)'
        }
    });
}

function blockPageUIclose() {
    $.unblockUI({ fadeOut: 0 });
}

function showBlockUIError(errorMessage, timeout) {

    var html = "Verifique campos obligatorios";
    var timeout = timeout || 2500;

    //if (getSiteLang() == 1) html = "Verify required fields";

    errorMessage = errorMessage || "";

    if (errorMessage != "") html = errorMessage;

    $.blockUI({
        message: "<h4 style='font-size:1.2rem'>" + html + "</h4>",
        fadeIn: 500,
        fadeOut: 500,
        timeout: timeout,
        showOverlay: false,
        centerY: true,
        baseZ: 1051,
        css: {
            border: '1px solid #A20000',
            padding: '10px',
            fontWeight: 'bold',
            backgroundColor: '#B94A48',
            '-webkit-border-radius': '5px',
            '-moz-border-radius': '5px',
            '-ms-border-radius': '5px',
            '-o-border-radius': '5px',
            'border-radius': '5px',
            opacity: .7,
            color: '#fff'
        }
    });
}


function xJsonObj(ajaxUrl, params, before, callback) {

    params = params || {};
    before = before || $.noop;
    callback = callback || $.noop;
    params['nocache'] = Math.random();

    $.ajax({
        type: "POST", cache: false,
        url: ajaxUrl,
        data: params,
        beforeSend: before,
        success: function (data) {
            xJSONResult = data;
            callback();
        },
        error: function (error) {
            if (IsDebug) {
                if (error.status === 403) {
                    window.location.href = Config.HomeSistemas;
                    return false;
                } else {
                    xAlert({
                        title: "Error",
                        alert: "alert-error",
                        html: "Ha ocurrido un error: " + error.statusText,
                        callback: function () { SelfRedirect(); }
                    });

                    console.log(error);
                    ajaxError = error;
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }

        }
    });
}

function getChildrenForSelect(ajaxUrl, objSend, ObjDestino, params, idProperty, txtProperty, callback) {

    var ObjDestinoEnd = $(ObjDestino);

    objSend.attr("disabled", true);
    ObjDestinoEnd.attr("disabled", true);

    $.ajax({
        type: 'POST', datatype: 'json', cache: false, url: ajaxUrl, data: params,
        success: function (data) {

            $("option[value!=''][value!='0']", ObjDestinoEnd).remove();

            //var lang = $.cookie('LanguageCk'); //Cambiar por valor de ViewBag.Lang

            $.each(data, function (i, item) {

                var text = $.trim(item[txtProperty]);
                var elID = item[idProperty];

                $(ObjDestinoEnd).append("<option value='" + elID + "' class='xdata'>" + text + "</option>")
            });

            objSend.attr("disabled", false);
            ObjDestinoEnd.attr("disabled", false);

            if ($.isFunction(callback)) {
                callback.apply();
            }
        },
        error: function (error) {
            if (IsDebug) {
                if (error.status === 403) {
                    window.location.href = Config.HomeSistemas;
                    return false;
                } else {

                    xAlert({
                        title: "Error",
                        alert: "alert-error",
                        html: "Ha ocurrido un error: " + error.statusText,
                        callback: function () { SelfRedirect(); }
                    });

                    console.log(error);
                    ajaxError = error;
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }

        }

    });
}

function xAjaxHtml(ajaxUrl, params, before, callback, metodo) {
    params = params || {};
    before = before || function () { };
    callback = callback || function () { };
    metodo = metodo || 'POST';

    params['nocache'] = Math.random();

    $.ajax({
        type: metodo,
        cache: false,
        url: ajaxUrl,
        data: params,
        beforeSend: before,
        success: function (data) {
            DataHTM = data;
            callback();
        },
        error: function (error) {

            if (IsDebug) {
                if (error.status === 403) {
                    window.location.href = Config.HomeSistemas;
                    return false;
                }
                else {
                    xAlert({
                        title: "Error",
                        alert: "alert-error",
                        html: "Ha ocurrido un error: " + error.statusText,
                        callback: function () { SelfRedirect(); }
                    });

                    console.log(error);
                    ajaxError = error;
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }
        }
    });
}

function CloseDialog() {
    $('.modal').modal('hide');
}
function DialogClose() {
    $('.modal').modal('hide');
}


var xAlertCallbackAction = false;

function xAlert(options) {

    //title, html, callback, btns, width, hideClose

    options.title = options.title || "Mensaje";
    options.txtBtn1 = options.txtBtn1 || Labels._aceptar;
    options.iconBtn1 = options.iconBtn1 || "ok";
    options.alert = options.alert || "alert-success"; //alert: "alert-success", "alert-secondary"
    options.onOpen = options.onOpen || $.noop;

    var html = '<div class="modalSiteAlert"><div class="modal fade" id="myModalAlert" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
              '<div class="modal-dialog modal-dialog-centered" role="document">' +
                '<div class="modal-content">' +
                  '<div class="modal-header ' + options.alert + '">' +
                    '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                    '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                      '<span aria-hidden="true">&times;</span>' +
                    '</button>' +
                  '</div>' +
                  '<div class="modal-body">' +
                    '<span>' + options.html + '</span>' + //class="' + options.cssMsg + '" style="' + options.styleMsg + '"
                  '</div>' +
                  '<div class="modal-footer">' +
                    '<button type="button" class="btn btn-default btnAceptar"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button>' +
                    '</div>' +
                '</div>' +
              '</div>' +
            '</div></div>';

    $('body').append(html);

    var $divModal = $("#myModalAlert");

    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })


    $divModal.on('shown.bs.modal', function (e) {

        options.onOpen.apply();

        var btnAceptar = $divModal.find(".btnAceptar");

        btnAceptar.on("click", function () {

            CloseDialog();

            if ($.isFunction(options.callback) && !xAlertCallbackAction) {
                options.callback.apply();
                xAlertCallbackAction = true;
            }
        })
    })

    //$divModal.on('hide.bs.modal', function (e) {

    //    console.log("hide.bs.modal");
    //    if ($.isFunction(options.callback)) {
    //        options.callback.apply();
    //    }

    //})

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $(".modalSiteAlert").remove();

        if ($.isFunction(options.callback) && !xAlertCallbackAction) {
            options.callback.apply();
            xAlertCallbackAction = true;
        }

        if (IsEdit) {
            SelfRedirect();
        }
        xAlertCallbackAction = false;

    })
}

function xAlert_gs(options) {

    //title, html, callback, btns, width, hideClose

    options.title = options.title || "Mensaje";
    options.txtBtn1 = options.txtBtn1 || Labels._aceptar;
    options.txtBtn2 = options.txtBtn2 || Labels._cancelar;
    options.iconBtn1 = options.iconBtn1 || "ok";
    options.alert = options.alert || "alert-success"; //alert: "alert-success", "alert-secondary"
    options.onOpen = options.onOpen || $.noop;

    var html = '<div class="modalSiteAlert"><div class="modal fade" id="myModalAlert" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
              '<div class="modal-dialog modal-dialog-centered" role="document">' +
                '<div class="modal-content">' +
                  '<div class="modal-header ' + options.alert + '">' +
                    '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                    '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                      '<span aria-hidden="true">&times;</span>' +
                    '</button>' +
                  '</div>' +
                  '<div class="modal-body">' +
                    '<span>' + options.html + '</span>' + //class="' + options.cssMsg + '" style="' + options.styleMsg + '"
                  '</div>' +
                  '<div class="modal-footer">' +
                    '<button type="button" class="btn btn-default btnAceptar"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button>' +
                    '<button class="btn btn-default btnCancelar" data-dismiss="modal" type="button"><i class="x-icon x-icon-cancel2 icon-in-button"></i> ' + options.txtBtn2 + '</button>' +
                    '</div>' +
                '</div>' +
              '</div>' +
            '</div></div>';

    $('body').append(html);

    var $divModal = $("#myModalAlert");

    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })


    $divModal.on('shown.bs.modal', function (e) {

        options.onOpen.apply();

        var btnAceptar = $divModal.find(".btnAceptar");
        
        var btnCancelar = $divModal.find(".btnCancelar");

        btnAceptar.on("click", function () {

            CloseDialog();

            if ($.isFunction(options.callback) && !xAlertCallbackAction) {
                CreateExpediente(true); 
                options.callback.apply();
                xAlertCallbackAction = true;
            }
        })

        btnCancelar.on("click", function () {

            CloseDialog();
            xAlertCallbackAction = false;

       
        })
    })

    //$divModal.on('hide.bs.modal', function (e) {

    //    console.log("hide.bs.modal");
    //    if ($.isFunction(options.callback)) {
    //        options.callback.apply();
    //    }

    //})

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $(".modalSiteAlert").remove();

        if ($.isFunction(options.callback) && !xAlertCallbackAction) {
            options.callback.apply();
            xAlertCallbackAction = true;
        }

        if (IsEdit) {
            SelfRedirect();
        }
        xAlertCallbackAction = false;

    })
}

//xDialogConfirmAction(title, msg1, msg2, url, params, callback)
function xDialogConfirmAction(options) {

    //width = width || 450;msg2
    //UrlAction = UrlAction || 0;
    //params = params || function () { };
    //callback = callback || SelfRedirect;
   
    options.txtBtn1 = options.txtBtn1 || Labels._aceptar;
    options.txtBtn2 = options.txtBtn2 || Labels._cancelar;
    options.iconBtn1 = options.iconBtn1 || "ok";//"accept6";
    options.alert = options.alert || "alert-primary";
    options.cssMsg = options.cssMsg || "";
    options.styleMsg = options.styleMsg || "";
    options.onOpen = options.onOpen || $.noop;
    options.id = options.id || "myModalConfirm";
    
    if (options.hasOwnProperty("closeInAccept")) {
        options.closeInAccept = options.closeInAccept;
    } else {
        options.closeInAccept = true;
    }

    if (options.hasOwnProperty("IsHideMessageEnd")) {
        options.IsHideMessageEnd = options.IsHideMessageEnd;
    } else {
        options.IsHideMessageEnd = false;
    }
    

    var html = '<div class="modalSiteConfirm"><div class="modal fade" id="' + options.id + '" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
                  '<div class="modal-dialog modal-dialog-centered" role="document">' +
                    '<div class="modal-content">' +
                      '<div class="modal-header ' + options.alert + '">' +
                        '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                          '<span aria-hidden="true">&times;</span>' +
                        '</button>' +
                      '</div>' +
                      '<div class="modal-body">' +
                        '<p>' + options.msg1 + '</p>' + //class="' + options.cssMsg + '" style="' + options.styleMsg + '"
                      '</div>' +
                      '<div class="modal-footer">' +
                        '<button type="button" class="btn btn-default btnAceptar"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button><i class="x-icon-loader hide"></i>' +
                        '<button class="btn btn-default" data-dismiss="modal" type="button"><i class="x-icon x-icon-cancel2 icon-in-button"></i> ' + options.txtBtn2 + '</button>' +
                      '</div>' +
                    '</div>' +
                  '</div>' +
                '</div></div>';

    $('body').append(html);

    var $divModal = $("#" + options.id);

    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })


    $divModal.on('shown.bs.modal', function (e) {

        options.onOpen.apply();

        if (options.replaceParrafo) {
            $divModal.find(".modal-body").html(options.msg1);
        }

        if (options.btn1Hide) {
            $divModal.find(".btnAceptar").remove();
        }
        else
        {
            var btnAceptar = $divModal.find(".btnAceptar");

            btnAceptar.on("click", function () {

                if (options.closeInAccept) {
                    CloseDialog();
                }

                if (options.url == null) {

                    if ($.isFunction(options.callback)) {
                        options.callback.apply();
                    }

                } else {

                    blockPageUI();

                    xJsonObj(options.url, options.params, function () {
                        //before
                    }, function () {

                        //callback

                        blockPageUIclose();

                        if (options.IsHideMessageEnd) {

                            if ($.isFunction(options.callback)) {
                                options.callback.apply();
                            }

                        } else {
                            showBlockUI(function () {
                                if ($.isFunction(options.callback)) {
                                    options.callback.apply();
                                }
                            });
                        }



                    })
                }
            })
        }


    })

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $(".modalSiteConfirm").remove();

        if (IsEdit) {
            SelfRedirect();
        }
    })

}

function xDialogConfirmAction3Options(options) {

    //width = width || 450;
    //UrlAction = UrlAction || 0;
    //params = params || function () { };
    //callback = callback || SelfRedirect;

    options.txtBtn1 = options.txtBtn1 || "Sí";
    options.txtBtn2 = options.txtBtn2 || "No";
    options.txtBtn3 = options.txtBtn3 || Labels._cancelar;
    options.iconBtn1 = options.iconBtn1 || "ok";
    options.iconBtn2 = options.iconBtn2 || "delete2";
    options.alert = options.alert || "alert-primary";
    options.cssMsg = options.cssMsg || "";
    options.styleMsg = options.styleMsg || "";
    options.onOpen = options.onOpen || $.noop;

    if (options.hasOwnProperty("closeInAccept")) {
        options.closeInAccept = options.closeInAccept;
    } else {
        options.closeInAccept = true;
    }


    var html = '<div class="modalSiteConfirm"><div class="modal fade" id="myModalConfirm" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
                  '<div class="modal-dialog modal-dialog-centered" role="document">' +
                    '<div class="modal-content">' +
                      '<div class="modal-header ' + options.alert + '">' +
                        '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                          '<span aria-hidden="true">&times;</span>' +
                        '</button>' +
                      '</div>' +
                      '<div class="modal-body">' +
                        '<p>' + options.msg1 + '</p>' + //class="' + options.cssMsg + '" style="' + options.styleMsg + '"
                      '</div>' +
                      '<div class="modal-footer">' +
                        '<button type="button" class="btn btn-default btnOpcion1"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button><i class="x-icon-loader hide"></i>' +
                        '<button type="button" class="btn btn-default btnOpcion2"><i class="x-icon x-icon-' + options.iconBtn2 + ' icon-in-button"></i> ' + options.txtBtn2 + '</button>' +
                        '<button class="btn btn-default" data-dismiss="modal" type="button"><i class="x-icon x-icon-cancel2 icon-in-button"></i> ' + options.txtBtn3 + '</button>' +
                      '</div>' +
                    '</div>' +
                  '</div>' +
                '</div></div>';

    $('body').append(html);

    var $divModal = $("#myModalConfirm");

    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })


    $divModal.on('shown.bs.modal', function (e) {

        options.onOpen.apply();

        if (options.replaceParrafo) {
            $divModal.find(".modal-body").html(options.msg1);
        }

        if (options.btn1Hide) {
            $divModal.find(".btnOpcion1").remove();
        }
        else {
            var btnSI = $divModal.find(".btnOpcion1");
            var btnNO = $divModal.find(".btnOpcion2");

            btnSI.on("click", function () {

                if (options.closeInAccept) {
                    CloseDialog();
                }

                if (options.url == null) {

                    if ($.isFunction(options.callback)) {
                        options.callback.apply();
                    }

                } else {

                    blockPageUI();

                    xJsonObj(options.url, options.params, function () {
                        //before
                    }, function () {

                        //callback

                        blockPageUIclose();

                        showBlockUI(function () {
                            if ($.isFunction(options.callback)) {
                                options.callback.apply();
                            }
                        });

                    })
                }
            })


            btnNO.on("click", function () {

                if (options.closeInAccept) {
                    CloseDialog();
                }

                if ($.isFunction(options.callback2)) {
                    options.callback2.apply();
                }

            });

        }


    })

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $(".modalSiteConfirm").remove();

        if (IsEdit) {
            SelfRedirect();
        }
    })

}


function GetModalContent(options) {
 
    //Small	.modal-sm	300px
    //Default	None	500px
    //Large	.modal-lg	800px
    //Extra large	.modal-xl	1140px

    options.alert = options.alert || "alert-primary";
    options.size = options.size || "";
    options.id = options.id || "xModal";
    options.xclass = options.xclass || "modalSite";

    var _static = options.static === true ? "static" : "";

    var top = options.top ? "setTop" : "";

    var html = '<div class="' + options.xclass + '" id="" ><div class="modal fade  ' + top + '" id="' + options.id + '" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
                  '<div class="modal-dialog modal-dialog-scrollable modal-dialog-centered ' + options.size + '" role="document">' +
                    '<div class="modal-content">' +
                      '<div class="modal-header  ' + options.alert + '">' +
                        '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">'+
                          '<span aria-hidden="true">&times;</span>'+
                        '</button>'+
                      '</div>'+
                      '<div class="modal-body ">'+
                        '<div class="d-flex justify-content-center"><div class="spinner-border text-secondary" role="status">' +
                          '<span class="sr-only">Cargando...</span>' +
                        '</div></div>' +
                      '</div>'+
                    '</div>'+
                  '</div>'+
                '</div></div>';

    $('body').append(html);

    var $divModal = $("#" + options.id);

    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })

    //responsive
    //$('#myModal').modal('handleUpdate')

    $divModal.on('shown.bs.modal', function (e) {

        if (options.replaceParrafo) {
            $divModal.find(".modal-body").html(options.html);
        }

        if (options.url == null) {

            if ($.isFunction(options.callback)) {
                options.callback.apply();
            }

        } else {

            xAjaxHtml(options.url, options.params,
                $.noop,
                function () {

                    $("#" + options.id).find(".modal-body").remove();

                    $("#" + options.id).find(".modal-content").append(DataHTM);

                    if ($.isFunction(options.callback)) {
                        options.callback.apply();
                    }
                }
            );
        }
    })

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $("." + options.xclass).remove();

        if (IsEdit) {
            SelfRedirect();
        }
    })
}

function GetModalContentShowPdf(options, pDocumentoCausaID, pCausaID, pHash, pTipoDoc, l_expedienteID, l_usuarioID, iTipoTramite) {

    //width = width || 450;msg2
    //UrlAction = UrlAction || 0;
    //params = params || function () { };
    //callback = callback || SelfRedirect;

    var cadenaADividir = getValidaSiDocumentoEstaTomado(l_expedienteID, l_usuarioID, iTipoTramite);
    var arrayDeCadenasValidaMuestraBotonFirma = cadenaADividir.split('|');
    var oValidaMuestraBotonFirma = arrayDeCadenasValidaMuestraBotonFirma[0] == '1';
    var smensajeRetorna = arrayDeCadenasValidaMuestraBotonFirma[1];

    options.txtBtn1 = options.txtBtn1 || Labels._aceptar;
    options.txtBtn2 = options.txtBtn2 || Labels._cancelar;
    options.iconBtn1 = options.iconBtn1 || "ok";//"accept6";
    options.alert = options.alert || "alert-primary";
    options.cssMsg = options.cssMsg || "";
    options.styleMsg = options.styleMsg || "";
    options.onOpen = options.onOpen || $.noop;
    options.id = options.id || "myModalConfirm";
    options.xclass = options.xclass || "modalSite";

    if (options.hasOwnProperty("closeInAccept")) {
        options.closeInAccept = options.closeInAccept;
    } else {
        options.closeInAccept = true;
    }

    if (options.hasOwnProperty("IsHideMessageEnd")) {
        options.IsHideMessageEnd = options.IsHideMessageEnd;
    } else {
        options.IsHideMessageEnd = false;
    }
    var sHtmlButonFirma = oValidaMuestraBotonFirma == true ? '<button type="button" class="btn btn-default btnAceptar"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button><i class="x-icon-loader hide"></i>' : '';
    var sMensaje1 = oValidaMuestraBotonFirma == true ? options.msg1 : smensajeRetorna;
    var oUrlDocPdf = "";
    oUrlDocPdf = getDownloadFileUrl(pDocumentoCausaID, pCausaID, pHash, pTipoDoc);
    //console.log(oUrlDocPdf);
    var html = "";
    html = '<div class="' + options.xclass + '" id="' + pDocumentoCausaID + '" ><div class="modal fade  ' + top + '" id="' + options.id + '" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">' +
                  '<div class="modal-dialog modal-dialog-scrollable modal-dialog-centered ' + options.size + '" role="document">' +
                    '<div class="modal-content">' +
                      '<div class="modal-header  ' + options.alert + '">' +
                        '<h5 class="modal-title" id="modalLabel">' + options.title + '</h5>' +
                        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
                          '<span aria-hidden="true">&times;</span>' +
                        '</button>' +
                      '</div>' +
                      '<div class="modal-body">' +
                       // '<p>' + options.msg1 + '</p>' + //class="' + options.cssMsg + '" style="' + options.styleMsg + '"
                        '<p>' + sMensaje1 + '</p>' +
                        '<p>' +
                           '<iframe id="htmlPdf" name="nPdf" src="' + oUrlDocPdf + '" style="width: 100%; height: 450px;"></iframe>' +
                        '</p>' +

                     '</div>' +
                      '<div class="modal-footer">' +
                         sHtmlButonFirma
                        //'<button type="button" class="btn btn-default btnAceptar"><i class="x-icon x-icon-' + options.iconBtn1 + ' icon-in-button"></i> ' + options.txtBtn1 + '</button><i class="x-icon-loader hide"></i>'
                        +
                        '<button class="btn btn-default" data-dismiss="modal" type="button"><i class="x-icon x-icon-cancel2 icon-in-button"></i> ' + options.txtBtn2 + '</button>' +
                      '</div>' +
                    '</div>' +
                  '</div>' +
                '</div></div>';
    $('body').append(html);
    $('#htmlPdf').attr('src', oUrlDocPdf)
    var $divModal = $("#" + options.id);
    
    $divModal.modal({
        keyboard: false,
        backdrop: 'static'
    })


    $divModal.on('shown.bs.modal', function (e) {

        options.onOpen.apply();

        if (options.replaceParrafo) {
            $divModal.find(".modal-body").html(options.msg1);
        }

        if (options.btn1Hide) {
            $divModal.find(".btnAceptar").remove();
        }
        else {
            var btnAceptar = $divModal.find(".btnAceptar");

            btnAceptar.on("click", function () {

                if (options.closeInAccept) {
                    CloseDialog();
                }

                if (options.url == null) {

                    if ($.isFunction(options.callback)) {
                        options.callback.apply();
                    }

                } else {

                    blockPageUI();

                    xJsonObj(options.url, options.params, function () {
                        //before
                    }, function () {

                        //callback

                        blockPageUIclose();

                        if (options.IsHideMessageEnd) {

                            if ($.isFunction(options.callback)) {
                                options.callback.apply();
                            }

                        } else {
                            showBlockUI(function () {
                                if ($.isFunction(options.callback)) {
                                    options.callback.apply();
                                }
                            });
                        }



                    })
                }
            })
        }


    })

    $divModal.on('hidden.bs.modal', function (e) {
        // do something...
        //console.log("hidden.bs.modal");

        $(".modalSiteConfirm").remove();

        if (IsEdit) {
            SelfRedirect();
        }
    })
}


function IsFechaValidaForm(obj) {
    var campo = $(obj).val();

    return moment(campo, 'DD-MM-YYYY', true).isValid();
}

function validateForm($form, options) {

    var cuenta = 0;

    //
    //
    //is-valid

    $form.find(".verificar:enabled:visible").each(function () {

        //:not([readonly='readonly'])
        //not([disabled='disabled'])

        var valor = $.trim($(this).val());
        var IsCalendar = $(this).hasClass("IsCalendar");
        var IsRadio = $(this).is(':radio');
        var IsSelect = $(this).hasClass("custom-select");
        var IsTextarea = $(this).is("textarea");
        var IsNotCero = $(this).hasClass("IsNotCero");

        if (IsRadio) {

            if ($(this).hasClass("is-invalid")) {
                $(this).removeClass("is-invalid");
            }
            //if (valor == "") {
            //    $(this).addClass("is-invalid");
            //    cuenta++;
            //}

        } else if (IsCalendar) {

            var novalid = false;

            if (valor == "") {
                $(this).addClass("is-invalid");
                novalid = true;
                cuenta++;
            } else {

                if (IsFechaValidaForm($(this))) {

                } else {
                    $(this).addClass("is-invalid");
                    novalid = true;
                    cuenta++;
                }

                //$(this).removeClass("is-invalid").addClass("is-valid");
            }

            if (novalid) {
                $(this).blur(function () {
                    if (IsFechaValidaForm($(this))) {
                        $(this).removeClass("is-invalid").addClass("is-valid");
                        //console.log("ok");
                    } else {
                        $(this).removeClass("is-valid").addClass("is-invalid");
                        //console.log("no");
                    }
                });
            }

        } else if (IsSelect) {

            var defecto = $.trim($(this).data("default")) || "";

            if (valor == defecto || valor == "") {
                $(this).addClass("is-invalid");
                cuenta++;

                SetActionValid_Select($(this));
            }


        } else if (IsNotCero) {

            if (valor == "") {
                $(this).addClass("is-invalid");
                cuenta++;

                $(this).blur(function () {
                    if ($.trim($(this).val()) != "") {
                        $(this).removeClass("is-invalid").addClass("is-valid");
                    } else {
                        $(this).removeClass("is-valid").addClass("is-invalid");
                    }
                })
            }
            else
            {
                if (parseInt(valor) == 0) {
                    $(this).addClass("is-invalid");
                    cuenta++;

                    $(this).blur(function () {
                        if (parseInt($(this).val()) > 0) {
                            $(this).removeClass("is-invalid").addClass("is-valid");
                        } else {
                            $(this).removeClass("is-valid").addClass("is-invalid");
                        }
                    })
                }
            }
        
        } else {

            if (valor == "") {
                $(this).addClass("is-invalid");
                cuenta++;
                
                $(this).blur(function () {
                    if ($.trim($(this).val()) != "") {
                        $(this).removeClass("is-invalid").addClass("is-valid");
                    } else {
                        $(this).removeClass("is-valid").addClass("is-invalid");
                    }
                })
            }
        }

        //if (IsCalendar) {

            //if (valor == "") {
            //    $(this).parent().parent().addClass("has-error");
            //    cuenta++;
            //} else {
            //    $(this).parent().parent().removeClass("has-error");
            //}

       // } else {



      //  }

    });

    if (cuenta > 0) {
        showBlockUIError();

        //$form.addClass("was-validated");


        try {
            if (!$(".modal-body").is(":visible")) {

                scrollToElement($(".is-invalid:visible").eq(0));
                //console.log("scrioll");
            }
            
        } catch (e) {

        }

        return false;
    }

    return true;
}

function SetActionValid_Select(obj) {
    $(obj).change(function () {

        var defecto = $.trim($(this).data("default")) || "";

        //console.log(defecto);

        if ($.trim($(obj).val()) != defecto) {
            $(obj).removeClass("is-invalid").addClass("is-valid");
        } else {
            $(obj).removeClass("is-valid").addClass("is-invalid");
        }

        //if ($.trim($(obj).val()) != "") {
        //    $(obj).removeClass("is-invalid").addClass("is-valid");
        //} else {
        //    $(obj).removeClass("is-valid").addClass("is-invalid");
        //}
    })
}

function activateForm(btn, frm, beforeSerialize, Success, validateFalse) {

    var btnSave = $("#" + btn);

    var validateFalse = validateFalse || function () { };

    var options = {
        type: 'post',
        iframe: iFrameGlobal,
        success: function (data) {
            //xUIBtnAjax(btnSave, 0);
            BtnInAction(btnSave, true, true);

            xResultJsonData = data;

            if ($.isFunction(Success)) {
                Success.apply();
            }
        },
        beforeSubmit: function (arr, $form, options) {
            //xUIBtnAjax(btnSave, 1);
            BtnInAction(btnSave, false, true);

            var n = _.where(arr, { name: "__RequestVerificationToken" }).length

            if (n === 0) {
                var opt = { name: "__RequestVerificationToken", type: "hidden", value: getVToken() };
                arr.push(opt);
            }


        },
        beforeSerialize: function ($form, options) {

            if (validateForm($form)) {

                if ($.isFunction(beforeSerialize)) {
                    return beforeSerialize.apply();
                }

                return true;
            }

            if ($.isFunction(validateFalse)) {
                validateFalse.apply();
                return false;
            }

            return false;
        },
        error: function (error) {
            if (IsDebug) {
                if (error.status === 403) {
                    //window.location.href = HomeSistemas;
                    //return false;
                } else {
                    xAlert({
                        title: "Error",
                        alert: "alert-error",
                        html: "Ha ocurrido un error: " + error.statusText,
                        callback: function () { SelfRedirect(); }
                    });

                    console.log(error);
                    ajaxError = error;
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }
        }
    };

    $("#" + frm).ajaxForm(options);

}

function divLoaderHtml(showInfo, txtInfo) {

    txtInfo = txtInfo || "Cargando...";

    if (!showInfo) {
        txtInfo = "";
    }

    var html = '<div class="ac"><br><div class="spinner-border text-secondary" role="status">' +
                  '<span class="sr-only">Loading...</span>' +
                '</div></div>';

    //var html = "<br><br><div class='ac'><i class='x-icon-loader'></i> " + txtInfo + "</div>";

    return html;

}

var _ReglaSolicitud = function (_Dias, _ClasificacionID, IsDenunciaDerivada, Status1, Status2) {
    this.Dias = _Dias;
    this.IsOIRS = (_ClasificacionID == 1);

    this.isGreen = function () {
        if (IsDenunciaDerivada) {
            return (this.Dias <= 110);
        }
        else {
            if (this.IsOIRS) {
                return (this.Dias <= Status1);
            }
            else {
                return (this.Dias <= 7);
            }
        }
    };

    this.isYellow = function () {
        if (IsDenunciaDerivada) {
            return (this.Dias > 110 && this.Dias <= 140);
        } else {
            if (this.IsOIRS) {
                return (this.Dias > Status1 && this.Dias <= Status2);
            }
            else {
                return (this.Dias > 7 && this.Dias <= 12);
            }
        }

    };

    this.isRed = function () {
        if (IsDenunciaDerivada) {
            return (this.Dias > 140)
        } else {
            if (this.IsOIRS) {
                return (this.Dias > Status2);
            }
            else {
                return (this.Dias > 12);
            }
        }
    };
}

function noSumbit() {

    $('.noSumbit').on("keyup keypress", function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            e.preventDefault();
            return false;
        }
    });

    $('.noSumbit').on("submit", function (e) {

        e.preventDefault();
        return false;

    });
}

function getDiasTranscurridos(Dias, FechaRespuesta, ClasificacionID, IsDenuncia, IsDenunciaDerivada, PlazoDias, Status1, Status2) {
    var strDias = (Dias > 1) ? "días" : "día";
    var isRespondida = ($.trim(FechaRespuesta) != "");
    var isOIRS = (ClasificacionID == 1);
    var color = "rgb(1, 186, 1)";
    var alarmaRoja = "";
    var alarma = "";
    var html = "";

    var regla = new _ReglaSolicitud(Dias, ClasificacionID, IsDenunciaDerivada, Status1, Status2);

    if (regla.isGreen()) {
        color = "rgb(1, 186, 1)"
        //alarma = "<br><i class='x-alerta-yellow'></i>";
        //console.log("regla.isGreen() -> ", regla.isGreen());
       
        if (isRespondida) alarma = "<br><i class='x-icon x-icon-email1'></i>";
    }

    if (regla.isYellow()) {
        color = "rgb(199, 197, 7)";
        if (!isRespondida) alarma = "<br><i class='x-alerta-yellow'></i>";
        if (isRespondida) alarma = "<br><i class='x-icon x-icon-email1'></i>";
    }

    if (regla.isRed()) {
        color = "red";
        if (!isRespondida) alarma = "<br><i class='x-alerta-red'></i>";
        if (isRespondida) alarma = "<br><i class='x-icon x-icon-email2'></i>";
        //console.log("regla.isRed() -> ", regla.isRed());
        //console.log(isRespondida);
    }

    if (IsDenuncia || IsDenunciaDerivada) {
        html = "<span style='color: " + color + "'>" + Dias + " de " + PlazoDias + " </span>" + alarma;
    } else {
        html = "<span style='color: " + color + "'>" + Dias + " " + strDias + "</span>" + alarma;
    }


    return html;
}

//enter
function getEnterTextBox(obj, callback) {

    var callback = callback || $.noop;

    $(obj).on("keypress", function (e) {
        if (e.keyCode == 13) {
            callback();
            return false;
        }
    });

}

function GetIconAction(lista) {

    var html = "";

    $.each(lista, function (index, valor) {

        //Ver Solicitud|/ES/SIAC/OIRS/57187/2|javascript:;| -zoom1

        var item = valor.split("|");

        if (item[0] != "") {

            var icon = "x-icon x-icon" + $.trim(item[3]);

            var f = "<a class='x-title-t rel ' title='" + item[0] + "' data-html='true' style='margin-right:3px' " +
                     "href='" + item[1] + "' data-txt=''  onclick='" + item[2] + "'>" +
                     "<i class='" + icon + "'></i>" +
                     "</a>";

            html = html + f;

        }



    });

    return html;
}

function setIsView() {

    //console.log("setIsView");

    $("input:visible, select:visible, textarea:visible").not(".notBlock").attr('disabled', 'disabled');
    $(":checkbox:visible").attr('disabled', 'disabled');
    $("#btnEnviar, #btnBuscarFuncionario").remove();

    $(".x-icon-clean").not(".notRemove").remove();

    $(".x-icon-delete").not(".notRemove").remove();

    $("#frmFileSolicitud").remove();

    $("#frmFileSolicitudDenuncia input").enable();

}

function activateFormUploadFile(parms)
{
    var form = $("#" + parms.formID);

    $(form).ajaxForm({
        iframe: iFrameGlobal,
        dataType: "json",
        beforeSend: function () { $(form).find("#percentVal").text('0%'); },
        uploadProgress: function (event, position, total, percentComplete) { $(form).find("#percentVal").text(percentComplete + '%'); },
        beforeSubmit: function (arr, $form, options) {
            var txt = "<h4>" + Labels._messageCargando + " <span id='percentVal'></span></h4>";
            $(form).block({ message: txt });

            var n = _.where(arr, { name: "__RequestVerificationToken" }).length

            if (n === 0) {
                var opt = { name: "__RequestVerificationToken", type: "hidden", value: getVToken() };
                arr.push(opt);
            }
        },
        success: function (result) {
            $(form).find("#percentVal").text('100%');
            $(form).unblock();

            if ($.isFunction(parms.callback)) {
                parms.callback.apply();
            }
        }
    });
}

function activateFormUpload_New(formID, callback, validaDesc, IsParent, validateSerialize, ReloadPage, showBlock, validateFile) {

    callback = callback || function () { };
    validaDesc = validaDesc || false;
    IsParent = IsParent || false;
    validateSerialize = validateSerialize || function () { return true; };

    var form = $("#" + formID);

    $(form).ajaxForm({
        iframe: iFrameGlobal,
        dataType: "json",
        beforeSend: function () { $(form).find("#percentVal").text('0%'); },
        uploadProgress: function (event, position, total, percentComplete) { $(form).find("#percentVal").text(percentComplete + '%'); },
        beforeSubmit: function (arr, $form, options) {
            var txt = "<h4>" + Labels._messageCargando + " <span id='percentVal'></span></h4>";
            $(form).block({ message: txt });

            var n = _.where(arr, { name: "__RequestVerificationToken" }).length

            if (n === 0) {
                var opt = { name: "__RequestVerificationToken", type: "hidden", value: getVToken() };
                arr.push(opt);
            }
        },
        beforeSerialize: function ($form, options) {

            xTmp = $form;

            if (validaDesc) {
                var f2 = $.trim($form.find("#Descripcion").val());
                if (f2 == "") {
                    showBlockUIError(Labels._messageErrorDescripcion);
                    return false;
                }
            }

            if (validateFile) {

                var f1 = $form.find("input[name='file']").val();
                if (f1 == "") {
                    showBlockUIError(Labels._messageErrorSelect);
                    return false;
                }
            }

            if ($.isFunction(validateSerialize)) {
                return validateSerialize.apply();
            }

            return true;
        },
        success: function (result) {

            xJSONResult = result;

            //console.log("ReloadPage => " + ReloadPage);

            $(form).find("#percentVal").text('100%');

            $(form).unblock();
            var r = parseInt(result.Error);

            if (r === 1) {
                if (IsParent) {
                    parent.xAlert(Labels._messageErrorCarga, result.Desc, function () {
                        if (ReloadPage) {
                            blockPageUI();
                            SelfRedirect();
                        }
                    });
                } else {

                    xAlert({
                        title: Labels._messageErrorCarga,
                        alert: "alert-error",
                        html: result.Desc,
                        callback: function () {
                            if (ReloadPage) {
                                SelfRedirect();
                            }

  
                        }
                    });

                    //xAlert(Labels._messageErrorCarga, result.Desc, function () {
                    //    if (ReloadPage) {
                    //        blockPageUI();
                    //        SelfRedirect();
                    //    }
                    //});
                }

                return false;
            }

            if (r === 0) {

                //if ($.isFunction(callback)) {
                    
                //    console.log("callback.apply");
                //}

                callback.apply();
            }

            if (showBlock) {
                showBlockUI(SelfRedirect);
                //console.log("okssss");
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            if (IsDebug) {
                if (xhr.status === 403) {
                    window.location.href = Config.HomeSistemas;
                    return false;
                } else {
                    if (IsParent) {
                        parent.xAlert("Error", Labels._messageErrorCarga);
                    } else {
                        xAlert("Error", Labels._messageErrorCarga);
                    }

                    $(form).unblock();
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }
        }
    });
}

function activateFormUploadTemp(formID, callback, validateSerialize) {

    callback = callback || function () { };
    validateSerialize = validateSerialize || function () { return true; };

    var form = $("#" + formID);

    $(form).ajaxForm({
        iframe: iFrameGlobal,
        dataType: "json",
        beforeSend: function () {
            //$(form).find("#percentVal").text('0%');
        },
        uploadProgress: function (event, position, total, percentComplete) {
            //$(form).find("#percentVal").text(percentComplete + '%');
        },
        beforeSubmit: function (arr, $form, options) {
            //var txt = "<h4>" + Labels._messageCargando + " <span id='percentVal'></span></h4>";
            //$(form).block({ message: txt });
        },
        beforeSerialize: function ($form, options) {

            xTmp = $form;

            if ($.isFunction(validateSerialize)) {
                return validateSerialize.apply();
            }

            return true;
        },
        success: function (result) {

            xJSONResult = result;

            $(form).find("#percentVal").text('100%');

            //$(form).unblock();
            //var r = parseInt(result.Error);

            if ($.isFunction(callback)) {
                callback.apply();
            }

            if (xJSONResult.Error === 0) {
                $(form).reset();

                $populated = $(form).find(".customfile-feedback-populated");
                $populated.removeClass("customfile-feedback-populated");
                $populated.text("");

                $populated.removeClass(function (index, className) {
                    return (className.match(/(^|\s)customfile-ext-\S+/g) || []).join(' ');
                });
            }


        },
        error: function (xhr, textStatus, errorThrown) {
            if (IsDebug) {
                if (xhr.status === 403) {
                    window.location.href = Config.HomeSistemas;
                    return false;
                } else {
                    if (IsParent) {
                        parent.xAlert("Error", Labels._messageErrorCarga);
                    } else {
                        xAlert("Error", Labels._messageErrorCarga);
                    }

                    $(form).unblock();
                }
            }
            else {
                RedirectExceptionPage(error.status);
            }
        }
    });
}

function DerivarInternamente(SolicitudID, PerfilID, DiasTranscurridos) {

    var opt = {
        title: "Derivar a otra Unidad",
        url: "/ES/SIAC/GetDerivar",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            PerfilID: PerfilID,
            DiasTranscurridos: DiasTranscurridos
        },
        size: 'modal-xl',
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);

    //getDivAjaxHtml(URL, , params, callback, 900)
}

function VerHistorial(SolicitudID, PerfilID) {
    var SRC = "/ES/SIAC/HistorialSolicitud?SolicitudID=" + SolicitudID + "&PerfilID=" + PerfilID;
    var title = "Historial";

    $("#frmHistorial #historialSolicitudID").val(SolicitudID);
    $("#frmHistorial #historialPerfilID").val(PerfilID);

    //$("#frmHistorial").attr("action", SRC);

    //console.log(SRC);

    $("#frmHistorial").submit();

    //var fnGuardar = $.noop();
    //var fnCancelar = parent.DialogClose();
    //var onClose = $.noop();

    //OpenDialogIframeBtns(SRC, title, onClose, 880, 530, 40);
}

function ResponderSolicitud(SolicitudID, PerfilID, Title) {

    //var URL = '/ES/SIAC/GetRespuestaSolicitud';
    //var params = {
    //    __RequestVerificationToken: getVToken(),
    //    SolicitudID: SolicitudID,
    //    PerfilID: PerfilID,
    //    Title: Title
    //};

    var opt = {
        title: Title,
        url: '/ES/SIAC/GetRespuestaSolicitud',
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            PerfilID: PerfilID,
            Title: Title
        },
        size: 'modal-xl',
        top: true,
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);

    //GetDivAjaxHtml(URL, Title, params, callback, 1000);
}

function getDeleteFileTemp(ObjID1, TipoDoc, VersionEnc, Hash, ObjID2, remove) {

    $(remove).parents("tr").remove();

    var Action = "/ES/Uploader/DeleteFile";
    var params = { __RequestVerificationToken: getVToken(), ObjID1: ObjID1, TipoDoc: TipoDoc, VersionEnc: VersionEnc, Hash: Hash, ObjID2: ObjID2 };
    var before = function () { };
    var callback = function () {
        enumerateItems('enumerate');
        $('*').tooltip('hide');
    };

    xJsonObj(Action, params, before, callback);

    //ObjID2 = ObjID2 || 0;

    //var Action = "/ES/Uploader/DeleteFile";
    //var params = { __RequestVerificationToken: getVToken(), ObjID1: ObjID1, TipoDoc: TipoDoc, VersionEnc: VersionEnc, Hash: Hash, ObjID2: ObjID2 };
    //var callback = function () { };

    //var msg1 = "¿Confirma <b>eliminar</b> el archivo?";
    //var msg2 = "El archivo ha sido eliminado";
    //var titleDiv = "Eliminar archivo";

    //var div = jQuery('<div/>', {
    //    title: titleDiv,
    //    "class": "alertTMP",
    //    html: "<br><p class='ac'>" + msg1 + "</p>"
    //}).appendTo('body');

    //$(div).dialog({
    //    modal: true,
    //    resizable: false,
    //    closeOnEscape: false,
    //    width: 450, height: 160,
    //    autoOpen: false,
    //    buttons: {
    //        "Aceptar": function () {

    //            $(this).dialog("close");

    //            blockPageUI();

    //            xAjax(Action, params, function () {
    //                blockPageUIclose();
    //                xAlert(titleDiv, msg2);

    //                $(remove).parents("tr").remove();

    //            });
    //        },
    //        "Cancelar": function () {
    //            $(this).dialog("close");
    //        }
    //    },
    //    open: function () {
    //        $('.ui-dialog-buttonpane').find('button').addClass("g-button").css("line-height", "0");
    //        $('.xAlert').css("text-align", "center").css("margin-top", "15px").css("min-height", "30px");
    //    },
    //    close: function () {
    //        $(div).remove();
    //    }
    //});

    //$(div).dialog('open');

    //xDialogConfirmActionAjax("Eliminar Archivo", msg1, msg2, Action, params);
}


function Admisibilidad(SolicitudID, ClasificacionID) {
    //var SRC = "/ES/SIAC/GetAdmisibilidad?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    //var title = "Admisible";

    //var fnGuardar = $.noop();
    //var fnCancelar = parent.DialogClose();
    //var onClose = $.noop();

    //var heigth = 380;
    //if (ClasificacionID == 2) heigth = 530;

    //OpenDialogIframe(SRC, title, onClose, 500, heigth, 40);

    var opt = {
        title: "Admisibilidad",
        url: "/ES/SIAC/GetAdmisibilidad",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        //top: false,
        size: 'modal-lg',
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);
}

function ContactoCiudadano(SolicitudID, ClasificacionID) {
    //var SRC = "/ES/SIAC/GetContactoCiudadano?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    //var title = "Contactar Ciudadano";

    //var fnGuardar = $.noop();
    //var fnCancelar = parent.DialogClose();
    //var onClose = $.noop();

    //var heigth = 230;
    //if (ClasificacionID == 2) heigth = 330;

    //OpenDialogIframe(SRC, title, onClose, 600, heigth, 40);

    var opt = {
        title: "Contactar Ciudadano",
        url: "/ES/SIAC/GetContactoCiudadano",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        //top: false,
        size: 'modal-lg',
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);
}

function RespuestaContactoCiudadano(SolicitudID, ClasificacionID) {
    //var SRC = "/ES/SIAC/GetRespuestaContactoCiudadano?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    //var title = "Respuesta Ciudadano";

    //var fnGuardar = $.noop();
    //var fnCancelar = parent.DialogClose();
    //var onClose = $.noop();

    //OpenDialogIframe(SRC, title, onClose, 500, 240, 40);

    var opt = {
        title: "Respuesta Ciudadano",
        url: "/ES/SIAC/GetRespuestaContactoCiudadano",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        id: "modalRespuesta",
        size: 'modal-lg',
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);
}

function VerHistorialDocumento(SolicitudID, DocumentoRespuestaID) {
    //var URL = "/ES/SIAC/GetHistorialDocumento";
    //var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID, DocumentoRespuestaID: DocumentoRespuestaID };
    //var callback = function () {
    //    addTHBG();
    //    callToolTips();
    //};

    //getDivAjaxHtml(URL, "Versiones de Documento", params, callback, 750, 0, ['10%', 200]);

    var opt = {
        title: "Versiones de Documento",
        url: "/ES/SIAC/GetHistorialDocumento",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            DocumentoRespuestaID: DocumentoRespuestaID
        },
        id: "modalHistorial",
        static: true,
        xclass: "modelChild",
        size: 'modal-lg',
        callback: function () {
            callToolTips();
        }
    }

    GetModalContent(opt);
}


function CambioClasificacion(IsPendienteCambio, SolicitudID, ClasificacionID, IsOIRS, NumeroTicket) {


    var action = (IsPendienteCambio) ? "GetAutorizaCambioClasificacion" : "GetSolicitaCambioClasificacion";
    var URL = "/ES/SIAC/" + action;
    var Title = (IsOIRS) ? "Cambio Clasificación: OIRS a Transparencia" : "Cambio Clasificación: Transparencia a OIRS";

    var clasificacion = (IsOIRS) ? "Transparencia" : "OIRS";

    var msgConfirma = (IsPendienteCambio) ?
                      "¿Confirma autorizar el cambio de clasificación a <b>" + clasificacion + "</b>?" :
					  "¿Solicita autorización cambiar la solicitud <br>" + NumeroTicket + " a <b>" + clasificacion + "</b>?";

    var optBtn = {
        title: Title,
        msg1: "<p class='ac'>" + msgConfirma + "</p>",
        url: null,
        callback: function () {

            xJsonObj(URL, {
                __RequestVerificationToken: getVToken(),
                SolicitudID: SolicitudID,
                ClasificacionID: ClasificacionID,
                Option: 1
            }, function () {
                blockPageUI();
            }, function () {

                blockPageUIclose();

                CloseDialog();

                xAlert({
                    title: Title,
                    html: "Operación realizada",
                    callback: function () {
                        goGrid();
                    }
                })

            })

        },
        callback2: function () {
            xJsonObj(URL, {
                __RequestVerificationToken: getVToken(),
                SolicitudID: SolicitudID,
                ClasificacionID: ClasificacionID,
                Option: 2
            }, function () {
                blockPageUI();
            }, function () {

                blockPageUIclose();

                CloseDialog();

                xAlert({
                    title: Title,
                    html: "Operación realizada",
                    callback: function () {
                        goGrid();
                    }
                })

            })
        },
        replaceParrafo: true
    }

    if (IsPendienteCambio) {
        xDialogConfirmAction3Options(optBtn);
    } else {
        xDialogConfirmAction(optBtn);
    }

}

function CambioClasificacion__(IsCoordinador, SolicitudID, ClasificacionID, IsOIRS, NumeroTicket) {
    var IsOIRS = IsOIRS.toString().bool();
    var IsCoordinador = IsCoordinador.toString().bool();
    var title = (IsOIRS) ? "Cambio Clasificación: OIRS a Transparencia" : "Cambio Clasificación: Transparencia a OIRS";
    var clasificacion = (IsOIRS) ? "Transparencia" : "OIRS";

    var msgConfirma = (IsCoordinador) ?
					  "¿Solicita autorización cambiar la solicitud " + NumeroTicket + " a <b>" + clasificacion + "?" :
					  "¿Confirma autorizar el cambio de clasificación a <b>" + clasificacion + "</b>?";

    var div = jQuery('<div/>', {
        id: "_CambioClasificacion",
        title: title,
        "class": "Clasificacion",
        style: "display:none",
        html: "<p class='ac' style='margin: 19px auto 4px !important'>" + msgConfirma + "</p>"
    }).appendTo('body');


    $(div).dialog({
        modal: true,
        resizable: false,
        closeOnEscape: false,
        width: 600,
        buttons: {
            "Sí": function () {
                $(div).dialog("close");

                var action = (IsCoordinador) ? "GetSolicitaCambioClasificacion" : "GetAutorizaCambioClasificacion";
                var URL = "/ES/SIAC/" + action;
                var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID, ClasificacionID: ClasificacionID };
                var callback = function () {
                    blockPageUIclose();

                    if (IsCoordinador) {
                        xAlert(title, "Se ha solicitado autorización");
                        goGrid();
                    }
                    else {
                        xAlert(title, "Se ha autorizado el Cambio de Clasificación para la Solicitud N° <b>" + NumeroTicket + "</b>");
                        goGrid();
                    }
                };

                blockPageUI();

                xAjax(URL, params, callback)

            },
            'No': function () {
                $(div).dialog("close");
            }
        },
        open: function () {
            $('.ui-dialog-buttonpane').find('button').addClass("g-button").css("width", "110px");
        },
        close: function () {
            $(div).remove();
        }
    });

}

function EliminarSolicitud(SolicitudID, ClasificacionID) {

    var opt = {
        title: "Retractar Solicitud",
        url: "/ES/SIAC/GetEliminarSolicitud",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        id: "modalRetractar",
        static: true,
        xclass: "modelChild",
        size: 'modal-lg',
        callback: function () {
            //callToolTips();
        }
    }

    GetModalContent(opt);
}

function Amparo(SolicitudID, ClasificacionID) {

    var opt = {
        title: "Insatisfacción",
        url: "/ES/SIAC/GetAmparo",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        id: "modalAmparo",
        static: true,
        xclass: "modelChild",
        size: 'modal-lg',
        callback: function () {
            callToolTips();
        }
    }

    GetModalContent(opt);
}

function Prorroga(SolicitudID, ClasificacionID, IsDenuncia) {
    var opt = {
        title: IsDenuncia ? "Prorrogar Denuncia" : "Prorrogar Solicitud",
        url: "/ES/SIAC/GetProrroga",
        params: {
            __RequestVerificationToken: getVToken(),
            SolicitudID: SolicitudID,
            ClasificacionID: ClasificacionID
        },
        id: "modalProrroga",
        static: true,
        xclass: "modelChild",
        size: 'modal-lg',
        callback: function () {
            callToolTips();
        }
    }

    GetModalContent(opt);
}

function FechaLiberacionFondosTGR(varNameStore) {
    
    var Datos = store.get(varNameStore) || [];

    if (Datos.length == 0) {
        showBlockUIError("Debe seleccionar al menos una Solicitud");
        return false;
    }

    var opt = {
        title: "Registro Fecha Liberación de Fondos",
        url: "/ES/PagoTasas/GetFechaLiberacion",
        params: {
            __RequestVerificationToken: getVToken(),
            Selected: Datos.join(",")
        },
        //top: false,

        //Small	.modal-sm	300px
        //Default	None	500px
        //Large	.modal-lg	800px
        //Extra large	.modal-xl	1140px

        size: '',
        callback: function () {
            //goGrid();
        }
    }

    GetModalContent(opt);
}

function seleccionarTRGrid(parent, obj) {

    $("#" + parent + " tr").removeClass("selected");

    var tr = $(obj).parents("tr");
    $(tr).addClass("selected");

}

function ValidarTelefonos() {

    //console.log("call ValidarTelefonos");

    $("#TelefonoFijo").on("blur", function () {

        var valor = $(this).val();

        if (valor != "") {

            var count = $(this).val().length;

            if (count < 5) {
                showBlockUIError("Debe indicar un Teléfono válido");
                $(this).parent().addClass("error");
                $(this).val("");
                return false;
            } else {
                $(this).parent().removeClass("error");
            }
        } else {
            $(this).parent().removeClass("error");
        }

    });

    $("#TelefonoMovil").on("blur", function () {

        var valor = $(this).val();

        if (valor != "") {

            var count = $(this).val().length;

            if (count < 5) {
                showBlockUIError("Debe indicar un Teléfono Móvil válido");
                $(this).parent().addClass("error");
                $(this).val("");
                return false;
            } else {
                $(this).parent().removeClass("error");
            }
        } else {
            $(this).parent().removeClass("error");
        }

    });

    $("#Telefono").on("blur", function () {

        var valor = $(this).val();

        if (valor != "") {

            var count = $(this).val().length;

            if (count < 5) {
                showBlockUIError("Debe indicar un Teléfono válido");
                $(this).val("").addClass("is-invalid");
                return false;
            } else {

                if ($(this).hasClass("is-invalid")) {
                    $(this).removeClass("is-invalid").addClass("is-valid");
                }
            }
        } else {
            $(this).removeClass("is-invalid");
        }

    });

}

function getFechaFinPlazo(FechaIngreso, PlazoDias) {
    var urlTimeServer = "/ES/Json/FechaFinPlazo";
    var FechaTermino;

    $.ajax({
        url: urlTimeServer,
        async: false,
        type: "POST",
        dataType: "json",
        data: { __RequestVerificationToken: getVToken(), FechaIngreso: FechaIngreso, PlazoDias: PlazoDias },
        success: function (data, status, xhr) {

            FechaTermino = data.result;
        }
    });

    return FechaTermino;
}