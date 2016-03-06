using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualPortfoliosController
        {
            readonly IPortfolioRepository _repository;

            public VirtualPortfoliosController(string connection)
            {
                _repository = new PortfolioRepository(connection);
                Tracer.Trace(this.ToString());
            }

            public IVirtualActionResult Get()
            {
                try
                {                    
                    var portfolios = _repository.GetPortfolios();    
                    var portfolioDtos = portfolios.ToList()
                        .Select(p => p.MapToDto());
                    return new OkMultipleActionResult<PortfolioDto>(portfolioDtos);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
                    return new InternalServerErrorActionResult();
                }
            }
        }
    }