using System.ComponentModel.DataAnnotations;

namespace WebApiLU2.Models
{
    public class Object2dModel
    {
        public Guid IdObject { get; set; }
        public int PrefabId { get; set; }
        public int SortingLayer {  get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int RotationZ { get; set; }
        public int ScaleX { get; set; }
        public int ScaleY { get; set; }
        public Guid IdEnvironment { get; set; }

   


    }
}
