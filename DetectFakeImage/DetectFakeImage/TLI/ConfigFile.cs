using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TLI
{
    public class ConfigFile
    {
        public static readonly ConfigFile Default = new ConfigFile();
        public class ConfigInfo
        {
            public string Name;
            public object Value;
            public bool IsComment;
            public override string ToString()
            {
                return IsComment
                    ? $"#{Name}"
                    : $"{Name} = {Value.ToString()}";
            }
        }
        public Dictionary<string, ConfigInfo> Values { get; }
        public string FilePath { get; }
        ConfigFile() : this(Path.GetFileNameWithoutExtension(Application.ExecutablePath)+ ".config") { }
        public ConfigFile(string filePath)
        {
            // load config
            this.FilePath = filePath;
            this.Values = new Dictionary<string, ConfigInfo>();
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "#config");
            else
            {
                string[] arr = File.ReadAllText(filePath).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in arr)
                {
                    var ci = new ConfigInfo();
                    ci.IsComment = s.StartsWith("#");
                    if (ci.IsComment)
                    {
                        ci.Name = s.Substring(1);
                        continue;
                    }

                    int index = s.IndexOf("=");
                    if (index > 0)
                    {
                        ci.Name = s.Substring(0, index).Trim();
                        ci.Value = s.Substring(index + 1);
                    }
                    else
                        ci.Value = s;
                    this.Values[ci.Name.ToLower()]= ci;
                }
            }
        }
        public string GetString(string name, string def = "")
        {
            try
            {
                name = name.Trim().ToLower();
                return this.Values.ContainsKey(name) && !this.Values[name].IsComment ? this.Values[name].Value.ToString() : def;
            }
            catch (Exception) { 
                return def; 
            }
        }
        public int GetInt(string name, int def = 0)
        {
            try
            {
                return int.Parse(GetString(name, def.ToString()));
            }
            catch (Exception)
            {
                return def;
            }
        }
        public float GetFloat(string name, float def = 0)
        {
            try
            {
                return float.Parse(GetString(name, def.ToString()));
            }
            catch (Exception)
            {
                return def;
            }
        }
        public ConfigFile Put(string name, object value, bool comment= false)
        {
            this.Values[name]= new ConfigInfo { 
            IsComment= comment,
            Name= name,
            Value= value
            };
            return this;
        }
        public void Save()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in this.Values)
                sb.AppendLine(v.Value.ToString());
            File.WriteAllText(this.FilePath, sb.ToString());
        }
    }
}
