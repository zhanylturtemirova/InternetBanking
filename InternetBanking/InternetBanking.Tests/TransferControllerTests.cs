using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using InternetBanking.Controllers;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InternetBanking.Tests
{
    public class TransferControllerTests
    {
        private readonly ApplicationContext context;
        public TransferControllerTests()
        {
            context = TestServicesProvider.GetContext();
        }

        [Fact]
        public async void GetTransfersModelsOnlyNotConfirm()
        {
            var mockUserService = new Mock<IUserService>();
            var mockTransferService = new Mock<ITransferService>();
            var mockEmployeeService = new Mock<IEmployeeService>();
            var mockValidationService = new Mock<IValidationService>();

            mockTransferService.Setup(t => t.GetNotConfirmedTransferViewModelsByCompanyId(new User())).Returns(GetNotConfirmedTransferViewModels(new User()));
            mockUserService.Setup(t => t.FindUserByName(string.Empty)).Returns(new User());
            mockEmployeeService.Setup(t => t.GetEmployeeInfoByUserId("someId")).Returns(GetNullEmployeeInfo("someId"));
            mockUserService.Setup(t => t.FindUserInfoByUserId("userId")).ReturnsAsync(new UserInfo());
            

            TransferController transferController = new TransferController(TestServicesProvider.GetSelectListService(), 
               mockUserService.Object, mockTransferService.Object, TestServicesProvider.GetAccountService(), null, new PagingService(),
                mockEmployeeService.Object, TestServicesProvider.GetCompanyService(), TestServicesProvider.GetExchangeRateService(), 
                mockValidationService.Object, TestServicesProvider.GetDocumentFormatService(), TestServicesProvider.GetTemplateService(),
                TestServicesProvider.GetPdfCreateAndLoadService(), TestServicesProvider.GetCurrencyService(), new HostingEnvironment()
                );
                //TransferController  transferController = new TransferController(TestServicesProvider.GetSelectListService(), null, mockTransferService.Object, TestServicesProvider.GetAccountService(), null, new PagingService(), null, TestServicesProvider.GetCompanyService(), TestServicesProvider.GetExchangeRateService(), null, TestServicesProvider.GetDocumentFormatService(), TestServicesProvider.GetTemplateService(), TestServicesProvider.GetPdfCreateAndLoadService(), TestServicesProvider.GetCurrencyService(), null);
            var result = await transferController.GetTransfers(section: "not_confirmed");// as ViewResult;
            Assert.NotNull(result);

        }

        public async Task<EmployeeInfo> GetNullEmployeeInfo(string someId)
        {
            return null;
        }


        public virtual IQueryable<ConfirmTransferViewModel> GetNotConfirmedTransferViewModels(User user)
        {
            Currency currency = new Currency
            {
                Code = "111",
                IsNativeCurrency = true,
                Name = "KGS"
            };
            Account myAccount = new Account
            {
                Number = "1234567890123456",
                Currency = currency,
                UserInfo = new UserInfo(),
            };
            List<ConfirmTransferViewModel> allModels = new List<ConfirmTransferViewModel>
            {
                new ConfirmTransferViewModel
                {
                    IsUserHaveRightOfConfirm = true,
                    Transfer = new InnerTransfer
                    {
                        Id = 1,
                        AccountSender = myAccount,
                        AccountReceiver = new Account
                        {
                            Number = "12343454562367678",
                            Currency = currency,
                            UserInfo = new UserInfo()
                        },
                        Amount = 100,
                        Comment = "No commentsыы",
                        TransferState = context.TransferStates.FirstOrDefault(t=>t.IsEqual(TransferStatesEnum.NotConfirmed)),
                        TransferDate = DateTime.Now.AddDays(-29)
                    },
                    TransferId = 1,
                },
                new ConfirmTransferViewModel
                {
                    IsUserHaveRightOfConfirm = true,
                    Transfer = new InnerTransfer
                    {
                        Id = 1,
                        AccountSender = myAccount,
                        AccountReceiver = new Account
                        {
                            Number = "1234345456567678",
                            Currency = currency,
                            UserInfo = new UserInfo()
                        },
                        Amount = 1010,
                        Comment = "No comments",
                        TransferState = context.TransferStates.FirstOrDefault(t=>t.IsEqual(TransferStatesEnum.NotConfirmed)),
                        TransferDate = DateTime.Now.AddDays(-10)
                        
                    },
                    TransferId = 1,
                },

                new ConfirmTransferViewModel
                {
                    IsUserHaveRightOfConfirm = true,
                    Transfer = new InnerTransfer
                    {
                        Id = 1,
                        AccountSender = myAccount,
                        AccountReceiver = new Account
                        {
                            Number = "12343454565127678",
                            Currency = currency,
                            UserInfo = new UserInfo()
                        },
                        Amount = 10,
                        Comment = "No comments",
                        TransferState = context.TransferStates.FirstOrDefault(t=>t.IsEqual(TransferStatesEnum.NotConfirmed)),
                        TransferDate = DateTime.Now.AddDays(-10)

                    },
                    TransferId = 1,
                },

            };
           // IQueryable<ConfirmTransferViewModel> models = (IQueryable<ConfirmTransferViewModel>)allModels.OrderByDescending(t=>t.Transfer.TransferDate);
            return allModels as IQueryable<ConfirmTransferViewModel>;
        }
    }
}
