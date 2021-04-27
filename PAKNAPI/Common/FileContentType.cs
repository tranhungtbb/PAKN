using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace PAKNAPI.Common
{
    public static class FileContentType
    {
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public static string GetTypeOfFile(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext;
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".rar", "application/x-rar-compressed"},
                {".zip", "application/zip"},
                {".7z", "application/x-7z-compressed"},
                {".aac", "audio/aac"},
                {".mp3", "audio/mpeg"},
                {".oga", "audio/ogg"},
                {".wav", "audio/wav"},
                {".weba", "audio/webm"},
                {".3gpp", "audio/3gpp"},
                {".3gpp2", "audio/3gpp2"},
                {".avi", "video/x-msvideo"},
                {".mpeg", "video/mpeg"},
                {".ogv", "video/ogg"},
                {".webm", "video/webm"},
                {".3gp", "video/3gpp"},
                {".3g2", "video/3gpp2"},
                {".wmv", "video/x-ms-wmv"},
                {".mp4", "video/mp4"},
            };
        }
    }
}
