using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameOfLife.Classes
{
    public class CellDto
    {        
        public int Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; }
        public CellDto(int row, int column, string status)
        {
            Row = row; 
            Column = column;
            Status = status;
        }
    }
}
