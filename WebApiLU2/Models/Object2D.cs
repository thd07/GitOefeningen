using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiLU2.Models
{

    using System.Text.Json.Serialization;

    public class Object2D
    {
        [JsonPropertyName("idEnvironment")]
        public Guid IdEnvironment { get; set; }

        [JsonPropertyName("prefabId")]
        public string PrefabId { get; set; }

        [JsonPropertyName("posX")]
        public float PosX { get; set; }

        [JsonPropertyName("posY")]
        public float PosY { get; set; }

        [JsonPropertyName("scaleX")]
        public float ScaleX { get; set; }

        [JsonPropertyName("scaleY")]
        public float ScaleY { get; set; }

        [JsonPropertyName("rotationZ")]
        public float RotationZ { get; set; }

        [JsonPropertyName("sortingLayer")]
        public int SortingLayer { get; set; }

        public Guid IdObject { get; set; }
    }





}
