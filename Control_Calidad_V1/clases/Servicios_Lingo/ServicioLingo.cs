using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ServicioLingo
{
    public async Task<ResultadoLingo> EjecutarModeloLingoAsync(DatosLingo datos)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string outputPath = Path.Combine(basePath, "SOL.TXT");
        string rutaModelo = Path.Combine(basePath, "mod_temp.lng");

        string modelo = $@"
MODEL:
SETS:
INSPECTORES /1..2/: sueldo, margen, costo_error, piezas, capacidad;
ENDSETS

DATA:
piezas = {datos.Piezas[0]} {datos.Piezas[1]};
sueldo = {datos.Sueldos[0]} {datos.Sueldos[1]};
margen = {datos.Margenes[0]} {datos.Margenes[1]};
costo_error = {datos.CostosError[0]} {datos.CostosError[1]};
capacidad = {datos.Capacidades[0]} {datos.Capacidades[1]};
horas = {datos.Horas};
piezasmin = {datos.PiezasMin};
ENDDATA

SUBMODEL CALIDAD:
! Calculamos el costo total con sueldos y errores por inspección;
[OBJETIVO] MIN = (sueldo(1) * horas + piezas(1) * horas * (margen(1)/100) * costo_error(1)) * i1 +
                 (sueldo(2) * horas + piezas(2) * horas * (margen(2)/100) * costo_error(2)) * i2;

! Se deben inspeccionar al menos piezasmin;
(piezas(1) * horas * i1 + piezas(2) * horas * i2) >= piezasmin;

! Restricción por disponibilidad de inspectores;
i1 <= capacidad(1);
i2 <= capacidad(2);

@GIN(i1);
@GIN(i2);
ENDSUBMODEL

CALC:
@SOLVE(CALIDAD);
@DIVERT('{outputPath.Replace("\\", "\\\\")}');
@WRITE('Inspectores i1 = ', i1, @NEWLINE(1));
@WRITE('Inspectores i2 = ', i2, @NEWLINE(1));
@WRITE('Costo total minimo = ', OBJETIVO, @NEWLINE(1));
@DIVERT();
ENDCALC

END
";

        File.WriteAllText(rutaModelo, modelo);

        if (!File.Exists(rutaModelo))
            throw new FileNotFoundException("El archivo del modelo LINGO no existe.");

        IntPtr lingoEnv = lingo.LScreateEnvLng();
        if (lingoEnv == IntPtr.Zero)
            throw new Exception("No se pudo crear el entorno de LINGO.");

        try
        {
            string script = $"TAKE {rutaModelo}\nGO\nQUIT\n";
            int resultado = lingo.LSexecuteScriptLng(lingoEnv, script);
            if (resultado != 0)
                throw new Exception($"Error al ejecutar el modelo. Código: {resultado}");

            await Task.Delay(1000);

            if (!File.Exists(outputPath))
                throw new FileNotFoundException("El archivo SOL.TXT no se generó.");

            string[] lineas = File.ReadAllLines(outputPath);

            string i1Texto = lineas.FirstOrDefault(l => l.Contains("Inspectores i1"));
            string i2Texto = lineas.FirstOrDefault(l => l.Contains("Inspectores i2"));
            string costoTexto = lineas.FirstOrDefault(l => l.Contains("Costo total minimo"));

            return new ResultadoLingo
            {
                Inspector1 = i1Texto != null ? Convert.ToDouble(i1Texto.Split('=').Last().Trim()) : 0,
                Inspector2 = i2Texto != null ? Convert.ToDouble(i2Texto.Split('=').Last().Trim()) : 0,
                CostoTotalMinimo = costoTexto != null ? Convert.ToDouble(costoTexto.Split('=').Last().Trim()) : 0
            };
        }
        finally
        {
            lingo.LSdeleteEnvLng(lingoEnv);
        }
    }
}
