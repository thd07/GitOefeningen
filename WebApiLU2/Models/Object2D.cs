using System.ComponentModel.DataAnnotations;

namespace WebApiLU2.Models
{
    
        public class Object2D
        {
            public Guid IdObject { get; set; } // Primaire sleutel
            public Guid IdEnvironment { get; set; } // Buitenlandse sleutel naar Environment2D
            public int PrefabId { get; set; } // Object type ID
            public int PosX { get; set; } // X-coördinaat
            public int PosY { get; set; } // Y-coördinaat
            public int ScaleX { get; set; } // Schaal
            public int ScaleY { get; set; } // Schaal   
            public int RotationZ { get; set; } // Rotatie in graden
            public int SortingLayer { get; set; } // Renderlaag
    }




    
}
