using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public class Basket : Isms
    {
        public int size {  get; set; } //Size of basket
        public List<Product> products = new List<Product>(); //List of all 

        public Basket(Manager manager)
        {
            this.size = manager.allowedBasketSize;
        }

        public int Search(string product)
        {
            int index = 0;
            //Loop through all products and try to find the index of the selected one
            foreach (var item in products)
            {
                if (item.info.key == product)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public float TotalCost()
        {
            //Calculate the total cost of all products in basket
            float totalCost = 0.0f;
            foreach (var item in products)
            {
                totalCost += item.Cost();
            }

            //Apply coffee discount
            totalCost -= 0.23f * (float)this.CoffeeDiscounts();

            return totalCost;
        }

        private int CoffeeDiscounts()
        {
            int coffeeDiscounts = 0;
            //Calculate the 0.23 discount for every coffee and excess bagel that exists
            int coffees = 0;
            int excessBagels = 0;

            //Find how many of each exists
            foreach (var item in products)
            {
                //Add Coffee
                if (item.info.key[0] == 'C')
                {
                    coffees += item.GetAmount();
                }

                //Add Bagel
                if (item.info.key[0] == 'B')
                {
                    excessBagels += item.GetExcessBagelAmount();
                }
            }

            //Check how many discounts can be applied
            for (int i = 0; i < coffees; i++)
            {
                if (excessBagels > 0) //If there are more bagels left
                {
                    coffeeDiscounts++;
                    excessBagels--;
                }
                else //Exit the loop
                {
                    break;
                }
            }

            return coffeeDiscounts;
        }

        public string Receipt()
        {
            //Masterfully construct the receipt
            string receipt = $"       ~~~Bob's Bagels~~~\n\n      {DateTime.Now.ToString("G")}\n\n-------------------------------\n\n";

            int coffeeDiscounts = this.CoffeeDiscounts();

            float totalDiscount = 0.0f;

            //For each item, add the name, amount and total cost
            foreach (var item in products)
            {
                //Check if it's an everything bagel (cause long name)
                if(item.info.key == "BGLE")
                {
                    receipt += $"{item.info.variant} {item.info.name}\t\t{item.GetAmount()}\t£{item.Cost()}\n";
                }
                else if(item.info.key == "COFC") //Check if it's a capuccino (cause long name)
                {
                    if (coffeeDiscounts > 0) //Check if discounts need to be applied
                    {
                        //If the coffee discounts exceed the amount of coffees of this type
                        if (coffeeDiscounts > item.GetAmount())
                        {
                            receipt += $"{item.info.variant} {item.info.name}\t\t{item.GetAmount()}\t£{(float)Math.Round((item.Cost() - 0.23f * item.GetAmount()), 2)}\n";
                            receipt += $"\t\t\t\t\t  (-£{0.23f * item.GetAmount()})\n";
                            totalDiscount += 0.23f * item.GetAmount();
                        }
                        else //The amount of coffees of this type exceeds the amount of coffee discounts left
                        {
                            receipt += $"{item.info.variant} {item.info.name}\t\t{item.GetAmount()}\t£{(float)Math.Round((item.Cost() - 0.23f * coffeeDiscounts), 2)}\n";
                            receipt += $"\t\t\t\t\t  (-£{0.23f * coffeeDiscounts})\n";
                            totalDiscount += 0.23f * coffeeDiscounts;
                        }
                        coffeeDiscounts -= item.GetAmount();
                    }
                    else //No discounts, just add the capuccino
                    {
                        receipt += $"{item.info.variant} {item.info.name}\t\t{item.GetAmount()}\t£{item.Cost()}\n";
                    }
                }
                else //Otherwise it doesn't have a long name and it's just the regular case
                {
                    if (item.info.key[0] == 'C' && coffeeDiscounts > 0) //Check if it's coffee and it needs a discount
                    {
                            //If the coffee discounts exceed the amount of coffees of this type
                            if (coffeeDiscounts > item.GetAmount())
                            {
                                receipt += $"{item.info.variant} {item.info.name}\t\t\t{item.GetAmount()}\t£{item.Cost() - 0.23f * item.GetAmount()}\n";
                                receipt += $"\t\t\t\t\t  (-£{0.23f * item.GetAmount()})\n";
                                totalDiscount += 0.23f * item.GetAmount();
                            }
                            else //The amount of coffees of this type exceeds the amount of coffee discounts left
                            {
                                receipt += $"{item.info.variant} {item.info.name}\t\t\t{item.GetAmount()}\t£{item.Cost() - 0.23f * coffeeDiscounts}\n";
                                receipt += $"\t\t\t\t\t  (-£{0.23f * coffeeDiscounts})\n";
                                totalDiscount += 0.23f * coffeeDiscounts;
                            }
                            coffeeDiscounts -= item.GetAmount();
                    }
                    else //Otherwise just write the item down on the receipt
                    {
                        receipt += $"{item.info.variant} {item.info.name}\t\t\t{item.GetAmount()}\t£{item.Cost()}\n";
                    }

                }

                //Check if any bagel discounts have been applied
                if (item.info.key[0] == 'B' && item.BagelDiscount() > 0) 
                {
                    receipt += $"\t\t\t\t\t  (-£{item.BagelDiscount()})\n";
                    totalDiscount += item.BagelDiscount();
                }
            }

            //Spacing, total cost, discount, and thank you
            receipt += $"\n-------------------------------\nTotal\t\t\t\t\t£{this.TotalCost()}\n\n   You saved a total of £{totalDiscount}\n\t\t on this shop\n\n\t\t   Thank you\n\t\tfor your order!";

            return receipt;
        }

        public void Empty()
        {
            this.products.Clear();
        }

        public async Task<string> Send(Basket basket)
        {
            await Task.Delay(1);
            return basket.Receipt();
        }
    }
}
