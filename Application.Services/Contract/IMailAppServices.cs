using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;
using Application.DTO.Models;

namespace Application.Services
{
    public interface IMailAppServices
    {
        void IngresoNuevaCausa(int CausaID, int UsuarioID);
        List<string> ListadoIngresoDiario(string Hash);
        void NotificacionDerivacion(int CausaID, int UsuarioID, int UsuarioActive, string ComentariosDerivacion);
        void NotificacionAdmisibilidad(DTO.Models.Expediente expediente, int UsuarioID, string Comentarios);
        void IngresoExpediente(int CausaID, int UsuarioID, string strTipoTramite);
        void RegistroAbogado(int UsuarioID);
        void SolicitudRegistroAbogado(int UsuarioID, string Hash);
        void SendAlarmaInterna(SP_Alarmas_Result alarma);
        void SendAlarmaSinAsignar(List<string> Roles);

    }
}
