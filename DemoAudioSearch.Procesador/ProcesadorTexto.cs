using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    public class ProcesadorTexto
    {
        private IEnumerable<string> _textoProcesar { get; set; }
        private ITextAnalyticsClient client { get; set; }

        public ProcesadorTexto(ITextAnalyticsClient client,IEnumerable<string> textoAProcesar)
        {
            _textoProcesar = textoAProcesar;
            this.client = client;
        }

        public IEnumerable<string> ObtenerFrasesClave()
        {
            List<string> palabrasClaves = new List<string>();

            foreach(var palabrasPorProcesar in _textoProcesar)
            {
                KeyPhraseBatchResult resultado = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("es", "0", palabrasPorProcesar)
                            })).Result;
                foreach (var document in resultado.Documents)
                {
                    palabrasClaves.AddRange(document.KeyPhrases);
                }
            }
            
            return palabrasClaves.Distinct();
        }

        public IEnumerable<Entidad> ObtenerEntidades()
        {
            List<Entidad> entidades = new List<Entidad>();
            foreach (var palabrasPorProcesar in _textoProcesar)
            {
                var procesadorEntidades = TextAnalyticsClientExtensions.EntitiesAsync(client, new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("es",Guid.NewGuid().ToString(),palabrasPorProcesar)
                            }));
                var entities = procesadorEntidades.Result.Documents;
                foreach (var documento in entities)
                {
                    entidades.AddRange(documento.Entities.Select(x => new Entidad { Nombre = x.Name, Tipo = x.Type, Subtipo = x.SubType, Coincidencias = x.Matches.Count(), WikipediaURL = x.WikipediaUrl }));
                }
            }
            return entidades.Distinct();
        }

    }
}
