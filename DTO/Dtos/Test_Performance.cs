using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dtos
{
    public class Test_Performance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool HasChildren { get; set; }
    }
}
