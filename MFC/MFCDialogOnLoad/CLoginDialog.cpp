// CLoginDialog.cpp : implementation file
//

#include "pch.h"
#include "MFCDialogOnLoad.h"
#include "CLoginDialog.h"
#include "afxdialogex.h"


// CLoginDialog dialog

IMPLEMENT_DYNAMIC(CLoginDialog, CDialogEx)

CLoginDialog::CLoginDialog(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_DIALOG1, pParent)
{

}

CLoginDialog::~CLoginDialog()
{
}

void CLoginDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CLoginDialog, CDialogEx)
END_MESSAGE_MAP()


// CLoginDialog message handlers
