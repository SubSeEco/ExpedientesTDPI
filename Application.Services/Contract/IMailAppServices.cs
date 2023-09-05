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

    }
}
