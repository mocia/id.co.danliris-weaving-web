﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Machines.Commands
{
    public class AddNewMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "MachineTypeId")]
        public string MachineTypeId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public string WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "Cutmark")]
        public int? Cutmark { get; set; }

        [JsonProperty(PropertyName = "CutmarkUomId")]
        public string CutmarkUomId { get; set; }

        [JsonProperty(PropertyName = "Process")]
        public string Process { get; set; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "Block")]
        public string Block { get; set; }
    }

    public class AddNewMachineCommandValidator : AbstractValidator<AddNewMachineCommand>
    {
        public AddNewMachineCommandValidator()
        {
            RuleFor(r => r.MachineNumber).NotEmpty();
            RuleFor(r => r.Location).NotEmpty();
            RuleFor(r => r.MachineTypeId).NotEmpty();
            RuleFor(r => r.WeavingUnitId).NotEmpty();
            RuleFor(r => r.Process).NotEmpty();
            RuleFor(r => r.Area).NotEmpty();
            RuleFor(r => r.Block).NotEmpty();
        }
    }
}
