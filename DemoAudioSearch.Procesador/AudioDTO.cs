using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    public class AudioDTO
    {
        public string Nombre { get; set; }
        public string Texto { get; set; }
        public string PalabrasClave { get; set; }
        public IEnumerable<Entidad> Entidades { get; internal set; }
    }
}
