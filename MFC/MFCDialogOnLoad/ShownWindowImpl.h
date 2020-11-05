#pragma once

class ShownDialogImpl
{
public:
	ShownDialogImpl();
	virtual ~ShownDialogImpl();
	void Init(HWND hWnd);
	virtual void OnDialogShown(LPARAM, WPARAM);

private:
	HWND _hWnd;
};

