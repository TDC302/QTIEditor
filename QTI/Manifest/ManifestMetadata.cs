using Microsoft.VisualBasic;
using QTIEditor.QTI.Manifest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QTIEditor.QTI.Manifest
{
    /// <summary>
    /// A ManifestMetadata object contains descriptive information about its parent Manifest object. The scope of ManifestMetadata is the entire logical package described
    /// by the parent Manifest.
    /// </summary>
    /// <remarks>
    /// The Schema and SchemaVersion children of ManifestMetadata provide information about the specification or profile that governs the meaning of the parent Manifest.
    /// A MetadataModel child of ManifestMetadata serves as a placeholder for general descriptive information about its parent Manifest.MetadataModel is an extension point
    /// that allows meta-data with an information structure that is defined in another namespace.Multiple, differing meta-data models may be declared as extensions contained
    /// within a single ManifestMetadata object. <br/><br/>
    /// If the meta-data is defined in an external Metadata object, then the link to that object is achieved using an IPointer object. Any combination of MetadataModel and
    /// externally referenced meta-data is permitted and their order within the Metadata object is not significant Appropriate targets for IPointer declared as a child of
    /// ManifestMetadata are defined in Section 6.9. <br/><br/>
    /// </remarks>
    public class ManifestMetadata : IXmlSerializable
    {

        [XmlElement]
        public const string schema = "QTIv2.2 Package";

        [XmlElement]
        public const string schemaversion = "1.0.0";

		[XmlIgnore]
		public required string title;

		[XmlIgnore]
		public required string description;

		[XmlIgnore]
		public required string copyright;

		public const string lomStr = @"
		<schema>QTIv2.2 Package</schema>
		<schemaversion>1.0.0</schemaversion>
		<imsmd:lom>
			<imsmd:general>
				<imsmd:title>
					<imsmd:string>{0}</imsmd:string>
				</imsmd:title>
				<imsmd:language>en</imsmd:language>
				<imsmd:description>
					<imsmd:string>{1}</imsmd:string>
				</imsmd:description>
			</imsmd:general>
			<imsmd:lifeCycle>
				<imsmd:version>
					<imsmd:string>2.1</imsmd:string>
				</imsmd:version>
				<imsmd:status>
					<imsmd:source>LOMv1.0</imsmd:source>
					<imsmd:value>Final</imsmd:value>
				</imsmd:status>
			</imsmd:lifeCycle>
			<imsmd:metaMetadata>
				<imsmd:metadataschema>LOMv1.0</imsmd:metadataschema>
				<imsmd:metadataschema>QTIv2.1</imsmd:metadataschema>
				<imsmd:language>en</imsmd:language>
			</imsmd:metaMetadata>
			<imsmd:technical>
				<imsmd:format>text/x-imsqti-item-xml</imsmd:format>
			</imsmd:technical>
			<imsmd:rights>
				<imsmd:description>
					<imsmd:string>{2}</imsmd:string>
				</imsmd:description>
			</imsmd:rights>
		</imsmd:lom>";

		public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteRaw(
				string.Format(lomStr,
						title,
						description,
						copyright));
        }
    }
}
