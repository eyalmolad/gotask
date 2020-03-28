#include <string>
#include "Product.h"

namespace JSONModels
{
	Product::Product() : _sales(0.0f), _id(0)
	{}

	Product::~Product()
	{}
	
	
	
	bool Product::Deserialize(const rapidjson::Value & obj)
	{
		Id(obj["id"].GetInt());
		Name(obj["name"].GetString());
		Category(obj["category"].GetString());
		Sales(obj["sales"].GetFloat());

		return true;
	}

	bool Product::Serialize(rapidjson::Writer<rapidjson::StringBuffer> * writer) const
	{
		writer->StartObject();

			writer->String("id");
			writer->Int(_id);

			writer->String("name");
			writer->String(_name.c_str());

			writer->String("category");
			writer->String(_category.c_str());

			writer->String("sales");
			writer->Double(_sales);

		writer->EndObject();

		return true;
	}	
}
