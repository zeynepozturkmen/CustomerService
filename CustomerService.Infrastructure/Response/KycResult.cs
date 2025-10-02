using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Infrastructure.Response
{
    public class KycResult
    {
        public bool Success { get; set; }
        public string VerificationId { get; set; }
        public string UserId { get; set; }
        public bool Verified { get; set; }
        public string Reason { get; set; }
        public int VerificationScore { get; set; }
        public string Level { get; set; }
        public long Timestamp { get; set; }
        public int Age { get; set; }
        public int ProcessingTimeMs { get; set; }
    }
}

