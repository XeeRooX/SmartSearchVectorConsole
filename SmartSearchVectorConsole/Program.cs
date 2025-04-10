#pragma warning disable SKEXP0070
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;
using SmartSearchVectorConsole.Common;
using SmartSearchVectorConsole.Services;

var vectorStore = new QdrantVectorStore(new QdrantClient("localhost"));

var builder = Kernel.CreateBuilder()
    //.AddOllamaTextEmbeddingGeneration("nomic-embed-text", new Uri("http://localhost:11434"))
    //.AddOllamaTextEmbeddingGeneration("snowflake-arctic-embed:137m", new Uri("http://localhost:11434"))
    //.AddOllamaTextEmbeddingGeneration("jeffh/intfloat-multilingual-e5-large-instruct:f16", new Uri("http://localhost:11434"))
    .AddOllamaTextEmbeddingGeneration("yxchia/multilingual-e5-base", new Uri("http://localhost:11434"))
    .AddQdrantVectorStore("localhost");

builder.Services.AddSingleton<DataUploader>();
builder.Services.AddTransient<ISearchService, SearchService>();

var kernel = builder.Build();

var dataUploader = kernel.Services.GetRequiredService<DataUploader>();
var searchService = kernel.Services.GetRequiredService<ISearchService>();

// Загрузка данных в векторную БД. После загрузки в БД можно закомментить
var filePath = "Files/word-document.docx";
var textParagraphs = DocumentReader.ReadParagraphs(new FileStream(filePath, FileMode.Open));
await dataUploader.GenerateEmbeddingsAndUpload("sk-wikiword", textParagraphs);
//

// Поиск
var query = "Статья о математике";
var result = await searchService.SearchParagrphs(query, "sk-wikiword");

Console.WriteLine($"Результаты поиска ({result.Count()}):");

foreach (var paragraph in result)
{
    Console.WriteLine($"Score: {paragraph.Item2}");
    Console.WriteLine(paragraph.Item1.Text);
    Console.WriteLine(new string('-', 30));
}