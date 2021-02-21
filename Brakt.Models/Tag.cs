using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class Tag : Validatable
    {
        public int TagId { get; set; }
        public string TagValue { get; set; }

        public override void Validate()
        {
            
        }
    }
}
