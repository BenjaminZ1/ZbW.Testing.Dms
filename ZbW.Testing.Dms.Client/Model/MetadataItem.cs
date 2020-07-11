using System;
using System.Collections.Generic;

namespace ZbW.Testing.Dms.Client.Model
{
    [Serializable]
    public class MetadataItem
    {
        // TODO: Write your Metadata properties here
        public string Benutzer { get; set; }

        public string Bezeichnung { get; set; }

        public DateTime Erfassungsdatum { get; set; }

        public string FilePath { get; set; }

        public string SelectedTypItem { get; set; }

        public string Stichwoerter { get; set; }

        public DateTime? ValutaDatum { get; set; }


        public MetadataItem(string benutzer, string bezeichnung, DateTime erfassungsdatum, string selectedTypItem, string stichwoerter,
             DateTime? valutaDatum)
        {
            Benutzer = benutzer;
            Bezeichnung = bezeichnung;
            Erfassungsdatum = erfassungsdatum;
            SelectedTypItem = selectedTypItem;
            Stichwoerter = stichwoerter;
            ValutaDatum = valutaDatum;
        }

        public MetadataItem() { }
    }
}