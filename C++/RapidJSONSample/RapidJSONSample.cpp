// RapidJSONSample.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include "Product.h"
#include "Products.h"

int main()
{   
    // load json
    JSONModels::Product product;
    product.DeserializeFromFile("DataSample.json");

    // print some value.
    printf("Name:%s, Sales:%.3f", product.Name().c_str(), product.Sales());
    
    // increase the sales.
    float newSales = product.Sales() + 100;
    product.Sales(product.Sales());

    // save json to new file.
    product.SerializeToFile("DataSampleNew.json");

    // load json array
    JSONModels::Products products;
    products.DeserializeFromFile("DataSampleArray.json");
    
    for (std::list<JSONModels::Product>::const_iterator it = products.ProductsList().begin();
        it != products.ProductsList().end(); it++)
    {
        // print some values.
        printf("Name:%s, Sales:%.3f", (*it).Name().c_str(), (*it).Sales());
    }

    // add new product
    JSONModels::Product newProduct;
    newProduct.Id(101);
    newProduct.Name("Global Value Mid-Back Manager's Chair, Gray");
    newProduct.Category("Furniture");
    newProduct.Sales(213.115f);    
    products.ProductsList().push_back(product);

    // save to new array file.
    products.SerializeToFile("DataSampleArrayNew.json");
}
