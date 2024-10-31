using System;
using OnSale.Common;

namespace OnSale.Helpers;

public interface IMailHelper
{
  Response SendMail(string toName, string toEmail, string subject, string body);
}

