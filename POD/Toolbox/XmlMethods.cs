using POD.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace POD.Toolbox
{
    public static class XmlMethods
    {
        // Public Methods - Generic
        public static XElement ListToXml(IEnumerable itemList, string enumName = "")
        {
            string elementName = "";
            List<XElement> items = new List<XElement>();
            foreach (object item in itemList)
            {
                elementName = item.GetType().ToString().Split('.').Last();
                items.Add(new XElement(elementName));
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    foreach (CustomAttributeData attr in propertyInfo.CustomAttributes)
                    {
                        if (attr.AttributeType.Name == "XmlSaveMode")
                        {
                            if (attr.ConstructorArguments[0].Value.ToString() == "0")
                            {
                                if (propertyInfo.GetValue(item, null) == null || string.IsNullOrEmpty(propertyInfo.GetValue(item, null).ToString())) { continue; } // don't write blanks to data
                                if (propertyInfo.GetType() == typeof(bool)) { if (propertyInfo.GetValue(item, null).ToString() == "false") { continue; } } // don't write falses (bool default)
                                items.Last().Add(new XAttribute(propertyInfo.Name, (propertyInfo.GetValue(item, null) != null) ? propertyInfo.GetValue(item, null).ToString() : ""));
                            }
                            if (attr.ConstructorArguments[0].Value.ToString() == "1")
                            {
                                items.Last().Add(ListToXml(propertyInfo.GetValue(item, null) as IEnumerable, propertyInfo.Name));
                            }
                        }
                    }
                }
            }

            if (items.Count() == 0) { return null; }
            return new XElement(elementName + "Set", items, new XAttribute("Name", enumName));

        }


        // Public Methods - POD Specific - Node to Object
        public static void NodeToObject(XmlNode itemNode, out ItemCard item)
        {
            ItemCard newItem = SetObjectPropertiesFromNode(itemNode, new ItemCard()) as ItemCard;

            foreach (XmlNode childNode in itemNode.ChildNodes)
            {
                if (childNode.Attributes.GetNamedItem("Name").InnerText == "ItemImages")
                {
                    foreach (XmlNode subNode in childNode.ChildNodes)
                    {
                        NodeToObject(subNode, out ItemImage obj);
                        newItem.ItemImages.Add(obj);
                    }
                }
                if (childNode.Attributes.GetNamedItem("Name").InnerText == "Links")
                {
                    foreach (XmlNode subNode in childNode.ChildNodes)
                    {
                        NodeToObject(subNode, out WebLink obj);
                        newItem.Links.Add(obj);
                    }
                }
            }

            item = newItem;

        }
        public static void NodeToObject(XmlNode itemNode, out ItemImage item)
        {
            ItemImage newItem = SetObjectPropertiesFromNode(itemNode, new ItemImage()) as ItemImage;

            item = newItem;

        }
        public static void NodeToObject(XmlNode linkNode, out WebLink link)
        {
            link = SetObjectPropertiesFromNode(linkNode, new WebLink()) as WebLink;
        }

        // Public Methods - POD Specific - XML to List
        public static void XmlToList(string filePath, out List<ItemCard> items, XmlDocument xmlDoc = null)
        {
            List<ItemCard> newItems = new List<ItemCard>();

            foreach (XmlNode itemNode in GetXmlNodeListFromXmlDoc(filePath, "ItemCardSet", xmlDoc))
            {
                NodeToObject(itemNode, out ItemCard item);
                newItems.Add(item);
            }

            items = newItems;

        }

        // Private Methods - Generic
        private static object SetObjectPropertiesFromNode(XmlNode node, object newObject)
        {
            foreach (PropertyInfo propertyInfo in newObject.GetType().GetProperties())
            {
                if (node.Attributes[propertyInfo.Name] != null)
                {
                    string value = node.Attributes[propertyInfo.Name].InnerText;
                    Type propType = propertyInfo.PropertyType;
                    if (propType == typeof(int)) { propertyInfo.SetValue(newObject, Convert.ToInt32(value)); }
                    if (propType == typeof(decimal)) { propertyInfo.SetValue(newObject, Convert.ToDecimal(value)); }
                    if (propType == typeof(bool)) { propertyInfo.SetValue(newObject, Convert.ToBoolean(value)); }
                    if (propType == typeof(long)) { propertyInfo.SetValue(newObject, Convert.ToInt64(value)); }
                    if (propType == typeof(string)) { propertyInfo.SetValue(newObject, value); }
                }
            }
            return newObject;
        }
        private static XmlNodeList GetXmlNodeListFromXmlDoc(string filePath, string setName, XmlDocument xmlDoc)
        {
            XmlNodeList xmlNodes;
            if (xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                if (xmlDoc.ChildNodes[1].Name != setName)
                {
                    HelperMethods.WriteToLogFile("Invalid XML for Import: " + filePath, true);
                    return null;
                }
                xmlNodes = xmlDoc.ChildNodes[1].ChildNodes;
            }
            else
            {
                xmlNodes = xmlDoc.ChildNodes[0].ChildNodes;
            }
            return xmlNodes;
        }

    }
}
