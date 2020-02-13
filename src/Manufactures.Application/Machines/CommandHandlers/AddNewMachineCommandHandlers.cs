﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.Commands;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Machines.CommandHandlers
{
    public class AddNewMachineCommandHandlers : ICommandHandler<AddNewMachineCommand, 
                                                                MachineDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachineRepository _machineRepository;

        public AddNewMachineCommandHandlers(IStorage storage)
        {
            _storage = storage;
            _machineRepository = _storage.GetRepository<IMachineRepository>();
        }
        public async Task<MachineDocument> Handle(AddNewMachineCommand request, 
                                                                   CancellationToken cancellationToken)
        {
            var exsistingMachine = _machineRepository.Find(o => o.MachineNumber.Equals(request.MachineNumber) && 
                                                                o.Deleted.Value.Equals(false))
                                                     .FirstOrDefault();


            if(exsistingMachine != null)
            {
                throw Validator.ErrorValidation(("MachineNumber", "Has available machine number"));
            }


            var machineDocument = new MachineDocument(Guid.NewGuid(),
                                                      request.MachineNumber,
                                                      request.Location,
                                                      new MachineTypeId(Guid.Parse(request.MachineTypeId)),
                                                      new UnitId(int.Parse(request.WeavingUnitId)),
                                                      request.Cutmark ?? 0,
                                                      new UomId(int.Parse(request.CutmarkUomId)) ?? null,
                                                      request.Process,
                                                      request.Area,
                                                      request.Block);

            await _machineRepository.Update(machineDocument);

            _storage.Save();

            return machineDocument;
        }
    }
}
