public class DatosLingo
{
    public double[] Piezas { get; set; } = new double[2];
    public double[] Sueldos { get; set; } = new double[2];
    public double[] Capacidades { get; set; } = new double[2];
    public double[] Margenes { get; set; } = new double[2];
    public double[] CostosError { get; set; } = new double[2];
    public double Horas { get; set; }
    public double PiezasMin { get; set; }
}

public class ResultadoLingo
{
    public double Inspector1 { get; set; }
    public double Inspector2 { get; set; }
    public double CostoTotalMinimo { get; set; }
}
