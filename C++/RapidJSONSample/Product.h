#pragma once

#include "JSONBase.h"
#include <list>

namespace JSONModels
{
	class Product : public JSONBase
	{
	public:
		Product();		
		virtual ~Product();			

		virtual bool Deserialize(const rapidjson::Value& obj);
		virtual bool Serialize(rapidjson::Writer<rapidjson::StringBuffer>* writer) const;

		// Getters/Setters.
		const std::string& Name() const { return _name; }
		void Name(const std::string& name) { _name = name; }

		const std::string& Category() const { return _category; }
		void Category(const std::string& category) { _category = category; }

		float Sales() const { return _sales; }
		void Sales(float sales) { _sales = sales; }

		int Id() const { return _id; }
		void Id(int id) { _id = id; }		
	private:
		std::string _name;
		std::string _category;
		float _sales;
		int _id;
	};	
}

