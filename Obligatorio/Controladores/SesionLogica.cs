namespace Controladores;

public class SesionLogica
{
        public String EmailUsuarioActual { get; set; }

        public SesionLogica(){ }

        public void IniciarSesion(String email)
        {
            EmailUsuarioActual = email;
        }
    
        public void CerrarSesion()
        {
            EmailUsuarioActual = null;
        }
}