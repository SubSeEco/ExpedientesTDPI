using System;
using WebConfig = Domain.Infrastructure.WebConfigValues;
using Enums = Domain.Infrastructure;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;

namespace Presentation.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class SignFile
    {
        internal const string TipoUploadChar = "{#}";

        private string FilePath { get; set; }
        private string Filename { get; set; }
        private string SignerAlias { get; set; }
        private Enums.SignPageNumber SignPageNumber { get; set; }
        private Enums.SignPosition SignPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileSignedBase64 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_filePath"></param>
        /// <param name="_fileName"></param>
        /// <param name="_signer"></param>
        /// <param name="_pageNumber"></param>
        /// <param name="_signPosition"></param>
        public SignFile(string _filePath, string _fileName, string _signer, Enums.SignPageNumber _pageNumber, Enums.SignPosition _signPosition)
        {
            FilePath = _filePath;
            Filename = _fileName;
            SignerAlias = _signer;
            SignPageNumber = _pageNumber;
            SignPosition = _signPosition;
        }


        private string GetFileBase64()
        {
            Byte[] bytesContent = File.ReadAllBytes(FilePath);
            String fileBase64 = Convert.ToBase64String(bytesContent);

            return fileBase64;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseSign Sign()
        {
            ResponseSign response = new ResponseSign();
            response.Status = Enums.SignStatus.Failed.ToString();
            response.Message = "";

            try
            {
                SignPdf data = new SignPdf();
                data.inputfolder = GetFileBase64();
                data.location = WebConfig.SignService_Location;
                data.page_number = (int)SignPageNumber;
                data.sign_location = SignPosition.ToString();
                data.signer_name = SignerAlias;
                data.username = WebConfig.SignService_User;
                data.password = WebConfig.SignService_Pass;


                using (WebClient wc = new WebClient())
                {
                    var values = new NameValueCollection();
                    values.Add("inputfolder", data.inputfolder);
                    values.Add("location", data.location);
                    values.Add("page_number", data.page_number.ToString());
                    values.Add("sign_location", data.sign_location);
                    values.Add("signer_name", data.signer_name);
                    values.Add("username", data.username);
                    values.Add("password", data.password);

                    //ServicePointManager.Expect100Continue = true;
                    //ServicePointManager.DefaultConnectionLimit = 9999;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    byte[] responsebytes = wc.UploadValues(WebConfig.SignService_URL, values);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);

                    response = JsonConvert.DeserializeObject<ResponseSign>(responsebody);
                }
            }
            catch (Exception ex)
            {
                response.Status = Enums.SignStatus.Failed.ToString();
                response.Message = ex.Message;
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        public void ReplaceFile(DateTime fecha)
        {
            #region Guardar Archivo Temp Dir

            string fileNameTmp, EndPathTmp;

            string BasePath = WebConfig.PathBaseRepository;
            string FolderSave = string.Format("\\Temp\\{0}", DateTime.Now.TimeOfDay.Ticks);
            string TmpDir = string.Format("{0}{1}", BasePath, FolderSave);

            if (!Directory.Exists(TmpDir)) Directory.CreateDirectory(TmpDir);

            fileNameTmp = Path.GetFileName(FilePath);
            EndPathTmp = Path.Combine(TmpDir, fileNameTmp);

            File.WriteAllBytes(EndPathTmp, Convert.FromBase64String(FileSignedBase64));

            #endregion

            #region Move File

            string FileNameBackup = FilePath.Replace(Filename, string.Format("{0}_{1}", fecha.ToString("ddMMyyyy_HHmmss"), Filename));
            File.Replace(EndPathTmp, FilePath, FileNameBackup, false);

            #endregion

        }
    }



    /// <summary>
    /// 
    /// </summary>
    public class ResponseSign
    {
        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SignedBase64EncodedString { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SignPdf
    {
        /// <summary>
        /// 
        /// </summary>
        public string inputfolder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int page_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign_location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string signer_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
    }
}