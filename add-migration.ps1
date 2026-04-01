param(
    [Parameter(Mandatory = $True)]
    [string] $module,

    [Parameter(Mandatory = $False)]
    [string] $migrationName = "Initial"
)

$project        = ".\src\Modules\$module\${module}.Core\${module}.Core.csproj"
$startupProject = ".\src\Bootstrapper\Bootstrapper.csproj"
$outputDir      = "Infrastructure/DAL/Migrations"
$dbContext      = "${module}DbContext"

Write-Host "Dodawanie migracji: $MigrationName" -ForegroundColor Cyan

dotnet ef migrations add $MigrationName `
    --project         $project `
    --startup-project $startupProject `
    --output-dir      $outputDir `
    --context         $dbContext

if ($LASTEXITCODE -ne 0) {
    Write-Host "Blad podczas dodawania migracji (kod: $LASTEXITCODE)." -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Migracja '$MigrationName' zostala dodana pomyslnie." -ForegroundColor Green
