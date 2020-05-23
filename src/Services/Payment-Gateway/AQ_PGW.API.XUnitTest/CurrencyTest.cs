using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AQ_PGW.Core.Models.DBTables;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace AQ_PGW.API.XUnitTest
{
    public class CurrencyTest
    {
        [Theory]
        [InlineData(1, "TC_1", "USD", "Test for USD", 535)]
        [InlineData(2, "TC_2", "USD", "Test for USD", 2407.5)]
        [InlineData(3, "TC_3", "USD", "Test for USD", 240.75)]
        [InlineData(4, "TC_4", "USD", "Test for USD", 1200.01)]
        [InlineData(5, "TC_5", "USD", "Test for USD", 786.45)]
        [InlineData(6, "TC_6", "USD", "Test for USD", 719.25)]
        [InlineData(7, "TC_7", "USD", "Test for USD", 85.6)]
        [InlineData(8, "TC_8", "USD", "Test for USD", 0)]
        [InlineData(9, "TC_9", "USD", "Test for USD", -100)]
        //USD

        public void PasstingTestCurrencyUSD(int No, string TestCase, string currencyType, string TestDer, decimal Currency)
        {
            Assert.True(IsTrue(TestCase, currencyType, Currency));
        }

        [Theory]
        [InlineData(10, "TC_10", "SGD", "Test for SGD", 428)]
        [InlineData(11, "TC_11", "SGD", "Test for SGD", 2407.5)]
        [InlineData(12, "TC_12", "SGD", "Test for SGD", 240.75)]
        [InlineData(13, "TC_13", "SGD", "Test for SGD", 1200.01)]
        [InlineData(14, "TC_14", "SGD", "Test for SGD", 786.45)]
        [InlineData(15, "TC_15", "SGD", "Test for SGD", 719.25)]
        [InlineData(16, "TC_16", "SGD", "Test for SGD", 85.6)]
        //SGD
        public void PasstingTestCurrencySGD(int No, string TestCase, string currencyType, string TestDer, decimal Currency)
        {
            Assert.True(IsTrue(TestCase, currencyType, Currency));
        }

        [Theory]
        [InlineData(17, "TC_17", "THB", "Test for THB", 856)]
        [InlineData(18, "TC_18", "THB", "Test for THB", 2407.5)]
        [InlineData(19, "TC_19", "THB", "Test for THB", 240.75)]
        [InlineData(20, "TC_20", "THB", "Test for THB", 1200.01)]
        [InlineData(21, "TC_21", "THB", "Test for THB", 786.45)]
        [InlineData(22, "TC_22", "THB", "Test for THB", 719.25)]
        [InlineData(23, "TC_23", "THB", "Test for THB", 85.6)]
        //THB
        public void PasstingTestCurrencyTHB(int No, string TestCase, string currencyType, string TestDer, decimal Currency)
        {
            Assert.True(IsTrue(TestCase, currencyType, Currency));
        }

        [Theory]
        [InlineData(24, "TC_24", "HKD", "Test for HKD", 1200)]
        [InlineData(25, "TC_25", "HKD", "Test for HKD", 2407.5)]
        [InlineData(26, "TC_26", "HKD", "Test for HKD", 240.75)]
        [InlineData(27, "TC_27", "HKD", "Test for HKD", 1200.01)]
        [InlineData(28, "TC_28", "HKD", "Test for HKD", 786.45)]
        [InlineData(29, "TC_29", "HKD", "Test for HKD", 719.25)]
        [InlineData(30, "TC_30", "HKD", "Test for HKD", 85.6)]
        //HKD
        public void PasstingTestCurrencyHKD(int No, string TestCase, string currencyType, string TestDer, decimal Currency)
        {
            Assert.True(IsTrue(TestCase, currencyType, Currency));
        }

        [Theory]
        [InlineData(31, "TC_31", "CNY", "Test for CNY", 1070)]
        [InlineData(32, "TC_32", "CNY", "Test for CNY", 2407.5)]
        [InlineData(33, "TC_33", "CNY", "Test for CNY", 240.75)]
        [InlineData(34, "TC_34", "CNY", "Test for CNY", 1200.01)]
        [InlineData(35, "TC_35", "CNY", "Test for CNY", 786.45)]
        [InlineData(36, "TC_36", "CNY", "Test for CNY", 719.25)]
        [InlineData(37, "TC_37", "CNY", "Test for CNY", 85.6)]
        //CNY
        public void PasstingTestCurrencyCNY(int No, string TestCase, string currencyType, string TestDer, decimal Currency)
        {
            Assert.True(IsTrue(TestCase, currencyType, Currency));
        }



        bool IsTrue(string description, string currencyType, decimal curren)
        {
            var trans = new Transactions()
            {
                ID = Guid.NewGuid(),
                Currency = currencyType,
                OrderAmount = curren,
                PaymentCardToken = "tok_visa",
                PaymentMethod = "Full",
                OrderId = "orderIDTest000",
                Description = description,
            };
            var charger = StripeHelpers.RequestCharge(trans, curren, description);
            var amount = charger.Amount.ToString();
            var strAmount = amount.Insert(amount.Length - 2, ".");
            var AmountPart = Convert.ToDecimal(strAmount);

            WriteToFile($"### Test Currency  [{currencyType}]  {DateTime.Now.ToString()}  ### \n\n");
            if (curren == AmountPart)
            {
                WriteToFile($" {currencyType}: CurrencyInput {curren} || CurrencyOutput {AmountPart} ==> true \n\n");
                return true;
            }
            WriteToFile($" {currencyType}: CurrencyInput {curren} || CurrencyOutput {AmountPart} ==> false \n\n");
            return false;
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\UnitTest_" + DateTime.Now.ToShortDateString().Replace('/', '_').Replace(":", "_") + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
