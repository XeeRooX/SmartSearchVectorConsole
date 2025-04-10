using DocumentFormat.OpenXml.Packaging;
using SmartSearchVectorConsole.Models;
using System.Text;
using System.Xml;

namespace SmartSearchVectorConsole.Common
{
    internal class DocumentReader
    {
        public static IEnumerable<TextParagraph> ReadParagraphs(Stream documentContent)
        {
            using var wordDoc = WordprocessingDocument.Open(documentContent, false);
            if (wordDoc.MainDocumentPart == null)
                yield break;

            var xmlDoc = new XmlDocument();
            var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            nsManager.AddNamespace("w14", "http://schemas.microsoft.com/office/word/2010/wordml");

            xmlDoc.Load(wordDoc.MainDocumentPart.GetStream());

            var paragraphs = xmlDoc.SelectNodes("//w:p", nsManager);
            if (paragraphs == null)
                yield break;

            foreach (XmlNode paragraph in paragraphs)
            {
                var texts = paragraph.SelectNodes(".//w:t", nsManager);
                if (texts == null)
                    continue;

                var textBuilder = new StringBuilder();
                foreach (XmlNode text in texts)
                {
                    if (!string.IsNullOrEmpty(text.InnerText))
                        textBuilder.Append(text.InnerText);
                }

                var combinedText = textBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(combinedText))
                {
                    yield return new TextParagraph
                    {
                        Key = Guid.NewGuid(),
                        ParagraphId = paragraph.Attributes?["w14:paraId"]?.Value ?? string.Empty,
                        Text = combinedText
                    };
                }
            }
        }
    }
}
