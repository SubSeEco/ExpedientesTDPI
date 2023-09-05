﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Persistence.Repository
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SGDE2Entities : DbContext
    {
        public SGDE2Entities()
            : base("name=SGDE2Entities")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 60 * 60;
            this.Configuration.LazyLoadingEnabled = false;

            Type type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AsocCausaDocumento> AsocCausaDocumento { get; set; }
        public virtual DbSet<AsocDocSistemaFirma> AsocDocSistemaFirma { get; set; }
        public virtual DbSet<AsocDocumentoSistemaEstadoDiario> AsocDocumentoSistemaEstadoDiario { get; set; }
        public virtual DbSet<AsocDocumentoSistemaTabla> AsocDocumentoSistemaTabla { get; set; }
        public virtual DbSet<AsocEscritoDocto> AsocEscritoDocto { get; set; }
        public virtual DbSet<AsocExpedienteOpcion> AsocExpedienteOpcion { get; set; }
        public virtual DbSet<AsocExpeFirma> AsocExpeFirma { get; set; }
        public virtual DbSet<AsocFirmaDocto> AsocFirmaDocto { get; set; }
        public virtual DbSet<AsocTipoCausaPerfil> AsocTipoCausaPerfil { get; set; }
        public virtual DbSet<AsocTipoDocumentoAdjunto> AsocTipoDocumentoAdjunto { get; set; }
        public virtual DbSet<AsocTipoNotificacionPerfil> AsocTipoNotificacionPerfil { get; set; }
        public virtual DbSet<AsocTipoTramiteOpciones> AsocTipoTramiteOpciones { get; set; }
        public virtual DbSet<AsocUsuarioPerfil> AsocUsuarioPerfil { get; set; }
        public virtual DbSet<Causa> Causa { get; set; }
        public virtual DbSet<ConfigurarDescripcion> ConfigurarDescripcion { get; set; }
        public virtual DbSet<ConfTipoCausa> ConfTipoCausa { get; set; }
        public virtual DbSet<Derivacion> Derivacion { get; set; }
        public virtual DbSet<DetalleEstadoDiario> DetalleEstadoDiario { get; set; }
        public virtual DbSet<DetalleTabla> DetalleTabla { get; set; }
        public virtual DbSet<DocumentoAdjunto> DocumentoAdjunto { get; set; }
        public virtual DbSet<DocumentoCausa> DocumentoCausa { get; set; }
        public virtual DbSet<DocumentoSistema> DocumentoSistema { get; set; }
        public virtual DbSet<DocumentoTmp> DocumentoTmp { get; set; }
        public virtual DbSet<EstadoCausa> EstadoCausa { get; set; }
        public virtual DbSet<EstadoDiario> EstadoDiario { get; set; }
        public virtual DbSet<EstadosAplica> EstadosAplica { get; set; }
        public virtual DbSet<EstadoTabla> EstadoTabla { get; set; }
        public virtual DbSet<Expediente> Expediente { get; set; }
        public virtual DbSet<FamiliasMimeType> FamiliasMimeType { get; set; }
        public virtual DbSet<Feriado> Feriado { get; set; }
        public virtual DbSet<Firma> Firma { get; set; }
        public virtual DbSet<Folio> Folio { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<LogCausa> LogCausa { get; set; }
        public virtual DbSet<LogSistema> LogSistema { get; set; }
        public virtual DbSet<MaximoTamanoArchivo> MaximoTamanoArchivo { get; set; }
        public virtual DbSet<OpcionesTramite> OpcionesTramite { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parte> Parte { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<Tabla> Tabla { get; set; }
        public virtual DbSet<TipoCanal> TipoCanal { get; set; }
        public virtual DbSet<TipoCausa> TipoCausa { get; set; }
        public virtual DbSet<TipoContencioso> TipoContencioso { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoEstadoDiario> TipoEstadoDiario { get; set; }
        public virtual DbSet<TipoFormato> TipoFormato { get; set; }
        public virtual DbSet<TipoGenero> TipoGenero { get; set; }
        public virtual DbSet<TipoNotificacion> TipoNotificacion { get; set; }
        public virtual DbSet<TipoParte> TipoParte { get; set; }
        public virtual DbSet<TipoTabla> TipoTabla { get; set; }
        public virtual DbSet<TipoTramite> TipoTramite { get; set; }
        public virtual DbSet<TipoVentana> TipoVentana { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Ventana> Ventana { get; set; }
        public virtual DbSet<VersionEncript> VersionEncript { get; set; }
    
        public virtual ObjectResult<SP_FlujoEstado_Result> SP_FlujoEstado(Nullable<int> tipoTramiteID)
        {
            var tipoTramiteIDParameter = tipoTramiteID.HasValue ?
                new ObjectParameter("TipoTramiteID", tipoTramiteID) :
                new ObjectParameter("TipoTramiteID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_FlujoEstado_Result>("SP_FlujoEstado", tipoTramiteIDParameter);
        }
    
        public virtual ObjectResult<SP_Causas_Result> SP_Causas(Nullable<int> usuarioID, string numeroTicket, Nullable<int> anio, Nullable<int> tipoCausaID, string numeroRegistro, string denominacion, string apelante, string fechaIngreso, string apelado, Nullable<int> estadoCausaID, string numeroSolicitud, Nullable<bool> isSoloEscritos, Nullable<bool> isSoloMisEscritos)
        {
            var usuarioIDParameter = usuarioID.HasValue ?
                new ObjectParameter("UsuarioID", usuarioID) :
                new ObjectParameter("UsuarioID", typeof(int));
    
            var numeroTicketParameter = numeroTicket != null ?
                new ObjectParameter("NumeroTicket", numeroTicket) :
                new ObjectParameter("NumeroTicket", typeof(string));
    
            var anioParameter = anio.HasValue ?
                new ObjectParameter("Anio", anio) :
                new ObjectParameter("Anio", typeof(int));
    
            var tipoCausaIDParameter = tipoCausaID.HasValue ?
                new ObjectParameter("TipoCausaID", tipoCausaID) :
                new ObjectParameter("TipoCausaID", typeof(int));
    
            var numeroRegistroParameter = numeroRegistro != null ?
                new ObjectParameter("NumeroRegistro", numeroRegistro) :
                new ObjectParameter("NumeroRegistro", typeof(string));
    
            var denominacionParameter = denominacion != null ?
                new ObjectParameter("Denominacion", denominacion) :
                new ObjectParameter("Denominacion", typeof(string));
    
            var apelanteParameter = apelante != null ?
                new ObjectParameter("Apelante", apelante) :
                new ObjectParameter("Apelante", typeof(string));
    
            var fechaIngresoParameter = fechaIngreso != null ?
                new ObjectParameter("FechaIngreso", fechaIngreso) :
                new ObjectParameter("FechaIngreso", typeof(string));
    
            var apeladoParameter = apelado != null ?
                new ObjectParameter("Apelado", apelado) :
                new ObjectParameter("Apelado", typeof(string));
    
            var estadoCausaIDParameter = estadoCausaID.HasValue ?
                new ObjectParameter("EstadoCausaID", estadoCausaID) :
                new ObjectParameter("EstadoCausaID", typeof(int));
    
            var numeroSolicitudParameter = numeroSolicitud != null ?
                new ObjectParameter("NumeroSolicitud", numeroSolicitud) :
                new ObjectParameter("NumeroSolicitud", typeof(string));
    
            var isSoloEscritosParameter = isSoloEscritos.HasValue ?
                new ObjectParameter("IsSoloEscritos", isSoloEscritos) :
                new ObjectParameter("IsSoloEscritos", typeof(bool));
    
            var isSoloMisEscritosParameter = isSoloMisEscritos.HasValue ?
                new ObjectParameter("IsSoloMisEscritos", isSoloMisEscritos) :
                new ObjectParameter("IsSoloMisEscritos", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Causas_Result>("SP_Causas", usuarioIDParameter, numeroTicketParameter, anioParameter, tipoCausaIDParameter, numeroRegistroParameter, denominacionParameter, apelanteParameter, fechaIngresoParameter, apeladoParameter, estadoCausaIDParameter, numeroSolicitudParameter, isSoloEscritosParameter, isSoloMisEscritosParameter);
        }
    }
}