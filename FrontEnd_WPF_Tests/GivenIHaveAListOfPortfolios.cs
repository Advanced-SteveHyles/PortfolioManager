using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.UIBuilders;
using Xunit;

namespace FrontEnd_WPF_Tests
{
    public class GivenIHaveAListOfPortfolios
    {
        private List<PortfolioDto> _portfolioDtos;

        public GivenIHaveAListOfPortfolios()
        {
            _portfolioDtos = new List<PortfolioDto>()
            {
                {new PortfolioDto() {Name = "One"} },
                {new PortfolioDto()},
                {new PortfolioDto()},
            };
        }

        [Fact]
        public async void WhenICallTheBuilderForOnePortfolioIGetATabWithTheCorrectName()
        {
            await StartSTATask(() =>
            {
                var tabItem = BuildPortfolioTabContent.CreateAccountListTab(_portfolioDtos[0]);

                Assert.Equal(_portfolioDtos[0].Name, tabItem.Header);
            }
            );            
        }

        //[Fact]
        //public void WhenICallTheBuilderIGetATabForEachPortfolio()
        //{
        //    foreach (var portfolio in _portfolioDtos)
        //    { 
        //    var tabItem = BuildPortfolioTabContent.CreateAccountListTab();
        //    }

        //}


        public static Task StartSTATask(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(new object());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }

}