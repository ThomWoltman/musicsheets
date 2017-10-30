using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public interface IConverter
    {
        void Visit(Note note);

        void Visit(Rest rest);
    }
}
