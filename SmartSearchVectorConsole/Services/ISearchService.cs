using SmartSearchVectorConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSearchVectorConsole.Services
{
    internal interface ISearchService
    {
        Task<IEnumerable<(TextParagraph, double?)>> SearchParagrphs(string query, string collectionName, int top = 3, int skip = 0);
    }
}
