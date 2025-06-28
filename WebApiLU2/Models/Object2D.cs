using System.ComponentModel.DataAnnotations;

namespace WebApiLU2.Models
{
    
        public class Object2D
        {
            
            public Guid IdEnvironment { get; set; } // Buitenlandse sleutel naar Environment2D
            public string PrefabId { get; set; } // Object type ID
            public float PosX { get; set; } // X-coördinaat
            public float PosY { get; set; } // Y-coördinaat
            public float ScaleX { get; set; } // Schaal
            public float ScaleY { get; set; } // Schaal   
            public float RotationZ { get; set; } // Rotatie in graden
            public int SortingLayer { get; set; } // Renderlaag
            public Guid IdObject { get; set; } // Primaire sleutel
    }




    
}
