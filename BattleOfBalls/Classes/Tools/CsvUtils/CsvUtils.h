#ifndef __CsvUtils_H__
#define __CsvUtils_H__

#include "cocos2d.h"

USING_NS_CC;

/*��ȡcsv�ļ�����Ҫ��������ʾ����*/

class CsvUtils : public Ref{
public:
	CsvUtils();
	~CsvUtils();

	static CsvUtils * getInstance();

	virtual bool init();

	void loadFile(const std::string & fileName);
	void splitString(std::vector<std::string> &vec, std::string & sSrc, const std::string & sSep);		//���ݷָ����ָ��ַ���
	std::string getMapData(int row, int col, const std::string & fileName);		//��ȡָ�������ַ���
	Size getFileRowCount(const std::string & fileName);		//��ȡ�ļ�����
private:
	static CsvUtils * s_CsvUtils;
	std::map<std::string, std::vector<std::vector<std::string>>> _map;
};


#endif