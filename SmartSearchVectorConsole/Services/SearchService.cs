using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;
using SmartSearchVectorConsole.Models;
#pragma warning disable SKEXP0001

namespace SmartSearchVectorConsole.Services
{
    internal class SearchService(IVectorStore vectorStore, ITextEmbeddingGenerationService textEmbeddingGenerationService) : ISearchService
    {
        public async Task<IEnumerable<(TextParagraph, double?)>> SearchParagrphs(string query, string collectionName, int top = 3, int skip = 0)
        {
            var collection = vectorStore.GetCollection<Guid, TextParagraph>(collectionName);

            ReadOnlyMemory<float> searchVector = await textEmbeddingGenerationService.GenerateEmbeddingAsync(query);

            var options = new HybridSearchOptions<TextParagraph>()
            {
                Top = top,
                Skip = skip,
            };

            var searchResult = await collection.VectorizedSearchAsync(searchVector);

            var result = new List<(TextParagraph, double?)>();
            await foreach (var paragraph in searchResult.Results)
            {
                result.Add((paragraph.Record, paragraph.Score));
            }

            return result;
        }
    }
}
