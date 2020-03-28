#include "Products.h"

namespace JSONModels
{
	bool Products::Deserialize(const std::string& s)
	{
		rapidjson::Document doc;
		if (!InitDocument(s, doc))
			return false;

		if (!doc.IsArray())
			return false;

		for (rapidjson::Value::ConstValueIterator itr = doc.Begin(); itr != doc.End(); ++itr)
		{
			Product p;
			p.Deserialize(*itr);
			_products.push_back(p);
		}

		return true;
	}

	bool Products::Serialize(rapidjson::Writer<rapidjson::StringBuffer>* writer) const
	{
		writer->StartArray();

		for (std::list<Product>::const_iterator it = _products.begin(); it != _products.end(); it++)
		{
			(*it).Serialize(writer);
		}
		writer->EndArray();

		return true;
	}
}