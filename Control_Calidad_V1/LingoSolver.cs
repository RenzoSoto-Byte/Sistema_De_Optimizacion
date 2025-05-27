using System;
using System.IO;
using System.Runtime.InteropServices;

public static class LingoSolver
{
    [DllImport("Lingd64_21.dll", EntryPoint = "LScreateEnvLng")]
    public static extern IntPtr LScreateEnvLng();

    [DllImport("Lingd64_21.dll", EntryPoint = "LSdeleteEnvLng")]
    public static extern int LSdeleteEnvLng(IntPtr pLingoEnv);

    [DllImport("Lingd64_21.dll", EntryPoint = "LSexecuteScriptLng")]
    public static extern int LSexecuteScriptLng(IntPtr pLingoEnv, string script);

    [DllImport("Lingd64_21.dll", EntryPoint = "LSgetCallbackVarPrimalLng")]
    public static extern int LSgetCallbackVarPrimalLng(IntPtr pLingoEnv,
        string varName, [MarshalAs(UnmanagedType.LPArray)] double[] result);

    public static double[] SolveModel(string path, string[] varNames)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Archivo LINGO no encontrado: " + path);

        IntPtr env = LScreateEnvLng();
        if (env == IntPtr.Zero)
            throw new Exception("No se pudo crear el entorno LINGO");

        // Leer y ejecutar el archivo LINGO
        string script = $"take \"{path}\" \ngo \nquit";
        int err = LSexecuteScriptLng(env, script);
        if (err != 0)
            throw new Exception("Error al ejecutar el modelo en LINGO");

        // Esperar un momento para asegurarse de que el modelo se resuelva
        System.Threading.Thread.Sleep(1000); // Dar tiempo al modelo para ejecutarse

        // Obtener las variables de decisión
        double[] resultados = new double[varNames.Length];
        for (int i = 0; i < varNames.Length; i++)
        {
            double[] val = new double[1];
            int res = LSgetCallbackVarPrimalLng(env, varNames[i], val);
            if (res != 0)
            {
                throw new Exception($"Error al obtener la variable: {varNames[i]}");
            }
            resultados[i] = val[0];

            // Imprimir el valor para depuración
            Console.WriteLine($"Variable {varNames[i]} = {resultados[i]}");
        }

        LSdeleteEnvLng(env);
        return resultados;
    }

}
