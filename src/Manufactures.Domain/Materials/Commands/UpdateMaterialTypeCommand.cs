﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Materials.Commands
{
    public class UpdateMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RingDocumentValueObject> RingDocuments { get; set; }
        public string Description { get; set; }
    }

    public class UpdateMaterialTypeCommadValidator : AbstractValidator<UpdateMaterialTypeCommand>
    {
        public UpdateMaterialTypeCommadValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
        }
    }
}
