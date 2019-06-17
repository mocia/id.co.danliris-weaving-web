﻿using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; set; }
    }

    public class UpdateStartDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateStartDailyOperationSizingDetailCommand>
    {
        public UpdateStartDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.StartDate).NotEmpty();
            RuleFor(validator => validator.StartTime).NotEmpty();
        }
    }
}
