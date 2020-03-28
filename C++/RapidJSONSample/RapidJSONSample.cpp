// RapidJSONSample.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include "Product.h"
#include "Products.h"

int main()
{       
    JSONModels::Product product;
    product.DeserializeFromFile("DataSample.json");
    float newSales = product.Sales() + 100;
    product.Sales(product.Sales());
    product.SerializeToFile("DataSampleNew.json");

    JSONModels::Products products;
    products.DeserializeFromFile("DataSampleArray.json");

    JSONModels::Product newProduct;
    newProduct.Id(101);
    newProduct.Name("Global Value Mid-Back Manager's Chair, Gray");
    newProduct.Category("Furniture");
    newProduct.Sales(213.115f);    
    products.ProductsList().push_back(product);
    products.SerializeToFile("DataSampleArrayNew.json");
}
