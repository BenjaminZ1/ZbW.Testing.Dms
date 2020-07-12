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

        public override bool Equals(object obj)
        {
            if (!(obj is MetadataItem))
                return false;
            MetadataItem other = (MetadataItem) obj;
            if (this.SelectedTypItem == other.SelectedTypItem && this.Benutzer == other.Benutzer &&
                this.Bezeichnung == other.Bezeichnung &&
                this.Erfassungsdatum == other.Erfassungsdatum && this.Stichwoerter == other.Stichwoerter &&
                this.ValutaDatum == other.ValutaDatum)
                return true;
            return false;
        }
    }
}