using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.Models;
using InternetBanking.Services;
using Moq;
using Xunit;

namespace InternetBanking.Tests
{
   public  class GeneratePasswordServiceTests
   {

      private GeneratePasswordService generatePasswordService;
      private string result;

       public GeneratePasswordServiceTests()
       {
           generatePasswordService = TestServicesProvider.GetGeneratePasswordService();
            result =  generatePasswordService.CreatePassword();
            TestServicesProvider.GetModelTestData().FillData();
        }


       [Fact]
        public void CheckPasswordLength()
        {
           Assert.True(result.Length> 7);
        }

       [Fact]
       public void CheckIfContainsUpperCase()
       {
           Assert.True(result.Any(char.IsUpper));
        }

       [Fact]
       public void CheckIfContainsLowerCase()
       {
           Assert.True(result.Any(char.IsLower));
       }

       [Fact]
       public void CheckIfContainsNumbers()
       {
           Assert.True(result.Any(char.IsNumber));
       }

       
   }
}
