using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web
{
    /// <summary>
    /// Persona CU
    /// </summary>
    public class Persona
    {
        /// <summary>
        /// Persona
        /// </summary>
        public Persona()
        {
            this.RolUnico = new RolUnico();
            this.name = new name();
        }

        /// <summary>
        /// Campo CU
        /// </summary>
        public string sub { get; set; }

        /// <summary>
        /// Campo CU
        /// </summary>
        public RolUnico RolUnico { get; set; }

        /// <summary>
        /// Campo CU
        /// </summary>
        public name name { get; set; }


        /// <summary>
        /// Validación
        /// </summary>
        public bool IsValid()
        {
            return ((this.name.nombres.Length > 0) && (this.RolUnico.numero != 0));
        }

        /// <summary>
        /// GetRolUnico
        /// </summary>
        public string GetRolUnico()
        {
            if (this.RolUnico.tipo == "RUN")
            {
                return string.Format("{0}-{1}", this.RolUnico.numero, this.RolUnico.DV);
            }
            else
            {
                return string.Format("{0}", this.RolUnico.numero);
            }
        }

        /// <summary>
        /// Campo CU
        /// </summary>
        public bool IsRUT()
        {
            return this.RolUnico.tipo == "RUN";
        }

        /// <summary>
        /// GetNombres
        /// </summary>
        public string GetNombres()
        {
            if (this.name.nombres.Length > 0)
            {
                return string.Join(" ", this.name.nombres);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// GetApellidos
        /// </summary>
        public string GetApellidos()
        {
            if (this.name.apellidos.Length > 0)
            {
                return string.Join(" ", this.name.apellidos);
            }
            else
            {
                return "";
            }
        }
    }


    /// <summary>
    /// RolUnico
    /// </summary>
    public class RolUnico
    {

        /// <summary>
        /// RolUnico
        /// </summary>
        public RolUnico()
        {
            this.DV = "";
            this.tipo = "";
            this.numero = 0;
        }

        /// <summary>
        /// numero
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// DV
        /// </summary>
        public string DV { get; set; }

        /// <summary>
        /// tipo
        /// </summary>
        public string tipo { get; set; }
    }


    /// <summary>
    /// name
    /// </summary>
    public class name
    {

        /// <summary>
        /// 
        /// </summary>
        public name()
        {
            this.nombres = new string[] { };
            this.apellidos = new string[] { };
        }

        /// <summary>
        /// nombres
        /// </summary>
        public string[] nombres { get; set; }

        /// <summary>
        /// apellidos
        /// </summary>
        public string[] apellidos { get; set; }
    }
}