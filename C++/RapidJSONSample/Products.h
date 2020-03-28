#pragma once

#include "Product.h"
#include <list>

namespace JSONModels
{
	class Products : public JSONBase
	{
	public:		
		virtual ~Products() {};
		virtual bool Deserialize(const std::string& s);		

		// Getters/Setters.
		std::list<Product>& ProductsList() { return _products; }
	public:
		virtual bool Deserialize(const rapidjson::Value& obj) { return false; };
		virtual bool Serialize(rapidjson::Writer<rapidjson::StringBuffer>* writer) const;
	private:
		std::list<Product> _products;
	};
}

