﻿using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories;

namespace Manufactures.Application.DailyOperations.Spu.QueryHandlers
{
    public class WeavingDailyOperationSpuMachineQueryHandler : IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;


        private readonly IWeavingDailyOperationMachineSizingRepository _repository;

        public WeavingDailyOperationSpuMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingDailyOperationMachineSizingRepository>();
        }





        public List<WeavingDailyOperationSpuMachineDto> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code)
        {
            var allData = from a in _repository.Query
                              where (shift == null || (shift != null && shift != "" && a.Shift.Contains(shift))) &&
                              (groupui == null || (groupui != null && groupui != "" && a.Group.Contains(groupui))) &&
                              (machineSizing == null || (machineSizing != null && machineSizing != "" && a.MachineSizing.Contains(machineSizing)))

                         
                          select new
                          {

                              machineSizing = a.MachineSizing,
                            
                              shift = a.Shift,
                              spu = a.SPU,
                              Group=a.Group,
                              year=a.Year,
                              periodeId=a.PeriodeId,
                            

                               Periode = new DateTime(Convert.ToInt32(a.Year), a.PeriodeId, a.Date),
                            

                          };
            var query = (from a in allData
                         where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)

                         select new WeavingDailyOperationSpuMachineDto
                         {
                    
                             MachineSizing = a.machineSizing,

                             Shift = a.shift,
                             SPU = a.spu,
                             Group = a.Group
                         
                         });

            // return query.OrderByDescending(a => a.Date).ToList();
            return query.OrderBy(a => a.Shift)
                .OrderBy(a => a.MachineSizing)
                .ToList();
        }

        public Task<IEnumerable<WeavingDailyOperationSpuMachineDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<WeavingDailyOperationSpuMachineDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
