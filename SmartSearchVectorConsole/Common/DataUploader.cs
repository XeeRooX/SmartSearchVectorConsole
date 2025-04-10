#pragma warning disable SKEXP0001
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;
using SmartSearchVectorConsole.Models;

namespace SmartSearchVectorConsole.Common
{
    internal class DataUploader(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingGenerationService)
    {
        public async Task GenerateEmbeddingsAndUpload(string collectionName, IEnumerable<TextParagraph> textParagraphs)
        {
            var collection = vectorStore.GetCollection<Guid, TextParagraph>(collectionName);
            await collection.CreateCollectionIfNotExistsAsync();

            foreach (var paragraph in textParagraphs)
            {
                Console.WriteLine($"Генерация эмбеддинга для параграфа: {paragraph.ParagraphId}");
                paragraph.TextEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(paragraph.Text);

                Console.WriteLine($"Добавление параграфа в векторную БД: {paragraph.ParagraphId}");
                await collection.UpsertAsync(paragraph);
                Console.WriteLine();
            }
        }
    }
}
