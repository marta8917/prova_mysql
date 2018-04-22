using System;

namespace ImprimisCore
{
    public class User
    {
        public string Email { get; internal set; }
        public string Phone { get; internal set; }
        public string Name { get; internal set; }
        public int Counter { get; internal set; }
        public string Notes { get; internal set; }
        public DateTime TimeStamp { get; internal set; }
        
    }


}
