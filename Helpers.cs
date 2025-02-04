using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor
{
    public static class Helpers
    {
        public static bool suppressUpdate = false;

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            XmlSerializer serializer = new(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader) ?? throw new NullReferenceException($"Could not deserialize {type}");
            }



            return result;
        }



        public static string ToAttrString(this List<UniqueIdentifier> uniqueIdentifiers)
        {
            StringBuilder stringBuilder = new();
            foreach (UniqueIdentifier item in uniqueIdentifiers)
            {
                stringBuilder.Append(item.ToString() + ",");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, stringBuilder.Length);
            string.Join(",", uniqueIdentifiers);

            return stringBuilder.ToString();
        }

        public static string ToAttrString(this List<string> strings)
        {
            StringBuilder stringBuilder = new();
            foreach (string item in strings)
            {
                stringBuilder.Append(item.ToString() + ",");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, stringBuilder.Length);

            return stringBuilder.ToString();
        }

        public static void WriteXmlAttr(this List<UniqueIdentifier> item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, string.Join(',', item));
        }

        public static void WriteXmlAttr(this string item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, item);
        }

        public static void WriteXmlAttr(this bool item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, XmlConvert.ToString(item));
        }

        public static void WriteXmlAttr(this Enum item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, item.ToString());
        }

        public static void WriteXmlAttr(this UniqueIdentifier item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, item.ToString());
        }

        public static void WriteXmlAttr(this List<string> items,  string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, items.ToAttrString());
        }

        public static void WriteXmlAttr(this uint item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, XmlConvert.ToString(item));
        }

        public static void WriteXmlAttr(this Uri item, string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, item.ToString());
        }



        public static void WriteFileHeaders(XmlWriter writer)
        {
            
            writer.WriteAttributeString("xmlns", "xsi", null, Constants.SCHEMA_INSTANCE);
            writer.WriteAttributeString("xsi", "schemaLocation", null, Constants.SCHEMA_LOCATION);
        }



        public static void SerializeFields(object self, XmlWriter writer)
        {
            var fields = self.GetType().GetFields();

            foreach (var field in fields)
            {

                var value = field.GetValue(self);

                if (value == null)
                    continue;


                foreach (var attr in field.GetCustomAttributes(false))
                {
                    if (attr is XmlAttributeAttribute xmlAttributeAttribute)
                    {
                        string attributeName = xmlAttributeAttribute.AttributeName == "" ? field.Name : xmlAttributeAttribute.AttributeName;
                        writer.WriteStartAttribute(attributeName);
                        if (value is IEnumerable<object> enumerable)
                        {
                            bool first = true;
                            foreach (var obj in enumerable)
                            {
                                if (!first)
                                    writer.WriteString(",");
                                WriteType(obj, writer);
                            }

                        } 
                        else
                        {
                            WriteType(value, writer);
                        }
                        
                        writer.WriteEndAttribute();
                        break;

                    }
                    else if (attr is XmlElementAttribute xmlElementAttribute)
                    {
                        string elementName = xmlElementAttribute.ElementName == "" ? field.Name : xmlElementAttribute.ElementName;
                        writer.WriteStartElement(elementName);
                        WriteType(value, writer);
                        writer.WriteEndElement();
                        break;

                    }
                    else if (attr is XmlArrayAttribute xmlArrayAttribute)
                    {
                        string? elementName = xmlArrayAttribute.ElementName == "" ? null : xmlArrayAttribute.ElementName;
                        if (value is IEnumerable<object> enumerable)
                        {
                            foreach (var obj in enumerable)
                            {
                                string newName = elementName ?? char.ToLower(obj.GetType().Name[0]) + obj.GetType().Name[1..];
                                writer.WriteStartElement(newName);
                                WriteType(obj, writer);
                                writer.WriteEndElement();

                            }


                        } else
                        {
                            throw new InvalidOperationException("That's not an array silly!");
                        }

                        break;
                    }

                }





               
            }
        }

        private static void WriteType(object value, XmlWriter writer)
        {
            if (value is IXmlSerializable ser)
            {
                ser.WriteXml(writer);

            } else if (value is string str)
            {
                writer.WriteString(str);

            } else if (value is uint num)
            {
                writer.WriteString(XmlConvert.ToString(num));

            } else if (value is bool boolVal)
            {
                writer.WriteString(XmlConvert.ToString(boolVal));

            } else if (value is double doubleVal)
            {
                writer.WriteString(XmlConvert.ToString(doubleVal));
            } else if (value is Enum enumVal)
            {
                writer.WriteString(enumVal.ToString());
            } 
            else
            {
                throw new ArgumentException($"Unexpected type: {value.GetType()}");
            }
        }

    }
}
