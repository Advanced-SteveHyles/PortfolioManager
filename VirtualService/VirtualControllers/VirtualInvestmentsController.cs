using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualInvestmentsController
    {
        private readonly IInvestmentRepository _repository;

        public VirtualInvestmentsController(string connection)
        {
            _repository = new InvestmentRepository(connection);
        }

        public IVirtualActionResult Get(int page = 1, int pageSize = ApiConstants.MaxPageSize)
        {
            try

            {
                var results = _repository.GetInvestments()
                    .ToList()
                    .Select(p => p.MapToDto());
                return new OkMultipleActionResult<InvestmentDto>(results);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }
    }
}