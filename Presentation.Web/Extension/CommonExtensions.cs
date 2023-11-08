using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Enums = Domain.Infrastructure;
using resources = Infrastructure.Resources;

using DTO = Application.DTO;
using System.IO;
using Infrastructure.Logging;
using Application.Services;
using Domain.Infrastructure;
using System.Globalization;

namespace Presentation.Web.Extension
{
    /// <summary>
    /// Extensiones utilitarias que para uso en vistas HTML
    /// </summary>
    public static class CommonExtensions
    {

        /// <summary>
        /// Versión del ensamblado
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string GetVersion(this HtmlHelper helper)
        {
            string version = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return version;
        }

        /// <summary>
        /// GetAbenis
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string GetAbenis(this HtmlHelper helper)
        {
            string abenis = "<a class='byAbenis' href='javascript:void(0)'>by Abenis</a>";
            return abenis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string GetBaseTableCSS(this HtmlHelper helper)
        {
            return "table table-striped table-sm table-bordered table-hover x-table";
        }

        /// <summary>
        /// DatetimeToString
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Fecha"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        public static string DatetimeToString(this HtmlHelper htmlHelper, DateTime? Fecha, string Format = "dd-MM-yyyy")
        {
            if (string.IsNullOrEmpty(Fecha.ToString())) return string.Empty;

            string strDate = (Fecha != null ? Fecha.Value.ToString(Format) : " ");
            return strDate;
        }

        /// <summary>
        /// Crea un radio personalizado
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="IsChecked"></param>
        /// <param name="id"></param>
        /// <param name="ObjID"></param>
        /// <param name="txtlabelText"></param>
        /// <param name="size"></param>
        /// <param name="section"></param>
        /// <param name="IsDisabled"></param>
        /// <param name="IsReadonly"></param>
        /// <param name="adicionalData"></param>
        /// <returns></returns>
        public static string MyCheckbox(this HtmlHelper htmlHelper, bool IsChecked, string id = "", int ObjID = 0,
            string txtlabelText = "", string size = "mini", int section = 0, bool IsDisabled = false, bool IsReadonly = false, string adicionalData = "")
        {
            string _disabled = IsDisabled ? "disabled" : "";
            string _readonly = IsReadonly ? "readonly" : "";
            string _checked = IsChecked ? "checked" : "";

            string labels = "data-on-text='SI' data-off-text='NO'";

            StringBuilder str = new StringBuilder();//
            str.Append("<input class='xCheckbox' id='" + id + "' value='true' name='" + id + "' type='checkbox' " + "data-obj-id='" + ObjID + "' data-add-data='" + adicionalData + "'");
            str.Append("data-size='" + size + "' " + labels + "data-obj-section='" + section + "' " + _disabled + " data-label-text='" + txtlabelText + "' " + _readonly + " " + _checked + " />");

            return str.ToString();
        }


        /// <summary>
        /// EnumToString
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnumToString(this HtmlHelper htmlHelper, Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    System.ComponentModel.DescriptionAttribute attr = Attribute.GetCustomAttribute(field,
                             typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;

                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// GetDv
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="rut"></param>
        /// <returns></returns>
        public static string GetDv(this HtmlHelper htmlHelper, int rut)
        {
            if (rut > 0)
            {
                Infrastructure.Utils.Mod11Validator mod11 = new Infrastructure.Utils.Mod11Validator(rut, "");
                return mod11.CalcularDigitoVerificador(rut);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Monto"></param>
        /// <param name="NumDecimales"></param>
        /// <param name="devuelve"></param>
        /// <returns></returns>
        public static string DecimalFormat(this HtmlHelper htmlHelper, decimal? Monto, int NumDecimales = 0, string devuelve = "")
        {
            decimal valor = Monto ?? 0m;

            if (valor == 0)
            {
                return devuelve;
            }

            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = WebConfigValues.SeparadorMiles;
            nfi.NumberDecimalSeparator = WebConfigValues.SeparadorDecimales;
            nfi.NumberDecimalDigits = NumDecimales;

            return valor.ToString("n", nfi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Monto"></param>
        /// <param name="NumDecimales"></param>
        /// <returns></returns>
        public static string DecimalFormatTouchSpin(this HtmlHelper htmlHelper, decimal? Monto, int NumDecimales = 0)
        {
            decimal valor = Monto ?? 0m;

            if (valor == 0)
            {
                return "";
            }

            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = WebConfigValues.SeparadorDecimales;
            nfi.NumberDecimalSeparator = WebConfigValues.SeparadorMiles;
            nfi.NumberDecimalDigits = NumDecimales;

            return valor.ToString("n", nfi);
        }

        /// <summary>
        /// EnumToInt
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int EnumToInt(this HtmlHelper htmlHelper, Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// GetInfoVentana
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="TipoVentana"></param>
        /// <param name="NotColapse"></param>
        /// <param name="AlertType"></param>
        /// <param name="Height"></param>
        /// <param name="showIcon"></param>
        /// <returns></returns>
        public static string GetInfoVentana(this HtmlHelper htmlHelper, Enums.TipoVentana TipoVentana, 
            bool NotColapse = false, string AlertType = "primary", string Height = "", bool showIcon = true)
        {
            StringBuilder str = new StringBuilder();

            try
            {
                Application.Services.ICommonAppServices application = new Application.Services.CommonAppServices();

                DTO.Models.Ventana ventana = application.GetVentanaByTipoVentanaID((int)TipoVentana);

                string info = ventana.TextoAyuda[htmlHelper.ViewBag.Language];
                string nc = (NotColapse) ? "isForm" : "";
                string height = string.IsNullOrEmpty(Height) ? "" : "height:auto";
                string icon = showIcon ? "<i class='x-icon x-icon-info rel' style='top:2px'></i> " : "";


                str.Append("<div class='alert alert-" + AlertType + " " + nc + "' style='" + height + "' role='alert'>" + icon + "");
                str.Append(HttpUtility.HtmlDecode(info));
                str.Append("</div>");
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }

            return str.ToString();            
        }

        /// <summary>
        /// GetInfoVentanaIpsum
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Texto"></param>
        /// <param name="NotColapse"></param>
        /// <param name="AlertType"></param>
        /// <returns></returns>
        public static string GetInfoVentanaIpsum(this HtmlHelper htmlHelper, string Texto = "", bool NotColapse = false, string AlertType = "primary")
        {

            string info = (!string.IsNullOrEmpty(Texto)) ? Texto : "Información de ayuda";

            string nc = (NotColapse) ? "isForm" : "";

            StringBuilder str = new StringBuilder();
            str.Append("<div class='alert alert-" + AlertType + " " + nc + "'><i class='x-icon x-icon-info rel' style='top:2px'></i> ");
            str.Append(HttpUtility.HtmlDecode(info));
            str.Append("</div>");

            return str.ToString();
        }


        /// <summary>
        /// SplitToUpper
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Texto"></param>
        /// <returns></returns>
        public static string SplitToUpper(this HtmlHelper htmlHelper, string Texto)
        {
            StringBuilder output = new StringBuilder();

            foreach (char letter in Texto)
            {
                if (Char.IsUpper(letter) && output.Length > 0)
                {
                    output.Append(" " + letter);
                }
                else
                {
                    output.Append(letter);
                }
            }

            return output.ToString().Replace("O I R S", "OIRS");
        }


        /// <summary>
        /// GetCSS
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetCSS(this HtmlHelper htmlHelper, string filename)
        {
            string file = "";

            try
            {
                string css = filename.Trim().ToLower();
                file = Path.GetExtension(css).Substring(1);
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
            }

            return file;
        }

        /// <summary>
        /// GetExtensionesPermitidas
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="AsocTD"></param>
        /// <param name="IncluyeTamanio"></param>
        /// <returns></returns>
        public static string GetExtensionesPermitidas(this HtmlHelper htmlHelper, IList<DTO.Models.AsocTipoDocumentoAdjunto> AsocTD, bool IncluyeTamanio = false)
        {
            string ExtensionesPermitidas = string.Empty;

            if (AsocTD.Count > 0)
            {

                List<string> ext = new List<string>();
                foreach (var item in AsocTD)
                {
                    if (IncluyeTamanio)
                    {
                        string info = string.Format("<b>{0}</b> (máximo: {1}MB)",
                            item.DocumentoAdjunto.TipoFormato.Descripcion.Trim(),
                            item.DocumentoAdjunto.MaximoTamanoArchivo.Tamano);

                        ext.Add(info);
                    }
                    else
                    {
                        ext.Add(item.DocumentoAdjunto.TipoFormato.Descripcion.Trim());
                    }
                }

                ExtensionesPermitidas = string.Join(", ", ext.ToArray());
            }


            return ExtensionesPermitidas;
        }


        /// <summary>
        /// HelpTitle
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="Title"></param>
        /// <param name="MarginTop"></param>
        /// <param name="placement"></param>
        /// <returns></returns>
        public static string HelpTitle(this HtmlHelper htmlHelper, string Title = "", bool MarginTop = true, string placement = "t")
        {
            string t = (MarginTop) ? "top: 3px;" : "";
            string c = $"x-title-{placement}";

            string a = "<a class='"+ c + "' data-html='true' title='" + Title + "' href='javascript:;' style='margin-left: 5px'><i class='x-icon x-icon-help3 rel' style='" + t + "'></i></a>";

            return a;
        }


    }
}