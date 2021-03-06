﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
{
    public class AccountInvestmentMapProcessor 
    {
        private readonly IAccountInvestmentMapRepository  _accountInvestmentMapRepository;

        public AccountInvestmentMapProcessor(
            IAccountInvestmentMapRepository accountInvestmentMapRepository)
        {
            _accountInvestmentMapRepository = accountInvestmentMapRepository;
        }        

        public void ChangeQuantity(int investmentMapId, decimal quantity)
        {            
            var investmentMap = _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId);
            investmentMap.Quantity += quantity;            
            _accountInvestmentMapRepository.UpdateAccountInvestmentMap(investmentMap);        
        }

        public decimal RevalueMap(int investmentMapId, decimal? currentSellPrice)
        {
            var investmentMap = _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId);

            var valuation = investmentMap.Quantity*currentSellPrice;
            investmentMap.Valuation = valuation??0 ;        
            _accountInvestmentMapRepository.UpdateAccountInvestmentMap(investmentMap);

            return valuation ??0;
        }

        public AccountInvestmentMap GetAccountInvestmentMap(int investmentMapId)
        {
            var accountInvestmentMap = _accountInvestmentMapRepository.GetAccountInvestmentMap(investmentMapId);         
            return accountInvestmentMap;
        }

        public List<AccountInvestmentMap> GetMapsByInvestmentId(int investmentId)
        {
            return _accountInvestmentMapRepository
                    .GetAccountInvestmentMapsByInvestmentId(investmentId)
                    .ToList();
        }

        public List<AccountInvestmentMap> GetMapsByAccountId(int accountId)
        {
            return _accountInvestmentMapRepository.GetAccountInvestmentMaps()
                .Where(map => map.AccountId == accountId)
                .ToList();
        }
    }
}