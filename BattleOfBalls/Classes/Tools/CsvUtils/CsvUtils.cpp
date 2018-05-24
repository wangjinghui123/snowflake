#include "CsvUtils.h"

CsvUtils * CsvUtils::s_CsvUtils = NULL;

CsvUtils::CsvUtils()
{

}

CsvUtils::~CsvUtils()
{

}

CsvUtils * CsvUtils::getInstance()
{
	if (!s_CsvUtils)
	{
		s_CsvUtils = new CsvUtils();
		if (s_CsvUtils && s_CsvUtils->init())
		{
			s_CsvUtils->autorelease();
		}
		else
		{
			CC_SAFE_DELETE(s_CsvUtils);
		}
	}
	return s_CsvUtils;
}

bool CsvUtils::init()
{
	return true;
}

void CsvUtils::loadFile(const std::string & fileName)
{
	std::vector<std::vector<std::string>> vec;

	std::string fileData = FileUtils::getInstance()->getStringFromFile(fileName);
	//log(fileData.c_str());
	std::vector<std::string> lines;
	splitString(lines, fileData, "\r\n");

	/* ��ÿһ�е��ַ�����ֳ����������ŷָ��� */
	for (auto line : lines)
	{
		std::vector<std::string> lineData;
		splitString(lineData, line, ",");
		vec.push_back(lineData);
	}

	/* ����б��ֵ��� */
	_map[fileName] = vec;

}

void CsvUtils::splitString(std::vector<std::string> &vec, std::string & sSrc, const std::string & sSep)
{
	int startIndex = 0;
	int sepLen = sSep.length();
	int endIndex = sSrc.find(sSep);
	int length = sSrc.size();
	/* ���ݻ��з�����ַ���������ӵ��б��� */

	while (endIndex != std::string::npos) {
		/* ��ȡһ���ַ��� */
		vec.push_back(sSrc.substr(startIndex, endIndex));
		sSrc = sSrc.substr(endIndex + sepLen, length);
		endIndex = sSrc.find(sSep);
	}

	/* ʣ�µ��ַ���Ҳ��ӽ��б� */
	if (sSrc.compare("") != 0)
	{
		vec.push_back(sSrc);
	}
}

std::string CsvUtils::getMapData(int row, int col, const std::string & fileName)
{
	/* ȡ�������ļ��Ķ�ά��� */
	auto vec = _map[fileName];

	/* ��������ļ������ݲ����ڣ�����������ļ� */
	if (vec.size() == 0)
	{
		loadFile(fileName);
		vec = _map[fileName];
	}

	int rowNum = vec.size();
	int colNum = vec[0].size();

	/* �±�Խ�� */
	if (row < 0 || row >= rowNum || col < 0 || col >= colNum)
	{
		return "";
	}

	return vec[row][col];

}

Size CsvUtils::getFileRowCount(const std::string & fileName)
{
	/* ȡ�������ļ��Ķ�ά��� */
	auto vec = _map[fileName];

	/* ��������ļ������ݲ����ڣ�����������ļ� */
	if (vec.size() == 0)
	{
		loadFile(fileName);
		vec = _map[fileName];
	}

	int rowNum = vec.size();
	int colNum = vec[0].size();

	return Size(rowNum, colNum);
}