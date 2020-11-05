#pragma once

// MFCDialogOnLoadDlg.h : header file
//
#include "ShownWindowImpl.h"

#pragma once

// CMFCDialogOnLoadDlg dialog
class CMFCDialogOnLoadDlg : public CDialogEx, ShownDialogImpl
{
// Construction
public:
	CMFCDialogOnLoadDlg(CWnd* pParent = nullptr);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_MFCDIALOGONLOAD_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	LRESULT OnDialogShown(WPARAM, LPARAM);
	
// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
};
