#pragma once

#include "Product.h"
#include <list>

namespace JSONModels
{
	class Products : public JSONBase
	{
	public:
		Products() {};
		virtual ~Products() {};
		virtual bool Deserialize(const std::string& s);
		virtual bool Deserialize(const rapidjson::Value& obj) { return false; };
		virtual bool Serialize(rapidjson::Writer<rapidjson::StringBuffer>* writer) const;

		std::list<Product>& ProductsList() { return _products; }
	private:
		std::list<Product> _products;
	};
}

