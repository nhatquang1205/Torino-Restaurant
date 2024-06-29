using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TorinoRestaurant.Application.Commons
{
    /// <summary>
    /// Các xử lý chung thường xuyên sử dụng trong hệ thống.
    /// <para>Author: QuyPN</para>
    /// <para>Created: 15/02/2020</para>
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Sinh chuỗi token ngẫu nhiên theo id account đăng nhập, độ dài mặc định 100 ký tự.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi không trùng nhau sẽ cộng thêm vào token</param>
        /// <param name="length">Dộ dài của token, mặc định 100 ký tự</param>
        /// <returns> Chuỗi token </returns>
        public static string RenderToken(string str, int length = 100)
        {
            string token = "";
            Random ran = new();
            string tmp = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
            for (int i = 0; i < length; i++)
            {
                token += tmp.Substring(ran.Next(0, 63), 1);
            }
            token += str;
            return token;
        }

        /// <summary>
        /// Chuyển từ tiếng việt có dấu thành tiếng việt không dấu.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="s">Chuỗi tiếng việt cần chuyển</param>
        /// <returns>Kết quả sau khi chuyển</returns>
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            temp = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            temp = temp.Replace(" ", "_");
            temp = temp.Replace(":", "");
            temp = temp.Replace("\\", "");
            temp = temp.Replace("/", "");
            temp = temp.Replace("\"", "");
            temp = temp.Replace("*", "");
            temp = temp.Replace("?", "");
            temp = temp.Replace(">", "");
            temp = temp.Replace("<", "");
            temp = temp.Replace("||", "");
            return temp;
        }

        /// <summary>
        /// Lấy dữ liệu từ cookies theo khóa, nếu không có dữ liệu thì trả về theo dữ liệu mặc định truyền vào hoặc rỗng
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="key">Khóa cần lấy dữ liệu trong cookie</param>
        /// <param name="returnDefault">Kết quả trả về mặc định nếu không có dữ liệu trong cookie, mặc định là chuỗi rỗng</param>
        /// <returns>Giá trị lưu trữ trong cookie</returns>
        public static string GetCookie(string key, string returnDefault = "")
        {
            try
            {
                string httpCookie = StartupState.Instance.Current.Request.Cookies[key];
                if (httpCookie == null)
                {
                    return returnDefault;
                }
                return Security.Base64Decode(HttpUtility.UrlDecode(httpCookie));
            }
            catch
            {
                return returnDefault;
            }
        }

        /// <summary>
        /// Xóa file theo danh sách url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrls">Danh sách url file sẽ xóa</param>
        /// <returns>True nếu xóa thành công tất cả các file. Exception nếu có lỗi.</returns>
        public static bool DeleteFiles(List<string> fileUrls)
        {
            foreach (string fileUrl in fileUrls)
            {
                Helpers.DeleteFile(fileUrl);
            }
            return true;
        }

        /// <summary>
        /// Xóa file theo danh sách url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrls">Danh sách url file sẽ xóa</param>
        /// <returns>True nếu xóa thành công tất cả các file. Exception nếu có lỗi.</returns>
        public static bool DeleteFiles(string[] fileUrls)
        {
            foreach (string fileUrl in fileUrls)
            {
                Helpers.DeleteFile(fileUrl);
            }
            return true;
        }

        /// <summary>
        /// Get file template
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="pathToTemplate"></param>
        /// <returns></returns>
        public static Stream GetFileTemplate(string templateFileName, string pathToTemplate = "/public/template/")
        {
            try
            {
                Stream stream = new MemoryStream();
                string fileName = templateFileName.EndsWith(".xlsx") ? templateFileName : $@"{templateFileName}.xlsx";
                string path = StartupState.Instance.WebHostEnvironment.WebRootPath + pathToTemplate + fileName;
                FileStream file = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                file.CopyTo(stream);
                return stream;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Xóa 1 file url đã cung cấp.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="fileUrl">Đường dẫn đến file cần xóa</param>
        /// <returns>True nếu xóa thành công file. Exception nếu có lỗi.</returns>
        public static bool DeleteFile(string fileUrl)
        {
            string path = "";
            if (!String.IsNullOrEmpty(fileUrl))
            {
                if (!fileUrl.StartsWith("~"))
                {
                    path = StartupState.Instance.WebHostEnvironment.WebRootPath + fileUrl.Substring(1, fileUrl.Length);
                }
                else
                {
                    path = StartupState.Instance.WebHostEnvironment.WebRootPath + fileUrl;
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            return true;
        }

        /// <summary>
        /// Thay thế các ký tự trong chuỗi search để có thể dùng được ở Store procedure.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi cần thay thế</param>
        /// <returns>Chuỗi sau khi đã thay thế</returns>
        public static string SqlServerEscapeString(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Replace("[", "[[]");
            str = str.Replace("%", "[%]");
            str = str.Replace("_", "[_]");
            str = str.Replace("\\", "[\\]");
            str = str.Replace("'", "''");
            return str;
        }
    }
}

