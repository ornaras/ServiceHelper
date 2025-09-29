#define PASSWORD "Pi141592"

[Setup]
AppName=ServiceHelper
AppVerName=ServiceHelper 29-09-2025
AppVersion=2025.09.29
DefaultDirName={tmp}
DisableDirPage=true
Uninstallable=false
OutputDir=.
OutputBaseFilename=ServiceHelper
Password={#PASSWORD}

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Components]
Name: "settings"; Description: "Настройка компьютера"; Types: full
Name: "settings\energy"; Description: "Энергосбережения"; Types: full compact

Name: "utilities"; Description: "Утилиты"; Types: full compact
Name: "utilities\anydesk"; Description: "AnyDesk"; Types: full
Name: "utilities\scankass"; Description: "ScanKass"; Types: full

Name: "atol"; Description: "Программы Atol"; Types: full
Name: "atol\xpos"; Description: "Frontol xPos 3"; Types: full
Name: "atol\frontol"; Description: "Frontol 6"; Types: full
Name: "atol\driver"; Description: "Драйвер ККТ"; Types: full

Name: "onec"; Description: "1С"; Types: full
Name: "onec\platform"; Description: "1С: Платформа"; Types: full

Name: "mssql"; Description: "Microsoft SQL"; Types: full
Name: "mssql\basic"; Description: "Microsoft SQL Server"; Types: full
Name: "mssql\msm"; Description: "Microsoft SQL Server Management Studio"; Types: full

Name: "itida"; Description: "Айтида"; Types: full

[Code]
function OnDownloadProgress(const Url, Filename: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if ProgressMax <> 0 then
    Log(Format('  %d of %d bytes done.', [Progress, ProgressMax]))
  else
    Log(Format('  %d bytes done.', [Progress]));
  Result := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  Path: String;
  ResultCode: Integer;
begin
  if CurStep = ssInstall then
  begin
    if WizardIsComponentSelected('utilities\anydesk') then
    begin
      WizardForm.StatusLabel.Caption := 'Установка AnyDesk...';
      DownloadTemporaryFile('https://download.anydesk.com/AnyDesk.exe', 'AnyDesk.exe', '', @OnDownloadProgress);
      Path := ExpandConstant('{tmp}\AnyDesk.exe');
      Exec(Path, '--install "C:\Program Files\AnyDesk"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
      Exec('cmd', Format('/c "echo \"Pi141592\" | \"%s\" --set-password"', [Path]), '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
    end;
  end;
end;