﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Expediente {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Expediente() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Infrastructure.Resources.Expediente", typeof(Expediente).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Estados Diarios.
        /// </summary>
        public static string EstadosDiarios {
            get {
                return ResourceManager.GetString("EstadosDiarios", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Individualización.
        /// </summary>
        public static string lblDenominacion {
            get {
                return ResourceManager.GetString("lblDenominacion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Denominación / Nombre variedad.
        /// </summary>
        public static string lblDenominacionHelp {
            get {
                return ResourceManager.GetString("lblDenominacionHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Listados.
        /// </summary>
        public static string Listados {
            get {
                return ResourceManager.GetString("Listados", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Estados Diarios.
        /// </summary>
        public static string MenuListadoEstadosDiarios {
            get {
                return ResourceManager.GetString("MenuListadoEstadosDiarios", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ingresos.
        /// </summary>
        public static string MenuListadoIngreso {
            get {
                return ResourceManager.GetString("MenuListadoIngreso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Tablas.
        /// </summary>
        public static string MenuListadoTablas {
            get {
                return ResourceManager.GetString("MenuListadoTablas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Tablas.
        /// </summary>
        public static string Tablas {
            get {
                return ResourceManager.GetString("Tablas", resourceCulture);
            }
        }
    }
}