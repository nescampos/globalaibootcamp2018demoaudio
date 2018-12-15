using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace DemoAudioSearch.Web.Helpers
{
    public class FeaturesSearch
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;

        public static string errorMessage;

        static FeaturesSearch()
        {
            try
            {
                string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
                string apiKey = ConfigurationManager.AppSettings["SearchServiceAdminApiKey"];

                // Create an HTTP reference to the catalog index
                _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                _indexClient = _searchClient.Indexes.GetClient(ConfigurationManager.AppSettings["SearchServiceIndex"]);
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        public DocumentSearchResult Search(string searchText)
        {
            // Execute search based on query string
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                return _indexClient.Documents.Search(searchText, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error consultando índice  {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public DocumentSearchResult<T> Search<T>(string searchText, List<string> sorts = null) where T : class
        {
            // Execute search based on query string
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.Any, OrderBy = sorts, Top = 300 };
                return _indexClient.Documents.Search<T>(searchText, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error consultando índice  {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }
}