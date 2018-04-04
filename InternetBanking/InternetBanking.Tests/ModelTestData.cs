using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Tests
{
    public class ModelTestData
    {
        private readonly ApplicationContext context;

        public ModelTestData(ApplicationContext context)
        {
            this.context = context;
        }

        public  void FillData()
        {

            /*if (!context.TypeOfDocuments.Any())
            {
                TypeOfDocument pasport = new TypeOfDocument { Name = "Паспорт" };
                TypeOfDocument pasport1 = new TypeOfDocument { Name = "ВоенныйБилет" };
                TypeOfDocument pasport2 = new TypeOfDocument { Name = "СвидетельствоОрождении" };
                context.TypeOfDocuments.AddRange(pasport, pasport1, pasport2);
                context.SaveChanges();
            }

            if (!context.Countries.Any())
            {
                Country country = new Country { CountryName = "Кыргызстан" };
                Country country1 = new Country { CountryName = "Таджикистан" };
                Country country2 = new Country { CountryName = "Казахстан" };
                context.Countries.AddRange(country, country1, country2);
                context.SaveChanges();
            }
            if (!context.LegalForms.Any())
            {
                LegalForm legalForm = new LegalForm { LegalFormName = "ОсОО" };
                LegalForm legalForm1 = new LegalForm { LegalFormName = "ОАО" };
                LegalForm legalForm2 = new LegalForm { LegalFormName = "ЗАО" };
                context.LegalForms.AddRange(legalForm, legalForm1, legalForm2);
                context.SaveChanges();
            }
            if (!context.PropertyTypes.Any())
            {
                PropertyType propertyType = new PropertyType { PropertyTypeName = "Государственная" };
                PropertyType propertyType1 = new PropertyType { PropertyTypeName = "Частная" };
                PropertyType propertyType2 = new PropertyType { PropertyTypeName = "Смешанная" };
                context.PropertyTypes.AddRange(propertyType, propertyType1, propertyType2);
                context.SaveChanges();
            }
            if (!context.Residencies.Any())
            {
                Residency residency = new Residency { ResidencyName = "Резидент" };
                Residency residency1 = new Residency { ResidencyName = "НеРезидент" };

                context.Residencies.AddRange(residency, residency1);
                context.SaveChanges();
            }
            if (!context.TaxInspections.Any())
            {
                TaxInspection taxInspection = new TaxInspection { TaxInspectionName = "Ленинский" };
                TaxInspection taxInspection1 = new TaxInspection { TaxInspectionName = "Свердловский" };
                TaxInspection taxInspection2 = new TaxInspection { TaxInspectionName = "Октябрьский" };
                context.TaxInspections.AddRange(taxInspection, taxInspection1, taxInspection2);
                context.SaveChanges();
            }*/
            if (!context.TransactionTypes.Any())
            {
                List<TransactionType> transactions = new List<TransactionType>();
                transactions.Add(TransactionType.Create(TransactionTypesEnum.Debit));
                transactions.Add(TransactionType.Create(TransactionTypesEnum.Credit));


                context.TransactionTypes.AddRange(transactions);
                context.SaveChanges();
            }
            if (!context.ExchangeRateTypes.Any())
            {
                ExchangeRateType exchangeRateType = ExchangeRateType.Create(ExchangeRateTypesEnum.NBKR);
                ExchangeRateType exchangeRateType1 = ExchangeRateType.Create(ExchangeRateTypesEnum.Market);

                context.ExchangeRateTypes.AddRange(exchangeRateType, exchangeRateType1);
                context.SaveChanges();
            }
            if (!context.TransferStates.Any())
            {
                context.AddRange(
                    TransferState.Create(TransferStatesEnum.Confirmed),
                    TransferState.Create(TransferStatesEnum.NotConfirmed),
                    TransferState.Create(TransferStatesEnum.Canceled),
                    TransferState.Create(TransferStatesEnum.BalanceNotEnough),
                    TransferState.Create(TransferStatesEnum.AccountIsLocked));

                context.SaveChanges();
            }
            if (!context.TypeOfTransfers.Any())
            {
                context.AddRange(
                    TypeOfTransfer.Create(TypeOfTransfersEnum.InnerTransfer),
                    TypeOfTransfer.Create(TypeOfTransfersEnum.InterBankTransfer),
                    TypeOfTransfer.Create(TypeOfTransfersEnum.Conversion));
                context.SaveChanges();
            }
            if (!context.Currencies.Any())
            {
                context.Currencies.Add
                (
                    new Currency { Code = "123", Name = "SOM", IsNativeCurrency = true }
                );
                context.SaveChanges();
            }
            if (!context.OurBank.Any())
            {
                Currency currency = context.Currencies.FirstOrDefault(n => n.IsNativeCurrency == true);
                Account bankAccount = new Account { Locked = false, CurrencyId = currency.Id, Number = "1234567890123456" };
                context.Accounts.Add(bankAccount);
                context.SaveChanges();
                BankInfo bank = new BankInfo { BankName = "OurBank", Email = "esdp_group1@mail.ru" };
                context.BankInfos.Add(bank);
                OurBank ourBank = new OurBank { BIK = "123", AccountId = bankAccount.Id, BankInfoId = bank.Id };
                context.OurBank.Add(ourBank);
                context.SaveChanges();
            }
            if (!context.IntervalTypes.Any())
            {
                List<IntervalType> intervalTypes = new List<IntervalType>();

                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceADay));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceAWeek));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceInTwoWeeks));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceAMonth));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceAQuarter));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceAHalfYear));
                intervalTypes.Add(IntervalType.Create(IntervalTypesEnum.OnceAYear));

                context.IntervalTypes.AddRange(intervalTypes);
                context.SaveChanges();
            }
            
           
            if (!context.AddressTypes.Any())
            {
                List<AddressType> addressTypes = new List<AddressType>
                {
                    AddressType.Create(AddressTypesEnum.FactAddress),
                    AddressType.Create(AddressTypesEnum.LegalAddress),
                    AddressType.Create(AddressTypesEnum.BirthAddress)
                };

                context.AddRange(addressTypes);
                context.SaveChanges();
            }

        }
    }
}
