using Microsoft.AspNetCore.Mvc;
using WebApiLU2.Models;
using WebApiLU2.Repository;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiLU2.Controllers
{
    [ApiController]
    [Route("Object2d")]
    public class Object2dController : ControllerBase
    {

        private readonly IObject2dRepository _repository;

        public Object2dController(IObject2dRepository repository)
        {
            _repository = repository;
        }


        //[HttpGet] ik wil dat deze alle objecten op haalt
        [HttpGet(Name ="GetAllObjects")]

        //public async Task<ActionResult> GetAll(Guid id)
        //{
        //    await _repository.ReadAsync(id);
        //    return Ok(id);
        //}
       

        //[HttpDelete] idk what to do with this one rn
        [HttpDelete(Name ="DeleteObjectOnId")]
        

        //[HttpPost]
        [HttpPost(Name = "AddObject")]
        public async Task<ActionResult> Add(Object2dModel Object2d)
        {

            await _repository.InsertAsync(Object2d);
            return Created();
        }
        //[HttpPut]
        
        

    }
}
