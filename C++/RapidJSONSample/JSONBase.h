#pragma once
#include "JSONIncludes.h"

class JSONBase
{
public:	
	bool DeserializeFromFile(const std::string& filePath);
	bool SerializeToFile(const std::string& filePath); 	

	virtual std::string Serialize() const;
	virtual bool Deserialize(const std::string& s);
	virtual bool Deserialize(const rapidjson::Value& obj) = 0;
	virtual bool Serialize(rapidjson::Writer<rapidjson::StringBuffer>* writer) const = 0;
protected:	
	bool InitDocument(const std::string & s, rapidjson::Document &doc);
};


