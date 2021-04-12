using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
/***
"But we are not of those who shrink back and are destroyed, 
but of those who believe and are saved" 
(Hebrews 10:39).
***/
namespace AUA_FCT_EDITOR.XmlControl
{
    class XmlRead
    {
        #region NodePath
        public const string _equipInfoNodePath = ConstAttriName.TITL;
        private const string _settingInfoNodePath = ConstAttriName.TITL
            + "/" + ConstAttriName.EUPI;
        private const string _inilInfoNodePath = ConstAttriName.TITL
           + "/" + ConstAttriName.EUPI
           + "/" + ConstAttriName.EINI;
        private const string _modelInfoNodePath = ConstAttriName.TITL
            + "/" + ConstAttriName.EUPI
            + "/" + ConstAttriName.STGI;
        private const string _labelInfoNodePath = ConstAttriName.TITL
            + "/" + ConstAttriName.EUPI
            + "/" + ConstAttriName.STGI;

        #endregion

        private static XmlNode equipInfoNode = null;
        private static XmlNode iniInfoNode = null;
        private static XmlNode settingInfoNode = null;

        public static Equipment _equipInfomation = null;

        public static void ReadXmlFile(string xmlFilePath, Equipment equipInfomation)
        {
            try
            {
                _equipInfomation = equipInfomation;
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);

                equipInfoNode = doc.SelectSingleNode(_equipInfoNodePath);
                iniInfoNode = doc.SelectSingleNode(_inilInfoNodePath);
                settingInfoNode = doc.SelectSingleNode(_settingInfoNodePath);

                XmlNodeList equipInfomationList = equipInfoNode.SelectNodes(ConstAttriName.EUPI);

                foreach (XmlNode node in equipInfomationList)
                {
                    _equipInfomation.Equipid = node.Attributes.GetNamedItem(ConstAttriName.ESID).Value;
                    _equipInfomation.Name = node.Attributes.GetNamedItem(ConstAttriName.ENAM).Value;
                    _equipInfomation.Description = node.Attributes.GetNamedItem(ConstAttriName.EDEC).Value;
                    _equipInfomation.IniValue = new EqIniSettingValue();
                    XmlNodeList iniInfomationList = settingInfoNode.SelectNodes(ConstAttriName.EINI);

                    foreach (XmlNode ininode in iniInfomationList)
                    {
                        _equipInfomation.IniValue.NetworkInfo = ininode.FirstChild.Attributes[0].Value;
                        _equipInfomation.IniValue.FolderInfo = ininode.FirstChild.NextSibling.Attributes[0].Value;
                        _equipInfomation.IniValue.Setting = ininode.FirstChild.NextSibling.NextSibling.Attributes[0].Value;
                        _equipInfomation.IniValue.LabelSerialInfo = ininode.FirstChild.NextSibling.NextSibling.NextSibling.Attributes[0].Value;
                        _equipInfomation.IniValue.BixolonInfo = ininode.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.Attributes[0].Value;
                        _equipInfomation.IniValue.LabelList = ininode.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.Attributes[0].Value;
                    }
                    XmlNodeList settingInfomationList = settingInfoNode.SelectNodes(ConstAttriName.STGI);

                    if (settingInfomationList.Count != 0)
                    {
                        _equipInfomation.SettingValue = new List<EqSettingValue>();
                        EqSettingValue settingValue = new EqSettingValue();
                        settingValue = GetSettingValue(settingInfomationList);
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
            }

        }
        // xml 한줄 읽기 
        // EqSettingValue 프로퍼티를 모두 읽어서 ConstAttriName이름으로 읽는다. 
        // 순서가 쌍으로 되어야 정상적으로 읽어들인다. 
        private static EqSettingValue GetSettingValue(XmlNodeList nodeList)
        {
            EqSettingValue settingValue = null;
            try
            {
                foreach (XmlNode node in nodeList)
                {
                    settingValue = new EqSettingValue();
                    Type typeSettingValue = settingValue.GetType();
                    Type type = typeof(ConstAttriName); // MyClass is static class with static properties
                    int counType = type.GetFields(BindingFlags.Static | BindingFlags.Public).Length;
                    string[] listEqSettingValueString = new string[counType];
                    int count_start = 0;
                    foreach (var p in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        var v = p.GetValue(null); // static classes cannot be instanced, so use null...
                        listEqSettingValueString[count_start] = (node.Attributes.GetNamedItem((string)v) != null) ? node.Attributes.GetNamedItem((string)v).Value : "";
                        count_start++;
                    }


                    count_start = 0;
                    Type typeInstance = typeof(EqSettingValue);
                    int countTypeInstance = typeInstance.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length;
                    foreach (var p in typeInstance.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (count_start == 0)
                        {
                            PropertyInfo prop = typeSettingValue.GetProperty(p.Name);
                            prop.SetValue(settingValue, Convert.ToInt32(listEqSettingValueString[count_start]), null);
                        }
                        else
                        {
                            PropertyInfo prop = typeSettingValue.GetProperty(p.Name);
                            prop.SetValue(settingValue, listEqSettingValueString[count_start], null);
                        }
                        count_start++;
                    }


                    _equipInfomation.SettingValue.Add(settingValue);
                    settingValue = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {

            }
            return settingValue;
        }
    }//class
}//namespace
