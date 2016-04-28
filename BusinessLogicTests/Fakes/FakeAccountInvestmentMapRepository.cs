using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicTests.FakeRepositories.DataFakes;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.Fakes
{
    public class FakeAccountInvestmentMapRepository : IAccountInvestmentMapRepository
    {
        private readonly FakeData _fakeData;
        public FakeAccountInvestmentMapRepository(FakeData fakeData)
        {
            _fakeData = fakeData;
        }

        public AccountInvestmentMap GetAccountInvestmentMap(int accountInvestmentMapId)
        {
            var accountInvestmentMapDto =
           _fakeData.InvestmentMaps().SingleOrDefault(i => i.AccountInvestmentMapId == accountInvestmentMapId);

            if (accountInvestmentMapDto != null)
                return new AccountInvestmentMap()
                {
                    AccountId = accountInvestmentMapDto.AccountId,
                    AccountInvestmentMapId = accountInvestmentMapDto.AccountInvestmentMapId,
                    InvestmentId = accountInvestmentMapDto.InvestmentId,
                    Quantity = accountInvestmentMapDto.Quantity,
                    Valuation = accountInvestmentMapDto.Valuation
                };

            return null;
        }



        public void UpdateAccountInvestmentMap(AccountInvestmentMap investmentMap)
        {
            var map = GetAccountInvestmentMap(investmentMap.AccountInvestmentMapId);
            map.Valuation = investmentMap.Valuation;
            map.Quantity = investmentMap.Quantity;

            _fakeData.InvestmentMaps().RemoveAll(f => f.AccountInvestmentMapId == map.AccountInvestmentMapId);
            _fakeData.InvestmentMaps().Add(map);
        }

        public RepositoryActionResult<AccountInvestmentMap> InsertAccountInvestmentMap(AccountInvestmentMap entityAccountInvestmentMap)
        {
            var map = new AccountInvestmentMap()
            {
                AccountInvestmentMapId = entityAccountInvestmentMap.AccountInvestmentMapId,
                AccountId = entityAccountInvestmentMap.AccountId,
                InvestmentId = entityAccountInvestmentMap.InvestmentId,
                Quantity = entityAccountInvestmentMap.Quantity,
                Valuation = entityAccountInvestmentMap.Valuation
            };

            _fakeData.InvestmentMaps().Add(map);

            return new RepositoryActionResult<AccountInvestmentMap>(map, RepositoryActionStatus.Created);
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByInvestmentId(int investmentId)
        {
            return _fakeData.InvestmentMaps().Where(inv => inv.InvestmentId == investmentId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMapsByAccountId(int accountId)
        {
            return _fakeData.InvestmentMaps().Where(inv => inv.AccountId == accountId).AsQueryable();
        }

        public IQueryable<AccountInvestmentMap> GetAccountInvestmentMaps()
        {
            return _fakeData.InvestmentMaps().Select(map => new AccountInvestmentMap()
            {
                AccountId = map.AccountId,
                Valuation = map.Valuation
            }).AsQueryable();
        }


   
    }
}