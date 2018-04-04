using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptionsTest.ViewModels;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Model
{
   
    [Binding]
    public class ModelData : Attribute
    {
        private ApplicationDbContext context;
  
       
            public static RegisterPersonViewModel RegPersonSuccess()
            {
                UserInfoViewModel userInfo = new UserInfoViewModel
                {
                    FirstName = "Ислам",
                    SecondName = "Байгазиев",
                    MiddleName = "Test",
                    BirthDay = "13.09.1998",
                    Email = "islam13@mail.ru",
                    Inn = "12345678912345",                
                };


                PassportInfoViewModel passportInfo = new PassportInfoViewModel
                {
                    Series = "AN",
                    DateofExtradition = "12.05.2011",
                    Validaty = "12.05.2016",
                    IssuedBy = "Lenin",
                    Number = "6542164",
                };
                ContactInfoViewModel contactInfo = new ContactInfoViewModel
                {
                     MobilePhone = "0555800565"
                };
            
                RegisterPersonViewModel model = new RegisterPersonViewModel
                {
                    UserInfo = userInfo,
                    PassportInfo = passportInfo,
                    ContactInfo = contactInfo,

                };
                return model;
            }
        public static AddCompanyViewModel RegCompanySuccess()
        {
            UserInfoViewModel userInfo = new UserInfoViewModel
            {
                FirstName = "Ислам",
                SecondName = "Байгазиев",
                MiddleName = "Test",
                BirthDay = "13.09.1998",
                Email = "islam13@mail.ru",
                Inn = "12345678912345",
            };

            ContactInfoViewModel contactInfo = new ContactInfoViewModel
            {
                MobilePhone = "0555800565"
            };


            AddCompanyViewModel model = new AddCompanyViewModel
            {
                NameCompany = "Samsung",
                InnCompany = "12345678912345",
                OkpoCompany = "Test",
                DateOfInitialRegistration = "13.09.1998",
                DateOfRegistrationMinistryJustice = "13.09.2017",
                RegistrationNumberSocialFund = "654116513",
                NumberOfEmployees = 1,
                ContactInfo = contactInfo,
                RegistrationAuthority = "CKK",
                IssuedBy = "Stalin"

            };           

            return model;
        }
        public static RegisterEmployeeViewModel RegEmployeeSuccess()
        {

            RegisterEmployeeViewModel model = new RegisterEmployeeViewModel
            {
                FirstName = "Rob",
                SecondName = "Stark",
                MiddleName = "Eddardovich",
                Email = "islam13@mail.ru",
                Position = "Boss",                
            };

            return model;
        }

    }

    public class DeleteData
    {
        public ApplicationDbContext context;
        public DeleteData(ApplicationDbContext context)
        {
            this.context = context;
        }

        

        public  void  DeleteUser(string email)
        {
            var user = context.Users.FirstOrDefault(u=>u.Email ==email);
            DeleteUserInfo(user);
            context.Entry(user).State = EntityState.Deleted;
           
            context.SaveChanges();
        }

        public  void DeleteUserInfo(User user)
        {
          
             UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.Email == user.Email.ToString());
            if (userInfo == null)
            {
              DeleteEmployeeInfo(user);
            }
            PassportInfo passport = userInfo.PassportInfo;
            context.Entry(passport).State = EntityState.Deleted;
            
            ContactInfo contactInfo = context.ContactInfos.FirstOrDefault(a => a.UserInfos == userInfo);
            List<Address> addresses = userInfo.Addresses.ToList();
           
            foreach (var a in addresses)
            {
                context.Entry(a).State = EntityState.Deleted;
              
                context.SaveChanges();
            }
            List<Account> accounts = userInfo.Accounts.ToList();
            DeleteAccounts(accounts);
            context.Entry(contactInfo).State = EntityState.Deleted;
           context.SaveChanges();


        }

        public  void DeleteAccounts(List< Account>accounts)
        {
           
           
            List<Transaction> transactions =new List<Transaction>();
            List<InnerTransfer> innerTransfers = new List<InnerTransfer>();
            List<PaymentSchedule> paymentSchedules = new List<PaymentSchedule>();
            foreach (var account in accounts)
            {
               Transaction transaction= context.Transactions.FirstOrDefault(t => t.AccountId==account.Id);
               InnerTransfer innerTransfer = context.InnerTransfers.FirstOrDefault(t => t.AccountSenderId==account.Id||t.AccountReceiverId== account.Id);
               transactions.Add(transaction);
               innerTransfers.Add(innerTransfer);
            }
            foreach (var innerTransfer in innerTransfers)
            {
                context.Entry(innerTransfer).State = EntityState.Deleted;
             
                context.SaveChanges();
            }
            foreach (var transaction in transactions)
            {
                context.Entry(transaction).State = EntityState.Deleted;
              
                context.SaveChanges();
            }
           
            foreach (var account in accounts)
            {
                context.Entry(account).State = EntityState.Deleted;
             
                context.SaveChanges();
            }
        }


        public void DeleteEmployeeInfo(User user)
        {
            EmployeeInfo employeeInfo = context.EmployeeInfos.FirstOrDefault(u => u.UserId == user.Id.ToString());
        }

        public  void TrancateDatabase()
        {
            string query = "DELETE FROM AspNetUsers;";
            context.Database.ExecuteSqlCommand(query);
            context.SaveChanges();
        }
    }
}
