using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        { 

            new VillaDto{Id=1, Nombre="Vista a la Piscina",Ocupantes = 3, MetrosCuadrados = 50 },
            new VillaDto{Id=2, Nombre="Vista a la Playa", Ocupantes = 5, MetrosCuadrados = 80},
            new VillaDto{Id=3, Nombre="ARRIBA EL TOLUCA", Ocupantes = 7, MetrosCuadrados = 100}

        };
       
    }
}
