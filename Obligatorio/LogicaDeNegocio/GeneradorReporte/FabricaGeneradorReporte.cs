namespace LogicaDeNegocio;

public class FabricaGeneradorReporte
{
    private readonly String _path;
    public FabricaGeneradorReporte(String path)
    {
        _path = path;
    }


    public IGeneradorReporte CrearGeneradorReporte(String tipo)
    {
        switch (tipo)
        {
            case "TXT":
                return new GeneradorReporteTxt(_path);
            case "CSV":
                return new GeneradorReporteCsv(_path);
            default:
                throw new Exception("La opción no es valida");
        }
    }
}