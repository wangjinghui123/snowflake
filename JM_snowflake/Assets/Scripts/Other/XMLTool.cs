using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class XMLTool : MonoBehaviour
{
    //======================================ConfigParamers==================================================
    private string GameConfigpath;
    private Dictionary<string, string> configDict;

    public void InitXML()
    {

        GameConfigpath = System.IO.Path.Combine(Application.streamingAssetsPath, "DeployXML/config.xml");
        configDict = new Dictionary<string, string>();
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(GameConfigpath);
        XmlNode root = xmlDoc.DocumentElement;
        XmlNodeList nodeList = root.ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlNode curNode = nodeList[i];
            string key = curNode.Name;
            string val = curNode.InnerText;
            if (!configDict.ContainsKey(key))
            {
                configDict.Add(key, val);
            }
            else
            {
                Debug.LogError("Duplicated key " + key + " in config file " + GameConfigpath);
            }
            Debug.Log("key" + GameConfigpath);
        }
        Debug.Log(GameConfigpath);

    }
    public string GetStringXML(string key, string defaultVal)
    {
        if (configDict.ContainsKey(key))
        {
            return configDict[key];
        }
        else
        {
            UnityEngine.Debug.Log("配置表没有此配置返回默认值并写入默认值");
            SetString(key, defaultVal);
            return defaultVal;
        }
    }
    public void SetString(string key, string val)
    {
        if (configDict.ContainsKey(key))
        {
            configDict[key] = val;
        }
        else
        {
            configDict.Add(key, val);
        }
        UpdateXml(key, val);
    }
    public void RemoveNodeXML(string nodeName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GameConfigpath);
        XmlNodeList list = xmlDoc.DocumentElement.ChildNodes;
        for (int i = 0; i < list.Count; i++)
        {
            XmlElement ee = (XmlElement)list[i];
            if (ee.Name == nodeName)
            {
                xmlDoc.DocumentElement.RemoveChild(ee);
                Debug.Log("成功移除配置表属性" + nodeName);
                break;
            }
        }

        xmlDoc.Save(GameConfigpath);
    }

    private void UpdateXml(string key, string val)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GameConfigpath);
        XmlNode root = xmlDoc.DocumentElement;
        XmlNodeList nodeList = root.ChildNodes;
        bool found = false;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlNode curNode = nodeList[i];
            if (curNode.Name == key)
            {
                curNode.InnerText = val;
                found = true;
                break;
            }
        }
        if (!found)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, key, null);
            node.InnerText = val;
            root.AppendChild(node);
        }
        xmlDoc.Save(GameConfigpath);
        Debug.Log(xmlDoc.InnerXml);
    }
}

