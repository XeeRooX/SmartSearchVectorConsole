using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSearchVectorConsole.Models
{
    internal class TextParagraph
    {
        [VectorStoreRecordKey]
        public required Guid Key { get; init; }
        [VectorStoreRecordData]
        public required string ParagraphId { get; init; }
        [VectorStoreRecordData(IsFullTextSearchable = true)]
        public required string Text { get; init; }
        [VectorStoreRecordVector(768)]
        public ReadOnlyMemory<float> TextEmbedding { get; set; }
    }
}
