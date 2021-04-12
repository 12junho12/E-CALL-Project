using System;
using System.Reflection;
using System.Xml;

namespace AUA_FCT_EDITOR.XmlControl
{
    class XmlWrite
    {
        public static Equipment _Equipment = null;

        public static void CreateXmlFile(Equipment equipmentItem, string xmlFilePath)
        {

            if (equipmentItem == null) equipmentItem = new Equipment();

            XmlDocument NewXmlDoc = new XmlDocument();

            XmlNode Source = NewXmlDoc.CreateElement("", ConstAttriName.TITL, "");
            NewXmlDoc.AppendChild(Source);
            NewXmlDoc.Save(xmlFilePath);
            //--
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(xmlFilePath);
        }

        public static void WriteXmlFile(Equipment equipmentItem, string xmlFilePath)
        {
            _Equipment = equipmentItem;
            //--
            XmlDocument XmlDoc = new XmlDocument();
            XmlNode Source = XmlDoc.CreateElement("", ConstAttriName.TITL, "");
            XmlDoc.AppendChild(Source);
            XmlDoc.Save(xmlFilePath);
            XmlDoc.Load(xmlFilePath);
            //--
            XmlNode FirstNode = XmlDoc.DocumentElement;
            XmlElement root = XmlDoc.CreateElement(ConstAttriName.EUPI);
            root.SetAttribute(ConstAttriName.ESID, _Equipment.Equipid);
            root.SetAttribute(ConstAttriName.ENAM, _Equipment.Name);
            root.SetAttribute(ConstAttriName.EDEC, _Equipment.Description);

            XmlElement iniRoot = XmlDoc.CreateElement(ConstAttriName.EINI);
            XmlElement networkInfo = XmlDoc.CreateElement(ConstAttriName.ENWI);
            networkInfo.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.NetworkInfo);
            networkInfo.InnerText = "NetworkInfo";

            XmlElement folderInfo = XmlDoc.CreateElement(ConstAttriName.EFDI);
            folderInfo.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.FolderInfo);
            folderInfo.InnerText = "FolderInfo";

            XmlElement sett = XmlDoc.CreateElement(ConstAttriName.ESET);
            sett.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.Setting);
            sett.InnerText = "Setting";

            XmlElement labelSerialInfo = XmlDoc.CreateElement(ConstAttriName.ELSI);
            labelSerialInfo.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.LabelSerialInfo);
            labelSerialInfo.InnerText = "LabelSerialInfo";

            XmlElement bixolonInfo = XmlDoc.CreateElement(ConstAttriName.EBLI);
            bixolonInfo.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.BixolonInfo);
            bixolonInfo.InnerText = "BixolonInfo";

            XmlElement labelList = XmlDoc.CreateElement(ConstAttriName.ELBL);
            labelList.SetAttribute(ConstAttriName.DATA, _Equipment.IniValue.LabelList);
            labelList.InnerText = "LabelList";
            iniRoot.AppendChild(networkInfo);
            iniRoot.AppendChild(folderInfo);
            iniRoot.AppendChild(sett);
            iniRoot.AppendChild(labelSerialInfo);
            iniRoot.AppendChild(bixolonInfo);
            iniRoot.AppendChild(labelList);

            root.AppendChild(iniRoot);
            int setItemCount = _Equipment.SettingValue.Count;

            foreach (EqSettingValue settingValue in _Equipment.SettingValue)
            {
                Type typeInstance = typeof(EqSettingValue);
                int countTypeInstance = typeInstance.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length;

                Type type = typeof(ConstAttriName);
                int counType = type.GetFields(BindingFlags.Static | BindingFlags.Public).Length;
                string[] listConstAttriName = new string[counType];
                int count_start = 0;

                foreach (var p in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    var v = p.GetValue(null);
                    listConstAttriName[count_start] = v.ToString();
                    count_start++;
                }
                XmlElement setRoot = XmlDoc.CreateElement(ConstAttriName.STGI);
                count_start = 0;

                foreach (var p in typeInstance.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {

                    PropertyInfo prop = typeInstance.GetProperty(p.Name);
                    string aa = prop.GetValue(settingValue).ToString();
                    setRoot.SetAttribute(listConstAttriName[count_start], prop.GetValue(settingValue).ToString());
                    count_start++;
                }
                root.AppendChild(setRoot);
            }

            FirstNode.AppendChild(root);
            XmlDoc.AppendChild(FirstNode);
            XmlDoc.Save(xmlFilePath);

        }


    }
}