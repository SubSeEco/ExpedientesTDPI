var showChar = 50;
var maxTextInGrid = 100;

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

function readOnlyPreview() {
    $(document).find("header").remove();
    $(document).find(".sideBar").remove();
    $(document).find(".content").removeClass("conSideBar");
    $(document).find("#footer").remove();

    $(document).find(".mainSection").css("margin", "2px auto 0");

    $("#btnCleanWindow").remove();
}

function parseShorten(txt, style) {
    var l = txt.length || "";
    var r = txt;
    var style = style || "";

    if (l >= maxTextInGrid) {
        //<span style="display: inline;">Short Content</span>
        r = "<span class='txtShortensss' style='word-break:break-all; " + style + "'>" + r + "</span>";
    }

    return r;
}

function readOnlyPreview2() {
    $(document).find("header").hide().remove();
    $(document).find("#footer").remove();
    $(document).find(".mainSection").css("margin", "0 auto");

    $(".padre").eq(1).remove();
    $(".padre").eq(2).remove();
    $(".liBottom").remove();
    $("#tabFooter").remove();

    $("#btnCleanWindow").remove();

    $(".g-button").remove();

    //$("#btnSaveAll, #btnSaveAll2").remove();

    $(".table").find(".x-icon-edit").remove();
    $(".table").find(".x-icon-delete").remove();
    $(".table").find(".x-icon-copy").remove();

    $(document).find("textarea").attr("disabled", true);
    $(document).find("input").not(":radio").attr("disabled", true);
    //$(document).find("select").attr("disabled", true);
    $(document).find("button").attr("disabled", true);
}


function xDialogContent(title, UrlAction, params, callback, width) {
    title = title || "";
    width = width || 450;

    var div = jQuery('<div/>', {
        title: title, "class": "alertTitle",
        html: "<div class='ac' style='margin-top: 10px;'><i class='x-icon-loader'></i></div>"
    }).appendTo('body');


    $(div).dialog({
        modal: true, resizable: false, closeOnEscape: false, width: width,// height: height,
        position: ['10%', 250],
        buttons: {
            "Aceptar": function () {

                if ($.isFunction(callback)) {
                    callback.apply();
                }

                $(this).dialog("close");
            }
        },
        open: function () {
            $('.ui-dialog-buttonpane').find('button').addClass("g-button").css("line-height", "0");
            //$('.xAlert').css("text-align", "center").css("margin-top", "15px").css("min-height", "30px");
        },
        close: function () {
            $(div).remove();
        }
    });

    xAjaxHtml(UrlAction, params, $.noop,
	function () {
	    $(div).html(DataHTM);
	});


}


function xDialogContentNew(title, UrlAction, params, callback, width) {
    title = title || "";
    width = width || 450;

    var div = jQuery('<div/>', {
        title: title, "class": "alertTitle",
        html: "<div class='ac' style='margin-top: 10px;'><i class='x-icon-loader'></i></div>"
    }).appendTo('body');


    $(div).dialog({
        modal: true, resizable: false, closeOnEscape: false, width: width,// height: height,
        //position: ['top', 'center'],
        position: { my: "center center-100", at: "center center", of: window },
        buttons: {
            "Aceptar": function () {

                if ($.isFunction(callback)) {
                    callback.apply();
                }

                $(this).dialog("close");
            }
        },
        open: function () {
            $('.ui-dialog-buttonpane').find('button').addClass("g-button").css("line-height", "0");
            //$('.xAlert').css("text-align", "center").css("margin-top", "15px").css("min-height", "30px");
        },
        close: function () {
            $(div).remove();
        }
    });

    xAjaxHtml(UrlAction, params, $.noop,
	function () {
	    $(div).html(DataHTM);
	});


}


function xDialogConfirmActionSINO(titleDiv, msg1, msg2, UrlAction, params, callbackSI, callbackNO, width, height) {

    width = width || 450;
    height = height || 160;
    UrlAction = UrlAction || 0;
    params = params || function () { };
    callbackSI = callbackSI || SelfRedirect;
    callbackNO = callbackNO || SelfRedirect;

    var div = jQuery('<div/>', { title: titleDiv, "class": "alertTMP", html: "<br><p class='ac'>" + msg1 + "</p>" }).appendTo('body');

    $(div).dialog({
        modal: true, resizable: false, closeOnEscape: false, width: width, height: height,
        position: ['10%', 250], //show: 'drop',// hide: 'drop',
        buttons: {
            "Sí": function () {

                $(this).dialog("close");

                blockPageUI();

                xAjax(UrlAction, params, function () {

                    blockPageUIclose();
                    xAlert(titleDiv, msg2, function () { callbackSI() });

                });


            },
            "No": function () {
                $(this).dialog("close");
            }
        },
        open: function () {
            $('.ui-dialog-buttonpane').find('button').addClass("g-button").css("line-height", "0");
            $('.xAlert').css("text-align", "center").css("margin-top", "15px").css("min-height", "30px");
        },
        close: function () {
            $(div).remove();
        }
    });

}


function setEditor(div) {

    $('#' + div).wysihtml5({
        stylesheets: ["../../Includes/css/wysiwyg-color.css"],
        "font-styles": true,
        "emphasis": true,
        "lists": true,
        "html": true,
        "link": true,
        "image": true,
        "color": false,
        "x-icon": false,
        "locale": "es-ES"
    });
}

function setCustimFile(objID, width) {

    _width = width || "80%";

    $('#' + objID).customFileInput({
        width: _width,
        buttonText: _buttonText,
        changeText: _changeText,
        inputText: _inputText,
        onLoad: function () { return false; },
        onChange: function (h) { fixFakePath(); }
    });
}


function createIcon(icon, title, link) {

    link = link || 'javascript:void(0)';
    title = title || "";

    var h = '<a href="' + link + '" title="' + title + '" class="x-title-t rel" style="padding-right:5px"><i class="x-icon ' + icon + '" style=""></i></a>';

    return h;
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


function ReloadCaptcha(obj, key) {

    //var info = obj + " - " + key;
    //alert(obj + " - " + key);
    //console.log(info);

    var obj = obj || "imgCaptcha";

    $("#" + obj).attr("src", "/Includes/css/images/bgCaptcha.png");

    $("#" + obj).attr("src", "/AntiBotImage.ashx?" + $.now() + "&" + "key=" + key);

    //console.log("ReloadCaptcha -> " + obj);
}



function actionMedioRespuesta(MedioRespuesta) {

    //$(".MedioRespuesta").on("click", function () {

    //	var valor = parseInt($(this).val());

    //	$(".error").removeClass("error");

    //	if (valor == parseInt(MedioRespuesta)) {

    //		$("#labelEmail").removeClass("hide");
    //		$("#Email").addClass("verificar");

    //		$("#labelDireccion").addClass("hide");
    //		$("#Direccion").removeClass("verificar");

    //		$("#labelRegion").addClass("hide");
    //		$("#RegionID").removeClass("verificar");

    //		$("#labelComuna").addClass("hide");
    //		$("#ComunaID").removeClass("verificar");

    //	} else {

    //		$("#labelEmail").addClass("hide");
    //		$("#Email").removeClass("verificar");

    //		$("#labelDireccion").removeClass("hide");
    //		$("#Direccion").addClass("verificar");

    //		$("#labelRegion").removeClass("hide");
    //		$("#RegionID").addClass("verificar");

    //		$("#labelComuna").removeClass("hide");
    //		$("#ComunaID").addClass("verificar");
    //	}

    //});

}

function validateMedios(CorreoElectronico, DireccionPostal) {


    $(".MedioNotificacion, .MedioRespuesta").on("click", function () {

        var Seleccion = parseInt($(this).val());

        $(".error").removeClass("error");

        var MedioNotificacion = parseInt($('input[name=MedioNotificacionID]:checked').val() || 0);
        var MedioRespuesta = parseInt($('input[name=MedioRespuestaID]:checked').val() || 0);

        if (Seleccion == CorreoElectronico) {
            //verificarEmail(true);
        }

        if (MedioNotificacion != CorreoElectronico && MedioRespuesta != CorreoElectronico) {
            //verificarEmail(false);
        }

        if (Seleccion == DireccionPostal) {
            //verificarDireccion(true)
        }

        if (MedioNotificacion != DireccionPostal && MedioRespuesta != DireccionPostal) {
            //verificarDireccion(false);
        }

    });
}

function verificarEmail(validar) {
    var validar = validar || false;

    if (validar) {
        $("#labelEmail").removeClass("hide");
        $("#Email").addClass("verificar");
    }
    else {
        $("#labelEmail").addClass("hide");
        $("#Email").removeClass("verificar");
    }
}

function verificarDireccion(validar) {
    var validar = validar || false;

    if (validar) {
        $("#labelDireccion").removeClass("hide");
        $("#Direccion").addClass("verificar");

        $("#labelRegion").removeClass("hide");
        $("#RegionID").addClass("verificar");

        $("#labelComuna").removeClass("hide");
        $("#ComunaID").addClass("verificar");
    }
    else {
        $("#labelDireccion").addClass("hide");
        $("#Direccion").removeClass("verificar");

        $("#labelRegion").addClass("hide");
        $("#RegionID").removeClass("verificar");

        $("#labelComuna").addClass("hide");
        $("#ComunaID").removeClass("verificar");
    }
}


function validateSubmitTransparencia() {

    if ($(".MedioNotificacion:checked").length == 0) {
        showBlockUIError("Debe seleccionar un Medio de Notificación");
        scrollToElement($("#fsMedioNotificacion").eq(0));
        return false;
    }

    if ($(":radio.MedioRespuesta:checked").length > 0) {
        if ($("#Email").parent().hasClass("error")) {
            scrollToElement($(".error").eq(0));
            return false;
        }
    }
    else {
        if ($(":radio.MedioRespuesta:visible").length > 0) {
            showBlockUIError("Debe indicar un medio para recibir nuestra respuesta");
            scrollToElement($("#fsRespuesta"));
            return false;
        }
    }

    if ($(".FormatoRespuesta:checked").length == 0) {
        showBlockUIError("Debe seleccionar un Formato de entrega para la respuesta");
        scrollToElement($("#fsMedioRespuesta").eq(0));
        return false;
    }



    if ($(".chkCarta:visible").length > 0) {
        if ($(".chkCarta:checked").length == 0) {
            showBlockUIError("Debe seleccionar al menos una Categoría");
            return false;
        }
    }

    if ($(".chkPresencial:visible").length > 0) {
        if ($(".chkPresencial:checked").length == 0) {
            showBlockUIError("Debe marcar al menos una Categoría");
            return false;
        }
    }

    //if ($("#FechaNacimiento").val() != "") {
    //    var date = ($("#FechaNacimiento").val().indexOf('_') !== -1);
    //    if (date) {
    //        showBlockUIError("Debe seleccionar una Fecha de Nacimiento válida");
    //        return false;
    //    }
    //}

    if ($("#FechaFormulario:visible").length > 0) {
        if (!IsFechaValida("FechaFormulario")) {
            showBlockUIError("Ingrese una Fecha Formulario válida.");
            return false;
        }
    }

    if ($("#TelefonoFijo").val() != "") {

        var count = $("#TelefonoFijo").val().length;

        if (count < 5) {
            showBlockUIError("Debe indicar un Teléfono válido");
            return false;
        }
    }

    if ($("#TelefonoMovil").val() != "") {

        var count = $("#TelefonoMovil").val().length;

        if (count < 5) {
            showBlockUIError("Debe indicar un Teléfono Móvil válido");
            return false;
        }
    }

    if ($("#FechaFormulario").length > 0) {

        var fechaForm = $("#FechaFormulario").val();

        var f1 = moment($("#FechaFormulario").val(), "DD-MM-YYYY");
        var ahora = moment();

        if (f1 > ahora) {
            showBlockUIError("La Fecha Formulario no es coherente");
            return false;
        }
    }

    if ($("#IsPuebloOriginarioS:visible").is(":checked")) {

        if (parseInt($("#PuebloOriginarioID").val()) == 0) {
            showBlockUIError("Debe indicar Pueblo Originario");
            return false;
        }
    }

    return true;
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

function VerHistorial_old(SolicitudID) {
    var URL = "/ES/SIAC/HistorialSolicitud";
    var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID };
    var callback = function () {

    };

    getDivAjaxHtml(URL, "Historial", params, callback, 880);
}

function OpenDialogIframeBtns(SRC, title, onClose, _width, _height, _top) {
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
        buttons: {
            'Aceptar': function () {
                parent.CloseDialog();
            }
        },
        open: function () {
            $('.ui-dialog-buttonpane').find('button').addClass("g-button x-btn x-btn-destaca");
        }
    }).width(_width - 10).height(_height);
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

function FixIframeRespuesta() {

    try {

        var d = $(".ui-dialog:visible").width();

        //console.log("==> ", $("#iframeDialog").width());
        $("#iframeDialog").css("width", d + "px");

    } catch (e) {

    }

}






function DerivarInternamente(SolicitudID, PerfilID, DiasTranscurridos) {
    var URL = "/ES/SIAC/GetDerivar";
    var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID, PerfilID: PerfilID, DiasTranscurridos: DiasTranscurridos };
    var callback = function () {

    };

    getDivAjaxHtml(URL, "Derivar a otra Unidad", params, callback, 900)
}

function Admisibilidad(SolicitudID, ClasificacionID) {
    var SRC = "/ES/SIAC/GetAdmisibilidad?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    var title = "Admisible";

    var fnGuardar = $.noop();
    var fnCancelar = parent.DialogClose();
    var onClose = $.noop();

    var heigth = 380;
    if (ClasificacionID == 2) heigth = 530;

    OpenDialogIframe(SRC, title, onClose, 500, heigth, 40);
}

function ContactoCiudadano(SolicitudID, ClasificacionID) {
    var SRC = "/ES/SIAC/GetContactoCiudadano?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    var title = "Contactar Ciudadano";

    var fnGuardar = $.noop();
    var fnCancelar = parent.DialogClose();
    var onClose = $.noop();

    var heigth = 230;
    if (ClasificacionID == 2) heigth = 330;

    OpenDialogIframe(SRC, title, onClose, 600, heigth, 40);
}

function RespuestaContactoCiudadano(SolicitudID, ClasificacionID) {
    var SRC = "/ES/SIAC/GetRespuestaContactoCiudadano?SolicitudID=" + SolicitudID + "&ClasificacionID=" + ClasificacionID;
    var title = "Respuesta Ciudadano";

    var fnGuardar = $.noop();
    var fnCancelar = parent.DialogClose();
    var onClose = $.noop();

    OpenDialogIframe(SRC, title, onClose, 500, 240, 40);
}

function CambioClasificacion(IsCoordinador, SolicitudID, ClasificacionID, IsOIRS, NumeroTicket) {
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

    //xDialogConfirmAction2(titleDiv, msg1, msg2, UrlAction, params, callback, width, height)

    //if (IsCoordinador)
    //{


    //    xDialogConfirmAction2("Cambio Clasificación: " + title,
    //				 "¿Solicita autorización cambiar la solicitud " + NumeroTicket + " a <b>" + clasificacion + "?",
    //				 "Se ha solicitado autorización", 0, $.noop(),
    //                 function () {

    //				     //_.where(grillaCoordinador, { id: idGrilla })[0].EstadoID = 22;
    //				     //goGrid();

    //				 }, 500);
    //}
    //else
    //{

    //}
    //console.log(SolicitudID, ClasificacionID, NumeroTicket);
}


function ResponderSolicitud_Old(SolicitudID, PerfilID, Title) {
    var SRC = "/ES/SIAC/GetRespuestaSolicitud?SolicitudID=" + SolicitudID + "&PerfilID=" + PerfilID;
    //var title = "Respuesta Ciudadano";

    var fnGuardar = $.noop();
    var fnCancelar = parent.DialogClose();
    var onClose = $.noop();


    //OpenDialogIframe(SRC, title, onClose, _width, _height, _top)
    //OpenDialogIframe(SRC, Title, onClose, 800, 590, 20);
    OpenDialogIframe(SRC, Title, onClose, 900, 790, 20);
}


function ResponderSolicitud(SolicitudID, PerfilID, Title) {

    var URL = '/ES/SIAC/GetRespuestaSolicitud';
    var params = {
        __RequestVerificationToken: getVToken(),
        SolicitudID: SolicitudID,
        PerfilID: PerfilID,
        Title: Title
    };

    var callback = function () {


    };

    GetDivAjaxHtml(URL, Title, params, callback, 1000);
}

function VerHistorialDocumento(SolicitudID, DocumentoRespuestaID) {
    var URL = "/ES/SIAC/GetHistorialDocumento";
    var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID, DocumentoRespuestaID: DocumentoRespuestaID };
    var callback = function () {
        addTHBG();
        callToolTips();
    };

    getDivAjaxHtml(URL, "Versiones de Documento", params, callback, 750, 0, ['10%', 200]);
}

function setIsView() {

    console.log("setIsView");

    $("input:visible, select:visible, textarea:visible").attr('disabled', 'disabled');
    $(":checkbox:visible").attr('disabled', 'disabled');
    $("#btnEnviar, #btnBuscarFuncionario").remove();

    $(".x-icon-clean").not(".notRemove").remove();

    $(".x-icon-delete").not(".notRemove").remove();

    $("#frmFileSolicitud").remove();

    $("#frmFileSolicitudDenuncia input").enable();

}

function getRespuestaSolicitud(SolicitudID) {

    var title = "Respuesta Solicitud";
    var params = { __RequestVerificationToken: getVToken(), SolicitudID: SolicitudID };
    var callback = function () {
        //callCalendarsAjax();
        //addTHBG();
    };

    getDivAjaxHtml("/ES/AtencionCiudadana/DetalleRespuesta", title, params, callback, 1200);
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