using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    public class ProcesadorAudio
    {
        private SpeechConfig config;

        public ProcesadorAudio(SpeechConfig config)
        {
            this.config = config;
        }

        public IEnumerable<string> GetText(string audioURI)
        {
            List<string> listaFrases = new List<string>();
            var stopRecognition = new TaskCompletionSource<int>();
            

            // Creates a speech recognizer using file as audio input.
            // Replace with your own audio file name.
            using (var audioInput = AudioConfig.FromWavFileInput(audioURI))
            {
                using (var recognizer = new SpeechRecognizer(config, audioInput))
                {
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        //Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            //Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}, Ticks={e.Result.OffsetInTicks}");
                            listaFrases.Add(e.Result.Text);
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            //Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        //Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            //Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        //Console.WriteLine("\n    Session started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        //Console.WriteLine("\n    Session stopped event.");
                        //Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    recognizer.StartContinuousRecognitionAsync().Wait();

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    recognizer.StopContinuousRecognitionAsync().Wait();
                }
            }

            return listaFrases;
        }

    }
}
