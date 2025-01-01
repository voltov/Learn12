using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            if (suppliers == null) throw new ArgumentNullException(nameof(suppliers));

            return customers.Select(c => (
                customer: c,
                suppliers: suppliers.Where(s => s.Country == c.Country && s.City == c.City)
            ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            if (suppliers == null) throw new ArgumentNullException(nameof(suppliers));

            var supplierGroups = suppliers.GroupBy(s => new { s.Country, s.City });

            return customers.Select(c => (
                customer: c,
                suppliers: supplierGroups
                    .Where(g => g.Key.Country == c.Country && g.Key.City == c.City)
                    .SelectMany(g => g)
            ));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers.Where(c => c.Orders.Any(o => o.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)))
                .OrderBy(result => result.dateOfEntry.Year)
                .ThenBy(result => result.dateOfEntry.Month)
                .ThenByDescending(result => result.customer.Orders.Sum(o => o.Total))
                .ThenBy(result => result.customer.CustomerID);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers.Where(c =>
                c.PostalCode.Any(ch => !char.IsDigit(ch)) ||
                string.IsNullOrEmpty(c.Region) ||
                !c.Phone.Contains('('));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));
            return products
                .GroupBy(p => p.Category)
                .Select(g => new Linq7CategoryGroup
                {
                    Category = g.Key,
                    UnitsInStockGroup = g
                        .GroupBy(p => p.UnitsInStock)
                        .Select(ug => new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = ug.Key,
                            Prices = ug.OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice)
                        })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            if (products == null) throw new ArgumentNullException(nameof(products));
            return products
                .GroupBy(p => p.UnitPrice <= cheap ? cheap :
                              p.UnitPrice <= middle ? middle : expensive)
                .Select(g => (category: g.Key, products: g.AsEnumerable()));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            if (customers == null) throw new ArgumentNullException(nameof(customers));
            return customers
                .GroupBy(c => c.City)
                .Select(g => (
                    city: g.Key,
                    averageIncome: (int)Math.Round(g.Average(c => c.Orders.Sum(o => o.Total))),
                    averageIntensity: (int)Math.Round(g.Average(c => c.Orders.Count()))
                ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            if (suppliers == null) throw new ArgumentNullException(nameof(suppliers));
            return string.Join("", suppliers
                .Select(s => s.Country)
                .Distinct()
                .OrderBy(c => c.Length)
                .ThenBy(c => c));
        }
    }
}