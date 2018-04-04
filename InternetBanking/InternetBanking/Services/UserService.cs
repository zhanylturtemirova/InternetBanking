using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly ISelectListService selectListService;
        private ApplicationContext context;
        private Random rnd;
        private readonly SignInManager<User> signInManager;

        public UserService(UserManager<User> userManager, ISelectListService selectListService, ApplicationContext context,  SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.selectListService = selectListService;
            this.context = context;
           rnd = new Random();
            this.signInManager = signInManager;
        }

        //public UserService(ApplicationContext context)
        //{
        //    this.context = context;
        //}

        public string CreateLogin(UserInfoViewModel model)
        {
            model.SecondName = Transliteration.Front(model.SecondName);
            model.FirstName = Transliteration.Front(model.FirstName);
            bool isUnique = false;
            rnd = new Random();
            string fName="";
            string sName = model.SecondName;
            string login="";
            int i = 0;
            while (!isUnique)
            {
                if (i >= model.FirstName.Length)
                {
                    while (!isUnique)
                    {
                        string rand = rnd.Next(0, 999).ToString();
                        login = fName + sName + rand;
                        isUnique = IsUniqueAsync(login).Result;
                        return login;
                    }                                      
                }
               fName += model.FirstName[i];
               i++;
               fName = fName.ToLower();
               login = fName + sName;
               isUnique = IsUniqueAsync(login).Result;
            }           
            return login ;
        }

        private AddressType GetAddressType(AddressTypesEnum enumItem)
        {
            return context.AddressTypes.FirstOrDefault(a => a.IsEqual(enumItem));
        }
        public string CreateLogin(RegisterEmployeeViewModel model)
        {
            model.SecondName = Transliteration.Front(model.SecondName);
            model.FirstName = Transliteration.Front(model.FirstName);
            bool isUnique = false;
            rnd = new Random();
            string fName = "";
            string sName = model.SecondName;
            string login = "";
            int i = 0;
            while (!isUnique)
            {
                if (i >= model.FirstName.Length)
                {
                    while (!isUnique)
                    {
                        string rand = rnd.Next(0, 999).ToString();
                        login = fName + sName + rand;
                        isUnique = IsUniqueAsync(login).Result;
                        return login;
                    }
                }
                fName += model.FirstName[i];
                i++;
                fName = fName.ToLower();
                login = fName + sName;
                isUnique = IsUniqueAsync(login).Result;
            }
            return login;
        }
        private async Task<bool> IsUniqueAsync(string login)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UserName == login);
            return user == null;
        }

        public async Task<ContactInfo> CreateContactInfo(ContactInfoViewModel model)
        {
            ContactInfo contactInfo = new ContactInfo{MobilePhone = model.MobilePhone, CityPhone = model.CityPhone, Email = model.Email, FullName = model.FullName, Address = model.Address, PhoneNumber = model.PhoneNumber};
            await context.ContactInfos.AddAsync(contactInfo);
            context.SaveChanges();
            return contactInfo;
        }
        public async void CreateAddress(UserInfo userInfo, FactAddressViewModel model)
        {
            Address factAddress = new Address { CountryId = model.CountryId, City = model.City, PostCode = model.PostCode, Street = model.Street, HouseAddress = model.HouseAddress, UserInfoId = userInfo.Id, AddressType = GetAddressType(AddressTypesEnum.FactAddress) };
            await context.Addresses.AddAsync(factAddress);
            context.SaveChanges();
        }

        public async void CreateAddress(UserInfo userInfo, LegalAddressViewModel model)
        {
            Address legalAddress = new Address { CountryId = model.CountryId, City = model.City, PostCode = model.PostCode, Street = model.Street, HouseAddress = model.HouseAddress, UserInfoId = userInfo.Id, AddressType = GetAddressType(AddressTypesEnum.LegalAddress) };
            await context.Addresses.AddAsync(legalAddress);
            context.SaveChanges();

        }

        public async void CreateAddress(UserInfo userInfo, PlaceOfBirthViewModel model)
        {
            Address placeOfBirth = new Address { CountryId = model.CountryId, City = model.City, UserInfoId = userInfo.Id, AddressType = GetAddressType(AddressTypesEnum.BirthAddress)};
            await context.Addresses.AddAsync(placeOfBirth);
            context.SaveChanges();
        }

        public async Task<PassportInfo> CreatePassportInfo(PassportInfoViewModel model)
        {
            DateTime dateOfExtradition = DateTime.Parse(model.DateofExtradition);
            DateTime validaty = DateTime.Parse(model.Validaty);
            PassportInfo passportInfo = new PassportInfo { Series = model.Series, DateofExtradition = dateOfExtradition, Validaty = validaty, Number = model.Number, IssuedBy = model.IssuedBy, TypeOfDocumentId = 1 };
            await context.PassportsInfo.AddAsync(passportInfo);
            context.SaveChanges();
            return passportInfo;
        }

        public async Task<IdentityResult> CreateUser(User user, UserInfoViewModel model)
        {

            return await userManager.CreateAsync(user, model.Password);
            // return await userManager.CreateAsync(user, generatePasswordService.CreatePassword());
        }

        public async Task<UserInfo> CreateUserInfo(User user, PassportInfo passportInfo, UserInfoViewModel model, ContactInfo contact)
        {
            DateTime birthDay = DateTime.Parse(model.BirthDay);
            UserInfo userInfo = new UserInfo { UserId = user.Id, FirstName = model.FirstName, SecondName = model.SecondName, MiddleName = model.MiddleName, PassportInfoId = passportInfo.Id, Email = model.Email, BirthDay = birthDay, Inn = model.Inn, CitezenshipId = model.CountryId, Gender = model.Gender, ContactInfoId = contact.Id};
            await context.AddAsync(userInfo);
            context.SaveChanges();
            return userInfo;
        }

        public UserInfo FindUserByIdInUserInfo(string id, ref string userName, ref int userId)
        {
            UserInfo user = context.UserInfo.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                userName = String.Format("{0} {1} {2}", user.FirstName, user.MiddleName, user.SecondName); ;
                userId = user.Id;
            }
            return user;
        }

        public EmployeeInfo FindUserByIdInCompany(string id, ref string userName, ref int companyId)
        {
            EmployeeInfo user = context.EmployeeInfos.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                userName = String.Format("{0} {1} {2}", user.FirstName, user.MiddleName, user.SecondName);
                companyId = user.CompanyId;
            }
            return user;
        }

        public async Task<UserInfo> FindUserInfoByUserId(string userId)
        {
            UserInfo userInfo = await GetUserList().FirstOrDefaultAsync(u => u.UserId == userId);
            return userInfo;
        }
        public async Task<UserInfo> FindUserInfoById(int userId)
        {
            UserInfo userInfo = await GetUserList().FirstOrDefaultAsync(u => u.Id == userId);
            return userInfo;
        }
        public User FindUserByName(string name)
        {
            User user = context.Users.FirstOrDefault(u => u.UserName == name);
            return user;
        }

        public async Task<User> FindUserById(string id)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public Address FindFactAddressByUserInfoId(int userInfoId)
        {
            Address address = context.Addresses.FirstOrDefault(a => a.UserInfoId == userInfoId && a.AddressType.TypeName == AddressTypesEnum.FactAddress.ToString());
            return address;
        }
        public Address FindLegalAddressByUserInfoId(int userInfoId1)
        {
            Address address = context.Addresses.FirstOrDefault(a => a.UserInfoId == userInfoId1 && a.AddressType.TypeName == AddressTypesEnum.LegalAddress.ToString());
            return address;
        }
        public Address FindBirthAddressByUserInfoId(int userInfoId2)
        {
            Address address = context.Addresses.FirstOrDefault(a => a.UserInfoId == userInfoId2 && a.AddressType.TypeName == AddressTypesEnum.BirthAddress.ToString());
            return address;
        }
        public async Task<IdentityResult> CreateUserTest(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User> IsAccountExistTest(string createdUser)
        {
            User user = await userManager.FindByNameAsync(createdUser);
            return user;
        }

        public IQueryable<UserInfo> GetUserList()
        {
            IQueryable<UserInfo> users = context.UserInfo.Include(u => u.User).Include(u => u.Country).Include(c => c.ContactInfo).Include(a => a.Addresses).Include(a => a.Accounts).Include(p => p.PassportInfo).ThenInclude(t => t.TypeOfDocument);
            return users;
        }

        public ICollection<Address> GetUserAdressList(UserInfo user)
        {
            ICollection<Address> address = context.Addresses.Include(u => u.Country).Where(a => a.UserInfoId == user.Id).ToList();
            return address;
        }

        public PassportInfo PassportEdit(PassportInfo passport, PassportInfoEditViewModel model)
        {
            passport.DateofExtradition = model.DateofExtradition;
            passport.IssuedBy = model.IssuedBy;
            passport.Number = model.Number;
            passport.Series = model.Series;
            passport.Validaty = model.Validaty;
            passport.TypeOfDocumentId = model.TypeOfDocumentId;
            context.PassportsInfo.Update(passport);
            context.SaveChanges();
            return passport;
        }
        public ContactInfo ContactInfoEdit(ContactInfo contactInfo, ContactInfoViewModel model)
        {
            contactInfo.Address = model.Address;
            contactInfo.CityPhone = model.CityPhone;
            contactInfo.FullName = model.FullName;
            contactInfo.Email = model.Email;
            contactInfo.MobilePhone =model.MobilePhone;
            contactInfo.PhoneNumber= model.PhoneNumber;
            context.ContactInfos.Update(contactInfo);
            context.SaveChanges();
            return contactInfo;

        }

            public Address AddressEdit(UserInfo user, FactAddressViewModel model)
        {
            Address address = FindFactAddressByUserInfoId(user.Id);
            address.PostCode = model.PostCode;
            address.Street = model.Street;
            address.HouseAddress = model.HouseAddress;
            address.CountryId = model.CountryId;
            address.City = model.City;
            context.Addresses.Update(address);
            context.SaveChanges();
            return address;
        }
        public Address AddressEdit(UserInfo user, LegalAddressViewModel model)
        {
            Address address = FindLegalAddressByUserInfoId(user.Id);
            address.PostCode = model.PostCode;
            address.Street = model.Street;
            address.HouseAddress = model.HouseAddress;
            address.CountryId = model.CountryId;
            address.City = model.City;
            context.Addresses.Update(address);
            context.SaveChanges();
            return address;
        }
        public Address AddressEdit(UserInfo user, PlaceOfBirthViewModel model)
        {
            Address address = FindBirthAddressByUserInfoId(user.Id);
            address.CountryId = model.CountryId;
            address.City = model.City;
            context.Addresses.Update(address);
            context.SaveChanges();
            return address;
        }

        public UserInfo UserEdit(UserInfo userInfo, UserInfoEditViewModel model)
        {
           
            UserInfo user = GetUserList().FirstOrDefault(u => u.Id == userInfo.Id);
            user.CitezenshipId = model.CountryId;
            user.BirthDay = model.BirthDay;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.MiddleName = model.MiddleName;
            user.Inn = model.Inn;
            user.Gender = model.Gender;
            user.User.IsTwoFactorOn = model.TwoFactorOn;
            user.User.Email = model.Email;

            context.Users.Update(user.User);
            context.UserInfo.Update(user);
            context.SaveChanges();
            return user;
        }

        public UserEditViewModel GetUserInfoEdit(int userId)
        {
            UserInfo user = GetUserList().FirstOrDefault(u => u.Id == userId);
            Address factAddress = FindFactAddressByUserInfoId(user.Id);
            Address legalAddress = FindLegalAddressByUserInfoId(user.Id);
            Address birthAddress = FindBirthAddressByUserInfoId(user.Id);
            UserEditViewModel userInfoEdit = new UserEditViewModel
            {
                User = user,
                FactAddress = new FactAddressViewModel { Countries = selectListService.GetCountries(), CountryId = factAddress.CountryId, City = factAddress.City, HouseAddress = factAddress.HouseAddress, PostCode = factAddress.PostCode, Street = factAddress.Street },
                PlaceOfBirth = new PlaceOfBirthViewModel { Countries = selectListService.GetCountries(), CountryId = birthAddress.CountryId, City = birthAddress.City },
                LegalAddress = new LegalAddressViewModel { Countries = selectListService.GetCountries(), CountryId = legalAddress.CountryId, City = legalAddress.City, HouseAddress = legalAddress.HouseAddress, PostCode = legalAddress.PostCode, Street = legalAddress.Street },
                UserInfo = new UserInfoEditViewModel { Countries = selectListService.GetCountries(), Gender = user.Gender, CountryId = user.CitezenshipId, Email = user.Email, FirstName = user.FirstName, SecondName = user.SecondName, MiddleName = user.MiddleName, Inn = user.Inn, BirthDay = user.BirthDay, TwoFactorOn =user.User.IsTwoFactorOn },
                PassportInfo = new PassportInfoEditViewModel { TypeOfDocuments = selectListService.GetTypeOfDocuments(), DateofExtradition = user.PassportInfo.DateofExtradition, IssuedBy = user.PassportInfo.IssuedBy, Validaty = user.PassportInfo.Validaty, Number = user.PassportInfo.Number, Series = user.PassportInfo.Series, TypeOfDocumentId = user.PassportInfo.TypeOfDocumentId },
                ContactInfo = new ContactInfoViewModel { Address = user.ContactInfo.Address, CityPhone = user.ContactInfo.CityPhone, Email = user.ContactInfo.Email, FullName = user.ContactInfo.FullName, MobilePhone = user.ContactInfo.MobilePhone, PhoneNumber = user.ContactInfo.PhoneNumber }
            };

            userInfoEdit.FactAddress = countryUserEmpty(userInfoEdit.FactAddress, factAddress);
            userInfoEdit.LegalAddress = countryUserEmpty(userInfoEdit.LegalAddress, legalAddress);
            userInfoEdit.PlaceOfBirth = countryUserEmpty(userInfoEdit.PlaceOfBirth, birthAddress);

            return userInfoEdit;
        }

        private FactAddressViewModel countryUserEmpty (FactAddressViewModel factAddress, Address address)
        {
            if (factAddress.CountryId == null)
            {
                factAddress.CountryName = string.Empty;
            }
            else
            {
                factAddress.CountryName = address.Country.CountryName;
            }
            return factAddress;
        }
        private LegalAddressViewModel countryUserEmpty(LegalAddressViewModel legalAddress, Address address)
        {
            if (legalAddress.CountryId == null)
            {
                legalAddress.CountryName = string.Empty;
            }
            else
            {
                legalAddress.CountryName = address.Country.CountryName;
            }
            return legalAddress;
        }
        private PlaceOfBirthViewModel countryUserEmpty(PlaceOfBirthViewModel birthAddress, Address address)
        {
            if (birthAddress.CountryId == null)
            {
                birthAddress.CountryName = string.Empty;
            }
            else
            {
                birthAddress.CountryName = address.Country.CountryName;
            }
            return birthAddress;
        }

        public void UserAccountLimit(Account account, string userId, int? limitId)
        {
            UserInfo user = FindUserInfoByUserId(userId).Result;
            EmployeeAccount userAccountLimit = new EmployeeAccount
            {
                UserId = user.Id,
                Account = account,
                LimitId = limitId
            };
            context.EmployeeAccounts.Add(userAccountLimit);
            context.SaveChanges();
        }

        //public async Task<PassportInfo> CreatePassportInfoTest(PassportInfo model)
        //{
        //    PassportInfo passportInfo = new PassportInfo { Series = model.Series, DateofExtradition = model.DateofExtradition, Validaty = model.Validaty, Number = model.Number, IssuedBy = model.IssuedBy, TypeOfDocumentId = 1 };
        //    await context.PassportsInfo.AddAsync(passportInfo);
        //    await context.SaveChangesAsync();
        //    return passportInfo;
        //}

        //public async Task<UserInfo> CreateUserInfoTest(User user, PassportInfo passportInfo, UserInfoViewModel model)
        //{
        //    UserInfo userInfo = new UserInfo { UserId = user.Id, FirstName = model.FirstName, SecondName = model.SecondName, MiddleName = model.MiddleName, PassportInfoId = passportInfo.Id, PassportInfo = passportInfo, Email = model.Email, BirthDay = model.BirthDay, Inn = model.Inn, Citizenship = model.Citizenship, Gender = model.Gender };
        //    await context.AddAsync(userInfo);
        //    await context.SaveChangesAsync();
        //    return userInfo;
        //}

        public SignInResult CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
           
            if (user.IsBlocked)
            {
                return SignInResult.LockedOut;
                
            }
            SignInResult result = signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure).Result;
            return result;


        }

        public async Task<IdentityResult> BlockUser(User user)
        {
            user.IsBlocked = true;
            var result = await userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> UnblockUser(User user)
        {
            user.IsBlocked = false;
            var result = await userManager.UpdateAsync(user);
            return result;
        }
    }
}
