using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Categories.Dtos.Commands
{
    public class UpdateCategoryDto
    {
        public required string OldName { get; set; }
        public required string NewName { get; set; }
    }
}
