using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Manifest
{
    /// <summary>
    /// A Manifest object is a container for the data structures that describe a complete instance of a logical package.
    /// </summary>
    /// <remarks>
    /// A Manifest may contain child objects of the Manifest class (child-manifests). Child-manifests define complete instances of logical packages that are part of
    /// the larger logical package.A Manifest may also contain child objects of the IPointer class that reference child-manifests external to the Manifest. Any combination
    /// of child-manifests and externally referenced child-manifests are permitted and their order within the Manifest is not significant.Appropriate targets for an
    /// IPointer declared as a direct child of a Manifest are defined in Section 6.9. <br/><br/>
    /// A Manifest may contain references to components that are local or remote to its ancestor InterchangePackage object. References are made via Resource, File, and
    /// IPointer child objects. Resource and File are used to reference content files and control files. IPointer is used to identify referenced-manifests. <br/><br/>
    /// A Manifest shall contain File objects that describe all of the control files needed to interpret the Manifest.
    /// </remarks>
    [XmlRoot("manifest", Namespace = "http://www.imsglobal.org/xsd/imscp_v1p1")]
    public class Manifest : IXmlSerializable
    {

        public UniqueIdentifier identifier = new(typeof(Manifest));

        // [, Organizations, Resources, Manifest, IPointer, Extension ], ordered

        public required ManifestMetadata metadata;


        public required List<IManifestLinkable> resources;

        public XmlSchema? GetSchema() => null;
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            
            

            writer.WriteAttributeString("xmlns", "imsmd", null, "http://ltsc.ieee.org/xsd/LOM");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "imsqti", null, "http://www.imsglobal.org/xsd/imsqti_metadata_v2p2");
            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.imsglobal.org/xsd/imscp_v1p1  http://www.imsglobal.org/xsd/qti/qtiv2p2/qtiv2p2_imscpv1p2_v1p0.xsd http://ltsc.ieee.org/xsd/LOM http://www.imsglobal.org/xsd/imsmd_loose_v1p3p2.xsd http://www.imsglobal.org/xsd/imsqti_metadata_v2p2  http://www.imsglobal.org/xsd/qti/qtiv2p2/imsqti_metadata_v2p2.xsd");

            identifier.WriteXmlAttr("identifier", writer);

            writer.WriteComment($"File automatically generated {DateTime.UtcNow} by {Constants.TOOL_NAME} v{Constants.VERSION}");

            writer.WriteStartElement("metadata");
            metadata.WriteXml(writer);
            writer.WriteEndElement();


            writer.WriteElementString("organizations", null);

            writer.WriteStartElement("resources");
            Queue<IManifestLinkable> waitingResources = new(resources);


            while (waitingResources.Count > 0)
            {
                IManifestLinkable resource = waitingResources.Dequeue();
            
                writer.WriteStartElement("resource");
                resource.id.WriteXmlAttr("identifier", writer);
                resource.type.WriteXmlAttr("type", writer);
                resource.href.WriteXmlAttr("href", writer);
                foreach (string file in resource.files)
                {
                    writer.WriteStartElement("file");
                    file.WriteXmlAttr("href", writer);
                    writer.WriteEndElement();
                }

                var depends = resource.dependencies;
                if (depends != null)
                {
                    foreach (IManifestLinkable dependency in depends)
                    {
                        writer.WriteStartElement("dependency");
                        dependency.id.WriteXmlAttr("identifierref", writer);
                        writer.WriteEndElement();
                        waitingResources.Enqueue(dependency);
                    }


                }

                resource.WriteToFile(resource.href);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

        }


        public void WriteToFile()
        {

            XmlSerializer ser = new(typeof(Manifest));

            TextWriter writer = File.CreateText("imsmanifest.xml");

            ser.Serialize(writer, this);

            writer.Close();


        }


    }

}
