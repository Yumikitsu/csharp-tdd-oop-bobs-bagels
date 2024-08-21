using exercise.main;

namespace exercise.tests;

public class Tests
{
    [TestCase("Fail", "BGLO")]
    public void AddBagelTest(string fail, string bagel)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        bool expectedFailure = false;

        //act
        bool result = customer.Add(manager, fail);

        //assert
        Assert.That(result, Is.EqualTo(expectedFailure));

        //arrange
        bool expectedSuccess = true;

        //act
        result = customer.Add(manager, bagel);

        //assert
        Assert.That(result, Is.EqualTo(expectedSuccess));
    }

    [TestCase("BGLO")]
    public void RemoveBagelTest(string bagel)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        bool expectedFailure = false;

        //act
        bool result = customer.Remove(manager, bagel);

        //assert
        Assert.That(result, Is.EqualTo(expectedFailure));

        //arrange
        customer.Add(manager, bagel);
        bool expectedSuccess = true;

        //act
        result = customer.Remove(manager, bagel);

        //assert
        Assert.That(result, Is.EqualTo(expectedSuccess));
    }

    [TestCase("BGLO", "BGLP", "BGLE")]
    public void OverfillTest(string product1, string product2, string product3)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        bool expectedSuccess = true;
        customer.Add(manager, product1);
        customer.Add(manager, product2);

        //act
        bool result = customer.Add(manager, product3);

        //assert
        Assert.That(result, Is.EqualTo(expectedSuccess));

        //arrange
        bool expectedFailure = false;

        //act
        result = customer.Add(manager, product3);

        //assert
        Assert.That(result, Is.EqualTo(expectedFailure));
    }

    [TestCase("COFB", "COFW")]
    public void CapacityChangeTest(string product1, string product2)
    {
        //arrange
        Manager manager = new Manager();
        int newSize = 1;
        manager.ChangeBasketSize(newSize);
        Customer customer = new Customer(manager, 3.50f);
        bool expectedFailure = false;
        customer.Add(manager, product1);

        //act
        bool result = customer.Add(manager, product2);

        bool changeToNegative = manager.ChangeBasketSize(-1);

        //assert
        Assert.That(result, Is.EqualTo(expectedFailure));
        Assert.That(changeToNegative, Is.EqualTo(expectedFailure));
    }

    [TestCase("BGLS")]
    public void RemoveNothingTest(string product)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        bool expectedFailure = false;

        //act
        bool result = customer.Remove(manager, product);

        //assert
        Assert.That(result, Is.EqualTo(expectedFailure));

        //arrange
        customer.Add(manager, product);
        bool expectedSuccess = true;

        //act
        result = customer.Remove(manager, product);

        //assert
        Assert.That(result, Is.EqualTo(expectedSuccess));
    }

    [TestCase("COFL", "COFW", "COFB", 3.47f)]
    [TestCase("BGLO", "BGLP", "BGLS", 1.37f)]
    [TestCase("COFB", "COFB", "COFB", 2.97f)]
    public void TotalCostTest(string product1, string product2, string product3, float sum)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        customer.Add(manager, product1);
        customer.Add(manager, product2);
        customer.Add(manager, product3);

        //act
        float totalSum = customer.TotalCost();

        //assert
        Assert.That(totalSum, Is.EqualTo(sum));
    }

    [TestCase("BGLO", 0.49f)]
    [TestCase("BGLP", 0.39f)]
    [TestCase("BGLB", 0.0f)]
    public void SingleCostTest(string product, float cost)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        
        //act
        float sum = customer.HowMuch(manager, product);

        //assert
        Assert.That(sum, Is.EqualTo(cost));
    }

    [TestCase("BGLO", "FILB", true, 0.61f)]
    [TestCase("COFB", "FILE", false, 0.99f)]
    public void FillingTest(string product, string filling, bool expectedOutcome, float expectedPrice)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        customer.Add(manager, product);

        //act
        bool result = customer.AddFilling(manager, filling, product);
        float actualPrice = customer.TotalCost();

        //assert
        Assert.That(result, Is.EqualTo(expectedOutcome));
        Assert.That(actualPrice, Is.EqualTo(expectedPrice));
    }

    [TestCase(0.12f)]
    public void FillingCostTest(float expectedCost)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);

        //act
        float result = customer.HowMuchFillings(manager);

        //assert
        Assert.That(result, Is.EqualTo(expectedCost));
    }

    [TestCase("BGLO", true)]
    [TestCase("FAKE", false)]
    public void InStockTest(string product, bool expectedOutcome)
    {
        //arrange
        Manager manager = new Manager();
        Customer customer = new Customer(manager, 3.50f);
        
        //act
        bool result = customer.Add(manager, product);

        //assert
        Assert.That(result, Is.EqualTo(expectedOutcome));
    }

    //Extension: 1

    [TestCase("BGLO", 5, 2.45f)]
    public void AddMultipleTest(string product, int amount, float expectedPrice)
    {
        Manager manager = new Manager();
        manager.ChangeBasketSize(5);
        Customer customer = new Customer(manager, 3.50f);
        customer.Add(manager, product, amount);

        //act
        float result = customer.TotalCost();

        //assert
        Assert.That(result, Is.EqualTo(expectedPrice));
    }

    [TestCase("BGLO", 6, 2.49f)]
    [TestCase("BGLE", 12, 3.99f)]
    [TestCase("BGLO", 13, 4.48f)]
    public void DiscountTest(string product, int amount, float expectedPrice)
    {
        //arrange
        Manager manager = new Manager();
        manager.ChangeBasketSize(15);
        Customer customer = new Customer(manager, 3.50f);
        customer.Add(manager, product, amount);

        //act
        float result = customer.TotalCost();

        //assert
        Assert.That(result, Is.EqualTo(expectedPrice));
    }

    [TestCase("BGLO", 13, "COFB", 2, 6.23f)]
    [TestCase("BGLE", 1, "COFW", 1, 1.45f)]
    [TestCase("BGLS", 5, "COFC", 5, 7.75f)]
    public void CombinationTest(string product1, int amount1, string product2, int amount2, float expectedPrice)
    {
        //arrange
        Manager manager = new Manager();
        manager.ChangeBasketSize(20);
        Customer customer = new Customer(manager, 3.50f);
        customer.Add(manager, product1, amount1);
        customer.Add(manager, product2, amount2);

        //act
        float result = customer.TotalCost();

        //assert
        Assert.That(result, Is.EqualTo(expectedPrice));
    }

    [TestCase("BGLO", "BGLP", "BGLE", 4, 8, 16)]
    public void PrintReceiptTest(string product1, string product2, string product3, int amount1, int amount2, int amount3)
    {
        //arrange
        Manager manager = new Manager();
        manager.ChangeBasketSize(30);
        Customer customer = new Customer(manager, 40.0f);
        customer.Add(manager, product1, amount1);
        customer.Add(manager, product2, amount2);
        customer.Add(manager, product3, amount3);

        //act
        string receipt = customer.Purchase(manager);

        //assert
        Assert.That(receipt, !Is.EqualTo("Insufficient Funds"));
    }

    [TestCase("BGLO", "BGLE", "COFE", "COFC", 19, 23, 5, 10)]
    public void PrintDiscountReceiptTest(string product1, string product2, string product3, string product4, int amount1, int amount2, int amount3, int amount4)
    {
        //arrange
        Manager manager = new Manager();
        manager.ChangeBasketSize(50);
        Customer customer = new Customer(manager, 40.0f);
        customer.Add(manager, product1, amount1);
        customer.Add(manager, product2, amount2);
        customer.Add(manager, product3, amount3);
        customer.Add(manager, product4, amount4);

        //act
        string receipt = customer.Purchase(manager);

        //assert
        Assert.That(receipt, !Is.EqualTo("Insufficient Funds"));
        Assert.That(receipt.Contains("-£"), Is.True);
    }

    [TestCase("BGLO", "BGLE", "COFE", "COFC", 19, 23, 5, 10)]
    public void SendSMSReceiptTest(string product1, string product2, string product3, string product4, int amount1, int amount2, int amount3, int amount4)
    {
        //arrange
        Manager manager = new Manager();
        manager.ChangeBasketSize(50);
        SMS sms = new SMS();
        Customer customer = new Customer(manager, 40.0f, sms);
        customer.Add(manager, product1, amount1);
        customer.Add(manager, product2, amount2);
        customer.Add(manager, product3, amount3);
        customer.Add(manager, product4, amount4);

        //act
        string receipt = customer.Purchase(manager);

        //assert
        Assert.That(receipt, !Is.EqualTo("Insufficient Funds"));
        Assert.That(receipt.Contains("-£"), Is.True);
    }
}