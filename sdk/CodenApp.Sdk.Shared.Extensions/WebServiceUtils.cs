using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CodenApp.Sdk.Shared.Extensions
{
    public static class WebServiceUtils
    {
        public static string XmlSerializer<T>(T objeto)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings()
            {
                DoNotEscapeUriAttributes = true,
                OmitXmlDeclaration = true,
                // Indent = true,
            };
            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(xmlWriter, objeto, namespaces);
                return stringBuilder.ToString();
            }
        }

        public static string SoapRequestBody<T>(T objeto, string? prefix, string localName, string? namespaceURI)
        {
            XmlDocument soapEnvelope = new XmlDocument();
            XmlElement envelopeElement = soapEnvelope.CreateElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            soapEnvelope.AppendChild(envelopeElement);

            XmlElement bodyElement = soapEnvelope.CreateElement("soapenv", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            envelopeElement.AppendChild(bodyElement);

            XmlElement actionElement = soapEnvelope.CreateElement(prefix, localName, namespaceURI);
            bodyElement.AppendChild(actionElement);

            XmlDocumentFragment xmlFragment = soapEnvelope.CreateDocumentFragment();
            xmlFragment.InnerXml = XmlSerializer<T>(objeto);
            actionElement.AppendChild(xmlFragment);

            using (MemoryStream writer = new MemoryStream())
            {
                soapEnvelope.Save(writer);
            }

            return soapEnvelope.OuterXml;

        }

    }
}