using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUA.AiS_FruiT
{
    class Model
    {
        public string Number { get; set; }
        public string ShortNum { get; set; }
        public string Name { get; set; }
        public string JigID { get; set; }
        public string BCR_Check { get; set; }
        public string note{ get; set; }

        public string Image { get; set; }
        public string LabelColor { get; set; }
        public string LabelImage { get; set; }
        public string LabelType { get; set; }


        //Label
        //Label Position
        public string HWChar { get; set; }
        public string HW { get; set; }
        public string HWPosX { get; set; }
        public string HWPosY { get; set; }
        public string SWChar { get; set; }
        public string SW { get; set; }
        public string SWPosX { get; set; }
        public string SWPosY { get; set; }
        public string LabelNumber { get; set; }
        public string LabelNumberPosX {
            get;
            set; }
        public string LabelNumberPosY {
            get;
            set; }
        public string LabelName { get; set; }
        public string LabelNamePosX { get; set; }
        public string LabelNamePosY { get; set; }
        public string LotNoChar { get; set; }

        public string LotNoCharPosX { get; set; }
        public string LotNoCharPosY { get; set; }
        public string D1PosX { get; set; }
        public string D1PosY { get; set; }
        public string D2PosX { get; set; }
        public string D2PosY { get; set; }
        public string BarcodeTextUse { get; set; }
        public string BarcodeTextPosX { get; set; }
        public string BarcodeTextPosY { get; set; }
        public string Dimension { get; set; }

        //Label Font
        public string FONT_SIZE { get; set; }
        public string WIDTH { get; set; }
        public string RATIO { get; set; }
        public string HEIGHT { get; set; }

        //Label Zone

        public string CAR_CODE { get; set; }
        public string OPTION { get; set; }
        public string SERIAL_START { get; set; }
        public string COUNT { get; set; }
        public string COMPANY_CODE { get; set; }
        public string SEQ_CODE { get; set; }
        public string EONO { get; set; }
        public string TRACE_CODE { get; set; }
        public string COMPANY_INFO { get; set; }

        //Label Date

        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string DAY { get; set; }

    }

}
