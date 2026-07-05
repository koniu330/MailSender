namespace MailSender.Core.Services
{
    using MailSender.Core.Models;

    public interface IMailFilterService
    {
        bool Register(string AppId, string AppName, string Pass);
        InboundEmails ProcessEmail(InboundEmails email);
    }

    public class MailFilterService : IMailFilterService
    {
        private readonly string _expectedPassword;
    // Lista nazwisk autorów projektu wykorzystywana do oznaczania treści wiadomości
        private readonly List<string> studentSurnames = new()
        {
            "Marczak",
            "Koń",
            "Francuz",
            "Cybak"
        };

        public MailFilterService(string expectedPassword)
        {
            _expectedPassword = expectedPassword;
        }

        public bool Register(string AppId, string AppName, string Pass)
        {
            return Pass == _expectedPassword;
        }

        public InboundEmails ProcessEmail(InboundEmails email)
        {
            if (!string.IsNullOrEmpty(email.Subject) && email.Subject.EndsWith("?"))
            {
                email.Subject = $"[Q] {email.Subject}";
            }

            if (!string.IsNullOrEmpty(email.Body))
            {
                foreach (var surname in studentSurnames)
                {
                    email.Body = email.Body.Replace(
                        surname,
                        $"[student.surname]{surname}[/student.surname]");
                }
            }

            return email;
        }
    }
}