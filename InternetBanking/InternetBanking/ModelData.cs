using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;
using InternetBanking.ViewModels.Enums;


namespace InternetBanking
{
    public class ModelData
    {
        public static void FillData(ApplicationContext context, UserManager<User> _userManager)
        {

            if (!context.TypeOfDocuments.Any())
            {
                TypeOfDocument pasport = new TypeOfDocument{Name = "Паспорт"};
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
                LegalForm legalForm1= new LegalForm { LegalFormName = "ОАО" };
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
                TaxInspection taxInspection2= new TaxInspection { TaxInspectionName = "Октябрьский" };
                context.TaxInspections.AddRange(taxInspection, taxInspection1, taxInspection2);
                context.SaveChanges();
            }
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
                    new Currency {Code = "123", Name = "SOM", IsNativeCurrency = true}
                );
                context.SaveChanges();
            }            
            if (!context.OurBank.Any())
            {
                Currency currency = context.Currencies.FirstOrDefault(n => n.IsNativeCurrency == true);
                Account bankAccount = new Account { Locked = false, CurrencyId = currency.Id , Number = "1234567890123456" };
                context.Accounts.Add(bankAccount);
                context.SaveChanges();
                BankInfo bank = new BankInfo { BankName = "OurBank", Email = "esdp_group1@mail.ru" };
                context.BankInfos.Add(bank);
                OurBank ourBank = new OurBank {BIK = "123" , AccountId =bankAccount.Id, BankInfoId = bank.Id};
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
            if (!context.Roles.Any())
            {

                context.Roles.AddRange
                (
                    new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "user", NormalizedName = "USER" }
                );
                context.SaveChanges();
            }
            if (context.Users.FirstOrDefault(u => u.UserName == "Admin") == null)
            {
                var result = _userManager.CreateAsync(new User
                {
                    UserName = "Admin",
                    Email = "esdp_group1@mail.ru",
                    IsTwoFactorOn = false,
                    IsPasswordChanged = true,
                    
                }, "Admin123@");
                if (result.Result.Succeeded)
                {
                    User user = context.Users.FirstOrDefault(u => u.UserName == "Admin");
                    IdentityRole role = context.Roles.FirstOrDefault(r => r.Name == "Admin");
                    context.UserRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });
                    context.SaveChanges();
                }
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


                //List<Address> adresses = context.Addresses.ToList();
                //AddressType factaddress =
                //    context.AddressTypes.FirstOrDefault(a => a.TypeName == AddressTypesEnum.FactAddress.ToString());
                //AddressType legaladdress =
                //    context.AddressTypes.FirstOrDefault(a => a.TypeName == AddressTypesEnum.LegalAddress.ToString());
                //AddressType birthaddress =
                //    context.AddressTypes.FirstOrDefault(a => a.TypeName == AddressTypesEnum.BirthAddress.ToString());
                //foreach (Address address in adresses)
                //{
                //    switch (address.TypeOfAddress)
                //    {
                //        case "factaddress": address.AddressType = factaddress;
                //            break;
                //        case "legaladdress": address.AddressType = legaladdress;
                //            break;
                //        case "birthaddress": address.AddressType = birthaddress;
                //            break;
                //    }
                //}
                //context.UpdateRange(adresses);
                //context.SaveChanges();

            }

        }
    }
}
