using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoAudioSearch.Web.Models.SearchModel
{
    public class EntidadDTO
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public int Coincidencias { get; set; }
        public string WikipediaURL { get; set; }
    }
}