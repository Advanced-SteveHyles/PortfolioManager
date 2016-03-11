using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualInvestmentsController
    {
        private readonly IInvestmentRepository _repository;

        public VirtualInvestmentsController(string connection)
        {
            _repository = new InvestmentRepository(connection);
        }

        public IVirtualActionResult GetAllInvestments()
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

        public IVirtualActionResult InsertInvestment(InvestmentRequest investmentRequest)
        {
            try
            {
                if (investmentRequest == null)
                {
                    return new BadRequestActionResult();
                }

                var entityInvestment = new InvestmentFactory().CreateInvestment(investmentRequest);
                if (entityInvestment == null)
                {
                    return new BadRequestActionResult();
                }
                
                var result = _repository.InsertInvestment(entityInvestment);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoInvestment = result.Entity.MapToDto();
                    return new CreatedActionResult<InvestmentDto>(dtoInvestment);
                }
                else
                {
                    return new BadRequestActionResult();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

    }
}