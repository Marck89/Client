using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

#nullable disable

namespace ModelClient
{
    [Serializable]
    public partial class SecondaryTable
    {
        public Guid Id { get; set; }
        public Guid MainTableId { get; set; }
        public string Identifier { get; set; }
        public decimal Value { get; set; }
        public decimal? AlternativeValue { get; set; }

        [XmlIgnore] [JsonIgnore] [field: NonSerialized] public virtual MainTable MainTable { get; set; }
    }
}
