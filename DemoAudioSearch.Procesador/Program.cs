using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var directorioAudios = ConfigurationManager.AppSettings["DirectorioAudios"];

            string[] archivosAudio = Directory.GetFiles(directorioAudios, "*.wav", SearchOption.TopDirectoryOnly);

            var config = SpeechConfig.FromSubscription(ConfigurationManager.AppSettings["ApiKeySpeechService"], ConfigurationManager.AppSettings["RegionSpeechService"]);
            config.SpeechRecognitionLanguage = ConfigurationManager.AppSettings["LanguageSpeechService"];

            ITextAnalyticsClient textClient = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = ConfigurationManager.AppSettings["URITextService"]
            };
            
            using (DocumentClient client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["CosmosdbURL"]), ConfigurationManager.AppSettings["CosmosdbKey"]))
            {

                var collectionLink = UriFactory.CreateDocumentCollectionUri(ConfigurationManager.AppSettings["CosmosdbDatabaseId"], ConfigurationManager.AppSettings["CosmosdbCollectionId"]);


                foreach (var archivo in archivosAudio)
                {
                    ProcesadorAudio procesador = new ProcesadorAudio(config);
                    var textoAudio = procesador.GetText(archivo);
                    ProcesadorTexto procesadorTexto = new ProcesadorTexto(textClient, textoAudio);
                    var palabrasClave = procesadorTexto.ObtenerFrasesClave();
                    string palabrasClaveConcatenadas = string.Join(",", palabrasClave);
                    var entidades = procesadorTexto.ObtenerEntidades();

                    string nombreAudio = Path.GetFileNameWithoutExtension(archivo);

                    AudioDTO audioDTO = new AudioDTO
                    {
                        PalabrasClave = palabrasClaveConcatenadas, Texto = string.Join(" ",textoAudio), Nombre = nombreAudio, 
                        Entidades = entidades
                    };

                    Document created = client.CreateDocumentAsync(collectionLink, audioDTO).Result;

                    File.Move(archivo, ConfigurationManager.AppSettings["DirectorioAudiosProcesados"] + Path.GetFileName(archivo));

                }
            }

            
        }

            
    }
}
