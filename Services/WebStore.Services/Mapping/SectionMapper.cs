﻿using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDTO ToDTO(this Section Section) => Section is null ? null : new SectionDTO
        {
            Id = Section.Id,
            Name = Section.Name
        };

        public static Section FromDTO(this SectionDTO Section) => Section is null ? null : new Section
        {
            Id = Section.Id,
            Name = Section.Name
        };
    }
}
