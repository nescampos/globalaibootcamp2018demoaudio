using DemoAudioSearch.Web.Helpers;
using DemoAudioSearch.Web.Models.SearchModel;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoAudioSearch.Web.Controllers
{
    public class SearchController : Controller
    {
        private FeaturesSearch MotorBusqueda = new FeaturesSearch();

        // GET: Search
        public ActionResult Index(string q)
        {
            IndexBuscadorViewModel model = new IndexBuscadorViewModel();
            if (string.IsNullOrWhiteSpace(q))
            {
                return View(model);
            }

            List<string> orden = null;
            var resultados = MotorBusqueda.Search<AudioDTO>(q, orden).Results;
            model.Resultados = resultados;
            model.Query = q;

            return View(model);
        }

        public ActionResult Detalle(string nombre)
        {
            using (DocumentClient client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["CosmosdbURL"]), ConfigurationManager.AppSettings["CosmosdbKey"]))
            {
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = 1 };
                var collectionLink = UriFactory.CreateDocumentCollectionUri(ConfigurationManager.AppSettings["CosmosdbDatabaseId"], ConfigurationManager.AppSettings["CosmosdbCollectionId"]);

                AudioDTO audio = client.CreateDocumentQuery<AudioDTO>(collectionLink, "SELECT * FROM AudioProcesado WHERE AudioProcesado.Nombre = '" + nombre + "'", queryOptions).ToList().SingleOrDefault();
                //var childrenSqlQuery = client.CreateDocumentQuery<EntidadDTO>(collectionLink,
                //"SELECT c " +
                //"FROM c IN f.Children WHERE c.Nombre = '" + nombre + "'").ToList();
                //audio.Entidades = childrenSqlQuery;
                DetalleViewModel model = new DetalleViewModel();
                model.Audio = audio;
                return View(model);
            }
        }
    }
}