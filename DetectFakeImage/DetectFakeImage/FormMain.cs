
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TLI;
using TLI.FakeImage;
using TLI.WinForm;
using static TLI.FakeImage.FakeImageDetection;

namespace FakeImageDetection
{
    public partial class FormMain : Form
    {
        private Dictionary<string, FakeImageInfo> fileInfos = new Dictionary<string, FakeImageInfo>();
        private LearningConfirmDialog dlgUpdateData = new LearningConfirmDialog();
        
        public FormMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image|*.jpg;*.png;*.jpge;*.bmp|All|*.*";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.detect(dlg.FileNames);
            }
        }
        private void btnBrowserFolder_Click(object sender, EventArgs e)
        {
            OnpenFolderDialog dlg = new OnpenFolderDialog();
            dlg.Title = "Chọn thư mục chứa ảnh";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.detect(GetAllFiles(dlg.SelectedPath));
            }
        }
        private void btnCheckImage_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(this.onCheckImage)).Start();
        }

        void setEnable(bool enable)
        {
            this.webBrowser1.Visible = enable;
            this.buttonPanel.Enabled = enable;
            this.btnCheckImage.Enabled = enable && this.fileInfos.Any();
            
            this.Cursor = enable? Cursors.Default: Cursors.WaitCursor;
        }
        private void detect(string[] files)
        {
            setEnable(false);
            fileInfos.Clear();
            foreach (var f in files)
                fileInfos.Add(f, null);
            new Thread(new ThreadStart(this.onDetect)).Start();
        }
        
        int totalAttr = 0;
        private void onDetect()
        {
            setEnable(false);
            this.webBrowser1.DocumentText = "";
            this.lbFake.Text = "0";
            this.lbReal.Text = "0";
            this.totalAttr = 0;

            var sb = new StringBuilder();
            var filePaths = this.fileInfos.Keys.ToArray();
            FakeImageDAO dao = new FakeImageDAO();
            foreach (var f in filePaths)
            {
                var li = dao.GetInfo(f);
                this.fileInfos[f] = li;
                sb = buildInfo(sb, dao, li, false);

                totalAttr += li.attrCount;
            }

            webBrowser1.DocumentText = "<html style='font-farmily:monospace;font-size:15px;'>"
                + "<h1>Image Metadata</h1>"
                    + sb.ToString().Replace('\0', ' ').Replace("\n", "</br>")
                    + "</html>";
            setEnable(true);
        }
        private void onCheckImage()
        {
            setEnable(false);
            this.webBrowser1.DocumentText = "";
            this.lbFake.Text = "0";
            this.lbReal.Text = "0";

            var sb = new StringBuilder();
            FakeImageDAO dao = new FakeImageDAO();
            if (dao.SampleData == null)
            {
                var json = File.ReadAllText(CACHE_FILEPATH);
                dao.SampleData = JsonConvert.DeserializeObject<Dictionary<long, FakeResult>>(json);
            }

            foreach (var f in this.fileInfos)
            {
                sb = buildInfo(sb, dao, f.Value, true);
            }

            webBrowser1.DocumentText = "<html style='font-farmily:monospace;font-size:15px;'>"
                + "<h1>Fake Image Detection</h1>"
                    + sb.ToString().Replace('\0', ' ').Replace("\n", "</br>")
                    + "</html>";
            setEnable(true);
        }
        StringBuilder buildInfo(StringBuilder sb, FakeImageDAO dao, FakeImageInfo li, bool checkImage)
        {
            sb.AppendLine();
            if (checkImage)
            {
                var outResults = new List<FakeResult>();
                var la = new List<FakeImageMetadata>();
                foreach (var x in li.attrs)
                {
                    var vi = li.values[x.Key];
                    la.Add(new FakeImageMetadata
                    {
                        attrId = vi.attrId,
                        valueLength = vi.valueLength,
                        valueText = vi.valueText,
                        valueType = (int)vi.valueType
                    } );
                }

                var isFake = dao.IsFake(la, ref outResults);
                if (isFake)
                {
                    sb.AppendLine($"<span style='color:#800000'><b>Fake image by {string.Join(", ", outResults.ConvertAll(x => $"{x.attrId}= '{x.valueText}'"))}</b>");
                    this.lbFake.Text = (int.Parse(this.lbFake.Text) + 1).ToString();
                }
                else
                {
                    sb.AppendLine($"<span style='color:green'><b>Real image</b>");
                    this.lbReal.Text = (int.Parse(this.lbReal.Text) + 1).ToString();
                }
            }

            sb.AppendLine($"<b>{li.filePath}</b>");
            sb.AppendLine($"Total: <b>{li.attrCount}</b> attributes");

            foreach (var x in li.attrs)
            {
                var vi = li.values[x.Key];
                var valueTypeString = $"{vi.valueType.ToString().ToLower()}[{vi.valueLength}]";
                sb.Append($"<div>{x.Key}: {vi.valueText} <span style='color:gray;font-size:14'>{valueTypeString}</span></div>");
            }

            return sb;
        }
        public static string[] GetAllFiles(String directory)
        {
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        }

        private void btnLearnFake_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(this.onLearn)).Start();
        }
        public void onLearn()
        {
            FakeImageDAO dao = new FakeImageDAO();
            if (this.fileInfos.Count <= 0)
            {
                dao.UpdateLearnData();
                setEnable(true);
                return;
            }
            
            if (this.dlgUpdateData.ShowDialog()== DialogResult.OK)
            {
                setEnable(false);
                this.webBrowser1.DocumentText = "";
                this.lbFake.Text = "0";
                this.lbReal.Text = "0";

                int totalFile = 0;
                int totalAttr = 0;
                
                var sb = new StringBuilder();
                foreach (var f in this.fileInfos)
                {
                    var li = dao.Learn(f.Key, dlgUpdateData.IsFake);
                    sb.AppendLine($"<br/><b>{f.Key}</b>");
                    sb.AppendLine($"New: <b>{li.isNewFile}</b>, <b>{li.newAttrCount}/{li.attrCount}</b> attributes");

                    foreach (var x in li.attrs)
                    {
                        var vi = li.values[x.Key];

                        var valueTypeString = $"{vi.valueType.ToString().ToLower()}[{vi.valueLength}]";
                        sb.Append($"<div>{x.Key}: {vi.valueText} <span style='color:gray;font-size:14'>{valueTypeString}</span></div>");
                    }

                    totalFile++;
                    totalAttr += li.attrCount;

                    if (li.isFake)
                        this.lbFake.Text = (int.Parse(this.lbFake.Text) + 1).ToString();
                    else
                        this.lbReal.Text = (int.Parse(this.lbReal.Text) + 1).ToString();
                }

                dao.UpdateLearnData();

                sb.AppendLine();

                setEnable(true);
                webBrowser1.DocumentText = "<html style='font-farmily:monospace;font-size:15px;'>"
                    + "<h1>Fake Image Learning</h1>"
                        + sb.ToString().Replace('\0', ' ').Replace("\n", "</br>")
                        + "</html>";
            }
        }

        const string CACHE_FILEPATH = "fake-images.json";
        private void btnCreateCache_Click(object sender, EventArgs e)
        {
            if (File.Exists(CACHE_FILEPATH))
            {
                webBrowser1.DocumentText = File.ReadAllText(CACHE_FILEPATH);
            }
            else
                new Thread(new ThreadStart(onCreateCache)).Start();
        }

        private void btnResetCache_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(CACHE_FILEPATH);
            }
            catch (Exception) { }
            this.btnCreateCache_Click(null, null);
        }
        void onCreateCache()
        {
            setEnable(false);
            this.webBrowser1.DocumentText = "";
            this.lbFake.Text = "0";
            this.lbReal.Text = "0";

            var d = new FakeImageDAO();
            d.SampleData= d.CreateSampleData();

            var s = new List<string>();
            foreach (var x in d.SampleData)
                s.Add(x.Key+":"+x.Value.ToJSON());

            string rs = "{" + string.Join(",", s) + "}";
            webBrowser1.DocumentText = rs;
            File.WriteAllText(CACHE_FILEPATH, rs);
            setEnable(true);
        }
    }

    public static class FakeResultExtension
    {
        static string qoute(string s)
        {
            return $"\"{s}\"";
        }
        static string qoute(string name, object value)
        {
            return qoute(name) + ":" + value.ToString();
        }
        static string qoute(string name, string value)
        {
            return qoute(name) + ":" + qoute(value.Trim().Replace("\"", "\\\""));
        }
        public static string ToJSON(this FakeResult r)
        {
            var s = new List<string>();
            r.childs.ForEach(x => s.Add(x.ToJSON()));

            return "{"
                + string.Join(",",
                //qoute("id", r.id),
                qoute("attrId", r.attrId),
                qoute("valueText", r.valueText),
                qoute("fakeCount", r.fakeCount),
                 //qoute("isCombine", isCombine ? 1 : 0),
                 //qoute("parentId", parentId),
                 $"\"childs\":[{string.Join(",", s)}]"
                 )
            + "}";
        }
    }
}
