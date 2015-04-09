using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.CheckBook;
using System.Linq;
using System.Collections.ObjectModel;

namespace CalculatorTests
{
    [TestClass]
    public class CheckBookTest
    {
        [TestMethod]
        public void FillsUpProperly()
        {
            var ob = new CheckBookVM();

            Assert.IsNull(ob.Transactions);

            ob.Fill();

            Assert.AreEqual(12, ob.Transactions.Count);
        }

        [TestMethod]
        public void CountofEqualsMoshe()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var count = ob.Transactions.Where( t => t.Payee == "Moshe" ).Count();

            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void SumOfMoneySpentOnFood()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var category = "Food";

            var food = ob.Transactions.Where(t=> t.Tag == category );

            var total = food.Sum(t => t.Amount);

            Assert.AreEqual(261, total);

        }

        [TestMethod]
        public void Group()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Sum=g.Sum( t=> t.Amount ) });

            Assert.AreEqual(261, total.First().Sum);
            Assert.AreEqual(300, total.Last().Sum);
        }
        [TestMethod]
        public void AverageTransaction()
        {
            var ob = new CheckBookVM();
            ob.Fill();
            //var category = "Food";
            //var food = ob.Transactions.Where(t => t.Tag == "Food");
            var totalFood = ob.Transactions.Where(t => t.Tag == "Food").Sum(g => g.Amount);
             var countFood = ob.Transactions.Where( t => t.Tag == "Food" ).Count();
             var averageSpentOnFood = totalFood / countFood;
             var totalAuto = ob.Transactions.Where(t => t.Tag == "Auto").Sum(g => g.Amount);
             var countAuto = ob.Transactions.Where(t => t.Tag == "Auto").Count();
             var averageSpentOnAuto = totalAuto / countAuto;
             Assert.AreEqual(32.625, averageSpentOnFood);
             Assert.AreEqual(75, averageSpentOnAuto);
             
    }
        //How much did you pay each payee?
        [TestMethod]
        public void AmountPaidPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();
           
            var amountPaidTim = ob.Transactions.Where(t => t.Payee == "Tim").Sum(g => g.Amount);
            var amountPaidMoshe = ob.Transactions.Where(t => t.Payee == "Moshe").Sum(g => g.Amount);
            var amountPaidBracha = ob.Transactions.Where(t => t.Payee == "Bracha").Sum(g => g.Amount);
            Assert.AreEqual(300, amountPaidTim);
            Assert.AreEqual(130, amountPaidMoshe);
            Assert.AreEqual(131, amountPaidBracha);
        }

        //How much did you pay each payee for food?
    [TestMethod]
        public void AmountPaidEachPayee4food()
        {
            var ob = new CheckBookVM();
            ob.Fill();
            var amountPaidMoshe4food = ob.Transactions.Where(t => t.Payee == "Moshe").Where(g => g.Tag == "Food").Sum(h => h.Amount);
            var amountPaidBracha4food = ob.Transactions.Where(t => t.Payee == "Bracha").Where(g => g.Tag == "Food").Sum(h => h.Amount);
            var amountPaidTim4food = ob.Transactions.Where(t => t.Payee == "Tim").Where(g => g.Tag == "Food").Sum(h => h.Amount);
            Assert.AreEqual(130, amountPaidMoshe4food);
            Assert.AreEqual(131, amountPaidBracha4food);
            Assert.AreEqual(0, amountPaidTim4food);
        }
    //List the transaction between April 5th and 7th
        [TestMethod]
     public void ListTransactionBtwApril5and7 ()
    {
        var ob = new CheckBookVM();
        var April5 = new DateTime(2015, 04, 05);
        var April7 = new DateTime(2015, 04, 07);
        ob.Fill();
        var query = ob.Transactions.Where(t => t.Date >= April5).Where(t => t.Date <= April7).Select(t => new { t.Date, t.Account, t.Payee, t.Amount, t.Tag });
        List<Transaction> myList = new List<Transaction>();
        myList.Add(new Transaction { Date = new DateTime(2015, 04, 07), Account = "Checking", Payee = "Moshe", Amount = 30, Tag = "Food" });
           myList.Add(new Transaction {Date= new DateTime(2015, 04, 05), Account="Checking", Payee="Tim", Amount=130, Tag="Auto" });
           myList.Add(new Transaction { Date= new DateTime(2015, 04, 07), Account="Credit", Payee="Moshe", Amount=30, Tag="Food" });
           myList.Add(new Transaction { Date= new DateTime(2015, 04, 06), Account="Credit", Payee="Bracha", Amount=30.5, Tag="Food" });
           myList.Add(new Transaction {Date= new DateTime(2015, 04, 05), Account="Credit", Payee="Tim", Amount=130, Tag="Auto" });
           myList.Add( new Transaction {Date= new DateTime(2015, 04, 06), Account="Checking", Payee="Bracha", Amount=30.5, Tag="Food" });
           for (int i = 0; i < myList.Count; i++)
           {
               Assert.AreEqual(myList[i].Date.Date, query.ElementAt(i).Date.Date);
               Assert.AreEqual(myList[i].Account, query.ElementAt(i).Account);
               Assert.AreEqual(myList[i].Payee, query.ElementAt(i).Payee);
               Assert.AreEqual(myList[i].Amount, query.ElementAt(i).Amount);
               Assert.AreEqual(myList[i].Tag, query.ElementAt(i).Tag);
           }
    
    }
        //List the dates on which each account was used
        [TestMethod]
        public void ListDate ()
        {
            var op = new CheckBookVM();
            op.Fill();
            var credit = op.Transactions.Where(t => t.Account == "Credit").Select(t => new {t.Date, t.Account, t.Payee, t.Amount, t.Tag });
            var checking = op.Transactions.Where(t => t.Account == "Checking").Select(t => new { t.Date });
            
            List<Transaction> creditList = new List<Transaction>();
            creditList.Add (new Transaction { Date= new DateTime (2015, 04, 07), Account="Credit", Payee="Moshe", Amount=30, Tag="Food" });
            creditList.Add ( new Transaction { Date= new DateTime (2015, 04, 06), Account="Credit", Payee="Bracha", Amount=30.5, Tag="Food" });
            creditList.Add (new Transaction {  Date= new DateTime (2015, 04, 05), Account="Credit", Payee="Tim", Amount=130, Tag="Auto" });
            creditList.Add (new Transaction { Date= new DateTime (2015, 04, 04), Account="Credit", Payee="Moshe", Amount=35, Tag="Food" });
            creditList.Add (new Transaction {  Date= new DateTime (2015, 04, 03), Account="Credit", Payee="Bracha", Amount=35, Tag="Food" });
            creditList.Add (new Transaction { Date= new DateTime (2015, 04, 02), Account="Credit", Payee="Tim", Amount=20, Tag="Auto" });

            List<DateTime> checkingList = new List<DateTime>() {new DateTime (2015, 04, 07), new DateTime(2015, 04, 05),
                                                                new DateTime (2015, 04, 04), new DateTime (2015, 04, 03),
                                                                 new DateTime (2015, 04, 02), new DateTime (2015,04,06) };
            
            for (int i = 0; i < creditList.Count; i++)
            {
                Assert.AreEqual(creditList[i].Date, credit.ElementAt(i).Date);
                Assert.AreEqual(creditList[i].Account, credit.ElementAt(i).Account);
                Assert.AreEqual(creditList[i].Payee, credit.ElementAt(i).Payee);
                Assert.AreEqual(creditList[i].Amount, credit.ElementAt(i).Amount);
                Assert.AreEqual(creditList[i].Tag, credit.ElementAt(i).Tag);
            }

            for (int i = 0; i <checkingList.Count; i++)
            {
                Assert.AreEqual(checkingList[i].Date, checking.ElementAt(i).Date);
            }
        }

        //Which account was used most (amount of money) on auto expenses?
        [TestMethod]
    public void MostUsedAccount()
        {
            var acc = new CheckBookVM();
            acc.Fill();
            var autoSpending = acc.Transactions.Where(t => t.Tag == "Auto").GroupBy(t => t.Account).Select(g => new { g.Key, Sum = g.Sum(t => t.Amount) });
            var mostUsed = autoSpending.OrderByDescending(t => t.Sum);
            Assert.AreEqual("Checking", mostUsed.First().Key);  // Both account was used equally for auto
            Assert.AreEqual(150, autoSpending.First().Sum);
            Assert.AreEqual(150, autoSpending.Last().Sum); 
            var equal = mostUsed.First().Sum.Equals(mostUsed.Last().Sum);
            Assert.AreEqual("True", equal.ToString());  // Credit was used as frequent as checking (amount of money)
         
        }


    //List the number of transactions from each account between April 5th and 7th
        [TestMethod]
    public void TransactionBtwApril5and7 ()
      {
          var ob = new CheckBookVM();
          ob.Fill();
          var query = ob.Transactions.Where(t => t.Date >= DateTime.Parse("4/5/2015")).Select(g => g.Date <= DateTime.Parse("4/7/2015"));
         
            Assert.AreEqual(6, query.Count());

       }
    }
}
