#include "utils.iss"

[Code]
procedure AnyDesk;
var
  Path: String;
  ResultCode: Integer;
begin
  WizardForm.StatusLabel.Caption := 'Установка AnyDesk...';
  DownloadTemporaryFile('https://download.anydesk.com/AnyDesk.exe', 'AnyDesk.exe', '', @OnDownloadProgress);
  Path := ExpandConstant('{tmp}\AnyDesk.exe');
  Exec(Path, '--install "C:\Program Files\AnyDesk"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  Exec('cmd', Format('/c "echo \"Pi141592\" | \"%s\" --set-password"', [Path]), '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;