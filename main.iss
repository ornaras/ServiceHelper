#define PASSWORD "Pi141592"

[Setup]
AppName=ServiceHelper
AppVerName=ServiceHelper
DefaultDirName={tmp}
DisableDirPage=true
OutputDir=.
OutputBaseFilename=ServiceHelper
Password={#PASSWORD}

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