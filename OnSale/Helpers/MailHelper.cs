using System;
using MailKit.Net.Smtp;
using MimeKit;
using OnSale.Common;

namespace OnSale.Helpers;

public class MailHelper(IConfiguration configuration) : IMailHelper
{
  private readonly IConfiguration _configuration = configuration;

  public Response<bool> SendMail(string toName, string toEmail, string subject, string body)
  {
    try
    {
      string? from = _configuration["Mail:From"];
      string? name = _configuration["Mail:Name"];
      string? smtp = _configuration["Mail:Smtp"];
      string? port = _configuration["Mail:Port"];
      string? password = _configuration["Mail:Password"];

      if (from == null || name == null || smtp == null || port == null || password == null)
      {
        throw new InvalidOperationException("Mail configuration is incomplete.");
      }

      MimeMessage message = new();
      message.From.Add(new MailboxAddress(name, from));
      message.To.Add(new MailboxAddress(toName, toEmail));
      message.Subject = subject;
      BodyBuilder bodyBuilder = new()
      {
        HtmlBody = body
      };
      message.Body = bodyBuilder.ToMessageBody();

      using (SmtpClient client = new())
      {
        client.Connect(smtp, int.Parse(port), false);
        client.Authenticate(from, password);
        client.Send(message);
        client.Disconnect(true);
      }

      return new Response<bool> { IsSuccess = true };

    }
    catch (Exception ex)
    {
      return new Response<bool>
      {
        IsSuccess = false,
        Message = ex.Message,
        Result = ex
      };
    }
  }
}

