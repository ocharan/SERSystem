namespace SER.DTO;

public class Lgac
{
    public string lgacNombre { get; set; }
    public int lgacId { get; set; }
    public string cuerpoAcademico { get; set; }

    public Lgac(string lgacNombre, int lgacId, string cuerpoAcademico)
    {
        this.lgacNombre = lgacNombre;
        this.lgacId = lgacId;
        this.cuerpoAcademico = cuerpoAcademico;
    }
}