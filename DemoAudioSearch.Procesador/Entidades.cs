using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    public class Entidad
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public int Coincidencias { get; set; }
        public string WikipediaURL { get; set; }
    }
}
