using Domain.Exceptions;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class Store_OTP(IConnectionMultiplexer connection)
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public  void StoreOTP(string email, string otp)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                throw new NullReferenceException("Email and OTP cannot be null or empty.");
            }
            if (otp.Length != 6 || !int.TryParse(otp, out _))
            {
                throw new BadRequestException("OTP must be a 6-digit number.");
            }
            // Store OTP in a Redis with expiration time
            _database.StringSet(email, otp, TimeSpan.FromMinutes(5));
        }

        public bool VerifyOTP(string email, string inputOtp)
        {
            // Validate input
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(inputOtp))
            {
                throw new NullReferenceException("Email and OTP cannot be null or empty.");
            }
            if (inputOtp.Length != 6 || !int.TryParse(inputOtp, out _))
            {
                throw new BadRequestException("OTP must be a 6-digit number.");
            }
            // Retrieve the stored OTP from Redis
            var result = _database.StringGet(email);
            if (result != inputOtp)
            {
                // OTP not found or expired
                return false;
            }
            return true;
        }
    }
}
