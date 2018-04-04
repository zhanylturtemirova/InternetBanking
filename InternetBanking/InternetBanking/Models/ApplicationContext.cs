using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetBanking.Models
{
    public class ApplicationContext : IdentityDbContext<User>//, DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PassportInfo> PassportsInfo { get; set; }
        public DbSet<TypeOfDocument> TypeOfDocuments { get; set; }
        public DbSet<Account> Accounts { get; set; }
		public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<RegistrationData> RegistrationDatas { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LegalForm> LegalForms { get; set; }
        public DbSet<Residency> Residencies { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<TaxInspection> TaxInspections { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankInfo> BankInfos { get; set; }
        public DbSet<OurBank> OurBank { get; set; }
        public DbSet<Limit> Limits { get; set; }
        public DbSet<PaymentСode> PaymentСodies { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<InnerTransfer> InnerTransfers { get; set; }
        public DbSet<ExchangeRateType> ExchangeRateTypes { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<BlackList> BlackListedPasswords { get; set; }
        public DbSet<EmployeeAccount> EmployeeAccounts { get; set; }
        public DbSet<TransferState> TransferStates { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<InterBankTransfer> InterBankTransfers { get; set; }
        public DbSet<TypeOfTransfer> TypeOfTransfers { get; set; }
        public DbSet<PaymentSchedule> PaymentSchedules { get; set; }
        public DbSet<IntervalType> IntervalTypes { get; set; }
        public DbSet<DocumentType> Documents { get;set; }
        public DbSet<AddressType> AddressTypes { get; set; }



    }
}
