using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoAudioSearch.Web.Models.SearchModel
{
    public class IndexBuscadorViewModel
    {
        public IList<Microsoft.Azure.Search.Models.SearchResult<AudioDTO>> Resultados { get; internal set; }
        public string Query { get; internal set; }

        public IndexBuscadorViewModel()
        {
            Resultados = new List<Microsoft.Azure.Search.Models.SearchResult<AudioDTO>>();
        }
    }
}