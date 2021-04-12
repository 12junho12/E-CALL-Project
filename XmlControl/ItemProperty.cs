using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AUA_FCT_EDITOR.XmlControl
{
    public static class ConstAttriName
    {
        //Element
        //Sub-Element
        //Sub-M Element
        //Sub-N Element
        //순서가 쌍으로 되어야 정상적으로 읽어들인다. 아래 이름과 EqSettingValue의 이름의 순서가 동일해야한다. 

        //Sub-Element  =>Setting
        public const string SSID = "SID"; // no
        public const string SNAM = "NAM"; //name
        public const string SDEC = "DEC"; //Description
        public const string SNUB = "NUB";   //Number
        public const string SSNU = "SNU";   //ShortNum
        public const string SBCK = "BCK";   //BCR_Check
        public const string SJIG = "JIG";        //JigID
        public const string SLBT = "LBT";       //LabelType
        public const string SLBC = "LBC";   //LabelColor
        public const string SNOT = "NOT"; //note
        //Sub-L Element => Label
        public const string LDEC = "LDEC";   //description
        public const string LHWC = "LHWC";   //hw char
        public const string LHAW = "LHAW";   //hw
        public const string LHPX = "LHPX";   //hw position X
        public const string LHPY = "LHPY";   //hw position Y
        public const string LSWC = "LSWC";   //sw char
        public const string LSOW = "LSOW";   //sw
        public const string LSPX = "LSPX";   //sw position X
        public const string LSPY = "LSPY";   //sw position Y
        public const string LLNO = "LLNO";   //Label number
        public const string LLPX = "LLPX";   //Label position X
        public const string LLPY = "LLPY";   //Label position Y
        public const string LNAM = "LNAM";   //name
        public const string LNPX = "LNPX";   //Name Position X
        public const string LNPY = "LNPY";   //Name Position Y
        public const string LLOT = "LLOT";   // Lot 
        public const string LLOX = "LLOX";   // Lot Position X
        public const string LLOY = "LLOY";   // Lot Position Y
        public const string LD1X = "LD1X";   // 1D Position X
        public const string LD1Y = "LD1Y";   // 1D Position Y
        public const string LD2X = "LD2X";   // 2D Position X
        public const string LD2Y = "LD2Y";   // 1D Position Y
        public const string LUSB = "LUSB";   //Use Barcode Text
        public const string LBTX = "LBTX";   //BarcodeTextPosX
        public const string LBTY = "LBTY";   //BarcodeTextPosY
        public const string LDMS = "LDMS";   //Dimension
        public const string LDIX = "LDIX";   //divisionX
        public const string LDIY = "LDIY";   //divisionY
        public const string LTRX = "LTRX";   //TraceX
        public const string LTRY = "LTRY";   //TraceY


        public const string LDAK = "LDAK";   //Darkness
        public const string LFTS = "LFTS";   //Font Size
        public const string LWID = "LWID";   //Width
        public const string LRAT = "LRAT";   //Ratio
        public const string LHEI = "LHEI";   //Height
        public const string LCAC = "LCAC";   //car code
        public const string LOPT = "LOPT";   //Option
        public const string LSST = "LSST";   //Serial Start
        public const string LCNT = "LCNT";   //Count
        public const string LCOC = "LCOC";   //Company Code
        public const string LPAR = "LPAR";   //PART_NUMBER
        public const string LSQC = "LSQC";   //sequence code
        public const string LEON = "LEON";   //EO No
        public const string LTRC = "LTRC";   //Trace code
        public const string LCOI = "LCOI";   //Company information
        public const string LINI = "LINI";   //Inital 초도품
        public const string LDIV = "LDIV";   //모델 구분 천/인조
        public const string LTRD = "LTRD";   //Trace description
        public const string LYEA = "LYEA";   //Year
        public const string LMTH = "LMTH";   //Month
        public const string LDAY = "LDAY";   //Day


        //     
        public const string DATA = "DATA";
        public const string SMDL = "MDL"; // model detail list
        public const string TITL = "AUA_FunctionTest_Editor"; //Title
        public const string EUPI = "EQUIPINFO";
        public const string EINI = "EQINIINFO";
        public const string STGI = "SETTINGINFO";
        public const string MELI = "MODELINFO";
        public const string LBLI = "LABELINFO";
        public const string NENO = "NEWNODE";
        //Element  => Equipment
        public const string ESID = "SID";
        public const string ENAM = "NAM";
        public const string EDEC = "DEC";

        public const string ENWI = "NWI"; //NETWORK_INFO
        public const string EFDI = "FDI"; //FOLDER_INFO
        public const string ESET = "SET"; //SETTING
        public const string ELSI = "LSI"; //LABEL_SERIAL_INFO
        public const string EBLI = "BLI"; //BIXOLON_INFO
        public const string ELBL = "LBL"; //Label info

        public const string ESTL = "STL";
        //Sub-M Element => Model
        public const string MNAM = "NAM";
        public const string MDEC = "DEC";
        public const string MMSM = "MSM";
        public const string MRST = "RST";
        public const string MMIN = "MIN";
        public const string MMEX = "MEX";
        public const string MENB = "ENB";

        public const string MPR0 = "PR0";
        public const string MPR1 = "PR1";
        public const string MPR2 = "PR2";
        public const string MPR3 = "PR3";
        public const string MPR4 = "PR4";
        public const string MPR5 = "PR5";
        public const string MPR6 = "PR6";
        public const string MPR7 = "PR7";
        public const string MPR8 = "PR8";
        public const string MPR9 = "PR9";

        //Sub-N Element  =>???
        public const string NSID = "SID";
        public const string NNAM = "NAM";
        public const string NDEC = "DEC";

        public const string NPR1 = "PR1";
        public const string NPR2 = "PR2";
        public const string NPR3 = "PR3";
        public const string NPR4 = "PR4";
        public const string NPR5 = "PR5";
    }

    public class Equipment
    {

        [ReadOnly(true)]
        [CategoryAttribute("Equipment Information")]
        public string Equipid { get; set; }
        [CategoryAttribute("Equipment Information")]
        public string Name { get; set; }
        [CategoryAttribute("Equipment Information")]
        public string Description { get; set; }
        [CategoryAttribute(" Ini Setting Information")]
        public EqIniSettingValue IniValue { get; set; }

        [Browsable(false)]
        [CategoryAttribute("Setting Information List")]
        public List<EqSettingValue> SettingValue { get; set; }

        public Equipment()
        {
            Equipid = "0";
            Name = "Name";
            Description = "Description";
        }

    }

    public class EqIniSettingValue
    {

        [CategoryAttribute("Equipment Ini Information")]
        public string NetworkInfo { get; set; }
        [CategoryAttribute("Equipment Ini Information")]
        public string FolderInfo { get; set; }
        [CategoryAttribute("Equipment Ini Information")]
        public string Setting { get; set; }
        [CategoryAttribute("Equipment Ini LabelInformation")]
        public string LabelSerialInfo { get; set; }
        [CategoryAttribute("Equipment Ini Information")]
        public string BixolonInfo { get; set; }
        [CategoryAttribute("Equipment Ini Label Information")]
        public string LabelList { get; set; }

        public EqIniSettingValue()
        {

        }
    }

    public class EqSettingValue
    {
        [ReadOnly(true)]
        [CategoryAttribute("0.Setting Information")]
        public int Setid
        {
            get;
            set;
        }
        [CategoryAttribute("0.Setting Information")]
        public string SetName { get; set; }
        [CategoryAttribute("0.Setting Information")]
        public string SetDescription { get; set; }
        [CategoryAttribute("item List")]
        public string SetModelNumber { get; set; }
        [CategoryAttribute("item List")]
        public string SetModelShortNumber { get; set; }
        [CategoryAttribute("item List")]
        public string SetBCRCheck { get; set; }
        [CategoryAttribute("item List")]
        public string SetJigID { get; set; }
        [CategoryAttribute("item List")]
        public string SetLabelType { get; set; }
        [CategoryAttribute("item List")]
        public string SetLabelColor { get; set; }
        [CategoryAttribute("item List")]
        public string SetNote { get; set; }

        // Label을 한곳에 모았다. 
        [CategoryAttribute("Label Information")]
        public string LabelDescription { get; set; }

        //Label Position
        [CategoryAttribute("Label Position")]
        public string HWChar { get; set; }
        [CategoryAttribute("Label Position")]
        public string HW { get; set; }
        [CategoryAttribute("Label Position")]
        public string HWPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string HWPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string SWChar { get; set; }
        [CategoryAttribute("Label Position")]
        public string SW { get; set; }
        [CategoryAttribute("Label Position")]
        public string SWPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string SWPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelNumber { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelNumberPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelNumberPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelName { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelNamePosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string LabelNamePosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string LotNoChar { get; set; }
        [CategoryAttribute("Label Position")]
        public string LotNoCharPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string LotNoCharPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string D1PosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string D1PosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string D2PosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string D2PosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string BarcodeTextUse { get; set; }
        [CategoryAttribute("Label Position")]
        public string BarcodeTextPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string BarcodeTextPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string Dimension { get; set; }
        [CategoryAttribute("Label Position")]
        public string DivisionPosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string DivisionPosY { get; set; }
        [CategoryAttribute("Label Position")]
        public string TracePosX { get; set; }
        [CategoryAttribute("Label Position")]
        public string TracePosY { get; set; }

        //Label Font
        [CategoryAttribute("Label Zone")]
        public string DARKNESS { get; set; }
        [CategoryAttribute("Label Zone")]
        public string FONT_SIZE { get; set; }
        [CategoryAttribute("Label Zone")]
        public string WIDTH { get; set; }
        [CategoryAttribute("Label Zone")]
        public string RATIO { get; set; }
        [CategoryAttribute("Label Zone")]
        public string HEIGHT { get; set; }

        //Label Zone
        [CategoryAttribute("Label Zone")]
        public string CAR_CODE { get; set; }
        [CategoryAttribute("Label Zone")]
        public string OPTION { get; set; }
        [CategoryAttribute("Label Zone")]
        public string SERIAL_START { get; set; }
        [CategoryAttribute("Label Zone")]
        public string COUNT { get; set; }

        [CategoryAttribute("Label Zone")]
        public string COMPANY_CODE { get; set; }
        [CategoryAttribute("Label Zone")]
        public string PART_NUMBER { get; set; }
        [CategoryAttribute("Label Zone")]
        public string SEQ_CODE { get; set; }
        [CategoryAttribute("Label Zone")]
        public string EONO { get; set; }
        [CategoryAttribute("Label Zone")]
        public string TRACE_CODE { get; set; }
        [CategoryAttribute("Label Zone")]
        public string COMPANY_INFO { get; set; }
        [CategoryAttribute("Label Zone")]
        public string INITIAL { get; set; }
        [CategoryAttribute("Label Zone")]
        public string DIVISION { get; set; }
        [CategoryAttribute("Label Zone")]
        public string TRACE_DESCRIPTION { get; set; }

        //Label Date
        [CategoryAttribute("Label Date")]
        public string YEAR { get; set; }
        [CategoryAttribute("Label Date")]
        public string MONTH { get; set; }
        [CategoryAttribute("Label Date")]
        public string DAY { get; set; }

        public EqSettingValue()
        {
            Setid = 0;
            //SetName = "Setting Name";
            SetDescription = "";
            SetModelNumber = "";
            SetModelShortNumber = "";
            SetBCRCheck = "";
            SetJigID = "";
            SetLabelType = "";
            SetLabelColor = "";
            SetNote = "";

            // Label part
            LabelName = "";
            LabelDescription = "";

            Dimension = "";
            HWChar = "";
            HW = "";
            HWPosX = "";
            HWPosY = "";
            SWChar = "";
            SW = "";
            SWPosX = "";
            SWPosY = "";
            LabelNumber = "";
            LabelNumberPosX = "";
            LabelNumberPosY = "";
            LabelNamePosX = "";
            LabelNamePosY = "";
            LotNoChar = "";
            LotNoCharPosX = "";
            LotNoCharPosY = "";
            BarcodeTextUse = "";
            BarcodeTextPosX = "";
            BarcodeTextPosY = "";

            TracePosX = "";
            TracePosY = "";
            DivisionPosX = "";
            DivisionPosY = "";

            D1PosX = "";
            D1PosY = "";
            D2PosX = "";
            D2PosY = "";

            YEAR = "";
            MONTH = "";
            DAY = "";
            CAR_CODE = "";
            OPTION = "";
            SERIAL_START = "";
            COUNT = "";
            DARKNESS = "";
            FONT_SIZE = "";
            WIDTH = "";
            RATIO = "";
            HEIGHT = "";
            COMPANY_CODE = "";
            PART_NUMBER = "";
            SEQ_CODE = "";
            EONO = "";
            TRACE_CODE = "";
            COMPANY_INFO = "";
            INITIAL = "";
            DIVISION = "";
        }

        public EqSettingValue(int idNum)
        {
            Setid = idNum;
            SetName = "";
            SetDescription = "";
            SetModelNumber = "";
            SetModelShortNumber = "";
            SetBCRCheck = "";
            SetJigID = "";
            SetLabelType = "";
            SetLabelColor = "";
            SetNote = "";

            // Label part
            LabelName = "";
            LabelDescription = "";

            Dimension = "";
            HWChar = "";
            HW = "";
            HWPosX = "";
            HWPosY = "";
            SWChar = "";
            SW = "";
            SWPosX = "";
            SWPosY = "";
            LabelNumber = "";
            LabelNumberPosX = "";
            LabelNumberPosY = "";
            LabelNamePosX = "";
            LabelNamePosY = "";
            LotNoChar = "";
            LotNoCharPosX = "";
            LotNoCharPosY = "";
            BarcodeTextUse = "";
            BarcodeTextPosX = "";
            BarcodeTextPosY = "";

            TracePosX = "";
            TracePosY = "";
            DivisionPosX = "";
            DivisionPosY = "";

            D1PosX = "";
            D1PosY = "";
            D2PosX = "";
            D2PosY = "";

            YEAR = "";
            MONTH = "";
            DAY = "";
            CAR_CODE = "";
            OPTION = "";
            SERIAL_START = "";
            COUNT = "";
            DARKNESS = "";
            FONT_SIZE = "";
            WIDTH = "";
            RATIO = "";
            HEIGHT = "";
            COMPANY_CODE = "";
            PART_NUMBER = "";
            SEQ_CODE = "";
            EONO = "";
            TRACE_CODE = "";
            COMPANY_INFO = "";
            INITIAL = "";
            DIVISION = "";

        }

        public EqSettingValue ShallowCopy()
        {
            return (EqSettingValue)this.MemberwiseClone();
        }


        public EqSettingValue DeepCopy()
        {
            EqSettingValue other = (EqSettingValue)this.MemberwiseClone();
            return other;
        }
    }

}
