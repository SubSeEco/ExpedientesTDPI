using Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Presentation.Web
{
    /// <summary>
    /// Clase que genera un link en vista HTML
    /// </summary>
    public class Link
    {
        /// <summary>
        /// 
        /// </summary>
        public Link()
        {
            id = string.Empty;
            href = "javascript:;";
            title = string.Empty;
            style = string.Empty;
            click = string.Empty;
            xicon = string.Empty;
            filename = string.Empty;
            _class = string.Empty;
            contenido = string.Empty;
            data_txt = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string href { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string style { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string click { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string xicon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contenido { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string data_txt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTargetBlank { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsOptimize"></param>
        /// <returns></returns>
        public string Generate(bool IsOptimize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<a class='x-title-t rel {0}' id='{1}' ", _class, id));
            sb.Append(string.Format("title='{0}' data-html='true' style='{1}' href='{2}' data-txt='{3}' {4} ", title, style, href, data_txt, (IsTargetBlank) ? "target='_blank'" : ""));
            sb.Append(string.Format("onclick='{0}'><i class='x-icon {1}'></i>{2}</a>", click, xicon, contenido));

            string end = sb.ToString();

            if (IsOptimize)
            {

                //[Ver Solicitud][/ES/SIAC/OIRS/57187/2][javascript:;][x-icon-zoom1]
                string icon = xicon.Replace("x-icon", "");

                string retorno = string.Format("{0}|{1}|{2}|{3}", title, href, click, icon);

                return retorno;
            }
            else
            {
                return end;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getExtension()
        {
            string file = "";

            try
            {
                string css = filename.Trim().ToLower();
                file = System.IO.Path.GetExtension(css).Substring(1);
            }
            catch (Exception e2)
            {
                Logger.Execute().Error(e2);
            }

            return file;
        }
    }
}