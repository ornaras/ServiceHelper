#define PASSWORD "Pi141592"

#include "components.iss"
#include "anydesk.iss"

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

[Code]

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssInstall then
  begin
    if WizardIsComponentSelected('utilities\anydesk') then
    begin
      AnyDesk();
    end;
  end;
end;