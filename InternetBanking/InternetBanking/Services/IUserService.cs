using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.AspNetCore.Identity;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface IUserService
    {
        Task<UserInfo> CreateUserInfo(User user, PassportInfo passportInfo, UserInfoViewModel model, ContactInfo contactInfo);
        Task<IdentityResult> CreateUser(User user, UserInfoViewModel model);
        Task<PassportInfo> CreatePassportInfo(PassportInfoViewModel model);
        Task<ContactInfo> CreateContactInfo(ContactInfoViewModel model);
        void CreateAddress(UserInfo userInfo, FactAddressViewModel model);
        void CreateAddress(UserInfo userInfo, LegalAddressViewModel model);
        void CreateAddress(UserInfo userInfo, PlaceOfBirthViewModel model);
        UserInfo FindUserByIdInUserInfo(string id, ref string userName, ref int userId);
        EmployeeInfo FindUserByIdInCompany(string id, ref string userName, ref int userId);
        User FindUserByName(string name);
        IQueryable<UserInfo> GetUserList();
        PassportInfo PassportEdit(PassportInfo passport, PassportInfoEditViewModel model);
        ContactInfo ContactInfoEdit(ContactInfo contactInfo, ContactInfoViewModel model);
        UserInfo UserEdit(UserInfo user, UserInfoEditViewModel model);
        Address AddressEdit(UserInfo user, FactAddressViewModel model);
        Address AddressEdit(UserInfo user, LegalAddressViewModel model);
        Address AddressEdit(UserInfo user, PlaceOfBirthViewModel model);
        ICollection<Address> GetUserAdressList(UserInfo user);
        UserEditViewModel GetUserInfoEdit(int userId);
        string CreateLogin(UserInfoViewModel model);
        string CreateLogin(RegisterEmployeeViewModel model);
        void UserAccountLimit(Account account, string userId, int? limitId);

        Task<User> IsAccountExistTest(string createdUser);
        Address FindFactAddressByUserInfoId(int userInfoId);
        Address FindLegalAddressByUserInfoId(int userInfoId1);
        Address FindBirthAddressByUserInfoId(int userInfoId2);
        Task<UserInfo> FindUserInfoByUserId(string userId);
        Task<UserInfo> FindUserInfoById(int userId);
        Task<User> FindUserById(string id);
        SignInResult CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure);
        Task<IdentityResult> UnblockUser(User user);
        Task<IdentityResult> BlockUser(User user);
    }
}
