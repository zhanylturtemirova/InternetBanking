﻿// <auto-generated />
using InternetBanking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace InternetBanking.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20180307095241_AddFieldToInnerTransfer")]
    partial class AddFieldToInnerTransfer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InternetBanking.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompanyId");

                    b.Property<int>("CurrencyId");

                    b.Property<bool>("Locked");

                    b.Property<string>("Number");

                    b.Property<int?>("UserInfoId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("InternetBanking.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<int?>("CompanyId");

                    b.Property<int?>("CountryId");

                    b.Property<string>("HouseAddress");

                    b.Property<string>("PostCode");

                    b.Property<string>("Street");

                    b.Property<string>("TypeOfAddress");

                    b.Property<int?>("UserInfoId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("InternetBanking.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BIK");

                    b.Property<int>("BankInfoId");

                    b.HasKey("Id");

                    b.HasIndex("BankInfoId");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("InternetBanking.Models.BankInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BankName");

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("BankInfos");
                });

            modelBuilder.Entity("InternetBanking.Models.BlackList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlackListedPassword")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BlackListedPasswords");
                });

            modelBuilder.Entity("InternetBanking.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CodeOKPO");

                    b.Property<int?>("ContactInfoId");

                    b.Property<int?>("CountryId");

                    b.Property<string>("INN");

                    b.Property<int?>("LegalFormId");

                    b.Property<string>("Logo");

                    b.Property<string>("NameCompany");

                    b.Property<int>("NumberOfEmployees");

                    b.Property<int?>("PropertyTypeId");

                    b.Property<int>("RegistrationDataId");

                    b.Property<string>("RegistrationNumberSocialFund");

                    b.Property<int?>("ResidencyId");

                    b.HasKey("Id");

                    b.HasIndex("ContactInfoId");

                    b.HasIndex("CountryId");

                    b.HasIndex("LegalFormId");

                    b.HasIndex("PropertyTypeId");

                    b.HasIndex("RegistrationDataId");

                    b.HasIndex("ResidencyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("InternetBanking.Models.ContactInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("CityPhone");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("ContactInfos");
                });

            modelBuilder.Entity("InternetBanking.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<bool>("IsNativeCurrency");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("InternetBanking.Models.EmployeeAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int?>("EmployeeId");

                    b.Property<int?>("LimitId");

                    b.Property<bool>("RightOfConfirmation");

                    b.Property<bool>("RightOfCreate");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LimitId");

                    b.HasIndex("UserId");

                    b.ToTable("EmployeeAccounts");
                });

            modelBuilder.Entity("InternetBanking.Models.EmployeeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Chief");

                    b.Property<int>("CompanyId");

                    b.Property<int?>("ContactInfoId");

                    b.Property<string>("FirstName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Position");

                    b.Property<string>("SecondName");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContactInfoId");

                    b.HasIndex("UserId");

                    b.ToTable("EmployeeInfos");
                });

            modelBuilder.Entity("InternetBanking.Models.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CurrencyId");

                    b.Property<int>("ExchangeRateTypeId");

                    b.Property<DateTime>("RateDate");

                    b.Property<decimal>("RateForPurchaise");

                    b.Property<decimal>("RateForSale");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ExchangeRateTypeId");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("InternetBanking.Models.ExchangeRateType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ExchangeRateTypes");
                });

            modelBuilder.Entity("InternetBanking.Models.InnerTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountReceiverId");

                    b.Property<int?>("AccountSenderId");

                    b.Property<decimal>("Amount");

                    b.Property<decimal?>("AmountReceive");

                    b.Property<string>("Comment");

                    b.Property<int?>("ExchangeRateId");

                    b.Property<int?>("ExchangeRateIdSecond");

                    b.Property<DateTime>("TransferDate");

                    b.Property<int>("TransferStateId");

                    b.HasKey("Id");

                    b.HasIndex("AccountReceiverId");

                    b.HasIndex("AccountSenderId");

                    b.HasIndex("ExchangeRateId");

                    b.HasIndex("ExchangeRateIdSecond");

                    b.HasIndex("TransferStateId");

                    b.ToTable("InnerTransfers");
                });

            modelBuilder.Entity("InternetBanking.Models.InterBankTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountNumber");

                    b.Property<int?>("BankId");

                    b.Property<int>("InnerTransferId");

                    b.Property<int?>("PaymentCodeId");

                    b.Property<int?>("PaymentСodeId");

                    b.Property<string>("ReciverName");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("InnerTransferId");

                    b.HasIndex("PaymentСodeId");

                    b.ToTable("InterBankTransfers");
                });

            modelBuilder.Entity("InternetBanking.Models.OurBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<string>("BIK");

                    b.Property<int>("BankInfoId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("BankInfoId");

                    b.ToTable("OurBank");
                });

            modelBuilder.Entity("InternetBanking.Models.PassportInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateofExtradition");

                    b.Property<string>("IssuedBy");

                    b.Property<string>("Number");

                    b.Property<string>("Series");

                    b.Property<int>("TypeOfDocumentId");

                    b.Property<DateTime>("Validaty");

                    b.HasKey("Id");

                    b.HasIndex("TypeOfDocumentId");

                    b.ToTable("PassportsInfo");
                });

            modelBuilder.Entity("InternetBanking.Models.RegistrationData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfInitialRegistration");

                    b.Property<DateTime>("DateOfRegistrationMinistryJustice");

                    b.Property<string>("IssuedBy");

                    b.Property<string>("RegistrationAuthority");

                    b.Property<int?>("TaxInspectionId");

                    b.HasKey("Id");

                    b.HasIndex("TaxInspectionId");

                    b.ToTable("RegistrationDatas");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryName");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.LegalForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LegalFormName");

                    b.HasKey("Id");

                    b.ToTable("LegalForms");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.Limit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("LimitAmount");

                    b.Property<string>("LimitName");

                    b.HasKey("Id");

                    b.ToTable("Limits");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.PaymentСode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("PaymentCodeName");

                    b.HasKey("Id");

                    b.ToTable("PaymentСodies");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.PropertyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PropertyTypeName");

                    b.HasKey("Id");

                    b.ToTable("PropertyTypes");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.Residency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ResidencyName");

                    b.HasKey("Id");

                    b.ToTable("Residencies");
                });

            modelBuilder.Entity("InternetBanking.Models.SelectTable.TaxInspection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TaxInspectionName");

                    b.HasKey("Id");

                    b.ToTable("TaxInspections");
                });

            modelBuilder.Entity("InternetBanking.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("DateOfTransaction");

                    b.Property<int>("TransactionTypeId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("InternetBanking.Models.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("InternetBanking.Models.TransferState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StateName");

                    b.HasKey("Id");

                    b.ToTable("TransferStates");
                });

            modelBuilder.Entity("InternetBanking.Models.TypeOfDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TypeOfDocuments");
                });

            modelBuilder.Entity("InternetBanking.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsPasswordChanged");

                    b.Property<bool>("IsTwoFactorOn");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int>("LoginAttemptsCount");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("userSendEmailToken");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("InternetBanking.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDay");

                    b.Property<int?>("CitezenshipId");

                    b.Property<int?>("ContactInfoId");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<string>("Inn");

                    b.Property<string>("MiddleName");

                    b.Property<int>("PassportInfoId");

                    b.Property<string>("SecondName");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CitezenshipId");

                    b.HasIndex("ContactInfoId");

                    b.HasIndex("PassportInfoId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("InternetBanking.Models.Account", b =>
                {
                    b.HasOne("InternetBanking.Models.Company", "Company")
                        .WithMany("Accounts")
                        .HasForeignKey("CompanyId");

                    b.HasOne("InternetBanking.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.UserInfo", "UserInfo")
                        .WithMany("Accounts")
                        .HasForeignKey("UserInfoId");
                });

            modelBuilder.Entity("InternetBanking.Models.Address", b =>
                {
                    b.HasOne("InternetBanking.Models.Company", "Company")
                        .WithMany("Addresses")
                        .HasForeignKey("CompanyId");

                    b.HasOne("InternetBanking.Models.SelectTable.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("InternetBanking.Models.UserInfo", "UserInfo")
                        .WithMany("Addresses")
                        .HasForeignKey("UserInfoId");
                });

            modelBuilder.Entity("InternetBanking.Models.Bank", b =>
                {
                    b.HasOne("InternetBanking.Models.BankInfo", "BankInfo")
                        .WithMany()
                        .HasForeignKey("BankInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.Company", b =>
                {
                    b.HasOne("InternetBanking.Models.ContactInfo", "ContactInfo")
                        .WithMany("Companies")
                        .HasForeignKey("ContactInfoId");

                    b.HasOne("InternetBanking.Models.SelectTable.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("InternetBanking.Models.SelectTable.LegalForm", "LegalForm")
                        .WithMany()
                        .HasForeignKey("LegalFormId");

                    b.HasOne("InternetBanking.Models.SelectTable.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId");

                    b.HasOne("InternetBanking.Models.RegistrationData", "RegistrationData")
                        .WithMany()
                        .HasForeignKey("RegistrationDataId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.SelectTable.Residency", "Residency")
                        .WithMany()
                        .HasForeignKey("ResidencyId");
                });

            modelBuilder.Entity("InternetBanking.Models.EmployeeAccount", b =>
                {
                    b.HasOne("InternetBanking.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.EmployeeInfo", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("InternetBanking.Models.SelectTable.Limit", "limit")
                        .WithMany()
                        .HasForeignKey("LimitId");

                    b.HasOne("InternetBanking.Models.UserInfo", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("InternetBanking.Models.EmployeeInfo", b =>
                {
                    b.HasOne("InternetBanking.Models.Company", "Company")
                        .WithMany("EmployeeInfos")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.ContactInfo", "ContactInfo")
                        .WithMany("EmployeeInfos")
                        .HasForeignKey("ContactInfoId");

                    b.HasOne("InternetBanking.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("InternetBanking.Models.ExchangeRate", b =>
                {
                    b.HasOne("InternetBanking.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.HasOne("InternetBanking.Models.ExchangeRateType", "ExchangeRateType")
                        .WithMany()
                        .HasForeignKey("ExchangeRateTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.InnerTransfer", b =>
                {
                    b.HasOne("InternetBanking.Models.Account", "AccountReceiver")
                        .WithMany()
                        .HasForeignKey("AccountReceiverId");

                    b.HasOne("InternetBanking.Models.Account", "AccountSender")
                        .WithMany()
                        .HasForeignKey("AccountSenderId");

                    b.HasOne("InternetBanking.Models.ExchangeRate", "ExchangeRate")
                        .WithMany()
                        .HasForeignKey("ExchangeRateId");

                    b.HasOne("InternetBanking.Models.ExchangeRate", "ExchangeSecond")
                        .WithMany()
                        .HasForeignKey("ExchangeRateIdSecond");

                    b.HasOne("InternetBanking.Models.TransferState", "TransferState")
                        .WithMany()
                        .HasForeignKey("TransferStateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.InterBankTransfer", b =>
                {
                    b.HasOne("InternetBanking.Models.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId");

                    b.HasOne("InternetBanking.Models.InnerTransfer", "Transfer")
                        .WithMany()
                        .HasForeignKey("InnerTransferId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.SelectTable.PaymentСode", "PaymentСode")
                        .WithMany()
                        .HasForeignKey("PaymentСodeId");
                });

            modelBuilder.Entity("InternetBanking.Models.OurBank", b =>
                {
                    b.HasOne("InternetBanking.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.BankInfo", "BankInfo")
                        .WithMany()
                        .HasForeignKey("BankInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.PassportInfo", b =>
                {
                    b.HasOne("InternetBanking.Models.TypeOfDocument", "TypeOfDocument")
                        .WithMany()
                        .HasForeignKey("TypeOfDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.RegistrationData", b =>
                {
                    b.HasOne("InternetBanking.Models.SelectTable.TaxInspection", "TaxInspection")
                        .WithMany()
                        .HasForeignKey("TaxInspectionId");
                });

            modelBuilder.Entity("InternetBanking.Models.Transaction", b =>
                {
                    b.HasOne("InternetBanking.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.TransactionType", "TransactionType")
                        .WithMany()
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetBanking.Models.UserInfo", b =>
                {
                    b.HasOne("InternetBanking.Models.SelectTable.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CitezenshipId");

                    b.HasOne("InternetBanking.Models.ContactInfo", "ContactInfo")
                        .WithMany("UserInfos")
                        .HasForeignKey("ContactInfoId");

                    b.HasOne("InternetBanking.Models.PassportInfo", "PassportInfo")
                        .WithMany()
                        .HasForeignKey("PassportInfoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InternetBanking.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InternetBanking.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetBanking.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InternetBanking.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
