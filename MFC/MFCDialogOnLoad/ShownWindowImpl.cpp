#include "pch.h"
#include "ShownWindowImpl.h"

#define WM_FIRST_SHOWN WM_USER + 100

ShownDialogImpl::ShownDialogImpl() : _hWnd(NULL)
{
	
}

ShownDialogImpl::~ShownDialogImpl()
{
	
}

void ShownDialogImpl::Init(HWND hWnd)
{
	_hWnd = hWnd;
	PostMessage(_hWnd, WM_FIRST_SHOWN, 0, 0);
}

void ShownDialogImpl::OnDialogShown(LPARAM, WPARAM)
{
	
}