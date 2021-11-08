
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
using static TLI.FakeImage.FakeImageDAO;

namespace FakeImageDetection
{
    public partial class FormMain : Form
    {
        private List<string> filePaths = new List<string>();
        private List<MetaInfo> fileInfos = new List<MetaInfo>();
        private UpdateDataConfirm dlgUpdateData = new UpdateDataConfirm();

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
        
        void setEnable(bool enable)
        {
            this.btnBrowseImage.Enabled = enable;
            this.btnBrowserFolder.Enabled = enable;
            this.btnLearn.Enabled = enable;
            this.Cursor = enable? Cursors.Default: Cursors.WaitCursor;
        }
        private void detect(string[] files)
        {
            setEnable(false);
            this.filePaths.Clear();
            this.filePaths.AddRange(files);
            new Thread(new ThreadStart(this.onDetect)).Start();
        }

        private void onDetect()
        {
            this.webBrowser1.DocumentText = "";
            this.lbFake.Text = "0";
            this.lbReal.Text = "0";
            
            int totalFile = 0;
            int totalAttr = 0;
            var dao = new FakeImageDAO();
            var sb = new StringBuilder();
            this.fileInfos.Clear();
            foreach (var f in this.filePaths)
            {
                var li = dao.GetInfo(f);
                this.fileInfos.Add(li);

                var la = new List<CheckImageParams>();
                foreach(var x in li.attrs)
                {
                    var vi = li.values[x.Key];
                    la.Add(new CheckImageParams
                    {
                        attrId = vi.attrId,
                        valueLength = vi.valueLength,
                        valueText = vi.valueText,
                        valueType = (int)vi.valueType
                    }
                    );
                }

                sb.AppendLine();
                var rsCheck = dao.CheckImage(la);
                if (rsCheck!= null)
                {
                    sb.AppendLine($"<span style='color:#800000'><b>Fake image by {rsCheck.attrId}= '{rsCheck.valueText}'</b>");
                    this.lbFake.Text = (int.Parse(this.lbFake.Text) + 1).ToString();
                }
                else
                {
                    sb.AppendLine($"<span style='color:green'><b>Real image</b>");
                    this.lbReal.Text = (int.Parse(this.lbReal.Text) + 1).ToString();
                }
                sb.AppendLine($"<b>{f}</b>");
                sb.AppendLine($"Total: <b>{li.attrCount}</b> attributes");

                foreach (var x in li.attrs)
                {
                    var vi = li.values[x.Key];

                    var valueTypeString = $"{vi.valueType.ToString().ToLower()}[{vi.valueLength}]";
                    sb.Append($"<div>{x.Key}: {vi.valueText} <span style='color:gray;font-size:14'>{valueTypeString}</span></div>");
                }

                totalFile++;
                totalAttr += li.attrCount;
            }
            sb.AppendLine();

            setEnable(true);
            webBrowser1.DocumentText = "<html style='font-farmily:monospace;font-size:15px;'>"
                +"<h1>Fake Image Detection</h1>"
                    + sb.ToString().Replace('\0', ' ').Replace("\n", "</br>")
                    + "</html>";

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
            setEnable(false);

            var dao = new FakeImageDAO();
            if (this.fileInfos.Count <= 0)
            {
                dao.UpdateLearnData();
                setEnable(true);
                return;
            }
            
            if (this.dlgUpdateData.ShowDialog()== DialogResult.OK)
            {
                this.webBrowser1.DocumentText = "";
                this.lbFake.Text = "0";
                this.lbReal.Text = "0";

                int totalFile = 0;
                int totalAttr = 0;
                
                var sb = new StringBuilder();
                foreach (var fi in this.fileInfos)
                {
                    var li = dao.Learn(fi, dlgUpdateData.IsFake);
                    sb.AppendLine($"<br/><b>{fi.filePath}</b>");
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
    }
}
