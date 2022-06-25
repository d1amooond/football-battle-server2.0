using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Extensions.Language
{
    public static class LanguageExtension
    {
        public static LanguageDTO ToDTO(this Domain.Entities.Language language)
        {
            return new LanguageDTO
            {
                Id = language.Id.AsGuid(),
                DisplayName = language.DisplayName,
                Name = language.Name,
                Status = language.Status
            };
        }
    }
}
