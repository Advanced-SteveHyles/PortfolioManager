using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using PortfolioManager.DTO.DTOs.Transactions;
using PortfolioManager.Repository.Entities;
using Portfolio_API.Helpers;

namespace Portfolio_API.Controllers
{
    internal class ShapedData
    {

        public static object CreateDataShapedObject(Portfolio portfolioEnt, List<string> lstOfFields)
        {
            // work with a new instance, as we'll manipulate this list in this method
            List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);

            if (!lstOfFieldsToWorkWith.Any())
            {
                return portfolioEnt;
            }
            else
            {

                // does it include any expense-related field?
                var lstOfAccountFields = lstOfFieldsToWorkWith.Where(f => f.Contains("accounts")).ToList();

                // if one of those fields is "expenses", we need to ensure the FULL expense is returned.  If
                // it's only subfields, only those subfields have to be returned.

                bool returnPartialAccounts = lstOfAccountFields.Any() && !lstOfAccountFields.Contains("accounts");

                // if we don't want to return the full expense, we need to know which fields
                if (returnPartialAccounts)
                {
                    // remove all expense-related fields from the list of fields,
                    // as we will use the CreateDateShapedObject function in ExpenseFactory
                    // for that.

                    lstOfFieldsToWorkWith.RemoveRange(lstOfAccountFields);
                    lstOfAccountFields = lstOfAccountFields.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();

                }
                else
                {
                    // we shouldn't return a partial expense, but the consumer might still have
                    // asked for a subfield together with the main field, ie: expense,expense.id.  We 
                    // need to remove those subfields in that case.

                    lstOfAccountFields.Remove("accounts");
                    lstOfFieldsToWorkWith.RemoveRange(lstOfAccountFields);
                }

                // create a new ExpandoObject & dynamically create the properties for this object

                // if we have an expense

                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFieldsToWorkWith)
                {
                    // need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.

                    var fieldValue = portfolioEnt.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(portfolioEnt, null);

                    // add the field to the ExpandoObject
                    ((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);
                }

                if (returnPartialAccounts)
                {
                    // add a list of accounts, and in that, add all those accounts
                    List<object> accounts = new List<object>();
                    foreach (var account in portfolioEnt.Accounts)
                    {
                        accounts.Add(ShapedData.CreateDataShapedObject(account, lstOfAccountFields));
                    }

                    ((IDictionary<String, Object>)objectToReturn).Add("accounts", accounts);
                }


                return objectToReturn;
            }
        }

        internal static AccountTransactionSummaryDto CreateDataShapedObject(int accountId, IQueryable<CashTransaction> transactionEnts)
        {
            var x = new AccountTransactionSummaryDto()
            {
                AccountId = accountId,
                Transactions = new List<TransactionDto>()
            };

            foreach (var tx in transactionEnts)
            {
                x.Transactions.Add(CreateDataShapedObject(tx));
            }

            return x;
        }

        private static TransactionDto CreateDataShapedObject(CashTransaction tx)
        {
            return new TransactionDto()
            {
                TransactionId = tx.CashTransactionId,
                AccountId = tx.AccountId,
                TransactionType = tx.TransactionType,
                TransactionDate = tx.TransactionDate,
                Source = tx.Source,
                Value = tx.TransactionValue,
                IsTaxRefund = tx.IsTaxRefund
            };
        }

        public static object CreateDataShapedObject(Account accountEnt, List<string> lstOfFields)
        {
            // work with a new instance, as we'll manipulate this list in this method
            List<string> lstOfFieldsToWorkWith = new List<string>(lstOfFields);

            if (!lstOfFieldsToWorkWith.Any())
            {
                return accountEnt;
            }
            else
            {

                // does it include any expense-related field?
                var lstOfInvestmentColumns = lstOfFieldsToWorkWith.Where(f => f.Contains("investments")).ToList();

                // if one of those fields is "expenses", we need to ensure the FULL expense is returned.  If
                // it's only subfields, only those subfields have to be returned.

                bool returnPartialInvestments = lstOfInvestmentColumns.Any() && !lstOfInvestmentColumns.Contains("investments");

                // if we don't want to return the full expense, we need to know which fields
                if (returnPartialInvestments)
                {
                    // remove all expense-related fields from the list of fields,
                    // as we will use the CreateDateShapedObject function in ExpenseFactory
                    // for that.

                    lstOfFieldsToWorkWith.RemoveRange(lstOfInvestmentColumns);
                    lstOfInvestmentColumns = lstOfInvestmentColumns.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();

                }
                else
                {
                    // we shouldn't return a partial expense, but the consumer might still have
                    // asked for a subfield together with the main field, ie: expense,expense.id.  We 
                    // need to remove those subfields in that case.

                    lstOfInvestmentColumns.Remove("investments");
                    lstOfFieldsToWorkWith.RemoveRange(lstOfInvestmentColumns);
                }

                // create a new ExpandoObject & dynamically create the properties for this object

                // if we have an expense

                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstOfFieldsToWorkWith)
                {
                    // need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.

                    var fieldValue = accountEnt.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(accountEnt, null);

                    // add the field to the ExpandoObject
                    ((IDictionary<String, Object>)objectToReturn).Add(field, fieldValue);
                }

                if (returnPartialInvestments)
                {
                    // add a list of investments, and in that, add all those investments
                    List<object> investments = new List<object>();
                    foreach (var investment in accountEnt.Investments)
                    {
                        investments.Add(ShapedData.CreateDataShapedObject(investment, lstOfInvestmentColumns));
                    }

                    ((IDictionary<String, Object>)objectToReturn).Add("investements", investments);
                }


                return objectToReturn;
            }
        }

        private static object CreateDataShapedObject(AccountInvestmentMap accountInvestmentMap, List<string> lstOfInvestmentColumns)
        {
            return accountInvestmentMap;
        }
    }
}