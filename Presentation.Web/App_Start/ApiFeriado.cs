using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Logging;

using Api.Abenis.Rest;
using Application.Services;
using DTO = Application.DTO;

namespace Presentation.Web
{
    /// <summary>
    /// API FEriados, actualiza registros de BBDD 
    /// </summary>
    /// <returns></returns>
    public static class ApiFeriado
    {
                
        #region Feriados
        /// <summary>
        /// API FEriados, actualiza registros de BBDD 
        /// </summary>
        /// <returns></returns>
        public static void Feriados()
        {
            IFeriados apiFeriado = new Feriados();
            ICommonAppServices appCommon = new CommonAppServices();

            try
            {
                IList<DiaFeriado> apiList = apiFeriado.GetListaFeriados();

                foreach (var item in apiList)
                {
                    DateTime apiFecha = Convert.ToDateTime(item.fecha);
                    IList<DTO.Models.Feriado> listDTO = appCommon.GetAllFeriados();

                    if (listDTO.Any(x => x.Fecha == apiFecha))
                    {
                        continue;
                    }
                    else
                    {
                        DTO.Models.Feriado _dto = new DTO.Models.Feriado();
                        _dto.Fecha = apiFecha;
                        _dto.FeriadoID = appCommon.SaveFeriado(_dto);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }

        }
        #endregion

    }
}