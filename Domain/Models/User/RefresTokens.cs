using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    [Owned]
    public class RefresTokens
    {
            public string Token { get; set; }

            public DateTime ExpiredOn { get; set; }
        
            public bool IsExpired => DateTime.UtcNow >= ExpiredOn;
            
            public DateTime CreatedOn { get; set; }
            
            public DateTime? RevokedOn { get; set; }
            
            public bool IsActive => RevokedOn == null && !IsExpired;
        
    }
}
