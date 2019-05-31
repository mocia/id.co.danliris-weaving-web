﻿using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateResumeDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryCommand History { get; set; }
    }

    public class UpdateResumeDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateResumeDailyOperationSizingDetailCommand>
    {
        public UpdateResumeDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.ShiftDocumentId.Value).NotEmpty();
            RuleFor(command => command.OperatorDocumentId.Value).NotEmpty();
            RuleFor(command => command.History).SetValidator(new DailyOperationSizingHistoryCommandValidator());
        }
    }
}
