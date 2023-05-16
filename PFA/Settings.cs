using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PFA
{
    internal class Settings
    {
        private string _DB = "";
        private string _Language = "English";
        private string _LogFileName = "pfa.log";
        private bool _Debug = false;
        private bool _ShowToday = true;
        private string FileName = "";
        private int[] _CellColor = { 245, 245, 245 };
        private int[] _CellHColor = { 255, 255, 192 };
        private int[] _HeaderBackColor = { 192, 192, 255 };
        private bool _WindMaximized = false;
        private int _WindWidth = 740;
        private int _WindHeight = 400;
        public DBTypes DBType = DBTypes.SQLite;
        #region Properies
        public string LogFileName
        {
            get { return _LogFileName; }
            set { _LogFileName = value; }
        }
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }
        public string DB
        {
            get { return _DB; }
            set
            {
                _DB = value.Trim();
            }
        }
        public bool Debug
        {
            get { return _Debug; }
        }
        public bool ShowToday
        {
            get { return _ShowToday; }
            set { _ShowToday = value; }
        }
        public int[] CellColor
        {
            get { return _CellColor; }
        }
        public int[] CellHColor
        {
            get { return _CellHColor; }
        }
        public int[] HeaderBackColor
        {
            get { return _HeaderBackColor; }
        }
        public bool WindMaximized
        {
            get { return _WindMaximized; }
            set { _WindMaximized = value; }
        }
        public int WindWidth
        {
            get { return _WindWidth; }
            set { _WindWidth = value; }
        }
        public int WindHeight
        {
            get { return _WindHeight; }
            set { _WindHeight = value; }
        }
        public bool HideInTaskbar = true;
        #endregion
        public Settings(string filename)
        {
            string varS;
            FileName = filename;
            if (!File.Exists(FileName))
                return;
            try
            {
                using (XmlTextReader xml = new XmlTextReader(filename))
                {
                    while (xml.Read())
                    {
                        if (xml.NodeType == XmlNodeType.Element)
                        {
                            switch (xml.LocalName)
                            {
                                case "LogFileName":
                                    _LogFileName = xml.ReadString();
                                    break;
                                case "DB":
                                    varS = "";
                                    if (xml.MoveToAttribute("DBType"))
                                    {
                                        while (xml.ReadAttributeValue())
                                            varS = xml.Value;
                                    }
                                    switch (varS.ToUpper())
                                    {
                                        case "MYSQL":
                                            DBType = DBTypes.MySQL;
                                            break;
                                        case "MSSQL":
                                            DBType = DBTypes.MSSQL;
                                            break;
                                        default:
                                            DBType = DBTypes.SQLite;
                                            break;
                                    }
                                    DB = xml.ReadString();
                                    break;
                                case "Language":
                                    _Language = xml.ReadString();
                                    break;
                                case "ShowToday":
                                    ShowToday = (xml.ReadString().ToUpper().Equals("TRUE")) ? true : false;
                                    break;
                                case "Debug":
                                    _Debug = (xml.ReadString().ToUpper().Equals("TRUE")) ? true : false;
                                    break;
                                case "CellColor":
                                    varS = "0";
                                    xml.MoveToAttribute("r");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellColor[0] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("g");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellColor[1] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("b");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellColor[2] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    break;
                                case "CellHColor":
                                    varS = "0";
                                    xml.MoveToAttribute("r");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellHColor[0] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("g");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellHColor[1] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("b");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _CellHColor[2] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    break;
                                case "HeaderBackColor":
                                    varS = "0";
                                    xml.MoveToAttribute("r");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _HeaderBackColor[0] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("g");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _HeaderBackColor[1] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("b");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _HeaderBackColor[2] = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    break;
                                case "MainForm":
                                    varS = "740";
                                    xml.MoveToAttribute("width");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _WindWidth = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "400";
                                    xml.MoveToAttribute("height");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    try
                                    {
                                        _WindHeight = Convert.ToInt32(varS);
                                    }
                                    catch { }
                                    varS = "0";
                                    xml.MoveToAttribute("maximized");
                                    while (xml.ReadAttributeValue())
                                        varS = xml.Value;
                                    _WindMaximized = varS.ToUpper().Equals("TRUE") ? true : false;
                                    break;
                                case "HideInTaskbar":
                                    HideInTaskbar = (xml.ReadString().ToUpper().Equals("TRUE")) ? true : false;
                                    break;
                            }
                        }
                    }
                    xml.Close();
                }
            }
            catch { }
        }
        public void Save()
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine("<configuration>");
                    sw.WriteLine("	<application>");
                    sw.WriteLine("		<DB DBType=\"" + DBType.ToString() + "\">" + DB + "</DB>");
                    sw.WriteLine("		<LogFileName>" + LogFileName + "</LogFileName>");
                    sw.WriteLine("		<Language>" + Language + "</Language>");
                    sw.WriteLine("		<ShowToday>" + ShowToday.ToString() + "</ShowToday>");
                    if (Debug)
                        sw.WriteLine("		<Debug>" + Debug.ToString() + "</Debug>");
                    sw.WriteLine("		<CellColor r=\"" + _CellColor[0].ToString() + "\" g=\"" + _CellColor[1].ToString() + "\" b=\"" + _CellColor[2].ToString() + "\"/>");
                    sw.WriteLine("		<CellHColor r=\"" + _CellHColor[0].ToString() + "\" g=\"" + _CellHColor[1].ToString() + "\" b=\"" + _CellHColor[2].ToString() + "\"/>");
                    sw.WriteLine("		<HeaderBackColor r=\"" + _HeaderBackColor[0].ToString() + "\" g=\"" + _HeaderBackColor[1].ToString() + "\" b=\"" + _HeaderBackColor[2].ToString() + "\"/>");
                    sw.WriteLine("		<MainForm width=\"" + WindWidth.ToString() + "\" height=\"" + WindHeight.ToString() + "\" maximized=\"" + WindMaximized.ToString() + "\"/>");
                    sw.WriteLine("		<HideInTaskbar>" + HideInTaskbar.ToString() + "</HideInTaskbar>");
                    sw.WriteLine("	</application>");
                    sw.WriteLine("</configuration>");
                    sw.Close();
                }
                fs.Close();
            }

        }

    }
}
