﻿using ShopGeneral.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopGeneral.Services
{
    public interface IMailService
    {
      
            public Task<bool> SendAsync(MailData mailData, CancellationToken ct);
      
    }
}
