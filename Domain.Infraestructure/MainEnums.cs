using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure
{

    public enum LanguageEnum
    {
        ES,
        EN,
        es,
        en
    }

    public enum ServiceResponse
    {
        Ok,
        Error
    }

    public enum SignServiceIdentity
    {
        Disable = 0,
        Economia = 1,
    }



    public enum TipoAcceso
    {
        ClaveUnica = 1
    }

    public enum TipoPersonaSol
    {
        Solicitante = 1,
        Mandatario = 2,
    }

    public enum ActionSystem
    {
        EliminarParte,
        KeepAlive,
        EliminarTabla,
        QuitarVigenciaDetalleTabla,
        AgregarCausaTabla,
        EditarCausaTabla,
        EliminarEstadoDiario,
        QuitarVigenciaDetalleEstadoDiario,
        AgregarExpedienteEstadoDiario,
        FinalizarEstadoDiario,
        FinalizarTabla,
        FinalizarExpediente,
        GenerarExpedientePDF,
        ExpedienteInadmisible,
        EliminarExpediente,
    }

    public enum SignPosition
    {
        TOP_RIGHT = 1,
        LEFT_TOP = 2,
        BOTTOM_LEFT = 3,
        BOTTOM_RIGHT = 4,
        CENTERED_MIDDLE = 5,
        TOP_EDGE_CENTER = 6,
        BOTTOM_EDGE_CENTER = 7,
        LEFT_EDGE_CENTER = 8,
        RIGHT_EDGE_CENTER = 9
    }

    public enum SignPageNumber
    {
        LastPage = 0,
        FirstPage = 1,
        AllPages = -1
    }

    public enum SignStatus
    {
        Failed,
        Success
    }


    public enum TipoLog
    {
        Error,
        ActionLog,
        ActualizarTipoFormato,
        AgregarTipoFormato,
        ActualizarFamiliaMimeType,
        AgregarFamiliaMimeType,
        EditarConfDocumentoAdjunto,
        AgregarConfDocumentoAdjunto,
        EliminarConfDocumentoAdjunto,
        EliminarArchivoAdjuntoTemporal,
        CambiaEstadoCausa,
        EliminarArchivoAdjunto,
        AgregarTipoTramiteOpciones,
        EditarTipoTramiteOpciones,
        EliminarTipoTramiteOpciones,
        AgregarEstadosAplica,
        EliminarEstadosAplica,
        GenerarIngresoPDF,
        ModificarAyudas,
        ModificarTipoNotificacion,
        SaveExpediente,
        CreaCiudadanoClaveUnica,
        LoginSuccessCU,
        AgregarUsuario,
        EditarUsuario,
        DeleteParteByID,
        SaveEventoExpediente,
        EliminarTabla,
        QuitarVigenciaDetalleTabla,
        AgregarCausaTabla,
        EditarCausaTabla,
        GenerarTablaPDF,
        EliminarEstadoDiario,
        QuitarVigenciaDetalleEstadoDiario,
        AgregarExpedienteEstadoDiario,
        FirmarDocumento,
        FinalizarEstadoDiario,
        FinalizarTabla,
        EnviarEmail,
        CambiarEmailExterno,
        LoginAnonymous,
        FinalizarExpediente,
        AgregarCausa,
        ActualizarCausa,
        AgregarExpediente,
        ModificarExpediente,
        DerivarExpediente,
        AdmitirExpediente,
        RegistroAbogado,
        SubirCertificadoAbogado,
        SaveSigner,
        EliminarCausa
    }


    public enum TipoVentana
    {
        FormularioApelacionMarca = 1,
        EscritorioGestionExpedientes = 2,
        ListadosIngreso = 3,
        ListadosTablas = 4,
        ListadosEstadosDiarios = 5,
        EscritorioFirmaDocumentos = 6,
        MantenedorAyudas = 7,
        MantenedorGenerico= 8,
        MantenedorTipoFormato = 9, 
        MantenedorTipoDocumento = 10,
        MantenedorOpcionesTipoTramite = 11,
        PlantillasEmail = 12,
        MantenedorUsuarios = 13,
        ExternoPrincipal = 14,
        RegistroAbogado = 15,

    }

    
    public enum GenericOption
    {
        Si = 1,
        No = 2
    }

    public enum Genero
    {
        Masculino = 1,
        Femenino = 0,
    }

    public enum Nacionalidad
    {
        Chilena = 1,
        Extranjera = 0,
    }

    public enum GenericJson
    {
        TempData = -1,
        UserInvitado = -500,
        UserTMP = -100
    }

    public enum ReturnJson
    {
        ErrorCaptcha = -2,
        ErrorModelo = -1,
        SinAccion = 0,
        ActionSuccess = 1,
        UsuarioYaExiste = 2,
        UsuarioNoRegistrado = 3,
        LoginSuccess = 4,
        ErrorCodigoVerificacion = 5,
        Updated = 6,
        ErrorSharePoint = 7,
        FolioYaExiste = 8,
        YaExisteDescripcion = 9,
        DatoNoEncontrado = 10,
        ValidaData = 11,
    }

    public enum Perfil
    {
        ClaveUnicaSinPerfil = 0,
        TDPI = 1,
        INAPI = 2,
        SAG = 3,
        Administrador = 4,
        Abogado = 5,
    }

    public enum TipoDocumento
    {
        Temporal = -1,
        Causa = 1,
        Escrito = 2,
        Expediente = 3,
        Ingreso = 4,
        Tabla = 5,
        EstadoDiario = 6,
        ExpedienteElectronicoPDF = 7,
        CertificadoTituloAbogado = 8,
    }

    public enum TipoContencioso
    {
        NA = 0,
        Oposicion = 1,
        Nulidad = 2
    }

    public enum TipoGrid
    {
        None,
        ListadoIngresos,
        ListadoTablas,
        ListadoEstadoDiario,
        PDFListadoIngresos,

    }

    public enum TipoGenero
    {
        NA = 0,
        Masculino = 1,
        Femenino = 2,
        Otro = 3
    }

    public enum TipoCanal
    {
        PaginaWeb = 1,
        Presencial = 2
    }
    public enum EstadoCausa
    {
        EventoLog = 0,
        PreIngresado = 1,
        Ingresado = 2,
        EnProceso = 3,
        EnTabla = 4,
        Fallo = 5,
        Acuerdo = 6,

        AutosEnRelacion = 19,
        DeseCuenta = 20,
        Eliminado = 29,

    }

    public enum TipoParte
    {
        Recurrente = 1,
        Recurrido = 2,
        Apelante = 3,
        Apelado = 4,
        Solicitante = 5
    }

    public enum EstadoTabla
    {
        Borrador = 0,
        Generado = 1,
        FirmadoPublicado = 2,
        Eliminado = 3
    }

    public enum TipoEstadoDiario
    {
        Borrador = 0,
        Generado = 1,
        FirmadoPublicado = 2,
        Eliminado = 3
    }

    public enum TipoTabla
    {
        ConAlegatos = 1,
        EnCuenta = 2
    }

    public enum TipoTramite
    {
        Escrito = 1,
        Oficio = 2,
        Actuacion = 3,
        Resolucion = 4
    }

    public enum OpcionesTramite
    {
        Tramite = 1,
        IngresoEscrito = 2,
        RecepcionDeOficio = 3,
        RemiteOficio = 4
    }

    public enum EstadosDoctoFirma
    {
        VerTodas = 1,
        SoloPendientes= 2,
        Firmadas= 3,
    }

    public enum TipoCausa
    {
        Ninguna = 0,
        Marca = 1,
        Patente = 2,
        VariedadVegetal = 3,
        ProteccionSuplementaria = 4,
        RecursoHechoMarca = 5,
        RecursoHechoPatente = 6
    }

    public enum TipoNotificacion
    {
        IngresoNuevaCausa = 1,
        ListadoIngresoDiario = 2,
        Derivacion = 3,
        IngresoExpediente = 4,
        Admisibilidad = 5,
        RegistroAbogado = 6,
        SolicitudRegistroAbogado = 7,
        RecordatorioAtraso = 8,
        ExpedientesSinAsignar = 9,
    }

    public enum AlarmasInternas
    {
        AlarmaNoAsignados= 0,
        AlarmasAtrasos = 1
    }
    public enum TipoFirmaTabla
    {
        Presidente = 1,
        SecretarioAbogado = 2
    }
    public enum TipoTramiteSinOrdenFirma
    {
        Estudio = 19,
        MedidaParaMejorResolver = 47,
        Otros = 48,
        PorInterpuestoRecursoDeCasación = 53,
        Sentencia = 57
    }



}