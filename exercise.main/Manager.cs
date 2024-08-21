using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public class Manager
    {
        public int allowedBasketSize { get; set; } = 3; //Maximum basket size for all newly created baskets

        private Inventory inv = new Inventory(); //Inventory containing all products in the store

        public bool AddProduct(Basket bskt, string product, int amount = 1)
        {
            //Check if product exists in inventory
            if(inv.Find(product))
            {
                //Check if the added amount of the product will fit in the basket
                if (bskt.products.Count + amount <= bskt.size)
                {
                    //Check if the product is already in the basket
                    int index = bskt.Search(product);
                    if(index != -1)
                    {
                        //Update the basket to have the new amount of the product
                        bskt.products[index].IncreaseAmount(amount);
                    }
                    else //Add new product to basket if it fits
                    {
                        bskt.products.Add(inv.GetProduct(product));
                        bskt.products.Last().IncreaseAmount(amount - 1);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ChangeBasketSize(int newSize)
        {
            //If newSize is a positive integer
            if(newSize >= 0)
            {
                this.allowedBasketSize = newSize;
                return true;
            }
            return false;
        }

        public bool RemoveProduct(Basket bskt, string product, int amount = 1)
        {
            //Check if product exists in basket
            int index = bskt.Search(product);
            if (index > -1)
            {
                //Remove product from basket if the amount to remove would take away all of a product
                if (bskt.products[index].GetAmount() <= amount)
                {
                    bskt.products.RemoveAt(index);
                }
                else //Otherwise just remove x amount of the product
                {
                    bskt.products[index].DecreaseAmount(amount);
                }
                return true;
            }
            return false;
        }

        public float HowMuchProduct(string product)
        {
            //Check the inventory for the price of the item
            if(inv.Find(product))
            {
                return inv.GetProduct(product).info.price;
            }
            return 0.0f;
        }

        public bool AddFilling(Basket basket, string filling, string product)
        {
            //Check if the product exists in the basket
            int index = basket.Search(product);
            if (index != -1)
            {
                //Check if the product is a bagel
                if (basket.products[index].IsBagel())
                {
                    //Check if filling exists in inventory
                    if (inv.Find(filling))
                    {
                        //Add the filling to donut 
                        basket.products[index].AddFilling(inv.GetProduct(filling));
                        return true;
                    }
                }
            }
            return false;
        }

        public float HowMuchFillings()
        {
            //Return the cost of a filling (Can add a function in inventory that returns a string consisting of all products starting with key "F---")
            return inv.GetProduct("FILB").Cost();
        }

        public string Purchase(Basket basket, Isms receiptHandler)
        {
            //Get the receipt
            string receipt = receiptHandler.Send(basket).Result;
            
            //Print it
            Console.WriteLine(receipt);

            //Clear the basket
            basket.Empty();

            //Return the receipt
            return receipt;
        }
    }
}
