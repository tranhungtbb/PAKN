using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TLI.FakeImage
{
    public class FakeImageDetection : IDisposable
    {
        System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
        protected FakeImageDetection() { }
        public FakeImageDetection(Dictionary<long, FakeResult> sampleData)
        {
            if (sampleData == null)
                throw new ArgumentNullException("sampleData");
            this.SampleData = sampleData;
        }
        
        protected virtual string getPropName(string name, string def)
        {
            return def;
        }

        public class FakeImageInfo
        {
            public string filePath;
            public Bitmap bitmap;
            public int attrCount;
            public Dictionary<int, FakeAttrs> attrs;
            public Dictionary<int, FakeImageAttrs> values;
            public Exception error;
        }
        public enum FakeStatus { DEFAULT, DISABLE = -1 }
        public class FakeAttrs
        {
            public long id;
            public string name;
            public string desc;
            public FakeStatus status;
        }

        public class FakeImageAttrs
        {
            public long fileId;
            public int attrId;
            public FakeValueTypes valueType;
            public int valueLength;
            public string valueText;
        }
        public enum FakeValueTypes
        {
            Byte = 1,
            /// <summary>
            /// An array of Byte objects encoded as ASCII
            /// </summary>
            ASCII = 2, // 
            Int16 = 3,
            Int32 = 4,
            NotUsed6 = 6,
            Undefined = 7,
            NotUsed8 = 8,
            /// <summary>
            ///  An array of two Byte objects that represent a rational number
            /// </summary>           
            Rational = 5,
            SLong = 9,
            SRational = 10
        }
        public FakeImageInfo GetInfo(string filePath)
        {
            var metaInfo = new FakeImageInfo
            {
                filePath = filePath,
                attrCount = 0,
                attrs = new Dictionary<int, FakeAttrs>(),
                values = new Dictionary<int, FakeImageAttrs>(),
            };

            try
            {
                metaInfo.bitmap = new Bitmap(filePath);

                PropertyItem[] propItems = metaInfo.bitmap.PropertyItems;
                foreach (PropertyItem pi in propItems)
                {
                    try
                    {
                        string valueText = "";
                        var valueType = (FakeValueTypes)pi.Type;
                        try
                        {
                            switch (valueType)
                            {
                                case FakeValueTypes.Byte:
                                    valueText = BitConverter.ToSingle(pi.Value, 0).ToString() ?? "0";
                                    break;
                                case FakeValueTypes.ASCII:
                                    valueText = asciiEncoding.GetString(pi.Value).Trim().ToLower().Replace("\0", "");
                                    break;
                                case FakeValueTypes.Int16:
                                    valueText = BitConverter.ToInt16(pi.Value, 0).ToString();
                                    break;
                                case FakeValueTypes.Int32:
                                    valueText = BitConverter.ToInt32(pi.Value, 0).ToString();
                                    break;
                                case FakeValueTypes.Rational:
                                case FakeValueTypes.NotUsed6:
                                case FakeValueTypes.Undefined:
                                case FakeValueTypes.NotUsed8:
                                case FakeValueTypes.SRational:
                                case FakeValueTypes.SLong:
                                    valueText = BitConverter.ToString(pi.Value, 0).Replace("-", " ");
                                    if (valueText.Length > 255)
                                        valueText = valueText.Substring(0, 250) + "...";
                                    valueText = valueText.Replace("\0", "");
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        string id = pi.Id.ToString();
                        var valueTypeString = $"{valueType.ToString().ToLower()}[{pi.Len}]";
                        string name = getPropName(id, id) + $" {valueTypeString}";
                        metaInfo.attrCount++;
                        metaInfo.attrs[pi.Id] = new FakeAttrs
                        {
                            id = pi.Id,
                            name = name,
                            status = 0
                        };
                        metaInfo.values[pi.Id] = new FakeImageAttrs
                        {
                            attrId = pi.Id,
                            fileId = 0,
                            valueLength = pi.Len,
                            valueText = valueText,
                            valueType = valueType
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                metaInfo.error = e;
            }
            return metaInfo;
        }
        public List<FakeImageMetadata> GetMetadata(string filePath)
        {
            var info = this.GetInfo(filePath);
            var rs = new List<FakeImageMetadata>();
            foreach(var x in info.values)
            {
                rs.Add(new FakeImageMetadata
                {
                    attrId = x.Value.attrId,
                    valueLength = x.Value.valueLength,
                    valueText = x.Value.valueText,
                    valueType = (int)x.Value.valueType
                });
            }
            return rs;
        }
        public class FakeResult
        {
            public long id;
            public long attrId;
            public string valueText;
            public int fakeCount;
            public bool isCombine;
            public long parentId;
            public List<FakeResult> childs = new List<FakeResult>();
        }
        public class FakeImageMetadata
        {
            public long attrId;
            public int valueType;
            public int valueLength;
            public string valueText;
        }
        public Dictionary<long, FakeResult> SampleData { get; protected set; }

        public bool IsFake(List<FakeImageMetadata> param, ref List<FakeResult> outResults)
        {
            var map = new Dictionary<long, FakeImageMetadata>();
            foreach (var x in param)
            {
                x.valueText = x.valueText.Trim().ToLower();
                map.Add(x.attrId, x);
            }

            //
            var stacks = new Stack<FakeResult>();
            FakeResult rs;
            long attrId;
            FakeImageMetadata p;
            foreach (var x in SampleData)
            {
                stacks.Push(x.Value);
                while (stacks.Any())
                {
                    rs = stacks.Pop();
                    attrId = rs.attrId;
                    p = map.ContainsKey(attrId) ? map[attrId] : new FakeImageMetadata { attrId = attrId, valueText = "" };
                    if (p.valueText == rs.valueText)
                    {
                        outResults.Add(rs);
                        if (rs.childs.Count <= 0)
                        {
                            outResults.Clear();
                            while (rs != null)
                            {
                                outResults.Insert(0, rs);
                                rs = SampleData.ContainsKey(rs.parentId) ? SampleData[rs.parentId] : null;
                            }

                            return true;
                        }

                        rs.childs.ForEach(c => stacks.Push(c));
                    }
                }

            }

            return false;
        }
      
        public bool IsFake(List<FakeImageMetadata> param)
        {
            var outResults = new List<FakeResult>();
            return IsFake(param,ref outResults);
        }
        public virtual void Dispose()
        {
        }
    }
}
