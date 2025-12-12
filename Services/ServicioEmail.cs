using System.Net;
using System.Net.Mail;
using System.Text;

namespace DentalClinic.Services
{
    public interface IServicioEmail
    {
        Task EnviarEmail(string emailReceptor, string tema, string cuerpoHtml);
    }

    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration _config;

        public ServicioEmail(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarEmail(string emailReceptor, string tema, string cuerpoHtml)
        {
            var emailEmisor = _config["CONFIGURACIONES_EMAIL:EMAIL"];
            var password = _config["CONFIGURACIONES_EMAIL:PASSWORD"];
            var host = _config["CONFIGURACIONES_EMAIL:HOST"];
            var puertoString = _config["CONFIGURACIONES_EMAIL:PUERTO"];

            Console.WriteLine(">>> [ServicioEmail] EMAIL LEÍDO  → " + emailEmisor);
            Console.WriteLine(">>> [ServicioEmail] PASS LEÍDO   → " + (string.IsNullOrEmpty(password) ? "VACÍO/NULL" : "******"));
            Console.WriteLine(">>> [ServicioEmail] HOST         → " + host);
            Console.WriteLine(">>> [ServicioEmail] PUERTO       → " + puertoString);

            if (string.IsNullOrWhiteSpace(emailEmisor) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine(">>> [ServicioEmail] CONFIG INCOMPLETA, NO SE ENVÍA CORREO.");
                return;
            }

            var puerto = int.Parse(puertoString!);

            using var smtp = new SmtpClient(host, puerto)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailEmisor, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var mensaje = new MailMessage()
            {
                From = new MailAddress(emailEmisor!),
                Subject = tema,
                Body = cuerpoHtml,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            mensaje.To.Add(emailReceptor);

            Console.WriteLine(">>> [ServicioEmail] Enviando correo a: " + emailReceptor);

            await smtp.SendMailAsync(mensaje);

            Console.WriteLine(">>> [ServicioEmail] Correo enviado OK (SMTP no lanzó excepción).");
        }
    }
}
