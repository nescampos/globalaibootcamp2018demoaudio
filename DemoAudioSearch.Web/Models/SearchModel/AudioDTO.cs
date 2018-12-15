using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoAudioSearch.Web.Models.SearchModel
{
    public class AudioDTO
    {
        public string Nombre { get; set; }
        public string Texto { get; set; }
        public string PalabrasClave { get; set; }
        public List<EntidadDTO> Entidades { get; internal set; }

        public string TextoCorto
        {
            get
            {
                return Texto.Length > 300 ? Texto.Substring(0, 300) : Texto;
            }
        }
        public AudioDTO()
        {
            Entidades = new List<EntidadDTO>();
        }
        
    }
}