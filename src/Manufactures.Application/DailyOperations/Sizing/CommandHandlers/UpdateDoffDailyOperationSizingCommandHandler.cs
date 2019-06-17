﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateDoffDailyOperationSizingCommandHandler : ICommandHandler<UpdateDoffFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateDoffFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query.Include(d => d.Details).Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var histories = existingDailyOperation.Details.OrderByDescending(e => e.DateTimeOperation);
            var lastHistory = histories.FirstOrDefault();
            
            //Validation for Start Status
            //var countStartStatus =
            //    existingDailyOperation
            //        .Details
            //        .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONSTART)
            //        .Count();

            //if (countStartStatus != 1)
            //{
            //    throw Validator.ErrorValidation(("StartStatus", "This operation has not started yet"));
            //}

            //Validation for Finish Status
            var countFinishStatus =
                existingDailyOperation
                    .Details
                    .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONCOMPLETE)
                    .Count();

            if (countFinishStatus == 1)
            {
                throw Validator.ErrorValidation(("FinishStatus", "This operation's status already COMPLETED"));
            }

            //Reformat DateTime
            var year = request.Details.FinishDate.Year;
            var month = request.Details.FinishDate.Month;
            var day = request.Details.FinishDate.Day;
            var hour = request.Details.FinishTime.Hours;
            var minutes = request.Details.FinishTime.Minutes;
            var seconds = request.Details.FinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for DoffFinish Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeOperation.Date, new TimeSpan(+7, 0, 0));
            var doffFinishDateMachineLogUtc = new DateTimeOffset(request.Details.FinishDate.Date, new TimeSpan(+7, 0, 0));

            if (doffFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("DoffDate", "Finish date cannot less than latest date log"));
            } else
            {
                if (dateTimeOperation < lastHistory.DateTimeOperation)
                {
                    throw Validator.ErrorValidation(("DoffTime", "Finish time cannot less than latest time log"));
                }
                else
                {
                    var Counter = existingDailyOperation.Counter;
                    var Weight = existingDailyOperation.Weight;

                    existingDailyOperation.SetCounter(new DailyOperationSizingCounterValueObject(Counter.Start, request.Counter.Finish));
                    existingDailyOperation.SetWeight(new DailyOperationSizingWeightValueObject(Weight.Netto, request.Weight.Bruto));
                    existingDailyOperation.SetMachineSpeed(request.MachineSpeed);
                    existingDailyOperation.SetTexSQ(request.TexSQ);
                    existingDailyOperation.SetVisco(request.Visco);
                    existingDailyOperation.SetPIS(request.PIS);
                    existingDailyOperation.SetSPU(request.SPU);
                    existingDailyOperation.SetSizingBeamDocumentId(request.SizingBeamDocumentId);
                    existingDailyOperation.SetOperationStatus(DailyOperationMachineStatus.ONFINISH);

                    var Causes = JsonConvert.DeserializeObject<DailyOperationSizingCausesValueObject>(lastHistory.Causes);

                    var newOperation =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.Details.ShiftId.Value),
                                                               new OperatorId(lastHistory.OperatorDocumentId),
                                                               dateTimeOperation,
                                                               DailyOperationMachineStatus.ONCOMPLETE,
                                                               "-",
                                                               new DailyOperationSizingCausesValueObject(Causes.BrokenBeam, Causes.MachineTroubled));

                    existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                    await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                    _storage.Save();

                    return existingDailyOperation;
                }

            }

            //Validation for DoffFinish Time
            //var lastTimeMachineLog = lastHistory.DateTimeOperation.TimeOfDay;
            //var doffFinishTimeMachineLog = request.Details.FinishTime;

            //if (doffFinishTimeMachineLog < lastTimeMachineLog)
            //{
            //    throw Validator.ErrorValidation(("DoffFinishTime", "Finish time cannot less than latest time log"));
            //}

            //var countFinishStatus =
            //    existingDailyOperation
            //        .Details
            //        .Where(e => e.MachineStatus == DailyOperationMachineStatus.ONCOMPLETE)
            //        .Count();

            //if (countFinishStatus > 0)
            //{
            //    throw Validator.ErrorValidation(("Status", "Finish status has available"));
            //}
        }
    }
}
