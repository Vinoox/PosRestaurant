using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Features.Categories.Dtos.Commands
{
    public class CreateCategoryDto
    {
        public required string Name { get; set; }
    }
}
