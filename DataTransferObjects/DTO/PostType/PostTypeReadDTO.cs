using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostTypeReadDTO
    (
        int Id,
        string Description,
        bool IsAvailable,
        int PostCount
    );
}
