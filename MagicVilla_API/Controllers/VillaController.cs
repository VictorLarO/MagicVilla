using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MagicVilla_API.Datos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly AplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, AplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        //public ActionResult <IEnumerable<VillaDto>> GetVillas()
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            /* return new List<VillaDto>
             {
                 new VillaDto{id=1, Nombre="Vista a la Piscina"},
                 new VillaDto{id=2, Nombre="Vista a la Playa"},
                 new VillaDto{id=3, Nombre="ARRIBA EL TOLUCA"}
             };
            */
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            //return Ok(VillaStore.villaList);
            //return Ok(_db.Villas.ToList());
            // return Ok(await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult  <VillaDto> GetVilla(int id)
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa con id " + id);
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            //return Ok(villa);
            return Ok(_mapper.Map<VillaDto>(villa)); 
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult<VillaCreateDto> CrearVilla([FromBody] VillaCreateDto villaDto)
        public async Task<ActionResult<VillaCreateDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /* if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
             {
                 ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                 return BadRequest(ModelState);
             } */
            //if (_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            /*
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            */
            
            /*Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            }; */

            Villa modelo = _mapper.Map<Villa>(createDto);


            /* 
              _db.Villas.Add(modelo);
            _db.SaveChanges();
             */
            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();
            /*
            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);
            //return Ok(villaDto);}  */
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo );
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult DeleteVilla(int id)
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            // var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //VillaStore.villaList.Remove(villa);
            _db.Villas.Remove(villa);
            //_db.SaveChanges();
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }
            /*
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            villa.Nombre = villaDto.Nombre;
            villa.Ocupantes = villaDto.Ocupantes;
            villa.MetrosCuadrados = villaDto.MetrosCuadrados;
            */
            /*Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            }; */
            Villa modelo = _mapper.Map<Villa>(updateDto);

            _db.Villas.Update(modelo);
            // _db.SaveChanges();
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            /*VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };*/
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            if (villa == null) return BadRequest();
            
            patchDTO.ApplyTo(villaDto, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /* Villa modelo = new()
             {
                 Id = villaDto.Id,
                 Nombre = villaDto.Nombre,
                 Detalle = villaDto.Detalle,
                 ImagenUrl = villaDto.ImagenUrl,
                 Ocupantes = villaDto.Ocupantes,
                 Tarifa = villaDto.Tarifa,
                 MetrosCuadrados = villaDto.MetrosCuadrados,
                 Amenidad = villaDto.Amenidad
             }; */
            Villa modelo = _mapper.Map<Villa>(villaDto);
            _db.Villas.Update(modelo);
            // _db.SaveChanges();
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}