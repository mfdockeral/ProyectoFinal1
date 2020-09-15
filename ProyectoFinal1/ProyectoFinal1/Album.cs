using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProyectoFinal1
{
	public class Album
	{
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("id")]
        public int AlbumId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
