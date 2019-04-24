﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperationalMachines.CommandHandlers
{
    public class RemoveDailyOperationalMachineCommandHandler : ICommandHandler<RemoveDailyOperationalLoomCommand, DailyOperationalLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationalLoomRepository _dailyOperationalDocumentRepository;

        public RemoveDailyOperationalMachineCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = _storage.GetRepository<IDailyOperationalLoomRepository>();
        }

        public async Task<DailyOperationalLoomDocument> Handle(RemoveDailyOperationalLoomCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationalDocumentRepository.Query.Include(d => d.DailyOperationMachineDetails);
            var existingOperation = _dailyOperationalDocumentRepository.Find(query).Where(entity => entity.Identity.Equals(request.Id)).FirstOrDefault();

            if(existingOperation == null)
            {
                Validator.ErrorValidation(("Daily Production Document", "Unavailable existing Daily Production Document with Id " + request.Id));
            }

            existingOperation.Remove();
            await _dailyOperationalDocumentRepository.Update(existingOperation);
            _storage.Save();

            return existingOperation;
        }
    }
}
